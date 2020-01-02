// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.ColliderSetActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Game
{
  public class ColliderSetActive : CommandBase
  {
    private bool isEnabled;

    public override string[] ArgsLabel
    {
      get
      {
        return new string[1]{ "isEnabled" };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[1]{ bool.TrueString };
      }
    }

    public override void Do()
    {
      base.Do();
      this.isEnabled = bool.Parse(this.args[0]);
      ((Collider) ((Component) this.scenario.currentChara.transform).GetComponent<Collider>()).set_enabled(this.isEnabled);
    }
  }
}
