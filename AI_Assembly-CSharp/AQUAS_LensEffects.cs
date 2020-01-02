// Decompiled with JetBrains decompiler
// Type: AQUAS_LensEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Reflection;
using UnityEngine;

public class AQUAS_LensEffects : MonoBehaviour
{
  public AQUAS_Parameters.UnderWaterParameters underWaterParameters;
  public AQUAS_Parameters.GameObjects gameObjects;
  public AQUAS_Parameters.BubbleSpawnCriteria bubbleSpawnCriteria;
  public AQUAS_Parameters.WetLens wetLens;
  public AQUAS_Parameters.CausticSettings causticSettings;
  public AQUAS_Parameters.Audio soundEffects;
  private int sprayFrameIndex;
  private GameObject tenkokuObj;
  private Material airLensMaterial;
  private Material waterPlaneMaterial;
  [HideInInspector]
  public float t;
  private float t2;
  private float bubbleSpawnTimer;
  private float defaultFogDensity;
  private Color defaultFogColor;
  private float defaultFoamContrast;
  private float defaultBloomIntensity;
  private float defaultSpecularity;
  private float defaultRefraction;
  private bool defaultFog;
  private bool defaultSunShaftsEnabled;
  private bool defaultBloomEnabled;
  private bool defaultBlurEnabled;
  private bool defaultVignetteEnabled;
  private bool defaultNoiseEnabled;
  [HideInInspector]
  public bool setAfloatFog;
  [HideInInspector]
  public bool rundown;
  private bool playSurfaceSplash;
  private bool playDiveSplash;
  private bool playUnderwater;
  private int bubbleCount;
  private int maxBubbleCount;
  private int activePlane;
  private int lastActivePlane;
  private FieldInfo fi;
  private AudioSource waterLensAudio;
  private AudioSource airLensAudio;
  private AudioSource audioComp;
  private AudioSource cameraAudio;
  private Projector primaryCausticsProjector;
  private Projector secondaryCausticsProjector;
  private AQUAS_Caustics primaryAquasCaustics;
  private AQUAS_Caustics secondaryAquasCaustics;
  private AQUAS_BubbleBehaviour bubbleBehaviour;

  public AQUAS_LensEffects()
  {
    base.\u002Ector();
  }

