// Decompiled with JetBrains decompiler
// Type: Studio.CharAnimeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Studio
{
  public class CharAnimeCtrl : MonoBehaviour
  {
    public Animator animator;
    public Transform transSon;

    public CharAnimeCtrl()
    {
      base.\u002Ector();
    }

    public bool isForceLoop
    {
      get
      {
        return this.oiCharInfo != null && this.oiCharInfo.isAnimeForceLoop;
      }
      set
      {
        this.oiCharInfo.isAnimeForceLoop = value;
      }
    }

    public OICharInfo oiCharInfo { get; set; }

    public int nameHadh { get; set; }

    public float normalizedTime
    {
      get
      {
        if (Object.op_Equality((Object) this.animator, (Object) null))
          return 0.0f;
        AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        return ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
      }
    }

    private bool isSon
    {
      get
      {
        return this.oiCharInfo != null && this.oiCharInfo.visibleSon;
      }
    }

    public void Play(string _name)
    {
      if (this.animator == null)
        return;
      this.animator.Play(_name);
    }

    public void Play(string _name, float _normalizedTime)
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
        return;
      this.animator.Play(_name, 0, _normalizedTime);
    }

    public void Play(string _name, float _normalizedTime, int _layer)
    {
      if (Object.op_Equality((Object) this.animator, (Object) null))
        return;
      if ((double) _normalizedTime != 0.0)
        this.animator.Play(_name, _layer, _normalizedTime);
      else
        this.animator.Play(_name, _layer);
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), (Func<M0, bool>) (_ => this.isForceLoop)), (Action<M0>) (_ =>
      {
        AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if (((AnimatorStateInfo) ref animatorStateInfo).get_loop() || (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() < 1.0)
          return;
        this.animator.Play(this.nameHadh, 0, 0.0f);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), (Action<M0>) (_ =>
      {
        if (!this.isSon || !Object.op_Implicit((Object) this.transSon))
          return;
        this.transSon.set_localScale(new Vector3(this.oiCharInfo.sonLength, this.oiCharInfo.sonLength, this.oiCharInfo.sonLength));
      }));
    }
  }
}
