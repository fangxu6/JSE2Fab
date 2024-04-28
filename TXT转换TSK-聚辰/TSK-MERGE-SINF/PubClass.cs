using System.Collections;

namespace DataToExcel
{
    class PubClass
    {

    }

    //定义需统计的字段
    class FieldsProp
    {
        public string Name;
        public string Checked;
    }

    //统计各种Die的数量
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
