// Decompiled with JetBrains decompiler
// Type: LogEventArgs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class LogEventArgs : EventArgs
{
  public string Content = string.Empty;
  public string Stacktrace = string.Empty;
  public int SeqID;
  public LogType LogType;
  public float Time;

  public LogEventArgs(int seqID, LogType type, string content, string stacktrace, float time)
  {
    this.SeqID = seqID;
    this.LogType = type;
    this.Content = content;
    this.Stacktrace = stacktrace;
    this.Time = time;
  }
}
