using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.ViewModel
{
    public class RequestGoodsVM
    {
        public string id { get; set; }
        public string idsupplier { get; set; }
        public string iduser { get; set; }
        public string subtotal { get; set; }

        public string[] idbarang { get; set; }
        public string[] quantity { get; set; }
       
    }
}
