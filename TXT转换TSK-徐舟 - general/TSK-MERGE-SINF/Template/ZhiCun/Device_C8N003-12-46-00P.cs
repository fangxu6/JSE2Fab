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
    /// <summary>
    /// 这个名字不对，后面需要更正
    /// </summary>
    public class Device_C8N003_12_46_00P : IncomingFileToTskTemplate
    {
        public override void ParseLine(string line)
        {
            try
            {
                //..LOT ID : NKP265000...........................................................................................................................................WAFER ID : 02................................................................................................................................................................................................................................................................................
                //从上面的字符串中获取LOT ID和WAFER ID
                if (line.Contains("LOT ID"))
                {
                    this.txtLot = line.Substring(line.IndexOf("LOT ID") + 6 + 3, 25).Trim();
                    this.txtLot = this.txtLot.Replace(".", "");
                }
                if (line.Contains("WAFER ID"))
                {
                    this.txtWaferID = this.txtLot + "-" + line.Substring(line.IndexOf("WAFER ID") + 8 + 3, 2).Trim();
                }

                this.ParseDies(line);
                this.txtFlat = "180";
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        protected override int GetFlat(string txtFlat)
        {
            return Convert.ToInt32(this.txtFlat);
        }

        protected override void ParseDies(string s)
        {
            if (!s.Contains(":") &&(s.Contains("1") || s.Contains("X")))
            {
                string newLine = s;
                //把.1替换成1
                newLine = newLine.Replace(".1", "1");
                //把.X替换成X
                newLine = newLine.Replace(".X", "X");
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
    }
}