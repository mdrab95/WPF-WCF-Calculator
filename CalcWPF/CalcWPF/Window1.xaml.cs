using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Reporting.WinForms;


namespace CalcWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        // List<myList> toExport;
        List<String> listOfValues;
        public Window1(List<string>savedList)
        {
            //toExport = new List<myList>();
            listOfValues = new List<string>();
            listOfValues = savedList;
            /*
            for (int i = 0; i < savedList.Count - 2; i+=3)
            {
                toExport.Add(new myList { Guid = savedList[i], Date = savedList[i + 1], Operation = savedList[i + 2] });
            }
            */
            InitializeComponent();
            //lvOperations.ItemsSource = toExport;
        }
        // ServiceReference1.ServiceClient Operations = new ServiceReference1.ServiceClient();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this._reportViewer.LocalReport.DataSources.Clear();

            string reportName = "report1.rdl";
            string myReportPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Reports\", reportName);

            this._reportViewer.LocalReport.ReportPath = myReportPath;
            
            DataTable dt = new DataTable();
            dt.Columns.Add("GUID", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Operation", typeof(string));
            
            for (int i=0; i<listOfValues.Count-2; i+=3)
            dt.Rows.Add(listOfValues[i], listOfValues[i+1], listOfValues[i+2]);

        

            this._reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            this._reportViewer.RefreshReport();
        }
    }

    public class myList
    {
        public string Guid { get; set; }
        public string Date { get; set; }
        public string Operation { get; set; }
    }
}
