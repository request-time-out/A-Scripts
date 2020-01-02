// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ItemListUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.Scene;
using AIProject.UI.Viewer;
using Illusion.Extensions;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

namespace AIProject.UI
{
  public class ItemListUI : MenuUIBehaviour
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
    private CanvasGroup _cursorCanvasGroup;
    [SerializeField]
    private Image _cursorFrame;
    [SerializeField]
    private Image _selectedCursorFrame;
    private IDisposable disposable;
    private float _alphaVelocity;

    public PlaySE playSE { get; } = new PlaySE();

    public VerticalLayoutGroup LayoutGroup
    {
      get
      {
        return this._layoutGroup;
      }
    }

    private Transform itemParent
    {
      get
      {
        return ((object) this).GetCacheObject<Transform>(ref this._itemParent, (Func<Transform>) (() => ((Component) this._layoutGroup).get_transform()));
      }
    }

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

    public event Action<int, ItemNodeUI> SelectChanged;

    public event Action<int, ItemNodeUI> CurrentChanged;

    public int CurrentID
    {
      get
      {
        return ((ReactiveProperty<int>) this._currentID).get_Value();
      }
      set
      {
        ((ReactiveProperty<int>) this._currentID).set_Value(value);
      }
    }

    public CanvasGroup CursorCanvasGroup
    {
      get
      {
        return this._cursorCanvasGroup ?? (this._cursorCanvasGroup = (CanvasGroup) ((Component) this._cursorFrame).GetComponent<CanvasGroup>());
      }
    }

    public Image CursorFrame
    {
      get
      {
        return this._cursorFrame;
      }
    }

    public Image SelectedCursorFrame
    {
      get
      {
        return this._selectedCursorFrame;
      }
    }

    public ItemNodeUI SelectedOption { get; private set; }

    public ItemNodeUI CurrentOption { get; private set; }

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

