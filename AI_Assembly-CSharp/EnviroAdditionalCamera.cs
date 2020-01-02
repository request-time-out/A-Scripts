// Decompiled with JetBrains decompiler
// Type: EnviroAdditionalCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Enviro/AddionalCamera")]
public class EnviroAdditionalCamera : MonoBehaviour
{
  private Camera myCam;
  private EnviroSkyRendering skyRender;
  private EnviroLightShafts lightShaftsScriptSun;
  private EnviroLightShafts lightShaftsScriptMoon;

  public EnviroAdditionalCamera()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    this.myCam = (Camera) ((Component) this).GetComponent<Camera>();
    if (!Object.op_Inequality((Object) this.myCam, (Object) null))
      return;
    this.InitImageEffects();
  }

  private void Update()
  {
    this.UpdateCameraComponents();
  }

  private void InitImageEffects()
  {
    this.skyRender = (EnviroSkyRendering) ((Component) this.myCam).get_gameObject().GetComponent<EnviroSkyRendering>();
    if (Object.op_Equality((Object) this.skyRender, (Object) null))
      this.skyRender = (EnviroSkyRendering) ((Component) this.myCam).get_gameObject().AddComponent<EnviroSkyRendering>();
    this.skyRender.isAddionalCamera = true;
    EnviroLightShafts[] components = (EnviroLightShafts[]) ((Component) this.myCam).get_gameObject().GetComponents<EnviroLightShafts>();
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
      this.lightShaftsScriptSun = (EnviroLightShafts) ((Component) this.myCam).get_gameObject().AddComponent<EnviroLightShafts>();
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
      this.lightShaftsScriptMoon = (EnviroLightShafts) ((Component) this.myCam).get_gameObject().AddComponent<EnviroLightShafts>();
      this.lightShaftsScriptMoon.sunShaftsMaterial = new Material(Shader.Find("Enviro/Effects/LightShafts"));
      this.lightShaftsScriptMoon.sunShaftsShader = this.lightShaftsScriptMoon.sunShaftsMaterial.get_shader();
      this.lightShaftsScriptMoon.simpleClearMaterial = new Material(Shader.Find("Enviro/Effects/ClearLightShafts"));
      this.lightShaftsScriptMoon.simpleClearShader = this.lightShaftsScriptMoon.simpleClearMaterial.get_shader();
    }
  }

  private void UpdateCameraComponents()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
      return;
    if (Object.op_Inequality((Object) this.skyRender, (Object) null))
    {
      this.skyRender.dirVolumeLighting = EnviroSky.instance.volumeLightSettings.dirVolumeLighting;
      this.skyRender.volumeLighting = EnviroSky.instance.volumeLighting;
      this.skyRender.distanceFog = EnviroSky.instance.fogSettings.distanceFog;
      this.skyRender.heightFog = EnviroSky.instance.fogSettings.heightFog;
      this.skyRender.height = EnviroSky.instance.fogSettings.height;
      this.skyRender.heightDensity = EnviroSky.instance.fogSettings.heightDensity;
      this.skyRender.useRadialDistance = EnviroSky.instance.fogSettings.useRadialDistance;
      this.skyRender.startDistance = EnviroSky.instance.fogSettings.startDistance;
    }
    if (Object.op_Inequality((Object) this.lightShaftsScriptSun, (Object) null))
    {
      this.lightShaftsScriptSun.resolution = EnviroSky.instance.lightshaftsSettings.resolution;
      this.lightShaftsScriptSun.screenBlendMode = EnviroSky.instance.lightshaftsSettings.screenBlendMode;
      this.lightShaftsScriptSun.useDepthTexture = EnviroSky.instance.lightshaftsSettings.useDepthTexture;
      this.lightShaftsScriptSun.sunThreshold = EnviroSky.instance.lightshaftsSettings.thresholdColorSun.Evaluate(EnviroSky.instance.GameTime.solarTime);
      this.lightShaftsScriptSun.sunShaftBlurRadius = EnviroSky.instance.lightshaftsSettings.blurRadius;
      this.lightShaftsScriptSun.sunShaftIntensity = EnviroSky.instance.lightshaftsSettings.intensity;
      this.lightShaftsScriptSun.maxRadius = EnviroSky.instance.lightshaftsSettings.maxRadius;
      this.lightShaftsScriptSun.sunColor = EnviroSky.instance.lightshaftsSettings.lightShaftsColorSun.Evaluate(EnviroSky.instance.GameTime.solarTime);
      this.lightShaftsScriptSun.sunTransform = EnviroSky.instance.Components.Sun.get_transform();
      if (EnviroSky.instance.LightShafts.sunLightShafts)
        ((Behaviour) this.lightShaftsScriptSun).set_enabled(true);
      else
        ((Behaviour) this.lightShaftsScriptSun).set_enabled(false);
    }
    if (!Object.op_Inequality((Object) this.lightShaftsScriptMoon, (Object) null))
      return;
    this.lightShaftsScriptMoon.resolution = EnviroSky.instance.lightshaftsSettings.resolution;
    this.lightShaftsScriptMoon.screenBlendMode = EnviroSky.instance.lightshaftsSettings.screenBlendMode;
    this.lightShaftsScriptMoon.useDepthTexture = EnviroSky.instance.lightshaftsSettings.useDepthTexture;
    this.lightShaftsScriptMoon.sunThreshold = EnviroSky.instance.lightshaftsSettings.thresholdColorMoon.Evaluate(EnviroSky.instance.GameTime.lunarTime);
    this.lightShaftsScriptMoon.sunShaftBlurRadius = EnviroSky.instance.lightshaftsSettings.blurRadius;
    this.lightShaftsScriptMoon.sunShaftIntensity = Mathf.Clamp(EnviroSky.instance.lightshaftsSettings.intensity - EnviroSky.instance.GameTime.solarTime, 0.0f, 100f);
    this.lightShaftsScriptMoon.maxRadius = EnviroSky.instance.lightshaftsSettings.maxRadius;
    this.lightShaftsScriptMoon.sunColor = EnviroSky.instance.lightshaftsSettings.lightShaftsColorMoon.Evaluate(EnviroSky.instance.GameTime.lunarTime);
    this.lightShaftsScriptMoon.sunTransform = EnviroSky.instance.Components.Moon.get_transform();
    if (EnviroSky.instance.LightShafts.moonLightShafts)
      ((Behaviour) this.lightShaftsScriptMoon).set_enabled(true);
    else
      ((Behaviour) this.lightShaftsScriptMoon).set_enabled(false);
  }
}
