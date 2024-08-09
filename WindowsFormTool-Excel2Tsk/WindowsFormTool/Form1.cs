using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MiniExcelLibs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormTool
{
    public partial class Form1 : Form
    {
        public string ExcelFilePath;
        public string TSKFilePath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "选择 Excel 文件";
                FileDialog.RestoreDirectory = true; // 记住上次打开的目录

                // 显示文件浏览对话框，并获取用户选择
                DialogResult result = FileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ExcelFilePath = FileDialog.FileName;
                    button6.Text = ExcelFilePath;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "选择 TSK 空图谱文件";
                FileDialog.RestoreDirectory = true; // 记住上次打开的目录

                // 显示文件浏览对话框，并获取用户选择
                DialogResult result = FileDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    TSKFilePath = FileDialog.FileName;
                    button2.Text = TSKFilePath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToMapping();
        }

        private bool ToMapping()
        {
            if (string.IsNullOrWhiteSpace(ExcelFilePath))
            {
                MessageBox.Show("请先选择 Excel文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(TSKFilePath))
            {
                MessageBox.Show("请先选择 TSK空图谱文件路径", "错误提醒", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            UpdateRichTextBox("开始恢复TSK图谱\n");
            //string path = "C:\\Users\\fangx\\Desktop\\图谱恢复\\#3待恢复.xlsx";
            // 打开指定路径的文件 this.FileName
            var table = MiniExcel.QueryAsDataTable(ExcelFilePath, useHeaderRow: false);

            UpdateRichTextBox("解析Excel信息成功\n");
            FileStream fs = new FileStream(TSKFilePath, FileMode.Open, FileAccess.Read);

            UpdateRichTextBox("打开TSk空图谱成功\n");
            //------------TSK READ--------------------------------------------------//
            BinaryReader br = new BinaryReader(fs);

            UpdateRichTextBox("开始解析TSk空图谱\n");
            ///头文件-------------------------------------------------------//
            //Operator Size 20
            string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim();
            //Device Size 16
            string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
            //WaferSize Size 2
            byte[] WaferSize = br.ReadBytes(2);
            //MachineNo Size2
            byte[] MachineNo = br.ReadBytes(2);
            //IndexSizeX Size4
            byte[] IndexSizeX = br.ReadBytes(4);
            //IndexSizeY Size4
            byte[] IndexSizeY = br.ReadBytes(4);
            //FlatDir Size2
            byte[] FlatDir = br.ReadBytes(2);
            //MachineType Size1
            byte MachineType = br.ReadByte();
            //MapVersion Size1
            byte MapVersion = br.ReadByte();
            //row Size2
            byte[] row = br.ReadBytes(2);
            //col Size2
            byte[] col = br.ReadBytes(2);
            //MapDataForm Size4
            byte[] MapDataForm = br.ReadBytes(4);
            //WaferID Size21
            string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
            //ProbingNo Size1
            byte ProbingNo = br.ReadByte();
            //LotNo Size18
            string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();
            //CassetteNo Size2
            byte[] CN = br.ReadBytes(2);
            this.Reverse(ref CN);
            int CassetteNo = BitConverter.ToInt16(CN, 0);

            //SlotNo Size2
            byte[] SN = br.ReadBytes(2);
            this.Reverse(ref SN);
            int SlotNo = BitConverter.ToInt16(SN, 0);
            //X axis coordinates increase direction Size1
            byte IdeX = br.ReadByte();
            //Y axis coordinates increase direction Size1
            byte IdeY = br.ReadByte();
            //Reference die setting procedures Size1
            byte Rdsp = br.ReadByte();
            //Reserved1 Size1
            byte Reserved1 = br.ReadByte();
            //Target die position X Size4
            byte[] Tdpx = br.ReadBytes(4);
            //Target die position Y Size4
            byte[] Tdpy = br.ReadBytes(4);
            //Reference die coordinator X Size2
            byte[] Rdcx = br.ReadBytes(2);
            //Reference die coordinator Y
            byte[] Rdcy = br.ReadBytes(2);
            // Probing start position Size1
            byte Psps = br.ReadByte();
            //Probing direction Size1
            byte Pds = br.ReadByte();
            //Reserved2 Size2
            byte[] Reserved2 = br.ReadBytes(2);
            //Distance X to wafer center die origin Szie4
            byte[] DistanceX = br.ReadBytes(4);
            //Distance Y to wafer center die origin Size4
            byte[] DistanceY = br.ReadBytes(4);
            //Coordinator X of wafer center die Size4
            byte[] CoordinatorX = br.ReadBytes(4);
            //Coordinator Y of wafer center die Size4
            byte[] CoordinatorY = br.ReadBytes(4);
            //First Die Coordinator X Size4
            byte[] FdcX = br.ReadBytes(4);
            //First Die Coordinator Y Size4
            byte[] FdcY = br.ReadBytes(4);
            //Wafer Testing Start Time Data Size12
            byte[] WTSTime = br.ReadBytes(12);
            //Wafer Testing End Time Data Size12
            byte[] WTETime = br.ReadBytes(12);
            //Wafer Loading Time Data Size 12
            byte[] WLTime = br.ReadBytes(12);
            //Wafer Unloading Time Data Size12
            byte[] WULT = br.ReadBytes(12);
            //Machine No1 Size4
            byte[] MachineNo1 = br.ReadBytes(4);
            //Machine No2 Size4
            byte[] MachineNo2 = br.ReadBytes(4);

            // Special Characters Size4
            byte[] SpecialChar = br.ReadBytes(4);
            //Testing End Information Size1
            byte TestEndInfo = br.ReadByte();
            //Reserved3 Size1
            byte Reserved3 = br.ReadByte();
            //Total tested dice Size2
            byte[] Totaldice = br.ReadBytes(2);
            //Total pass dice Size2
            byte[] TotalPdice = br.ReadBytes(2);
            //Total fail dice Size2
            byte[] TotalFdice = br.ReadBytes(2);
            //Test Die Information Address Size4
            byte[] TDIAdress = br.ReadBytes(4);
            //Number of line category data Size4
            byte[] NumberCategory = br.ReadBytes(4);
            //Line category address Size4
            byte[] LineCategory = br.ReadBytes(4);
            // Map File Configuration Size2
            byte[] MapConfig = br.ReadBytes(2);
            // Max. Multi Site Size2
            byte[] MMSite = br.ReadBytes(2);
            //Max. Categories Size2
            byte[] MCategory = br.ReadBytes(2);
            //Do not use,Reserved4 Size2
            byte[] Reserved4 = br.ReadBytes(2);
            UpdateRichTextBox("解析TSk空图谱头信息部分完成\n");
            ////////Die 信息/////////////////////

            int row1 = ByteToInt16(ref row);
            int col1 = ByteToInt16(ref col);

            ArrayList arryfirstbyte1 = new ArrayList();
            ArrayList arryfirstbyte2 = new ArrayList();
            ArrayList arrysecondbyte1 = new ArrayList();
            ArrayList arrysecondbyte2 = new ArrayList();
            ArrayList arrythirdbyte1 = new ArrayList();
            ArrayList arrythirdbyte2 = new ArrayList();

            for (int k = 0; k < row1 * col1; k++)
            {
                arryfirstbyte1.Add(br.ReadByte());
                arryfirstbyte2.Add(br.ReadByte());
                arrysecondbyte1.Add(br.ReadByte());
                arrysecondbyte2.Add(br.ReadByte());
                arrythirdbyte1.Add(br.ReadByte());
                arrythirdbyte2.Add(br.ReadByte());
            }
            ArrayList arry = new ArrayList();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                arry.Add(br.ReadByte());
            }

            br.Close();
            fs.Close();

            UpdateRichTextBox("解析TSk空图谱结束\n");
            //------------------------------TSK模板Read 结束------------------------------//

            //-------------------------------------------------------写TSK MAP--------------------------------------

            UpdateRichTextBox("开始写入TSk空图谱\n");
            FileStream fw;
            int flag2 = 0;
            string newTskFilePath = @"D:\New-Tsk\" + Path.GetFileName(TSKFilePath);
            richTextBox1.Text += "生成图谱路径" + newTskFilePath + "\n";
            fw = new FileStream(newTskFilePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fw);

            byte[] firstbyte1 = (byte[])arryfirstbyte1.ToArray(typeof(byte));
            byte[] firstbyte2 = (byte[])arryfirstbyte2.ToArray(typeof(byte));

            byte[] secondbyte1 = (byte[])arrysecondbyte1.ToArray(typeof(byte));
            byte[] secondbyte2 = (byte[])arrysecondbyte2.ToArray(typeof(byte));

            byte[] thirdbyte1 = (byte[])arrythirdbyte1.ToArray(typeof(byte));
            byte[] thirdbyte2 = (byte[])arrythirdbyte2.ToArray(typeof(byte));

            /////--------------------TSK修改BIN信息代码----------------------------------------------------

            this.progressBar1.Maximum = row1 * col1;
            this.progressBar1.Value = 0;


            for (int k = 0; k < row1 * col1; k++)
            {
                this.progressBar1.Value++;
                if ((secondbyte1[k] & 192) == 0)//Skip Die
                {
                    continue;
                }

                if ((secondbyte1[k] & 192) == 128)//Mark Die
                {
                    continue;
                }


                if ((secondbyte1[k] & 192) == 64)//Probe Die
                {

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var aaa = table.Rows[i];
                        var x = aaa[0];
                        var y = aaa[1];
                        var binNo = aaa[2];


                        if (x == null || y == null || binNo == null)
                        {
                            continue;
                        }
                        //if (!JSECommonUtil.IsNumber(x.ToString()))
                        //{
                        //    continue;
                        //}
                        //if (JSECommonUtil.OverIndexOfXLimit(x.ToString()) || JSECommonUtil.OverIndexOfXLimit(y.ToString()))
                        //{
                        //    continue;
                        //}

                        short first = (short)((firstbyte1[k] << 8) | firstbyte2[k]);//合并2位 x坐标
                        short second = (short)((secondbyte1[k] << 8) | secondbyte2[k]);//合并2位 y坐标

                        // if (((firstbyte2[k] & 255) == Convert.ToInt32(aryX[m, 1])) && ((secondbyte2[k] & 255) == Convert.ToInt32(aryY[m, 1])))

                        // code bit of coordinator value y
                        int s5 = (secondbyte1[k] >> 2) & 0x1;
                        // code bit of coordinator value x
                        int s4 = (secondbyte1[k] >> 3) & 0x1;
                        if (s5 != 0)
                        {
                            Console.WriteLine("s5:" + s5);
                        }
                        if (s4 != 0)
                        {
                            Console.WriteLine("s4:" + s4);
                        }

                        int X = s4 == 0 ? (first & 511) : (first & 511) * (-1);
                        int Y = s5 == 0 ? (second & 511) : (second & 511) * (-1);
                        if (X == 513)
                        {
                            Console.WriteLine("X:" + X);
                        }
                        if ((X == Convert.ToInt32(x)) && (Y == Convert.ToInt32(y)))  //数据文件 x坐标 y坐标
                        {
                            firstbyte1[k] = Convert.ToByte((firstbyte1[k] & 1));
                            if (Convert.ToInt32(binNo) == 1)
                            {
                                firstbyte1[k] = Convert.ToByte(firstbyte1[k] | 64); //标记为Pass
                            }
                            else
                            {
                                firstbyte1[k] = Convert.ToByte(firstbyte1[k] | 128); //标记为Fail
                            }
                            thirdbyte2[k] = Convert.ToByte((thirdbyte2[k] & 192));
                            thirdbyte2[k] = Convert.ToByte((thirdbyte2[k] | Convert.ToInt32(binNo)));  //换category,全部换成4

                        }
                    }
                }
            }
            //----------------------------TSK修改BIN信息-----------------------------------------------------

            UpdateRichTextBox("正在写入TSk空图谱\n");
            //Operator Size20
            string str = string.Format("{0,-20:G}", Operator);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);

            //Device Size16
            str = string.Format("{0,-16:G}", Device);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);

            byte[] buf;
            //WaferSize
            bw.Write(WaferSize);
            //MachineNo
            bw.Write(MachineNo);
            //IndexSizeX
            bw.Write(IndexSizeX);
            //IndexSizeY
            bw.Write(IndexSizeY);
            //FlatDir
            bw.Write(FlatDir);
            //MachineType
            bw.Write(MachineType);
            //MapVersion
            bw.Write(MapVersion);
            //Row
            bw.Write(row[1]);
            bw.Write(row[0]);
            //Col
            bw.Write(col[1]);
            bw.Write(col[0]);
            //MapDataForm
            bw.Write(MapDataForm);

            //NewWaferID
            str = string.Format("{0,-21:G}", WaferID);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);


            //ProbingNo
            bw.Write(BitConverter.GetBytes(ProbingNo), 0, 1);

            //NewLotNo
            str = string.Format("{0,-18:G}", LotNo);
            bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

            //CN
            buf = BitConverter.GetBytes((short)CassetteNo);
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            //SN
            buf = BitConverter.GetBytes((short)SlotNo);
            this.Reverse(ref buf);
            bw.Write(buf, 0, 2);
            //Idex
            bw.Write(IdeX);
            //Idey
            bw.Write(IdeY);
            //Rdsp
            bw.Write(Rdsp);
            //Reserved1
            bw.Write(Reserved1);
            //Tdpx
            bw.Write(Tdpx);
            //Tdpy
            bw.Write(Tdpy);

            //Rdcx
            bw.Write(Rdcx);
            //Rdcy
            bw.Write(Rdcy);
            //Psps
            bw.Write(Psps);
            //Pds
            bw.Write(Pds);
            //Reserved2
            bw.Write(Reserved2);
            //DistanceX
            bw.Write(DistanceX);
            //DistanceY
            bw.Write(DistanceY);

            //CoordinatorX
            bw.Write(CoordinatorX);
            //CoordinatorY
            bw.Write(CoordinatorY);
            //Fdcx
            bw.Write(FdcX);
            //Fdxy
            bw.Write(FdcY);
            //WTSTIME
            bw.Write(WTSTime);
            //WTETIME
            bw.Write(WTETime);
            //WLTIME
            bw.Write(WLTime);
            //WULT
            bw.Write(WULT);

            //MachineNo1
            bw.Write(MachineNo1);
            //MachineNo2
            bw.Write(MachineNo2);
            //Specialchar
            bw.Write(SpecialChar);
            //TestEndInfo
            bw.Write(TestEndInfo);
            //Reserved3
            bw.Write(Reserved3);
            //Totaldice
            bw.Write(Totaldice);
            //TotalPdice
            bw.Write(TotalPdice);
            //TotalFdice
            bw.Write(TotalFdice);
            //DIAdress
            bw.Write(TDIAdress);
            //Numbercategory
            bw.Write(NumberCategory);
            //Linecategory
            bw.Write(LineCategory);
            //mapconfig
            bw.Write(MapConfig);
            //mmsite
            bw.Write(MMSite);
            //mcategory
            bw.Write(MCategory);
            //Reserved4
            bw.Write(Reserved4);

            UpdateRichTextBox("正在写入TSk空图谱...\n");
            for (int k = 0; k < row1 * col1; k++)
            {
                bw.Write(firstbyte1[k]);
                bw.Write(firstbyte2[k]);
                bw.Write(secondbyte1[k]);
                bw.Write(secondbyte2[k]);
                bw.Write(thirdbyte1[k]);
                bw.Write(thirdbyte2[k]);
            }
            UpdateRichTextBox("TSk新图谱生成\n");
            //扩展模式
            foreach (byte obj in arry)
            {
                bw.Write(obj);
            }
            bw.Flush();
            bw.Close();
            fw.Close();

            if (MessageBox.Show("TSk新图谱生成，是否打开所在文件夹?", "confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Process.Start(Path.GetDirectoryName(newTskFilePath));
            }

            return true;
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
        //更新RichTextBox
        private void UpdateRichTextBox(string message)
        {
            richTextBox1.Text += message;
            Application.DoEvents();
        }
    }
}
