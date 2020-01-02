// Decompiled with JetBrains decompiler
// Type: AIProject.UI.ChickenCoopUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
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
using UnityEx;

namespace AIProject.UI
{
  public class ChickenCoopUI : MenuUIBehaviour
  {
    private int _prevFocusLevel = -1;
    [Header("ChickenCoopUI")]
    [SerializeField]
    private ChickenCoopUI.Mode _mode;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private InventoryFacadeViewer _inventoryUI;
    [SerializeField]
    private HarvestListViewer _harvestListViewer;
    [SerializeField]
    private ChickenCoopListUI _chickenCoopListUI;
    [SerializeField]
    private ChickenNameChangeUI _chickenNameChangeUI;
    [SerializeField]
    private Button _addButton;
    [Header("HarvestList")]
    [SerializeField]
    private CanvasGroup _harvestListCG;
    [SerializeField]
    private Button _harvestListCloseButton;
    private IDisposable _fadeDisposable;
    private MenuUIBehaviour[] _menuUIList;
    private FarmPoint _currentFarmPoint;

    public PlaySE playSE { get; } = new PlaySE(false);

    public List<AIProject.SaveData.Environment.ChickenInfo> currentChickens
    {
      get
      {
        return this._currentChickens;
      }
      set
      {
        this._currentChickens = value;
      }
    }

    private List<AIProject.SaveData.Environment.ChickenInfo> _currentChickens { get; set; }

    public void SetMode(ChickenCoopUI.Mode mode)
    {
      this._mode = mode;
    }

    private void HarvestListOpen()
    {
      this._harvestListCG.set_alpha(1f);
      this._harvestListCG.set_blocksRaycasts(true);
    }

    private void HarvestListClose()
    {
      this._harvestListCG.set_alpha(0.0f);
      this._harvestListCG.set_blocksRaycasts(false);
    }

    private MenuUIBehaviour[] MenuUIList
    {
      get
      {
        return ((object) this).GetCache<MenuUIBehaviour[]>(ref this._menuUIList, (Func<MenuUIBehaviour[]>) (() => ((IEnumerable<MenuUIBehaviour>) new MenuUIBehaviour[6]
        {
          (MenuUIBehaviour) this,
          (MenuUIBehaviour) this._chickenCoopListUI,
          (MenuUIBehaviour) this._chickenNameChangeUI,
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
      return (IEnumerator) new ChickenCoopUI.\u003CBindingUI\u003Ec__Iterator0()
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
        PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
        if (Object.op_Inequality((Object) playerActor, (Object) null))
          this._currentFarmPoint = playerActor.CurrentFarmPoint;
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        this._prevFocusLevel = instance.FocusLevel;
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.DoOpen();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = this._prevFocusLevel;
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
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenCoopUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ChickenCoopUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    public Action ClosedEvent { get; set; }

    private new void SetFocusLevel(int level)
    {
      this._inventoryUI.viewer.SetFocusLevel(level);
      this._harvestListViewer.itemListUI.EnabledInput = level == this._harvestListViewer.itemListUI.FocusLevel;
    }

    private void Send(int currentID, ItemNodeUI currentOption)
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
      int currentIndex = this._chickenCoopListUI.currentIndex;
      while (this.currentChickens.Count <= currentIndex)
        this.currentChickens.Add((AIProject.SaveData.Environment.ChickenInfo) null);
      AIProject.SaveData.Environment.ChickenInfo info = new AIProject.SaveData.Environment.ChickenInfo();
      info.name = "ニワトリ";
      ValueTuple<AIProject.SaveData.AnimalData, PetChicken> chicken = this.CreateChicken(info);
      info.AnimalData = (AIProject.SaveData.AnimalData) chicken.Item1;
      if (Object.op_Inequality((Object) this._currentFarmPoint, (Object) null))
        this._currentFarmPoint.AddChicken(currentIndex, (PetChicken) chicken.Item2);
      this.currentChickens[currentIndex] = info;
      this._chickenCoopListUI.Refresh(currentIndex);
      this._inventoryUI.Visible = false;
    }

    private ValueTuple<AIProject.SaveData.AnimalData, PetChicken> CreateChicken(
      AIProject.SaveData.Environment.ChickenInfo info)
    {
      ValueTuple<AIProject.SaveData.AnimalData, PetChicken> valueTuple;
      ((ValueTuple<AIProject.SaveData.AnimalData, PetChicken>) ref valueTuple).\u002Ector((AIProject.SaveData.AnimalData) null, (PetChicken) null);
      if (info == null || !Singleton<AnimalManager>.IsInstance() || Object.op_Equality((Object) this._currentFarmPoint, (Object) null))
        return valueTuple;
      int _animalTypeID = 1;
      AnimalBase animalBase = Singleton<AnimalManager>.Instance.CreateBase(_animalTypeID, 1);
      IPetAnimal petAnimal = animalBase as IPetAnimal;
      if (Object.op_Equality((Object) animalBase, (Object) null))
        return valueTuple;
      ((Component) animalBase).get_transform().SetParent(this._currentFarmPoint.AnimalRoot, true);
      AIProject.SaveData.AnimalData animalData = new AIProject.SaveData.AnimalData();
      animalData.AnimalID = animalBase.AnimalID;
      animalData.RegisterID = this._currentFarmPoint.RegisterID;
      animalData.AnimalType = AnimalTypes.Chicken;
      animalData.AnimalTypeID = _animalTypeID;
      animalData.InitAnimalTypeID = true;
      animalData.BreedingType = BreedingTypes.Pet;
      animalData.Nickname = info.name;
      ItemIDKeyPair itemId = animalBase.ItemID;
      animalData.ItemCategoryID = itemId.categoryID;
      animalData.ItemID = itemId.itemID;
      if (petAnimal != null)
      {
        petAnimal.AnimalData = animalData;
        if (animalBase is PetChicken)
          (animalBase as PetChicken).Initialize(this._currentFarmPoint);
      }
      valueTuple.Item1 = (__Null) animalData;
      valueTuple.Item2 = (__Null) (animalBase as PetChicken);
      return valueTuple;
    }

    private void RemoveChicken(int index, AIProject.SaveData.Environment.ChickenInfo info)
    {
      AnimalBase animalBase;
      if (info == null || info.AnimalData == null || (!Singleton<AnimalManager>.IsInstance() || !Singleton<AnimalManager>.Instance.AnimalTable.TryGetValue(info.AnimalData.AnimalID, ref animalBase)) || !Object.op_Inequality((Object) animalBase, (Object) null))
        return;
      if (Object.op_Inequality((Object) this._currentFarmPoint, (Object) null))
        this._currentFarmPoint.RemoveChicken(index, animalBase as PetChicken);
      if (!(animalBase is IPetAnimal))
        return;
      (animalBase as IPetAnimal).Release();
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }

    public enum Mode
    {
      EggBox,
      Coop,
    }
  }
}
