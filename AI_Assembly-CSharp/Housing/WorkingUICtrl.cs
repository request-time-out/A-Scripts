// Decompiled with JetBrains decompiler
// Type: Housing.WorkingUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;

namespace Housing
{
  public class WorkingUICtrl : MonoBehaviour
  {
    [Header("表示関係")]
    [SerializeField]
    private CanvasGroup canvasGroup;
    [Header("UIアニメ関係")]
    [SerializeField]
    private Animator animatorProgressbar;
    private BoolReactiveProperty visibleReactive;

    public WorkingUICtrl()
    {
      base.\u002Ector();
    }

    public bool Visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this.visibleReactive).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this.visibleReactive).set_Value(value);
      }
    }

    public void Play()
    {
      ((Behaviour) this.animatorProgressbar).set_enabled(true);
      this.animatorProgressbar.Play("progressbar", 0, 0.0f);
    }

    public void Stop()
    {
      ((Behaviour) this.animatorProgressbar).set_enabled(false);
    }

    private void Awake()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.visibleReactive, (Action<M0>) (_b =>
      {
        this.canvasGroup.Enable(_b, false);
        if (_b)
          this.Play();
        else
          this.Stop();
      }));
    }
  }
}
