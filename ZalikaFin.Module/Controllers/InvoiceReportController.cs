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
using static System.Net.Mime.MediaTypeNames;
using ZalikaFin.Module.BusinessObjects.Invoice;

namespace ZalikaFin.Module.Controllers
{
    public class InvoiceReportsController : ObjectViewController<DetailView, glinvoice>
    {
        public InvoiceReportsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction PreviewInvoice = new SimpleAction(this,
                                                           "PreviewInvoiceAction",
                                                           "Reports");
            PreviewInvoice.Caption = "Preview Invoice";
            PreviewInvoice.ImageName = "Business_Report";
            PreviewInvoice.ToolTip = "Preview Invoice";
            PreviewInvoice.Execute += PreviewInvoice_Execute;
        }

        private void PreviewInvoice_Execute(object sender, SimpleActionExecuteEventArgs e)

        {
            IObjectSpace os = Application.CreateObjectSpace();
            Session _sess = ((XPObjectSpace)os).Session;

            glinvoice req = (glinvoice)View.CurrentObject;

            UnitOfWork uow = new UnitOfWork(_sess.DataLayer);
            glinvoice FO = uow.FindObject<glinvoice>(CriteriaOperator.Parse("InvoiceNumber=?", req.InvoiceNumber));

            if (FO != null)
            {
            
                if (FO != null)
                {
                    // Retrieve the report data object for the Invoice report
                    var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
                    IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Invoice");

                    // Check if the report data object is not null
                    if (reportData != null)
                    {
                        // Retrieve the handle of the report container
                        string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

                        // Show the preview of the report using the ReportServiceController
                        ReportServiceController controller = Frame.GetController<ReportServiceController>();
                        if (controller != null)
                        {
                            // Filter the report data by Invoice id
                            CriteriaOperator criteria = CriteriaOperator.Parse("InvoiceNumber=?", FO.InvoiceNumber);
                            controller.ShowPreview(handle, criteria);
                        }
                    }
                }
                else
                {
                    // Invoice object not found
                    // Handle the case when the Invoice object is not found
                }
            }
        }
    }
}
