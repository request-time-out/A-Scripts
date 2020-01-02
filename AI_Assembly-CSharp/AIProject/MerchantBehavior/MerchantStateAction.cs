// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.MerchantStateAction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  public abstract class MerchantStateAction : MerchantAction
  {
    protected ActorAnimInfo animInfo = new ActorAnimInfo();
    private bool prevIsTalkable = true;
    private float distanceToPlayer = float.MaxValue;
    [SerializeField]
    private Merchant.EventType stateType;
    protected int actionID;
    protected int poseID;
    protected Subject<Unit> onActionPlay;
    protected Subject<Unit> onEndAction;
    protected Transform recoveryPoint;
    private bool isLooking;
    private bool isNearPlayer;
    private IDisposable lookingDisposable;

    protected Merchant.EventType StateType
    {
      get
      {
        return this.stateType;
      }
    }

    protected PlayerActor Player
    {
      get
      {
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.Player : (PlayerActor) null;
      }
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Merchant.SetActiveOnEquipedItem(false);
      this.Merchant.ChaControl.setAllLayerWeight(0.0f);
      this.prevIsTalkable = this.Merchant.Talkable;
      this.isLooking = false;
      this.isNearPlayer = false;
      this.distanceToPlayer = float.MaxValue;
      this.onActionPlay = (Subject<Unit>) null;
      this.onEndAction = (Subject<Unit>) null;
      this.animInfo = new ActorAnimInfo();
      this.recoveryPoint = (Transform) null;
      this.CurrentMerchantPoint = this.TargetInSightMerchantPoint;
      if (Object.op_Equality((Object) this.CurrentMerchantPoint, (Object) null))
        return;
      this.Merchant.SetPointIDInfo(this.CurrentMerchantPoint);
      Tuple<MerchantPointInfo, Transform, Transform> eventInfo = this.CurrentMerchantPoint.GetEventInfo(this.stateType);
      Transform t = eventInfo.Item2;
      this.recoveryPoint = eventInfo.Item3;
      if (this.Merchant.Talkable != eventInfo.Item1.isTalkable)
        this.Merchant.Talkable = eventInfo.Item1.isTalkable;
      this.isLooking = eventInfo.Item1.isLooking;
      this.actionID = eventInfo.Item1.eventID;
      this.poseID = eventInfo.Item1.poseID;
      Dictionary<int, PlayState> dictionary;
      PlayState info;
      if (!Singleton<Resources>.Instance.Animation.MerchantOnlyActionAnimStateTable.TryGetValue(this.actionID, out dictionary) || !dictionary.TryGetValue(this.poseID, out info))
        return;
      this.animInfo = this.Merchant.Animation.LoadActionState(this.actionID, this.poseID, info);
      this.Merchant.ActivateNavMeshObstacle(t.get_position());
      this.Merchant.Animation.StopAllAnimCoroutine();
      this.Merchant.Animation.PlayInAnimation(this.animInfo.inEnableBlend, this.animInfo.inBlendSec, this.animInfo.inFadeOutTime, this.animInfo.layer);
      this.onEndAction = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.onEndAction, ((Component) this.Merchant).get_gameObject()), 1), (System.Action<M0>) (_ => this.Merchant.Animation.PlayOutAnimation(this.animInfo.outEnableBlend, this.animInfo.outBlendSec, this.animInfo.layer)));
      if (this.animInfo.hasAction)
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) this.onActionPlay, ((Component) this.Merchant).get_gameObject()), 1), (System.Action<M0>) (_ => this.Merchant.Animation.PlayActionAnimation(this.animInfo.layer)));
      this.Merchant.CurrentMerchantPoint.SetStand(this.Merchant, t, this.animInfo.inEnableBlend, this.animInfo.inFadeOutTime, this.animInfo.directionType, (System.Action) null);
      if (!this.isLooking)
        return;
      if (this.lookingDisposable != null)
        this.lookingDisposable.Dispose();
      this.lookingDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this.Merchant).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this.Merchant).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.LookAtPlayer()));
    }

    private bool IsCloseToPlayer
    {
      get
      {
        return (double) this.distanceToPlayer <= (double) Singleton<Resources>.Instance.AgentProfile.RangeSetting.arrivedDistance;
      }
    }

    private bool IsFarPlayer
    {
      get
      {
        return (double) Singleton<Resources>.Instance.AgentProfile.RangeSetting.leaveDistance < (double) this.distanceToPlayer;
      }
    }

    private void LookAtPlayer()
    {
      if (!this.isLooking)
        return;
      PlayerActor player = this.Player;
      this.distanceToPlayer = Vector3.Distance(player.Position, this.Merchant.Position);
      if (this.IsCloseToPlayer && !this.isNearPlayer)
      {
        Transform trfTarg = player.FovTargetPointTable.get_Item(Actor.FovBodyPart.Head);
        ChaControl chaControl = this.Merchant.ChaControl;
        chaControl.ChangeLookEyesTarget(1, trfTarg, 0.5f, 0.0f, 1f, 2f);
        chaControl.ChangeLookEyesPtn(1);
        chaControl.ChangeLookNeckTarget(1, trfTarg, 0.5f, 0.0f, 1f, 0.8f);
        chaControl.ChangeLookNeckPtn(1, 1f);
        this.isNearPlayer = true;
      }
      else
      {
        if (!this.IsFarPlayer || !this.isNearPlayer)
          return;
        ChaControl chaControl = this.Merchant.ChaControl;
        chaControl.ChangeLookEyesPtn(3);
        chaControl.ChangeLookNeckPtn(3, 1f);
        this.isNearPlayer = false;
      }
    }

    protected void Complete()
    {
      if (Object.op_Equality((Object) this.Merchant.CurrentMerchantPoint, (Object) null))
        return;
      this.Merchant.Animation.ResetDefaultAnimatorController();
      this.Merchant.CurrentMerchantPoint.SetStand(this.Merchant, this.recoveryPoint, this.animInfo.outEnableBlend, this.animInfo.outBlendSec, this.animInfo.directionType, (System.Action) null);
      this.PrevMerchantPoint = this.CurrentMerchantPoint;
      this.CurrentMerchantPoint = (MerchantPoint) null;
    }

    public virtual void OnEnd()
    {
      this.Merchant.ClearItems();
      this.Merchant.ClearParticles();
      this.Merchant.SetActiveOnEquipedItem(true);
      if (this.prevIsTalkable != this.Merchant.Talkable)
        this.Merchant.Talkable = this.prevIsTalkable;
      if (this.lookingDisposable != null)
        this.lookingDisposable.Dispose();
      this.Merchant.SetLookPtn(0, 3);
      this.Merchant.SetLookTarget(0, 0, (Transform) null);
      ((Task) this).OnEnd();
    }
  }
}
