using Cashier.Model;
using Cashier.ViewModel;
using CashierClient.Base.Urls;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashierClient.Repository.Data
{
    public class CategoryRepository : GeneralRepository<Category, int>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public CategoryRepository(Address address, string request = "category/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }
        public async Task<List<Category>> getAllCategory()
        {
            List<Category> entities = new List<Category>();

            using (var response = await httpClient.GetAsync(request + "allcategory"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<Category>>(apiResponse);
            }
            return entities;
        }


    }
}
