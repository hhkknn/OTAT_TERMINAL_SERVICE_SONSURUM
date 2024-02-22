using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIF.TerminalService.Models
{
    public class PaletYapma
    {
        public string PaletNumarasi { get; set; }
        public string Durum { get; set; }
        public double ToplamKap { get; set; }
        public double NetKilo { get; set; }
        public double BrutKilo { get; set; }
        public List<PaletYapmaDetay> paletYapmaDetays { get; set; }

    }
    public class PaletYapmaDetay
    {
        public int DocEntry { get; set; }

        public string Barkod { get; set; }

        public string MuhatapKatalogNo { get; set; }

        public string KalemKodu { get; set; }

        public string KalemTanimi { get; set; }

        public int SiparisNumarasi { get; set; }

        public int SiparisSatirNo { get; set; }

        public double Quantity { get; set; }

        public int CekmeNo { get; set; }

        public string Kaynak { get; set; }

    }
}
