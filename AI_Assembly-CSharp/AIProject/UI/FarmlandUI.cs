// Decompiled with JetBrains decompiler
// Type: AIProject.UI.FarmlandUI
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
using UnityEngine.UI;

namespace AIProject.UI
{
  public class FarmlandUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private InventoryFacadeViewer _inventoryUI;
    [SerializeField]
    private HarvestListViewer _harvestListViewer;
    [SerializeField]
    private PlantUI _plantUI;
    [SerializeField]
    private PlantInfoUI _plantInfoUI;
    [SerializeField]
    private Button _plantButton;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Button _allPlantButton;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuUIList;

    public PlaySE playSE { get; } = new PlaySE(false);

    public List<AIProject.SaveData.Environment.PlantInfo> currentPlant
    {
      get
      {
        return this._currentPlant;
      }
      set
      {
        this._currentPlant = value;
      }
    }

    private List<AIProject.SaveData.Environment.PlantInfo> _currentPlant { get; set; }

    private MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[6]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._plantUI,
          (MenuUIBehaviour) this._plantInfoUI,
          (MenuUIBehaviour) this._inventoryUI.categoryUI,
          (MenuUIBehaviour) this._inventoryUI.itemListUI,
          (MenuUIBehaviour) this._harvestListViewer.itemListUI
        }).Where<MenuUIBehaviour>((Func<MenuUIBehaviour, bool>) (p => Object.op_Inequality((Object) p, (Object) null))).ToArray<MenuUIBehaviour>()));
      }
    }

    private void CursorOFF()
    {
      ((Behaviour) this._inventoryUI.cursor).set_enabled(false);
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FarmlandUI.\u003CBindingUI\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void SetActiveControl(bool isActive)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (isActive)
      {
        MapUIContainer.SetVisibleHUD(false);
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        MapUIContainer.SetVisibleHUD(true);
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
      Time.set_timeScale(1f);
      this.IsActiveControl = false;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FarmlandUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FarmlandUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private new void SetFocusLevel(int level)
    {
      this._inventoryUI.viewer.SetFocusLevel(level);
      this.EnabledInput = level == this.FocusLevel;
      this._plantUI.EnabledInput = level == this._plantUI.FocusLevel;
      this._plantInfoUI.EnabledInput = level == this._plantInfoUI.FocusLevel;
      this._harvestListViewer.itemListUI.EnabledInput = level == this._harvestListViewer.itemListUI.FocusLevel;
    }

    private void Planting(int currentID, ItemNodeUI currentOption)
    {
      StuffItem item = currentOption.Item;
      StuffItem stuffItem = this._inventoryUI.itemList.Find((Predicate<StuffItem>) (x => x == item));
      --stuffItem.Count;
      if (stuffItem.Count <= 0)
      {
        this._inventoryUI.itemList.Remove(stuffItem);
        this._inventoryUI.itemListUI.RemoveItemNode(currentID);
        this._inventoryUI.itemListUI.ForceSetNonSelect();
      }
      this._inventoryUI.itemListUI.Refresh();
      this._plantUI.SetPlantItem(item);
      this._plantUI.Refresh();
      this._plantInfoUI.ItemCancelInteractable(((IReadOnlyCollection<StuffItem>) this._inventoryUI.itemList).CanAddItem(this._inventoryUI.slotCounter.y, item));
    }

    private void PlantingForAll(int currentID, ItemNodeUI currentOption)
    {
      int emptySum = this._plantUI.GetEmptySum();
      if (emptySum == 0)
        return;
      StuffItem item = currentOption.Item;
      StuffItem stuffItem = this._inventoryUI.itemList.Find((Predicate<StuffItem>) (x => x == item));
      int count = 0;
      while (stuffItem.Count > 0)
      {
        --stuffItem.Count;
        if (++count >= emptySum)
          break;
      }
      if (stuffItem.Count <= 0)
      {
        this._inventoryUI.itemList.Remove(stuffItem);
        this._inventoryUI.itemListUI.RemoveItemNode(currentID);
        this._inventoryUI.itemListUI.ForceSetNonSelect();
      }
      this._inventoryUI.itemListUI.Refresh();
      this._plantUI.SetPlantItemForAll(item, count);
      this._plantUI.Refresh();
      this._plantInfoUI.ItemCancelInteractable(((IReadOnlyCollection<StuffItem>) this._inventoryUI.itemList).CanAddItem(this._inventoryUI.slotCounter.y, item));
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
