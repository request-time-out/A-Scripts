// Decompiled with JetBrains decompiler
// Type: AIProject.UI.GuideOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class GuideOption : MonoBehaviour
  {
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private Text _captionText;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public GuideOption()
    {
      base.\u002Ector();
    }

    public Sprite Icon
    {
      get
      {
        return this._iconImage.get_sprite();
      }
      set
      {
        this._iconImage.set_sprite(value);
      }
    }

    public string CaptionText
    {
      get
      {
        return this._captionText.get_text();
      }
      set
      {
        this._captionText.set_text(value);
      }
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Open()
    {
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(1f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())));
    }

    private void Close()
    {
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(1f, true), true), (Action<M0>) (x => this._canvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())));
    }
  }
}
