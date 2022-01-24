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
    public class TransactionRepository : GeneralRepository<Transaction, string>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public TransactionRepository(Address address, string request = "Transaction/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }

        public async Task<List<Transaction>> getAllTransaction()
        {
            List<Transaction> entities = new List<Transaction>();

            using (var response = await httpClient.GetAsync(request + "AllTransaction"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<Transaction>>(apiResponse);
            }
            return entities;
        }

        public HttpStatusCode TransactionInsert(TransactionVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            string myContent = content.ReadAsStringAsync().Result;
            var result = httpClient.PostAsync(address.Link + request + "insertTransaction", content).Result;
            return result.StatusCode;
        }

        public async Task<MidtransVM> PaymentWithMidtrans(TransactionVM entity)
        {
            MidtransVM entitys = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(address.Link + request + "MidtransPayment", content).Result;
            string apiResponse = await result.Content.ReadAsStringAsync();
            entitys = JsonConvert.DeserializeObject<MidtransVM>(apiResponse);
            return entitys;
        }

        public async Task<StatusMidtransVM> StatusMidtrans(string id)
        {

            StatusMidtransVM entitys = null;
            using (var response = await httpClient.GetAsync(request + "StatusPayment/" + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entitys = JsonConvert.DeserializeObject<StatusMidtransVM>(apiResponse);
            }

            //StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            //var result = httpClient.PostAsync(address.Link + request + "StatusPayment", content).Result;
            //string apiResponse = await result.Content.ReadAsStringAsync();
            //entitys = JsonConvert.DeserializeObject<MidtransVM>(apiResponse);
            return entitys;
        }


    }
}
