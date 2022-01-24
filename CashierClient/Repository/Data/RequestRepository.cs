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
    public class RequestRepository : GeneralRepository<RequestGoods, string>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public RequestRepository(Address address, string request = "Request/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }

        public HttpStatusCode RequestGoodsSupplier(RequestGoodsVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            string myContent = content.ReadAsStringAsync().Result;
            var result = httpClient.PostAsync(address.Link + request + "RequestGoods", content).Result;
            return result.StatusCode;
        }

        public async Task<List<RequestGoods>> getAllRequest()
        {
            List<RequestGoods> entities = new List<RequestGoods>();

            using (var response = await httpClient.GetAsync(request + "AllRequest"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<RequestGoods>>(apiResponse);
            }
            return entities;
        }
        public HttpStatusCode UpdateStatusRequest(UpdateStatusVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            string myContent = content.ReadAsStringAsync().Result;
            var result = httpClient.PostAsync(address.Link + request + "UpdateStatusRequest", content).Result;
            return result.StatusCode;
        }


    }
}
