// Decompiled with JetBrains decompiler
// Type: TextController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
  [SerializeField]
  private Text nameWindow;
  [SerializeField]
  private Text messageWindow;
  private const int MAX_FONT_SPEED = 100;
  [SerializeField]
  [RangeReactiveProperty(1f, 100f)]
  private IntReactiveProperty _fontSpeed;
  private ColorReactiveProperty _fontColor;
  private TypefaceAnimatorEx TA;
  private HyphenationJpn hypJpn;
  public bool isMovie;
  [SerializeField]
  private float movieFontSpeed;
  [SerializeField]
  [Range(0.0f, 1f)]
  private float movieProgress;

  public TextController()
  {
    base.\u002Ector();
  }

  public bool initialized { get; private set; }

  public bool IsCompleteDisplayText
  {
    get
    {
      return !this.isMovie ? !this.TA.isPlaying : (double) this.movieProgress >= 1.0;
    }
  }

  public bool NameMessageVisible
  {
    get
    {
      return this.nameVisible && this.messageVisible;
    }
    set
    {
      this.nameVisible = value;
      this.messageVisible = value;
    }
  }

  public bool nameVisible
  {
    get
    {
      return !Object.op_Equality((Object) this.nameWindow, (Object) null) && ((Behaviour) this.nameWindow).get_enabled();
    }
    set
    {
      this.nameWindow.SafeProc<Text>((Action<Text>) (p => ((Behaviour) p).set_enabled(value)));
    }
  }

  public bool messageVisible
  {
    get
    {
      return !Object.op_Equality((Object) this.messageWindow, (Object) null) && ((Behaviour) this.messageWindow).get_enabled();
    }
    set
    {
      this.messageWindow.SafeProc<Text>((Action<Text>) (p => ((Behaviour) p).set_enabled(value)));
    }
  }

  public Text NameWindow
  {
    get
    {
      return this.nameWindow;
    }
  }

  public Text MessageWindow
  {
    get
    {
      return this.messageWindow;
    }
  }

  public int FontSpeed
  {
    get
    {
      return ((ReactiveProperty<int>) this._fontSpeed).get_Value();
    }
    set
    {
      ((ReactiveProperty<int>) this._fontSpeed).set_Value(Mathf.Clamp(value, 1, 100));
    }
  }

  public Color FontColor
  {
    get
    {
      return ((ReactiveProperty<Color>) this._fontColor).get_Value();
    }
    set
    {
      ((ReactiveProperty<Color>) this._fontColor).set_Value(value);
    }
  }

  public void Change(Text nameWindow, Text messageWindow)
  {
    this.Clear();
    this.nameWindow = nameWindow;
    this.messageWindow = messageWindow;
    Object.Destroy((Object) this.hypJpn);
    this.Initialize();
  }

  public void Clear()
  {
    if (!this.initialized)
      this.Initialize();
    this.nameWindow.SafeProc<Text>((Action<Text>) (p => p.set_text(string.Empty)));
    this.messageWindow.set_text(string.Empty);
    if (Object.op_Inequality((Object) this.hypJpn, (Object) null))
      this.hypJpn.SetText(string.Empty);
    this.TA.Stop();
    this.TA.progress = 0.0f;
    this.movieProgress = 0.0f;
  }

  public void Set(string nameText, string messageText)
  {
    this.nameWindow.SafeProc<Text>((Action<Text>) (p => p.set_text(nameText)));
    this.messageWindow.set_text(messageText);
    if (Object.op_Inequality((Object) this.hypJpn, (Object) null))
      this.hypJpn.SetText(this.messageWindow.get_text());
    this.TA.Play();
    this.movieProgress = 0.0f;
  }

  public void ForceCompleteDisplayText()
  {
    this.TA.progress = 1f;
  }

  public void Initialize()
  {
    this.hypJpn = (HyphenationJpn) ((Component) this.messageWindow).GetComponent<HyphenationJpn>();
    if (false)
    {
      Object.Destroy((Object) this.hypJpn);
      this.hypJpn = (HyphenationJpn) null;
    }
    else if (Object.op_Equality((Object) this.hypJpn, (Object) null))
      this.hypJpn = (HyphenationJpn) ((Component) this.messageWindow).get_gameObject().AddComponent<HyphenationJpn>();
    if (Object.op_Inequality((Object) this.hypJpn, (Object) null))
      this.hypJpn.SetText(this.messageWindow);
    this.TA = (TypefaceAnimatorEx) ((Component) this.messageWindow).GetComponent<TypefaceAnimatorEx>();
    this.initialized = true;
  }

  private void Awake()
  {
    if (this.initialized)
      return;
    this.Initialize();
  }

  private void Start()
  {
    ObservableExtensions.Subscribe<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._fontSpeed, (Component) this), (Action<M0>) (value =>
    {
      this.TA.isNoWait = value == 100;
      if (this.TA.isNoWait)
        return;
      this.TA.timeMode = TypefaceAnimatorEx.TimeMode.Speed;
      this.TA.speed = (float) value;
    }));
    ObservableExtensions.Subscribe<Color>(Observable.TakeUntilDestroy<Color>((IObservable<M0>) this._fontColor, (Component) this), (Action<M0>) (color =>
    {
      this.nameWindow.SafeProc<Text>((Action<Text>) (p => ((Graphic) p).set_color(color)));
      this.messageWindow.SafeProc<Text>((Action<Text>) (p => ((Graphic) p).set_color(color)));
    }));
    ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_enabled())), (Func<M0, bool>) (_ => this.isMovie)), (Action<M0>) (_ => this.movieProgress = Mathf.Min(this.movieProgress + Time.get_deltaTime() / this.movieFontSpeed, 1f)));
  }
}
