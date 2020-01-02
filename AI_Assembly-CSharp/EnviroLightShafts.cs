// Decompiled with JetBrains decompiler
// Type: EnviroLightShafts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Enviro/Effects/LightShafts")]
public class EnviroLightShafts : EnviroEffects
{
  [HideInInspector]
  public EnviroLightShafts.SunShaftsResolution resolution = EnviroLightShafts.SunShaftsResolution.Normal;
  [HideInInspector]
  public int radialBlurIterations = 2;
  [HideInInspector]
  public Color sunColor = Color.get_white();
  [HideInInspector]
  public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);
  [HideInInspector]
  public float sunShaftBlurRadius = 2.5f;
  [HideInInspector]
  public float sunShaftIntensity = 1.15f;
  [HideInInspector]
  public float maxRadius = 0.75f;
  [HideInInspector]
  public bool useDepthTexture = true;
  [HideInInspector]
  public EnviroLightShafts.ShaftsScreenBlendMode screenBlendMode;
  [HideInInspector]
  public Transform sunTransform;
  [HideInInspector]
  public Shader sunShaftsShader;
  [HideInInspector]
  public Material sunShaftsMaterial;
  [HideInInspector]
  public Shader simpleClearShader;
  [HideInInspector]
  public Material simpleClearMaterial;
  private Camera cam;

  public override bool CheckResources()
  {
    this.CheckSupport(this.useDepthTexture);
    this.sunShaftsMaterial = this.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
    this.simpleClearMaterial = this.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
      Graphics.Blit((Texture) source, destination);
    else if (Object.op_Equality((Object) EnviroSky.instance, (Object) null))
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if (Object.op_Equality((Object) this.cam, (Object) null))
        this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      if (this.useDepthTexture && this.cam.get_actualRenderingPath() == 1)
      {
        Camera cam = this.cam;
        cam.set_depthTextureMode((DepthTextureMode) (cam.get_depthTextureMode() | 1));
      }
      int num1 = 4;
      if (this.resolution == EnviroLightShafts.SunShaftsResolution.Normal)
        num1 = 2;
      else if (this.resolution == EnviroLightShafts.SunShaftsResolution.High)
        num1 = 1;
      Vector3 vector3 = Vector3.op_Multiply(Vector3.get_one(), 0.5f);
      if (Object.op_Implicit((Object) this.sunTransform))
        vector3 = this.cam.WorldToViewportPoint(this.sunTransform.get_position());
      else
        ((Vector3) ref vector3).\u002Ector(0.5f, 0.5f, 0.0f);
      int num2 = ((Texture) source).get_width() / num1;
      int num3 = ((Texture) source).get_height() / num1;
      RenderTexture temporary1 = RenderTexture.GetTemporary(num2, num3, 0, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, 1, (RenderTextureMemoryless) 0, source.get_vrUsage());
      this.sunShaftsMaterial.SetVector("_BlurRadius4", Vector4.op_Multiply(new Vector4(1f, 1f, 0.0f, 0.0f), this.sunShaftBlurRadius));
      this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4((float) vector3.x, (float) vector3.y, (float) vector3.z, this.maxRadius));
      this.sunShaftsMaterial.SetVector("_SunThreshold", Color.op_Implicit(this.sunThreshold));
      if (!this.useDepthTexture)
      {
        RenderTextureFormat renderTextureFormat = !EnviroSky.instance.GetCameraHDR(this.cam) ? (RenderTextureFormat) 7 : (RenderTextureFormat) 9;
        RenderTexture temporary2 = RenderTexture.GetTemporary(((Texture) source).get_width(), ((Texture) source).get_height(), 0, renderTextureFormat);
        RenderTexture.set_active(temporary2);
        GL.ClearWithSkybox(false, this.cam);
        this.sunShaftsMaterial.SetTexture("_Skybox", (Texture) temporary2);
        Graphics.Blit((Texture) source, temporary1, this.sunShaftsMaterial, 3);
        RenderTexture.ReleaseTemporary(temporary2);
      }
      else
        Graphics.Blit((Texture) source, temporary1, this.sunShaftsMaterial, 2);
      if (this.cam.get_stereoActiveEye() == 2)
        this.DrawBorder(temporary1, this.simpleClearMaterial);
      this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
      float num4 = this.sunShaftBlurRadius * (1f / 768f);
      this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num4, num4, 0.0f, 0.0f));
      this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4((float) vector3.x, (float) vector3.y, (float) vector3.z, this.maxRadius));
      for (int index = 0; index < this.radialBlurIterations; ++index)
      {
        RenderTexture temporary2 = RenderTexture.GetTemporary(num2, num3, 0, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, 1, (RenderTextureMemoryless) 0, source.get_vrUsage());
        Graphics.Blit((Texture) temporary1, temporary2, this.sunShaftsMaterial, 1);
        RenderTexture.ReleaseTemporary(temporary1);
        float num5 = (float) ((double) this.sunShaftBlurRadius * (((double) index * 2.0 + 1.0) * 6.0) / 768.0);
        this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num5, num5, 0.0f, 0.0f));
        temporary1 = RenderTexture.GetTemporary(num2, num3, 0, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, 1, (RenderTextureMemoryless) 0, source.get_vrUsage());
        Graphics.Blit((Texture) temporary2, temporary1, this.sunShaftsMaterial, 1);
        RenderTexture.ReleaseTemporary(temporary2);
        float num6 = (float) ((double) this.sunShaftBlurRadius * (((double) index * 2.0 + 2.0) * 6.0) / 768.0);
        this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num6, num6, 0.0f, 0.0f));
      }
      if (vector3.z >= 0.0)
        this.sunShaftsMaterial.SetVector("_SunColor", Vector4.op_Multiply(new Vector4((float) this.sunColor.r, (float) this.sunColor.g, (float) this.sunColor.b, (float) this.sunColor.a), this.sunShaftIntensity));
      else
        this.sunShaftsMaterial.SetVector("_SunColor", Vector4.get_zero());
      this.sunShaftsMaterial.SetTexture("_ColorBuffer", (Texture) temporary1);
      Graphics.Blit((Texture) source, destination, this.sunShaftsMaterial, this.screenBlendMode != EnviroLightShafts.ShaftsScreenBlendMode.Screen ? 4 : 0);
      RenderTexture.ReleaseTemporary(temporary1);
    }
  }

  public enum SunShaftsResolution
  {
    Low,
    Normal,
    High,
  }

  public enum ShaftsScreenBlendMode
  {
    Screen,
    Add,
  }
}
