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
    public class Device_IML8976_8_8_8300 : IncomingFileToTskTemplate
    {
        public override void ParseLine(string line)
        {
            try
            {
                if (line.Contains(':') || line.Contains('='))
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
                            this.TxtDevice = body;
                            break;
                        case "LOT":
                        case "LOT NO":
                        case "LOTNO":
                            this.TxtLot = body;
                            break;
                        case "SLOT NO":
                            this.TxtSlot = Convert.ToInt32(body); ;
                            break;
                        case "WAFER":
                        case "WAFER ID":
                        case "WAFERID":
                        case "WAFER-ID":
                            this.TxtWaferId = body;
                            break;
                        case "FNLOC":
                        case "FLAT DIR":
                        case "FLATDIR":
                        case "FLAT":
                            this.TxtFlat = body;
                            break;
                        case "COLCT":
                            this.TxtColCount = Convert.ToInt32(body);
                            break;
                        case "PASS DIE":
                            this.TxtPass = Convert.ToInt32(body);
                            break;
                        case "FAIL DIE":
                            this.TxtFail = Convert.ToInt32(body);
                            break;
                        case "GROSS_DIES":
                        case "TOTAL TEST DIE":
                            this.TxtTotal = Convert.ToInt32(body);
                            break;
                    }
                }
                else
                {
                    if (IsMapLine(line))
                        this.ParseDies(line);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        protected override int GetFlat(string txtFlat)
        {
            if (txtFlat.ToUpper().Equals("DOWN"))
                return 180;
            if (txtFlat.ToUpper().Equals("UP"))
                return 0;
            if (txtFlat.ToUpper().Equals("LEFT"))
                return 90;
            if (txtFlat.ToUpper().Equals("RIGHT"))
                return 270;
            return Convert.ToInt32(this.TxtFlat);
        }

        protected override void ParseDies(string s)
        {
            string newLine = s;
            //按照tab分割
            TxtColCount = newLine.Length;
            TxtRowCount++;
            for (int i = 0; i < newLine.Length;)
            {
                string binNo = newLine.Substring(i, 1);
                if (binNo.Equals("."))
                {
                    txtData.Add(".");
                }
                else if (binNo.Equals("1"))
                {
                    txtData.Add("0");
                    this.TxtPass++;
                }
                else if (binNo.Equals("#"))//对位点比较
                {
                    txtData.Add("#");
                }
                else
                {
                    txtData.Add("X");
                    this.TxtFail++;
                }
                i = i + 1;
            }
        }

        //判断所在行是否是图谱数据
        private bool IsMapLine(string str)
        {
            if (str == null || str.Length == 0)
            {
                return false;
            }
            if (str.Length > 26)//Magic Number
            {
                return true;
            }
            return false;
        }
    }
}