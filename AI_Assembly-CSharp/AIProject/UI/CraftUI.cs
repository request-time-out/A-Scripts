// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CraftUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Player;
using AIProject.SaveData;
using AIProject.UI.Viewer;
using Illusion.Extensions;
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
  public class CraftUI : MenuUIBehaviour
  {
    [Header("CraftUI Setting")]
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Button _closeButton;
    [Header("Craft Viewer")]
    [SerializeField]
    private CraftTagSelectionUI _craftTagSelection;
    private CraftViewer[] _craftVieweres;
    [Header("Recipe Viewer")]
    [SerializeField]
    private RectTransform _recipeViewerLayout;
    [SerializeField]
    private RecipeViewer _recipeViewer;
    [Header("InfoUI")]
    [SerializeField]
    private RectTransform _itemInfoLayout;
    [SerializeField]
    private CraftItemInfoUI _itemInfoUI;
    private IDisposable _fadeDisposable;

    public PlaySE playSE { get; } = new PlaySE(false);

    public static bool CreateCooking(
      CraftUI.CreateItem createItem,
      List<StuffItem> itemList,
      float pheromone,
      bool chef)
    {
      if (createItem == null)
        return false;
      int count = 1;
      StuffItem stuffItem = new StuffItem(createItem.item);
      List<StuffItem> itemListInPantry = Singleton<Game>.Instance.Environment.ItemListInPantry;
      int pantryCapacity = Singleton<Resources>.Instance.DefinePack.ItemBoxCapacityDefines.PantryCapacity;
      if (!((IReadOnlyCollection<StuffItem>) itemListInPantry).CanAddItem(pantryCapacity, stuffItem, count))
        return false;
      RecipeDataInfo info = createItem.info;
      foreach (RecipeDataInfo.NeedData need in info.NeedList)
        StuffItem.RemoveStorages(new StuffItem(need.CategoryID, need.ID, need.Sum * count), (IReadOnlyCollection<List<StuffItem>>) new List<StuffItem>[2]
        {
          itemList,
          itemListInPantry
        });
      float num1 = 50f;
      StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
      float t = Mathf.InverseLerp(statusProfile.FlavorCookSuccessBoostMinMax.min, statusProfile.FlavorCookSuccessBoostMinMax.max, pheromone);
      float num2 = statusProfile.FlavorCookSuccessBoost.Lerp(t);
      float num3 = 0.0f;
      if (chef)
        num3 += (float) statusProfile.ChefCookSuccessBoost;
      if ((double) Random.Range(0.0f, 100f) <= (double) (num1 + num2 + num3))
      {
        itemListInPantry.AddItem(stuffItem, info.CreateSum * count);
      }
      else
      {
        stuffItem.CategoryID = 7;
        stuffItem.ID = 12;
        itemListInPantry.AddItem(stuffItem, info.CreateSum * count);
      }
      return true;
    }

    public static CraftUI.CreateItem CreateCheck(
      IReadOnlyDictionary<int, RecipeDataInfo[]> targetTable,
      IReadOnlyCollection<IReadOnlyCollection<StuffItem>> storages)
    {
      // ISSUE: object of a compiler-generated type is created
      Dictionary<int, RecipeDataInfo[]> dictionary = ((IEnumerable<KeyValuePair<int, RecipeDataInfo[]>>) targetTable).Select<KeyValuePair<int, RecipeDataInfo[]>, \u003C\u003E__AnonType11<int, RecipeDataInfo[]>>((Func<KeyValuePair<int, RecipeDataInfo[]>, \u003C\u003E__AnonType11<int, RecipeDataInfo[]>>) (v => new \u003C\u003E__AnonType11<int, RecipeDataInfo[]>(v.Key, CraftUI.Possible(storages, v.Value)))).Where<\u003C\u003E__AnonType11<int, RecipeDataInfo[]>>((Func<\u003C\u003E__AnonType11<int, RecipeDataInfo[]>, bool>) (v => ((IEnumerable<RecipeDataInfo>) v.Value).Any<RecipeDataInfo>())).ToDictionary<\u003C\u003E__AnonType11<int, RecipeDataInfo[]>, int, RecipeDataInfo[]>((Func<\u003C\u003E__AnonType11<int, RecipeDataInfo[]>, int>) (v => v.Key), (Func<\u003C\u003E__AnonType11<int, RecipeDataInfo[]>, RecipeDataInfo[]>) (v => v.Value));
      if (!dictionary.Any<KeyValuePair<int, RecipeDataInfo[]>>())
        return (CraftUI.CreateItem) null;
      KeyValuePair<int, RecipeDataInfo[]> keyValuePair = dictionary.Shuffle<KeyValuePair<int, RecipeDataInfo[]>>().First<KeyValuePair<int, RecipeDataInfo[]>>();
      StuffItemInfo itemInfo = Singleton<Resources>.Instance.GameInfo.FindItemInfo(keyValuePair.Key);
      return new CraftUI.CreateItem()
      {
        info = ((IEnumerable<RecipeDataInfo>) keyValuePair.Value).Shuffle<RecipeDataInfo>().First<RecipeDataInfo>(),
        item = new StuffItem(itemInfo.CategoryID, itemInfo.ID, 0)
      };
    }

    private static RecipeDataInfo[] Possible(
      IReadOnlyCollection<IReadOnlyCollection<StuffItem>> storages,
      RecipeDataInfo[] info)
    {
      return ((IEnumerable<RecipeDataInfo>) info).Where<RecipeDataInfo>((Func<RecipeDataInfo, bool>) (x => ((IEnumerable<RecipeDataInfo.NeedData>) x.NeedList).All<RecipeDataInfo.NeedData>((Func<RecipeDataInfo.NeedData, bool>) (need => ((IEnumerable<IReadOnlyCollection<StuffItem>>) storages).SelectMany<IReadOnlyCollection<StuffItem>, StuffItem>((Func<IReadOnlyCollection<StuffItem>, IEnumerable<StuffItem>>) (storage => (IEnumerable<StuffItem>) storage)).FindItems(new StuffItem(need.CategoryID, need.ID, 0)).Sum<StuffItem>((Func<StuffItem, int>) (p => p.Count)) / need.Sum > 0)))).ToArray<RecipeDataInfo>();
    }

    public SystemMenuUI Observer { get; set; }

    public Action OnClose { get; set; }

    public Action OnClosedEvent { get; set; }

    public IReadOnlyCollection<IReadOnlyCollection<StuffItem>> checkStorages
    {
      get
      {
        return (IReadOnlyCollection<IReadOnlyCollection<StuffItem>>) this._checkStorages;
      }
    }

    private IReadOnlyCollection<List<StuffItem>> _checkStorages { get; set; }

    public void SetID(int id)
    {
      this._id = id;
      List<List<StuffItem>> stuffItemListList = new List<List<StuffItem>>();
      stuffItemListList.Add(Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList);
      stuffItemListList.Add(Singleton<Game>.Instance.Environment.ItemListInStorage);
      if (this._id == 2)
        stuffItemListList.Add(Singleton<Game>.Instance.Environment.ItemListInPantry);
      this._checkStorages = (IReadOnlyCollection<List<StuffItem>>) stuffItemListList;
    }

    private int _id { get; set; }

    private int _selectedIndexOf { get; set; }

    private MenuUIBehaviour[] MenuUIList { get; set; }

    private bool initialized { get; set; }

    private CraftViewer currentViewer { get; set; }

    private CraftUI.CreateItem createItem { get; } = new CraftUI.CreateItem();

    protected override void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      ((MonoBehaviour) this).StartCoroutine(this.BindingUI());
      ActionIDDownCommand actionIdDownCommand1 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Submit
      };
      // ISSUE: method pointer
      actionIdDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      this._actionCommands.Add(actionIdDownCommand1);
      ActionIDDownCommand actionIdDownCommand2 = new ActionIDDownCommand()
      {
        ActionID = ActionID.Cancel
      };
      // ISSUE: method pointer
      actionIdDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      this._actionCommands.Add(actionIdDownCommand2);
      ActionIDDownCommand actionIdDownCommand3 = new ActionIDDownCommand()
      {
        ActionID = ActionID.MouseRight
      };
      // ISSUE: method pointer
      actionIdDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__6)));
      this._actionCommands.Add(actionIdDownCommand3);
      base.Start();
    }

    [DebuggerHidden]
    private IEnumerator BindingUI()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftUI.\u003CBindingUI\u003Ec__Iterator0()
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
      if (Singleton<Manager.Map>.Instance.Player.Controller.State is Craft)
      {
        Time.set_timeScale(1f);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      }
      this.IsActiveControl = false;
      if (Object.op_Inequality((Object) this.Observer, (Object) null))
        this.Observer.ChangeBackground(-1);
      Action onClose = this.OnClose;
      if (onClose != null)
        onClose();
      this.playSE.Play(SoundPack.SystemSE.Cancel);
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftUI.\u003CDoOpen\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CraftUI.\u003CDoClose\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    private new void SetFocusLevel(int level)
    {
      Singleton<Input>.Instance.FocusLevel = level;
    }

    private void OnInputSubmit()
    {
    }

    private void OnInputCancel()
    {
      this.Close();
    }

    public class CreateItem
    {
      public StuffItem item { get; set; }

      public RecipeDataInfo info { get; set; }
    }
  }
}
