// Decompiled with JetBrains decompiler
// Type: AIProject.UI.InventoryFilterUIController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.UI.Viewer;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject.UI
{
  public abstract class InventoryFilterUIController : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    protected ItemInfoUI _itemInfoUI;
    [SerializeField]
    protected InventoryFilterUIController.InventoryUI _inventoryUI;
    private MenuUIBehaviour[] _menuUIList;
    private IDisposable _fadeDisposable;
    private IDisposable _emptyTextUpdateDisposable;

    public PlaySE playSE { get; } = new PlaySE(false);

    public virtual Func<int> capacity { get; set; }

    public void SetItemFilter(InventoryFacadeViewer.ItemFilter[] itemFilters)
    {
      this._inventoryUI.SetItemFilter(itemFilters);
    }

    public Func<List<StuffItem>> itemList { get; set; }

    public Func<List<StuffItem>> itemList_System { get; set; }

    public void CountViewerVisible(bool isVisible)
    {
      this._itemInfoUI.isCountViewerVisible = isVisible;
    }

    public void EmptyTextAutoVisible(bool isVisible)
    {
      this._inventoryUI.viewer.isAutoEmptyText = isVisible;
    }

    public void DoubleClickAction(
      Action<InventoryFacadeViewer.DoubleClickData> action)
    {
      this._inventoryUI.ItemNodeOnDoubleClick = (Action<InventoryFacadeViewer.DoubleClickData>) (x =>
      {
        Action<InventoryFacadeViewer.DoubleClickData> action1 = action;
        if (action1 != null)
          action1(x);
        this._itemInfoUI.OnSubmitForce();
      });
    }

    protected abstract void ItemInfoEvent();

    public SystemMenuUI Observer { get; set; }

    public Action OnClose { get; set; }

    protected ItemListUI itemListUI
    {
      get
      {
        return this._inventoryUI.viewer.itemListUI;
      }
    }

    protected MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[2]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._itemInfoUI
        }).Concat<MenuUIBehaviour>((IEnumerable<MenuUIBehaviour>) this._inventoryUI.viewer.MenuUIList).ToArray<MenuUIBehaviour>()));
      }
    }

    [DebuggerHidden]
    private IEnumerator LoadViewer()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryFilterUIController.\u003CLoadViewer\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void OnBeforeStart()
    {
      ((MonoBehaviour) this).StartCoroutine(this.LoadViewer());
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand2);
    }

    protected void SelectItem(StuffItem info)
    {
      this._itemInfoUI.Open(info);
    }

    protected new void SetFocusLevel(int level)
    {
      this._itemInfoUI.EnabledInput = level == this._itemInfoUI.FocusLevel;
    }

    private void SetActiveControl(bool isActive)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (isActive)
      {
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.DoClose();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    private void Close()
    {
      ((Behaviour) this._inventoryUI.viewer.cursor).set_enabled(false);
      this._inventoryUI.viewer.itemListUI.ClearItems();
      this._inventoryUI.viewer.sortUI.playSE.use = false;
      this._inventoryUI.viewer.sortUI.SetDefault();
      this._inventoryUI.viewer.sortUI.Close();
      this._inventoryUI.viewer.sortUI.playSE.use = true;
      this._itemInfoUI.Close();
      if (this._emptyTextUpdateDisposable != null)
        this._emptyTextUpdateDisposable.Dispose();
      this._emptyTextUpdateDisposable = (IDisposable) null;
      this._inventoryUI.viewer.isAutoEmptyText = false;
      ((Behaviour) this._inventoryUI.viewer.emptyText).set_enabled(false);
      this._inventoryUI.ItemNodeOnDoubleClick = (Action<InventoryFacadeViewer.DoubleClickData>) null;
      Action onClose = this.OnClose;
      if (onClose != null)
        onClose();
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryFilterUIController.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new InventoryFilterUIController.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }

    [Serializable]
    public class InventoryUI : InventoryFacadeViewer
    {
      public void SetOwner(InventoryFilterUIController owner)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        InventoryFilterUIController.InventoryUI.\u003CSetOwner\u003Ec__AnonStorey0 ownerCAnonStorey0 = new InventoryFilterUIController.InventoryUI.\u003CSetOwner\u003Ec__AnonStorey0();
        // ISSUE: reference to a compiler-generated field
        ownerCAnonStorey0.owner = owner;
        // ISSUE: reference to a compiler-generated field
        this.owner = ownerCAnonStorey0.owner;
        // ISSUE: reference to a compiler-generated field
        this.viewer.SetParentMenuUI((MenuUIBehaviour) ownerCAnonStorey0.owner);
        // ISSUE: reference to a compiler-generated method
        this.viewer.setFocusLevel = new Action<int>(ownerCAnonStorey0.\u003C\u003Em__0);
        // ISSUE: method pointer
        this.categoryUI.OnCancel.AddListener(new UnityAction((object) ownerCAnonStorey0, __methodptr(\u003C\u003Em__1)));
        // ISSUE: method pointer
        this.itemListUI.OnCancel.AddListener(new UnityAction((object) ownerCAnonStorey0, __methodptr(\u003C\u003Em__2)));
      }

      public InventoryFilterUIController owner { get; private set; }

      public override void ItemListNodeCreate()
      {
        this.SetItemList(this.owner.itemList());
        Func<List<StuffItem>> itemListSystem = this.owner.itemList_System;
        this.SetItemList_System((itemListSystem != null ? itemListSystem() : (List<StuffItem>) null) ?? new List<StuffItem>());
        base.ItemListNodeCreate();
        this.viewer.sortUI.SetDefault();
        this.viewer.sortUI.Close();
        this.viewer.sorter.set_isOn(true);
        this.Refresh();
      }

      public override void ItemListNodeFilter(int category, bool isSort)
      {
        base.ItemListNodeFilter(category, isSort);
        this.owner._itemInfoUI.Close();
      }
    }
  }
}
