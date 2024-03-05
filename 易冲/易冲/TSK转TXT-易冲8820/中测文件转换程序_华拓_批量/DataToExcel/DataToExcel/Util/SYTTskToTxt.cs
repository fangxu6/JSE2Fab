/*
 * 作者：Aegon
 * 时间：2020-12-09
 * 作用：SYT 要求，将 tsk 格式转换成 txt 格式
 */

namespace DataToExcel
{
    using System;
    using System.Collections;
    using DataToExcel;

    public class SYTTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile,int mapdeg)
        {
            // 读取来源文件
            Dat source = new Dat(tskfile);  //zjf 2008.09.03
            //IMappingFile source = new Dat(tskfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "syttxt");

            // 创建新文件
            SytTxt syttxt = new SytTxt(txtfile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                syttxt.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            syttxt.DieMatrix = source.DieMatrix.Clone();
            syttxt.RowCount = syttxt.DieMatrix.YMax;
            syttxt.ColCount = syttxt.DieMatrix.XMax;

            //zjf 2008.09.03
            //begin
            syttxt.Device = source.Device;
            syttxt.LotNo = source.LotNo;
            syttxt.WaferID = source.WaferID;
            syttxt.WaferSize = source.WaferSize;
            syttxt.TotalDie = source.TotalDie;
            syttxt.IndexSizeX = source.IndexSizeX;
            syttxt.IndexSizeY = source.IndexSizeY;
            syttxt.LoadTime = source.LoadTime;
            syttxt.UnloadTime = source.UnloadTime;
            syttxt.StartTime = source.StartTime;
            syttxt.EndTime = source.EndTime;
            syttxt.SlotNo = source.SlotNo;
            syttxt.FlatDir = source.FlatDir;
            //end

            // 重新计算统计数据
            syttxt.PassDie = 0;
            syttxt.FailDie = 0;

            foreach (DieData die in syttxt.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    syttxt.FailDie += 1;
                else if (die.Attribute == DieCategory.PassDie)
                    syttxt.PassDie += 1;
            }

            // 旋转角度
          //  syttxt.DeasilRotate(convertConfig.Rotate);
            syttxt.DeasilRotate(mapdeg);
            syttxt.FlatDir = syttxt.FlatDir + mapdeg;
            if (syttxt.FlatDir >= 360)
            {
                syttxt.FlatDir = syttxt.FlatDir - 360;
            }

            // 保存文件
            syttxt.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "syttxt");

            // 创建新文件
            CmdTxt syttxt = new CmdTxt(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                syttxt.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            syttxt.DieMatrix = source.DieMatrix.Clone();
            syttxt.RowCount = syttxt.DieMatrix.YMax;
            syttxt.ColCount = syttxt.DieMatrix.XMax;

            // 旋转角度
            syttxt.DeasilRotate(convertConfig.Rotate);

            return syttxt;
        }
    }
}
