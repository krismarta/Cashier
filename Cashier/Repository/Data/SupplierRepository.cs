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
    public class SupplierRepository : GeneralRepository<MyContext, Supplier, string>
    {
        private readonly MyContext context;
        public SupplierRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }
        public ICollection getAllSupplier()
        {

            var query = context.Suppliers.AsEnumerable();
            return query.ToList();
        }
    }
}
