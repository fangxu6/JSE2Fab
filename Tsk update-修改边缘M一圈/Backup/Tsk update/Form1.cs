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
using System.Timers;
using Tsk_update.File;
using Tsk_update.Util;


namespace Tsk_update
{

  



    public partial class Form1 : Form
    {


        private string NewWaferID;
        private string NewLotNo;
        private int NewSlotNo;
        private string tskpath;

        ArrayList arryWaferID = new ArrayList();
        ArrayList arryLotNo = new ArrayList();
        ArrayList arrySlotNo = new ArrayList();
        ArrayList arrayFilepath = new ArrayList();

        public Form1()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {


            for (int i = 0; i < arryLotNo.Count; i++)
            {


                if (arryLotNo[i].ToString() == this.dataGridView1[0, i].Value.ToString() && arrySlotNo[i].ToString() == this.dataGridView1[1, i].Value.ToString() && arryWaferID[i].ToString() == this.dataGridView1[2, i].Value.ToString() && arrayFilepath[i].ToString() == this.dataGridView1[3, i].Value.ToString())
                {
                    continue;

                }

                else
                {
                    arryLotNo[i] = this.dataGridView1[0, i].Value;
                    arrySlotNo[i] = this.dataGridView1[1, i].Value;
                    arryWaferID[i] = this.dataGridView1[2, i].Value;
                    arrayFilepath[i] = this.dataGridView1[3, i].Value;



                    FileStream fs;

                    fs = new FileStream(arrayFilepath[i].ToString(), FileMode.Open);

                    BinaryReader br = new BinaryReader(fs);


                    string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim();
                    string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
                    byte[] WaferSize = br.ReadBytes(2);
                    byte[] MachineNo = br.ReadBytes(2);
                    byte[] IndexSizeX = br.ReadBytes(4);
                    byte[] IndexSizeY = br.ReadBytes(4);
                    byte[] FlatDir = br.ReadBytes(2);
                    byte MachineType = br.ReadByte();
                    byte MapVersion = br.ReadByte(); 


                    // int rows = br.ReadInt16();
                    // int cols = br.ReadInt16();
                    byte row1 = br.ReadByte();
                    byte row2 = br.ReadByte();
                    byte col1 = br.ReadByte();
                    byte col2 = br.ReadByte();
                    byte[] MapDataForm = br.ReadBytes(4);
                    string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
                    byte ProbingNo = br.ReadByte();
                    string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();

                    byte[] CN = br.ReadBytes(2);
                    this.Reverse(ref CN);
                    int CassetteNo = BitConverter.ToInt16(CN, 0);



                    byte[] SN = br.ReadBytes(2);
                    this.Reverse(ref SN);
                    int SlotNo = BitConverter.ToInt16(SN, 0);
                    //int SlotNo = br.ReadInt16();


                    ArrayList arry = new ArrayList();


                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        arry.Add(br.ReadByte());
                    }


                    br.Close();
                    fs.Close();


                    FileStream fw;


                    NewWaferID = arryWaferID[i].ToString();
                    NewLotNo = arryLotNo[i].ToString();
                    NewSlotNo = Convert.ToInt16(arrySlotNo[i]);


                    fw = new FileStream("C:\\"+ NewSlotNo.ToString("000") + "." + WaferID, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fw);


                    string str = string.Format("{0,-20:G}", Operator);
                    bw.Write(Encoding.ASCII.GetBytes(str), 0, 20);//写入Operator

                    str = string.Format("{0,-16:G}", Device);
                    bw.Write(Encoding.ASCII.GetBytes(str), 0, 16);//写入Device

                    byte[] buf;

                 //   buf = BitConverter.GetBytes((short)WaferSize);
                  //  this.Reverse(ref buf);
                  //  bw.Write(buf, 0, 2); //写入WaferSize
                    bw.Write(WaferSize);

                  //  buf = BitConverter.GetBytes((short)MachineNo);
                 //   this.Reverse(ref buf);
                  //  bw.Write(buf, 0, 2);//写入WaferNo
                    bw.Write(MachineNo);

                  //  buf = BitConverter.GetBytes(IndexSizeX);
                //    this.Reverse(ref buf);
                 //   bw.Write(buf, 0, 4);//写入IndexSizeX
                    bw.Write(IndexSizeX);

                  //  buf = BitConverter.GetBytes(IndexSizeY);
                 //   this.Reverse(ref buf);
                 //   bw.Write(buf, 0, 4);//写入IndexSizeY
                    bw.Write(IndexSizeY);

                   // buf = BitConverter.GetBytes((short)FlatDir);
                  //  this.Reverse(ref buf);
                  //  bw.Write(buf, 0, 2);
                    bw.Write(FlatDir);


                    bw.Write(MachineType);
                    bw.Write(MapVersion);
                    bw.Write(row1);
                    bw.Write(row2);
                    bw.Write(col1);
                    bw.Write(col2);
                    //buf = BitConverter.GetBytes((short)rows);
                    //this.Reverse(ref buf);
                    //bw.Write(buf, 0, 2);

                    //buf = BitConverter.GetBytes((short)cols);
                    //this.Reverse(ref buf);
                    //bw.Write(buf, 0, 2);

                  //  buf = BitConverter.GetBytes(MapDataForm);
                 //   this.Reverse(ref buf);
                 //   bw.Write(buf, 0, 4);
                    bw.Write(MapDataForm);

                    str = string.Format("{0,-21:G}", WaferID);
                    bw.Write(Encoding.ASCII.GetBytes(str), 0, 21);


                    bw.Write(BitConverter.GetBytes(ProbingNo), 0, 1);

                    str = string.Format("{0,-18:G}", LotNo);
                    bw.Write(Encoding.ASCII.GetBytes(str), 0, 18);

                   buf = BitConverter.GetBytes((short)CassetteNo);
                    this.Reverse(ref buf);
                    bw.Write(buf, 0, 2);
                   // bw.Write(CN);


                    buf = BitConverter.GetBytes((short)NewSlotNo);
                    this.Reverse(ref buf);
                    bw.Write(buf, 0, 2);



                    foreach (byte obj in arry)
                    {
                        bw.Write(obj);

                    }

                    bw.Flush();
                    bw.Close();
                    fw.Close();
                }
            }

