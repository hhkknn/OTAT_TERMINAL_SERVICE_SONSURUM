using AIF.TerminalService.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIF.TerminalService.SAPLayer
{
    public class AddOrUpdateInventoryGenEntry
    {
        public Response addOrUpdateStockTransfer(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            Random rastgele = new Random();
            int ID = rastgele.Next(0, 9999);
            Logger logger = LogManager.GetCurrentClassLogger();

            //var requestJson_New = JsonConvert.SerializeObject(protocol);

            //logger.Info(" ");

            logger.Info("ID: " + ID + " addOrUpdateStockTransfer Servisine Geldi.");
            //logger.Info("ID: " + ID + " ISTEK :" + requestJson_New);

            int clNum = 0;
            string companyDbCode = "";
            try
            {
                ConnectionList connection = new ConnectionList();

                LoginCompany log = new LoginCompany();

                log.DisconnectSAP(dbName);

                connection = log.getSAPConnection(dbName, ID);

                //if (connection.number == -1)
                //{
                //    for (int ix = 1; ix <= 3; ix++)
                //    {
                //        connection = log.getSAPConnection(dbName);

                //        if (connection.number > -1)
                //        {
                //            break;
                //        }
                //    }

                //}

                if (connection.number == -1)
                {
                    logger.Fatal("ID: " + ID + " " + "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu.");
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }

                clNum = connection.number;
                companyDbCode = connection.dbCode;
                SAPbobsCOM.Company oCompany = connection.oCompany;

                logger.Info("ID: " + ID + " Şirket bağlantısını başarıyla geçtik. Bağlantı sağladığımız DB :" + oCompany.CompanyDB + " clnum: " + clNum);

                SAPbobsCOM.StockTransfer oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                DateTime dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                oDocuments.DocDate = dt;
                oDocuments.Comments = inventoryGenEntry.Comments;
                oDocuments.CardCode = inventoryGenEntry.CardCode;

                //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                //oDocuments.DocDueDate = dt;

                //oDocuments.bpl = 1;
                int i = 0;
                foreach (var item in inventoryGenEntry.InventoryGenEntryLines)
                {
                    //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                    if (item.BaseEntry != null && item.BaseEntry != 0)
                    {
                        oDocuments.Lines.BaseEntry = item.BaseEntry;
                        oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                        oDocuments.Lines.BaseLine = item.BaseLine;
                    }
                    else
                    {
                        oDocuments.Lines.ItemCode = item.ItemCode;
                    }

                    oDocuments.Lines.Quantity = item.Quantity;

                    i = 0;

                    if (item.BinCode_from != null && item.BinCode_from != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_from);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    i = 1;

                    if (item.BinCode_to != null && item.BinCode_to != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_to);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    if (item.fromWhsCode != null && item.fromWhsCode != "")
                    {
                        oDocuments.Lines.FromWarehouseCode = item.fromWhsCode;
                    }

                    if (item.toWhsCode != null && item.toWhsCode != "")
                    {
                        oDocuments.Lines.WarehouseCode = item.toWhsCode;
                    }

                    foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                    {
                        oDocuments.Lines.BatchNumbers.Add();
                        oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                        oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                        oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                        i++;
                    }

                    oDocuments.Lines.Add();

                    i = 0;
                }

                int retval = oDocuments.Add();

                if (retval != 0)
                {
                    logger.Fatal("ID: " + ID + " HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription());
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription() };
                }
                else
                {
                    logger.Info("ID: " + ID + " KAYIT BAŞARILI.");
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = 0, Desc = "KAYIT BAŞARILI." };
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ID: " + ID + " BİLİNMEYEN HATA OLUŞTU." + ex.Message);
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
                return new Response { _list = null, Val = -999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            finally
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
            }
        }

        public Response addOrUpdateStockTransferDraft(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            Random rastgele = new Random();
            int ID = rastgele.Next(0, 9999);
            Logger logger = LogManager.GetCurrentClassLogger();

            //var requestJson_New = JsonConvert.SerializeObject(protocol);

            //logger.Info(" ");

            logger.Info("ID: " + ID + " addOrUpdateStockTransferDraft Servisine Geldi.");
            //logger.Info("ID: " + ID + " ISTEK :" + requestJson_New);

            int clNum = 0;
            string companyDbCode = "";
            SAPbobsCOM.Company oCompany = null;
            try
            {
                ConnectionList connection = new ConnectionList();

                LoginCompany log = new LoginCompany();

                log.DisconnectSAP(dbName);

                connection = log.getSAPConnection(dbName, ID);

                //if (connection.number == -1)
                //{
                //    for (int ix = 1; ix <= 3; ix++)
                //    {
                //        connection = log.getSAPConnection(dbName);

                //        if (connection.number > -1)
                //        {
                //            break;
                //        }
                //    }

                //}

                if (connection.number == -1)
                {
                    logger.Fatal("ID: " + ID + " " + "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu.");
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }

                clNum = connection.number;
                companyDbCode = connection.dbCode;
                oCompany = connection.oCompany;

                logger.Info("ID: " + ID + " Şirket bağlantısını başarıyla geçtik. Bağlantı sağladığımız DB :" + oCompany.CompanyDB + " clnum: " + clNum);

                //oCompany.StartTransaction();
                SAPbobsCOM.StockTransfer oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer;

                DateTime dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                oDocuments.DocDate = dt;
                oDocuments.Comments = inventoryGenEntry.Comments;
                oDocuments.CardCode = inventoryGenEntry.CardCode;

                dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(6, 2))); //new

                oDocuments.DueDate = dt; //new

                //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                //oDocuments.DocDueDate = dt;

                //oDocuments.bpl = 1;
                int i = 0;
                int retval = 0;
                bool check = false;
                foreach (var item in inventoryGenEntry.InventoryGenEntryLines.Where(x => x.uretimdenGonderildi != "Y"))
                {
                    //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                    if (item.BaseEntry != null && item.BaseEntry != 0)
                    {
                        oDocuments.Lines.BaseEntry = item.BaseEntry;
                        oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                        oDocuments.Lines.BaseLine = item.BaseLine;
                        oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value = item.Quantity;
                    }
                    else
                    {
                        oDocuments.Lines.ItemCode = item.ItemCode;
                    }

                    oDocuments.Lines.Quantity = item.Quantity;

                    i = 0;

                    if (item.BinCode_from != null && item.BinCode_from != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_from);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    i = 1;

                    if (item.BinCode_to != null && item.BinCode_to != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_to);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    if (item.fromWhsCode != null && item.fromWhsCode != "")
                    {
                        oDocuments.Lines.FromWarehouseCode = item.fromWhsCode;
                    }

                    if (item.toWhsCode != null && item.toWhsCode != "")
                    {
                        oDocuments.Lines.WarehouseCode = item.toWhsCode;
                    }

                    foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                    {
                        oDocuments.Lines.BatchNumbers.Add();
                        oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                        oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                        oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                        i++;
                    }

                    oDocuments.Lines.Add();

                    i = 0;
                    check = true;
                }
                string belgeeklemehatasi = "";

                if (check)
                {
                    retval = oDocuments.Add();
                }

                if (retval != 0)
                {
                    try
                    {
                        belgeeklemehatasi = oCompany.GetLastErrorDescription();
                        //oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                    catch (Exception)
                    {
                    }

                    logger.Fatal("ID: " + ID + " HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription());
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + belgeeklemehatasi };
                }
                else
                {
                    if (inventoryGenEntry.InventoryGenEntryLines.Where(x => x.uretimdenGonderildi == "Y").Count() > 0)
                    {
                        oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                        dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                        oDocuments.DocDate = dt;
                        oDocuments.Comments = inventoryGenEntry.Comments;
                        oDocuments.CardCode = inventoryGenEntry.CardCode;

                        dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDueDate.Substring(6, 2))); //new

                        oDocuments.DueDate = dt; //new

                        //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                        //oDocuments.DocDueDate = dt;

                        //oDocuments.bpl = 1;
                        i = 0;
                        foreach (var item in inventoryGenEntry.InventoryGenEntryLines.Where(x => x.uretimdenGonderildi == "Y"))
                        {
                            //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                            if (item.BaseEntry != null && item.BaseEntry != 0)
                            {
                                oDocuments.Lines.BaseEntry = item.BaseEntry;
                                oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                                oDocuments.Lines.BaseLine = item.BaseLine;
                            }
                            else
                            {
                                oDocuments.Lines.ItemCode = item.ItemCode;
                            }

                            oDocuments.Lines.Quantity = item.Quantity;

                            if (item.fromWhsCode != null && item.fromWhsCode != "")
                            {
                                oDocuments.Lines.FromWarehouseCode = item.fromWhsCode;
                            }

                            if (item.toWhsCode != null && item.toWhsCode != "")
                            {
                                oDocuments.Lines.WarehouseCode = item.toWhsCode;
                            }

                            foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                            {
                                oDocuments.Lines.BatchNumbers.Add();
                                oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                                oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                                oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                                i++;
                            }

                            oDocuments.Lines.Add();

                            i = 0;
                        }

                        retval = oDocuments.Add();

                        if (retval != 0)
                        {
                            string hata = oCompany.GetLastErrorDescription();
                            try
                            {
                                //oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                            }
                            catch (Exception ex)
                            {
                            }
                            logger.Fatal("ID: " + ID + " HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription());
                            LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                            return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + hata };
                        }
                    }
                    try
                    {
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    }
                    catch (Exception)
                    {
                    }
                    logger.Info("ID: " + ID + " KAYIT BAŞARILI.");
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = 0, Desc = "KAYIT BAŞARILI." };
                }
            }
            catch (Exception ex)
            {
                try
                {
                    //oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                }
                catch (Exception)
                {
                }
                logger.Fatal("ID: " + ID + " BİLİNMEYEN HATA OLUŞTU." + ex.Message);
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
                return new Response { _list = null, Val = -999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            finally
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
            }
        }

        public Response addOrUpdateStockTransfer_2(string dbName, InventoryGenEntry inventoryGenEntry) //Taslak belgeyi onaylayan kişi için bu method kullanılıyor.
        {
            Random rastgele = new Random();
            int ID = rastgele.Next(0, 9999);
            int clNum = 0;
            string companyDbCode = "";
            try
            {
                //ConnectionList connection = new ConnectionList();

                //LoginCompany log = new LoginCompany();

                //connection = log.getSAPConnection(dbName);

                //if (connection.number == -1)
                //{
                //    for (int ix = 1; ix <= 3; ix++)
                //    {
                //        connection = log.getSAPConnection(dbName);

                //        if (connection.number > -1)
                //        {
                //            break;
                //        }
                //    }

                //}

                ConnectionList connection = new ConnectionList();

                LoginCompany log = new LoginCompany();

                log.DisconnectSAP(dbName);

                connection = log.getSAPConnection(dbName, ID);

                if (connection.number == -1)
                {
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }

                clNum = connection.number;
                companyDbCode = connection.dbCode;
                SAPbobsCOM.Company oCompany = connection.oCompany;

                SAPbobsCOM.StockTransfer oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer;

                SAPbobsCOM.StockTransfer oStockTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                DateTime dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                var belgenumaralari = inventoryGenEntry.InventoryGenEntryLines.Select(x => x.BaseEntry).ToList().Distinct();

                foreach (var item in belgenumaralari)
                {
                    oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                    oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer;
                    var aaaa = oDocuments.GetByKey(item);
                    oStockTransfer.DocDate = oDocuments.DocDate;
                    oStockTransfer.DueDate = oDocuments.DueDate;
                    oStockTransfer.TaxDate = oDocuments.TaxDate;
                    oStockTransfer.CardCode = oDocuments.CardCode;
                    oStockTransfer.FromWarehouse = oDocuments.FromWarehouse;
                    oStockTransfer.ToWarehouse = oDocuments.ToWarehouse;
                    //oDocuments.DocDate = dt;
                    //oDocuments.Comments = inventoryGenEntry.Comments;
                    //oDocuments.CardCode = inventoryGenEntry.CardCode;

                    //var asdas = oDocuments.Remove();
                    //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                    //oDocuments.DocDueDate = dt;

                    //oDocuments.bpl = 1;
                    //int i = 0;

                    var xml = oDocuments.GetAsXML();

                    //for (int i = oDocuments.Lines.Count - 1; i >= 0; i--)
                    //{
                    //    oDocuments.Lines.SetCurrentLine(i);
                    //    //if (inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.BaseLine == oDocuments.Lines.LineNum).Count() == 0)
                    //    //{
                    //    //    oDocuments.Lines.Delete();
                    //    //}
                    //    //else
                    //    //{
                    //    //    oDocuments.Lines.Quantity = inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.BaseLine == oDocuments.Lines.LineNum).Select(y => y.Quantity).FirstOrDefault();
                    //    //}

                    //}

                    for (int i = 0; i < oDocuments.Lines.Count; i++)
                    {
                        oDocuments.Lines.SetCurrentLine(i);
                        if (inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.BaseLine == oDocuments.Lines.LineNum).Count() == 0)
                        {
                            continue;
                        }

                        List<InventoryGenEntryLinesBatch> inventoryGenEntryLinesBatch = new List<InventoryGenEntryLinesBatch>();
                        if (inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry).Select(y => y.taslakGercek).FirstOrDefault() != "T")
                        {
                            oStockTransfer.Lines.BaseEntry = oDocuments.Lines.BaseEntry;

                            oStockTransfer.Lines.BaseLine = oDocuments.Lines.BaseLine;

                            oStockTransfer.Lines.BaseType = oDocuments.Lines.BaseType;

                            oStockTransfer.Lines.Quantity = Convert.ToDouble(inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.BaseLine == oDocuments.Lines.LineNum).Select(y => y.Quantity).FirstOrDefault());

                            inventoryGenEntryLinesBatch = inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.BaseLine == oDocuments.Lines.LineNum).Select(y => y.InventoryGenEntryLinesBatch).FirstOrDefault();
                        }
                        else
                        {
                            oStockTransfer.Lines.ItemCode = oDocuments.Lines.ItemCode;

                            oStockTransfer.Lines.FromWarehouseCode = oDocuments.Lines.FromWarehouseCode;

                            oStockTransfer.Lines.WarehouseCode = oDocuments.Lines.WarehouseCode;

                            oStockTransfer.Lines.Quantity = Convert.ToDouble(inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.taslakGercek == "T" && x.BaseLine == oDocuments.Lines.LineNum).Select(y => y.Quantity).FirstOrDefault());

                            inventoryGenEntryLinesBatch = inventoryGenEntry.InventoryGenEntryLines.Where(x => x.BaseEntry == oDocuments.DocEntry && x.taslakGercek == "T" && x.BaseLine == oDocuments.Lines.LineNum).Select(y => y.InventoryGenEntryLinesBatch).FirstOrDefault();
                        }

                        for (int x = 0; x <= oDocuments.Lines.BatchNumbers.Count - 1; x++)
                        {
                            oDocuments.Lines.BatchNumbers.SetCurrentLine(x);

                            if (inventoryGenEntryLinesBatch.Where(y => y.BatchNumber == oDocuments.Lines.BatchNumbers.BatchNumber).Count() > 0)
                            {
                                oStockTransfer.Lines.BatchNumbers.BatchNumber = oDocuments.Lines.BatchNumbers.BatchNumber;
                                //oStockTransfer.Lines.BatchNumbers.BatchNumber = oDocuments.Lines.BatchNumbers.BatchNumber;

                                oStockTransfer.Lines.BatchNumbers.Quantity = Convert.ToDouble(inventoryGenEntryLinesBatch.Where(y => y.BatchNumber == oDocuments.Lines.BatchNumbers.BatchNumber).Select(z => z.BatchQuantity).FirstOrDefault());

                                oStockTransfer.Lines.BatchNumbers.Add();
                            }
                        }

                        oStockTransfer.Lines.Add();
                    }
                }

                //foreach (var item in inventoryGenEntry.InventoryGenEntryLines)
                //{
                //    ////Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                //    //if (item.BaseEntry != null && item.BaseEntry != 0)
                //    //{
                //    //    oDocuments.Lines.BaseEntry = item.BaseEntry;
                //    //    oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                //    //    oDocuments.Lines.BaseLine = item.BaseLine;
                //    //}
                //    //else
                //    //{
                //    //    oDocuments.Lines.ItemCode = item.ItemCode;
                //    //}

                //    //oDocuments.Lines.Quantity = item.Quantity;

                //    //if (item.fromWhsCode != null && item.fromWhsCode != "")
                //    //{
                //    //    oDocuments.Lines.FromWarehouseCode = item.fromWhsCode;
                //    //}

                //    //if (item.toWhsCode != null && item.toWhsCode != "")
                //    //{
                //    //    oDocuments.Lines.WarehouseCode = item.toWhsCode;
                //    //}

                //    //foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                //    //{
                //    //    oDocuments.Lines.BatchNumbers.Add();
                //    //    oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                //    //    oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                //    //    oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                //    //    i++;
                //    //}

                //    //oDocuments.Lines.Add();

                //    //i = 0;
                //}

                //int retval = oDocuments.SaveDraftToDocument();
                int retval = oStockTransfer.Add();

                if (retval != 0)
                {
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription() };
                }
                else
                {
                    int stoktransfernumarasi = Convert.ToInt32(oCompany.GetNewObjectKey());

                    foreach (var item in belgenumaralari)
                    {
                        oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                        oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer;
                        oDocuments.GetByKey(item);

                        oStockTransfer = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);

                        oStockTransfer.GetByKey(stoktransfernumarasi);

                        for (int i = 0; i <= oStockTransfer.Lines.Count - 1; i++)
                        {
                            oStockTransfer.Lines.SetCurrentLine(i);

                            for (int x = 0; x <= oDocuments.Lines.Count - 1; x++)
                            {
                                oDocuments.Lines.SetCurrentLine(x);

                                if (oDocuments.Lines.ItemCode == oStockTransfer.Lines.ItemCode && oDocuments.Lines.FromWarehouseCode == oStockTransfer.Lines.FromWarehouseCode && oDocuments.Lines.WarehouseCode == oStockTransfer.Lines.WarehouseCode)
                                {
                                    oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value = Convert.ToDouble(oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value) == 0 ? 0 : Convert.ToDouble(oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value) - oStockTransfer.Lines.Quantity;
                                }
                            }
                        }

                        try
                        {
                            var aaa = oDocuments.Update();

                            oDocuments.GetByKey(Convert.ToInt32(oCompany.GetNewObjectKey()));

                            int satirSayisi = oDocuments.Lines.Count;
                            int sifirkalanacikMiktarSayisi = 0;
                            for (int i = 0; i <= oDocuments.Lines.Count - 1; i++)
                            {
                                oDocuments.Lines.SetCurrentLine(i);

                                if (Convert.ToDouble(oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value) == 0)
                                {
                                    sifirkalanacikMiktarSayisi++;
                                }
                            }

                            if (satirSayisi == sifirkalanacikMiktarSayisi)
                            {
                                var bb = oDocuments.Close();
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = 0, Desc = "KAYIT BAŞARILI." };
                }
            }
            catch (Exception ex)
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
                return new Response { _list = null, Val = -999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            finally
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
            }
        }

        public Response addOrUpdateInventoryGenEntry(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            Random rastgele = new Random();
            int ID = rastgele.Next(0, 9999);
            Logger logger = LogManager.GetCurrentClassLogger();

            //var requestJson_New = JsonConvert.SerializeObject(protocol);

            //logger.Info(" ");

            logger.Info("ID: " + ID + " addOrUpdateInventoryGenEntry Servisine Geldi.");
            //logger.Info("ID: " + ID + " ISTEK :" + requestJson_New);

            int clNum = 0;
            string companyDbCode = "";
            try
            {
                ConnectionList connection = new ConnectionList();

                LoginCompany log = new LoginCompany();

                log.DisconnectSAP(dbName);

                connection = log.getSAPConnection(dbName, ID);
                //if (connection.number == -1)
                //{
                //    for (int ix = 1; ix <= 3; ix++)
                //    {
                //        connection = log.getSAPConnection(dbName);

                //        if (connection.number > -1)
                //        {
                //            break;
                //        }
                //    }

                //}

                if (connection.number == -1)
                {
                    logger.Fatal("ID: " + ID + " " + "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu.");
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }
                clNum = connection.number;
                companyDbCode = connection.dbCode;
                SAPbobsCOM.Company oCompany = connection.oCompany;

                logger.Info("ID: " + ID + " Şirket bağlantısını başarıyla geçtik. Bağlantı sağladığımız DB :" + oCompany.CompanyDB + " clnum: " + clNum);

                SAPbobsCOM.Documents oDocuments = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry);

                DateTime dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                oDocuments.DocDate = dt;
                oDocuments.Comments = inventoryGenEntry.Comments;
                //oDocuments.CardCode = inventoryGenEntry.CardCode;

                //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                //oDocuments.DocDueDate = dt;

                oDocuments.BPL_IDAssignedToInvoice = 1;
                int i = 0;
                foreach (var item in inventoryGenEntry.InventoryGenEntryLines)
                {
                    //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                    //if (item.BaseEntry != null && item.BaseEntry != 0)
                    //{
                    //    oDocuments.Lines.BaseEntry = item.BaseEntry;
                    //    oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                    //    oDocuments.Lines.BaseLine = item.BaseLine;
                    //}
                    //else
                    //{
                    oDocuments.Lines.ItemCode = item.ItemCode;
                    //}

                    oDocuments.Lines.Quantity = item.Quantity;

                    if (item.BinCode != null && item.BinCode != "")
                    {
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    if (item.toWhsCode != null && item.toWhsCode != "")
                    {
                        oDocuments.Lines.WarehouseCode = item.toWhsCode;
                    }

                    foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                    {
                        oDocuments.Lines.BatchNumbers.Add();
                        oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                        oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                        oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                        i++;
                    }

                    oDocuments.Lines.Add();

                    i = 0;
                }

                int retval = oDocuments.Add();

                if (retval != 0)
                {
                    logger.Fatal("ID: " + ID + " HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription());
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription() };
                }
                else
                {
                    logger.Info("ID: " + ID + " KAYIT BAŞARILI.");
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = 0, Desc = "KAYIT BAŞARILI." };
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ID: " + ID + " BİLİNMEYEN HATA OLUŞTU." + ex.Message);
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
                return new Response { _list = null, Val = -999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            finally
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
            }
        }

        public Response addOrUpdateStockTransfer_3(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            Random rastgele = new Random();
            int ID = rastgele.Next(0, 9999);
            Logger logger = LogManager.GetCurrentClassLogger();

            //var requestJson_New = JsonConvert.SerializeObject(protocol);

            //logger.Info(" ");

            logger.Info("ID: " + ID + " addOrUpdateStockTransfer_3 Servisine Geldi.");
            //logger.Info("ID: " + ID + " ISTEK :" + requestJson_New);

            int clNum = 0;
            string companyDbCode = "";
            try
            {
                //ConnectionList connection = new ConnectionList();

                //LoginCompany log = new LoginCompany();

                //connection = log.getSAPConnection(dbName);

                //if (connection.number == -1)
                //{
                //    for (int ix = 1; ix <= 3; ix++)
                //    {
                //        connection = log.getSAPConnection(dbName);

                //        if (connection.number > -1)
                //        {
                //            break;
                //        }
                //    }

                //}

                ConnectionList connection = new ConnectionList();

                LoginCompany log = new LoginCompany();

                log.DisconnectSAP(dbName);

                connection = log.getSAPConnection(dbName, ID);

                if (connection.number == -1)
                {
                    logger.Fatal("ID: " + ID + " " + "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu.");
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }

                clNum = connection.number;
                companyDbCode = connection.dbCode;
                SAPbobsCOM.Company oCompany = connection.oCompany;

                logger.Info("ID: " + ID + " Şirket bağlantısını başarıyla geçtik. Bağlantı sağladığımız DB :" + oCompany.CompanyDB + " clnum: " + clNum);

                SAPbobsCOM.StockTransfer oDocuments = (SAPbobsCOM.StockTransfer)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransferDraft);
                oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oStockTransfer;

                DateTime dt = new DateTime(Convert.ToInt32(inventoryGenEntry.DocDate.Substring(0, 4)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(4, 2)), Convert.ToInt32(inventoryGenEntry.DocDate.Substring(6, 2)));

                oDocuments.DocDate = dt;
                oDocuments.Comments = inventoryGenEntry.Comments;
                oDocuments.CardCode = inventoryGenEntry.CardCode;

                //dt = new DateTime(Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(0, 4)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(4, 2)), Convert.ToInt32(goodRecieptPO.DocDueDate.Substring(6, 2)));

                //oDocuments.DocDueDate = dt;

                //oDocuments.bpl = 1;
                int i = 0;
                foreach (var item in inventoryGenEntry.InventoryGenEntryLines)
                {
                    //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                    if (item.BaseEntry != null && item.BaseEntry != 0)
                    {
                        oDocuments.Lines.BaseEntry = item.BaseEntry;
                        oDocuments.Lines.BaseType = SAPbobsCOM.InvBaseDocTypeEnum.InventoryTransferRequest;
                        oDocuments.Lines.BaseLine = item.BaseLine;
                    }
                    else
                    {
                        oDocuments.Lines.ItemCode = item.ItemCode;
                    }

                    oDocuments.Lines.Quantity = item.Quantity;

                    try
                    {
                        try
                        {
                            oDocuments.Lines.UserFields.Fields.Item("U_AcikMiktar").Value = oDocuments.Lines.Quantity;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    catch (Exception)
                    {
                    }

                    if (item.fromWhsCode != null && item.fromWhsCode != "")
                    {
                        oDocuments.Lines.FromWarehouseCode = item.fromWhsCode;
                    }

                    if (item.toWhsCode != null && item.toWhsCode != "")
                    {
                        oDocuments.Lines.WarehouseCode = item.toWhsCode;
                    }

                    i = 0;

                    if (item.BinCode_from != null && item.BinCode_from != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_from);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                        i++;
                    }

                    if (item.BinCode_to != null && item.BinCode_to != "")
                    {
                        oDocuments.Lines.BinAllocations.SetCurrentLine(i);
                        oDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = i;
                        oDocuments.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batToWarehouse;
                        oDocuments.Lines.BinAllocations.BinAbsEntry = Convert.ToInt32(item.BinCode_to);
                        oDocuments.Lines.BinAllocations.Quantity = Convert.ToInt32(item.Quantity);
                        oDocuments.Lines.BinAllocations.Add();
                    }

                    foreach (var aifteam in item.InventoryGenEntryLinesBatch)
                    {
                        oDocuments.Lines.BatchNumbers.Add();
                        oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                        oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                        oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                        i++;
                    }

                    oDocuments.Lines.Add();

                    i = 0;
                }

                int retval = oDocuments.Add();

                if (retval != 0)
                {
                    logger.Fatal("ID: " + ID + " HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription());
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription() };
                }
                else
                {
                    logger.Info("ID: " + ID + " KAYIT BAŞARILI.");
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = 0, Desc = "KAYIT BAŞARILI." };
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("ID: " + ID + " BİLİNMEYEN HATA OLUŞTU." + ex.Message);
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
                return new Response { _list = null, Val = -999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            finally
            {
                LoginCompany.ReleaseConnection(clNum, companyDbCode, ID);
            }
        }
    }
}