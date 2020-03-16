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
        public int payment_time { get; set; }
        public string payment_descriptions { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string payment_method { get; set; }
        public string username { get; set; }
        public string counter_offer_type { get; set; }
        public bool disabled { get; set; }
        public double price { get; set; }
        public object max_coin_price { get; set; }
        public double? min_coin_price { get; set; }
        public string reference_exchange { get; set; }
        public string bank_name { get; set; }
        public DateTime? last_online_all { get; set; }
        public string last_online_type { get; set; }
        public object buyer_trust_score { get; set; }
        public double seller_speed_score { get; set; }
        public int seller_released_trades_count { get; set; }
        public bool require_verified_buyer { get; set; }
        public bool site_country_strict { get; set; }
        public List<string> taker_warnings { get; set; }
        public object aml_adjustment { get; set; }
        public bool has_fiat_balance { get; set; }
        public string coin_currency { get; set; }
        public bool scheduled { get; set; }
        public object on1 { get; set; }
        public object off1 { get; set; }
        public object on2 { get; set; }
        public object off2 { get; set; }
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
        public int page { get; set; } = 1;
        public string coin_currency { get; set; } = "btc";

        public RequestOffers(){}
        public RequestOffers(string coin)
        {
            this.coin = coin;
            this.coin_currency = coin;
        }
    }
}
