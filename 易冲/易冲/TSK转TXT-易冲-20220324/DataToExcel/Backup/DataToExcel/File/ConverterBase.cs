
/*
 * 作者：sky
 * 时间：2008-01-14
 * 作用：文件枨式转换基类定义
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

        // 去除空白行或空白列
        protected void Trim(IMappingFile mapping, string trimdir)
        {
            try
            {
                int emptyCount = 0;

                //统计空白行或列的数量
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

        // 获取 mapping 矩阵左侧空白行
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
        
        // 获取 mapping 矩阵右侧空白行
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

        // 获取 mapping 矩阵上方空白行
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

        // 获取 mapping 矩阵下方空白行
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
