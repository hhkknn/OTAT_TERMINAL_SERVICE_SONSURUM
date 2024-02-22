using AIF.TerminalService.DatabaseLayer;
using AIF.TerminalService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AIF.TerminalService.SAPLayer
{
    public class AddOrUpdateReturnWaybillEntry
    {
        public Response addOrUpdateReturnWaybillEntry(string dbName, WaybillReturn waybillReturns)
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

                connection = log.getSAPConnection(dbName,ID);

                if (connection.number == -1)
                {
                    return new Response { Val = -3100, Desc = "Hata Kodu - 3100 Veritabanı bağlantısı sırasında hata oluştu. ", _list = null };
                }

                clNum = connection.number;
                companyDbCode = connection.dbCode;
                SAPbobsCOM.Company oCompany = connection.oCompany;

                #region iade
                //SAPbobsCOM.Documents oDocuments = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns); 
                #endregion

                #region iade taslak
                SAPbobsCOM.Documents oDocuments = (SAPbobsCOM.Documents)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                oDocuments.DocObjectCode = SAPbobsCOM.BoObjectTypes.oReturns;
                #endregion

                oDocuments.CardCode = waybillReturns.CardCode;
                oDocuments.NumAtCard = waybillReturns.WayBillNo;

                DateTime dt = new DateTime(Convert.ToInt32(waybillReturns.TaxDate.Substring(0, 4)), Convert.ToInt32(waybillReturns.TaxDate.Substring(4, 2)), Convert.ToInt32(waybillReturns.TaxDate.Substring(6, 2)));

                oDocuments.TaxDate = dt;
                oDocuments.Comments = waybillReturns.Comments;

                oDocuments.ShipToCode = waybillReturns.ShipToCode;

                //oDocuments.BPL_IDAssignedToInvoice = 1;??
                int i = 0;
                foreach (var item in waybillReturns.WaybillReturnDetails)
                {
                    //oDocuments.GetByKey(itemx.DocEntry);
                    //foreach (var item in itemx.InvoiceReturnDetails)
                    //{
                    //Satınalma siparişine bağlı satınalma siparişli mal girişi oluşturmaya yarar.
                    //if (item.LineNum != null)
                    //{
                    //    oDocuments.Lines.BaseEntry = item.DocEntry;
                    //    oDocuments.Lines.BaseType = 13;
                    //    oDocuments.Lines.BaseLine = item.LineNum.ToString() == "" ? 0 : Convert.ToInt32(item.LineNum);
                    //}

                    oDocuments.Lines.ItemCode = item.ItemCode;

                    oDocuments.Lines.Quantity = item.Quantity;

                    if (item.WareHouse != "" && item.WareHouse != null)
                    {
                        oDocuments.Lines.WarehouseCode = item.WareHouse;
                    }

                    foreach (var aifteam in item.BatchLists)
                    {
                        oDocuments.Lines.BatchNumbers.Add();
                        oDocuments.Lines.BatchNumbers.SetCurrentLine(i);
                        oDocuments.Lines.BatchNumbers.BatchNumber = aifteam.BatchNumber;
                        oDocuments.Lines.BatchNumbers.Quantity = aifteam.BatchQuantity;
                        i++;
                    }

                    #region OITM DE KDV ALANI DOLU OLANLARI GÖNDERİR

                    SAPbobsCOM.Recordset recordset = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    string sql = "Select \"U_Kdv\" from OITM Where \"ItemCode\" = '" + item.ItemCode + "' ";

                    recordset.DoQuery(sql);

                    if (recordset.RecordCount > 0)
                    {
                        oDocuments.Lines.VatGroup = recordset.Fields.Item("U_Kdv").Value.ToString();
                    } 

                    #endregion OITM DE KDV ALANI DOLU OLANLARI GÖNDERİR

                    oDocuments.Lines.Add();

                    i = 0;
                    //}
                }

                int retval = oDocuments.Add();

                if (retval != 0)
                {
                    LoginCompany.ReleaseConnection(connection.number, connection.dbCode, ID);
                    return new Response { _list = null, Val = -1200, Desc = "HATA KODU : -1200 HATA AÇIKLAMASI : " + oCompany.GetLastErrorDescription() };
                }
                else
                {
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

    }
}