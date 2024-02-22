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
    //commit.
    public class GetProducts
    {
        public Response getProductsByBarCodeWithWareHouse(string dbName, string barCode, string wareHouse, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select TOP 1 T0.\"ItemCode\" as \"Kalem Kodu\",T0.\"ItemName\" as \"Kalem Tanımı\",T2.\"UomCode\" as \"Ölçü Birimi\",T0.\"CodeBars\" as \"Barkod\",T0.\"ManBtchNum\" as \"Partili\",ISNULL(T1.\"OnHand\",0) as \"Depo Miktari\"";

                    if (mKod == "10TRMN")
                    {
                        query += ",T0.U_KoliMiktari as \"KoliIciAD\" ,T0.U_PALET as \"PaletIciAD\",T0.U_PaletKoli  as \"PaletIciKoliAD\" ";
                    }
                    else
                    {
                        query += ",0 as \"KoliIciAD\" ,0 as \"PaletIciAD\",0 as \"PaletIciKoliAD\" ";
                    }

                    query += ",T3.\"Substitute\" as \"MuhatapKatalogNo\" from OITM as T0 WITH (NOLOCK) INNER JOIN OITW as T1 WITH (NOLOCK) ON T0.\"ItemCode\" = T1.\"ItemCode\" LEFT JOIN OUOM as T2 WITH (NOLOCK) ON T0.InvntryUom = T2.UomName LEFT JOIN OSCN as T3 WITH (NOLOCK) ON  T0.\"ItemCode\" = T3.\"ItemCode\" where CodeBars = '" + barCode + "' ";

                    if (wareHouse != "")
                    {
                        query += "and T1.\"WhsCode\" = '" + wareHouse + "'";
                    }

                    query += " AND T0.\"validFor\" = 'Y' and ISNULL(T0.\"AssetClass\",'') = ''  and ISNULL(T0.\"InvntItem\",'') = 'Y' ";

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
                                        dt.TableName = "ItemCodeByBarCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByItemCodeWithWareHouse(string dbName, string itemCode, string wareHouse, string cardCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select TOP 1 T0.\"ItemCode\" as \"Kalem Kodu\",T0.\"ItemName\" as \"Kalem Tanımı\",T2.\"UomCode\" as \"Ölçü Birimi\",T0.\"CodeBars\" as \"Barkod\",T0.\"ManBtchNum\" as \"Partili\",ISNULL(T1.\"OnHand\",0) as \"Depo Miktari\" ";

                    if (mKod == "10TRMN")
                    {
                        query += ",T0.U_KoliMiktari as \"KoliIciAD\" ,T0.U_PALET as \"PaletIciAD\",T0.U_PaletKoli  as \"PaletIciKoliAD\" ";
                    }
                    else
                    {
                        query += ",0 as \"KoliIciAD\" ,0 as \"PaletIciAD\",0  as \"PaletIciKoliAD\" ";
                    }

                    query += ",T3.\"Substitute\" as \"MuhatapKatalogNo\" from OITM as T0 WITH (NOLOCK) INNER JOIN OITW as T1 WITH (NOLOCK) ON T0.\"ItemCode\" = T1.\"ItemCode\" LEFT JOIN OUOM as T2 WITH (NOLOCK) ON T0.InvntryUom = T2.UomName LEFT JOIN OSCN as T3 WITH (NOLOCK) ON  T0.\"ItemCode\" = T3.\"ItemCode\"  where T0.\"ItemCode\" = '" + itemCode + "'";

                    if (wareHouse != "")
                    {
                        query += "and T1.\"WhsCode\" = '" + wareHouse + "'";
                    }

                    query += " AND T0.\"validFor\" = 'Y' and ISNULL(T0.\"AssetClass\",'') = '' and ISNULL(T0.\"InvntItem\",'') = 'Y' ";
                    //if (cardCode != "")
                    //{
                    //    query += "and T3.\"CardCode\" = '" + cardCode + "'";
                    //}
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
                                        dt.TableName = "ItemCodeByItemCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByItemCode(string dbName, string itemCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select T0.\"ItemCode\" as \"Kalem Kodu\",T0.\"ItemName\" as \"Kalem Tanımı\",T1.\"UomCode\" as \"Ölçü Birimi\",T0.\"CodeBars\" as \"Barkod\",T0.\"ManBtchNum\" as \"Partili\" from OITM as T0 WITH (NOLOCK) left join OUOM T1 WITH (NOLOCK) on T0.InvntryUom = T1.UomName where T0.\"ItemCode\" = '" + itemCode + "'";

                    query += " AND T0.\"validFor\" = 'Y' and ISNULL(T0.\"AssetClass\",'') = '' and ISNULL(T0.\"InvntItem\",'') = 'Y' ";

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
                                        dt.TableName = "ItemCodeByItemCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByBarCode(string dbName, string barCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select T0.\"ItemCode\" as \"Kalem Kodu\",T0.\"ItemName\" as \"Kalem Tanımı\",T1.\"UomCode\" as \"Ölçü Birimi\",T0.\"CodeBars\" as \"Barkod\",T0.\"ManBtchNum\" as \"Partili\" from OITM as T0 WITH (NOLOCK) left join OUOM T1 WITH (NOLOCK) on T0.InvntryUom = T1.UomName where CodeBars = '" + barCode + "' ";

                    query += " AND T0.\"validFor\" = 'Y' and ISNULL(T0.\"AssetClass\",'') = '' and ISNULL(T0.\"InvntItem\",'') = 'Y' ";
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
                                        dt.TableName = "ItemCodeByBarCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByBarCodeWithWhsDetails(string dbName, string barCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "select T0.\"ItemCode\" ,T0.\"ItemName\",T0.CodeBars,T2.WhsCode,T2.WhsName,T1.OnHand,T3.UomCode from OITM T0 WITH (NOLOCK) inner join OITW T1 WITH (NOLOCK) on T0.ItemCode = t1.ItemCode inner join OWHS T2  WITH (NOLOCK) on T1.WhsCode = T2.WhsCode LEFT join OUOM T3 WITH (NOLOCK) on T0.InvntryUom = T3.UomName where T0.CodeBars ='" + barCode + "'";
                    query += " and T1.[OnHand] > 0";

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
                                        dt.TableName = "ItemCodeByBarCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByItemCodeWithWhsDetails(string dbName, string itemCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "select T0.\"ItemCode\",T0.\"ItemName\",T0.CodeBars,T2.WhsCode,T2.WhsName,T1.OnHand,T3.UomCode from OITM T0 WITH (NOLOCK) inner join OITW T1 WITH (NOLOCK) on T0.ItemCode = t1.ItemCode inner join OWHS T2 WITH (NOLOCK) on T1.WhsCode = T2.WhsCode LEFT join OUOM T3WITH (NOLOCK) on T0.InvntryUom = T3.UomName where T0.ItemCode ='" + itemCode + "'";
                    query += " and T1.[OnHand] > 0";

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
                                        dt.TableName = "ItemCodeByItemCode";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProducts(string dbName, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select TOP 1 '' as \"ItemCode\",'' as \"ItemName\",'' as \"CodeBars\" from OITM UNION ALL Select \"ItemCode\", (ISNULL(\"ItemCode\",'') + '-' +  ISNULL(\"ItemName\",'')) as \"ItemName\",ISNULL(\"CodeBars\",'') AS \"CodeBars\" from OITM WITH (NOLOCK)";

                    query += " where \"validFor\" = 'Y' and ISNULL(\"AssetClass\",'') = '' and ISNULL(\"InvntItem\",'') = 'Y' ";

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
                                        dt.TableName = "ItemCodes";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsByMuhatapKatalogNoWithWareHouse(string dbName, string barCode, string wareHouse, string cardCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    var query = "Select ";
                    if (cardCode == "")
                    {
                        query += "TOP 1";
                    }
                    else
                    {
                        query += "DISTINCT";
                    }

                    query += " T0.\"ItemCode\" as \"Kalem Kodu\",T0.\"ItemName\" as \"Kalem Tanımı\",T2.\"UomCode\" as \"Ölçü Birimi\",T0.\"CodeBars\" as \"Barkod\",T0.\"ManBtchNum\" as \"Partili\",ISNULL(T1.\"OnHand\",0) as \"Depo Miktari\" ";

                    if (mKod == "10TRMN")
                    {
                        query += ",T0.U_KoliMiktari as \"KoliIciAD\" ,T0.U_PALET as \"PaletIciAD\",T0.U_PaletKoli  as \"PaletIciKoliAD\" ";
                    }
                    else
                    {
                        query += ",0  as \"KoliIciAD\",0 as \"PaletIciAD\",0 as \"PaletIciKoliAD\" ";
                    }

                    query += ",T3.\"Substitute\" as \"MuhatapKatalogNo\" from OITM as T0 WITH (NOLOCK) INNER JOIN OITW as T1 WITH (NOLOCK) ON T0.\"ItemCode\" = T1.\"ItemCode\" LEFT JOIN OUOM as T2 WITH (NOLOCK) ON T0.InvntryUom = T2.UomName LEFT JOIN OSCN as T3 WITH (NOLOCK) ON  T0.\"ItemCode\" = T3.\"ItemCode\"  where \"Substitute\" = '" + barCode + "' ";

                    if (wareHouse != "")
                    {
                        query += "and T1.\"WhsCode\" = '" + wareHouse + "'";
                    }

                    if (cardCode != "")
                    {
                        query += "and T3.\"CardCode\" = '" + cardCode + "'";
                    }

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
                                        dt.TableName = "ItemCodeByMuhatapKatalogNo";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KALEM BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }

        public Response getProductsMuhatapKatalogNo(string dbName, string barCode, string mKod)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnectionString n = new GetConnectionString();
                string connstring = n.getConnectionString(dbName, mKod);

                if (connstring != "")
                {
                    string query = "SELECT T0.\"CardCode\" as \"MUHATAP KODU\", T1.\"CardName\" AS \"MUHATAP ADI\",T0.\"ItemCode\" AS \"ÜRÜN KODU\",T2.\"ItemName\" AS \"ÜRÜN ADI\", T0.\"Substitute\" AS \"KATALOG NO\" FROM OSCN T0 WITH (NOLOCK) LEFT JOIN OCRD AS T1 WITH (NOLOCK) ON T0.\"CardCode\" = T1.\"CardCode\" LEFT JOIN OITM AS T2 WITH (NOLOCK) ON T0.\"ItemCode\" = T2.\"ItemCode\" WHERE T0.\"Substitute\" = '" + barCode + "'";

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
                                        dt.TableName = "MuhatapKatalogNoListesi";

                                        if (dt.Rows.Count == 0)
                                        {
                                            return new Response { _list = null, Val = -111, Desc = "KATALOG BULUNAMADI." };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return new Response { _list = dt, Val = 0 };
        }
    }
}