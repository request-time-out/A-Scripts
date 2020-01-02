// Decompiled with JetBrains decompiler
// Type: AIProject.Stand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;

namespace AIProject
{
  [TaskCategory("")]
  public class Stand : AgentAction
  {
    protected Subject<Unit> _onEndAction = new Subject<Unit>();
    private int _layer;
    private bool _inEnableFade;
    private float _inFadeTime;
    private bool _outEnableFade;
    private float _outFadeTime;
    protected IDisposable _onEndActionDisposable;
    private PlayState _info;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      if (agent.EquipedItem != null)
      {
        ItemCache equipedItem = agent.EquipedItem;
        if (equipedItem.EventItemID == Singleton<Resources>.Instance.LocomotionProfile.ObonEventItemID)
          return;
        ItemIDKeyPair umbrellaId = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine.UmbrellaID;
        EquipEventItemInfo equipEventItemInfo = Singleton<Resources>.Instance.GameInfo.CommonEquipEventItemTable[umbrellaId.categoryID][umbrellaId.itemID];
        if (equipedItem.EventItemID == equipEventItemInfo.EventItemID)
          return;
      }
      ((Task) this).OnStart();
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      int id = agent.AgentData.SickState.ID;
      this._info = (PlayState) null;
      Dictionary<int, Dictionary<int, PlayState>> agentActionAnimTable = Singleton<Resources>.Instance.Animation.AgentActionAnimTable;
      AgentProfile.PoseIDCollection poseIdTable = Singleton<Resources>.Instance.AgentProfile.PoseIDTable;
      PoseKeyPair? nullable = new PoseKeyPair?();
      switch (id)
      {
        case 0:
          nullable = new PoseKeyPair?(poseIdTable.CoughID);
          break;
        case 4:
          nullable = new PoseKeyPair?(poseIdTable.StandHurtID);
          break;
        default:
          if ((double) agent.AgentData.StatsTable[2] <= 0.0)
          {
            nullable = new PoseKeyPair?(poseIdTable.StandHurtID);
            break;
          }
          switch (Singleton<Manager.Map>.Instance.Simulator.Temperature)
          {
            case Temperature.Hot:
              nullable = new PoseKeyPair?(poseIdTable.ColdPoseID);
              break;
            case Temperature.Cold:
              nullable = new PoseKeyPair?(poseIdTable.ColdPoseID);
              break;
          }
          break;
      }
      if (!nullable.HasValue)
        return;
      this._info = agentActionAnimTable[nullable.Value.postureID][nullable.Value.poseID];
      if (this._info == null)
        return;
      agent.Animation.LoadEventKeyTable(nullable.Value.postureID, nullable.Value.poseID);
      this._layer = this._info.Layer;
      this._inEnableFade = this._info.MainStateInfo.InStateInfo.EnableFade;
      this._inFadeTime = this._info.MainStateInfo.InStateInfo.FadeSecond;
      agent.Animation.OnceActionStates.Clear();
      if (!this._info.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info stateInfo in this._info.MainStateInfo.InStateInfo.StateInfos)
          agent.Animation.OnceActionStates.Add(stateInfo);
      }
      agent.Animation.OutStates.Clear();
      if (!this._info.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info stateInfo in this._info.MainStateInfo.OutStateInfo.StateInfos)
          agent.Animation.OutStates.Enqueue(stateInfo);
      }
      agent.Animation.PlayOnceActionAnimation(this._inEnableFade, this._inFadeTime, this._layer);
      this._onEndActionDisposable = ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(this._outEnableFade, this._outFadeTime, this._layer);
      }));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.EquipedItem != null && this.Agent.EquipedItem.EventItemID == Singleton<Resources>.Instance.LocomotionProfile.ObonEventItemID)
        return (TaskStatus) 2;
      if (this._info == null)
        return (TaskStatus) 2;
      if (this.Agent.Animation.PlayingOnceActionAnimation)
        return (TaskStatus) 3;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      return this.Agent.Animation.PlayingOutAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      agent.Animation.StopOnceActionAnimCoroutine();
      agent.Animation.StopOutAnimCoroutine();
      if (this._onEndActionDisposable != null)
        this._onEndActionDisposable.Dispose();
      agent.ChangeDynamicNavMeshAgentAvoidance();
    }
  }
}
