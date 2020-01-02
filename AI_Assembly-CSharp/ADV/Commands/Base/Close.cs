// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Close
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Base
{
  public class Close : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return (string[]) null;
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Do()
    {
      base.Do();
      this.Proc();
    }

    private void Proc()
    {
      bool isAdvActionActive = Program.isADVActionActive;
      if (Object.op_Inequality((Object) this.scenario.advScene, (Object) null))
        this.scenario.advScene.Release();
      else
        ((Component) this.scenario).get_gameObject().SetActive(false);
    }
  }
}
