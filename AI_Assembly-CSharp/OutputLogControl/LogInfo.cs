// Decompiled with JetBrains decompiler
// Type: OutputLogControl.LogInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System;
using System.Collections.Generic;

namespace OutputLogControl
{
  [MessagePackObject(true)]
  public class LogInfo
  {
    public const string LogTag = "OutputLog";
    private const string LogVersion = "1.0.0";

    public LogInfo()
    {
      this.tag = "OutputLog";
      this.version = new Version("1.0.0");
      this.dictLog = new Dictionary<string, List<LogData>>();
    }

    public string tag { get; set; }

    public Version version { get; set; }

    public Dictionary<string, List<LogData>> dictLog { get; set; }
  }
}
