// Decompiled with JetBrains decompiler
// Type: EnviroZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Enviro/Weather Zone")]
public class EnviroZone : MonoBehaviour
{
  [Tooltip("Defines the zone name.")]
  public string zoneName;
  [Tooltip("Uncheck to remove OnTriggerExit call when using overlapping zone layout.")]
  public bool ExitToDefault;
  public List<EnviroWeatherPrefab> zoneWeather;
  public List<EnviroWeatherPrefab> curPossibleZoneWeather;
  [Header("Zone weather settings:")]
  [Tooltip("Add all weather prefabs for this zone here.")]
  public List<EnviroWeatherPreset> zoneWeatherPresets;
  [Tooltip("Shall weather changes occure based on gametime or realtime?")]
  public EnviroZone.WeatherUpdateMode updateMode;
  [Tooltip("Defines how often (gametime hours or realtime minutes) the system will heck to change the current weather conditions.")]
  public float WeatherUpdateIntervall;
  [Header("Zone scaling and gizmo:")]
  [Tooltip("Defines the zone scale.")]
  public Vector3 zoneScale;
  [Tooltip("Defines the color of the zone's gizmo in editor mode.")]
  public Color zoneGizmoColor;
  [Header("Current active weather:")]
  [Tooltip("The current active weather conditions.")]
  public EnviroWeatherPrefab currentActiveZoneWeatherPrefab;
  public EnviroWeatherPreset currentActiveZoneWeatherPreset;
  [HideInInspector]
  public EnviroWeatherPrefab lastActiveZoneWeatherPrefab;
  [HideInInspector]
  public EnviroWeatherPreset lastActiveZoneWeatherPreset;
  private BoxCollider zoneCollider;
  private double nextUpdate;
  private float nextUpdateRealtime;
  private bool init;
  private bool isDefault;

