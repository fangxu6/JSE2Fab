
/*
 * 作者：sky
 * 时间：2008-01-11
 * 作用：Mapping 文件格式转换接口定义
 */

namespace DataToExcel
{
    using System;
    using DataToExcel;

    public interface IConverter
    {
        void Convert(string source, string target,int deg);
       
        IMappingFile Convert(IMappingFile source);
    }
}
