// Decompiled with JetBrains decompiler
// Type: ADV.Commands.EventCG.Next
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV.EventCG;
using UnityEngine;

namespace ADV.Commands.EventCG
{
  public class Next : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "No" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ "0" };
      }
    }

    public override void Do()
    {
      base.Do();
      int index = int.Parse(this.args[0]);
      this.scenario.CrossFadeStart();
      ((Data) ((Component) this.scenario.commandController.EventCGRoot.GetChild(0)).GetComponent<Data>()).Next(index, this.scenario.commandController.Characters);
    }
  }
}
