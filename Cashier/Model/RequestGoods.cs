using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Model
{
    [Table("tb_t_request_goods")]
    public class RequestGoods
    {
        public string id { get; set; }
        public DateTime date_trs { get; set; }
        public int total { get; set; }
        public string status { get; set; }
        public string Userid { get; set; }
        public string Supplierid { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<DetailRequest> DetailRequests { get; set; }

    }
}
