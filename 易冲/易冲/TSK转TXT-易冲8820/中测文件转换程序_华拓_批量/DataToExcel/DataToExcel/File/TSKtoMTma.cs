namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToMTma : ConverterBase
    {
        public override void Convert(string datfile, string Mtmafile,int mapdeg)
        {
            // 读取来源文件
            IMappingFile source = new Tsk(datfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "Mtma");

            // 创建新文件
            IMappingFile Mtma = new MTma(Mtmafile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                Mtma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            Mtma.DieMatrix = source.DieMatrix.Clone();

            Mtma.Properties["ColCount"] = Mtma.DieMatrix.XMax;
            Mtma.Properties["RowCount"] = Mtma.DieMatrix.YMax;
            Mtma.Properties["Yield"] = (decimal)((int)Mtma.Properties["PassDie"] / (int)Mtma.Properties["TotalDie"]);


            int newdeg=(int)Mtma.Properties["FlatDir"];
            // 旋转角度
            Mtma.DeasilRotate(mapdeg);
            Mtma.Properties["FlatDir"] =(int) Mtma.Properties["FlatDir"] + mapdeg;
            if ((int)Mtma.Properties["FlatDir"] >= 360)
            {
                Mtma.Properties["FlatDir"] = (int)Mtma.Properties["FlatDir"] - 360;
            }


            Mtma.Properties["TotalDie"] = 0;
            Mtma.Properties["PassDie"] = 0;
            Mtma.Properties["FailDie"] = 0;

            foreach (DieData die in Mtma.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    Mtma.Properties["FailDie"] = (int)Mtma.Properties["FailDie"] + 1;
                else if (die.Attribute == DieCategory.PassDie)
                    Mtma.Properties["PassDie"] = (int)Mtma.Properties["PassDie"] + 1;
            }

            Mtma.Properties["TotalDie"] = (int)Mtma.Properties["FailDie"] + (int)Mtma.Properties["PassDie"];

            Mtma.Properties["Yield"] = (decimal)((int)Mtma.Properties["PassDie"] / (int)Mtma.Properties["TotalDie"]);


            // 保存文件
            Mtma.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "Mtma");

            // 创建新文件
            IMappingFile Mtma = new MTma(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                Mtma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            Mtma.DieMatrix = source.DieMatrix.Clone();
            Mtma.Properties["ColCount"] = Mtma.DieMatrix.XMax;
            Mtma.Properties["RowCount"] = Mtma.DieMatrix.YMax;
            Mtma.Properties["Yield"] = (decimal)((int)Mtma.Properties["PassDie"] / (int)Mtma.Properties["TotalDie"]);

            // 旋转角度
            Mtma.DeasilRotate(convertConfig.Rotate);

            return Mtma;
        }
    }
}
