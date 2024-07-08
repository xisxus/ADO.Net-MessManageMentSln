using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessManageMent.ViewModel;

namespace MessManageMent
{
    public partial class ReportMess : Form
    {
        List<MessViewModel> _list;
        public ReportMess(List<MessViewModel> list)
        {
            InitializeComponent();
            _list = list;
        }

        private void ReportMess_Load(object sender, EventArgs e)
        {
            RptMessReport rptMessReport = new RptMessReport();
            rptMessReport.SetDataSource(_list);
            
            crystalReportViewer1.ReportSource = rptMessReport;
            crystalReportViewer1.Refresh();
        }
    }
}
