// Decompiled with JetBrains decompiler
// Type: EnviroCTSIntegration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using CTS;
using UnityEngine;

[AddComponentMenu("Enviro/Integration/CTS Integration")]
public class EnviroCTSIntegration : MonoBehaviour
{
  public CTSWeatherManager ctsWeatherManager;
  public bool updateSnow;
  public bool updateWetness;
  public bool updateSeasons;
  private float daysInYear;

  public EnviroCTSIntegration()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Equality((Object) this.ctsWeatherManager, (Object) null))
      this.ctsWeatherManager = (CTSWeatherManager) Object.FindObjectOfType<CTSWeatherManager>();
    if (Object.op_Equality((Object) this.ctsWeatherManager, (Object) null))
      Debug.LogWarning((object) "CTS WeatherManager not found! Component -> CTS -> Add Weather Manager");
    else if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      Debug.LogWarning((object) "EnviroSky not found! Please add EnviroSky prefab to your scene!");
    else
      this.daysInYear = EnviroSky.instance.seasonsSettings.SpringInDays + EnviroSky.instance.seasonsSettings.SummerInDays + EnviroSky.instance.seasonsSettings.AutumnInDays + EnviroSky.instance.seasonsSettings.WinterInDays;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.ctsWeatherManager, (Object) null) || Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (this.updateSnow)
      this.ctsWeatherManager.SnowPower = EnviroSky.instance.Weather.curSnowStrength;
    if (this.updateWetness)
      this.ctsWeatherManager.RainPower = EnviroSky.instance.Weather.curWetness;
    if (!this.updateSeasons)
      return;
    this.ctsWeatherManager.Season = Mathf.Lerp(0.0f, 4f, EnviroSky.instance.currentDay / this.daysInYear);
  }
}
