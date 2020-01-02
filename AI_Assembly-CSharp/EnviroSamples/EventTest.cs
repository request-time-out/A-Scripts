// Decompiled with JetBrains decompiler
// Type: EnviroSamples.EventTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EnviroSamples
{
  public class EventTest : MonoBehaviour
  {
    public EventTest()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      EnviroSky.instance.OnWeatherChanged += (EnviroSky.WeatherChanged) (type =>
      {
        this.DoOnWeatherChange(type);
        Debug.Log((object) ("Weather changed to: " + type.Name));
      });
      EnviroSky.instance.OnZoneChanged += (EnviroSky.ZoneChanged) (z =>
      {
        this.DoOnZoneChange(z);
        Debug.Log((object) ("ChangedZone: " + z.zoneName));
      });
      EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => Debug.Log((object) "Season changed"));
      EnviroSky.instance.OnHourPassed += (EnviroSky.HourPassed) (() => Debug.Log((object) "Hour Passed!"));
      EnviroSky.instance.OnDayPassed += (EnviroSky.DayPassed) (() => Debug.Log((object) "New Day!"));
      EnviroSky.instance.OnYearPassed += (EnviroSky.YearPassed) (() => Debug.Log((object) "New Year!"));
    }

    private void DoOnWeatherChange(EnviroWeatherPreset type)
    {
      if (!(type.Name == "Light Rain"))
        ;
    }

    private void DoOnZoneChange(EnviroZone type)
    {
      if (!(type.zoneName == "Swamp"))
        ;
    }

    public void TestEventsWWeather()
    {
      MonoBehaviour.print((object) "Weather Changed though interface!");
    }

    public void TestEventsNight()
    {
      MonoBehaviour.print((object) "Night now!!");
    }

    public void TestEventsDay()
    {
      MonoBehaviour.print((object) "Day now!!");
    }
  }
}
