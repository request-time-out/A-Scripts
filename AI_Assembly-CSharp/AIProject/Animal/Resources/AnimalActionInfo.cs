// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Resources.AnimalActionInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace AIProject.Animal.Resources
{
  public class AnimalActionInfo
  {
    public AnimalActionInfo.TimeInfo timeInfo;

    public AnimalActionInfo()
    {
      this.timeInfo = new AnimalActionInfo.TimeInfo();
      this.timeInfo.Disable();
    }

    public struct TimeInfo
    {
      public TimeInfo(bool _manageTimeEnable, int _min, int _max)
      {
        this.manageTimeEnable = _manageTimeEnable;
        this.min = _min;
        this.max = _max;
      }

      public bool manageTimeEnable { get; private set; }

      public int min { get; private set; }

      public int max { get; private set; }

      public void Disable()
      {
        this.manageTimeEnable = false;
      }
    }
  }
}
