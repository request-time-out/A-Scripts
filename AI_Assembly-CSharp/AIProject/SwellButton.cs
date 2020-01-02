// Decompiled with JetBrains decompiler
// Type: AIProject.SwellButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class SwellButton : MonoBehaviour
  {
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _swellTarget;
    [SerializeField]
    private Vector3 _destScale;
    [SerializeField]
    private float _focusDuration;
    [SerializeField]
    private float _clickDuration;
    private Vector3 _defaultScale;
    private bool _entered;
    private IDisposable _disposable;

    public SwellButton()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!Object.op_Inequality((Object) this._button, (Object) null))
        return;
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) this._button), (Func<M0, bool>) (_ => ((Selectable) this._button).IsInteractable())), (Action<M0>) (_ =>
      {
        this._entered = true;
        this.Easing(true, this._focusDuration);
      }));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerExitAsObservable((UIBehaviour) this._button), (Func<M0, bool>) (_ => ((Selectable) this._button).IsInteractable())), (Action<M0>) (_ =>
      {
        this._entered = false;
        this.Easing(false, this._focusDuration);
      }));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerDownAsObservable((UIBehaviour) this._button), (Func<M0, bool>) (_ => ((Selectable) this._button).IsInteractable() && this._entered)), (Action<M0>) (_ => this.Easing(false, this._clickDuration)));
      ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerUpAsObservable((UIBehaviour) this._button), (Func<M0, bool>) (_ => ((Selectable) this._button).IsInteractable() && this._entered)), (Action<M0>) (_ => this.Easing(true, this._clickDuration)));
    }

    private void Easing(bool fadeIn, float duration)
    {
      if (this._disposable != null)
        this._disposable.Dispose();
      Vector3 startScale = ((Component) this._swellTarget).get_transform().get_localScale();
      Vector3 dest = !fadeIn ? this._defaultScale : this._destScale;
      this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(duration, true), true), (Action<M0>) (x => ((Component) this._swellTarget).get_transform().set_localScale(Vector3.Lerp(startScale, dest, ((TimeInterval<float>) ref x).get_Value()))));
    }
  }
}
