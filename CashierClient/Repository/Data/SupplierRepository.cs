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
    public class SupplierRepository : GeneralRepository<Supplier, string>
    {
        private readonly Address address;
        private readonly string request;
        //private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public SupplierRepository(Address address, string request = "suppliers/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            //_contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.Link)
            };
        }
        public async Task<List<Supplier>> getAllsupplier()
        {
            List<Supplier> entities = new List<Supplier>();

            using (var response = await httpClient.GetAsync(request + "AllSupplier"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse);
            }
            return entities;
        }


    }
}
