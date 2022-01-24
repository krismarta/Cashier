using Cashier.Context;
using Cashier.Model;
using Cashier.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Cashier.Repository.Data
{
    public class RequestRepository : GeneralRepository<MyContext, RequestGoods, string>
    {
        private readonly MyContext context;
        public RequestRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }

        public ICollection getAllRequest()
        {
            var query = context.RequestGoods.AsEnumerable();
            return query.ToList();
        }

        public int RequestGoods(RequestGoodsVM requestGoodsVM)
        {
            DateTime localDate = DateTime.Now;
            var result = 0;
            var datenow = localDate;

            try
            {
                RequestGoods requestGoods = new RequestGoods()
                {
                    id = requestGoodsVM.id,
                    date_trs = datenow,
                    total = Convert.ToInt32(requestGoodsVM.subtotal),
                    status = "pending",
                    Userid = requestGoodsVM.iduser,
                    Supplierid = requestGoodsVM.idsupplier


                };
                context.RequestGoods.Add(requestGoods);
                result = context.SaveChanges();
                //input detail

                for (int i = 0; i < requestGoodsVM.idbarang.Length; i++)
                {
                    DetailRequest detailRequest = new DetailRequest()
                    {
                        RequestGoodsid = requestGoodsVM.id,
                        quantity = requestGoodsVM.quantity[i],
                        Goodsid = requestGoodsVM.idbarang[i],
                    };
                    context.DetailRequests.Add(detailRequest);
                    result = context.SaveChanges();
                }

                //cariemailSupplier untuk dikirim email
                

                if (result == 1)
                {
                    var findMailSup = context.Suppliers.Where(c => c.id == requestGoodsVM.idsupplier).FirstOrDefault();
                    var data = "";
                    for (int i = 0; i < requestGoodsVM.idbarang.Length; i++)
                    {
                        var caridata = context.Goods.Where(g => g.id == requestGoodsVM.idbarang[i]).FirstOrDefault();
                        data  += "<tr>" +
                                "<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>"+ requestGoodsVM.idbarang[i]+ "</td> " +
                                "<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>"+ caridata.name +"</td> " +
                                "<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>"+ requestGoodsVM.quantity[i] +" </td>" +
                                "</tr>";
                    }

                    var bodyMail =
                       "<table width='95%' cellspacing='0' cellpadding='0' align='center'> <tbody> <tr> " +
                       "<td align='center'> <table style='border-spacing: 2px 5px;' width='600' cellspacing='0' cellpadding='0' align='center' bgcolor='#fff'> " +
                       "<tbody> <tr> <td bgcolor='#fff'> <table width='100%' cellspacing='0' cellpadding='0'> <tbody> <tr> " +
                       "<td style='padding: 10px 0 10px 0; font-family: Nunito, sans-serif; font-size: 20px; font-weight: 900;'>" +
                       "New Request Stok List From Cashier App</td> </tr> </tbody> </table> </td> </tr> <tr> <td bgcolor='#fff'>" +
                       " <table style='height: 282px; width: 100%;' width='100%%' cellspacing='0' cellpadding='0'> <tbody> " +
                       "<tr style='height: 20px;'> <td style='padding: 20px 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 20px;'>" +
                       "Hi, <span id='name'>" + findMailSup.name + " We request to " + findMailSup.companyName + 
                       " send the goods according to our request. so that we can ensure that the stock in our store is sufficient. </span > " +
                       "</td> </tr> <tr style = 'height: 41px;'> <td style ='padding: 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 41px;'>" +
                       " Here we attach our stock requirements.</td> </tr> <tr style = 'height: 54px;'> " +
                       "<td style = 'padding: 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 41px;'> " +
                       "<table style = 'font-family: arial, sans-serif;border-collapse: collapse;width: 100%;' > <tr> " +
                       "<th style = 'border: 1px solid #dddddd;text-align: left;padding: 8px;'> ID Goods </th>" +
                       " <th style = 'border: 1px solid #dddddd;text-align: left;padding: 8px;' > Name Goods </th > " +
                       "<th style = 'border: 1px solid #dddddd;text-align: left;padding: 8px;' > Request Stock </th > <tr>" +
                       data +
                        "</table></td></tr><tr style = 'height: 94px;' > <td style = 'padding: 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 94px;' > We hope that our orders will be processed as soon as needed" +
                        "</td > </tr> <tr style = 'height: 73px;' > " +
                        "<td style = 'padding: 50px 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 73px;' > " +
                        "Head Store, <p> Cashier App </p> </td > </tr > </tbody > </table > </td > </tr > </tbody> " +
                        "</table > </td > </tr > </tbody > </table > ";

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add(findMailSup.email);
                    //mail.To.Add("kristianto.kt@gmail.com");
                    mail.From = new MailAddress("cashierapp.system@gmail.com", "Cashier App", System.Text.Encoding.UTF8);
                    DateTimeOffset now = (DateTimeOffset)DateTime.Now;
                    mail.Subject = "Request Stok Cashier App " + now;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = bodyMail;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential("cashierapp.system@gmail.com", "Cashier123456789");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Send(mail);
                    return 1;
                }



            }
            catch (Exception)
            {
                return 0;
            }

            return result;
        }

        public int UpdateStatusRequest(UpdateStatusVM updateStatusVM)
        {
            var result = 0;
            var checkreq = context.RequestGoods.Where(r => r.id == updateStatusVM.id).FirstOrDefault();
            if (checkreq != null)
            {
                if (checkreq.status == "pending")
                {
                    if (updateStatusVM.status == "success")
                    {
                        //stok tambahin & ganti status
                        var checkdetail = context.DetailRequests.Where(dt => dt.RequestGoodsid == checkreq.id).ToList();

                        for (int i = 0; i < checkdetail.Count; i++)
                        {
                            var reqstok = checkdetail[i].quantity;
                            //cari id goods
                            var getGoods = context.Goods.Where(g => g.id == checkdetail[i].Goodsid).FirstOrDefault();
                            getGoods.stok = Convert.ToInt32(getGoods.stok) + Convert.ToInt32(reqstok);
                            context.SaveChanges();
                            result = 1;
                        }
                        checkreq.status = "success";
                        context.SaveChanges();
                        result = 1;
                    }
                    else
                    {
                        //hanya ganti status
                        checkreq.status = updateStatusVM.status;
                        context.SaveChanges();
                        result = 1;
                    }
                }
                else
                {
                    result = 2;
                }
                
            }
            return result;
        }


      
    }
}
