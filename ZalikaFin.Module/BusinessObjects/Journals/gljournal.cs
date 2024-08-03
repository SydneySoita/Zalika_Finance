using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalikaFin.Module.BusinessObjects.Payments;

namespace ZalikaFin.Module.BusinessObjects.Journals
{
    [DefaultClassOptions]
    [XafDisplayName("Journal")]
    [ImageName("BO_Journal")]
    [NavigationItem("Journals")]
    public class gljournal : BaseObject
    {
        private string journalNumber;
        private string journalType;
        private DateTime voucherDate;
        private string currencyCode;
        private Double currencyRate;
        private int numberOfEntries;
        private Double localAmount;
        private Double totalForeignAmount;
        private Double total;

        public gljournal(Session session) : base(session) { }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string JournalNumber
        {
            get => journalNumber;
            set => SetPropertyValue(nameof(JournalNumber), ref journalNumber, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string JournalType
        {
            get => journalType;
            set => SetPropertyValue(nameof(JournalType), ref journalType, value);
        }

        public DateTime VoucherDate
        {
            get => voucherDate;
            set => SetPropertyValue(nameof(VoucherDate), ref voucherDate, value);
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

        public int NumberOfEntries
        {
            get => numberOfEntries;
            set => SetPropertyValue(nameof(NumberOfEntries), ref numberOfEntries, value);
        }

        public Double LocalAmount
        {
            get => localAmount;
            set => SetPropertyValue(nameof(LocalAmount), ref localAmount, value);
        }

        public Double TotalForeignAmount
        {
            get => totalForeignAmount;
            set => SetPropertyValue(nameof(TotalForeignAmount), ref totalForeignAmount, value);
        }

        //public Double Total
        //{
        //    get => total;
        //    set => SetPropertyValue(nameof(Total), ref total, value);
        //}

        private void GenerateJournalNumber()
        {
            const string JournalNumberFormat = "JOU{0}{1}{2:0000}";
            var lastJournal = Session.Query<gljournal>()?.OrderByDescending(i => i.VoucherDate).FirstOrDefault();
            if (lastJournal != null)
            {
                var year = lastJournal.VoucherDate.Year;
                var month = lastJournal.VoucherDate.Month;
                var sequence = int.Parse(lastJournal.JournalNumber[7..]);
                sequence++;
                var newJournalNumber = string.Format(JournalNumberFormat, year, month, sequence);
                JournalNumber = newJournalNumber;
            }
            else
            {
                JournalNumber = string.Format(JournalNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
            // Implementation for generating Journal number
        }


        protected override void OnSaving()
        {

            if (Session.IsNewObject(this))
            {
                GenerateJournalNumber();
            }
            base.OnSaving();
        }
    }
}
