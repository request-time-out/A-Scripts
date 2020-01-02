// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.DS_HazeLightVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace DeepSky.Haze
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (Light))]
  [AddComponentMenu("DeepSky Haze/Light Volume", 2)]
  public class DS_HazeLightVolume : MonoBehaviour
  {
    private static int kConeSubdivisions = 16;
    private static Shader kLightVolumeShader;
    private Light m_Light;
    private Mesh m_ProxyMesh;
    private Matrix4x4 m_LightVolumeTransform;
    private CommandBuffer m_RenderCmd;
    private Material m_VolumeMaterial;
    private Vector3 m_DensityOffset;
    [SerializeField]
    private DS_SamplingQuality m_Samples;
    [SerializeField]
    private DS_LightFalloff m_Falloff;
    [SerializeField]
    private bool m_UseFog;
    [SerializeField]
    [Range(0.0f, 100f)]
    private float m_Scattering;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float m_SecondaryScattering;
    [SerializeField]
    [Range(-1f, 1f)]
    private float m_ScatteringDirection;
    [SerializeField]
    private Texture3D m_DensityTexture;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float m_DensityTextureScale;
    [SerializeField]
    [Range(0.1f, 3f)]
    private float m_DensityTextureContrast;
    [SerializeField]
    private Vector3 m_AnimateDirection;
    [SerializeField]
    [Range(0.0f, 10f)]
    private float m_AnimateSpeed;
    [SerializeField]
    private float m_StartFade;
    [SerializeField]
    private float m_EndFade;
    [SerializeField]
    [Range(0.01f, 1f)]
    private float m_FarClip;
    private LightType m_PreviousLightType;
    private float m_PreviousAngle;
    private LightShadows m_PreviousShadowMode;

    public DS_HazeLightVolume()
    {
      base.\u002Ector();
    }

    public Light LightSource
    {
      get
      {
        return this.m_Light;
      }
    }

    public LightType Type
    {
      get
      {
        return Object.op_Inequality((Object) this.m_Light, (Object) null) ? this.m_Light.get_type() : (LightType) 2;
      }
    }

    public bool CastShadows
    {
      get
      {
        return this.m_Light.get_shadows() != null;
      }
    }

    public CommandBuffer RenderCommandBuffer
    {
      get
      {
        return this.m_RenderCmd;
      }
    }

    public DS_SamplingQuality Samples
    {
      get
      {
        return this.m_Samples;
      }
      set
      {
        this.m_Samples = value;
      }
    }

    public DS_LightFalloff Falloff
    {
      get
      {
        return this.m_Falloff;
      }
      set
      {
        this.m_Falloff = value;
      }
    }

    public bool UseFog
    {
      get
      {
        return this.m_UseFog;
      }
      set
      {
        this.m_UseFog = value;
      }
    }

    public float Scattering
    {
      get
      {
        return this.m_Scattering;
      }
      set
      {
        this.m_Scattering = Mathf.Clamp01(value);
      }
    }

    public float ScatteringDirection
    {
      get
      {
        return this.m_ScatteringDirection;
      }
      set
      {
        this.m_ScatteringDirection = Mathf.Clamp(value, -1f, 1f);
      }
    }

    public Texture3D DensityTexture
    {
      get
      {
        return this.m_DensityTexture;
      }
      set
      {
        this.m_DensityTexture = value;
      }
    }

    public float DensityTextureScale
    {
      get
      {
        return this.m_DensityTextureScale;
      }
      set
      {
        this.m_DensityTextureScale = Mathf.Clamp01(this.m_DensityTextureScale);
      }
    }

    public Vector3 AnimateDirection
    {
      get
      {
        return this.m_AnimateDirection;
      }
      set
      {
        this.m_AnimateDirection = ((Vector3) ref value).get_normalized();
      }
    }

    public float AnimateSpeed
    {
      get
      {
        return this.m_AnimateSpeed;
      }
      set
      {
        this.m_AnimateSpeed = Mathf.Clamp01(value);
      }
    }

    public float StartFade
    {
      get
      {
        return this.m_StartFade;
      }
      set
      {
        this.m_StartFade = (double) value <= 0.0 ? 1f : value;
      }
    }

    public float EndFade
    {
      get
      {
        return this.m_EndFade;
      }
      set
      {
        this.m_EndFade = (double) value <= (double) this.m_StartFade ? this.m_StartFade + 1f : value;
      }
    }

    private void CreateProxyMeshCone(Mesh proxyMesh)
    {
      float num1 = Mathf.Tan((float) ((double) this.m_Light.get_spotAngle() / 2.0 * (Math.PI / 180.0))) * this.m_FarClip;
      Vector3[] vector3Array = new Vector3[DS_HazeLightVolume.kConeSubdivisions + 2];
      int[] numArray = new int[DS_HazeLightVolume.kConeSubdivisions * 6];
      float num2 = 6.283185f / (float) DS_HazeLightVolume.kConeSubdivisions;
      float num3 = 0.0f;
      for (int index = 0; index < DS_HazeLightVolume.kConeSubdivisions; ++index)
      {
        vector3Array[index] = new Vector3(Mathf.Sin(num3) * num1, Mathf.Cos(num3) * num1, this.m_FarClip);
        num3 += num2;
      }
      vector3Array[DS_HazeLightVolume.kConeSubdivisions] = new Vector3(0.0f, 0.0f, this.m_FarClip);
      vector3Array[DS_HazeLightVolume.kConeSubdivisions + 1] = new Vector3(0.0f, 0.0f, -0.1f);
      for (int index = 0; index < DS_HazeLightVolume.kConeSubdivisions; ++index)
      {
        numArray[index * 3] = DS_HazeLightVolume.kConeSubdivisions;
        numArray[index * 3 + 1] = index != DS_HazeLightVolume.kConeSubdivisions - 1 ? index + 1 : 0;
        numArray[index * 3 + 2] = index;
        numArray[DS_HazeLightVolume.kConeSubdivisions * 3 + index * 3] = index;
        numArray[DS_HazeLightVolume.kConeSubdivisions * 3 + index * 3 + 1] = index != DS_HazeLightVolume.kConeSubdivisions - 1 ? index + 1 : 0;
        numArray[DS_HazeLightVolume.kConeSubdivisions * 3 + index * 3 + 2] = DS_HazeLightVolume.kConeSubdivisions + 1;
      }
      proxyMesh.set_vertices(vector3Array);
      proxyMesh.set_triangles(numArray);
      ((Object) proxyMesh).set_hideFlags((HideFlags) 61);
      this.m_PreviousAngle = this.m_Light.get_spotAngle();
    }

    public bool ProxyMeshRequiresRebuild()
    {
      return !Object.op_Equality((Object) this.m_Light, (Object) null) && (Object.op_Equality((Object) this.m_ProxyMesh, (Object) null) || this.m_Light.get_type() == null && (double) this.m_Light.get_spotAngle() != (double) this.m_PreviousAngle);
    }

    public bool LightTypeChanged()
    {
      return !Object.op_Equality((Object) this.m_Light, (Object) null) && this.m_Light.get_type() != this.m_PreviousLightType;
    }

    public void UpdateLightType()
    {
      this.m_VolumeMaterial.DisableKeyword("POINT_COOKIE");
      this.m_VolumeMaterial.DisableKeyword("SPOT_COOKIE");
      if (this.m_Light.get_type() == 2)
      {
        this.m_VolumeMaterial.EnableKeyword("POINT");
        this.m_VolumeMaterial.DisableKeyword("SPOT");
      }
      else if (this.m_Light.get_type() == null)
      {
        this.m_VolumeMaterial.EnableKeyword("SPOT");
        this.m_VolumeMaterial.DisableKeyword("POINT");
      }
      else
      {
        Debug.LogError((object) ("DeepSky::DS_HazeLightVolume: Unsupported light type! " + ((Object) ((Component) this).get_gameObject()).get_name() + " will not render volumetrics."));
        ((Behaviour) this).set_enabled(false);
        return;
      }
      this.RebuildProxyMesh();
      this.m_PreviousLightType = this.m_Light.get_type();
    }

    public void RebuildProxyMesh()
    {
      LightType type = this.m_Light.get_type();
      if (type != 2)
      {
        if (type == null)
        {
          if (this.m_PreviousLightType == 2)
            this.m_ProxyMesh = new Mesh();
          else if (Object.op_Inequality((Object) this.m_ProxyMesh, (Object) null))
            this.m_ProxyMesh.Clear();
          this.CreateProxyMeshCone(this.m_ProxyMesh);
        }
        else
        {
          Debug.LogError((object) ("DeepSky::DS_HazeLightVolume: Unsupported light type! " + ((Object) ((Component) this).get_gameObject()).get_name() + " will not render volumetrics."));
          ((Behaviour) this).set_enabled(false);
        }
      }
      else
      {
        if (this.m_PreviousLightType != 2)
          Object.DestroyImmediate((Object) this.m_ProxyMesh);
        this.m_ProxyMesh = (Mesh) Resources.Load<Mesh>("DS_HazeMeshProxySphere");
      }
    }

    public bool ShadowModeChanged()
    {
      return !Object.op_Equality((Object) this.m_Light, (Object) null) && this.m_Light.get_shadows() != this.m_PreviousShadowMode;
    }

    public void UpdateShadowMode()
    {
      if (this.m_Light.get_shadows() == null)
      {
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_DEPTH");
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_CUBE");
      }
      else if (this.m_Light.get_type() == 2)
      {
        this.m_VolumeMaterial.EnableKeyword("SHADOWS_CUBE");
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_DEPTH");
      }
      else if (this.m_Light.get_type() == null)
      {
        this.m_VolumeMaterial.EnableKeyword("SHADOWS_DEPTH");
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_CUBE");
      }
      this.m_PreviousShadowMode = this.m_Light.get_shadows();
    }

    public void Register()
    {
      DS_HazeCore instance = DS_HazeCore.Instance;
      if (Object.op_Equality((Object) instance, (Object) null))
        Debug.LogError((object) "DeepSky::DS_HazeLightVolume: Attempting to add a light volume but no HS_HazeCore found in scene! Please make sure there is a DS_HazeCore object.");
      else
        instance.AddLightVolume(this);
    }

    public void Deregister()
    {
      DS_HazeCore instance = DS_HazeCore.Instance;
      if (!Object.op_Inequality((Object) instance, (Object) null))
        return;
      instance.RemoveLightVolume(this);
    }

    public bool WillRender(Vector3 cameraPos)
    {
      return ((Behaviour) this).get_isActiveAndEnabled() & (double) Vector3.Distance(cameraPos, ((Component) this).get_transform().get_position()) < (double) this.m_EndFade;
    }

    private void Update()
    {
      DS_HazeLightVolume dsHazeLightVolume = this;
      dsHazeLightVolume.m_DensityOffset = Vector3.op_Subtraction(dsHazeLightVolume.m_DensityOffset, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.m_AnimateDirection, this.m_AnimateSpeed), Time.get_deltaTime()), 0.1f));
    }

    private void OnEnable()
    {
      this.m_Light = (Light) ((Component) this).GetComponent<Light>();
      if (Object.op_Equality((Object) this.m_Light, (Object) null))
      {
        Debug.LogError((object) ("DeepSky::DS_HazeLightVolume: No Light component found on " + ((Object) ((Component) this).get_gameObject()).get_name()));
        ((Behaviour) this).set_enabled(false);
      }
      if (Object.op_Equality((Object) DS_HazeLightVolume.kLightVolumeShader, (Object) null))
        DS_HazeLightVolume.kLightVolumeShader = (Shader) Resources.Load<Shader>(nameof (DS_HazeLightVolume));
      if (Object.op_Equality((Object) this.m_VolumeMaterial, (Object) null))
      {
        this.m_VolumeMaterial = new Material(DS_HazeLightVolume.kLightVolumeShader);
        ((Object) this.m_VolumeMaterial).set_hideFlags((HideFlags) 61);
      }
      if (this.m_RenderCmd == null)
      {
        this.m_RenderCmd = new CommandBuffer();
        this.m_RenderCmd.set_name(((Object) ((Component) this).get_gameObject()).get_name() + "_DS_Haze_RenderLightVolume");
        this.m_Light.AddCommandBuffer((LightEvent) 1, this.m_RenderCmd);
      }
      if (this.LightTypeChanged())
        this.UpdateLightType();
      else if (this.ProxyMeshRequiresRebuild())
        this.RebuildProxyMesh();
      if (this.ShadowModeChanged())
        this.UpdateShadowMode();
      this.Register();
    }

    private void OnDisable()
    {
      this.Deregister();
    }

    private void OnDestroy()
    {
      if (this.m_RenderCmd != null)
        this.m_RenderCmd.Dispose();
      this.Deregister();
      if (Object.op_Inequality((Object) this.m_ProxyMesh, (Object) null) && this.m_Light.get_type() != 2)
        Object.DestroyImmediate((Object) this.m_ProxyMesh);
      if (!Object.op_Inequality((Object) this.m_VolumeMaterial, (Object) null))
        return;
      Object.DestroyImmediate((Object) this.m_VolumeMaterial);
    }

    private int SetShaderPassAndMatrix(
      Transform cameraTransform,
      int downSampleFactor,
      out Matrix4x4 worldMtx)
    {
      worldMtx = Matrix4x4.TRS(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation(), new Vector3(this.m_Light.get_range(), this.m_Light.get_range(), this.m_Light.get_range()));
      int num1 = 0;
      if (this.m_Light.get_type() == null)
      {
        float num2 = Mathf.Cos((float) ((double) this.m_Light.get_spotAngle() / 2.0 * (Math.PI / 180.0)));
        Vector3 vector3 = Vector3.op_Subtraction(cameraTransform.get_position(), ((Component) this).get_transform().get_position());
        num1 = (double) Vector3.Dot(((Vector3) ref vector3).get_normalized(), ((Component) this).get_transform().get_forward()) <= (double) num2 ? 2 : 1;
      }
      if (downSampleFactor == 4)
        num1 += 3;
      if (this.m_Falloff == DS_LightFalloff.Quadratic)
        num1 += 6;
      if (this.m_UseFog)
        num1 += 12;
      return num1;
    }

    public void FillLightCommandBuffer(
      RenderTexture radianceTarget,
      Transform cameraTransform,
      int downSampleFactor)
    {
      this.m_RenderCmd.SetGlobalTexture("_ShadowMapTexture", RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType) 1));
      Matrix4x4 worldMtx;
      int num = this.SetShaderPassAndMatrix(cameraTransform, downSampleFactor, out worldMtx);
      this.m_RenderCmd.SetRenderTarget(RenderTargetIdentifier.op_Implicit((Texture) radianceTarget));
      this.m_RenderCmd.DrawMesh(this.m_ProxyMesh, worldMtx, this.m_VolumeMaterial, 0, num);
    }

    public void AddLightRenderCommand(
      Transform cameraTransform,
      CommandBuffer cmd,
      int downSampleFactor)
    {
      Matrix4x4 worldMtx;
      int num = this.SetShaderPassAndMatrix(cameraTransform, downSampleFactor, out worldMtx);
      cmd.DrawMesh(this.m_ProxyMesh, worldMtx, this.m_VolumeMaterial, 0, num);
    }

    public void SetupMaterialPerFrame(
      Matrix4x4 viewProjMtx,
      Matrix4x4 viewMtx,
      Transform cameraTransform,
      float offsetIndex)
    {
      this.m_VolumeMaterial.DisableKeyword("SAMPLES_4");
      this.m_VolumeMaterial.DisableKeyword("SAMPLES_8");
      this.m_VolumeMaterial.DisableKeyword("SAMPLES_16");
      this.m_VolumeMaterial.DisableKeyword("SAMPLES_32");
      switch (this.m_Samples)
      {
        case DS_SamplingQuality.x4:
          this.m_VolumeMaterial.EnableKeyword("SAMPLES_4");
          break;
        case DS_SamplingQuality.x8:
          this.m_VolumeMaterial.EnableKeyword("SAMPLES_8");
          break;
        case DS_SamplingQuality.x16:
          this.m_VolumeMaterial.EnableKeyword("SAMPLES_16");
          break;
        case DS_SamplingQuality.x32:
          this.m_VolumeMaterial.EnableKeyword("SAMPLES_32");
          break;
        default:
          this.m_VolumeMaterial.EnableKeyword("SAMPLES_16");
          break;
      }
      float num1 = 1f - Mathf.Clamp01((float) (((double) Vector3.Distance(cameraTransform.get_position(), ((Component) this).get_transform().get_position()) - (double) this.m_StartFade) / ((double) this.m_EndFade - (double) this.m_StartFade)));
      this.m_VolumeMaterial.SetVector("_DS_HazeSamplingParams", new Vector4(offsetIndex, 0.0f, this.m_DensityTextureContrast, 0.0f));
      this.m_VolumeMaterial.SetVector("_DS_HazeCameraDirection", new Vector4((float) cameraTransform.get_forward().x, (float) cameraTransform.get_forward().y, (float) cameraTransform.get_forward().z, 1f));
      Material volumeMaterial = this.m_VolumeMaterial;
      Color color1 = this.m_Light.get_color();
      Color color2 = Color.op_Multiply(Color.op_Multiply(((Color) ref color1).get_linear(), this.m_Light.get_intensity()), num1);
      volumeMaterial.SetColor("_DS_HazeLightVolumeColour", color2);
      this.m_VolumeMaterial.SetVector("_DS_HazeLightVolumeScattering", new Vector4(this.m_Scattering, this.m_SecondaryScattering, this.m_ScatteringDirection, Mathf.Clamp01(1f - this.m_SecondaryScattering)));
      this.m_VolumeMaterial.SetVector("_DS_HazeLightVolumeParams0", new Vector4((float) ((Component) this).get_transform().get_position().x, (float) ((Component) this).get_transform().get_position().y, (float) ((Component) this).get_transform().get_position().z, this.m_Light.get_range()));
      Matrix4x4 matrix4x4_1 = Matrix4x4.TRS(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation(), new Vector3(this.m_Light.get_range(), this.m_Light.get_range(), this.m_Light.get_range()));
      this.m_VolumeMaterial.SetMatrix("_WorldViewProj", Matrix4x4.op_Multiply(viewProjMtx, matrix4x4_1));
      this.m_VolumeMaterial.SetMatrix("_WorldView", Matrix4x4.op_Multiply(viewMtx, matrix4x4_1));
      if (Object.op_Implicit((Object) this.m_DensityTexture))
      {
        this.m_VolumeMaterial.EnableKeyword("DENSITY_TEXTURE");
        this.m_VolumeMaterial.SetTexture("_DensityTexture", (Texture) this.m_DensityTexture);
        this.m_VolumeMaterial.SetVector("_DS_HazeDensityParams", new Vector4((float) this.m_DensityOffset.x, (float) this.m_DensityOffset.y, (float) this.m_DensityOffset.z, this.m_DensityTextureScale * 0.01f));
      }
      else
        this.m_VolumeMaterial.DisableKeyword("DENSITY_TEXTURE");
      bool flag = this.m_Light.get_shadows() != 0;
      if (this.m_Light.get_type() == 2)
      {
        this.m_VolumeMaterial.DisableKeyword("SPOT_COOKIE");
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_DEPTH");
        if (flag)
          this.m_VolumeMaterial.EnableKeyword("SHADOWS_CUBE");
        else
          this.m_VolumeMaterial.DisableKeyword("SHADOWS_CUBE");
        if (Object.op_Implicit((Object) this.m_Light.get_cookie()))
        {
          this.m_VolumeMaterial.EnableKeyword("POINT_COOKIE");
          this.m_VolumeMaterial.SetMatrix("_DS_Haze_WorldToCookie", ((Component) this).get_transform().get_worldToLocalMatrix());
          this.m_VolumeMaterial.SetTexture("_LightTexture0", this.m_Light.get_cookie());
        }
        else
          this.m_VolumeMaterial.DisableKeyword("POINT_COOKIE");
      }
      else
      {
        if (this.m_Light.get_type() != null)
          return;
        this.m_VolumeMaterial.DisableKeyword("POINT_COOKIE");
        this.m_VolumeMaterial.DisableKeyword("SHADOWS_CUBE");
        if (flag)
        {
          this.m_VolumeMaterial.EnableKeyword("SHADOWS_DEPTH");
          Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation(), Vector3.get_one());
          Matrix4x4 inverse = ((Matrix4x4) ref matrix4x4_2).get_inverse();
          Matrix4x4 matrix4x4_3 = Matrix4x4.op_Multiply(Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.get_identity(), new Vector3(0.5f, 0.5f, 0.5f)), Matrix4x4.Perspective(this.m_Light.get_spotAngle(), 1f, this.m_Light.get_range(), this.m_Light.get_shadowNearPlane()));
          // ISSUE: variable of a reference type
          Matrix4x4& local1;
          ((Matrix4x4) (local1 = ref matrix4x4_3)).set_Item(0, 2, ((Matrix4x4) ref local1).get_Item(0, 2) * -1f);
          // ISSUE: variable of a reference type
          Matrix4x4& local2;
          ((Matrix4x4) (local2 = ref matrix4x4_3)).set_Item(1, 2, ((Matrix4x4) ref local2).get_Item(1, 2) * -1f);
          // ISSUE: variable of a reference type
          Matrix4x4& local3;
          ((Matrix4x4) (local3 = ref matrix4x4_3)).set_Item(2, 2, ((Matrix4x4) ref local3).get_Item(2, 2) * -1f);
          // ISSUE: variable of a reference type
          Matrix4x4& local4;
          ((Matrix4x4) (local4 = ref matrix4x4_3)).set_Item(3, 2, ((Matrix4x4) ref local4).get_Item(3, 2) * -1f);
          this.m_VolumeMaterial.SetMatrix("_DS_Haze_WorldToShadow", Matrix4x4.op_Multiply(matrix4x4_3, inverse));
        }
        else
          this.m_VolumeMaterial.DisableKeyword("SHADOWS_DEPTH");
        float num2 = Mathf.Cos((float) ((double) this.m_Light.get_spotAngle() / 2.0 * (Math.PI / 180.0)));
        float num3 = -Vector3.Dot(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.m_Light.get_range())), ((Component) this).get_transform().get_forward());
        this.m_VolumeMaterial.SetVector("_DS_HazeLightVolumeParams1", new Vector4((float) ((Component) this).get_transform().get_forward().x, (float) ((Component) this).get_transform().get_forward().y, (float) ((Component) this).get_transform().get_forward().z, 1f));
        this.m_VolumeMaterial.SetVector("_DS_HazeLightVolumeParams2", new Vector4(num2, 1f / num2, num3, 0.0f));
        if (Object.op_Implicit((Object) this.m_Light.get_cookie()))
        {
          this.m_VolumeMaterial.EnableKeyword("SPOT_COOKIE");
          Matrix4x4 matrix4x4_2 = Matrix4x4.TRS(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation(), Vector3.get_one());
          this.m_VolumeMaterial.SetMatrix("_DS_Haze_WorldToCookie", Matrix4x4.op_Multiply(Matrix4x4.op_Multiply(Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.0f), Quaternion.get_identity(), new Vector3(-0.5f, -0.5f, 1f)), Matrix4x4.Perspective(this.m_Light.get_spotAngle(), 1f, 0.0f, 1f)), ((Matrix4x4) ref matrix4x4_2).get_inverse()));
          this.m_VolumeMaterial.SetTexture("_LightTexture0", this.m_Light.get_cookie());
        }
        else
          this.m_VolumeMaterial.DisableKeyword("SPOT_COOKIE");
      }
    }
  }
}
