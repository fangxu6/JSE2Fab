
/*
 * ���ߣ�sky
 * ʱ�䣺2008-01-11
 * ���ã�Mapping �ļ���ʽת���ӿڶ���
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
