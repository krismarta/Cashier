using Cashier.Context;
using Cashier.Model;
using Cashier.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cashier.Repository.Data
{
    public class GoodsRepository : GeneralRepository<MyContext, Goods, string>
    {
        private readonly MyContext context;
        public GoodsRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }
        public ICollection getAllGoods()
        {
            var query = context.Goods.AsEnumerable();
            return query.ToList();
        }

        public ICollection getGoodsBySupplier(string id_supplier)
        {
            var query = context.Goods.Where(g => g.Supplierid == id_supplier).AsEnumerable();
            return query.ToList();
                
        }
    }
}
