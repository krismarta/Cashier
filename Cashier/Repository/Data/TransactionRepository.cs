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
    public class TransactionRepository : GeneralRepository<MyContext, Transaction, string>
    {
        private readonly MyContext context;
        public TransactionRepository(MyContext myContext) : base(myContext)
        {
            context = myContext;
        }

        public ICollection getAlltransaction()
        {
            var query = context.Transactions.AsEnumerable();
            return query.ToList();
        }

        public int RequestTransaction(TransactionVM transactionVM)
        {
            DateTime localDate = DateTime.Now;
            var result = 0;
            var datenow = localDate;
            var statusTrs = "pending";
            try
            {
                if (transactionVM.payment.Equals("cash"))
                {
                    statusTrs = "settlement";
                }
                for (int i = 0; i < transactionVM.idGoods.Length; i++)
                {
                    //check dulu stoknya
                    var getstok = context.Goods.Where(g => g.id == transactionVM.idGoods[i]).FirstOrDefault();
                    if (Convert.ToInt32(getstok.stok) < Convert.ToInt32(transactionVM.quantity[i]))
                    {
                        result = 2;
                    }
                    else
                    {
                        result = 1;
                    }
                }

                if (result == 1)
                {
                    Transaction transaction = new Transaction()
                    {
                        id = transactionVM.id,
                        total = Convert.ToInt32(transactionVM.total),
                        payment_type = transactionVM.payment,
                        date_trs = datenow,
                        Userid = transactionVM.idUser,
                        status = statusTrs
                    };
                    context.Transactions.Add(transaction);
                    result = context.SaveChanges();

                    for (int i = 0; i < transactionVM.idGoods.Length; i++)
                    {
                        DetailTransaction detailTransaction = new DetailTransaction()
                        {
                            Transactionid = transactionVM.id,
                            Goodsid = transactionVM.idGoods[i],
                            quantity = transactionVM.quantity[i],

                        };
                        context.DetailTransactions.Add(detailTransaction);
                        result = context.SaveChanges();
                        var getstok = context.Goods.Where(g => g.id == transactionVM.idGoods[i]).FirstOrDefault();
                        var sisa = Convert.ToInt32(getstok.stok) - Convert.ToInt32(transactionVM.quantity[i]);
                        //pengurangan stok
                        var GoodsStok = context.Goods.Find(transactionVM.idGoods[i]);
                        GoodsStok.stok = sisa;
                        result = context.SaveChanges();
                    }
                    result = 1;
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
                return 0;
            }

            return result;
        }

        public int CallbackMidtrans(CallbackMidtrans callbackMidtrans)
        {
            var result = 0;
            var findOrderid = context.Transactions.Where(t => t.id == callbackMidtrans.order_id).FirstOrDefault();

            if (findOrderid != null)
            {
                if (callbackMidtrans.transaction_status == "settlement")
                {
                    findOrderid.status = "settlement";
                    context.SaveChanges();
                    result = 1;
                }
            }

            return result;

        }



    }
}
