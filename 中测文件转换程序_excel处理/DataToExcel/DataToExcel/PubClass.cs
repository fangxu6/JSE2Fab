using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Collections;

namespace DataToExcel
{
    class PubClass
    {

    } 

    //������ͳ�Ƶ��ֶ�
    class FieldsProp
    {
        public string Name;
        public string Checked;
    }

    //ͳ�Ƹ���Die������
    internal class ToCountDie
    {
        // Fields
        public static Hashtable _ToCountDie;

        // Methods
        public bool CountDie(int FailDie)
        {
            if (_ToCountDie.ContainsKey(FailDie))
            {
                _ToCountDie[FailDie] = ((int)_ToCountDie[FailDie]) + 1;
            }
            else
            {
                _ToCountDie.Add(FailDie, 1);
            }
            return true;
        }

        public bool TotalBin()
        {
            ToCountDie die = new ToCountDie();
            _ToCountDie = new Hashtable();
            return true;
        }
    }   
}
