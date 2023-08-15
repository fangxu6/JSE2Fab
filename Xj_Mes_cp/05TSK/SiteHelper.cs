using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pwt_Tsk;

namespace Xj_Mes_cp
{
   public class SiteHelper
    {
       public static void C1(int t_x, int t_y, DieData item_ic,  ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass=0;
           string site_temp = "0-0"; ;

           //   1 2
           //   3 4
           //   4针定义
           //   00  01
           //   10  11

           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
              
               default:
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }

       }

       public static void C2(int t_x, int t_y, DieData item_ic,   ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass = 0;
           string site_temp =   "0-" + (t_y % 2).ToString();

           //   1 2
           //   2针定义
           //   00  01

           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
               case "0-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site02"]++;
                   }
                   site_number_total["site02"]++;
                   break;
               default:
                    if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }

       }
       
       public static void C4(int t_x, int t_y, DieData item_ic,   ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass = 0;
           string site_temp = (t_x % 2).ToString() + "-" + (t_y % 2).ToString();
          
           //   1 2
           //   3 4
           //   4针定义
           //   00  01
           //   10  11
           
           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
               case "0-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site02"]++;
                   }
                   site_number_total["site02"]++;
                   break;
               case "1-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site3"]++;
                   }
                   site_number_total["site3"]++;
                   break;
               case "1-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site04"]++;
                   }
                   site_number_total["site04"]++;
                   break;

               default:
                    if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }
          
       }
       public static void C6(int t_x, int t_y, DieData item_ic,   ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass = 0;
           string site_temp = (t_x % 2).ToString() + "-" + (t_y % 2).ToString();

          

           //   1 2 3
           //   4 5 6
           //   6针定义
           //   00  01 02
           //   10  11 12

           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
               case "0-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site02"]++;
                   }
                   site_number_total["site02"]++;
                   break;
               case "0-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site03"]++;
                   }
                   site_number_total["site03"]++;
                   break;
               case "1-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site04"]++;
                   }
                   site_number_total["site04"]++;
                   break;
               case "1-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site05"]++;
                   }
                   site_number_total["site05"]++;
                   break;
               case "1-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site06"]++;
                   }
                   site_number_total["site06"]++;
                   break;
               default:
                    if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }

       }
       public static void C8(int t_x, int t_y, DieData item_ic,   ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass = 0;
           string site_temp = (t_x % 2).ToString() + "-" + (t_y % 4).ToString();

           //   1 2 3 4
           //   5 6 7 8
           //   8针定义
           //   00  01 02 03
           //   10  11 12 13

           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
               case "0-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site02"]++;
                   }
                   site_number_total["site02"]++;
                   break;
               case "0-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site03"]++;
                   }
                   site_number_total["site03"]++;
                   break;
               case "0-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site04"]++;
                   }
                   site_number_total["site04"]++;
                   break;
               case "1-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site05"]++;
                   }
                   site_number_total["site05"]++;
                   break;
               case "1-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site06"]++;
                   }
                   site_number_total["site06"]++;
                   break;
               case "1-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site07"]++;
                   }
                   site_number_total["site07"]++;
                   break;
               case "1-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site08"]++;
                   }
                   site_number_total["site08"]++;
                   break;
               default:
                    if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }

       }
       public static void C16(int t_x, int t_y, DieData item_ic,   ref Dictionary<string, long> site_number_pass, ref Dictionary<string, long> site_number_total)
       {
           long iPass = 0;
           string site_temp = (t_x % 4).ToString() + "-" + (t_y % 4).ToString();

           //   1 2 3 4
           //   5 6 7 8
           //   9 10 11 12
           //   13 14 15 16
           //   8针定义
           //   00  01 02 03
           //   10  11 12 13
           //   20  21 22 23
           //   30  31 32 33

           switch (site_temp)
           {
               case "0-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;
               case "0-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site02"]++;
                   }
                   site_number_total["site02"]++;
                   break;
               case "0-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site03"]++;
                   }
                   site_number_total["site03"]++;
                   break;
               case "0-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site04"]++;
                   }
                   site_number_total["site04"]++;
                   break;
               case "1-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site05"]++;
                   }
                   site_number_total["site05"]++;
                   break;
               case "1-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site06"]++;
                   }
                   site_number_total["site06"]++;
                   break;
               case "1-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site07"]++;
                   }
                   site_number_total["site07"]++;
                   break;
               case "1-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site08"]++;
                   }
                   site_number_total["site08"]++;
                   break;

               case "2-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site09"]++;
                   }
                   site_number_total["site09"]++;
                   break;
               case "2-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site10"]++;
                   }
                   site_number_total["site10"]++;
                   break;
               case "2-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site11"]++;
                   }
                   site_number_total["site11"]++;
                   break;

               case "2-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site12"]++;
                   }
                   site_number_total["site12"]++;
                   break;
               case "3-0":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site13"]++;
                   }
                   site_number_total["site13"]++;
                   break;
               case "3-1":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site14"]++;
                   }
                   site_number_total["site14"]++;
                   break;
               case "3-2":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site15"]++;
                   }
                   site_number_total["site15"]++;
                   break;
               case "3-3":
                   if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site16"]++;
                   }
                   site_number_total["site16"]++;
                   break;
               default:
                    if (item_ic.Attribute == DieCategory.PassDie)
                   {
                       iPass++;
                       site_number_pass["site01"]++;
                   }
                   site_number_total["site01"]++;
                   break;

           }

       }
      
   }
}
