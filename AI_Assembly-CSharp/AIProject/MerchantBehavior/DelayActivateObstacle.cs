// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.DelayActivateObstacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class DelayActivateObstacle : MerchantAction
  {
    [SerializeField]
    private bool checkUnnecessary = true;
    private bool possible;
    private bool unnecessary;
    private IDisposable disposable;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this.possible = false;
      this.unnecessary = this.Merchant.IsActiveObstacle;
      if (this.checkUnnecessary && this.unnecessary)
        return;
      this.disposable = ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) Singleton<Resources>.Instance.MerchantProfile.ActivateNavMeshElementDelayTime)), (Action<M0>) (_ => this.possible = true), (Action) (() => this.possible = true));
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.checkUnnecessary && this.unnecessary)
        return (TaskStatus) 2;
      if (!this.possible)
        return (TaskStatus) 3;
      this.Merchant.ActivateNavMeshObstacle(this.Merchant.Position);
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      if (this.disposable != null)
        this.disposable.Dispose();
      ((Task) this).OnEnd();
    }
  }
}
