using AIF.TerminalService.Models;
using AIF.TerminalService.SAPLayer;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;

namespace AIF.TerminalService
{
    /// <summary>
    /// Summary description for AIFTerminalService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class AIFTerminalService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public Response GetcompanyList(string dbName, string mKod)
        {
            GetCompanyList n = new GetCompanyList();
            Response resp = n.getCompanyList(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response Login(string userName, string Password, string dbName, string mKod)
        {
            LoginCompany l = new LoginCompany();
            Response resp = l.Connect(userName, Password, dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetButtonAuthorization(string userName, string dbName, string mKod)
        {
            GetButtonsAuthorization l = new GetButtonsAuthorization();
            Response resp = l.getButtonAuthorization(userName, dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response AddGoodRecieptWithoutOrder(string dbName, string mKod)
        {
            GoodRecieptWithoutOrder l = new GoodRecieptWithoutOrder();
            Response resp = l.AddGoodRecieptWithoutOrder(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBusinessVendorPartner(string dbName, string mKod)
        {
            GetBusinessPartner l = new GetBusinessPartner();
            Response resp = l.getBusinessVendorPartner(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBusinessCustomerPartner(string dbName, string mKod)
        {
            GetBusinessPartner l = new GetBusinessPartner();
            Response resp = l.getBusinessCustomerPartner(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetAllBusinessPartner(string dbName, string mKod)
        {
            GetBusinessPartner l = new GetBusinessPartner();
            Response resp = l.getAllBusinessPartner(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetWareHouse(string dbName, string mKod)
        {
            GetWareHouse l = new GetWareHouse();
            Response resp = l.getWareHouse(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetWareHouseDetailsWithQty(string dbName, string warehouse, string mKod)
        {
            GetWareHouse l = new GetWareHouse();
            Response resp = l.getWareHouseDetailsWithQty(dbName, warehouse, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetWareHouseByUserCodeAddName(string dbName, string kullaniciId, string belgeTipi, string mKod)
        {
            GetWareHouse l = new GetWareHouse();
            Response resp = l.getWareHouseByUserCodeAddName(dbName, kullaniciId, belgeTipi, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetAllProducts(string dbName, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProducts(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetWareHouseByUserCode(string dbName, string kullaniciId, string belgeTipi, string mKod)
        {
            GetWareHouse l = new GetWareHouse();
            Response resp = l.getWareHouseByUserCode(dbName, kullaniciId, belgeTipi, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetPurchaseOrders(string dbName, string mKod)
        {
            GetPurchaseOrders l = new GetPurchaseOrders();
            Response resp = l.getPurchaseOrders(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetPurchaseOrdersByDate(string dbName, string startDate, string endDate, List<string> WhsCodes, string mKod)
        {
            GetPurchaseOrders l = new GetPurchaseOrders();
            Response resp = l.getPurchaseOrdersByDate(dbName, startDate, endDate, WhsCodes, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetPurchaseOrdersDetails(string dbName, List<string> docEntryList, List<string> WhsCodes, string mKod)
        {
            GetPurchaseOrders l = new GetPurchaseOrders();
            Response resp = l.getPurchaseOrderDetils(dbName, docEntryList, WhsCodes, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductsByItemCodeWithWhsDetails(string dbName, string itemCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByItemCodeWithWhsDetails(dbName, itemCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductsByBarCodeWithWhsDetails(string dbName, string barCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByBarCodeWithWhsDetails(dbName, barCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductByBarCodeWithWareHouse(string dbName, string barCode, string wareHouse, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByBarCodeWithWareHouse(dbName, barCode, wareHouse, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductByMuhatapKatalogNoWithWareHouse(string dbName, string barCode, string wareHouse, string cardCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByMuhatapKatalogNoWithWareHouse(dbName, barCode, wareHouse, cardCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductByItemCode(string dbName, string itemCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByItemCode(dbName, itemCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductByBarCode(string dbName, string barCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByBarCode(dbName, barCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetProductByItemCodeWithWareHouse(string dbName, string itemCode, string wareHouse, string cardCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsByItemCodeWithWareHouse(dbName, itemCode, wareHouse, cardCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetInventoryTransferRequest(string dbName, string startDate, string endDate, string fromWhsCode, string toWhsCode, List<string> WhsCodes, string kullaniciKodu, string mKod)
        {
            GetInventoryTransferRequest l = new GetInventoryTransferRequest();
            Response resp = l.getInventoryTransferRequestByDate(dbName, startDate, endDate, fromWhsCode, toWhsCode, WhsCodes, kullaniciKodu, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetDraftInventoryTransferRequest(string dbName, string startDate, string endDate, string fromWhsCode, string toWhsCode, List<string> WhsCodes, string kullaniciKodu, string talepsizDepoNaklindeTaslakBelgeOlustur, string mKod)
        {
            GetInventoryTransferRequest l = new GetInventoryTransferRequest();
            Response resp = l.getDraftInventoryTransferRequestByDate(dbName, startDate, endDate,  WhsCodes, kullaniciKodu, talepsizDepoNaklindeTaslakBelgeOlustur, fromWhsCode, toWhsCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchNextNumber(string dbName, string mKod)
        {
            GetBatchCustomTable l = new GetBatchCustomTable();
            Response resp = l.getBatchNextNumber(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response UpdateBatchNextNumber(string dbName, int docentry, int nextnumber, string mKod)
        {
            GetBatchCustomTable l = new GetBatchCustomTable();
            Response resp = l.updateBatchNextNumber(dbName, docentry, nextnumber, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchQuantity(string dbName, string warehouse, string batchnumber, string itemCode, string mKod)
        {
            GetBatchQuantity l = new GetBatchQuantity();
            Response resp = l.getBatchQuantity(dbName, warehouse, batchnumber, itemCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchByBatchNumber(string dbName, string batchnumber, string mKod)
        {
            GetBatchQuantity l = new GetBatchQuantity();
            Response resp = l.getBatchByBatchNumber(dbName, batchnumber, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchByItemCode(string dbName, string warehouse, string itemcode, string mKod)
        {
            GetBatchQuantity l = new GetBatchQuantity();
            Response resp = l.getBathByItemCode(dbName, warehouse, itemcode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchByItemCodeToDraftDocument(string dbName, string warehouse, List<string> itemCodes, int DocEntry, string mKod)
        {
            GetBatchQuantity l = new GetBatchQuantity();
            Response resp = l.getBathByItemCodeToDraftDocument(dbName, warehouse, itemCodes, DocEntry, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetBatchNumberByCardCodeAndItemCode(string dbName, string cardcode, string itemcode, string mKod)
        {
            GetBatchNumber l = new GetBatchNumber();
            Response resp = l.getBatchNumberByCardCodeAndItemCode(dbName, cardcode, itemcode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetInventoryTransferRequestDetails(string dbName, List<string> docEntryList, List<string> WhsCodes, string kullaniciKodu, string mKod)
        {
            GetInventoryTransferRequest l = new GetInventoryTransferRequest();
            Response resp = l.getInventoryTransferRequestDetails(dbName, docEntryList, WhsCodes, kullaniciKodu, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetDraftInventoryTransferRequestDetails(string dbName, List<string> docEntryList, List<string> WhsCodes, string kullaniciKodu, string talepsizDepoNaklindeTaslakBelgeOlustur, string fromwhsCode, string toWhsCode, string mKod)
        {
            GetInventoryTransferRequest l = new GetInventoryTransferRequest();
            Response resp = l.getDraftInventoryTransferRequestDetails(dbName, docEntryList, WhsCodes, kullaniciKodu, talepsizDepoNaklindeTaslakBelgeOlustur, fromwhsCode, toWhsCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetOrdersByDate(string dbName, string startDate, string endDate, List<string> WhsCodes, string mKod)
        {
            GetOrders l = new GetOrders();
            Response resp = l.getOrdersByDate(dbName, startDate, endDate, WhsCodes, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetOrdersDetails(string dbName, List<string> docEntryList, List<string> WhsCodes, string mKod)
        {
            GetOrders l = new GetOrders();
            Response resp = l.getOrdersDetails(dbName, docEntryList, WhsCodes, mKod);
            return resp;
        }

        [WebMethod]
        public Response AddPurchaseOrder(string dbName, PurchaseOrders purchaserOrders)
        {
            AddOrUpdatePurchaseOrder l = new AddOrUpdatePurchaseOrder();
            Response resp = l.addOrUpdatePurchaseOrder(dbName, purchaserOrders);
            return resp;
        }

        [WebMethod]
        public Response AddGoodRecieptPO(string dbName, GoodRecieptPO goodRecieptPO)
        {
            AddOrUpdateGoodRecieptPO l = new AddOrUpdateGoodRecieptPO();
            Response resp = l.addOrUpdateGoodRecieptPO(dbName, goodRecieptPO);
            return resp;
        }

        [WebMethod]
        public Response AddGoodRecieptPODraft(string dbName, GoodRecieptPO goodRecieptPO)
        {
            AddOrUpdateGoodRecieptPO l = new AddOrUpdateGoodRecieptPO();
            Response resp = l.addOrUpdateGoodRecieptPODraft(dbName, goodRecieptPO);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateReturnWaybillEntry(string dbName, WaybillReturn waybillReturn)
        {
            AddOrUpdateReturnWaybillEntry l = new AddOrUpdateReturnWaybillEntry();
            Response resp = l.addOrUpdateReturnWaybillEntry(dbName, waybillReturn);
            return resp;
        }

        [WebMethod]
        public Response GetInvoices(string dbName, string startDate, string endDate, string mKod)
        {
            GetInvoices l = new GetInvoices();
            Response resp = l.getInvoices(dbName, startDate, endDate, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetInvoicesDetails(string dbName, List<string> docEntryList, string mKod)
        {
            GetInvoices l = new GetInvoices();
            Response resp = l.getInvoicesDetails(dbName, docEntryList, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetDeliveryNote(string dbName, string startDate, string endDate, string mKod)
        {
            GetDeliveryNote l = new GetDeliveryNote();
            Response resp = l.getDeliveryNote(dbName, startDate, endDate, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetDeliveryNoteDetails(string dbName, List<string> docEntryList, string mKod)
        {
            GetDeliveryNote l = new GetDeliveryNote();
            Response resp = l.getDeliveryNoteDetails(dbName, docEntryList, mKod);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateStockTransfer(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            AddOrUpdateInventoryGenEntry l = new AddOrUpdateInventoryGenEntry();
            Response resp = l.addOrUpdateStockTransfer(dbName, inventoryGenEntry);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateStockTransferDraft(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            AddOrUpdateInventoryGenEntry l = new AddOrUpdateInventoryGenEntry();
            Response resp = l.addOrUpdateStockTransferDraft(dbName, inventoryGenEntry);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateStockTransfer_2(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            AddOrUpdateInventoryGenEntry l = new AddOrUpdateInventoryGenEntry();
            Response resp = l.addOrUpdateStockTransfer_2(dbName, inventoryGenEntry);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateStockTransfer_3(string dbName, InventoryGenEntry inventoryGenEntry) //Talepsiz depo nakli ekranında onaylamada eğer taslak oluşturulmak isteniyorsa bu method kullanılıyor.
        {
            AddOrUpdateInventoryGenEntry l = new AddOrUpdateInventoryGenEntry();
            Response resp = l.addOrUpdateStockTransfer_3(dbName, inventoryGenEntry);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateOrders(string dbName, Orders orders)
        {
            AddOrUpdateOrders l = new AddOrUpdateOrders();
            Response resp = l.addOrUpdateOrders(dbName, orders);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateInventoryGenEntry(string dbName, InventoryGenEntry inventoryGenEntry)
        {
            AddOrUpdateInventoryGenEntry l = new AddOrUpdateInventoryGenEntry();
            Response resp = l.addOrUpdateInventoryGenEntry(dbName, inventoryGenEntry);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateInventoryGenExit(string dbName, InventoryGenExit inventoryGenExit)
        {
            AddOrUpdateInventoryGenExit l = new AddOrUpdateInventoryGenExit();
            Response resp = l.addOrUpdateInventoryGenExit(dbName, inventoryGenExit);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateInvoiceReturn(string dbName, InvoiceReturn invoiceReturns)
        {
            AddOrUpdateReturns l = new AddOrUpdateReturns();
            Response resp = l.addOrUpdateReturns(dbName, invoiceReturns);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateDeliveryNote(string dbName, DeliveryNotesReturns deliveryNotesReturns)
        {
            AddOrUpdateDeliveryNotes l = new AddOrUpdateDeliveryNotes();
            Response resp = l.addOrUpdateDeliveryNotes(dbName, deliveryNotesReturns);
            return resp;
        }

        [WebMethod]
        public Response AddOrUpdateStockCounting(string dbName, StockCounting stockCounting)
        {
            AddOrUpdateStockCounting l = new AddOrUpdateStockCounting();
            Response resp = l.addOrUpdateStockCounting(dbName, stockCounting);
            return resp;
        }

        [WebMethod]
        public Response getCountingCustomTable(string dbName, string kullaniciId, string mKod)
        {
            GetCountingCustomTable l = new GetCountingCustomTable();
            Response resp = l.getCountingCustomTable(dbName, kullaniciId, mKod);
            return resp;
        }

        [WebMethod]
        public Response getLinesCountingCustomTableByDocEntry(string dbName, int DocEntry, string mKod)
        {
            GetCountingCustomTable l = new GetCountingCustomTable();
            Response resp = l.getLinesCountingCustomTableByDocEntry(dbName, DocEntry, mKod);
            return resp;
        }

        [WebMethod]
        public Response getPartiesCountingCustomTableByDocEntry(string dbName, int DocEntry, string mKod)
        {
            GetCountingCustomTable l = new GetCountingCustomTable();
            Response resp = l.getPartiesCountingCustomTableByDocEntry(dbName, DocEntry, mKod);
            return resp;
        }

        [WebMethod]
        public Response getShipmentTypes(string dbName, string mKod)
        {
            GetShipmentType l = new GetShipmentType();
            Response resp = l.getShitmentType(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response getGenelParametreler(string dbName, string mKod)
        {
            GetGenelParametreler l = new GetGenelParametreler();
            Response resp = l.getGenelParametreler(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response getDepoYerleri(string dbName, string warehouseCode, string mKod)
        {
            GetDepoYerleri l = new GetDepoYerleri();
            Response resp = l.getDepoYerleri(dbName, warehouseCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response getDepoYeriMiktarlari(string dbName, string urunKod, string mKod)
        {
            GetDepoYeriMiktarlari l = new GetDepoYeriMiktarlari();
            Response resp = l.getDepoYeriMiktarlari(dbName, urunKod, mKod);
            return resp;
        }

        [WebMethod]
        public Response getSonSatinalmaBelgesiNo(string dbName, string urunKod, string mKod)
        {
            GetSonSatinalmaBelgesiNo l = new GetSonSatinalmaBelgesiNo();
            Response resp = l.getSonSatinalmaBelgesiNo(dbName, urunKod, mKod);
            return resp;
        }

        [WebMethod]
        public Response getCekmeListesi(string dbName, string baslangicTarihi, string bitisTarihi, string mKod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getCekmeListesi(dbName, baslangicTarihi, bitisTarihi, mKod);
            return resp;
        }


        [WebMethod]
        public Response getCekmeListesiMusterileri(string dbName, string mKod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getCekmeListesiMusterileri(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response getCekmeListesiMusteriyeGoreUrunListesi(string dbName, string urunKodu, string mKod, string musterikod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getCekmeListesiMusteriyeGoreUrunListesi(dbName, urunKodu, mKod, musterikod);
            return resp;
        }

        [WebMethod]
        public Response getCekmeListesiDetaylari(string dbName, string docEntry, string mKod, string cekmeListesiKalemleriniGrupla)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getCekmeListesiDetaylari(dbName, docEntry, mKod, cekmeListesiKalemleriniGrupla);
            return resp;
        }


        [WebMethod]
        public Response getCekmeListesiKoliDetaylari(string dbName, string docEntry, string mKod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getCekmeListesiKoliDetaylari(dbName, docEntry, mKod);
            return resp;
        }

        [WebMethod]
        public Response getKalemKoduMuhatapKatalogNoBarkod(string dbName, string itemCode, string cardCode, string mKod)
        {
            GetKalemKoduMuhKatBarkod l = new GetKalemKoduMuhKatBarkod();
            Response resp = l.getKalemKoduMuhKatBarkod(dbName, itemCode, cardCode, mKod);
            return resp;
        }

        [WebMethod]
        public Response getPaletYapmaListesi(string dbName, string pasifleriGoster, string mKod)
        {
            GetPaletYapmaListesi l = new GetPaletYapmaListesi();
            Response resp = l.getPaletYapmaListesi(dbName, pasifleriGoster, mKod);
            return resp;
        }

        [WebMethod]
        public Response getPaletYapmaListesiDetaylari(string dbName, string paletNo, string mKod, string cekmeListesiKalemleriniGrupla)
        {
            GetPaletYapmaListesi l = new GetPaletYapmaListesi();
            Response resp = l.getPaletYapmaDetaylari(dbName, paletNo, mKod, cekmeListesiKalemleriniGrupla);
            return resp;
        }


        [WebMethod]
        public Response addOrUpdatePaletYapmaListesi(string dbName, PaletYapma paletYapma)
        {
            AddOrUpdatePaletYapmaListesi l = new AddOrUpdatePaletYapmaListesi();
            Response resp = l.addOrUpdatePaletYapmaListesi(dbName, paletYapma);
            return resp;
        }

        [WebMethod]
        public Response deletePaletListesi(string dbName, List<SilinecekPaletler> silinecekPaletlers, string cekmeListesiKalemleriniGrupla)
        {
            DeletePaletListesi l = new DeletePaletListesi();
            Response resp = l.deletePaletListesi(dbName, silinecekPaletlers, cekmeListesiKalemleriniGrupla);
            return resp;
        }

        [WebMethod]
        public Response getKonteynerYapmaListesi(string dbName, string mKod)
        {
            GetKonteynerYapmaListesi l = new GetKonteynerYapmaListesi();
            Response resp = l.getKonteynerYapmaListesi(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response getKonteynerYapmaListesiDetaylari(string dbName, string konteynerNo, string mKod)
        {
            GetKonteynerYapmaListesi l = new GetKonteynerYapmaListesi();
            Response resp = l.getKonteynerYapmaDetaylari(dbName, konteynerNo, mKod);
            return resp;
        }

        [WebMethod]
        public Response addOrUpdateKonteynerYapmaListesi(string dbName, KonteynerYapma konteynerYapma, List<KonteynerIcindenSilinenler> konteynerIcindenSilinenlers, string cekmeListesiKalemleriniGrupla)
        {
            AddOrUpdateKonteynerYapmaListesi l = new AddOrUpdateKonteynerYapmaListesi();
            Response resp = l.addOrUpdateKonteynerYapmaListesi(dbName, konteynerYapma, konteynerIcindenSilinenlers, cekmeListesiKalemleriniGrupla);
            return resp;
        }

        [WebMethod]
        public Response addOrUpdateKonteynerYapmaListesi_Final(string dbName, KonteynerYapma konteynerYapma, List<KonteynerIcindenSilinenler> konteynerIcindenSilinenlers, string cekmeListesiKalemleriniGrupla)
        {
            AddOrUpdateKonteynerYapmaListesi l = new AddOrUpdateKonteynerYapmaListesi();
            Response resp = l.addOrUpdateKonteynerYapmaListesi_Final(dbName, konteynerYapma, konteynerIcindenSilinenlers, cekmeListesiKalemleriniGrupla);
            return resp;
        }

        [WebMethod]
        public Response addOrUpdateCekmeListesiOnaylama(string dbName, CekmeListesiOnaylama cekmeListesiOnaylama, List<SilinenPaletNoListesi> silinenPaletNoListesis, string cekmeListesiKalemleriniGrupla)
        {
            AddOrUpdateCekmeListesiOnay l = new AddOrUpdateCekmeListesiOnay();
            Response resp = l.addOrUpdateCekmeListesiOnay(dbName, cekmeListesiOnaylama, silinenPaletNoListesis, cekmeListesiKalemleriniGrupla);
            return resp;
        }


        [WebMethod]
        public Response GetPaletNumarasiGetir(string dbName, string mKod)
        {
            GetPaletNumarasiCustomTable l = new GetPaletNumarasiCustomTable();
            Response resp = l.getPaletNumarasiCustomTable(dbName, mKod);
            return resp;
        }

        [WebMethod]
        public Response UpdatePaletNumarasi(string dbName, int docentry, int nextnumber, string mKod)
        {
            GetPaletNumarasiCustomTable l = new GetPaletNumarasiCustomTable();
            Response resp = l.updatePaletNumarasi(dbName, docentry, nextnumber, mKod);
            return resp;
        }

        [WebMethod]
        public Response getCountingCustomTableByDate(string dbName, string kullaniciid, string sayimtarihi, string mKod)
        {
            GetCountingCustomTable l = new GetCountingCustomTable();
            Response resp = l.getCountingCustomTableByDate(dbName, kullaniciid, sayimtarihi, mKod);
            return resp;
        }
        [WebMethod]
        public Response getToplamaListesi(string dbName, string baslangicTarihi, string bitisTarihi, string mKod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getToplamaListesi(dbName, baslangicTarihi, bitisTarihi, mKod);
            return resp;
        }
        [WebMethod]
        public Response getToplamaListesiDetay(string dbName, string docEntry, string mKod)
        {
            GetCekmeListesi l = new GetCekmeListesi();
            Response resp = l.getToplamaListesiDetay(dbName, docEntry, mKod);
            return resp;
        }
        [WebMethod]
        public Response getProductsMuhatapKatalogNo(string dbName, string barCode, string mKod)
        {
            GetProducts l = new GetProducts();
            Response resp = l.getProductsMuhatapKatalogNo(dbName, barCode, mKod);
            return resp;
        }
        [WebMethod]
        public Response getCekmeListesiKontrol(string dbName, string docEntry, string mKod)
        {
            GetCekmeListesiKontrol l = new GetCekmeListesiKontrol();
            Response resp = l.getCekmeListesiKontrol(dbName, docEntry, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetStokNakliFazlaMiktarKontrol(string dbName, List<string> docEntryList, List<string> WhsCodes, string mKod)
        {
            GetStokNakliFazlaMiktarKontrol l = new GetStokNakliFazlaMiktarKontrol();
            Response resp = l.getStokNakliFazlaMiktarKontrol(dbName, docEntryList, WhsCodes, mKod);
            return resp;
        }
        [WebMethod]
        public Response getPaletNoVarmi(string dbName, string paletNo, string mKod)
        {
            GetPaletYapmaListesi l = new GetPaletYapmaListesi();
            Response resp = l.getPaletNoVarmi(dbName, paletNo, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetSevkiyatAdresi(string dbName, string cardcode, string mKod)
        {
            GetSevkiyatAdresi l = new GetSevkiyatAdresi();
            Response resp = l.getSevkiyatAdresi(dbName, cardcode, mKod);
            return resp;
        }

        [WebMethod]
        public Response GetPartiliHareketRaporu(string dbName, string partiNo, string depoKodu, string kalemKodu, string mKod)
        {
            GetPartiliHareketRaporu l = new GetPartiliHareketRaporu();
            Response resp = l.getPartiliHareketRaporu(dbName, partiNo, depoKodu, kalemKodu, mKod);
            return resp;
        }


        [WebMethod]
        public Response GetBelgeBazliDepolar(string dbName, string kullaniciId, string belgeTipi, string mKod)
        {
            GetWareHouse l = new GetWareHouse();
            Response resp = l.getBelgeBazliDepolar(dbName, kullaniciId, belgeTipi, mKod);
            return resp;
        }
    }
}