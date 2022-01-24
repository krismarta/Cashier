using Cashier.Context;
using Cashier.Model;
using Cashier.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Repository.Data
{
    public class UserRepository : GeneralRepository<MyContext, User, string>
    {
        private readonly MyContext context;
        public UserRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }

        public int NewCashier(NewAccountVM newAccountVM)
        {
            Random random = new Random();
            try
            {
                var result = 0;
                string id_builder = "C" + random.Next(9999999);
                string passwordnew = Guid.NewGuid().ToString().Substring(0, 12);

                User user = new User()
                {
                    id = id_builder,
                    name = newAccountVM.name,
                    email = newAccountVM.email,
                    phone = newAccountVM.phone,
                };
                var checkMail = context.Users.Where(c => c.email == newAccountVM.email).FirstOrDefault();
                if (checkMail != null)
                {
                    return 2;
                }
                context.Users.Add(user);
                result = context.SaveChanges();

                Account account = new Account()
                {
                    id = id_builder,
                    password = Hashing.Hashing.HashPassword(passwordnew),
                    Roleid = 2,
                };
                context.Accounts.Add(account);
                result = context.SaveChanges();

                if (result == 1)
                {
                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add(newAccountVM.email);
                    mail.From = new MailAddress("cashierapp.system@gmail.com", "Cashier App", System.Text.Encoding.UTF8);
                    DateTimeOffset now = (DateTimeOffset)DateTime.Now;
                    mail.Subject = "New Account Cashier Login " + now;
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = "<table width='95%' cellspacing='0' cellpadding='0' align='center'><tbody><td bgcolor='#fff'>" +
                        "<table width='100%%' cellspacing='0' cellpadding='0'><tbody><tr>" +
                        "<td style='padding: 10px 0 10px 0; font-family: Nunito, sans serif; font-weight: 900;'>" +
                        "Akun untuk kamu berhasil dibuat oleh Headstore</td></tr></tbody></table</td></tr><tr>" +
                        "<td bgcolor='#fff'><table style='height: 282px; width: 100%;' width='100%%' cellspacing='0' cellpadding='0'>" +
                        "<tbody><tr style='height: 20px;'><td style='padding: 20px 0px; font-family: Nunito, sans-serif; font-size: 16px;'>Hi, <span id='name'>" +
                        "' "+ newAccountVM.name + "'</span></td></tr><tr style='height: 41px;'>" +
                        "<td style='padding: 0px; font-family: Nunito, sans-serif; font-size: 16px; height: 41px;'>Password untuk akun kamu adalah : " + passwordnew +
                        " </td></tr><tr style='height: 94px;'><td style='padding: 0px; font-family: Nunito, sans-serif; height: 94px;'>" +
                        "Pastikan setelah kamu berhasil login untuk mengganti kata sandi agar akun kamu menjadi lebih aman</td></tr>" +
                        "<tr style='height: 73px;'><td style='padding: 50px 0px; font-family: Nunito, sans-serif; font-size: 16px; height: '" +
                        "' 73px;'>System,<p>Cashier App</p></td></tr></tbody</table></td></tr></tbody></table></td></tr></tbody></table><p>'</p>";
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
                return 0;
            }
            catch(Exception)
            {
                return 0;
            }
        }
        public ICollection getAllCashier()
        {

            var query = context.Users.Where(c => c.Account.Roleid == 2).AsEnumerable();
            return query.ToList();
        }

        public int changepassword(ChangePasswordVM changePasswordVM)
        {
            var result = 0;
            var checkEmail = context.Users.Where(b => b.email == changePasswordVM.Email).FirstOrDefault();
            //Tidak ada email
            if (checkEmail == null)
            {
                return 2;
            }
            else
            {
                var account = context.Accounts.Find(checkEmail.id);
                account.password = Hashing.Hashing.HashPassword(changePasswordVM.NewPassword);
                result = context.SaveChanges();
                result = 1;
                return result;
            }
        }

        public CounterVM CounterDashboard()
        {
            CounterVM counterVM = new CounterVM();
            counterVM.cashier = context.Accounts.Where(u => u.Roleid == 2).Count();
            counterVM.supplier = context.Suppliers.Count();
            counterVM.items = context.Goods.Count();
            return counterVM;
        }

    }
}
