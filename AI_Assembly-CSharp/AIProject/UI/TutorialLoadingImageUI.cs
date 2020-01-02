// Decompiled with JetBrains decompiler
// Type: AIProject.UI.TutorialLoadingImageUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
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
  [RequireComponent(typeof (CanvasGroup))]
  [RequireComponent(typeof (RectTransform))]
  public class TutorialLoadingImageUI : MenuUIBehaviour
  {
    [SerializeField]
    private float _pageFadeTime = 0.5f;
    [SerializeField]
    private float _pageMoveX = 20f;
    private List<TutorialLoadingImageUIElement> _elementPool = new List<TutorialLoadingImageUIElement>();
    private List<Sprite> _loadingSpriteList = new List<Sprite>();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Transform _elementRoot;
    [SerializeField]
    private GameObject _elementPrefab;
    [SerializeField]
    private TutorialUI _tutorialUI;
    [SerializeField]
    private TutorialGroupListUI _listUI;
    private TutorialLoadingImageUIElement _currentElement;
    private MenuUIBehaviour[] _prevMenuUIBehavioures;
    private MenuUIBehaviour[] _menuUIBehaviours;
    private bool _initialize;
    private IDisposable _fadeDisposable;
    private int _prevFocusLevel;

    public int PageIndex { get; set; }

    public MenuUIBehaviour[] MenuUIBehaviours
    {
      get
      {
        if (this._menuUIBehaviours == null)
          this._menuUIBehaviours = new MenuUIBehaviour[1]
          {
            (MenuUIBehaviour) this
          };
        return this._menuUIBehaviours;
      }
    }

    public bool InputEnabled
    {
      get
      {
        return this.EnabledInput && Singleton<Input>.Instance.FocusLevel == this._focusLevel;
      }
    }

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

    public float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      private set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (Object.op_Equality((Object) this._rectTransform, (Object) null))
        this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
      this.Hide();
    }

    private void Hide()
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        return;
      this.SetBlockRaycasts(false);
      this.SetInteractable(false);
      this.CanvasAlpha = 0.0f;
      this.EnabledInput = false;
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
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
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._closeButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.DoClose()));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._rightButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.PageMove(1)));
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._leftButton), (Func<M0, bool>) (_ => this.InputEnabled)), (Action<M0>) (_ => this.PageMove(-1)));
    }

    protected override void OnAfterStart()
    {
      base.OnAfterStart();
      this.SetActive(((Component) this).get_gameObject(), false);
      IEnumerator coroutine = this.LoadElementSprites();
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false)), (Component) this);
    }

    [DebuggerHidden]
    private IEnumerator LoadElementSprites()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialLoadingImageUI.\u003CLoadElementSprites\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void SetActiveControl(bool active)
    {
      IEnumerator coroutine = !active ? this.CloseCoroutine() : this.OpenCoroutine();
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Component) this), (Action<M0>) (_ => {}), (Action<Exception>) (ex =>
      {
        if (!Debug.get_isDebugBuild())
          return;
        Debug.LogException(ex);
      }));
    }

    private void DoClose()
    {
      this.PlaySE(SoundPack.SystemSE.Cancel);
      this.IsActiveControl = false;
    }

    private void PageMove(int move)
    {
      int num1 = Math.Sign(move);
      int index = this.PageIndex + move;
      Sprite sprite = this.GetSprite(ref index);
      if (this.PageIndex == index)
        return;
      if (this._currentElement != null)
        this._currentElement.Close(this._pageFadeTime, this._pageMoveX * (float) num1);
      this._currentElement = this.GetElement();
      TutorialLoadingImageUIElement currentElement = this._currentElement;
      int num2 = index;
      this.PageIndex = num2;
      int num3 = num2;
      currentElement.Index = num3;
      this._currentElement.ImageSprite = sprite;
      this._currentElement.Open(this._pageFadeTime, this._pageMoveX * (float) num1);
      this.PlaySE(SoundPack.SystemSE.Page);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialLoadingImageUI.\u003COpenCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new TutorialLoadingImageUI.\u003CCloseCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void SetupElement(int index)
    {
      if (Object.op_Inequality((Object) this._currentElement, (Object) null))
      {
        this.ReturnElement(this._currentElement);
        this._currentElement = (TutorialLoadingImageUIElement) null;
      }
      TutorialLoadingImageUIElement loadingImageUiElement = this._currentElement = this.GetElement();
      loadingImageUiElement.ImageSprite = this.GetSprite(ref index);
      loadingImageUiElement.Index = index;
      loadingImageUiElement.CanvasAlpha = 1f;
      ((Component) loadingImageUiElement).get_transform().set_localPosition(Vector3.get_zero());
    }

    private TutorialLoadingImageUIElement GetElement()
    {
      TutorialLoadingImageUIElement loadingImageUiElement = this._elementPool.PopFront<TutorialLoadingImageUIElement>();
      if (Object.op_Equality((Object) loadingImageUiElement, (Object) null))
        loadingImageUiElement = (TutorialLoadingImageUIElement) ((GameObject) Object.Instantiate<GameObject>((M0) this._elementPrefab, this._elementRoot, false)).GetComponent<TutorialLoadingImageUIElement>();
      loadingImageUiElement.CanvasAlpha = 0.0f;
      ((Component) loadingImageUiElement).get_transform().SetAsLastSibling();
      this.SetActive((Component) loadingImageUiElement, true);
      loadingImageUiElement.OnCloseEvent = (Action<TutorialLoadingImageUIElement>) (x => this.ReturnElement(x));
      return loadingImageUiElement;
    }

    private void ReturnElement(TutorialLoadingImageUIElement elm)
    {
      if (Object.op_Equality((Object) elm, (Object) null) || this._elementPool.Contains(elm))
        return;
      this.SetActive((Component) elm, false);
      this._elementPool.Add(elm);
    }

    private Sprite GetSprite(ref int index)
    {
      if (this._loadingSpriteList.IsNullOrEmpty<Sprite>())
      {
        index = -1;
        return (Sprite) null;
      }
      if (index < 0)
        index = this._loadingSpriteList.Count - 1;
      else if (this._loadingSpriteList.Count <= index)
        index = 0;
      return this._loadingSpriteList.GetElement<Sprite>(index);
    }

    private void SetInteractable(bool active)
    {
      if (this._canvasGroup.get_interactable() == active)
        return;
      this._canvasGroup.set_interactable(active);
    }

    private void SetBlockRaycasts(bool active)
    {
      if (this._canvasGroup.get_blocksRaycasts() == active)
        return;
      this._canvasGroup.set_blocksRaycasts(active);
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

    private void PlaySE(SoundPack.SystemSE se)
    {
      (!Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
