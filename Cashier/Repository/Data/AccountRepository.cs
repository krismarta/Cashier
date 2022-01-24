using Cashier.Context;
using Cashier.Model;
using Cashier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }

        public int Login(LoginVM registerVM)
        {
            try
            {
                var checkEmail = context.Users.Where(b => b.email == registerVM.email).FirstOrDefault();
                if (checkEmail != null)
                {
                    var password = (from e in context.Set<User>()
                                    where e.email == registerVM.email
                                    join a in context.Set<Account>() on e.id equals a.id
                                    select a.password).Single();


                    var checkPassword = Hashing.Hashing.ValidatePassword(registerVM.password, password);
                    //Password salah
                    if (checkPassword == false)
                    {
                        return 3;
                    }
                    //Login Berhasil
                    else
                    {
                        return 1;
                    }
                }
                //Email salah
                else
                {
                    return 2;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int ForgotPassword(LoginVM loginVM)
        {
            var result = 0;
            var checkmail = context.Users.Where(u => u.email == loginVM.email).FirstOrDefault();
            if (checkmail != null)
            {
                var idLogin = checkmail.id;
                var account = context.Accounts.Find(idLogin);
                string uniqueString = Guid.NewGuid().ToString().Substring(0, 12);
                StringBuilder sb = new StringBuilder();
                sb.Append($"<h1> {uniqueString} <h1>");

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(loginVM.email);
                mail.From = new MailAddress("cashierapp.system@gmail.com", "Cashier App", System.Text.Encoding.UTF8);
                DateTimeOffset now = (DateTimeOffset)DateTime.Now;
                mail.Subject = "Forgot Password " + now;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = "<table width='95%' cellspacing='0' cellpadding='0' align='center'> " +
                    "<tbody> <tr> <td align='center'> <table style='border-spacing: 2px 5px;' width='600' cellspacing='0' cellpadding='0' " +
                    "align='center' bgcolor='#fff'> <tbody> <tr> <td bgcolor='#fff'> <table width='100%' cellspacing='0' cellpadding='0'>" +
                    " <tbody> <tr> <td style='padding: 10px 0 10px 0; font-family: Nunito, sans-serif; font-size: 20px; font-weight: 900;'>" +
                    "New Password Your Account</td> </tr> </tbody> </table> </td> </tr> <tr> <td bgcolor='#fff'> <table style='height: 282px; " +
                    "width: 100%;' width='100%%' cellspacing='0' cellpadding='0'> <tbody> <tr style='height: 20px;'> <td style='padding: 20px" +
                    " 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 20px;'>Hi, <span id='name'>" + checkmail.name + " Kami " +
                    "baru saja menerima permintaan password untuk akun anda, kamu dapat melakukan login menggunakan password baru </span> " +
                    "</td> </tr> <tr style='height: 41px;'> " +
                    "<td style='padding: 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 41px;'>Password baru akun kamu : "
                    +sb.ToString() + "</td> </tr> <tr style='height: 73px;'> <td style='padding: 50px 0px; font-family: Nunito, sans-serif; " +
                    "font-size: 16px; height: 73px;'> System, <p>Cashier App</p> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table>" +
                    " </td> </tr> </tbody> </table>";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("cashierapp.system@gmail.com", "Cashier123456789");
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                    account.password = Hashing.Hashing.HashPassword(uniqueString);
                    context.SaveChanges();
                    result = 1;
                }
                catch (Exception)
                {

                    result = 2;
                }
            }
            else
            {
                result = 2;
            }
            return result;
        }
    }
}
