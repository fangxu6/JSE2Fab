using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using WindowsFormTool.TskUtil;

public class TskProcessor
{
    private readonly Dictionary<int, ITskProcessor> _processors;

    public TskProcessor()
    {
        _processors = new Dictionary<int, ITskProcessor>
        {
            { 0, new TskMergeProcessor() },  // Case 0: TSK合并
            { 1, new TskInkProcessor() }      // Case 1: INK规则
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