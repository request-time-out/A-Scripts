// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.AddNavMeshAgent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

namespace ADV.Commands.Game
{
  public class AddNavMeshAgent : CommandBase
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
      ((Component) this.scenario.currentChara.transform).get_gameObject().AddComponent<NavMeshAgent>();
    }
  }
}
