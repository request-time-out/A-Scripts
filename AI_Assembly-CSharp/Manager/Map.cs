// Decompiled with JetBrains decompiler
// Type: Manager.Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using AIProject.Animal;
using AIProject.Definitions;
using AIProject.MiniGames.Fishing;
using AIProject.Player;
using AIProject.SaveData;
using AIProject.Scene;
using ConfigScene;
using PlaceholderSoftware.WetStuff;
using Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using UnityEx;
using UnityEx.Misc;

namespace Manager
{
  public class Map : Singleton<Map>
  {
    private Dictionary<int, Actor> _actorTable = new Dictionary<int, Actor>();
    private Dictionary<int, AgentActor> _agentTable = new Dictionary<int, AgentActor>();
    private List<int> _agentKeys = new List<int>();
    private List<int> _removeRegIDCache = new List<int>();
    private List<ICommandable> _appendCommandables = new List<ICommandable>();
    private List<Map.VisibleObject> lstMapVanish = new List<Map.VisibleObject>();
    private Transform _enviroGroupRoot;
    [SerializeField]
    private Transform _mapRoot;
    [SerializeField]
    private GameObject _waterObject;
    [SerializeField]
    private PointManager _pointAgent;
    private GameObject _mapDataEffect;
    private List<int> _agentVanishAreaList;
    private GameObject _agentPrefab;
    private GameObject _tutorialSearchPointObject;

    public EnvironmentSimulator Simulator { get; private set; }

    public ReadOnlyDictionary<int, Actor> ActorTable { get; set; }

    public List<Actor> Actors { get; private set; } = new List<Actor>();

    public PlayerActor Player { get; private set; }

    public ReadOnlyDictionary<int, AgentActor> AgentTable { get; private set; }

    public MerchantActor Merchant { get; private set; }

    public AgentActor TutorialAgent { get; set; }

    public static bool TutorialMode
    {
      get
      {
        return Map.GetTutorialProgress() < 16;
      }
    }

    public Dictionary<int, ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE>> EnviroSETable { get; private set; }

    public Dictionary<int, Tuple<Transform, AudioReverbZone[]>> ReverbTable { get; private set; }

    public Transform EnviroGroupRoot
    {
      get
      {
        if (Object.op_Inequality((Object) this._enviroGroupRoot, (Object) null))
          return this._enviroGroupRoot;
        this._enviroGroupRoot = new GameObject("Enviro Group Root").get_transform();
        this._enviroGroupRoot.SetParent(this._mapRoot, false);
        return this._enviroGroupRoot;
      }
    }

    public Dictionary<int, Transform> EnviroRootElement { get; private set; }

    public Dictionary<int, ValueTuple<bool, List<Env3DSEPoint>>> HousingEnvSEPointTable { get; } = new Dictionary<int, ValueTuple<bool, List<Env3DSEPoint>>>();

    public Dictionary<int, Dictionary<bool, ForcedHideObject[]>> AreaOpenObjectTable { get; private set; } = new Dictionary<int, Dictionary<bool, ForcedHideObject[]>>();

    public Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>> TimeRelationObjectTable { get; private set; } = new Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>>();

    public EnvironmentProfile EnvironmentProfile
    {
      get
      {
        return this.Simulator?.EnvironmentProfile;
      }
    }

    public Transform MapRoot
    {
      get
      {
        return this._mapRoot;
      }
      set
      {
        this._mapRoot = value;
        this._pointAgent = (PointManager) ((Component) value).GetComponentInChildren<PointManager>(true);
      }
    }

    public GameObject WaterObject
    {
      get
      {
        return this._waterObject;
      }
      set
      {
        this._waterObject = value;
      }
    }

    public AQUAS_Reflection[] WaterRefrections { get; set; }

    public Transform PlayerStartPoint { get; set; }

    public Transform ActorRoot { get; set; }

    public NavMeshSurface NavMeshSurface { get; set; }

    public GameObject NavMeshSrc { get; set; }

    public GameObject ChunkSrc { get; set; }

    public PointManager PointAgent
    {
      get
      {
        return this._pointAgent;
      }
    }

    public int MapID { get; private set; }

    public int AccessDeviceID { get; set; } = -1;

    public Dictionary<int, Dictionary<int, Transform>> EventStartPointDic { get; set; } = new Dictionary<int, Dictionary<int, Transform>>();

    public Dictionary<int, string> ChangedCharaFiles { get; set; } = new Dictionary<int, string>();

    public Dictionary<int, Transform> HousingPointTable { get; set; } = new Dictionary<int, Transform>();

    public int HousingID { get; set; }

    public int HousingAreaID { get; set; }

    public Dictionary<int, List<Transform>> HousingRecoveryPointTable { get; set; } = new Dictionary<int, List<Transform>>();

    public Dictionary<int, Dictionary<int, List<WarpPoint>>> WarpPointDic { get; set; } = new Dictionary<int, Dictionary<int, List<WarpPoint>>>();

    public FishingManager FishingSystem { get; set; }

    public Dictionary<int, Chunk> ChunkTable { get; } = new Dictionary<int, Chunk>();

    public Dictionary<int, GameObject> MapGroupObjList { get; private set; } = new Dictionary<int, GameObject>();

    public Dictionary<int, ValueTuple<Desire.ActionType, Desire.ActionType, bool>> AgentModeCache { get; set; } = new Dictionary<int, ValueTuple<Desire.ActionType, Desire.ActionType, bool>>();

    public bool IsHour7After
    {
      get
      {
        return this.Simulator.Now.Hour >= 7;
      }
    }

    public List<TimeLinkedLightObject> TimeLinkedLightObjectList { get; private set; } = new List<TimeLinkedLightObject>();

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.ActorTable = new ReadOnlyDictionary<int, Actor>((IDictionary<int, Actor>) this._actorTable);
      this.AgentTable = new ReadOnlyDictionary<int, AgentActor>((IDictionary<int, AgentActor>) this._agentTable);
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
      LayerMask areaDetectionLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.AreaDetectionLayer;
      foreach (Actor actor in this.Actors)
      {
        RaycastHit raycastHit;
        if (Physics.Raycast(Vector3.op_Addition(actor.Position, Vector3.op_Multiply(Vector3.get_up(), 5f)), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(areaDetectionLayer)))
        {
          bool flag = false;
          foreach (KeyValuePair<int, Chunk> keyValuePair in this.ChunkTable)
          {
            foreach (MapArea mapArea in keyValuePair.Value.MapAreas)
            {
              if (flag = mapArea.ContainsCollider(((RaycastHit) ref raycastHit).get_collider()))
              {
                actor.MapArea = mapArea;
                actor.ChunkID = mapArea.ChunkID;
                actor.AreaID = mapArea.AreaID;
                break;
              }
            }
            if (flag)
              break;
          }
          if (!flag)
            actor.MapArea = (MapArea) null;
        }
        else
          actor.MapArea = (MapArea) null;
      }
      if (Object.op_Inequality((Object) this.Player, (Object) null))
      {
        this.AreaTypeUpdate((Actor) this.Player);
        if (Object.op_Inequality((Object) this.Player.MapArea, (Object) null))
          this.Player.PlayerData.AreaID = this.Player.MapArea.AreaID;
      }
      if (Object.op_Inequality((Object) this.Merchant, (Object) null))
        this.AreaTypeUpdate((Actor) this.Merchant);
      if (this._agentTable.IsNullOrEmpty<int, AgentActor>())
        return;
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
      {
        if (!Object.op_Equality((Object) keyValuePair.Value, (Object) null))
        {
          this.AreaTypeUpdate((Actor) keyValuePair.Value);
          if (!Object.op_Equality((Object) keyValuePair.Value.MapArea, (Object) null))
            keyValuePair.Value.AgentData.AreaID = keyValuePair.Value.MapArea.AreaID;
        }
      }
    }

    public bool UpdateTexSetting { get; set; } = true;

