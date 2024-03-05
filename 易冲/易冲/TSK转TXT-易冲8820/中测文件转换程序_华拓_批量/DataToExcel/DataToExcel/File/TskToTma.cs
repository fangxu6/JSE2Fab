
namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToTma : ConverterBase
    {
        public override void Convert(string datfile, string tmafile,int mapdeg)
        {
            // 读取来源文件
            IMappingFile source = new Tsk(datfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "tma");

            // 创建新文件
            IMappingFile tma = new Tma(tmafile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                tma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            tma.DieMatrix = source.DieMatrix.Clone();

            tma.Properties["ColCount"] = tma.DieMatrix.XMax;
            tma.Properties["RowCount"] = tma.DieMatrix.YMax;
            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);

            // 旋转角度
            tma.DeasilRotate(mapdeg);
            //tma.Properties["FlatDir"] = source.FlatDir;
            //tma.DeasilRotate(mapdeg);
            //tma.FlatDir = tma.FlatDir + mapdeg;
            //if (tma.FlatDir >= 360)
            //{
            //    tma.FlatDir = tma.FlatDir - 360;
            //}
            tma.Properties["TotalDie"] = 0;
            tma.Properties["PassDie"] = 0;
            tma.Properties["FailDie"] = 0;

            foreach (DieData die in tma.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    tma.Properties["FailDie"] = (int)tma.Properties["FailDie"] + 1;
                else if (die.Attribute == DieCategory.PassDie)
                    tma.Properties["PassDie"] = (int)tma.Properties["PassDie"] + 1;
            }

            tma.Properties["TotalDie"] = (int)tma.Properties["FailDie"] + (int)tma.Properties["PassDie"];

            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);


           

            // 保存文件
            tma.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "tma");

            // 创建新文件
            IMappingFile tma = new Tma(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                tma.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            tma.DieMatrix = source.DieMatrix.Clone();
            tma.Properties["ColCount"] = tma.DieMatrix.XMax;
            tma.Properties["RowCount"] = tma.DieMatrix.YMax;
            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);

            // 旋转角度
           // tma.DeasilRotate(convertConfig.Rotate);
        


            return tma;
        }
    }
}
