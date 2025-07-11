﻿using DataToExcel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TSK_MERGE_SINF.Template
{
    public abstract class IncomingFileToTskTemplate
    {
        private List<string> _txtName = new List<string>();
        private List<string> _tskName = new List<string>();

        protected int TxtTotal = 0;
        protected int TxtPass = 0;
        protected int TxtFail = 0;
        protected int FullTxtPass = 0;
        protected int FullTxtFail = 0;
        protected List<string> txtData; //原始txt数据
        //protected List<string> DegtxtData; //旋转角度后的txt数据
        //protected List<string> txtFullData; //生成的txt数据
        //-----Sinf 头文件----//////
        protected string TxtDevice;
        protected string TxtLot;
        protected int TxtSlot;
        protected string TxtWaferId;
        protected string TxtFlat;
        protected int TxtRowCount = 0;   //行数
        protected int TxtColCount = 0;   //列数
        protected int FullTxtMark = 0;

        public List<string> TxtName { get => _txtName; set => _txtName = value; }
        public List<string> TskName { get => _tskName; set => _tskName = value; }

        public abstract void ParseLine(string line);

        // The "Template Method"
        public void Run(Tsk tsk, string txtFile, string tskFile, string inkBinNoStr, string isPassAlignmentMarkDie, string isWaferIdCompare)
        {
            //get txtData
            LoadTxt(txtFile);

            if (isWaferIdCompare.Equals("是"))
            {
                if (tsk.WaferID != this.TxtWaferId)
                {
                    if (MessageBox.Show("WaferID不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Environment.Exit(0);
                    }
                }
            }

            //TXT图谱转角度degTxtData
            List<string> degTxtData = GetDegtxtData(tsk, txtData);
            //生成txt图谱数据
            string[,] TxtMap = new string[this.TxtColCount, this.TxtRowCount];
            for (int y = 0; y < this.TxtRowCount; y++)
            {
                for (int x = 0; x < this.TxtColCount; x++)
                {
                    TxtMap[x, y] = degTxtData[x + y * TxtColCount];
                }
            }
            //TXT图谱补边工作
            //获取tskmap
            string[,] TSKMap = CreateTskMap(tsk);

            //获取tsk的边缘
            int xMin = Int32.MaxValue;
            int yMin = Int32.MaxValue;
            int xMax = Int32.MinValue;
            int yMax = Int32.MinValue;
            if (isPassAlignmentMarkDie.Equals("是"))
            {
                GetXYMinMax(tsk, ref xMin, ref yMin, ref xMax, ref yMax);
            }
            else
            {
                GetXYMinMax(tsk, ref xMin, ref yMin, ref xMax, ref yMax, true);
            }


            //生成新的TxtMap
            if (TxtRowCount > yMax || TxtColCount > xMax)
            {
                xMin = 0;
                yMin = 0;
                xMax = TxtColCount - 1;
                yMax = TxtRowCount - 1;
            }
            //生成完整的TxtMap
            string[,] txtFullMap = GetTxtFullMap(TxtMap, xMin, yMin, xMax, yMax, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);

            //生成新的TxtData
            List<string> txtFullData = GetTxtFullData(txtFullMap, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);
            //对位点比对工作
            int countPass = 0;
            int countFail = 0;
            int countMark = 0;

            CountPassAndFail(txtFullMap, ref countPass, ref countFail, ref countMark);
            FullTxtPass = countPass;
            FullTxtFail = countFail;
            FullTxtMark = countMark;

            if (isPassAlignmentMarkDie.Equals("是"))
            {
                for (int y = 0; y < tsk.DieMatrix.YMax; y++)
                {
                    for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                    {
                        if (txtFullMap[x, y].ToString() == "#" && TSKMap[x, y].ToString() != "#")
                            if (MessageBox.Show("对位点不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                Environment.Exit(0);
                    }
                }
            }
            int tskTotalDie = 0;
            foreach (DieData die in tsk.DieMatrix.Items)
            {
                if(die.Attribute == DieCategory.PassDie|| die.Attribute == DieCategory.FailDie|| die.Attribute == DieCategory.NoneDie)
                {
                    tskTotalDie++;
                }
            }

            if ((FullTxtPass + FullTxtFail) != tskTotalDie)
            {
                if (MessageBox.Show("总颗数不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }

            //根据SINF生成新的TSK-MAP
            if (string.IsNullOrEmpty(this.TxtWaferId))
            {
                this.TxtLot = tsk.LotNo;
                this.TxtWaferId = tsk.WaferID;
            }
            string slotNo = getSlotNo(this.TxtWaferId);
            tsk.FullName = "D:\\MERGE\\" + slotNo + "." + this.TxtWaferId.TrimEnd('\0');
                

            int inkBinNo = Convert.ToInt32(inkBinNoStr);
            if (!tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (txtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                    {
                        tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                        tsk.DieMatrix[k].Bin = inkBinNo;
                    }
                }
            }

            if (tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (txtFullData[k].ToString() == ".")//Skip Die
                    {
                        continue;
                    }
                    else
                    {
                        if (Convert.ToInt32(tsk.MapVersion) == 2)
                        {
                            if (txtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                            {
                                tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                                tsk.DieMatrix[k].Bin = inkBinNo;
                            }
                        }
                        else if (Convert.ToInt32(tsk.MapVersion) == 4 || Convert.ToInt32(tsk.MapVersion) == 7)
                        {
                            if (txtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                            {
                                tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                                tsk.DieMatrix[k].Bin = inkBinNo;
                            }
                        }
                    }
                }
            }

            tsk.PassDie = 0;
            tsk.FailDie = 0;
            for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
            {
                if (tsk.DieMatrix[k].Attribute == DieCategory.PassDie)
                {
                    tsk.PassDie++;
                }
                else if (tsk.DieMatrix[k].Attribute == DieCategory.FailDie)
                {
                    tsk.FailDie++;
                }
            }
            tsk.TotalDie = tsk.PassDie + tsk.FailDie;

            tsk.LotNo = this.TxtLot;
            tsk.WaferID = this.TxtWaferId;
            tsk.Save();

            PrintTxtTskPair(tsk.LotNo, txtFile, tskFile);
        }

        private void LoadTxt(string txtFile)
        {
            this.TxtPass = 0;
            this.TxtFail = 0;
            this.TxtRowCount = 0;
            this.TxtColCount = 0;
            FileStream txt1 = new FileStream(txtFile, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(txt1, Encoding.Default);

            if (this.txtData == null)
            {
                this.txtData = new List<string>();
            }
            else
            {
                this.txtData.Clear();
            }
            while (true)
            {

                string line = read.ReadLine();
                if (line != null)
                {
                    this.ParseLine(line);//每家客户的来料文件不同 所以parse方法也不同
                }
                else
                { break; }
            }

            if (TxtRowCount == 0 || TxtColCount == 0)
            {
                // MessageBox.Show("SINF格式不正确!");
                if (MessageBox.Show("TXT格式不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
            txt1.Close();
            read.Close();
        }

        /// <summary>
        /// 计算TxtNewMap pass、fail和mark颗数
        /// </summary>
        /// <param name="TxtNewMap"></param>
        /// <param name="countPass"></param>
        /// <param name="countFail"></param>
        /// <param name="countMark"></param>
        private static void CountPassAndFail(string[,] TxtNewMap, ref int countPass, ref int countFail, ref int countMark)
        {
            int xMaxCordinate = TxtNewMap.GetLength(0);
            int yMaxCordinate = TxtNewMap.GetLength(1);
            for (int y = 0; y < yMaxCordinate; y++)
            {
                for (int x = 0; x < xMaxCordinate; x++)
                {
                    if (TxtNewMap[x, y].ToString() == "0")
                    {
                        countPass++;
                    }
                    if (TxtNewMap[x, y].ToString() == "X")
                    {
                        countFail++;
                    }
                    if (TxtNewMap[x, y].ToString() == "M")
                    {
                        countMark++;
                    }
                }
            }
        }

        private List<string> GetTxtFullData(string[,] TxtFullMap, int xMax, int yMax)
        {
            //if (this.txtFullData == null)
            //{
            //    this.txtFullData = new List<string>();
            //}
            //else
            //{
            //    this.txtFullData.Clear();
            //}
            List<string> txtFullData = new List<string>();

            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    txtFullData.Add(TxtFullMap[x, y].ToString());
                }
            }
            return txtFullData;
        }

        private static string[,] GetTxtFullMap(string[,] TxtMap, int xMin, int yMin, int xMax, int yMax, int xMaxCordinate, int yMaxCordinate)
        {
            string[,] TxtNewMap = new string[xMaxCordinate, yMaxCordinate];
            for (int y = 0; y < yMaxCordinate; y++)
            {
                for (int x = 0; x < xMaxCordinate; x++)
                {
                    TxtNewMap[x, y] = ".";
                }
            }

            for (int y = yMin; y <= yMax; y++)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    TxtNewMap[x, y] = TxtMap[x - xMin, y - yMin];
                }
            }
            return TxtNewMap;
        }

        private static void GetXYMinMax(Tsk tsk, ref int xMin, ref int yMin, ref int xMax, ref int yMax, bool isPassAlignmentMarkDie)
        {
            for (int y = 0; y < tsk.DieMatrix.YMax; y++)
            {
                for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                {

                    switch (tsk.DieMatrix[x, y].Attribute)
                    {
                        case DieCategory.PassDie:
                        case DieCategory.NoneDie:
                        case DieCategory.FailDie:
                            if (xMin > x) { xMin = x; }
                            if (yMin > y) { yMin = y; }
                            if (yMax < y) { yMax = y; }
                            if (xMax < x) { xMax = x; }
                            break;
                    }
                }
            }
        }
        private static void GetXYMinMax(Tsk tsk, ref int xMin, ref int yMin, ref int xMax, ref int yMax)
        {
            for (int y = 0; y < tsk.DieMatrix.YMax; y++)
            {
                for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                {

                    switch (tsk.DieMatrix[x, y].Attribute)
                    {
                        case DieCategory.PassDie:
                        case DieCategory.NoneDie:
                        case DieCategory.FailDie:
                        case DieCategory.SkipDie2:
                            if (xMin > x) { xMin = x; }
                            if (yMin > y) { yMin = y; }
                            if (yMax < y) { yMax = y; }
                            if (xMax < x) { xMax = x; }
                            break;
                    }
                }
            }
        }

        private static string[,] CreateTskMap(Tsk tsk)
        {
            //理解的不对
            //int row1_1 = tsk.Rows;  //tsk的行和列和常规的反了 size of horizontal  水平方向  x轴方向的最大值
            //int col1_1 = tsk.Cols;  //tsk的行和列和常规的反了 size of verticatl   垂直方向  y轴方向的最大值
            string[,] TSKMap = new string[tsk.DieMatrix.XMax, tsk.DieMatrix.YMax];

            //生成TSKMap
            for (int y = 0; y < tsk.DieMatrix.YMax; y++)
            {
                for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                {
                    switch (tsk.DieMatrix[x, y].Attribute)
                    {
                        case DieCategory.PassDie:
                        case DieCategory.FailDie:
                            TSKMap[x, y] = "1";
                            break;
                        case DieCategory.SkipDie2:
                            TSKMap[x, y] = "#";
                            break;
                        default:
                            TSKMap[x, y] = ".";
                            break;
                    }
                }
            }
            return TSKMap;
        }

        private List<string> GetDegtxtData(Tsk tsk, List<string> txtData)
        {
            List<string> DegtxtData = new List<string>();
            //if (this.DegtxtData == null)
            //{
            //    this.DegtxtData = new List<string>();
            //}
            //else
            //{
            //    this.DegtxtData.Clear();
            //}
            int count = TxtColCount * TxtRowCount;

            for (int i = 0; i < count; i++)
            {
                DegtxtData.Add(".");
            }

            if (!String.IsNullOrEmpty(this.TxtFlat))
            {
                //int txtFlat1 = Convert.ToInt32(this.TxtFlat);
                int flat = GetFlat(this.TxtFlat);
                int flatDifference = (tsk.FlatDir - flat + 360) % 360;

                if (flatDifference == 180)////TXT转180
                {
                    int x = -1, y = -1, xr = -1, yr = -1;

                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            // 计算 x,y 坐标
                            // x = i % this._xmax;
                            x = i % TxtColCount;
                            // y = i / this._xmax;
                            y = i / TxtColCount;

                            xr = (TxtColCount) - 1 - x;
                            yr = (TxtRowCount) - 1 - y;

                            DegtxtData[yr * TxtColCount + xr] = txtData[i];
                        }
                        catch (Exception ee)
                        {
                            string msg = ee.Message;
                        }
                    }
                }

                else if (flatDifference == 270)////TXT转270
                {

                    int x = -1, y = -1, xr = -1, yr = -1;

                    for (int i = 0; i < count; i++)
                    {
                        // 计算 x,y 坐标
                        x = i % TxtColCount;
                        y = i / TxtColCount;

                        xr = y;
                        yr = (TxtColCount - 1) - x;

                        DegtxtData[yr * TxtRowCount + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = TxtColCount;
                    TxtColCount = TxtRowCount;
                    TxtRowCount = x;

                }

                else if (flatDifference == 90)////TXT转90
                {

                    int x = -1, y = -1, xr = -1, yr = -1;
                    for (int i = 0; i < count; i++)
                    {
                        // 计算 x,y 坐标
                        x = i % TxtColCount;
                        y = i / TxtColCount;

                        xr = (TxtRowCount - 1) - y;
                        yr = x;

                        DegtxtData[yr * TxtRowCount + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = TxtColCount;
                    TxtColCount = TxtRowCount;
                    TxtRowCount = x;
                }
                else //TXT不转角度
                {

                    for (int i = 0; i < count; i++)
                    {

                        DegtxtData[i] = txtData[i];
                    }

                }
            }
            else //TXT不转角度
            {

                for (int i = 0; i < count; i++)
                {

                    DegtxtData[i] = txtData[i];
                }

            }
            return DegtxtData;
        }

        protected abstract int GetFlat(string txtFlat);

        private void PrintTxtTskPair(string lotNo, string txtFile, string tskFile)
        {
            ////////////////////////////////输出TXT//////////////////////////////////
            FileStream fwt = new FileStream("D:\\MERGE\\" + lotNo + "_txt_with_tsk" + ".txt", FileMode.Append);
            StreamWriter swt = new StreamWriter(fwt);
            swt.WriteLine(txtFile + " " + tskFile);

            swt.Close();
            fwt.Close();
        }

        protected abstract void ParseDies(string s);

        // todo  :要考虑各种去情况
        private string getSlotNo(string txtWaferID)
        {
            string slotNo = "";
            //01
            if (!txtWaferID.Contains("-"))
                slotNo = txtWaferID;
            else
                slotNo = txtWaferID.Split('-')[1];
            //str[1].Substring(0, 2) 3位，第一位补0
            if (slotNo.Length==2)
                return "0" + slotNo.Substring(0, 2);
            else
                return "00" + slotNo.Substring(0, 1);
        }
    }
}