using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Drawing.Drawing2D;
using DataToExcel;
using System.Threading.Tasks;

namespace TSK_MERGE_SINF
{
    public partial class Form1 : Form
    {
        // Fields
        private IMappingFile _currFile;
        private FieldsProp Field;
        private ArrayList FieldsArray;
        private string FilePath = Application.StartupPath;
        private string LotNo;
        private string ResultFileName;
        private string TskFile;
        private string Device;
        private int waferNum;

        List<string> txt_Name = new List<string>();
        List<string> tsk_Name  = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        int txtTotal = 0;
        int txtPass = 0;
        int txtFail = 0;
        int tskPass = 0;
        int tskFail = 0;

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                this.LoadTSKFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void LoadTSKFile()
        {
            this.LoadTSK();
        }

        private void LoadTSK()
        {
            tsk_Name.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = dialog.SelectedPath;
                //this.textBox1.Text = @"C:\Users\fangx\Desktop\卢浩楠合图\tsk";
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox1.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    tsk_Name.Add(str.FullName);
                }
            }

            if (txt_Name.Count != tsk_Name.Count)
            {
                MessageBox.Show("图谱数量不对应");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.LoadSINFFile();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void LoadSINFFile()
        {
            this.LoadSINF();
        }

        private void LoadSINF()
        {
            txt_Name.Clear();
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = dialog.SelectedPath;
                //this.textBox2.Text = @"C:\Users\fangx\Desktop\卢浩楠合图\txt";//TODO 优化
                DirectoryInfo TheFolder = new DirectoryInfo(this.textBox2.Text);

                foreach (FileInfo str in TheFolder.GetFiles("*", SearchOption.AllDirectories))
                {
                    txt_Name.Add(str.FullName);
                }
            }
        }

        List<string> txtData; //原始txt数据
        List<string> DegtxtData; //旋转角度后的txt数据
        List<string> txtNewData; //生成的txt数据
        //-----Sinf 头文件----//////
        string txtDevice;
        string txtLot;
        int txtSlot;
        string txtWaferID;
        string txtFlat;
        int txtRowct = 0;   //行数
        int txtColct = 0;   //列数

        int txtMark = 0;

        //---------------///////



        private void button3_Click(object sender, EventArgs e)
        {

            if (this.textBox2.Text == "")
            {
                MessageBox.Show("请选择txt图谱");
            }

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请选择TSK图谱");
            }
            
            for (int i = 0; i < tsk_Name.Count; i++)
            {
                string txtFile = txt_Name[i];
                string tskFile = tsk_Name[i];
                Txt2Tsk(txtFile, tskFile);
            }
            if (MessageBox.Show("转换成功，是否打开?", "确定", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start("D:\\MERGE\\");
            }

        }

        private Tsk LoadTsk(string tskFile)
        {
            Tsk tsk = new Tsk(tskFile);
            tsk.Read(); //版本2和4的拓展还是没有体现进binNo
            //this.LotNo = tsk.LotNo.Trim();
            return tsk;
        }

