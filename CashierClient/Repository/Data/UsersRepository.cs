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
    public class UserRepository : GeneralRepository<User, string>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public UserRepository(Address address, string request = "users/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }

        public HttpStatusCode RegisterPost(NewAccountVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            string myContent = content.ReadAsStringAsync().Result;
            var result = httpClient.PostAsync(address.Link + request + "newAccount", content).Result;
            return result.StatusCode;
        }

        public async Task<List<User>> GetAllCashier()
        {
            List<User> entities = new List<User>();

            using (var response = await httpClient.GetAsync(request + "AllCashier"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            }
            return entities;
        }


        public HttpStatusCode changePassword(ChangePasswordVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            string myContent = content.ReadAsStringAsync().Result;
            var result = httpClient.PostAsync(address.Link + request + "changepassword", content).Result;
            return result.StatusCode;
        }

        public async Task<CounterVM> counterdashboardAsync()
        {
            CounterVM entities = new CounterVM();
            using (var response = await httpClient.GetAsync(request + "Counter"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<CounterVM>(apiResponse);
            }
            return entities;
        }

        public async Task<GraphVM> GraphUser()
        {
            GraphVM entities = new GraphVM();

            using (var response = await httpClient.GetAsync(request + "graph1"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                //entities = apiResponse;
                entities = JsonConvert.DeserializeObject<GraphVM>(apiResponse);
            }
            return entities;
        }
        public async Task<GraphVM> GraphUser2()
        {
            GraphVM entities = new GraphVM();

            using (var response = await httpClient.GetAsync(request + "graph2"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                //entities = apiResponse;
                entities = JsonConvert.DeserializeObject<GraphVM>(apiResponse);
            }
            return entities;
        }
        public async Task<GraphVM> GraphUser3()
        {
            GraphVM entities = new GraphVM();

            using (var response = await httpClient.GetAsync(request + "Graph3"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                //entities = apiResponse;
                entities = JsonConvert.DeserializeObject<GraphVM>(apiResponse);
            }
            return entities;
        }
    }
    
}
