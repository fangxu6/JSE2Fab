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
   

    public class HTTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile,int mapdeg)
        {
            // 读取来源文件
            Dat source = new Dat(tskfile);  //zjf 2008.09.03
            //IMappingFile source = new Dat(tskfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "httxt");
        
            // 创建新文件
            HtTxt httxt = new HtTxt(txtfile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                httxt.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            httxt.DieMatrix = source.DieMatrix.Clone();
            httxt.RowCount = httxt.DieMatrix.YMax;
            httxt.ColCount = httxt.DieMatrix.XMax;

            //zjf 2008.09.03
            //begin
            httxt.Device = source.Device;
            httxt.LotNo = source.LotNo.Replace("-CP2", "").Replace("-CP1", "").Replace("-CP3", "");
            httxt.WaferID = source.WaferID.Replace("-CP2", "").Replace("-CP1", "").Replace("-CP3", "");
            httxt.WaferSize = source.WaferSize;
            httxt.TotalDie = source.TotalDie;
            httxt.IndexSizeX = source.IndexSizeX;
            httxt.IndexSizeY = source.IndexSizeY;
            httxt.LoadTime = source.LoadTime;
            httxt.UnloadTime = source.UnloadTime;
            httxt.StartTime = source.StartTime;
            httxt.EndTime = source.EndTime;
            httxt.SlotNo = source.SlotNo;
            httxt.FlatDir = source.FlatDir;
            //end

            // 重新计算统计数据
            httxt.PassDie = 0;
            httxt.FailDie = 0;

            foreach (DieData die in httxt.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    httxt.FailDie += 1;
                else if (die.Attribute == DieCategory.PassDie)
                    httxt.PassDie += 1;
            }

            // 旋转角度
           // httxt.DeasilRotate(convertConfig.Rotate);
            httxt.DeasilRotate(mapdeg);
            httxt.FlatDir = httxt.FlatDir + mapdeg;
            if (httxt.FlatDir >= 360)
            {
                httxt.FlatDir = httxt.FlatDir - 360;
            }

            // 保存文件
            httxt.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "httxt");

            // 创建新文件
            CmdTxt httxt = new CmdTxt(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                httxt.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            httxt.DieMatrix = source.DieMatrix.Clone();
            httxt.RowCount = httxt.DieMatrix.YMax;
            httxt.ColCount = httxt.DieMatrix.XMax;

            // 旋转角度
            httxt.DeasilRotate(convertConfig.Rotate);

            return httxt;
        }
    }
}
