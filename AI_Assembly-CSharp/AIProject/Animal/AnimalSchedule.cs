// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalSchedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace AIProject.Animal
{
  public struct AnimalSchedule
  {
    public bool enable;
    public string name;
    public DateTime start;
    public TimeSpan duration;
    public TimeSpan elapsedTime;
    public bool managing;

    public AnimalSchedule(bool _enable, DateTime _start, TimeSpan _duration, bool _managing)
    {
      this.enable = _enable;
      this.name = string.Empty;
      this.start = _start;
      this.duration = _duration;
      this.elapsedTime = TimeSpan.Zero;
      this.managing = _managing;
    }
  }
}
