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
    public class RequisitionReportsController : ObjectViewController<DetailView, glforequisition>
    {
        public RequisitionReportsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
            SimpleAction PreviewRequisition = new SimpleAction(this,
                                                           "PreviewRequisitionAction",
                                                           "Reports");
            PreviewRequisition.Caption = "Preview Requisition";
            PreviewRequisition.ImageName = "Business_Report";
            PreviewRequisition.ToolTip = "Preview Requisition";
            PreviewRequisition.Execute += PreviewRequisition_Execute;
        }

        private void PreviewRequisition_Execute(object sender, SimpleActionExecuteEventArgs e)

        {
            IObjectSpace os = Application.CreateObjectSpace();
            Session _sess = ((XPObjectSpace)os).Session;

            glforequisition req = (glforequisition)View.CurrentObject;

            UnitOfWork uow = new UnitOfWork(_sess.DataLayer);
            glforequisition FO = uow.FindObject<glforequisition>(CriteriaOperator.Parse("Reference=?", req.RequisitionNumber));

            if (FO != null)
            {

                if (FO != null)
                {
                    // Retrieve the report data object for the Requisition report
                    var reportOsProvider = ReportDataProvider.GetReportObjectSpaceProvider(this.Application.ServiceProvider);
                    IObjectSpace objectSpace = reportOsProvider.CreateObjectSpace(typeof(ReportDataV2));
                    IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(data => data.DisplayName == "Requisition");

                    // Check if the report data object is not null
                    if (reportData != null)
                    {
                        // Retrieve the handle of the report container
                        string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);

                        // Show the preview of the report using the ReportServiceController
                        ReportServiceController controller = Frame.GetController<ReportServiceController>();
                        if (controller != null)
                        {
                            // Filter the report data by Requisition id
                            CriteriaOperator criteria = CriteriaOperator.Parse("RequisitionNumber=?", FO.RequisitionNumber);
                            controller.ShowPreview(handle, criteria);
                        }
                    }
                }
                else
                {
                    // Requisition object not found
                    // Handle the case when the Requisition object is not found
                }
            }
        }
    }
}
