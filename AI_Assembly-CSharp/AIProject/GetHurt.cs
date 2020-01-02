// Decompiled with JetBrains decompiler
// Type: AIProject.GetHurt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class GetHurt : AgentAction
  {
    private int _layer = -1;
    private bool _inEnableFade;
    private float _inFadeSecond;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      if (agent.AgentData.SickState.ID == -1)
        return;
      agent.StateType = AIProject.Definitions.State.Type.Immobility;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair standHurtId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.StandHurtID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[standHurtId.postureID][standHurtId.poseID];
      agent.Animation.LoadEventKeyTable(standHurtId.postureID, standHurtId.poseID);
      this._layer = info.Layer;
      this._inEnableFade = info.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeSecond = info.MainStateInfo.InStateInfo.FadeSecond;
      agent.Animation.InitializeStates(info);
      agent.Animation.PlayInAnimation(this._inEnableFade, this._inFadeSecond, info.MainStateInfo.FadeOutTime, this._layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.AgentData.SickState.ID == -1)
        return (TaskStatus) 2;
      if (agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      agent.AgentData.SickState.ID = 4;
      int num = Random.Range(0, (int) TimeSpan.FromDays(2.0).TotalHours);
      agent.AgentData.SickState.Duration = agent.AgentData.SickState.ElapsedTime + TimeSpan.FromDays(2.0) + TimeSpan.FromHours((double) num);
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.StateType = AIProject.Definitions.State.Type.Normal;
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
