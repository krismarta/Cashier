using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Model
{
    [Table("tb_t_detail_request_goods")]
    public class DetailRequest
    {
        public int id { get; set; }
        public string quantity { get; set; }
        public string RequestGoodsid { get; set; }
        public string Goodsid { get; set; }
        public virtual RequestGoods RequestGoods { get; set; }
        public virtual Goods Goods { get; set; }
    }
}
