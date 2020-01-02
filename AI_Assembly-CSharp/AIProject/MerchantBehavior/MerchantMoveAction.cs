// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.MerchantMoveAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  public abstract class MerchantMoveAction : MerchantAction
  {
    protected Subject<Unit> onActionPlay = new Subject<Unit>();
    protected Subject<Unit> onEndAction = new Subject<Unit>();
    protected ActorAnimInfo animInfo = new ActorAnimInfo();
    private int actionID;
    private int poseID;
    protected Transform recoveryPoint;
    private bool prevTalkable;

    protected ActionPoint CurrentPoint
    {
      get
      {
        return this.Merchant.CurrentPoint;
      }
      set
      {
        this.Merchant.CurrentPoint = value;
      }
    }

    protected bool IsActiveNavMeshAgent
    {
      get
      {
        return Object.op_Inequality((Object) this.Merchant, (Object) null) && Object.op_Inequality((Object) this.Merchant.NavMeshAgent, (Object) null) && ((Behaviour) this.Merchant.NavMeshAgent).get_isActiveAndEnabled() && this.Merchant.NavMeshAgent.get_isOnNavMesh();
      }
    }

    public virtual void OnStart()
    {
      if (this.prevTalkable = this.Merchant.Talkable)
        this.Merchant.Talkable = false;
      ((Task) this).OnStart();
      if (Object.op_Equality((Object) this.CurrentPoint, (Object) null))
        return;
      this.Merchant.SetActiveOnEquipedItem(false);
      this.Merchant.ChaControl.setAllLayerWeight(0.0f);
      this.Merchant.IsActionMoving = true;
      ActionPointInfo outInfo;
      this.CurrentPoint.TryGetAgentActionPointInfo(this.Merchant.EventKey, out outInfo);
      GameObject loop1 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName);
      Transform t = !Object.op_Equality((Object) loop1, (Object) null) ? loop1.get_transform() : ((Component) this.CurrentPoint).get_transform();
      GameObject loop2 = ((Component) this.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      this.recoveryPoint = !Object.op_Inequality((Object) loop2, (Object) null) ? (Transform) null : loop2.get_transform();
      this.actionID = outInfo.eventID;
      this.poseID = outInfo.poseID;
      this.animInfo = this.Merchant.Animation.LoadActionState(this.actionID, this.poseID, Singleton<Resources>.Instance.Animation.MerchantCommonActionAnimStateTable[this.actionID][this.poseID]);
      this.Merchant.ActivateNavMeshObstacle(this.Merchant.Position);
      this.Merchant.Animation.StopAllAnimCoroutine();
      this.Merchant.Animation.PlayInAnimation(this.animInfo.inEnableBlend, this.animInfo.inBlendSec, this.animInfo.inFadeOutTime, this.animInfo.layer);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this.onEndAction, 1), (System.Action<M0>) (_ => this.Merchant.Animation.PlayOutAnimation(this.animInfo.outEnableBlend, this.animInfo.outBlendSec, this.animInfo.layer)));
      if (this.animInfo.hasAction)
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this.onActionPlay, 1), (System.Action<M0>) (_ => this.Merchant.Animation.PlayActionAnimation(this.animInfo.layer)));
      this.CurrentPoint.SetSlot((Actor) this.Merchant);
      this.Merchant.SetStand(t, this.animInfo.inEnableBlend, this.animInfo.inBlendSec, this.animInfo.directionType);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Merchant.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (this.onEndAction != null)
        this.onEndAction.OnNext(Unit.get_Default());
      if (this.Merchant.Animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      this.OnCompletedStateTask();
      this.Merchant.Animation.ResetDefaultAnimatorController();
      if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
      {
        ActionPoint currentPoint = this.CurrentPoint;
        this.CurrentPoint = (ActionPoint) null;
        this.Merchant.PrevActionPoint = currentPoint;
        if (this.IsActiveNavMeshAgent)
          this.Merchant.NavMeshAgent.CompleteOffMeshLink();
        if (Merchant.NormalModeList.Contains(this.Merchant.CurrentMode))
          this.Merchant.SetStand(this.recoveryPoint, this.animInfo.outEnableBlend, this.animInfo.outBlendSec, this.animInfo.directionType);
        currentPoint.ReleaseSlot((Actor) this.Merchant);
        this.Merchant.ClearItems();
        this.Merchant.ClearParticles();
      }
      this.Merchant.EventKey = (AIProject.EventType) 0;
    }

    protected virtual void OnCompletedStateTask()
    {
    }

    public virtual void OnEnd()
    {
      if (Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        this.Complete();
      this.Merchant.SetActiveOnEquipedItem(true);
      if (this.prevTalkable)
        this.Merchant.Talkable = true;
      this.Merchant.IsActionMoving = false;
      this.Merchant.ClearItems();
      this.Merchant.ClearParticles();
      ((Task) this).OnEnd();
    }
  }
}
