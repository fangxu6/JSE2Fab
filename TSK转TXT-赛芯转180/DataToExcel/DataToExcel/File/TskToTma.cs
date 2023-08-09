
namespace DataToExcel
{
    using System;
    using System.Collections;

    public class TskToTma : ConverterBase
    {
        public override void Convert(string datfile, string tmafile)
        {
            // ��ȡ��Դ�ļ�
           // IMappingFile source = new Tsk(datfile);
            Dat source = new Dat(datfile); 
            source.Read();

            // ����ת������
            ConvertConfig convertConfig = new ConvertConfig("tsk", "tma");

            // �������ļ�
            IMappingFile tma = new Tma(tmafile);
           // Tma tma = new Tma(tmafile);

            // ��ȡӳ���ֶ�ֵ
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                tma.Properties[f.To] = source.Properties[f.From];
            }

            // ���� die �б�
            tma.DieMatrix = source.DieMatrix.Clone();

            tma.Properties["ColCount"] = tma.DieMatrix.XMax;
            tma.Properties["RowCount"] = tma.DieMatrix.YMax;

            if (source.FlatDir == 90)
            {
                // ��ת�Ƕ�
                tma.DeasilRotate(90);
            }

            if (source.FlatDir == 0)
            {
                // ��ת�Ƕ�
                tma.DeasilRotate(180);
            }

            if (source.FlatDir == 270)
            {
                // ��ת�Ƕ�
                tma.DeasilRotate(270);
            }

            tma.Properties["TotalDie"] = 0;
            tma.Properties["PassDie"] = 0;
            tma.Properties["FailDie"] = 0;

            foreach (DieData die in tma.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                     tma.Properties["FailDie"] =  (int)tma.Properties["FailDie"] + 1;
                else if (die.Attribute == DieCategory.PassDie)
                    tma.Properties["PassDie"] = (int)tma.Properties["PassDie"] + 1;
            }

            tma.Properties["TotalDie"] = (int)tma.Properties["FailDie"] + (int)tma.Properties["PassDie"];

            tma.Properties["Yield"] = (decimal)((int)tma.Properties["PassDie"] / (int)tma.Properties["TotalDie"]);


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
