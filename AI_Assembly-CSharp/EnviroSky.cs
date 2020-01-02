// Decompiled with JetBrains decompiler
// Type: EnviroSky
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class EnviroSky : MonoBehaviour
{
  private static EnviroSky _instance;
  public string prefabVersion;
  [Tooltip("Assign your player gameObject here. Required Field! or enable AssignInRuntime!")]
  public GameObject Player;
  [Tooltip("Assign your main camera here. Required Field! or enable AssignInRuntime!")]
  public Camera PlayerCamera;
  [Tooltip("If enabled Enviro will search for your Player and Camera by Tag!")]
  public bool AssignInRuntime;
  [Tooltip("Your Player Tag")]
  public string PlayerTag;
  [Tooltip("Your CameraTag")]
  public string CameraTag;
  [Header("General")]
  [Tooltip("Enable this when using singlepass rendering.")]
  public bool dontDestroy;
  [Header("Camera Settings")]
  [Tooltip("Enable HDR Rendering. You want to use a third party tonemapping effect for best results!")]
  public bool HDR;
  [Header("Layer Setup")]
  [Tooltip("This is the layer id forfor the moon.")]
  public int moonRenderingLayer;
  [Tooltip("This is the layer id for additional satellites like moons, planets.")]
  public int satelliteRenderingLayer;
  [Tooltip("Activate to set recommended maincamera clear flag.")]
  public bool setCameraClearFlags;
  [Header("Virtual Reality")]
  [Tooltip("Enable this when using singlepass rendering.")]
  public bool singlePassVR;
  [Tooltip("Enable this to activate volume lighing")]
  [HideInInspector]
  public bool volumeLighting;
  [Tooltip("Enable this to activate global scattering fog. Disabled will also disable volume lighting")]
  [HideInInspector]
  public bool globalFog;
  [Header("Profile")]
  public EnviroProfile profile;
  [Header("Control")]
  public EnviroTime GameTime;
  public EnviroAudio Audio;
  public EnviroWeather Weather;
  public EnviroSeasons Seasons;
  public EnviroFogging Fog;
  public EnviroLightshafts LightShafts;
  [Header("Components")]
  public EnviroComponents Components;
  [HideInInspector]
  public bool started;
  [HideInInspector]
  public bool isNight;
  [HideInInspector]
  public EnviroLightSettings lightSettings;
  [HideInInspector]
  public EnviroVolumeLightingSettings volumeLightSettings;
  [HideInInspector]
  public EnviroSkySettings skySettings;
  [HideInInspector]
  public EnviroCloudSettings cloudsSettings;
  [HideInInspector]
  public EnviroWeatherSettings weatherSettings;
  [HideInInspector]
  public EnviroFogSettings fogSettings;
  [HideInInspector]
  public EnviroLightShaftsSettings lightshaftsSettings;
  [HideInInspector]
  public EnviroSeasonSettings seasonsSettings;
  [HideInInspector]
  public EnviroAudioSettings audioSettings;
  [HideInInspector]
  public EnviroSatellitesSettings satelliteSettings;
  [HideInInspector]
  public EnviroQualitySettings qualitySettings;
  public EnviroSky.EnviroCloudsMode cloudsMode;
  private EnviroSky.EnviroCloudsMode lastCloudsMode;
  private EnviroCloudSettings.CloudQuality lastCloudsQuality;
  private Material cloudShadows;
  [HideInInspector]
  public Camera moonCamera;
  [HideInInspector]
  public Camera satCamera;
  [HideInInspector]
  public EnviroVolumeLight directVolumeLight;
  [HideInInspector]
  public EnviroLightShafts lightShaftsScriptSun;
  [HideInInspector]
  public EnviroLightShafts lightShaftsScriptMoon;
  [HideInInspector]
  public EnviroSkyRendering EnviroSkyRender;
  [HideInInspector]
  public GameObject EffectsHolder;
  [HideInInspector]
  public EnviroAudioSource AudioSourceWeather;
  [HideInInspector]
  public EnviroAudioSource AudioSourceWeather2;
  [HideInInspector]
  public EnviroAudioSource AudioSourceAmbient;
  [HideInInspector]
  public EnviroAudioSource AudioSourceAmbient2;
  [HideInInspector]
  public AudioSource AudioSourceThunder;
  [HideInInspector]
  public EnviroAudioSource AudioSourceZone;
  [HideInInspector]
  public List<EnviroVegetationInstance> EnviroVegetationInstances;
  [HideInInspector]
  public Color currentWeatherSkyMod;
  [HideInInspector]
  public Color currentWeatherLightMod;
  [HideInInspector]
  public Color currentWeatherFogMod;
  [HideInInspector]
  public Color currentInteriorDirectLightMod;
  [HideInInspector]
  public Color currentInteriorAmbientLightMod;
  [HideInInspector]
  public Color currentInteriorAmbientEQLightMod;
  [HideInInspector]
  public Color currentInteriorAmbientGRLightMod;
  [HideInInspector]
  public Color currentInteriorSkyboxMod;
  [HideInInspector]
  public Color currentInteriorFogColorMod;
  [HideInInspector]
  public float currentInteriorFogMod;
  [HideInInspector]
  public float currentInteriorWeatherEffectMod;
  [HideInInspector]
  public float currentInteriorZoneAudioVolume;
  [HideInInspector]
  public float currentInteriorZoneAudioFadingSpeed;
  [HideInInspector]
  public float globalVolumeLightIntensity;
  [HideInInspector]
  public EnviroWeatherCloudsConfig cloudsConfig;
  [HideInInspector]
  public float thunder;
  [HideInInspector]
  public List<GameObject> satellites;
  [HideInInspector]
  public List<GameObject> satellitesRotation;
  [HideInInspector]
  public DateTime dateTime;
  [HideInInspector]
  public float internalHour;
  [HideInInspector]
  public float currentHour;
  [HideInInspector]
  public float currentDay;
  [HideInInspector]
  public float currentYear;
  [HideInInspector]
  public double currentTimeInHours;
  [HideInInspector]
  public RenderTexture cloudsRenderTarget;
  [HideInInspector]
  public RenderTexture flatCloudsRenderTarget;
  [HideInInspector]
  public Material flatCloudsMat;
  [HideInInspector]
  public RenderTexture weatherMap;
  [HideInInspector]
  public RenderTexture moonRenderTarget;
  [HideInInspector]
  public RenderTexture satRenderTarget;
  [HideInInspector]
  public float customMoonPhase;
  [HideInInspector]
  public bool updateFogDensity;
  [HideInInspector]
  public Color customFogColor;
  [HideInInspector]
  public float customFogIntensity;
  [HideInInspector]
  public bool profileLoaded;
  [HideInInspector]
  public bool interiorMode;
  [HideInInspector]
  public EnviroInterior lastInteriorZone;
  [HideInInspector]
  public Vector2 cloudAnim;
  [HideInInspector]
  public Vector2 cloudAnimNonScaled;
  [HideInInspector]
  public Material skyMat;
  private Transform DomeTransform;
  private Transform SunTransform;
  private Light MainLight;
  private Transform MoonTransform;
  private Renderer MoonRenderer;
  private Material MoonShader;
  private float lastHourUpdate;
  private float starsRot;
  private float lastHour;
  private double lastRelfectionUpdate;
  private double lastMoonUpdate;
  private float lastAmbientSkyUpdate;
  private bool serverMode;
  private RenderTexture cloudShadowMap;
  private Material cloudShadowMat;
  private const float pi = 3.141593f;
  private Vector3 K;
  private const float n = 1.0003f;
  private const float N = 2.545E+25f;
  private const float pn = 0.035f;
  private float hourTime;
  private float LST;
  private ParticleSystem lightningEffect;
  [HideInInspector]
  public bool showSettings;
  private Material weatherMapMat;

  public EnviroSky()
  {
    base.\u002Ector();
  }

  public static EnviroSky instance
  {
    get
    {
      if (Object.op_Equality((Object) EnviroSky._instance, (Object) null))
        EnviroSky._instance = (EnviroSky) Object.FindObjectOfType<EnviroSky>();
      return EnviroSky._instance;
    }
  }

  private float OrbitRadius
  {
    get
    {
      return (float) this.DomeTransform.get_localScale().x;
    }
  }

  public event EnviroSky.HourPassed OnHourPassed;

  public event EnviroSky.DayPassed OnDayPassed;

  public event EnviroSky.YearPassed OnYearPassed;

  public event EnviroSky.WeatherChanged OnWeatherChanged;

  public event EnviroSky.ZoneWeatherChanged OnZoneWeatherChanged;

  public event EnviroSky.SeasonChanged OnSeasonChanged;

  public event EnviroSky.isNightE OnNightTime;

  public event EnviroSky.isDay OnDayTime;

  public event EnviroSky.ZoneChanged OnZoneChanged;

  public virtual void NotifyHourPassed()
  {
    if (this.OnHourPassed == null)
      return;
    this.OnHourPassed();
  }

  public virtual void NotifyDayPassed()
  {
    if (this.OnDayPassed == null)
      return;
    this.OnDayPassed();
  }

  public virtual void NotifyYearPassed()
  {
    if (this.OnYearPassed == null)
      return;
    this.OnYearPassed();
  }

  public virtual void NotifyWeatherChanged(EnviroWeatherPreset type)
  {
    if (this.OnWeatherChanged == null)
      return;
    this.OnWeatherChanged(type);
  }

  public virtual void NotifyZoneWeatherChanged(EnviroWeatherPreset type, EnviroZone zone)
  {
    if (this.OnZoneWeatherChanged == null)
      return;
    this.OnZoneWeatherChanged(type, zone);
  }

  public virtual void NotifySeasonChanged(EnviroSeasons.Seasons season)
  {
    if (this.OnSeasonChanged == null)
      return;
    this.OnSeasonChanged(season);
  }

  public virtual void NotifyIsNight()
  {
    if (this.OnNightTime == null)
      return;
    this.OnNightTime();
  }

  public virtual void NotifyIsDay()
  {
    if (this.OnDayTime == null)
      return;
    this.OnDayTime();
  }

  public virtual void NotifyZoneChanged(EnviroZone zone)
  {
    if (this.OnZoneChanged == null)
      return;
    this.OnZoneChanged(zone);
  }

  private void Start()
  {
    this.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
    this.lastHourUpdate = (float) Mathf.RoundToInt(this.internalHour);
    this.currentTimeInHours = this.GetInHours(this.internalHour, (float) this.GameTime.Days, (float) this.GameTime.Years);
    this.Weather.weatherFullyChanged = false;
    this.thunder = 0.0f;
    this.lastCloudsQuality = this.cloudsSettings.cloudsQuality;
    if (Application.get_isPlaying() && this.dontDestroy)
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    if (Object.op_Equality((Object) this.weatherMapMat, (Object) null))
      this.weatherMapMat = new Material(Shader.Find("Enviro/WeatherMap"));
    if (!this.profileLoaded)
      return;
    this.InvokeRepeating("UpdateEnviroment", 0.0f, this.qualitySettings.UpdateInterval);
    this.CreateEffects();
    if (!Object.op_Inequality((Object) this.PlayerCamera, (Object) null) || !Object.op_Inequality((Object) this.Player, (Object) null) || (this.AssignInRuntime || !Object.op_Inequality((Object) this.profile, (Object) null)))
      return;
    this.Init();
  }

  private void OnEnable()
  {
    this.DomeTransform = ((Component) this).get_transform();
    this.Weather.currentActiveWeatherPreset = this.Weather.zones[0].currentActiveZoneWeatherPreset;
    this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
    if (Object.op_Equality((Object) this.profile, (Object) null))
    {
      Debug.LogError((object) "No profile assigned!");
    }
    else
    {
      if (!this.profileLoaded)
        this.ApplyProfile(this.profile);
      this.PreInit();
      if (this.AssignInRuntime)
      {
        this.started = false;
      }
      else
      {
        if (!Object.op_Inequality((Object) this.PlayerCamera, (Object) null) || !Object.op_Inequality((Object) this.Player, (Object) null))
          return;
        this.Init();
      }
    }
  }

  public void ApplyProfile(EnviroProfile p)
  {
    this.profile = p;
    this.lightSettings = (EnviroLightSettings) JsonUtility.FromJson<EnviroLightSettings>(JsonUtility.ToJson((object) p.lightSettings));
    this.volumeLightSettings = (EnviroVolumeLightingSettings) JsonUtility.FromJson<EnviroVolumeLightingSettings>(JsonUtility.ToJson((object) p.volumeLightSettings));
    this.skySettings = (EnviroSkySettings) JsonUtility.FromJson<EnviroSkySettings>(JsonUtility.ToJson((object) p.skySettings));
    this.cloudsSettings = (EnviroCloudSettings) JsonUtility.FromJson<EnviroCloudSettings>(JsonUtility.ToJson((object) p.cloudsSettings));
    this.weatherSettings = (EnviroWeatherSettings) JsonUtility.FromJson<EnviroWeatherSettings>(JsonUtility.ToJson((object) p.weatherSettings));
    this.fogSettings = (EnviroFogSettings) JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson((object) p.fogSettings));
    this.lightshaftsSettings = (EnviroLightShaftsSettings) JsonUtility.FromJson<EnviroLightShaftsSettings>(JsonUtility.ToJson((object) p.lightshaftsSettings));
    this.audioSettings = (EnviroAudioSettings) JsonUtility.FromJson<EnviroAudioSettings>(JsonUtility.ToJson((object) p.audioSettings));
    this.satelliteSettings = (EnviroSatellitesSettings) JsonUtility.FromJson<EnviroSatellitesSettings>(JsonUtility.ToJson((object) p.satelliteSettings));
    this.qualitySettings = (EnviroQualitySettings) JsonUtility.FromJson<EnviroQualitySettings>(JsonUtility.ToJson((object) p.qualitySettings));
    this.seasonsSettings = (EnviroSeasonSettings) JsonUtility.FromJson<EnviroSeasonSettings>(JsonUtility.ToJson((object) p.seasonsSettings));
    this.profileLoaded = true;
  }

  public void SaveProfile()
  {
    this.profile.lightSettings = (EnviroLightSettings) JsonUtility.FromJson<EnviroLightSettings>(JsonUtility.ToJson((object) this.lightSettings));
    this.profile.volumeLightSettings = (EnviroVolumeLightingSettings) JsonUtility.FromJson<EnviroVolumeLightingSettings>(JsonUtility.ToJson((object) this.volumeLightSettings));
    this.profile.skySettings = (EnviroSkySettings) JsonUtility.FromJson<EnviroSkySettings>(JsonUtility.ToJson((object) this.skySettings));
    this.profile.cloudsSettings = (EnviroCloudSettings) JsonUtility.FromJson<EnviroCloudSettings>(JsonUtility.ToJson((object) this.cloudsSettings));
    this.profile.weatherSettings = (EnviroWeatherSettings) JsonUtility.FromJson<EnviroWeatherSettings>(JsonUtility.ToJson((object) this.weatherSettings));
    this.profile.fogSettings = (EnviroFogSettings) JsonUtility.FromJson<EnviroFogSettings>(JsonUtility.ToJson((object) this.fogSettings));
    this.profile.lightshaftsSettings = (EnviroLightShaftsSettings) JsonUtility.FromJson<EnviroLightShaftsSettings>(JsonUtility.ToJson((object) this.lightshaftsSettings));
    this.profile.audioSettings = (EnviroAudioSettings) JsonUtility.FromJson<EnviroAudioSettings>(JsonUtility.ToJson((object) this.audioSettings));
    this.profile.satelliteSettings = (EnviroSatellitesSettings) JsonUtility.FromJson<EnviroSatellitesSettings>(JsonUtility.ToJson((object) this.satelliteSettings));
    this.profile.qualitySettings = (EnviroQualitySettings) JsonUtility.FromJson<EnviroQualitySettings>(JsonUtility.ToJson((object) this.qualitySettings));
    this.profile.seasonsSettings = (EnviroSeasonSettings) JsonUtility.FromJson<EnviroSeasonSettings>(JsonUtility.ToJson((object) this.seasonsSettings));
  }

  public void ReInit()
  {
    this.OnEnable();
  }

  private void PreInit()
  {
    this.isNight = (double) this.GameTime.solarTime < 0.449999988079071;
    if (this.serverMode)
      return;
    this.CheckSatellites();
    RenderSettings.set_fogMode(this.fogSettings.Fogmode);
    this.SetupSkybox();
    RenderSettings.set_ambientMode(this.lightSettings.ambientMode);
    RenderSettings.set_fogDensity(0.0f);
    RenderSettings.set_fogStartDistance(0.0f);
    RenderSettings.set_fogEndDistance(1000f);
    this.Components.GlobalReflectionProbe.set_size(((Component) this).get_transform().get_localScale());
    this.Components.GlobalReflectionProbe.set_refreshMode((ReflectionProbeRefreshMode) 2);
    if (Object.op_Implicit((Object) this.Components.Sun))
      this.SunTransform = this.Components.Sun.get_transform();
    else
      Debug.LogError((object) "Please set sun object in inspector!");
    if (Object.op_Implicit((Object) this.Components.Moon))
    {
      this.MoonTransform = this.Components.Moon.get_transform();
      this.MoonRenderer = (Renderer) this.Components.Moon.GetComponent<Renderer>();
      if (Object.op_Equality((Object) this.MoonRenderer, (Object) null))
        this.MoonRenderer = (Renderer) this.Components.Moon.AddComponent<MeshRenderer>();
      this.MoonRenderer.set_shadowCastingMode((ShadowCastingMode) 0);
      this.MoonRenderer.set_receiveShadows(false);
      if (Object.op_Inequality((Object) this.MoonRenderer.get_sharedMaterial(), (Object) null))
        Object.DestroyImmediate((Object) this.MoonRenderer.get_sharedMaterial());
      this.MoonShader = this.skySettings.moonPhaseMode != EnviroSkySettings.MoonPhases.Realistic ? new Material(Shader.Find("Enviro/MoonShaderPhased")) : new Material(Shader.Find("Enviro/MoonShader"));
      this.MoonShader.SetTexture("_MainTex", this.skySettings.moonTexture);
      this.MoonRenderer.set_sharedMaterial(this.MoonShader);
      this.customMoonPhase = this.skySettings.startMoonPhase;
    }
    else
      Debug.LogError((object) "Please set moon object in inspector!");
    if (Object.op_Inequality((Object) this.Components.cloudsShadowPlane, (Object) null))
      Object.DestroyImmediate((Object) this.Components.cloudsShadowPlane);
    if (Object.op_Equality((Object) this.weatherMap, (Object) null))
    {
      this.weatherMap = new RenderTexture(1024, 1024, 0, (RenderTextureFormat) 7);
      ((Texture) this.weatherMap).set_wrapMode((TextureWrapMode) 0);
    }
    if (Object.op_Inequality((Object) this.cloudShadows, (Object) null) && Object.op_Inequality((Object) this.weatherMap, (Object) null))
      this.cloudShadows.SetTexture("_MainTex", (Texture) this.weatherMap);
    if (Object.op_Implicit((Object) this.Components.DirectLight))
    {
      if (((Object) this.Components.DirectLight).get_name() == "Direct Lght")
      {
        Object.DestroyImmediate((Object) ((Component) this.Components.DirectLight).get_gameObject());
        this.Components.DirectLight = this.CreateDirectionalLight();
      }
      this.MainLight = (Light) ((Component) this.Components.DirectLight).GetComponent<Light>();
      if (Object.op_Equality((Object) this.directVolumeLight, (Object) null))
        this.directVolumeLight = (EnviroVolumeLight) ((Component) this.Components.DirectLight).GetComponent<EnviroVolumeLight>();
      if (Object.op_Equality((Object) this.directVolumeLight, (Object) null))
        this.directVolumeLight = (EnviroVolumeLight) ((Component) this.Components.DirectLight).get_gameObject().AddComponent<EnviroVolumeLight>();
      if (this.dontDestroy && Application.get_isPlaying())
        Object.DontDestroyOnLoad((Object) this.Components.DirectLight);
    }
    else
    {
      GameObject gameObject = GameObject.Find("Enviro Directional Light");
      this.Components.DirectLight = !Object.op_Inequality((Object) gameObject, (Object) null) ? this.CreateDirectionalLight() : gameObject.get_transform();
      this.MainLight = (Light) ((Component) this.Components.DirectLight).GetComponent<Light>();
      if (Object.op_Equality((Object) this.directVolumeLight, (Object) null))
        this.directVolumeLight = (EnviroVolumeLight) ((Component) this.Components.DirectLight).GetComponent<EnviroVolumeLight>();
      if (Object.op_Equality((Object) this.directVolumeLight, (Object) null))
        this.directVolumeLight = (EnviroVolumeLight) ((Component) this.Components.DirectLight).get_gameObject().AddComponent<EnviroVolumeLight>();
      if (this.dontDestroy && Application.get_isPlaying())
        Object.DontDestroyOnLoad((Object) this.Components.DirectLight);
    }
    if (Object.op_Inequality((Object) this.cloudShadowMap, (Object) null))
      Object.DestroyImmediate((Object) this.cloudShadowMap);
    this.cloudShadowMap = new RenderTexture(2048, 2048, 0, (RenderTextureFormat) 7);
    ((Texture) this.cloudShadowMap).set_wrapMode((TextureWrapMode) 0);
    if (Object.op_Inequality((Object) this.cloudShadowMat, (Object) null))
      Object.DestroyImmediate((Object) this.cloudShadowMat);
    this.cloudShadowMat = new Material(Shader.Find("Enviro/ShadowCookie"));
    if ((double) this.cloudsSettings.shadowIntensity > 0.0)
    {
      Graphics.Blit((Texture) this.weatherMap, this.cloudShadowMap, this.cloudShadowMat);
      this.MainLight.set_cookie((Texture) this.cloudShadowMap);
      this.MainLight.set_cookieSize(10000f);
    }
    else
      this.MainLight.set_cookie((Texture) null);
  }

  private void SetupSkybox()
  {
    if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.Default)
    {
      if (Object.op_Inequality((Object) this.skyMat, (Object) null))
        Object.DestroyImmediate((Object) this.skyMat);
      this.skyMat = this.cloudsMode == EnviroSky.EnviroCloudsMode.None || this.cloudsMode == EnviroSky.EnviroCloudsMode.Volume ? new Material(Shader.Find("Enviro/Skybox")) : new Material(Shader.Find("Enviro/SkyboxFlatClouds"));
      if (Object.op_Inequality((Object) this.skySettings.starsCubeMap, (Object) null))
        this.skyMat.SetTexture("_Stars", (Texture) this.skySettings.starsCubeMap);
      if (Object.op_Inequality((Object) this.skySettings.galaxyCubeMap, (Object) null))
        this.skyMat.SetTexture("_Galaxy", (Texture) this.skySettings.galaxyCubeMap);
      RenderSettings.set_skybox(this.skyMat);
    }
    else if (this.skySettings.skyboxMode == EnviroSkySettings.SkyboxModi.CustomSkybox)
    {
      if (Object.op_Inequality((Object) this.skySettings.customSkyboxMaterial, (Object) null))
        RenderSettings.set_skybox(this.skySettings.customSkyboxMaterial);
      this.skyMat = this.skySettings.customSkyboxMaterial;
    }
    this.lastCloudsMode = this.cloudsMode;
    if (this.lightSettings.ambientMode != null)
      return;
    this.StartCoroutine(this.UpdateAmbientLightWithDelay());
  }

  [DebuggerHidden]
  private IEnumerator UpdateAmbientLightWithDelay()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    EnviroSky.\u003CUpdateAmbientLightWithDelay\u003Ec__Iterator0 withDelayCIterator0 = new EnviroSky.\u003CUpdateAmbientLightWithDelay\u003Ec__Iterator0();
    return (IEnumerator) withDelayCIterator0;
  }

  private void Init()
  {
    if (Object.op_Equality((Object) this.profile, (Object) null))
      return;
    if (this.serverMode)
    {
      this.started = true;
    }
    else
    {
      this.InitImageEffects();
      if (Object.op_Inequality((Object) this.PlayerCamera, (Object) null))
      {
        if (this.setCameraClearFlags)
          this.PlayerCamera.set_clearFlags((CameraClearFlags) 1);
        if (this.PlayerCamera.get_actualRenderingPath() == 3)
          this.SetCameraHDR(this.PlayerCamera, true);
        else
          this.SetCameraHDR(this.PlayerCamera, this.HDR);
        this.Components.GlobalReflectionProbe.set_farClipPlane(this.PlayerCamera.get_farClipPlane());
        if (Object.op_Equality((Object) this.moonCamera, (Object) null))
        {
          this.CreateMoonCamera();
        }
        else
        {
          this.moonCamera.set_cullingMask(1 << this.moonRenderingLayer);
          this.moonCamera.set_farClipPlane(this.PlayerCamera.get_farClipPlane() * 0.5f);
          Camera[] objectsOfType = (Camera[]) Object.FindObjectsOfType<Camera>();
          for (int index = 0; index < objectsOfType.Length; ++index)
          {
            if (Object.op_Inequality((Object) objectsOfType[index], (Object) this.moonCamera))
            {
              Camera camera = objectsOfType[index];
              camera.set_cullingMask(camera.get_cullingMask() & ~(1 << this.moonRenderingLayer));
            }
          }
          ((Component) this.moonCamera).get_transform().SetParent(this.Components.Moon.get_transform(), false);
          ((Component) this.moonCamera).get_transform().set_localPosition(new Vector3(0.0f, 0.0f, 1.5f));
          ((Component) this.moonCamera).get_transform().set_localEulerAngles(new Vector3(-180f, 0.0f, 45f));
          ((Component) this.moonCamera).get_transform().set_localScale(Vector3.get_one());
          ((Behaviour) this.moonCamera).set_enabled(false);
        }
      }
      this.CreateMoonTexture();
      this.Components.Moon.set_layer(this.moonRenderingLayer);
      Object.DestroyImmediate((Object) GameObject.Find("Enviro Cameras"));
      if (this.satelliteSettings.additionalSatellites.Count > 0)
        this.InitSatCamera();
      this.started = true;
      if (Object.op_Inequality((Object) this.MoonShader, (Object) null))
      {
        this.MoonShader.SetFloat("_Phase", this.customMoonPhase);
        this.MoonShader.SetColor("_Color", this.skySettings.moonColor);
        this.MoonShader.SetFloat("_Brightness", this.skySettings.moonBrightness * (1f - this.GameTime.solarTime));
      }
      if (!Object.op_Inequality((Object) this.moonCamera, (Object) null))
        return;
      this.moonCamera.Render();
    }
  }

  public void SetCameraHDR(Camera cam, bool hdr)
  {
    cam.set_allowHDR(hdr);
  }

  public bool GetCameraHDR(Camera cam)
  {
    return cam.get_allowHDR();
  }

  private Transform CreateDirectionalLight()
  {
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("Enviro Directional Light");
    gameObject.get_transform().set_parent(((Component) this).get_transform());
    gameObject.get_transform().set_parent((Transform) null);
    Light light = (Light) gameObject.AddComponent<Light>();
    light.set_type((LightType) 1);
    light.set_shadows((LightShadows) 2);
    return gameObject.get_transform();
  }

  private void InitImageEffects()
  {
    this.EnviroSkyRender = (EnviroSkyRendering) ((Component) this.PlayerCamera).get_gameObject().GetComponent<EnviroSkyRendering>();
    if (Object.op_Equality((Object) this.EnviroSkyRender, (Object) null))
      this.EnviroSkyRender = (EnviroSkyRendering) ((Component) this.PlayerCamera).get_gameObject().AddComponent<EnviroSkyRendering>();
    EnviroLightShafts[] components = (EnviroLightShafts[]) ((Component) this.PlayerCamera).get_gameObject().GetComponents<EnviroLightShafts>();
    if (components.Length > 0)
      this.lightShaftsScriptSun = components[0];
    if (Object.op_Inequality((Object) this.lightShaftsScriptSun, (Object) null))
    {
      Object.DestroyImmediate((Object) this.lightShaftsScriptSun.sunShaftsMaterial);
      Object.DestroyImmediate((Object) this.lightShaftsScriptSun.simpleClearMaterial);
      this.lightShaftsScriptSun.sunShaftsMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));
      this.lightShaftsScriptSun.sunShaftsShader = this.lightShaftsScriptSun.sunShaftsMaterial.get_shader();
      this.lightShaftsScriptSun.simpleClearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));
      this.lightShaftsScriptSun.simpleClearShader = this.lightShaftsScriptSun.simpleClearMaterial.get_shader();
    }
    else
    {
      this.lightShaftsScriptSun = (EnviroLightShafts) ((Component) this.PlayerCamera).get_gameObject().AddComponent<EnviroLightShafts>();
      this.lightShaftsScriptSun.sunShaftsMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));
      this.lightShaftsScriptSun.sunShaftsShader = this.lightShaftsScriptSun.sunShaftsMaterial.get_shader();
      this.lightShaftsScriptSun.simpleClearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));
      this.lightShaftsScriptSun.simpleClearShader = this.lightShaftsScriptSun.simpleClearMaterial.get_shader();
    }
    if (components.Length > 1)
      this.lightShaftsScriptMoon = components[1];
    if (Object.op_Inequality((Object) this.lightShaftsScriptMoon, (Object) null))
    {
      Object.DestroyImmediate((Object) this.lightShaftsScriptMoon.sunShaftsMaterial);
      Object.DestroyImmediate((Object) this.lightShaftsScriptMoon.simpleClearMaterial);
      this.lightShaftsScriptMoon.sunShaftsMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));
      this.lightShaftsScriptMoon.sunShaftsShader = this.lightShaftsScriptMoon.sunShaftsMaterial.get_shader();
      this.lightShaftsScriptMoon.simpleClearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));
      this.lightShaftsScriptMoon.simpleClearShader = this.lightShaftsScriptMoon.simpleClearMaterial.get_shader();
    }
    else
    {
      this.lightShaftsScriptMoon = (EnviroLightShafts) ((Component) this.PlayerCamera).get_gameObject().AddComponent<EnviroLightShafts>();
      this.lightShaftsScriptMoon.sunShaftsMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));
      this.lightShaftsScriptMoon.sunShaftsShader = this.lightShaftsScriptMoon.sunShaftsMaterial.get_shader();
      this.lightShaftsScriptMoon.simpleClearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));
      this.lightShaftsScriptMoon.simpleClearShader = this.lightShaftsScriptMoon.simpleClearMaterial.get_shader();
    }
  }

  public void InitSatCamera()
  {
    foreach (Camera camera in (Camera[]) Object.FindObjectsOfType<Camera>())
      camera.set_cullingMask(camera.get_cullingMask() & ~(1 << this.satelliteRenderingLayer));
    Object.DestroyImmediate((Object) GameObject.Find("Enviro Sat Camera"));
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("Enviro Sat Camera");
    gameObject.get_transform().set_position(((Component) this.PlayerCamera).get_transform().get_position());
    gameObject.get_transform().set_rotation(((Component) this.PlayerCamera).get_transform().get_rotation());
    ((Object) gameObject).set_hideFlags((HideFlags) 52);
    this.satCamera = (Camera) gameObject.AddComponent<Camera>();
    this.satCamera.set_farClipPlane(this.PlayerCamera.get_farClipPlane());
    this.satCamera.set_nearClipPlane(this.PlayerCamera.get_nearClipPlane());
    this.satCamera.set_aspect(this.PlayerCamera.get_aspect());
    this.SetCameraHDR(this.satCamera, this.HDR);
    this.satCamera.set_useOcclusionCulling(false);
    this.satCamera.set_renderingPath((RenderingPath) 1);
    this.satCamera.set_fieldOfView(this.PlayerCamera.get_fieldOfView());
    this.satCamera.set_clearFlags((CameraClearFlags) 2);
    this.satCamera.set_backgroundColor(new Color(0.0f, 0.0f, 0.0f, 0.0f));
    this.satCamera.set_cullingMask(1 << this.satelliteRenderingLayer);
    this.satCamera.set_depth(this.PlayerCamera.get_depth() + 1f);
    ((Behaviour) this.satCamera).set_enabled(true);
    Camera playerCamera = this.PlayerCamera;
    playerCamera.set_cullingMask(playerCamera.get_cullingMask() & ~(1 << this.satelliteRenderingLayer));
    RenderTextureFormat renderTextureFormat1 = !this.GetCameraHDR(this.satCamera) ? (RenderTextureFormat) 7 : (RenderTextureFormat) 9;
    Resolution currentResolution1 = Screen.get_currentResolution();
    int width = ((Resolution) ref currentResolution1).get_width();
    Resolution currentResolution2 = Screen.get_currentResolution();
    int height = ((Resolution) ref currentResolution2).get_height();
    RenderTextureFormat renderTextureFormat2 = renderTextureFormat1;
    this.satRenderTarget = new RenderTexture(width, height, 16, renderTextureFormat2);
    this.satCamera.set_targetTexture(this.satRenderTarget);
    ((Behaviour) this.satCamera).set_enabled(false);
  }

  private void CreateMoonCamera()
  {
    foreach (Camera camera in (Camera[]) Object.FindObjectsOfType<Camera>())
      camera.set_cullingMask(camera.get_cullingMask() & ~(1 << this.moonRenderingLayer));
    Object.DestroyImmediate((Object) GameObject.Find("Enviro Moon Render Cam"));
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("Enviro Moon Render Cam");
    gameObject.get_transform().SetParent(this.Components.Moon.get_transform(), false);
    gameObject.get_transform().set_localPosition(new Vector3(0.0f, 0.0f, 1.5f));
    gameObject.get_transform().set_localEulerAngles(new Vector3(-180f, 0.0f, 45f));
    gameObject.get_transform().set_localScale(Vector3.get_one());
    this.moonCamera = (Camera) gameObject.AddComponent<Camera>();
    this.moonCamera.set_farClipPlane(this.PlayerCamera.get_farClipPlane() * 0.5f);
    this.moonCamera.set_nearClipPlane(0.01f);
    this.moonCamera.set_aspect(this.PlayerCamera.get_aspect());
    this.SetCameraHDR(this.moonCamera, this.HDR);
    this.moonCamera.set_renderingPath((RenderingPath) 1);
    this.moonCamera.set_fieldOfView(this.PlayerCamera.get_fieldOfView());
    this.moonCamera.set_clearFlags((CameraClearFlags) 2);
    this.moonCamera.set_backgroundColor(Color.get_black());
    this.moonCamera.set_cullingMask(1 << this.moonRenderingLayer);
    Camera playerCamera = this.PlayerCamera;
    playerCamera.set_cullingMask(playerCamera.get_cullingMask() & ~(1 << this.moonRenderingLayer));
    ((Behaviour) this.moonCamera).set_enabled(false);
  }

  private void RenderMoon()
  {
    if (this.currentTimeInHours <= this.lastMoonUpdate + 0.1 && (this.currentTimeInHours >= this.lastMoonUpdate - 0.1 || !this.skySettings.renderMoon))
      return;
    this.moonCamera.Render();
    this.lastMoonUpdate = this.currentTimeInHours;
  }

  private void CreateMoonTexture()
  {
    if (Object.op_Inequality((Object) this.moonRenderTarget, (Object) null) && Object.op_Inequality((Object) this.moonCamera, (Object) null))
    {
      this.moonCamera.set_targetTexture((RenderTexture) null);
      Object.DestroyImmediate((Object) this.moonRenderTarget);
    }
    this.moonRenderTarget = new RenderTexture(512, 512, 0, !this.GetCameraHDR(this.moonCamera) ? (RenderTextureFormat) 7 : (RenderTextureFormat) 9);
    this.moonCamera.set_targetTexture(this.moonRenderTarget);
  }

  public void CreateEffects()
  {
    this.EffectsHolder = GameObject.Find("Enviro Effects");
    if (Object.op_Inequality((Object) this.EffectsHolder, (Object) null))
    {
      for (int index = this.EffectsHolder.get_transform().get_childCount() - 1; 0 <= index; --index)
        Object.DestroyImmediate((Object) ((Component) this.EffectsHolder.get_transform().GetChild(index)).get_gameObject());
    }
    else
    {
      this.EffectsHolder = new GameObject("Enviro Effects");
      this.EffectsHolder.get_transform().set_parent(((Component) this).get_transform());
      this.EffectsHolder.get_transform().set_parent((Transform) null);
    }
    this.CreateWeatherEffectHolder();
    if (Application.get_isPlaying() && this.dontDestroy)
      Object.DontDestroyOnLoad((Object) this.EffectsHolder);
    this.SetEffectsHolderPlace(this.EffectsHolder);
  }

  public int RegisterMe(EnviroVegetationInstance me)
  {
    this.EnviroVegetationInstances.Add(me);
    return this.EnviroVegetationInstances.Count - 1;
  }

  public void ChangeSeason(EnviroSeasons.Seasons season)
  {
    this.Seasons.currentSeasons = season;
    this.NotifySeasonChanged(season);
  }

  private void UpdateSeason()
  {
    if ((double) this.currentDay >= 0.0 && (double) this.currentDay < (double) this.seasonsSettings.SpringInDays)
    {
      this.Seasons.currentSeasons = EnviroSeasons.Seasons.Spring;
      if (this.Seasons.lastSeason != this.Seasons.currentSeasons)
        this.NotifySeasonChanged(EnviroSeasons.Seasons.Spring);
      this.Seasons.lastSeason = this.Seasons.currentSeasons;
    }
    else if ((double) this.currentDay >= (double) this.seasonsSettings.SpringInDays && (double) this.currentDay < (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays)
    {
      this.Seasons.currentSeasons = EnviroSeasons.Seasons.Summer;
      if (this.Seasons.lastSeason != this.Seasons.currentSeasons)
        this.NotifySeasonChanged(EnviroSeasons.Seasons.Summer);
      this.Seasons.lastSeason = this.Seasons.currentSeasons;
    }
    else if ((double) this.currentDay >= (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays && (double) this.currentDay < (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays + (double) this.seasonsSettings.AutumnInDays)
    {
      this.Seasons.currentSeasons = EnviroSeasons.Seasons.Autumn;
      if (this.Seasons.lastSeason != this.Seasons.currentSeasons)
        this.NotifySeasonChanged(EnviroSeasons.Seasons.Autumn);
      this.Seasons.lastSeason = this.Seasons.currentSeasons;
    }
    else
    {
      if ((double) this.currentDay < (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays + (double) this.seasonsSettings.AutumnInDays || (double) this.currentDay > (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays + (double) this.seasonsSettings.AutumnInDays + (double) this.seasonsSettings.WinterInDays)
        return;
      this.Seasons.currentSeasons = EnviroSeasons.Seasons.Winter;
      if (this.Seasons.lastSeason != this.Seasons.currentSeasons)
        this.NotifySeasonChanged(EnviroSeasons.Seasons.Winter);
      this.Seasons.lastSeason = this.Seasons.currentSeasons;
    }
  }

  private void UpdateEnviroment()
  {
    if (this.Seasons.calcSeasons)
      this.UpdateSeason();
    if (this.EnviroVegetationInstances.Count <= 0)
      return;
    for (int index = 0; index < this.EnviroVegetationInstances.Count; ++index)
    {
      if (Object.op_Inequality((Object) this.EnviroVegetationInstances[index], (Object) null))
        this.EnviroVegetationInstances[index].UpdateInstance();
    }
  }

  private void CreateSatellite(int id)
  {
    if (Object.op_Equality((Object) this.satelliteSettings.additionalSatellites[id].prefab, (Object) null))
    {
      Debug.Log((object) "Satellite without prefab! Pleae assign a prefab to all satellites.");
    }
    else
    {
      GameObject gameObject1 = new GameObject();
      ((Object) gameObject1).set_name(this.satelliteSettings.additionalSatellites[id].name);
      gameObject1.get_transform().set_parent(this.Components.satellites);
      this.satellitesRotation.Add(gameObject1);
      GameObject gameObject2 = (GameObject) Object.Instantiate<GameObject>((M0) this.satelliteSettings.additionalSatellites[id].prefab, gameObject1.get_transform());
      gameObject2.set_layer(this.satelliteRenderingLayer);
      this.satellites.Add(gameObject2);
    }
  }

  public void CheckSatellites()
  {
    this.satellites = new List<GameObject>();
    for (int index = this.Components.satellites.get_childCount() - 1; index >= 0; --index)
      Object.DestroyImmediate((Object) ((Component) this.Components.satellites.GetChild(index)).get_gameObject());
    this.satellites.Clear();
    this.satellitesRotation.Clear();
    for (int id = 0; id < this.satelliteSettings.additionalSatellites.Count; ++id)
      this.CreateSatellite(id);
  }

  private void CalculateSatPositions(float siderealTime)
  {
    for (int index = 0; index < this.satelliteSettings.additionalSatellites.Count; ++index)
    {
      Quaternion quaternion = Quaternion.op_Multiply(Quaternion.Euler(90f - this.GameTime.Latitude, this.GameTime.Longitude, 0.0f), Quaternion.Euler(this.satelliteSettings.additionalSatellites[index].yRot, siderealTime, this.satelliteSettings.additionalSatellites[index].xRot));
      if (this.satellites.Count >= index)
        this.satellites[index].get_transform().set_localPosition(new Vector3(0.0f, this.satelliteSettings.additionalSatellites[index].orbit, 0.0f));
      if (this.satellitesRotation.Count >= index)
        this.satellitesRotation[index].get_transform().set_localRotation(quaternion);
    }
  }

  private void UpdateCameraComponents()
  {
    if (Object.op_Inequality((Object) this.EnviroSkyRender, (Object) null))
    {
      this.EnviroSkyRender.volumeLighting = this.volumeLighting;
      this.EnviroSkyRender.dirVolumeLighting = this.volumeLightSettings.dirVolumeLighting;
      this.EnviroSkyRender.simpleFog = this.fogSettings.useSimpleFog;
      this.EnviroSkyRender.distanceFog = this.fogSettings.distanceFog;
      this.EnviroSkyRender.heightFog = this.fogSettings.heightFog;
      this.EnviroSkyRender.height = this.fogSettings.height;
      this.EnviroSkyRender.heightDensity = this.fogSettings.heightDensity;
      this.EnviroSkyRender.useRadialDistance = this.fogSettings.useRadialDistance;
      this.EnviroSkyRender.startDistance = this.fogSettings.startDistance;
    }
    if (Object.op_Inequality((Object) this.lightShaftsScriptSun, (Object) null))
    {
      this.lightShaftsScriptSun.resolution = this.lightshaftsSettings.resolution;
      this.lightShaftsScriptSun.screenBlendMode = this.lightshaftsSettings.screenBlendMode;
      this.lightShaftsScriptSun.useDepthTexture = this.lightshaftsSettings.useDepthTexture;
      this.lightShaftsScriptSun.sunThreshold = this.lightshaftsSettings.thresholdColorSun.Evaluate(this.GameTime.solarTime);
      this.lightShaftsScriptSun.sunShaftBlurRadius = this.lightshaftsSettings.blurRadius;
      this.lightShaftsScriptSun.sunShaftIntensity = this.lightshaftsSettings.intensity;
      this.lightShaftsScriptSun.maxRadius = this.lightshaftsSettings.maxRadius;
      this.lightShaftsScriptSun.sunColor = this.lightshaftsSettings.lightShaftsColorSun.Evaluate(this.GameTime.solarTime);
      this.lightShaftsScriptSun.sunTransform = this.Components.Sun.get_transform();
      if (this.LightShafts.sunLightShafts)
        ((Behaviour) this.lightShaftsScriptSun).set_enabled(true);
      else
        ((Behaviour) this.lightShaftsScriptSun).set_enabled(false);
    }
    if (!Object.op_Inequality((Object) this.lightShaftsScriptMoon, (Object) null))
      return;
    this.lightShaftsScriptMoon.resolution = this.lightshaftsSettings.resolution;
    this.lightShaftsScriptMoon.screenBlendMode = this.lightshaftsSettings.screenBlendMode;
    this.lightShaftsScriptMoon.useDepthTexture = this.lightshaftsSettings.useDepthTexture;
    this.lightShaftsScriptMoon.sunThreshold = this.lightshaftsSettings.thresholdColorMoon.Evaluate(this.GameTime.lunarTime);
    this.lightShaftsScriptMoon.sunShaftBlurRadius = this.lightshaftsSettings.blurRadius;
    this.lightShaftsScriptMoon.sunShaftIntensity = Mathf.Clamp(this.lightshaftsSettings.intensity - this.GameTime.solarTime, 0.0f, 100f);
    this.lightShaftsScriptMoon.maxRadius = this.lightshaftsSettings.maxRadius;
    this.lightShaftsScriptMoon.sunColor = this.lightshaftsSettings.lightShaftsColorMoon.Evaluate(this.GameTime.lunarTime);
    this.lightShaftsScriptMoon.sunTransform = this.Components.Moon.get_transform();
    if (this.LightShafts.moonLightShafts)
      ((Behaviour) this.lightShaftsScriptMoon).set_enabled(true);
    else
      ((Behaviour) this.lightShaftsScriptMoon).set_enabled(false);
  }

  private Vector3 CalculatePosition()
  {
    Vector3 vector3;
    vector3.x = this.Player.get_transform().get_position().x;
    vector3.z = this.Player.get_transform().get_position().z;
    vector3.y = this.Player.get_transform().get_position().y;
    return vector3;
  }

  private void RenderFlatCloudsMap()
  {
    if (Object.op_Equality((Object) this.flatCloudsMat, (Object) null))
      this.flatCloudsMat = new Material(Shader.Find("Enviro/FlatCloudMap"));
    this.flatCloudsRenderTarget = RenderTexture.GetTemporary(512 * (int) (this.cloudsSettings.flatCloudsResolution + 1), 512 * (int) (this.cloudsSettings.flatCloudsResolution + 1), 0, (RenderTextureFormat) 9);
    ((Texture) this.flatCloudsRenderTarget).set_wrapMode((TextureWrapMode) 0);
    this.flatCloudsMat.SetVector("_CloudAnimation", Vector4.op_Implicit(this.cloudAnimNonScaled));
    this.flatCloudsMat.SetTexture("_NoiseTex", this.cloudsSettings.flatCloudsNoiseTexture);
    this.flatCloudsMat.SetFloat("_CloudScale", this.cloudsSettings.flatCloudsScale);
    this.flatCloudsMat.SetFloat("_Coverage", this.cloudsConfig.flatCoverage);
    this.flatCloudsMat.SetInt("noiseOctaves", this.cloudsSettings.flatCloudsNoiseOctaves);
    this.flatCloudsMat.SetFloat("_Softness", this.cloudsConfig.flatSoftness);
    this.flatCloudsMat.SetFloat("_Brightness", this.cloudsConfig.flatBrightness);
    this.flatCloudsMat.SetFloat("_MorphingSpeed", this.cloudsSettings.flatCloudsMorphingSpeed);
    Graphics.Blit((Texture) null, this.flatCloudsRenderTarget, this.flatCloudsMat);
    RenderTexture.ReleaseTemporary(this.flatCloudsRenderTarget);
  }

  private void RenderWeatherMap()
  {
    if (!Object.op_Equality((Object) this.cloudsSettings.customWeatherMap, (Object) null))
      return;
    this.weatherMapMat.SetVector("_WindDir", Vector4.op_Implicit(this.cloudAnimNonScaled));
    this.weatherMapMat.SetFloat("_AnimSpeedScale", this.cloudsSettings.weatherAnimSpeedScale);
    this.weatherMapMat.SetInt("_Tiling", this.cloudsSettings.weatherMapTiling);
    this.weatherMapMat.SetVector("_Location", Vector4.op_Implicit(this.cloudsSettings.locationOffset));
    this.weatherMapMat.SetFloat("_Coverage", (float) Math.Round((double) EnviroSky.instance.cloudsConfig.coverage * (double) this.cloudsSettings.globalCloudCoverage, 4));
    Graphics.Blit((Texture) null, this.weatherMap, this.weatherMapMat);
  }

  private void RenderCloudMaps()
  {
    switch (this.cloudsMode)
    {
      case EnviroSky.EnviroCloudsMode.Both:
        this.RenderFlatCloudsMap();
        this.RenderWeatherMap();
        break;
      case EnviroSky.EnviroCloudsMode.Volume:
        this.RenderWeatherMap();
        break;
      case EnviroSky.EnviroCloudsMode.Flat:
        this.RenderFlatCloudsMap();
        this.RenderWeatherMap();
        break;
    }
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.profile, (Object) null))
    {
      Debug.Log((object) "No profile applied! Please create and assign a profile.");
    }
    else
    {
      if (!this.started && !this.serverMode)
      {
        this.UpdateTime();
        this.UpdateSunAndMoonPosition();
        this.CalculateDirectLight();
        this.UpdateAmbientLight();
        this.UpdateReflections();
        this.RenderMoon();
        if (this.AssignInRuntime && this.PlayerTag != string.Empty && (this.CameraTag != string.Empty && Application.get_isPlaying()))
        {
          GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag(this.PlayerTag);
          if (Object.op_Inequality((Object) gameObjectWithTag, (Object) null))
            this.Player = gameObjectWithTag;
          for (int index = 0; index < Camera.get_allCameras().Length; ++index)
          {
            if (((Component) Camera.get_allCameras()[index]).get_tag() == this.CameraTag)
              this.PlayerCamera = Camera.get_allCameras()[index];
          }
          if (Object.op_Inequality((Object) this.Player, (Object) null) && Object.op_Inequality((Object) this.PlayerCamera, (Object) null))
          {
            this.Init();
            this.started = true;
          }
          else
          {
            this.started = false;
            return;
          }
        }
        else
        {
          this.started = false;
          return;
        }
      }
      this.UpdateTime();
      this.ValidateParameters();
      if (!this.serverMode)
      {
        if (this.lastCloudsMode != this.cloudsMode)
          this.SetupSkybox();
        this.RenderCloudMaps();
        this.UpdateCameraComponents();
        this.UpdateAmbientLight();
        this.UpdateReflections();
        this.UpdateWeather();
        this.UpdateCloudShadows();
        this.UpdateSkyRenderingComponent();
        this.RenderMoon();
        this.UpdateSunAndMoonPosition();
        this.CalculateDirectLight();
        this.CalculateSatPositions(this.LST);
        if (!this.isNight && (double) this.GameTime.solarTime < 0.449999988079071)
        {
          this.isNight = true;
          this.NotifyIsNight();
        }
        else if (this.isNight && (double) this.GameTime.solarTime >= 0.449999988079071)
        {
          this.isNight = false;
          this.NotifyIsDay();
        }
        if (this.lastCloudsQuality == this.cloudsSettings.cloudsQuality || this.cloudsMode != EnviroSky.EnviroCloudsMode.Volume && this.cloudsMode != EnviroSky.EnviroCloudsMode.Both)
          return;
        this.ChangeCloudsQuality(this.cloudsSettings.cloudsQuality);
      }
      else
      {
        this.UpdateWeather();
        if (!this.isNight && (double) this.GameTime.solarTime < 0.449999988079071)
        {
          this.isNight = true;
          this.NotifyIsNight();
        }
        else
        {
          if (!this.isNight || (double) this.GameTime.solarTime < 0.449999988079071)
            return;
          this.isNight = false;
          this.NotifyIsDay();
        }
      }
    }
  }

  private void LateUpdate()
  {
    if (this.serverMode || !Object.op_Inequality((Object) this.PlayerCamera, (Object) null) || !Object.op_Inequality((Object) this.Player, (Object) null))
      return;
    ((Component) this).get_transform().set_position(this.Player.get_transform().get_position());
    ((Component) this).get_transform().set_localScale(new Vector3(this.PlayerCamera.get_farClipPlane(), this.PlayerCamera.get_farClipPlane(), this.PlayerCamera.get_farClipPlane()));
    this.SetEffectsHolderPlace(this.EffectsHolder);
  }

  private void SetEffectsHolderPlace(GameObject obj)
  {
    if (Object.op_Equality((Object) obj, (Object) null))
      return;
    if (Object.op_Equality((Object) this.Player, (Object) null))
      obj.get_transform().SetPositionAndRotation(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation());
    else if (!Singleton<Resources>.IsInstance() || Object.op_Equality((Object) this.Player, (Object) this.PlayerCamera))
    {
      obj.get_transform().SetPositionAndRotation(this.Player.get_transform().get_position(), this.Player.get_transform().get_rotation());
    }
    else
    {
      Vector3 enviroEffectOffset = Singleton<Resources>.Instance.LocomotionProfile.EnviroEffectOffset;
      Vector3 position1 = this.Player.get_transform().get_position();
      Vector3 position2 = ((Component) this.PlayerCamera).get_transform().get_position();
      position1.y = (__Null) (double) (position2.y = (__Null) 0.0f);
      Quaternion quaternion = Quaternion.Euler(0.0f, Vector3.SignedAngle(Vector3.get_forward(), Vector3.op_Subtraction(position1, position2), Vector3.get_up()), 0.0f);
      obj.get_transform().SetPositionAndRotation(Vector3.op_Addition(this.Player.get_transform().get_position(), Quaternion.op_Multiply(quaternion, enviroEffectOffset)), quaternion);
    }
  }

  private void UpdateCloudShadows()
  {
    if ((double) this.cloudsSettings.shadowIntensity == 0.0 || this.cloudsMode == EnviroSky.EnviroCloudsMode.None || this.cloudsMode == EnviroSky.EnviroCloudsMode.Flat)
    {
      if (!Object.op_Inequality((Object) this.MainLight.get_cookie(), (Object) null))
        return;
      this.MainLight.set_cookie((Texture) null);
    }
    else
    {
      if ((double) this.cloudsSettings.shadowIntensity <= 0.0)
        return;
      this.cloudShadowMap.DiscardContents(true, true);
      this.cloudShadowMat.SetFloat("_shadowIntensity", this.cloudsSettings.shadowIntensity);
      if (this.cloudsMode == EnviroSky.EnviroCloudsMode.Volume || this.cloudsMode == EnviroSky.EnviroCloudsMode.Both)
      {
        this.cloudShadowMat.SetTexture("_MainTex", (Texture) this.weatherMap);
        Graphics.Blit((Texture) this.weatherMap, this.cloudShadowMap, this.cloudShadowMat);
      }
      if (Application.get_isPlaying())
        this.MainLight.set_cookie((Texture) this.cloudShadowMap);
      else
        this.MainLight.set_cookie((Texture) null);
      this.MainLight.set_cookieSize((float) this.cloudsSettings.shadowCookieSize);
    }
  }

  public Vector3 BetaRay()
  {
    Vector3 vector3_1 = Vector3.op_Multiply(this.skySettings.waveLength, 1E-09f);
    Vector3 vector3_2;
    vector3_2.x = (__Null) (8.0 * (double) Mathf.Pow(3.141593f, 3f) * (double) Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.10500001907349 / (7.63499991982415E+25 * (double) Mathf.Pow((float) vector3_1.x, 4f) * 5.75500011444092) * 2000.0);
    vector3_2.y = (__Null) (8.0 * (double) Mathf.Pow(3.141593f, 3f) * (double) Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.10500001907349 / (7.63499991982415E+25 * (double) Mathf.Pow((float) vector3_1.y, 4f) * 5.75500011444092) * 2000.0);
    vector3_2.z = (__Null) (8.0 * (double) Mathf.Pow(3.141593f, 3f) * (double) Mathf.Pow(Mathf.Pow(1.0003f, 2f) - 1f, 2f) * 6.10500001907349 / (7.63499991982415E+25 * (double) Mathf.Pow((float) vector3_1.z, 4f) * 5.75500011444092) * 2000.0);
    return vector3_2;
  }

  public Vector3 BetaMie()
  {
    float num = (float) (0.200000002980232 * (double) this.skySettings.turbidity * 10.0);
    Vector3 vector3;
    vector3.x = (__Null) (434.0 * (double) num * 3.14159274101257 * (double) Mathf.Pow((float) (6.28318548202515 / this.skySettings.waveLength.x), 2f) * this.K.x);
    vector3.y = (__Null) (434.0 * (double) num * 3.14159274101257 * (double) Mathf.Pow((float) (6.28318548202515 / this.skySettings.waveLength.y), 2f) * this.K.y);
    vector3.z = (__Null) (434.0 * (double) num * 3.14159274101257 * (double) Mathf.Pow((float) (6.28318548202515 / this.skySettings.waveLength.z), 2f) * this.K.z);
    vector3.x = (__Null) (double) Mathf.Pow((float) vector3.x, -1f);
    vector3.y = (__Null) (double) Mathf.Pow((float) vector3.y, -1f);
    vector3.z = (__Null) (double) Mathf.Pow((float) vector3.z, -1f);
    return vector3;
  }

  public Vector3 GetMieG()
  {
    return new Vector3((float) (1.0 - (double) this.skySettings.g * (double) this.skySettings.g), (float) (1.0 + (double) this.skySettings.g * (double) this.skySettings.g), 2f * this.skySettings.g);
  }

  public Vector3 GetMieGScene()
  {
    return new Vector3((float) (1.0 - (double) this.fogSettings.g * (double) this.fogSettings.g), (float) (1.0 + (double) this.fogSettings.g * (double) this.fogSettings.g), 2f * this.fogSettings.g);
  }

  private void SetupShader(float setup)
  {
    if (Object.op_Inequality((Object) this.skyMat, (Object) null))
    {
      this.skyMat.SetVector("_SunDir", Vector4.op_Implicit(Vector3.op_UnaryNegation(((Component) this.SunTransform).get_transform().get_forward())));
      this.skyMat.SetVector("_MoonDir", Vector4.op_Implicit(this.Components.Moon.get_transform().get_forward()));
      this.skyMat.SetColor("_MoonColor", this.skySettings.moonColor);
      this.skyMat.SetFloat("_MoonSize", this.skySettings.moonSize);
      this.skyMat.SetFloat("_MoonBrightness", this.skySettings.moonBrightness);
      if (this.skySettings.renderMoon)
        this.skyMat.SetTexture("_MoonTex", (Texture) this.moonRenderTarget);
      else
        this.skyMat.SetTexture("_MoonTex", (Texture) null);
      this.skyMat.SetColor("_scatteringColor", this.skySettings.scatteringColor.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetColor("_sunDiskColor", this.skySettings.sunDiskColor.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetColor("_weatherSkyMod", Color.Lerp(this.currentWeatherSkyMod, this.currentInteriorSkyboxMod, (float) this.currentInteriorSkyboxMod.a));
      this.skyMat.SetColor("_weatherFogMod", Color.Lerp(this.currentWeatherFogMod, this.currentInteriorFogColorMod, (float) this.currentInteriorFogColorMod.a));
      this.skyMat.SetVector("_Bm", Vector4.op_Implicit(Vector3.op_Multiply(this.BetaMie(), this.skySettings.mie * this.Fog.scatteringStrenght)));
      this.skyMat.SetVector("_Br", Vector4.op_Implicit(Vector3.op_Multiply(this.BetaRay(), this.skySettings.rayleigh)));
      this.skyMat.SetVector("_mieG", Vector4.op_Implicit(this.GetMieG()));
      this.skyMat.SetFloat("_SunIntensity", this.skySettings.sunIntensity);
      this.skyMat.SetFloat("_SunDiskSize", this.skySettings.sunDiskScale);
      this.skyMat.SetFloat("_SunDiskIntensity", this.skySettings.sunDiskIntensity);
      this.skyMat.SetFloat("_SunDiskSize", this.skySettings.sunDiskScale);
      this.skyMat.SetFloat("_Exposure", this.skySettings.skyExposure);
      this.skyMat.SetFloat("_SkyLuminance", this.skySettings.skyLuminence.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetFloat("_scatteringPower", this.skySettings.scatteringCurve.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetFloat("_SkyColorPower", this.skySettings.skyColorPower.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetFloat("_StarsIntensity", this.skySettings.starsIntensity.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetFloat("_GalaxyIntensity", this.skySettings.galaxyIntensity.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetColor("_moonGlowColor", this.skySettings.moonGlowColor);
      if (this.skySettings.blackGroundMode)
        this.skyMat.SetInt("_blackGround", 1);
      else
        this.skyMat.SetInt("_blackGround", 0);
      this.skyMat.SetFloat("_hdr", !this.HDR ? 0.0f : 1f);
      this.skyMat.SetFloat("_moonGlowStrenght", this.skySettings.moonGlow.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetVector("_CloudAnimation", Vector4.op_Implicit(this.cloudAnim));
      if (Object.op_Inequality((Object) this.cloudsSettings.cirrusCloudsTexture, (Object) null))
        this.skyMat.SetTexture("_CloudMap", this.cloudsSettings.cirrusCloudsTexture);
      this.skyMat.SetColor("_CloudColor", this.cloudsSettings.cirrusCloudsColor.Evaluate(this.GameTime.solarTime));
      this.skyMat.SetFloat("_CloudAltitude", this.cloudsSettings.cirrusCloudsAltitude);
      this.skyMat.SetFloat("_CloudAlpha", this.cloudsConfig.cirrusAlpha);
      this.skyMat.SetFloat("_CloudCoverage", this.cloudsConfig.cirrusCoverage);
      this.skyMat.SetFloat("_CloudColorPower", this.cloudsConfig.cirrusColorPow);
      if (Object.op_Inequality((Object) this.flatCloudsRenderTarget, (Object) null))
      {
        this.skyMat.SetTexture("_Cloud1Map", (Texture) this.flatCloudsRenderTarget);
        this.skyMat.SetColor("_Cloud1Color", this.cloudsSettings.flatCloudsColor.Evaluate(this.GameTime.solarTime));
        this.skyMat.SetFloat("_Cloud1Altitude", this.cloudsSettings.flatCloudsAltitude);
        this.skyMat.SetFloat("_Cloud1Alpha", this.cloudsConfig.flatAlpha);
        this.skyMat.SetFloat("_Cloud1ColorPower", this.cloudsConfig.flatColorPow);
      }
    }
    Shader.SetGlobalVector("_SunDir", Vector4.op_Implicit(Vector3.op_UnaryNegation(this.Components.Sun.get_transform().get_forward())));
    Shader.SetGlobalVector("_MoonDir", Vector4.op_Implicit(Vector3.op_UnaryNegation(this.Components.Moon.get_transform().get_forward())));
    Shader.SetGlobalColor("_scatteringColor", this.skySettings.scatteringColor.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalColor("_sunDiskColor", this.skySettings.sunDiskColor.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalColor("_weatherSkyMod", Color.Lerp(this.currentWeatherSkyMod, this.currentInteriorSkyboxMod, (float) this.currentInteriorSkyboxMod.a));
    Shader.SetGlobalColor("_weatherFogMod", Color.Lerp(this.currentWeatherFogMod, this.currentInteriorFogColorMod, (float) this.currentInteriorFogColorMod.a));
    Shader.SetGlobalFloat("_gameTime", Mathf.Clamp(1f - this.GameTime.solarTime, 0.5f, 1f));
    Shader.SetGlobalFloat("_SkyFogHeight", this.Fog.skyFogHeight);
    Shader.SetGlobalFloat("_scatteringStrenght", this.Fog.scatteringStrenght);
    Shader.SetGlobalFloat("_skyFogIntensity", this.fogSettings.skyFogIntensity);
    Shader.SetGlobalFloat("_SunBlocking", this.Fog.sunBlocking);
    Shader.SetGlobalVector("_EnviroParams", new Vector4(Mathf.Clamp(1f - this.GameTime.solarTime, 0.5f, 1f), !this.fogSettings.distanceFog ? 0.0f : 1f, !this.fogSettings.heightFog ? 0.0f : 1f, !this.HDR ? 0.0f : 1f));
    Shader.SetGlobalVector("_Bm", Vector4.op_Implicit(Vector3.op_Multiply(this.BetaMie(), this.skySettings.mie * (this.Fog.scatteringStrenght * this.GameTime.solarTime))));
    Shader.SetGlobalVector("_BmScene", Vector4.op_Implicit(Vector3.op_Multiply(this.BetaMie(), this.fogSettings.mie * (this.Fog.scatteringStrenght * this.GameTime.solarTime))));
    Shader.SetGlobalVector("_Br", Vector4.op_Implicit(Vector3.op_Multiply(this.BetaRay(), this.skySettings.rayleigh)));
    Shader.SetGlobalVector("_mieG", Vector4.op_Implicit(this.GetMieG()));
    Shader.SetGlobalVector("_mieGScene", Vector4.op_Implicit(this.GetMieGScene()));
    Shader.SetGlobalFloat("_SunIntensity", this.skySettings.sunIntensity);
    Shader.SetGlobalFloat("_SunDiskSize", this.skySettings.sunDiskScale);
    Shader.SetGlobalFloat("_SunDiskIntensity", this.skySettings.sunDiskIntensity);
    Shader.SetGlobalFloat("_SunDiskSize", this.skySettings.sunDiskScale);
    Shader.SetGlobalFloat("_Exposure", this.skySettings.skyExposure);
    Shader.SetGlobalFloat("_SkyLuminance", this.skySettings.skyLuminence.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalFloat("_scatteringPower", this.skySettings.scatteringCurve.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalFloat("_SkyColorPower", this.skySettings.skyColorPower.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalFloat("_heightFogIntensity", this.fogSettings.heightFogIntensity);
    Shader.SetGlobalFloat("_distanceFogIntensity", this.fogSettings.distanceFogIntensity);
    if (Application.get_isPlaying())
      Shader.SetGlobalFloat("_maximumFogDensity", 1f - this.fogSettings.maximumFogDensity);
    else
      Shader.SetGlobalFloat("_maximumFogDensity", 1f);
    Shader.SetGlobalFloat("_lightning", this.thunder);
    float num = 0.0f;
    if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPreset, (Object) null))
      num = this.Weather.currentActiveWeatherPreset.WindStrenght;
    if (this.cloudsSettings.useWindZoneDirection)
    {
      this.cloudsSettings.cloudsWindDirectionX = (float) -((Component) this.Components.windZone).get_transform().get_forward().x;
      this.cloudsSettings.cloudsWindDirectionY = (float) -((Component) this.Components.windZone).get_transform().get_forward().z;
    }
    EnviroSky enviroSky1 = this;
    enviroSky1.cloudAnim = Vector2.op_Addition(enviroSky1.cloudAnim, new Vector2(this.cloudsSettings.cloudsTimeScale * (num * this.cloudsSettings.cloudsWindDirectionX) * this.cloudsSettings.cloudsWindStrengthModificator * Time.get_deltaTime(), this.cloudsSettings.cloudsTimeScale * (num * this.cloudsSettings.cloudsWindDirectionY) * this.cloudsSettings.cloudsWindStrengthModificator * Time.get_deltaTime()));
    EnviroSky enviroSky2 = this;
    enviroSky2.cloudAnimNonScaled = Vector2.op_Addition(enviroSky2.cloudAnimNonScaled, new Vector2((float) ((double) this.cloudsSettings.cloudsTimeScale * ((double) num * (double) this.cloudsSettings.cloudsWindDirectionX) * (double) this.cloudsSettings.cloudsWindStrengthModificator * (double) Time.get_deltaTime() * 0.100000001490116), (float) ((double) this.cloudsSettings.cloudsTimeScale * ((double) num * (double) this.cloudsSettings.cloudsWindDirectionY) * (double) this.cloudsSettings.cloudsWindStrengthModificator * (double) Time.get_deltaTime() * 0.100000001490116)));
    if (this.cloudAnim.x > 1.0)
      this.cloudAnim.x = (__Null) -1.0;
    else if (this.cloudAnim.x < -1.0)
      this.cloudAnim.x = (__Null) 1.0;
    if (this.cloudAnim.y > 1.0)
      this.cloudAnim.y = (__Null) -1.0;
    else if (this.cloudAnim.y < -1.0)
      this.cloudAnim.y = (__Null) 1.0;
    if (!Object.op_Inequality((Object) this.MoonShader, (Object) null))
      return;
    this.MoonShader.SetFloat("_Phase", this.customMoonPhase);
    this.MoonShader.SetColor("_Color", this.skySettings.moonColor);
    this.MoonShader.SetFloat("_Brightness", this.skySettings.moonBrightness * (1f - this.GameTime.solarTime));
  }

  private void UpdateSkyRenderingComponent()
  {
    if (Object.op_Equality((Object) this.EnviroSkyRender, (Object) null))
      return;
    this.EnviroSkyRender.Resolution = this.volumeLightSettings.Resolution;
    if (!Object.op_Inequality((Object) this.EnviroSkyRender._volumeRenderingMaterial, (Object) null))
      return;
    this.EnviroSkyRender._volumeRenderingMaterial.SetTexture("_Clouds", (Texture) this.cloudsRenderTarget);
    this.EnviroSkyRender._volumeRenderingMaterial.SetFloat("_hdr", !this.HDR ? 0.0f : 1f);
  }

  private DateTime CreateSystemDate()
  {
    DateTime dateTime = new DateTime();
    dateTime = dateTime.AddYears(this.GameTime.Years - 1);
    dateTime = dateTime.AddDays((double) (this.GameTime.Days - 1));
    return dateTime;
  }

  private void UpdateSunAndMoonPosition()
  {
    DateTime systemDate = this.CreateSystemDate();
    float d = (float) (367 * systemDate.Year - 7 * (systemDate.Year + (systemDate.Month / 12 + 9) / 12) / 4 + 275 * systemDate.Month / 9 + systemDate.Day - 730530) + this.GetUniversalTimeOfDay() / 24f;
    float ecl = (float) (23.4393005371094 - 3.56300006387755E-07 * (double) d);
    if (this.skySettings.sunAndMoonPosition == EnviroSkySettings.SunAndMoonCalc.Realistic)
    {
      this.CalculateSunPosition(d, ecl, false);
      this.CalculateMoonPosition(d, ecl);
    }
    else
      this.CalculateSunPosition(d, ecl, true);
    this.CalculateStarsPosition(this.LST);
  }

  private float Remap(float value, float from1, float to1, float from2, float to2)
  {
    return (float) (((double) value - (double) from1) / ((double) to1 - (double) from1) * ((double) to2 - (double) from2)) + from2;
  }

  private void CalculateSunPosition(float d, float ecl, bool simpleMoon)
  {
    float num1 = (float) (282.940399169922 + 4.70935010525864E-05 * (double) d);
    float num2 = (float) (0.0167089998722076 - 1.1509999620074E-09 * (double) d);
    float num3 = (float) (356.046997070313 + 0.985600233078003 * (double) d);
    float num4 = num3 + (float) ((double) num2 * 57.2957801818848 * (double) Mathf.Sin((float) Math.PI / 180f * num3) * (1.0 + (double) num2 * (double) Mathf.Cos((float) Math.PI / 180f * num3)));
    float num5 = Mathf.Cos((float) Math.PI / 180f * num4) - num2;
    float num6 = Mathf.Sin((float) Math.PI / 180f * num4) * Mathf.Sqrt((float) (1.0 - (double) num2 * (double) num2));
    float num7 = 57.29578f * Mathf.Atan2(num6, num5);
    float num8 = Mathf.Sqrt((float) ((double) num5 * (double) num5 + (double) num6 * (double) num6));
    float num9 = num7 + num1;
    float num10 = num8 * Mathf.Cos((float) Math.PI / 180f * num9);
    float num11 = num8 * Mathf.Sin((float) Math.PI / 180f * num9);
    float num12 = num10;
    float num13 = num11 * Mathf.Cos((float) Math.PI / 180f * ecl);
    float num14 = Mathf.Atan2(num11 * Mathf.Sin((float) Math.PI / 180f * ecl), Mathf.Sqrt((float) ((double) num12 * (double) num12 + (double) num13 * (double) num13)));
    float num15 = Mathf.Sin(num14);
    float num16 = Mathf.Cos(num14);
    this.LST = num9 + 180f + this.GetUniversalTimeOfDay() * 15f + this.GameTime.Longitude;
    float num17 = (float) Math.PI / 180f * (this.LST - 57.29578f * Mathf.Atan2(num13, num12));
    float num18 = Mathf.Sin(num17);
    float num19 = Mathf.Cos(num17) * num16;
    float num20 = num18 * num16;
    float num21 = num15;
    float num22 = Mathf.Sin((float) Math.PI / 180f * this.GameTime.Latitude);
    float num23 = Mathf.Cos((float) Math.PI / 180f * this.GameTime.Latitude);
    float num24 = (float) ((double) num19 * (double) num22 - (double) num21 * (double) num23);
    float num25 = num20;
    float num26 = (float) ((double) num19 * (double) num23 + (double) num21 * (double) num22);
    float num27 = Mathf.Atan2(num25, num24) + 3.141593f;
    float num28 = 1.570796f - Mathf.Atan2(num26, Mathf.Sqrt((float) ((double) num24 * (double) num24 + (double) num25 * (double) num25)));
    float phi = num27;
    this.GameTime.solarTime = Mathf.Clamp01(this.Remap(num28, -1.5f, 0.0f, 1.5f, 1f));
    this.SunTransform.set_localPosition(this.OrbitalToLocal(num28, phi));
    this.SunTransform.LookAt(this.DomeTransform.get_position());
    if (simpleMoon)
    {
      this.MoonTransform.set_localPosition(this.OrbitalToLocal(num28 - 3.141593f, phi));
      this.MoonTransform.LookAt(this.DomeTransform.get_position());
    }
    this.SetupShader(num28);
  }

  private void CalculateMoonPosition(float d, float ecl)
  {
    float num1 = (float) (125.122802734375 - 0.0529538094997406 * (double) d);
    float num2 = 5.1454f;
    float num3 = (float) (318.063385009766 + 0.16435731947422 * (double) d);
    float num4 = 60.2666f;
    float num5 = 0.0549f;
    float num6 = (float) Math.PI / 180f * (float) (115.36540222168 + 13.0649929046631 * (double) d);
    float num7 = num6 + (float) ((double) num5 * (double) Mathf.Sin(num6) * (1.0 + (double) num5 * (double) Mathf.Cos(num6)));
    float num8 = num4 * (Mathf.Cos(num7) - num5);
    float num9 = num4 * (Mathf.Sqrt((float) (1.0 - (double) num5 * (double) num5)) * Mathf.Sin(num7));
    float num10 = 57.29578f * Mathf.Atan2(num9, num8);
    float num11 = Mathf.Sqrt((float) ((double) num8 * (double) num8 + (double) num9 * (double) num9));
    float num12 = (float) Math.PI / 180f * num1;
    float num13 = Mathf.Sin(num12);
    float num14 = Mathf.Cos(num12);
    float num15 = (float) (Math.PI / 180.0 * ((double) num10 + (double) num3));
    float num16 = Mathf.Sin(num15);
    float num17 = Mathf.Cos(num15);
    float num18 = (float) Math.PI / 180f * num2;
    float num19 = Mathf.Cos(num18);
    float num20 = num11 * (float) ((double) num14 * (double) num17 - (double) num13 * (double) num16 * (double) num19);
    float num21 = num11 * (float) ((double) num13 * (double) num17 + (double) num14 * (double) num16 * (double) num19);
    float num22 = num11 * (num16 * Mathf.Sin(num18));
    float num23 = Mathf.Cos((float) Math.PI / 180f * ecl);
    float num24 = Mathf.Sin((float) Math.PI / 180f * ecl);
    float num25 = num20;
    float num26 = (float) ((double) num21 * (double) num23 - (double) num22 * (double) num24);
    float num27 = (float) ((double) num21 * (double) num24 + (double) num22 * (double) num23);
    float num28 = Mathf.Atan2(num26, num25);
    float num29 = Mathf.Atan2(num27, Mathf.Sqrt((float) ((double) num25 * (double) num25 + (double) num26 * (double) num26)));
    float num30 = (float) Math.PI / 180f * this.LST - num28;
    float num31 = Mathf.Cos(num30) * Mathf.Cos(num29);
    float num32 = Mathf.Sin(num30) * Mathf.Cos(num29);
    float num33 = Mathf.Sin(num29);
    float num34 = (float) Math.PI / 180f * this.GameTime.Latitude;
    float num35 = Mathf.Sin(num34);
    float num36 = Mathf.Cos(num34);
    float num37 = (float) ((double) num31 * (double) num35 - (double) num33 * (double) num36);
    float num38 = num32;
    float num39 = (float) ((double) num31 * (double) num36 + (double) num33 * (double) num35);
    float num40 = Mathf.Atan2(num38, num37) + 3.141593f;
    float theta = 1.570796f - Mathf.Atan2(num39, Mathf.Sqrt((float) ((double) num37 * (double) num37 + (double) num38 * (double) num38)));
    float phi = num40;
    this.MoonTransform.set_localPosition(this.OrbitalToLocal(theta, phi));
    this.GameTime.lunarTime = Mathf.Clamp01(this.Remap(theta, -1.5f, 0.0f, 1.5f, 1f));
    this.MoonTransform.LookAt(this.DomeTransform.get_position());
  }

  private void CalculateStarsPosition(float siderealTime)
  {
    if ((double) siderealTime > 24.0)
      siderealTime -= 24f;
    else if ((double) siderealTime < 0.0)
      siderealTime += 24f;
    this.Components.starsRotation.set_localRotation(Quaternion.op_Multiply(Quaternion.Euler(90f - this.GameTime.Latitude, (float) Math.PI / 180f * this.GameTime.Longitude, 0.0f), Quaternion.Euler(0.0f, siderealTime, 0.0f)));
    if (!Object.op_Inequality((Object) RenderSettings.get_skybox(), (Object) null))
      return;
    RenderSettings.get_skybox().SetMatrix("_StarsMatrix", this.Components.starsRotation.get_worldToLocalMatrix());
  }

  private Vector3 UpdateSatellitePosition(float orbit, float orbit2, float speed)
  {
    float num1 = (float) Math.PI / 180f * this.GameTime.Latitude;
    float num2 = Mathf.Sin(num1);
    float num3 = Mathf.Cos(num1);
    float num4 = (float) Math.PI / 180f * this.GameTime.Longitude;
    float num5 = orbit2 * Mathf.Sin((float) (Math.PI / 184.0 * ((double) this.GameTime.Days - 81.0)));
    float num6 = Mathf.Sin(num5);
    float num7 = Mathf.Cos(num5);
    float num8 = 0.2617994f * (float) (int) ((double) this.GameTime.Longitude / 15.0);
    float num9 = 0.2617994f * (float) ((double) this.GetUniversalTimeOfDay() + (double) orbit * (double) Mathf.Sin((float) (0.0333325490355492 * ((double) this.GameTime.Days - 80.0))) - (double) speed * (double) Mathf.Sin((float) (Math.PI / 355.0 * ((double) this.GameTime.Days - 8.0))) + 3.81971859931946 * ((double) num8 - (double) num4));
    float num10 = Mathf.Sin(num9);
    float num11 = Mathf.Cos(num9);
    return this.OrbitalToLocal(1.570796f - Mathf.Asin((float) ((double) num2 * (double) num6 - (double) num3 * (double) num7 * (double) num11)), Mathf.Atan2(-num7 * num10, (float) ((double) num3 * (double) num6 - (double) num2 * (double) num7 * (double) num11)));
  }

  private Vector3 OrbitalToLocal(float theta, float phi)
  {
    float num1 = Mathf.Sin(theta);
    float num2 = Mathf.Cos(theta);
    float num3 = Mathf.Sin(phi);
    float num4 = Mathf.Cos(phi);
    Vector3 vector3;
    vector3.z = (__Null) ((double) num1 * (double) num4);
    vector3.y = (__Null) (double) num2;
    vector3.x = (__Null) ((double) num1 * (double) num3);
    return vector3;
  }

  private void UpdateReflections()
  {
    this.Components.GlobalReflectionProbe.set_intensity(this.lightSettings.globalReflectionsIntensity);
    this.Components.GlobalReflectionProbe.set_size(Vector3.op_Multiply(((Component) this).get_transform().get_localScale(), this.lightSettings.globalReflectionsScale));
    if ((this.currentTimeInHours > this.lastRelfectionUpdate + (double) this.lightSettings.globalReflectionsUpdate || this.currentTimeInHours < this.lastRelfectionUpdate - (double) this.lightSettings.globalReflectionsUpdate) && this.lightSettings.globalReflections)
    {
      ((Behaviour) this.Components.GlobalReflectionProbe).set_enabled(true);
      this.lastRelfectionUpdate = this.currentTimeInHours;
      this.Components.GlobalReflectionProbe.RenderProbe();
    }
    else
    {
      if (this.lightSettings.globalReflections)
        return;
      ((Behaviour) this.Components.GlobalReflectionProbe).set_enabled(false);
    }
  }

  private void UpdateTime()
  {
    if (Application.get_isPlaying())
    {
      this.hourTime = (this.isNight ? 0.4f / this.GameTime.NightLengthInMinutes : 0.4f / this.GameTime.DayLengthInMinutes) * Time.get_deltaTime();
      switch (this.GameTime.ProgressTime)
      {
        case EnviroTime.TimeProgressMode.None:
          this.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
          break;
        case EnviroTime.TimeProgressMode.Simulated:
          this.internalHour += this.hourTime;
          this.SetGameTime();
          this.customMoonPhase += (float) ((double) Time.get_deltaTime() / (30.0 * ((double) this.GameTime.DayLengthInMinutes * 60.0)) * 2.0);
          break;
        case EnviroTime.TimeProgressMode.OneDay:
          this.internalHour += this.hourTime;
          this.SetGameTime();
          this.customMoonPhase += (float) ((double) Time.get_deltaTime() / (30.0 * ((double) this.GameTime.DayLengthInMinutes * 60.0)) * 2.0);
          break;
        case EnviroTime.TimeProgressMode.SystemTime:
          this.SetTime(DateTime.Now);
          this.customMoonPhase += (float) ((double) Time.get_deltaTime() / 2592000.0 * 2.0);
          break;
      }
    }
    else
      this.SetTime(this.GameTime.Years, this.GameTime.Days, this.GameTime.Hours, this.GameTime.Minutes, this.GameTime.Seconds);
    if ((double) this.customMoonPhase < -1.0)
      this.customMoonPhase += 2f;
    else if ((double) this.customMoonPhase > 1.0)
      this.customMoonPhase -= 2f;
    if ((double) this.internalHour > (double) this.lastHourUpdate + 1.0)
    {
      this.lastHourUpdate = this.internalHour;
      this.NotifyHourPassed();
    }
    if ((double) this.GameTime.Days >= (double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays + (double) this.seasonsSettings.AutumnInDays + (double) this.seasonsSettings.WinterInDays)
    {
      ++this.GameTime.Years;
      this.GameTime.Days = 0;
      this.NotifyYearPassed();
    }
    this.currentHour = this.internalHour;
    this.currentDay = (float) this.GameTime.Days;
    this.currentYear = (float) this.GameTime.Years;
    this.currentTimeInHours = this.GetInHours(this.internalHour, this.currentDay, this.currentYear);
  }

  private void SetInternalTime(int year, int dayOfYear, int hour, int minute, int seconds)
  {
    this.GameTime.Years = year;
    this.GameTime.Days = dayOfYear;
    this.GameTime.Minutes = minute;
    this.GameTime.Hours = hour;
    this.internalHour = (float) ((double) hour + (double) minute * 0.0166666992008686 + (double) seconds * 0.00027777798823081);
  }

  private void SetGameTime()
  {
    if ((double) this.internalHour >= 24.0)
    {
      this.internalHour -= 24f;
      this.NotifyHourPassed();
      this.lastHourUpdate = this.internalHour;
      if (this.GameTime.ProgressTime != EnviroTime.TimeProgressMode.OneDay)
      {
        ++this.GameTime.Days;
        this.NotifyDayPassed();
      }
    }
    else if ((double) this.internalHour < 0.0)
    {
      this.internalHour = 24f + this.internalHour;
      this.lastHourUpdate = this.internalHour;
      if (this.GameTime.ProgressTime != EnviroTime.TimeProgressMode.OneDay)
      {
        --this.GameTime.Days;
        this.NotifyDayPassed();
      }
    }
    float internalHour = this.internalHour;
    this.GameTime.Hours = (int) internalHour;
    float num = internalHour - (float) this.GameTime.Hours;
    this.GameTime.Minutes = (int) ((double) num * 60.0);
    this.GameTime.Seconds = (int) ((double) (num - (float) this.GameTime.Minutes * 0.0166667f) * 3600.0);
  }

  private void UpdateAmbientLight()
  {
    AmbientMode ambientMode = this.lightSettings.ambientMode;
    if (ambientMode != 3)
    {
      if (ambientMode != 1)
      {
        if (ambientMode != null)
          return;
        RenderSettings.set_ambientIntensity(this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime));
        if ((double) this.lastAmbientSkyUpdate >= (double) this.internalHour && (double) this.lastAmbientSkyUpdate <= (double) this.internalHour + 0.101000003516674)
          return;
        DynamicGI.UpdateEnvironment();
        this.lastAmbientSkyUpdate = this.internalHour + 0.1f;
      }
      else
      {
        RenderSettings.set_ambientSkyColor(Color.Lerp(Color.op_Multiply(Color.Lerp(this.lightSettings.ambientSkyColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, (float) this.currentWeatherLightMod.a), this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime)), this.currentInteriorAmbientLightMod, (float) this.currentInteriorAmbientLightMod.a));
        RenderSettings.set_ambientEquatorColor(Color.Lerp(Color.op_Multiply(Color.Lerp(this.lightSettings.ambientEquatorColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, (float) this.currentWeatherLightMod.a), this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime)), this.currentInteriorAmbientEQLightMod, (float) this.currentInteriorAmbientEQLightMod.a));
        RenderSettings.set_ambientGroundColor(Color.Lerp(Color.op_Multiply(Color.Lerp(this.lightSettings.ambientGroundColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, (float) this.currentWeatherLightMod.a), this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime)), this.currentInteriorAmbientGRLightMod, (float) this.currentInteriorAmbientGRLightMod.a));
      }
    }
    else
      RenderSettings.set_ambientSkyColor(Color.Lerp(Color.op_Multiply(Color.Lerp(this.lightSettings.ambientSkyColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, (float) this.currentWeatherLightMod.a), this.lightSettings.ambientIntensity.Evaluate(this.GameTime.solarTime)), this.currentInteriorAmbientLightMod, (float) this.currentInteriorAmbientLightMod.a));
  }

  private void CalculateDirectLight()
  {
    this.MainLight.set_color(Color.Lerp(Color.Lerp(this.lightSettings.LightColor.Evaluate(this.GameTime.solarTime), this.currentWeatherLightMod, (float) this.currentWeatherLightMod.a), this.currentInteriorDirectLightMod, (float) this.currentInteriorDirectLightMod.a));
    Shader.SetGlobalColor("_EnviroLighting", this.lightSettings.LightColor.Evaluate(this.GameTime.solarTime));
    Shader.SetGlobalVector("_SunDirection", Vector4.op_Implicit(Vector3.op_UnaryNegation(this.Components.Sun.get_transform().get_forward())));
    Shader.SetGlobalVector("_SunPosition", Vector4.op_Implicit(Vector3.op_Addition(this.Components.Sun.get_transform().get_localPosition(), Vector3.op_Multiply(Vector3.op_UnaryNegation(this.Components.Sun.get_transform().get_forward()), 10000f))));
    Shader.SetGlobalVector("_MoonPosition", Vector4.op_Implicit(this.Components.Moon.get_transform().get_localPosition()));
    float num;
    if (!this.isNight)
    {
      num = this.lightSettings.directLightSunIntensity.Evaluate(this.GameTime.solarTime);
      this.Components.Sun.get_transform().LookAt(new Vector3((float) this.DomeTransform.get_position().x, (float) this.DomeTransform.get_position().y - this.lightSettings.directLightAngleOffset, (float) this.DomeTransform.get_position().z));
      this.Components.DirectLight.set_rotation(this.Components.Sun.get_transform().get_rotation());
    }
    else
    {
      num = this.lightSettings.directLightMoonIntensity.Evaluate(this.GameTime.lunarTime);
      this.Components.Moon.get_transform().LookAt(new Vector3((float) this.DomeTransform.get_position().x, (float) this.DomeTransform.get_position().y - this.lightSettings.directLightAngleOffset, (float) this.DomeTransform.get_position().z));
      this.Components.DirectLight.set_rotation(this.Components.Moon.get_transform().get_rotation());
    }
    this.MainLight.set_intensity(num);
    this.MainLight.set_shadowStrength(this.lightSettings.shadowIntensity.Evaluate(this.GameTime.solarTime));
  }

  private Quaternion LightLookAt(Quaternion inputRotation, Quaternion newRotation)
  {
    return Quaternion.Lerp(inputRotation, newRotation, 500f * Time.get_deltaTime());
  }

  private void ValidateParameters()
  {
    this.internalHour = Mathf.Repeat(this.internalHour, 24f);
    this.GameTime.Longitude = Mathf.Clamp(this.GameTime.Longitude, -180f, 180f);
    this.GameTime.Latitude = Mathf.Clamp(this.GameTime.Latitude, -90f, 90f);
  }

  public void RegisterZone(EnviroZone zoneToAdd)
  {
    this.Weather.zones.Add(zoneToAdd);
  }

  public void EnterZone(EnviroZone zone)
  {
    this.Weather.currentActiveZone = zone;
  }

  public void ExitZone()
  {
  }

  public void CreateWeatherEffectHolder()
  {
    if (!Object.op_Equality((Object) this.Weather.VFXHolder, (Object) null))
      return;
    GameObject gameObject = new GameObject();
    ((Object) gameObject).set_name("VFX");
    gameObject.get_transform().set_parent(this.EffectsHolder.get_transform());
    gameObject.get_transform().set_localPosition(Vector3.get_zero());
    this.Weather.VFXHolder = gameObject;
  }

  private void UpdateAudioSource(EnviroWeatherPreset i)
  {
    if (Object.op_Inequality((Object) i, (Object) null) && Object.op_Inequality((Object) i.weatherSFX, (Object) null))
    {
      if (Object.op_Equality((Object) i.weatherSFX, (Object) this.Weather.currentAudioSource.audiosrc.get_clip()))
      {
        if ((double) this.Weather.currentAudioSource.audiosrc.get_volume() >= 0.100000001490116)
          return;
        this.Weather.currentAudioSource.FadeIn(i.weatherSFX);
      }
      else if (Object.op_Equality((Object) this.Weather.currentAudioSource, (Object) this.AudioSourceWeather))
      {
        this.AudioSourceWeather.FadeOut();
        this.AudioSourceWeather2.FadeIn(i.weatherSFX);
        this.Weather.currentAudioSource = this.AudioSourceWeather2;
      }
      else
      {
        if (!Object.op_Equality((Object) this.Weather.currentAudioSource, (Object) this.AudioSourceWeather2))
          return;
        this.AudioSourceWeather2.FadeOut();
        this.AudioSourceWeather.FadeIn(i.weatherSFX);
        this.Weather.currentAudioSource = this.AudioSourceWeather;
      }
    }
    else
    {
      this.AudioSourceWeather.FadeOut();
      this.AudioSourceWeather2.FadeOut();
    }
  }

  private void UpdateClouds(EnviroWeatherPreset i, bool withTransition)
  {
    if (Object.op_Equality((Object) i, (Object) null))
      return;
    float num = 500f * Time.get_deltaTime();
    if (withTransition)
      num = this.weatherSettings.cloudTransitionSpeed * Time.get_deltaTime();
    this.cloudsConfig.topColor = Color.Lerp(this.cloudsConfig.topColor, i.cloudsConfig.topColor, num);
    this.cloudsConfig.bottomColor = Color.Lerp(this.cloudsConfig.bottomColor, i.cloudsConfig.bottomColor, num);
    this.cloudsConfig.coverage = Mathf.Lerp(this.cloudsConfig.coverage, i.cloudsConfig.coverage, num);
    this.cloudsConfig.coverageHeight = Mathf.Lerp(this.cloudsConfig.coverageHeight, i.cloudsConfig.coverageHeight, num);
    this.cloudsConfig.raymarchingScale = Mathf.Lerp(this.cloudsConfig.raymarchingScale, i.cloudsConfig.raymarchingScale, num);
    this.cloudsConfig.skyBlending = Mathf.Lerp(this.cloudsConfig.skyBlending, i.cloudsConfig.skyBlending, num);
    this.cloudsConfig.density = Mathf.Lerp(this.cloudsConfig.density, i.cloudsConfig.density, num);
    this.cloudsConfig.alphaCoef = Mathf.Lerp(this.cloudsConfig.alphaCoef, i.cloudsConfig.alphaCoef, num);
    this.cloudsConfig.scatteringCoef = Mathf.Lerp(this.cloudsConfig.scatteringCoef, i.cloudsConfig.scatteringCoef, num);
    this.cloudsConfig.cloudType = Mathf.Lerp(this.cloudsConfig.cloudType, i.cloudsConfig.cloudType, num);
    this.cloudsConfig.cirrusAlpha = Mathf.Lerp(this.cloudsConfig.cirrusAlpha, i.cloudsConfig.cirrusAlpha, num);
    this.cloudsConfig.cirrusCoverage = Mathf.Lerp(this.cloudsConfig.cirrusCoverage, i.cloudsConfig.cirrusCoverage, num);
    this.cloudsConfig.cirrusColorPow = Mathf.Lerp(this.cloudsConfig.cirrusColorPow, i.cloudsConfig.cirrusColorPow, num);
    this.cloudsConfig.flatAlpha = Mathf.Lerp(this.cloudsConfig.flatAlpha, i.cloudsConfig.flatAlpha, num);
    this.cloudsConfig.flatCoverage = Mathf.Lerp(this.cloudsConfig.flatCoverage, i.cloudsConfig.flatCoverage, num);
    this.cloudsConfig.flatColorPow = Mathf.Lerp(this.cloudsConfig.flatColorPow, i.cloudsConfig.flatColorPow, num);
    this.cloudsConfig.flatSoftness = Mathf.Lerp(this.cloudsConfig.flatSoftness, i.cloudsConfig.flatSoftness, num);
    this.cloudsConfig.flatBrightness = Mathf.Lerp(this.cloudsConfig.flatBrightness, i.cloudsConfig.flatBrightness, num);
    this.globalVolumeLightIntensity = Mathf.Lerp(this.globalVolumeLightIntensity, i.volumeLightIntensity, num);
    this.currentWeatherSkyMod = Color.Lerp(this.currentWeatherSkyMod, i.weatherSkyMod.Evaluate(this.GameTime.solarTime), num);
    this.currentWeatherFogMod = Color.Lerp(this.currentWeatherFogMod, i.weatherFogMod.Evaluate(this.GameTime.solarTime), num * 10f);
    this.currentWeatherLightMod = Color.Lerp(this.currentWeatherLightMod, i.weatherLightMod.Evaluate(this.GameTime.solarTime), num);
  }

  private void UpdateFog(EnviroWeatherPreset i, bool withTransition)
  {
    if (!Object.op_Inequality((Object) i, (Object) null))
      return;
    float num = 500f * Time.get_deltaTime();
    if (withTransition)
      num = this.weatherSettings.fogTransitionSpeed * Time.get_deltaTime();
    if (this.fogSettings.Fogmode == 1)
    {
      RenderSettings.set_fogEndDistance(Mathf.Lerp(RenderSettings.get_fogEndDistance(), i.fogDistance, num));
      RenderSettings.set_fogStartDistance(Mathf.Lerp(RenderSettings.get_fogStartDistance(), i.fogStartDistance, num));
    }
    else if (this.updateFogDensity)
      RenderSettings.set_fogDensity(Mathf.Lerp(RenderSettings.get_fogDensity(), i.fogDensity, num) * this.currentInteriorFogMod);
    RenderSettings.set_fogColor(Color.Lerp(Color.Lerp(this.lightSettings.ambientSkyColor.Evaluate(this.GameTime.solarTime), this.customFogColor, this.customFogIntensity), this.currentWeatherFogMod, (float) this.currentWeatherFogMod.a));
    this.fogSettings.heightDensity = Mathf.Lerp(this.fogSettings.heightDensity, i.heightFogDensity, num);
    this.Fog.skyFogHeight = Mathf.Lerp(this.Fog.skyFogHeight, i.SkyFogHeight, num);
    this.Fog.skyFogStrength = Mathf.Lerp(this.Fog.skyFogStrength, i.SkyFogIntensity, num);
    this.fogSettings.skyFogIntensity = Mathf.Lerp(this.fogSettings.skyFogIntensity, i.SkyFogIntensity, num);
    this.Fog.scatteringStrenght = Mathf.Lerp(this.Fog.scatteringStrenght, i.FogScatteringIntensity, num);
    this.Fog.sunBlocking = Mathf.Lerp(this.Fog.sunBlocking, i.fogSunBlocking, num);
  }

  private void UpdateEffectSystems(EnviroWeatherPrefab id, bool withTransition)
  {
    if (!Object.op_Inequality((Object) id, (Object) null))
      return;
    float num = 500f * Time.get_deltaTime();
    if (withTransition)
      num = this.weatherSettings.effectTransitionSpeed * Time.get_deltaTime();
    for (int index = 0; index < id.effectSystems.Count; ++index)
    {
      if (id.effectSystems[index].get_isStopped())
        id.effectSystems[index].Play();
      float emissionRate = Mathf.Lerp(EnviroSky.GetEmissionRate(id.effectSystems[index]), id.effectEmmisionRates[index] * this.qualitySettings.GlobalParticleEmissionRates, num) * this.currentInteriorWeatherEffectMod;
      EnviroSky.SetEmissionRate(id.effectSystems[index], emissionRate);
    }
    for (int index1 = 0; index1 < this.Weather.WeatherPrefabs.Count; ++index1)
    {
      if (Object.op_Inequality((Object) ((Component) this.Weather.WeatherPrefabs[index1]).get_gameObject(), (Object) ((Component) id).get_gameObject()))
      {
        for (int index2 = 0; index2 < this.Weather.WeatherPrefabs[index1].effectSystems.Count; ++index2)
        {
          float emissionRate = Mathf.Lerp(EnviroSky.GetEmissionRate(this.Weather.WeatherPrefabs[index1].effectSystems[index2]), 0.0f, num);
          if ((double) emissionRate < 1.0)
            emissionRate = 0.0f;
          EnviroSky.SetEmissionRate(this.Weather.WeatherPrefabs[index1].effectSystems[index2], emissionRate);
          if ((double) emissionRate == 0.0 && !this.Weather.WeatherPrefabs[index1].effectSystems[index2].get_isStopped())
            this.Weather.WeatherPrefabs[index1].effectSystems[index2].Stop();
        }
      }
    }
    this.UpdateWeatherVariables(id.weatherPreset);
  }

  private void UpdateWeatherVariables(EnviroWeatherPreset p)
  {
    this.Components.windZone.set_windMain(p.WindStrenght);
    this.Weather.wetness = (double) this.Weather.wetness >= (double) p.wetnessLevel ? Mathf.Lerp(this.Weather.curWetness, p.wetnessLevel, this.weatherSettings.wetnessDryingSpeed * Time.get_deltaTime()) : Mathf.Lerp(this.Weather.curWetness, p.wetnessLevel, this.weatherSettings.wetnessAccumulationSpeed * Time.get_deltaTime());
    this.Weather.wetness = Mathf.Clamp(this.Weather.wetness, 0.0f, 1f);
    this.Weather.curWetness = this.Weather.wetness;
    this.Weather.snowStrength = (double) this.Weather.snowStrength >= (double) p.snowLevel ? Mathf.Lerp(this.Weather.curSnowStrength, p.snowLevel, this.weatherSettings.snowMeltingSpeed * Time.get_deltaTime()) : Mathf.Lerp(this.Weather.curSnowStrength, p.snowLevel, this.weatherSettings.snowAccumulationSpeed * Time.get_deltaTime());
    this.Weather.snowStrength = Mathf.Clamp(this.Weather.snowStrength, 0.0f, 1f);
    this.Weather.curSnowStrength = this.Weather.snowStrength;
    Shader.SetGlobalFloat("_EnviroGrassSnow", this.Weather.curSnowStrength);
  }

  public static float GetEmissionRate(ParticleSystem system)
  {
    ParticleSystem.EmissionModule emission = system.get_emission();
    ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
    return ((ParticleSystem.MinMaxCurve) ref rateOverTime).get_constantMax();
  }

  public static void SetEmissionRate(ParticleSystem sys, float emissionRate)
  {
    ParticleSystem.EmissionModule emission = sys.get_emission();
    ParticleSystem.MinMaxCurve rateOverTime = ((ParticleSystem.EmissionModule) ref emission).get_rateOverTime();
    ((ParticleSystem.MinMaxCurve) ref rateOverTime).set_constantMax(emissionRate);
    ((ParticleSystem.EmissionModule) ref emission).set_rateOverTime(rateOverTime);
  }

  [DebuggerHidden]
  private IEnumerator PlayThunderRandom()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroSky.\u003CPlayThunderRandom\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator PlayLightningEffect(Vector3 position)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroSky.\u003CPlayLightningEffect\u003Ec__Iterator2()
    {
      position = position,
      \u0024this = this
    };
  }

  public void PlayLightning()
  {
    if (Object.op_Inequality((Object) this.lightningEffect, (Object) null))
      this.StartCoroutine(this.PlayLightningEffect(new Vector3((float) ((Component) this).get_transform().get_position().x + Random.Range(-this.weatherSettings.lightningRange, this.weatherSettings.lightningRange), this.weatherSettings.lightningHeight, (float) ((Component) this).get_transform().get_position().z + Random.Range(-this.weatherSettings.lightningRange, this.weatherSettings.lightningRange))));
    if (this.audioSettings.ThunderSFX != null && 0 < this.audioSettings.ThunderSFX.Count)
    {
      AudioClip audioClip = this.audioSettings.ThunderSFX[Random.Range(0, this.audioSettings.ThunderSFX.Count)];
      if (Object.op_Inequality((Object) audioClip, (Object) null))
      {
        this.AudioSourceThunder.set_clip(audioClip);
        this.AudioSourceThunder.set_loop(false);
        this.AudioSourceThunder.Play();
      }
    }
    this.Components.LightningGenerator.Lightning();
  }

  private void UpdateWeather()
  {
    if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPreset, (Object) this.Weather.currentActiveZone.currentActiveZoneWeatherPreset))
    {
      this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
      this.Weather.lastActiveWeatherPrefab = this.Weather.currentActiveWeatherPrefab;
      this.Weather.currentActiveWeatherPreset = this.Weather.currentActiveZone.currentActiveZoneWeatherPreset;
      this.Weather.currentActiveWeatherPrefab = this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab;
      if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPreset, (Object) null))
      {
        this.NotifyWeatherChanged(this.Weather.currentActiveWeatherPreset);
        this.Weather.weatherFullyChanged = false;
        if (!this.serverMode)
        {
          if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPrefab, (Object) null) && this.Weather.currentActiveWeatherPrefab.weatherPreset.isLightningStorm)
          {
            this.StartCoroutine(this.PlayThunderRandom());
          }
          else
          {
            this.StopCoroutine(this.PlayThunderRandom());
            this.Components.LightningGenerator.StopLightning();
          }
        }
      }
    }
    if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPrefab, (Object) null) && !this.serverMode)
    {
      this.UpdateClouds(this.Weather.currentActiveWeatherPreset, true);
      this.UpdateFog(this.Weather.currentActiveWeatherPreset, true);
      this.UpdateEffectSystems(this.Weather.currentActiveWeatherPrefab, true);
      if (this.Weather.weatherFullyChanged)
        return;
      this.CalcWeatherTransitionState();
    }
    else
    {
      if (!Object.op_Inequality((Object) this.Weather.currentActiveWeatherPrefab, (Object) null))
        return;
      this.UpdateWeatherVariables(this.Weather.currentActiveWeatherPrefab.weatherPreset);
    }
  }

  public void ForceWeatherUpdate()
  {
    this.Weather.lastActiveWeatherPreset = this.Weather.currentActiveWeatherPreset;
    this.Weather.lastActiveWeatherPrefab = this.Weather.currentActiveWeatherPrefab;
    this.Weather.currentActiveWeatherPreset = this.Weather.currentActiveZone.currentActiveZoneWeatherPreset;
    this.Weather.currentActiveWeatherPrefab = this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab;
    if (!Object.op_Inequality((Object) this.Weather.currentActiveWeatherPreset, (Object) null))
      return;
    this.NotifyWeatherChanged(this.Weather.currentActiveWeatherPreset);
    this.Weather.weatherFullyChanged = false;
    if (this.serverMode)
      return;
    if (Object.op_Inequality((Object) this.Weather.currentActiveWeatherPrefab, (Object) null) && this.Weather.currentActiveWeatherPrefab.weatherPreset.isLightningStorm)
    {
      this.StartCoroutine(this.PlayThunderRandom());
    }
    else
    {
      this.StopCoroutine(this.PlayThunderRandom());
      this.Components.LightningGenerator.StopLightning();
    }
  }

  private void CalcWeatherTransitionState()
  {
    this.Weather.weatherFullyChanged = (double) this.cloudsConfig.coverage >= (double) this.Weather.currentActiveWeatherPreset.cloudsConfig.coverage - 0.00999999977648258;
  }

  public void SetWeatherOverwrite(int weatherId)
  {
    if (weatherId < 0 || weatherId > this.Weather.WeatherPrefabs.Count)
      return;
    if (Object.op_Inequality((Object) this.Weather.WeatherPrefabs[weatherId], (Object) this.Weather.currentActiveWeatherPrefab))
    {
      this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[weatherId];
      this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = this.Weather.WeatherPrefabs[weatherId].weatherPreset;
      EnviroSky.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[weatherId].weatherPreset, this.Weather.currentActiveZone);
    }
    this.UpdateClouds(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, false);
    this.UpdateFog(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, false);
    this.UpdateEffectSystems(this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab, false);
  }

  public void SetWeatherOverwrite(EnviroWeatherPreset preset)
  {
    if (Object.op_Equality((Object) preset, (Object) null))
      return;
    if (Object.op_Inequality((Object) preset, (Object) this.Weather.currentActiveWeatherPreset))
    {
      for (int index = 0; index < this.Weather.WeatherPrefabs.Count; ++index)
      {
        if (Object.op_Equality((Object) preset, (Object) this.Weather.WeatherPrefabs[index].weatherPreset))
        {
          this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[index];
          this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = preset;
          EnviroSky.instance.NotifyZoneWeatherChanged(preset, this.Weather.currentActiveZone);
        }
      }
    }
    this.UpdateClouds(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, false);
    this.UpdateFog(this.Weather.currentActiveZone.currentActiveZoneWeatherPreset, false);
    this.UpdateEffectSystems(this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab, false);
  }

  public void ChangeWeather(int weatherId)
  {
    if (weatherId < 0 || weatherId >= this.Weather.WeatherPrefabs.Count || !Object.op_Inequality((Object) this.Weather.WeatherPrefabs[weatherId], (Object) this.Weather.currentActiveWeatherPrefab))
      return;
    this.Weather.currentActiveZone.currentActiveZoneWeatherPrefab = this.Weather.WeatherPrefabs[weatherId];
    this.Weather.currentActiveZone.currentActiveZoneWeatherPreset = this.Weather.WeatherPrefabs[weatherId].weatherPreset;
    EnviroSky.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[weatherId].weatherPreset, this.Weather.currentActiveZone);
  }

  public void ChangeWeather(string weatherName)
  {
    for (int weatherId = 0; weatherId < this.Weather.WeatherPrefabs.Count; ++weatherId)
    {
      if (this.Weather.WeatherPrefabs[weatherId].weatherPreset.Name == weatherName && Object.op_Inequality((Object) this.Weather.WeatherPrefabs[weatherId], (Object) this.Weather.currentActiveWeatherPrefab))
      {
        this.ChangeWeather(weatherId);
        EnviroSky.instance.NotifyZoneWeatherChanged(this.Weather.WeatherPrefabs[weatherId].weatherPreset, this.Weather.currentActiveZone);
      }
    }
  }

  public void ChangeCloudsQuality(EnviroCloudSettings.CloudQuality q)
  {
    switch (q)
    {
      case EnviroCloudSettings.CloudQuality.Lowest:
        this.cloudsSettings.bottomCloudHeight = 2000f;
        this.cloudsSettings.topCloudHeight = 4000f;
        this.cloudsSettings.cloudsWorldScale = 120000f;
        this.cloudsSettings.raymarchSteps = 75;
        this.cloudsSettings.stepsInDepthModificator = 0.7f;
        this.cloudsSettings.cloudsRenderResolution = 2;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Medium;
        this.cloudsSettings.baseNoiseUV = 26f;
        this.cloudsSettings.detailNoiseUV = 1f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.Low:
        this.cloudsSettings.bottomCloudHeight = 2000f;
        this.cloudsSettings.topCloudHeight = 4000f;
        this.cloudsSettings.cloudsWorldScale = 120000f;
        this.cloudsSettings.raymarchSteps = 90;
        this.cloudsSettings.stepsInDepthModificator = 0.7f;
        this.cloudsSettings.cloudsRenderResolution = 2;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Low;
        this.cloudsSettings.baseNoiseUV = 30f;
        this.cloudsSettings.detailNoiseUV = 1f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.Medium:
        this.cloudsSettings.bottomCloudHeight = 2000f;
        this.cloudsSettings.topCloudHeight = 4500f;
        this.cloudsSettings.cloudsWorldScale = 120000f;
        this.cloudsSettings.raymarchSteps = 100;
        this.cloudsSettings.stepsInDepthModificator = 0.7f;
        this.cloudsSettings.cloudsRenderResolution = 1;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Medium;
        this.cloudsSettings.baseNoiseUV = 35f;
        this.cloudsSettings.detailNoiseUV = 50f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.High:
        this.cloudsSettings.bottomCloudHeight = 2000f;
        this.cloudsSettings.topCloudHeight = 5000f;
        this.cloudsSettings.cloudsWorldScale = 120000f;
        this.cloudsSettings.raymarchSteps = 128;
        this.cloudsSettings.stepsInDepthModificator = 0.6f;
        this.cloudsSettings.cloudsRenderResolution = 1;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Medium;
        this.cloudsSettings.baseNoiseUV = 40f;
        this.cloudsSettings.detailNoiseUV = 50f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.Ultra:
        this.cloudsSettings.bottomCloudHeight = 2000f;
        this.cloudsSettings.topCloudHeight = 5500f;
        this.cloudsSettings.cloudsWorldScale = 120000f;
        this.cloudsSettings.raymarchSteps = 150;
        this.cloudsSettings.stepsInDepthModificator = 0.5f;
        this.cloudsSettings.cloudsRenderResolution = 1;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Low;
        this.cloudsSettings.baseNoiseUV = 40f;
        this.cloudsSettings.detailNoiseUV = 70f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.VR_Low:
        this.cloudsSettings.bottomCloudHeight = 3000f;
        this.cloudsSettings.topCloudHeight = 4200f;
        this.cloudsSettings.cloudsWorldScale = 30000f;
        this.cloudsSettings.raymarchSteps = 60;
        this.cloudsSettings.cloudsRenderResolution = 2;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Low;
        this.cloudsSettings.baseNoiseUV = 20f;
        this.cloudsSettings.detailNoiseUV = 1f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.VR_Medium:
        this.cloudsSettings.bottomCloudHeight = 3000f;
        this.cloudsSettings.topCloudHeight = 4500f;
        this.cloudsSettings.cloudsWorldScale = 30000f;
        this.cloudsSettings.raymarchSteps = 75;
        this.cloudsSettings.cloudsRenderResolution = 1;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Medium;
        this.cloudsSettings.baseNoiseUV = 22f;
        this.cloudsSettings.detailNoiseUV = 1f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.VR_High:
        this.cloudsSettings.bottomCloudHeight = 3000f;
        this.cloudsSettings.topCloudHeight = 4500f;
        this.cloudsSettings.cloudsWorldScale = 30000f;
        this.cloudsSettings.raymarchSteps = 80;
        this.cloudsSettings.cloudsRenderResolution = 1;
        this.cloudsSettings.reprojectionPixelSize = EnviroCloudSettings.ReprojectionPixelSize.Medium;
        this.cloudsSettings.baseNoiseUV = 23f;
        this.cloudsSettings.detailNoiseUV = 1f;
        this.cloudsSettings.detailQuality = EnviroCloudSettings.CloudDetailQuality.Low;
        break;
      case EnviroCloudSettings.CloudQuality.Custom:
        return;
    }
    this.lastCloudsQuality = q;
    this.cloudsSettings.cloudsQuality = q;
  }

  public int GetActiveWeatherID()
  {
    for (int index = 0; index < this.Weather.WeatherPrefabs.Count; ++index)
    {
      if (Object.op_Equality((Object) this.Weather.WeatherPrefabs[index].weatherPreset, (Object) this.Weather.currentActiveWeatherPreset))
        return index;
    }
    return -1;
  }

  public void Save()
  {
    PlayerPrefs.SetFloat("Time_Hours", this.internalHour);
    PlayerPrefs.SetInt("Time_Days", this.GameTime.Days);
    PlayerPrefs.SetInt("Time_Years", this.GameTime.Years);
    for (int index = 0; index < this.Weather.WeatherPrefabs.Count; ++index)
    {
      if (Object.op_Equality((Object) this.Weather.WeatherPrefabs[index], (Object) this.Weather.currentActiveWeatherPrefab))
        PlayerPrefs.SetInt("currentWeather", index);
    }
  }

  public void Load()
  {
    if (PlayerPrefs.HasKey("Time_Hours"))
      this.internalHour = PlayerPrefs.GetFloat("Time_Hours");
    if (PlayerPrefs.HasKey("Time_Days"))
      this.GameTime.Days = PlayerPrefs.GetInt("Time_Days");
    if (PlayerPrefs.HasKey("Time_Years"))
      this.GameTime.Years = PlayerPrefs.GetInt("Time_Years");
    if (!PlayerPrefs.HasKey("currentWeather"))
      return;
    this.SetWeatherOverwrite(PlayerPrefs.GetInt("currentWeather"));
  }

  public void SetTime(DateTime date)
  {
    this.GameTime.Years = date.Year;
    this.GameTime.Days = date.DayOfYear;
    this.GameTime.Minutes = date.Minute;
    this.GameTime.Seconds = date.Second;
    this.GameTime.Hours = date.Hour;
    this.internalHour = (float) ((double) date.Hour + (double) date.Minute * 0.0166666992008686 + (double) date.Second * 0.00027777798823081);
  }

  public void SetTime(int year, int dayOfYear, int hour, int minute, int seconds)
  {
    this.GameTime.Years = year;
    this.GameTime.Days = dayOfYear;
    this.GameTime.Minutes = minute;
    this.GameTime.Hours = hour;
    this.internalHour = (float) ((double) hour + (double) minute * 0.0166666992008686 + (double) seconds * 0.00027777798823081);
  }

  public void SetInternalTimeOfDay(float inHours)
  {
    this.internalHour = inHours;
    this.GameTime.Hours = (int) inHours;
    inHours -= (float) this.GameTime.Hours;
    this.GameTime.Minutes = (int) ((double) inHours * 60.0);
    inHours -= (float) this.GameTime.Minutes * 0.0166667f;
    this.GameTime.Seconds = (int) ((double) inHours * 3600.0);
  }

  public string GetTimeStringWithSeconds()
  {
    return string.Format("{0:00}:{1:00}:{2:00}", (object) this.GameTime.Hours, (object) this.GameTime.Minutes, (object) this.GameTime.Seconds);
  }

  public string GetTimeString()
  {
    return string.Format("{0:00}:{1:00}", (object) this.GameTime.Hours, (object) this.GameTime.Minutes);
  }

  public float GetUniversalTimeOfDay()
  {
    return this.internalHour - (float) this.GameTime.utcOffset;
  }

  public double GetInHours(float hours, float days, float years)
  {
    return (double) hours + (double) days * 24.0 + (double) years * ((double) this.seasonsSettings.SpringInDays + (double) this.seasonsSettings.SummerInDays + (double) this.seasonsSettings.AutumnInDays + (double) this.seasonsSettings.WinterInDays) * 24.0;
  }

  public void AssignAndStart(GameObject player, Camera Camera)
  {
    this.Player = player;
    this.PlayerCamera = Camera;
    this.Init();
    this.started = true;
    if (!Singleton<Manager.Map>.IsInstance() || !Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null) || !Object.op_Inequality((Object) player, (Object) null))
      return;
    Singleton<Manager.Map>.Instance.Simulator.SetEnviroParticleTarget(player.get_transform());
  }

  public void StartAsServer()
  {
    this.Player = ((Component) this).get_gameObject();
    this.serverMode = true;
    this.Init();
  }

  public void ChangeFocus(GameObject player, Camera Camera)
  {
    this.Player = player;
    this.RemoveEnviroCameraComponents(this.PlayerCamera);
    this.PlayerCamera = Camera;
    this.InitImageEffects();
    if (!Singleton<Manager.Map>.IsInstance() || !Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null) || !Object.op_Inequality((Object) player, (Object) null))
      return;
    Singleton<Manager.Map>.Instance.Simulator?.SetEnviroParticleTarget(player.get_transform());
  }

  private void RemoveEnviroCameraComponents(Camera cam)
  {
    EnviroFog component1 = (EnviroFog) ((Component) cam).GetComponent<EnviroFog>();
    if (Object.op_Inequality((Object) component1, (Object) null))
      Object.Destroy((Object) component1);
    foreach (Object component2 in (EnviroLightShafts[]) ((Component) cam).GetComponents<EnviroLightShafts>())
      Object.Destroy(component2);
    EnviroSkyRendering component3 = (EnviroSkyRendering) ((Component) cam).GetComponent<EnviroSkyRendering>();
    if (!Object.op_Inequality((Object) component3, (Object) null))
      return;
    Object.Destroy((Object) component3);
  }

  public void Play(EnviroTime.TimeProgressMode progressMode = EnviroTime.TimeProgressMode.Simulated)
  {
    this.SetupSkybox();
    if (!((Component) this.Components.DirectLight).get_gameObject().get_activeSelf())
      ((Component) this.Components.DirectLight).get_gameObject().SetActive(true);
    this.GameTime.ProgressTime = progressMode;
    this.EffectsHolder.SetActive(true);
    ((Behaviour) this.EnviroSkyRender).set_enabled(true);
    this.started = true;
  }

  public void Stop(bool disableLight = false, bool stopTime = true)
  {
    if (disableLight)
      ((Component) this.Components.DirectLight).get_gameObject().SetActive(false);
    if (stopTime)
      this.GameTime.ProgressTime = EnviroTime.TimeProgressMode.None;
    this.EffectsHolder.SetActive(false);
    ((Behaviour) this.EnviroSkyRender).set_enabled(false);
    ((Behaviour) this.lightShaftsScriptSun).set_enabled(false);
    ((Behaviour) this.lightShaftsScriptMoon).set_enabled(false);
    this.started = false;
  }

  public enum EnviroCloudsMode
  {
    None,
    Both,
    Volume,
    Flat,
  }

  public delegate void HourPassed();

  public delegate void DayPassed();

  public delegate void YearPassed();

  public delegate void WeatherChanged(EnviroWeatherPreset weatherType);

  public delegate void ZoneWeatherChanged(EnviroWeatherPreset weatherType, EnviroZone zone);

  public delegate void SeasonChanged(EnviroSeasons.Seasons season);

  public delegate void isNightE();

  public delegate void isDay();

  public delegate void ZoneChanged(EnviroZone zone);
}