  public EnviroZone()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.zoneWeatherPresets.Count > 0)
    {
      this.zoneCollider = (BoxCollider) ((Component) this).get_gameObject().AddComponent<BoxCollider>();
      ((Collider) this.zoneCollider).set_isTrigger(true);
      if (!Object.op_Implicit((Object) ((Component) this).GetComponent<EnviroSky>()))
        EnviroSky.instance.RegisterZone(this);
      else
        this.isDefault = true;
      this.UpdateZoneScale();
      this.nextUpdate = EnviroSky.instance.currentTimeInHours + (double) this.WeatherUpdateIntervall;
      this.nextUpdateRealtime = Time.get_time() + this.WeatherUpdateIntervall * 60f;
    }
    else
      Debug.LogError((object) ("Please add Weather Prefabs to Zone:" + ((Object) ((Component) this).get_gameObject()).get_name()));
  }

  public void UpdateZoneScale()
  {
    if (!this.isDefault)
      this.zoneCollider.set_size(this.zoneScale);
    else
      this.zoneCollider.set_size(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), (float) (1.0 / ((Component) EnviroSky.instance).get_transform().get_localScale().y)), 0.25f));
  }

  public void CreateZoneWeatherTypeList()
  {
    for (int index1 = 0; index1 < this.zoneWeatherPresets.Count; ++index1)
    {
      if (Object.op_Equality((Object) this.zoneWeatherPresets[index1], (Object) null))
      {
        Debug.Log((object) ("Warning! Missing Weather Preset in Zone: " + this.zoneName));
        return;
      }
      bool flag = true;
      for (int index2 = 0; index2 < EnviroSky.instance.Weather.weatherPresets.Count; ++index2)
      {
        if (Object.op_Equality((Object) this.zoneWeatherPresets[index1], (Object) EnviroSky.instance.Weather.weatherPresets[index2]))
        {
          flag = false;
          this.zoneWeather.Add(EnviroSky.instance.Weather.WeatherPrefabs[index2]);
        }
      }
      if (Object.op_Equality((Object) EnviroSky.instance.Weather.VFXHolder, (Object) null))
        flag = false;
      if (flag)
      {
        GameObject gameObject = new GameObject();
        EnviroWeatherPrefab enviroWeatherPrefab = (EnviroWeatherPrefab) gameObject.AddComponent<EnviroWeatherPrefab>();
        enviroWeatherPrefab.weatherPreset = this.zoneWeatherPresets[index1];
        ((Object) gameObject).set_name(enviroWeatherPrefab.weatherPreset.Name);
        enviroWeatherPrefab.effectEmmisionRates.Clear();
        gameObject.get_transform().set_parent(EnviroSky.instance.Weather.VFXHolder.get_transform());
        gameObject.get_transform().set_localPosition(Vector3.get_zero());
        gameObject.get_transform().set_localRotation(Quaternion.get_identity());
        this.zoneWeather.Add(enviroWeatherPrefab);
        EnviroSky.instance.Weather.WeatherPrefabs.Add(enviroWeatherPrefab);
        EnviroSky.instance.Weather.weatherPresets.Add(this.zoneWeatherPresets[index1]);
      }
    }
    for (int index1 = 0; index1 < this.zoneWeather.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.zoneWeather[index1].effectSystems.Count; ++index2)
      {
        this.zoneWeather[index1].effectEmmisionRates.Add(EnviroSky.GetEmissionRate(this.zoneWeather[index1].effectSystems[index2]));
        EnviroSky.SetEmissionRate(this.zoneWeather[index1].effectSystems[index2], 0.0f);
      }
    }
    if (this.isDefault && Object.op_Inequality((Object) EnviroSky.instance.Weather.startWeatherPreset, (Object) null))
    {
      EnviroSky.instance.SetWeatherOverwrite(EnviroSky.instance.Weather.startWeatherPreset);
      for (int index = 0; index < this.zoneWeather.Count; ++index)
      {
        if (Object.op_Equality((Object) this.zoneWeather[index].weatherPreset, (Object) EnviroSky.instance.Weather.startWeatherPreset))
        {
          this.currentActiveZoneWeatherPrefab = this.zoneWeather[index];
          this.lastActiveZoneWeatherPrefab = this.zoneWeather[index];
        }
      }
      this.currentActiveZoneWeatherPreset = EnviroSky.instance.Weather.startWeatherPreset;
      this.lastActiveZoneWeatherPreset = EnviroSky.instance.Weather.startWeatherPreset;
    }
    else
    {
      this.currentActiveZoneWeatherPrefab = this.zoneWeather[0];
      this.lastActiveZoneWeatherPrefab = this.zoneWeather[0];
      this.currentActiveZoneWeatherPreset = this.zoneWeatherPresets[0];
      this.lastActiveZoneWeatherPreset = this.zoneWeatherPresets[0];
    }
    this.nextUpdate = EnviroSky.instance.currentTimeInHours + (double) this.WeatherUpdateIntervall;
  }

  private void BuildNewWeatherList()
  {
    this.curPossibleZoneWeather = new List<EnviroWeatherPrefab>();
    for (int index = 0; index < this.zoneWeather.Count; ++index)
    {
      switch (EnviroSky.instance.Seasons.currentSeasons)
      {
        case EnviroSeasons.Seasons.Spring:
          if (this.zoneWeather[index].weatherPreset.Spring)
          {
            this.curPossibleZoneWeather.Add(this.zoneWeather[index]);
            break;
          }
          break;
        case EnviroSeasons.Seasons.Summer:
          if (this.zoneWeather[index].weatherPreset.Summer)
          {
            this.curPossibleZoneWeather.Add(this.zoneWeather[index]);
            break;
          }
          break;
        case EnviroSeasons.Seasons.Autumn:
          if (this.zoneWeather[index].weatherPreset.Autumn)
          {
            this.curPossibleZoneWeather.Add(this.zoneWeather[index]);
            break;
          }
          break;
        case EnviroSeasons.Seasons.Winter:
          if (this.zoneWeather[index].weatherPreset.winter)
          {
            this.curPossibleZoneWeather.Add(this.zoneWeather[index]);
            break;
          }
          break;
      }
    }
  }

  private EnviroWeatherPrefab PossibiltyCheck()
  {
    List<EnviroWeatherPrefab> enviroWeatherPrefabList = new List<EnviroWeatherPrefab>();
    for (int index = 0; index < this.curPossibleZoneWeather.Count; ++index)
    {
      int num = Random.Range(0, 100);
      if (EnviroSky.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Spring)
      {
        if ((double) num <= (double) this.curPossibleZoneWeather[index].weatherPreset.possibiltyInSpring)
          enviroWeatherPrefabList.Add(this.curPossibleZoneWeather[index]);
      }
      else if (EnviroSky.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Summer)
      {
        if ((double) num <= (double) this.curPossibleZoneWeather[index].weatherPreset.possibiltyInSummer)
          enviroWeatherPrefabList.Add(this.curPossibleZoneWeather[index]);
      }
      else if (EnviroSky.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Autumn)
      {
        if ((double) num <= (double) this.curPossibleZoneWeather[index].weatherPreset.possibiltyInAutumn)
          enviroWeatherPrefabList.Add(this.curPossibleZoneWeather[index]);
      }
      else if (EnviroSky.instance.Seasons.currentSeasons == EnviroSeasons.Seasons.Winter && (double) num <= (double) this.curPossibleZoneWeather[index].weatherPreset.possibiltyInWinter)
        enviroWeatherPrefabList.Add(this.curPossibleZoneWeather[index]);
    }
    if (enviroWeatherPrefabList.Count <= 0)
      return this.currentActiveZoneWeatherPrefab;
    EnviroSky.instance.NotifyZoneWeatherChanged(enviroWeatherPrefabList[0].weatherPreset, this);
    return enviroWeatherPrefabList[0];
  }

  private void WeatherUpdate()
  {
    this.nextUpdate = EnviroSky.instance.currentTimeInHours + (double) this.WeatherUpdateIntervall;
    this.nextUpdateRealtime = Time.get_time() + this.WeatherUpdateIntervall * 60f;
    this.BuildNewWeatherList();
    this.lastActiveZoneWeatherPrefab = this.currentActiveZoneWeatherPrefab;
    this.lastActiveZoneWeatherPreset = this.currentActiveZoneWeatherPreset;
    this.currentActiveZoneWeatherPrefab = this.PossibiltyCheck();
    this.currentActiveZoneWeatherPreset = this.currentActiveZoneWeatherPrefab.weatherPreset;
    EnviroSky.instance.NotifyZoneWeatherChanged(this.currentActiveZoneWeatherPreset, this);
  }

  [DebuggerHidden]
  private IEnumerator CreateWeatherListLate()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroZone.\u003CCreateWeatherListLate\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
    {
      Debug.Log((object) "No EnviroSky instance found!");
    }
    else
    {
      if (EnviroSky.instance.started && !this.init)
      {
        if (this.isDefault)
        {
          this.CreateZoneWeatherTypeList();
          this.init = true;
        }
        else
          this.StartCoroutine(this.CreateWeatherListLate());
      }
      if (this.updateMode == EnviroZone.WeatherUpdateMode.GameTimeHours)
      {
        if (EnviroSky.instance.currentTimeInHours > this.nextUpdate && EnviroSky.instance.Weather.updateWeather && EnviroSky.instance.started)
          this.WeatherUpdate();
      }
      else if ((double) Time.get_time() > (double) this.nextUpdateRealtime && EnviroSky.instance.Weather.updateWeather && EnviroSky.instance.started)
        this.WeatherUpdate();
      if (Object.op_Equality((Object) EnviroSky.instance.Player, (Object) null) || !this.isDefault || !this.init)
        return;
      this.zoneCollider.set_center(new Vector3(0.0f, (float) ((EnviroSky.instance.Player.get_transform().get_position().y - ((Component) EnviroSky.instance).get_transform().get_position().y) / ((Component) EnviroSky.instance).get_transform().get_lossyScale().y), 0.0f));
    }
  }

  private void OnTriggerEnter(Collider col)
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (EnviroSky.instance.profile.weatherSettings.useTag)
    {
      if (!(((Component) col).get_gameObject().get_tag() == ((Component) EnviroSky.instance).get_gameObject().get_tag()))
        return;
      EnviroSky.instance.Weather.currentActiveZone = this;
      EnviroSky.instance.NotifyZoneChanged(this);
    }
    else
    {
      if (!Object.op_Implicit((Object) ((Component) col).get_gameObject().GetComponent<EnviroSky>()))
        return;
      EnviroSky.instance.Weather.currentActiveZone = this;
      EnviroSky.instance.NotifyZoneChanged(this);
    }
  }

  private void OnTriggerExit(Collider col)
  {
    if (!this.ExitToDefault || Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (EnviroSky.instance.profile.weatherSettings.useTag)
    {
      if (!(((Component) col).get_gameObject().get_tag() == ((Component) EnviroSky.instance).get_gameObject().get_tag()))
        return;
      EnviroSky.instance.Weather.currentActiveZone = EnviroSky.instance.Weather.zones[0];
      EnviroSky.instance.NotifyZoneChanged(EnviroSky.instance.Weather.zones[0]);
    }
    else
    {
      if (!Object.op_Implicit((Object) ((Component) col).get_gameObject().GetComponent<EnviroSky>()))
        return;
      EnviroSky.instance.Weather.currentActiveZone = EnviroSky.instance.Weather.zones[0];
      EnviroSky.instance.NotifyZoneChanged(EnviroSky.instance.Weather.zones[0]);
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(this.zoneGizmoColor);
    Gizmos.DrawCube(((Component) this).get_transform().get_position(), new Vector3((float) this.zoneScale.x, (float) this.zoneScale.y, (float) this.zoneScale.z));
  }

  public enum WeatherUpdateMode
  {
    GameTimeHours,
    RealTimeMinutes,
  }
}
