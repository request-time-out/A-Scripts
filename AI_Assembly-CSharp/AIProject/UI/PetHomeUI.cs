// Decompiled with JetBrains decompiler
// Type: AIProject.UI.PetHomeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal;
using AIProject.Animal.Resources;
using AIProject.MiniGames.Fishing;
using AIProject.SaveData;
using AIProject.Scene;
using AIProject.UI.Viewer;
using Manager;
using ReMotion;
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
  [RequireComponent(typeof (RectTransform))]
  [RequireComponent(typeof (CanvasGroup))]
  public class PetHomeUI : MenuUIBehaviour
  {
    [SerializeField]
    private string _noneNameStr = string.Empty;
    private int _prevFocusLevel = -1;
    private float _prevTimeScale = 1f;
    private InventoryFacadeViewer.ItemFilter[] _emptyFilter = new InventoryFacadeViewer.ItemFilter[0];
    private Dictionary<int, InventoryFacadeViewer.ItemFilter[]> _itemFilterTable = new Dictionary<int, InventoryFacadeViewer.ItemFilter[]>();
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _rectTransform;
    [SerializeField]
    private PetNameChangeUI _nameChangeUI;
    [SerializeField]
    private InventoryFacadeViewer _inventoryUI;
    [SerializeField]
    private Image _mainIconImage;
    [SerializeField]
    private Text _mainTitleText;
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Toggle _elementToggle;
    [SerializeField]
    private Text _elementText;
    [SerializeField]
    private Button _nameChangeButton;
    [SerializeField]
    private Toggle _chaseToggle;
    [SerializeField]
    private Button _escapeButton;
    [SerializeField]
    private Button _addButton;
    [SerializeField]
    private Image _selectImage;
    private MenuUIBehaviour[] _menuUIBehaviorList;
    private IObservable<TimeInterval<float>> _lerpStream;
    private PetHomePoint _currentPetHomePoint;
    private IDisposable _fadeDisposable;

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this._rectTransform;
      }
    }

    public MenuUIBehaviour[] MenuUIBehaviorList
    {
      get
      {
        if (this._menuUIBehaviorList == null)
          this._menuUIBehaviorList = new MenuUIBehaviour[4]
          {
            (MenuUIBehaviour) this,
            (MenuUIBehaviour) this._nameChangeUI,
            (MenuUIBehaviour) this._inventoryUI.categoryUI,
            (MenuUIBehaviour) this._inventoryUI.itemListUI
          };
        return this._menuUIBehaviorList;
      }
    }

    private float CanvasAlpha
    {
      get
      {
        return Object.op_Inequality((Object) this._canvasGroup, (Object) null) ? this._canvasGroup.get_alpha() : 0.0f;
      }
      set
      {
        if (!Object.op_Inequality((Object) this._canvasGroup, (Object) null))
          return;
        this._canvasGroup.set_alpha(value);
      }
    }

    private void CursorOFF()
    {
      ((Behaviour) this._inventoryUI.cursor).set_enabled(false);
    }

    protected override void Awake()
    {
      base.Awake();
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null))
        this._canvasGroup = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
      if (!Object.op_Equality((Object) this._rectTransform, (Object) null))
        return;
      this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
    }

    protected override void OnBeforeStart()
    {
      base.OnBeforeStart();
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      this._lerpStream = (IObservable<TimeInterval<float>>) Observable.TakeUntilDestroy<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(this._alphaAccelerationTime, true), true), (Component) this);
      if (this._closeButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      }
      ActionIDDownCommand actionIdDownCommand = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__2)));
      this._actionCommands.Add(actionIdDownCommand);
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__3)));
      this._keyCommands.Add(keyCodeDownCommand);
      if (this._escapeButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this._escapeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__4)));
      }
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this._chaseToggle), (Action<M0>) (x => this.ChangeChaseActor(x)));
      if (this._nameChangeButton != null)
      {
        // ISSUE: method pointer
        ((UnityEvent) this._nameChangeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__6)));
      }
      this._nameChangeUI.SubmitAction += (Action<string>) (str => this.NameChanged(str));
      ((MonoBehaviour) this).StartCoroutine(this.UISettingCoroutine());
    }

    [DebuggerHidden]
    private IEnumerator UISettingCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PetHomeUI.\u003CUISettingCoroutine\u003Ec__Iterator0()
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
      this.IsActiveControl = false;
    }

    private void ForcedClose()
    {
      this.CanvasAlpha = 0.0f;
      this._nameChangeUI.QuickClose();
      this._inventoryUI.Visible = false;
      this.SetAllFocusLevel(0);
      this.SetAllEnableInput(false);
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PetHomeUI.\u003COpenCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PetHomeUI.\u003CCloseCoroutine\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private void SettingInventoryFilter()
    {
      Manager.Resources resources = !Singleton<Manager.Resources>.IsInstance() ? (Manager.Resources) null : Singleton<Manager.Resources>.Instance;
      PlayerActor playerActor = !Singleton<Manager.Map>.IsInstance() ? (PlayerActor) null : Singleton<Manager.Map>.Instance.Player;
      if (Object.op_Equality((Object) resources, (Object) null) || Object.op_Equality((Object) playerActor, (Object) null))
        return;
      PetHomePoint.HomeKind kind = this._currentPetHomePoint.Kind;
      Dictionary<int, List<ValueTuple<ItemIDKeyPair, int>>> petItemInfoTable = resources.AnimalTable.PetItemInfoTable;
      int key = (int) kind;
      InventoryFacadeViewer.ItemFilter[] itemFilter1 = (InventoryFacadeViewer.ItemFilter[]) null;
      if (!this._itemFilterTable.TryGetValue(key, out itemFilter1))
      {
        List<ValueTuple<ItemIDKeyPair, int>> valueTupleList;
        petItemInfoTable.TryGetValue(key, out valueTupleList);
        if (!((IReadOnlyList<ValueTuple<ItemIDKeyPair, int>>) valueTupleList).IsNullOrEmpty<ValueTuple<ItemIDKeyPair, int>>())
        {
          Dictionary<int, List<int>> toRelease = DictionaryPool<int, List<int>>.Get();
          using (List<ValueTuple<ItemIDKeyPair, int>>.Enumerator enumerator = valueTupleList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ItemIDKeyPair itemIdKeyPair = (ItemIDKeyPair) enumerator.Current.Item1;
              List<int> intList1;
              if (!toRelease.TryGetValue(itemIdKeyPair.categoryID, out intList1) || intList1 == null)
              {
                List<int> intList2 = ListPool<int>.Get();
                toRelease[itemIdKeyPair.categoryID] = intList2;
                intList1 = intList2;
              }
              intList1.Add(itemIdKeyPair.itemID);
            }
          }
          InventoryFacadeViewer.ItemFilter[] itemFilter2 = new InventoryFacadeViewer.ItemFilter[toRelease.Count];
          int index1 = 0;
          foreach (KeyValuePair<int, List<int>> keyValuePair in toRelease)
          {
            int[] IDs = new int[keyValuePair.Value.Count];
            for (int index2 = 0; index2 < IDs.Length; ++index2)
              IDs[index2] = keyValuePair.Value[index2];
            itemFilter2[index1] = new InventoryFacadeViewer.ItemFilter(keyValuePair.Key, IDs);
            ++index1;
          }
          List<int> list = toRelease.Keys.ToList<int>();
          for (int index2 = 0; index2 < list.Count; ++index2)
            ListPool<int>.Release(toRelease[list[index2]]);
          DictionaryPool<int, List<int>>.Release(toRelease);
          this._itemFilterTable[key] = itemFilter2;
          this._inventoryUI.SetItemFilter(itemFilter2);
        }
        else
        {
          this._itemFilterTable[key] = this._emptyFilter;
          this._inventoryUI.SetItemFilter(this._emptyFilter);
        }
      }
      else
        this._inventoryUI.SetItemFilter(itemFilter1);
    }

    private void Send(int currentID, ItemNodeUI currentOption)
    {
      if (Object.op_Equality((Object) this._currentPetHomePoint, (Object) null))
        return;
      Dictionary<int, List<ValueTuple<ItemIDKeyPair, int>>> dictionary1 = !Singleton<Manager.Resources>.IsInstance() ? (Dictionary<int, List<ValueTuple<ItemIDKeyPair, int>>>) null : Singleton<Manager.Resources>.Instance.AnimalTable.PetItemInfoTable;
      if (((IReadOnlyDictionary<int, List<ValueTuple<ItemIDKeyPair, int>>>) dictionary1).IsNullOrEmpty<int, List<ValueTuple<ItemIDKeyPair, int>>>())
        return;
      int kind = (int) this._currentPetHomePoint.Kind;
      List<ValueTuple<ItemIDKeyPair, int>> valueTupleList;
      if (!dictionary1.TryGetValue(kind, out valueTupleList) || ((IReadOnlyList<ValueTuple<ItemIDKeyPair, int>>) valueTupleList).IsNullOrEmpty<ValueTuple<ItemIDKeyPair, int>>())
        return;
      StuffItem item = currentOption.Item;
      int index;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      if ((index = valueTupleList.FindIndex((Predicate<ValueTuple<ItemIDKeyPair, int>>) (x => (^(ItemIDKeyPair&) ref x.Item1).categoryID == item.CategoryID && (^(ItemIDKeyPair&) ref x.Item1).itemID == item.ID))) < 0)
        return;
      ValueTuple<ItemIDKeyPair, int> valueTuple = valueTupleList[index];
      StuffItem stuffItem = this._inventoryUI.itemList.Find((Predicate<StuffItem>) (x => x == item));
      --stuffItem.Count;
      if (stuffItem.Count <= 0)
      {
        this._inventoryUI.itemList.Remove(stuffItem);
        this._inventoryUI.itemListUI.RemoveItemNode(currentID);
        this._inventoryUI.itemListUI.ForceSetNonSelect();
      }
      this._inventoryUI.itemListUI.Refresh();
      string name = Singleton<Manager.Resources>.Instance.GameInfo.GetItem(item.CategoryID, item.ID).Name;
      IPetAnimal animal = (IPetAnimal) null;
      int pointID = 0;
      switch (this._currentPetHomePoint.Kind)
      {
        case PetHomePoint.HomeKind.PetMat:
          animal = Singleton<AnimalManager>.Instance.CreateBase((int) valueTuple.Item2, 1) as IPetAnimal;
          break;
        case PetHomePoint.HomeKind.FishTank:
          Dictionary<int, Dictionary<int, FishInfo>> dictionary2 = !Singleton<Manager.Resources>.IsInstance() ? (Dictionary<int, Dictionary<int, FishInfo>>) null : Singleton<Manager.Resources>.Instance.Fishing.FishInfoTable;
          Dictionary<int, FishInfo> dictionary3;
          FishInfo fishInfo;
          if (dictionary2 != null && dictionary2.TryGetValue(item.CategoryID, out dictionary3) && (!((IReadOnlyDictionary<int, FishInfo>) dictionary3).IsNullOrEmpty<int, FishInfo>() && dictionary3.TryGetValue(item.ID, out fishInfo)))
          {
            animal = Singleton<AnimalManager>.Instance.CreateBase((int) valueTuple.Item2, 1) as IPetAnimal;
            pointID = fishInfo.TankPointID;
            break;
          }
          break;
      }
      this._elementText.set_text(name ?? this._noneNameStr);
      this.SetActive((Component) this._nameChangeButton, animal != null);
      if (animal != null)
      {
        this._inventoryUI.Visible = false;
        this.SetInteractable((Selectable) this._escapeButton, true);
        animal.AnimalData = new AIProject.SaveData.AnimalData()
        {
          First = true,
          AnimalID = animal.AnimalID,
          RegisterID = this._currentPetHomePoint.RegisterID,
          AnimalType = (AnimalTypes) (1 << valueTuple.Item2),
          AnimalTypeID = (int) valueTuple.Item2,
          InitAnimalTypeID = true,
          BreedingType = BreedingTypes.Pet,
          ItemCategoryID = item.CategoryID,
          ItemID = item.ID,
          ModelID = this.GetPetAnimalModelID((int) valueTuple.Item2),
          Position = this._currentPetHomePoint.Position,
          Rotation = this._currentPetHomePoint.Rotation
        };
        animal.Nickname = name;
        if (animal is IGroundPet)
          (animal as IGroundPet).ChaseActor = this._currentPetHomePoint.SaveData.ChaseActor;
        this._currentPetHomePoint.SetUser(animal);
        this._currentPetHomePoint.SetRootPoint(pointID, animal);
      }
      this.PlaySystemSE(SoundPack.SystemSE.OK_L);
    }

    private int GetPetAnimalModelID(int animalTypeID)
    {
      Dictionary<int, Dictionary<int, AnimalModelInfo>> dictionary1 = !Singleton<Manager.Resources>.IsInstance() ? (Dictionary<int, Dictionary<int, AnimalModelInfo>>) null : Singleton<Manager.Resources>.Instance.AnimalTable?.ModelInfoTable;
      Dictionary<int, AnimalModelInfo> dictionary2;
      return ((IReadOnlyDictionary<int, Dictionary<int, AnimalModelInfo>>) dictionary1).IsNullOrEmpty<int, Dictionary<int, AnimalModelInfo>>() || !dictionary1.TryGetValue(animalTypeID, out dictionary2) || ((IReadOnlyDictionary<int, AnimalModelInfo>) dictionary2).IsNullOrEmpty<int, AnimalModelInfo>() ? -1 : dictionary2.Keys.ToList<int>().Rand<int>();
    }

    private void RemoveAnimal()
    {
      ConfirmScene.Sentence = string.Format("{0}を逃しますか？\n", (object) ((this._currentPetHomePoint != null ? this._currentPetHomePoint.User : (IPetAnimal) null)?.Nickname ?? "ペット")) + "逃がすとアイテムとして戻ってきません。".Coloring("#DE4529FF").Size(24);
      ConfirmScene.OnClickedYes = (Action) (() =>
      {
        this._inventoryUI.Visible = true;
        this.PlaySystemSE(SoundPack.SystemSE.OK_L);
        this._currentPetHomePoint.RemoveUser();
        this._elementText.set_text(this._noneNameStr);
        this.SetInteractable((Selectable) this._escapeButton, false);
        this.SetActive((Component) this._nameChangeButton, false);
        if (!this._nameChangeUI.IsActiveControl)
          return;
        this._nameChangeUI.IsActiveControl = false;
      });
      ConfirmScene.OnClickedNo = (Action) (() => this.PlaySystemSE(SoundPack.SystemSE.Cancel));
      Singleton<Game>.Instance.LoadDialog();
    }

    private void ChangeChaseActor(bool active)
    {
      AIProject.SaveData.Environment.PetHomeInfo petHomeInfo = this._currentPetHomePoint != null ? this._currentPetHomePoint.SaveData : (AIProject.SaveData.Environment.PetHomeInfo) null;
      if (petHomeInfo == null)
        return;
      petHomeInfo.ChaseActor = active;
    }

    private void DoNameChange()
    {
      if (this._nameChangeUI.IsActiveControl)
        return;
      this._nameChangeUI.NameInputField.set_text(this._elementText.get_text());
      this._nameChangeUI.IsActiveControl = true;
    }

    private void NameChanged(string petName)
    {
      this._elementText.set_text(petName);
      IPetAnimal petAnimal = this._currentPetHomePoint != null ? this._currentPetHomePoint.User : (IPetAnimal) null;
      if (petAnimal == null)
        return;
      petAnimal.Nickname = petName;
    }

    private void SetAllFocusLevel(int level)
    {
      foreach (MenuUIBehaviour menuUiBehavior in this.MenuUIBehaviorList)
        menuUiBehavior.SetFocusLevel(level);
    }

    private void SetAllEnableInput(bool active)
    {
      foreach (MenuUIBehaviour menuUiBehavior in this.MenuUIBehaviorList)
        menuUiBehavior.EnabledInput = active;
    }

    private void SetBlockRaycast(bool active)
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_blocksRaycasts() == active)
        return;
      this._canvasGroup.set_blocksRaycasts(active);
    }

    private void SetInteractable(bool active)
    {
      if (Object.op_Equality((Object) this._canvasGroup, (Object) null) || this._canvasGroup.get_interactable() == active)
        return;
      this._canvasGroup.set_interactable(active);
    }

    private void SetActive(GameObject obj, bool active)
    {
      if (Object.op_Equality((Object) obj, (Object) null) || obj.get_activeSelf() == active)
        return;
      obj.SetActive(active);
    }

    private void SetActive(Component com, bool active)
    {
      if (Object.op_Equality((Object) com, (Object) null))
        return;
      GameObject gameObject = com.get_gameObject();
      if (Object.op_Equality((Object) gameObject, (Object) null) || gameObject.get_activeSelf() == active)
        return;
      gameObject.SetActive(active);
    }

    private void SetInteractable(Selectable sel, bool active)
    {
      if (Object.op_Equality((Object) sel, (Object) null) || sel.get_interactable() == active)
        return;
      sel.set_interactable(active);
    }

    private void PlaySystemSE(SoundPack.SystemSE se)
    {
      (!Singleton<Manager.Resources>.IsInstance() ? (SoundPack) null : Singleton<Manager.Resources>.Instance.SoundPack)?.Play(se);
    }
  }
}
