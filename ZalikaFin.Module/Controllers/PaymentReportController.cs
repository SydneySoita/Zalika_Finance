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
using ZalikaFin.Module.BusinessObjects.Payments;

namespace ZalikaFin.Module.Controllers
{
    public class PaymentReportsController : ObjectViewController<DetailView, glpayment>
    {
        public PaymentReportsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction PreviewPayment = new SimpleAction(this,
                                                           "PreviewPaymentAction",
                                                           "Reports");
            PreviewPayment.Caption = "Preview Payment";
            PreviewPayment.ImageName = "Business_Report";
            PreviewPayment.ToolTip = "Preview Payment";
            PreviewPayment.Execute += PreviewPayment_Execute;
        }

        private void PreviewPayment_Execute(object sender, SimpleActionExecuteEventArgs e)

        {
            IObjectSpace os = Application.CreateObjectSpace();
            Session _sess = ((XPObjectSpace)os).Session;

            glpayment req = (glpayment)View.CurrentObject;

            UnitOfWork uow = new UnitOfWork(_sess.DataLayer);
            glpayment FO = uow.FindObject<glpayment>(CriteriaOperator.Parse("Reference=?", req.Reference));

            if (FO != null)
            {

                if (FO != null)
                {
                    // Retrieve the report data object for the Payment report
                    var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
                    IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Payment");

                    // Check if the report data object is not null
                    if (reportData != null)
                    {
                        // Retrieve the handle of the report container
                        string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

                        // Show the preview of the report using the ReportServiceController
                        ReportServiceController controller = Frame.GetController<ReportServiceController>();
                        if (controller != null)
                        {
                            // Filter the report data by Payment id
                            CriteriaOperator criteria = CriteriaOperator.Parse("PaymentNumber=?", FO.Reference);
                            controller.ShowPreview(handle, criteria);
                        }
                    }
                }
                else
                {
                    // Payment object not found
                    // Handle the case when the Payment object is not found
                }
            }
        }
    }
}
