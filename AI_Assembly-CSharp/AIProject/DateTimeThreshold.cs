// Decompiled with JetBrains decompiler
// Type: AIProject.DateTimeThreshold
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  public struct DateTimeThreshold
  {
    public DateTime start;
    public DateTime end;

    public DateTimeThreshold(DateTime start, DateTime end)
    {
      this.start = start;
      this.end = end;
    }

    public bool Contains(DateTime time)
    {
      return this.end > this.start ? time > this.start && time < this.end : (time > this.start && time > this.end ? time > this.start && time < new DateTime(1, 1, 1, 24, 0, 0) : time > new DateTime(1, 1, 1, 0, 0, 0) && time < this.end);
    }
  }
}