    private void Update()
    {
      GraphicSystem graphicData = Config.GraphicData;
      if (graphicData != null)
      {
        bool selfShadow = graphicData.SelfShadow;
        int qualityLevel = QualitySettings.GetQualityLevel();
        int num = qualityLevel / 2 * 2 + (!selfShadow ? 1 : 0);
        if (num != qualityLevel)
          QualitySettings.SetQualityLevel(num);
        if (this.UpdateTexSetting)
        {
          byte mapGraphicQuality = graphicData.MapGraphicQuality;
          if (QualitySettings.get_masterTextureLimit() != (int) mapGraphicQuality)
            QualitySettings.set_masterTextureLimit((int) mapGraphicQuality);
        }
      }
      PlayerActor player = this.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return;
      if (Object.op_Inequality((Object) this.Simulator, (Object) null))
      {
        DecalSettings settings = player.CameraControl.WetDecal.get_Settings();
        float num = 0.0f;
        switch (this.Simulator.Weather)
        {
          case Weather.Rain:
            settings.set_Saturation(Mathf.SmoothDamp(settings.get_Saturation(), 1f, ref num, 0.5f));
            break;
          case Weather.Storm:
            settings.set_Saturation(Mathf.SmoothDamp(settings.get_Saturation(), 1f, ref num, 0.3f));
            break;
          default:
            settings.set_Saturation(Mathf.SmoothDamp(settings.get_Saturation(), 0.0f, ref num, 1f));
            break;
        }
      }
      if (Singleton<Game>.IsInstance() && Singleton<Game>.Instance.Environment != null)
      {
        foreach (KeyValuePair<int, List<AIProject.SaveData.Environment.PlantInfo>> keyValuePair in Singleton<Game>.Instance.Environment.FarmlandTable)
        {
          foreach (AIProject.SaveData.Environment.PlantInfo plantInfo in keyValuePair.Value)
            plantInfo?.AddTimer(Time.get_unscaledDeltaTime());
        }
      }
      if (Singleton<Game>.IsInstance() && Singleton<Game>.Instance.Environment != null)
      {
        Dictionary<int, RecyclingData> recyclingDataTable = Singleton<Game>.Instance.Environment.RecyclingDataTable;
        if (!recyclingDataTable.IsNullOrEmpty<int, RecyclingData>())
        {
          foreach (KeyValuePair<int, RecyclingData> keyValuePair in recyclingDataTable)
          {
            RecyclingData recyclingData = keyValuePair.Value;
            if (recyclingData != null && recyclingData.CreateCountEnabled)
              recyclingData.CreateCounter += Time.get_unscaledDeltaTime();
          }
        }
      }
      if (Singleton<Game>.IsInstance() && Singleton<Game>.Instance.Environment != null)
      {
        foreach (KeyValuePair<int, AIProject.SaveData.Environment.SearchActionInfo> keyValuePair in Singleton<Game>.Instance.Environment.SearchActionLockTable)
        {
          AIProject.SaveData.Environment.SearchActionInfo searchActionInfo = keyValuePair.Value;
          if (searchActionInfo.Count != 0)
          {
            searchActionInfo.ElapsedTime += Time.get_unscaledDeltaTime();
            if ((double) searchActionInfo.ElapsedTime > (double) this.EnvironmentProfile.SearchCoolTimeDuration)
            {
              searchActionInfo.ElapsedTime = 0.0f;
              searchActionInfo.Count = 0;
            }
          }
        }
      }
      float talkLockDuration = Singleton<Resources>.Instance.AgentProfile.TalkLockDuration;
      float dynamicBoneDistance = Singleton<Resources>.Instance.LocomotionProfile.EffectiveDynamicBoneDistance;
      float charaVisibleDistance = Singleton<Resources>.Instance.LocomotionProfile.CharaVisibleDistance;
      Transform transform = ((Component) player.CameraControl).get_transform();
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this._agentTable.TryGetValue(agentKey, out agentActor))
        {
          if (agentActor.AgentData.LockTalk)
          {
            agentActor.AgentData.TalkElapsedTime += Time.get_unscaledDeltaTime();
            if ((double) agentActor.AgentData.TalkElapsedTime > (double) talkLockDuration)
            {
              agentActor.AgentData.LockTalk = false;
              agentActor.AgentData.TalkElapsedTime = 0.0f;
              agentActor.AgentData.TalkMotivation = agentActor.AgentData.StatsTable[5];
            }
          }
          agentActor.AgentData.CallCTCount += Time.get_unscaledDeltaTime();
          if (agentActor.AgentData.IsPlayerForBirthdayEvent && !player.IsBirthday((AgentActor) null))
            agentActor.AgentData.IsPlayerForBirthdayEvent = false;
          foreach (KeyValuePair<int, AIProject.SaveData.Environment.SearchActionInfo> keyValuePair in agentActor.AgentData.SearchActionLockTable)
          {
            AIProject.SaveData.Environment.SearchActionInfo searchActionInfo = keyValuePair.Value;
            if (searchActionInfo.Count != 0)
            {
              searchActionInfo.ElapsedTime += Time.get_deltaTime();
              if ((double) searchActionInfo.ElapsedTime > (double) this.EnvironmentProfile.SearchCoolTimeDuration)
              {
                searchActionInfo.ElapsedTime = 0.0f;
                searchActionInfo.Count = 0;
              }
            }
          }
          StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
          if (agentActor.AgentData.ColdLockInfo.Lock)
          {
            SickLockInfo coldLockInfo = agentActor.AgentData.ColdLockInfo;
            coldLockInfo.ElapsedTime += Time.get_deltaTime();
            if ((double) coldLockInfo.ElapsedTime > (double) statusProfile.ColdLockDuration)
            {
              coldLockInfo.Lock = false;
              coldLockInfo.ElapsedTime = 0.0f;
            }
          }
          if (agentActor.AgentData.HeatStrokeLockInfo.Lock)
          {
            SickLockInfo heatStrokeLockInfo = agentActor.AgentData.HeatStrokeLockInfo;
            heatStrokeLockInfo.ElapsedTime += Time.get_deltaTime();
            if ((double) heatStrokeLockInfo.ElapsedTime > (double) statusProfile.HeatStrokeLockDuration)
            {
              heatStrokeLockInfo.Lock = false;
              heatStrokeLockInfo.ElapsedTime = 0.0f;
            }
          }
          if (((Behaviour) agentActor).get_enabled() && ((Behaviour) agentActor.Controller).get_enabled() && ((Behaviour) agentActor.AnimationAgent).get_enabled())
          {
            if (!this._agentVanishAreaList.IsNullOrEmpty<int>())
              agentActor.Visible = !this._agentVanishAreaList.Contains(agentActor.AreaID);
            else
              agentActor.Visible = true;
            agentActor.ChaControl.visibleAll = agentActor.Visible && agentActor.IsVisible && agentActor.IsVisibleDistanceAll(transform);
            if (!(player.PlayerController.State is Sex))
            {
              bool flag1 = agentActor.ChaControl.fileGameInfo.flavorState[2] >= Singleton<Resources>.Instance.StatusProfile.LampEquipableBorder;
              bool flag2 = this.Simulator.TimeZone == AIProject.TimeZone.Night;
              agentActor.ChaControl.ShowExtraAccessory(ChaControlDefine.ExtraAccessoryParts.Waist, flag1 && flag2);
            }
            ItemScrounge itemScrounge = agentActor.AgentData.ItemScrounge;
            itemScrounge.AddTimer(Time.get_deltaTime());
            if (itemScrounge.isEnd)
            {
              itemScrounge.Reset();
              int id1 = (int) ((IEnumerable<FlavorSkill.Type>) new FlavorSkill.Type[3]
              {
                FlavorSkill.Type.Reliability,
                FlavorSkill.Type.Sociability,
                FlavorSkill.Type.Reason
              }.Shuffle<FlavorSkill.Type>()).First<FlavorSkill.Type>();
              agentActor.AgentData.SetFlavorSkill(id1, agentActor.ChaControl.fileGameInfo.flavorState[id1] - 20);
              int id2 = (int) ((IEnumerable<FlavorSkill.Type>) new FlavorSkill.Type[3]
              {
                FlavorSkill.Type.Darkness,
                FlavorSkill.Type.Wariness,
                FlavorSkill.Type.Instinct
              }.Shuffle<FlavorSkill.Type>()).First<FlavorSkill.Type>();
              agentActor.AgentData.SetFlavorSkill(id2, agentActor.ChaControl.fileGameInfo.flavorState[id2] + 20);
              int id3 = 1;
              agentActor.SetStatus(id3, agentActor.AgentData.StatsTable[id3] - 30f);
              int id4 = 7;
              agentActor.SetStatus(id4, (float) (agentActor.ChaControl.fileGameInfo.morality - 5));
            }
          }
        }
      }
      MerchantActor merchant = this.Merchant;
      if (!Object.op_Inequality((Object) merchant, (Object) null) || !((Behaviour) merchant).get_enabled() || (!((Behaviour) merchant.Controller).get_enabled() || !((Behaviour) merchant.AnimationMerchant).get_enabled()))
        return;
      merchant.ChaControl.visibleAll = merchant.IsVisible && merchant.IsVisibleDistanceAll(transform);
    }

    [DebuggerHidden]
    public IEnumerator LoadMap(int id)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadMap\u003Ec__Iterator0()
      {
        id = id,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadMap(string assetBundleName, string assetName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadMap\u003Ec__Iterator1()
      {
        assetBundleName = assetBundleName,
        assetName = assetName
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadEnvironment()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadEnvironment\u003Ec__Iterator2()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadDemoMap()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Map.\u003CLoadDemoMap\u003Ec__Iterator3 demoMapCIterator3 = new Map.\u003CLoadDemoMap\u003Ec__Iterator3();
      return (IEnumerator) demoMapCIterator3;
    }

    [DebuggerHidden]
    public IEnumerator LoadDemoEnvironment(string assetName)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadDemoEnvironment\u003Ec__Iterator4()
      {
        assetName = assetName,
        \u0024this = this
      };
    }

    public GameObject DemoLightObject { get; private set; }

    [DebuggerHidden]
    public IEnumerator LoadNavMeshSource()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadNavMeshSource\u003Ec__Iterator5()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadElements()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadElements\u003Ec__Iterator6()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadHousingObj(int mapID = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadHousingObj\u003Ec__Iterator7()
      {
        mapID = mapID
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadMerchantPoint()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadMerchantPoint\u003Ec__Iterator8()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadEventPoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadEventPoints\u003Ec__Iterator9()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadStoryPoints()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadStoryPoints\u003Ec__IteratorA()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadEnviroObject()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadEnviroObject\u003Ec__IteratorB()
      {
        \u0024this = this
      };
    }

    private Transform GetEnviroRootElement(int areaID)
    {
      if (this.EnviroRootElement == null)
        this.EnviroRootElement = new Dictionary<int, Transform>();
      Transform transform = (Transform) null;
      if (!this.EnviroRootElement.TryGetValue(areaID, out transform) || Object.op_Equality((Object) transform, (Object) null))
      {
        this.EnviroRootElement[areaID] = transform = new GameObject(string.Format("Enviro Root Element {0}", (object) areaID.ToString("00"))).get_transform();
        transform.SetParent(this.EnviroGroupRoot, false);
      }
      return transform;
    }

    public bool ActiveEnvAreaID(int areaID)
    {
      if (this.MapID < 0 || areaID < 0 || !Singleton<Resources>.IsInstance())
        return false;
      MapArea mapArea = !Object.op_Inequality((Object) this.Player, (Object) null) ? (MapArea) null : this.Player.MapArea;
      if (Object.op_Equality((Object) mapArea, (Object) null))
        return false;
      int areaId = mapArea.AreaID;
      if (areaID == areaId)
        return true;
      Dictionary<int, Dictionary<int, int[]>> adjacentInfoTable = Singleton<Resources>.Instance.Sound.EnviroSEAdjacentInfoTable;
      Dictionary<int, int[]> source;
      int[] numArray;
      return !adjacentInfoTable.IsNullOrEmpty<int, Dictionary<int, int[]>>() && adjacentInfoTable.TryGetValue(this.MapID, out source) && (!source.IsNullOrEmpty<int, int[]>() && source.TryGetValue(areaId, out numArray)) && !((IList<int>) numArray).IsNullOrEmpty<int>() && ((IEnumerable<int>) numArray).Contains<int>(areaID);
    }

    [DebuggerHidden]
    private IEnumerator LoadEnviroSEObject()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadEnviroSEObject\u003Ec__IteratorC()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadReverbObject()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadReverbObject\u003Ec__IteratorD()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadAnimalPoint()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadAnimalPoint\u003Ec__IteratorE()
      {
        \u0024this = this
      };
    }

    public void StartSubscribe()
    {
      ObservableExtensions.Subscribe<Weather>(Observable.OnErrorRetry<Weather, Exception>(Observable.Do<Weather>(Observable.Where<Weather>(Observable.TakeUntilDestroy<Weather>((IObservable<M0>) this.Simulator.OnWeatherChangedAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (weather => this.RefreshWeather(weather))), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnWeatherChangedAsObservable"));
      })));
      ObservableExtensions.Subscribe<AIProject.TimeZone>(Observable.OnErrorRetry<AIProject.TimeZone, Exception>(Observable.Do<AIProject.TimeZone>(Observable.Where<AIProject.TimeZone>(Observable.TakeUntilDestroy<AIProject.TimeZone>((IObservable<M0>) this.Simulator.OnTimeZoneChangedAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (timeZone => this.RefreshTimeZone(timeZone))), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnTimeZoneChangedAsObservable"));
      })));
      ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Do<TimeSpan>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) this.Simulator.OnDayAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (timeSpan => this.OnElapsedDay(timeSpan))), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnDayAsObservable"));
      })));
      ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Do<TimeSpan>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) this.Simulator.OnMinuteAsObservable(), ((Component) this.Simulator).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (timeSpan => this.OnElapsedMinute(timeSpan))), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します: {0} -> OnElapsedMinute", (object) "OnMinuteAsObservable"));
      })));
      ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Do<TimeSpan>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) this.Simulator.OnMinuteAsObservable(), ((Component) this.Simulator).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (timeSpan => this.Simulator.OnMinuteUpdate(timeSpan))), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します：{0} -> Simulator.OnMinuteUpdate", (object) "OnMinuteAsObservable"));
      })));
      ObservableExtensions.Subscribe<TimeSpan>(Observable.OnErrorRetry<TimeSpan, Exception>(Observable.Where<TimeSpan>(Observable.TakeUntilDestroy<TimeSpan>((IObservable<M0>) this.Simulator.OnSecondAsObservable(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します: {0}", (object) "OnSecondAsObservable"));
      })), (System.Action<M0>) (timeSpan => this.OnElapsedSecond(timeSpan)), (System.Action<Exception>) (ex => Debug.LogException(ex)), (System.Action) (() => {}));
      ObservableExtensions.Subscribe<AIProject.TimeZone>(Observable.OnErrorRetry<AIProject.TimeZone, Exception>(Observable.Do<AIProject.TimeZone>(Observable.Where<AIProject.TimeZone>(Observable.TakeUntilDestroy<AIProject.TimeZone>((IObservable<M0>) this.Simulator.OnMapLightTimeZoneChangedAsObservable(), (Component) this.Simulator), (Func<M0, bool>) (_ => ((Behaviour) this.Simulator).get_isActiveAndEnabled())), (System.Action<M0>) (timeZone => this.RefreshActiveTimeRelationObjects())), (System.Action<M1>) (ex =>
      {
        Debug.LogException(ex);
        if (!Debug.get_isDebugBuild())
          return;
        Debug.Log((object) string.Format("再購読します：{0}", (object) "OnMapLightTimeZoneChangedAsObservable"));
      })));
      ObservableExtensions.Subscribe<int>(Observable.Do<int>((IObservable<M0>) this.Player.OnMapAreaChangedAsObservable(), (System.Action<M0>) (areaID =>
      {
        this.OnMapAreaChanged(areaID);
        this.ResettingEnviroAreaElement(this.MapID, areaID);
        this.RefreshHousingEnv3DSEPoints(this.MapID, areaID);
      })));
    }

    private void OnMapAreaChanged(int areaID)
    {
      int mapId = this.MapID;
      Dictionary<int, List<int>> dictionary1;
      List<int> intList;
      if (Singleton<Resources>.Instance.Map.MapGroupHiddenAreaList.TryGetValue(mapId, out dictionary1) && dictionary1.TryGetValue(areaID, out intList))
      {
        using (Dictionary<int, GameObject>.Enumerator enumerator = this.MapGroupObjList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, GameObject> current = enumerator.Current;
            if (!Object.op_Equality((Object) current.Value, (Object) null))
              current.Value.SetActive(!intList.Contains(current.Key));
          }
        }
        ((Component) this.HousingPointTable[0]).get_gameObject().SetActive(!intList.Contains(100));
        ((Component) this.HousingPointTable[1]).get_gameObject().SetActive(!intList.Contains(101));
        ((Component) this.HousingPointTable[2]).get_gameObject().SetActive(!intList.Contains(102));
      }
      Dictionary<int, List<int>> dictionary2;
      if (Singleton<Resources>.Instance.Map.AgentHiddenAreaList.TryGetValue(mapId, out dictionary2))
      {
        if (!dictionary2.TryGetValue(areaID, out this._agentVanishAreaList))
          this._agentVanishAreaList = (List<int>) null;
      }
      else
        this._agentVanishAreaList = (List<int>) null;
      ActorCameraControl cameraControl = this.Player.CameraControl;
      int[] numArray;
      if (Singleton<Resources>.Instance.PlayerProfile.DisableWaterVFXAreaList.TryGetValue(mapId, out numArray))
      {
        bool flag = !((IEnumerable<int>) numArray).Contains<int>(areaID);
        ((Behaviour) cameraControl.UnderWaterFX).set_enabled(flag);
        ((Behaviour) cameraControl.UnderWaterBlurFX).set_enabled(flag);
      }
      else
      {
        ((Behaviour) cameraControl.UnderWaterFX).set_enabled(true);
        ((Behaviour) cameraControl.UnderWaterBlurFX).set_enabled(true);
      }
    }

    public void SetAgentOpenState(int id, bool openState)
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData?.AgentTable;
      AgentData agentData;
      if (agentTable == null || !agentTable.TryGetValue(id, out agentData) || agentData.OpenState == openState)
        return;
      agentData.OpenState = openState;
      PlayerActor player = this.Player;
      player.PlayerController.CommandArea.UpdateCollision(player);
    }

    public bool GetBasePointOpenState(int id, out bool flag)
    {
      Dictionary<int, Dictionary<int, bool>> basePointOpenState = Singleton<Game>.Instance.WorldData?.Environment?.BasePointOpenState;
      Dictionary<int, bool> dictionary1;
      if (basePointOpenState.TryGetValue(this.MapID, out dictionary1))
        return dictionary1.TryGetValue(id, out flag);
      Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
      basePointOpenState[this.MapID] = dictionary2;
      dictionary1 = dictionary2;
      flag = false;
      return false;
    }

    public bool SetBaseOpenState(int id, bool openState)
    {
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, Dictionary<int, bool>> basePointOpenState = Singleton<Game>.Instance.WorldData?.Environment?.BasePointOpenState;
        if (basePointOpenState != null)
        {
          Dictionary<int, bool> dictionary1;
          if (!basePointOpenState.TryGetValue(this.MapID, out dictionary1))
          {
            Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
            basePointOpenState[this.MapID] = dictionary2;
            dictionary1 = dictionary2;
          }
          bool flag;
          dictionary1.TryGetValue(id, out flag);
          if (flag != openState)
          {
            dictionary1[id] = openState;
            return true;
          }
        }
      }
      return false;
    }

    public bool SetBaseOpenState(int mapID, int id, bool openState)
    {
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, Dictionary<int, bool>> basePointOpenState = Singleton<Game>.Instance.WorldData?.Environment?.BasePointOpenState;
        if (basePointOpenState != null)
        {
          Dictionary<int, bool> dictionary1;
          if (!basePointOpenState.TryGetValue(mapID, out dictionary1))
          {
            Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
            basePointOpenState[mapID] = dictionary2;
            dictionary1 = dictionary2;
          }
          bool flag;
          dictionary1.TryGetValue(id, out flag);
          if (flag != openState)
          {
            dictionary1[id] = openState;
            return true;
          }
        }
      }
      return false;
    }

    public void InitializeDefaultState()
    {
      this.InitializeDefaultAreaOpenID();
      this.InitializeDefaultTimeRelationStateID();
      this.RefreshTutorialOpenState();
    }

    public void InitializeDefaultAreaOpenID()
    {
      if (!Singleton<Resources>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      bool isFreeMode = Game.IsFreeMode;
      Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment?.AreaOpenState;
      Dictionary<int, string> areaOpenIdTable = Singleton<Resources>.Instance.Map.AreaOpenIDTable;
      if (areaOpenState == null || areaOpenIdTable == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in areaOpenIdTable)
      {
        if (isFreeMode)
          areaOpenState[keyValuePair.Key] = true;
        else if (!areaOpenState.ContainsKey(keyValuePair.Key))
          areaOpenState[keyValuePair.Key] = false;
      }
    }

    public void InitializeDefaultTimeRelationStateID()
    {
      if (!Singleton<Resources>.IsInstance() || !Singleton<Game>.IsInstance())
        return;
      bool isFreeMode = Game.IsFreeMode;
      Dictionary<int, bool> timeObjOpenState = Singleton<Game>.Instance.Environment?.TimeObjOpenState;
      Dictionary<int, string> relationObjectIdTable = Singleton<Resources>.Instance.Map.TimeRelationObjectIDTable;
      if (timeObjOpenState == null || relationObjectIdTable == null)
        return;
      foreach (KeyValuePair<int, string> keyValuePair in relationObjectIdTable)
      {
        if (isFreeMode)
          timeObjOpenState[keyValuePair.Key] = true;
        else if (!timeObjOpenState.ContainsKey(keyValuePair.Key))
          timeObjOpenState[keyValuePair.Key] = false;
      }
    }

    public void SetOpenAreaState(int id, bool active)
    {
      bool flag1 = false;
      if (Singleton<Game>.IsInstance())
      {
        Dictionary<int, bool> dictionary1 = Singleton<Game>.Instance.Environment.AreaOpenState;
        if (dictionary1 == null)
        {
          Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
          Singleton<Game>.Instance.Environment.AreaOpenState = dictionary2;
          dictionary1 = dictionary2;
        }
        bool flag2;
        flag1 = !dictionary1.TryGetValue(id, out flag2) || flag2 != active;
        dictionary1[id] = active;
      }
      Dictionary<bool, ForcedHideObject[]> source;
      if (!this.AreaOpenObjectTable.IsNullOrEmpty<int, Dictionary<bool, ForcedHideObject[]>>() && this.AreaOpenObjectTable.TryGetValue(id, out source) && !source.IsNullOrEmpty<bool, ForcedHideObject[]>())
      {
        foreach (KeyValuePair<bool, ForcedHideObject[]> keyValuePair in source)
        {
          if (!((IList<ForcedHideObject>) keyValuePair.Value).IsNullOrEmpty<ForcedHideObject>())
          {
            bool key = keyValuePair.Key;
            foreach (ForcedHideObject forcedHideObject in keyValuePair.Value)
            {
              if (!Object.op_Equality((Object) forcedHideObject, (Object) null))
              {
                bool active1 = !key ? !active : active;
                forcedHideObject.SetActive(active1);
              }
            }
          }
        }
      }
      if (Object.op_Inequality((Object) this.PointAgent, (Object) null))
      {
        EventPoint[] eventPoints = this.PointAgent.EventPoints;
        if (!((IList<EventPoint>) eventPoints).IsNullOrEmpty<EventPoint>())
        {
          foreach (EventPoint eventPoint in eventPoints)
          {
            if (!Object.op_Equality((Object) eventPoint, (Object) null) && eventPoint.OpenAreaID >= 0 && eventPoint.OpenAreaID == id)
              eventPoint.SetActive(!active);
          }
        }
      }
      List<GameObject> self;
      if (!AreaOpenLinkedObject.Table.IsNullOrEmpty<int, List<GameObject>>() && AreaOpenLinkedObject.Table.TryGetValue(id, ref self) && !self.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = self.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (!Object.op_Equality((Object) current, (Object) null) && current.get_activeSelf() != active)
              current.SetActive(active);
          }
        }
      }
      if (!flag1)
        return;
      if (Singleton<MapUIContainer>.IsInstance() && Object.op_Inequality((Object) Singleton<MapUIContainer>.Instance.MinimapUI, (Object) null))
        Singleton<MapUIContainer>.Instance.MinimapUI.RoadNaviMesh.Reflesh();
      if (!Object.op_Inequality((Object) this.Merchant, (Object) null) || !active)
        return;
      this.Merchant.SetOpenAreaID(this);
    }

    public bool GetOpenAreaState(int id)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      bool? nullable1;
      if (environment == null)
      {
        nullable1 = new bool?();
      }
      else
      {
        Dictionary<int, bool> areaOpenState = environment.AreaOpenState;
        nullable1 = areaOpenState != null ? new bool?(areaOpenState.IsNullOrEmpty<int, bool>()) : new bool?();
      }
      bool? nullable2 = nullable1;
      bool flag;
      return (!nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0)) == 0 && Singleton<Game>.Instance.Environment.AreaOpenState.TryGetValue(id, out flag) && flag;
    }

    public void RefreshAreaOpenObject()
    {
      if (this.AreaOpenObjectTable.IsNullOrEmpty<int, Dictionary<bool, ForcedHideObject[]>>())
        return;
      int num;
      if (Singleton<Game>.IsInstance())
      {
        AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
        bool? nullable1;
        if (environment == null)
        {
          nullable1 = new bool?();
        }
        else
        {
          Dictionary<int, bool> areaOpenState = environment.AreaOpenState;
          nullable1 = areaOpenState != null ? new bool?(areaOpenState.IsNullOrEmpty<int, bool>()) : new bool?();
        }
        bool? nullable2 = nullable1;
        num = !nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0);
      }
      else
        num = 1;
      bool flag1 = num != 0;
      bool isFreeMode = Game.IsFreeMode;
      if (flag1)
      {
        foreach (KeyValuePair<int, Dictionary<bool, ForcedHideObject[]>> keyValuePair1 in this.AreaOpenObjectTable)
        {
          if (!keyValuePair1.Value.IsNullOrEmpty<bool, ForcedHideObject[]>())
          {
            foreach (KeyValuePair<bool, ForcedHideObject[]> keyValuePair2 in keyValuePair1.Value)
            {
              if (!((IList<ForcedHideObject>) keyValuePair2.Value).IsNullOrEmpty<ForcedHideObject>())
              {
                bool flag2 = keyValuePair2.Key;
                if (isFreeMode)
                  flag2 = !flag2;
                foreach (ForcedHideObject forcedHideObject in keyValuePair2.Value)
                {
                  if (!Object.op_Equality((Object) forcedHideObject, (Object) null))
                    forcedHideObject.SetActive(!flag2);
                }
              }
            }
          }
        }
      }
      else
      {
        Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment.AreaOpenState;
        foreach (KeyValuePair<int, Dictionary<bool, ForcedHideObject[]>> keyValuePair1 in this.AreaOpenObjectTable)
        {
          if (!keyValuePair1.Value.IsNullOrEmpty<bool, ForcedHideObject[]>())
          {
            int key1 = keyValuePair1.Key;
            foreach (KeyValuePair<bool, ForcedHideObject[]> keyValuePair2 in keyValuePair1.Value)
            {
              if (!((IList<ForcedHideObject>) keyValuePair2.Value).IsNullOrEmpty<ForcedHideObject>())
              {
                bool key2 = keyValuePair2.Key;
                foreach (ForcedHideObject forcedHideObject in keyValuePair2.Value)
                {
                  if (!Object.op_Equality((Object) forcedHideObject, (Object) null))
                  {
                    bool active;
                    if (!areaOpenState.TryGetValue(key1, out active))
                    {
                      bool flag2 = isFreeMode;
                      areaOpenState[key1] = flag2;
                      active = flag2;
                    }
                    if (!key2)
                      active = !active;
                    forcedHideObject.SetActive(active);
                  }
                }
              }
            }
          }
        }
      }
    }

    public void RefreshEventPointActive()
    {
      if (Object.op_Equality((Object) this.PointAgent, (Object) null))
        return;
      EventPoint[] eventPointArray = this.PointAgent.EventPoints;
      if (((IList<EventPoint>) eventPointArray).IsNullOrEmpty<EventPoint>() && Object.op_Inequality((Object) this.PointAgent.EventPointRoot, (Object) null))
        eventPointArray = (EventPoint[]) ((Component) this.PointAgent.EventPointRoot).GetComponentsInChildren<EventPoint>(true);
      if (((IList<EventPoint>) eventPointArray).IsNullOrEmpty<EventPoint>())
        return;
      int num;
      if (Singleton<Game>.IsInstance())
      {
        AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
        bool? nullable1;
        if (environment == null)
        {
          nullable1 = new bool?();
        }
        else
        {
          Dictionary<int, bool> areaOpenState = environment.AreaOpenState;
          nullable1 = areaOpenState != null ? new bool?(areaOpenState.IsNullOrEmpty<int, bool>()) : new bool?();
        }
        bool? nullable2 = nullable1;
        num = !nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0);
      }
      else
        num = 1;
      bool flag1 = num != 0;
      bool isFreeMode = Game.IsFreeMode;
      if (flag1)
      {
        foreach (EventPoint eventPoint in eventPointArray)
        {
          if (!Object.op_Equality((Object) eventPoint, (Object) null) && eventPoint.OpenAreaID >= 0 && !((Component) eventPoint).get_gameObject().get_activeSelf())
            ((Component) eventPoint).get_gameObject().SetActive(!isFreeMode);
        }
      }
      else
      {
        Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment.AreaOpenState;
        foreach (EventPoint eventPoint in eventPointArray)
        {
          if (!Object.op_Equality((Object) eventPoint, (Object) null) && eventPoint.OpenAreaID >= 0)
          {
            int openAreaId = eventPoint.OpenAreaID;
            bool flag2;
            if (!areaOpenState.TryGetValue(openAreaId, out flag2))
              flag2 = false;
            if (isFreeMode)
              flag2 = true;
            if (((Component) eventPoint).get_gameObject().get_activeSelf() == flag2)
              ((Component) eventPoint).get_gameObject().SetActive(!flag2);
          }
        }
      }
    }

    public void RefreshAreaOpenLinkedObject()
    {
      if (AreaOpenLinkedObject.Table.IsNullOrEmpty<int, List<GameObject>>())
        return;
      int num;
      if (Singleton<Game>.IsInstance())
      {
        AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
        bool? nullable1;
        if (environment == null)
        {
          nullable1 = new bool?();
        }
        else
        {
          Dictionary<int, bool> areaOpenState = environment.AreaOpenState;
          nullable1 = areaOpenState != null ? new bool?(areaOpenState.IsNullOrEmpty<int, bool>()) : new bool?();
        }
        bool? nullable2 = nullable1;
        num = !nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0);
      }
      else
        num = 1;
      bool flag1 = num != 0;
      bool isFreeMode = Game.IsFreeMode;
      if (flag1)
      {
        using (IEnumerator<KeyValuePair<int, List<GameObject>>> enumerator1 = AreaOpenLinkedObject.Table.GetEnumerator())
        {
          while (((IEnumerator) enumerator1).MoveNext())
          {
            KeyValuePair<int, List<GameObject>> current1 = enumerator1.Current;
            if (!current1.Value.IsNullOrEmpty<GameObject>())
            {
              using (List<GameObject>.Enumerator enumerator2 = current1.Value.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  GameObject current2 = enumerator2.Current;
                  if (!Object.op_Equality((Object) current2, (Object) null) && current2.get_activeSelf())
                    current2.SetActive(isFreeMode);
                }
              }
            }
          }
        }
      }
      else
      {
        Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment.AreaOpenState;
        using (IEnumerator<KeyValuePair<int, List<GameObject>>> enumerator1 = AreaOpenLinkedObject.Table.GetEnumerator())
        {
          while (((IEnumerator) enumerator1).MoveNext())
          {
            KeyValuePair<int, List<GameObject>> current1 = enumerator1.Current;
            bool flag2;
            if (!current1.Value.IsNullOrEmpty<GameObject>() && areaOpenState.TryGetValue(current1.Key, out flag2))
            {
              using (List<GameObject>.Enumerator enumerator2 = current1.Value.GetEnumerator())
              {
                while (enumerator2.MoveNext())
                {
                  GameObject current2 = enumerator2.Current;
                  if (!Object.op_Equality((Object) current2, (Object) null) && current2.get_activeSelf() != flag2)
                    current2.SetActive(flag2);
                }
              }
            }
          }
        }
      }
    }

    public void RefreshTimeOpenLinkedObject()
    {
      if (TimeOpenLinkedObject.Table.IsNullOrEmpty<int, List<TimeOpenLinkedObject>>())
        return;
      int num;
      if (Singleton<Game>.IsInstance())
      {
        AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
        bool? nullable1;
        if (environment == null)
        {
          nullable1 = new bool?();
        }
        else
        {
          Dictionary<int, bool> timeObjOpenState = environment.TimeObjOpenState;
          nullable1 = timeObjOpenState != null ? new bool?(timeObjOpenState.IsNullOrEmpty<int, bool>()) : new bool?();
        }
        bool? nullable2 = nullable1;
        num = !nullable2.HasValue ? 1 : (nullable2.Value ? 1 : 0);
      }
      else
        num = 1;
      if (num != 0)
      {
        using (IEnumerator<KeyValuePair<int, List<TimeOpenLinkedObject>>> enumerator = TimeOpenLinkedObject.Table.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, List<TimeOpenLinkedObject>> current = enumerator.Current;
            if (!current.Value.IsNullOrEmpty<TimeOpenLinkedObject>())
            {
              foreach (TimeOpenLinkedObject openLinkedObject in current.Value)
              {
                if (!Object.op_Equality((Object) openLinkedObject, (Object) null) && !Object.op_Equality((Object) ((Component) openLinkedObject).get_gameObject(), (Object) null))
                  openLinkedObject.SetActive(!openLinkedObject.EnableFlag);
              }
            }
          }
        }
      }
      else
      {
        Dictionary<int, bool> timeObjOpenState = Singleton<Game>.Instance.Environment.TimeObjOpenState;
        using (IEnumerator<KeyValuePair<int, List<TimeOpenLinkedObject>>> enumerator = TimeOpenLinkedObject.Table.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, List<TimeOpenLinkedObject>> current = enumerator.Current;
            if (!current.Value.IsNullOrEmpty<TimeOpenLinkedObject>())
            {
              bool flag;
              if (!timeObjOpenState.TryGetValue(current.Key, out flag))
                flag = false;
              foreach (TimeOpenLinkedObject openLinkedObject in current.Value)
              {
                if (!Object.op_Equality((Object) openLinkedObject, (Object) null) && !Object.op_Equality((Object) ((Component) openLinkedObject).get_gameObject(), (Object) null))
                  openLinkedObject.SetActive(openLinkedObject.EnableFlag == flag);
              }
            }
          }
        }
      }
    }

    public void SetTimeRelationAreaOpenState(int areaID, bool active)
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Dictionary<int, bool> dictionary1 = Singleton<Game>.Instance.Environment.TimeObjOpenState;
      if (dictionary1 == null)
      {
        Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
        Singleton<Game>.Instance.Environment.TimeObjOpenState = dictionary2;
        dictionary1 = dictionary2;
      }
      bool flag1;
      bool flag2 = !dictionary1.TryGetValue(areaID, out flag1) || flag1 != active;
      if (flag2)
        dictionary1[areaID] = active;
      List<TimeOpenLinkedObject> self;
      if (!TimeOpenLinkedObject.Table.IsNullOrEmpty<int, List<TimeOpenLinkedObject>>() && TimeOpenLinkedObject.Table.TryGetValue(areaID, ref self) && !self.IsNullOrEmpty<TimeOpenLinkedObject>())
      {
        foreach (TimeOpenLinkedObject openLinkedObject in self)
        {
          if (!Object.op_Equality((Object) openLinkedObject, (Object) null) && !Object.op_Equality((Object) ((Component) openLinkedObject).get_gameObject(), (Object) null))
            openLinkedObject.SetActive(openLinkedObject.EnableFlag == active);
        }
      }
      if (!flag2)
        return;
      this.RefreshActiveTimeRelationObjects();
    }

    public bool CanSleepInTime()
    {
      DateTime now = this.Simulator.Now;
      foreach (EnvironmentSimulator.DateTimeThreshold dateTimeThreshold in Singleton<Resources>.Instance.PlayerProfile.CanSleepTime)
      {
        if (dateTimeThreshold.min.Time <= now && dateTimeThreshold.max.Time > now)
          return true;
      }
      return false;
    }

    public bool GetTimeObjOpenState(int id)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, bool> timeObjOpenState = Singleton<Game>.Instance.Environment?.TimeObjOpenState;
      bool flag;
      return timeObjOpenState != null && timeObjOpenState.TryGetValue(id, out flag) && flag;
    }

    public void RefreshTutorialOpenState()
    {
      if (!Singleton<Game>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      bool isFirstGame = Game.IsFirstGame;
      bool isFreeMode = Game.IsFreeMode;
      WorldData worldData = Singleton<Game>.Instance.WorldData;
      if (worldData == null)
        return;
      Dictionary<int, bool> dictionary = worldData.TutorialOpenStateTable ?? (worldData.TutorialOpenStateTable = new Dictionary<int, bool>());
      ReadOnlyDictionary<int, ValueTuple<string, GameObject[]>> tutorialPrefabTable = Singleton<Resources>.Instance.PopupInfo.TutorialPrefabTable;
      if (tutorialPrefabTable.IsNullOrEmpty<int, ValueTuple<string, GameObject[]>>())
        return;
      using (IEnumerator<int> enumerator = tutorialPrefabTable.get_Keys().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          int current = enumerator.Current;
          if (isFreeMode && isFirstGame)
            dictionary[current] = true;
          else if (!dictionary.ContainsKey(current))
            dictionary[current] = isFreeMode;
        }
      }
    }

    public bool CheckAvailableMapArea(int mapAreaID)
    {
      if (this.MapID != 0)
        return true;
      if (!Singleton<Resources>.IsInstance() || !Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, bool> areaOpenState = Singleton<Game>.Instance.Environment?.AreaOpenState;
      int[] numArray;
      if (areaOpenState == null || !Singleton<Resources>.Instance.Map.AreaOpenStateMapAreaLinkerTable.TryGetValue(mapAreaID, out numArray))
        return false;
      if (((IList<int>) numArray).IsNullOrEmpty<int>())
        return true;
      bool flag1 = true;
      foreach (int key in numArray)
      {
        bool flag2;
        if (areaOpenState.TryGetValue(key, out flag2))
        {
          flag1 &= flag2;
        }
        else
        {
          int num = flag1 ? 1 : 0;
          flag1 = false;
        }
      }
      return flag1;
    }

    public void ResettingEnviroSEActive(int mapID, int areaID)
    {
      if (mapID < 0 || areaID < 0 || !Singleton<Resources>.IsInstance())
        return;
      Resources.SoundTable sound = Singleton<Resources>.Instance.Sound;
      if (sound == null || this.EnviroSETable.IsNullOrEmpty<int, ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE>>())
        return;
      Dictionary<int, int[]> dictionary = (Dictionary<int, int[]>) null;
      int[] numArray = (int[]) null;
      if (sound.EnviroSEAdjacentInfoTable.TryGetValue(mapID, out dictionary) && dictionary.TryGetValue(areaID, out numArray) && !((IList<int>) numArray).IsNullOrEmpty<int>())
      {
        List<int> toRelease = ListPool<int>.Get();
        using (Dictionary<int, ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE>>.Enumerator enumerator = this.EnviroSETable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE>> current = enumerator.Current;
            toRelease.Add(current.Key);
          }
        }
        if (toRelease.Contains(areaID))
          toRelease.Remove(areaID);
        ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE> valueTuple1;
        if (this.EnviroSETable.TryGetValue(areaID, out valueTuple1) && Object.op_Inequality((Object) valueTuple1.Item1, (Object) null) && !((GameObject) valueTuple1.Item1).get_activeSelf())
          ((GameObject) valueTuple1.Item1).SetActive(true);
        foreach (int key in numArray)
        {
          ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE> valueTuple2;
          if (this.EnviroSETable.TryGetValue(key, out valueTuple2) && Object.op_Inequality((Object) valueTuple2.Item1, (Object) null) && !((GameObject) valueTuple2.Item1).get_activeSelf())
            ((GameObject) valueTuple2.Item1).SetActive(true);
          if (toRelease.Contains(key))
            toRelease.Remove(key);
        }
        foreach (int key in toRelease)
        {
          ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE> valueTuple2;
          if (this.EnviroSETable.TryGetValue(key, out valueTuple2) && Object.op_Inequality((Object) valueTuple2.Item1, (Object) null) && ((GameObject) valueTuple2.Item1).get_activeSelf())
            ((GameObject) valueTuple2.Item1).SetActive(false);
        }
        ListPool<int>.Release(toRelease);
      }
      else
      {
        using (Dictionary<int, ValueTuple<GameObject, EnvArea3DSE, EnvLineArea3DSE>>.Enumerator enumerator = this.EnviroSETable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject gameObject = (GameObject) enumerator.Current.Value.Item1;
            if (!Object.op_Equality((Object) gameObject, (Object) null) && gameObject.get_activeSelf())
              gameObject.SetActive(false);
          }
        }
      }
    }

    public void ResettingEnviroAreaElement(int mapID, int areaID)
    {
      if (mapID < 0 || areaID < 0 || !Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, int[]>> adjacentInfoTable = Singleton<Resources>.Instance.Sound?.EnviroSEAdjacentInfoTable;
      if (adjacentInfoTable.IsNullOrEmpty<int, Dictionary<int, int[]>>() || this.EnviroRootElement.IsNullOrEmpty<int, Transform>())
        return;
      Dictionary<int, int[]> source = (Dictionary<int, int[]>) null;
      int[] numArray = (int[]) null;
      if (adjacentInfoTable.TryGetValue(mapID, out source) && !source.IsNullOrEmpty<int, int[]>() && (source.TryGetValue(areaID, out numArray) && !((IList<int>) numArray).IsNullOrEmpty<int>()))
      {
        List<int> toRelease1 = ListPool<int>.Get();
        toRelease1.AddRange((IEnumerable<int>) numArray);
        if (!toRelease1.Contains(areaID))
          toRelease1.Add(areaID);
        List<int> toRelease2 = ListPool<int>.Get();
        toRelease2.AddRange((IEnumerable<int>) this.EnviroRootElement.Keys);
        toRelease2.Remove(areaID);
        foreach (int key in toRelease1)
        {
          Transform transform;
          if (this.EnviroRootElement.TryGetValue(key, out transform) && Object.op_Inequality((Object) transform, (Object) null) && !((Component) transform).get_gameObject().get_activeSelf())
            ((Component) transform).get_gameObject().SetActive(true);
          if (toRelease2.Contains(key))
            toRelease2.Remove(key);
        }
        foreach (int key in toRelease2)
        {
          Transform transform;
          if (this.EnviroRootElement.TryGetValue(key, out transform) && Object.op_Inequality((Object) transform, (Object) null) && ((Component) transform).get_gameObject().get_activeSelf())
            ((Component) transform).get_gameObject().SetActive(false);
        }
        ListPool<int>.Release(toRelease1);
        ListPool<int>.Release(toRelease2);
      }
      else
      {
        using (Dictionary<int, Transform>.Enumerator enumerator = this.EnviroRootElement.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<int, Transform> current = enumerator.Current;
            Transform transform = current.Value;
            if (!Object.op_Equality((Object) transform, (Object) null) && !Object.op_Equality((Object) ((Component) transform).get_gameObject(), (Object) null))
            {
              bool flag = areaID == current.Key;
              if (((Component) transform).get_gameObject().get_activeSelf() != flag)
                ((Component) transform).get_gameObject().SetActive(flag);
            }
          }
        }
      }
    }

    public void RefreshHousingEnv3DSEPoints(int mapID, int areaID)
    {
      if (mapID < 0 || areaID < 0 || !Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, int[]>> adjacentInfoTable = Singleton<Resources>.Instance.Sound?.EnviroSEAdjacentInfoTable;
      if (adjacentInfoTable.IsNullOrEmpty<int, Dictionary<int, int[]>>() || this.HousingEnvSEPointTable.IsNullOrEmpty<int, ValueTuple<bool, List<Env3DSEPoint>>>())
        return;
      Dictionary<int, int[]> source = (Dictionary<int, int[]>) null;
      int[] numArray = (int[]) null;
      if (adjacentInfoTable.TryGetValue(mapID, out source) && !source.IsNullOrEmpty<int, int[]>() && (source.TryGetValue(areaID, out numArray) && !((IList<int>) numArray).IsNullOrEmpty<int>()))
      {
        List<int> toRelease1 = ListPool<int>.Get();
        toRelease1.AddRange((IEnumerable<int>) numArray);
        if (!toRelease1.Contains(areaID))
          toRelease1.Add(areaID);
        List<int> toRelease2 = ListPool<int>.Get();
        toRelease2.AddRange((IEnumerable<int>) this.HousingEnvSEPointTable.Keys);
        toRelease2.Remove(areaID);
        foreach (int key in toRelease1)
        {
          ValueTuple<bool, List<Env3DSEPoint>> valueTuple;
          if (this.HousingEnvSEPointTable.TryGetValue(key, out valueTuple))
          {
            valueTuple.Item1 = (__Null) 1;
            if (!((List<Env3DSEPoint>) valueTuple.Item2).IsNullOrEmpty<Env3DSEPoint>())
            {
              foreach (Env3DSEPoint env3DsePoint in (List<Env3DSEPoint>) valueTuple.Item2)
              {
                if (!Object.op_Equality((Object) env3DsePoint, (Object) null))
                  ((Behaviour) env3DsePoint).set_enabled(true);
              }
            }
            this.HousingEnvSEPointTable[key] = valueTuple;
          }
          if (toRelease2.Contains(key))
            toRelease2.Remove(key);
        }
        foreach (int key in toRelease2)
        {
          ValueTuple<bool, List<Env3DSEPoint>> valueTuple;
          if (this.HousingEnvSEPointTable.TryGetValue(key, out valueTuple))
          {
            valueTuple.Item1 = (__Null) 0;
            if (!((List<Env3DSEPoint>) valueTuple.Item2).IsNullOrEmpty<Env3DSEPoint>())
            {
              foreach (Env3DSEPoint env3DsePoint in (List<Env3DSEPoint>) valueTuple.Item2)
              {
                if (!Object.op_Equality((Object) env3DsePoint, (Object) null))
                  ((Behaviour) env3DsePoint).set_enabled(false);
              }
            }
          }
        }
        ListPool<int>.Release(toRelease1);
        ListPool<int>.Release(toRelease2);
      }
      else
      {
        if (this.HousingEnvSEPointTable.IsNullOrEmpty<int, ValueTuple<bool, List<Env3DSEPoint>>>())
          return;
        foreach (int index in ((IEnumerable<int>) this.HousingEnvSEPointTable.Keys).ToList<int>())
        {
          ValueTuple<bool, List<Env3DSEPoint>> valueTuple = this.HousingEnvSEPointTable[index];
          valueTuple.Item1 = (__Null) (index == areaID ? 1 : 0);
          if (!((List<Env3DSEPoint>) valueTuple.Item2).IsNullOrEmpty<Env3DSEPoint>())
          {
            foreach (Env3DSEPoint env3DsePoint in (List<Env3DSEPoint>) valueTuple.Item2)
            {
              if (Object.op_Inequality((Object) env3DsePoint, (Object) null) && (((Behaviour) env3DsePoint).get_enabled() ? 1 : 0) != valueTuple.Item1)
                ((Behaviour) env3DsePoint).set_enabled((bool) valueTuple.Item1);
            }
          }
          this.HousingEnvSEPointTable[index] = valueTuple;
        }
      }
    }

    public static StoryPoint GetStoryPoint(int id)
    {
      if (!Singleton<Map>.IsInstance() || !Object.op_Inequality((Object) Singleton<Map>.Instance._pointAgent, (Object) null))
        return (StoryPoint) null;
      StoryPoint storyPoint = (StoryPoint) null;
      Singleton<Map>.Instance._pointAgent.StoryPointTable.TryGetValue(id, out storyPoint);
      return storyPoint;
    }

    public static bool SetTutorialProgress(int number)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return false;
      int tutorialProgress = environment.TutorialProgress;
      environment.TutorialProgress = Mathf.Max(tutorialProgress, number);
      return tutorialProgress < number;
    }

    public static bool ForcedSetTutorialProgress(int number)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return false;
      environment.TutorialProgress = number;
      return true;
    }

    public static bool ForcedSetTutorialProgressAndUIUpdate(int number)
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return false;
      environment.TutorialProgress = number;
      MapUIContainer.StorySupportUI.Open(number);
      return true;
    }

    public static void SetTutorialProgressAndUIUpdate(int number)
    {
      if (!Map.SetTutorialProgress(number))
        return;
      MapUIContainer.OpenStorySupportUI(number);
    }

    public static void RefreshStoryUI()
    {
      if (!Singleton<Game>.IsInstance())
        return;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      MapUIContainer.OpenStorySupportUI(environment.TutorialProgress);
    }

    public void CheckTutorialState(PlayerActor player)
    {
      if (Object.op_Equality((Object) player, (Object) null) || !Singleton<Resources>.IsInstance())
        return;
      Resources instance = Singleton<Resources>.Instance;
      if (Map.TutorialMode)
      {
        if (this.ChangeTutorialUI(player))
          return;
        switch (Map.GetTutorialProgress())
        {
          case 3:
            ItemIDKeyPair driftwoodID = instance.CommonDefine.ItemIDDefine.DriftwoodID;
            if (player.PlayerData.ItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == driftwoodID.categoryID && x.ID == driftwoodID.itemID && 0 < x.Count)))
            {
              Singleton<Map>.Instance.DestroyTutorialSearchPoint();
              Map.SetTutorialProgress(4);
              player.AddTutorialUI(Popup.Tutorial.Type.Craft, false);
              break;
            }
            break;
          case 4:
            FishingDefinePack.ItemIDPair fishingRodID = instance.FishingDefinePack.IDInfo.FishingRod;
            if (player.PlayerData.ItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == fishingRodID.CategoryID && x.ID == fishingRodID.ItemID && 0 < x.Count)))
            {
              Map.SetTutorialProgress(5);
              player.AddTutorialUI(Popup.Tutorial.Type.Equipment, false);
              break;
            }
            break;
          case 5:
            if (player.PlayerData.EquipedFishingItem.ID == instance.CommonDefine.ItemIDDefine.RodID.itemID)
            {
              Map.SetTutorialProgress(6);
              break;
            }
            break;
          case 6:
            List<FishingDefinePack.ItemIDPair> fishList = instance.FishingDefinePack.IDInfo.FishList;
            bool flag1 = false;
            if (!fishList.IsNullOrEmpty<FishingDefinePack.ItemIDPair>())
            {
              foreach (FishingDefinePack.ItemIDPair itemIdPair in fishList)
              {
                FishingDefinePack.ItemIDPair data = itemIdPair;
                if (player.PlayerData.ItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == data.CategoryID && x.ID == data.ItemID && 0 < x.Count)))
                {
                  flag1 = true;
                  break;
                }
              }
              if (flag1)
              {
                Map.SetTutorialProgress(7);
                break;
              }
              break;
            }
            break;
          case 11:
            bool flag2 = false;
            FishingDefinePack fishingDefinePack = instance?.FishingDefinePack;
            List<StuffItem> itemList = player?.PlayerData?.ItemList;
            if (Object.op_Inequality((Object) fishingDefinePack, (Object) null) && itemList != null)
            {
              FishingDefinePack.ItemIDPair idinfo = fishingDefinePack.IDInfo.GrilledFish;
              flag2 = itemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == idinfo.CategoryID && x.ID == idinfo.ItemID && 0 < x.Count));
              if (flag2)
                Map.SetTutorialProgress(13);
            }
            if (!flag2)
            {
              CommonDefine commonDefine = !Singleton<Resources>.IsInstance() ? (CommonDefine) null : Singleton<Resources>.Instance.CommonDefine;
              if (Object.op_Inequality((Object) commonDefine, (Object) null) && Object.op_Inequality((Object) this._pointAgent, (Object) null) && !((IList<int>) commonDefine.Tutorial.KitchenPointIDList).IsNullOrEmpty<int>())
              {
                foreach (int kitchenPointId in commonDefine.Tutorial.KitchenPointIDList)
                {
                  int kitchenID = kitchenPointId;
                  if (this._pointAgent.AppendActionPoints.Exists((Predicate<ActionPoint>) (x => x.ID == kitchenID)))
                  {
                    Map.SetTutorialProgress(12);
                    break;
                  }
                }
                break;
              }
              break;
            }
            break;
          case 12:
            FishingDefinePack.ItemIDPair idinfo1 = instance.FishingDefinePack.IDInfo.GrilledFish;
            if (player.PlayerData.ItemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == idinfo1.CategoryID && x.ID == idinfo1.ItemID && 0 < x.Count)))
            {
              Map.SetTutorialProgress(13);
              break;
            }
            break;
        }
      }
      if (this.ChangeTutorialUI(player) || this.MapID != 0 || Map.GetTutorialProgress() == MapUIContainer.StorySupportUI.Index)
        return;
      bool flag3 = Singleton<MapScene>.IsInstance() && Singleton<MapScene>.Instance.IsLoading;
      bool flag4 = !flag3 && Singleton<Manager.Scene>.IsInstance() && Singleton<Manager.Scene>.Instance.IsNowLoadingFade;
      if (flag3)
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.SkipWhile<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) Singleton<MapScene>.Instance).get_gameObject()), ((Component) player).get_gameObject()), (Func<M0, bool>) (_ => Singleton<MapScene>.IsInstance() && Singleton<MapScene>.Instance.IsLoading)), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending() || player.Animation.PlayingInLocoAnimation)), 1), 15, (FrameCountType) 0), (System.Action<M0>) (_ => Map.RefreshStoryUI()));
      else if (flag4)
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.SkipWhile<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) Singleton<MapScene>.Instance).get_gameObject()), ((Component) player).get_gameObject()), (Func<M0, bool>) (_ => Singleton<Manager.Scene>.IsInstance() && Singleton<Manager.Scene>.Instance.IsNowLoadingFade)), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending() || player.Animation.PlayingInLocoAnimation)), 1), 15, (FrameCountType) 0), (System.Action<M0>) (_ => Map.RefreshStoryUI()));
      else
        ObservableExtensions.Subscribe<long>(Observable.DelayFrame<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) player), 1), (Func<M0, bool>) (_ => player.CameraControl.CinemachineBrain.get_IsBlending() || player.Animation.PlayingInLocoAnimation)), 1), 15, (FrameCountType) 0), (System.Action<M0>) (_ => Map.RefreshStoryUI()));
    }

    private bool ChangeTutorialUI(PlayerActor player)
    {
      float delayTime = !Singleton<Resources>.IsInstance() ? 1f : Singleton<Resources>.Instance.CommonDefine.Tutorial.UIDisplayDelayTime;
      if (player.TutorialIndexList.IsNullOrEmpty<ValueTuple<Popup.Tutorial.Type, bool>>())
        return false;
      ValueTuple<Popup.Tutorial.Type, bool> valueTuple = player.TutorialIndexList.Pop<ValueTuple<Popup.Tutorial.Type, bool>>();
      MapUIContainer.TutorialUI.SetCondition((Popup.Tutorial.Type) valueTuple.Item1, (bool) valueTuple.Item2);
      EventPoint.SetCurrentPlayerStateName();
      player.PlayerController.ChangeState("Idle");
      bool flag1 = Singleton<MapScene>.IsInstance() && Singleton<MapScene>.Instance.IsLoading;
      bool flag2 = !flag1 && Singleton<Manager.Scene>.IsInstance() && Singleton<Manager.Scene>.Instance.IsNowLoadingFade;
      if (flag1)
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) Singleton<MapScene>.Instance).get_gameObject()), ((Component) player).get_gameObject()), (Func<M0, bool>) (_ => Singleton<MapScene>.IsInstance() && Singleton<MapScene>.Instance.IsLoading)), 1), (System.Action<M0>) (_ => this.DelayChangeTutorialUI(player, delayTime)));
      else if (flag2)
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) Singleton<Manager.Scene>.Instance), ((Component) player).get_gameObject()), (Func<M0, bool>) (_ => Singleton<Manager.Scene>.IsInstance() && Singleton<Manager.Scene>.Instance.IsNowLoadingFade)), 1), (System.Action<M0>) (_ => this.DelayChangeTutorialUI(player, delayTime)));
      else
        this.DelayChangeTutorialUI(player, delayTime);
      return true;
    }

    private void DelayChangeTutorialUI(PlayerActor player, float delayTime)
    {
      if (Map.GetTutorialProgress() != MapUIContainer.StorySupportUI.Index)
      {
        MapUIContainer.StorySupportUI.OpenedAction = (System.Action) (() =>
        {
          ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) delayTime)), ((Component) player).get_gameObject()), (Func<M0, bool>) (_ => Singleton<Map>.IsInstance())), (System.Action<M0>) (_ => player.PlayerController.ChangeState("Tutorial")));
          MapUIContainer.StorySupportUI.OpenedAction = (System.Action) null;
        });
        ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) player).get_gameObject()), 1), (Func<M0, bool>) (_ => player.Animation.PlayingInLocoAnimation || player.CameraControl.CinemachineBrain.get_IsBlending())), 1), (System.Action<M0>) (_ => Map.RefreshStoryUI()));
      }
      else
        ObservableExtensions.Subscribe<long>(Observable.Delay<long>(Observable.Take<long>(Observable.SkipWhile<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryUpdate(), 1), (Func<M0, bool>) (_ => player.Animation.PlayingInLocoAnimation || player.CameraControl.CinemachineBrain.get_IsBlending())), 1), TimeSpan.FromSeconds((double) delayTime)), (System.Action<M0>) (_ => player.PlayerController.ChangeState("Tutorial")));
    }

    public void CheckStoryProgress()
    {
      if (this.MapID != 0)
        return;
      Popup.StorySupport.Type tutorialProgress1 = (Popup.StorySupport.Type) Map.GetTutorialProgress();
      switch (tutorialProgress1)
      {
        case Popup.StorySupport.Type.GrowGirls1:
          EventPoint eventPoint1 = EventPoint.Get(1, 0);
          if (Object.op_Inequality((Object) eventPoint1, (Object) null) && eventPoint1.LabelIndex == 1)
          {
            Map.ForcedSetTutorialProgressAndUIUpdate(17);
            break;
          }
          break;
        case Popup.StorySupport.Type.GrowGirls2:
          EventPoint eventPoint2 = EventPoint.Get(1, 1);
          if (Object.op_Inequality((Object) eventPoint2, (Object) null) && eventPoint2.LabelIndex == 1)
          {
            Map.ForcedSetTutorialProgressAndUIUpdate(20);
            break;
          }
          break;
        case Popup.StorySupport.Type.GrowGirls3:
        case Popup.StorySupport.Type.ExamineStoryPoint3:
        case Popup.StorySupport.Type.RepairGenerator:
        case Popup.StorySupport.Type.ExamineStoryPoint4:
          EventPoint eventPoint3 = EventPoint.Get(1, 2);
          if (Object.op_Inequality((Object) eventPoint3, (Object) null))
          {
            switch (eventPoint3.LabelIndex)
            {
              case 0:
                if (tutorialProgress1 != Popup.StorySupport.Type.GrowGirls3)
                {
                  Map.ForcedSetTutorialProgressAndUIUpdate(22);
                  break;
                }
                break;
              case 1:
                if (tutorialProgress1 != Popup.StorySupport.Type.ExamineStoryPoint3)
                {
                  Map.ForcedSetTutorialProgressAndUIUpdate(23);
                  break;
                }
                break;
              case 2:
                if (tutorialProgress1 != Popup.StorySupport.Type.RepairGenerator)
                {
                  Map.ForcedSetTutorialProgressAndUIUpdate(24);
                  break;
                }
                break;
              case 3:
                if (tutorialProgress1 != Popup.StorySupport.Type.ExamineStoryPoint4)
                {
                  Map.ForcedSetTutorialProgressAndUIUpdate(25);
                  break;
                }
                break;
            }
          }
          else
            break;
          break;
      }
      bool flag = false;
      if (Object.op_Inequality((Object) this.Player, (Object) null))
        flag = this.Player.Mode == Desire.ActionType.Date;
      Popup.StorySupport.Type tutorialProgress2 = (Popup.StorySupport.Type) Map.GetTutorialProgress();
      if (!flag)
      {
        switch (tutorialProgress2)
        {
          case Popup.StorySupport.Type.GrowGirls1:
          case Popup.StorySupport.Type.ExamineStoryPoint1:
            EventPoint.SetTargetID(1, 0);
            break;
          case Popup.StorySupport.Type.ExamineNextStoryPoint1:
          case Popup.StorySupport.Type.GrowGirls2:
          case Popup.StorySupport.Type.ExamineStoryPoint2:
            EventPoint.SetTargetID(1, 1);
            break;
          case Popup.StorySupport.Type.ExamineNextStoryPoint2:
          case Popup.StorySupport.Type.GrowGirls3:
          case Popup.StorySupport.Type.ExamineStoryPoint3:
          case Popup.StorySupport.Type.ExamineStoryPoint4:
            EventPoint.SetTargetID(1, 2);
            break;
          case Popup.StorySupport.Type.RepairGenerator:
            EventPoint.SetTargetID(1, 3);
            break;
          case Popup.StorySupport.Type.ExamineNextStoryPoint3:
          case Popup.StorySupport.Type.RepairShip:
            EventPoint.SetTargetID(1, 6);
            break;
          default:
            EventPoint.SetTargetID(-1, -1);
            break;
        }
      }
      else if (tutorialProgress2 == Popup.StorySupport.Type.ExamineStoryPoint4)
        EventPoint.SetTargetID(1, 2);
      else
        EventPoint.SetTargetID(-1, -1);
    }

    public static int GetTutorialProgress()
    {
      if (!Singleton<Game>.IsInstance())
        return -1;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      return environment != null ? environment.TutorialProgress : -1;
    }

    public static int GetAgentTotalFlavorAdditionAmount()
    {
      if (!Singleton<Map>.IsInstance() || Singleton<Map>.Instance.AgentTable.IsNullOrEmpty<int, AgentActor>())
        return 0;
      int num = 0;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Map>.Instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (!Object.op_Equality((Object) agentActor, (Object) null))
          {
            AgentData agentData = agentActor.AgentData;
            if (agentData != null && agentData.OpenState)
              num += agentData.FlavorAdditionAmount;
          }
        }
      }
      return num;
    }

    public static int GetTotalAgentFlavorAdditionAmount()
    {
      if (!Singleton<Map>.IsInstance() || !Singleton<Game>.IsInstance())
        return 0;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      return environment == null ? 0 : environment.TotalAgentFlavorAdditionAmount;
    }

    [DebuggerHidden]
    public IEnumerator SetupPoint()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CSetupPoint\u003Ec__IteratorF()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator ApplyProfile(WorldData profile, bool existsBackup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CApplyProfile\u003Ec__Iterator10()
      {
        profile = profile,
        existsBackup = existsBackup,
        \u0024this = this
      };
    }

    private void LoadPlayer(PlayerData playerInfo)
    {
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      string bundlePath = definePack.ABPaths.ActorPrefab;
      string manifest = definePack.ABManifests.Default;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>(bundlePath, "Player", false, manifest);
      if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundlePath && (string) x.Item2 == manifest)))
        MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundlePath, manifest));
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
      UnityExtensions.Initiate(gameObject2.get_transform(), this.ActorRoot);
      PlayerActor player = (PlayerActor) gameObject2.GetComponent<PlayerActor>();
      player.ID = -99;
      Vector3 pos = playerInfo.Position;
      ObservableExtensions.Subscribe<long>(Observable.TakeWhile<long>(Observable.Where<long>((IObservable<M0>) Observable.EveryUpdate(), (Func<M0, bool>) (_ => ((Behaviour) player.NavMeshAgent).get_enabled())), (Func<M0, bool>) (_ => !player.NavMeshAgent.get_isOnNavMesh())), (System.Action<M0>) (_ => player.NavMeshAgent.Warp(pos)));
      player.Rotation = playerInfo.Rotation;
      player.PlayerData = playerInfo;
      if (((IList<string>) player.PlayerData.CharaFileNames).IsNullOrEmpty<string>())
      {
        player.PlayerData.CharaFileNames[0] = "charaF_20170613163526688";
        player.PlayerData.CharaFileNames[1] = "charaF_20170613163526688";
      }
      player.Relocate();
      UnityExtensions.SetActiveSafe(gameObject2, true);
      this.RegisterPlayer(player);
      this.RegisterActor(-99, (Actor) player);
    }

    [DebuggerHidden]
    private IEnumerator LoadPlayerAsync(PlayerData playerInfo, bool existsBackup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadPlayerAsync\u003Ec__Iterator11()
      {
        existsBackup = existsBackup,
        playerInfo = playerInfo,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator LoadAgents(WorldData profile, bool existsBackup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadAgents\u003Ec__Iterator12()
      {
        profile = profile,
        existsBackup = existsBackup,
        \u0024this = this
      };
    }

    private bool FirstAgentPositionSetting(AgentActor agent)
    {
      StoryPoint storyPoint = Map.GetStoryPoint(3);
      if (Object.op_Equality((Object) storyPoint, (Object) null) || Object.op_Equality((Object) agent, (Object) null))
        return false;
      if (((Behaviour) agent.NavMeshAgent).get_isActiveAndEnabled())
        agent.NavMeshAgent.Warp(storyPoint.Position);
      else
        agent.Position = storyPoint.Position;
      agent.Rotation = storyPoint.Rotation;
      agent.MapArea = storyPoint.OwnerArea;
      return true;
    }

    private bool FirstAgentDataPositionSetting(AgentData data)
    {
      if (data == null)
        return false;
      StoryPoint storyPoint = Map.GetStoryPoint(3);
      if (Object.op_Equality((Object) storyPoint, (Object) null))
        return false;
      data.Position = storyPoint.Position;
      data.Rotation = storyPoint.Rotation;
      return true;
    }

    private void AgentPositionSetting(AgentActor agent)
    {
      Chunk chunk;
      this.ChunkTable.TryGetValue(0, out chunk);
      List<Waypoint> waypoints = chunk.MapAreas[0].Waypoints;
      List<Waypoint> all = waypoints.FindAll((Predicate<Waypoint>) (x =>
      {
        Vector3 position = this.Player.Position;
        return (double) Vector3.Distance(((Component) x).get_transform().get_position(), position) > 50.0;
      }));
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("近距離で除外されたウェイポイントの数 = {0}", (object) (waypoints.Count - all.Count)));
      Waypoint element = all.GetElement<Waypoint>(Random.Range(0, all.Count));
      agent.Position = ((Component) element).get_transform().get_position();
      agent.MapArea = chunk.MapAreas[0];
    }

    public void LoadAgentTargetActionPoint()
    {
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
      {
        KeyValuePair<int, AgentActor> agent = keyValuePair;
        ActionPoint actionPoint = ((IEnumerable<ActionPoint>) this.PointAgent.ActionPoints).FirstOrDefault<ActionPoint>((Func<ActionPoint, bool>) (x => x.RegisterID == agent.Value.AgentData.CurrentActionPointID));
        if (Object.op_Equality((Object) actionPoint, (Object) null))
          actionPoint = this.PointAgent.AppendActionPoints.Find((Predicate<ActionPoint>) (x => x.RegisterID == agent.Value.AgentData.CurrentActionPointID));
        if (Object.op_Inequality((Object) actionPoint, (Object) null) && !actionPoint.AgentEventType.Contains(AIProject.EventType.Move) && !actionPoint.AgentEventType.Contains(AIProject.EventType.DoorOpen))
          agent.Value.TargetInSightActionPoint = actionPoint;
      }
    }

    public void LoadAgentTargetActor()
    {
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
      {
        Actor actor;
        if (this._actorTable.TryGetValue(keyValuePair.Value.AgentData.ActionTargetID, out actor))
          keyValuePair.Value.TargetInSightActor = actor;
      }
    }

    public void RemovePlayer(PlayerActor player)
    {
      this.UnregisterActor(player.ID);
    }

    public AgentActor AddAgent(int id, AgentData agentData)
    {
      if (Object.op_Equality((Object) this._agentPrefab, (Object) null))
      {
        DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
        string bundlePath = definePack.ABPaths.ActorPrefab;
        string manifest = definePack.ABManifests.Default;
        this._agentPrefab = CommonLib.LoadAsset<GameObject>(bundlePath, "Agent", false, manifest);
        if (!MapScene.AssetBundlePaths.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == bundlePath && (string) x.Item2 == manifest)))
          MapScene.AssetBundlePaths.Add(new ValueTuple<string, string>(bundlePath, manifest));
      }
      this.ChunkTable.Keys.ToArray<int>();
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this._agentPrefab, this.ActorRoot);
      ((Object) gameObject).set_name(string.Format("Heroine_{0}", (object) id.ToString("00")));
      AgentActor component = (AgentActor) gameObject.GetComponent<AgentActor>();
      component.ID = id;
      component.AgentData = agentData;
      int num1;
      if (Singleton<Resources>.Instance.AgentProfile.DefaultAreaIDTable.TryGetValue(id, out num1))
      {
        AgentActor agentActor = component;
        int num2 = num1;
        agentData.AreaID = num2;
        int num3 = num2;
        agentActor.AreaID = num3;
        foreach (KeyValuePair<int, Chunk> keyValuePair in this.ChunkTable)
        {
          foreach (MapArea mapArea in keyValuePair.Value.MapAreas)
          {
            if (mapArea.AreaID == num1)
              component.MapArea = mapArea;
          }
        }
      }
      agentData.param.Bind((Actor) component);
      this.RegisterActor(id, (Actor) component);
      this.RegisterAgent(id, component);
      if (Singleton<AnimalManager>.IsInstance())
        Singleton<AnimalManager>.Instance.AddTargetAnimals(component);
      component.Relocate();
      UnityExtensions.SetActiveSafe(gameObject, true);
      component.Load();
      return component;
    }

    public void RemoveAgent(AgentActor agent)
    {
      agent.DisableBehavior();
      agent.BehaviorResources.DisableAllBehaviors();
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, FadePlayer>>>.Enumerator enumerator1 = agent.Animation.ActionLoopSETable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          using (Dictionary<int, ValueTuple<AudioSource, FadePlayer>>.Enumerator enumerator2 = enumerator1.Current.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              KeyValuePair<int, ValueTuple<AudioSource, FadePlayer>> current = enumerator2.Current;
              if (!Object.op_Equality((Object) current.Value.Item2, (Object) null))
                ((FadePlayer) current.Value.Item2).Stop(0.0f);
            }
          }
        }
      }
      agent.Animation.ActionLoopSETable.Clear();
      this.Player.PlayerController.CommandArea.RemoveCommandableObject((ICommandable) agent);
      this.UnregisterAgent(agent.ID);
      this.UnregisterActor(agent.ID);
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) agent))
            current.Value.RemoveActor((Actor) agent);
        }
      }
      Singleton<Character>.Instance.DeleteChara(agent.ChaControl, false);
      Object.Destroy((Object) ((Component) agent).get_gameObject());
    }

    [DebuggerHidden]
    private IEnumerator LoadMerchantAsync(MerchantData merchantData, bool existsBackup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadMerchantAsync\u003Ec__Iterator13()
      {
        merchantData = merchantData,
        existsBackup = existsBackup,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator LoadAnimals(WorldData profile, bool existsBackup)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CLoadAnimals\u003Ec__Iterator14()
      {
        profile = profile,
        \u0024this = this
      };
    }

    private void AddTutorialItems()
    {
      List<StuffItem> itemList = this.Player.PlayerData.ItemList;
      FishingDefinePack.ItemIDPair fishingRodID = Singleton<Resources>.Instance.FishingDefinePack.IDInfo.FishingRod;
      if (itemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == fishingRodID.CategoryID && x.ID == fishingRodID.ItemID && 0 < x.Count)))
        return;
      itemList.AddItem(new StuffItem(fishingRodID.CategoryID, fishingRodID.ItemID, 1));
    }

    public void CreateTutorialLockArea()
    {
      string mapScenePrefab = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
      string assetName = "p_ai_mi_tutorialmesh00";
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) CommonLib.LoadAsset<GameObject>(mapScenePrefab, assetName, false, string.Empty));
      gameObject.get_transform().SetParent(this._mapRoot, false);
      gameObject.get_transform().set_localPosition(Vector3.get_zero());
      gameObject.get_transform().set_localRotation(Quaternion.get_identity());
      gameObject.get_transform().set_localScale(Vector3.get_one());
      this.TutorialLockAreaObject = gameObject;
      MapScene.AddAssetBundlePath(mapScenePrefab, string.Empty);
    }

    public void DestroyTutorialLockArea()
    {
      if (!Object.op_Inequality((Object) this.TutorialLockAreaObject, (Object) null))
        return;
      Object.Destroy((Object) this.TutorialLockAreaObject);
      this.TutorialLockAreaObject = (GameObject) null;
    }

    public GameObject TutorialLockAreaObject { get; private set; }

    public void CreateTutorialSearchPoint()
    {
      if (Object.op_Inequality((Object) this._tutorialSearchPointObject, (Object) null))
        return;
      string mapScenePrefab = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
      string assetName = "TutorialSearchPoint_00";
      string str = Singleton<Resources>.Instance.DefinePack.ABManifests.Default;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>(mapScenePrefab, assetName, false, str);
      if (Object.op_Equality((Object) gameObject1, (Object) null))
        return;
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
      gameObject2.get_transform().SetParent(this._mapRoot, false);
      this._tutorialSearchPointObject = gameObject2;
      MapScene.AddAssetBundlePath(mapScenePrefab, str);
    }

    public void DestroyTutorialSearchPoint()
    {
      if (!Object.op_Inequality((Object) this._tutorialSearchPointObject, (Object) null))
        return;
      Object.Destroy((Object) this._tutorialSearchPointObject);
      this._tutorialSearchPointObject = (GameObject) null;
    }

    public void CreateStoryPointEffect()
    {
      if (Object.op_Inequality((Object) this.StoryPointEffect, (Object) null))
        return;
      string mapScenePrefab = Singleton<Resources>.Instance.DefinePack.ABPaths.MapScenePrefab;
      string assetName = "StoryPointEffect";
      string str = Singleton<Resources>.Instance.DefinePack.ABManifests.Default;
      GameObject gameObject1 = CommonLib.LoadAsset<GameObject>(mapScenePrefab, assetName, false, str);
      if (Object.op_Equality((Object) gameObject1, (Object) null))
        return;
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) gameObject1);
      gameObject2.get_transform().SetParent(((Component) this._pointAgent).get_transform(), false);
      this.StoryPointEffect = (StoryPointEffect) gameObject2.GetComponent<StoryPointEffect>();
      if (Object.op_Equality((Object) this.StoryPointEffect, (Object) null))
        Object.Destroy((Object) gameObject2);
      else
        StoryPointEffect.Switch = true;
      MapScene.AddAssetBundlePath(mapScenePrefab, str);
    }

    public void DestroyStoryPointEffect()
    {
      if (Object.op_Equality((Object) this.StoryPointEffect, (Object) null))
        return;
      Object.Destroy((Object) ((Component) this.StoryPointEffect).get_gameObject());
      this.StoryPointEffect = (StoryPointEffect) null;
    }

    public StoryPointEffect StoryPointEffect { get; private set; }

    public void RegisterPlayer(PlayerActor player)
    {
      if (Object.op_Equality((Object) this.Player, (Object) player))
        return;
      this.Player = player;
    }

    public void RegisterActor(int id, Actor actor)
    {
      if (!this._actorTable.TryGetValue(id, out Actor _))
        ;
      this._actorTable[id] = actor;
      if (this.Actors.Contains(actor))
        return;
      this.Actors.Add(actor);
    }

    public void UnregisterActor(int id)
    {
      if (!this._actorTable.ContainsKey(id))
        return;
      this._actorTable.Remove(id);
    }

    public void RegisterAgent(int id, AgentActor agent)
    {
      if (!this._agentTable.TryGetValue(id, out AgentActor _))
        ;
      this._agentTable[id] = agent;
      if (this._agentKeys.Contains(id))
        return;
      this._agentKeys.Add(id);
    }

    public void UnregisterAgent(int id)
    {
      if (this._agentKeys.Contains(id))
        this._agentKeys.Remove(id);
      if (!this._agentTable.ContainsKey(id))
        return;
      this._agentTable.Remove(id);
    }

    public void RegisterMerchant(MerchantActor merchant)
    {
      if (Object.op_Equality((Object) this.Merchant, (Object) merchant))
        return;
      this.Merchant = merchant;
    }

    public void ApplyConfig(System.Action onCompleteIn, System.Action onCompleteOut)
    {
      if (Map.TutorialMode)
      {
        System.Action action = onCompleteOut;
        if (action == null)
          return;
        action();
      }
      else
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
        {
          System.Action action1 = onCompleteIn;
          if (action1 != null)
            action1();
          bool[] charasEntry = Config.GraphicData.CharasEntry;
          Dictionary<int, AgentData> agentTable1 = Singleton<Game>.Instance.WorldData.AgentTable;
          ReadOnlyDictionary<int, AgentActor> agentTable2 = Singleton<Map>.Instance.AgentTable;
          ChaFileControl chaFileControl = new ChaFileControl();
          foreach (KeyValuePair<int, AgentData> keyValuePair in Singleton<Game>.Instance.WorldData.AgentTable)
          {
            string charaFileName = keyValuePair.Value.CharaFileName;
            if (charasEntry[keyValuePair.Key] && !charaFileName.IsNullOrEmpty() && (chaFileControl.LoadCharaFile(charaFileName, (byte) 1, false, true) && keyValuePair.Value.MapID == this.MapID))
            {
              if (!agentTable2.ContainsKey(keyValuePair.Key))
              {
                AgentActor agent = Singleton<Map>.Instance.AddAgent(keyValuePair.Key, keyValuePair.Value);
                agent.RefreshWalkStatus(this.PointAgent.Waypoints);
                Singleton<Map>.Instance.InitSearchActorTargets(agent);
                this.Player.PlayerController.CommandArea.AddCommandableObject((ICommandable) agent);
                using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Map>.Instance.AgentTable.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    KeyValuePair<int, AgentActor> current = enumerator.Current;
                    if (!Object.op_Equality((Object) current.Value, (Object) agent))
                      current.Value.AddActor((Actor) agent);
                  }
                }
                agent.ActivateNavMeshAgent();
                Transform transform = this.MapID != 0 ? ((Component) this.PointAgent.DevicePointDic[0].RecoverPoints[keyValuePair.Key]).get_transform() : ((Component) this.PointAgent.DevicePointDic[keyValuePair.Key].RecoverPoints[0]).get_transform();
                agent.NavMeshAgent.Warp(transform.get_position());
                agent.Rotation = transform.get_rotation();
                agent.EnableBehavior();
                agent.ChangeBehavior(Desire.ActionType.Normal);
              }
            }
            else
            {
              AgentActor agent;
              if (agentTable2.TryGetValue(keyValuePair.Key, ref agent))
              {
                agent.DisableBehavior();
                agent.ClearItems();
                agent.ClearParticles();
                Actor.BehaviorSchedule schedule = agent.Schedule;
                schedule.enabled = false;
                agent.Schedule = schedule;
                agent.TargetInSightActor = (Actor) null;
                if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
                {
                  agent.CurrentPoint.SetActiveMapItemObjs(true);
                  agent.CurrentPoint.ReleaseSlot((Actor) agent);
                  agent.CurrentPoint = (ActionPoint) null;
                }
                agent.TargetInSightActionPoint = (ActionPoint) null;
                if (Object.op_Inequality((Object) agent.Partner, (Object) null))
                {
                  if (agent.Partner is PlayerActor)
                  {
                    PlayerActor partner = agent.Partner as PlayerActor;
                    agent.ActivateHoldingHands(0, false);
                    if (partner.PlayerController.State is Normal || partner.PlayerController.State is Houchi || partner.PlayerController.State is Onbu)
                    {
                      partner.PlayerController.ChangeState("Normal");
                      partner.Mode = Desire.ActionType.Normal;
                      partner.Partner = (Actor) null;
                      partner.ActivateTransfer();
                    }
                    else if (partner.PlayerController.State is Menu)
                    {
                      partner.PlayerController.ChangeState("Menu");
                      partner.PlayerController.PrevStateName = "Normal";
                      partner.Mode = Desire.ActionType.Normal;
                      partner.Partner = (Actor) null;
                      partner.ActivateTransfer();
                    }
                  }
                  else if (agent.Partner is AgentActor)
                  {
                    AgentActor partner = agent.Partner as AgentActor;
                    agent.StopLesbianSequence();
                    partner.StopLesbianSequence();
                    partner.Animation.EndIgnoreEvent();
                    Singleton<Game>.Instance.GetExpression(partner.ChaControl.fileParam.personality, "標準")?.Change(partner.ChaControl);
                    partner.Animation.ResetDefaultAnimatorController();
                    partner.ChangeBehavior(Desire.ActionType.Normal);
                  }
                  else if (agent.Partner is MerchantActor)
                  {
                    MerchantActor partner = agent.Partner as MerchantActor;
                    agent.StopLesbianSequence();
                    partner.ResetState();
                    partner.ChangeBehavior(partner.LastNormalMode);
                  }
                  agent.Partner = (Actor) null;
                }
                if (Object.op_Inequality((Object) agent.CommandPartner, (Object) null))
                {
                  if (agent.CommandPartner is AgentActor)
                    (agent.CommandPartner as AgentActor).ChangeBehavior(Desire.ActionType.Normal);
                  else if (agent.CommandPartner is MerchantActor)
                  {
                    MerchantActor commandPartner = agent.CommandPartner as MerchantActor;
                    commandPartner.ChangeBehavior(commandPartner.LastNormalMode);
                  }
                  agent.CommandPartner = (Actor) null;
                }
                agent.ChaControl.chaFile.SaveCharaFile(agent.ChaControl.chaFile.charaFileName, byte.MaxValue, false);
                this.RemoveAgent(agent);
              }
            }
          }
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, true), (System.Action<M0>) (__ => {}), (System.Action) (() =>
          {
            System.Action action2 = onCompleteOut;
            if (action2 == null)
              return;
            action2();
          }));
        }));
    }

    public void ReleaseComponents()
    {
      if (Object.op_Inequality((Object) this.Player, (Object) null))
      {
        Object.Destroy((Object) ((Component) this.Player).get_gameObject());
        this.Player = (PlayerActor) null;
      }
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
      {
        if (!Object.op_Equality((Object) keyValuePair.Value, (Object) null))
          Object.Destroy((Object) ((Component) keyValuePair.Value).get_gameObject());
      }
      this._agentTable.Clear();
      if (Object.op_Inequality((Object) this.Merchant, (Object) null))
      {
        Object.Destroy((Object) ((Component) this.Merchant).get_gameObject());
        this.Merchant = (MerchantActor) null;
      }
      this._actorTable.Clear();
      this._agentKeys.Clear();
      if (Singleton<Character>.IsInstance())
      {
        Singleton<Character>.Instance.DeleteCharaAll();
        Singleton<Character>.Instance.EndLoadAssetBundle(false);
      }
      if (Singleton<Housing>.IsInstance())
        Singleton<Housing>.Instance.Release();
      this.HousingPointTable.Clear();
      this.ChunkTable.Clear();
    }

    private void ReleaseAgents()
    {
      foreach (int key in this._agentKeys.ToArray())
      {
        AgentActor agent;
        if (this._agentTable.TryGetValue(key, out agent))
          this.RemoveAgent(agent);
      }
    }

    private void ReleaseMap()
    {
      Singleton<Housing>.Instance.DeleteRoot();
      Singleton<Housing>.Instance.Release();
      this.HousingPointTable.Clear();
      if (Object.op_Inequality((Object) this.NavMeshSrc, (Object) null))
      {
        Object.Destroy((Object) this.NavMeshSrc.get_gameObject());
        this.NavMeshSrc = (GameObject) null;
      }
      if (Object.op_Inequality((Object) this.ChunkSrc, (Object) null))
      {
        Object.Destroy((Object) this.ChunkSrc.get_gameObject());
        this.ChunkSrc = (GameObject) null;
      }
      this._pointAgent.Release();
    }

    public void InitSearchActorTargetsAll()
    {
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          this.InitSearchActorTargets(enumerator.Current.Value);
      }
    }

    public void InitSearchActorTargets(AgentActor agent)
    {
      agent.AddActor((Actor) this.Player);
      agent.AddActor((Actor) this.Merchant);
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) agent))
            agent.AddActor((Actor) current.Value);
        }
      }
    }

    private void AreaTypeUpdate(Actor actor)
    {
      if (Object.op_Equality((Object) actor, (Object) null))
        return;
      LayerMask roofLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.RoofLayer;
      RaycastHit raycastHit;
      actor.AreaType = !Physics.Raycast(actor.Position, Vector3.get_up(), ref raycastHit, 1000f, LayerMask.op_Implicit(roofLayer)) ? MapArea.AreaType.Normal : MapArea.AreaType.Indoor;
    }

    private void OnElapsedDay(TimeSpan timeSpan)
    {
      if (Object.op_Inequality((Object) this.Merchant, (Object) null))
        this.Merchant.OnDayUpdated(timeSpan);
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this._agentTable.TryGetValue(agentKey, out agentActor))
          agentActor.OnDayUpdated(timeSpan);
      }
    }

    private void OnElapsedMinute(TimeSpan timeSpan)
    {
      this.Player.OnMinuteUpdated(timeSpan);
      if (Object.op_Inequality((Object) this.Merchant, (Object) null))
        this.Merchant.OnMinuteUpdated(timeSpan);
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this._agentTable.TryGetValue(agentKey, out agentActor))
          agentActor.OnMinuteUpdated(timeSpan);
      }
    }

    private void OnElapsedSecond(TimeSpan timeSpan)
    {
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this._agentTable.TryGetValue(agentKey, out agentActor))
          agentActor.OnSecondUpdated(timeSpan);
      }
    }

    public void RefreshWeather(Weather weather)
    {
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
        this.RefreshAgentLocomotionStatus(keyValuePair.Value);
    }

    public void RefreshPlayerLocomotionStatus()
    {
    }

    public void RefreshAgentLocomotionStatus(AgentActor agent)
    {
      if (!((Behaviour) agent.NavMeshAgent).get_enabled() || agent.NavMeshAgent.get_isStopped())
        return;
      bool flag = false;
      AnimatorStateInfo animatorStateInfo = agent.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      foreach (KeyValuePair<int, PlayState> keyValuePair in Singleton<Resources>.Instance.Animation.AgentLocomotionStateTable)
      {
        if (!flag)
        {
          foreach (PlayState.Info stateInfo in keyValuePair.Value.MainStateInfo.InStateInfo.StateInfos)
          {
            if (!flag && ((AnimatorStateInfo) ref animatorStateInfo).IsName(stateInfo.stateName))
            {
              flag = true;
              agent.ActivateTransfer(true);
              agent.AbortCalc();
              agent.AbortPatrol();
              agent.ClearWalkPath();
              agent.StartPatrol();
            }
          }
        }
      }
    }

    public void RefreshTimeZone(AIProject.TimeZone timeZone)
    {
      foreach (int num1 in ((IEnumerable<int>) this.AgentTable.get_Keys()).ToArray<int>())
      {
        AgentActor agent;
        if (this.AgentTable.TryGetValue(num1, ref agent))
        {
          AgentData agentData = agent.AgentData;
          if (!agentData.SickState.Enabled && agentData.SickState.UsedMedicine)
            agentData.SickState.UsedMedicine = false;
          float t;
          if (!agentData.StatsTable.TryGetValue(1, out t))
          {
            float num2 = 50f;
            agentData.StatsTable[1] = num2;
            t = num2;
          }
          if (agent.ChaControl.fileGameInfo.phase < 3)
          {
            agentData.StatsTable[5] = (float) (Singleton<Resources>.Instance.DefinePack.MapDefines.DefaultMotivation + agent.ChaControl.fileGameInfo.motivation);
          }
          else
          {
            Dictionary<int, Threshold> dictionary;
            Threshold threshold;
            if (Singleton<Resources>.Instance.Action.PersonalityMotivation.TryGetValue(agent.ChaControl.fileParam.personality, out dictionary) && dictionary.TryGetValue(agent.ChaControl.fileGameInfo.phase, out threshold))
            {
              float num2 = threshold.Lerp(t) + (float) agent.ChaControl.fileGameInfo.motivation;
              if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(9))
                num2 += Singleton<Resources>.Instance.StatusProfile.GWifeMotivationBuff;
              if (agent.ChaControl.fileGameInfo.normalSkill.ContainsValue(33))
                num2 += Singleton<Resources>.Instance.StatusProfile.ActiveBuffMotivation;
              agentData.StatsTable[5] = num2;
            }
          }
          if (timeZone == AIProject.TimeZone.Morning)
            agentData.Greeted = false;
          this.RefreshAgentLocomotionStatus(agent);
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator RefreshAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CRefreshAsync\u003Ec__Iterator15()
      {
        \u0024this = this
      };
    }

    public void InitActionPoints()
    {
      foreach (ActionPoint actionPoint in this._pointAgent.ActionPoints)
        actionPoint.Init();
    }

    private void RefreshAgentStatus()
    {
      Waypoint[] waypoints = this._pointAgent.Waypoints;
      foreach (AgentActor agentActor in this._agentTable.Values)
        agentActor.RefreshWalkStatus(waypoints);
    }

    private void RefreshAnimalStatus()
    {
      if (!Singleton<AnimalManager>.IsInstance())
        return;
      Singleton<AnimalManager>.Instance.RefreshStates(this);
    }

    public void RefreshActiveTimeRelationObjects()
    {
      if (Object.op_Equality((Object) this.Simulator, (Object) null))
        return;
      string str1 = "_EmissionColor";
      string str2 = "_EMISSION";
      AIProject.TimeZone mapLightTimeZone = this.Simulator.MapLightTimeZone;
      Dictionary<int, bool> dictionary = (Dictionary<int, bool>) null;
      if (Singleton<Game>.IsInstance())
        dictionary = Singleton<Game>.Instance.Environment?.TimeObjOpenState;
      int num1;
      switch (mapLightTimeZone)
      {
        case AIProject.TimeZone.Morning:
          num1 = 0;
          break;
        case AIProject.TimeZone.Day:
          num1 = 1;
          break;
        case AIProject.TimeZone.Night:
          num1 = 2;
          break;
        default:
          num1 = -1;
          break;
      }
      int num2 = num1;
      Dictionary<int, GameObject> toRelease1 = DictionaryPool<int, GameObject>.Get();
      Dictionary<int, GameObject> toRelease2 = DictionaryPool<int, GameObject>.Get();
      using (Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>>.Enumerator enumerator1 = this.TimeRelationObjectTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>> current1 = enumerator1.Current;
          if (!current1.Value.IsNullOrEmpty<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>())
          {
            int key1 = current1.Key;
            using (Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>> current2 = enumerator2.Current;
                if (!current2.Value.IsNullOrEmpty<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>())
                {
                  int key2 = current2.Key;
                  using (Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>.Enumerator enumerator3 = current2.Value.GetEnumerator())
                  {
                    while (enumerator3.MoveNext())
                    {
                      KeyValuePair<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>> current3 = enumerator3.Current;
                      if (!current3.Value.IsNullOrEmpty<int, ValueTuple<GameObject, Material, float, Color>[]>())
                      {
                        bool key3 = current3.Key;
                        using (Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>.Enumerator enumerator4 = current3.Value.GetEnumerator())
                        {
                          while (enumerator4.MoveNext())
                          {
                            KeyValuePair<int, ValueTuple<GameObject, Material, float, Color>[]> current4 = enumerator4.Current;
                            if (!((IList<ValueTuple<GameObject, Material, float, Color>>) current4.Value).IsNullOrEmpty<ValueTuple<GameObject, Material, float, Color>>())
                            {
                              int key4 = current4.Key;
                              bool flag1 = key1 == num2;
                              if (key3 && flag1)
                              {
                                if (dictionary == null)
                                {
                                  flag1 = false;
                                }
                                else
                                {
                                  bool flag2;
                                  if (!dictionary.TryGetValue(key4, out flag2))
                                    flag2 = false;
                                  else
                                    flag1 = flag2;
                                }
                              }
                              switch (key2)
                              {
                                case 0:
                                  foreach (ValueTuple<GameObject, Material, float, Color> valueTuple in current4.Value)
                                  {
                                    if (!Object.op_Equality((Object) valueTuple.Item1, (Object) null))
                                    {
                                      int instanceId = ((Object) valueTuple.Item1).GetInstanceID();
                                      if (flag1)
                                      {
                                        if (toRelease2.ContainsKey(instanceId))
                                          toRelease2.Remove(instanceId);
                                        if (!toRelease1.ContainsKey(instanceId))
                                          toRelease1[instanceId] = (GameObject) valueTuple.Item1;
                                      }
                                      else if (!toRelease1.ContainsKey(instanceId) && !toRelease2.ContainsKey(instanceId))
                                        toRelease2[instanceId] = (GameObject) valueTuple.Item1;
                                    }
                                  }
                                  continue;
                                case 1:
                                  if (key1 == num2)
                                  {
                                    foreach (ValueTuple<GameObject, Material, float, Color> valueTuple in current4.Value)
                                    {
                                      if (!Object.op_Equality((Object) valueTuple.Item1, (Object) null) && !Object.op_Equality((Object) valueTuple.Item2, (Object) null))
                                      {
                                        GameObject gameObject = (GameObject) valueTuple.Item1;
                                        Material material = (Material) valueTuple.Item2;
                                        ((Object) valueTuple.Item1).GetInstanceID();
                                        if (((Material) valueTuple.Item2).HasProperty(str1))
                                        {
                                          if (flag1)
                                          {
                                            Color color1 = (Color) valueTuple.Item4;
                                            float num3 = (float) valueTuple.Item3;
                                            Color color2 = Color.op_Addition(Color.op_Multiply(Color.get_white(), Mathf.Sign(num3) * Mathf.Pow(num3, 1.5f)), color1);
                                            if (0.0 <= color2.r && 0.0 <= color2.g && 0.0 <= color2.b)
                                            {
                                              if (!material.IsKeywordEnabled(str2))
                                                material.EnableKeyword(str2);
                                              material.SetVector(str1, Color.op_Implicit(color2));
                                            }
                                            else if (material.IsKeywordEnabled(str2))
                                              material.DisableKeyword(str2);
                                          }
                                          else if (material.IsKeywordEnabled(str2))
                                            material.DisableKeyword(str2);
                                        }
                                      }
                                    }
                                    continue;
                                  }
                                  continue;
                                default:
                                  continue;
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      using (Dictionary<int, GameObject>.Enumerator enumerator = toRelease1.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Value, (Object) null) && !current.Value.get_activeSelf())
            current.Value.SetActive(true);
        }
      }
      using (Dictionary<int, GameObject>.Enumerator enumerator = toRelease2.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, GameObject> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Value, (Object) null) && current.Value.get_activeSelf())
            current.Value.SetActive(false);
        }
      }
      DictionaryPool<int, GameObject>.Release(toRelease1);
      DictionaryPool<int, GameObject>.Release(toRelease2);
      if (this.TimeLinkedLightObjectList.IsNullOrEmpty<TimeLinkedLightObject>())
        return;
      List<TimeLinkedLightObject> linkedLightObjectList = this.TimeLinkedLightObjectList;
      linkedLightObjectList.RemoveAll((Predicate<TimeLinkedLightObject>) (x => Object.op_Equality((Object) x, (Object) null)));
      foreach (TimeLinkedLightObject linkedLightObject in linkedLightObjectList)
        linkedLightObject.Refresh(mapLightTimeZone);
    }

    public void SetActiveMapEffect(bool active)
    {
      if (Object.op_Equality((Object) this._mapDataEffect, (Object) null) || this._mapDataEffect.get_activeSelf() == active)
        return;
      this._mapDataEffect.SetActive(active);
    }

    public static PlayerActor GetPlayer()
    {
      if (!Singleton<Map>.IsInstance())
        return (PlayerActor) null;
      PlayerActor player = Singleton<Map>.Instance.Player;
      return Object.op_Inequality((Object) player, (Object) null) ? player : (PlayerActor) null;
    }

    public static MerchantActor GetMerchant()
    {
      if (!Singleton<Map>.IsInstance())
        return (MerchantActor) null;
      MerchantActor merchant = Singleton<Map>.Instance.Merchant;
      return Object.op_Inequality((Object) merchant, (Object) null) ? merchant : (MerchantActor) null;
    }

    public static Camera GetCameraComponent()
    {
      if (!Singleton<Map>.IsInstance())
        return (Camera) null;
      PlayerActor player = Singleton<Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return (Camera) null;
      ActorCameraControl cameraControl = player.CameraControl;
      if (Object.op_Equality((Object) cameraControl, (Object) null))
        return (Camera) null;
      Camera cameraComponent = cameraControl.CameraComponent;
      return Object.op_Inequality((Object) cameraComponent, (Object) null) ? cameraComponent : (Camera) null;
    }

    public static Camera GetCameraComponent(PlayerActor player)
    {
      if (Object.op_Equality((Object) player, (Object) null))
        return (Camera) null;
      ActorCameraControl cameraControl = player.CameraControl;
      if (Object.op_Equality((Object) cameraControl, (Object) null))
        return (Camera) null;
      Camera cameraComponent = cameraControl.CameraComponent;
      return Object.op_Inequality((Object) cameraComponent, (Object) null) ? cameraComponent : (Camera) null;
    }

    public static CommandArea GetCommandArea()
    {
      if (!Singleton<Map>.IsInstance())
        return (CommandArea) null;
      PlayerActor player = Singleton<Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return (CommandArea) null;
      PlayerController playerController = player.PlayerController;
      if (Object.op_Equality((Object) playerController, (Object) null))
        return (CommandArea) null;
      CommandArea commandArea = playerController.CommandArea;
      return Object.op_Inequality((Object) commandArea, (Object) null) ? commandArea : (CommandArea) null;
    }

    public static CommandArea GetCommandArea(PlayerActor player)
    {
      if (Object.op_Equality((Object) player, (Object) null))
        return (CommandArea) null;
      PlayerController playerController = player.PlayerController;
      if (Object.op_Equality((Object) playerController, (Object) null))
        return (CommandArea) null;
      CommandArea commandArea = playerController.CommandArea;
      return Object.op_Inequality((Object) commandArea, (Object) null) ? commandArea : (CommandArea) null;
    }

    public static ActorCameraControl GetCameraControl()
    {
      if (!Singleton<Map>.IsInstance())
        return (ActorCameraControl) null;
      PlayerActor player = Singleton<Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return (ActorCameraControl) null;
      ActorCameraControl cameraControl = player.CameraControl;
      return Object.op_Inequality((Object) cameraControl, (Object) null) ? cameraControl : (ActorCameraControl) null;
    }

    public static ActorCameraControl GetCameraControl(PlayerActor player)
    {
      if (Object.op_Equality((Object) player, (Object) null))
        return (ActorCameraControl) null;
      ActorCameraControl cameraControl = player.CameraControl;
      return Object.op_Inequality((Object) cameraControl, (Object) null) ? cameraControl : (ActorCameraControl) null;
    }

    public static bool FadeStart(float time = -1f)
    {
      if (!Singleton<Map>.IsInstance())
        return false;
      PlayerActor player = Singleton<Map>.Instance.Player;
      if (Object.op_Equality((Object) player, (Object) null))
        return false;
      ActorCameraControl cameraControl = player.CameraControl;
      if (Object.op_Equality((Object) cameraControl, (Object) null))
        return false;
      CrossFade crossFade = cameraControl.CrossFade;
      if (Object.op_Equality((Object) crossFade, (Object) null))
        return false;
      crossFade.FadeStart(time);
      return true;
    }

    public void BuildNavMesh()
    {
      this.NavMeshSurface.BuildNavMesh();
    }

    [DebuggerHidden]
    public IEnumerator RebuildNavMeshAsync()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CRebuildNavMeshAsync\u003Ec__Iterator16()
      {
        \u0024this = this
      };
    }

    public void SetVisibleAll(bool active)
    {
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Value.IsVisible = active;
      }
    }

    public void SetVisibleAll(bool active, AgentActor exAgent)
    {
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) exAgent))
            current.Value.IsVisible = active;
        }
      }
    }

    public void EnableEntity()
    {
      PlayerActor player = this.Player;
      ((Behaviour) player.CameraControl).set_enabled(true);
      player.ChaControl.visibleAll = true;
      player.ActivateNavMeshAgent();
      using (IEnumerator<AgentActor> enumerator = this.AgentTable.get_Values().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor current = enumerator.Current;
          ValueTuple<Desire.ActionType, Desire.ActionType, bool> valueTuple = this.AgentModeCache[current.ID];
          if (valueTuple.Item3 != null)
          {
            current.ActivateNavMeshAgent();
            current.NavMeshAgent.Warp(current.Position);
          }
          else if (current.EventKey == AIProject.EventType.Move)
          {
            current.ActivateNavMeshAgent();
            current.SetDefaultStateHousingItem();
            if (Object.op_Inequality((Object) current.CurrentPoint, (Object) null))
            {
              OffMeshLink component = (OffMeshLink) ((Component) current.CurrentPoint).GetComponent<OffMeshLink>();
              if (Object.op_Inequality((Object) component, (Object) null))
              {
                Transform endTransform = component.get_endTransform();
                current.NavMeshAgent.Warp(endTransform.get_position());
                current.Rotation = endTransform.get_rotation();
              }
              current.CurrentPoint.RemoveBookingUser((Actor) current);
              current.CurrentPoint.SetActiveMapItemObjs(true);
              current.CurrentPoint.ReleaseSlot((Actor) current);
              current.CurrentPoint = (ActionPoint) null;
            }
            current.EventKey = (AIProject.EventType) 0;
            current.TargetInSightActionPoint = (ActionPoint) null;
            current.Animation.ResetDefaultAnimatorController();
            valueTuple.Item1 = (__Null) 0;
            valueTuple.Item2 = (__Null) 0;
          }
          else if (current.EventKey == AIProject.EventType.DoorOpen)
          {
            current.ActivateNavMeshAgent();
            current.SetDefaultStateHousingItem();
            if (Object.op_Inequality((Object) current.CurrentPoint, (Object) null))
            {
              DoorPoint currentPoint = current.CurrentPoint as DoorPoint;
              if (Object.op_Inequality((Object) currentPoint, (Object) null))
              {
                if (currentPoint.OpenState == DoorPoint.OpenPattern.Close)
                {
                  if (currentPoint.OpenType == DoorPoint.OpenTypeState.Right || currentPoint.OpenType == DoorPoint.OpenTypeState.Right90)
                    currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenRight, true);
                  else
                    currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenLeft, true);
                }
                currentPoint.RemoveBookingUser((Actor) current);
              }
              current.CurrentPoint.SetActiveMapItemObjs(true);
              current.CurrentPoint.ReleaseSlot((Actor) current);
              current.CurrentPoint = (ActionPoint) null;
            }
            current.EventKey = (AIProject.EventType) 0;
            current.TargetInSightActionPoint = (ActionPoint) null;
            current.Animation.ResetDefaultAnimatorController();
            valueTuple.Item1 = (__Null) 0;
            valueTuple.Item2 = (__Null) 0;
          }
          else if (current.EventKey == AIProject.EventType.Toilet || current.EventKey == AIProject.EventType.DressIn || (current.EventKey == AIProject.EventType.Bath || current.EventKey == AIProject.EventType.DressOut) || (current.AgentData.PlayedDressIn || valueTuple.Item1 == 97))
          {
            current.ActivateNavMeshAgent();
            current.SetDefaultStateHousingItem();
            if (Object.op_Inequality((Object) current.CurrentPoint, (Object) null))
            {
              current.CurrentPoint.SetActiveMapItemObjs(true);
              current.CurrentPoint.ReleaseSlot((Actor) current);
              current.CurrentPoint = (ActionPoint) null;
            }
            current.ChaControl.ChangeNowCoordinate(true, true);
            current.AgentData.BathCoordinateFileName = (string) null;
            current.ChaControl.SetClothesState(0, (byte) 0, true);
            current.ChaControl.SetClothesState(1, (byte) 0, true);
            current.ChaControl.SetClothesState(2, (byte) 0, true);
            current.ChaControl.SetClothesState(3, (byte) 0, true);
            current.AgentData.PlayedDressIn = false;
            current.EventKey = (AIProject.EventType) 0;
            current.TargetInSightActionPoint = (ActionPoint) null;
            current.Animation.ResetDefaultAnimatorController();
            valueTuple.Item1 = (__Null) 0;
            valueTuple.Item2 = (__Null) 0;
          }
          else if (valueTuple.Item1 == 44)
          {
            current.ActivateNavMeshAgent();
            current.EventKey = (AIProject.EventType) 0;
            current.TargetInSightActionPoint = (ActionPoint) null;
            current.Animation.ResetDefaultAnimatorController();
            valueTuple.Item1 = (__Null) 0;
            valueTuple.Item2 = (__Null) 0;
          }
          ((Behaviour) current.Controller).set_enabled(true);
          current.ChaControl.visibleAll = true;
          ((Behaviour) current.AnimationAgent).set_enabled(true);
          current.SetActiveOnEquipedItem(false);
          current.EnableBehavior();
          if (valueTuple.Item1 == null && valueTuple.Item2 == null)
          {
            current.ResetActionFlag();
            if (current.EnabledSchedule)
              current.EnabledSchedule = false;
          }
          current.Mode = (Desire.ActionType) valueTuple.Item1;
          current.BehaviorResources.ChangeMode((Desire.ActionType) valueTuple.Item2);
          current.AnimationAgent.EnableItems();
          current.AnimationAgent.EnableParticleRenderer();
        }
      }
      Singleton<Map>.Instance.Merchant.EnableEntity();
      AnimalBase.CreateDisplay = true;
      AnimalManager instance = Singleton<AnimalManager>.Instance;
      for (int index = 0; index < instance.Animals.Count; ++index)
      {
        AnimalBase animal = instance.Animals[index];
        if (!Object.op_Equality((Object) animal, (Object) null))
        {
          animal.BodyEnabled = true;
          ((Behaviour) animal).set_enabled(true);
        }
      }
      instance.SettingAnimalPointBehavior();
    }

    public void DisableEntity()
    {
      PlayerActor player = this.Player;
      ((Behaviour) player.CameraControl).set_enabled(false);
      player.ChaControl.visibleAll = false;
      using (IEnumerator<AgentActor> enumerator = this.AgentTable.get_Values().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor current = enumerator.Current;
          Desire.ActionType mode = current.BehaviorResources.Mode;
          bool enabled = ((Behaviour) current.NavMeshAgent).get_enabled();
          this.AgentModeCache[current.ID] = new ValueTuple<Desire.ActionType, Desire.ActionType, bool>(current.Mode, mode, enabled);
          current.SetActiveOnEquipedItem(false);
          ((Behaviour) current.Controller).set_enabled(false);
          if (enabled)
            ((Behaviour) current.NavMeshAgent).set_enabled(false);
          ((Behaviour) current.AnimationAgent).set_enabled(false);
          current.ChaControl.visibleAll = false;
          if (mode == Desire.ActionType.EndTaskMasturbation || mode == Desire.ActionType.EndTaskLesbianH || mode == Desire.ActionType.EndTaskLesbianMerchantH)
            current.BehaviorResources.ChangeMode(Desire.ActionType.Idle);
          current.DisableBehavior();
          current.AnimationAgent.DisableItems();
          current.AnimationAgent.DisableParticleRenderer();
        }
      }
      Singleton<Map>.Instance.Merchant.DisableEntity();
      AnimalBase.CreateDisplay = false;
      AnimalManager instance = Singleton<AnimalManager>.Instance;
      for (int index = 0; index < instance.Animals.Count; ++index)
      {
        AnimalBase animal = instance.Animals[index];
        if (!Object.op_Equality((Object) animal, (Object) null))
        {
          ((Behaviour) animal).set_enabled(false);
          animal.BodyEnabled = false;
        }
      }
      instance.ClearAnimalPointBehavior();
    }

    public void EnableEntity(Actor exActor)
    {
      ReadOnlyDictionary<int, AgentActor> agentTable = Singleton<Map>.Instance.AgentTable;
      if (agentTable.IsNullOrEmpty<int, AgentActor>())
        return;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = agentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (!Object.op_Equality((Object) agentActor, (Object) null) && !Object.op_Equality((Object) agentActor, (Object) exActor))
            agentActor.EnableEntity();
        }
      }
      if (!Object.op_Inequality((Object) this.Merchant, (Object) null) || !Object.op_Inequality((Object) this.Merchant, (Object) exActor))
        return;
      this.Merchant.EnableEntity();
    }

    public void DisableEntity(Actor exActor)
    {
      ReadOnlyDictionary<int, AgentActor> agentTable = Singleton<Map>.Instance.AgentTable;
      if (agentTable.IsNullOrEmpty<int, AgentActor>())
        return;
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = agentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AgentActor agentActor = enumerator.Current.Value;
          if (!Object.op_Equality((Object) agentActor, (Object) null) && !Object.op_Equality((Object) agentActor, (Object) exActor))
            agentActor.DisableEntity();
        }
      }
      if (!Object.op_Inequality((Object) this.Merchant, (Object) null) || !Object.op_Inequality((Object) this.Merchant, (Object) exActor))
        return;
      this.Merchant.DisableEntity();
    }

    public void ResetAgentsInHousingArea()
    {
      List<AgentActor> toRelease = ListPool<AgentActor>.Get();
      foreach (KeyValuePair<int, AgentActor> keyValuePair in this._agentTable)
      {
        if (keyValuePair.Value.AreaID == this.HousingAreaID)
        {
          toRelease.Add(keyValuePair.Value);
          Actor commandPartner = keyValuePair.Value.CommandPartner;
          if (Object.op_Inequality((Object) commandPartner, (Object) null) && ((object) commandPartner).GetType() == typeof (AgentActor) && !((IEnumerable<Actor>) toRelease).Contains<Actor>(commandPartner))
            toRelease.Add(commandPartner as AgentActor);
        }
      }
      List<Transform> source;
      if (this.HousingRecoveryPointTable.TryGetValue(this.HousingID, out source))
      {
        int num = 0;
        foreach (AgentActor agentActor1 in toRelease)
        {
          Transform element = source.GetElement<Transform>(num++);
          if (!Object.op_Equality((Object) element, (Object) null))
          {
            agentActor1.ActivateNavMeshAgent();
            agentActor1.ClearItems();
            agentActor1.ClearParticles();
            agentActor1.SetActiveOnEquipedItem(false);
            agentActor1.AgentData.CarryingItem = (StuffItem) null;
            agentActor1.ChaControl.ChangeNowCoordinate(true, true);
            agentActor1.AgentData.BathCoordinateFileName = (string) null;
            agentActor1.ChaControl.SetClothesState(0, (byte) 0, true);
            agentActor1.ChaControl.SetClothesState(1, (byte) 0, true);
            agentActor1.ChaControl.SetClothesState(2, (byte) 0, true);
            agentActor1.ChaControl.SetClothesState(3, (byte) 0, true);
            agentActor1.AgentData.PlayedDressIn = false;
            agentActor1.SetDefaultStateHousingItem();
            if (Object.op_Inequality((Object) agentActor1.CurrentPoint, (Object) null))
            {
              agentActor1.CurrentPoint.SetActiveMapItemObjs(true);
              agentActor1.CurrentPoint.ReleaseSlot((Actor) agentActor1);
              agentActor1.CurrentPoint = (ActionPoint) null;
            }
            agentActor1.EventKey = (AIProject.EventType) 0;
            agentActor1.TargetInSightActionPoint = (ActionPoint) null;
            agentActor1.CommandPartner = (Actor) null;
            agentActor1.TargetInSightActor = (Actor) null;
            agentActor1.Animation.ResetDefaultAnimatorController();
            agentActor1.ChangeBehavior(Desire.ActionType.Normal);
            agentActor1.NavMeshAgent.Warp(element.get_position());
            AgentActor agentActor2 = agentActor1;
            Quaternion rotation = element.get_rotation();
            Quaternion quaternion = Quaternion.Euler(0.0f, (float) ((Quaternion) ref rotation).get_eulerAngles().y, 0.0f);
            agentActor2.Rotation = quaternion;
          }
        }
      }
      ListPool<AgentActor>.Release(toRelease);
    }

    public void InitCommandable()
    {
      this.Player.PlayerController.CommandArea.SetCommandableObjects((ICommandable[]) ((Component) this._pointAgent).GetComponentsInChildren<ICommandable>(true));
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this.AgentTable.TryGetValue(agentKey, ref agentActor))
          this.AddCommandable((ICommandable) agentActor);
      }
      this.AddCommandable((ICommandable) this.Merchant);
      foreach (AnimalBase animal in Singleton<AnimalManager>.Instance.Animals)
      {
        if (Object.op_Inequality((Object) animal, (Object) null) && animal.IsCommandable)
          this.AddCommandable((ICommandable) animal);
      }
    }

    public void AddCommandable(ICommandable command)
    {
      this.Player.PlayerController.CommandArea.AddCommandableObject(command);
    }

    private int UnusedRegID()
    {
      int num = 10000;
      while (Singleton<Game>.Instance.Environment.RegIDList.Contains(num))
        ++num;
      return num;
    }

    public int RegisterRuntimePoint(Point pt)
    {
      if (Object.op_Equality((Object) pt, (Object) null))
        return -1;
      int key;
      if (pt.RegisterID == -1)
      {
        key = this.UnusedRegID();
        pt.RegisterID = key;
      }
      else
        key = pt.RegisterID;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      environment.RegIDList.Add(key);
      switch (pt)
      {
        case SearchActionPoint _:
          if (!environment.SearchActionLockTable.TryGetValue(key, out AIProject.SaveData.Environment.SearchActionInfo _))
          {
            environment.SearchActionLockTable[key] = new AIProject.SaveData.Environment.SearchActionInfo();
            break;
          }
          break;
        case FarmPoint _:
          FarmPoint farmPoint = pt as FarmPoint;
          List<AIProject.SaveData.Environment.PlantInfo> plantInfoList1;
          if (!environment.FarmlandTable.TryGetValue(key, out plantInfoList1))
          {
            List<AIProject.SaveData.Environment.PlantInfo> plantInfoList2 = new List<AIProject.SaveData.Environment.PlantInfo>();
            environment.FarmlandTable[key] = plantInfoList2;
            plantInfoList1 = plantInfoList2;
            if (farmPoint.Kind == FarmPoint.FarmKind.Plant && plantInfoList1.Count != farmPoint.HarvestSections.Length)
            {
              plantInfoList1.Clear();
              for (int index = 0; index < farmPoint.HarvestSections.Length; ++index)
                plantInfoList1.Add((AIProject.SaveData.Environment.PlantInfo) null);
            }
          }
          foreach (FarmSection harvestSection in farmPoint.HarvestSections)
            harvestSection.HarvestID = key;
          break;
      }
      return key;
    }

    public void UnregisterRuntimePoint(Point pt, bool removeEntry = true)
    {
      if (Object.op_Equality((Object) pt, (Object) null))
        return;
      int registerId = pt.RegisterID;
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      if (removeEntry)
      {
        if (!environment.RegIDList.Remove(registerId))
          return;
        environment.SearchActionLockTable.Remove(registerId);
        environment.FarmlandTable.Remove(registerId);
      }
      else
      {
        if (this._removeRegIDCache.Contains(registerId))
          return;
        this._removeRegIDCache.Add(registerId);
      }
    }

    public void RemoveRegIDCache(int regID)
    {
      if (!this._removeRegIDCache.Contains(regID))
        return;
      this._removeRegIDCache.Remove(regID);
    }

    public void SeqRemoveCacheRegID()
    {
      AIProject.SaveData.Environment environment = Singleton<Game>.Instance.Environment;
      foreach (int key in this._removeRegIDCache)
      {
        if (environment.RegIDList.Remove(key))
        {
          environment.SearchActionLockTable.Remove(key);
          environment.FarmlandTable.Remove(key);
        }
      }
      this._removeRegIDCache.Clear();
    }

    public void AddAppendTargetPoints(ActionPoint[] actionPoints)
    {
      this._pointAgent.AppendActionPoints.Clear();
      this._pointAgent.AppendActionPoints.AddRange((IEnumerable<ActionPoint>) actionPoints);
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk))
          chunk.LoadAppendActionPoints(actionPoints);
      }
      Singleton<Map>.Instance.WarpPointDic.Clear();
      foreach (ActionPoint actionPoint in actionPoints)
      {
        actionPoint.Init();
        this.AddAppendCommandable((ICommandable) actionPoint);
        this.CheckWarpPoint(actionPoint);
      }
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this.AgentTable.TryGetValue(agentKey, ref agentActor))
          agentActor.AgentController.SearchArea.AddPoints(actionPoints);
      }
    }

    private void CheckWarpPoint(ActionPoint pt)
    {
      WarpPoint warpPoint = pt as WarpPoint;
      if (Object.op_Equality((Object) warpPoint, (Object) null))
        return;
      MapArea ownerArea = warpPoint.OwnerArea;
      if (Object.op_Equality((Object) ownerArea, (Object) null))
        return;
      int chunkId = ownerArea.ChunkID;
      Dictionary<int, List<WarpPoint>> dictionary1;
      if (!Singleton<Map>.Instance.WarpPointDic.TryGetValue(chunkId, out dictionary1))
      {
        Dictionary<int, List<WarpPoint>> dictionary2 = new Dictionary<int, List<WarpPoint>>();
        Singleton<Map>.Instance.WarpPointDic[chunkId] = dictionary2;
        dictionary1 = dictionary2;
      }
      List<WarpPoint> warpPointList1;
      if (!dictionary1.TryGetValue(warpPoint.TableID, out warpPointList1))
      {
        List<WarpPoint> warpPointList2 = new List<WarpPoint>();
        dictionary1[warpPoint.TableID] = warpPointList2;
        warpPointList1 = warpPointList2;
      }
      if (warpPointList1.Contains(warpPoint))
        return;
      warpPointList1.Add(warpPoint);
    }

    public void RemoveAppendActionPoint(ActionPoint point)
    {
      this._pointAgent.AppendActionPoints.Remove(point);
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk))
        {
          foreach (MapArea mapArea in chunk.MapAreas)
            mapArea.AppendActionPoints.Remove(point);
          chunk.AppendActionPoints.Remove(point);
        }
      }
    }

    public void RemoveAgentSearchActionPoint(ActionPoint actionPoint)
    {
      foreach (int agentKey in this._agentKeys)
      {
        AgentActor agentActor;
        if (this.AgentTable.TryGetValue(agentKey, ref agentActor))
        {
          agentActor.AgentController.SearchArea.RemovePoint(actionPoint);
          agentActor.RemoveActionPoint(actionPoint);
        }
      }
    }

    public void AddRuntimeFarmPoints(FarmPoint[] farmPoints)
    {
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk))
          chunk.LoadAppendFarmPoints(farmPoints);
      }
      if (Object.op_Inequality((Object) this._pointAgent, (Object) null))
        this._pointAgent.AddRuntimeFarmPoints(farmPoints);
      foreach (FarmPoint farmPoint in farmPoints)
      {
        this.AddAppendCommandable((ICommandable) farmPoint);
        farmPoint.SetChickenWayPoint();
        farmPoint.CreateChicken();
      }
    }

    public void RemoveRuntimeFarmPoint(FarmPoint point)
    {
      if (Object.op_Inequality((Object) point, (Object) null))
        point.DestroyChicken();
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk))
        {
          foreach (MapArea mapArea in chunk.MapAreas)
            mapArea.AppendFarmPoints.Remove(point);
          chunk.AppendFarmPoints.Remove(point);
        }
      }
      if (!Object.op_Inequality((Object) this._pointAgent, (Object) null))
        return;
      this._pointAgent.RemoveRuntimeFarmPoint(point);
    }

    public void AddPetHomePoints(PetHomePoint[] petHomePoints)
    {
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
          chunk.LoadAppendPetHomePoints(petHomePoints);
      }
      foreach (ICommandable petHomePoint in petHomePoints)
        this.AddAppendCommandable(petHomePoint);
      if (Object.op_Inequality((Object) this.PointAgent, (Object) null))
        this.PointAgent.AddPetHomePoints(petHomePoints);
      Dictionary<int, AIProject.SaveData.Environment.PetHomeInfo> dictionary = !Singleton<Game>.IsInstance() ? (Dictionary<int, AIProject.SaveData.Environment.PetHomeInfo>) null : Singleton<Game>.Instance.Environment?.PetHomeStateTable;
      if (dictionary == null)
        return;
      foreach (PetHomePoint petHomePoint1 in petHomePoints)
      {
        if (!Object.op_Equality((Object) petHomePoint1, (Object) null))
        {
          int registerId = petHomePoint1.RegisterID;
          AIProject.SaveData.Environment.PetHomeInfo petHomeInfo1;
          if (!dictionary.TryGetValue(registerId, out petHomeInfo1) || petHomeInfo1 == null)
          {
            PetHomePoint petHomePoint2 = petHomePoint1;
            AIProject.SaveData.Environment.PetHomeInfo petHomeInfo2 = new AIProject.SaveData.Environment.PetHomeInfo();
            dictionary[registerId] = petHomeInfo2;
            AIProject.SaveData.Environment.PetHomeInfo petHomeInfo3;
            petHomeInfo1 = petHomeInfo3 = petHomeInfo2;
            petHomePoint2.SaveData = petHomeInfo3;
            petHomeInfo1.HousingID = this.HousingID;
            petHomeInfo1.AnimalData = (AIProject.SaveData.AnimalData) null;
            petHomeInfo1.ChaseActor = false;
          }
          else
            petHomePoint1.SaveData = petHomeInfo1;
        }
      }
    }

    public void RemovePetHomePoint(PetHomePoint petHomePoint)
    {
      if (Object.op_Equality((Object) petHomePoint, (Object) null))
        return;
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
        {
          if (!((IList<MapArea>) chunk.MapAreas).IsNullOrEmpty<MapArea>())
          {
            foreach (MapArea mapArea in chunk.MapAreas)
              mapArea.AppendPetHomePoints.Remove(petHomePoint);
          }
          chunk.AppendPetHomePoints.Remove(petHomePoint);
        }
      }
      if (Object.op_Inequality((Object) this.PointAgent, (Object) null))
        this.PointAgent.RemovePetHomePoint(petHomePoint);
      Dictionary<int, AIProject.SaveData.Environment.PetHomeInfo> source = !Singleton<Game>.IsInstance() ? (Dictionary<int, AIProject.SaveData.Environment.PetHomeInfo>) null : Singleton<Game>.Instance.Environment?.PetHomeStateTable;
      if (source.IsNullOrEmpty<int, AIProject.SaveData.Environment.PetHomeInfo>())
        return;
      source.Remove(petHomePoint.RegisterID);
    }

    public void AddJukePoints(JukePoint[] jukePoints)
    {
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
          chunk.LoadAppendJukePoints(jukePoints);
      }
      foreach (JukePoint jukePoint in jukePoints)
      {
        if (!Object.op_Equality((Object) jukePoint, (Object) null))
        {
          jukePoint.SetAreaID();
          this.AddAppendCommandable((ICommandable) jukePoint);
        }
      }
      if (!Object.op_Inequality((Object) this._pointAgent, (Object) null))
        return;
      this._pointAgent.AddJukePoints(jukePoints);
    }

    public void RemoveJukePoint(JukePoint point)
    {
      int[] array = this.ChunkTable.Keys.ToArray<int>();
      if (!((IList<int>) array).IsNullOrEmpty<int>())
      {
        foreach (int key in array)
        {
          Chunk chunk;
          if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
          {
            foreach (MapArea mapArea in chunk.MapAreas)
              mapArea.AppendJukePoints.Remove(point);
            chunk.AppendJukePoints.Remove(point);
          }
        }
      }
      if (Object.op_Inequality((Object) this._pointAgent, (Object) null))
        this._pointAgent.RemoveJukePoint(point);
      Dictionary<int, string> source = (Dictionary<int, string>) null;
      if (Singleton<Game>.IsInstance())
      {
        if (this.MapID == 0)
        {
          source = Singleton<Game>.Instance.Environment?.JukeBoxAudioNameTable;
        }
        else
        {
          Dictionary<int, Dictionary<int, string>> boxAudioNameTable = Singleton<Game>.Instance.Environment?.AnotherJukeBoxAudioNameTable;
          if (boxAudioNameTable != null && (!boxAudioNameTable.TryGetValue(this.MapID, out source) || source == null))
          {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            boxAudioNameTable[this.MapID] = dictionary;
            source = dictionary;
          }
        }
      }
      if (source.IsNullOrEmpty<int, string>() || !Object.op_Inequality((Object) point, (Object) null))
        return;
      int areaId = point.AreaID;
      bool flag = true;
      if (Object.op_Inequality((Object) this._pointAgent, (Object) null))
      {
        IReadOnlyDictionary<int, List<JukePoint>> jukePointTable = this._pointAgent.JukePointTable;
        if (jukePointTable != null && 0 < ((IReadOnlyCollection<KeyValuePair<int, List<JukePoint>>>) jukePointTable).get_Count())
        {
          List<JukePoint> self = (List<JukePoint>) null;
          if (jukePointTable.TryGetValue(areaId, ref self) && !self.IsNullOrEmpty<JukePoint>())
          {
            self.RemoveAll((Predicate<JukePoint>) (x => Object.op_Equality((Object) x, (Object) null)));
            flag = self.Count == 0 || !self.Exists((Predicate<JukePoint>) (x => Object.op_Inequality((Object) x, (Object) point)));
          }
        }
      }
      if (!flag || !source.ContainsKey(areaId))
        return;
      source.Remove(areaId);
    }

    public void AddRuntimeCraftPoints(CraftPoint[] craftPoints)
    {
      List<int> list = this.ChunkTable.Keys.ToList<int>();
      if (!list.IsNullOrEmpty<int>())
      {
        foreach (int key in list)
        {
          Chunk chunk;
          if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
            chunk.LoadAppendCraftPoints(craftPoints);
        }
      }
      foreach (CraftPoint craftPoint in craftPoints)
      {
        this.AddAppendCommandable((ICommandable) craftPoint);
        if (Object.op_Inequality((Object) craftPoint, (Object) null) && craftPoint.Kind == CraftPoint.CraftKind.Recycling)
        {
          AIProject.SaveData.Environment environment = !Singleton<Game>.IsInstance() ? (AIProject.SaveData.Environment) null : Singleton<Game>.Instance.Environment;
          Dictionary<int, RecyclingData> dictionary = environment == null ? (Dictionary<int, RecyclingData>) null : environment.RecyclingDataTable;
          if (dictionary != null)
          {
            RecyclingData recyclingData = (RecyclingData) null;
            if (!dictionary.TryGetValue(craftPoint.RegisterID, out recyclingData) || recyclingData == null)
              dictionary[craftPoint.RegisterID] = new RecyclingData();
          }
        }
      }
    }

    public void RemoveCraftPoint(CraftPoint point)
    {
      foreach (int key in this.ChunkTable.Keys.ToList<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
        {
          foreach (MapArea mapArea in chunk.MapAreas)
            mapArea.AppendCraftPoints.Remove(point);
          chunk.AppendCraftPoints.Remove(point);
        }
      }
      if (!Object.op_Inequality((Object) point, (Object) null) || point.Kind != CraftPoint.CraftKind.Recycling)
        return;
      AIProject.SaveData.Environment environment = !Singleton<Game>.IsInstance() ? (AIProject.SaveData.Environment) null : Singleton<Game>.Instance.Environment;
      Dictionary<int, RecyclingData> dictionary = environment == null ? (Dictionary<int, RecyclingData>) null : environment.RecyclingDataTable;
      if (dictionary == null || !dictionary.ContainsKey(point.RegisterID))
        return;
      dictionary.Remove(point.RegisterID);
    }

    public void AddRuntimeLightSwitchPoints(LightSwitchPoint[] lightSwitchPoints)
    {
      List<int> list = this.ChunkTable.Keys.ToList<int>();
      if (!list.IsNullOrEmpty<int>())
      {
        foreach (int key in list)
        {
          Chunk chunk;
          if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
            chunk.LoadAppendLightSwitchPoints(lightSwitchPoints);
        }
      }
      if (((IList<LightSwitchPoint>) lightSwitchPoints).IsNullOrEmpty<LightSwitchPoint>())
        return;
      foreach (LightSwitchPoint lightSwitchPoint in lightSwitchPoints)
      {
        this.AddAppendCommandable((ICommandable) lightSwitchPoint);
        lightSwitchPoint.Switch(lightSwitchPoint.IsSwitch());
      }
    }

    public void RemoveRuntimeLightSwitchPoint(LightSwitchPoint point)
    {
      List<int> list = this.ChunkTable.Keys.ToList<int>();
      if (!list.IsNullOrEmpty<int>())
      {
        foreach (int key in list)
        {
          Chunk chunk;
          if (this.ChunkTable.TryGetValue(key, out chunk) && !Object.op_Equality((Object) chunk, (Object) null))
          {
            foreach (MapArea mapArea in chunk.MapAreas)
              mapArea.AppendLightSwitchPoints.Remove(point);
            chunk.AppendLightSwitchPoints.Remove(point);
          }
        }
      }
      Dictionary<int, bool> source = !Singleton<Game>.IsInstance() ? (Dictionary<int, bool>) null : Singleton<Game>.Instance.Environment?.LightObjectSwitchStateTable;
      if (source.IsNullOrEmpty<int, bool>() || !source.ContainsKey(point.RegisterID))
        return;
      source.Remove(point.RegisterID);
    }

    public void AddAppendCommandable(ICommandable command)
    {
      this._appendCommandables.Add(command);
      this.Player.PlayerController.CommandArea.AddCommandableObject(command);
    }

    public void RemoveAppendCommandable(ICommandable command)
    {
      this._appendCommandables.Remove(command);
      this.Player.PlayerController.CommandArea.RemoveCommandableObject(command);
    }

    public void ClearAppendCommands()
    {
      if (this._appendCommandables.IsNullOrEmpty<ICommandable>())
        return;
      foreach (ICommandable appendCommandable in this._appendCommandables)
      {
        this.Player.PlayerController.CommandArea.RemoveCommandableObject(appendCommandable);
        if (appendCommandable is ActionPoint)
        {
          ActionPoint ap = appendCommandable as ActionPoint;
          foreach (int agentKey in this._agentKeys)
          {
            AgentActor agentActor;
            if (this.AgentTable.TryGetValue(agentKey, ref agentActor))
              agentActor.AgentController.SearchArea.RemovePoint(ap);
          }
        }
      }
      this._appendCommandables.Clear();
    }

    public void AddAppendHPoints(HPoint[] hPoints)
    {
      this._pointAgent.AppendHPoints.Clear();
      this._pointAgent.AppendHPoints.AddRange((IEnumerable<HPoint>) hPoints);
      foreach (int key in this.ChunkTable.Keys.ToArray<int>())
      {
        Chunk chunk;
        if (this.ChunkTable.TryGetValue(key, out chunk))
          chunk.LoadAppendHPoints(hPoints);
      }
    }

    public List<ValueTuple<int, List<StuffItem>>> GetInventoryList()
    {
      List<ValueTuple<int, List<StuffItem>>> valueTupleList = ListPool<ValueTuple<int, List<StuffItem>>>.Get();
      WorldData worldData = !Singleton<Game>.IsInstance() ? (WorldData) null : Singleton<Game>.Instance.WorldData;
      AIProject.SaveData.Environment environment = worldData == null ? (AIProject.SaveData.Environment) null : worldData.Environment;
      DefinePack definePack = !Singleton<Resources>.IsInstance() ? (DefinePack) null : Singleton<Resources>.Instance.DefinePack;
      if (worldData != null && environment != null)
      {
        for (int index = 0; index < 2; ++index)
        {
          List<StuffItem> stuffItemList;
          int num;
          if (index != 0)
          {
            if (index == 1)
            {
              stuffItemList = environment.ItemListInStorage;
              num = !Object.op_Inequality((Object) definePack, (Object) null) ? 0 : definePack.ItemBoxCapacityDefines.StorageCapacity;
            }
            else
              continue;
          }
          else
          {
            PlayerData playerData = worldData.PlayerData;
            stuffItemList = playerData.ItemList;
            num = playerData.InventorySlotMax;
          }
          if (0 < num && stuffItemList != null)
            valueTupleList.Add(new ValueTuple<int, List<StuffItem>>(num, stuffItemList));
        }
      }
      return valueTupleList;
    }

    public void ReturnInventoryList(List<ValueTuple<int, List<StuffItem>>> list)
    {
      if (list == null)
        return;
      list.Clear();
      ListPool<ValueTuple<int, List<StuffItem>>>.Release(list);
    }

    public void SendItemListToList(int slotMax, List<StuffItem> sender, List<StuffItem> receiver)
    {
      if (slotMax <= 0 || sender.IsNullOrEmpty<StuffItem>() || receiver == null)
        return;
      for (int index = 0; index < sender.Count; ++index)
      {
        StuffItem element = sender.GetElement<StuffItem>(index);
        if (element == null || element.Count <= 0)
        {
          sender.RemoveAt(index);
          --index;
        }
        else
        {
          StuffItem stuffItem = new StuffItem(element);
          int possible = 0;
          ((IReadOnlyCollection<StuffItem>) receiver).CanAddItem(slotMax, stuffItem, out possible);
          if (0 < possible)
          {
            possible = Mathf.Min(stuffItem.Count, possible);
            receiver.AddItem(stuffItem, possible, slotMax);
          }
          element.Count -= possible;
          if (element.Count <= 0)
          {
            sender.RemoveAt(index);
            --index;
          }
        }
      }
    }

    public void WarpToBasePoint(BasePoint basePoint, System.Action onEndFadeIn = null, System.Action onCompleted = null)
    {
      if (Object.op_Equality((Object) basePoint, (Object) null))
        return;
      this.IsWarpProc = true;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        System.Action action1 = onEndFadeIn;
        if (action1 != null)
          action1();
        Transform warpPt = basePoint.WarpPoint;
        PlayerActor player = this.Player;
        player.NavMeshAgent.Warp(warpPt.get_position());
        Quaternion rotation = warpPt.get_rotation();
        player.Rotation = rotation;
        Quaternion quaternion = rotation;
        player.CameraControl.XAxisValue = (float) ((Quaternion) ref quaternion).get_eulerAngles().y;
        player.CameraControl.YAxisValue = 0.6f;
        Actor partner = player.Partner;
        if (Object.op_Inequality((Object) partner, (Object) null))
        {
          AgentActor agentActor = partner as AgentActor;
          if (Object.op_Inequality((Object) agentActor, (Object) null) && agentActor.EventKey == AIProject.EventType.Move)
            agentActor.Animation.StopAllAnimCoroutine();
          ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 2), 2), (System.Action<M0>) (_ =>
          {
            partner.NavMeshAgent.Warp(Vector3.op_Addition(player.Position, Quaternion.op_Multiply(player.Rotation, Singleton<Resources>.Instance.AgentProfile.GetOffsetInParty(partner.ChaControl.GetShapeBodyValue(0)))));
            partner.Rotation = warpPt.get_rotation();
          }));
        }
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, false), (System.Action<M0>) (__ => {}), (System.Action) (() =>
        {
          this.IsWarpProc = false;
          System.Action action2 = onCompleted;
          if (action2 == null)
            return;
          action2();
        }));
      }));
    }

    public bool IsWarpProc { get; private set; }

    public void WarpToPairPoint(WarpPoint warpPoint, System.Action onEndFadeIn = null, System.Action onCompleted = null)
    {
      if (Object.op_Equality((Object) warpPoint, (Object) null))
        return;
      this.IsWarpProc = true;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (System.Action<M0>) (_ => {}), (System.Action) (() =>
      {
        System.Action action1 = onEndFadeIn;
        if (action1 != null)
          action1();
        Transform warpTrans = ((Component) warpPoint).get_transform();
        PlayerActor player = this.Player;
        player.NavMeshWarp(warpTrans, 3, 100f);
        player.CameraControl.XAxisValue = (float) warpTrans.get_eulerAngles().y;
        player.CameraControl.YAxisValue = 0.6f;
        Actor partner = player.Partner;
        if (Object.op_Inequality((Object) partner, (Object) null))
        {
          AgentActor agentActor = partner as AgentActor;
          if (Object.op_Inequality((Object) agentActor, (Object) null) && agentActor.EventKey == AIProject.EventType.Move)
            agentActor.Animation.StopAllAnimCoroutine();
          ObservableExtensions.Subscribe<long>(Observable.Take<long>(Observable.Skip<long>((IObservable<M0>) Observable.EveryLateUpdate(), 2), 2), (System.Action<M0>) (_ => partner.NavMeshWarp(Vector3.op_Addition(player.Position, Quaternion.op_Multiply(player.Rotation, Singleton<Resources>.Instance.AgentProfile.GetOffsetInParty(partner.ChaControl.GetShapeBodyValue(0)))), warpTrans.get_rotation(), 0, 100f)));
        }
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, false), (System.Action<M0>) (__ => {}), (System.Action) (() =>
        {
          this.IsWarpProc = false;
          System.Action action2 = onCompleted;
          if (action2 == null)
            return;
          action2();
        }));
      }));
    }

    public void ChangeMap(int mapID, System.Action onEndFadeIn = null, System.Action onCompleted = null)
    {
      Singleton<Game>.Instance.WorldData.MapID = mapID;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => this.ChangeMapCoroutine(mapID, onEndFadeIn, onCompleted)), false));
    }

    [DebuggerHidden]
    private IEnumerator ChangeMapCoroutine(
      int mapID,
      System.Action onEndFadeIn = null,
      System.Action onCompleted = null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new Map.\u003CChangeMapCoroutine\u003Ec__Iterator17()
      {
        onEndFadeIn = onEndFadeIn,
        mapID = mapID,
        onCompleted = onCompleted,
        \u0024this = this
      };
    }

    public bool WaitCompletionAgents()
    {
      foreach (Actor actor in this._agentTable.Values)
      {
        if (!actor.IsInit)
          return false;
      }
      return true;
    }

    public List<Map.VisibleObject> LstMapVanish
    {
      get
      {
        return this.lstMapVanish;
      }
    }

    public void SetVanishList()
    {
      Dictionary<int, List<List<Resources.MapTables.VisibleObjectInfo>>> vanishList = Singleton<Resources>.Instance.Map.VanishList;
      List<List<Resources.MapTables.VisibleObjectInfo>> visibleObjectInfoListList;
      if (!Singleton<Resources>.Instance.Map.VanishList.TryGetValue(this.MapID, out visibleObjectInfoListList))
        return;
      for (int index1 = 0; index1 < visibleObjectInfoListList.Count; ++index1)
      {
        int index2 = index1;
        for (int index3 = 0; index3 < visibleObjectInfoListList[index2].Count; ++index3)
        {
          int index4 = index3;
          this.lstMapVanish.Add(new Map.VisibleObject()
          {
            nameCollider = visibleObjectInfoListList[index2][index4].nameCollider,
            collider = (Collider) GameObject.Find(visibleObjectInfoListList[index2][index4].nameCollider)?.GetComponentInChildren<Collider>(),
            vanishObj = GameObject.Find(visibleObjectInfoListList[index2][index4].VanishObjName)
          });
        }
      }
    }

    [Serializable]
    public class VisibleObject
    {
      public string nameCollider = string.Empty;
      public bool isVisible = true;
      public Collider collider;
      public float delay;
      public GameObject vanishObj;
    }
  }
}
