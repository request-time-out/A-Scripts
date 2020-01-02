// Decompiled with JetBrains decompiler
// Type: AIProject.UI.TutorialUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.Definitions;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class TutorialUI : MenuUIBehaviour
  {
    [SerializeField]
    private int _elementBackImageAlpha = 245;
    [SerializeField]
    private float _pageFadeTime = 0.5f;
    [SerializeField]
    private float _pageMoveX = 20f;
    [SerializeField]
    private float _groupFadeTime = 0.5f;
    private Dictionary<int, List<TutorialUIElement>> ElementTable = new Dictionary<int, List<TutorialUIElement>>();
    private Dictionary<int, Tuple<int, CanvasGroup, IDisposable>> ElementRootTable = new Dictionary<int, Tuple<int, CanvasGroup, IDisposable>>();
    private Dictionary<int, CanvasGroup> _elementRootTable = new Dictionary<int, CanvasGroup>();
    private CommandLabel.AcceptionState _prevCommandAcceptionState = CommandLabel.AcceptionState.None;
    [SerializeField]
    private CanvasGroup _rootCanvas;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private CanvasGroup _backgroundCanvas;
    [SerializeField]
    private Transform _elementRoot;
    private TutorialUIElement _currentElement;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private TutorialGroupListUI _listUI;
    [SerializeField]
    private TutorialLoadingImageUI _loadingImageUI;
    private MenuUIBehaviour[] uiElements;
    private Tuple<int, CanvasGroup, IDisposable> CurrentElementRoot;
    private List<TutorialUIElement> _currentElementList;
    private bool _initialize;
    private IDisposable _fadeDisposable;
    private bool _prevPlayerScheduledInteractionState;
    private Manager.Input.ValidType _prevInputState;
    private MenuUIBehaviour[] _prevMenuUIBehaviour;

    public int OpenElementNumber { get; protected set; } = -1;

    public int OpenIndex { get; private set; }

    public bool OpenGroupEnabled { get; protected set; }

    public float TimeScale { get; set; }

    public float PrevTimeScale { get; private set; }

    public System.Action ClosedEvent { get; set; }

    public override bool IsActiveControl
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActive).get_Value();
      }
      set
      {
        if (((ReactiveProperty<bool>) this._isActive).get_Value() == value)
          return;
        ((ReactiveProperty<bool>) this._isActive).set_Value(value);
      }
    }

    protected float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._rootCanvas, (Object) null) ? this._rootCanvas.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._rootCanvas, (Object) null))
          return;
        this._rootCanvas.set_alpha(value);
      }
    }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && Singleton<Manager.Input>.Instance.FocusLevel == this._focusLevel;
      }
    }

    private MenuUIBehaviour[] MenuUIElements
    {
      get
      {
        MenuUIBehaviour[] uiElements = this.uiElements;
        if (uiElements != null)
          return uiElements;
        return this.uiElements = new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._listUI
        };
      }
    }

    protected override void Awake()
    {
      if (Object.op_Equality((Object) this._rootCanvas, (Object) null))
        this._rootCanvas = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._rectTransform, (Object) null))
        this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      if (Object.op_Equality((Object) this._listUI, (Object) null))
        this._listUI = (TutorialGroupListUI) ((Component) this).GetComponentInChildren<TutorialGroupListUI>(true);
      if (!Object.op_Equality((Object) this._loadingImageUI, (Object) null))
        return;
      this._loadingImageUI = (TutorialLoadingImageUI) ((Component) this).GetComponentInChildren<TutorialLoadingImageUI>(true);
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (System.Action<M0>) (x => this.SetActiveControl(x)));
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._keyCommands.Add(keyCodeDownCommand);
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._rightButton), (Func<M0, bool>) (_ => this.InputEnabled)), (System.Action<M0>) (_ => this.PageMove(1)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._leftButton), (Func<M0, bool>) (_ => this.InputEnabled)), (System.Action<M0>) (_ => this.PageMove(-1)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (System.Action<M0>) (_ => this.DoClose()));
    }

    public void DoClose()
    {
      this.IsActiveControl = false;
    }

    protected override void OnAfterStart()
    {
      if (this._canvasGroup.get_blocksRaycasts())
        this._canvasGroup.set_blocksRaycasts(false);
      if (this._canvasGroup.get_interactable())
        this._canvasGroup.set_interactable(false);
      this.CanvasAlpha = 0.0f;
      this.SetActive((Component) this._canvasGroup, false);
      IEnumerator coroutine = this.CreateElements();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator CreateElements()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUI.\u003CCreateElements\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    public void SetCondition(int index, bool openGroup = false)
    {
      this.OpenElementNumber = index;
      this.OpenGroupEnabled = openGroup;
    }

    public void SetCondition(Popup.Tutorial.Type type, bool openGroup = false)
    {
      this.OpenElementNumber = (int) type;
      this.OpenGroupEnabled = openGroup;
    }

    public bool BlockRaycastEnabled
    {
      get
      {
        return this._backgroundCanvas.get_blocksRaycasts();
      }
      set
      {
        this._backgroundCanvas.set_blocksRaycasts(value);
      }
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUI.\u003COpenCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialUI.\u003CCloseCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void SetupElement(int openGroup)
    {
      bool flag = this.CurrentElementRoot == null;
      if (this.CurrentElementRoot != null && this.CurrentElementRoot.Item1 != openGroup)
      {
        this.FadeOutRoot(this.CurrentElementRoot);
        this.CurrentElementRoot = (Tuple<int, CanvasGroup, IDisposable>) null;
      }
      List<TutorialUIElement> tutorialUiElementList;
      if (!this.ElementTable.TryGetValue(openGroup, out tutorialUiElementList))
      {
        this.SetActive((Component) this._rightButton, false);
        this.SetActive((Component) this._leftButton, false);
        this.SetActive((Component) this._closeButton, !this.OpenGroupEnabled);
      }
      else
      {
        this.ElementRootTable.TryGetValue(openGroup, out this.CurrentElementRoot);
        if (this.CurrentElementRoot != null)
        {
          if (flag)
          {
            this.CurrentElementRoot.Item3?.Dispose();
            this.CurrentElementRoot.Item2.set_alpha(1f);
          }
          else
            this.FadeInRoot(this.CurrentElementRoot);
          ((Component) this.CurrentElementRoot.Item2)?.get_transform()?.SetAsLastSibling();
        }
        this._currentElementList = tutorialUiElementList;
        if (!this._currentElementList.IsNullOrEmpty<TutorialUIElement>())
        {
          for (int index = 0; index < this._currentElementList.Count; ++index)
          {
            TutorialUIElement element = this._currentElementList.GetElement<TutorialUIElement>(index);
            if (!Object.op_Equality((Object) element, (Object) null))
            {
              element.CanvasAlpha = index != 0 || flag ? 0.0f : 1f;
              if (Object.op_Inequality((Object) element.BackImage, (Object) null))
              {
                Color color = ((Graphic) element.BackImage).get_color();
                color.a = (__Null) ((double) this._elementBackImageAlpha / (double) byte.MaxValue);
                ((Graphic) element.BackImage).set_color(color);
              }
            }
          }
        }
        this.PageMove(0);
      }
    }

    private void SetupGroupListUI(int groupID)
    {
      if (this.OpenGroupEnabled)
      {
        this.SetActive((Component) this._listUI, true);
        this._listUI.RefreshElements();
        this._listUI.SelectButton(groupID);
        this._loadingImageUI.PageIndex = 0;
      }
      else
        this.SetActive((Component) this._listUI, false);
    }

    private void FadeInRoot(Tuple<int, CanvasGroup, IDisposable> root)
    {
      if (root == null)
        return;
      root.Item3?.Dispose();
      float startAlpha = root.Item2.get_alpha();
      root.Item3 = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._groupFadeTime, true), true), (Component) root.Item2), (System.Action<M0>) (x => root.Item2.set_alpha(Mathf.Lerp(startAlpha, 1f, ((TimeInterval<float>) ref x).get_Value()))));
    }

    private void FadeOutRoot(Tuple<int, CanvasGroup, IDisposable> root)
    {
      if (root == null)
        return;
      root.Item3?.Dispose();
      float startAlpha = root.Item2.get_alpha();
      root.Item3 = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._groupFadeTime, true), true), (Component) root.Item2), (System.Action<M0>) (x => root.Item2.set_alpha(Mathf.Lerp(startAlpha, 0.0f, ((TimeInterval<float>) ref x).get_Value()))));
    }

    private void CloseAllRoot()
    {
      if (!this.ElementRootTable.IsNullOrEmpty<int, Tuple<int, CanvasGroup, IDisposable>>())
      {
        using (Dictionary<int, Tuple<int, CanvasGroup, IDisposable>>.Enumerator enumerator = this.ElementRootTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Tuple<int, CanvasGroup, IDisposable> tuple = enumerator.Current.Value;
            tuple.Item3?.Dispose();
            if (Object.op_Inequality((Object) tuple.Item2, (Object) null))
              tuple.Item2.set_alpha(0.0f);
          }
        }
      }
      this.CurrentElementRoot = (Tuple<int, CanvasGroup, IDisposable>) null;
    }

    private void AllClose()
    {
      this.CloseAllRoot();
      foreach (KeyValuePair<int, List<TutorialUIElement>> keyValuePair in this.ElementTable)
      {
        if (!keyValuePair.Value.IsNullOrEmpty<TutorialUIElement>())
        {
          foreach (TutorialUIElement tutorialUiElement in keyValuePair.Value)
            tutorialUiElement.CloseForced();
        }
      }
      this._currentElement = (TutorialUIElement) null;
    }

    private void PageMove(int moveIndex)
    {
      int openIndex = this.OpenIndex;
      int idx = this.OpenIndex + moveIndex;
      List<TutorialUIElement> currentElementList = this._currentElementList;
      if (!this.ListRange<TutorialUIElement>(currentElementList, idx))
        return;
      this.OpenIndex = idx;
      if (openIndex == this.OpenIndex)
      {
        if (this._currentElement != null)
          this._currentElement.Close(this._pageFadeTime, 0.0f);
        this._currentElement = currentElementList.GetElement<TutorialUIElement>(this.OpenIndex);
        if (this._currentElement != null)
          this._currentElement.Open(this._pageFadeTime, 0.0f);
      }
      else
      {
        this.PlaySE(SoundPack.SystemSE.Page);
        float moveX = this._pageMoveX * Mathf.Sign((float) moveIndex);
        if (this._currentElement != null)
          this._currentElement.Close(this._pageFadeTime, moveX);
        this._currentElement = currentElementList.GetElement<TutorialUIElement>(this.OpenIndex);
        if (this._currentElement != null)
          this._currentElement.Open(this._pageFadeTime, moveX);
      }
      bool flag1 = this.ListRange<TutorialUIElement>(currentElementList, this.OpenIndex - 1);
      bool flag2 = this.ListRange<TutorialUIElement>(currentElementList, this.OpenIndex + 1);
      if (!flag1 && !flag2)
      {
        this.SetActive((Component) this._rightButton, false);
        this.SetActive((Component) this._leftButton, false);
        this.SetActive((Component) this._closeButton, !this.OpenGroupEnabled);
      }
      else if (flag1 && flag2)
      {
        this.SetActive((Component) this._rightButton, true);
        this.SetActive((Component) this._leftButton, true);
        this.SetActive((Component) this._closeButton, false);
      }
      else if (flag2)
      {
        this.SetActive((Component) this._rightButton, true);
        this.SetActive((Component) this._leftButton, false);
        this.SetActive((Component) this._closeButton, false);
      }
      else
      {
        if (!flag1)
          return;
        this.SetActive((Component) this._rightButton, false);
        this.SetActive((Component) this._leftButton, true);
        this.SetActive((Component) this._closeButton, !this.OpenGroupEnabled);
      }
    }

    public void ChangeUIGroup(int groupIndex)
    {
      if (this.OpenElementNumber == groupIndex)
        return;
      this.OpenIndex = 0;
      int openGroup = groupIndex;
      this.OpenElementNumber = openGroup;
      this.SetupElement(openGroup);
    }

    private void SetAllEnabledInput(bool active)
    {
      if (this.MenuUIElements.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour menuUiElement in this.MenuUIElements)
      {
        if (Object.op_Inequality((Object) menuUiElement, (Object) null) && menuUiElement.EnabledInput != active)
          menuUiElement.EnabledInput = active;
      }
    }

    private void SetAllFocusLevel(int level)
    {
      if (this.MenuUIElements.IsNullOrEmpty<MenuUIBehaviour>())
        return;
      foreach (MenuUIBehaviour menuUiElement in this.MenuUIElements)
      {
        if (Object.op_Inequality((Object) menuUiElement, (Object) null))
          menuUiElement.SetFocusLevel(level);
      }
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
      if (Object.op_Equality((Object) soundPack, (Object) null))
        return;
      soundPack.Play(se);
    }

    private bool ListRange<T>(List<T> list, int idx)
    {
      return !list.IsNullOrEmpty<T>() && 0 <= idx && idx < list.Count;
    }

    private bool ArrayRange<T>(T[] array, int idx)
    {
      return !array.IsNullOrEmpty<T>() && 0 <= idx && idx < array.Length;
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (Object.op_Equality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }

    private void SetActive(Component com, bool active)
    {
      if (Object.op_Equality((Object) com, (Object) null) || Object.op_Equality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return;
      com.get_gameObject().SetActive(active);
    }
  }
}
