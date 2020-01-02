// Decompiled with JetBrains decompiler
// Type: ADV.Regulate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace ADV
{
  [Serializable]
  public class Regulate
  {
    [EnumFlags]
    public Regulate.Control control;
    private TextScenario scenario;

    public Regulate(TextScenario scenario)
    {
      this.scenario = scenario;
    }

    public void AddRegulate(Regulate.Control regulate)
    {
      this.control = (Regulate.Control) this.control.Add((Enum) regulate);
    }

    public void SubRegulate(Regulate.Control regulate)
    {
      this.control = (Regulate.Control) this.control.Sub((Enum) regulate);
    }

    public void SetRegulate(Regulate.Control regulate)
    {
      this.control = regulate;
      if (this.control != (Regulate.Control) 0)
        return;
      this.scenario.isSkip = false;
      this.scenario.isAuto = false;
    }

    public enum Control
    {
      Next = 1,
      ClickNext = 2,
      Skip = 4,
      Auto = 8,
      AutoForce = 16, // 0x00000010
    }
  }
}
