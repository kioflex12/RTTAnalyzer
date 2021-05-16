using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;

namespace RTTAnalyser
{
    /// <summary>
    /// Класс управления LiveTimeChart
    /// </summary>
    public class Chart
    {
        private LiveCharts.WinForms.CartesianChart   _cartesianChart;
        private                     Status           _status;
        private                     SeriesCollection _series;
        private                     ChartValues<int> _ipValues;
        private                     LineSeries       _pingLine;
        private                     List<string>     _dates = new List<string>() { };
        /// <summary>
        /// Конструктор инциализации LiveTimeChart
        /// </summary>
        /// <param name="cartesianChart"></param>
        /// <param name="status"></param>
        public Chart(LiveCharts.WinForms.CartesianChart cartesianChart, Status status)
        {
            _cartesianChart = cartesianChart;
            _status = status;
            InitChart();

        }
        /// <summary>
        /// Инициализация LiveTimeChart
        /// </summary>
        private void InitChart()
        {

            //коллекция линий
            _series = new SeriesCollection();
            //точки ip
            _ipValues = new ChartValues<int>();
            //сводка по точкам
            _pingLine = new LineSeries();



        }
        /// <summary>
        /// Обновление LiveTimeChart
        /// </summary>
        /// <param name="date">"Дата"</param>
        /// <param name="ipValue">"ip адресс"</param>
        public void UpdateChart(string date, int ipValue)
        {
            
            _ipValues.Add(ipValue);
            _dates.Add(date);

            _cartesianChart.AxisX.Clear();
            _cartesianChart.AxisX.Add(new Axis()
            {
                Title = "Date",
                Labels = _dates
            });

            //pingLine;
            _pingLine.Title = _status.GetIpList[0];
            _pingLine.Values = _ipValues;

            _series.Clear();
            _series.Add(_pingLine);

            _cartesianChart.Series = _series;
        }
    }
}
