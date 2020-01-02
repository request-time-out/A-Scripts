// Decompiled with JetBrains decompiler
// Type: AIProject.MapUIContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using AIProject.Animal;
using AIProject.ColorDefine;
using AIProject.DebugUtil;
using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.Scene;
using AIProject.UI;
using AIProject.UI.Recycling;
using Manager;
using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEx;

namespace AIProject
{
  public class MapUIContainer : Singleton<MapUIContainer>
  {
    private readonly ADVData _advData = new ADVData();
    private bool _activeCanvas = true;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private Canvas _hudCanvas;
    [SerializeField]
    private Canvas _commandCanvas;
    [Header("Progression")]
    [SerializeField]
    private FadeCanvas _fadeCanvas;
    [SerializeField]
    private NotifyMessageList _notifyPanel;
    [SerializeField]
    [Header("HUD")]
    private CanvasGroup _centerPointerCanvasGroup;
    [SerializeField]
    private MiniMapControler _minimapUI;
    [SerializeField]
    private CanvasGroup _minimapCanvasGroup;
    [SerializeField]
    private GameObject _MiniMapRenderTex;
    [SerializeField]
    private GameObject _MiniMapObject;
    [Header("Command")]
    [SerializeField]
    private SystemMenuUI _systemMenuUI;
    [SerializeField]
    private GameLog _gameLogUI;
    [SerializeField]
    private CommCommandList _commandList;
    [SerializeField]
    private CommCommandList _choiceUI;
    [SerializeField]
    private CommandLabel _commandLabel;
    private ClosetUI _dressRoomUI;
    private ClosetUI _closetUI;
    private CharaEntryUI _charaEntryUI;
    private CharaChangeUI _charaChangeUI;
    private PlayerChangeUI _playerChangeUI;
    private CharaLookEditUI _charaLookEditUI;
    private PlayerLookEditUI _playerLookEditUI;
    private CharaMigrateUI _charaMigrateUI;
    [SerializeField]
    private ItemBoxUI _itemBoxUI;
    [SerializeField]
    private FishingUI _fishingUI;
    private ResultMessageUI _resultMessageUI;
    private WarningMessageUI _warningMessageUI;
    private StorySupportUI _storySupportUI;
    private RequestUI _requestUI;
    private PhotoShotUI _photoShotUI;
    private TutorialUI _tutorialUI;
    private EventDialogUI _eventDialogUI;
    [SerializeField]
    private AllAreaMapUI _AllAreaMapUI;
    [SerializeField]
    private ShopUI _ShopUI;
    [SerializeField]
    private ScroungeUI _ScroungeUI;
    [SerializeField]
    private RefrigeratorUI _RefrigeratorUI;
    [SerializeField]
    private CraftUI _CraftUI;
    [SerializeField]
    private CraftUI _CookingUI;
    [SerializeField]
    private CraftUI _PetCraftUI;
    [SerializeField]
    private CraftUI _MedicineCraftUI;
    [SerializeField]
    private FarmlandUI _FarmlandUI;
    [SerializeField]
    private ChickenCoopUI _ChickenCoopUI;
    [SerializeField]
    private PetHomeUI _petHomeUI;
    [SerializeField]
    private JukeBoxUI _jukeBoxUI;
    [SerializeField]
    private SpendMoneyUI _spendMoneyUI;
    [SerializeField]
    private AnimalNicknameOutput _nicknameUI;
    [SerializeField]
    private RecyclingUI _recyclingUI;
    private IDisposable _hudActivateSubscriber;
    private IDisposable _storySupprtUIActivateSubscriber;
    [Header("Debug")]
    [SerializeField]
    private Canvas _debugUIRoot;
    [SerializeField]
    private Canvas _debugBackgroundUIRoot;
    private ADVScene _advScene;

    public CanvasGroup CanvasGroup
    {
      get
      {
        return this._canvasGroup;
      }
    }

