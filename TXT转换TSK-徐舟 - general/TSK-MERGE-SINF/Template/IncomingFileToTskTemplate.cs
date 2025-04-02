using DataToExcel;
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
        private List<string> txt_Name = new List<string>();
        private List<string> tsk_Name = new List<string>();

        protected int txtTotal = 0;
        protected int txtPass = 0;
        protected int txtFail = 0;
        protected int fullTxtPass = 0;
        protected int fullTxtFail = 0;
        protected List<string> txtData; //原始txt数据
        //protected List<string> DegtxtData; //旋转角度后的txt数据
        //protected List<string> txtFullData; //生成的txt数据
        //-----Sinf 头文件----//////
        protected string txtDevice;
        protected string txtLot;
        protected int txtSlot;
        protected string txtWaferID;
        protected string txtFlat;
        protected int txtRowct = 0;   //行数
        protected int txtColct = 0;   //列数
        protected int fullTxtMark = 0;

        public List<string> Txt_Name { get => txt_Name; set => txt_Name = value; }
        public List<string> Tsk_Name { get => tsk_Name; set => tsk_Name = value; }

        abstract public void ParseLine(string line);

        // The "Template Method"
        public void Run(Tsk tsk, string txtFile, string inkBinNoStr, string isPassAlignmentMarkDie)
        {
            //get txtData
            LoadTxt(txtFile);

            //waferID比对
            //if (Txt_Name.Count == Tsk_Name.Count)
            //{
            //    if (tsk.WaferID != this.txtWaferID)
            //    {
            //        if (MessageBox.Show("WaferID不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            Environment.Exit(0);
            //        }
            //    }
            //}

            //TXT图谱转角度
            List<string> DegtxtData = GetDegtxtData(tsk, txtData);
            //生成txt图谱数据
            string[,] TxtMap = new string[this.txtColct, this.txtRowct];
            for (int y = 0; y < this.txtRowct; y++)
            {
                for (int x = 0; x < this.txtColct; x++)
                {
                    TxtMap[x, y] = DegtxtData[x + y * txtColct];
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
            GetXYMinMax(tsk, ref xMin, ref yMin, ref xMax, ref yMax);

            //生成新的TxtMap
            if (txtRowct > yMax || txtColct > xMax)
            {
                xMin = 0;
                yMin = 0;
                xMax = txtColct - 1;
                yMax = txtRowct - 1;
            }
            //生成完整的TxtMap
            string[,] TxtFullMap = GetTxtFullMap(TxtMap, xMin, yMin, xMax, yMax, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);

            //生成新的TxtData
            List<string> TxtFullData = GetTxtFullData(TxtFullMap, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);
            //对位点比对工作
            int countPass = 0;
            int countFail = 0;
            int countMark = 0;

            CountPassAndFail(TxtFullMap, ref countPass, ref countFail, ref countMark);
            fullTxtPass = countPass;
            fullTxtFail = countFail;
            fullTxtMark = countMark;

            if (isPassAlignmentMarkDie.Equals("是"))
            {
                for (int y = 0; y < tsk.DieMatrix.YMax; y++)
                {
                    for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                    {
                        if (TxtFullMap[x, y].ToString() == "#" && TSKMap[x, y].ToString() != "#")
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

            //颗数比对
            //if ((this.txtPass + this.txtFail != (fullTxtPass + fullTxtFail))|| (this.txtPass + this.txtFail != tskTotalDie))
            //{
            //    if (MessageBox.Show("总颗数不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        Environment.Exit(0);
            //    }
            //}

            //根据SINF生成新的TSK-MAP
            if (string.IsNullOrEmpty(this.txtWaferID))
            {
                this.txtLot = tsk.LotNo;
                this.txtWaferID = tsk.WaferID;
            }
            string slotNo = getSlotNo(this.txtWaferID);
            tsk.FullName = "D:\\MERGE\\" + slotNo + "." + this.txtWaferID.TrimEnd('\0');
                

            int inkBinNo = Convert.ToInt32(inkBinNoStr);
            if (!tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (TxtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
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
                    if (TxtFullData[k].ToString() == ".")//Skip Die
                    {
                        continue;
                    }
                    else
                    {
                        if (Convert.ToInt32(tsk.MapVersion) == 2)
                        {
                            if (TxtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                            {
                                tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                                tsk.DieMatrix[k].Bin = inkBinNo;
                            }
                        }
                        else if (Convert.ToInt32(tsk.MapVersion) == 4 || Convert.ToInt32(tsk.MapVersion) == 7)
                        {
                            if (TxtFullData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
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

            if (tsk.FailDie != txtFail)
            {
                throw new Exception("FailDie count mismatch. tsk.FailDie: " + tsk.FailDie + ", txtFail: " + txtFail);
            }

            tsk.LotNo = this.txtLot;
            tsk.WaferID = this.txtWaferID;
            tsk.Save();

            printTxtTskPair(tsk.LotNo);
        }

        private Tsk ParseTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read();
            return tsk;
        }

        private void LoadTxt(string txtFile)
        {
            this.txtPass = 0;
            this.txtFail = 0;
            this.txtRowct = 0;
            this.txtColct = 0;
            FileStream txt_1;

            txt_1 = new FileStream(txtFile, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(txt_1, Encoding.Default);

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

            if (txtRowct == 0 || txtColct == 0)
            {
                // MessageBox.Show("SINF格式不正确!");
                if (MessageBox.Show("TXT格式不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
            txt_1.Close();
            read.Close();
        }


        static int ConvertCharToValue(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return c - 'A' + 10;
            }
            else if (c >= 'a' && c <= 'z')
            {
                return c - 'a' + 10 + 26;
            }
            else
            {
                return c - '0';
            }
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
            int count = txtColct * txtRowct;

            for (int i = 0; i < count; i++)
            {
                DegtxtData.Add(".");
            }

            if (!String.IsNullOrEmpty(this.txtFlat))
            {
                //int txtFlat1 = Convert.ToInt32(this.txtFlat);
                int txtFlat1 = GetFlat(this.txtFlat);
                int flatDifference = (tsk.FlatDir - txtFlat1 + 360) % 360;

                if (flatDifference == 180)////TXT转180
                {
                    int x = -1, y = -1, xr = -1, yr = -1;

                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            // 计算 x,y 坐标
                            // x = i % this._xmax;
                            x = i % txtColct;
                            // y = i / this._xmax;
                            y = i / txtColct;

                            xr = (txtColct) - 1 - x;
                            yr = (txtRowct) - 1 - y;

                            DegtxtData[yr * txtColct + xr] = txtData[i];
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
                        x = i % txtColct;
                        y = i / txtColct;

                        xr = y;
                        yr = (txtColct - 1) - x;

                        DegtxtData[yr * txtRowct + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = txtColct;
                    txtColct = txtRowct;
                    txtRowct = x;

                }

                else if (flatDifference == 90)////TXT转90
                {

                    int x = -1, y = -1, xr = -1, yr = -1;
                    for (int i = 0; i < count; i++)
                    {
                        // 计算 x,y 坐标
                        x = i % txtColct;
                        y = i / txtColct;

                        xr = (txtRowct - 1) - y;
                        yr = x;

                        DegtxtData[yr * txtRowct + xr] = txtData[i];
                    }

                    // 交换行数与列数
                    x = txtColct;
                    txtColct = txtRowct;
                    txtRowct = x;
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

        private void printTxtTskPair(string lotNo)
        {
            ////////////////////////////////输出TXT//////////////////////////////////
            FileStream fwt = new FileStream("D:\\MERGE\\" + lotNo + "_txt_with_tsk" + ".txt", FileMode.Create);
            StreamWriter swt = new StreamWriter(fwt);
            for (int ii = 0; ii < tsk_Name.Count; ii++)
            {
                swt.WriteLine(txt_Name[ii] + " " + tsk_Name[ii]);
            }
            swt.WriteLine();

            swt.Close();
            fwt.Close();
        }

        //private void ParseLine(string line)
        //{
        //    try
        //    {
        //        if (line.Contains(':') || line.Contains('='))
        //        {
        //            string[] strs = line.Split(new char[] { ':', '=' });
        //            string head = strs[0].Trim().ToUpper();
        //            string body = strs[1].Trim();
        //            if (string.IsNullOrEmpty(body))
        //            {
        //                return;
        //            }
        //            switch (head)
        //            {

        //                case "DEVICE":
        //                case "DEVICE NAME":
        //                    this.txtDevice = body;
        //                    break;
        //                case "LOT":
        //                case "LOT NO":
        //                    this.txtLot = body;
        //                    break;
        //                case "SLOT NO":
        //                    this.txtSlot = Convert.ToInt32(body); ;
        //                    break;
        //                case "WAFER":
        //                case "WAFER ID":
        //                case "WAFER-ID":
        //                    this.txtWaferID = body;
        //                    break;
        //                case "FNLOC":
        //                case "FLAT DIR":
        //                case "FLAT":
        //                    this.txtFlat = body;
        //                    break;
        //                case "ROWCT":
        //                    this.txtRowct = Convert.ToInt32(body);
        //                    break;
        //                case "COLCT":
        //                    this.txtColct = Convert.ToInt32(body);
        //                    break;
        //                case "PASS DIE":
        //                    this.txtPass = Convert.ToInt32(body);
        //                    break;
        //                case "FAIL DIE":
        //                    this.txtFail = Convert.ToInt32(body);
        //                    break;
        //                case "GROSS_DIES":
        //                case "TOTAL TEST DIE":
        //                    this.txtTotal = Convert.ToInt32(body);
        //                    break;

        //            }
        //        }
        //        else
        //        {
        //            this.ParseDies(line);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        throw ee;
        //    }
        //}

        abstract protected void ParseDies(string s);
        //{
        //    //PasrseDieWithDeviceGeneral(s);
        //    PasrseDieWithDeviceWTM2100COfZhiCun(s);
        //    //TODO null报错
        //    //if (this.txtDevice.Contains("IML7972"))
        //    //{
        //    //PasrseDieWithDeviceIML7972(s);
        //    //} else if (this.txtDevice.Contains("UPM7231"))
        //    //{
        //    //    PasrseDieWithDeviceUPM7231(s);
        //    //} else
        //    //{
        //    //    PasrseDieWithDeviceUPM6700(s);
        //    //}
        //}

        private void PasrseDieWithDeviceWTM2100COfZhiCun(string s)
        {
            if (s.Contains("|"))
            {
                string newLine = s.Substring(s.IndexOf("|") + 1);
                txtColct = newLine.Length / 3;
                txtRowct++;
                for (int i = 0; i < newLine.Length;)
                {

                    string binNo = newLine.Substring(i + 2, 1);
                    if (binNo.Equals("."))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("P"))
                    {
                        txtData.Add("0");
                        this.txtPass++;
                    }
                    else if (binNo.Equals("M"))//对位点比较
                    {
                        txtData.Add("#");
                    }
                    else
                    {
                        txtData.Add("X");
                        this.txtFail++;
                    }
                    i = i + 3;
                }
            }
        }
        private void PasrseDieWithDeviceIML7972(string s)
        {
            if (s.StartsWith(".") || s.StartsWith("S") || s.StartsWith("#"))
            {
                string newLine = s;
                txtColct = newLine.Length;
                txtRowct++;
                for (int i = 0; i < newLine.Length; i++)
                {
                    string binNo = newLine.Substring(i, 1);
                    if (binNo.Equals("."))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("S"))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("#"))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("1"))
                    {
                        txtData.Add("0");
                        this.txtPass++;
                    }
                    else
                    {
                        txtData.Add("X");
                        this.txtFail++;
                    }
                }
            }
        }

        //笑脸Device_General
        private void PasrseDieWithDeviceGeneral(string s)
        {
            if (s.StartsWith(".") || s.StartsWith("1") || s.StartsWith("S") || s.StartsWith("#"))
            {
                string newLine = s;
                txtColct = newLine.Length;
                txtRowct++;
                for (int i = 0; i < newLine.Length; i++)
                {
                    string binNo = newLine.Substring(i, 1);
                    if (binNo.Equals("."))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("#"))//对位点比较
                    {
                        txtData.Add("#");
                    }
                    else if (binNo.Equals("1"))
                    {
                        txtData.Add("0");
                        this.txtPass++;
                    }
                    else
                    {
                        txtData.Add(binNo);
                        this.txtFail++;
                    }
                }
            }
        }
        private void PasrseDieWithDeviceUPM7231(string s)
        {
            if (s.StartsWith("RowData"))
            {
                string newLine = s.Substring(s.IndexOf("RowData") + 7 + 1);
                for (int i = 0; i < newLine.Length;)
                {
                    string binNo = newLine.Substring(i, 2);
                    if (binNo.StartsWith("_"))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("00"))
                    {
                        txtData.Add("0");
                        this.txtPass++;
                    }
                    else if (binNo.Equals("@@"))//对位点比较
                    {
                        txtData.Add("#");
                    }
                    else
                    {
                        txtData.Add("X");
                        this.txtFail++;
                    }
                    i = i + 3;
                }
            }
        }
        private void PasrseDieWithDeviceUPM6700(string s)
        {
            //还缺少对位点
            if (s.StartsWith("RowData"))
            {
                string newLine = s.Substring(s.IndexOf("RowData") + 7 + 1);
                for (int i = 0; i < newLine.Length;)
                {
                    string binNo = newLine.Substring(i, 3);
                    if (binNo.StartsWith("_"))
                    {
                        txtData.Add(".");
                    }
                    else if (binNo.Equals("000"))
                    {
                        txtData.Add("0");
                        this.txtPass++;
                    }
                    else
                    {
                        txtData.Add("X");
                        this.txtFail++;

                    }
                    i = i + 4;
                }
            }
        }
        private string getSlotNo(string txtWaferID)
        {
            //F9N984-09F5根据-获取-后面的2位，
            string[] str = txtWaferID.Split('-');
            //str[1].Substring(0, 2) 3位，第一位补0
            if (str[1].Length==2)
                return "0" + str[1].Substring(0, 2);
            else
                return "00" +str[1].Substring(0, 1);
        }
    }
}