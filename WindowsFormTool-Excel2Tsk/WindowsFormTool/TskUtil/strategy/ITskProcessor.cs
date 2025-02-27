using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormTool.TskUtil
{
    public interface ITskProcessor
    {
        void ProcessSingle(string firstFile, string secondFile, Action<string> updateStatus, ProgressBar progressBar = null);
        void ProcessBatch(List<string> firstFiles, List<string> secondFiles, Action<string> updateStatus, ProgressBar progressBar = null);
    }
}
