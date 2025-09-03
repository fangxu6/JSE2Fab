
/*
 * 作者：sky
 * 时间：2008-06-25
 * 作用：CMD 要求，将 tsk 格式转换成 txt 格式
 */

namespace DataToExcel
{
    using DataToExcel.ExpDataToExcelFactory;

    public class CMDTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile)
        {
            // 读取来源文件
            Tsk source = new Tsk(tskfile);  
            source.Read();

            ExpToExcelSoftBin expToExcelSoftBin = ExpToExcelSoftBinFactory.GetExpToExcelSoft(source.Device);
            //if (expToExcelSoftBin != null)
            //{
            //    if (!expToExcelSoftBin.defatultBinPlusOne())
            //    {
            //        //bin -1
            //        source.DieMatrix = source.DieMatrix.CloneWithMinusOne();
            //    }
            //}
            expToExcelSoftBin.SaveToTxt(source, txtfile);

            
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
