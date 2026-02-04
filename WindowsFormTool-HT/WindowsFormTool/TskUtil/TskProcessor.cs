using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WindowsFormTool.TskUtil;

public class TskProcessor
{
    private readonly Dictionary<int, ITskProcessor> _processors;
    private const int OperationMerge = 0;
    private const int OperationInk = 1;
    private const int OperationStackedWafers = 2;

    public TskProcessor()
    {
        _processors = new Dictionary<int, ITskProcessor>
        {
            { OperationMerge, new TskMergeProcessor() },  // Case 0: TSK合并
            { OperationInk, new TskInkProcessor() },      // Case 1: INK规则
            { OperationStackedWafers, new TskStackedWafersProcessor() } // Case 2: Stacked Wafers
        };
    }

    public void ProcessSingle(string firstFile, string secondFile, int operationType,
        Action<string> updateStatus, ProgressBar progressBar = null)
    {
        if (_processors.TryGetValue(operationType, out var processor))
        {
            processor.ProcessSingle(firstFile, secondFile, updateStatus, progressBar);
        }
        else
        {
            throw new NotSupportedException($"操作类型 {operationType} 不支持");
        }
    }

    public void ProcessBatch(List<string> firstFiles, List<string> secondFiles, int operationType,
        Action<string> updateStatus, ProgressBar progressBar = null)
    {
        if (_processors.TryGetValue(operationType, out var processor))
        {
            processor.ProcessBatch(firstFiles, secondFiles, updateStatus, progressBar);
        }
        else
        {
            throw new NotSupportedException($"操作类型 {operationType} 不支持");
        }
    }
}
