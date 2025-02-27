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
    public class Device_CGD001_12_16_01P : IncomingFileToTskTemplate
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
                        case "WAFER-ID":
                            //F9N984-09F5根据-获取-后面的2位，
                            string[] str = body.Split('-');
                            //str[1].Substring(0, 2) 3位，第一位补0
                            this.txtWaferID = str[0] + "-" + str[1].Substring(0, 2);
                            break;
                        case "FNLOC":
                        case "FLAT DIR":
                        case "FLAT":
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

        protected override int GetFlat(string txtFlat)
        {
            return Convert.ToInt32(this.txtFlat);
        }

        protected override void ParseDies(string s)
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
    }
}