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
    public class GoodsRepository : GeneralRepository<Goods, string>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public GoodsRepository(Address address, string request = "Goods/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }
        public async Task<List<Goods>> getAllGoods()
        {
            List<Goods> entities = new List<Goods>();

            using (var response = await httpClient.GetAsync(request + "allgoods"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<Goods>>(apiResponse);
            }
            return entities;
        }

        public async Task<List<Goods>> GetGoodsbySupplier(string id)
        {
            List<Goods> entities = new List<Goods>();
            using (var response = await httpClient.GetAsync(request + "getbysupplier/" + id))
            {

                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<Goods>>(apiResponse);

            }
            return entities;
        }

    }
}
