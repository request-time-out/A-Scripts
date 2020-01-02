// Decompiled with JetBrains decompiler
// Type: AIProject.CaptionScript.CaptionSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ConfigScene;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace AIProject.CaptionScript
{
  public class CaptionSystem : MonoBehaviour
  {
    private static readonly string[] _replaceStrings = new string[2]
    {
      "\r\n",
      "\r"
    };
    private const int _drawCount = 88;
    [SerializeField]
    private CanvasGroup _advWindowRootCG;
    [SerializeField]
    private CanvasGroup _advWindowCG;
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private Image _windowImage;
    [SerializeField]
    private Image _nameFrameImage;
    [SerializeField]
    private Image _raycasterImage;
    [SerializeField]
    private UnityEngine.UI.Text _nameLabel;
    [SerializeField]
    private UnityEngine.UI.Text _messageLabel;
    [SerializeField]
    private TypefaceAnimatorEx _typefaceAnimator;
    private const int MAX_FONT_SPEED = 100;
    [SerializeField]
    [RangeReactiveProperty(1f, 100f)]
    private IntReactiveProperty _fontSpeed;
    [SerializeField]
    private ColorReactiveProperty _fontColor;
    private string[] _textBuffer;
    private int _currentLine;
    private bool _endAnimation;
    private bool _clicked;
    private HyphenationJpn _hypJpn;

    public CaptionSystem()
    {
      base.\u002Ector();
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

    public bool Visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._advWindowRootVisible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._advWindowRootVisible).set_Value(value);
      }
    }

    private BoolReactiveProperty _advWindowRootVisible { get; }

    private BoolReactiveProperty _advWindowVisible { get; }

    public Image Background
    {
      get
      {
        return this._backgroundImage;
      }
      set
      {
        this._backgroundImage = value;
      }
    }

    public Image RaycasterImage
    {
      get
      {
        return this._raycasterImage;
      }
    }

    public Action Action { get; set; }

    private StringReactiveProperty _name { get; }

    private StringReactiveProperty _message { get; }

    public CrossFade CrossFade { get; private set; }

    private void Awake()
    {
      this._hypJpn = ((Component) this._messageLabel).GetOrAddComponent<HyphenationJpn>();
      ((ReactiveProperty<string>) this._name).set_Value(string.Empty);
      UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._name, this._nameLabel);
      ((ReactiveProperty<string>) this._message).set_Value(string.Empty);
      UnityUIComponentExtensions.SubscribeToText((IObservable<string>) this._message, this._messageLabel);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._advWindowRootVisible, (Action<M0>) (isOn =>
      {
        this._advWindowRootCG.set_alpha(!isOn ? 0.0f : 1f);
        this._advWindowRootCG.set_blocksRaycasts(isOn);
      })), (Component) this._advWindowRootCG);
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._advWindowVisible, (Action<M0>) (isOn =>
      {
        this._advWindowCG.set_alpha(!isOn ? 0.0f : 1f);
        this._advWindowCG.set_blocksRaycasts(isOn);
      })), (Component) this._advWindowCG);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) Observable.Select<string, bool>((IObservable<M0>) this._message, (Func<M0, M1>) (s => !s.IsNullOrEmpty())), (Action<M0>) (isOn => ((ReactiveProperty<bool>) this._advWindowVisible).set_Value(isOn)));
      ((Behaviour) this).set_enabled(false);
      ((Graphic) this._windowImage).set_color(Color.op_Subtraction(Color.get_black(), Color.op_Multiply(Color.get_black(), 0.5f)));
      if (Object.op_Inequality((Object) this._nameFrameImage, (Object) null))
        ((Graphic) this._nameFrameImage).set_color(Color.get_black());
      ObservableExtensions.Subscribe<int>(Observable.TakeUntilDestroy<int>((IObservable<M0>) this._fontSpeed, (Component) this), (Action<M0>) (value =>
      {
        this._typefaceAnimator.isNoWait = value == 100;
        if (this._typefaceAnimator.isNoWait)
          return;
        this._typefaceAnimator.timeMode = TypefaceAnimatorEx.TimeMode.Speed;
        this._typefaceAnimator.speed = (float) value;
      }));
    }

    private void Start()
    {
      CanvasGroup canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (!Object.op_Inequality((Object) canvasGroup, (Object) null))
        return;
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryGameObjectUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => Mathf.Approximately(canvasGroup.get_alpha(), 1f))), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      if (Singleton<Scene>.IsInstance() && Singleton<Scene>.Instance.IsNowLoadingFade || Singleton<MapUIContainer>.IsInstance() && MapUIContainer.FadeCanvas.IsFading || (!Singleton<Game>.IsInstance() || Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null) || (Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null) || Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null))) || (Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null) || !this.Visible || (!((ReactiveProperty<bool>) this._advWindowVisible).get_Value() || !Input.GetKeyDown((KeyCode) 284))))
        return;
      ConfigWindow.TitleChangeAction = (Action) (() => Singleton<Game>.Instance.Dialog.TimeScale = 1f);
      Singleton<Game>.Instance.LoadConfig();
    }

    public bool ChangeChara(int id)
    {
      return false;
    }

    public void Clear()
    {
      ((ReactiveProperty<string>) this._name).set_Value(string.Empty);
      ((ReactiveProperty<string>) this._message).set_Value(string.Empty);
    }

    public void SetName(string name)
    {
      ((Behaviour) this._nameLabel).set_enabled(false);
      ((ReactiveProperty<string>) this._name).set_Value(name);
      ((Behaviour) this._nameLabel).set_enabled(true);
    }

    public void SetText(string text, bool noWait = false)
    {
      ((Behaviour) this).set_enabled(true);
      ((ReactiveProperty<string>) this._message).set_Value(text);
      if (this._hypJpn != null)
        this._hypJpn.SetText(text);
      this.FontSpeed = Manager.Config.GameData.FontSpeed;
      if (noWait && !this._typefaceAnimator.isNoWait)
        this.FontSpeed = 100;
      this._typefaceAnimator.Play();
    }

    public bool IsCompleteDisplayText
    {
      get
      {
        return !this._typefaceAnimator.isPlaying;
      }
    }

    public void ForceCompleteDisplayText()
    {
      this._typefaceAnimator.progress = 1f;
    }

    private List<string> AnalysisText(string text)
    {
      List<string> stringList = new List<string>();
      int startIndex = 0;
      text = text.Replace("\n", CaptionSystem._replaceStrings);
      StringBuilder stringBuilder = new StringBuilder();
      do
      {
        int length = 29;
        stringBuilder.Length = 0;
        for (int index = 0; index < 3; ++index)
        {
          if (startIndex + length > text.Length)
          {
            length += text.Length - (startIndex + length);
            if (length <= 0)
              break;
          }
          string str1 = text.Substring(startIndex, length);
          int num = str1.IndexOf("\n");
          if (num < 0)
          {
            stringBuilder.Append(str1);
            startIndex += length;
          }
          else
          {
            string str2 = str1.Substring(0, num + 1);
            stringBuilder.Append(str2);
            startIndex += str2.Length;
          }
        }
        stringList.Add(stringBuilder.ToString());
      }
      while (startIndex < text.Length);
      return stringList;
    }

    private void OnStartText()
    {
      this._endAnimation = false;
    }

    private void OnEndText()
    {
      if (this._currentLine < this._textBuffer.Length)
        this.NextLine(false);
      else
        this._endAnimation = true;
    }

    private void NextLine(bool noWait = false)
    {
      StringBuilder stringBuilder = StringBuilderPool.Get();
      stringBuilder.Append(((ReactiveProperty<string>) this._name).get_Value());
      stringBuilder.Append("： ");
      stringBuilder.Append(this._textBuffer[this._currentLine]);
      ((ReactiveProperty<string>) this._message).set_Value(stringBuilder.ToString());
      ++this._currentLine;
      this.FontSpeed = Manager.Config.GameData.FontSpeed;
      if (noWait && !this._typefaceAnimator.isNoWait)
        this.FontSpeed = 100;
      this._typefaceAnimator.Play();
    }
  }
}
