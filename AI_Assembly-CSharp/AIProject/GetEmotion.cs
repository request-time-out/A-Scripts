// Decompiled with JetBrains decompiler
// Type: AIProject.GetEmotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AIProject
{
  public class GetEmotion : AgentAction
  {
    private float _duration = 5f;
    private float _elapsedTime;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      int hash = Animator.StringToHash(string.Empty);
      agent.Animation.Animator.CrossFadeInFixedTime(hash, 0.1f, 0, 0.1f, 0.0f);
    }

    public virtual TaskStatus OnUpdate()
    {
      this._elapsedTime += Time.get_deltaTime();
      return (double) this._elapsedTime < (double) this._duration ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
