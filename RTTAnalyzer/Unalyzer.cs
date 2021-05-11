using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTAnalyzer
{
    static class Unalyzer
    {
        #region Service code (PInvoke)
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        #endregion

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
        static int seq_id = 1;
        static long total_time = 0;
        static int pkt_sent = 0;
        static int success_pkts = 0;
        static int exited_threads = 0;
        static Dictionary<string, int> measure_results = new Dictionary<string, int>();
        static HttpClient httpc;
        static DateTime first_fail_time;
        static bool prev_inet_ok = true;
        static MainForm _uI;
        #endregion

        public static void InitAnylyzer(List<string> pingHosts, MainForm uI)
        {
            //Load config file
            _uI = uI;
            var config = JsonConvert.DeserializeObject<Dictionary<String, Object>>(File.ReadAllText("nmon.json"));
            MethodInvoker methodInvoker = delegate ()
            {
                foreach (var host in pingHosts)
                {
                    _uI.IpArray.Columns.Add(host, host);

                }
            };
            _uI.Invoke(methodInvoker);
            PING_HOSTS = pingHosts;
            //PING_HOSTS = PING_HOSTS = ((JArray)config["ping_hosts"]).ToObject<List<String>>();


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

            String CSV_HEADER = CSV_PATTERN
                .Replace("FTIME", "Snapshot time")
                .Replace("IUP", "Internet up")
                .Replace("AVGRTT", "Average ping (ms)")
                .Replace("ROUTERRTT", "Ping to router (ms)")
                .Replace("LOSS", "Packet loss, %")
                .Replace("MID", "Measure ID")
                .Replace("SEQ", "SeqID")
                .Replace("HTTP", "HTTP OK")
                .Replace("STIME", "STime");
            foreach (var host in PING_HOSTS)
            {
                CSV_HEADER = CSV_HEADER.Replace("RN", $"RTT to {host};RN");
            }
            CSV_HEADER = CSV_HEADER.Replace("RN", ";;\r\n");
            if (WRITE_CSV)
            {
                if (!File.Exists(OUT_CSV_FILE)) File.WriteAllText(OUT_CSV_FILE, CSV_HEADER);
            }
            //_statusLabel = uI.GetStatusLabel;
            //ShowWindow(GetConsoleWindow(), SW_HIDE);
            DoMeasures();
        }
        static async void TgNotify(String message, bool with_sound)
        {
            if (!TG_NOTIFY) return;
            Dictionary<String, String> req_data = new Dictionary<string, string>();
            req_data.Add("chat_id", TG_CHAT_ID);
            req_data.Add("text", message);
            req_data.Add("disable_notification", (!with_sound).ToString().ToLower());
            String sf = JsonConvert.SerializeObject(req_data);
            try
            {
                var result = await httpc.PostAsync($"https://api.telegram.org/bot{TG_TOKEN}/sendMessage", new StringContent(sf, System.Text.Encoding.UTF8, "application/json"));
            }
            catch
            {
            }
        }
        static void WriteLog(String message)
        {
            var msg = $"[{DateTime.Now.ToShortTimeString()}] {message}\r\n";
            try
            {
                File.AppendAllText("nmon.log", msg);
            }
            catch { }
            if (CUI_ENABLED) Console.WriteLine(msg);
            TgNotify(msg, false);
        }

        static void SaveSnapshot(net_state snapshot)
        {
            String raw_json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
            String snapshot_path = Path.Combine(Environment.CurrentDirectory, "snapshots", $"net{snapshot.measure_id}.json");
            File.WriteAllText(snapshot_path, raw_json);
            String rtts = "";
            int avg_rtt = 0;
            int max_rtt = 0;
            foreach (var ci in PING_HOSTS)
            {
                rtts += $"{snapshot.avg_rtts[ci]};";
                avg_rtt += snapshot.avg_rtts[ci];
                max_rtt = snapshot.avg_rtts[ci] > max_rtt ? snapshot.avg_rtts[ci] : max_rtt; 
                

               
            }
            avg_rtt = avg_rtt / PING_HOSTS.Count;
            List<string> vs  = new List<string>();
            MethodInvoker methodInvoker = delegate()
            {
                var index = _uI.IpArray.Rows.Add();

                for (int i = 0; i < _uI.IpArray.Columns.Count; i++)
                {
                    _uI.IpArray.Rows[index].Cells[i].Value = snapshot.avg_rtts[_uI.IpArray.Columns[i].Name];
                }
                _uI.Status.MaxPing = _uI.Status.MaxPing > max_rtt ? _uI.Status.MaxPing : max_rtt;
                _uI.Status.AvgPing = avg_rtt;
                _uI.Status.CountMembers = PING_HOSTS.Count;
                if (_uI.Status.AvgPing < 1)
                {
                    _uI.Status.NetworkStatus = "Good";
                }
               
            };
            _uI.Invoke(methodInvoker);

            _uI.UpdateStatusSummary();
            

            if (WRITE_CSV)
            {
                //Generate RTT string
                rtts = "";
                avg_rtt = 0;
                foreach (var ci in PING_HOSTS)
                {
                    rtts += $"{snapshot.avg_rtts[ci]};";
                    avg_rtt += snapshot.avg_rtts[ci];
                }
                avg_rtt = avg_rtt / PING_HOSTS.Count;
                File.AppendAllText(OUT_CSV_FILE, CSV_PATTERN
                    .Replace("FTIME", snapshot.measure_time.ToShortDateString() + " " + snapshot.measure_time.ToShortTimeString())
                    .Replace("IUP", snapshot.inet_ok.ToString())
                    .Replace("AVGRTT", avg_rtt.ToString())
                    .Replace("ROUTERRTT", snapshot.router_rtt.ToString())
                    .Replace("LOSS", snapshot.packet_loss.ToString())
                    .Replace("HTTP", snapshot.http_ok.ToString())
                    .Replace("MID", snapshot.measure_id.ToString())
                    .Replace("STIME", snapshot.measure_time.ToShortTimeString())
                    .Replace("SEQ", seq_id++.ToString())
                    .Replace("RN", $"{rtts};;\r\n"));
            }
        }


        private static Task DoMeasures()
        {
            System.Timers.Timer _timer = new System.Timers.Timer();
            _timer.AutoReset = true;
            _timer.Interval = MEASURE_DELAY;
            _timer.Elapsed += delegate
            {
                net_state snapshot = new net_state();
                snapshot.inet_ok = true;
                snapshot.measure_id = start_measure_id++;
                snapshot.measure_time = DateTime.Now;
                Ping ping = new Ping();
                //First, check if router is available
                var prr = ping.Send(ROUTER_IP, PING_TIMEOUT);
                snapshot.router_rtt = prr.Status == IPStatus.Success ? (int)prr.RoundtripTime : PING_TIMEOUT;
                if (prr.Status != IPStatus.Success)
                {

                    //Router is unreachable. Don't waste resources
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
                        //Internet was fine but failed now
                        prev_inet_ok = false;
                        first_fail_time = DateTime.Now;
                    }
                    return;
                }
                //Still alive so router is up
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
                if (prev_inet_ok && !snapshot.inet_ok)
                {
                    //Internet was fine but failed now
                    prev_inet_ok = false;
                    first_fail_time = DateTime.Now;
                }
                else if (!prev_inet_ok && snapshot.inet_ok)
                {
                    String t_s = new TimeSpan(DateTime.Now.Ticks - first_fail_time.Ticks).ToString(@"hh\:mm\:ss");
                    TgNotify($"Internet was down from {first_fail_time.ToShortTimeString()} to {DateTime.Now.ToShortTimeString()} (downtime {t_s})\r\n\r\n" +
                        $"Current average ping: {snapshot.avg_rtts.Values.Average()} ms\r\n" +
                        $"HTTP test: " + (snapshot.http_ok ? "PASSED" : "FAILED") + "\r\n" +
                        $"Packet loss: " + (snapshot.packet_loss * 100).ToString("N2"), true);
                    prev_inet_ok = true;
                }
            };
            //Foo();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpc = new HttpClient();
            _timer.Start();
            //TgNotify("nmon is now running", true);
            while (true) Thread.Sleep(1000000);
        }
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

