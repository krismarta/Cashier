using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Model
{
    [Table("tb_m_user")]
    public class User
    {
        [Key]
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }

       
        public virtual Account Account { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transaction> Transactions { get; set; }
        [JsonIgnore]
        public virtual ICollection<RequestGoods> RequestGoods { get; set; }
    }
}
