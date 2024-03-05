namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToSinf : ConverterBase
    {
        public override void Convert(string datfile, string sinffile)
        {
            // 读取来源文件
            IMappingFile source = new Tsk(datfile);
            source.Read();

            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "sinf");

            // 创建新文件
            IMappingFile sinf = new Sinf(sinffile);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                sinf.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            sinf.DieMatrix = source.DieMatrix.Clone();

            sinf.Properties["ColCount"] = sinf.DieMatrix.XMax;
            sinf.Properties["RowCount"] = sinf.DieMatrix.YMax;


            // 旋转角度
            sinf.DeasilRotate(convertConfig.Rotate);

            sinf.Properties["TotalDie"] = 0;
            sinf.Properties["PassDie"] = 0;
            sinf.Properties["FailDie"] = 0;

            foreach (DieData die in sinf.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    sinf.Properties["FailDie"] = (int)sinf.Properties["FailDie"] + 1;
                else if (die.Attribute == DieCategory.PassDie)
                    sinf.Properties["PassDie"] = (int)sinf.Properties["PassDie"] + 1;
            }

            sinf.Properties["TotalDie"] = (int)sinf.Properties["FailDie"] + (int)sinf.Properties["PassDie"];

            sinf.Properties["Yield"] = (decimal)((int)sinf.Properties["PassDie"] / (int)sinf.Properties["TotalDie"]);


            // 保存文件
            sinf.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // 加载转换配置
            ConvertConfig convertConfig = new ConvertConfig("tsk", "sinf");

            // 创建新文件
            IMappingFile sinf = new Tma(source.FileName);

            // 读取映射字段值
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                sinf.Properties[f.To] = source.Properties[f.From];
            }

            // 导入 die 列表
            sinf.DieMatrix = source.DieMatrix.Clone();
            sinf.Properties["ColCount"] = sinf.DieMatrix.XMax;
            sinf.Properties["RowCount"] = sinf.DieMatrix.YMax;
            sinf.Properties["Yield"] = (decimal)((int)sinf.Properties["PassDie"] / (int)sinf.Properties["TotalDie"]);

            // 旋转角度
            sinf.DeasilRotate(convertConfig.Rotate);

            return sinf;
        }
    }
}

