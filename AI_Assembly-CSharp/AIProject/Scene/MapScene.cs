// Decompiled with JetBrains decompiler
// Type: AIProject.Scene.MapScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using AIProject.UI;
using Housing;
using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEx;

namespace AIProject.Scene
{
  public class MapScene : Singleton<MapScene>, ISystemCommand
  {
    private List<ICommandData> _systemCommands = new List<ICommandData>();
    [Header("Root")]
    [SerializeField]
    [FormerlySerializedAs("Map")]
    [Indent(1)]
    private Transform _mapRoot;
    [SerializeField]
    [FormerlySerializedAs("Entity")]
    [Indent(1)]
    private Transform _actorRoot;
    [SerializeField]
    private NavMeshSurface _navMeshSurface;
    [SerializeField]
    private Transform _startPositionPoint;
    [SerializeField]
    private Transform _warpPoint;
    private bool _enabledCommandArea;
    private bool[] _charasEntry;

    public static List<ValueTuple<string, string>> AssetBundlePaths { get; private set; } = new List<ValueTuple<string, string>>();

    public NavMeshSurface NavMeshSurface
    {
      get
      {
        return this._navMeshSurface;
      }
    }

    public Transform WarpPoint
    {
      get
      {
        return this._warpPoint;
      }
    }

    public bool EnabledInput { get; set; }

    public void OnUpdateInput()
    {
      Manager.Input instance = Singleton<Manager.Input>.Instance;
      foreach (ICommandData systemCommand in this._systemCommands)
        systemCommand.Invoke(instance);
    }

