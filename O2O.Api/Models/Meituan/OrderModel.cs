using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O2O.Api.Models.Meituan
{
    public class OrderModel
    {
        public string id { get; set; }

        public string order_id { get; set; }
        public string wm_order_id_view { get; set; }
        public string app_poi_code { get; set; }
        public string wm_poi_name { get; set; }
        public string wm_poi_address { get; set; }
        public string wm_poi_phone { get; set; }
        public string recipient_address { get; set; }
        public string recipient_phone { get; set; }
        public string backup_recipient_phone { get; set; }
        public string recipient_name { get; set; }
        public double shipping_fee { get; set; }
        public double total { get; set; }
        public double original_price { get; set; }
        public string caution { get; set; }
        public string shipper_phone { get; set; }
        public int status { get; set; }
        public long city_id { get; set; }
        public int has_invoiced { get; set; }
        public string invoice_title { get; set; }
        public string taxpayer_id { get; set; }
        public long ctime { get; set; }
        public long utime { get; set; }
        public long delivery_time { get; set; }
        public int is_third_shipping { get; set; }
        public int pay_type { get; set; }
        public int pick_type { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int day_seq { get; set; }
        public bool is_favorites { get; set; }
        public bool is_poi_first_order { get; set; }
        public int dinners_number { get; set; }
        public string logistics_code { get; set; }
        public double avg_send_time { get; set; }
        public string channel { get; set; }
        public int invMakeType { get; set; }
        public int is_saled { get; set; }
        public string detail { get; set; }
    }
}
