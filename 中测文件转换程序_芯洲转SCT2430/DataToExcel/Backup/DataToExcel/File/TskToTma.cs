
namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToTma : ConverterBase
    {
        public override void Convert(string datfile, string tmafile)
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
            tma.DeasilRotate(convertConfig.Rotate);

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
            tma.DeasilRotate(convertConfig.Rotate);

            return tma;
        }
    }
}