  public bool underWater { get; private set; }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.gameObjects.waterLens, (Object) null))
      this.waterLensAudio = (AudioSource) this.gameObjects.waterLens.GetComponent<AudioSource>();
    if (Object.op_Inequality((Object) this.gameObjects.airLens, (Object) null))
      this.airLensAudio = (AudioSource) this.gameObjects.airLens.GetComponent<AudioSource>();
    this.audioComp = (AudioSource) ((Component) this).GetComponent<AudioSource>();
    this.cameraAudio = (AudioSource) this.gameObjects.mainCamera.GetComponent<AudioSource>();
    this.bubbleBehaviour = (AQUAS_BubbleBehaviour) this.gameObjects.bubble.GetComponent<AQUAS_BubbleBehaviour>();
    if (Object.op_Inequality((Object) this.gameObjects.airLens, (Object) null))
      this.gameObjects.airLens.SetActive(true);
    if (Object.op_Inequality((Object) this.gameObjects.waterLens, (Object) null))
      this.gameObjects.waterLens.SetActive(false);
    this.waterPlaneMaterial = ((Renderer) this.gameObjects.waterPlanes[0].GetComponent<Renderer>()).get_material();
    this.t = this.wetLens.wetTime + this.wetLens.dryingTime;
    this.t2 = 0.0f;
    this.bubbleSpawnTimer = 0.0f;
    this.defaultFog = RenderSettings.get_fog();
    this.defaultFogDensity = RenderSettings.get_fogDensity();
    this.defaultFogColor = RenderSettings.get_fogColor();
    this.defaultFoamContrast = this.waterPlaneMaterial.GetFloat("_FoamContrast");
    this.defaultSpecularity = this.waterPlaneMaterial.GetFloat("_Specular");
    if (this.waterPlaneMaterial.HasProperty("_Refraction"))
      this.defaultRefraction = this.waterPlaneMaterial.GetFloat("_Refraction");
    this.audioComp.set_clip(this.soundEffects.sounds[0]);
    this.audioComp.set_loop(true);
    this.audioComp.Stop();
    if (Object.op_Inequality((Object) this.airLensAudio, (Object) null))
    {
      this.airLensAudio.set_clip(this.soundEffects.sounds[1]);
      this.airLensAudio.set_loop(false);
      this.airLensAudio.Stop();
    }
    if (Object.op_Inequality((Object) this.waterLensAudio, (Object) null))
    {
      this.waterLensAudio.set_clip(this.soundEffects.sounds[2]);
      this.waterLensAudio.set_loop(false);
      this.waterLensAudio.Stop();
    }
    if (!Object.op_Inequality((Object) GameObject.Find("Tenkoku DynamicSky"), (Object) null))
      return;
    this.tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
  }

  private void Update()
  {
    this.CheckIfStillUnderWater();
    if (this.underWater)
    {
      this.t = 0.0f;
      this.t2 += Time.get_deltaTime();
      if (Object.op_Inequality((Object) this.gameObjects.airLens, (Object) null))
        this.gameObjects.airLens.SetActive(false);
      if (Object.op_Inequality((Object) this.gameObjects.waterLens, (Object) null))
        this.gameObjects.waterLens.SetActive(true);
      this.sprayFrameIndex = 0;
      this.rundown = true;
      this.BubbleSpawner();
      if (this.playUnderwater)
      {
        this.audioComp.Play();
        this.playUnderwater = false;
      }
      if (this.playDiveSplash)
      {
        this.waterLensAudio.Play();
        this.playDiveSplash = false;
      }
      this.playSurfaceSplash = true;
      if (Object.op_Inequality((Object) this.airLensAudio, (Object) null))
        this.airLensAudio.Stop();
      if (Object.op_Inequality((Object) this.cameraAudio, (Object) null))
        ((Behaviour) this.cameraAudio).set_enabled(false);
      if (Object.op_Inequality((Object) this.airLensAudio, (Object) null))
        this.airLensAudio.set_volume(this.soundEffects.surfacingVolume);
      this.audioComp.set_volume(this.soundEffects.diveVolume);
      if (Object.op_Inequality((Object) this.waterLensAudio, (Object) null))
        this.waterLensAudio.set_volume(this.soundEffects.underwaterVolume);
      if (Object.op_Inequality((Object) this.primaryCausticsProjector, (Object) null))
      {
        this.primaryCausticsProjector.get_material().SetTextureScale("_Texture", new Vector2((float) this.causticSettings.causticTiling.y, (float) this.causticSettings.causticTiling.y));
        this.primaryCausticsProjector.get_material().SetFloat("_Intensity", (float) this.causticSettings.causticIntensity.y);
        this.primaryAquasCaustics.maxCausticDepth = this.causticSettings.maxCausticDepth;
      }
      if (Object.op_Inequality((Object) this.secondaryCausticsProjector, (Object) null))
      {
        this.secondaryCausticsProjector.get_material().SetTextureScale("_Texture", new Vector2((float) this.causticSettings.causticTiling.y, (float) this.causticSettings.causticTiling.y));
        this.secondaryCausticsProjector.get_material().SetFloat("_Intensity", (float) this.causticSettings.causticIntensity.y);
        this.secondaryAquasCaustics.maxCausticDepth = this.causticSettings.maxCausticDepth;
      }
      this.waterPlaneMaterial.SetFloat("_UnderwaterMode", 1f);
      this.waterPlaneMaterial.SetFloat("_FoamContrast", 0.0f);
      this.waterPlaneMaterial.SetFloat("_Specular", this.defaultSpecularity * 5f);
      this.waterPlaneMaterial.SetFloat("_Refraction", 0.7f);
      if (Object.op_Inequality((Object) this.tenkokuObj, (Object) null))
      {
        Component component = this.tenkokuObj.GetComponent("TenkokuModule");
        FieldInfo field = ((object) component).GetType().GetField("enableFog", BindingFlags.Instance | BindingFlags.Public);
        if (field != (FieldInfo) null)
          field.SetValue((object) component, (object) false);
      }
      RenderSettings.set_fog(true);
      RenderSettings.set_fogDensity(this.underWaterParameters.fogDensity);
      RenderSettings.set_fogColor(this.underWaterParameters.fogColor);
    }
    else
    {
      this.t2 = 0.0f;
      this.t += Time.get_deltaTime();
      if (Object.op_Inequality((Object) this.gameObjects.airLens, (Object) null))
        this.gameObjects.airLens.SetActive(true);
      if (Object.op_Inequality((Object) this.gameObjects.waterLens, (Object) null))
        this.gameObjects.waterLens.SetActive(false);
      if (this.rundown)
      {
        this.sprayFrameIndex = 0;
        this.NextFrame();
        this.InvokeRepeating("NextFrame", 1f / this.wetLens.rundownSpeed, 1f / this.wetLens.rundownSpeed);
        this.rundown = false;
      }
      this.bubbleCount = 0;
      this.maxBubbleCount = Random.Range(this.bubbleSpawnCriteria.minBubbleCount, this.bubbleSpawnCriteria.maxBubbleCount);
      this.bubbleSpawnTimer = 0.0f;
      if (this.playSurfaceSplash)
      {
        this.airLensAudio.Play();
        this.playSurfaceSplash = false;
      }
      this.playUnderwater = true;
      this.playDiveSplash = true;
      this.audioComp.Stop();
      if (Object.op_Inequality((Object) this.waterLensAudio, (Object) null))
        this.waterLensAudio.Stop();
      if (Object.op_Inequality((Object) this.cameraAudio, (Object) null))
        ((Behaviour) this.cameraAudio).set_enabled(true);
      if (Object.op_Inequality((Object) this.primaryCausticsProjector, (Object) null))
      {
        this.primaryCausticsProjector.get_material().SetTextureScale("_Texture", new Vector2((float) this.causticSettings.causticTiling.x, (float) this.causticSettings.causticTiling.x));
        this.primaryCausticsProjector.get_material().SetFloat("_Intensity", (float) this.causticSettings.causticIntensity.x);
      }
      if (Object.op_Inequality((Object) this.secondaryCausticsProjector, (Object) null))
      {
        this.secondaryCausticsProjector.get_material().SetTextureScale("_Texture", new Vector2((float) this.causticSettings.causticTiling.x, (float) this.causticSettings.causticTiling.x));
        this.secondaryCausticsProjector.get_material().SetFloat("_Intensity", (float) this.causticSettings.causticIntensity.x);
      }
      if ((double) this.t <= (double) this.wetLens.wetTime)
      {
        if (Object.op_Inequality((Object) this.airLensMaterial, (Object) null))
        {
          this.airLensMaterial.SetFloat("_Refraction", 1f);
          this.airLensMaterial.SetFloat("_Transparency", 0.01f);
        }
      }
      else if (Object.op_Inequality((Object) this.airLensMaterial, (Object) null))
      {
        this.airLensMaterial.SetFloat("_Refraction", Mathf.Lerp(1f, 0.0f, (this.t - this.wetLens.wetTime) / this.wetLens.dryingTime));
        this.airLensMaterial.SetFloat("_Transparency", Mathf.Lerp(0.01f, 0.0f, (this.t - this.wetLens.wetTime) / this.wetLens.dryingTime));
      }
      this.waterPlaneMaterial.SetFloat("_FoamContrast", this.defaultFoamContrast);
      this.waterPlaneMaterial.SetFloat("_UnderwaterMode", 0.0f);
      this.waterPlaneMaterial.SetFloat("_Specular", this.defaultSpecularity);
      this.waterPlaneMaterial.SetFloat("_Refraction", this.defaultRefraction);
      if (Object.op_Inequality((Object) this.tenkokuObj, (Object) null))
      {
        Component component = this.tenkokuObj.GetComponent("TenkokuModule");
        FieldInfo field = ((object) component).GetType().GetField("enableFog", BindingFlags.Instance | BindingFlags.Public);
        if (field != (FieldInfo) null)
          field.SetValue((object) component, (object) true);
      }
      RenderSettings.set_fog(this.defaultFog);
      if (!this.setAfloatFog)
        return;
      RenderSettings.set_fogColor(this.defaultFogColor);
      RenderSettings.set_fogDensity(this.defaultFogDensity);
    }
  }

  private bool CheckIfUnderWater(int waterPlanesCount)
  {
    if (!this.gameObjects.useSquaredPlanes)
    {
      for (int index = 0; index < waterPlanesCount; ++index)
      {
        double num1 = (double) Mathf.Pow((float) (((Component) this).get_transform().get_position().x - this.gameObjects.waterPlanes[index].get_transform().get_position().x), 2f) + (double) Mathf.Pow((float) (((Component) this).get_transform().get_position().z - this.gameObjects.waterPlanes[index].get_transform().get_position().z), 2f);
        Bounds bounds = ((Renderer) this.gameObjects.waterPlanes[index].GetComponent<Renderer>()).get_bounds();
        double num2 = (double) Mathf.Pow((float) ((Bounds) ref bounds).get_extents().x, 2f);
        if (num1 < num2)
        {
          if (this.activePlane != this.lastActivePlane)
          {
            if (Object.op_Inequality((Object) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector"), (Object) null))
            {
              this.primaryCausticsProjector = (Projector) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector")).GetComponent<Projector>();
              this.primaryAquasCaustics = (AQUAS_Caustics) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector")).GetComponent<AQUAS_Caustics>();
            }
            if (Object.op_Inequality((Object) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector"), (Object) null))
            {
              this.secondaryCausticsProjector = (Projector) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector")).GetComponent<Projector>();
              this.secondaryAquasCaustics = (AQUAS_Caustics) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector")).GetComponent<AQUAS_Caustics>();
            }
            this.lastActivePlane = this.activePlane;
          }
          this.activePlane = index;
          if (((Component) this).get_transform().get_position().y < this.gameObjects.waterPlanes[index].get_transform().get_position().y)
          {
            this.waterPlaneMaterial = ((Renderer) this.gameObjects.waterPlanes[index].GetComponent<Renderer>()).get_material();
            this.activePlane = index;
            return true;
          }
        }
      }
    }
    else
    {
      for (int index = 0; index < waterPlanesCount; ++index)
      {
        double num1 = (double) Mathf.Abs((float) (((Component) this).get_transform().get_position().x - this.gameObjects.waterPlanes[index].get_transform().get_position().x));
        Bounds bounds1 = ((Renderer) this.gameObjects.waterPlanes[index].GetComponent<Renderer>()).get_bounds();
        // ISSUE: variable of the null type
        __Null x = ((Bounds) ref bounds1).get_extents().x;
        if (num1 < x)
        {
          double num2 = (double) Mathf.Abs((float) (((Component) this).get_transform().get_position().z - this.gameObjects.waterPlanes[index].get_transform().get_position().z));
          Bounds bounds2 = ((Renderer) this.gameObjects.waterPlanes[index].GetComponent<Renderer>()).get_bounds();
          // ISSUE: variable of the null type
          __Null z = ((Bounds) ref bounds2).get_extents().z;
          if (num2 < z)
          {
            if (this.activePlane != this.lastActivePlane)
            {
              if (Object.op_Inequality((Object) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector"), (Object) null))
              {
                this.primaryCausticsProjector = (Projector) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector")).GetComponent<Projector>();
                this.primaryAquasCaustics = (AQUAS_Caustics) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("PrimaryCausticsProjector")).GetComponent<AQUAS_Caustics>();
              }
              if (Object.op_Inequality((Object) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector"), (Object) null))
              {
                this.secondaryCausticsProjector = (Projector) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector")).GetComponent<Projector>();
                this.secondaryAquasCaustics = (AQUAS_Caustics) ((Component) this.gameObjects.waterPlanes[this.activePlane].get_transform().Find("SecondaryCausticsProjector")).GetComponent<AQUAS_Caustics>();
              }
              this.lastActivePlane = this.activePlane;
            }
            this.activePlane = index;
            if (((Component) this).get_transform().get_position().y < this.gameObjects.waterPlanes[index].get_transform().get_position().y)
            {
              this.waterPlaneMaterial = ((Renderer) this.gameObjects.waterPlanes[0].GetComponent<Renderer>()).get_material();
              this.activePlane = index;
              return true;
            }
          }
        }
      }
    }
    return false;
  }

  private void CheckIfStillUnderWater()
  {
    if (!this.gameObjects.useSquaredPlanes)
    {
      if (this.underWater)
      {
        double num1 = (double) Mathf.Pow((float) (((Component) this).get_transform().get_position().x - this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().x), 2f) + (double) Mathf.Pow((float) (((Component) this).get_transform().get_position().z - this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().z), 2f);
        Bounds bounds = ((Renderer) this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>()).get_bounds();
        double num2 = (double) Mathf.Pow((float) ((Bounds) ref bounds).get_extents().x, 2f);
        if (num1 > num2)
        {
          this.underWater = false;
          return;
        }
      }
      if (this.underWater && ((Component) this).get_transform().get_position().y > this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().y)
      {
        this.underWater = false;
      }
      else
      {
        if (this.underWater)
          return;
        this.underWater = this.CheckIfUnderWater(this.gameObjects.waterPlanes.Count);
      }
    }
    else
    {
      if (this.underWater)
      {
        double num = (double) Mathf.Abs((float) (((Component) this).get_transform().get_position().x - this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().x));
        Bounds bounds = ((Renderer) this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>()).get_bounds();
        // ISSUE: variable of the null type
        __Null x = ((Bounds) ref bounds).get_extents().x;
        if (num > x)
          goto label_12;
      }
      if (this.underWater)
      {
        double num = (double) Mathf.Abs((float) (((Component) this).get_transform().get_position().z - this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().z));
        Bounds bounds = ((Renderer) this.gameObjects.waterPlanes[this.activePlane].GetComponent<Renderer>()).get_bounds();
        // ISSUE: variable of the null type
        __Null z = ((Bounds) ref bounds).get_extents().z;
        if (num > z)
          goto label_12;
      }
      if (this.underWater && ((Component) this).get_transform().get_position().y > this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().y)
      {
        this.underWater = false;
        return;
      }
      if (this.underWater)
        return;
      this.underWater = this.CheckIfUnderWater(this.gameObjects.waterPlanes.Count);
      return;
label_12:
      this.underWater = false;
    }
  }

  private void NextFrame()
  {
    if (this.sprayFrameIndex >= this.wetLens.sprayFrames.Length - 1)
    {
      this.sprayFrameIndex = 0;
      this.CancelInvoke(nameof (NextFrame));
    }
    this.airLensMaterial.SetTexture("_CutoutReferenceTexture", (Texture) this.wetLens.sprayFramesCutout[this.sprayFrameIndex]);
    this.airLensMaterial.SetTexture("_Normal", (Texture) this.wetLens.sprayFrames[this.sprayFrameIndex]);
    ++this.sprayFrameIndex;
  }

  private void BubbleSpawner()
  {
    if ((double) this.t2 > (double) this.bubbleSpawnTimer && this.maxBubbleCount > this.bubbleCount)
    {
      float num = Random.Range(0.0f, this.bubbleSpawnCriteria.avgScaleSummand * 2f);
      this.bubbleBehaviour.mainCamera = this.gameObjects.mainCamera;
      this.bubbleBehaviour.waterLevel = (float) this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().y;
      this.bubbleBehaviour.averageUpdrift = this.bubbleSpawnCriteria.averageUpdrift + Random.Range((float) (-(double) this.bubbleSpawnCriteria.averageUpdrift * 0.75), this.bubbleSpawnCriteria.averageUpdrift * 0.75f);
      Transform transform = this.gameObjects.bubble.get_transform();
      transform.set_localScale(Vector3.op_Addition(transform.get_localScale(), new Vector3(num, num, num)));
      Object.Instantiate<GameObject>((M0) this.gameObjects.bubble, new Vector3((float) ((Component) this).get_transform().get_position().x + Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance), (float) (((Component) this).get_transform().get_position().y - 0.400000005960464), (float) ((Component) this).get_transform().get_position().z + Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.get_identity());
      this.bubbleSpawnTimer += Random.Range(this.bubbleSpawnCriteria.minSpawnTimer, this.bubbleSpawnCriteria.maxSpawnTimer);
      ++this.bubbleCount;
      this.gameObjects.bubble.get_transform().set_localScale(new Vector3(this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale));
    }
    else
    {
      if ((double) this.t2 <= (double) this.bubbleSpawnTimer || this.maxBubbleCount != this.bubbleCount)
        return;
      float num = Random.Range(0.0f, this.bubbleSpawnCriteria.avgScaleSummand * 2f);
      this.bubbleBehaviour.mainCamera = this.gameObjects.mainCamera;
      this.bubbleBehaviour.waterLevel = (float) this.gameObjects.waterPlanes[this.activePlane].get_transform().get_position().y;
      this.bubbleBehaviour.averageUpdrift = this.bubbleSpawnCriteria.averageUpdrift + Random.Range((float) (-(double) this.bubbleSpawnCriteria.averageUpdrift * 0.75), this.bubbleSpawnCriteria.averageUpdrift * 0.75f);
      Transform transform = this.gameObjects.bubble.get_transform();
      transform.set_localScale(Vector3.op_Addition(transform.get_localScale(), new Vector3(num, num, num)));
      Object.Instantiate<GameObject>((M0) this.gameObjects.bubble, new Vector3((float) ((Component) this).get_transform().get_position().x + Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance), (float) (((Component) this).get_transform().get_position().y - 0.400000005960464), (float) ((Component) this).get_transform().get_position().z + Random.Range(-this.bubbleSpawnCriteria.maxSpawnDistance, this.bubbleSpawnCriteria.maxSpawnDistance)), Quaternion.get_identity());
      this.bubbleSpawnTimer += Random.Range(this.bubbleSpawnCriteria.minSpawnTimerL, this.bubbleSpawnCriteria.maxSpawnTimerL);
      this.gameObjects.bubble.get_transform().set_localScale(new Vector3(this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale, this.bubbleSpawnCriteria.baseScale));
    }
  }
}
