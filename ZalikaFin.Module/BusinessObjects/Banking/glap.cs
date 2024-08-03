using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalikaFin.Module.BusinessObjects.Invoice;

namespace ZalikaFin.Module.BusinessObjects.Banking
{
    [DefaultClassOptions]
    [XafDisplayName("Banking")]
    [ImageName("BO_Requisition")]
    [NavigationItem("Banking")]
    public class glap : BaseObject
    {
        private string reference;
        private string transType;
        private string transactionAccount;
        private string glBankAccount;
        private string drCr;
        private DateTime transDate;
        private int accountYear;
        private int accountMonth;
        private string currencyCode;
        private decimal currencyRate;
        private decimal localAmount;
        private decimal foreignAmount;
        private string narration;

        public glap(Session session) : base(session) { }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Reference
        {
            get => reference;
            set => SetPropertyValue(nameof(Reference), ref reference, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TransType
        {
            get => transType;
            set => SetPropertyValue(nameof(TransType), ref transType, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TransactionAccount
        {
            get => transactionAccount;
            set => SetPropertyValue(nameof(TransactionAccount), ref transactionAccount, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string GLBankAccount
        {
            get => glBankAccount;
            set => SetPropertyValue(nameof(GLBankAccount), ref glBankAccount, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DrCr
        {
            get => drCr;
            set => SetPropertyValue(nameof(DrCr), ref drCr, value);
        }

        public DateTime TransDate
        {
            get => transDate;
            set => SetPropertyValue(nameof(TransDate), ref transDate, value);
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

        public decimal LocalAmount
        {
            get => localAmount;
            set => SetPropertyValue(nameof(LocalAmount), ref localAmount, value);
        }

        public decimal ForeignAmount
        {
            get => foreignAmount;
            set => SetPropertyValue(nameof(ForeignAmount), ref foreignAmount, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Narration
        {
            get => narration;
            set => SetPropertyValue(nameof(Narration), ref narration, value);
        }


        private void GenerateBankingNumber()
        {
            const string BankingNumberFormat = "BNK{0}{1}{2:0000}";
            var lastBanking = Session.Query<glap>()?.OrderByDescending(i => i.transDate).FirstOrDefault();
            if (lastBanking != null)
            {
                var year = lastBanking.transDate.Year;
                var month = lastBanking.transDate.Month;
                var sequence = int.Parse(lastBanking.Reference[7..]);
                sequence++;
                var newBankingNumber = string.Format(BankingNumberFormat, year, month, sequence);
                Reference = newBankingNumber;
            }
            else
            {
                Reference = string.Format(BankingNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            // Implementation for generating Banking number
        }
    }
}
