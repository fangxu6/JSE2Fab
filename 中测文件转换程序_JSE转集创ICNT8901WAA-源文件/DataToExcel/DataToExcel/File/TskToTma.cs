
namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToTma : ConverterBase
    {
        public override void Convert(string datfile, string tmafile)
        {
            // ��ȡ��Դ�ļ�
            IMappingFile source = new Tsk(datfile);
            source.Read();

            // ����ת������
            ConvertConfig convertConfig = new ConvertConfig("tsk", "tma");

            // �������ļ�
            IMappingFile tma = new Tma(tmafile);

            // ��ȡӳ���ֶ�ֵ
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                tma.Properties[f.To] = source.Properties[f.From];
            }

            // ���� die �б�
            tma.DieMatrix = source.DieMatrix.Clone();

            tma.Properties["ColCount"] = tma.DieMatrix.XMax;
            tma.Properties["RowCount"] = tma.DieMatrix.YMax;
            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);

            // ��ת�Ƕ�
            tma.DeasilRotate(convertConfig.Rotate);

            // �����ļ�
            tma.Save();
        }

        public override IMappingFile Convert(IMappingFile source)
        {
            // ����ת������
            ConvertConfig convertConfig = new ConvertConfig("tsk", "tma");

            // �������ļ�
            IMappingFile tma = new Tma(source.FileName);

            // ��ȡӳ���ֶ�ֵ
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                tma.Properties[f.To] = source.Properties[f.From];
            }

            // ���� die �б�
            tma.DieMatrix = source.DieMatrix.Clone();
            tma.Properties["ColCount"] = tma.DieMatrix.XMax;
            tma.Properties["RowCount"] = tma.DieMatrix.YMax;
            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);

            // ��ת�Ƕ�
            tma.DeasilRotate(convertConfig.Rotate);

            return tma;
        }
    }
}
