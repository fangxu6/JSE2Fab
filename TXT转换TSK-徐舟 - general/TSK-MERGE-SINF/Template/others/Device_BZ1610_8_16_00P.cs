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
    public class Device_BZ1610_8_16_00P : IncomingFileToTskTemplate
    {
        public override void ParseLine(string line)
        {
            try
            {
                this.ParseDies(line);
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
            string newLine = s;
            //按照tab分割
            txtColct = newLine.Length;
            txtRowct++;
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
                i = i + 1;
            }
        }

        //判断所在行是否是图谱数据
        private bool IsPictureLine(string str)
        {
            if (str == null || str.Length == 0)
            {
                return false;
            }
            if (str.Length > 100)//Magic Number
            {
                return true;
            }


            return false;
        }
    }
}