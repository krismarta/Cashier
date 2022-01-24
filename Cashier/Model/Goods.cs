using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Model
{
    [Table("tb_t_goods")]
    public class Goods
    {
        public string id { get; set; }
        public string name { get; set; }
        public int priceSell { get; set; }
        public int priceBuy { get; set; }
        public int stok { get; set; }
        public string Supplierid { get; set; }
        public int Categoryid { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<DetailTransaction> DetailTransactions { get; set; }
        public virtual ICollection<DetailRequest> DetailRequests { get; set; }
    }
}
