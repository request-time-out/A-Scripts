// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.NavMeshAgentSetActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace ADV.Commands.Game
{
  public class NavMeshAgentSetActive : CommandBase
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
      ((Behaviour) ((Component) this.scenario.currentChara.transform).GetComponent<NavMeshAgent>()).set_enabled(this.isEnabled);
    }
  }
}
