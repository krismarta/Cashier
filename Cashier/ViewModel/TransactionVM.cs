using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.ViewModel
{
    public class TransactionVM
    {
        public string id { get; set; }
        public string total { get; set; }
        public string idUser { get; set; }
        public string payment { get; set; }

        public string[] idGoods { get; set; }
        public string[] quantity { get; set; }
        public string[] namaGoods { get; set; }
        public string[] priceGoods { get; set; }
       
    }
}
