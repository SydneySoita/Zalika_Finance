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
    [XafDisplayName("Payment Request")]
    [ImageName("BO_Requisition")]
    [NavigationItem("Payments")]
    public class glforequisition : BaseObject
    {
        private string requisitionNumber;
        private string accountPayableCode;
        private DateTime requisitionDate;
        private int accountMonth;
        private int accountYear;
        private string status;
        private string currencyCode;
        private Double currencyRate;
        private string payee;
        private Double grossAmount;
        private Double vatableAmount;
        private Double netAmount;
        private Double netLocalAmount;
        private string narration;

        public glforequisition(Session session) : base(session) { }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string RequisitionNumber
        {
            get => requisitionNumber;
            set => SetPropertyValue(nameof(RequisitionNumber), ref requisitionNumber, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AccountPayableCode
        {
            get => accountPayableCode;
            set => SetPropertyValue(nameof(AccountPayableCode), ref accountPayableCode, value);
        }

        public DateTime RequisitionDate
        {
            get => requisitionDate;
            set => SetPropertyValue(nameof(RequisitionDate), ref requisitionDate, value);
        }

        public int AccountMonth
        {
            get => accountMonth;
            set => SetPropertyValue(nameof(AccountMonth), ref accountMonth, value);
        }

        public int AccountYear
        {
            get => accountYear;
            set => SetPropertyValue(nameof(AccountYear), ref accountYear, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CurrencyCode
        {
            get => currencyCode;
            set => SetPropertyValue(nameof(CurrencyCode), ref currencyCode, value);
        }

        public Double CurrencyRate
        {
            get => currencyRate;
            set => SetPropertyValue(nameof(CurrencyRate), ref currencyRate, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Payee
        {
            get => payee;
            set => SetPropertyValue(nameof(Payee), ref payee, value);
        }

        public Double GrossAmount
        {
            get => grossAmount;
            set => SetPropertyValue(nameof(GrossAmount), ref grossAmount, value);
        }

        public Double VatableAmount
        {
            get => vatableAmount;
            set => SetPropertyValue(nameof(VatableAmount), ref vatableAmount, value);
        }

        public Double NetAmount
        {
            get => netAmount;
            set => SetPropertyValue(nameof(NetAmount), ref netAmount, value);
        }

        public Double NetLocalAmount
        {
            get => netLocalAmount;
            set => SetPropertyValue(nameof(NetLocalAmount), ref netLocalAmount, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Narration
        {
            get => narration;
            set => SetPropertyValue(nameof(Narration), ref narration, value);
        }

        private void GenerateRequisitionNumber()
        {
            const string RequisitionNumberFormat = "REQ{0}{1}{2:0000}";
            var lastRequisition = Session.Query<glforequisition>()?.OrderByDescending(i => i.RequisitionDate).FirstOrDefault();
            if (lastRequisition != null)
            {
                var year = lastRequisition.RequisitionDate.Year;
                var month = lastRequisition.RequisitionDate.Month;
                var sequence = int.Parse(lastRequisition.RequisitionNumber[7..]);
                sequence++;
                var newRequisitionNumber = string.Format(RequisitionNumberFormat, year, month, sequence);
                RequisitionNumber = newRequisitionNumber;
            }
            else
            {
                RequisitionNumber = string.Format(RequisitionNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            // Implementation for generating Requisition number
        }


        protected override void OnSaving()
        {
         
            if (Session.IsNewObject(this))
            {
                GenerateRequisitionNumber();
            }
            base.OnSaving();
        }
    }


}