    public static void AddAssetBundlePath(AssetBundleInfo info)
    {
      if (MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == (string) info.assetbundle && (string) x.Item2 == (string) info.manifest)))
        return;
      MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>((string) info.assetbundle, (string) info.manifest));
    }

    public static void AddAssetBundlePath(string assetBundle, string manifest)
    {
      if (MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == assetBundle && (string) x.Item2 == manifest)))
        return;
      MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(assetBundle, manifest));
    }

    public static void AddAssetBundlePath(string assetBundle)
    {
      if (MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == assetBundle)))
        return;
      MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(assetBundle, string.Empty));
    }

    public bool IsLoading { get; private set; }

    public static bool EqualsSequence(bool[] a, bool[] b)
    {
      if (a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if (a[index] != b[index])
          return false;
      }
      return true;
    }

    private void StartFadingStream(IConnectableObservable<TimeInterval<float>> stream)
    {
      LoadingPanel loadingPanel = Singleton<Manager.Scene>.Instance.loadingPanel;
      float startAlpha = loadingPanel.CanvasGroup.get_alpha();
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (current.Value.AgentData.CarryingItem != null && current.Value.Mode != Desire.ActionType.SearchEatSpot && (current.Value.Mode != Desire.ActionType.EndTaskEat && current.Value.Mode != Desire.ActionType.EndTaskEatThere))
            current.Value.AgentData.CarryingItem = (StuffItem) null;
          current.Value.EnableBehavior();
          current.Value.BehaviorResources.ChangeMode(current.Value.Mode);
        }
      }
      PlayerActor player = instance.Player;
      string stateFromCharaCreate = Game.PrevPlayerStateFromCharaCreate;
      if (!stateFromCharaCreate.IsNullOrEmpty())
      {
        player.PlayerController.ChangeState(stateFromCharaCreate);
        player.CurrentDevicePoint = Singleton<Manager.Map>.Instance.PointAgent.DevicePointDic[Game.PrevAccessDeviceID];
      }
      else
      {
        ReadOnlyDictionary<int, AgentActor> agentTable = instance.AgentTable;
        int? partnerId = player.PlayerData.PartnerID;
        int num = !partnerId.HasValue ? -1 : partnerId.Value;
        AgentActor agentActor;
        ref AgentActor local = ref agentActor;
        if (agentTable.TryGetValue(num, ref local))
        {
          player.Partner = (Actor) agentActor;
          agentActor.Partner = (Actor) player;
          if (player.PlayerData.IsOnbu)
          {
            player.PlayerController.ChangeState("Onbu");
          }
          else
          {
            agentActor.BehaviorResources.ChangeMode(Desire.ActionType.Date);
            agentActor.Mode = Desire.ActionType.Date;
            player.Mode = Desire.ActionType.Date;
            player.PlayerController.ChangeState("Normal");
          }
        }
        else
          player.PlayerController.ChangeState("Normal");
      }
      Game.PrevPlayerStateFromCharaCreate = (string) null;
      this._enabledCommandArea = ((Behaviour) player.PlayerController.CommandArea).get_enabled();
      ((Behaviour) player.PlayerController.CommandArea).set_enabled(false);
      player.PlayerController.CommandArea.InitCommandStates();
      player.PlayerController.CommandArea.RefreshCommands();
      if (Singleton<Game>.Instance.Environment.TutorialProgress == 0)
      {
        FadeItem blackoutPanel = MapUIContainer.FadeCanvas.GetPanel(FadeCanvas.PanelType.Blackout);
        blackoutPanel.CanvasGroup.set_alpha(1f);
        ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) stream, (System.Action<M0>) (x => this.Fade(loadingPanel.CanvasGroup, startAlpha, 0.0f, ((TimeInterval<float>) ref x).get_Value())), (System.Action) (() =>
        {
          loadingPanel.Stop();
          blackoutPanel = MapUIContainer.FadeCanvas.GetPanel(FadeCanvas.PanelType.Blackout);
          blackoutPanel.CanvasGroup.set_alpha(1f);
          this.OnLoaded();
          blackoutPanel = MapUIContainer.FadeCanvas.GetPanel(FadeCanvas.PanelType.Blackout);
          blackoutPanel.CanvasGroup.set_alpha(1f);
        }));
        stream.Connect();
      }
      else
      {
        ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) stream, (System.Action<M0>) (x => this.Fade(loadingPanel.CanvasGroup, startAlpha, 0.0f, ((TimeInterval<float>) ref x).get_Value())), (System.Action) (() =>
        {
          loadingPanel.Stop();
          this.OnLoaded();
        }));
        stream.Connect();
      }
    }

    private void Fade(FadeItem panel, float start, float destination, float value)
    {
      panel.CanvasGroup.set_alpha(Mathf.Lerp(start, destination, value));
    }

    private void Fade(CanvasGroup canvasGroup, float start, float end, float value)
    {
      canvasGroup.set_alpha(Mathf.Lerp(start, end, value));
    }

    private void OnLoaded()
    {
      Debug.Log((object) string.Format("ーーーーー{0}ーーーーー", (object) nameof (OnLoaded)));
      ((Component) Singleton<Manager.Scene>.Instance.loadingPanel).get_gameObject().SetActive(false);
      Singleton<Manager.Map>.Instance.StartSubscribe();
      Singleton<AnimalManager>.Instance.StartSubscribe();
      int tutorialProgress = Manager.Map.GetTutorialProgress();
      if (tutorialProgress != 0)
      {
        Singleton<SoundPlayer>.Instance.StartAllSubscribe();
        Singleton<SoundPlayer>.Instance.PlayActiveAll = true;
      }
      ((Behaviour) Singleton<Manager.Map>.Instance.Player.PlayerController.CommandArea).set_enabled(this._enabledCommandArea);
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      MapUIContainer.SetMarkerTargetCameraTransform(((Component) instance.Player.CameraControl).get_transform());
      Singleton<Manager.Input>.Instance.ReserveState(Manager.Input.ValidType.Action);
      Singleton<Manager.Input>.Instance.SetupState();
      bool tutorialMode = Manager.Map.TutorialMode;
      instance.InitSearchActorTargetsAll();
      if (Object.op_Inequality((Object) instance.TutorialAgent, (Object) null))
        instance.TutorialAgent.ChangeFirstTutorialBehavior();
      if (tutorialMode)
      {
        if (tutorialProgress < 14)
        {
          instance.CreateTutorialLockArea();
          Singleton<MapUIContainer>.Instance.MinimapUI.MinimapLockAreaInit();
        }
        instance.SetActiveMapEffect(false);
        if (tutorialProgress == 3)
          instance.CreateTutorialSearchPoint();
      }
      if (Singleton<Game>.Instance.WorldData.Cleared)
        ;
      if (Singleton<Game>.Instance.Environment.TutorialProgress == 0)
        instance.Player.PlayerController.ChangeState("OpeningWakeUp");
      this.IsLoading = false;
      if (!tutorialMode)
        instance.Simulator.EnabledTimeProgression = true;
      IObservable<long> observable1 = (IObservable<long>) Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled()));
      ObservableExtensions.Subscribe<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) Observable.Select<long, bool>((IObservable<M0>) observable1, (Func<M0, M1>) (_ => MapUIContainer.AnyUIActive()))), (System.Action<M0>) (isOn => this.IsCursorLock = !isOn));
      IObservable<bool> observable2 = (IObservable<bool>) Observable.Select<long, bool>((IObservable<M0>) observable1, (Func<M0, M1>) (_ => this.IsCursorLock));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.DistinctUntilChanged<bool>((IObservable<M0>) observable2), (Func<M0, bool>) (isOn => !isOn)), (System.Action<M0>) (isOn =>
      {
        Cursor.set_lockState((CursorLockMode) 0);
        Cursor.set_visible(true);
      }));
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) observable2, (Func<M0, bool>) (isOn => isOn)), (System.Action<M0>) (_ =>
      {
        Cursor.set_lockState((CursorLockMode) 1);
        Cursor.set_visible(false);
      }), (System.Action) (() =>
      {
        Cursor.set_lockState((CursorLockMode) 0);
        Cursor.set_visible(true);
        Cursor.SetCursor((Texture2D) null, Vector2.get_zero(), (CursorMode) 0);
      }));
      this.IsCursorLock = true;
      this.InitShortCutEvents();
      Manager.Map.RefreshStoryUI();
      if (Game.IsFreeMode && Game.IsFirstGame)
        this.SaveProfile(false);
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>(Observable.ObserveOnMainThread<long>((IObservable<M0>) Observable.Interval(TimeSpan.FromMinutes(10.0))), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ =>
      {
        if (Singleton<CraftScene>.IsInstance())
          return;
        Debug.Log((object) "progressed 10 minutes");
        this.SaveProfile(true);
        Debug.Log((object) "End Saving");
      }));
      Debug.Log((object) string.Format("ーーーーー{0}ーーーーー", (object) nameof (OnLoaded)));
    }

    private void InitShortCutEvents()
    {
      KeyCodeDownCommand keyCodeDownCommand1 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 27
      };
      UnityEvent triggerEvent1 = keyCodeDownCommand1.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cache6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cache6 = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__B));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache6 = MapScene.\u003C\u003Ef__am\u0024cache6;
      triggerEvent1.AddListener(fAmCache6);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand1);
      KeyCodeDownCommand keyCodeDownCommand2 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 325
      };
      UnityEvent triggerEvent2 = keyCodeDownCommand2.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cache7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cache7 = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__C));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache7 = MapScene.\u003C\u003Ef__am\u0024cache7;
      triggerEvent2.AddListener(fAmCache7);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand2);
      KeyCodeDownCommand keyCodeDownCommand3 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 109
      };
      UnityEvent triggerEvent3 = keyCodeDownCommand3.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cache8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cache8 = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__D));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache8 = MapScene.\u003C\u003Ef__am\u0024cache8;
      triggerEvent3.AddListener(fAmCache8);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand3);
      KeyCodeDownCommand keyCodeDownCommand4 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 32
      };
      UnityEvent triggerEvent4 = keyCodeDownCommand4.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cache9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cache9 = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__E));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCache9 = MapScene.\u003C\u003Ef__am\u0024cache9;
      triggerEvent4.AddListener(fAmCache9);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand4);
      KeyCodeDownCommand keyCodeDownCommand5 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 282
      };
      UnityEvent triggerEvent5 = keyCodeDownCommand5.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cacheA == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cacheA = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__F));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCacheA = MapScene.\u003C\u003Ef__am\u0024cacheA;
      triggerEvent5.AddListener(fAmCacheA);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand5);
      KeyCodeDownCommand keyCodeDownCommand6 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 283
      };
      UnityEvent triggerEvent6 = keyCodeDownCommand6.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cacheB == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cacheB = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__10));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCacheB = MapScene.\u003C\u003Ef__am\u0024cacheB;
      triggerEvent6.AddListener(fAmCacheB);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand6);
      KeyCodeDownCommand keyCodeDownCommand7 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 284
      };
      // ISSUE: method pointer
      keyCodeDownCommand7.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CInitShortCutEvents\u003Em__11)));
      this._systemCommands.Add((ICommandData) keyCodeDownCommand7);
      KeyCodeDownCommand keyCodeDownCommand8 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 292
      };
      // ISSUE: method pointer
      keyCodeDownCommand8.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CInitShortCutEvents\u003Em__12)));
      this._systemCommands.Add((ICommandData) keyCodeDownCommand8);
      KeyCodeDownCommand keyCodeDownCommand9 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 107
      };
      UnityEvent triggerEvent7 = keyCodeDownCommand9.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cacheC == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cacheC = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__13));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCacheC = MapScene.\u003C\u003Ef__am\u0024cacheC;
      triggerEvent7.AddListener(fAmCacheC);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand9);
      KeyCodeDownCommand keyCodeDownCommand10 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 108
      };
      UnityEvent triggerEvent8 = keyCodeDownCommand10.TriggerEvent;
      // ISSUE: reference to a compiler-generated field
      if (MapScene.\u003C\u003Ef__am\u0024cacheD == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        MapScene.\u003C\u003Ef__am\u0024cacheD = new UnityAction((object) null, __methodptr(\u003CInitShortCutEvents\u003Em__14));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction fAmCacheD = MapScene.\u003C\u003Ef__am\u0024cacheD;
      triggerEvent8.AddListener(fAmCacheD);
      this._systemCommands.Add((ICommandData) keyCodeDownCommand10);
    }

    public bool IsCursorLock { get; set; }

    public bool isLoadEnd { get; private set; }

    [DebuggerHidden]
    private IEnumerator Load()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapScene.\u003CLoad\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private bool WaitCompletionAgents()
    {
      using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (!enumerator.Current.IsInit)
            return false;
        }
      }
      return true;
    }

    public void SaveProfile(bool isAuto = false)
    {
      if (!Singleton<Game>.IsInstance())
        return;
      string worldSaveDataFile = Path.WorldSaveDataFile;
      WorldData worldData1 = Singleton<Game>.Instance.WorldData;
      worldData1.SaveTime = DateTime.Now;
      worldData1.SaveTimeString = worldData1.SaveTime.ToString("o");
      worldData1.Environment.SetSimulation(Singleton<Manager.Map>.Instance.Simulator);
      PlayerActor player = Singleton<Manager.Map>.Instance.Player;
      worldData1.PlayerData.IsOnbu = player.PlayerController.State != null && player.PlayerController.PrevStateName == "Onbu";
      worldData1.PlayerData.PartnerID = !Object.op_Inequality((Object) player.AgentPartner, (Object) null) ? new int?(-1) : new int?(player.AgentPartner.ID);
      using (IEnumerator<AgentActor> enumerator = Singleton<Manager.Map>.Instance.AgentTable.get_Values().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor current = enumerator.Current;
          current.ChaControl.chaFile.SaveCharaFile(current.ChaControl.chaFile.charaFileName, byte.MaxValue, false);
          current.AgentData.CurrentActionPointID = !Object.op_Inequality((Object) current.CurrentPoint, (Object) null) ? -1 : current.CurrentPoint.RegisterID;
          current.AgentData.ActionTargetID = !Object.op_Inequality((Object) current.TargetInSightActor, (Object) null) ? -1 : current.TargetInSightActor.ID;
          current.AgentData.ScheduleEnabled = current.Schedule.enabled;
          current.AgentData.ScheduleElapsedTime = current.Schedule.elapsedTime;
          current.AgentData.ScheduleDuration = current.Schedule.duration;
          current.AgentData.ReservedWaypointIDList.Clear();
          foreach (Waypoint reservedWaypoint in current.GetReservedWaypoints())
            current.AgentData.ReservedWaypointIDList.Add(reservedWaypoint.ID);
          current.AgentData.WalkRouteWaypointIDList.Clear();
          foreach (Waypoint waypoint in current.WalkRoute)
          {
            if (!Object.op_Equality((Object) waypoint, (Object) null))
              current.AgentData.WalkRouteWaypointIDList.Add(waypoint.ID);
          }
        }
      }
      if (Singleton<AnimalManager>.IsInstance())
        Singleton<AnimalManager>.Instance.SetupSaveDataWildAnimals();
      AIProject.SaveData.SaveData data = Singleton<Game>.Instance.Data;
      if (isAuto)
      {
        if (data.AutoData == null)
          data.AutoData = new WorldData();
        data.AutoData.Copy(worldData1);
      }
      else
      {
        WorldData worldData2;
        if (data.WorldList.TryGetValue(worldData1.WorldID, out worldData2))
        {
          worldData2.Copy(worldData1);
        }
        else
        {
          Debug.LogWarning((object) string.Format("ワールドID = [ {0} ] のデータがなかったので作成", (object) worldData1.WorldID));
          WorldData worldData3 = new WorldData();
          data.WorldList[worldData1.WorldID] = worldData3;
          worldData2 = worldData3;
          worldData2.Copy(worldData1);
        }
      }
      Singleton<Game>.Instance.SaveProfile(worldSaveDataFile);
      Debug.Log((object) "保存完了: MapScene side");
      GC.Collect();
    }

    private void CaptureSS()
    {
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Photo);
      Singleton<Manager.Map>.Instance.Player.CameraControl.ScreenShot.Capture(string.Empty);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      base.Awake();
    }

    [DebuggerHidden]
    private IEnumerator Start()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new MapScene.\u003CStart\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }

    private void Update()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      environment.TotalPlayTime = (AIProject.SaveData.Environment.SerializableTimeSpan) (environment.TotalPlayTime.TimeSpan + TimeSpan.FromSeconds((double) Time.get_unscaledDeltaTime()));
    }

    private void OnDisable()
    {
      if (!Singleton<Manager.Input>.IsInstance())
        return;
      Singleton<Manager.Input>.Instance.SystemElements.Remove((ISystemCommand) this);
      if (!Singleton<MapUIContainer>.IsInstance())
        return;
      Singleton<Manager.Input>.Instance.ActionElements.Remove((IActionCommand) MapUIContainer.CommandLabel);
    }

    protected override void OnDestroy()
    {
      if (Object.op_Inequality((Object) Singleton<MapScene>.Instance, (Object) this))
        return;
      base.OnDestroy();
      if (!Singleton<Manager.Scene>.IsInstance())
        return;
      if (Singleton<Manager.Map>.IsInstance())
        Singleton<Manager.Map>.Instance.ReleaseComponents();
      if (Singleton<Resources>.IsInstance())
      {
        Singleton<Resources>.Instance.ReleaseMapResources();
        Singleton<Resources>.Instance.EndLoadAssetBundle(true);
      }
      if (Singleton<SoundPlayer>.IsInstance())
        Singleton<SoundPlayer>.Instance.Release();
      if (!Singleton<AnimalManager>.IsInstance())
        return;
      Singleton<AnimalManager>.Instance.ActiveMapScene = false;
    }

    public void PushResult(int id, string message)
    {
    }
  }
}
