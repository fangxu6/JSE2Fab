using System;
using System.Collections.Generic;
using System.Text;

namespace DataToExcel.ExpDataToExcelFactory
{
    public abstract class ExpToExcelSoftBin
    {
        public static string _Device = "";
        public static string _LotNo = "";
        public static int _singleTotalDie = 0;
        public static int _TotalDie = 0;
        public static int _TotalFailDie = 0;
        public static int _TotalPassDie = 0;
        public static string _TotalYield = "";
        public abstract void expToExcel(Excel.Worksheet worksheet);

        public  void SaveToTxt(Dat source,string txtfile)
        {
            // 创建新文件
            CmdTxt cmdtxt = new CmdTxt(txtfile);


            // 导入 die 列表
            cmdtxt.DieMatrix = source.DieMatrix.Clone();
            cmdtxt.RowCount = cmdtxt.DieMatrix.YMax;
            cmdtxt.ColCount = cmdtxt.DieMatrix.XMax;

            //zjf 2008.09.03
            //begin
            cmdtxt.Device = source.Device;
            cmdtxt.LotNo = source.LotNo;
            cmdtxt.WaferID = source.WaferID;
            cmdtxt.WaferSize = source.WaferSize;
            cmdtxt.TotalDie = source.TotalDie;
            cmdtxt.IndexSizeX = source.IndexSizeX;
            cmdtxt.IndexSizeY = source.IndexSizeY;
            cmdtxt.LoadTime = source.LoadTime;
            cmdtxt.UnloadTime = source.UnloadTime;
            cmdtxt.StartTime = source.StartTime;
            cmdtxt.EndTime = source.EndTime;
            cmdtxt.SlotNo = source.SlotNo;
            cmdtxt.FlatDir = source.FlatDir;
            //end

            // 重新计算统计数据
            cmdtxt.PassDie = 0;
            cmdtxt.FailDie = 0;

            foreach (DieData die in cmdtxt.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    cmdtxt.FailDie += 1;
                else if (die.Attribute == DieCategory.PassDie)
                    cmdtxt.PassDie += 1;
            }

            // 旋转角度
            if(defatultRotate()>0)
            {
                cmdtxt.DeasilRotate(defatultRotate());
                cmdtxt.FlatDir += defatultRotate();
            }

            // 保存文件
            if (defatultSave())
            {
                cmdtxt.Save();
            } else
            {
                Save(cmdtxt);
            }

        }

        public virtual int defatultRotate()
        {
            return 0;
        }

        public virtual void Save(CmdTxt cmd)
        {
            return;
        }

        public virtual bool defatultSave()
        {
            return true;
        }


    }
}
