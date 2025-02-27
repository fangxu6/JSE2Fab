using DataToExcel;

namespace WindowsFormTool.TskUtil
{
    public static class TskFileLoader
    {
        public static Tsk LoadTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read();
            return tsk;
        }
    }
}