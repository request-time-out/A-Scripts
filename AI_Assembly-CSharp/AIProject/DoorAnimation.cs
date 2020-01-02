// Decompiled with JetBrains decompiler
// Type: AIProject.DoorAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace AIProject
{
  [RequireComponent(typeof (DoorPoint))]
  public class DoorAnimation : ActionPointAnimation
  {
    private Queue<PlayState.Info> _queue = new Queue<PlayState.Info>();
    [SerializeField]
    protected int _animatorID = 1;
    private DoorPoint _linkedDoorPoint;
    private IEnumerator _animEnumerator;
    private IDisposable _animDisposable;
    private IEnumerator _closeAnimEnumerator;
    private IDisposable _closeAnimDisposable;

    protected override void OnStart()
    {
      this._animator = DoorAnimData.Table.get_Item(this._id);
      if (!Object.op_Inequality((Object) this._animator, (Object) null))
        return;
      this._animator.set_runtimeAnimatorController(Singleton<Resources>.Instance.Animation.GetItemAnimator(this._animatorID));
      this._animator.Play(Singleton<Resources>.Instance.CommonDefine.ItemAnims.DoorDefaultState, 0, 0.0f);
      if (!Object.op_Inequality((Object) (this._linkedDoorPoint = (DoorPoint) ((Component) this).GetComponent<DoorPoint>()), (Object) null))
        return;
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnEnableAsObservable((Component) this._animator), (Component) this._linkedDoorPoint), (Component) this), (Action<M0>) (_ =>
      {
        if (!Singleton<Resources>.IsInstance())
          return;
        CommonDefine.ItemAnimGroup itemAnims = Singleton<Resources>.Instance.CommonDefine.ItemAnims;
        string self = (string) null;
        switch (this._linkedDoorPoint.OpenState)
        {
          case DoorPoint.OpenPattern.Close:
            self = itemAnims.DoorCloseLoopState;
            break;
          case DoorPoint.OpenPattern.OpenRight:
            self = itemAnims.DoorOpenIdleRight;
            break;
          case DoorPoint.OpenPattern.OpenLeft:
            self = itemAnims.DoorOpenIdleLeft;
            break;
        }
        if (self.IsNullOrEmpty())
          return;
        this._animator.Play(self, 0, 0.0f);
      }));
    }

    public void Load(PlayState.Info[] states)
    {
      foreach (PlayState.Info state in states)
        this._queue.Enqueue(state);
    }

    public virtual void PlayMoveSE(bool open)
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Transform transform = ((Component) Manager.Map.GetCameraComponent())?.get_transform();
      if (Object.op_Equality((Object) transform, (Object) null))
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      DoorMatType key;
      SoundPack.DoorSEIDInfo doorSeidInfo;
      if (!DoorAnimData.MatTable.TryGetValue(this._id, ref key) || !soundPack.DoorIDTable.TryGetValue(key, out doorSeidInfo))
        return;
      int clipID = !open ? doorSeidInfo.CloseID : doorSeidInfo.OpenID;
      SoundPack.Data3D data;
      if (!soundPack.TryGetActionSEData(clipID, out data))
        return;
      Vector3 position = ((Component) this).get_transform().get_position();
      float num = Mathf.Pow(data.MaxDistance + soundPack.Game3DInfo.MarginMaxDistance, 2f);
      Vector3 vector3 = Vector3.op_Subtraction(position, transform.get_position());
      float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
      if ((double) num < (double) sqrMagnitude)
        return;
      AudioSource audioSource = soundPack.Play((SoundPack.IData) data, Sound.Type.GameSE3D, 0.0f);
      if (Object.op_Equality((Object) audioSource, (Object) null))
        return;
      audioSource.Stop();
      ((Component) audioSource).get_transform().set_position(position);
      audioSource.Play();
    }

    public void PlayAnimation(bool enableFade, float fadeTime, float fadeOutTime, int layer)
    {
      this._animEnumerator = this.StartAnimation(enableFade, fadeTime, fadeOutTime, layer);
      this._animDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._animEnumerator), false));
    }

    public void StopAnimation()
    {
      if (this._animDisposable == null)
        return;
      this._animDisposable.Dispose();
      this._animEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartAnimation(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DoorAnimation.\u003CStartAnimation\u003Ec__Iterator0()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        fadeOutTime = fadeOutTime,
        \u0024this = this
      };
    }

    public void PlayCloseAnimation(DoorPoint.OpenPattern pattern)
    {
      this.PlayMoveSE(false);
      this._closeAnimEnumerator = this.StartCloseAnimation(pattern);
      this._closeAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<CancellationToken, IEnumerator>) (_ => this._closeAnimEnumerator), false));
    }

    private void StopCloseAnimation(DoorPoint.OpenPattern pattern)
    {
      if (this._closeAnimDisposable == null)
        return;
      this._closeAnimDisposable.Dispose();
      this._closeAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartCloseAnimation(DoorPoint.OpenPattern pattern)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DoorAnimation.\u003CStartCloseAnimation\u003Ec__Iterator1()
      {
        pattern = pattern,
        \u0024this = this
      };
    }

    public bool PlayingOpenAnim
    {
      get
      {
        return this._animEnumerator != null;
      }
    }

    public bool PlayingCloseAnim
    {
      get
      {
        return this._closeAnimEnumerator != null;
      }
    }

    public void PlayCloseLoop()
    {
      this.Animator.Play(Singleton<Resources>.Instance.CommonDefine.ItemAnims.DoorCloseLoopState);
    }
  }
}
