﻿// Decompiled with JetBrains decompiler
// Type: AIProject.Faint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class Faint : AgentAction
  {
    private int _layer = -1;
    protected bool _inEnableFade;
    protected float _inFadeSecond;
    protected bool _outEnableFade;
    protected float _outFadeSecond;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StateType = AIProject.Definitions.State.Type.Immobility;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair faintId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.FaintID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[faintId.postureID][faintId.poseID];
      agent.Animation.LoadEventKeyTable(faintId.postureID, faintId.poseID);
      this._layer = info.Layer;
      this._inEnableFade = info.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeSecond = info.MainStateInfo.InStateInfo.FadeSecond;
      agent.Animation.InitializeStates(info);
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(this._inEnableFade, this._inFadeSecond, info.MainStateInfo.FadeOutTime, this._layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.Animation.PlayingInAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      this.Agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
