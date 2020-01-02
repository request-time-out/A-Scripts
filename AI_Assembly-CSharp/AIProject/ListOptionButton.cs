// Decompiled with JetBrains decompiler
// Type: AIProject.ListOptionButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using ReMotion;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject
{
  public class ListOptionButton : FromToLocomotion2D, IUIFader
  {
    [SerializeField]
    private StringReactiveProperty _text = new StringReactiveProperty(string.Empty);
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public Image Image
    {
      get
      {
        return this._image;
      }
    }

    public StringReactiveProperty Text
    {
      get
      {
        return this._text;
      }
    }

    public Button.ButtonClickedEvent OnClicked
    {
      get
      {
        return this._button.get_onClick();
      }
      set
      {
        this._button.set_onClick(value);
      }
    }

    private void Start()
    {
      this._canvasGroup.set_alpha(0.0f);
      ObservableExtensions.Subscribe<string>(Observable.Where<string>((IObservable<M0>) this._text, (Func<M0, bool>) (x => ((TMP_Text) this._label).get_text() != x)), (System.Action<M0>) (x => ((TMP_Text) this._label).set_text(x)));
    }

    public IObservable<TimeInterval<float>[]> Open()
    {
      Vector2 diff = Vector2.op_Subtraction(this._source, this._destination);
      IConnectableObservable<TimeInterval<float>> iconnectableObservable1 = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Create(Tween.MotionFunctionTable.get_Item(this._motionTypes.@in), 0.3f, true), true));
      IConnectableObservable<TimeInterval<float>> iconnectableObservable2 = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Create(Tween.MotionFunctionTable.get_Item(this._alphaFadingTypes.@in), 0.3f, true), true));
      iconnectableObservable1.Connect();
      iconnectableObservable2.Connect();
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable1, (System.Action<M0>) (x => this.SetPosition(diff, 1f - ((TimeInterval<float>) ref x).get_Value())));
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable2, (System.Action<M0>) (x => this._canvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())));
      return (IObservable<TimeInterval<float>[]>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[2]
      {
        (IObservable<TimeInterval<float>>) iconnectableObservable1,
        (IObservable<TimeInterval<float>>) iconnectableObservable2
      });
    }

    public IObservable<TimeInterval<float>[]> Close()
    {
      Vector2 diff = Vector2.op_Subtraction(this._source, this._destination);
      IConnectableObservable<TimeInterval<float>> iconnectableObservable1 = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Create(Tween.MotionFunctionTable.get_Item(this._motionTypes.@out), 0.3f, true), true));
      IConnectableObservable<TimeInterval<float>> iconnectableObservable2 = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Create(Tween.MotionFunctionTable.get_Item(this._alphaFadingTypes.@out), 0.3f, true), true));
      iconnectableObservable1.Connect();
      iconnectableObservable2.Connect();
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable1, (System.Action<M0>) (x => this.SetPosition(diff, ((TimeInterval<float>) ref x).get_Value())));
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable2, (System.Action<M0>) (x => this._canvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())));
      return (IObservable<TimeInterval<float>[]>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[2]
      {
        (IObservable<TimeInterval<float>>) iconnectableObservable1,
        (IObservable<TimeInterval<float>>) iconnectableObservable2
      });
    }
  }
}
