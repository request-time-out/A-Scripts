// Decompiled with JetBrains decompiler
// Type: AIProject.Scene.ConfirmScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.UI;
using Manager;
using ReMotion;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.Scene
{
  public class ConfirmScene : MenuUIBehaviour
  {
    private float _timeScale = 1f;
    private IntReactiveProperty _selectedID = new IntReactiveProperty(1);
    [SerializeField]
    private Image _panel;
    [SerializeField]
    private Image _back;
    [SerializeField]
    private Image _sentenceTextBack;
    [SerializeField]
    private Text _sentenceText;
    [SerializeField]
    private Text _yesText;
    [SerializeField]
    private Text _noText;
    [SerializeField]
    private ConfirmationButton _runButton;
    [SerializeField]
    private ConfirmationButton _cancelButton;
    [SerializeField]
    private CanvasGroup _backgroundCanvasGroup;
    [SerializeField]
    private CanvasGroup _panelCanvasGroup;
    private Input.ValidType _validType;
    private ConfirmationButton _selectedButton;

    public float TimeScale
    {
      get
      {
        return this._timeScale;
      }
      set
      {
        this._timeScale = value;
      }
    }

    public static Sprite Sprite { get; set; }

    public static string Sentence { get; set; }

    public static Func<string> YesTextFunc { get; set; }

    public static Func<string> NoTextFunc { get; set; }

    public static Action OnClickedYes { get; set; }

    public static Action OnClickedNo { get; set; }

    public static bool CloseImmediately { get; set; }

    public static float? BackAlpha { get; set; }

    private Color _backColor { get; set; }

    public static Vector2? Offset { get; set; }

    private Vector2 _offset { get; set; }

    private RectTransform _offsetTarget { get; set; }

    private void DisableRaycast()
    {
      this._runButton.DisableRaycast();
      this._cancelButton.DisableRaycast();
    }

    protected override void Awake()
    {
      if (Singleton<Game>.IsInstance())
        Singleton<Game>.Instance.Dialog = this;
      this._timeScale = Time.get_timeScale();
      Time.set_timeScale(0.0f);
      this._backColor = ((Graphic) this._panel).get_color();
      this._offsetTarget = ((Graphic) this._back).get_rectTransform();
      this._offset = this._offsetTarget.get_anchoredPosition();
      if (!Singleton<Input>.IsInstance())
        return;
      this._validType = Singleton<Input>.Instance.State;
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
    }

    protected override void Start()
    {
      this._back.set_sprite(ConfirmScene.Sprite);
      this._runButton.AddListener((Action) (() =>
      {
        Action onClickedYes = ConfirmScene.OnClickedYes;
        if (onClickedYes == null)
          return;
        onClickedYes();
      }));
      this._runButton.AddListener((Action) (() => this.DisableRaycast()));
      this._cancelButton.AddListener((Action) (() => this.DisableRaycast()));
      this._cancelButton.AddListener((Action) (() =>
      {
        Action onClickedNo = ConfirmScene.OnClickedNo;
        if (onClickedNo == null)
          return;
        onClickedNo();
      }));
      this._runButton.AddListener((Action) (() => this.Close()));
      this._cancelButton.AddListener((Action) (() => this.Close()));
      this.Open();
      this._sentenceText.set_text(ConfirmScene.Sentence);
      if (ConfirmScene.YesTextFunc != null)
        this._yesText.set_text(ConfirmScene.YesTextFunc());
      if (ConfirmScene.NoTextFunc != null)
        this._noText.set_text(ConfirmScene.NoTextFunc());
      if (ConfirmScene.BackAlpha.HasValue)
      {
        float num = ConfirmScene.BackAlpha.Value;
        Color color = ((Graphic) this._panel).get_color();
        color.a = (__Null) (double) num;
        ((Graphic) this._panel).set_color(color);
        ((Behaviour) this._sentenceTextBack).set_enabled((double) num <= 0.0);
      }
      else
        ((Behaviour) this._sentenceTextBack).set_enabled(false);
      if (ConfirmScene.Offset.HasValue)
        this._offsetTarget.set_anchoredPosition(Vector2.op_Addition(this._offsetTarget.get_anchoredPosition(), ConfirmScene.Offset.Value));
      base.Start();
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand();
      actionIdDownCommand1.ActionID = ActionID.Submit;
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__6)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand();
      actionIdDownCommand2.ActionID = ActionID.Cancel;
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__7)));
      this._actionCommands.Add(actionIdDownCommand2);
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._selectedID, (Action<M0>) (x => this.UpdateSelectedFrame(x)));
      this.UpdateSelectedFrame(1);
      // ISSUE: method pointer
      this._runButton.OnPointerEnterEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      // ISSUE: method pointer
      this._cancelButton.OnPointerEnterEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      base.Start();
    }

    private void UpdateSelectedFrame(int x)
    {
      this._runButton.IsActiveSelectedFrame = x == 0;
      this._cancelButton.IsActiveSelectedFrame = x == 1;
      if (x == 0)
        this._selectedButton = this._runButton;
      else
        this._selectedButton = this._cancelButton;
    }

    private void Open()
    {
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.1f, true), true), (Action<M0>) (x =>
      {
        CanvasGroup backgroundCanvasGroup = this._backgroundCanvasGroup;
        float num1 = ((TimeInterval<float>) ref x).get_Value();
        this._panelCanvasGroup.set_alpha(num1);
        double num2 = (double) num1;
        backgroundCanvasGroup.set_alpha((float) num2);
      }), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() => this._panelCanvasGroup.set_blocksRaycasts(true)));
    }

    private void Close()
    {
      if (ConfirmScene.CloseImmediately)
      {
        this._backgroundCanvasGroup.set_alpha(0.0f);
        this._panelCanvasGroup.set_blocksRaycasts(false);
        Object.Destroy((Object) ((Component) this).get_gameObject());
        GC.Collect();
        Resources.UnloadUnusedAssets();
      }
      else
      {
        this._panelCanvasGroup.set_blocksRaycasts(false);
        ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.1f, true), true), (Action<M0>) (x =>
        {
          CanvasGroup backgroundCanvasGroup = this._backgroundCanvasGroup;
          float num1 = 1f - ((TimeInterval<float>) ref x).get_Value();
          this._panelCanvasGroup.set_alpha(num1);
          double num2 = (double) num1;
          backgroundCanvasGroup.set_alpha((float) num2);
        }), (Action<Exception>) (ex => Debug.LogException(ex)), (Action) (() =>
        {
          Object.Destroy((Object) ((Component) this).get_gameObject());
          GC.Collect();
          Resources.UnloadUnusedAssets();
        }));
      }
    }

    protected override void OnDisable()
    {
      if (Singleton<Game>.IsInstance())
        Singleton<Game>.Instance.Dialog = (ConfirmScene) null;
      Time.set_timeScale(this._timeScale);
      ((Graphic) this._panel).set_color(this._backColor);
      this._offsetTarget.set_anchoredPosition(this._offset);
      ConfirmScene.BackAlpha = new float?();
      ConfirmScene.Offset = new Vector2?();
      ConfirmScene.YesTextFunc = (Func<string>) null;
      ConfirmScene.NoTextFunc = (Func<string>) null;
      if (!Singleton<Input>.IsInstance())
        return;
      Singleton<Input>.Instance.ReserveState(this._validType);
      Singleton<Input>.Instance.SetupState();
    }

    protected void OnSubmit()
    {
      this._selectedButton.Invoke();
    }

    protected void OnCancel()
    {
      this._cancelButton.Invoke();
    }

    public override void OnInputMoveDirection(MoveDirection moveDir)
    {
      if (moveDir != null)
      {
        if (moveDir != 2)
          return;
        IntReactiveProperty selectedId = this._selectedID;
        ((ReactiveProperty<int>) selectedId).set_Value(((ReactiveProperty<int>) selectedId).get_Value() + 1);
        if (((ReactiveProperty<int>) this._selectedID).get_Value() <= 1)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(0);
      }
      else
      {
        IntReactiveProperty selectedId = this._selectedID;
        ((ReactiveProperty<int>) selectedId).set_Value(((ReactiveProperty<int>) selectedId).get_Value() - 1);
        if (((ReactiveProperty<int>) this._selectedID).get_Value() >= 0)
          return;
        ((ReactiveProperty<int>) this._selectedID).set_Value(1);
      }
    }
  }
}
