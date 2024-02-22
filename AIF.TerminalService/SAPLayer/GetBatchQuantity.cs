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
    public class GetBatchQuantity
    {
        public Response getBatchQuantity(string dbName, string warehouse, string BatchNumber, string ItemCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "select SUM(T1.Quantity) as Miktar from OBTN T0 WITH (NOLOCK) inner join OBTQ T1 WITH (NOLOCK) on T0.ItemCode = T1.ItemCode and T0.SysNumber = T1.SysNumber inner join OITM T2 on T0.ItemCode = T2.ItemCode where T1.Quantity > 0 and T0.DistNumber = N'" + BatchNumber + "' and T1.WhsCode = '" + warehouse + "' and T0.ItemCode = '" + ItemCode + "'";

                    try
                    {
                        using (SqlConnection con = new SqlConnection(connstring))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 1000;
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    using (dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        dt.TableName = "BatchSum";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -333, Desc = "PARTİ NUMARASI BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Response { _list = null, Val = -9999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response { _list = null, Val = -9998, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getBathByItemCode(string dbName, string warehouse, string ItemCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "select T0.ItemCode, T2.ItemName, T0.DistNumber as BatchNum, T1.WhsCode,T3.WhsName, T1.Quantity ";
                    query += "from OBTN T0 WITH (NOLOCK)";
                    query += "inner join OBTQ T1 WITH (NOLOCK) on T0.ItemCode = T1.ItemCode and T0.SysNumber = T1.SysNumber ";
                    query += "inner join OITM T2 WITH (NOLOCK) on T0.ItemCode = T2.ItemCode ";
                    query += "inner join OWHS T3 WITH (NOLOCK) on T1.WhsCode = T3.WhsCode ";
                    query += "where T1.Quantity > 0 and T0.ItemCode = '" + ItemCode + "' and T1.WhsCode = '" + warehouse + "' ";
                    query += "order by T1.WhsCode, T0.DistNumber";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connstring))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 1000;
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    using (dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        dt.TableName = "BatchSum";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -333, Desc = "PARTİ NUMARASI BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Response { _list = null, Val = -9999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response { _list = null, Val = -9998, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getBatchByBatchNumber(string dbName, string BatchNumber, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "select T0.ItemCode as \"Kalem Kodu\", T2.ItemName as \"Kalem Tanımı\", T0.DistNumber as \"PartiNo\", T1.WhsCode as \"Depo Kodu\",T3.WhsName as \"Depo Adı\", T1.Quantity as \"Miktar\", T4.UomCode as \"Birim\" ";
                    query += "from OBTN T0 WITH (NOLOCK) ";
                    query += "inner join OBTQ T1 WITH (NOLOCK) on T0.ItemCode = T1.ItemCode and T0.SysNumber = T1.SysNumber ";
                    query += "inner join OITM T2 WITH (NOLOCK) on T0.ItemCode = T2.ItemCode inner join OUOM T4 on T2.InvntryUom = T4.UomName ";
                    query += "inner join OWHS T3 WITH (NOLOCK) on T1.WhsCode = T3.WhsCode ";
                    query += "where T1.Quantity > 0 and T0.DistNumber = N'" + BatchNumber + "'";
                    query += "order by T1.WhsCode, T0.DistNumber";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connstring))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 1000;
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    using (dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        dt.TableName = "BatchSum";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -333, Desc = "PARTİ NUMARASI BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Response { _list = null, Val = -9999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response { _list = null, Val = -9998, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getBathByItemCodeToDraftDocument(string dbName, string warehouse, List<string> itemCodes, int DocEntry, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    string query = "";

                    //"select T0.ItemCode, T2.ItemName, T0.DistNumber as BatchNum, T1.WhsCode,T3.WhsName, T1.Quantity ";
                    //query += "from OBTN T0 ";
                    //query += "inner join OBTQ T1 on T0.ItemCode = T1.ItemCode and T0.SysNumber = T1.SysNumber ";
                    //query += "inner join OITM T2 on T0.ItemCode = T2.ItemCode ";
                    //query += "inner join OWHS T3 on T1.WhsCode = T3.WhsCode ";
                    //query += "where T1.Quantity > 0 and T0.ItemCode = '" + ItemCode + "' and T1.WhsCode = '" + warehouse + "' ";
                    //query += "order by T1.WhsCode, T0.DistNumber";

                    query = "SELECT DISTINCT  DRF1.ItemCode, OITM.ItemName,CASE WHEN OITM.ManBtchNum = 'Y' THEN obtn.distnumber WHEN OITM.ManSerNum = 'Y' THEN osrn.DistNumber  END AS BatchNum,";
                    if (mKod == "20TRMN")
                    {
                        query += "obtn.mnfserial AS MnfSerial,";
                    }
                    query += "(Select TOP 1 WhsCode from OBTQ as T11 where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as KaynakDepo, ";
                    query += "(Select TOP 1 WhsName from OBTQ as T11 INNER JOIN OWHS as T22 ON T11.WhsCode = T22.WhsCode where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as KaynakDepoAdi, ";
                    //query += "(Select TOP 1 Quantity from OBTQ as T11 where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as Quantity, ";
                    //query +=" CASE WHEN ISNULL(DRF1.U_AcikMiktar, 0) > 0 THEN ISNULL(DRF1.U_AcikMiktar, 0) ELSE DRF16.Quantity END Quantity,";

                    #region sorgu yavaş geldiğinden bu kısım kaldırıldı 20221103
                    query += " DRF16.Quantity  AS Quantity,";
                    //query += "DRF16.Quantity - (Select ISNULL(SUM(T97.Quantity),0) from OWTR as t99 INNER JOIN WTR1 as T98 ON T99.DocEntry = T98.DocEntry LEFT JOIN IBT1 AS T97 ON T97.BaseEntry = odrf.DocEntry and T97.BaseType = '67' and T97.LineNum = T98.LineNum ";

                    //query += " and T97.BatchNum = obtn.DistNumber ";

                    //query += " where t99.draftKey = odrf.DocEntry) AS Quantity,"; 
                    #endregion
                    query += "OITM.ManBtchNum as \"Partili\" ,OITM.ManSerNum as \"Serili\",";
                    query += "odrf.docentry,DRF1.LineNum  ";
                    query += "FROM odrf JOIN DRF1 ON odrf.docentry = DRF1.docentry ";
                    query += "LEFT JOIN DRF16 ON DRF1.docentry = DRF16.absentry AND DRF1.linenum = drf16.linenum ";
                    query += "LEFT JOIN obtn ON DRF16.objabs = obtn.absentry ";
                    query += "LEFT JOIN osrn ON DRF16.objabs = osrn.absentry ";
                    query += "INNER JOIN OITM ON OITM.ItemCode = DRF1.ItemCode ";
                    query += "where DRF1.dscription IS NOT NULL AND odrf.objtype = 67  and";
                    query += " odrf.DocEntry = '" + DocEntry + "'";

                    if (itemCodes != null)
                    {
                        if (itemCodes.Count > 0)
                        {
                            var values = "";
                            foreach (var item in itemCodes)
                            {
                                values += "'" + item + "'" + ",";
                            }

                            if (values != "")
                            {
                                values = values.Remove(values.Length - 1, 1);
                            }

                            query += " and (DRF1.ItemCode IN (" + values + "))";
                        }
                    }
                    //query += "order by T3OBTQ.WhsCode, obtn.DistNumber";
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connstring))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandTimeout = 1000;
                                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                                {
                                    using (dt = new DataTable())
                                    {
                                        sda.Fill(dt);
                                        dt.TableName = "BatchSum";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -333, Desc = "PARTİ NUMARASI BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Response { _list = null, Val = -9999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response { _list = null, Val = -9998, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            }
            #region otatta yavaş kapattım 20231110
            //try
            //{
            //    GetConnectionString n = new GetConnectionString();
            //    string connstring = n.getConnectionString(dbName, mKod);

            //    if (connstring != "")
            //    {
            //        string query = "";

            //        //"select T0.ItemCode, T2.ItemName, T0.DistNumber as BatchNum, T1.WhsCode,T3.WhsName, T1.Quantity ";
            //        //query += "from OBTN T0 ";
            //        //query += "inner join OBTQ T1 on T0.ItemCode = T1.ItemCode and T0.SysNumber = T1.SysNumber ";
            //        //query += "inner join OITM T2 on T0.ItemCode = T2.ItemCode ";
            //        //query += "inner join OWHS T3 on T1.WhsCode = T3.WhsCode ";
            //        //query += "where T1.Quantity > 0 and T0.ItemCode = '" + ItemCode + "' and T1.WhsCode = '" + warehouse + "' ";
            //        //query += "order by T1.WhsCode, T0.DistNumber";

            //        query = "SELECT DRF1.ItemCode, OITM.ItemName,obtn.distnumber as BatchNum,";
            //        query += "(Select TOP 1 WhsCode from OBTQ as T11 where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as WhsCode, ";
            //        query += "(Select TOP 1 WhsName from OBTQ as T11 INNER JOIN OWHS as T22 ON T11.WhsCode = T22.WhsCode where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as WhsCode, ";
            //        //query += "(Select TOP 1 Quantity from OBTQ as T11 where T11.ItemCode = DRF1.ItemCode and T11.SysNumber = obtn.SysNumber) as Quantity, ";
            //        //query +=" CASE WHEN ISNULL(DRF1.U_AcikMiktar, 0) > 0 THEN ISNULL(DRF1.U_AcikMiktar, 0) ELSE DRF16.Quantity END Quantity,";
            //        query += "DRF16.Quantity - (Select ISNULL(SUM(T97.Quantity),0) from OWTR as t99 INNER JOIN WTR1 as T98 ON T99.DocEntry = T98.DocEntry LEFT JOIN IBT1 AS T97 ON T97.BaseEntry = odrf.DocEntry and T97.BaseType = '67' and T97.LineNum = T98.LineNum ";

            //        query += " and T97.BatchNum = obtn.DistNumber ";

            //        query += " where t99.draftKey = odrf.DocEntry) AS Quantity,";
            //        query += "odrf.docentry,DRF1.LineNum ";
            //        query += "FROM odrf WITH (NOLOCK) JOIN DRF1 WITH (NOLOCK) ON odrf.docentry = DRF1.docentry ";
            //        query += "LEFT JOIN DRF16 WITH (NOLOCK) ON DRF1.docentry = DRF16.absentry AND DRF1.linenum = drf16.linenum ";
            //        query += "LEFT JOIN obtn  WITH (NOLOCK) ON DRF16.objabs = obtn.absentry ";
            //        query += "INNER JOIN OITM WITH (NOLOCK) ON OITM.ItemCode = DRF1.ItemCode ";
            //        query += "where DRF1.dscription IS NOT NULL AND odrf.objtype = 67  and";
            //        query += " odrf.DocEntry = '" + DocEntry + "'";

            //        if (itemCodes != null)
            //        {
            //            if (itemCodes.Count > 0)
            //            {
            //                var values = "";
            //                foreach (var item in itemCodes)
            //                {
            //                    values += "'" + item + "'" + ",";
            //                }

            //                if (values != "")
            //                {
            //                    values = values.Remove(values.Length - 1, 1);
            //                }

            //                query += " and (DRF1.ItemCode IN (" + values + "))";
            //            }
            //        }
            //        //query += "order by T3OBTQ.WhsCode, obtn.DistNumber";
            //        try
            //        {
            //            using (SqlConnection con = new SqlConnection(connstring))
            //            {
            //                using (SqlCommand cmd = new SqlCommand(query, con))
            //                {
            //                    cmd.CommandType = CommandType.Text;
            //                    cmd.CommandTimeout = 1000;
            //                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //                    {
            //                        using (dt = new DataTable())
            //                        {
            //                            sda.Fill(dt);
            //                            dt.TableName = "BatchSum";

            //                            if (dt.Rows.Count == 0)
            //                            {
            //                                return new Response { _list = null, Val = -333, Desc = "PARTİ NUMARASI BULUNAMADI." };
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            return new Response { _list = null, Val = -9999, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return new Response { _list = null, Val = -9998, Desc = "BİLİNMEYEN HATA OLUŞTU." + ex.Message };
            //} 
            #endregion
            return new Response { _list = dt, Val = 0 };
        }
    }
}