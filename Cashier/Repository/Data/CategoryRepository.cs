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
    public class CategoryRepository : GeneralRepository<MyContext, Category, int>
    {
        private readonly MyContext context;
        public CategoryRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }
        public ICollection getAllSCategory()
        {

            var query = context.Categories.AsEnumerable();
            return query.ToList();
        }
    }
}