    public static FadeCanvas FadeCanvas
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._fadeCanvas;
      }
    }

    public static NotifyMessageList NotifyPanel
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._notifyPanel;
      }
    }

    public MiniMapControler MinimapUI
    {
      get
      {
        return this._minimapUI;
      }
    }

    public static GameObject MiniMapRenderTex
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._MiniMapRenderTex;
      }
    }

    public static SystemMenuUI SystemMenuUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._systemMenuUI;
      }
    }

    public static GameLog GameLogUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._gameLogUI;
      }
    }

    public static CommCommandList CommandList
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._commandList;
      }
    }

    public static CommCommandList ChoiceUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._choiceUI;
      }
    }

    public static CommandLabel CommandLabel
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._commandLabel;
      }
    }

    public static ClosetUI DressRoomUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._dressRoomUI;
      }
    }

    public static ClosetUI ClosetUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._closetUI;
      }
    }

    public static CharaEntryUI CharaEntryUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._charaEntryUI;
      }
    }

    public static CharaChangeUI CharaChangeUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._charaChangeUI;
      }
    }

    public static PlayerChangeUI PlayerChangeUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._playerChangeUI;
      }
    }

    public static CharaLookEditUI CharaLookEditUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._charaLookEditUI;
      }
    }

    public static PlayerLookEditUI PlayerLookEditUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._playerLookEditUI;
      }
    }

    public static CharaMigrateUI CharaMigrateUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._charaMigrateUI;
      }
    }

    public static ItemBoxUI ItemBoxUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._itemBoxUI;
      }
    }

    public static FishingUI FishingUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._fishingUI;
      }
    }

    public static ResultMessageUI ResultMessageUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._resultMessageUI;
      }
    }

    public static WarningMessageUI WarningMessageUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._warningMessageUI;
      }
    }

    public static StorySupportUI StorySupportUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._storySupportUI;
      }
    }

    public static RequestUI RequestUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._requestUI;
      }
    }

    public static PhotoShotUI PhotoShotUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._photoShotUI;
      }
    }

    public static TutorialUI TutorialUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._tutorialUI;
      }
    }

    public static EventDialogUI EventDialogUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._eventDialogUI;
      }
    }

    public static AllAreaMapUI AllAreaMapUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._AllAreaMapUI;
      }
    }

    public static ShopUI ShopUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._ShopUI;
      }
    }

    public static ScroungeUI ScroungeUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._ScroungeUI;
      }
    }

    public static RefrigeratorUI RefrigeratorUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._RefrigeratorUI;
      }
    }

    public static CraftUI CraftUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._CraftUI;
      }
    }

    public static CraftUI CookingUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._CookingUI;
      }
    }

    public static CraftUI PetCraftUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._PetCraftUI;
      }
    }

    public static CraftUI MedicineCraftUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._MedicineCraftUI;
      }
    }

    public static FarmlandUI FarmlandUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._FarmlandUI;
      }
    }

    public static ChickenCoopUI ChickenCoopUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._ChickenCoopUI;
      }
    }

    public static PetHomeUI PetHomeUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._petHomeUI;
      }
    }

    public static JukeBoxUI JukeBoxUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._jukeBoxUI;
      }
    }

    public static SpendMoneyUI SpendMoneyUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._spendMoneyUI;
      }
    }

    public static AnimalNicknameOutput NicknameUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._nicknameUI;
      }
    }

    public static RecyclingUI RecyclingUI
    {
      get
      {
        return Singleton<MapUIContainer>.Instance._recyclingUI;
      }
    }

    public void OpenADV(OpenData openData, IPack pack)
    {
      this._advData.openData = openData;
      this._advData.pack = pack;
      ((Component) this._advScene).get_gameObject().SetActive(true);
    }

    public void CloseADV()
    {
      this._advScene.Scenario.captions.EndADV((System.Action) (() => ((Component) this._advScene).get_gameObject().SetActive(false)));
    }

    public ADVScene advScene
    {
      get
      {
        return this._advScene;
      }
    }

    public ADVData advData
    {
      get
      {
        return this._advData;
      }
    }

    private void Reset()
    {
      this._systemMenuUI = (SystemMenuUI) ((Component) this).GetComponentInChildren<SystemMenuUI>();
      this._commandLabel = (CommandLabel) ((Component) this).GetComponentInChildren<CommandLabel>();
      this._notifyPanel = (NotifyMessageList) ((Component) this).GetComponentInChildren<NotifyMessageList>();
      this._itemBoxUI = (ItemBoxUI) ((Component) this).GetComponentInChildren<ItemBoxUI>();
      this._AllAreaMapUI = (AllAreaMapUI) ((Component) this).GetComponentInChildren<AllAreaMapUI>();
      this._ShopUI = (ShopUI) ((Component) this).GetComponentInChildren<ShopUI>();
      this._ScroungeUI = (ScroungeUI) ((Component) this).GetComponentInChildren<ScroungeUI>();
      this._RefrigeratorUI = (RefrigeratorUI) ((Component) this).GetComponentInChildren<RefrigeratorUI>();
      foreach (CraftUI componentsInChild in (CraftUI[]) ((Component) this).GetComponentsInChildren<CraftUI>())
      {
        if (((Object) componentsInChild).get_name().StartsWith("CraftUI"))
          this._CraftUI = componentsInChild;
        else if (((Object) componentsInChild).get_name().StartsWith("CookingUI"))
          this._CookingUI = componentsInChild;
        else if (((Object) componentsInChild).get_name().StartsWith("PetCraftUI"))
          this._PetCraftUI = componentsInChild;
        else if (((Object) componentsInChild).get_name().StartsWith("MedicineCraftUI"))
          this._MedicineCraftUI = componentsInChild;
      }
      this._FarmlandUI = (FarmlandUI) ((Component) this).GetComponentInChildren<FarmlandUI>();
      this._ChickenCoopUI = (ChickenCoopUI) ((Component) this).GetComponentInChildren<ChickenCoopUI>();
      if (!Object.op_Equality((Object) this._MiniMapRenderTex, (Object) null))
        return;
      this._MiniMapRenderTex = ((Component) ((Component) this._minimapCanvasGroup).GetComponentInChildren<RawImage>(true)).get_gameObject();
    }

    public static void SwitchSystemMenu()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._systemMenuUI.IsActiveControl = !Singleton<MapUIContainer>.Instance._systemMenuUI.IsActiveControl;
    }

    public static void InitializeMinimap()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      if (Object.op_Equality((Object) Singleton<MapUIContainer>.Instance._MiniMapRenderTex, (Object) null))
        Singleton<MapUIContainer>.Instance._MiniMapRenderTex = ((Component) ((Component) Singleton<MapUIContainer>.Instance._minimapCanvasGroup).GetComponentInChildren<RawImage>(true)).get_gameObject();
      Singleton<MapUIContainer>.Instance.MinimapUI.Init(-1);
    }

    public static void SetVisibleHUD(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      float from = Singleton<MapUIContainer>.Instance._minimapCanvasGroup.get_alpha();
      float fromStoryUIAlpha = Singleton<MapUIContainer>.Instance._storySupportUI.CanvasAlpha;
      float fromPointerAlpha = Singleton<MapUIContainer>.Instance._centerPointerCanvasGroup.get_alpha();
      float duration = 0.3f;
      int to = !active ? 0 : 1;
      Singleton<MapUIContainer>.Instance._hudActivateSubscriber?.Dispose();
      Singleton<MapUIContainer>.Instance._hudActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(duration, true), true), (System.Action<M0>) (x =>
      {
        Singleton<MapUIContainer>.Instance._minimapCanvasGroup.set_alpha(Mathf.Lerp(from, (float) to, ((TimeInterval<float>) ref x).get_Value()));
        Singleton<MapUIContainer>.Instance._centerPointerCanvasGroup.set_alpha(Mathf.Lerp(fromPointerAlpha, (float) to, ((TimeInterval<float>) ref x).get_Value()));
      }), (System.Action<Exception>) (ex => Debug.LogException(ex)));
      Singleton<MapUIContainer>.Instance._storySupprtUIActivateSubscriber?.Dispose();
      Singleton<MapUIContainer>.Instance._storySupprtUIActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(duration, true), true), (System.Action<M0>) (x => Singleton<MapUIContainer>.Instance._storySupportUI.CanvasAlpha = Mathf.Lerp(fromStoryUIAlpha, (float) to, ((TimeInterval<float>) ref x).get_Value())), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public static void SetVisibleHUDExceptStoryUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      float fromMiniMapUIAlpha = Singleton<MapUIContainer>.Instance._minimapCanvasGroup.get_alpha();
      float fromPointerAlpha = Singleton<MapUIContainer>.Instance._centerPointerCanvasGroup.get_alpha();
      float to = !active ? 0.0f : 1f;
      Singleton<MapUIContainer>.Instance._hudActivateSubscriber?.Dispose();
      Singleton<MapUIContainer>.Instance._hudActivateSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(0.3f, true), true), (System.Action<M0>) (x =>
      {
        Singleton<MapUIContainer>.Instance._minimapCanvasGroup.set_alpha(Mathf.Lerp(fromMiniMapUIAlpha, to, ((TimeInterval<float>) ref x).get_Value()));
        Singleton<MapUIContainer>.Instance._centerPointerCanvasGroup.set_alpha(Mathf.Lerp(fromPointerAlpha, to, ((TimeInterval<float>) ref x).get_Value()));
      }), (System.Action<Exception>) (ex => Debug.LogException(ex)));
    }

    public static void ChangeActiveCanvas()
    {
      Singleton<MapUIContainer>.Instance._canvasGroup.set_alpha(!(Singleton<MapUIContainer>.Instance._activeCanvas = !Singleton<MapUIContainer>.Instance._activeCanvas) ? 0.0f : 1f);
    }

    public static void AddNotify(string message)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._notifyPanel.AddMessage(message);
    }

    public static string InfoName
    {
      get
      {
        return "system";
      }
    }

    public static string ItemGetText
    {
      get
      {
        return " を入手しました。";
      }
    }

    public static string ItemGetEmptyText
    {
      get
      {
        return "何も入手できませんでした。";
      }
    }

    public static string ToHtmlStringRGB(Color color)
    {
      return string.Format("#{0}", (object) ColorUtility.ToHtmlStringRGB(color));
    }

    public static string CharaNameColor(Actor actor)
    {
      if (Object.op_Equality((Object) actor, (Object) null) || actor.CharaName.IsNullOrEmpty())
        return string.Empty;
      Color? actorColor = MapUIContainer.GetActorColor(actor.ID);
      return !actorColor.HasValue ? actor.CharaName : actor.CharaName.Coloring(MapUIContainer.ToHtmlStringRGB(actorColor.Value));
    }

    public static Color? GetActorColor(int actorID)
    {
      Color? nullable = new Color?();
      switch (actorID)
      {
        case -99:
          nullable = new Color?(Define.Get(Colors.LightGreen));
          break;
        case -90:
          nullable = new Color?(Define.Get(Colors.Green));
          break;
        case 0:
          nullable = new Color?(Color32.op_Implicit(new Color32((byte) 233, (byte) 67, (byte) 33, byte.MaxValue)));
          break;
        case 1:
          nullable = new Color?(Define.Get(Colors.Blue));
          break;
        case 2:
          nullable = new Color?(Color32.op_Implicit(new Color32((byte) 250, (byte) 239, (byte) 0, byte.MaxValue)));
          break;
        case 3:
          nullable = new Color?(Color32.op_Implicit(new Color32(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue)));
          break;
        default:
          Debug.LogError((object) string.Format("{0} none:{1}", (object) "CharaNameColor", (object) actorID));
          break;
      }
      return nullable;
    }

    public static string GetItemColor(StuffItemInfo info)
    {
      if (info == null)
        return string.Empty;
      Color color;
      switch (info.Grade)
      {
        case Grade._1:
          color = Define.Get(Colors.White);
          break;
        case Grade._2:
          color = Define.Get(Colors.Blue);
          break;
        case Grade._3:
          color = Define.Get(Colors.Orange);
          break;
        default:
          color = Define.Get(Colors.White);
          Debug.LogError((object) string.Format("{0}:{1}", (object) nameof (GetItemColor), (object) info.Grade));
          break;
      }
      return info.Name.Coloring(MapUIContainer.ToHtmlStringRGB(color));
    }

    private static string GetItemMessage(StuffItemInfo info, int count)
    {
      return string.Format("{0} x {1}{2}", (object) MapUIContainer.GetItemColor(info), (object) count, (object) MapUIContainer.ItemGetText);
    }

    public static void AddSystemItemLog(StuffItemInfo info, int count, bool isNotify)
    {
      MapUIContainer.AddSystemLog(MapUIContainer.GetItemMessage(info, count), isNotify);
    }

    public static void AddSystemLog(string message, bool isNotify)
    {
      MapUIContainer.AddLog(MapUIContainer.InfoName.Coloring(MapUIContainer.ToHtmlStringRGB(Define.Get(Colors.Green))), message, (IReadOnlyCollection<TextScenario.IVoice[]>) null);
      if (!isNotify)
        return;
      MapUIContainer.AddNotify(message);
    }

    public static void AddItemLog(Actor actor, StuffItemInfo info, int count, bool isNotify)
    {
      string itemMessage = MapUIContainer.GetItemMessage(info, count);
      MapUIContainer.AddLog(actor, itemMessage, (IReadOnlyCollection<TextScenario.IVoice[]>) null);
      if (!isNotify)
        return;
      MapUIContainer.AddNotify(itemMessage);
    }

    public static void AddLog(
      Actor actor,
      string message,
      IReadOnlyCollection<TextScenario.IVoice[]> voices = null)
    {
      MapUIContainer.AddLog(MapUIContainer.CharaNameColor(actor), message, voices);
    }

    public static void AddLog(
      string name,
      string message,
      IReadOnlyCollection<TextScenario.IVoice[]> voices = null)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      if (!name.IsNullOrEmpty())
        name += " :";
      Singleton<MapUIContainer>.Instance._gameLogUI.AddLog(name, message, voices);
    }

    public static void PushResultMessage(string message)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._resultMessageUI.ShowMessage(message);
    }

    public static void PushWarningMessage(Popup.Warning.Type type)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._warningMessageUI.ShowMessage(type);
    }

    public static void PushMessageUI(
      Popup.Warning.Type type,
      int colorID,
      int poseID,
      System.Action onComplete = null)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._warningMessageUI.ShowMessage(type, colorID, poseID, onComplete);
    }

    public static void PushMessageUI(string mes, int colorID, int posID, System.Action onComplete = null)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._warningMessageUI.ShowMessage(mes, colorID, posID, onComplete);
    }

    public static void OpenRequestUI(Popup.Request.Type type)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._requestUI.Open(type);
    }

    public static void CloseRequestUI()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._requestUI.IsActiveControl = false;
    }

    public static void OpenStorySupportUI(Popup.StorySupport.Type type)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._storySupportUI.Open(type);
    }

    public static void OpenStorySupportUI(int id)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._storySupportUI.Open(id);
    }

    public static void CloseStorySupportUI()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._storySupportUI.Close();
    }

    public static void SetActiveSystemMenuUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._systemMenuUI.IsActiveControl = active;
    }

    public static void SetActiveCommandList(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._commandList.IsActiveControl = active;
    }

    public static void SetActiveCommandList(bool active, string title)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._commandList.Label.set_text(title);
      Singleton<MapUIContainer>.Instance._commandList.IsActiveControl = active;
    }

    public static void SetActiveChoiceUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._choiceUI.IsActiveControl = active;
    }

    public static void SetActiveChoiceUI(bool active, string title = "")
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._choiceUI.Label.set_text(title);
      Singleton<MapUIContainer>.Instance._choiceUI.IsActiveControl = active;
    }

    public static void SetActiveItemBoxUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._itemBoxUI.IsActiveControl = active;
    }

    public static void SetActiveDressRoomUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._dressRoomUI.IsActiveControl = active;
    }

    public static void SetActiveClosetUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._closetUI.IsActiveControl = active;
    }

    public static void SetActiveCharaEntryUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._charaEntryUI.IsActiveControl = active;
    }

    public static void SetActiveCharaChangeUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._charaChangeUI.IsActiveControl = active;
    }

    public static void SetActivePlayerChangeUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._playerChangeUI.IsActiveControl = active;
    }

    public static void SetActiveCharaLookEditUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._charaLookEditUI.IsActiveControl = active;
    }

    public static void SetActivePlayerLookEditUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._playerLookEditUI.IsActiveControl = active;
    }

    public static void SetActiveCharaMigrationUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._charaMigrateUI.IsActiveControl = active;
    }

    public static void SetActiveFishingUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._fishingUI.IsActiveControl = active;
    }

    public static void SetActiveShopUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._ShopUI.IsActiveControl = active;
    }

    public static void SetActiveScroungeUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._ScroungeUI.IsActiveControl = active;
    }

    public static void SetActiveRefrigeratorUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._RefrigeratorUI.IsActiveControl = active;
    }

    public static void SetActiveCraftUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._CraftUI.IsActiveControl = active;
    }

    public static void SetActiveCookingUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._CookingUI.IsActiveControl = active;
    }

    public static void SetActivePetCraftUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._PetCraftUI.IsActiveControl = active;
    }

    public static void SetActiveMedicineCraftUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._MedicineCraftUI.IsActiveControl = active;
    }

    public static void SetActiveFarmlandUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._FarmlandUI.IsActiveControl = active;
    }

    public static void SetActiveChickenCoopUI(bool active, ChickenCoopUI.Mode mode)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._ChickenCoopUI.SetMode(mode);
      Singleton<MapUIContainer>.Instance._ChickenCoopUI.IsActiveControl = active;
    }

    public static void SetActivePetHomeUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._petHomeUI.IsActiveControl = active;
    }

    public static void SetActiveJukeBoxUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._jukeBoxUI.IsActiveControl = active;
    }

    public static void SetVisibledSpendMoneyUI(bool visibled)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._spendMoneyUI.Visibled = visibled;
    }

    public static void SetActiveRecyclingUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._recyclingUI.IsActiveControl = active;
    }

    public static void OpenTutorialUI(int id, bool groupDisplay = false)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      if (Singleton<Game>.IsInstance())
      {
        WorldData worldData = Singleton<Game>.Instance.WorldData;
        Dictionary<int, bool> dictionary = worldData == null ? (Dictionary<int, bool>) null : worldData.TutorialOpenStateTable;
        if (dictionary != null)
          dictionary[id] = true;
      }
      TutorialUI tutorialUi = Singleton<MapUIContainer>.Instance._tutorialUI;
      tutorialUi.SetCondition(id, groupDisplay);
      tutorialUi.IsActiveControl = true;
    }

    public static bool OpenOnceTutorial(int id, bool groupDisplay = false)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return false;
      TutorialUI tutorialUi = Singleton<MapUIContainer>.Instance._tutorialUI;
      if (Object.op_Equality((Object) tutorialUi, (Object) null) || tutorialUi.IsActiveControl || !Singleton<Game>.IsInstance())
        return false;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      if (worldData == null)
        return false;
      Dictionary<int, bool> tutorialOpenStateTable = worldData.TutorialOpenStateTable;
      bool flag;
      if (tutorialOpenStateTable == null || tutorialOpenStateTable.TryGetValue(id, out flag) && flag)
        return false;
      tutorialOpenStateTable[id] = true;
      tutorialUi.SetCondition(id, groupDisplay);
      tutorialUi.IsActiveControl = true;
      return true;
    }

    public static bool GetTutorialOpenState(int id)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      if (worldData == null)
        return false;
      Dictionary<int, bool> tutorialOpenStateTable = worldData.TutorialOpenStateTable;
      if (tutorialOpenStateTable == null)
        return false;
      bool flag;
      if (!tutorialOpenStateTable.TryGetValue(id, out flag))
        tutorialOpenStateTable[id] = flag = false;
      return flag;
    }

    public static void SetActiveTutorialUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._tutorialUI.IsActiveControl = active;
    }

    [DebuggerHidden]
    public static IEnumerator DrawOnceTutorialUI(
      int tutorialID,
      ActorCameraControl camera = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapUIContainer.\u003CDrawOnceTutorialUI\u003Ec__Iterator0()
      {
        tutorialID = tutorialID,
        camera = camera
      };
    }

    public static void SetActiveEventDialogUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._eventDialogUI.IsActiveControl = active;
    }

    public static void SetActivePhotoShotUI(bool active)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._photoShotUI.IsActiveControl = active;
    }

    public static void SetSystemNotify(string message)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._systemMenuUI.SetNotify(message);
    }

    public static void SetInventoryStock(StuffItem presentingItem, System.Action<bool> onCompleted)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        ;
    }

    public static void RefreshCommands(int id, CommCommandList.CommandInfo[] options)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._commandList.ID = id;
      Singleton<MapUIContainer>.Instance._commandList.Options = options;
    }

    public static void RefreshChoices(int id, CommCommandList.CommandInfo[] options)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._choiceUI.ID = id;
      Singleton<MapUIContainer>.Instance._choiceUI.Options = options;
    }

    public static void SetCommandLabelAcception(CommandLabel.AcceptionState acception)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._commandLabel.Acception = acception;
    }

    public static void ReserveSystemMenuMode(SystemMenuUI.MenuMode mode)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<MapUIContainer>.Instance._systemMenuUI.ReserveMove(mode);
    }

    public static IObservable<Unit> StartFade(
      FadeCanvas.PanelType panelType,
      global::FadeType fadeType,
      float duration,
      bool ignoreTimeScale)
    {
      return Singleton<MapUIContainer>.IsInstance() ? Singleton<MapUIContainer>.Instance._fadeCanvas.StartFade(panelType, fadeType, duration, ignoreTimeScale) : (IObservable<Unit>) null;
    }

    public static void SetMarkerTargetCameraTransform(Transform cameraTransform)
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      ((MarkerOutput) ((Component) Singleton<MapUIContainer>.Instance).GetComponentInChildren<MarkerOutput>()).CameraTransform = cameraTransform;
    }

    private void InitiateDebugOutput()
    {
    }

    private T LoadAsset<T>(string assetBundlePath, string assetName, string manifest = "") where T : Object
    {
      T obj = CommonLib.LoadAsset<T>(assetBundlePath, assetName, false, manifest);
      if (Object.op_Equality((Object) (object) obj, (Object) null))
        return (T) null;
      if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == assetBundlePath && (string) x.Item2 == manifest)))
        MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(assetBundlePath, manifest));
      return obj;
    }

    public static bool AnyUIActive()
    {
      if (!Singleton<MapUIContainer>.IsInstance())
        return false;
      if (Singleton<MapUIContainer>.Instance._commandList.IsActiveControl || Singleton<MapUIContainer>.Instance._choiceUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._itemBoxUI.IsActiveControl || Singleton<MapUIContainer>.Instance._charaEntryUI.IsActiveControl) || (Singleton<MapUIContainer>.Instance._charaChangeUI.IsActiveControl || Singleton<MapUIContainer>.Instance._playerChangeUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._charaLookEditUI.IsActiveControl || Singleton<MapUIContainer>.Instance._playerLookEditUI.IsActiveControl)) || (Singleton<MapUIContainer>.Instance._charaMigrateUI.IsActiveControl || Singleton<MapUIContainer>.Instance._dressRoomUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._closetUI.IsActiveControl || Singleton<MapUIContainer>.Instance._RefrigeratorUI.IsActiveControl) || (Singleton<MapUIContainer>.Instance._CookingUI.IsActiveControl || Singleton<MapUIContainer>.Instance._MedicineCraftUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._PetCraftUI.IsActiveControl || Singleton<MapUIContainer>.Instance._FarmlandUI.IsActiveControl))) || (Singleton<MapUIContainer>.Instance._ChickenCoopUI.IsActiveControl || Singleton<MapUIContainer>.Instance._petHomeUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._jukeBoxUI.IsActiveControl || Singleton<MapUIContainer>.Instance._systemMenuUI.IsActiveControl) || (Singleton<MapUIContainer>.Instance._requestUI.IsActiveControl || Singleton<MapUIContainer>.Instance._tutorialUI.IsActiveControl || (Singleton<MapUIContainer>.Instance._eventDialogUI.IsActiveControl || Singleton<MapUIContainer>.Instance._recyclingUI.IsActiveControl)) || (Singleton<Manager.ADV>.Instance.Captions.Active || Singleton<MapUIContainer>.Instance._minimapUI.AllAreaMap.get_activeSelf() || Singleton<Resources>.Instance.HSceneTable.HSceneUISet.get_activeSelf() || (Singleton<Housing>.IsInstance() && Singleton<Housing>.Instance.IsCraft || Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null)))) || (Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.Config, (Object) null) || Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null)))
        return true;
      return Singleton<Game>.IsInstance() && Object.op_Inequality((Object) Singleton<Game>.Instance.ExitScene, (Object) null);
    }

    private void Start()
    {
      DefinePack.AssetBundlePathsDefine abPaths = Singleton<Resources>.Instance.DefinePack.ABPaths;
      DefinePack.AssetBundleManifestsDefine abManifests = Singleton<Resources>.Instance.DefinePack.ABManifests;
      Transform transform1 = ((Component) this._hudCanvas).get_transform();
      Debug.Log((object) "通知UI生成");
      this._notifyPanel = (NotifyMessageList) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "NotifyLog", abManifests.Default), transform1, false)).GetComponent<NotifyMessageList>();
      Debug.Log((object) "リザルトUI生成");
      this._resultMessageUI = (ResultMessageUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ResultMessageUI", abManifests.Default), transform1, false)).GetComponent<ResultMessageUI>();
      Debug.Log((object) "ストーリーサポートUI生成");
      this._storySupportUI = (StorySupportUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "StorySupportUI", abManifests.Default), transform1, false)).GetComponent<StorySupportUI>();
      Debug.Log((object) "ワーニングUI生成");
      this._warningMessageUI = (WarningMessageUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "WarningMessageUI", abManifests.Default), transform1, false)).GetComponent<WarningMessageUI>();
      Debug.Log((object) "点UI生成");
      this._centerPointerCanvasGroup = (CanvasGroup) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CenterPointer", abManifests.Default), transform1, false)).GetComponent<CanvasGroup>();
      Debug.Log((object) "ミニマップ生成");
      this._minimapUI = (MiniMapControler) ((GameObject) Object.Instantiate<GameObject>((M0) this._MiniMapObject)).GetComponentInChildren<MiniMapControler>();
      this._minimapCanvasGroup = (CanvasGroup) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "MiniMapUI", abManifests.Default), transform1, false)).GetComponent<CanvasGroup>();
      Transform transform2 = ((Component) this._commandCanvas).get_transform();
      this._nicknameUI = (AnimalNicknameOutput) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "NicknameUI", abManifests.Default), transform2, false)).GetComponent<AnimalNicknameOutput>();
      Debug.Log((object) "コマンドラベル生成");
      this._commandLabel = (CommandLabel) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CommandLabel", abManifests.Default), transform2, false)).GetComponent<CommandLabel>();
      Debug.Log((object) "ADVWindow生成");
      this._advScene = (ADVScene) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ADVWindow", abManifests.Default), transform2, false)).GetComponentInChildren<ADVScene>(true);
      ParameterList.Add((SceneParameter) new ADVParames((ADV.IData) this._advData));
      Debug.Log((object) "ADV選択肢UI生成");
      this._choiceUI = (CommCommandList) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ChoiceUI", abManifests.Default), transform2, false)).GetComponent<CommCommandList>();
      Debug.Log((object) "コマンドリスト生成");
      this._commandList = (CommCommandList) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd05, "CommandList", abManifests.Add05), transform2, false)).GetComponent<CommCommandList>();
      Debug.Log((object) "メニューUI生成");
      this._systemMenuUI = (SystemMenuUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "MenuUI", abManifests.Default), transform2, false)).GetComponent<SystemMenuUI>();
      Debug.Log((object) "ゲームログUI生成");
      this._gameLogUI = (GameLog) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "GameLog", abManifests.Default), transform2, false)).GetComponent<GameLog>();
      Debug.Log((object) "アイテムボックスUI生成");
      this._itemBoxUI = (ItemBoxUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ItemBoxUI", abManifests.Default), transform2, false)).GetComponent<ItemBoxUI>();
      Debug.Log((object) "脱衣所UIの作成");
      this._dressRoomUI = (ClosetUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "DressRoomUI", abManifests.Default), transform2, false)).GetComponent<ClosetUI>();
      Debug.Log((object) "衣装棚UIの作成");
      this._closetUI = (ClosetUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ClosetUI", abManifests.Default), transform2, false)).GetComponent<ClosetUI>();
      Debug.Log((object) "登場キャラ選択UI生成");
      this._charaEntryUI = (CharaEntryUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CharaEntryUI", abManifests.Default), transform2, false)).GetComponent<CharaEntryUI>();
      Debug.Log((object) "女の子編集UI生成");
      this._charaChangeUI = (CharaChangeUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CharaChangeUI", abManifests.Default), transform2, false)).GetComponent<CharaChangeUI>();
      Debug.Log((object) "主人公編集UI生成");
      this._playerChangeUI = (PlayerChangeUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "PlayerChangeUI", abManifests.Default), transform2, false)).GetComponent<PlayerChangeUI>();
      Debug.Log((object) "女の子容姿変更UI生成");
      this._charaLookEditUI = (CharaLookEditUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CharaLookEditUI", abManifests.Default), transform2, false)).GetComponent<CharaLookEditUI>();
      Debug.Log((object) "主人公容姿変更UI生成");
      this._playerLookEditUI = (PlayerLookEditUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "PlayerLookEditUI", abManifests.Default), transform2, false)).GetComponent<PlayerLookEditUI>();
      Debug.Log((object) "女の子島変更UI生成");
      this._charaMigrateUI = (CharaMigrateUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd05, "CharaMigrateUI", abManifests.Add05), transform2, false)).GetComponent<CharaMigrateUI>();
      Debug.Log((object) "釣りUI生成");
      GameObject gameObject1 = this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd05, "FishingUI", abManifests.Add05);
      this._fishingUI = !Object.op_Equality((Object) gameObject1, (Object) null) ? (FishingUI) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject1, transform2, false)).GetComponent<FishingUI>() : (FishingUI) null;
      GameObject gameObject2 = this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd05, "RequestUI", abManifests.Add05);
      this._requestUI = !Object.op_Equality((Object) gameObject2, (Object) null) ? (RequestUI) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject2, transform2, false)).GetComponent<RequestUI>() : (RequestUI) null;
      this._photoShotUI = (PhotoShotUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "PhotoShotUI", abManifests.Default), transform2, false)).GetComponent<PhotoShotUI>();
      this._spendMoneyUI = (SpendMoneyUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "SpendMoneyUI", abManifests.Default), transform2, false)).GetComponent<SpendMoneyUI>();
      this._AllAreaMapUI = (AllAreaMapUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd12, "AllAreaMap", abManifests.Add12), transform2, false)).GetComponent<AllAreaMapUI>();
      this._ShopUI = (ShopUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ShopUI", abManifests.Default), transform2, false)).GetComponent<ShopUI>();
      this._ScroungeUI = (ScroungeUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ScroungeUI", abManifests.Default), transform2, false)).GetComponent<ScroungeUI>();
      this._RefrigeratorUI = (RefrigeratorUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "RefrigeratorUI", abManifests.Default), transform2, false)).GetComponent<RefrigeratorUI>();
      this._CraftUI = (CraftUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CraftUI", abManifests.Default), transform2, false)).GetComponent<CraftUI>();
      this._CookingUI = (CraftUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "CookingUI", abManifests.Default), transform2, false)).GetComponent<CraftUI>();
      this._PetCraftUI = (CraftUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "PetCraftUI", abManifests.Default), transform2, false)).GetComponent<CraftUI>();
      this._MedicineCraftUI = (CraftUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "MedicineCraftUI", abManifests.Default), transform2, false)).GetComponent<CraftUI>();
      GameObject gameObject3 = this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd12, "RecyclingUI", abManifests.Add12);
      this._recyclingUI = !Object.op_Inequality((Object) gameObject3, (Object) null) ? (RecyclingUI) null : (RecyclingUI) ((GameObject) Object.Instantiate<GameObject>((M0) gameObject3, transform2, false)).GetComponent<RecyclingUI>();
      this._FarmlandUI = (FarmlandUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefabAdd07, "FarmlandUI", abManifests.Add07), transform2, false)).GetComponent<FarmlandUI>();
      this._ChickenCoopUI = (ChickenCoopUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "ChickenCoopUI", abManifests.Default), transform2, false)).GetComponent<ChickenCoopUI>();
      this._petHomeUI = (PetHomeUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "PetHomeUI", abManifests.Default), transform2, false)).GetComponent<PetHomeUI>();
      this._jukeBoxUI = (JukeBoxUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "JukeBoxUI", abManifests.Default), transform2, false)).GetComponent<JukeBoxUI>();
      this._eventDialogUI = (EventDialogUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "EventDialogUI", abManifests.Default), transform2, false)).GetComponent<EventDialogUI>();
      this._tutorialUI = (TutorialUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.LoadAsset<GameObject>(abPaths.MapScenePrefab, "TutorialUI", abManifests.Default), transform2, false)).GetComponent<TutorialUI>();
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this._centerPointerCanvasGroup, (Object) null))
        return;
      ((Component) this._centerPointerCanvasGroup).get_gameObject().SetActiveIfDifferent(Manager.Config.GameData.CenterPointer);
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      ParameterList.Remove((ADV.IData) this._advData);
    }
  }
}
