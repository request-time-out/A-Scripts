// Decompiled with JetBrains decompiler
// Type: AIProject.EnvironmentSimulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;
using UnityEx.Misc;

namespace AIProject
{
  public class EnvironmentSimulator : SerializedMonoBehaviour
  {
    [SerializeField]
    private EnviroSky _enviroSky;
    [SerializeField]
    private EnviroZone _defaultEnviroZone;
    [SerializeField]
    private EnviroEvents _enviroEvents;
    private bool _isActiveSimElement;
    [SerializeField]
    private BoolReactiveProperty _enableTimeProgression;
    private IConnectableObservable<bool> _enableTimeChanged;
    [SerializeField]
    private GameObject _skyDomeObject;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private TimeZone _timeZone;
    private ReactiveProperty<TimeZone> _reactiveTimeZone;
    private Subject<Weather> _weatherChanged;
    [SerializeField]
    [HideInEditorMode]
    [DisableInPlayMode]
    private Weather _weather;
    [SerializeField]
    [DisableInEditorMode]
    [DisableInPlayMode]
    private Temperature _temperature;
    [ShowInInspector]
    [HideInEditorMode]
    [DisableInPlayMode]
    private float _temperatureValue;
    [SerializeField]
    private float _fadeTime;
    [SerializeField]
    private EnvironmentProfile _environmentProfile;
    [SerializeField]
    private float _enviroParticleHeight;
    [SerializeField]
    private bool _enviroParticleFollowTargetHeight;
    [SerializeField]
    [DisableInPlayMode]
    private Dictionary<Weather, List<ParticleSystem>> _enviroParticlePrefabTable;
    [SerializeField]
    private Dictionary<Weather, float> _weatherTemperatureCorrectedValueTable;
    private Subject<Unit> _onEnvironmentChange;
    private Subject<TimeZone> _onTimeZoneChange;
    private Subject<TimeZone> _onMapLightTimeZoneChange;
    private DateTime _prevDateTime;
    private int _deltaSeconds;
    private List<AIProject.SaveData.Environment.ScheduleData> _schedules;
    private Subject<TimeSpan> _onDay;
    private Subject<TimeSpan> _onHour;
    private Subject<TimeSpan> _onMinute;
    private Subject<TimeSpan> _onSecond;
    private TimeSpan _tempDayTimeOfDay;
    private TimeSpan _tempNightTimeOfDay;
    private TimeZone _tempTimeZone;
    private ReactiveProperty<TimeZone> _reactiveTempTimeZone;
    private TimeZone _mapLightTimeZone;
    private ReactiveProperty<TimeZone> _reactiveMapLightTimeZone;
    private List<EnviroParticleInfo> _playingEnviroParticles;
    private Dictionary<int, EnviroParticleInfo> _enviroParticleDataTable;
    private Dictionary<Weather, List<EnviroParticleInfo>> _enviroParticleObjectTable;
    private GameObject _enviroParticleRoot;
    private IDisposable _enviroParticleFollowDisposable;

    public EnvironmentSimulator()
    {
      base.\u002Ector();
    }

    public bool EnabledSky
    {
      get
      {
        return ((Behaviour) this._enviroSky).get_enabled();
      }
      set
      {
        ((Behaviour) this._enviroSky).set_enabled(value);
      }
    }

    public EnviroSky EnviroSky
    {
      get
      {
        return this._enviroSky;
      }
      set
      {
        this._enviroSky = value;
        value.Weather.updateWeather = false;
      }
    }

    public EnviroZone DefaultEnviroZone
    {
      get
      {
        return this._defaultEnviroZone;
      }
      set
      {
        this._defaultEnviroZone = value;
      }
    }

    public EnviroEvents EnviroEvents
    {
      get
      {
        return this._enviroEvents;
      }
      set
      {
        this._enviroEvents = value;
      }
    }

    public bool IsActiveSimElement
    {
      get
      {
        return this._isActiveSimElement;
      }
      set
      {
        this._isActiveSimElement = value;
        UnityExtensions.SetActiveSafe(this._skyDomeObject, value);
      }
    }

    public bool EnabledTimeProgression
    {
      get
      {
        return ((ReactiveProperty<bool>) this._enableTimeProgression).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._enableTimeProgression).set_Value(value);
      }
    }

