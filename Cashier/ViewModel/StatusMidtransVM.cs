using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.ViewModel
{
    public class StatusMidtransVM
    {
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_status { get; set; }
    }
}
