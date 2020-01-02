// Decompiled with JetBrains decompiler
// Type: AIProject.UI.SystemMenuUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using AIProject.Scene;
using ConfigScene;
using Manager;
using ReMotion;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class SystemMenuUI : SerializedMonoBehaviour
  {
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private BoolReactiveProperty _isActive;
    private IConnectableObservable<bool> _activeChanged;
    private bool _visible;
    private IDisposable _visibleChangeDisposable;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private RectTransform _uiRoot;
    [SerializeField]
    private Button _closeButton;
    private bool _activeCloseButton;
    private CanvasGroup _closeButtonCanvasGroup;
    private IDisposable _activeCloseDisposable;
    [SerializeField]
    private NotifyingUI _notifyingUI;
    [SerializeField]
    private Dictionary<int, CanvasGroup> _backgrounds;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private HomeMenu _homeMenu;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private StatusUI _statusUI;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private InventoryUIController _inventoryUI;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private InventoryUIController _inventoryEnterUI;
    private SystemMenuUI.MenuMode? _reservedMode;
    private IConnectableObservable<Unit> _fadeStream;
    private IDisposable _fadeDisposable;
    private bool[] _charasEntry;
    private IDisposable[] _backgroundDisposables;

    public SystemMenuUI()
    {
      base.\u002Ector();
    }

    public bool IsActiveControl
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isActive).get_Value();
      }
      set
      {
        if (((ReactiveProperty<bool>) this._isActive).get_Value() && this._notifyingUI.IsActiveControl)
          return;
        this._visible = value;
        ((ReactiveProperty<bool>) this._isActive).set_Value(value);
      }
    }

    protected IObservable<bool> OnActiveChangedAsObservable()
    {
      if (this._activeChanged == null)
      {
        this._activeChanged = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isActive, ((Component) this).get_gameObject()));
        this._activeChanged.Connect();
      }
      return (IObservable<bool>) this._activeChanged;
    }

    public bool Visible
    {
      get
      {
        return this._visible;
      }
      set
      {
        if (this._visible == value)
          return;
        this._visible = value;
        if (this._visibleChangeDisposable != null)
          this._visibleChangeDisposable.Dispose();
        float startAlpha = this._canvasGroup.get_alpha();
        int destAlpha = !value ? 0 : 1;
        if (!value)
          this._canvasGroup.set_blocksRaycasts(false);
        this._visibleChangeDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>(Observable.DoOnCompleted<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.3f, true), true), (Action) (() =>
        {
          if (!value)
            return;
          this._canvasGroup.set_blocksRaycasts(true);
        })), (Action<M0>) (x => this._canvasGroup.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
      }
    }

    public bool ActiveCloseButton
    {
      get
      {
        return this._activeCloseButton;
      }
      set
      {
        if (this._activeCloseButton == value)
          return;
        this._activeCloseButton = value;
        if (this._activeCloseDisposable != null)
          this._activeCloseDisposable.Dispose();
        if (value)
        {
          this._activeCloseDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.1f, true), true), (Action<M0>) (x => this._closeButtonCanvasGroup.set_alpha(((TimeInterval<float>) ref x).get_Value())), (Action) (() => this._closeButtonCanvasGroup.set_blocksRaycasts(true)));
        }
        else
        {
          this._closeButtonCanvasGroup.set_blocksRaycasts(false);
          this._activeCloseDisposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.1f, true), true), (Action<M0>) (x => this._closeButtonCanvasGroup.set_alpha(1f - ((TimeInterval<float>) ref x).get_Value())));
        }
      }
    }

    public HomeMenu HomeMenu
    {
      get
      {
        return this._homeMenu;
      }
    }

    public StatusUI StatusUI
    {
      get
      {
        return this._statusUI;
      }
    }

    public InventoryUIController InventoryUI
    {
      get
      {
        return this._inventoryUI;
      }
    }

    public InventoryUIController InventoryEnterUI
    {
      get
      {
        return this._inventoryEnterUI;
      }
    }

    public Action OnClose { get; set; }

    public void ReserveMove(SystemMenuUI.MenuMode mode)
    {
      this._reservedMode = new SystemMenuUI.MenuMode?(mode);
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (x => this.SetActiveControl(x)));
      Debug.Log((object) "ーーーーーーーーーーホームメニューUI生成(Start)ーーーーーーーーーー");
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      string manifestName = "abdata";
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefab, "HomeMenu", false, manifestName);
      if (Object.op_Inequality((Object) gameObject1, (Object) null))
      {
        this._homeMenu = (HomeMenu) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject1, (Transform) this._uiRoot, false)).GetComponent<HomeMenu>();
        this._homeMenu.Observer = this;
        this._homeMenu.OnClose = (Action) (() =>
        {
          this._homeMenu.IsActiveControl = false;
          this.IsActiveControl = false;
        });
      }
      Debug.Log((object) "ーーーーーーーーーーホームメニューUI生成(End)ーーーーーーーーーー");
      Debug.Log((object) "ーーーーーーーーーーポーチUI生成(Start)ーーーーーーーーーー");
      GameObject gameObject2 = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefab, "InventoryUI", false, manifestName);
      if (Object.op_Inequality((Object) gameObject2, (Object) null))
      {
        this._inventoryUI = (InventoryUIController) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject2, (Transform) this._uiRoot, false)).GetComponent<InventoryUIController>();
        this._inventoryUI.capacity = (Func<int>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax);
        this._inventoryUI.itemList = (Func<List<StuffItem>>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList);
        this._inventoryUI.Observer = this;
      }
      Debug.Log((object) "ーーーーーーーーーーポーチUI生成(End)ーーーーーーーーーー");
      Debug.Log((object) "ーーーーーーーーーーポーチ(Enter)UI生成(Start)ーーーーーーーーーー");
      GameObject gameObject3 = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefab, "InventoryEnterUI", false, manifestName);
      if (Object.op_Inequality((Object) gameObject3, (Object) null))
      {
        this._inventoryEnterUI = (InventoryUIController) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject3, (Transform) this._uiRoot, false)).GetComponent<InventoryUIController>();
        this._inventoryEnterUI.capacity = (Func<int>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.InventorySlotMax);
        this._inventoryEnterUI.itemList = (Func<List<StuffItem>>) (() => Singleton<Manager.Map>.Instance.Player.PlayerData.ItemList);
        this._inventoryEnterUI.Observer = this;
      }
      Debug.Log((object) "ーーーーーーーーーーポーチ(Enter)UI生成(End)ーーーーーーーーーー");
      Debug.Log((object) "ーーーーーーーーーーステータスUI生成(Start)ーーーーーーーーーー");
      GameObject gameObject4 = CommonLib.LoadAsset<GameObject>(definePack.ABPaths.MapScenePrefabAdd05, "StatusUI", false, definePack.ABManifests.Add05);
      if (Object.op_Inequality((Object) gameObject4, (Object) null))
      {
        this._statusUI = (StatusUI) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject4, (Transform) this._uiRoot, false)).GetComponent<StatusUI>();
        this._statusUI.Observer = this;
        this._statusUI.OnClose = (Action) (() =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
          this._statusUI.OpenID = 0;
          this._statusUI.IsActiveControl = false;
          this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
        });
      }
      Debug.Log((object) "ーーーーーーーーーーステータスUI生成(End)ーーーーーーーーーー");
      // ISSUE: method pointer
      ((UnityEvent) this._notifyingUI.PouchOpen.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__A)));
      // ISSUE: method pointer
      ((UnityEvent) this._notifyingUI.NotGet.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__B)));
      if (!Object.op_Implicit((Object) this._closeButton))
        return;
      // ISSUE: method pointer
      ((UnityEvent) this._closeButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__C)));
      this._closeButtonCanvasGroup = (CanvasGroup) ((Component) this._closeButton).GetComponent<CanvasGroup>();
    }

    private void SetActiveControl(bool active)
    {
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        this.ChangeBackground(-1);
        Time.set_timeScale(0.0f);
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        instance.FocusLevel = 0;
        coroutine = this.DoOpen();
        if (this._reservedMode.HasValue)
        {
          this.OpenModeMenu(this._reservedMode.Value);
          this._reservedMode = new SystemMenuUI.MenuMode?();
        }
        else
          this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.DoClose();
      }
      this._fadeStream = (IConnectableObservable<Unit>) Observable.PublishLast<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false));
      this._fadeDisposable = this._fadeStream.Connect();
    }

    [DebuggerHidden]
    private IEnumerator DoOpen()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SystemMenuUI.\u003CDoOpen\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator DoClose()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SystemMenuUI.\u003CDoClose\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    public void SetNotify(string notifyText)
    {
      this._notifyingUI.Display(notifyText);
    }

    public void OpenModeMenu(SystemMenuUI.MenuMode mode)
    {
      switch (mode + 1)
      {
        case SystemMenuUI.MenuMode.Status:
          this._homeMenu.UsageRestriction();
          this._homeMenu.IsActiveControl = true;
          break;
        case SystemMenuUI.MenuMode.Inventory:
          this._statusUI.IsActiveControl = true;
          break;
        case SystemMenuUI.MenuMode.Map:
          this._inventoryUI.IsActiveControl = true;
          break;
        case SystemMenuUI.MenuMode.Craft:
          this.Visible = false;
          Singleton<MapUIContainer>.Instance.MinimapUI.OpenAllMap(-1);
          Singleton<MapUIContainer>.Instance.MinimapUI.FromHomeMenu = true;
          int prevMapVisibleMode = Singleton<MapUIContainer>.Instance.MinimapUI.VisibleMode;
          Singleton<MapUIContainer>.Instance.MinimapUI.VisibleMode = 1;
          Singleton<MapUIContainer>.Instance.MinimapUI.AllMapClosedAction = (Action) (() =>
          {
            this.Visible = true;
            this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
            Singleton<MapUIContainer>.Instance.MinimapUI.AllMapClosedAction = (Action) null;
            bool miniMap = Manager.Config.GameData.MiniMap;
            if (prevMapVisibleMode != 0 || !miniMap)
              return;
            Singleton<MapUIContainer>.Instance.MinimapUI.OpenMiniMap();
          });
          Singleton<MapUIContainer>.Instance.MinimapUI.WarpProc = (MiniMapControler.OnWarp) (x =>
          {
            string prevStateName = Singleton<Manager.Map>.Instance.Player.PlayerController.PrevStateName;
            Singleton<MapUIContainer>.Instance.MinimapUI.AllMapClosedAction = (Action) (() => {});
            Singleton<Manager.Map>.Instance.WarpToBasePoint(x, (Action) (() =>
            {
              this.IsActiveControl = false;
              if (prevStateName == "Onbu")
                Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Onbu");
              else
                Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Normal");
              Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Idle");
              GC.Collect();
              bool miniMap = Manager.Config.GameData.MiniMap;
              if (prevMapVisibleMode != 0 || !miniMap)
                return;
              Singleton<MapUIContainer>.Instance.MinimapUI.OpenMiniMap();
            }), (Action) (() =>
            {
              if (prevStateName == "Onbu")
                Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Onbu");
              else
                Singleton<Manager.Map>.Instance.Player.Controller.ChangeState("Normal");
              Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
              Singleton<Input>.Instance.SetupState();
              Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(true);
              Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
            }));
            Singleton<MapUIContainer>.Instance.MinimapUI.WarpProc = (MiniMapControler.OnWarp) null;
          });
          break;
        case SystemMenuUI.MenuMode.Camera:
          this.Visible = false;
          MapUIContainer.SetActiveCraftUI(true);
          MapUIContainer.CraftUI.Observer = this;
          MapUIContainer.CraftUI.OnClose = (Action) (() =>
          {
            this.Visible = true;
            this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
            MapUIContainer.CraftUI.OnClose = (Action) null;
          });
          break;
        case SystemMenuUI.MenuMode.Call:
          Singleton<Manager.Map>.Instance.Player.PlayerController.ChangeState("Photo");
          Action onClose1 = this.OnClose;
          if (onClose1 == null)
            break;
          onClose1();
          break;
        case SystemMenuUI.MenuMode.Help:
          Singleton<Manager.Map>.Instance.Player.CallProc();
          Action onClose2 = this.OnClose;
          if (onClose2 == null)
            break;
          onClose2();
          break;
        case SystemMenuUI.MenuMode.Log:
          this.Visible = false;
          MapUIContainer.TutorialUI.ClosedEvent = (Action) (() =>
          {
            this.Visible = true;
            MapUIContainer.SetVisibleHUDExceptStoryUI(true);
            this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
          });
          MapUIContainer.SetVisibleHUDExceptStoryUI(false);
          MapUIContainer.TutorialUI.SetCondition(-1, true);
          MapUIContainer.SetActiveTutorialUI(true);
          break;
        case SystemMenuUI.MenuMode.Save:
          this.Visible = false;
          MapUIContainer.GameLogUI.IsActiveControl = true;
          MapUIContainer.GameLogUI.OnClosed = (Action) (() =>
          {
            this.Visible = true;
            this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
            MapUIContainer.GameLogUI.OnClosed = (Action) null;
          });
          break;
        case SystemMenuUI.MenuMode.InventoryEnter:
          this.Visible = false;
          if (this._charasEntry == null || this._charasEntry.Length != Manager.Config.GraphicData.CharasEntry.Length)
            this._charasEntry = new bool[Manager.Config.GraphicData.CharasEntry.Length];
          for (int index = 0; index < this._charasEntry.Length; ++index)
            this._charasEntry[index] = Manager.Config.GraphicData.CharasEntry[index];
          ConfigWindow.UnLoadAction = (Action) (() =>
          {
            if (!MapScene.EqualsSequence(this._charasEntry, Manager.Config.GraphicData.CharasEntry))
            {
              bool interactable = this._canvasGroup.get_interactable();
              this._canvasGroup.set_interactable(true);
              Singleton<Manager.Map>.Instance.ApplyConfig((Action) null, (Action) (() =>
              {
                this._canvasGroup.set_interactable(interactable);
                this.Visible = true;
                this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
              }));
            }
            else
            {
              this.Visible = true;
              this.OpenModeMenu(SystemMenuUI.MenuMode.Home);
            }
          });
          ConfigWindow.TitleChangeAction = (Action) (() =>
          {
            ConfigWindow.UnLoadAction = (Action) null;
            Singleton<Game>.Instance.Config.timeScaleChange = 1f;
            Singleton<Game>.Instance.Dialog.TimeScale = 1f;
          });
          Singleton<Game>.Instance.LoadConfig();
          break;
        case SystemMenuUI.MenuMode.Craft | SystemMenuUI.MenuMode.Save:
          this._inventoryEnterUI.IsActiveControl = true;
          break;
      }
    }

    public void ChangeBackground(int id)
    {
      foreach (IDisposable disposable in this._backgroundDisposables ?? (this._backgroundDisposables = new IDisposable[this._backgrounds.Count]))
        disposable?.Dispose();
      IObservable<TimeInterval<float>> observable = (IObservable<TimeInterval<float>>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.3f, true), true);
      int num = 0;
      using (Dictionary<int, CanvasGroup>.Enumerator enumerator = this._backgrounds.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, CanvasGroup> kvp = enumerator.Current;
          float startAlpha = kvp.Value.get_alpha();
          int destAlpha = kvp.Key != id ? 0 : 1;
          this._backgroundDisposables[num++] = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) observable, (Action<M0>) (x => kvp.Value.set_alpha(Mathf.Lerp(startAlpha, (float) destAlpha, ((TimeInterval<float>) ref x).get_Value()))));
        }
      }
    }

    public enum MenuMode
    {
      Home = -1, // 0xFFFFFFFF
      Status = 0,
      Inventory = 1,
      Map = 2,
      Craft = 3,
      Camera = 4,
      Call = 5,
      Help = 6,
      Log = 7,
      Save = 8,
      Config = 9,
      InventoryEnter = 10, // 0x0000000A
    }
  }
}
