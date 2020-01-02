// Decompiled with JetBrains decompiler
// Type: EnviroEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;

public class EnviroEvents : MonoBehaviour
{
  public EnviroEvents.EnviroActionEvent onHourPassedActions;
  public EnviroEvents.EnviroActionEvent onDayPassedActions;
  public EnviroEvents.EnviroActionEvent onYearPassedActions;
  public EnviroEvents.EnviroActionEvent onWeatherChangedActions;
  public EnviroEvents.EnviroActionEvent onSeasonChangedActions;
  public EnviroEvents.EnviroActionEvent onNightActions;
  public EnviroEvents.EnviroActionEvent onDayActions;
  public EnviroEvents.EnviroActionEvent onZoneChangedActions;

  public EnviroEvents()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    EnviroSky.instance.OnHourPassed += (EnviroSky.HourPassed) (() => this.HourPassed());
    EnviroSky.instance.OnDayPassed += (EnviroSky.DayPassed) (() => this.DayPassed());
    EnviroSky.instance.OnYearPassed += (EnviroSky.YearPassed) (() => this.YearPassed());
    EnviroSky.instance.OnWeatherChanged += (EnviroSky.WeatherChanged) (type => this.WeatherChanged());
    EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.SeasonsChanged());
    EnviroSky.instance.OnNightTime += (EnviroSky.isNightE) (() => this.NightTime());
    EnviroSky.instance.OnDayTime += (EnviroSky.isDay) (() => this.DayTime());
    EnviroSky.instance.OnZoneChanged += (EnviroSky.ZoneChanged) (zone => this.ZoneChanged());
  }

  private void HourPassed()
  {
    this.onHourPassedActions.Invoke();
  }

  private void DayPassed()
  {
    this.onDayPassedActions.Invoke();
  }

  private void YearPassed()
  {
    this.onYearPassedActions.Invoke();
  }

  private void WeatherChanged()
  {
    this.onWeatherChangedActions.Invoke();
  }

  private void SeasonsChanged()
  {
    this.onSeasonChangedActions.Invoke();
  }

  private void NightTime()
  {
    this.onNightActions.Invoke();
  }

  private void DayTime()
  {
    this.onDayActions.Invoke();
  }

  private void ZoneChanged()
  {
    this.onZoneChangedActions.Invoke();
  }

  [Serializable]
  public class EnviroActionEvent : UnityEvent
  {
    public EnviroActionEvent()
    {
      base.\u002Ector();
    }
  }
}
