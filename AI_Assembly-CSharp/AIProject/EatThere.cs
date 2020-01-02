// Decompiled with JetBrains decompiler
// Type: AIProject.EatThere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class EatThere : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Play;
      ((Task) this).OnStart();
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      PoseKeyPair eatStandId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.EatStandID;
      agent.ActionID = eatStandId.postureID;
      agent.PoseID = eatStandId.poseID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[eatStandId.postureID][eatStandId.poseID];
      ActorAnimInfo animInfo = agent.Animation.LoadActionState(eatStandId.postureID, eatStandId.poseID, info);
      StuffItem carryingItem = agent.AgentData.CarryingItem;
      Dictionary<int, int> dictionary;
      int num;
      ActionItemInfo eventItemInfo;
      if (Singleton<Resources>.Instance.Map.FoodEventItemList.TryGetValue(carryingItem.CategoryID, out dictionary) && dictionary.TryGetValue(carryingItem.ID, out num) && Singleton<Resources>.Instance.Map.EventItemList.TryGetValue(num, out eventItemInfo))
      {
        string rightHandParentName = Singleton<Resources>.Instance.LocomotionProfile.RightHandParentName;
        GameObject gameObject = agent.LoadEventItem(num, rightHandParentName, false, eventItemInfo);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          foreach (Renderer componentsInChild in (Renderer[]) gameObject.GetComponentsInChildren<Renderer>(true))
            componentsInChild.set_enabled(true);
        }
      }
      agent.LoadActionFlag(eatStandId.postureID, eatStandId.poseID);
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, info.MainStateInfo.FadeOutTime, animInfo.layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) agent.AnimationAgent.OnEndActionAsObservable(), 1), (System.Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      if (animInfo.hasAction)
        this._onActionPlayDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnActionPlayAsObservable(), (System.Action<M0>) (_ => agent.Animation.PlayActionAnimation(animInfo.layer)));
      this._onCompleteActionDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) agent.AnimationAgent.OnCompleteActionAsObservable(), (System.Action<M0>) (_ => this.Complete()));
      if (!animInfo.isLoop)
        return;
      agent.SetCurrentSchedule(animInfo.isLoop, "その場で食べる", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
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
      int desireKey = Desire.GetDesireKey(Desire.Type.Eat);
      agent.SetDesire(desireKey, 0.0f);
      if (!agent.Animation.AnimInfo.outEnableBlend)
        agent.Animation.CrossFadeScreen(-1f);
      agent.Animation.RefsActAnimInfo = true;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.EventKey = (EventType) 0;
      agent.ApplyFoodParameter(agent.AgentData.CarryingItem);
      agent.AgentData.CarryingItem = (StuffItem) null;
    }
  }
}
