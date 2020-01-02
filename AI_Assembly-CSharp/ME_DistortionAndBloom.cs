// Decompiled with JetBrains decompiler
// Type: ME_DistortionAndBloom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("KriptoFX/ME_BloomAndDistortion")]
public class ME_DistortionAndBloom : MonoBehaviour
{
  public LayerMask CullingMask;
  [Range(0.05f, 1f)]
  [Tooltip("Camera render texture resolution")]
  public float RenderTextureResolutoinFactor;
  public bool UseBloom;
  [Range(0.1f, 3f)]
  [Tooltip("Filters out pixels under this level of brightness.")]
  public float Threshold;
  [SerializeField]
  [Range(0.0f, 1f)]
  [Tooltip("Makes transition between under/over-threshold gradual.")]
  public float SoftKnee;
  [Range(1f, 7f)]
  [Tooltip("Changes extent of veiling effects in A screen resolution-independent fashion.")]
  public float Radius;
  [Tooltip("Blend factor of the result image.")]
  public float Intensity;
  [Tooltip("Controls filter quality and buffer resolution.")]
  public bool HighQuality;
  [Tooltip("Reduces flashing noise with an additional filter.")]
  public bool AntiFlicker;
  private const string shaderName = "Hidden/KriptoFX/PostEffects/ME_Bloom";
  private const string shaderAdditiveName = "Hidden/KriptoFX/PostEffects/ME_BloomAdditive";
  private const string cameraName = "MobileCamera(Distort_Bloom_Depth)";
  private RenderTexture source;
  private RenderTexture depth;
  private RenderTexture destination;
  private int previuosFrameWidth;
  private int previuosFrameHeight;
  private float previousScale;
  private Camera addCamera;
  private GameObject tempGO;
  private bool HDRSupported;
  private Material m_Material;
  private Material m_MaterialAdditive;
  private const int kMaxIterations = 16;
  private readonly RenderTexture[] m_blurBuffer1;
  private readonly RenderTexture[] m_blurBuffer2;

  public ME_DistortionAndBloom()
  {
    base.\u002Ector();
  }

  public Material mat
  {
    get
    {
      if (Object.op_Equality((Object) this.m_Material, (Object) null))
        this.m_Material = ME_DistortionAndBloom.CheckShaderAndCreateMaterial(Shader.Find("Hidden/KriptoFX/PostEffects/ME_Bloom"));
      return this.m_Material;
    }
  }

  public Material matAdditive
  {
    get
    {
      if (Object.op_Equality((Object) this.m_MaterialAdditive, (Object) null))
      {
        this.m_MaterialAdditive = ME_DistortionAndBloom.CheckShaderAndCreateMaterial(Shader.Find("Hidden/KriptoFX/PostEffects/ME_BloomAdditive"));
        this.m_MaterialAdditive.set_renderQueue(3900);
      }
      return this.m_MaterialAdditive;
    }
  }

  public static Material CheckShaderAndCreateMaterial(Shader s)
  {
    if (Object.op_Equality((Object) s, (Object) null) || !s.get_isSupported())
      return (Material) null;
    Material material = new Material(s);
    ((Object) material).set_hideFlags((HideFlags) 52);
    return material;
  }

  private void OnDisable()
  {
    if (Object.op_Inequality((Object) this.m_Material, (Object) null))
      Object.DestroyImmediate((Object) this.m_Material);
    this.m_Material = (Material) null;
    if (Object.op_Inequality((Object) this.m_MaterialAdditive, (Object) null))
      Object.DestroyImmediate((Object) this.m_MaterialAdditive);
    this.m_MaterialAdditive = (Material) null;
    if (Object.op_Inequality((Object) this.tempGO, (Object) null))
      Object.DestroyImmediate((Object) this.tempGO);
    Shader.DisableKeyword("DISTORT_OFF");
    Shader.DisableKeyword("_MOBILEDEPTH_ON");
  }

  private void Start()
  {
    this.InitializeRenderTarget();
  }

  private void LateUpdate()
  {
    if (this.previuosFrameWidth != Screen.get_width() || this.previuosFrameHeight != Screen.get_height() || (double) Mathf.Abs(this.previousScale - this.RenderTextureResolutoinFactor) > 0.00999999977648258)
    {
      this.InitializeRenderTarget();
      this.previuosFrameWidth = Screen.get_width();
      this.previuosFrameHeight = Screen.get_height();
      this.previousScale = this.RenderTextureResolutoinFactor;
    }
    Shader.EnableKeyword("DISTORT_OFF");
    Shader.EnableKeyword("_MOBILEDEPTH_ON");
    this.GrabImage();
    if (this.UseBloom && this.HDRSupported)
      this.UpdateBloom();
    Shader.SetGlobalTexture("_GrabTexture", (Texture) this.source);
    Shader.SetGlobalTexture("_GrabTextureMobile", (Texture) this.source);
    Shader.SetGlobalTexture("_CameraDepthTexture", (Texture) this.depth);
    Shader.SetGlobalFloat("_GrabTextureScale", this.RenderTextureResolutoinFactor);
    Shader.SetGlobalFloat("_GrabTextureMobileScale", this.RenderTextureResolutoinFactor);
    Shader.DisableKeyword("DISTORT_OFF");
  }

  private void OnPostRender()
  {
    Graphics.Blit((Texture) this.destination, (RenderTexture) null, this.matAdditive);
  }

