// Decompiled with JetBrains decompiler
// Type: AIProject.CommCommandOption
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject
{
  public class CommCommandOption : MonoBehaviour
  {
    [SerializeField]
    private Text _label;
    [SerializeField]
    private Image _panel;
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Text _timerText;
    [SerializeField]
    private CanvasGroup _timerCanvasGroup;
    private IDisposable _timerDisposable;

    public CommCommandOption()
    {
      base.\u002Ector();
    }

    public Text TimerText
    {
      get
      {
        return this._timerText;
      }
    }

    public CanvasGroup TimerCanvasGroup
    {
      get
      {
        return this._timerCanvasGroup;
      }
    }

    public Action OnClick { get; set; }

    public string LabelText
    {
      get
      {
        return this._label == null ? (string) null : this._label.get_text();
      }
      set
      {
        if (!Object.op_Inequality((Object) this._label, (Object) null))
          return;
        this._label.set_text(value);
      }
    }

    public Sprite Sprite
    {
      get
      {
        return this._panel == null ? (Sprite) null : this._panel.get_sprite();
      }
      set
      {
        if (!Object.op_Inequality((Object) this._panel, (Object) null))
          return;
        this._panel.set_sprite(value);
      }
    }

    public void Start()
    {
      if (Object.op_Inequality((Object) this._button, (Object) null))
      {
        // ISSUE: method pointer
        ((UnityEvent) this._button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__0)));
      }
      if (!Object.op_Inequality((Object) this._timerCanvasGroup, (Object) null))
        return;
      this._timerCanvasGroup.set_alpha(0.0f);
    }

    public void SetActiveTimer(bool active)
    {
      if (Object.op_Equality((Object) this._timerText, (Object) null) || Object.op_Equality((Object) this._timerCanvasGroup, (Object) null) || this.ActiveTimer == active)
        return;
      this.ActiveTimer = active;
      if (this._timerDisposable != null)
        this._timerDisposable.Dispose();
      float startAlpha = this._timerCanvasGroup.get_alpha();
      int destAlpha = !active ? 0 : 1;
      ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.DoOnError<TimeInterval<float>>(Observable.Do<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.3f, true), true), (Action<M0>) (x => this._timerCanvasGroup.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value())))), (Action<Exception>) (ex => Debug.LogException(ex))));
    }

    public bool ActiveTimer { get; private set; }

    public void SetTimeColor(Color color)
    {
      if (Object.op_Equality((Object) this._timerText, (Object) null))
        return;
      ((Graphic) this._timerText).set_color(color);
    }

    public void SetTime(float time)
    {
      if (Object.op_Equality((Object) this._timerText, (Object) null))
        return;
      this._timerText.set_text(this.FloatToString(time));
    }

    private string FloatToString(float timer)
    {
      int num1 = (int) timer;
      int num2 = num1 / 3600;
      int num3 = num1 % 3600;
      int num4 = num3 / 60;
      int num5 = num3 % 60;
      string empty = string.Empty;
      if (num2 > 0)
        empty += string.Format("{0}", (object) num2);
      return empty + string.Format("{0:00}:{1:00}", (object) num4, (object) num5);
    }
  }
}
