using System;
using System.Collections.Generic;
using DataToExcel;
using System.Data;
using System.Windows.Forms;

public class TskDataProcessor
{
    private readonly ProgressBar _progressBar;

    public TskDataProcessor(ProgressBar progressBar)
    {
        _progressBar = progressBar;
    }

    /// <summary>
    /// ����Excel����������ļ��ָ�TSK�ļ�
    /// </summary>
    /// <param name="tsk">��ʼtsk�ļ�</param>
    /// <param name="table">excel��DataTable</param>
    public void ProcessFromExcelData(Tsk tsk, DataTable table)
    {
        InitializeProgressBar(tsk);
        var binNoMap = CreateBinNoMap(table);
        UpdateTskMatrixFromExcelData(tsk, binNoMap);
        UpdateTskStatistics(tsk);
    }

    private void InitializeProgressBar(Tsk tsk)
    {
        if (_progressBar != null)
        {
            _progressBar.Maximum = tsk.Rows * tsk.Cols;
            _progressBar.Value = 0;
        }
    }

    private Dictionary<(int, int), int> CreateBinNoMap(DataTable table)
    {
        var binNoMap = new Dictionary<(int, int), int>();
        foreach (DataRow row in table.Rows)
        {
            if (row[0] is DBNull || row[1] is DBNull || row[2] is DBNull)
                continue;

            int x = Convert.ToInt32(row[0]);
            int y = Convert.ToInt32(row[1]);
            int binNo = Convert.ToInt32(row[2]);
            binNoMap[(x, y)] = binNo;
        }
        return binNoMap;
    }

    private void UpdateTskMatrixFromExcelData(Tsk tsk, Dictionary<(int, int), int> binNoMap)
    {
        for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
        {
            if (_progressBar != null)
                _progressBar.Value++;

            DieData die = tsk.DieMatrix[k];
            if (binNoMap.TryGetValue((die.X, die.Y), out int binNo))
            {
                die.Bin = binNo;
                die.Attribute = binNo == 1 ? DieCategory.PassDie : DieCategory.FailDie;
            }
        }
    }

    private void UpdateTskStatistics(Tsk tsk)
    {
        tsk.PassDie = 0;
        tsk.FailDie = 0;
        for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
        {
            if (tsk.DieMatrix[k].Attribute == DieCategory.PassDie)
                tsk.PassDie++;
            else if (tsk.DieMatrix[k].Attribute == DieCategory.FailDie)
                tsk.FailDie++;
        }
        tsk.TotalDie = tsk.PassDie + tsk.FailDie;
    }

    /// <summary>
    /// ����Excel Map�ļ��ָ�TSK�ļ�
    /// </summary>
    /// <param name="tsk">��ʼtsk�ļ�</param>
    /// <param name="table">excel��DataTable</param>
    internal void ProcessFromExcelMap(Tsk tsk, DataTable table)
    {
        InitializeProgressBar(tsk);
        UpdateTskMatrixFromExcelMap(tsk, table);
        UpdateTskStatistics(tsk);
    }

    private void UpdateTskMatrixFromExcelMap(Tsk tsk, DataTable table)
    {
        if (table.Rows.Count != tsk.Cols)
        {
            MessageBox.Show("Excel�ļ�������TSK�ļ�������ƥ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (table.Columns.Count !=tsk.Rows)
        {
            MessageBox.Show("Excel�ļ�������TSK�ļ�������ƥ��", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        for (int i = 0; i < tsk.Cols; i++)
        {
            DataRow row = table.Rows[i];
            for (int j = 0; j < tsk.Rows; j++)
            {
                if (_progressBar != null)
                    _progressBar.Value++;
                if (row[j] is DBNull) // ��������
                    continue;

                // ��ȡ��Ԫ���ԭʼֵ
                string cellValue = row[j].ToString().Trim();

                DieData die = tsk.DieMatrix[i * tsk.Rows + j];

                // ��������������ַ�"0"
                if (cellValue == "0")
                {
                    die.Bin = 61;
                    die.Attribute = DieCategory.FailDie;
                }
            }
        }

        //// ���Excel������������TSK��Die��������ʣ���Die��Ϊ�ջ�Ĭ��ֵ
        //for (int k = rowCount; k < tsk.Rows * tsk.Cols; k++)
        //{
        //    if (_progressBar != null)
        //        _progressBar.Value++;

        //    DieData die = tsk.DieMatrix[k];
        //    die.Bin = -1;
        //    die.Attribute = DieCategory.Unknow;
        //}
    }
}
