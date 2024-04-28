
/*
 * 作用：sky
 * 时间：2008-01-09
 * 作用：Mapping 文件接口描述
 */

namespace DataToExcel
{
    using System;
    using System.Collections;

    public interface IMappingFile
    {
        string FileType { get;}
        string Path { get;set;}
        string FileName { get;set;}
        string FullName { get;set;}

        DieMatrix DieMatrix { get;set;}
        Hashtable Properties { get;}

        object Tag { get;set;}

        void Read(); // 读取文件
        void Save(); // 保存文件

        void DeasilRotate(int rd); // 旋转指定角度
        bool IsEmptyDie(DieData die); // 判断一个 die 是否为空 die

        IMappingFile Merge(IMappingFile map, string newfile); // 合并 mapping 文件
    }
}
