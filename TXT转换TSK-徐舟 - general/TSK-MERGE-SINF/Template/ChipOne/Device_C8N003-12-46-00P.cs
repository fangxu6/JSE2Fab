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
                if (line.Contains("LOT ID"))
                {
                    this.TxtLot = line.Substring(line.IndexOf("LOT ID") + 6 + 3, 25).Trim();
                    this.TxtLot = this.TxtLot.Replace(".", "");
                }
                if (line.Contains("WAFER ID"))
                {
                    this.TxtWaferId = this.TxtLot + "-" + line.Substring(line.IndexOf("WAFER ID") + 8 + 3, 2).Trim();
                }

                this.ParseDies(line);
                this.TxtFlat = "180";
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        protected override int GetFlat(string txtFlat)
        {
            return Convert.ToInt32(this.TxtFlat);
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
                TxtColCount = newLine.Length;
                TxtRowCount++;
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
                        this.TxtPass++;
                    }
                    else
                    {
                        txtData.Add("X");
                        this.TxtFail++;
                    }
                }
            }
        }
    }
}