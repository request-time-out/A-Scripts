// Decompiled with JetBrains decompiler
// Type: CTS.CTSProfile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CTS
{
  [Serializable]
  public class CTSProfile : ScriptableObject
  {
    [SerializeField]
    private int m_ctsMajorVersion;
    [SerializeField]
    private int m_ctsMinorVersion;
    public bool m_persistMaterials;
    public bool m_useMaterialControlBlock;
    public bool m_showGlobalSettings;
    public bool m_showSnowSettings;
    public bool m_showTextureSettings;
    public bool m_showGeoSettings;
    public bool m_showDetailSettings;
    public bool m_showColorMapSettings;
    public bool m_showOptimisationSettings;
    public string m_ctsDirectory;
    [SerializeField]
    private CTSConstants.ShaderType m_shaderType;
    public float m_globalUvMixPower;
    public float m_globalUvMixStartDistance;
    public float m_globalNormalPower;
    public float m_globalDetailNormalClosePower;
    public float m_globalDetailNormalCloseTiling;
    public float m_globalDetailNormalFarPower;
    public float m_globalDetailNormalFarTiling;
    public float m_globalTerrainSmoothness;
    public float m_globalTerrainSpecular;
    public float m_globalTesselationPower;
    public float m_globalTesselationMinDistance;
    public float m_globalTesselationMaxDistance;
    public float m_globalTesselationPhongStrength;
    public CTSConstants.AOType m_globalAOType;
    public float m_globalAOPower;
    public float m_globalBasemapDistance;
    public bool m_globalStripTexturesAtRuntime;
    public bool m_globalDisconnectProfileAtRuntime;
    public float m_colorMapClosePower;
    public float m_colorMapFarPower;
    public float m_colorMapOpacity;
    public float m_geoMapCloseOffset;
    public float m_geoMapClosePower;
    public float m_geoMapTilingClose;
    public float m_geoMapFarOffset;
    public float m_geoMapFarPower;
    public float m_geoMapTilingFar;
    public float m_snowAmount;
    public float m_snowMaxAngle;
    public float m_snowMaxAngleHardness;
    public float m_snowMinHeight;
    public float m_snowMinHeightBlending;
    public float m_snowNoisePower;
    public float m_snowNoiseTiling;
    public float m_snowNormalScale;
    public float m_snowDetailPower;
    public float m_snowTilingClose;
    public float m_snowTilingFar;
    public float m_snowBrightness;
    public float m_snowBlendNormal;
    public float m_snowSmoothness;
    public Color m_snowTint;
    public float m_snowSpecular;
    public float m_snowHeightmapBlendClose;
    public float m_snowHeightmapBlendFar;
    public float m_snowHeightmapDepth;
    public float m_snowHeightmapContrast;
    public float m_snowHeightmapMinValue;
    public float m_snowHeightmapMaxValue;
    public float m_snowTesselationDepth;
    public float m_snowAOStrength;
    public Vector4 m_snowAverage;
    public TextureFormat m_albedoFormat;
    public int m_albedoAniso;
    public FilterMode m_albedoFilterMode;
    [SerializeField]
    private CTSConstants.TextureSize m_albedoTextureSize;
    public int m_albedoTextureSizePx;
    [SerializeField]
    private bool m_albedoCompress;
    public TextureFormat m_normalFormat;
    public int m_normalAniso;
    public FilterMode m_normalFilterMode;
    [SerializeField]
    private CTSConstants.TextureSize m_normalTextureSize;
    public int m_normalTextureSizePx;
    [SerializeField]
    private bool m_normalCompress;
    public int m_globalDetailNormalMapIdx;
    [SerializeField]
    private Texture2D m_globalDetailNormalMap;
    public int m_snowAlbedoTextureIdx;
    [SerializeField]
    private Texture2D m_snowAlbedoTexture;
    public int m_snowNormalTextureIdx;
    [SerializeField]
    private Texture2D m_snowNormalTexture;
    public int m_snowHeightTextureIdx;
    [SerializeField]
    private Texture2D m_snowHeightTexture;
    public int m_snowAOTextureIdx;
    [SerializeField]
    private Texture2D m_snowAOTexture;
    public int m_snowEmissionTextureIdx;
    [SerializeField]
    private Texture2D m_snowEmissionTexture;
    [SerializeField]
    private Texture2D m_snowGlitterTexture;
    public float m_snowGlitterColorPower;
    public float m_snowGlitterNoiseThreshold;
    public float m_snowGlitterSpecularPower;
    public float m_snowGlitterSmoothness;
    public float m_snowGlitterRefreshSpeed;
    public float m_snowGlitterTiling;
    [SerializeField]
    private Texture2D m_geoAlbedoTexture;
    [SerializeField]
    private List<CTSTerrainTextureDetails> m_terrainTextures;
    [SerializeField]
    private List<Texture2D> m_replacementTerrainAlbedos;
    [SerializeField]
    private List<Texture2D> m_replacementTerrainNormals;
    [SerializeField]
    private Texture2DArray m_albedosTextureArray;
    public bool m_needsAlbedosArrayUpdate;
    [SerializeField]
    private Texture2DArray m_normalsTextureArray;
    public bool m_needsNormalsArrayUpdate;

    public CTSProfile()
    {
      base.\u002Ector();
    }

    public int MajorVersion
    {
      get
      {
        return this.m_ctsMajorVersion;
      }
      set
      {
        if (this.m_ctsMajorVersion.Equals(value))
          return;
        this.m_ctsMajorVersion = value;
        this.m_needsAlbedosArrayUpdate = true;
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public int MinorVersion
    {
      get
      {
        return this.m_ctsMinorVersion;
      }
      set
      {
        if (this.m_ctsMinorVersion.Equals(value))
          return;
        this.m_ctsMinorVersion = value;
        this.m_needsAlbedosArrayUpdate = true;
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public CTSConstants.ShaderType ShaderType
    {
      get
      {
        return this.m_shaderType;
      }
      set
      {
        if (this.m_shaderType == value)
          return;
        this.m_shaderType = value;
      }
    }

    public CTSConstants.TextureSize AlbedoTextureSize
    {
      get
      {
        return this.m_albedoTextureSize;
      }
      set
      {
        if (this.m_albedoTextureSize == value)
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_albedoTextureSize = value;
        this.m_albedoTextureSizePx = CTSConstants.GetTextureSize(this.m_albedoTextureSize);
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public bool AlbedoCompressionEnabled
    {
      get
      {
        return this.m_albedoCompress;
      }
      set
      {
        if (this.m_albedoCompress == value)
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_albedoCompress = value;
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public CTSConstants.TextureSize NormalTextureSize
    {
      get
      {
        return this.m_normalTextureSize;
      }
      set
      {
        if (this.m_normalTextureSize == value)
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_normalTextureSize = value;
        this.m_normalTextureSizePx = CTSConstants.GetTextureSize(this.m_normalTextureSize);
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public bool NormalCompressionEnabled
    {
      get
      {
        return this.m_normalCompress;
      }
      set
      {
        if (this.m_normalCompress == value)
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_normalCompress = value;
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public Texture2D GlobalDetailNormalMap
    {
      get
      {
        return this.m_globalDetailNormalMap;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_globalDetailNormalMap, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_globalDetailNormalMap = value;
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public Texture2D SnowAlbedo
    {
      get
      {
        return this.m_snowAlbedoTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowAlbedoTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowAlbedoTexture = value;
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public Texture2D SnowNormal
    {
      get
      {
        return this.m_snowNormalTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowNormalTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowNormalTexture = value;
        this.m_needsNormalsArrayUpdate = true;
      }
    }

    public Texture2D SnowHeight
    {
      get
      {
        return this.m_snowHeightTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowHeightTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowHeightTexture = value;
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public Texture2D SnowAmbientOcclusion
    {
      get
      {
        return this.m_snowAOTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowAOTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowAOTexture = value;
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public Texture2D SnowEmission
    {
      get
      {
        return this.m_snowEmissionTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowEmissionTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowEmissionTexture = value;
        this.m_needsAlbedosArrayUpdate = true;
      }
    }

    public Texture2D SnowGlitter
    {
      get
      {
        return this.m_snowGlitterTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_snowGlitterTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_snowGlitterTexture = value;
      }
    }

    public Texture2D GeoAlbedo
    {
      get
      {
        return this.m_geoAlbedoTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_geoAlbedoTexture, value))
          return;
        CompleteTerrainShader.SetDirty((Object) this, false, true);
        this.m_geoAlbedoTexture = value;
      }
    }

    public List<CTSTerrainTextureDetails> TerrainTextures
    {
      get
      {
        return this.m_terrainTextures;
      }
      set
      {
        this.m_terrainTextures = value;
      }
    }

    public List<Texture2D> ReplacementTerrainAlbedos
    {
      get
      {
        return this.m_replacementTerrainAlbedos;
      }
    }

    public List<Texture2D> ReplacementTerrainNormals
    {
      get
      {
        return this.m_replacementTerrainNormals;
      }
    }

    public Texture2DArray AlbedosTextureArray
    {
      get
      {
        return this.m_albedosTextureArray;
      }
      set
      {
        CompleteTerrainShader.SetDirty((Object) this, false, false);
        this.m_albedosTextureArray = value;
        this.m_needsAlbedosArrayUpdate = false;
      }
    }

    public Texture2DArray NormalsTextureArray
    {
      get
      {
        return this.m_normalsTextureArray;
      }
      set
      {
        CompleteTerrainShader.SetDirty((Object) this, false, false);
        this.m_normalsTextureArray = value;
        this.m_needsNormalsArrayUpdate = false;
      }
    }

    public bool NeedsArrayUpdate()
    {
      if (this.m_needsAlbedosArrayUpdate || this.m_needsNormalsArrayUpdate)
        return true;
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
      {
        if (this.m_terrainTextures[index].TextureHasChanged())
          return true;
      }
      return false;
    }

    public void RegenerateArraysIfNecessary()
    {
      int num = 0;
      while (num < this.m_terrainTextures.Count)
        ++num;
      if (this.m_needsAlbedosArrayUpdate)
        this.ConstructAlbedosTextureArray();
      if (this.m_needsNormalsArrayUpdate)
        this.ConstructNormalsTextureArray();
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
        this.m_terrainTextures[index].ResetChangedFlags();
    }

    private void ConstructAlbedosTextureArray()
    {
      this.m_needsAlbedosArrayUpdate = false;
      List<Texture2D> sourceTextures = new List<Texture2D>();
      int num1 = 0;
      byte minHeight;
      byte maxHeight;
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_terrainTextures[index];
        if (Object.op_Inequality((Object) terrainTexture.Albedo, (Object) null))
        {
          Texture2D texture2D = !Object.op_Equality((Object) terrainTexture.Smoothness, (Object) null) || !Object.op_Equality((Object) terrainTexture.Roughness, (Object) null) ? this.BakeAlbedo(terrainTexture.Albedo, terrainTexture.Smoothness, terrainTexture.Roughness) : (!this.m_albedoCompress ? CTSProfile.ResizeTexture(terrainTexture.Albedo, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, true, false, false) : CTSProfile.ResizeTexture(terrainTexture.Albedo, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, true, false, true));
          sourceTextures.Add(texture2D);
          Color linear = ((Color) ref texture2D.GetPixels(texture2D.get_mipmapCount() - 1)[0]).get_linear();
          terrainTexture.m_albedoAverage = new Vector4((float) linear.r, (float) linear.g, (float) linear.b, (float) linear.a);
          terrainTexture.m_albedoIdx = num1++;
          if ((this.m_shaderType == CTSConstants.ShaderType.Advanced || this.m_shaderType == CTSConstants.ShaderType.Tesselation) && (Object.op_Inequality((Object) terrainTexture.Height, (Object) null) || Object.op_Inequality((Object) terrainTexture.AmbientOcclusion, (Object) null)))
          {
            sourceTextures.Add(this.BakeHAOTexture(terrainTexture.m_name, terrainTexture.Height, terrainTexture.AmbientOcclusion, out minHeight, out maxHeight));
            if (Object.op_Inequality((Object) terrainTexture.Height, (Object) null))
            {
              terrainTexture.m_heightIdx = num1;
              terrainTexture.m_heightMin = (float) minHeight / (float) byte.MaxValue;
              terrainTexture.m_heightMax = (float) maxHeight / (float) byte.MaxValue;
            }
            else
              terrainTexture.m_heightIdx = -1;
            terrainTexture.m_aoIdx = !Object.op_Inequality((Object) terrainTexture.AmbientOcclusion, (Object) null) ? -1 : num1;
            ++num1;
          }
          else
          {
            terrainTexture.m_aoIdx = -1;
            terrainTexture.m_heightIdx = -1;
          }
        }
        else
          terrainTexture.m_albedoIdx = -1;
        terrainTexture.m_albedoWasChanged = false;
      }
      if (Object.op_Inequality((Object) this.m_snowAlbedoTexture, (Object) null))
      {
        Texture2D texture2D = !this.m_albedoCompress ? CTSProfile.ResizeTexture(this.m_snowAlbedoTexture, this.m_albedoFormat, this.m_normalAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, true, false, false) : CTSProfile.ResizeTexture(this.m_snowAlbedoTexture, this.m_albedoFormat, this.m_normalAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, true, false, true);
        Color linear = ((Color) ref texture2D.GetPixels(texture2D.get_mipmapCount() - 1)[0]).get_linear();
        this.m_snowAverage = new Vector4((float) linear.r, (float) linear.g, (float) linear.b, (float) linear.a);
        sourceTextures.Add(texture2D);
        this.m_snowAlbedoTextureIdx = num1++;
      }
      else
        this.m_snowAlbedoTextureIdx = -1;
      if (Object.op_Inequality((Object) this.m_snowHeightTexture, (Object) null) || Object.op_Inequality((Object) this.m_snowAOTexture, (Object) null))
      {
        sourceTextures.Add(this.BakeHAOTexture("CTS_SnowHAO", this.m_snowHeightTexture, this.m_snowAOTexture, out minHeight, out maxHeight));
        if (Object.op_Inequality((Object) this.m_snowHeightTexture, (Object) null))
        {
          this.m_snowHeightTextureIdx = num1;
          this.m_snowHeightmapMinValue = (float) minHeight / (float) byte.MaxValue;
          this.m_snowHeightmapMaxValue = (float) maxHeight / (float) byte.MaxValue;
        }
        else
        {
          this.m_snowHeightTextureIdx = -1;
          this.m_snowHeightmapMinValue = 0.0f;
          this.m_snowHeightmapMaxValue = 1f;
        }
        this.m_snowAOTextureIdx = !Object.op_Inequality((Object) this.m_snowAOTexture, (Object) null) ? -1 : num1;
        int num2 = num1 + 1;
      }
      else
      {
        this.m_snowAOTextureIdx = -1;
        this.m_snowHeightTextureIdx = -1;
      }
      this.AlbedosTextureArray = this.GetTextureArray(sourceTextures, CTSConstants.TextureType.Albedo, this.m_albedoAniso);
    }

    private void ConstructNormalsTextureArray()
    {
      this.m_needsNormalsArrayUpdate = false;
      List<Texture2D> sourceTextures = new List<Texture2D>();
      int num1 = 0;
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_terrainTextures[index];
        if (Object.op_Inequality((Object) terrainTexture.Normal, (Object) null))
        {
          sourceTextures.Add(this.BakeNormal(terrainTexture.Normal));
          terrainTexture.m_normalIdx = num1++;
        }
        else
          terrainTexture.m_normalIdx = -1;
        terrainTexture.m_normalWasChanged = false;
      }
      if (Object.op_Inequality((Object) this.m_snowNormalTexture, (Object) null))
      {
        sourceTextures.Add(this.BakeNormal(this.m_snowNormalTexture));
        this.m_snowNormalTextureIdx = num1++;
      }
      else
        this.m_snowNormalTextureIdx = -1;
      if (Object.op_Implicit((Object) this.m_globalDetailNormalMap))
      {
        sourceTextures.Add(this.BakeNormal(this.m_globalDetailNormalMap));
        int num2 = num1;
        int num3 = num2 + 1;
        this.m_globalDetailNormalMapIdx = num2;
      }
      else
        this.m_globalDetailNormalMapIdx = -1;
      this.NormalsTextureArray = this.GetTextureArray(sourceTextures, CTSConstants.TextureType.Normal, this.m_normalAniso);
    }

    public void UpdateSettingsFromTerrain(Terrain terrain, bool forceUpdate)
    {
      if (Object.op_Equality((Object) terrain, (Object) null) || Object.op_Equality((Object) terrain.get_terrainData(), (Object) null))
        return;
      if (forceUpdate)
      {
        this.m_needsAlbedosArrayUpdate = true;
        this.m_needsNormalsArrayUpdate = true;
      }
      while (this.m_terrainTextures.Count > terrain.get_terrainData().get_splatPrototypes().Length)
      {
        this.m_terrainTextures.RemoveAt(this.m_terrainTextures.Count - 1);
        this.m_needsAlbedosArrayUpdate = true;
        this.m_needsNormalsArrayUpdate = true;
      }
      SplatPrototype[] splatPrototypes = terrain.get_terrainData().get_splatPrototypes();
      for (int index = 0; index < splatPrototypes.Length; ++index)
      {
        SplatPrototype splatPrototype = splatPrototypes[index];
        if (index < this.m_terrainTextures.Count)
        {
          CTSTerrainTextureDetails terrainTexture = this.m_terrainTextures[index];
          terrainTexture.Albedo = splatPrototype.get_texture();
          terrainTexture.m_albedoTilingClose = (float) terrain.get_terrainData().get_splatPrototypes()[index].get_tileSize().x;
          terrainTexture.Normal = splatPrototype.get_normalMap();
        }
        else
          this.m_terrainTextures.Add(new CTSTerrainTextureDetails()
          {
            m_textureIdx = index,
            Albedo = terrain.get_terrainData().get_splatPrototypes()[index].get_texture(),
            m_albedoTilingClose = (float) terrain.get_terrainData().get_splatPrototypes()[index].get_tileSize().x,
            Normal = terrain.get_terrainData().get_splatPrototypes()[index].get_normalMap()
          });
      }
      this.RegenerateArraysIfNecessary();
    }

    private void ImportTexture(Texture2D texture, int textureSize, bool asNormal = false)
    {
      if (Object.op_Equality((Object) texture, (Object) null))
        return;
      Debug.Log((object) ("Importing " + ((Object) texture).get_name() + " " + (object) textureSize));
    }

    private Color32[] GetTextureColors(
      Texture2D source,
      TextureFormat format,
      int dimensions)
    {
      Texture2D texture2D = CTSProfile.ResizeTexture(source, format, this.m_albedoAniso, dimensions, dimensions, false, false, false);
      Color32[] pixels32 = texture2D.GetPixels32();
      if (!Application.get_isPlaying())
        Object.DestroyImmediate((Object) texture2D);
      else
        Object.Destroy((Object) texture2D);
      return pixels32;
    }

    public Texture2D BakeHAOTexture(
      string name,
      Texture2D hTexture,
      Texture2D aoTexture,
      out byte minHeight,
      out byte maxHeight)
    {
      minHeight = (byte) 0;
      maxHeight = byte.MaxValue;
      int num = this.m_albedoTextureSizePx * this.m_albedoTextureSizePx;
      if (num == 0)
        return (Texture2D) null;
      Texture2D texture2D1 = new Texture2D(this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, this.m_albedoFormat, true, false);
      ((Object) texture2D1).set_name("CTS_" + name + "_HAO");
      ((Texture) texture2D1).set_anisoLevel(this.m_albedoAniso);
      ((Texture) texture2D1).set_filterMode(this.m_albedoFilterMode);
      Color32[] pixels32_1 = texture2D1.GetPixels32();
      if (Object.op_Inequality((Object) hTexture, (Object) null))
      {
        minHeight = byte.MaxValue;
        maxHeight = (byte) 0;
        Texture2D texture2D2 = CTSProfile.ResizeTexture(hTexture, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, false, false, false);
        Color32[] pixels32_2 = texture2D2.GetPixels32();
        for (int index = 0; index < num; ++index)
        {
          byte g = (byte) pixels32_2[index].g;
          if ((int) g < (int) minHeight)
            minHeight = g;
          if ((int) g > (int) maxHeight)
            maxHeight = g;
          pixels32_1[index].r = (__Null) (int) (pixels32_1[index].g = pixels32_1[index].b = (__Null) g);
        }
        if (Application.get_isPlaying())
          Object.Destroy((Object) texture2D2);
        else
          Object.DestroyImmediate((Object) texture2D2);
      }
      if (Object.op_Inequality((Object) aoTexture, (Object) null))
      {
        Texture2D texture2D2 = CTSProfile.ResizeTexture(aoTexture, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, false, false, false);
        Color32[] pixels32_2 = texture2D2.GetPixels32();
        for (int index = 0; index < num; ++index)
          pixels32_1[index].a = pixels32_2[index].g;
        if (Application.get_isPlaying())
          Object.Destroy((Object) texture2D2);
        else
          Object.DestroyImmediate((Object) texture2D2);
      }
      else
      {
        for (int index = 0; index < num; ++index)
          pixels32_1[index].a = (__Null) (int) byte.MaxValue;
      }
      texture2D1.SetPixels32(pixels32_1);
      texture2D1.Apply(true);
      if (this.m_albedoCompress)
      {
        texture2D1.Compress(true);
        texture2D1.Apply(true);
      }
      return texture2D1;
    }

    private Texture2D BakeNormal(Texture2D normalTexture)
    {
      int num = this.m_normalTextureSizePx * this.m_normalTextureSizePx;
      if (num == 0 || Object.op_Equality((Object) normalTexture, (Object) null))
        return (Texture2D) null;
      Texture2D texture2D1 = new Texture2D(this.m_normalTextureSizePx, this.m_normalTextureSizePx, this.m_normalFormat, true, true);
      ((Object) texture2D1).set_name("CTS_" + ((Object) this).get_name() + "_Normal");
      ((Texture) texture2D1).set_anisoLevel(this.m_normalAniso);
      ((Texture) texture2D1).set_filterMode(this.m_normalFilterMode);
      Color32[] pixels32_1 = texture2D1.GetPixels32();
      Texture2D texture2D2 = CTSProfile.ResizeTexture(normalTexture, this.m_normalFormat, this.m_normalAniso, this.m_normalTextureSizePx, this.m_normalTextureSizePx, false, true, false);
      Color32[] pixels32_2 = texture2D2.GetPixels32();
      for (int index = 0; index < num; ++index)
      {
        pixels32_1[index].r = (__Null) 128;
        pixels32_1[index].g = pixels32_2[index].g;
        pixels32_1[index].b = (__Null) 128;
        pixels32_1[index].a = pixels32_2[index].a;
      }
      if (Application.get_isPlaying())
        Object.Destroy((Object) texture2D2);
      else
        Object.DestroyImmediate((Object) texture2D2);
      texture2D1.SetPixels32(pixels32_1);
      texture2D1.Apply(true);
      if (this.m_normalCompress)
      {
        texture2D1.Compress(true);
        texture2D1.Apply(true);
      }
      return texture2D1;
    }

    private Texture2D BakeAlbedo(
      Texture2D albedoTexture,
      Texture2D smoothnessTexture,
      Texture2D roughnessTexture)
    {
      int num = this.m_normalTextureSizePx * this.m_normalTextureSizePx;
      if (num == 0)
        return (Texture2D) null;
      Texture2D texture2D1 = new Texture2D(this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, this.m_albedoFormat, true, false);
      ((Object) texture2D1).set_name("CTS_" + ((Object) this).get_name() + "_ASm");
      ((Texture) texture2D1).set_anisoLevel(this.m_albedoAniso);
      ((Texture) texture2D1).set_filterMode(this.m_albedoFilterMode);
      Color32[] pixels32_1 = texture2D1.GetPixels32();
      if (Object.op_Inequality((Object) albedoTexture, (Object) null))
      {
        Texture2D texture2D2 = CTSProfile.ResizeTexture(albedoTexture, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, false, false, false);
        Color32[] pixels32_2 = texture2D2.GetPixels32();
        for (int index = 0; index < num; ++index)
        {
          pixels32_1[index].r = pixels32_2[index].r;
          pixels32_1[index].g = pixels32_2[index].g;
          pixels32_1[index].b = pixels32_2[index].b;
        }
        if (Application.get_isPlaying())
          Object.Destroy((Object) texture2D2);
        else
          Object.DestroyImmediate((Object) texture2D2);
      }
      if (Object.op_Inequality((Object) roughnessTexture, (Object) null) && Object.op_Equality((Object) smoothnessTexture, (Object) null))
      {
        Texture2D texture2D2 = CTSProfile.ResizeTexture(roughnessTexture, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, false, false, false);
        Color32[] pixels32_2 = texture2D2.GetPixels32();
        for (int index = 0; index < num; ++index)
          pixels32_1[index].a = (__Null) (int) (byte) ((int) byte.MaxValue - pixels32_2[index].g);
        if (Application.get_isPlaying())
          Object.Destroy((Object) texture2D2);
        else
          Object.DestroyImmediate((Object) texture2D2);
      }
      if (Object.op_Inequality((Object) smoothnessTexture, (Object) null))
      {
        Texture2D texture2D2 = CTSProfile.ResizeTexture(smoothnessTexture, this.m_albedoFormat, this.m_albedoAniso, this.m_albedoTextureSizePx, this.m_albedoTextureSizePx, false, false, false);
        Color32[] pixels32_2 = texture2D2.GetPixels32();
        for (int index = 0; index < num; ++index)
          pixels32_1[index].a = pixels32_2[index].g;
        if (Application.get_isPlaying())
          Object.Destroy((Object) texture2D2);
        else
          Object.DestroyImmediate((Object) texture2D2);
      }
      texture2D1.SetPixels32(pixels32_1);
      texture2D1.Apply(true);
      if (this.m_albedoCompress)
      {
        texture2D1.Compress(true);
        texture2D1.Apply(true);
      }
      return texture2D1;
    }

    private void DebugTextureColorData(string name, Color32 color)
    {
      Debug.Log((object) string.Format("{0} - r{1} g{2} b{3} a{4}", (object) name, (object) (byte) color.r, (object) (byte) color.g, (object) (byte) color.b, (object) (byte) color.a));
    }

    private void SaveTexture(string path, Texture2D texture)
    {
      byte[] png = ImageConversion.EncodeToPNG(texture);
      File.WriteAllBytes(path + ".png", png);
    }

    public static Texture2D ResizeTexture(
      Texture2D texture,
      TextureFormat format,
      int aniso,
      int width,
      int height,
      bool mipmap,
      bool linear,
      bool compress)
    {
      RenderTexture renderTexture = !linear ? RenderTexture.GetTemporary(width, height, 0, (RenderTextureFormat) 7, (RenderTextureReadWrite) 2) : RenderTexture.GetTemporary(width, height, 0, (RenderTextureFormat) 7, (RenderTextureReadWrite) 1);
      bool sRgbWrite = GL.get_sRGBWrite();
      if (linear)
        GL.set_sRGBWrite(false);
      else
        GL.set_sRGBWrite(true);
      Graphics.Blit((Texture) texture, renderTexture);
      RenderTexture active = RenderTexture.get_active();
      RenderTexture.set_active(renderTexture);
      Texture2D texture2D = new Texture2D(width, height, format, mipmap, linear);
      ((Object) texture2D).set_name(((Object) texture).get_name() + " X");
      ((Texture) texture2D).set_anisoLevel(aniso);
      ((Texture) texture2D).set_filterMode(((Texture) texture).get_filterMode());
      ((Texture) texture2D).set_wrapMode(((Texture) texture).get_wrapMode());
      ((Texture) texture2D).set_mipMapBias(((Texture) texture).get_mipMapBias());
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) ((Texture) renderTexture).get_width(), (float) ((Texture) renderTexture).get_height()), 0, 0);
      texture2D.Apply(true);
      if (compress)
      {
        texture2D.Compress(true);
        texture2D.Apply(true);
      }
      RenderTexture.set_active(active);
      RenderTexture.ReleaseTemporary(renderTexture);
      GL.set_sRGBWrite(sRgbWrite);
      return texture2D;
    }

    private Texture2DArray GetTextureArray(
      List<Texture2D> sourceTextures,
      CTSConstants.TextureType textureType,
      int aniso)
    {
      if (sourceTextures == null)
        return (Texture2DArray) null;
      if (sourceTextures.Count == 0)
        return (Texture2DArray) null;
      Texture2D sourceTexture1 = sourceTextures[0];
      TextureFormat format = sourceTexture1.get_format();
      int width = ((Texture) sourceTexture1).get_width();
      int height = ((Texture) sourceTexture1).get_height();
      for (int index = 1; index < sourceTextures.Count; ++index)
      {
        if (((Texture) sourceTextures[index]).get_width() != width || ((Texture) sourceTextures[index]).get_height() != height)
        {
          Debug.Log((object) string.Format("GetTextureArray : {0} width {1} <> {2}, height {3} <> {4}", (object) ((Object) sourceTextures[index]).get_name(), (object) ((Texture) sourceTextures[index]).get_width(), (object) width, (object) ((Texture) sourceTextures[index]).get_height(), (object) height));
          return (Texture2DArray) null;
        }
      }
      Texture2DArray texture2Darray;
      switch (textureType)
      {
        case CTSConstants.TextureType.Albedo:
        case CTSConstants.TextureType.AmbientOcclusion:
        case CTSConstants.TextureType.Height:
          texture2Darray = new Texture2DArray(width, height, sourceTextures.Count, format, true, false);
          break;
        case CTSConstants.TextureType.Normal:
          texture2Darray = new Texture2DArray(width, height, sourceTextures.Count, format, true, true);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (textureType), (object) textureType, (string) null);
      }
      ((Texture) texture2Darray).set_filterMode(((Texture) sourceTexture1).get_filterMode());
      ((Texture) texture2Darray).set_wrapMode(((Texture) sourceTexture1).get_wrapMode());
      ((Texture) texture2Darray).set_anisoLevel(aniso);
      ((Texture) texture2Darray).set_mipMapBias(((Texture) sourceTexture1).get_mipMapBias());
      for (int index1 = 0; index1 < sourceTextures.Count; ++index1)
      {
        if (Object.op_Inequality((Object) sourceTextures[index1], (Object) null))
        {
          Texture2D sourceTexture2 = sourceTextures[index1];
          for (int index2 = 0; index2 < sourceTexture2.get_mipmapCount(); ++index2)
            Graphics.CopyTexture((Texture) sourceTexture2, 0, index2, (Texture) texture2Darray, index1, index2);
        }
      }
      texture2Darray.Apply(false);
      return texture2Darray;
    }

    public static bool IsDifferentTexture(Texture2D src, Texture2D target)
    {
      return Object.op_Equality((Object) src, (Object) null) ? Object.op_Inequality((Object) target, (Object) null) : Object.op_Equality((Object) target, (Object) null) || ((Object) src).GetInstanceID() != ((Object) target).GetInstanceID() || (((Texture) src).get_width() != ((Texture) target).get_width() || ((Texture) src).get_height() != ((Texture) target).get_height());
    }

    public void ConstructTerrainReplacementAlbedos()
    {
      if (Application.get_isPlaying())
        return;
      while (this.m_replacementTerrainAlbedos.Count > this.m_terrainTextures.Count)
        this.m_replacementTerrainAlbedos.RemoveAt(this.m_replacementTerrainAlbedos.Count - 1);
      while (this.m_replacementTerrainAlbedos.Count < this.m_terrainTextures.Count)
        this.m_replacementTerrainAlbedos.Add((Texture2D) null);
      string path1 = this.m_ctsDirectory + "Terrains/ReplacementTextures/";
      Directory.CreateDirectory(path1);
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_terrainTextures[index];
        if (Object.op_Inequality((Object) terrainTexture.Albedo, (Object) null))
        {
          string path2 = path1 + ((Object) terrainTexture.Albedo).get_name() + "_cts.png";
          if (!File.Exists(path2))
          {
            Texture2D texture2D = CTSProfile.ResizeTexture(terrainTexture.Albedo, this.m_albedoFormat, this.m_albedoAniso, 64, 64, false, true, false);
            ((Object) texture2D).set_name(((Object) terrainTexture.Albedo).get_name() + "_cts");
            this.m_replacementTerrainAlbedos[index] = texture2D;
            byte[] png = ImageConversion.EncodeToPNG(this.m_replacementTerrainAlbedos[index]);
            File.WriteAllBytes(path2, png);
          }
        }
        else
          this.m_replacementTerrainAlbedos[index] = (Texture2D) null;
      }
      CompleteTerrainShader.SetDirty((Object) this, false, true);
    }

    public void ConstructTerrainReplacementNormals()
    {
      if (Application.get_isPlaying())
        return;
      while (this.m_replacementTerrainNormals.Count > this.m_terrainTextures.Count)
        this.m_replacementTerrainNormals.RemoveAt(this.m_replacementTerrainNormals.Count - 1);
      while (this.m_replacementTerrainNormals.Count < this.m_terrainTextures.Count)
        this.m_replacementTerrainNormals.Add((Texture2D) null);
      string path1 = this.m_ctsDirectory + "Terrains/ReplacementTextures/";
      Directory.CreateDirectory(path1);
      for (int index = 0; index < this.m_terrainTextures.Count; ++index)
      {
        CTSTerrainTextureDetails terrainTexture = this.m_terrainTextures[index];
        if (Object.op_Inequality((Object) terrainTexture.Normal, (Object) null))
        {
          string path2 = path1 + ((Object) terrainTexture.Normal).get_name() + "_nrm_cts.png";
          if (!File.Exists(path2))
          {
            Texture2D texture2D = CTSProfile.ResizeTexture(terrainTexture.Normal, this.m_normalFormat, this.m_normalAniso, 64, 64, false, true, false);
            ((Object) texture2D).set_name(((Object) terrainTexture.Normal).get_name() + "_nrm_cts");
            this.m_replacementTerrainNormals[index] = texture2D;
            byte[] png = ImageConversion.EncodeToPNG(this.m_replacementTerrainNormals[index]);
            File.WriteAllBytes(path2, png);
          }
        }
        else
          this.m_replacementTerrainNormals[index] = (Texture2D) null;
      }
      CompleteTerrainShader.SetDirty((Object) this, false, true);
    }
  }
}
