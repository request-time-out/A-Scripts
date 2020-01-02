// Decompiled with JetBrains decompiler
// Type: OutputLogControl.OutputLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace OutputLogControl
{
  public sealed class OutputLog
  {
    public static readonly string outputDir = Application.get_dataPath() + "/";

    [Conditional("OUTPUT_LOG")]
    public static void Log(string msg, bool unityLog = false, string filename = "Log")
    {
      OutputLog.AddMessage(filename, msg, (byte) 0, unityLog);
    }

    [Conditional("OUTPUT_LOG")]
    public static void Warning(string msg, bool unityLog = false, string filename = "Log")
    {
      OutputLog.AddMessage(filename, msg, (byte) 1, unityLog);
    }

    [Conditional("OUTPUT_LOG")]
    public static void Error(string msg, bool unityLog = false, string filename = "Log")
    {
      OutputLog.AddMessage(filename, msg, (byte) 2, unityLog);
    }

    private static void AddMessage(string filename, string msg, byte type, bool unityLog = false)
    {
      if (unityLog)
      {
        switch (type)
        {
          case 0:
            Debug.Log((object) msg);
            break;
          case 1:
            Debug.LogWarning((object) msg);
            break;
          case 2:
            Debug.LogError((object) msg);
            break;
        }
      }
      if (!Directory.Exists(OutputLog.outputDir))
        Directory.CreateDirectory(OutputLog.outputDir);
      string key = DateTime.Now.ToString("yyyy年MM月dd日");
      string str = DateTime.Now.ToString("HH:mm:ss");
      string path = OutputLog.outputDir + filename + ".mpt";
      LogInfo logInfo = new LogInfo();
      try
      {
        if (File.Exists(path))
        {
          byte[] numArray = File.ReadAllBytes(path);
          if (numArray != null)
            logInfo = (LogInfo) MessagePackSerializer.Deserialize<LogInfo>(numArray);
        }
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) string.Format("{0}:ファイルが読み込めない", (object) filename));
      }
      List<LogData> logDataList;
      if (!logInfo.dictLog.TryGetValue(key, out logDataList))
      {
        logDataList = new List<LogData>();
        logInfo.dictLog[key] = logDataList;
      }
      logDataList.Add(new LogData()
      {
        time = str,
        type = (int) type,
        msg = msg
      });
      byte[] bytes = MessagePackSerializer.Serialize<LogInfo>((M0) logInfo);
      File.WriteAllBytes(path, bytes);
    }
  }
}
