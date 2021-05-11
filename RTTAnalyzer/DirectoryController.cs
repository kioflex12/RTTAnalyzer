using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTAnalyser
{
    class DirectoryController
    {
        public void ClearTempDirectory()
        {
            DirectoryInfo dirPath = new DirectoryInfo("snapshots");
            foreach (FileInfo file in dirPath.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
