// Decompiled with JetBrains decompiler
// Type: AIProject.WaitBeforeAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class WaitBeforeAction : AgentAction
  {
    [SerializeField]
    private float _durationTime;
    private float _elapsedTime;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      int hash = Animator.StringToHash("Forward");
      agent.Animation.SetFloat(hash, 0.0f);
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }

    public virtual TaskStatus OnUpdate()
    {
      this._elapsedTime += Time.get_deltaTime();
      return (double) this._elapsedTime > (double) this._durationTime ? (TaskStatus) 2 : (TaskStatus) 3;
    }
  }
}