            MessageBox.Show("修改完成");

            


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


     /*   private void ReadTSk(string str)
        { 
        
             FileStream fs;


            fs = new FileStream(str, FileMode.Open);


            BinaryReader br = new BinaryReader(fs);


            string Operator = Encoding.ASCII.GetString(br.ReadBytes(20)).Trim(); 

            string Device = Encoding.ASCII.GetString(br.ReadBytes(16)).Trim();
            int WaferSize = br.ReadInt16();
            int MachineNo = br.ReadInt16();
            int IndexSizeX = br.ReadInt32();
            int IndexSizeY = br.ReadInt32();
            int FlatDir = br.ReadInt16();
            byte MachineType = br.ReadByte();
            byte MapVersion = br.ReadByte();


           // int rows = br.ReadInt16();
           // int cols = br.ReadInt16();
            byte row1 = br.ReadByte();
            byte row2 = br.ReadByte();
            byte col1 = br.ReadByte();
            byte col2 = br.ReadByte();
            int MapDataForm = br.ReadInt32();
            string WaferID = Encoding.ASCII.GetString(br.ReadBytes(21)).Trim();
            byte ProbingNo = br.ReadByte();
            string LotNo = Encoding.ASCII.GetString(br.ReadBytes(18)).Trim();

            byte[] CN = br.ReadBytes(2);
            this.Reverse(ref CN);
            int CassetteNo  = BitConverter.ToInt16(CN, 0);
          

           
            byte[] SN = br.ReadBytes(2);
            this.Reverse(ref SN);
            int SlotNo=BitConverter.ToInt16(SN, 0);
            //int SlotNo = br.ReadInt16();

             
            ArrayList arry = new ArrayList();


            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                arry.Add(br.ReadByte());
            }






            br.Close();
            fs.Close();
        
        
        }
       */

     /*   private void button2_Click(object sender, EventArgs e)
        {
            

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = false;
            dialog.Filter = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = Path.GetFullPath(dialog.FileName);
                tskpath = Path.GetDirectoryName(dialog.FileName);
  
            }


            
        }
      */

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = false;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string str in dialog.FileNames)
                {

                    Tsk tsk = new Tsk(str);
                    tsk.Read();
                  
                    arryLotNo.Add(tsk.LotNo);
                    arryWaferID.Add(tsk.WaferID);
                    arrySlotNo.Add(tsk.SlotNo);
                    arrayFilepath.Add(str);


                }

            }

            if (arryLotNo.Count > 0)
            {

                this.dataGridView1.Columns.Clear();

                this.dataGridView1.Columns.Add("c1", "LotNo");
                this.dataGridView1.Columns.Add("c2", "SlotNo");
                this.dataGridView1.Columns.Add("c3", "WaferID");
                this.dataGridView1.Columns.Add("c3", "PATH");
                this.dataGridView1.Rows.Add(arryLotNo.Count);
                for (int i = 0; i < arryLotNo.Count; i++)
                {
                    this.dataGridView1[0, i].Value = arryLotNo[i];
                    this.dataGridView1[1, i].Value = arrySlotNo[i];
                    this.dataGridView1[2, i].Value = arryWaferID[i];
                    this.dataGridView1[3, i].Value = arrayFilepath[i];
                    if (arrySlotNo[i].ToString() != "0")
                    {
                        this.dataGridView1[1, i].ReadOnly = true;
                    
                    }

                }


                for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
                {
                    this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

            }



        }


    }
}