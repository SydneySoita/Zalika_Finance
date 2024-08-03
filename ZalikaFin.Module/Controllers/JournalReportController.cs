using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZalikaFin.Module.BusinessObjects.Journals;


namespace ZalikaFin.Module.Controllers
{
    public class JournalReportsController : ObjectViewController<DetailView, gljournal>
    {
        public JournalReportsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction PreviewJournal = new SimpleAction(this,
                                                           "PreviewJournalAction",
                                                           "Reports");
            PreviewJournal.Caption = "Preview Journal";
            PreviewJournal.ImageName = "Business_Report";
            PreviewJournal.ToolTip = "Preview Journal";
            PreviewJournal.Execute += PreviewJournal_Execute;
        }

        private void PreviewJournal_Execute(object sender, SimpleActionExecuteEventArgs e)

        {
            IObjectSpace os = Application.CreateObjectSpace();
            Session _sess = ((XPObjectSpace)os).Session;

            gljournal req = (gljournal)View.CurrentObject;

            UnitOfWork uow = new UnitOfWork(_sess.DataLayer);
            gljournal FO = uow.FindObject<gljournal>(CriteriaOperator.Parse("JournalNumber=?", req.JournalNumber));

            if (FO != null)
            {

                if (FO != null)
                {
                    // Retrieve the report data object for the Journal report
                    var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
                    IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Journal");

                    // Check if the report data object is not null
                    if (reportData != null)
                    {
                        // Retrieve the handle of the report container
                        string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

                        // Show the preview of the report using the ReportServiceController
                        ReportServiceController controller = Frame.GetController<ReportServiceController>();
                        if (controller != null)
                        {
                            // Filter the report data by Journal id
                            CriteriaOperator criteria = CriteriaOperator.Parse("JournalNumber=?", FO.JournalNumber);
                            controller.ShowPreview(handle, criteria);
                        }
                    }
                }
                else
                {
                    // Journal object not found
                    // Handle the case when the Journal object is not found
                }
            }
        }
    }
}
