
/*
 * ���ã�sky
 * ʱ�䣺2008-01-09
 * ���ã�Mapping �ļ��ӿ�����
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

        void Read(); // ��ȡ�ļ�
        void Save(); // �����ļ�

        void DeasilRotate(int rd); // ��תָ���Ƕ�
        bool IsEmptyDie(DieData die); // �ж�һ�� die �Ƿ�Ϊ�� die

        IMappingFile Merge(IMappingFile map, string newfile); // �ϲ� mapping �ļ�
    }
}
