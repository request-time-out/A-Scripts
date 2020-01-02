// Decompiled with JetBrains decompiler
// Type: AIProject.Steal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Steal : AgentAction
  {
    [SerializeField]
    private AIProject.Definitions.State.Type _stateType;
    private IDisposable _onActionPlayDisposable;
    private IDisposable _onEndActionDisposable;
    private IDisposable _onCompleteActionDisposable;
    private bool _existsFoods;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Cook;
      Resources instance = Singleton<Resources>.Instance;
      Dictionary<int, Dictionary<int, Dictionary<int, FoodParameterPacket>>> foodParameterTable = instance.GameInfo.FoodParameterTable;
      List<StuffItem> itemListInPantry = Singleton<Game>.Instance.Environment.ItemListInPantry;
      List<StuffItem> stuffItemList = ListPool<StuffItem>.Get();
      foreach (StuffItem stuffItem in itemListInPantry)
      {
        Dictionary<int, Dictionary<int, FoodParameterPacket>> dictionary;
        if (foodParameterTable.TryGetValue(stuffItem.CategoryID, out dictionary) && dictionary.TryGetValue(stuffItem.ID, out Dictionary<int, FoodParameterPacket> _))
        {
          this._existsFoods = true;
          stuffItemList.Add(stuffItem);
        }
      }
      if (!this._existsFoods)
      {
        ListPool<StuffItem>.Release(stuffItemList);
      }
      else
      {
        StuffItem element = stuffItemList.GetElement<StuffItem>(Random.Range(0, stuffItemList.Count));
        ListPool<StuffItem>.Release(stuffItemList);
        if (element == null)
        {
          this._existsFoods = false;
        }
        else
        {
          agent.AgentData.CarryingItem = new StuffItem(element.CategoryID, element.ID, 1);
          agent.CurrentPoint = agent.TargetInSightActionPoint;
          agent.SetActiveOnEquipedItem(false);
          agent.ChaControl.setAllLayerWeight(0.0f);
          agent.ElectNextPoint();
          agent.CurrentPoint.SetActiveMapItemObjs(false);
          PoseKeyPair snitchFoodId = instance.AgentProfile.PoseIDTable.SnitchFoodID;
          agent.ActionID = snitchFoodId.postureID;
          agent.PoseID = snitchFoodId.poseID;
          Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(instance.DefinePack.MapDefines.StealPivotName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
          agent.Animation.RecoveryPoint = (Transform) null;
          PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[snitchFoodId.postureID][snitchFoodId.poseID];
          ActorAnimInfo animInfo = agent.Animation.LoadActionState(snitchFoodId.postureID, snitchFoodId.poseID, info);
          agent.LoadActionFlag(snitchFoodId.postureID, snitchFoodId.poseID);
          agent.DeactivateNavMeshAgent();
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
          agent.CurrentPoint.SetSlot((Actor) agent);
          agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
          if (!animInfo.isLoop)
            return;
          agent.SetCurrentSchedule(animInfo.isLoop, "盗み食い", animInfo.loopMinTime, animInfo.loopMaxTime, animInfo.hasAction, false);
        }
      }
    }

    public virtual TaskStatus OnUpdate()
    {
      return !this._existsFoods ? (TaskStatus) 2 : this.Agent.AnimationAgent.OnUpdateActionState();
    }

    public virtual void OnEnd()
    {
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      if (this._onActionPlayDisposable != null)
        this._onActionPlayDisposable.Dispose();
      if (this._onCompleteActionDisposable != null)
        this._onCompleteActionDisposable.Dispose();
      AgentActor agent = this.Agent;
      agent.SetActiveOnEquipedItem(true);
      agent.ClearItems();
      agent.ClearParticles();
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.CauseSick();
      agent.ApplyFoodParameter(agent.AgentData.CarryingItem);
      agent.AgentData.CarryingItem = (StuffItem) null;
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      agent.Animation.EndStates();
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.CreateByproduct(agent.ActionID, agent.PoseID);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }
  }
}
