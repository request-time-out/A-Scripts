// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemFilterCategoryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI.Viewer;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ItemFilterCategoryUI : MenuUIBehaviour
  {
    private readonly Dictionary<int, ItemFilterCategoryUI.LabelAndButton> categorize = new Dictionary<int, ItemFilterCategoryUI.LabelAndButton>();
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _selectedID = new IntReactiveProperty(0);
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _categoryID = new IntReactiveProperty(0);
    private StringReactiveProperty categoryTitle = new StringReactiveProperty(string.Empty);
    private Vector3 _velocity = Vector3.get_zero();
    private Vector3 _selectedVelocity = Vector3.get_zero();
    [SerializeField]
    private ScrollRect scroll;
    [SerializeField]
    private GameObject _rootCategoryButton;
    [SerializeField]
    private Image _selectedCursorFrame;
    private CanvasGroup _cursorCanvasGroup;
    [SerializeField]
    private Image _cursorFrame;
    [SerializeField]
    private Image _sortCursorFrame;
    [SerializeField]
    private Text _CategoryLabel;
    [SerializeField]
    private Button _leftButton;
    [SerializeField]
    private Button _rightButton;
    private float _alphaVelocity;

    public PlaySE playSE { get; } = new PlaySE(false);

    public int SelectedID
    {
      get
      {
        return ((ReactiveProperty<int>) this._selectedID).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._selectedID).set_Value(value);
      }
    }

    public int CategoryID
    {
      get
      {
        return ((ReactiveProperty<int>) this._categoryID).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._categoryID).set_Value(value);
      }
    }

    public CanvasGroup CursorCanvasGroup
    {
      get
      {
        return this._cursorCanvasGroup ?? (this._cursorCanvasGroup = (CanvasGroup) ((Component) this._cursorFrame).GetComponent<CanvasGroup>());
      }
    }

    public Button leftButton
    {
      get
      {
        return this._leftButton;
      }
    }

    public Button rightButton
    {
      get
      {
        return this._rightButton;
      }
    }

    public bool useCursor
    {
      get
      {
        return this._useCursor;
      }
      set
      {
        this._useCursor = value;
      }
    }

    private bool _useCursor { get; set; }

    public Button SelectedButton { get; private set; }

    public Button CurrentCategoryButton { get; private set; }

    public Action OnEntered { get; set; }

    public UnityEvent OnSubmit { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    private Vector2 scrollPos
    {
      get
      {
        return Object.op_Equality((Object) this.scroll.get_content(), (Object) null) ? Vector2.get_zero() : this.scroll.get_content().get_anchoredPosition();
      }
      set
      {
        if (!Object.op_Inequality((Object) this.scroll.get_content(), (Object) null))
          return;
        this.scroll.get_content().set_anchoredPosition(value);
      }
    }

    private float layoutWidthSpacing { get; set; }

    public void SetSelectAndCategory(int value)
    {
      this.playSE.use = false;
      ((ReactiveProperty<int>) this._selectedID).SetValueAndForceNotify(value);
      ((ReactiveProperty<int>) this._categoryID).SetValueAndForceNotify(value);
      this.playSE.use = true;
      int num = ((IEnumerable<int>) this.Visibles).Select<int, int>((Func<int, int, int>) ((_, i) => i)).FirstOrDefault<int>();
      Vector2 scrollPos = this.scrollPos;
      scrollPos.x = (__Null) ((double) num * (double) this.layoutWidthSpacing);
      this.scrollPos = scrollPos;
      this.ScrollNormalized();
    }

    public void Filter(params int[] visibles)
    {
      bool flag = !((IEnumerable<int>) visibles).Any<int>();
      foreach (KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton> keyValuePair in this.categorize)
        keyValuePair.Value.Visible = flag || ((IEnumerable<int>) visibles).Contains<int>(keyValuePair.Key);
      this.SetSelectAndCategory(((IEnumerable<int>) this.Visibles).FirstOrDefault<int>());
    }

    public bool SetButtonListener(UnityAction<int> action)
    {
      if (!this.categorize.Any<KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton>>())
        return false;
      foreach (KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton> keyValuePair in this.categorize)
      {
        KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton> item = keyValuePair;
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.Value.button), (Action<M0>) (_ => action.Invoke(item.Key)));
      }
      return true;
    }

    public int[] Visibles
    {
      get
      {
        return this.categorize.Where<KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton>>((Func<KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton>, bool>) (v => v.Value.Visible)).Select<KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton>, int>((Func<KeyValuePair<int, ItemFilterCategoryUI.LabelAndButton>, int>) (v => v.Key)).ToArray<int>();
      }
    }

    [DebuggerHidden]
    private IEnumerator CreateCategoryIcon()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ItemFilterCategoryUI.\u003CCreateCategoryIcon\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      if (!Application.get_isPlaying())
        return;
      float num1 = 0.0f;
      if (Object.op_Inequality((Object) this._rootCategoryButton, (Object) null))
      {
        Rect rect = ((RectTransform) this._rootCategoryButton.GetComponent<RectTransform>()).get_rect();
        num1 = ((Rect) ref rect).get_width();
      }
      float num2 = 0.0f;
      if (Object.op_Inequality((Object) this.scroll.get_content(), (Object) null))
      {
        HorizontalOrVerticalLayoutGroup component = (HorizontalOrVerticalLayoutGroup) ((Component) this.scroll.get_content()).GetComponent<HorizontalOrVerticalLayoutGroup>();
        if (Object.op_Inequality((Object) component, (Object) null))
          num2 = component.get_spacing();
      }
      this.layoutWidthSpacing = num1 + num2;
      this.scroll.set_scrollSensitivity(-this.layoutWidthSpacing);
      ((MonoBehaviour) this).StartCoroutine(this.CreateCategoryIcon());
      ObservableExtensions.Subscribe<float>(Observable.Merge<float>((IObservable<M0>[]) new IObservable<float>[2]
      {
        (IObservable<float>) Observable.Select<Unit, float>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._leftButton), (Func<M0, M1>) (__ => this.layoutWidthSpacing)),
        (IObservable<float>) Observable.Select<Unit, float>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._rightButton), (Func<M0, M1>) (__ => -this.layoutWidthSpacing))
      }), (Action<M0>) (x =>
      {
        Vector2 scrollPos = this.scrollPos;
        ref Vector2 local = ref scrollPos;
        local.x = (__Null) (local.x + (double) x);
        this.scrollPos = scrollPos;
        this.playSE.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__8)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__9)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      this._actionCommands.Add(actionIdDownCommand3);
      ActionIDDownCommand actionIdDownCommand4 = new ActionIDDownCommand()
      {
        ActionID = ActionID.LeftShoulder1
      };
      // ISSUE: method pointer
      actionIdDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__B)));
      this._actionCommands.Add(actionIdDownCommand4);
      ActionIDDownCommand actionIdDownCommand5 = new ActionIDDownCommand()
      {
        ActionID = ActionID.RightShoulder1
      };
      // ISSUE: method pointer
      actionIdDownCommand5.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__C)));
      this._actionCommands.Add(actionIdDownCommand5);
      base.Start();
    }

    private void OnUpdate()
    {
      this.ScrollNormalized();
      this.CursorCanvasGroup.set_alpha(Smooth.Damp(this.CursorCanvasGroup.get_alpha(), !this.EnabledInput || this.FocusLevel != Singleton<Input>.Instance.FocusLevel || !this._useCursor || !Object.op_Inequality((Object) this.SelectedButton, (Object) null) ? 0.0f : 1f, ref this._alphaVelocity, this._alphaAccelerationTime));
      if (Object.op_Inequality((Object) this.SelectedButton, (Object) null))
        CursorFrame.Set(((Graphic) this._cursorFrame).get_rectTransform(), (RectTransform) ((Component) this.SelectedButton).GetComponent<RectTransform>(), ref this._velocity, new float?(), new float?(0.025f));
      if (!Object.op_Inequality((Object) this.CurrentCategoryButton, (Object) null))
        return;
      CursorFrame.Set(((Graphic) this._selectedCursorFrame).get_rectTransform(), (RectTransform) ((Component) this.CurrentCategoryButton).GetComponent<RectTransform>(), ref this._selectedVelocity, new float?(), new float?(0.025f));
    }

    private void ScrollNormalized()
    {
      float num = this.scroll.get_horizontalNormalizedPosition();
      ((Selectable) this._leftButton).set_interactable((double) num > 0.0);
      ((Selectable) this._rightButton).set_interactable((double) num < 1.0);
      if ((double) num < 0.001)
        num = 0.0f;
      else if ((double) num > 0.999)
        num = 1f;
      this.scroll.set_horizontalNormalizedPosition(num);
    }

    private void OnInputSubmit()
    {
      this.OnSubmit?.Invoke();
    }

    private void OnInputCancel()
    {
      this.OnCancel?.Invoke();
    }

    private class LabelAndButton
    {
      public readonly string label;
      public readonly Button button;
      private readonly GameObject gameObject;

      public LabelAndButton(string label, Button button)
      {
        this.label = label;
        this.button = button;
        this.gameObject = ((Component) button).get_gameObject();
      }

      public bool Visible
      {
        get
        {
          return !Object.op_Equality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
        }
        set
        {
          if (!Object.op_Inequality((Object) this.gameObject, (Object) null))
            return;
          this.gameObject.SetActive(value);
        }
      }
    }
  }
}
