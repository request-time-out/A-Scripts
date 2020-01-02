// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ScroungeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
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
using UnityEngine.UI;

namespace AIProject.UI
{
  public class ScroungeUI : MenuUIBehaviour
  {
    [Header("ScroungeUI Setting")]
    [SerializeField]
    private ShopUI.InventoryUI _inventoryUI;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private ShopInfoPanelUI _shopInfoPanelUI;
    [SerializeField]
    private ScroungeRequestViewer _scroungeRequestViewer;
    [SerializeField]
    private ShopSendViewer _shopSendViewer;
    [SerializeField]
    private Button _sendButton;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuUIList;
    private ShopViewer.ItemListController[] _controllers;

    public PlaySE playSE { get; } = new PlaySE();

    public AgentActor agent
    {
      get
      {
        return this._scroungeRequestViewer.agent;
      }
      set
      {
        this._scroungeRequestViewer.agent = value;
      }
    }

    public System.Action OnClose { get; set; }

    private MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[5]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._shopInfoPanelUI,
          (MenuUIBehaviour) this._inventoryUI.categoryUI,
          (MenuUIBehaviour) this._inventoryUI.itemListUI,
          (MenuUIBehaviour) this._inventoryUI.itemSortUI
        }).Concat<MenuUIBehaviour>((IEnumerable<MenuUIBehaviour>) ((IEnumerable<ShopViewer.ItemListController>) this.controllers).Select<ShopViewer.ItemListController, ItemListUI>((Func<ShopViewer.ItemListController, ItemListUI>) (p => p.itemListUI))).ToArray<MenuUIBehaviour>()));
      }
    }

    private ShopViewer.ItemListController[] controllers
    {
      get
      {
        return ((object) this).GetCache<ShopViewer.ItemListController[]>(ref this._controllers, (Func<ShopViewer.ItemListController[]>) (() => new ShopViewer.ItemListController[3]
        {
          this._inventoryUI.controller,
          this._scroungeRequestViewer.controller,
          this._shopSendViewer.controller
        }));
      }
    }

    private bool initialized { get; set; }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (System.Action<M0>) (x => this.SetActiveControl(x)));
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ScroungeUI.\u003CBindingUI\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void ItemDecideProc(
      int count,
      int sel,
      ShopViewer.ItemListController sender,
      ShopViewer.ItemListController receiver)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(sel);
      StuffItem stuffItem = new StuffItem(node.Item);
      stuffItem.Count = count;
      receiver.AddItem(stuffItem, new ShopViewer.ExtraPadding(node.Item, sender));
      if (ShopUI.RemoveItem(count, sel, stuffItem, sender, this._inventoryUI))
        this.SetFocusLevel(sender.itemListUI.FocusLevel);
      bool flag1 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) receiver.itemListUI);
      if (!flag1)
        receiver.itemListUI.Refresh();
      bool flag2 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) sender.itemListUI);
      if (!flag2)
        sender.itemListUI.Refresh();
      if (flag1 || flag2)
        this._inventoryUI.Refresh();
      this._shopInfoPanelUI.Refresh();
      this.SendCheck();
    }

    private void ItemReturnProc(
      int count,
      int sel,
      ShopViewer.ItemListController sender,
      ShopViewer.ItemListController receiver)
    {
      ItemNodeUI node = sender.itemListUI.GetNode(sel);
      StuffItem stuffItem = new StuffItem(node.Item);
      stuffItem.Count = count;
      sender.RemoveItem(sel, stuffItem);
      ShopViewer.ExtraPadding extraData = node.extraData as ShopViewer.ExtraPadding;
      receiver = extraData.source;
      if (receiver != this._inventoryUI.controller)
        receiver.AddItem(stuffItem, new ShopViewer.ExtraPadding(extraData.item, sender));
      else if (this._inventoryUI.itemList.AddItem(stuffItem))
      {
        this._inventoryUI.ItemListAddNode(this._inventoryUI.itemListUI.SearchNotUsedIndex, stuffItem);
        this._inventoryUI.ItemListNodeFilter(this._inventoryUI.categoryUI.CategoryID, true);
      }
      bool flag1 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) receiver.itemListUI);
      if (!flag1)
        receiver.itemListUI.Refresh();
      bool flag2 = Object.op_Equality((Object) this._inventoryUI.itemListUI, (Object) sender.itemListUI);
      if (!flag2)
        sender.itemListUI.Refresh();
      if (flag1 || flag2)
        this._inventoryUI.Refresh();
      this._shopInfoPanelUI.Refresh();
      this.SendCheck();
    }

    private void SendCheck()
    {
      ((Component) this._sendButton).get_gameObject().SetActive(this._scroungeRequestViewer.Check((IReadOnlyCollection<StuffItem>) ((IEnumerable<KeyValuePair<int, ItemNodeUI>>) this._shopSendViewer.itemListUI.optionTable).Select<KeyValuePair<int, ItemNodeUI>, StuffItem>((Func<KeyValuePair<int, ItemNodeUI>, StuffItem>) (x => x.Value.Item)).ToArray<StuffItem>()));
    }

    private void Send()
    {
      this.playSE.Play(SoundPack.SystemSE.OK_S);
      ILookup<int, \u003C\u003E__AnonType24<StuffItem, int, int>> lookup = this._inventoryUI.itemList.Select<StuffItem, \u003C\u003E__AnonType24<StuffItem, int, int>>((Func<StuffItem, \u003C\u003E__AnonType24<StuffItem, int, int>>) (item =>
      {
        int id = -1;
        foreach (KeyValuePair<int, ItemNodeUI> keyValuePair in (IEnumerable<KeyValuePair<int, ItemNodeUI>>) this._inventoryUI.itemListUI.optionTable)
        {
          if (keyValuePair.Value.Item == item)
          {
            id = keyValuePair.Key;
            break;
          }
        }
        // ISSUE: object of a compiler-generated type is created
        return new \u003C\u003E__AnonType24<StuffItem, int, int>(item, Singleton<Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID).nameHash, id);
      })).ToLookup<\u003C\u003E__AnonType24<StuffItem, int, int>, int>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, int>) (p => p.nameHash));
      int itemSlotMax = Singleton<Resources>.Instance.DefinePack.MapDefines.ItemSlotMax;
      List<StuffItem> stuffItemList = new List<StuffItem>();
      foreach (IGrouping<int, \u003C\u003E__AnonType24<StuffItem, int, int>> source in (IEnumerable<IGrouping<int, \u003C\u003E__AnonType24<StuffItem, int, int>>>) lookup)
      {
        int num = source.Sum<\u003C\u003E__AnonType24<StuffItem, int, int>>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, int>) (x => x.item.Count));
        foreach (\u003C\u003E__AnonType24<StuffItem, int, int> anonType24 in (IEnumerable<\u003C\u003E__AnonType24<StuffItem, int, int>>) source)
        {
          if (num > itemSlotMax)
          {
            anonType24.item.Count = itemSlotMax;
            num -= itemSlotMax;
          }
          else
          {
            anonType24.item.Count = num;
            num = 0;
          }
        }
        \u003C\u003E__AnonType24<StuffItem, int, int>[] array = source.Where<\u003C\u003E__AnonType24<StuffItem, int, int>>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, bool>) (x => x.item.Count <= 0)).ToArray<\u003C\u003E__AnonType24<StuffItem, int, int>>();
        foreach (\u003C\u003E__AnonType24<StuffItem, int, int> anonType24 in array)
          this._inventoryUI.itemListUI.RemoveItemNode(anonType24.id);
        stuffItemList.AddRange(((IEnumerable<\u003C\u003E__AnonType24<StuffItem, int, int>>) array).Select<\u003C\u003E__AnonType24<StuffItem, int, int>, StuffItem>((Func<\u003C\u003E__AnonType24<StuffItem, int, int>, StuffItem>) (x => x.item)));
      }
      foreach (StuffItem stuffItem in stuffItemList)
        this._inventoryUI.itemList.Remove(stuffItem);
      this._scroungeRequestViewer.controller.Clear();
      this._shopSendViewer.controller.Clear();
      this._inventoryUI.Refresh();
      this._scroungeRequestViewer.itemScrounge.Reset();
      AgentActor agent = this.agent;
      int id1 = (int) ((IEnumerable<FlavorSkill.Type>) new FlavorSkill.Type[3]
      {
        FlavorSkill.Type.Reliability,
        FlavorSkill.Type.Sociability,
        FlavorSkill.Type.Reason
      }.Shuffle<FlavorSkill.Type>()).First<FlavorSkill.Type>();
      agent.AgentData.SetFlavorSkill(id1, agent.ChaControl.fileGameInfo.flavorState[id1] + 20);
      int id2 = (int) ((IEnumerable<FlavorSkill.Type>) new FlavorSkill.Type[3]
      {
        FlavorSkill.Type.Darkness,
        FlavorSkill.Type.Wariness,
        FlavorSkill.Type.Instinct
      }.Shuffle<FlavorSkill.Type>()).First<FlavorSkill.Type>();
      agent.AgentData.SetFlavorSkill(id2, agent.ChaControl.fileGameInfo.flavorState[id2] - 20);
      int id3 = 1;
      agent.SetStatus(id3, agent.AgentData.StatsTable[id3] + 30f);
      int id4 = 7;
      agent.SetStatus(id4, (float) (agent.ChaControl.fileGameInfo.morality + 5));
      int id5 = 6;
      agent.SetStatus(id5, agent.AgentData.StatsTable[id5] + 30f);
      this.OnInputCancel();
    }

    private void Reverse()
    {
      this._scroungeRequestViewer.controller.Clear();
      if (this._scroungeRequestViewer.itemScrounge.isEvent)
      {
        foreach (ItemNodeUI itemNodeUi in this._shopSendViewer.itemListUI)
        {
          StuffItem stuffItem = itemNodeUi.Item;
          if (this._inventoryUI.itemList.AddItem(stuffItem))
          {
            this._inventoryUI.ItemListAddNode(this._inventoryUI.itemListUI.SearchNotUsedIndex, stuffItem);
            this._inventoryUI.ItemListNodeFilter(this._inventoryUI.categoryUI.CategoryID, true);
          }
        }
      }
      this._shopSendViewer.controller.Clear();
    }

    private void SetActiveControl(bool isActive)
    {
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      IEnumerator coroutine;
      if (isActive)
      {
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = 0;
        coroutine = this.DoClose();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (System.Action<M0>) (_ => {}), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    private void Close()
    {
      Time.set_timeScale(1f);
      this.IsActiveControl = false;
      System.Action onClose = this.OnClose;
      if (onClose != null)
        onClose();
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ScroungeUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ScroungeUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private new void SetFocusLevel(int level)
    {
      Singleton<Manager.Input>.Instance.FocusLevel = level;
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }
  }
}
