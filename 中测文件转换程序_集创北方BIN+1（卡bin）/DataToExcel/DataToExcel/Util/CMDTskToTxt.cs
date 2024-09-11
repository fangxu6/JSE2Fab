
/*
 * ���ߣ�sky
 * ʱ�䣺2008-06-25
 * ���ã�CMD Ҫ�󣬽� tsk ��ʽת���� txt ��ʽ
 */

namespace DataToExcel
{
    using System;
    using System.Collections;
    using DataToExcel;
    using DataToExcel.ExpDataToExcelFactory;

    public class CMDTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile)
        {
            // ��ȡ��Դ�ļ�
            Dat source = new Dat(tskfile);  
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
            // ����ת������
            ConvertConfig convertConfig = new ConvertConfig("tsk", "cmdtxt");

            // �������ļ�
            CmdTxt cmdtxt = new CmdTxt(source.FileName);

            // ��ȡӳ���ֶ�ֵ
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                cmdtxt.Properties[f.To] = source.Properties[f.From];
            }

            // ���� die �б�
            cmdtxt.DieMatrix = source.DieMatrix.Clone();
            cmdtxt.RowCount = cmdtxt.DieMatrix.YMax;
            cmdtxt.ColCount = cmdtxt.DieMatrix.XMax;

            // ��ת�Ƕ�
            cmdtxt.DeasilRotate(convertConfig.Rotate);

            return cmdtxt;
        }
    }
}
