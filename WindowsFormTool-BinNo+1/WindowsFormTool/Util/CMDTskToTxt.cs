
/*
 * 作者：sky
 * 时间：2008-06-25
 * 作用：CMD 要求，将 tsk 格式转换成 txt 格式
 */

namespace DataToExcel
{
    using System;
    using System.Collections;
    using DataToExcel;

    public class CMDTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile)
        {
            // 读取来源文件
            Dat source = new Dat(tskfile);  //zjf 2008.09.03
            //IMappingFile source = new Dat(tskfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "cmdtxt");

            // 创建新文件
            CmdTxt cmdtxt = new CmdTxt(txtfile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                cmdtxt.Properties[f.To] = source.Properties[f.From];
            }

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
            cmdtxt.DeasilRotate(convertConfig.Rotate);

            // 保存文件
            cmdtxt.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "cmdtxt");

            // 创建新文件
            CmdTxt cmdtxt = new CmdTxt(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                cmdtxt.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            cmdtxt.DieMatrix = source.DieMatrix.Clone();
            cmdtxt.RowCount = cmdtxt.DieMatrix.YMax;
            cmdtxt.ColCount = cmdtxt.DieMatrix.XMax;

            // 旋转角度
            cmdtxt.DeasilRotate(convertConfig.Rotate);

            return cmdtxt;
        }
    }
}
