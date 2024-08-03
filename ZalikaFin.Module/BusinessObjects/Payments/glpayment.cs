using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalikaFin.Module.BusinessObjects.Payments
{
    [DefaultClassOptions]
    [XafDisplayName("Payment Voucher")]
    [ImageName("BO_Requisition")]
    [NavigationItem("Payments")]
    public class glpayment : BaseObject
    {
        private string reference;
        private string status;
        private DateTime transactionDate;
        private int accountYear;
        private int accountMonth;
        private string currencyCode;
        private decimal currencyRate;
        private string bankAccountToCredit;
        private string paymentMode;
        private string payee;
        private decimal foreignAmount;
        private decimal localAmount;
        private string narration;

        public glpayment(Session session) : base(session) { }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Reference
        {
            get => reference;
            set => SetPropertyValue(nameof(Reference), ref reference, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        public DateTime TransactionDate
        {
            get => transactionDate;
            set => SetPropertyValue(nameof(TransactionDate), ref transactionDate, value);
        }

        public int AccountYear
        {
            get => accountYear;
            set => SetPropertyValue(nameof(AccountYear), ref accountYear, value);
        }

        public int AccountMonth
        {
            get => accountMonth;
            set => SetPropertyValue(nameof(AccountMonth), ref accountMonth, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CurrencyCode
        {
            get => currencyCode;
            set => SetPropertyValue(nameof(CurrencyCode), ref currencyCode, value);
        }

        public decimal CurrencyRate
        {
            get => currencyRate;
            set => SetPropertyValue(nameof(CurrencyRate), ref currencyRate, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string BankAccountToCredit
        {
            get => bankAccountToCredit;
            set => SetPropertyValue(nameof(BankAccountToCredit), ref bankAccountToCredit, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string PaymentMode
        {
            get => paymentMode;
            set => SetPropertyValue(nameof(PaymentMode), ref paymentMode, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Payee
        {
            get => payee;
            set => SetPropertyValue(nameof(Payee), ref payee, value);
        }

        public decimal ForeignAmount
        {
            get => foreignAmount;
            set => SetPropertyValue(nameof(ForeignAmount), ref foreignAmount, value);
        }

        public decimal LocalAmount
        {
            get => localAmount;
            set => SetPropertyValue(nameof(LocalAmount), ref localAmount, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Narration
        {
            get => narration;
            set => SetPropertyValue(nameof(Narration), ref narration, value);
        }

        private void GeneratePaymentNumber()
        {
            const string PaymentNumberFormat = "PAY{0}{1}{2:0000}";
            var lastPayment = Session.Query<glpayment>()?.OrderByDescending(i => i.Reference).FirstOrDefault();
            if (lastPayment != null)
            {
                var year = lastPayment.TransactionDate.Year;
                var month = lastPayment.TransactionDate.Month;
                var sequence = int.Parse(lastPayment.Reference[7..]);
                sequence++;
                var newPaymentNumber = string.Format(PaymentNumberFormat, year, month, sequence);
                Reference = newPaymentNumber;
            }
            else
            {
                Reference = string.Format(PaymentNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            // Implementation for generating Payment number
        }


        protected override void OnSaving()
        {

            if (Session.IsNewObject(this))
            {
                GeneratePaymentNumber();
            }
            base.OnSaving();
        }
    }
}
