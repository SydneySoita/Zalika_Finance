using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalikaFin.Module.BusinessObjects.Invoice
{
    [DefaultClassOptions]
    [XafDisplayName("Invoices")]
    [ImageName("BO_Invoice")]
    [NavigationItem("Invoices")]
    public class glinvoice : BaseObject
    {
        public glinvoice(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here
            GenerateInvoiceNumber();
            InvoiceDate = DateTime.Now;
        }

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value); }
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value); }
        }

    

        public double AmountSettled
        {
            get => amountSettled;
            set => SetPropertyValue(nameof(AmountSettled), ref amountSettled, value);
        }

        public double Balance
        {
            get => balance;
            set => SetPropertyValue(nameof(Balance), ref balance, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Narration
        {
            get => narration;
            set => SetPropertyValue(nameof(Narration), ref narration, value);
        }

        public double Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }

        //public bool TaxExempt { get => taxExempt; set => SetPropertyValue(nameof(TaxExempt), ref taxExempt, value); }

        /*[VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]*/
        /*public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }*/

        private void GenerateInvoiceNumber()
        {
            const string InvoiceNumberFormat = "INV{0}{1}{2:0000}";
            var lastInvoice = Session.Query<glinvoice>()?.OrderByDescending(i => i.InvoiceDate).FirstOrDefault();
            if (lastInvoice != null)
            {
                var year = lastInvoice.InvoiceDate.Year;
                var month = lastInvoice.InvoiceDate.Month;
                var sequence = int.Parse(lastInvoice.InvoiceNumber[7..]);
                sequence++;
                var newInvoiceNumber = string.Format(InvoiceNumberFormat, year, month, sequence);
                InvoiceNumber = newInvoiceNumber;
            }
            else
            {
                InvoiceNumber = string.Format(InvoiceNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            // Implementation for generating invoice number
        }

        protected override void OnSaving()
        {

            if (Session.IsNewObject(this))
            {
                GenerateInvoiceNumber();
            }
            base.OnSaving();
            // Implementation for OnSaving
        }

        private string invoiceNumber;
        private DateTime invoiceDate;
        private DateTime invoiceDueDate;
        private double amountSettled;
        private double balance;
        private string narration;
        private double amount;
        private bool taxExempt;
    }
}