// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.AgentData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ADV;
using AIChara;
using AIProject.Definitions;
using Manager;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class AgentData : ICharacterInfo, IDiffComparer, IParams, ICommandData
  {
    public static int[] DesireTableKeys = Enumerable.Range(0, 16).ToArray<int>();
    private CharaParams _param;

    public AgentData()
    {
      this.Init();
    }

    [IgnoreMember]
    public CharaParams param
    {
      get
      {
        return this.GetCache<CharaParams>(ref this._param, (Func<CharaParams>) (() => new CharaParams((ICommandData) this, "H")));
      }
    }

    [IgnoreMember]
    Params IParams.param
    {
      get
      {
        return (Params) this.param;
      }
    }

    [Key(0)]
    public System.Version Version { get; set; } = new System.Version();

    [Key(1)]
    public bool OpenState { get; set; }

    [Key(2)]
    public string CharaFileName { get; set; }

    [Key(3)]
    public bool PlayEnterScene { get; set; }

    [Key(4)]
    public Dictionary<int, float> StatsTable { get; set; } = AgentData.StatsTableTemprate.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));

    [Key(5)]
    public Dictionary<int, float> DesireTable { get; set; } = AgentData.DesireTableTemprate.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));

    [Key(6)]
    public Dictionary<int, float> MotivationTable { get; set; }

    [Key(9)]
    public int ChunkID { get; set; }

    [Key(10)]
    public int AreaID { get; set; }

    [Key(11)]
    public List<int> ReservedWaypointIDList { get; set; } = new List<int>();

    [Key(12)]
    public List<int> WalkRouteWaypointIDList { get; set; } = new List<int>();

    [Key(15)]
    public int ActionTargetID { get; set; } = -1;

    [Key(16)]
    public Desire.ActionType ModeType { get; set; }

    [Key(17)]
    public Dictionary<int, int> TiredNumberTable { get; set; } = new Dictionary<int, int>();

    [Key(21)]
    public Vector3 Position { get; set; }

    [Key(22)]
    public Quaternion Rotation { get; set; }

    [Key(23)]
    public Sickness SickState { get; set; } = new Sickness();

    [Key(24)]
    public float Wetness { get; set; }

    [Key(25)]
    public List<StuffItem> ItemList { get; set; } = new List<StuffItem>();

    [Key(27)]
    public StuffItem CarryingItem { get; set; }

    [Key(28)]
    public StuffItem EquipedGloveItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(29)]
    public StuffItem EquipedShovelItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(30)]
    public StuffItem EquipedPickelItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(31)]
    public StuffItem EquipedNetItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(32)]
    public StuffItem EquipedFishingItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(33)]
    public StuffItem EquipedHeadItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(34)]
    public StuffItem EquipedBackItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(35)]
    public StuffItem EquipedNeckItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(36)]
    public StuffItem EquipedLampItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(37)]
    public StuffItem EquipedUmbrellaItem { get; set; } = new StuffItem()
    {
      CategoryID = -1,
      ID = -1
    };

    [Key(38)]
    public Dictionary<int, int> FriendlyRelationShipTable { get; set; } = AgentData.FriendlyRelationShipTableTemprate.ToDictionary<KeyValuePair<int, int>, int, int>((Func<KeyValuePair<int, int>, int>) (x => x.Key), (Func<KeyValuePair<int, int>, int>) (x => x.Value));

    [Key(39)]
    public float CallCTCount { get; set; }

    [Key(40)]
    public int CalledCount { get; set; }

    [Key(41)]
    public bool Greeted { get; set; }

    [Key(42)]
    public Tutorial.ActionType TutorialModeType { get; set; }

    [Key(43)]
    public float TalkMotivation { get; set; }

    [Key(44)]
    public int FlavorAdditionAmount { get; set; }

    [Key(45)]
    public bool CheckCatEvent { get; set; }

    [Key(46)]
    public string NowCoordinateFileName { get; set; }

    [Key(47)]
    public string BathCoordinateFileName { get; set; }

    [Key(48)]
    public bool PlayedDressIn { get; set; }

    [Key(49)]
    public Dictionary<int, Environment.SearchActionInfo> SearchActionLockTable { get; set; } = new Dictionary<int, Environment.SearchActionInfo>();

    [Key(50)]
    public bool IsOtherCoordinate { get; set; }

    [Key(51)]
    public ItemScrounge ItemScrounge { get; set; } = new ItemScrounge();

    [Key(52)]
    public bool LockTalk { get; set; }

    [Key(53)]
    public float TalkElapsedTime { get; set; }

    [Key(54)]
    public bool IsPlayerForBirthdayEvent { get; set; }

    [Key(55)]
    public Dictionary<int, HashSet<string>> advEventCheck { get; set; } = new Dictionary<int, HashSet<string>>();

    [Key(56)]
    public bool IsWet { get; set; }

    [Key(57)]
    public HashSet<int> advEventLimitation { get; set; } = new HashSet<int>();

    [Key(58)]
    public Desire.ActionType PrevMode { get; set; }

    [Key(59)]
    public int LocationCount { get; set; }

    [Key(60)]
    public int LocationTaskCount { get; set; }

    [Key(61)]
    public int CurrentActionPointID { get; set; } = -1;

    [Key(62)]
    public bool ScheduleEnabled { get; set; }

    [Key(63)]
    public float ScheduleElapsedTime { get; set; }

    [Key(64)]
    public float ScheduleDuration { get; set; }

    [Key(65)]
    public float WeaknessMotivation { get; set; }

    [Key(66)]
    public SickLockInfo ColdLockInfo { get; set; } = new SickLockInfo();

    [Key(67)]
    public SickLockInfo HeatStrokeLockInfo { get; set; } = new SickLockInfo();

    [Key(68)]
    public bool YobaiTrigger { get; set; }

    [Key(69)]
    public int MapID { get; set; }

    [Key(70)]
    public bool ParameterLock { get; set; }

    [Key(71)]
    public int? PrevMapID { get; set; }

    public static Dictionary<int, float> StatsTableTemprate
    {
      get
      {
        return new Dictionary<int, float>()
        {
          [0] = 50f,
          [1] = 50f,
          [2] = 100f,
          [3] = 100f,
          [4] = 100f,
          [5] = 100f,
          [6] = 0.0f
        };
      }
    }

    public static Dictionary<int, int> FriendlyRelationShipTableTemprate
    {
      get
      {
        return new Dictionary<int, int>()
        {
          [-99] = 50,
          [-90] = 50,
          [0] = 50,
          [1] = 50,
          [2] = 50,
          [3] = 50
        };
      }
    }

    public static Dictionary<int, float> DesireTableTemprate
    {
      get
      {
        return ((IEnumerable<int>) AgentData.DesireTableKeys).ToDictionary<int, int, float>((Func<int, int>) (x => x), (Func<int, float>) (_ => 0.0f));
      }
    }

    public static ReadOnlyDictionary<int, int> FlavorSkillTemplate { get; } = new ReadOnlyDictionary<int, int>((IDictionary<int, int>) Enumerable.Range(0, 8).ToDictionary<int, int, int>((Func<int, int>) (x => x), (Func<int, int>) (x => 0)));

    public StuffItem EquipedSearchItem(int id)
    {
      switch (id)
      {
        case 0:
        case 1:
        case 2:
          return this.EquipedGloveItem;
        case 3:
          return this.EquipedShovelItem;
        case 4:
          return this.EquipedPickelItem;
        case 5:
          return this.EquipedNetItem;
        case 6:
          return this.EquipedFishingItem;
        default:
          return (StuffItem) null;
      }
    }

    public void Init()
    {
      this.MotivationTable = ((IEnumerable<int>) AgentData.DesireTableKeys).ToDictionary<int, int, float>((Func<int, int>) (x => x), (Func<int, float>) (_ => this.StatsTable[5]));
      this.TalkMotivation = this.StatsTable[5];
    }

    public void Copy(AgentData source)
    {
      this.Version = source.Version;
      this.OpenState = source.OpenState;
      this.CharaFileName = source.CharaFileName;
      this.PlayEnterScene = source.PlayEnterScene;
      if (!this.PlayEnterScene)
        this.CharaFileName = (string) null;
      this.StatsTable = source.StatsTable.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));
      this.DesireTable = source.DesireTable.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));
      this.MotivationTable = source.MotivationTable.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));
      this.ChunkID = source.ChunkID;
      this.AreaID = source.AreaID;
      this.ReservedWaypointIDList = source.ReservedWaypointIDList.ToList<int>();
      this.WalkRouteWaypointIDList = source.WalkRouteWaypointIDList.ToList<int>();
      this.ActionTargetID = source.ActionTargetID;
      this.ModeType = source.ModeType;
      this.TiredNumberTable = source.TiredNumberTable.ToDictionary<KeyValuePair<int, int>, int, int>((Func<KeyValuePair<int, int>, int>) (x => x.Key), (Func<KeyValuePair<int, int>, int>) (x => x.Value));
      this.Position = source.Position;
      this.Rotation = source.Rotation;
      this.SickState = new Sickness(source.SickState);
      this.ItemList.Clear();
      foreach (StuffItem source1 in source.ItemList)
        this.ItemList.Add(new StuffItem(source1));
      this.CarryingItem = source.CarryingItem == null ? (StuffItem) null : new StuffItem(source.CarryingItem);
      this.EquipedGloveItem = new StuffItem(source.EquipedGloveItem);
      this.EquipedShovelItem = new StuffItem(source.EquipedShovelItem);
      this.EquipedPickelItem = new StuffItem(source.EquipedPickelItem);
      this.EquipedNetItem = new StuffItem(source.EquipedNetItem);
      this.EquipedFishingItem = new StuffItem(source.EquipedFishingItem);
      this.EquipedHeadItem = new StuffItem(source.EquipedHeadItem);
      this.EquipedBackItem = new StuffItem(source.EquipedBackItem);
      this.EquipedNeckItem = new StuffItem(source.EquipedNeckItem);
      this.EquipedLampItem = new StuffItem(source.EquipedLampItem);
      this.EquipedUmbrellaItem = new StuffItem(source.EquipedUmbrellaItem);
      this.FriendlyRelationShipTable = source.FriendlyRelationShipTable.ToDictionary<KeyValuePair<int, int>, int, int>((Func<KeyValuePair<int, int>, int>) (x => x.Key), (Func<KeyValuePair<int, int>, int>) (x => x.Value));
      this.CallCTCount = source.CallCTCount;
      this.CalledCount = source.CalledCount;
      this.Greeted = source.Greeted;
      this.TutorialModeType = source.TutorialModeType;
      this.TalkMotivation = source.TalkMotivation;
      this.FlavorAdditionAmount = source.FlavorAdditionAmount;
      this.CheckCatEvent = source.CheckCatEvent;
      this.NowCoordinateFileName = source.NowCoordinateFileName;
      this.BathCoordinateFileName = source.BathCoordinateFileName;
      this.PlayedDressIn = source.PlayedDressIn;
      this.SearchActionLockTable = source.SearchActionLockTable.ToDictionary<KeyValuePair<int, Environment.SearchActionInfo>, int, Environment.SearchActionInfo>((Func<KeyValuePair<int, Environment.SearchActionInfo>, int>) (x => x.Key), (Func<KeyValuePair<int, Environment.SearchActionInfo>, Environment.SearchActionInfo>) (x => x.Value));
      this.IsOtherCoordinate = source.IsOtherCoordinate;
      this.ItemScrounge = new ItemScrounge(source.ItemScrounge);
      this.LockTalk = source.LockTalk;
      this.TalkElapsedTime = source.TalkElapsedTime;
      this.IsPlayerForBirthdayEvent = source.IsPlayerForBirthdayEvent;
      if (source.advEventCheck == null)
        source.advEventCheck = new Dictionary<int, HashSet<string>>();
      this.advEventCheck = source.advEventCheck.ToDictionary<KeyValuePair<int, HashSet<string>>, int, HashSet<string>>((Func<KeyValuePair<int, HashSet<string>>, int>) (v => v.Key), (Func<KeyValuePair<int, HashSet<string>>, HashSet<string>>) (v => new HashSet<string>((IEnumerable<string>) v.Value)));
      this.IsWet = source.IsWet;
      if (source.advEventLimitation == null)
        source.advEventLimitation = new HashSet<int>();
      this.advEventLimitation = new HashSet<int>((IEnumerable<int>) source.advEventLimitation);
      this.PrevMode = source.PrevMode;
      this.CurrentActionPointID = source.CurrentActionPointID;
      this.ScheduleEnabled = source.ScheduleEnabled;
      this.ScheduleElapsedTime = source.ScheduleElapsedTime;
      this.ScheduleDuration = source.ScheduleDuration;
      this.WeaknessMotivation = source.WeaknessMotivation;
      if (source.ColdLockInfo != null)
        this.ColdLockInfo = new SickLockInfo()
        {
          Lock = source.ColdLockInfo.Lock,
          ElapsedTime = source.ColdLockInfo.ElapsedTime
        };
      if (source.HeatStrokeLockInfo != null)
        this.HeatStrokeLockInfo = new SickLockInfo()
        {
          Lock = source.HeatStrokeLockInfo.Lock,
          ElapsedTime = source.HeatStrokeLockInfo.ElapsedTime
        };
      this.MapID = source.MapID;
      this.ParameterLock = source.ParameterLock;
      this.PrevMapID = source.PrevMapID;
    }

    public void ComplementDiff()
    {
      if (this.Version < new System.Version("0.0.1"))
      {
        this.ModeType = Desire.ActionType.Normal;
        this.CurrentActionPointID = -1;
      }
      this.Version = AIProject.Definitions.Version.AgentDataVersion;
    }

    public void UpdateDiff()
    {
      foreach (int key in AgentData.StatsTableTemprate.Keys)
      {
        if (!this.StatsTable.ContainsKey(key))
          this.StatsTable[key] = AgentData.StatsTableTemprate[key];
      }
      foreach (int key in AgentData.DesireTableTemprate.Keys)
      {
        if (this.DesireTable.TryGetValue(key, out float _))
          this.DesireTable[key] = AgentData.DesireTableTemprate[key];
      }
      this.ItemList.OrganizeItemList();
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      List<CommandData> list = new List<CommandData>();
      string str1 = head + "DesireTable";
      foreach (KeyValuePair<int, float> keyValuePair in this.DesireTable)
      {
        KeyValuePair<int, float> item = keyValuePair;
        AgentData agentData = this;
        int key = item.Key;
        list.Add(new CommandData(CommandData.Command.FLOAT, str1 + string.Format("[{0}]", (object) key), (Func<object>) (() => (object) item.Value), (System.Action<object>) (o => closure_0.DesireTable[key] = Mathf.Clamp((float) o, 0.0f, 100f))));
      }
      string head1 = head + "SickState";
      this.SickState.AddList(list, head1);
      if (Object.op_Inequality((Object) this.param.actor, (Object) null) && Object.op_Inequality((Object) this.param.actor.ChaControl, (Object) null) && this.param.actor.ChaControl.fileGameInfo != null)
      {
        string str2 = head + "StatsTable";
        foreach (KeyValuePair<int, float> keyValuePair in this.StatsTable)
        {
          KeyValuePair<int, float> item = keyValuePair;
          int key = item.Key;
          list.Add(new CommandData(CommandData.Command.FLOAT, str2 + string.Format("[{0}]", (object) key), (Func<object>) (() => item.Key == 7 ? (object) this.param.actor.ChaControl.fileGameInfo.morality : (object) item.Value), (System.Action<object>) (o =>
          {
            AgentActor actor = this.param.actor as AgentActor;
            if (!Object.op_Inequality((Object) actor, (Object) null))
              return;
            float num1 = (float) o;
            if (key == 3)
            {
              StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
              int flavor = actor.ChaControl.fileGameInfo.flavorState[6];
              float num2 = AgentActor.FlavorVariation(statusProfile.DarknessPhysicalBuffMinMax, statusProfile.DarknessPhysicalBuff, flavor);
              num1 += num2;
            }
            actor.SetStatus(key, num1);
          })));
        }
        ChaFileGameInfo fileGameInfo = this.param.actor.ChaControl.fileGameInfo;
        ChaFileGameInfo.MinMaxInfo tempBound = fileGameInfo.tempBound;
        string str3 = head + "tempBound";
        list.Add(new CommandData(CommandData.Command.FLOAT, str3 + ".low", (Func<object>) (() => (object) tempBound.lower), (System.Action<object>) null));
        list.Add(new CommandData(CommandData.Command.FLOAT, str3 + ".high", (Func<object>) (() => (object) tempBound.upper), (System.Action<object>) null));
        ChaFileGameInfo.MinMaxInfo MoodBound = fileGameInfo.moodBound;
        string str4 = head + "MoodBound";
        list.Add(new CommandData(CommandData.Command.FLOAT, str4 + ".low", (Func<object>) (() => (object) MoodBound.lower), (System.Action<object>) null));
        list.Add(new CommandData(CommandData.Command.FLOAT, str4 + ".high", (Func<object>) (() => (object) MoodBound.upper), (System.Action<object>) null));
        Dictionary<int, int> flavorState = fileGameInfo.flavorState;
        string str5 = head + "FlavorSkillTable";
        foreach (KeyValuePair<int, int> keyValuePair in flavorState)
        {
          KeyValuePair<int, int> item = keyValuePair;
          list.Add(new CommandData(CommandData.Command.Int, str5 + string.Format("[{0}]", (object) item.Key), (Func<object>) (() => (object) item.Value), (System.Action<object>) (o => this.SetFlavorSkill(item.Key, (int) o))));
        }
        string key1 = string.Format("{0}{1}", (object) head, (object) "TalkMotivation");
        list.Add(new CommandData(CommandData.Command.FLOAT, key1, (Func<object>) (() => (object) this.TalkMotivation), (System.Action<object>) (o =>
        {
          float num = this.StatsTable[5];
          this.TalkMotivation = Mathf.Clamp((float) o, 0.0f, num);
        })));
        string key2 = string.Format("{0}InstructProb", (object) head);
        list.Add(new CommandData(CommandData.Command.FLOAT, key2, (Func<object>) (() =>
        {
          int num1 = fileGameInfo.flavorState[1];
          StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
          float defaultInstructionRate = statusProfile.DefaultInstructionRate;
          Threshold instructionMinMax = statusProfile.FlavorReliabilityInstructionMinMax;
          float t = Mathf.InverseLerp(instructionMinMax.min, instructionMinMax.max, (float) num1);
          float num2 = defaultInstructionRate + statusProfile.FlavorReliabilityInstruction.Lerp(t);
          if (fileGameInfo.normalSkill.ContainsValue(27))
            num2 += statusProfile.InstructionRateDebuff;
          return (object) num2;
        }), (System.Action<object>) null));
        string key3 = string.Format("{0}FollowProb", (object) head);
        list.Add(new CommandData(CommandData.Command.FLOAT, key3, (Func<object>) (() =>
        {
          int num1 = fileGameInfo.flavorState[1];
          StatusProfile statusProfile = Singleton<Resources>.Instance.StatusProfile;
          float defaultFollowRate = statusProfile.DefaultFollowRate;
          float t = Mathf.InverseLerp(statusProfile.FollowReliabilityMinMax.min, statusProfile.FollowReliabilityMinMax.max, (float) num1);
          float num2 = defaultFollowRate + statusProfile.FollowRateReliabilityBuff.Lerp(t);
          if (fileGameInfo.normalSkill.ContainsValue(8))
            num2 += statusProfile.FollowRateBuff;
          return (object) num2;
        }), (System.Action<object>) null));
        int TotalFlavor = fileGameInfo.totalFlavor;
        string key4 = head + "TotalFlavor";
        list.Add(new CommandData(CommandData.Command.Int, key4, (Func<object>) (() => (object) TotalFlavor), (System.Action<object>) null));
        Dictionary<int, float> desireDefVal = fileGameInfo.desireDefVal;
        string str6 = head + "DesireDef";
        foreach (KeyValuePair<int, float> keyValuePair in desireDefVal)
        {
          KeyValuePair<int, float> item = keyValuePair;
          int desireKey = Desire.GetDesireKey((Desire.Type) item.Key);
          list.Add(new CommandData(CommandData.Command.FLOAT, str6 + string.Format("[{0}]", (object) desireKey), (Func<object>) (() => (object) item.Value), (System.Action<object>) null));
        }
        int Phase = fileGameInfo.phase;
        string key5 = head + "Phase";
        list.Add(new CommandData(CommandData.Command.Int, key5, (Func<object>) (() => (object) Phase), (System.Action<object>) null));
        Dictionary<int, int> NormalSkill = fileGameInfo.normalSkill;
        string str7 = head + "NormalSkill";
        foreach (KeyValuePair<int, int> keyValuePair in NormalSkill)
        {
          KeyValuePair<int, int> item = keyValuePair;
          list.Add(new CommandData(CommandData.Command.Int, str7 + string.Format("[{0}]", (object) item.Key), (Func<object>) (() => (object) item.Value), (System.Action<object>) (o => NormalSkill[item.Key] = (int) o)));
        }
        Dictionary<int, int> HSkill = fileGameInfo.hSkill;
        string str8 = head + "HSkill";
        foreach (KeyValuePair<int, int> keyValuePair in HSkill)
        {
          KeyValuePair<int, int> item = keyValuePair;
          list.Add(new CommandData(CommandData.Command.Int, str8 + string.Format("[{0}]", (object) item.Key), (Func<object>) (() => (object) item.Value), (System.Action<object>) (o => HSkill[item.Key] = (int) o)));
        }
        int FavoritePlace = fileGameInfo.favoritePlace;
        string key6 = head + "FavoritePlace";
        list.Add(new CommandData(CommandData.Command.Int, key6, (Func<object>) (() => (object) FavoritePlace), (System.Action<object>) null));
      }
      return (IEnumerable<CommandData>) list;
    }

    public void AddFlavorSkill(int id, int value)
    {
      this.AddFlavorAdditionAmount(value);
    }

    public void AddFlavorAdditionAmount(int value)
    {
      this.FlavorAdditionAmount = Mathf.Max(this.FlavorAdditionAmount + value, 0);
      if (!Singleton<Game>.IsInstance())
        return;
      Environment environment = Singleton<Game>.Instance.Environment;
      if (environment == null)
        return;
      int num = environment.TotalAgentFlavorAdditionAmount + value;
      environment.TotalAgentFlavorAdditionAmount = Mathf.Clamp(num, 0, 99999);
    }

    public void SetFlavorSkill(int id, int value)
    {
      ChaFileGameInfo fileGameInfo = this.param.actor.ChaControl.fileGameInfo;
      if (!fileGameInfo.flavorState.ContainsKey(id))
        return;
      int num1 = value - fileGameInfo.flavorState[id];
      fileGameInfo.flavorState[id] = Mathf.Clamp(value, 0, 99999);
      int num2 = fileGameInfo.totalFlavor + num1;
      fileGameInfo.totalFlavor = Mathf.Max(num2, 0);
      this.AddFlavorAdditionAmount(num1);
    }

    public HashSet<string> GetAdvEventCheck(int category)
    {
      if (this.advEventCheck == null)
        this.advEventCheck = new Dictionary<int, HashSet<string>>();
      HashSet<string> stringSet;
      if (!this.advEventCheck.TryGetValue(category, out stringSet))
        this.advEventCheck[category] = stringSet = new HashSet<string>();
      return stringSet;
    }
  }
}
