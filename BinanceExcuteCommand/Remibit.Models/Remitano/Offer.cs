using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remibit.Models.Remitano
{
    public class Offer
    {
        public int id { get; set; }
        public string canonical_name { get; set; }
        public string offer_type { get; set; }
        public double min_amount { get; set; }
        public double max_amount { get; set; }
        public string currency { get; set; }
        public bool disabled { get; set; }
        public double price { get; set; }
        public object max_coin_price { get; set; }
        public double? min_coin_price { get; set; }
    }

    public class Meta
    {
        public int per_page { get; set; }
        public int current_page { get; set; }
        public int next_page { get; set; }
        public object prev_page { get; set; }
        public int total_pages { get; set; }
        public int total_items { get; set; }
    }


    public class Offers
    {
        public List<Offer> offers { get; set; }
        public Meta meta { get; set; }
    }

    public class RequestOffers
    {
        public string offer_type { get; set; } = "buy";    // buy or sell
        public string country_code { get; set; } = "vn"; // vn
        public string coin { get; set; } = "btc";         // eth, btc
        public bool offline { get; set; } = false ;
        public int page { get; set; }
        public string coin_currency { get; set; } = "btc";

        public RequestOffers(){}
        public RequestOffers(string coin)
        {
            this.coin = coin;
            this.coin_currency = coin;
        }
    }
}