    private static GameObject SystemNode
    {
      get
      {
        if (Object.op_Inequality((Object) ItemListUI._systemNode, (Object) null))
          return ItemListUI._systemNode;
        string bundle = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
        GameObject gameObject1 = CommonLib.LoadAsset<GameObject>(bundle, "ItemOption_system", false, string.Empty);
        if (Object.op_Equality((Object) gameObject1, (Object) null))
          return (GameObject) null;
        if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundle)))
          MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundle, string.Empty));
        GameObject gameObject2 = gameObject1;
        ItemListUI._systemNode = gameObject2;
        return gameObject2;
      }
    }

    private static GameObject _systemNode { get; set; }

    public ItemNodeUI GetNode(int index)
    {
      ItemNodeUI itemNodeUi;
      this._optionTable.TryGetValue(index, ref itemNodeUi);
      return itemNodeUi;
    }

    [DebuggerHidden]
    public IEnumerator<ItemNodeUI> GetEnumerator()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<ItemNodeUI>) new ItemListUI.\u003CGetEnumerator\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public IReadOnlyReactiveDictionary<int, ItemNodeUI> optionTable
    {
      get
      {
        return (IReadOnlyReactiveDictionary<int, ItemNodeUI>) this._optionTable;
      }
    }

    private ReactiveDictionary<int, ItemNodeUI> _optionTable { get; } = new ReactiveDictionary<int, ItemNodeUI>();

    public IObservable<int> OnEntered
    {
      get
      {
        return (IObservable<int>) Observable.Where<int>(Observable.AsObservable<int>((IObservable<M0>) this._selectedID), (Func<M0, bool>) (id => id != -1));
      }
    }

    public UnityEvent OnSubmit { get; private set; } = new UnityEvent();

    public UnityEvent OnCancel { get; private set; } = new UnityEvent();

    public void SetupDefault()
    {
      this.ForceSetSelectedID(0);
    }

    public void ForceSetSelectedID(int id)
    {
      ((ReactiveProperty<int>) this._selectedID).SetValueAndForceNotify(id);
    }

    public void ForceSetNonSelect()
    {
      int num = -1;
      this.playSE.use = false;
      ((ReactiveProperty<int>) this._selectedID).SetValueAndForceNotify(num);
      ((ReactiveProperty<int>) this._currentID).SetValueAndForceNotify(num);
      this.playSE.use = true;
    }

    public ItemNodeUI AddItemNode(int id, StuffItem item)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ItemListUI.\u003CAddItemNode\u003Ec__AnonStorey2 nodeCAnonStorey2 = new ItemListUI.\u003CAddItemNode\u003Ec__AnonStorey2();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2.id = id;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2.\u0024this = this;
      StuffItemInfo itemInfo = ItemNodeUI.GetItemInfo(item);
      if (itemInfo == null)
        return (ItemNodeUI) null;
      GameObject gameObject = itemInfo.isNone ? ItemListUI.SystemNode : this.OptionNode;
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2.opt = (ItemNodeUI) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject, this.itemParent)).GetComponent<ItemNodeUI>();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey2.opt.Bind(item, itemInfo);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) nodeCAnonStorey2.opt.onEnter, (Action<M0>) new Action<PointerEventData>(nodeCAnonStorey2.\u003C\u003Em__0)), (Component) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: method pointer
      ((UnityEvent) nodeCAnonStorey2.opt.OnClick).AddListener(new UnityAction((object) nodeCAnonStorey2, __methodptr(\u003C\u003Em__1)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this._optionTable.Add(nodeCAnonStorey2.id, nodeCAnonStorey2.opt);
      // ISSUE: reference to a compiler-generated field
      return nodeCAnonStorey2.opt;
    }

    public bool RemoveItemNode(int index)
    {
      ItemNodeUI itemNodeUi;
      if (!this._optionTable.TryGetValue(index, ref itemNodeUi))
        return false;
      if (Object.op_Inequality((Object) itemNodeUi, (Object) null))
        Object.Destroy((Object) ((Component) itemNodeUi).get_gameObject());
      return this._optionTable.Remove(index);
    }

    public int SearchNotUsedIndex
    {
      get
      {
        return Enumerable.Range(0, this._optionTable.get_Count()).Where<int>((Func<int, bool>) (i => !this._optionTable.ContainsKey(i))).DefaultIfEmpty<int>(this._optionTable.get_Count()).First<int>();
      }
    }

    public int SearchIndex(StuffItem item)
    {
      if (item != null)
      {
        using (Dictionary<int, ItemNodeUI>.Enumerator enumerator = this._optionTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ItemNodeUI> current = enumerator.Current;
            if (current.Value.Item.CategoryID == item.CategoryID && current.Value.Item.ID == item.ID)
              return current.Key;
          }
        }
      }
      return -1;
    }

    public int SearchIndex(ItemNodeUI node)
    {
      if (Object.op_Inequality((Object) node, (Object) null))
      {
        using (Dictionary<int, ItemNodeUI>.Enumerator enumerator = this._optionTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ItemNodeUI> current = enumerator.Current;
            if (Object.op_Equality((Object) current.Value, (Object) node))
              return current.Key;
          }
        }
      }
      return -1;
    }

    public int[] ItemVisiblesID
    {
      get
      {
        return this.ConvertID(this.ItemVisibles);
      }
    }

    public ItemNodeUI[] ItemVisibles
    {
      get
      {
        return ((IEnumerable<Transform>) this.itemParent.Children()).Select<Transform, ItemNodeUI>((Func<Transform, ItemNodeUI>) (t => (ItemNodeUI) ((Component) t).GetComponent<ItemNodeUI>())).Where<ItemNodeUI>((Func<ItemNodeUI, bool>) (p => p.Visible)).ToArray<ItemNodeUI>();
      }
    }

    public void Filter(int category)
    {
      foreach (ItemNodeUI itemNodeUi in ((IEnumerable<KeyValuePair<int, ItemNodeUI>>) this._optionTable).Select<KeyValuePair<int, ItemNodeUI>, ItemNodeUI>((Func<KeyValuePair<int, ItemNodeUI>, ItemNodeUI>) (p => p.Value)))
        itemNodeUi.Visible = category == 0 || itemNodeUi.CategoryID == 0 || itemNodeUi.CategoryID == category || itemNodeUi.isNone && Mathf.Abs(itemNodeUi.CategoryID) == category;
    }

    public void Refresh()
    {
      foreach (ItemNodeUI itemNodeUi in this._optionTable.get_Values().Where<ItemNodeUI>((Func<ItemNodeUI, bool>) (item => Object.op_Inequality((Object) item, (Object) null))))
        itemNodeUi.Refresh();
    }

    public void ClearItems()
    {
      foreach (Component component in this._optionTable.get_Values().Where<ItemNodeUI>((Func<ItemNodeUI, bool>) (item => Object.op_Inequality((Object) item, (Object) null))))
        Object.Destroy((Object) component.get_gameObject());
      this._optionTable.Clear();
      this.ForceSetNonSelect();
    }

    public void SortItems(int type, bool ascending)
    {
      ItemNodeUI.Sort(type, ascending, (IDictionary<int, ItemNodeUI>) this._optionTable);
    }

    private int[] ConvertID(ItemNodeUI[] nodeUIs)
    {
      return ((IEnumerable<ItemNodeUI>) nodeUIs).Select<ItemNodeUI, int>((Func<ItemNodeUI, int>) (p =>
      {
        using (Dictionary<int, ItemNodeUI>.Enumerator enumerator = this._optionTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ItemNodeUI> current = enumerator.Current;
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
        ItemNodeUI itemNodeUi;
        this._optionTable.TryGetValue(index, ref itemNodeUi);
        this.SelectedOption = itemNodeUi;
        if (Object.op_Equality((Object) itemNodeUi, (Object) null) || this.SelectChanged == null)
          return;
        this.SelectChanged(index, itemNodeUi);
      }));
      ObservableExtensions.Subscribe<int>((IObservable<M0>) this._currentID, (Action<M0>) (index =>
      {
        ItemNodeUI itemNodeUi;
        this._optionTable.TryGetValue(index, ref itemNodeUi);
        this.CurrentOption = itemNodeUi;
        if (this.CurrentChanged != null)
          this.CurrentChanged(index, itemNodeUi);
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
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__F)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__10)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__11)));
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
        AIProject.UI.Viewer.CursorFrame.Set(((Graphic) this._cursorFrame).get_rectTransform(), (RectTransform) ((Component) this.SelectedOption).GetComponent<RectTransform>(), ref this._velocity, new float?(), new float?(this._followAccelerationTime));
      if (Object.op_Equality((Object) this.CurrentOption, (Object) null))
      {
        ((Component) this._selectedCursorFrame).get_gameObject().SetActive(false);
      }
      else
      {
        ((Component) this._selectedCursorFrame).get_gameObject().SetActive(true);
        AIProject.UI.Viewer.CursorFrame.Set(((Graphic) this._selectedCursorFrame).get_rectTransform(), (RectTransform) ((Component) this.CurrentOption).GetComponent<RectTransform>(), ref this._selectedvelocity, new float?(), new float?(this._followAccelerationTime));
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
