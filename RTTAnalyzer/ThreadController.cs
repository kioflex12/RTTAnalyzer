using System.Threading;

namespace RTTAnalyser
{
    /// <summary>
    /// Контроллер потоков
    /// </summary>
    public class ThreadController
    {
        private Status   _status;
        private Thread   _thread;
        private MainForm _main;
        /// <summary>
        /// Инициализатор контроллера потоков
        /// </summary>
        /// <param name="status"></param>
        /// <param name="main"></param>
        public ThreadController(Status status, MainForm main)
        {
            _status = status;
            _main = main;
        }

        /// <summary>
        /// Запустить поток
        /// </summary>
        public void StartThread()
        {
            if (_thread != null)
                CloseThread();
            _thread = new Thread(InitThread);
            _thread.Start();
            Unalyser.locker = false;
            
        }
        /// <summary>
        /// Инциализирует поток
        /// </summary>
        public void InitThread()
        {
            Unalyser.InitAnylyzer(_status.GetIpList, _main);
        }
        /// <summary>
        /// Выключает поток
        /// </summary>
        public void CloseThread()
        {
            if (_thread != null)
            {
                Unalyser.locker = true;
                _thread.Abort();
                _thread = null;
            }
        }
    }
}