    public IObservable<bool> OnEnableChangeTimeAsObservable()
    {
      if (this._enableTimeChanged == null)
      {
        this._enableTimeChanged = (IConnectableObservable<bool>) Observable.Publish<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._enableTimeProgression, ((Component) this).get_gameObject()));
        this._enableTimeChanged.Connect();
      }
      return (IObservable<bool>) this._enableTimeChanged;
    }

    public void SetTimeProgress(bool progress)
    {
      if (Object.op_Equality((Object) this._enviroSky, (Object) null))
        return;
      if (progress)
        this._enviroSky.GameTime.ProgressTime = EnviroTime.TimeProgressMode.Simulated;
      else
        this._enviroSky.GameTime.ProgressTime = EnviroTime.TimeProgressMode.None;
    }

    public GameObject SkyDomeObject
    {
      get
      {
        return this._skyDomeObject;
      }
      set
      {
        this._skyDomeObject = value;
      }
    }

    public TimeZone TimeZone
    {
      get
      {
        return this._timeZone;
      }
    }

    public IObservable<Weather> OnWeatherChangedAsObservable()
    {
      return (IObservable<Weather>) this._weatherChanged ?? (IObservable<Weather>) (this._weatherChanged = new Subject<Weather>());
    }

    public Weather Weather
    {
      get
      {
        return this._weather;
      }
    }

    public Temperature Temperature
    {
      get
      {
        return this._temperature;
      }
    }

    public float TemperatureValue
    {
      get
      {
        return this._temperatureValue;
      }
      set
      {
        float num = value;
        if (Object.op_Inequality((Object) this._environmentProfile, (Object) null))
        {
          float minDegree = (float) this._environmentProfile.TemperatureBorder.MinDegree;
          float maxDegree = (float) this._environmentProfile.TemperatureBorder.MaxDegree;
          num = Mathf.Clamp(num, minDegree, maxDegree);
        }
        if ((double) this._temperatureValue == (double) num)
          return;
        this._temperatureValue = num;
        if (Object.op_Equality((Object) this._environmentProfile, (Object) null))
          return;
        this.RefreshTemperature(this._environmentProfile.TemperatureBorder.GetTemperatureType(this._temperatureValue));
      }
    }

    public EnvironmentProfile EnvironmentProfile
    {
      get
      {
        return this._environmentProfile;
      }
    }

    public IObservable<Unit> OnEnvironmentChangedAsObservable()
    {
      return (IObservable<Unit>) this._onEnvironmentChange ?? (IObservable<Unit>) (this._onEnvironmentChange = new Subject<Unit>());
    }

    public IObservable<TimeZone> OnTimeZoneChangedAsObservable()
    {
      return (IObservable<TimeZone>) this._onTimeZoneChange ?? (IObservable<TimeZone>) (this._onTimeZoneChange = new Subject<TimeZone>());
    }

    public IObservable<TimeZone> OnMapLightTimeZoneChangedAsObservable()
    {
      return (IObservable<TimeZone>) this._onMapLightTimeZoneChange ?? (IObservable<TimeZone>) (this._onMapLightTimeZoneChange = new Subject<TimeZone>());
    }

    public DateTime Now
    {
      get
      {
        EnviroTime gameTime = this._enviroSky.GameTime;
        DateTime dateTime = new DateTime(gameTime.Years, 1, 1);
        dateTime = dateTime.AddDays((double) (gameTime.Days - 1));
        dateTime = dateTime.AddHours((double) gameTime.Hours);
        dateTime = dateTime.AddMinutes((double) gameTime.Minutes);
        dateTime = dateTime.AddSeconds((double) gameTime.Seconds);
        return dateTime;
      }
      set
      {
        this._enviroSky.SetTime(value);
      }
    }

    public int DeltaSeconds
    {
      get
      {
        return this._deltaSeconds;
      }
    }

    public ReadOnlyCollection<AIProject.SaveData.Environment.ScheduleData> Schedules
    {
      get
      {
        return this._schedules.AsReadOnly();
      }
    }

    private void Awake()
    {
      this.ResetTempDateTime();
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<TimeZone>((IObservable<M0>) this._reactiveTimeZone, (System.Action<M0>) (timeZone => this.SetTimeZone(timeZone)));
      ObservableExtensions.Subscribe<TimeZone>((IObservable<M0>) this._reactiveTempTimeZone, (System.Action<M0>) (timeZone => this.SetTempTimeZone(timeZone)));
      ObservableExtensions.Subscribe<TimeZone>((IObservable<M0>) this._reactiveMapLightTimeZone, (System.Action<M0>) (timeZone => this.SetMapLightTimeZone(timeZone)));
    }

    private void Update()
    {
      float dayLengthInMinute = this._environmentProfile.DayLengthInMinute;
      this._enviroSky.GameTime.DayLengthInMinutes = dayLengthInMinute;
      this._enviroSky.GameTime.NightLengthInMinutes = dayLengthInMinute;
      this.OnTimeUpdate();
      TimeSpan timeOfDay = this.Now.TimeOfDay;
      if (timeOfDay >= this._environmentProfile.MorningTime.TimeOfDay && timeOfDay < this._environmentProfile.DayTime.TimeOfDay)
        this._reactiveTimeZone.set_Value(TimeZone.Morning);
      else if (timeOfDay >= this._environmentProfile.DayTime.TimeOfDay && timeOfDay < this._environmentProfile.NightTime.TimeOfDay)
        this._reactiveTimeZone.set_Value(TimeZone.Day);
      else
        this._reactiveTimeZone.set_Value(TimeZone.Night);
      this.OnTemperatureValueUpdate(timeOfDay);
      this.OnMapLightTimeUpdate(timeOfDay);
    }

    public void InitializeEvents()
    {
    }

    private void OnHourUpdate()
    {
    }

    public void OnMinuteUpdate(TimeSpan deltaTime)
    {
    }

    private void OnSecondUpdate()
    {
      DateTime now = this.Now;
      this._deltaSeconds = (int) (now - this._prevDateTime).TotalSeconds;
      this._prevDateTime = now;
    }

    private void OnTimeUpdate()
    {
      EnviroTime oldTime = this.OldTime;
      EnviroTime gameTime = this._enviroSky.GameTime;
      DateTime newTime = new DateTime(1, 1, gameTime.Days, gameTime.Hours, gameTime.Minutes, gameTime.Seconds);
      bool flag = false;
      if (gameTime.Days > oldTime.Days)
      {
        TimeSpan timeSpan1 = this.DeltaTimeSpan(newTime, this.OldDayUpdatedTime);
        TimeSpan timeSpan2 = this.DeltaTimeSpan(newTime, this.OldHourUpdatedTime);
        TimeSpan timeSpan3 = this.DeltaTimeSpan(newTime, this.OldMinuteUpdatedTime);
        TimeSpan timeSpan4 = this.DeltaTimeSpan(newTime, this.OldSecondUpdatedTime);
        gameTime.Days = 1;
        if (this._onDay != null)
          this._onDay.OnNext(timeSpan1);
        if (this._onHour != null)
          this._onHour.OnNext(timeSpan2);
        if (this._onMinute != null)
          this._onMinute.OnNext(timeSpan3);
        if (this._onSecond != null)
          this._onSecond.OnNext(timeSpan4);
        flag = true;
        this.SetTimeToEnviroTime(this.OldDayUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldHourUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldMinuteUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldSecondUpdatedTime, gameTime);
      }
      else if (gameTime.Hours > oldTime.Hours)
      {
        TimeSpan timeSpan1 = this.DeltaTimeSpan(newTime, this.OldHourUpdatedTime);
        TimeSpan timeSpan2 = this.DeltaTimeSpan(newTime, this.OldMinuteUpdatedTime);
        TimeSpan timeSpan3 = this.DeltaTimeSpan(newTime, this.OldSecondUpdatedTime);
        if (this._onHour != null)
          this._onHour.OnNext(timeSpan1);
        if (this._onMinute != null)
          this._onMinute.OnNext(timeSpan2);
        if (this._onSecond != null)
          this._onSecond.OnNext(timeSpan3);
        flag = true;
        this.SetTimeToEnviroTime(this.OldHourUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldMinuteUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldSecondUpdatedTime, gameTime);
      }
      else if (gameTime.Minutes > oldTime.Minutes)
      {
        TimeSpan timeSpan1 = this.DeltaTimeSpan(newTime, this.OldMinuteUpdatedTime);
        TimeSpan timeSpan2 = this.DeltaTimeSpan(newTime, this.OldSecondUpdatedTime);
        if (this._onMinute != null)
          this._onMinute.OnNext(timeSpan1);
        if (this._onSecond != null)
          this._onSecond.OnNext(timeSpan2);
        flag = true;
        this.SetTimeToEnviroTime(this.OldMinuteUpdatedTime, gameTime);
        this.SetTimeToEnviroTime(this.OldSecondUpdatedTime, gameTime);
      }
      else if (gameTime.Seconds > oldTime.Seconds)
      {
        TimeSpan timeSpan = this.DeltaTimeSpan(newTime, this.OldSecondUpdatedTime);
        if (this._onSecond != null)
          this._onSecond.OnNext(timeSpan);
        flag = true;
        this.SetTimeToEnviroTime(this.OldSecondUpdatedTime, gameTime);
      }
      if (!flag)
        return;
      this.OldTime.Days = gameTime.Days;
      this.OldTime.Hours = gameTime.Hours;
      this.OldTime.Minutes = gameTime.Minutes;
      this.OldTime.Seconds = gameTime.Seconds;
    }

    public EnviroTime OldTime { get; private set; }

    public EnviroTime OldDayUpdatedTime { get; private set; }

    public EnviroTime OldHourUpdatedTime { get; private set; }

    public EnviroTime OldMinuteUpdatedTime { get; private set; }

    public EnviroTime OldSecondUpdatedTime { get; private set; }

    public void SetTimeToEnviroTime(EnviroTime time, EnviroTime newTime)
    {
      time.Days = newTime.Days;
      time.Hours = newTime.Hours;
      time.Minutes = newTime.Minutes;
      time.Seconds = newTime.Seconds;
    }

    public TimeSpan DeltaTimeSpan(DateTime newTime, EnviroTime oldEnviroTime)
    {
      DateTime dateTime = new DateTime(1, 1, oldEnviroTime.Days, oldEnviroTime.Hours, oldEnviroTime.Minutes, oldEnviroTime.Seconds);
      return newTime - dateTime;
    }

    public IObservable<TimeSpan> OnDayAsObservable()
    {
      return (IObservable<TimeSpan>) this._onDay ?? (IObservable<TimeSpan>) (this._onDay = new Subject<TimeSpan>());
    }

    public IObservable<TimeSpan> OnHourAsObservable()
    {
      return (IObservable<TimeSpan>) this._onHour ?? (IObservable<TimeSpan>) (this._onHour = new Subject<TimeSpan>());
    }

    public IObservable<TimeSpan> OnMinuteAsObservable()
    {
      return (IObservable<TimeSpan>) this._onMinute ?? (IObservable<TimeSpan>) (this._onMinute = new Subject<TimeSpan>());
    }

    public IObservable<TimeSpan> OnSecondAsObservable()
    {
      return (IObservable<TimeSpan>) this._onSecond ?? (IObservable<TimeSpan>) (this._onSecond = new Subject<TimeSpan>());
    }

    public void SetTimeZone(TimeZone zone)
    {
      this._timeZone = zone;
      if (this._onTimeZoneChange != null)
        this._onTimeZoneChange.OnNext(zone);
      if (this._onEnvironmentChange == null)
        return;
      this._onEnvironmentChange.OnNext(Unit.get_Default());
    }

    public void RefreshWeather(Weather weather, bool forced = false)
    {
      if (!forced && this._weather == weather)
        return;
      Debug.Log((object) string.Format("set weather: {0}", (object) weather.ToString()));
      this._weather = weather;
      if (this._weatherChanged != null)
        this._weatherChanged.OnNext(weather);
      this._enviroSky.ChangeWeather((int) weather);
      this.RefreshEnviroParticles(weather);
    }

    public int MapID { get; set; }

    public TimeZone TempTimeZone
    {
      get
      {
        return this._tempTimeZone;
      }
    }

    private void ResetTempDateTime()
    {
      if (!Object.op_Inequality((Object) this._environmentProfile, (Object) null))
        return;
      this._tempDayTimeOfDay = this._environmentProfile.WeatherTemperatureRange.DayTime.Time.TimeOfDay;
      this._tempNightTimeOfDay = this._environmentProfile.WeatherTemperatureRange.NightTime.Time.TimeOfDay;
    }

    private void SetTempTimeZone(TimeZone timeZone)
    {
      this._tempTimeZone = timeZone;
      switch (this._tempTimeZone)
      {
        case TimeZone.Day:
          WeatherProbability probWeatherProfile = Singleton<Resources>.Instance.CommonDefine.ProbWeatherProfile;
          List<Weather> list = Enum.GetValues(typeof (Weather)).Cast<Weather>().Skip<Weather>(0).ToList<Weather>();
          Weather weather;
          for (weather = probWeatherProfile.Lottery(list); this._weather == weather; weather = probWeatherProfile.Lottery(list))
            list.Remove(weather);
          this.RefreshWeather(weather, false);
          break;
      }
      this.RefreshTemperatureValue();
    }

    public void SetTemperatureValue(float tempValue)
    {
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.TimerFrame(2, (FrameCountType) 0), (Component) this), (System.Action<M0>) (_ =>
      {
        if (Object.op_Equality((Object) this._environmentProfile, (Object) null))
          return;
        Threshold range = this._environmentProfile.WeatherTemperatureRange.GetRange(this._tempTimeZone, this._weather);
        if ((double) range.min <= (double) tempValue && (double) tempValue <= (double) range.max)
          this.TemperatureValue = tempValue;
        else
          this.TemperatureValue = range.RandomValue;
      }));
    }

    public void RefreshTemperatureValue()
    {
      if (!Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>>> tempRangeTable = Singleton<Resources>.Instance.Map.TempRangeTable;
      if (tempRangeTable.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>>>())
        return;
      Dictionary<int, Dictionary<int, List<ValueTuple<int, int>>>> source1;
      if (!tempRangeTable.TryGetValue(this.MapID, out source1) || source1.IsNullOrEmpty<int, Dictionary<int, List<ValueTuple<int, int>>>>())
      {
        if (this.MapID != 0)
          tempRangeTable.TryGetValue(0, out source1);
        if (source1.IsNullOrEmpty<int, Dictionary<int, List<ValueTuple<int, int>>>>())
          return;
      }
      int key;
      Dictionary<int, List<ValueTuple<int, int>>> source2;
      List<ValueTuple<int, int>> source3;
      if (!AIProject.Definitions.Environment.TimeZoneIDTable.TryGetValue(this._tempTimeZone, ref key) || !source1.TryGetValue(key, out source2) || (source2.IsNullOrEmpty<int, List<ValueTuple<int, int>>>() || !source2.TryGetValue((int) this._weather, out source3)) || source3.IsNullOrEmpty<ValueTuple<int, int>>())
        return;
      List<ValueTuple<int, int>> toRelease = ListPool<ValueTuple<int, int>>.Get();
      toRelease.AddRange((IEnumerable<ValueTuple<int, int>>) source3);
      int num1 = 0;
      using (List<ValueTuple<int, int>>.Enumerator enumerator = toRelease.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, int> current = enumerator.Current;
          num1 += (int) current.Item2;
        }
      }
      int num2 = Random.Range(0, num1);
      while (0 < toRelease.Count)
      {
        ValueTuple<int, int> valueTuple = ((IList<ValueTuple<int, int>>) toRelease).PopFront<ValueTuple<int, int>>();
        if (num2 < valueTuple.Item2)
        {
          this.TemperatureValue = (float) valueTuple.Item1;
          ListPool<ValueTuple<int, int>>.Release(toRelease);
          return;
        }
        num2 -= (int) valueTuple.Item2;
      }
      ListPool<ValueTuple<int, int>>.Release(toRelease);
    }

    private void OnTemperatureValueUpdate(TimeSpan now)
    {
      if (this._tempNightTimeOfDay < this._tempDayTimeOfDay)
      {
        if (now < this._tempNightTimeOfDay || this._tempDayTimeOfDay <= now)
          this._reactiveTempTimeZone.set_Value(TimeZone.Day);
        else
          this._reactiveTempTimeZone.set_Value(TimeZone.Night);
      }
      else if (this._tempDayTimeOfDay <= now && now < this._tempNightTimeOfDay)
        this._reactiveTempTimeZone.set_Value(TimeZone.Day);
      else
        this._reactiveTempTimeZone.set_Value(TimeZone.Night);
    }

    public TimeZone MapLightTimeZone
    {
      get
      {
        return this._mapLightTimeZone;
      }
    }

    public void SetMapLightTimeZone(TimeZone timeZone)
    {
      this._mapLightTimeZone = timeZone;
      if (this._onMapLightTimeZoneChange == null)
        return;
      this._onMapLightTimeZoneChange.OnNext(timeZone);
    }

    private void OnMapLightTimeUpdate(TimeSpan now)
    {
      if (this._environmentProfile.LightMorningTime.TimeOfDay <= now && now < this._environmentProfile.LightDayTime.TimeOfDay)
        this._reactiveMapLightTimeZone.set_Value(TimeZone.Morning);
      else if (this._environmentProfile.LightDayTime.TimeOfDay <= now && now < this._environmentProfile.LightNightTime.TimeOfDay)
        this._reactiveMapLightTimeZone.set_Value(TimeZone.Day);
      else
        this._reactiveMapLightTimeZone.set_Value(TimeZone.Night);
    }

    public GameObject EnviroParticleRoot
    {
      get
      {
        if (Object.op_Equality((Object) this._enviroParticleRoot, (Object) null))
        {
          this._enviroParticleRoot = new GameObject("Enviro Particle Root");
          this._enviroParticleRoot.get_transform().SetParent(((Component) this).get_transform(), false);
        }
        return this._enviroParticleRoot;
      }
      private set
      {
        this._enviroParticleRoot = value;
      }
    }

    public void EnviroParticleActivate(bool active)
    {
      GameObject enviroParticleRoot = this.EnviroParticleRoot;
      if (Object.op_Equality((Object) enviroParticleRoot, (Object) null) || enviroParticleRoot.get_activeSelf() == active)
        return;
      enviroParticleRoot.SetActive(active);
    }

    public void InitializeEnviroParticle()
    {
      if (this._enviroParticlePrefabTable.IsNullOrEmpty<Weather, List<ParticleSystem>>())
        return;
      GameObject enviroParticleRoot = this.EnviroParticleRoot;
      using (Dictionary<Weather, List<ParticleSystem>>.Enumerator enumerator1 = this._enviroParticlePrefabTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<Weather, List<ParticleSystem>> current1 = enumerator1.Current;
          if (!current1.Value.IsNullOrEmpty<ParticleSystem>())
          {
            using (List<ParticleSystem>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                ParticleSystem current2 = enumerator2.Current;
                if (!Object.op_Equality((Object) current2, (Object) null))
                {
                  int instanceId = ((Object) current2).GetInstanceID();
                  EnviroParticleInfo enviroParticleInfo;
                  if (!this._enviroParticleDataTable.TryGetValue(instanceId, out enviroParticleInfo))
                  {
                    ParticleSystem _particle = (ParticleSystem) Object.Instantiate<ParticleSystem>((M0) current2, enviroParticleRoot.get_transform());
                    this._enviroParticleDataTable[instanceId] = enviroParticleInfo = new EnviroParticleInfo(instanceId, _particle);
                    _particle.Stop(true, (ParticleSystemStopBehavior) 0);
                    enviroParticleInfo.Emission = 0.0f;
                  }
                  List<EnviroParticleInfo> enviroParticleInfoList;
                  if (!this._enviroParticleObjectTable.TryGetValue(current1.Key, out enviroParticleInfoList))
                    this._enviroParticleObjectTable[current1.Key] = enviroParticleInfoList = new List<EnviroParticleInfo>();
                  enviroParticleInfoList.Add(enviroParticleInfo);
                }
              }
            }
          }
        }
      }
    }

    public void SetEnviroParticleTarget(Transform target)
    {
      if (Object.op_Equality((Object) target, (Object) null))
        return;
      GameObject envParRoot = this.EnviroParticleRoot;
      if (Object.op_Equality((Object) envParRoot, (Object) null))
        return;
      if (this._enviroParticleFollowDisposable != null)
        this._enviroParticleFollowDisposable.Dispose();
      this._enviroParticleFollowDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) envParRoot.get_transform()), (Component) target), (System.Action<M0>) (_ =>
      {
        Vector3 position = target.get_position();
        if (this._enviroParticleFollowTargetHeight)
        {
          ref Vector3 local = ref position;
          local.y = (__Null) (local.y + (double) this._enviroParticleHeight);
        }
        else
          position.y = (__Null) (double) this._enviroParticleHeight;
        envParRoot.get_transform().set_position(position);
      }));
    }

    public void StopEnviroParticleTargetFollow()
    {
      if (this._enviroParticleFollowDisposable == null)
        return;
      this._enviroParticleFollowDisposable.Dispose();
      this._enviroParticleFollowDisposable = (IDisposable) null;
    }

    private void RefreshEnviroParticles(Weather weather)
    {
      List<EnviroParticleInfo> source;
      if (!this._enviroParticleObjectTable.TryGetValue(weather, out source) || source.IsNullOrEmpty<EnviroParticleInfo>())
      {
        if (this._playingEnviroParticles.IsNullOrEmpty<EnviroParticleInfo>())
          return;
        foreach (EnviroParticleInfo playingEnviroParticle in this._playingEnviroParticles)
          playingEnviroParticle.PlayFadeOut(this._fadeTime);
        this._playingEnviroParticles.Clear();
      }
      else
      {
        List<EnviroParticleInfo> toRelease = ListPool<EnviroParticleInfo>.Get();
        foreach (EnviroParticleInfo playingEnviroParticle in this._playingEnviroParticles)
        {
          if (!source.Contains(playingEnviroParticle) && !toRelease.Contains(playingEnviroParticle))
            toRelease.Add(playingEnviroParticle);
        }
        foreach (EnviroParticleInfo enviroParticleInfo in toRelease)
          enviroParticleInfo.PlayFadeOut(this._fadeTime);
        ListPool<EnviroParticleInfo>.Release(toRelease);
        foreach (EnviroParticleInfo enviroParticleInfo in source)
          enviroParticleInfo.PlayFadeIn(this._fadeTime);
        this._playingEnviroParticles.Clear();
        this._playingEnviroParticles.AddRange((IEnumerable<EnviroParticleInfo>) source);
      }
    }

    public void RefreshTemperature(Temperature temperature)
    {
      if (this._temperature == temperature)
        return;
      this._temperature = temperature;
    }

    public bool Elapsed(DateTime dateTime)
    {
      return this.Now > dateTime;
    }

    public void ElapseDays(int days)
    {
      foreach (AIProject.SaveData.Environment.ScheduleData schedule in this.Schedules)
        schedule.DaysToGo -= days;
      foreach (AIProject.SaveData.Environment.ScheduleData scheduleData in this._schedules.FindAll((Predicate<AIProject.SaveData.Environment.ScheduleData>) (x =>
      {
        TimeSpan timeSpan = this.Now - x.StartTime.DateTime;
        return x.DaysToGo == 0 && x.StartTime.DateTime < this.Now && x.Duration.TimeSpan < timeSpan;
      })))
      {
        Singleton<Manager.Map>.Instance.AgentTable.get_Item(scheduleData.Event.agentID);
        this._schedules.Remove(scheduleData);
      }
    }

    [Serializable]
    public struct DateTimeThreshold
    {
      public EnvironmentSimulator.DateTimeSerialization min;
      public EnvironmentSimulator.DateTimeSerialization max;
    }

    [Serializable]
    public struct DateTimeSerialization
    {
      [SerializeField]
      private int _day;
      [SerializeField]
      private int _hour;
      [SerializeField]
      private int _minute;

      public DateTimeSerialization(DateTime dateTime)
      {
        this._day = dateTime.Day;
        this._hour = dateTime.Hour;
        this._minute = dateTime.Minute;
      }

      public DateTime Time
      {
        get
        {
          return new DateTime(1, 1, 1 + this._day, this._hour, this._minute, 0);
        }
        set
        {
          this._day = value.Day - 1;
          this._hour = value.Hour;
          this._minute = value.Minute;
        }
      }
    }
  }
}
