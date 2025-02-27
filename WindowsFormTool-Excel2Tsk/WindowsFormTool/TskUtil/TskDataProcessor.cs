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
    /// 根据Excel里面的数据文件恢复TSK文件
    /// </summary>
    /// <param name="tsk">初始tsk文件</param>
    /// <param name="table">excel的DataTable</param>
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
    /// 根据Excel Map文件恢复TSK文件
    /// </summary>
    /// <param name="tsk">初始tsk文件</param>
    /// <param name="table">excel的DataTable</param>
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
            MessageBox.Show("Excel文件行数与TSK文件行数不匹配", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (table.Columns.Count !=tsk.Rows)
        {
            MessageBox.Show("Excel文件列数与TSK文件列数不匹配", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        for (int i = 0; i < tsk.Cols; i++)
        {
            DataRow row = table.Rows[i];
            for (int j = 0; j < tsk.Rows; j++)
            {
                if (_progressBar != null)
                    _progressBar.Value++;
                if (row[j] is DBNull) // 跳过空行
                    continue;

                // 获取单元格的原始值
                string cellValue = row[j].ToString().Trim();

                DieData die = tsk.DieMatrix[i * tsk.Rows + j];

                // 处理特殊情况：字符"0"
                if (cellValue == "0")
                {
                    die.Bin = 61;
                    die.Attribute = DieCategory.FailDie;
                }
            }
        }

        //// 如果Excel数据行数少于TSK的Die数量，将剩余的Die设为空或默认值
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