  private void InitializeRenderTarget()
  {
    int num1 = (int) ((double) Screen.get_width() * (double) this.RenderTextureResolutoinFactor);
    int num2 = (int) ((double) Screen.get_height() * (double) this.RenderTextureResolutoinFactor);
    RenderTextureFormat renderTextureFormat = (RenderTextureFormat) 22;
    if (SystemInfo.SupportsRenderTextureFormat(renderTextureFormat))
    {
      this.source = new RenderTexture(num1, num2, 0, renderTextureFormat);
      this.depth = new RenderTexture(num1, num2, 8, (RenderTextureFormat) 1);
      this.HDRSupported = true;
      if (!this.UseBloom)
        return;
      this.destination = new RenderTexture((double) this.RenderTextureResolutoinFactor <= 0.99 ? num1 / 2 : num1, (double) this.RenderTextureResolutoinFactor <= 0.99 ? num2 / 2 : num2, 0, renderTextureFormat);
    }
    else
    {
      this.HDRSupported = false;
      this.source = new RenderTexture(num1, num2, 0, (RenderTextureFormat) 4);
      this.depth = new RenderTexture(num1, num2, 8, (RenderTextureFormat) 1);
    }
  }

  private void UpdateBloom()
  {
    bool isMobilePlatform = Application.get_isMobilePlatform();
    if (Object.op_Equality((Object) this.source, (Object) null))
      return;
    int width = ((Texture) this.source).get_width();
    int height = ((Texture) this.source).get_height();
    if (!this.HighQuality)
    {
      width /= 2;
      height /= 2;
    }
    RenderTextureFormat renderTextureFormat = !isMobilePlatform ? (RenderTextureFormat) 9 : (RenderTextureFormat) 7;
    float num1 = (float) ((double) Mathf.Log((float) height, 2f) + (double) this.Radius - 8.0);
    int num2 = (int) num1;
    int num3 = Mathf.Clamp(num2, 1, 16);
    float linearSpace = Mathf.GammaToLinearSpace(this.Threshold);
    this.mat.SetFloat("_Threshold", linearSpace);
    float num4 = (float) ((double) linearSpace * (double) this.SoftKnee + 9.99999974737875E-06);
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector(linearSpace - num4, num4 * 2f, 0.25f / num4);
    this.mat.SetVector("_Curve", Vector4.op_Implicit(vector3));
    this.mat.SetFloat("_PrefilterOffs", this.HighQuality || !this.AntiFlicker ? 0.0f : -0.5f);
    this.mat.SetFloat("_SampleScale", 0.5f + num1 - (float) num2);
    this.mat.SetFloat("_Intensity", Mathf.Max(0.0f, this.Intensity));
    RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, renderTextureFormat);
    Graphics.Blit((Texture) this.source, temporary, this.mat, !this.AntiFlicker ? 0 : 1);
    RenderTexture renderTexture1 = temporary;
    for (int index = 0; index < num3; ++index)
    {
      this.m_blurBuffer1[index] = RenderTexture.GetTemporary(((Texture) renderTexture1).get_width() / 2, ((Texture) renderTexture1).get_height() / 2, 0, renderTextureFormat);
      Graphics.Blit((Texture) renderTexture1, this.m_blurBuffer1[index], this.mat, index != 0 ? 4 : (!this.AntiFlicker ? 2 : 3));
      renderTexture1 = this.m_blurBuffer1[index];
    }
    for (int index = num3 - 2; index >= 0; --index)
    {
      RenderTexture renderTexture2 = this.m_blurBuffer1[index];
      this.mat.SetTexture("_BaseTex", (Texture) renderTexture2);
      this.m_blurBuffer2[index] = RenderTexture.GetTemporary(((Texture) renderTexture2).get_width(), ((Texture) renderTexture2).get_height(), 0, renderTextureFormat);
      Graphics.Blit((Texture) renderTexture1, this.m_blurBuffer2[index], this.mat, !this.HighQuality ? 5 : 6);
      renderTexture1 = this.m_blurBuffer2[index];
    }
    this.destination.DiscardContents();
    Graphics.Blit((Texture) renderTexture1, this.destination, this.mat, !this.HighQuality ? 7 : 8);
    for (int index = 0; index < 16; ++index)
    {
      if (Object.op_Inequality((Object) this.m_blurBuffer1[index], (Object) null))
        RenderTexture.ReleaseTemporary(this.m_blurBuffer1[index]);
      if (Object.op_Inequality((Object) this.m_blurBuffer2[index], (Object) null))
        RenderTexture.ReleaseTemporary(this.m_blurBuffer2[index]);
      this.m_blurBuffer1[index] = (RenderTexture) null;
      this.m_blurBuffer2[index] = (RenderTexture) null;
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  private void GrabImage()
  {
    Camera camera = Camera.get_current();
    if (Object.op_Equality((Object) camera, (Object) null))
      camera = Camera.get_main();
    if (Object.op_Equality((Object) this.tempGO, (Object) null))
    {
      this.tempGO = new GameObject();
      ((Object) this.tempGO).set_hideFlags((HideFlags) 61);
      ((Object) this.tempGO).set_name("MobileCamera(Distort_Bloom_Depth)");
      this.addCamera = (Camera) this.tempGO.AddComponent<Camera>();
      ((Behaviour) this.addCamera).set_enabled(false);
      this.addCamera.set_cullingMask(~(1 << LayerMask.NameToLayer("CustomPostEffectIgnore")) & LayerMask.op_Implicit(this.CullingMask));
    }
    else
      this.addCamera = (Camera) this.tempGO.GetComponent<Camera>();
    this.addCamera.CopyFrom(camera);
    this.addCamera.SetTargetBuffers(this.source.get_colorBuffer(), this.depth.get_depthBuffer());
    Camera addCamera = this.addCamera;
    addCamera.set_depth(addCamera.get_depth() - 1f);
    this.addCamera.set_cullingMask(~(1 << LayerMask.NameToLayer("CustomPostEffectIgnore")) & LayerMask.op_Implicit(this.CullingMask));
    this.addCamera.Render();
  }
}
