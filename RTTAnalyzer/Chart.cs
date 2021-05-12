using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTAnalyser
{
    public class Chart
    {
        private DataGridView _dataGridView;
        private LiveCharts.WinForms.CartesianChart _cartesianChart;
        private Status _status;
        private SeriesCollection series;
        private ChartValues<int> ipValues;
        private LineSeries pingLine;
        private List<string> dates = new List<string>() { };
        public Chart(DataGridView dataGrid, LiveCharts.WinForms.CartesianChart cartesianChart, Status status)
        {
            _dataGridView = dataGrid;
            _cartesianChart = cartesianChart;
            _status = status;
            InitChart();

        }
        private void InitChart()
        {


            series = new SeriesCollection();
            ipValues = new ChartValues<int>();
            pingLine = new LineSeries();



        }
        public void UpdateChart(string date, int ipValue)
        {
            
            //сделать плавный update
            ipValues.Add(ipValue);
            dates.Add(date);

            _cartesianChart.AxisX.Clear();
            _cartesianChart.AxisX.Add(new Axis()
            {
                Title = "Date",
                Labels = dates
            });

            //pingLine;
            pingLine.Title = _status.GetIpList[0];
            pingLine.Values = ipValues;

            series.Clear();
            series.Add(pingLine);

            _cartesianChart.Series = series;
        }
    }
}
