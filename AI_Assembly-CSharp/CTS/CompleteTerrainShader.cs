// Decompiled with JetBrains decompiler
// Type: CTS.CompleteTerrainShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace CTS
{
  [RequireComponent(typeof (Terrain))]
  [AddComponentMenu("CTS/Add CTS To Terrain")]
  [ExecuteInEditMode]
  [Serializable]
  public class CompleteTerrainShader : MonoBehaviour
  {
    [SerializeField]
    private CTSProfile m_profile;
    [SerializeField]
    private Texture2D m_normalMap;
    [SerializeField]
    private bool m_bakeNormalMap;
    [SerializeField]
    private Texture2D m_colorMap;
    [SerializeField]
    private bool m_bakeColorMap;
    [SerializeField]
    private bool m_bakeGrassTextures;
    [SerializeField]
    private float m_bakeGrassMixStrength;
    [SerializeField]
    private float m_bakeGrassDarkenAmount;
    [SerializeField]
    private bool m_useCutout;
    [SerializeField]
    private Texture2D m_cutoutMask;
    [SerializeField]
    private float m_cutoutHeight;
    [SerializeField]
    private Texture2D m_splat1;
    [SerializeField]
    private Texture2D m_splat2;
    [SerializeField]
    private Texture2D m_splat3;
    [SerializeField]
    private Texture2D m_splat4;
    [SerializeField]
    public bool m_stripTexturesAtRuntime;
    [NonSerialized]
    private float[,,] m_splatBackupArray;
    [SerializeField]
    private CTSConstants.ShaderType m_activeShaderType;
    [NonSerialized]
    private Terrain m_terrain;
    [NonSerialized]
    private Material m_material;
    [NonSerialized]
    private MaterialPropertyBlock m_materialPropertyBlock;
    private static string s_ctsDirectory;

    public CompleteTerrainShader()
    {
      base.\u002Ector();
    }

    public CTSProfile Profile
    {
      get
      {
        return this.m_profile;
      }
      set
      {
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
          this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_profile, (Object) null))
        {
          this.m_profile = value;
          if (Object.op_Inequality((Object) this.m_profile, (Object) null))
          {
            if (this.m_profile.TerrainTextures.Count == 0)
              this.UpdateProfileFromTerrainForced();
            else if (this.TerrainNeedsTextureUpdate())
              this.ReplaceTerrainTexturesFromProfile(false);
          }
        }
        else if (Object.op_Equality((Object) value, (Object) null))
        {
          this.m_profile = value;
        }
        else
        {
          if (((Object) this.m_profile).get_name() != ((Object) value).get_name())
            this.m_profile = value;
          if (this.m_profile.TerrainTextures.Count == 0)
            this.UpdateProfileFromTerrainForced();
          else if (this.TerrainNeedsTextureUpdate())
            this.ReplaceTerrainTexturesFromProfile(false);
        }
        if (!Object.op_Inequality((Object) this.m_profile, (Object) null))
          return;
        this.ApplyMaterialAndUpdateShader();
      }
    }

    public Texture2D NormalMap
    {
      get
      {
        return this.m_normalMap;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_normalMap, (Object) null))
            return;
          this.m_normalMap = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_normalMap, (Object) null) && ((Object) this.m_normalMap).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_normalMap = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public bool AutoBakeNormalMap
    {
      get
      {
        return this.m_bakeNormalMap;
      }
      set
      {
        this.m_bakeNormalMap = value;
      }
    }

    public Texture2D ColorMap
    {
      get
      {
        return this.m_colorMap;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_colorMap, (Object) null))
            return;
          this.m_colorMap = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_colorMap, (Object) null) && ((Object) this.m_colorMap).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_colorMap = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public bool AutoBakeColorMap
    {
      get
      {
        return this.m_bakeColorMap;
      }
      set
      {
        this.m_bakeColorMap = value;
      }
    }

    public bool AutoBakeGrassIntoColorMap
    {
      get
      {
        return this.m_bakeGrassTextures;
      }
      set
      {
        this.m_bakeGrassTextures = value;
      }
    }

    public float AutoBakeGrassMixStrength
    {
      get
      {
        return this.m_bakeGrassMixStrength;
      }
      set
      {
        this.m_bakeGrassMixStrength = value;
      }
    }

    public float AutoBakeGrassDarkenAmount
    {
      get
      {
        return this.m_bakeGrassDarkenAmount;
      }
      set
      {
        this.m_bakeGrassDarkenAmount = value;
      }
    }

    public bool UseCutout
    {
      get
      {
        return this.m_useCutout;
      }
      set
      {
        if (this.m_useCutout == value)
          return;
        this.m_useCutout = value;
        CompleteTerrainShader.SetDirty((Object) this, false, false);
      }
    }

    public Texture2D CutoutMask
    {
      get
      {
        return this.m_cutoutMask;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_cutoutMask, (Object) null))
            return;
          this.m_cutoutMask = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_cutoutMask, (Object) null) && ((Object) this.m_cutoutMask).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_cutoutMask = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public float CutoutHeight
    {
      get
      {
        return this.m_cutoutHeight;
      }
      set
      {
        if ((double) this.m_cutoutHeight == (double) value)
          return;
        this.m_cutoutHeight = value;
        CompleteTerrainShader.SetDirty((Object) this, false, false);
      }
    }

    public Texture2D Splat1
    {
      get
      {
        return this.m_splat1;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_splat1, (Object) null))
            return;
          this.m_splat1 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_splat1, (Object) null) && ((Object) this.m_splat1).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_splat1 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public Texture2D Splat2
    {
      get
      {
        return this.m_splat2;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_splat2, (Object) null))
            return;
          this.m_splat2 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_splat2, (Object) null) && ((Object) this.m_splat2).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_splat2 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public Texture2D Splat3
    {
      get
      {
        return this.m_splat3;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_splat3, (Object) null))
            return;
          this.m_splat3 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_splat3, (Object) null) && ((Object) this.m_splat3).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_splat3 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    public Texture2D Splat4
    {
      get
      {
        return this.m_splat4;
      }
      set
      {
        if (Object.op_Equality((Object) value, (Object) null))
        {
          if (!Object.op_Inequality((Object) this.m_splat4, (Object) null))
            return;
          this.m_splat4 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        else
        {
          if (!Object.op_Equality((Object) this.m_splat4, (Object) null) && ((Object) this.m_splat4).GetInstanceID() == ((Object) value).GetInstanceID())
            return;
          this.m_splat4 = value;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
      }
    }

    private void Awake()
    {
      if (!Object.op_Equality((Object) this.m_terrain, (Object) null))
        return;
      this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (!Object.op_Equality((Object) this.m_terrain, (Object) null))
        return;
      Debug.LogWarning((object) "CTS needs a terrain to work!");
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.LogWarning((object) "CTS needs a terrain, exiting!");
          return;
        }
      }
      this.ApplyMaterialAndUpdateShader();
    }

    private void OnEnable()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      CTSSingleton<CTSTerrainManager>.Instance.RegisterShader(this);
    }

    private void OnDisable()
    {
      CTSSingleton<CTSTerrainManager>.Instance.UnregisterShader(this);
    }

    public static Object GetAsset(string fileNameOrPath, Type assetType)
    {
      return (Object) null;
    }

    public static string GetAssetPath(string fileName)
    {
      return string.Empty;
    }

    public static Type GetType(string TypeName)
    {
      Type type1 = Type.GetType(TypeName);
      if (type1 != (Type) null)
        return type1;
      if (TypeName.Contains("."))
      {
        string assemblyString = TypeName.Substring(0, TypeName.IndexOf('.'));
        try
        {
          Assembly assembly = Assembly.Load(assemblyString);
          if (assembly == (Assembly) null)
            return (Type) null;
          Type type2 = assembly.GetType(TypeName);
          if (type2 != (Type) null)
            return type2;
        }
        catch (Exception ex)
        {
        }
      }
      Assembly callingAssembly = Assembly.GetCallingAssembly();
      if (callingAssembly != (Assembly) null)
      {
        Type type2 = callingAssembly.GetType(TypeName);
        if (type2 != (Type) null)
          return type2;
      }
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      for (int index = 0; index < assemblies.GetLength(0); ++index)
      {
        Type type2 = assemblies[index].GetType(TypeName);
        if (type2 != (Type) null)
          return type2;
      }
      foreach (AssemblyName referencedAssembly in callingAssembly.GetReferencedAssemblies())
      {
        Assembly assembly = Assembly.Load(referencedAssembly);
        if (assembly != (Assembly) null)
        {
          Type type2 = assembly.GetType(TypeName);
          if (type2 != (Type) null)
            return type2;
        }
      }
      return (Type) null;
    }

    private void ApplyUnityShader()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.LogError((object) "Unable to locate Terrain, apply unity shader cancelled.");
          return;
        }
      }
      if (Application.get_isPlaying() && Object.op_Inequality((Object) this.m_profile, (Object) null) && (this.m_splatBackupArray != null && this.m_splatBackupArray.GetLength(0) > 0))
      {
        this.ReplaceTerrainTexturesFromProfile(true);
        this.m_terrain.get_terrainData().SetAlphamaps(0, 0, this.m_splatBackupArray);
        this.m_terrain.Flush();
      }
      this.m_terrain.set_basemapDistance(5000f);
      this.m_activeShaderType = CTSConstants.ShaderType.Unity;
      this.m_terrain.set_materialType((Terrain.MaterialType) 0);
      this.m_terrain.set_materialTemplate((Material) null);
      this.m_material = (Material) null;
      this.m_materialPropertyBlock = (MaterialPropertyBlock) null;
    }

    private void ApplyMaterial()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.LogWarning((object) "CTS needs terrain to function - exiting!");
          return;
        }
      }
      if (Object.op_Equality((Object) this.m_profile, (Object) null))
      {
        Debug.LogWarning((object) "CTS needs a profile to function - applying unity shader and exiting!");
        this.ApplyUnityShader();
      }
      else if (Object.op_Equality((Object) this.m_profile.AlbedosTextureArray, (Object) null))
      {
        Debug.LogWarning((object) "CTS profile needs albedos texture array to function - applying unity shader and exiting!");
        this.m_profile.m_needsAlbedosArrayUpdate = true;
        this.ApplyUnityShader();
      }
      else if (Object.op_Equality((Object) this.m_profile.NormalsTextureArray, (Object) null))
      {
        Debug.LogWarning((object) "CTS profile needs normals texture array to function - applying unity shader and exiting!");
        this.m_profile.m_needsNormalsArrayUpdate = true;
        this.ApplyUnityShader();
      }
      else
      {
        if (Object.op_Equality((Object) this.m_splat1, (Object) null) && this.m_terrain.get_terrainData().get_alphamapTextures().Length > 0)
          this.m_splat1 = this.m_terrain.get_terrainData().get_alphamapTextures()[0];
        if (Object.op_Equality((Object) this.m_splat2, (Object) null) && this.m_terrain.get_terrainData().get_alphamapTextures().Length > 1)
          this.m_splat2 = this.m_terrain.get_terrainData().get_alphamapTextures()[1];
        if (Object.op_Equality((Object) this.m_splat3, (Object) null) && this.m_terrain.get_terrainData().get_alphamapTextures().Length > 2)
          this.m_splat3 = this.m_terrain.get_terrainData().get_alphamapTextures()[2];
        if (Object.op_Equality((Object) this.m_splat4, (Object) null) && this.m_terrain.get_terrainData().get_alphamapTextures().Length > 3)
          this.m_splat4 = this.m_terrain.get_terrainData().get_alphamapTextures()[3];
        this.m_materialPropertyBlock = (MaterialPropertyBlock) null;
        this.m_activeShaderType = this.m_profile.ShaderType;
        switch (this.m_activeShaderType)
        {
          case CTSConstants.ShaderType.Unity:
            this.ApplyUnityShader();
            return;
          case CTSConstants.ShaderType.Basic:
            this.m_material = this.m_useCutout ? CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Basic CutOut", this.m_profile) : CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Basic", this.m_profile);
            break;
          case CTSConstants.ShaderType.Advanced:
            this.m_material = this.m_useCutout ? CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Advanced CutOut", this.m_profile) : CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Advanced", this.m_profile);
            break;
          case CTSConstants.ShaderType.Tesselation:
            this.m_material = this.m_useCutout ? CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Advanced Tess CutOut", this.m_profile) : CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Advanced Tess", this.m_profile);
            break;
          case CTSConstants.ShaderType.Lite:
            this.m_material = CTSMaterials.GetMaterial("CTS/CTS Terrain Shader Lite", this.m_profile);
            break;
        }
        if (Object.op_Equality((Object) this.m_material, (Object) null))
        {
          Debug.LogErrorFormat("CTS could not locate shader {0} - exiting!", new object[1]
          {
            (object) this.m_activeShaderType
          });
        }
        else
        {
          this.m_terrain.set_materialType((Terrain.MaterialType) 3);
          this.m_terrain.set_materialTemplate(this.m_material);
          this.UpdateTerrainSplatsAtRuntime();
        }
      }
    }

    public void ApplyMaterialAndUpdateShader()
    {
      if (Object.op_Equality((Object) this.m_profile, (Object) null))
        this.ApplyMaterial();
      else if (this.m_activeShaderType != this.m_profile.ShaderType)
        this.ApplyMaterial();
      if (this.m_activeShaderType == CTSConstants.ShaderType.Unity)
        return;
      this.UpdateShader();
    }

    public void UpdateShader()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.LogWarning((object) "CTS missing terrain, cannot operate without terrain!");
          return;
        }
      }
      if (this.m_activeShaderType == CTSConstants.ShaderType.Unity)
        this.m_terrain.set_basemapDistance(5000f);
      else if (Object.op_Equality((Object) this.m_profile, (Object) null))
        Debug.LogWarning((object) "Missing CTS profile!");
      else if (Object.op_Equality((Object) this.m_profile.AlbedosTextureArray, (Object) null))
        Debug.LogError((object) "Missing CTS texture array - rebake textures");
      else if (Object.op_Equality((Object) this.m_profile.NormalsTextureArray, (Object) null))
        Debug.LogError((object) "Missing CTS texture array - rebake textures");
      else if (Object.op_Equality((Object) this.m_splat1, (Object) null))
      {
        Debug.LogError((object) "Missing splat textures - add some textures to your terrain");
      }
      else
      {
        Stopwatch stopwatch = Stopwatch.StartNew();
        if (this.m_activeShaderType != this.m_profile.ShaderType || Object.op_Equality((Object) this.m_material, (Object) null))
          this.ApplyMaterial();
        if (this.m_stripTexturesAtRuntime != this.m_profile.m_globalStripTexturesAtRuntime)
        {
          this.m_stripTexturesAtRuntime = this.m_profile.m_globalStripTexturesAtRuntime;
          CompleteTerrainShader.SetDirty((Object) this, false, false);
        }
        if ((double) this.m_terrain.get_basemapDistance() != (double) this.m_profile.m_globalBasemapDistance)
          this.m_terrain.set_basemapDistance(this.m_profile.m_globalBasemapDistance);
        this.m_material.SetTexture(CTSShaderID.Texture_Array_Albedo, (Texture) this.m_profile.AlbedosTextureArray);
        this.m_material.SetTexture(CTSShaderID.Texture_Array_Normal, (Texture) this.m_profile.NormalsTextureArray);
        this.m_material.SetFloat(CTSShaderID.UV_Mix_Power, this.m_profile.m_globalUvMixPower);
        this.m_material.SetFloat(CTSShaderID.UV_Mix_Start_Distance, this.m_profile.m_globalUvMixStartDistance + Random.Range(1f / 1000f, 3f / 1000f));
        this.m_material.SetFloat(CTSShaderID.Perlin_Normal_Tiling_Close, this.m_profile.m_globalDetailNormalCloseTiling);
        this.m_material.SetFloat(CTSShaderID.Perlin_Normal_Tiling_Far, this.m_profile.m_globalDetailNormalFarTiling);
        this.m_material.SetFloat(CTSShaderID.Perlin_Normal_Power, this.m_profile.m_globalDetailNormalFarPower);
        this.m_material.SetFloat(CTSShaderID.Perlin_Normal_Power_Close, this.m_profile.m_globalDetailNormalClosePower);
        this.m_material.SetFloat(CTSShaderID.Terrain_Smoothness, this.m_profile.m_globalTerrainSmoothness);
        this.m_material.SetFloat(CTSShaderID.Terrain_Specular, this.m_profile.m_globalTerrainSpecular);
        this.m_material.SetFloat(CTSShaderID.TessValue, this.m_profile.m_globalTesselationPower);
        this.m_material.SetFloat(CTSShaderID.TessMin, this.m_profile.m_globalTesselationMinDistance);
        this.m_material.SetFloat(CTSShaderID.TessMax, this.m_profile.m_globalTesselationMaxDistance);
        this.m_material.SetFloat(CTSShaderID.TessPhongStrength, this.m_profile.m_globalTesselationPhongStrength);
        this.m_material.SetFloat(CTSShaderID.TessDistance, this.m_profile.m_globalTesselationMaxDistance);
        this.m_material.SetInt(CTSShaderID.Ambient_Occlusion_Type, (int) this.m_profile.m_globalAOType);
        if (this.m_profile.m_globalAOType == CTSConstants.AOType.None)
        {
          this.m_material.DisableKeyword("_Use_AO_ON");
          this.m_material.DisableKeyword("_USE_AO_TEXTURE_ON");
          this.m_material.SetInt(CTSShaderID.Use_AO, 0);
          this.m_material.SetInt(CTSShaderID.Use_AO_Texture, 0);
          this.m_material.SetFloat(CTSShaderID.Ambient_Occlusion_Power, 0.0f);
        }
        else if (this.m_profile.m_globalAOType == CTSConstants.AOType.NormalMapBased)
        {
          this.m_material.DisableKeyword("_USE_AO_TEXTURE_ON");
          this.m_material.SetInt(CTSShaderID.Use_AO_Texture, 0);
          if ((double) this.m_profile.m_globalAOPower > 0.0)
          {
            this.m_material.EnableKeyword("_USE_AO_ON");
            this.m_material.SetInt(CTSShaderID.Use_AO, 1);
            this.m_material.SetFloat(CTSShaderID.Ambient_Occlusion_Power, this.m_profile.m_globalAOPower);
          }
          else
          {
            this.m_material.DisableKeyword("_USE_AO_ON");
            this.m_material.SetInt(CTSShaderID.Use_AO, 0);
            this.m_material.SetFloat(CTSShaderID.Ambient_Occlusion_Power, 0.0f);
          }
        }
        else if ((double) this.m_profile.m_globalAOPower > 0.0)
        {
          this.m_material.EnableKeyword("_USE_AO_ON");
          this.m_material.EnableKeyword("_USE_AO_TEXTURE_ON");
          this.m_material.SetInt(CTSShaderID.Use_AO, 1);
          this.m_material.SetInt(CTSShaderID.Use_AO_Texture, 1);
          this.m_material.SetFloat(CTSShaderID.Ambient_Occlusion_Power, this.m_profile.m_globalAOPower);
        }
        else
        {
          this.m_material.DisableKeyword("_USE_AO_ON");
          this.m_material.DisableKeyword("_USE_AO_TEXTURE_ON");
          this.m_material.SetInt(CTSShaderID.Use_AO, 0);
          this.m_material.SetInt(CTSShaderID.Use_AO_Texture, 0);
          this.m_material.SetFloat(CTSShaderID.Ambient_Occlusion_Power, 0.0f);
        }
        if ((double) this.m_profile.m_globalDetailNormalClosePower > 0.0 || (double) this.m_profile.m_globalDetailNormalFarPower > 0.0)
          this.m_material.SetInt(CTSShaderID.Texture_Perlin_Normal_Index, this.m_profile.m_globalDetailNormalMapIdx);
        else
          this.m_material.SetInt(CTSShaderID.Texture_Perlin_Normal_Index, -1);
        if (Object.op_Inequality((Object) this.m_profile.GeoAlbedo, (Object) null))
        {
          if ((double) this.m_profile.m_geoMapClosePower > 0.0 || (double) this.m_profile.m_geoMapFarPower > 0.0)
          {
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Offset_Close, this.m_profile.m_geoMapCloseOffset);
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Close_Power, this.m_profile.m_geoMapClosePower);
            this.m_material.SetFloat(CTSShaderID.Geological_Tiling_Close, this.m_profile.m_geoMapTilingClose);
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Offset_Far, this.m_profile.m_geoMapFarOffset);
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Far_Power, this.m_profile.m_geoMapFarPower);
            this.m_material.SetFloat(CTSShaderID.Geological_Tiling_Far, this.m_profile.m_geoMapTilingFar);
            this.m_material.SetTexture(CTSShaderID.Texture_Geological_Map, (Texture) this.m_profile.GeoAlbedo);
          }
          else
          {
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Close_Power, 0.0f);
            this.m_material.SetFloat(CTSShaderID.Geological_Map_Far_Power, 0.0f);
            this.m_material.SetTexture(CTSShaderID.Texture_Geological_Map, (Texture) null);
          }
        }
        else
        {
          this.m_material.SetFloat(CTSShaderID.Geological_Map_Close_Power, 0.0f);
          this.m_material.SetFloat(CTSShaderID.Geological_Map_Far_Power, 0.0f);
          this.m_material.SetTexture(CTSShaderID.Texture_Geological_Map, (Texture) null);
        }
        this.m_material.SetFloat(CTSShaderID.Snow_Amount, this.m_profile.m_snowAmount);
        this.m_material.SetInt(CTSShaderID.Texture_Snow_Index, this.m_profile.m_snowAlbedoTextureIdx);
        this.m_material.SetInt(CTSShaderID.Texture_Snow_Normal_Index, this.m_profile.m_snowNormalTextureIdx);
        this.m_material.SetInt(CTSShaderID.Texture_Snow_H_AO_Index, this.m_profile.m_snowHeightTextureIdx == -1 ? this.m_profile.m_snowAOTextureIdx : this.m_profile.m_snowHeightTextureIdx);
        this.m_material.SetTexture(CTSShaderID.Texture_Glitter, (Texture) this.m_profile.SnowGlitter);
        this.m_material.SetFloat(CTSShaderID.Snow_Maximum_Angle, this.m_profile.m_snowMaxAngle);
        this.m_material.SetFloat(CTSShaderID.Snow_Maximum_Angle_Hardness, this.m_profile.m_snowMaxAngleHardness);
        this.m_material.SetFloat(CTSShaderID.Snow_Min_Height, this.m_profile.m_snowMinHeight);
        this.m_material.SetFloat(CTSShaderID.Snow_Min_Height_Blending, this.m_profile.m_snowMinHeightBlending);
        this.m_material.SetFloat(CTSShaderID.Snow_Noise_Power, this.m_profile.m_snowNoisePower);
        this.m_material.SetFloat(CTSShaderID.Snow_Noise_Tiling, this.m_profile.m_snowNoiseTiling);
        this.m_material.SetFloat(CTSShaderID.Snow_Normal_Scale, this.m_profile.m_snowNormalScale);
        this.m_material.SetFloat(CTSShaderID.Snow_Perlin_Power, this.m_profile.m_snowDetailPower);
        this.m_material.SetFloat(CTSShaderID.Snow_Tiling, this.m_profile.m_snowTilingClose);
        this.m_material.SetFloat(CTSShaderID.Snow_Tiling_Far_Multiplier, this.m_profile.m_snowTilingFar);
        this.m_material.SetFloat(CTSShaderID.Snow_Brightness, this.m_profile.m_snowBrightness);
        this.m_material.SetFloat(CTSShaderID.Snow_Blend_Normal, this.m_profile.m_snowBlendNormal);
        this.m_material.SetFloat(CTSShaderID.Snow_Smoothness, this.m_profile.m_snowSmoothness);
        this.m_material.SetFloat(CTSShaderID.Snow_Specular, this.m_profile.m_snowSpecular);
        this.m_material.SetFloat(CTSShaderID.Snow_Heightblend_Close, this.m_profile.m_snowHeightmapBlendClose);
        this.m_material.SetFloat(CTSShaderID.Snow_Heightblend_Far, this.m_profile.m_snowHeightmapBlendFar);
        this.m_material.SetFloat(CTSShaderID.Snow_Height_Contrast, this.m_profile.m_snowHeightmapContrast);
        this.m_material.SetFloat(CTSShaderID.Snow_Heightmap_Depth, this.m_profile.m_snowHeightmapDepth);
        this.m_material.SetFloat(CTSShaderID.Snow_Heightmap_MinHeight, this.m_profile.m_snowHeightmapMinValue);
        this.m_material.SetFloat(CTSShaderID.Snow_Heightmap_MaxHeight, this.m_profile.m_snowHeightmapMaxValue);
        this.m_material.SetFloat(CTSShaderID.Snow_Ambient_Occlusion_Power, this.m_profile.m_snowAOStrength);
        this.m_material.SetFloat(CTSShaderID.Snow_Tesselation_Depth, this.m_profile.m_snowTesselationDepth);
        this.m_material.SetVector(CTSShaderID.Snow_Color, new Vector4((float) this.m_profile.m_snowTint.r * this.m_profile.m_snowBrightness, (float) this.m_profile.m_snowTint.g * this.m_profile.m_snowBrightness, (float) this.m_profile.m_snowTint.b * this.m_profile.m_snowBrightness, this.m_profile.m_snowSmoothness));
        this.m_material.SetVector(CTSShaderID.Texture_Snow_Average, this.m_profile.m_snowAverage);
        this.m_material.SetFloat(CTSShaderID.Glitter_Color_Power, this.m_profile.m_snowGlitterColorPower);
        this.m_material.SetFloat(CTSShaderID.Glitter_Noise_Threshold, this.m_profile.m_snowGlitterNoiseThreshold);
        this.m_material.SetFloat(CTSShaderID.Glitter_Specular, this.m_profile.m_snowGlitterSpecularPower);
        this.m_material.SetFloat(CTSShaderID.Glitter_Smoothness, this.m_profile.m_snowGlitterSmoothness);
        this.m_material.SetFloat(CTSShaderID.Glitter_Refreshing_Speed, this.m_profile.m_snowGlitterRefreshSpeed);
        this.m_material.SetFloat(CTSShaderID.Glitter_Tiling, this.m_profile.m_snowGlitterTiling);
        for (int index = 0; index < this.m_profile.TerrainTextures.Count; ++index)
        {
          CTSTerrainTextureDetails terrainTexture = this.m_profile.TerrainTextures[index];
          this.m_material.SetInt(CTSShaderID.Texture_X_Albedo_Index[index], terrainTexture.m_albedoIdx);
          this.m_material.SetInt(CTSShaderID.Texture_X_Normal_Index[index], terrainTexture.m_normalIdx);
          this.m_material.SetInt(CTSShaderID.Texture_X_H_AO_Index[index], terrainTexture.m_heightIdx == -1 ? terrainTexture.m_aoIdx : terrainTexture.m_heightIdx);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Tiling[index], terrainTexture.m_albedoTilingClose);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Far_Multiplier[index], terrainTexture.m_albedoTilingFar);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Perlin_Power[index], terrainTexture.m_detailPower);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Snow_Reduction[index], terrainTexture.m_snowReductionPower);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Geological_Power[index], terrainTexture.m_geologicalPower);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Heightmap_Depth[index], terrainTexture.m_heightDepth);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Height_Contrast[index], terrainTexture.m_heightContrast);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Heightblend_Close[index], terrainTexture.m_heightBlendClose);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Heightblend_Far[index], terrainTexture.m_heightBlendFar);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Tesselation_Depth[index], terrainTexture.m_heightTesselationDepth);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Heightmap_MinHeight[index], terrainTexture.m_heightMin);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Heightmap_MaxHeight[index], terrainTexture.m_heightMax);
          this.m_material.SetFloat(CTSShaderID.Texture_X_AO_Power[index], terrainTexture.m_aoPower);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Normal_Power[index], terrainTexture.m_normalStrength);
          this.m_material.SetFloat(CTSShaderID.Texture_X_Triplanar[index], !terrainTexture.m_triplanar ? 0.0f : 1f);
          this.m_material.SetVector(CTSShaderID.Texture_X_Average[index], terrainTexture.m_albedoAverage);
          this.m_material.SetVector(CTSShaderID.Texture_X_Color[index], new Vector4((float) terrainTexture.m_tint.r * terrainTexture.m_tintBrightness, (float) terrainTexture.m_tint.g * terrainTexture.m_tintBrightness, (float) terrainTexture.m_tint.b * terrainTexture.m_tintBrightness, terrainTexture.m_smoothness));
        }
        for (int count = this.m_profile.TerrainTextures.Count; count < 16; ++count)
        {
          this.m_material.SetInt(CTSShaderID.Texture_X_Albedo_Index[count], -1);
          this.m_material.SetInt(CTSShaderID.Texture_X_Normal_Index[count], -1);
          this.m_material.SetInt(CTSShaderID.Texture_X_H_AO_Index[count], -1);
        }
        if (this.m_profile.m_useMaterialControlBlock)
        {
          if (this.m_materialPropertyBlock == null)
            this.m_materialPropertyBlock = new MaterialPropertyBlock();
          this.m_terrain.GetSplatMaterialPropertyBlock(this.m_materialPropertyBlock);
          this.m_materialPropertyBlock.SetTexture(CTSShaderID.Texture_Splat_1, (Texture) this.m_splat1);
          if (Object.op_Inequality((Object) this.m_splat2, (Object) null))
            this.m_materialPropertyBlock.SetTexture(CTSShaderID.Texture_Splat_2, (Texture) this.m_splat2);
          if (Object.op_Inequality((Object) this.m_splat3, (Object) null))
            this.m_materialPropertyBlock.SetTexture(CTSShaderID.Texture_Splat_3, (Texture) this.m_splat3);
          if (Object.op_Inequality((Object) this.m_splat4, (Object) null))
            this.m_materialPropertyBlock.SetTexture(CTSShaderID.Texture_Splat_4, (Texture) this.m_splat4);
          this.m_materialPropertyBlock.SetFloat(CTSShaderID.Remove_Vert_Height, this.m_cutoutHeight);
          if (Object.op_Inequality((Object) this.m_cutoutMask, (Object) null))
            this.m_materialPropertyBlock.SetTexture(CTSShaderID.Texture_Additional_Masks, (Texture) this.m_cutoutMask);
          if (Object.op_Inequality((Object) this.NormalMap, (Object) null))
          {
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Normalmap_Power, this.m_profile.m_globalNormalPower);
            if ((double) this.m_profile.m_globalNormalPower > 0.0 && Object.op_Inequality((Object) this.NormalMap, (Object) null))
              this.m_materialPropertyBlock.SetTexture(CTSShaderID.Global_Normal_Map, (Texture) this.NormalMap);
          }
          else
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Normalmap_Power, 0.0f);
          if (Object.op_Inequality((Object) this.ColorMap, (Object) null))
          {
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Map_Far_Power, this.m_profile.m_colorMapFarPower);
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Map_Close_Power, this.m_profile.m_colorMapClosePower);
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Opacity_Power, this.m_profile.m_colorMapOpacity);
            if ((double) this.m_profile.m_colorMapFarPower > 0.0 || (double) this.m_profile.m_colorMapClosePower > 0.0)
              this.m_materialPropertyBlock.SetTexture(CTSShaderID.Global_Color_Map, (Texture) this.ColorMap);
          }
          else
          {
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Map_Far_Power, 0.0f);
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Map_Close_Power, 0.0f);
            this.m_materialPropertyBlock.SetFloat(CTSShaderID.Global_Color_Opacity_Power, 0.0f);
          }
          this.m_terrain.SetSplatMaterialPropertyBlock(this.m_materialPropertyBlock);
        }
        else
        {
          this.m_material.SetTexture(CTSShaderID.Texture_Splat_1, (Texture) this.m_splat1);
          if (Object.op_Inequality((Object) this.m_splat2, (Object) null))
            this.m_material.SetTexture(CTSShaderID.Texture_Splat_2, (Texture) this.m_splat2);
          if (Object.op_Inequality((Object) this.m_splat3, (Object) null))
            this.m_material.SetTexture(CTSShaderID.Texture_Splat_3, (Texture) this.m_splat3);
          if (Object.op_Inequality((Object) this.m_splat4, (Object) null))
            this.m_material.SetTexture(CTSShaderID.Texture_Splat_4, (Texture) this.m_splat4);
          this.m_material.SetFloat(CTSShaderID.Remove_Vert_Height, this.m_cutoutHeight);
          if (Object.op_Inequality((Object) this.m_cutoutMask, (Object) null))
            this.m_material.SetTexture(CTSShaderID.Texture_Additional_Masks, (Texture) this.m_cutoutMask);
          if (Object.op_Inequality((Object) this.NormalMap, (Object) null))
          {
            this.m_material.SetFloat(CTSShaderID.Global_Normalmap_Power, this.m_profile.m_globalNormalPower);
            if ((double) this.m_profile.m_globalNormalPower > 0.0 && Object.op_Inequality((Object) this.NormalMap, (Object) null))
              this.m_material.SetTexture(CTSShaderID.Global_Normal_Map, (Texture) this.NormalMap);
          }
          else
            this.m_material.SetFloat(CTSShaderID.Global_Normalmap_Power, 0.0f);
          if (Object.op_Inequality((Object) this.ColorMap, (Object) null))
          {
            this.m_material.SetFloat(CTSShaderID.Global_Color_Map_Far_Power, this.m_profile.m_colorMapFarPower);
            this.m_material.SetFloat(CTSShaderID.Global_Color_Map_Close_Power, this.m_profile.m_colorMapClosePower);
            this.m_material.SetFloat(CTSShaderID.Global_Color_Opacity_Power, this.m_profile.m_colorMapOpacity);
            if ((double) this.m_profile.m_colorMapFarPower > 0.0 || (double) this.m_profile.m_colorMapClosePower > 0.0)
              this.m_material.SetTexture(CTSShaderID.Global_Color_Map, (Texture) this.ColorMap);
          }
          else
          {
            this.m_material.SetFloat(CTSShaderID.Global_Color_Map_Far_Power, 0.0f);
            this.m_material.SetFloat(CTSShaderID.Global_Color_Map_Close_Power, 0.0f);
            this.m_material.SetFloat(CTSShaderID.Global_Color_Opacity_Power, 0.0f);
          }
        }
        if (stopwatch.ElapsedMilliseconds <= 5L)
          ;
      }
    }

    public void UpdateProfileFromTerrainForced()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        {
          Debug.LogError((object) "CTS is missing terrain, cannot update.");
          return;
        }
      }
      this.m_profile.UpdateSettingsFromTerrain(this.m_terrain, true);
      this.ApplyMaterialAndUpdateShader();
    }

    private bool ProfileNeedsTextureUpdate()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        Debug.LogWarning((object) "No terrain , unable to check if needs texture update");
        return false;
      }
      if (Object.op_Equality((Object) this.m_profile, (Object) null))
      {
        Debug.LogWarning((object) "No profile, unable to check if needs texture update");
        return false;
      }
      if (this.m_profile.TerrainTextures.Count == 0)
        return false;
      SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
      if (this.m_profile.TerrainTextures.Count != splatPrototypes.Length)
        return true;
      for (int index = 0; index < splatPrototypes.Length; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_profile.TerrainTextures[index];
        SplatPrototype splatPrototype = splatPrototypes[index];
        if (Object.op_Equality((Object) terrainTexture.Albedo, (Object) null))
        {
          if (Object.op_Inequality((Object) splatPrototype.get_texture(), (Object) null))
            return true;
        }
        else if (Object.op_Equality((Object) splatPrototype.get_texture(), (Object) null) || ((Object) terrainTexture.Albedo).get_name() != ((Object) splatPrototype.get_texture()).get_name())
          return true;
        if (Object.op_Equality((Object) terrainTexture.Normal, (Object) null))
        {
          if (Object.op_Inequality((Object) splatPrototype.get_normalMap(), (Object) null))
            return true;
        }
        else if (Object.op_Equality((Object) splatPrototype.get_normalMap(), (Object) null) || ((Object) terrainTexture.Normal).get_name() != ((Object) splatPrototype.get_normalMap()).get_name())
          return true;
      }
      return false;
    }

    private bool TerrainNeedsTextureUpdate()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        Debug.LogWarning((object) "No terrain , unable to check if needs texture update");
        return false;
      }
      if (Object.op_Equality((Object) this.m_profile, (Object) null))
      {
        Debug.LogWarning((object) "No profile, unable to check if needs texture update");
        return false;
      }
      if (this.m_profile.TerrainTextures.Count == 0)
        return false;
      SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
      if (this.m_profile.TerrainTextures.Count != splatPrototypes.Length)
        return true;
      for (int index = 0; index < splatPrototypes.Length; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_profile.TerrainTextures[index];
        SplatPrototype splatPrototype = splatPrototypes[index];
        if (Object.op_Equality((Object) terrainTexture.Albedo, (Object) null))
        {
          if (Object.op_Inequality((Object) splatPrototype.get_texture(), (Object) null))
            return true;
        }
        else if (Object.op_Equality((Object) splatPrototype.get_texture(), (Object) null) || ((Object) terrainTexture.Albedo).get_name() != ((Object) splatPrototype.get_texture()).get_name() || (double) terrainTexture.m_albedoTilingClose != splatPrototype.get_tileSize().x)
          return true;
        if (Object.op_Equality((Object) terrainTexture.Normal, (Object) null))
        {
          if (Object.op_Inequality((Object) splatPrototype.get_normalMap(), (Object) null))
            return true;
        }
        else if (Object.op_Equality((Object) splatPrototype.get_normalMap(), (Object) null) || ((Object) terrainTexture.Normal).get_name() != ((Object) splatPrototype.get_normalMap()).get_name())
          return true;
      }
      return false;
    }

    public void ReplaceAlbedoInTerrain(Texture2D texture, int textureIdx, float tiling)
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (!Object.op_Inequality((Object) this.m_terrain, (Object) null))
        return;
      SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
      if (textureIdx >= 0 && textureIdx < splatPrototypes.Length)
      {
        splatPrototypes[textureIdx].set_texture(texture);
        splatPrototypes[textureIdx].set_tileSize(new Vector2(tiling, tiling));
        this.m_terrain.get_terrainData().set_splatPrototypes(splatPrototypes);
        this.m_terrain.Flush();
        CompleteTerrainShader.SetDirty((Object) this.m_terrain, false, false);
      }
      else
        Debug.LogWarning((object) "Invalid texture index in replace albedo");
    }

    public void ReplaceNormalInTerrain(Texture2D texture, int textureIdx, float tiling)
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (!Object.op_Inequality((Object) this.m_terrain, (Object) null))
        return;
      SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
      if (textureIdx >= 0 && textureIdx < splatPrototypes.Length)
      {
        splatPrototypes[textureIdx].set_normalMap(texture);
        splatPrototypes[textureIdx].set_tileSize(new Vector2(tiling, tiling));
        this.m_terrain.get_terrainData().set_splatPrototypes(splatPrototypes);
        this.m_terrain.Flush();
        CompleteTerrainShader.SetDirty((Object) this.m_terrain, false, false);
      }
      else
        Debug.LogWarning((object) "Invalid texture index in replace normal!");
    }

    public void BakeTerrainNormals()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        Debug.LogWarning((object) "Could not make terrain normal, as terrain object not set.");
      }
      else
      {
        Texture2D normals = this.CalculateNormals(this.m_terrain);
        ((Object) normals).set_name(((Object) this.m_terrain).get_name() + " Nrm");
        this.NormalMap = normals;
      }
    }

    public Texture2D CalculateNormals(Terrain terrain)
    {
      int heightmapWidth = terrain.get_terrainData().get_heightmapWidth();
      int heightmapHeight = terrain.get_terrainData().get_heightmapHeight();
      float num1 = (float) (1.0 / ((double) heightmapWidth - 1.0));
      float num2 = (float) (1.0 / ((double) heightmapHeight - 1.0));
      float num3 = (float) heightmapWidth / 2f;
      float num4 = num3 / (float) heightmapWidth;
      float num5 = num3 / (float) heightmapHeight;
      float[] numArray = new float[heightmapWidth * heightmapHeight];
      Buffer.BlockCopy((Array) terrain.get_terrainData().GetHeights(0, 0, heightmapWidth, heightmapHeight), 0, (Array) numArray, 0, numArray.Length * 4);
      Texture2D texture2D = new Texture2D(heightmapWidth, heightmapHeight, (TextureFormat) 20, false, true);
      for (int index1 = 0; index1 < heightmapHeight; ++index1)
      {
        for (int index2 = 0; index2 < heightmapWidth; ++index2)
        {
          int num6 = index2 != heightmapWidth - 1 ? index2 + 1 : index2;
          int num7 = index2 != 0 ? index2 - 1 : index2;
          int num8 = index1 != heightmapHeight - 1 ? index1 + 1 : index1;
          int num9 = index1 != 0 ? index1 - 1 : index1;
          float num10 = numArray[num7 + index1 * heightmapWidth] * num4;
          float num11 = numArray[num6 + index1 * heightmapWidth] * num4;
          float num12 = numArray[index2 + num9 * heightmapWidth] * num5;
          float num13 = numArray[index2 + num8 * heightmapWidth] * num5;
          float num14 = (float) (((double) num11 - (double) num10) / (2.0 * (double) num1));
          float num15 = (float) (((double) num13 - (double) num12) / (2.0 * (double) num2));
          Vector3 vector3;
          vector3.x = (__Null) -(double) num14;
          vector3.y = (__Null) -(double) num15;
          vector3.z = (__Null) 1.0;
          ((Vector3) ref vector3).Normalize();
          Color color;
          color.r = (__Null) (vector3.x * 0.5 + 0.5);
          color.g = (__Null) (vector3.y * 0.5 + 0.5);
          color.b = vector3.z;
          color.a = (__Null) 1.0;
          texture2D.SetPixel(index2, index1, color);
        }
      }
      texture2D.Apply();
      return texture2D;
    }

    public void BakeTerrainBaseMap()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        Debug.LogWarning((object) "Could not make terrain base map, as terrain object not set.");
      }
      else
      {
        int width = 2048;
        int height = 2048;
        Texture2D[] alphamapTextures = this.m_terrain.get_terrainData().get_alphamapTextures();
        SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
        if (alphamapTextures.Length > 0)
        {
          width = ((Texture) alphamapTextures[0]).get_width();
          height = ((Texture) alphamapTextures[0]).get_height();
        }
        float num1 = (float) (width * height);
        Color[] colorArray = new Color[splatPrototypes.Length];
        for (int index = 0; index < splatPrototypes.Length; ++index)
        {
          Texture2D texture2D = CTSProfile.ResizeTexture(splatPrototypes[index].get_texture(), (TextureFormat) 5, 8, width, height, true, false, false);
          Color[] pixels = texture2D.GetPixels(texture2D.get_mipmapCount() - 1);
          colorArray[index] = new Color((float) pixels[0].r, (float) pixels[0].g, (float) pixels[0].b, (float) pixels[0].a);
        }
        Texture2D texture2D1 = new Texture2D(width, height, (TextureFormat) 4, false);
        ((Object) texture2D1).set_name(((Object) this.m_terrain).get_name() + "_BaseMap");
        ((Texture) texture2D1).set_wrapMode((TextureWrapMode) 0);
        ((Texture) texture2D1).set_filterMode((FilterMode) 1);
        ((Texture) texture2D1).set_anisoLevel(8);
        for (int index1 = 0; index1 < width; ++index1)
        {
          for (int index2 = 0; index2 < height; ++index2)
          {
            int num2 = 0;
            Color color = Color.get_black();
            for (int index3 = 0; index3 < alphamapTextures.Length; ++index3)
            {
              Color pixel = alphamapTextures[index3].GetPixel(index1, index2);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.r);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.g);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.b);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.a);
              color.a = (__Null) 1.0;
            }
            texture2D1.SetPixel(index1, index2, color);
          }
        }
        texture2D1.Apply();
        this.ColorMap = texture2D1;
      }
    }

    public void BakeTerrainBaseMapWithGrass()
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        Debug.LogWarning((object) "Could not make terrain base map, as terrain object not set.");
      }
      else
      {
        int width = 2048;
        int height = 2048;
        Texture2D[] alphamapTextures = this.m_terrain.get_terrainData().get_alphamapTextures();
        SplatPrototype[] splatPrototypes = this.m_terrain.get_terrainData().get_splatPrototypes();
        if (alphamapTextures.Length > 0)
        {
          width = ((Texture) alphamapTextures[0]).get_width();
          height = ((Texture) alphamapTextures[0]).get_height();
        }
        float num1 = (float) (width * height);
        Color[] colorArray = new Color[splatPrototypes.Length];
        for (int index = 0; index < splatPrototypes.Length; ++index)
        {
          Texture2D texture2D = CTSProfile.ResizeTexture(splatPrototypes[index].get_texture(), (TextureFormat) 5, 8, width, height, true, false, false);
          Color[] pixels = texture2D.GetPixels(texture2D.get_mipmapCount() - 1);
          colorArray[index] = new Color((float) pixels[0].r, (float) pixels[0].g, (float) pixels[0].b, (float) pixels[0].a);
        }
        List<Color> colorList = new List<Color>();
        DetailPrototype[] detailPrototypes = this.m_terrain.get_terrainData().get_detailPrototypes();
        List<CTSHeightMap> ctsHeightMapList = new List<CTSHeightMap>();
        int detailWidth = this.m_terrain.get_terrainData().get_detailWidth();
        int detailHeight = this.m_terrain.get_terrainData().get_detailHeight();
        for (int index = 0; index < detailPrototypes.Length; ++index)
        {
          DetailPrototype detailPrototype = detailPrototypes[index];
          if (!detailPrototype.get_usePrototypeMesh() && Object.op_Inequality((Object) detailPrototype.get_prototypeTexture(), (Object) null))
          {
            ctsHeightMapList.Add(new CTSHeightMap(this.m_terrain.get_terrainData().GetDetailLayer(0, 0, detailWidth, detailHeight, index)));
            Texture2D texture2D = CTSProfile.ResizeTexture(detailPrototype.get_prototypeTexture(), (TextureFormat) 5, 8, detailWidth, detailHeight, true, false, false);
            Color[] pixels = texture2D.GetPixels(texture2D.get_mipmapCount() - 1);
            Color color1;
            ((Color) ref color1).\u002Ector((float) pixels[0].r, (float) pixels[0].g, (float) pixels[0].b, 1f);
            Color color2 = Color.Lerp(detailPrototype.get_healthyColor(), detailPrototype.get_dryColor(), 0.2f);
            color1 = Color.Lerp(color1, color2, 0.3f);
            colorList.Add(color1);
          }
        }
        Texture2D texture2D1 = new Texture2D(width, height, (TextureFormat) 4, false);
        ((Object) texture2D1).set_name(((Object) this.m_terrain).get_name() + "_BaseMap");
        ((Texture) texture2D1).set_wrapMode((TextureWrapMode) 0);
        ((Texture) texture2D1).set_filterMode((FilterMode) 1);
        ((Texture) texture2D1).set_anisoLevel(8);
        for (int index1 = 0; index1 < width; ++index1)
        {
          for (int index2 = 0; index2 < height; ++index2)
          {
            int num2 = 0;
            Color color = Color.get_black();
            for (int index3 = 0; index3 < alphamapTextures.Length; ++index3)
            {
              Color pixel = alphamapTextures[index3].GetPixel(index1, index2);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.r);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.g);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.b);
              if (num2 < colorArray.Length)
                color = Color.Lerp(color, colorArray[num2++], (float) pixel.a);
            }
            for (int index3 = 0; index3 < colorList.Count; ++index3)
            {
              float num3 = ctsHeightMapList[index3][(float) index2 / (float) height, (float) index1 / (float) width] * this.m_bakeGrassMixStrength;
              color = Color.Lerp(color, Color.Lerp(colorList[index3], Color.get_black(), this.m_bakeGrassDarkenAmount), num3);
            }
            color.a = (__Null) 1.0;
            texture2D1.SetPixel(index1, index2, color);
          }
        }
        texture2D1.Apply();
        this.ColorMap = texture2D1;
      }
    }

    private void UpdateTerrainSplatsAtRuntime()
    {
      if (!Application.get_isPlaying() || Object.op_Equality((Object) this.m_terrain, (Object) null))
        return;
      if (Object.op_Inequality((Object) this.m_profile, (Object) null))
        this.m_stripTexturesAtRuntime = this.m_profile.m_globalStripTexturesAtRuntime;
      if (!this.m_stripTexturesAtRuntime)
        return;
      if (this.m_splatBackupArray == null || this.m_splatBackupArray.GetLength(0) == 0)
        this.m_splatBackupArray = this.m_terrain.get_terrainData().GetAlphamaps(0, 0, this.m_terrain.get_terrainData().get_alphamapWidth(), this.m_terrain.get_terrainData().get_alphamapHeight());
      if (this.m_terrain.get_materialType() != 3 || ((Object) this.m_terrain.get_terrainData()).get_name().EndsWith("_copy"))
        return;
      TerrainData terrainData = new TerrainData();
      ((Object) terrainData).set_name(((Object) this.m_terrain.get_terrainData()).get_name() + "_copy");
      terrainData.set_thickness(this.m_terrain.get_terrainData().get_thickness());
      terrainData.set_alphamapResolution(this.m_terrain.get_terrainData().get_alphamapResolution());
      terrainData.set_baseMapResolution(this.m_terrain.get_terrainData().get_baseMapResolution());
      terrainData.SetDetailResolution(this.m_terrain.get_terrainData().get_detailResolution(), 64);
      terrainData.set_detailPrototypes(this.m_terrain.get_terrainData().get_detailPrototypes());
      for (int index = 0; index < terrainData.get_detailPrototypes().Length; ++index)
        terrainData.SetDetailLayer(0, 0, index, this.m_terrain.get_terrainData().GetDetailLayer(0, 0, terrainData.get_detailResolution(), terrainData.get_detailResolution(), index));
      terrainData.set_wavingGrassAmount(this.m_terrain.get_terrainData().get_wavingGrassAmount());
      terrainData.set_wavingGrassSpeed(this.m_terrain.get_terrainData().get_wavingGrassSpeed());
      terrainData.set_wavingGrassStrength(this.m_terrain.get_terrainData().get_wavingGrassStrength());
      terrainData.set_wavingGrassTint(this.m_terrain.get_terrainData().get_wavingGrassTint());
      terrainData.set_treePrototypes(this.m_terrain.get_terrainData().get_treePrototypes());
      terrainData.set_treeInstances(this.m_terrain.get_terrainData().get_treeInstances());
      terrainData.set_heightmapResolution(this.m_terrain.get_terrainData().get_heightmapResolution());
      terrainData.SetHeights(0, 0, this.m_terrain.get_terrainData().GetHeights(0, 0, terrainData.get_heightmapResolution(), terrainData.get_heightmapResolution()));
      terrainData.set_size(this.m_terrain.get_terrainData().get_size());
      this.m_terrain.set_terrainData(terrainData);
      this.m_terrain.Flush();
      TerrainCollider component = (TerrainCollider) ((Component) this.m_terrain).get_gameObject().GetComponent<TerrainCollider>();
      if (!Object.op_Inequality((Object) component, (Object) null))
        return;
      component.set_terrainData(terrainData);
    }

    private void ReplaceTerrainTexturesFromProfile(bool ignoreStripTextures)
    {
      if (Object.op_Equality((Object) this.m_terrain, (Object) null))
      {
        this.m_terrain = (Terrain) ((Component) ((Component) this).get_transform()).GetComponent<Terrain>();
        if (Object.op_Equality((Object) this.m_terrain, (Object) null))
          return;
      }
      if (Object.op_Equality((Object) this.m_profile, (Object) null))
        Debug.LogWarning((object) "No profile, unable to replace terrain textures");
      else if (this.m_profile.TerrainTextures.Count == 0)
      {
        Debug.LogWarning((object) "No profile textures, unable to replace terrain textures");
      }
      else
      {
        this.m_stripTexturesAtRuntime = this.m_profile.m_globalStripTexturesAtRuntime;
        if (Application.get_isPlaying() && !ignoreStripTextures && this.m_stripTexturesAtRuntime)
          return;
        SplatPrototype[] splatPrototypeArray = new SplatPrototype[this.m_profile.TerrainTextures.Count];
        for (int index = 0; index < splatPrototypeArray.Length; ++index)
        {
          splatPrototypeArray[index] = new SplatPrototype();
          splatPrototypeArray[index].set_texture(this.m_profile.TerrainTextures[index].Albedo);
          splatPrototypeArray[index].set_normalMap(this.m_profile.TerrainTextures[index].Normal);
          splatPrototypeArray[index].set_tileSize(new Vector2(this.m_profile.TerrainTextures[index].m_albedoTilingClose, this.m_profile.TerrainTextures[index].m_albedoTilingClose));
        }
        this.m_terrain.get_terrainData().set_splatPrototypes(splatPrototypeArray);
        this.m_terrain.Flush();
        CompleteTerrainShader.SetDirty((Object) this.m_terrain, false, false);
      }
    }

    public static string GetCTSDirectory()
    {
      if (string.IsNullOrEmpty(CompleteTerrainShader.s_ctsDirectory))
        CompleteTerrainShader.s_ctsDirectory = "Assets/CTS/";
      return CompleteTerrainShader.s_ctsDirectory;
    }

    public static void SetDirty(Object obj, bool recordUndo, bool isPlayingAllowed)
    {
    }

    public void RemoveWorldSeams()
    {
      Terrain[] activeTerrains = Terrain.get_activeTerrains();
      if (activeTerrains.Length == 0)
        return;
      float x = (float) activeTerrains[0].get_terrainData().get_size().x;
      float z = (float) activeTerrains[0].get_terrainData().get_size().z;
      float num1 = float.MaxValue;
      float num2 = float.MinValue;
      float num3 = float.MaxValue;
      float num4 = (float) int.MinValue;
      foreach (Terrain terrain in activeTerrains)
      {
        Vector3 position = terrain.GetPosition();
        if (position.x < (double) num1)
          num1 = (float) position.x;
        if (position.z < (double) num3)
          num3 = (float) position.z;
        if (position.x > (double) num2)
          num2 = (float) position.x;
        if (position.z > (double) num4)
          num4 = (float) position.z;
      }
      int length1 = (int) (((double) num2 - (double) num1) / (double) x) + 1;
      int length2 = (int) (((double) num4 - (double) num3) / (double) z) + 1;
      Terrain[,] terrainArray = new Terrain[length1, length2];
      foreach (Terrain terrain in activeTerrains)
      {
        Vector3 position = terrain.GetPosition();
        int index1 = length1 - (int) (((double) num2 - position.x) / (double) x) - 1;
        int index2 = length2 - (int) (((double) num4 - position.z) / (double) z) - 1;
        terrainArray[index1, index2] = terrain;
      }
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
        {
          Terrain terrain1 = (Terrain) null;
          Terrain terrain2 = (Terrain) null;
          Terrain terrain3 = (Terrain) null;
          Terrain terrain4 = (Terrain) null;
          if (index1 > 0)
            terrain2 = terrainArray[index1 - 1, index2];
          if (index1 < length1 - 1)
            terrain1 = terrainArray[index1 + 1, index2];
          if (index2 > 0)
            terrain3 = terrainArray[index1, index2 - 1];
          if (index2 < length2 - 1)
            terrain4 = terrainArray[index1, index2 + 1];
          terrainArray[index1, index2].SetNeighbors(terrain2, terrain4, terrain1, terrain3);
        }
      }
    }

    [Flags]
    internal enum TerrainChangedFlags
    {
      NoChange = 0,
      Heightmap = 1,
      TreeInstances = 2,
      DelayedHeightmapUpdate = 4,
      FlushEverythingImmediately = 8,
      RemoveDirtyDetailsImmediately = 16, // 0x00000010
      WillBeDestroyed = 256, // 0x00000100
    }
  }
}
