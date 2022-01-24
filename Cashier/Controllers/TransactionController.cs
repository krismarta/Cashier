using Cashier.Base;
using Cashier.Context;
using Cashier.Model;
using Cashier.Repository.Data;
using Cashier.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "HeadStore")]
    public class TransactionController : BaseController<Transaction, TransactionRepository, string>
    {
        private TransactionRepository transactionRepository;
        public IConfiguration _configuration;
        private readonly MyContext context;

        public TransactionController(MyContext myContext, TransactionRepository transactionRepository, IConfiguration configuration) : base(transactionRepository)
        {
            this.transactionRepository = transactionRepository;
            this._configuration = configuration;
            context = myContext;
        }

        [HttpGet("AllTransaction")]
        public ActionResult AllTransaction()
        {
            var result = transactionRepository.getAlltransaction();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound(new { status = HttpStatusCode.NotFound, result = result, message = $"Data tidak ada" });
        }

        [HttpPost("insertTransaction")]
        public ActionResult<TransactionVM> insertTransaction(TransactionVM transactionVM)
        {
            var result = transactionRepository.RequestTransaction(transactionVM);
            switch (result)
            {
                case 1:
                    return Ok(result);
                case 0:
                    return NotFound();
                case 2:
                    return Conflict();
                default:
                    return BadRequest();

            }
        }

        [HttpPost("MidtransPayment")]
        public ActionResult<MidtransVM> data(TransactionVM transactionVM)
        {
            var url = "https://app.sandbox.midtrans.com/snap/v1/transactions";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.Headers["Authorization"] = "Basic U0ItTWlkLXNlcnZlci1laURob0wzTFJvQUxZVVgzMnBHd1pYUEY6";
            httpRequest.ContentType = "application/json";
            var dataawal = "";
            var detailorder = "";
            for (int i = 0; i < transactionVM.idGoods.Length; i++)
            {
                detailorder += @"{""id"": ""_ITEMID_"",""price"": ""_PRICE_"",""quantity"": ""_QTY_"",""name"": ""_TITLE_""}"
                .Replace("_ITEMID_", transactionVM.idGoods[i])
                .Replace("_PRICE_", transactionVM.priceGoods[i])
                .Replace("_QTY_", transactionVM.quantity[i])
                .Replace("_TITLE_", transactionVM.namaGoods[i]);
                if (i+1 != transactionVM.idGoods.Length)
                {
                    detailorder += ",";
                }
            }
            dataawal = @"""order_id"": ""_ORDERID_"",""gross_amount"": ""_GROSS_"""
            .Replace("_ORDERID_", transactionVM.id)
            .Replace("_GROSS_", transactionVM.total);




            //var datas = @"{""transaction_details"": {" + dataawal + "},""item_details"": ""[" + detailorder + "]}";
            var test = @"{""transaction_details"" : {" + dataawal;
            var teslast = @"},""item_details"" : [" + detailorder;
            var last = @"]}";

            var gabung = test + teslast + last;
            
            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(gabung);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            MidtransVM mi = JsonConvert.DeserializeObject<MidtransVM>(result);

            Console.WriteLine(httpResponse.StatusCode);

            return mi;
        }

        [HttpGet("StatusPayment/{id}")]
        public ActionResult<StatusMidtransVM> StatusPayment(string id)
        {
            var url = "https://api.sandbox.midtrans.com/v2/"+id+"/status";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["Authorization"] = "Basic U0ItTWlkLXNlcnZlci1laURob0wzTFJvQUxZVVgzMnBHd1pYUEY6";


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var result = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
               result  = streamReader.ReadToEnd();
            }

            StatusMidtransVM mi = JsonConvert.DeserializeObject<StatusMidtransVM>(result);

            Console.WriteLine(httpResponse.StatusCode);

            return mi;
        }

        [HttpPost("Callbackmidtrans")]
        public IActionResult Callbackmidtrans(CallbackMidtrans callbackMidtrans)
        {
            var result = transactionRepository.CallbackMidtrans(callbackMidtrans);
            switch (result)
            {
                case 0:
                    return BadRequest();
                case 1:
                    return Ok();
                default:
                    return BadRequest();
            }

        }



    }
}
