// Decompiled with JetBrains decompiler
// Type: AIProject.Turn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Turn : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      if (!Object.op_Inequality((Object) this.Agent.CommandPartner, (Object) null))
        return;
      this.Agent.StopNavMeshAgent();
      this.Agent.Animation.PlayTurnAnimation(this.Agent.CommandPartner.Position, 1f, Singleton<Resources>.Instance.DefinePack.AnimatorState.IdleStateInfo, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.Animation.PlayingTurnAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.Animation.StopTurnAnimCoroutine();
    }
  }
}
