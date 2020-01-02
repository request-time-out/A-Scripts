// Decompiled with JetBrains decompiler
// Type: EnviroSamples.DemoUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace EnviroSamples
{
  public class DemoUI : MonoBehaviour
  {
    public Slider sliderTime;
    public Slider sliderQuality;
    public Text timeText;
    public Dropdown weatherDropdown;
    public Dropdown seasonDropdown;
    private bool seasonmode;
    private bool fastdays;
    private bool started;

    public DemoUI()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      EnviroSky.instance.OnWeatherChanged += (EnviroSky.WeatherChanged) (type => this.UpdateWeatherSlider());
      EnviroSky.instance.OnSeasonChanged += (EnviroSky.SeasonChanged) (season => this.UpdateSeasonSlider(season));
    }

    [DebuggerHidden]
    private IEnumerator setupDrodown()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new DemoUI.\u003CsetupDrodown\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void ChangeTimeSlider()
    {
      if ((double) this.sliderTime.get_value() < 0.0)
        this.sliderTime.set_value(0.0f);
      EnviroSky.instance.SetInternalTimeOfDay(this.sliderTime.get_value() * 24f);
    }

    public void ChangeTimeLenghtSlider(float value)
    {
      EnviroSky.instance.GameTime.DayLengthInMinutes = value;
    }

    public void ChangeCloudQuality(int value)
    {
      EnviroSky.instance.cloudsSettings.cloudsQuality = (EnviroCloudSettings.CloudQuality) value;
    }

    public void ChangeQualitySlider()
    {
      EnviroSky.instance.profile.qualitySettings.GlobalParticleEmissionRates = this.sliderQuality.get_value();
    }

    public void ChangeAmbientVolume(float value)
    {
      EnviroSky.instance.Audio.ambientSFXVolume = value;
    }

    public void ChangeWeatherVolume(float value)
    {
      EnviroSky.instance.Audio.weatherSFXVolume = value;
    }

    public void SetWeatherID(int id)
    {
      EnviroSky.instance.ChangeWeather(id);
    }

    public void SetClouds(int id)
    {
      EnviroSky.instance.cloudsMode = (EnviroSky.EnviroCloudsMode) id;
    }

    public void OverwriteSeason()
    {
      if (!this.seasonmode)
      {
        this.seasonmode = true;
        EnviroSky.instance.Seasons.calcSeasons = true;
      }
      else
      {
        this.seasonmode = false;
        EnviroSky.instance.Seasons.calcSeasons = false;
      }
    }

    public void FastDays()
    {
      if (!this.fastdays)
      {
        this.fastdays = true;
        EnviroSky.instance.GameTime.DayLengthInMinutes = 0.2f;
      }
      else
      {
        this.fastdays = false;
        EnviroSky.instance.GameTime.DayLengthInMinutes = 5f;
      }
    }

    public void SetSeason(int id)
    {
      switch (id)
      {
        case 0:
          EnviroSky.instance.ChangeSeason(EnviroSeasons.Seasons.Spring);
          break;
        case 1:
          EnviroSky.instance.ChangeSeason(EnviroSeasons.Seasons.Summer);
          break;
        case 2:
          EnviroSky.instance.ChangeSeason(EnviroSeasons.Seasons.Autumn);
          break;
        case 3:
          EnviroSky.instance.ChangeSeason(EnviroSeasons.Seasons.Winter);
          break;
      }
    }

    public void SetTimeProgress(int id)
    {
      switch (id)
      {
        case 0:
          EnviroSky.instance.GameTime.ProgressTime = EnviroTime.TimeProgressMode.None;
          break;
        case 1:
          EnviroSky.instance.GameTime.ProgressTime = EnviroTime.TimeProgressMode.Simulated;
          break;
        case 2:
          EnviroSky.instance.GameTime.ProgressTime = EnviroTime.TimeProgressMode.SystemTime;
          break;
      }
    }

    private void UpdateWeatherSlider()
    {
      if (!Object.op_Inequality((Object) EnviroSky.instance.Weather.currentActiveWeatherPreset, (Object) null))
        return;
      for (int index = 0; index < this.weatherDropdown.get_options().Count; ++index)
      {
        if (this.weatherDropdown.get_options()[index].get_text() == EnviroSky.instance.Weather.currentActiveWeatherPreset.Name)
          this.weatherDropdown.set_value(index);
      }
    }

    private void UpdateSeasonSlider(EnviroSeasons.Seasons s)
    {
      switch (s)
      {
        case EnviroSeasons.Seasons.Spring:
          this.seasonDropdown.set_value(0);
          break;
        case EnviroSeasons.Seasons.Summer:
          this.seasonDropdown.set_value(1);
          break;
        case EnviroSeasons.Seasons.Autumn:
          this.seasonDropdown.set_value(2);
          break;
        case EnviroSeasons.Seasons.Winter:
          this.seasonDropdown.set_value(3);
          break;
      }
    }

    private void Update()
    {
      if (!EnviroSky.instance.started)
        return;
      if (!this.started)
        this.StartCoroutine(this.setupDrodown());
      this.timeText.set_text(EnviroSky.instance.GetTimeString());
      this.ChangeQualitySlider();
    }

    private void LateUpdate()
    {
      this.sliderTime.set_value(EnviroSky.instance.internalHour / 24f);
    }
  }
}
