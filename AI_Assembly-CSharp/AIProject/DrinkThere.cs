// Decompiled with JetBrains decompiler
// Type: AIProject.DrinkThere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class DrinkThere : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    private StuffItem _targetItem;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Drink;
      this._targetItem = agent.SelectDrinkItem();
      ((Task) this).OnStart();
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      PoseKeyPair drinkStandId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.DrinkStandID;
      agent.ActionID = drinkStandId.postureID;
      agent.PoseID = drinkStandId.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[drinkStandId.postureID][drinkStandId.poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(drinkStandId.postureID, drinkStandId.poseID, info);
      agent.LoadActionFlag(drinkStandId.postureID, drinkStandId.poseID);
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ => agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer)));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (System.Action<M0>) (_ => this.Complete()));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, "その場で飲む", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.AnimationAgent.OnUpdateActionState();
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onActionPlayDisposable != null)
        this._onActionPlayDisposable.Dispose();
      if (this._onCompleteActionDisposable != null)
        this._onCompleteActionDisposable.Dispose();
      AgentActor agent = this.Agent;
      agent.ChangeDynamicNavMeshAgentAvoidance();
      agent.SetActiveOnEquipedItem(true);
      agent.ClearItems();
      agent.ClearParticles();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      int desireKey = Desire.GetDesireKey(Desire.Type.Drink);
      agent.SetDesire(desireKey, 0.0f);
      agent.Animation.EndStates();
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
      agent.ApplyDrinkParameter(this._targetItem);
      this._targetItem = (StuffItem) null;
    }
  }
}
