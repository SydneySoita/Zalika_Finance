using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalikaFin.Module.BusinessObjects.User;

namespace ZalikaFin.Module.BusinessObjects.Receipts
{
    [DefaultClassOptions]
    [XafDisplayName("Receipts")]
    [NavigationItem("Receipts")]
    [ImageName("BO_Invoice")]
    public class glreceipt : BaseObject
    {
        public glreceipt(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            //GenerateReceiptNo();
            // Place here your initialization code.
        }

        string reference;
        //payment_type paymentMode;
        DateTime transactionDate;
        string narration;
        string debitAccount;
        string payee;
        string chequeNo;
        DateTime chequeDate;
        string chequeBank;
        string mobileMoneyPhoneNo;
        string mpesaReference;
        Int32 idd;
        ZalikaUser client;


        /* //[Key(true)]
         [Key(true)]
         [VisibleInListView(false)]
         [VisibleInLookupListView(false)]
         [VisibleInDetailView(false)]
         public Int32 Idd
         {
             get => idd;
             set => SetPropertyValue(nameof(Idd), ref idd, value);
         }*/

        private double famount;
        [XafDisplayName("Amount")]
        [ImmediatePostData(true)]
        [ModelDefault("DisplayFormat", "#,##0.00")]
        [DbType("decimal(16,2)")]
        [RuleValueComparison("", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, "Amount is Mandatory, Please Specify !", TargetCriteria = "ReceiptType.isBulkReceipt Or ReceiptType.isPensionReceipt Or ReceiptType.isLoanReceipt Or ReceiptType.isInvoiceReceipt Or ReceiptType.isAssetReceipt Or ReceiptType.isInvestmentCoupon Or ReceiptType.isPopFund")]
        [VisibleInLookupListView(true)]
        public double amount
        {
            get
            {
                return famount;
            }
            set
            {
                SetPropertyValue<double>("amount", ref famount, value);
                if (!IsLoading && !IsDeleted)
                {
                    ToWords();
                }
            }
        }

        public string Reference
        {
            get => reference;
            set => SetPropertyValue(nameof(Reference), ref reference, value);
        }

        public ZalikaUser Client
        {
            get => client;
            set => SetPropertyValue(nameof(Client), ref client, value);
        }

        //public payment_type PaymentMode
        //{
        //    get => paymentMode;
        //    set => SetPropertyValue(nameof(PaymentMode), ref paymentMode, value);
        //}

        public DateTime TransactionDate
        {
            get => transactionDate;
            set => SetPropertyValue(nameof(TransactionDate), ref transactionDate, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Narration
        {
            get => narration;
            set => SetPropertyValue(nameof(Narration), ref narration, value);
        }

        public string DebitAccount
        {
            get => debitAccount;
            set => SetPropertyValue(nameof(DebitAccount), ref debitAccount, value);
        }

        public string Payee
        {
            get => payee;
            set => SetPropertyValue(nameof(Payee), ref payee, value);
        }

        public string ChequeNo
        {
            get => chequeNo;
            set => SetPropertyValue(nameof(ChequeNo), ref chequeNo, value);
        }

        public DateTime ChequeDate
        {
            get => chequeDate;
            set => SetPropertyValue(nameof(ChequeDate), ref chequeDate, value);
        }

        public string ChequeBank
        {
            get => chequeBank;
            set => SetPropertyValue(nameof(ChequeBank), ref chequeBank, value);
        }

        public string MobileMoneyPhoneNo
        {
            get => mobileMoneyPhoneNo;
            set => SetPropertyValue(nameof(MobileMoneyPhoneNo), ref mobileMoneyPhoneNo, value);
        }

        public string MpesaReference
        {
            get => mpesaReference;
            set => SetPropertyValue(nameof(MpesaReference), ref mpesaReference, value);
        }
        private string famountInWords;
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Amount in Words")]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string amountInWords
        {
            get
            {
                return famountInWords;
            }
            set
            {
                SetPropertyValue<string>("amountInWords", ref famountInWords, value);
            }
        }


        private void GenerateReceiptNo()
        {
            const string ReceiptNumberFormat = "REC{0}{1}{2:0000}";
            var lastReceipt = Session.Query<glreceipt>()?.OrderByDescending(i => i.TransactionDate).FirstOrDefault();
            if (lastReceipt != null)
            {
                var year = lastReceipt.TransactionDate.Year;
                var month = lastReceipt.TransactionDate.Month;
                var sequence = int.Parse(lastReceipt.Reference[7..]);
                sequence++;
                var newReceiptNumber = string.Format(ReceiptNumberFormat, year, month, sequence);
                Reference = newReceiptNumber;
            }
            else
            {
                Reference = string.Format(ReceiptNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
        }

        public void ToWords()
        {
            //amountInWords = stdClass.ConvertToWords((double)amount, currency_code.description, currency_code.singlecent, currency_code.manycents);
        }

        protected override void OnSaving()
        {
            base.OnSaved();
            GenerateReceiptNo();
        }
    }
}
