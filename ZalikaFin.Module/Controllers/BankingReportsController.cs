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
using ZalikaFin.Module.BusinessObjects.Banking;

namespace ZalikaFin.Module.Controllers
{
    public class BankingReportsController : ObjectViewController<DetailView, glap>
    {
        public BankingReportsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction PreviewBanking = new SimpleAction(this,
                                                           "PreviewBankingAction",
                                                           "Reports");
            PreviewBanking.Caption = "Preview Banking";
            PreviewBanking.ImageName = "Business_Report";
            PreviewBanking.ToolTip = "Preview Banking";
            PreviewBanking.Execute += PreviewBanking_Execute;
        }

        private void PreviewBanking_Execute(object sender, SimpleActionExecuteEventArgs e)

        {
            IObjectSpace os = Application.CreateObjectSpace();
            Session _sess = ((XPObjectSpace)os).Session;

            glap req = (glap)View.CurrentObject;

            UnitOfWork uow = new UnitOfWork(_sess.DataLayer);
            glap FO = uow.FindObject<glap>(CriteriaOperator.Parse("Reference=?", req.Reference));

            if (FO != null)
            {

                if (FO != null)
                {
                    // Retrieve the report data object for the Banking report
                    var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
                    IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Banking");

                    // Check if the report data object is not null
                    if (reportData != null)
                    {
                        // Retrieve the handle of the report container
                        string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

                        // Show the preview of the report using the ReportServiceController
                        ReportServiceController controller = Frame.GetController<ReportServiceController>();
                        if (controller != null)
                        {
                            // Filter the report data by Banking id
                            CriteriaOperator criteria = CriteriaOperator.Parse("Reference=?", FO.Reference);
                            controller.ShowPreview(handle, criteria);
                        }
                    }
                }
                else
                {
                    // Banking object not found
                    // Handle the case when the Banking object is not found
                }
            }
        }
    }
}
