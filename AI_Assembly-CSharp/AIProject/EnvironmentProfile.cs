// Decompiled with JetBrains decompiler
// Type: AIProject.EnvironmentProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class EnvironmentProfile : ScriptableObject
  {
    [SerializeField]
    [PropertyRange(1.0, 120.0)]
    private float _dayLengthInMinute;
    [SerializeField]
    [DisableInPlayMode]
    private EnvironmentSimulator.DateTimeSerialization _morningTime;
    [SerializeField]
    [DisableInPlayMode]
    private EnvironmentSimulator.DateTimeSerialization _dayTime;
    [SerializeField]
    [DisableInPlayMode]
    private EnvironmentSimulator.DateTimeSerialization _nightTime;
    [SerializeField]
    private Threshold _weatherDurationMinThreshold;
    [SerializeField]
    private float _weatherDurationMax;
    [SerializeField]
    [PropertyRange(0.0, 1.0)]
    private float _rainIntensity;
    [SerializeField]
    [PropertyRange(0.0, 1.0)]
    private float _rainMistThreshold;
    [SerializeField]
    private float _windSoundVolumeModifier;
    [SerializeField]
    private Threshold _windSpeedRange;
    [SerializeField]
    [MinValue(1.0)]
    private float _windSoundMultiplier;
    [SerializeField]
    private Threshold _windChangeIntervalThreshold;
    [SerializeField]
    private EnvironmentProfile.TemperatureBorderInfo _temperatureBorder;
    [SerializeField]
    private int _searchCount;
    [SerializeField]
    private float _searchCoolTimeDuration;
    [SerializeField]
    private float _runtimeMapItemLifeTime;
    [SerializeField]
    private EnvironmentSimulator.DateTimeSerialization _lightMorningTime;
    [SerializeField]
    private EnvironmentSimulator.DateTimeSerialization _lightDayTime;
    [SerializeField]
    private EnvironmentSimulator.DateTimeSerialization _lightNightTime;
    [SerializeField]
    private EnvironmentProfile.WeatherTemperatureRangeDayInfo _weatherTemperatureRange;

    public EnvironmentProfile()
    {
      base.\u002Ector();
    }

    public float DayLengthInMinute
    {
      get
      {
        return this._dayLengthInMinute;
      }
      set
      {
        this._dayLengthInMinute = value;
      }
    }

    public DateTime MorningTime
    {
      get
      {
        return this._morningTime.Time;
      }
    }

    public DateTime DayTime
    {
      get
      {
        return this._dayTime.Time;
      }
    }

    public DateTime NightTime
    {
      get
      {
        return this._nightTime.Time;
      }
    }

    public Threshold WeatherDurationMinThreshold
    {
      get
      {
        return this._weatherDurationMinThreshold;
      }
    }

    public float WeatherDurationMax
    {
      get
      {
        return this._weatherDurationMax;
      }
    }

    public float RainIntensity
    {
      get
      {
        return this._rainIntensity;
      }
    }

    public float RainMistThreshold
    {
      get
      {
        return this._rainMistThreshold;
      }
    }

    public float WindSoundVolumeModifier
    {
      get
      {
        return this._windSoundVolumeModifier;
      }
    }

    public Threshold WindSpeedRange
    {
      get
      {
        return this._windSpeedRange;
      }
    }

    public float WindSoundMultiplier
    {
      get
      {
        return this._windSoundMultiplier;
      }
    }

    public Threshold WindChangeIntervalThreshold
    {
      get
      {
        return this._windChangeIntervalThreshold;
      }
    }

    public EnvironmentProfile.TemperatureBorderInfo TemperatureBorder
    {
      get
      {
        return this._temperatureBorder;
      }
    }

    public int SearchCount
    {
      get
      {
        return this._searchCount;
      }
    }

    public float SearchCoolTimeDuration
    {
      get
      {
        return this._searchCoolTimeDuration;
      }
    }

    public float RuntimeMapItemLifeTime
    {
      get
      {
        return this._runtimeMapItemLifeTime;
      }
    }

    public DateTime LightMorningTime
    {
      get
      {
        return this._lightMorningTime.Time;
      }
    }

    public DateTime LightDayTime
    {
      get
      {
        return this._lightDayTime.Time;
      }
    }

    public DateTime LightNightTime
    {
      get
      {
        return this._lightNightTime.Time;
      }
    }

    public EnvironmentProfile.WeatherTemperatureRangeDayInfo WeatherTemperatureRange
    {
      get
      {
        return this._weatherTemperatureRange;
      }
    }

    [Serializable]
    public struct TemperatureBorderInfo
    {
      [SerializeField]
      private int _minDegree;
      [SerializeField]
      private int _lowBorder;
      [SerializeField]
      private int _highBorder;
      [SerializeField]
      private int _maxDegree;

      public int MinDegree
      {
        get
        {
          return this._minDegree;
        }
      }

      public int LowBorder
      {
        get
        {
          return this._lowBorder;
        }
      }

      public int HighBorder
      {
        get
        {
          return this._highBorder;
        }
      }

      public int MaxDegree
      {
        get
        {
          return this._maxDegree;
        }
      }

      public int TotalDegree
      {
        get
        {
          return this._maxDegree - this._minDegree;
        }
      }

      public Temperature GetTemperatureType(float degree)
      {
        degree = Mathf.Clamp(degree, (float) this._minDegree, (float) this._maxDegree);
        if ((double) degree < (double) this._lowBorder)
          return Temperature.Cold;
        return (double) this._lowBorder <= (double) degree && (double) degree < (double) this._highBorder || (double) this._highBorder > (double) degree ? Temperature.Normal : Temperature.Hot;
      }

      public float GetRandValue()
      {
        return (float) Random.Range(this._minDegree, this._maxDegree + 1);
      }

      public ValueTuple<int, int> GetValueRange(Temperature temp)
      {
        switch (temp)
        {
          case Temperature.Normal:
            return new ValueTuple<int, int>(this._lowBorder, this._highBorder - this._lowBorder - 1);
          case Temperature.Hot:
            return new ValueTuple<int, int>(this._highBorder, this._maxDegree - this._highBorder);
          case Temperature.Cold:
            return new ValueTuple<int, int>(this._minDegree, this._lowBorder - this._minDegree - 1);
          default:
            return new ValueTuple<int, int>(0, 0);
        }
      }

      public int GetRandValue(Temperature temp)
      {
        switch (temp)
        {
          case Temperature.Normal:
            return Random.Range(this._lowBorder, this._highBorder);
          case Temperature.Hot:
            return Random.Range(this._highBorder, this._maxDegree + 1);
          case Temperature.Cold:
            return Random.Range(this._minDegree, this._lowBorder);
          default:
            return 0;
        }
      }
    }

    [Serializable]
    public struct WeatherTemperatureRangeDayInfo
    {
      [SerializeField]
      private EnvironmentSimulator.DateTimeSerialization _dayTime;
      [SerializeField]
      private EnvironmentSimulator.DateTimeSerialization _nightTime;
      [SerializeField]
      private EnvironmentProfile.WeatherTemperatureRangeInfo _dayTimeRange;
      [SerializeField]
      private EnvironmentProfile.WeatherTemperatureRangeInfo _nightTimeRange;

      public EnvironmentSimulator.DateTimeSerialization DayTime
      {
        get
        {
          return this._dayTime;
        }
      }

      public EnvironmentSimulator.DateTimeSerialization NightTime
      {
        get
        {
          return this._nightTime;
        }
      }

      public Threshold GetRange(TimeZone timeZone, Weather weather)
      {
        if (timeZone == TimeZone.Day)
          return this._dayTimeRange.GetRange(weather);
        return timeZone == TimeZone.Night ? this._nightTimeRange.GetRange(weather) : new Threshold(0.0f, 0.0f);
      }
    }

    [Serializable]
    public struct WeatherTemperatureRangeInfo
    {
      [SerializeField]
      private Threshold _clearRange;
      [SerializeField]
      private Threshold _cloud1Range;
      [SerializeField]
      private Threshold _cloud2Range;
      [SerializeField]
      private Threshold _cloud3Range;
      [SerializeField]
      private Threshold _cloud4Range;
      [SerializeField]
      private Threshold _fogRange;
      [SerializeField]
      private Threshold _rainRange;
      [SerializeField]
      private Threshold _stormRange;

      public Threshold GetRange(Weather weather)
      {
        switch (weather)
        {
          case Weather.Clear:
            return this._clearRange;
          case Weather.Cloud1:
            return this._cloud1Range;
          case Weather.Cloud2:
            return this._cloud2Range;
          case Weather.Cloud3:
            return this._cloud3Range;
          case Weather.Cloud4:
            return this._cloud4Range;
          case Weather.Fog:
            return this._fogRange;
          case Weather.Rain:
            return this._rainRange;
          case Weather.Storm:
            return this._stormRange;
          default:
            return new Threshold(0.0f, 0.0f);
        }
      }
    }
  }
}
