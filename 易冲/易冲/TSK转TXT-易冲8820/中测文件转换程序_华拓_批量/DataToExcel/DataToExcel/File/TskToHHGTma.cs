namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToHHGTma : ConverterBase
    {
        public override void Convert(string datfile, string HHGtmafile,int mapdeg)
        {
            // 读取来源文件
            IMappingFile source = new Tsk(datfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "HHGtma");

            // 创建新文件
            IMappingFile HHGtma = new HHGTma(HHGtmafile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                HHGtma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            HHGtma.DieMatrix = source.DieMatrix.Clone();

            HHGtma.Properties["ColCount"] = HHGtma.DieMatrix.XMax;
            HHGtma.Properties["RowCount"] = HHGtma.DieMatrix.YMax;
            HHGtma.Properties["Yield"] = (decimal)((int)HHGtma.Properties["PassDie"] / (int)HHGtma.Properties["TotalDie"]);

            int newdeg = (int)HHGtma.Properties["FlatDir"];
            // 旋转角度
            HHGtma.DeasilRotate(mapdeg);
            HHGtma.Properties["FlatDir"] = (int)HHGtma.Properties["FlatDir"] + mapdeg;
            if ((int)HHGtma.Properties["FlatDir"] >= 360)
            {
                HHGtma.Properties["FlatDir"] = (int)HHGtma.Properties["FlatDir"] - 360;
            }

            // 旋转角度
           // HHGtma.DeasilRotate(convertConfig.Rotate);
           
           
            HHGtma.Properties["TotalDie"] = 0;
            HHGtma.Properties["PassDie"] = 0;
            HHGtma.Properties["FailDie"] = 0;

            foreach (DieData die in HHGtma.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    HHGtma.Properties["FailDie"] = (int)HHGtma.Properties["FailDie"] + 1;
                else if (die.Attribute == DieCategory.PassDie)
                    HHGtma.Properties["PassDie"] = (int)HHGtma.Properties["PassDie"] + 1;
            }

            HHGtma.Properties["TotalDie"] = (int)HHGtma.Properties["FailDie"] + (int)HHGtma.Properties["PassDie"];

            HHGtma.Properties["Yield"] = (decimal)((int)HHGtma.Properties["PassDie"] / (int)HHGtma.Properties["TotalDie"]);


            // 保存文件
            HHGtma.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "HHGtma");

            // 创建新文件
            IMappingFile HHGtma = new HHGTma(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                HHGtma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            HHGtma.DieMatrix = source.DieMatrix.Clone();
            HHGtma.Properties["ColCount"] = HHGtma.DieMatrix.XMax;
            HHGtma.Properties["RowCount"] = HHGtma.DieMatrix.YMax;
            HHGtma.Properties["Yield"] = (decimal)((int)HHGtma.Properties["PassDie"] / (int)HHGtma.Properties["TotalDie"]);

            // 旋转角度
            HHGtma.DeasilRotate(convertConfig.Rotate);

            return HHGtma;
        }
    }
}
