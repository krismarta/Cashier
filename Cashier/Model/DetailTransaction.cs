using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Model
{
    [Table("tb_t_detail_transaction")]
    public class DetailTransaction
    {
        public int id { get; set; }
        public string quantity { get; set; }
        public string Transactionid { get; set; }
        public string Goodsid { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual Goods Goods { get; set; }


    }
}
