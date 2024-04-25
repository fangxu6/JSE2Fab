
/*
 * ���ߣ�sky
 * ʱ�䣺2008-01-14
 * ���ã��ļ���ʽת�����ඨ��
 */

namespace DataToExcel
{
    using System;
    using System.Xml;
    using System.Collections;
    using DataToExcel;

    public abstract class ConverterBase : IConverter
    {
        public abstract void Convert(string f1, string f2);
        public abstract IMappingFile Convert(IMappingFile source);

        // ȥ���հ��л�հ���
        protected void Trim(IMappingFile mapping, string trimdir)
        {
            try
            {
                int emptyCount = 0;

                //ͳ�ƿհ��л��е�����
                switch (trimdir)
                {
                    case "left":
                        emptyCount = this.GetLEmptyCount(mapping);
                        if (emptyCount > 0)
                            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Left, emptyCount);
                        break;
                    case "right":
                        emptyCount = this.GetREmptyCount(mapping);
                        if (emptyCount > 0)
                            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Right, emptyCount);
                        break;
                    case "up":
                        emptyCount = this.GetUEmptyCount(mapping);
                        if (emptyCount > 0)
                            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Up, emptyCount);
                        break;
                    case "down":
                        emptyCount = this.GetDEmptyCount(mapping);
                        if (emptyCount > 0)
                            mapping.DieMatrix.Collapse(DieMatrix.ExpandDir.Down, emptyCount);
                        break;
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        // ��ȡ mapping �������հ���
        protected virtual int GetLEmptyCount(IMappingFile mapping)
        {
            for (int i = 0; i < mapping.DieMatrix.XMax; i++)
            {
                for (int j = 0; j < mapping.DieMatrix.YMax; j++)
                {
                    if (!mapping.IsEmptyDie(mapping.DieMatrix[i, j]))
                    {
                        return (i + 1);
                    }
                }
            }

            return mapping.DieMatrix.XMax;
        }
        
        // ��ȡ mapping �����Ҳ�հ���
        protected virtual int GetREmptyCount(IMappingFile mapping)
        {
            int x = mapping.DieMatrix.XMax - 1;
            int y = mapping.DieMatrix.YMax - 1;

            for (int i = x; i >= 0; i--)
            {
                for (int j = y; j >= 0; j--)
                {
                    if (!mapping.IsEmptyDie(mapping.DieMatrix[i, j]))
                    {
                        return (mapping.DieMatrix.XMax - i - 1);
                    }
                }
            }

            return mapping.DieMatrix.XMax;
        }

        // ��ȡ mapping �����Ϸ��հ���
        protected virtual int GetUEmptyCount(IMappingFile mapping)
        {
            for (int i = 0; i < mapping.DieMatrix.YMax; i++)
            {
                for (int j = 0; j < mapping.DieMatrix.XMax; j++)
                {
                    if (!mapping.IsEmptyDie(mapping.DieMatrix[j, i]))
                    {
                        return (i + 1);
                    }
                }
            }

            return mapping.DieMatrix.YMax;
        }

        // ��ȡ mapping �����·��հ���
        protected virtual int GetDEmptyCount(IMappingFile mapping)
        {
            int x = mapping.DieMatrix.XMax - 1;
            int y = mapping.DieMatrix.YMax - 1;

            for (int i = y; i >= 0; i--)
            {
                for (int j = x; j >= 0; j--)
                {
                    if (!mapping.IsEmptyDie(mapping.DieMatrix[j, i]))
                    {
                        return (mapping.DieMatrix.YMax - i - 1);
                    }
                }
            }

            return mapping.DieMatrix.YMax;
        }
    }
}
