using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace RTTAnalyser
{
    static class Unalyser
    {
        #region params
        static String HTTP_TEST_HOST; // HTTP сервер, соединение до которого будем тестировать
        static int HTTP_TEST_PORT; // Порт HTTP сервера
        static int HTTP_TIMEOUT;   // Таймаут подключения
        static int PING_COUNT;     // Количество пакетов пинга
        static int PING_DELAY;     // Ожидание перед отправкой следующего пакета пинга
        static int PING_TIMEOUT;   // Таймаут пинга
        static List<String> PING_HOSTS;     // Хосты, пинг до которых меряем
        static int MEASURE_DELAY;  // Время между соседними проверками
        static String ROUTER_IP;      // IP роутера
        static bool CUI_ENABLED;    // Включить/выключить консольный вывод
        static double MAX_PKT_LOSS;   // Максимально допустимый Packet loss
        static String OUT_CSV_FILE;   // Выходной файл CSV
        static bool WRITE_CSV;      // Писать ли CSV
        static String CSV_PATTERN;    // Шаблон для записи в CSV
        static String TG_TOKEN;       // Токен бота Telegram
        static String TG_CHAT_ID;     // ID чата Telegram
        static bool TG_NOTIFY;      // Включить уведомления в Telegram
        #endregion

        #region global vars
        static long start_measure_id = DateTime.Now.ToBinary();
        static long total_time = 0;
        static int pkt_sent = 0;
        static int success_pkts = 0;
        static int exited_threads = 0;
        static Dictionary<string, int> measure_results = new Dictionary<string, int>();
        static HttpClient httpc;
        static DateTime first_fail_time;
        static bool prev_inet_ok = true;
        static MainForm _uI;
        /// <summary>
        /// Переменная локер отвечающая за блокировку потока
        /// </summary>
        public static bool locker = true;
        #endregion

        static List<string> dates = new List<string>() { };

        public static void InitAnylyzer(List<string> pingHosts, MainForm uI)
        {
            //Load config file
            _uI = uI;
            var config = JsonConvert.DeserializeObject<Dictionary<String, Object>>(File.ReadAllText("nmon.json"));
            MethodInvoker methodInvoker = delegate ()
            {
                foreach (DataGridViewColumn column in _uI.IpArray.Columns)
                {
                    _uI.IpArray.Columns.Remove(column);
                }
                

                _uI.IpArray.Columns.Clear();
                foreach (var host in pingHosts)
                {

                    _uI.IpArray.Columns.Add(host, host);

                }
            };
            _uI.Invoke(methodInvoker);
           
            PING_HOSTS = pingHosts;
            HTTP_TEST_HOST = (String)config["http_test_host"];
            ROUTER_IP = (String)config["router_ip"];
            HTTP_TEST_PORT = int.Parse((String)config["http_test_port"]);
            HTTP_TIMEOUT = int.Parse((String)config["http_timeout"]);
            PING_COUNT = int.Parse((String)config["ping_count"]);
            PING_TIMEOUT = int.Parse((String)config["ping_timeout"]);
            HTTP_TIMEOUT = int.Parse((String)config["http_timeout"]);
            PING_DELAY = int.Parse((String)config["ping_packet_delay"]);
            MEASURE_DELAY = int.Parse((String)config["measure_delay"]);
            CUI_ENABLED = bool.Parse((String)config["cui_output"]);
            OUT_CSV_FILE = (String)config["out_file"];
            WRITE_CSV = bool.Parse((String)config["w_csv"]);
            CSV_PATTERN = (String)config["out_format"];
            MAX_PKT_LOSS = double.Parse((String)config["nq_max_loss"]);
            TG_NOTIFY = bool.Parse((String)config["tg_notify"]);
            TG_TOKEN = (String)config["tg_token"];
            TG_CHAT_ID = (String)config["tg_chat_id"];

           
            DoMeasures();
        }

      
        static void WriteLog(String message)
        {
            var msg = $"[{DateTime.Now.ToShortTimeString()}] {message}\r\n";
            MessageBox.Show(msg);
        }

        static void SaveSnapshot(net_state snapshot)
        {
            if (!locker)
            {
                //среднее значение ртт
                int avg_rtt = 0;
                //максимальное значение ртт
                int max_rtt = 0;
                //среднее значение ртт внутри локальной сети
                int avg_rtt_local = 0;
             

                List<string> vs = new List<string>();
                //т.к пингует отдельный поток чтобы UI не завис, нам необходим делегат , который позволит общаться между потоками
                MethodInvoker methodInvoker = delegate ()
                {
                    if (_uI.IpArray.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in _uI.IpArray.Rows)
                        {
                            int res;
                            int.TryParse((row.Cells[0].Value.ToString()), out res);
                            avg_rtt += res;
                        }
                        avg_rtt = avg_rtt / _uI.IpArray.Rows.Count;
                    }
                    var index = _uI.IpArray.Rows.Add();

                    for (int i = 0; i < _uI.IpArray.Columns.Count; i++)
                    {
                        _uI.IpArray.Rows[index].Cells[i].Value = snapshot.avg_rtts[_uI.IpArray.Columns[i].Name];

                    }
                    _uI.Status.MaxPing = _uI.Status.MaxPing > max_rtt ? _uI.Status.MaxPing : max_rtt;

                    for (int i = 1; i < PING_HOSTS.Count; i++)
                    {
                        avg_rtt_local += snapshot.avg_rtts[PING_HOSTS[i]];
                    }
                    _uI.Chart.UpdateChart(snapshot.measure_time.ToString(), snapshot.avg_rtts[PING_HOSTS[0]]);

                    _uI.Status.AvgPing = avg_rtt;
                    _uI.Status.CountMembers = PING_HOSTS.Count;
                    _uI.Status.IntenetStatus = snapshot.inet_ok;
                    if ((avg_rtt_local / PING_HOSTS.Count - 1) < 1)
                    {
                        _uI.Status.NetworkStatus = "Good";
                    }
                    else
                    {
                        _uI.Status.NetworkStatus = "Bad";
                        WriteLog("Плохое соеденение в локальной сети");


                    }
                    _uI.UpdateStatusSummary();
                };
                //invoke отправляет делегат родительскому потоку
                _uI.Invoke(methodInvoker);
            }
            else 
                return;
        }

        ///<summary>
        ///запускает программу проверки ping
        ///</summary>
        private static void DoMeasures()
        {
            System.Timers.Timer _timer = new System.Timers.Timer();
            _timer.AutoReset = true;
            _timer.Interval = MEASURE_DELAY;
            _timer.Elapsed += delegate
            {
                if (!locker)
                {
                    net_state snapshot = new net_state();
                    snapshot.inet_ok = true;
                    snapshot.measure_id = start_measure_id++;
                    snapshot.measure_time = DateTime.Now;
                    Ping ping = new Ping();
                    //Первая проверка если роутер активен
                    var prr = ping.Send(ROUTER_IP, PING_TIMEOUT);
                    snapshot.router_rtt = prr.Status == IPStatus.Success ? (int)prr.RoundtripTime : PING_TIMEOUT;
                    if (prr.Status != IPStatus.Success)
                    {

                        //Роутер не доступен
                        snapshot.avg_rtts = new Dictionary<string, int>();
                        snapshot.http_ok = false;
                        snapshot.inet_ok = false;
                        snapshot.packet_loss = 1;
                        foreach (var ci in PING_HOSTS)
                        {
                            snapshot.avg_rtts.Add(ci, PING_TIMEOUT);
                        }
                        WriteLog("Router was unreachable.");

                        SaveSnapshot(snapshot);
                        if (prev_inet_ok)
                        {
                            //Нет доступа к интернету
                            prev_inet_ok = false;
                            first_fail_time = DateTime.Now;
                        }
                        return;
                    }
                    
                    try
                    {
                        snapshot.http_ok = true;
                        TcpClient tc = new TcpClient();
                        tc.BeginConnect(HTTP_TEST_HOST, HTTP_TEST_PORT, null, null);
                        Thread.Sleep(HTTP_TIMEOUT);
                        if (!tc.Connected)
                        {
                            snapshot.http_ok = false;
                        }
                        tc.Dispose();
                    }
                    catch { snapshot.http_ok = false; snapshot.inet_ok = false; }
                    //Now do ping test
                    exited_threads = 0;
                    pkt_sent = 0;
                    success_pkts = 0;
                    total_time = 0;
                    measure_results = new Dictionary<string, int>();
                    foreach (var ci in PING_HOSTS)
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(PerformPingTest));
                        thread.Start(ci);
                    }
                    while (exited_threads < PING_HOSTS.Count) continue;
                    //Analyze results
                    snapshot.avg_rtts = measure_results;
                    snapshot.packet_loss = (double)(pkt_sent - success_pkts) / pkt_sent;
                    snapshot.inet_ok = !(
                        snapshot.http_ok == false ||
                        ((double)total_time / success_pkts >= 0.75 * PING_TIMEOUT) ||
                        snapshot.packet_loss >= MAX_PKT_LOSS ||
                        snapshot.router_rtt == PING_TIMEOUT);
                    SaveSnapshot(snapshot);
                   
                }
                else 
                    _timer.Stop() ;
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpc = new HttpClient();
            _timer.Start();
            if (locker)
                return ;
            while (true) Thread.Sleep(1000000);
        }
        /// <summary>
        /// Пингует переданный хост (arg)
        /// </summary>
        /// <param name="arg"></param>
        static void PerformPingTest(Object arg)
        {
            String host = (String)arg;
            int pkts_lost_row = 0;
            int local_success = 0;
            long local_time = 0;
            Ping ping = new Ping();
            for (int i = 0; i < PING_COUNT; i++)
            {
                if (pkts_lost_row == 3)
                {
                    measure_results.Add(host, (int)(local_time / (local_success == 0 ? 1 : local_success)));
                    exited_threads++;
                    return;
                }
                try
                {
                    var result = ping.SendPingAsync(host, PING_TIMEOUT);
                    if (result.Result.Status == IPStatus.Success)
                    {
                        pkts_lost_row = 0;
                        local_success++;
                        local_time += result.Result.RoundtripTime;
                        total_time += result.Result.RoundtripTime;
                        pkt_sent++;
                        success_pkts++;
                    }
                    switch (result.Result.Status)
                    {
                        case IPStatus.Success: break; //Already handled 
                        case IPStatus.BadDestination:
                            WriteLog($"Bad ping destination address: {host}");
                            measure_results.Add(host, -1);
                            exited_threads++;
                            return;
                        case IPStatus.DestinationHostUnreachable:
                        case IPStatus.DestinationNetworkUnreachable:
                        case IPStatus.DestinationUnreachable:
                            WriteLog($"Target host is unreachable: {host}");
                            measure_results.Add(host, -1);
                            exited_threads++;
                            return;
                        case IPStatus.TimedOut:
                            pkts_lost_row++;
                            pkt_sent++;
                            break;
                        default:
                            WriteLog($"Error pinging {host}: {result.Status}");
                            measure_results.Add(host, -1);
                            exited_threads++;
                            return;
                    }
                }
                catch (Exception xc)
                {
                    WriteLog(xc.Message);
                    exited_threads++;
                    measure_results.Add(host, -1);
                    return;
                }
            }
            try
            {
                measure_results.Add(host, (int)(local_time / (local_success == 0 ? 1 : local_success)));
                exited_threads++;
            }
            catch { }

            return;
        }
    }
    /// <summary>
    /// Представление однотипных данных под структурой
    /// </summary>
    struct net_state
    {
        public bool inet_ok;
        public bool http_ok;
        public Dictionary<String, int> avg_rtts;
        public double packet_loss;
        public DateTime measure_time;
        public int router_rtt;
        public long measure_id;
    }
}

