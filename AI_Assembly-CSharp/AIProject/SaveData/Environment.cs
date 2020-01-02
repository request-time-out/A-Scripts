// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.Environment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class Environment : IDiffComparer, ICommandData
  {
    public Environment()
    {
      this.Init();
    }

    [Key(0)]
    public System.Version Version { get; set; } = new System.Version();

    [Key(1)]
    public int TutorialProgress { get; set; }

    [Key(2)]
    public Environment.SerializableTimeSpan TotalPlayTime { get; set; } = new Environment.SerializableTimeSpan();

    [Key(3)]
    public Environment.SerializableDateTime Time { get; set; } = new Environment.SerializableDateTime();

    [Key(5)]
    public Weather Weather { get; set; } = Weather.Cloud1;

    [Key(7)]
    public Temperature Temperature { get; set; } = Temperature.Normal;

    [Key(8)]
    public List<StuffItem> ItemListInStorage { get; set; } = new List<StuffItem>();

    [Key(9)]
    public List<StuffItem> ItemListInPantry { get; set; } = new List<StuffItem>();

    [Key(10)]
    public Dictionary<int, bool> AreaOpenState { get; set; } = new Dictionary<int, bool>();

    [Key(11)]
    public float TemperatureValue { get; set; }

    [Key(12)]
    public Dictionary<int, bool> TimeObjOpenState { get; set; } = new Dictionary<int, bool>();

    [Key(13)]
    public Dictionary<int, Dictionary<int, bool>> BasePointOpenState { get; set; } = new Dictionary<int, Dictionary<int, bool>>();

    [Key(14)]
    public Dictionary<int, bool> LightObjectSwitchStateTable { get; set; } = new Dictionary<int, bool>();

    [Key(15)]
    public Dictionary<int, Dictionary<int, int>> EventPointStateTable { get; set; } = new Dictionary<int, Dictionary<int, int>>();

    [Key(16)]
    public Dictionary<int, List<Environment.PlantInfo>> FarmlandTable { get; set; } = new Dictionary<int, List<Environment.PlantInfo>>();

    [Key(17)]
    public Dictionary<int, Environment.SearchActionInfo> SearchActionLockTable { get; set; } = new Dictionary<int, Environment.SearchActionInfo>();

    [Key(18)]
    public Dictionary<int, List<Environment.ChickenInfo>> ChickenTable { get; set; } = new Dictionary<int, List<Environment.ChickenInfo>>();

    [Key(19)]
    public List<StuffItem> ItemListInEggBox { get; set; } = new List<StuffItem>();

    [Key(20)]
    public int TotalAgentFlavorAdditionAmount { get; set; }

    [Key(21)]
    public List<int> RegIDList { get; set; } = new List<int>();

    [Key(22)]
    public Dictionary<int, Environment.PetHomeInfo> PetHomeStateTable { get; set; } = new Dictionary<int, Environment.PetHomeInfo>();

    [Key(23)]
    public List<string> ClosetCoordinateList { get; set; } = new List<string>();

    [Key(24)]
    public List<string> DressCoordinateList { get; set; } = new List<string>();

    [Key(25)]
    public Dictionary<int, string> JukeBoxAudioNameTable { get; set; } = new Dictionary<int, string>();

    [Key(28)]
    public Dictionary<int, bool> OnceActionPointStateTable { get; set; } = new Dictionary<int, bool>();

    [Key(29)]
    public Dictionary<int, float> DropSearchActionPointCoolTimeTable { get; set; } = new Dictionary<int, float>();

    [Key(31)]
    public Dictionary<int, Dictionary<int, string>> AnotherJukeBoxAudioNameTable { get; set; } = new Dictionary<int, Dictionary<int, string>>();

    [Key(32)]
    public Dictionary<int, Dictionary<int, Dictionary<int, AnimalData>>> HousingChickenDataTable { get; set; } = new Dictionary<int, Dictionary<int, Dictionary<int, AnimalData>>>();

    [Key(33)]
    public Dictionary<int, RecyclingData> RecyclingDataTable { get; set; } = new Dictionary<int, RecyclingData>();

    public void Init()
    {
      this.BasePointOpenState = new Dictionary<int, Dictionary<int, bool>>()
      {
        [0] = new Dictionary<int, bool>()
        {
          [-1] = false,
          [0] = false,
          [1] = false,
          [2] = false
        }
      };
    }

    public void Copy(Environment source)
    {
      this.Version = source.Version;
      this.TutorialProgress = source.TutorialProgress;
      this.TotalPlayTime = source.TotalPlayTime;
      this.Time = source.Time;
      this.Weather = source.Weather;
      this.Temperature = source.Temperature;
      this.ItemListInStorage.Clear();
      foreach (StuffItem stuffItem in source.ItemListInStorage)
        this.ItemListInStorage.Add(new StuffItem(stuffItem.CategoryID, stuffItem.ID, stuffItem.Count));
      this.ItemListInPantry.Clear();
      foreach (StuffItem stuffItem in source.ItemListInPantry)
        this.ItemListInPantry.Add(new StuffItem(stuffItem.CategoryID, stuffItem.ID, stuffItem.Count));
      this.AreaOpenState = source.AreaOpenState.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (x => x.Key), (Func<KeyValuePair<int, bool>, bool>) (x => x.Value));
      this.TemperatureValue = source.TemperatureValue;
      this.TimeObjOpenState = source.TimeObjOpenState.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (x => x.Key), (Func<KeyValuePair<int, bool>, bool>) (x => x.Value));
      this.BasePointOpenState = source.BasePointOpenState.ToDictionary<KeyValuePair<int, Dictionary<int, bool>>, int, Dictionary<int, bool>>((Func<KeyValuePair<int, Dictionary<int, bool>>, int>) (x => x.Key), (Func<KeyValuePair<int, Dictionary<int, bool>>, Dictionary<int, bool>>) (x =>
      {
        Dictionary<int, bool> source1 = x.Value;
        return source1 == null ? (Dictionary<int, bool>) null : source1.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (y => y.Key), (Func<KeyValuePair<int, bool>, bool>) (y => y.Value));
      }));
      this.LightObjectSwitchStateTable = source.LightObjectSwitchStateTable.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (x => x.Key), (Func<KeyValuePair<int, bool>, bool>) (x => x.Value));
      this.EventPointStateTable = source.EventPointStateTable.ToDictionary<KeyValuePair<int, Dictionary<int, int>>, int, Dictionary<int, int>>((Func<KeyValuePair<int, Dictionary<int, int>>, int>) (x => x.Key), (Func<KeyValuePair<int, Dictionary<int, int>>, Dictionary<int, int>>) (x =>
      {
        Dictionary<int, int> source1 = x.Value;
        return source1 == null ? (Dictionary<int, int>) null : source1.ToDictionary<KeyValuePair<int, int>, int, int>((Func<KeyValuePair<int, int>, int>) (y => y.Key), (Func<KeyValuePair<int, int>, int>) (y => y.Value));
      }));
      this.FarmlandTable = source.FarmlandTable.ToDictionary<KeyValuePair<int, List<Environment.PlantInfo>>, int, List<Environment.PlantInfo>>((Func<KeyValuePair<int, List<Environment.PlantInfo>>, int>) (x => x.Key), (Func<KeyValuePair<int, List<Environment.PlantInfo>>, List<Environment.PlantInfo>>) (x =>
      {
        List<Environment.PlantInfo> source1 = x.Value;
        if (source1 == null)
          return (List<Environment.PlantInfo>) null;
        IEnumerable<Environment.PlantInfo> source2 = source1.Select<Environment.PlantInfo, Environment.PlantInfo>((Func<Environment.PlantInfo, Environment.PlantInfo>) (y => y != null ? new Environment.PlantInfo(y.nameHash, y.timeLimit, y.timer, y.result) : (Environment.PlantInfo) null));
        return source2 == null ? (List<Environment.PlantInfo>) null : source2.ToList<Environment.PlantInfo>();
      }));
      this.SearchActionLockTable = source.SearchActionLockTable.ToDictionary<KeyValuePair<int, Environment.SearchActionInfo>, int, Environment.SearchActionInfo>((Func<KeyValuePair<int, Environment.SearchActionInfo>, int>) (x => x.Key), (Func<KeyValuePair<int, Environment.SearchActionInfo>, Environment.SearchActionInfo>) (x => x.Value));
      this.ChickenTable = source.ChickenTable.ToDictionary<KeyValuePair<int, List<Environment.ChickenInfo>>, int, List<Environment.ChickenInfo>>((Func<KeyValuePair<int, List<Environment.ChickenInfo>>, int>) (x => x.Key), (Func<KeyValuePair<int, List<Environment.ChickenInfo>>, List<Environment.ChickenInfo>>) (x =>
      {
        List<Environment.ChickenInfo> source1 = x.Value;
        if (source1 == null)
          return (List<Environment.ChickenInfo>) null;
        IEnumerable<Environment.ChickenInfo> source2 = source1.Select<Environment.ChickenInfo, Environment.ChickenInfo>((Func<Environment.ChickenInfo, Environment.ChickenInfo>) (y =>
        {
          if (y == null)
            return (Environment.ChickenInfo) null;
          return new Environment.ChickenInfo()
          {
            name = y.name,
            AnimalData = y.AnimalData == null ? (AnimalData) null : new AnimalData(y.AnimalData)
          };
        }));
        return source2 == null ? (List<Environment.ChickenInfo>) null : source2.ToList<Environment.ChickenInfo>();
      }));
      this.ItemListInEggBox.Clear();
      foreach (StuffItem stuffItem in source.ItemListInEggBox)
        this.ItemListInEggBox.Add(new StuffItem(stuffItem.CategoryID, stuffItem.ID, stuffItem.Count));
      this.TotalAgentFlavorAdditionAmount = source.TotalAgentFlavorAdditionAmount;
      this.RegIDList = source.RegIDList.ToList<int>();
      this.PetHomeStateTable = source.PetHomeStateTable.ToDictionary<KeyValuePair<int, Environment.PetHomeInfo>, int, Environment.PetHomeInfo>((Func<KeyValuePair<int, Environment.PetHomeInfo>, int>) (x => x.Key), (Func<KeyValuePair<int, Environment.PetHomeInfo>, Environment.PetHomeInfo>) (x => new Environment.PetHomeInfo(x.Value)));
      this.ClosetCoordinateList = source.ClosetCoordinateList.ToList<string>();
      this.DressCoordinateList = source.DressCoordinateList.ToList<string>();
      this.JukeBoxAudioNameTable = source.JukeBoxAudioNameTable.Where<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (pair => !pair.Value.IsNullOrEmpty())).ToDictionary<KeyValuePair<int, string>, int, string>((Func<KeyValuePair<int, string>, int>) (x => x.Key), (Func<KeyValuePair<int, string>, string>) (x => x.Value));
      if (!source.OnceActionPointStateTable.IsNullOrEmpty<int, bool>())
        this.OnceActionPointStateTable = source.OnceActionPointStateTable.ToDictionary<KeyValuePair<int, bool>, int, bool>((Func<KeyValuePair<int, bool>, int>) (x => x.Key), (Func<KeyValuePair<int, bool>, bool>) (x => x.Value));
      else
        this.OnceActionPointStateTable.Clear();
      if (!source.DropSearchActionPointCoolTimeTable.IsNullOrEmpty<int, float>())
        this.DropSearchActionPointCoolTimeTable = source.DropSearchActionPointCoolTimeTable.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));
      else
        this.DropSearchActionPointCoolTimeTable.Clear();
      if (!source.AnotherJukeBoxAudioNameTable.IsNullOrEmpty<int, Dictionary<int, string>>())
        this.AnotherJukeBoxAudioNameTable = source.AnotherJukeBoxAudioNameTable.Where<KeyValuePair<int, Dictionary<int, string>>>((Func<KeyValuePair<int, Dictionary<int, string>>, bool>) (x => !x.Value.IsNullOrEmpty<int, string>())).ToDictionary<KeyValuePair<int, Dictionary<int, string>>, int, Dictionary<int, string>>((Func<KeyValuePair<int, Dictionary<int, string>>, int>) (x => x.Key), (Func<KeyValuePair<int, Dictionary<int, string>>, Dictionary<int, string>>) (x => x.Value.Where<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (y => !y.Value.IsNullOrEmpty())).ToDictionary<KeyValuePair<int, string>, int, string>((Func<KeyValuePair<int, string>, int>) (y => y.Key), (Func<KeyValuePair<int, string>, string>) (y => y.Value))));
      else
        this.AnotherJukeBoxAudioNameTable.Clear();
      if (!source.HousingChickenDataTable.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, AnimalData>>>())
        this.HousingChickenDataTable = source.HousingChickenDataTable.Where<KeyValuePair<int, Dictionary<int, Dictionary<int, AnimalData>>>>((Func<KeyValuePair<int, Dictionary<int, Dictionary<int, AnimalData>>>, bool>) (a => !a.Value.IsNullOrEmpty<int, Dictionary<int, AnimalData>>())).ToDictionary<KeyValuePair<int, Dictionary<int, Dictionary<int, AnimalData>>>, int, Dictionary<int, Dictionary<int, AnimalData>>>((Func<KeyValuePair<int, Dictionary<int, Dictionary<int, AnimalData>>>, int>) (a => a.Key), (Func<KeyValuePair<int, Dictionary<int, Dictionary<int, AnimalData>>>, Dictionary<int, Dictionary<int, AnimalData>>>) (a => a.Value.Where<KeyValuePair<int, Dictionary<int, AnimalData>>>((Func<KeyValuePair<int, Dictionary<int, AnimalData>>, bool>) (b => !b.Value.IsNullOrEmpty<int, AnimalData>())).ToDictionary<KeyValuePair<int, Dictionary<int, AnimalData>>, int, Dictionary<int, AnimalData>>((Func<KeyValuePair<int, Dictionary<int, AnimalData>>, int>) (b => b.Key), (Func<KeyValuePair<int, Dictionary<int, AnimalData>>, Dictionary<int, AnimalData>>) (b => b.Value.Where<KeyValuePair<int, AnimalData>>((Func<KeyValuePair<int, AnimalData>, bool>) (c => c.Value != null)).ToDictionary<KeyValuePair<int, AnimalData>, int, AnimalData>((Func<KeyValuePair<int, AnimalData>, int>) (c => c.Key), (Func<KeyValuePair<int, AnimalData>, AnimalData>) (c => new AnimalData(c.Value)))))));
      else
        this.HousingChickenDataTable.Clear();
      if (!source.RecyclingDataTable.IsNullOrEmpty<int, RecyclingData>())
        this.RecyclingDataTable = source.RecyclingDataTable.Where<KeyValuePair<int, RecyclingData>>((Func<KeyValuePair<int, RecyclingData>, bool>) (x => x.Value != null)).ToDictionary<KeyValuePair<int, RecyclingData>, int, RecyclingData>((Func<KeyValuePair<int, RecyclingData>, int>) (x => x.Key), (Func<KeyValuePair<int, RecyclingData>, RecyclingData>) (x => new RecyclingData(x.Value)));
      else
        this.RecyclingDataTable.Clear();
    }

    public void ComplementWithVersion()
    {
      this.Version = AIProject.Definitions.Version.EnvironmentDataVersion;
    }

    public void SetSimulation(EnvironmentSimulator sim)
    {
      this.Time = (Environment.SerializableDateTime) sim.Now;
      this.Weather = sim.Weather;
      this.Temperature = sim.Temperature;
      this.TemperatureValue = sim.TemperatureValue;
    }

    public void UpdateDiff()
    {
      this.ItemListInStorage.OrganizeItemList();
    }

    public IEnumerable<CommandData> CreateCommandData(string head)
    {
      List<CommandData> commandDataList = new List<CommandData>();
      string str = head + "AreaOpenState";
      foreach (KeyValuePair<int, bool> keyValuePair in this.AreaOpenState)
      {
        int key = keyValuePair.Key;
        bool value = keyValuePair.Value;
        commandDataList.Add(new CommandData(CommandData.Command.BOOL, str + string.Format("[{0}]", (object) key), (Func<object>) (() => (object) value), (System.Action<object>) (o =>
        {
          bool active = (bool) o;
          if (Singleton<Manager.Map>.IsInstance())
            Singleton<Manager.Map>.Instance.SetOpenAreaState(key, active);
          else
            this.AreaOpenState[key] = active;
        })));
      }
      return (IEnumerable<CommandData>) commandDataList;
    }

    [MessagePackObject(false)]
    public struct SerializableDateTime : ICommandData
    {
      private int _year;
      private int _month;
      private int _day;

      public SerializableDateTime(int year, int month, int day)
        : this(year, month, day, 0, 0, 0, 0)
      {
      }

      public SerializableDateTime(
        int year,
        int month,
        int day,
        int hour,
        int minute,
        int second)
        : this(year, month, day, hour, minute, second, 0)
      {
      }

      public SerializableDateTime(
        int year,
        int month,
        int day,
        int hour,
        int minute,
        int second,
        int millisecond)
      {
        this._year = Mathf.Max(0, year);
        this._month = Mathf.Max(0, month);
        this._day = Mathf.Max(0, day);
        this.Hour = hour;
        this.Minute = minute;
        this.Second = second;
        this.Millisecond = millisecond;
      }

      [Key(0)]
      public int Year
      {
        get
        {
          return this.SetYear(this._year);
        }
        set
        {
          this.SetYear(value);
        }
      }

      private int SetYear(int year)
      {
        return this._year = Mathf.Max(1, year);
      }

      [Key(1)]
      public int Month
      {
        get
        {
          return this.SetMonth(this._month);
        }
        set
        {
          this.SetMonth(value);
        }
      }

      private int SetMonth(int month)
      {
        return this._month = Mathf.Max(1, month);
      }

      [Key(2)]
      public int Day
      {
        get
        {
          return this.SetDay(this._day);
        }
        set
        {
          this.SetDay(value);
        }
      }

      private int SetDay(int day)
      {
        return this._day = Mathf.Max(1, day);
      }

      [Key(3)]
      public int Hour { get; set; }

      [Key(4)]
      public int Minute { get; set; }

      [Key(5)]
      public int Second { get; set; }

      [Key(6)]
      public int Millisecond { get; set; }

      [IgnoreMember]
      public DateTime DateTime
      {
        get
        {
          return new DateTime(this.Year, this.Month, this.Day, this.Hour, this.Minute, this.Second, this.Millisecond);
        }
      }

      public static implicit operator Environment.SerializableDateTime(DateTime dateTime)
      {
        return new Environment.SerializableDateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
      }

      public IEnumerable<CommandData> CreateCommandData(string head)
      {
        Environment.SerializableDateTime s = this;
        return (IEnumerable<CommandData>) new CommandData[1]
        {
          new CommandData(CommandData.Command.String, head + string.Format("[{0}]", (object) "DateTime"), (Func<object>) (() => (object) s.DateTime.ToString()), (System.Action<object>) null)
        };
      }
    }

    [MessagePackObject(false)]
    public struct SerializableTimeSpan : ICommandData
    {
      private int _days;
      private int _hours;
      private int _minutes;
      private int _seconds;
      private int _milliseconds;

      public SerializableTimeSpan(
        int days,
        int hours,
        int minutes,
        int seconds,
        int milliseconds)
      {
        this._days = Mathf.Max(0, days);
        this._hours = Mathf.Max(0, hours);
        this._minutes = Mathf.Max(0, minutes);
        this._seconds = Mathf.Max(0, seconds);
        this._milliseconds = Mathf.Max(0, milliseconds);
      }

      public SerializableTimeSpan(int days, int hours, int minutes, int seconds)
      {
        this._days = Mathf.Max(0, days);
        this._hours = Mathf.Max(0, hours);
        this._minutes = Mathf.Max(0, minutes);
        this._seconds = Mathf.Max(0, seconds);
        this._milliseconds = Mathf.Max(0, 0);
      }

      [Key(0)]
      public int Days
      {
        get
        {
          return this.SetDays(this._days);
        }
        set
        {
          this.SetDays(value);
        }
      }

      private int SetDays(int days)
      {
        return this._days = Mathf.Max(0, days);
      }

      [Key(1)]
      public int Hours
      {
        get
        {
          return this.SetHours(this._hours);
        }
        set
        {
          this.SetHours(value);
        }
      }

      private int SetHours(int hours)
      {
        return this._hours = Mathf.Max(0, hours);
      }

      [Key(2)]
      public int Minutes
      {
        get
        {
          return this.SetMinutes(this._minutes);
        }
        set
        {
          this.SetMinutes(value);
        }
      }

      private int SetMinutes(int minutes)
      {
        return this._minutes = Mathf.Max(0, minutes);
      }

      [Key(3)]
      public int Seconds
      {
        get
        {
          return this.SetSeconds(this._seconds);
        }
        set
        {
          this.SetSeconds(value);
        }
      }

      private int SetSeconds(int seconds)
      {
        return this._seconds = Mathf.Max(0, this._seconds);
      }

      [Key(4)]
      public int MilliSeconds
      {
        get
        {
          return this.SetMilliseconds(this._milliseconds);
        }
        set
        {
          this.SetMilliseconds(value);
        }
      }

      private int SetMilliseconds(int milliseconds)
      {
        return this._milliseconds = Mathf.Max(0, this._milliseconds);
      }

      [IgnoreMember]
      public TimeSpan TimeSpan
      {
        get
        {
          return new TimeSpan(this.Days, this.Hours, this.Minutes, this.Seconds, this.MilliSeconds);
        }
      }

      public static implicit operator Environment.SerializableTimeSpan(TimeSpan timeSpan)
      {
        return new Environment.SerializableTimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
      }

      public IEnumerable<CommandData> CreateCommandData(string head)
      {
        Environment.SerializableTimeSpan s = this;
        return (IEnumerable<CommandData>) new CommandData[1]
        {
          new CommandData(CommandData.Command.String, head + string.Format("[{0}]", (object) "TimeSpan"), (Func<object>) (() => (object) s.TimeSpan.ToString()), (System.Action<object>) null)
        };
      }
    }

    [MessagePackObject(false)]
    public class ScheduleData
    {
      public ScheduleData()
      {
      }

      public ScheduleData(Environment.ScheduleData source)
      {
        this.DaysToGo = source.DaysToGo;
        this.Event = source.Event;
      }

      [Key(0)]
      public int DaysToGo { get; set; }

      [Key(1)]
      public Environment.SerializableDateTime StartTime { get; set; }

      [Key(2)]
      public Environment.SerializableTimeSpan Duration { get; set; }

      [Key(3)]
      public Environment.SchedulingEvent Event { get; set; }
    }

    [MessagePackObject(false)]
    public struct SchedulingEvent
    {
      [Key(0)]
      public int agentID;
      [Key(1)]
      public int pointID;

      public SchedulingEvent(int aID, int pID)
      {
        this.agentID = aID;
        this.pointID = pID;
      }
    }

    [MessagePackObject(false)]
    public class SearchActionInfo
    {
      [Key(0)]
      public int Count { get; set; }

      [Key(1)]
      public float ElapsedTime { get; set; }
    }

    [MessagePackObject(false)]
    public class PlantInfo
    {
      public PlantInfo(int nameHash, int timeLimit, StuffItem[] result)
      {
        this.nameHash = nameHash;
        this.timeLimit = timeLimit;
        this.result = result;
      }

      [SerializationConstructor]
      public PlantInfo(int nameHash, int timeLimit, float timer, StuffItem[] result)
      {
        this.nameHash = nameHash;
        this.timeLimit = timeLimit;
        this.timer = timer;
        this.result = ((IEnumerable<StuffItem>) result).Select<StuffItem, StuffItem>((Func<StuffItem, StuffItem>) (x => new StuffItem(x))).ToArray<StuffItem>();
      }

      [Key(0)]
      public int nameHash { get; }

      [Key(1)]
      public int timeLimit { get; }

      [Key(2)]
      public float timer { get; private set; }

      [Key(3)]
      public StuffItem[] result { get; }

      [IgnoreMember]
      public float progress
      {
        get
        {
          return Mathf.InverseLerp(0.0f, (float) this.timeLimit, this.timer);
        }
      }

      [IgnoreMember]
      public bool isEnd
      {
        get
        {
          return (double) this.timer >= (double) this.timeLimit;
        }
      }

      public void AddTimer(float add)
      {
        this.timer = Mathf.Min(this.timer + add, (float) this.timeLimit);
      }

      public void Finish()
      {
        this.timer = (float) this.timeLimit;
      }

      public override string ToString()
      {
        int num1 = this.timeLimit - (int) this.timer;
        int num2 = num1 / 3600;
        int num3 = num1 % 3600;
        int num4 = num3 / 60;
        int num5 = num3 % 60;
        string empty = string.Empty;
        if (num2 > 0)
          empty += string.Format("{0}", (object) num2);
        return empty + string.Format("{0:00}:{1:00}", (object) num4, (object) num5);
      }
    }

    [MessagePackObject(false)]
    public class ChickenInfo
    {
      [Key(0)]
      public string name = string.Empty;
      [Key(1)]
      public AnimalData AnimalData;
    }

    [MessagePackObject(false)]
    public class PetHomeInfo
    {
      public PetHomeInfo()
      {
      }

      public PetHomeInfo(Environment.PetHomeInfo source)
      {
        this.Copy(source);
      }

      [Key(0)]
      public int HousingID { get; set; }

      [Key(1)]
      public AnimalData AnimalData { get; set; }

      [Key(2)]
      public bool ChaseActor { get; set; }

      [Key(3)]
      public bool NicknameDisplay { get; set; }

      public void Copy(Environment.PetHomeInfo source)
      {
        if (source == null)
          return;
        this.HousingID = source.HousingID;
        if (source.AnimalData != null)
        {
          this.AnimalData = new AnimalData();
          this.AnimalData.Copy(source.AnimalData);
        }
        this.ChaseActor = source.ChaseActor;
        this.NicknameDisplay = source.NicknameDisplay;
      }
    }

    [MessagePackObject(false)]
    public class DateTemperatureInfo
    {
      public DateTemperatureInfo(Temperature temp, float morning, float day, float night)
      {
        this.Temperature = temp;
        this.MorningTemp = morning;
        this.DayTemp = day;
        this.NightTemp = night;
      }

      public DateTemperatureInfo()
      {
      }

      public DateTemperatureInfo(Environment.DateTemperatureInfo info)
      {
        this.Copy(info);
      }

      [Key(0)]
      public Temperature Temperature { get; set; } = Temperature.Normal;

      [Key(1)]
      public float MorningTemp { get; set; }

      [Key(2)]
      public float DayTemp { get; set; }

      [Key(3)]
      public float NightTemp { get; set; }

      public void Copy(Environment.DateTemperatureInfo info)
      {
        if (info == null)
          return;
        this.Temperature = info.Temperature;
        this.MorningTemp = info.MorningTemp;
        this.DayTemp = info.DayTemp;
        this.NightTemp = info.NightTemp;
      }

      public float GetTempValue(AIProject.TimeZone timeZone)
      {
        switch (timeZone)
        {
          case AIProject.TimeZone.Morning:
            return this.MorningTemp;
          case AIProject.TimeZone.Day:
            return this.DayTemp;
          case AIProject.TimeZone.Night:
            return this.NightTemp;
          default:
            return (float) (((double) this.MorningTemp + (double) this.DayTemp + (double) this.NightTemp) / 3.0);
        }
      }
    }
  }
}
