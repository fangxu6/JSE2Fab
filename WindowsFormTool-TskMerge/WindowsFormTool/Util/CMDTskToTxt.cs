
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

    public class CMDTskToTxt : ConverterBase
    {
        public override void Convert(string tskfile, string txtfile)
        {
            // ��ȡ��Դ�ļ�
            Dat source = new Dat(tskfile);  //zjf 2008.09.03
            //IMappingFile source = new Dat(tskfile);
            source.Read();

            // ����ת������
            ConvertConfig convertConfig = new ConvertConfig("tsk", "cmdtxt");

            // �������ļ�
            CmdTxt cmdtxt = new CmdTxt(txtfile);

            // ��ȡӳ���ֶ�ֵ
            foreach (ConvertConfig.ConvertField f in convertConfig.Fields)
            {
                cmdtxt.Properties[f.To] = source.Properties[f.From];
            }

            // ���� die �б�
            cmdtxt.DieMatrix = source.DieMatrix.Clone();
            cmdtxt.RowCount = cmdtxt.DieMatrix.YMax;
            cmdtxt.ColCount = cmdtxt.DieMatrix.XMax;

            //zjf 2008.09.03
            //begin
            cmdtxt.Device = source.Device;
            cmdtxt.LotNo = source.LotNo;
            cmdtxt.WaferID = source.WaferID;
            cmdtxt.WaferSize = source.WaferSize;
            cmdtxt.TotalDie = source.TotalDie;
            cmdtxt.IndexSizeX = source.IndexSizeX;
            cmdtxt.IndexSizeY = source.IndexSizeY;
            cmdtxt.LoadTime = source.LoadTime;
            cmdtxt.UnloadTime = source.UnloadTime;
            cmdtxt.StartTime = source.StartTime;
            cmdtxt.EndTime = source.EndTime;
            cmdtxt.SlotNo = source.SlotNo;
            cmdtxt.FlatDir = source.FlatDir;
            //end

            // ���¼���ͳ������
            cmdtxt.PassDie = 0;
            cmdtxt.FailDie = 0;

            foreach (DieData die in cmdtxt.DieMatrix.Items)
            {
                if (die.Attribute == DieCategory.FailDie)
                    cmdtxt.FailDie += 1;
                else if (die.Attribute == DieCategory.PassDie)
                    cmdtxt.PassDie += 1;
            }

            // ��ת�Ƕ�
            cmdtxt.DeasilRotate(convertConfig.Rotate);

            // �����ļ�
            cmdtxt.Save();
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
