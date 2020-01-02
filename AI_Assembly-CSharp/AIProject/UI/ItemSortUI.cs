// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemSortUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.ColorDefine;
using AIProject.UI.Viewer;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ItemSortUI : MenuUIBehaviour
  {
    private BoolReactiveProperty _isOpen = new BoolReactiveProperty(false);
    private IntReactiveProperty _sortType = new IntReactiveProperty(0);
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _selectedID = new IntReactiveProperty(0);
    private Vector3 _velocity = Vector3.get_zero();
    private Toggle _selectedOptionInstance;
    [SerializeField]
    private Image _cursor;
    [SerializeField]
    private Button _close;
    private RectTransform _closeSize;
    private CanvasGroup _cursorCG;
    private CanvasGroup _sortPanelCG;
    private IDisposable _subscriber;
    private float _alphaVelocity;

    public PlaySE playSE { get; } = new PlaySE(false);

    public event Action<int> TypeChanged;

    public void SetDefault()
    {
      foreach (Toggle toggle in this._toggles)
        toggle.set_isOn(false);
      this._toggles[0].set_isOn(false);
    }

    public event Action OnEntered;

    public UnityEvent OnSubmit { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public void Open()
    {
      ((ReactiveProperty<bool>) this._isOpen).set_Value(true);
    }

    public void Close()
    {
      ((ReactiveProperty<bool>) this._isOpen).set_Value(false);
    }

    public bool isOpen
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isOpen).get_Value();
      }
    }

    private Toggle[] _toggles { get; set; }

    private RectTransform closeSize
    {
      get
      {
        return ((object) this).GetCacheObject<RectTransform>(ref this._closeSize, (Func<RectTransform>) (() => ((Component) this._close).get_transform().get_childCount() >= 1 ? ((Component) this._close).get_transform().GetChild(0) as RectTransform : (RectTransform) ((Component) this._close).GetComponent<RectTransform>()));
      }
    }

    private CanvasGroup cursorCG
    {
      get
      {
        return ((object) this).GetCacheObject<CanvasGroup>(ref this._cursorCG, (Func<CanvasGroup>) (() => ((Component) this._cursor).GetComponentCache<CanvasGroup>(ref this._cursorCG)));
      }
    }

    private CanvasGroup sortPanelCG
    {
      get
      {
        return ((Component) this).GetComponentCache<CanvasGroup>(ref this._sortPanelCG);
      }
    }

    private void OpenCloseAnimation(bool isOpen)
    {
      this.sortPanelCG.set_blocksRaycasts(isOpen);
      if (this._subscriber != null)
        this._subscriber.Dispose();
      this._subscriber = ObservableExtensions.Subscribe<float>((IObservable<M0>) Observable.Select<TimeInterval<float>, float>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), true), (Func<M0, M1>) (x => isOpen ? ((TimeInterval<float>) ref x).get_Value() : 1f - ((TimeInterval<float>) ref x).get_Value())), (Action<M0>) (alpha => this.sortPanelCG.set_alpha(alpha)), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    protected override void Start()
    {
      if (!Application.get_isPlaying())
        return;
      this._toggles = (Toggle[]) ((Component) this).GetComponentsInChildren<Toggle>(true);
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._isOpen, (Action<M0>) (isOn =>
      {
        if (!isOn)
        {
          ((ReactiveProperty<int>) this._selectedID).set_Value(-1);
          this.playSE.Play(SoundPack.SystemSE.Cancel);
        }
        this.OpenCloseAnimation(isOn);
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._selectedID, (Action<M0>) (x =>
      {
        Toggle element = this._toggles.GetElement<Toggle>(x);
        if (Object.op_Equality((Object) element, (Object) null))
          this._selectedOptionInstance = (Toggle) null;
        else
          this._selectedOptionInstance = element;
      }));
      this.cursorCG.set_alpha(1f);
      ((Behaviour) this._cursor).set_enabled(false);
      ColorBlock colors = ((Selectable) this._close).get_colors();
      ((ColorBlock) ref colors).set_highlightedColor(Define.Get(Colors.Green));
      ((Selectable) this._close).set_colors(colors);
      DisposableExtensions.AddTo<CompositeDisposable>((M0) ((IEnumerable<Selectable>) this._toggles).BindToEnter(true, this._cursor), (Component) this);
      DisposableExtensions.AddTo<IDisposable>((M0) ((IEnumerable<Toggle>) this._toggles).BindToGroup((Action<int>) (type => ((ReactiveProperty<int>) this._sortType).set_Value(type))), (Component) this);
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._sortType, (Action<M0>) (type =>
      {
        if (this.TypeChanged != null)
          this.TypeChanged(type);
        this.playSE.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this._close), (Action<M0>) (_ => this.OnInputCancel()));
      Image component = (Image) ((Component) this).GetComponent<Image>();
      if (Object.op_Inequality((Object) component, (Object) null))
        ObservableExtensions.Subscribe<PointerEventData>(Observable.Where<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) component), (Func<M0, bool>) (_ => ((ReactiveProperty<bool>) this._isOpen).get_Value())), (Action<M0>) (_ =>
        {
          if (this.OnEntered == null)
            return;
          this.OnEntered();
        }));
      // ISSUE: object of a compiler-generated type is created
      using (IEnumerator<\u003C\u003E__AnonType23<Selectable, int>> enumerator = ((IEnumerable<Selectable>) new Selectable[1]
      {
        (Selectable) this._close
      }).Concat<Selectable>((IEnumerable<Selectable>) this._toggles).Select<Selectable, \u003C\u003E__AnonType23<Selectable, int>>((Func<Selectable, int, \u003C\u003E__AnonType23<Selectable, int>>) ((o, index) => new \u003C\u003E__AnonType23<Selectable, int>(o, index - 1))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          // ISSUE: variable of a compiler-generated type
          \u003C\u003E__AnonType23<Selectable, int> item = enumerator.Current;
          ItemSortUI itemSortUi = this;
          ObservableExtensions.Subscribe<int>((IObservable<M0>) Observable.Select<PointerEventData, int>((IObservable<M0>) ObservableTriggerExtensions.OnPointerEnterAsObservable((UIBehaviour) item.o), (Func<M0, M1>) (_ => item.index)), (Action<M0>) (index =>
          {
            ((ReactiveProperty<int>) itemSortUi._selectedID).set_Value(index);
            if (itemSortUi.OnEntered == null)
              return;
            itemSortUi.OnEntered();
          }));
        }
      }
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__B)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__C)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__D)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
      this.playSE.use = true;
    }

    private void OnInputSubmit()
    {
      if (!((ReactiveProperty<bool>) this._isOpen).get_Value())
        return;
      if (Object.op_Inequality((Object) this._selectedOptionInstance, (Object) null))
        this._selectedOptionInstance.OnSubmit((BaseEventData) null);
      else
        this._close.OnSubmit((BaseEventData) null);
      this.OnSubmit?.Invoke();
    }

    private void OnInputCancel()
    {
      if (!((ReactiveProperty<bool>) this._isOpen).get_Value())
        return;
      this.Close();
      this.OnCancel?.Invoke();
    }
  }
}
