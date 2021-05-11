using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTTAnalyzer
{
    public class ThreadController
    {
        private Status _status;
        private Thread _thread;
        private MainForm _main;

        public ThreadController(Status status, MainForm main)
        {
            _status = status;
            _main = main;
        }

        public void StartThread()
        {
            if (_thread != null)
                CloseThread();
            _thread = new Thread(InitThread);
            _thread.Start();
            Unalyser.locker = false;
            
        }

        public void InitThread()
        {
            Unalyser.InitAnylyzer(_status.GetIpList, _main);
        }

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
