// Decompiled with JetBrains decompiler
// Type: AIProject.HarborDoorAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class HarborDoorAnimData : MonoBehaviour
  {
    private static Dictionary<int, HarborDoorAnimData> _table = new Dictionary<int, HarborDoorAnimData>();
    public static ReadOnlyDictionary<int, HarborDoorAnimData> Table = (ReadOnlyDictionary<int, HarborDoorAnimData>) null;
    [SerializeField]
    private int _linkID;
    [SerializeField]
    private Animator _doorAnimator;
    [SerializeField]
    private int _animatorID;
    [SerializeField]
    private string _openIdleStateName;
    [SerializeField]
    private string _closeIdleStateName;
    [SerializeField]
    private string _toOpenStateName;
    [SerializeField]
    private string _toCloseStateName;
    private Queue<string> _queue;
    private IDisposable _animationDisposable;

    public HarborDoorAnimData()
    {
      base.\u002Ector();
    }

    public int LinkID
    {
      get
      {
        return this._linkID;
      }
    }

    public Animator DoorAnimator
    {
      get
      {
        return this._doorAnimator;
      }
    }

    public int AnimatorID
    {
      get
      {
        return this._animatorID;
      }
    }

    private string OpenIdleStateName
    {
      get
      {
        return this._openIdleStateName;
      }
    }

    public string CloseIdleStateName
    {
      get
      {
        return this._closeIdleStateName;
      }
    }

    public string ToOpenStateName
    {
      get
      {
        return this._toOpenStateName;
      }
    }

    public string ToCloseStateName
    {
      get
      {
        return this._toCloseStateName;
      }
    }

    public bool ActiveAnimator
    {
      get
      {
        return Object.op_Inequality((Object) this._doorAnimator, (Object) null) && ((Behaviour) this._doorAnimator).get_isActiveAndEnabled() && Object.op_Inequality((Object) this._doorAnimator.get_runtimeAnimatorController(), (Object) null);
      }
    }

    public bool PlayingAnimation
    {
      get
      {
        return this._animationDisposable != null;
      }
    }

    public Action AnimEndAction { get; set; }

    protected virtual void Awake()
    {
      if (HarborDoorAnimData.Table == null)
        HarborDoorAnimData.Table = new ReadOnlyDictionary<int, HarborDoorAnimData>((IDictionary<int, HarborDoorAnimData>) HarborDoorAnimData._table);
      if (Object.op_Equality((Object) this._doorAnimator, (Object) null))
        this._doorAnimator = (Animator) ((Component) this).GetComponent<Animator>();
      if (Object.op_Equality((Object) this._doorAnimator, (Object) null))
        return;
      this._doorAnimator.set_runtimeAnimatorController(this.GetAnimatorController());
      HarborDoorAnimData._table[this._linkID] = this;
    }

    private void OnDestroy()
    {
      if (!HarborDoorAnimData._table.ContainsKey(this._linkID))
        return;
      HarborDoorAnimData._table.Remove(this._linkID);
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
      if (!Singleton<Resources>.IsInstance())
        return (RuntimeAnimatorController) null;
      return Singleton<Resources>.Instance.Animation?.GetItemAnimator(this._animatorID);
    }

    public void SetState(string stateName)
    {
      this._queue.Clear();
      if (stateName.IsNullOrEmpty())
        return;
      this._queue.Enqueue(stateName);
    }

    public void SetState(string[] stateNames)
    {
      this._queue.Clear();
      if (stateNames.IsNullOrEmpty<string>())
        return;
      foreach (string stateName in stateNames)
      {
        if (!stateName.IsNullOrEmpty())
          this._queue.Enqueue(stateName);
      }
    }

    public void SetState(List<string> stateNames)
    {
      this._queue.Clear();
      if (stateNames.IsNullOrEmpty<string>())
        return;
      foreach (string stateName in stateNames)
      {
        if (!stateName.IsNullOrEmpty())
          this._queue.Enqueue(stateName);
      }
    }

    public void PlayOpenIdleAnimation(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      this.SetState(this._openIdleStateName);
      this.PlayAnimation(enableFade, fadeTime, fadeOutTime, layer);
    }

    public void PlayCloseIdleAnimation(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      this.SetState(this._closeIdleStateName);
      this.PlayAnimation(enableFade, fadeTime, fadeOutTime, layer);
    }

    public void PlayToOpenAnimation(bool enableFade, float fadeTime, float fadeOutTime, int layer)
    {
      this.SetState(this._toOpenStateName);
      this.PlayAnimation(enableFade, fadeTime, fadeOutTime, layer);
    }

    public void PlayToCloseAnimation(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      this.SetState(this._toCloseStateName);
      this.PlayAnimation(enableFade, fadeTime, fadeOutTime, layer);
    }

    public void PlayAnimation(bool enableFade, float fadeTime, float fadeOutTime, int layer)
    {
      if (this._queue.IsNullOrEmpty<string>())
        return;
      this.StopAnimation();
      IEnumerator coroutine = this.PlayAnimCoroutine(enableFade, fadeTime, fadeOutTime, layer);
      this._animationDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
    }

    public void StopAnimation()
    {
      if (this._animationDisposable == null)
        return;
      this._animationDisposable.Dispose();
      this._animationDisposable = (IDisposable) null;
    }

    [DebuggerHidden]
    private IEnumerator PlayAnimCoroutine(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new HarborDoorAnimData.\u003CPlayAnimCoroutine\u003Ec__Iterator0()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        fadeOutTime = fadeOutTime,
        \u0024this = this
      };
    }

    private void PlayAnimation(string stateName, int layer, float normalizedTime)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0}: Play to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      this.DoorAnimator.Play(stateName, layer, normalizedTime);
    }

    private void CrossFadeAnimation(
      string stateName,
      float fadeTime,
      int layer,
      float fixedTimeOffset)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0}: CrossFade to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      this.DoorAnimator.CrossFadeInFixedTime(stateName, fadeTime, layer, fixedTimeOffset);
    }
  }
}
