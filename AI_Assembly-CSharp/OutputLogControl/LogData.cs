// Decompiled with JetBrains decompiler
// Type: OutputLogControl.LogData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;

namespace OutputLogControl
{
  [MessagePackObject(true)]
  public class LogData
  {
    public int type { get; set; }

    public string time { get; set; }

    public string msg { get; set; }
  }
}
