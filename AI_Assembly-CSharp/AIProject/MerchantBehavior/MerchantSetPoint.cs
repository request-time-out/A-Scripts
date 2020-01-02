// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.MerchantSetPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  public abstract class MerchantSetPoint : MerchantAction
  {
    protected List<MerchantPoint> merchantPoints = new List<MerchantPoint>();
    protected ReactiveProperty<bool> EndFlag = new ReactiveProperty<bool>(false);
    private IDisposable nextPointSettingDisposable;
    protected bool Success;
    protected NavMeshPath navMeshPath;

    protected MerchantActor.MerchantSchedule schedule { get; private set; }

    protected bool ChangedSchedule { get; private set; }

    protected bool ActiveAgent
    {
      get
      {
        return ((Behaviour) this.Merchant.NavMeshAgent).get_isActiveAndEnabled() && this.Merchant.NavMeshAgent.get_isOnNavMesh();
      }
    }

    public virtual void OnAwake()
    {
      ((Task) this).OnAwake();
      this.navMeshPath = new NavMeshPath();
    }

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.Success = false;
      this.EndFlag.set_Value(false);
      this.StopSetting();
      this.merchantPoints.Clear();
      if (this.ActiveAgent)
        this.Merchant.NavMeshAgent.ResetPath();
      if (this.Merchant.MerchantPoints.IsNullOrEmpty<MerchantPoint>())
        return;
      this.ChangedSchedule = this.Merchant.MerchantData.CurrentSchedule != this.schedule;
      this.schedule = this.Merchant.MerchantData.CurrentSchedule;
      IEnumerator _coroutine = this.NextPointSettingCoroutine();
      this.nextPointSettingDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this.Merchant).get_gameObject()), (Action<M0>) (_ => {}), (Action) (() => this.EndFlag.set_Value(true)));
    }

    protected abstract IEnumerator NextPointSettingCoroutine();

    public virtual TaskStatus OnUpdate()
    {
      if (!this.Success && !this.EndFlag.get_Value() && this.nextPointSettingDisposable != null)
        return (TaskStatus) 3;
      if (!this.Success)
        return (TaskStatus) 1;
      MerchantData merchantData = this.Merchant.MerchantData;
      merchantData.PointPosition = !Object.op_Inequality((Object) this.TargetInSightMerchantPoint, (Object) null) ? new Vector3(-999f, -999f, -999f) : ((Component) this.TargetInSightMerchantPoint).get_transform().get_position();
      merchantData.MainPointPosition = !Object.op_Inequality((Object) this.MainTargetInSightMerchantPoint, (Object) null) ? new Vector3(-999f, -999f, -999f) : ((Component) this.MainTargetInSightMerchantPoint).get_transform().get_position();
      return (TaskStatus) 2;
    }

    protected void StopSetting()
    {
      if (this.nextPointSettingDisposable != null)
        this.nextPointSettingDisposable.Dispose();
      this.nextPointSettingDisposable = (IDisposable) null;
    }

    public virtual void OnEnd()
    {
      this.StopSetting();
      ((Task) this).OnEnd();
    }
  }
}
