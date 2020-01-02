// Decompiled with JetBrains decompiler
// Type: AIProject.UI.RecipeItemListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Illusion.Extensions;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class RecipeItemListUI : MenuUIBehaviour
  {
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _selectedID = new IntReactiveProperty(-1);
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private IntReactiveProperty _currentID = new IntReactiveProperty(-1);
    private Vector3 _velocity = Vector3.get_zero();
    private Vector3 _selectedvelocity = Vector3.get_zero();
    [SerializeField]
    private VerticalLayoutGroup _layoutGroup;
    private Transform _itemParent;
    private CraftUI _craftUI;
    private CanvasGroup _cursorCanvasGroup;
    [SerializeField]
    private Image _cursorFrame;
    [SerializeField]
    private Image _selectedCursorFrame;
    private IDisposable disposable;
    private float _alphaVelocity;

    public PlaySE playSE { get; } = new PlaySE();

    private Transform itemParent
    {
      get
      {
        return ((object) this).GetCacheObject<Transform>(ref this._itemParent, (Func<Transform>) (() => ((Component) this._layoutGroup).get_transform()));
      }
    }

    public event Action<int, RecipeItemTitleNodeUI> SelectChanged;

    public event Action<int, RecipeItemTitleNodeUI> CurrentChanged;

    public void SetCraftUI(CraftUI craftUI)
    {
      this._craftUI = craftUI;
    }

    public CanvasGroup CursorCanvasGroup
    {
      get
      {
        return this._cursorCanvasGroup ?? (this._cursorCanvasGroup = (CanvasGroup) ((Component) this._cursorFrame).GetComponent<CanvasGroup>());
      }
    }

    public RecipeItemTitleNodeUI SelectedOption { get; private set; }

    public RecipeItemTitleNodeUI CurrentOption { get; private set; }

    public bool isOptionNode
    {
      get
      {
        return Object.op_Inequality((Object) this.OptionNode, (Object) null);
      }
    }

    public void SetOptionNode(GameObject node)
    {
      this.OptionNode = node;
    }

    private GameObject OptionNode { get; set; }

    private ReactiveDictionary<int, RecipeItemTitleNodeUI> _optionTable { get; } = new ReactiveDictionary<int, RecipeItemTitleNodeUI>();

    public Action OnEntered { get; set; }

    public UnityEvent OnSubmit { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public void ForceSetNonSelect()
    {
      int num = -1;
      this.playSE.use = false;
      ((ReactiveProperty<int>) this._selectedID).SetValueAndForceNotify(num);
      ((ReactiveProperty<int>) this._currentID).SetValueAndForceNotify(num);
      this.playSE.use = true;
    }

    public IReadOnlyList<RecipeItemTitleNodeUI> AddItemNode(
      int id,
      RecipeDataInfo[] data)
    {
      GameObject optionNode = this.OptionNode;
      List<RecipeItemTitleNodeUI> recipeItemTitleNodeUiList = new List<RecipeItemTitleNodeUI>();
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<RecipeDataInfo, int> anonType18 in ((IEnumerable<RecipeDataInfo>) data).Select<RecipeDataInfo, \u003C\u003E__AnonType18<RecipeDataInfo, int>>((Func<RecipeDataInfo, int, \u003C\u003E__AnonType18<RecipeDataInfo, int>>) ((p, i) => new \u003C\u003E__AnonType18<RecipeDataInfo, int>(p, i))))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        RecipeItemListUI.\u003CAddItemNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new RecipeItemListUI.\u003CAddItemNode\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey0.item = anonType18;
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey0.\u0024this = this;
        RecipeItemTitleNodeUI component = (RecipeItemTitleNodeUI) ((GameObject) Object.Instantiate<GameObject>((M0) optionNode, this.itemParent)).GetComponent<RecipeItemTitleNodeUI>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        component.Bind(this._craftUI, nodeCAnonStorey0.item.i, nodeCAnonStorey0.item.p);
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) component.onEnter).AddListener(new UnityAction<BaseEventData>((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0)));
        // ISSUE: method pointer
        ((UnityEvent) component.OnClick).AddListener(new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__1)));
        // ISSUE: reference to a compiler-generated field
        this._optionTable.Add(nodeCAnonStorey0.item.i, component);
        recipeItemTitleNodeUiList.Add(component);
      }
      return (IReadOnlyList<RecipeItemTitleNodeUI>) recipeItemTitleNodeUiList;
    }

    public int[] ItemVisiblesID
    {
      get
      {
        return this.ConvertID(this.ItemVisibles);
      }
    }

    public RecipeItemTitleNodeUI[] ItemVisibles
    {
      get
      {
        return ((IEnumerable<Transform>) this.itemParent.Children()).Select<Transform, RecipeItemTitleNodeUI>((Func<Transform, RecipeItemTitleNodeUI>) (t => (RecipeItemTitleNodeUI) ((Component) t).GetComponent<RecipeItemTitleNodeUI>())).Where<RecipeItemTitleNodeUI>((Func<RecipeItemTitleNodeUI, bool>) (p => p.Visible)).ToArray<RecipeItemTitleNodeUI>();
      }
    }

    public void Refresh()
    {
      foreach (RecipeItemTitleNodeUI recipeItemTitleNodeUi in this._optionTable.get_Values().Where<RecipeItemTitleNodeUI>((Func<RecipeItemTitleNodeUI, bool>) (item => Object.op_Inequality((Object) item, (Object) null))))
        recipeItemTitleNodeUi.Refresh();
    }

    public void ClearItems()
    {
      foreach (Component component in this._optionTable.get_Values().Where<RecipeItemTitleNodeUI>((Func<RecipeItemTitleNodeUI, bool>) (item => Object.op_Inequality((Object) item, (Object) null))))
        Object.Destroy((Object) component.get_gameObject());
      this._optionTable.Clear();
      this.ForceSetNonSelect();
    }

    private int[] ConvertID(RecipeItemTitleNodeUI[] nodeUIs)
    {
      return ((IEnumerable<RecipeItemTitleNodeUI>) nodeUIs).Select<RecipeItemTitleNodeUI, int>((Func<RecipeItemTitleNodeUI, int>) (p =>
      {
        using (Dictionary<int, RecipeItemTitleNodeUI>.Enumerator enumerator = this._optionTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, RecipeItemTitleNodeUI> current = enumerator.Current;
            if (Object.op_Equality((Object) p, (Object) current.Value))
              return current.Key;
          }
        }
        return -1;
      })).ToArray<int>();
    }

    protected override void Start()
    {
      if (!Application.get_isPlaying())
        return;
      ((Component) this.itemParent).get_gameObject().Children().ForEach((Action<GameObject>) (go => Object.Destroy((Object) go)));
      ObservableExtensions.Subscribe<int>(Observable.Where<int>((IObservable<M0>) this._selectedID, (Func<M0, bool>) (_ => Singleton<Resources>.IsInstance())), (Action<M0>) (index =>
      {
        Action onEntered = this.OnEntered;
        if (onEntered != null)
          onEntered();
        RecipeItemTitleNodeUI recipeItemTitleNodeUi;
        this._optionTable.TryGetValue(index, ref recipeItemTitleNodeUi);
        if (Object.op_Inequality((Object) recipeItemTitleNodeUi, (Object) null) && !recipeItemTitleNodeUi.isSuccess)
          recipeItemTitleNodeUi = (RecipeItemTitleNodeUI) null;
        this.SelectedOption = recipeItemTitleNodeUi;
        if (Object.op_Equality((Object) recipeItemTitleNodeUi, (Object) null) || this.SelectChanged == null)
          return;
        this.SelectChanged(index, recipeItemTitleNodeUi);
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._currentID, (Action<M0>) (index =>
      {
        RecipeItemTitleNodeUI recipeItemTitleNodeUi;
        this._optionTable.TryGetValue(index, ref recipeItemTitleNodeUi);
        this.CurrentOption = recipeItemTitleNodeUi;
        if (this.CurrentChanged != null)
          this.CurrentChanged(index, recipeItemTitleNodeUi);
        if (index < 0)
          return;
        this.playSE.Play(SoundPack.SystemSE.OK_S);
      }));
      this.disposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__D)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__E)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__F)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    private void OnDestroy()
    {
      if (this.disposable == null)
        return;
      this.disposable.Dispose();
    }

    private void OnUpdate()
    {
      this.CursorCanvasGroup.set_alpha(Smooth.Damp(this.CursorCanvasGroup.get_alpha(), !this.EnabledInput || this.FocusLevel != Singleton<Input>.Instance.FocusLevel || !Object.op_Inequality((Object) this.SelectedOption, (Object) null) ? 0.0f : 1f, ref this._alphaVelocity, this._alphaAccelerationTime));
      if (Object.op_Inequality((Object) this.SelectedOption, (Object) null))
        CursorFrame.Set(((Graphic) this._cursorFrame).get_rectTransform(), (RectTransform) ((Component) this.SelectedOption).GetComponent<RectTransform>(), ref this._velocity, new float?(), new float?(this._followAccelerationTime));
      if (Object.op_Equality((Object) this.CurrentOption, (Object) null))
      {
        ((Component) this._selectedCursorFrame).get_gameObject().SetActive(false);
      }
      else
      {
        ((Component) this._selectedCursorFrame).get_gameObject().SetActive(true);
        CursorFrame.Set(((Graphic) this._selectedCursorFrame).get_rectTransform(), (RectTransform) ((Component) this.CurrentOption).GetComponent<RectTransform>(), ref this._selectedvelocity, new float?(), new float?(this._followAccelerationTime));
      }
    }

    private void OnInputSubmit()
    {
      this.OnSubmit?.Invoke();
    }

    private void OnInputCancel()
    {
      this.OnCancel?.Invoke();
    }

    [Serializable]
    public class ValueChangeEvent : UnityEvent<int, StuffItem>
    {
      public ValueChangeEvent()
      {
        base.\u002Ector();
      }
    }
  }
}
