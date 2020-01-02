// Decompiled with JetBrains decompiler
// Type: AIProject.DateTurn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class DateTurn : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      if (!Object.op_Inequality((Object) agent.Partner, (Object) null))
        return;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PlayState.AnimStateInfo idleStateInfo = Singleton<Resources>.Instance.DefinePack.AnimatorState.IdleStateInfo;
      float angleFromForward = this.Agent.Partner.Animation.GetAngleFromForward(((Component) agent).get_transform().get_forward());
      agent.Animation.PlayTurnAnimation(angleFromForward, 1f, idleStateInfo);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.Animation.PlayingTurnAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      agent.Animation.StopTurnAnimCoroutine();
      agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