        private void LoadTxt(string txtFile)
        {
            this.txtPass = 0;
            this.txtFail = 0;
            //Tma2 tma = new Tma2(txtFile);
            //tma.Read(); //版本2和4的拓展还是没有体现进binNo

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
                    this.Parse(line);//每家客户的来料文件不同 所以parse方法也不同
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

        private void Txt2Tsk(string txtFile, string tskFile)
        {
            txtRowct = 0;
            txtColct = 0;

            ///////-------------------------------TSK读取-------------------------//////
            Tsk tsk = LoadTsk(tskFile);

            //------------------------------TXT读取------------------------------//
            //get txtData
            LoadTxt(txtFile);


            //-------------------------------------TXT图谱转角度---------------------------//
            GetDegtxtData(tsk, txtData);



            //生成txt图谱数据
            string[,] TxtMap = new string[ this.txtColct, this.txtRowct];// 76行 70列
            for (int y = 0; y < this.txtRowct; y++)
            {
                for (int x = 0; x < this.txtColct; x++)
                {
                    TxtMap[x, y] = DegtxtData[x + y * txtColct];
                }
            }


            ///////------------------------------TXT图谱补边工作---------------------------//
            //获取tskmap
            string[,] TSKMap = CreateTskMap(tsk);

            //获取tsk的边缘
            int xMin = Int32.MaxValue;
            int yMin = Int32.MaxValue;
            int xMax = Int32.MinValue;
            int yMax = Int32.MinValue;
            GetXYMinMax(tsk, ref xMin, ref yMin, ref xMax, ref yMax);

            //生成新的TxtMap
            if(txtRowct> yMax || txtColct > xMax)
            {
                xMin = 0;
                yMin = 0;
                xMax = txtColct-1;
                yMax = txtRowct-1;
            }
            string[,] TxtNewMap = GetNewTxtMap(TxtMap, xMin, yMin, xMax, yMax, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);

            //生成新的TxtData
            GetNewTxtData(TxtNewMap, tsk.DieMatrix.XMax, tsk.DieMatrix.YMax);
            ///////////////////////////对位点比对工作//////////////////////////////////////////////////

            
            int countPass = 0;
            int countFail = 0;
            int countMark = 0;
            CountPassAndFail(tsk, TxtNewMap, ref countPass, ref countFail, ref countMark);
            tskPass = countPass;//TODO 这个需要优化
            tskFail = countFail;
            txtMark = countMark;
            for (int y = 0; y < tsk.DieMatrix.YMax; y++)
            {
                for (int x = 0; x < tsk.DieMatrix.XMax; x++)
                {
                    if (TxtNewMap[x, y].ToString() == "#" && TSKMap[x, y].ToString() != "#")
                    {
                        if (MessageBox.Show("对位点不正确!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }


            //////////////////////////////PASS数比对///////////////////////////////////////

            if (this.txtPass + this.txtFail != (tskPass + tskFail))//12979 84  12811 77 13063
            {

                if (MessageBox.Show("总颗数不匹配!", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }

            //------------------------------根据SINF生成新的TSK-MAP----------------------------//

            string WaferID_1 = this.txtWaferID;
            tsk.FullName = "D:\\MERGE\\" + WaferID_1.TrimEnd('\0');
            //const int inkBinNo = 61;
            //tsk.SaveWithTxtMap(txtNewData, inkBinNo);//TODO


            /////--------------------Map版本为2，且无扩展信息TSK修改BIN信息代码-------------------////
            const int inkBinNo = 61;
            if (!tsk.ExtendFlag && ((Convert.ToInt32(tsk.MapVersion) == 2)))
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (txtNewData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                    {
                        tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                        tsk.DieMatrix[k].Bin = inkBinNo;
                        //convertToFailBin(firstbyte1_1, thirdbyte1_1, thirdbyte2_1, inkBinNo, k);
                    }

                }
            }

            /////--------------------Map版本为2，且有扩展信息TSK修改BIN信息代码-------------------////
            if (tsk.ExtendFlag)
            {
                for (int k = 0; k < tsk.Rows * tsk.Cols; k++)
                {
                    if (txtNewData[k].ToString() == ".")//Skip Die
                    {
                        continue;
                    }

                    else
                    {
                        if (Convert.ToInt32(tsk.MapVersion) == 2)
                        {
                            if (txtNewData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                            {
                                tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                                tsk.DieMatrix[k].Bin = inkBinNo;
                                //convertToFailBinWithExtention(firstbyte1_1, thirdbyte1_1, thirdbyte2_1, inkBinNo, k, arry_1, 4 * k + 1);

                                //arry_1[4 * k + 1] = Convert.ToByte(Convert.ToByte(arry_1[4 * k + 1]) & 192);
                                //arry_1[4 * k + 1] = Convert.ToByte(Convert.ToByte(arry_1[4 * k + 1]) | binNo);//换成想要的BIN58


                            }
                        }
                        else if (Convert.ToInt32(tsk.MapVersion) == 4)
                        {
                            if (txtNewData[k].ToString() == "X")//sinf fail,需要改为fail属性，BIN也需要改
                            {
                                tsk.DieMatrix[k].Attribute = DieCategory.FailDie;
                                tsk.DieMatrix[k].Bin = inkBinNo;
                                //convertToFailBinWithExtention(firstbyte1_1, thirdbyte1_1, thirdbyte2_1, inkBinNo, k, arry_1, 4 * k + 3);


                                //arry_1[4 * k + 3] = Convert.ToByte(Convert.ToByte(arry_1[4 * k + 3]) | binNo);//换成想要的BIN58

                            }
                        }
                    }
                }
            }


            /////--------------------Map版本为4，且有扩展信息TSK修改BIN信息代码-------------------////



            //----------------------------TSK修改BIN信息-----------------------------------------------------
            tsk.WaferID = WaferID_1;
            tsk.Save();
            
            printTxtTskPair(tsk.LotNo);



        }

        private static void CountPassAndFail(Tsk tsk, string[,] TxtNewMap, ref int countPass, ref int countFail, ref int countMark)
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

        private void GetNewTxtData(string[,] TxtNewMap, int xMax, int yMax)
        {
            if (this.txtNewData == null)
            {
                this.txtNewData = new List<string>();
            }
            else
            {
                this.txtNewData.Clear();
            }

            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    txtNewData.Add(TxtNewMap[x, y].ToString());
                }
            }
        }

        private static string[,] GetNewTxtMap(string[,] TxtMap, int xMin, int yMin, int xMax, int yMax, int xMaxCordinate, int yMaxCordinate)
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
            //int row1_1 = tsk.Rows;  //tsk的行和列和常规的反了 size of horizontal  水平方向 列  66  x轴方向的最大值
            //int col1_1 = tsk.Cols;  //tsk的行和列和常规的反了 size of verticatl   垂直方向 行  63  y轴方向的最大值
            string[,] TSKMap = new string[tsk.DieMatrix.XMax, tsk.DieMatrix.YMax];
            //74列 78行


            //生成TSKMap
            for (int y = 0; y < tsk.DieMatrix.YMax; y++)//83
            {
                for (int x = 0; x < tsk.DieMatrix.XMax; x++)//57
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

        private void GetDegtxtData(Tsk tsk, List<string> txtData)
        {
            if (this.DegtxtData == null)
            {
                this.DegtxtData = new List<string>();
            }
            else
            {
                this.DegtxtData.Clear();
            }
            int count = txtColct * txtRowct;

            for (int i = 0; i < count; i++)
            {
                DegtxtData.Add(".");
            }

            if (!String.IsNullOrEmpty(this.txtFlat))
            {
                int txtFlat1 = Convert.ToInt32(this.txtFlat);
                //int txtFlat1 = 180;
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

                else if (flatDifference == 0)////TXT不转角度
                {

                    for (int i = 0; i < count; i++)
                    {

                        DegtxtData[i] = txtData[i];
                    }

                }
            }
        }

        private void printTxtTskPair(string LotNo_1)
        {
            ////////////////////////////////输出TXT//////////////////////////////////
            FileStream fwt = new FileStream("D:\\MERGE\\" + LotNo_1 + "_txt_with_tsk" + ".txt", FileMode.Create);
            StreamWriter swt = new StreamWriter(fwt);
            for (int ii = 0; ii < tsk_Name.Count; ii++)
            {
                swt.WriteLine(txt_Name[ii] + " " + tsk_Name[ii]);
            }
            swt.WriteLine();

            swt.Close();
            fwt.Close();
        }

        private static void convertToFailBin(byte[] firstbyte1_1, byte[] thirdbyte1_1, byte[] thirdbyte2_1, int binNo, int k)
        {
            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 128);//标记成fail

            thirdbyte1_1[k] = thirdbyte1_1[k];
            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] & 192);
            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] | binNo);
        }

        private static void convertToFailBinWithExtention(byte[] firstbyte1_1, byte[] thirdbyte1_1, byte[] thirdbyte2_1, int binNo, int k,
            ArrayList arry_1, int ExtentionIndex)
        {
            
            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] & 1);
            firstbyte1_1[k] = Convert.ToByte(firstbyte1_1[k] | 128);//标记成fail

            thirdbyte1_1[k] = thirdbyte1_1[k];
            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] & 192);
            thirdbyte2_1[k] = Convert.ToByte(thirdbyte2_1[k] | binNo);

            arry_1[ExtentionIndex] = Convert.ToByte(Convert.ToByte(arry_1[ExtentionIndex]) & 0);
            arry_1[ExtentionIndex] = Convert.ToByte(Convert.ToByte(arry_1[ExtentionIndex]) | binNo);
        }

        private int getTotalOfX(string[,] txtNewData, string v, int col1_1, int row1_1)
        {
            int total = 0;
            for (int i = 0; i < col1_1; i++)
            {
                for (int j = 0; j < row1_1; j++)
                {
                    if ((string)(txtNewData[i, j]) == v)
                    {
                        total += 1;
                    }
                }
            }

            return total;
        }

        private void Parse(string line)
        {
            try
            {
                //TODO 头信息
                if (line.Contains(':'))
                {
                    string[] strs = line.Split(new char[] { ':', '=' });
                    string head = strs[0].Trim().ToUpper();
                    string body = strs[1].Trim();
                    if (string.IsNullOrEmpty(body))
                    {
                        return;
                    }
                    switch (head)
                    {
                    
                        case "DEVICE":
                        case "DEVICE NAME":
                            this.txtDevice = body;
                            break;
                        case "LOT":
                        case "LOT NO":
                            this.txtLot = body;
                            break;
                        case "SLOT NO":
                            this.txtSlot = Convert.ToInt32(body); ;
                            break;
                        case "WAFER":
                        case "WAFER ID":
                            this.txtWaferID = body;
                            break;
                        case "FNLOC":
                        case "FLAT DIR":
                            this.txtFlat = body;
                            break;
                        case "ROWCT":
                            this.txtRowct = Convert.ToInt32(body);
                            break;
                        case "COLCT":
                            this.txtColct = Convert.ToInt32(body);
                            break;
                        case "PASS DIE":
                            this.txtPass = Convert.ToInt32(body);
                            break;
                        case "FAIL DIE":
                            this.txtFail = Convert.ToInt32(body);
                            break;
                        case "GROSS_DIES":
                        case "TOTAL TEST DIE":
                            this.txtTotal = Convert.ToInt32(body);
                            break;

                    }
                }
                else
                {
                    this.ParseDies(line);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        private void ParseDies(string s)
        {
            PasrseDieWithDeviceWTM2100COfZhiCun(s);
            //TODO null报错
            //if (this.txtDevice.Contains("IML7972"))
            //{
            //    PasrseDieWithDeviceIML7972(s);
            //} else if (this.txtDevice.Contains("UPM7231"))
            //{
            //    PasrseDieWithDeviceUPM7231(s);
            //} else
            //{
            //    PasrseDieWithDeviceUPM6700(s);
            //}
        }

        private void PasrseDieWithDeviceWTM2100COfZhiCun(string s)
        {
            if (s.Contains("|"))
            {
                string newLine = s.Substring(s.IndexOf("|") + 1);
                txtColct = newLine.Length/3;
                txtRowct++;
                for (int i = 0; i < newLine.Length;)
                {
                    
                    string binNo = newLine.Substring(i+2, 1);
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

                // 312/3 = 104 列
                // 123行 123*104= 12792
            }
            Console.WriteLine("txtRowct:" + txtData.Count);
        }
        private void PasrseDieWithDeviceIML7972(string s)
        {
            if (s.StartsWith(".") || s.StartsWith("S") || s.StartsWith("#"))
            {
                string newLine = s;
                txtColct = newLine.Length;
                txtRowct++;
                for (int i = 0; i < newLine.Length;i++)
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

        private void Reverse(ref byte[] target)
        {
            int n1 = 0, n2 = target.Length - 1;
            byte temp;
            while (n1 < n2)
            {
                temp = target[n1];
                target[n1] = target[n2];
                target[n2] = temp;

                n1++;
                n2--;
            }
        }

        private short ByteToInt16(ref byte[] target)
        {
            this.Reverse(ref target);
            return BitConverter.ToInt16(target, 0);

        }




    }
}
