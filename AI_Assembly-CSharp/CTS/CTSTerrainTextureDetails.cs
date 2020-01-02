// Decompiled with JetBrains decompiler
// Type: CTS.CTSTerrainTextureDetails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace CTS
{
  [Serializable]
  public class CTSTerrainTextureDetails
  {
    public string m_name = "Texture";
    public float m_detailPower = 1f;
    public float m_geologicalPower = 1f;
    public Color m_tint = new Color(1f, 1f, 1f);
    public float m_tintBrightness = 1f;
    public float m_smoothness = 1f;
    public int m_albedoIdx = -1;
    public float m_albedoTilingClose = 15f;
    public float m_albedoTilingFar = 3f;
    public int m_normalIdx = -1;
    public float m_normalStrength = 1f;
    public int m_heightIdx = -1;
    public float m_heightDepth = 8f;
    public float m_heightContrast = 1f;
    public float m_heightBlendClose = 1f;
    public float m_heightBlendFar = 1f;
    public float m_heightMax = 1f;
    public int m_aoIdx = -1;
    public float m_aoPower = 1f;
    public int m_emissionIdx = -1;
    public float m_emissionStrength = 1f;
    public bool m_isOpenInEditor;
    public int m_textureIdx;
    public float m_snowReductionPower;
    public bool m_triplanar;
    [NonSerialized]
    public bool m_albedoWasChanged;
    public Vector4 m_albedoAverage;
    [SerializeField]
    private Texture2D m_albedoTexture;
    [NonSerialized]
    public bool m_smoothnessWasChanged;
    [SerializeField]
    private Texture2D m_smoothnessTexture;
    [NonSerialized]
    public bool m_roughnessWasChanged;
    [SerializeField]
    private Texture2D m_roughnessTexture;
    [NonSerialized]
    public bool m_normalWasChanged;
    [SerializeField]
    private Texture2D m_normalTexture;
    public float m_heightTesselationDepth;
    public float m_heightMin;
    [NonSerialized]
    public bool m_heightWasChanged;
    [SerializeField]
    private Texture2D m_heightTexture;
    [NonSerialized]
    public bool m_aoWasChanged;
    [SerializeField]
    private Texture2D m_aoTexture;
    [NonSerialized]
    public bool m_emissionWasChanged;
    [SerializeField]
    private Texture2D m_emissionTexture;

    public CTSTerrainTextureDetails()
    {
    }

    public CTSTerrainTextureDetails(CTSTerrainTextureDetails src)
    {
      this.m_isOpenInEditor = src.m_isOpenInEditor;
      this.m_textureIdx = src.m_textureIdx;
      this.m_name = src.m_name;
      this.m_detailPower = src.m_detailPower;
      this.m_snowReductionPower = src.m_snowReductionPower;
      this.m_geologicalPower = src.m_geologicalPower;
      this.m_triplanar = src.m_triplanar;
      this.m_tint = src.m_tint;
      this.m_tintBrightness = src.m_tintBrightness;
      this.m_smoothness = src.m_smoothness;
      this.m_albedoIdx = src.m_albedoIdx;
      this.m_albedoTilingClose = src.m_albedoTilingClose;
      this.m_albedoTilingFar = src.m_albedoTilingFar;
      this.m_albedoWasChanged = src.m_albedoWasChanged;
      this.m_albedoTexture = src.m_albedoTexture;
      this.m_normalIdx = src.m_normalIdx;
      this.m_normalStrength = src.m_normalStrength;
      this.m_normalWasChanged = src.m_normalWasChanged;
      this.m_normalTexture = src.m_normalTexture;
      this.m_heightIdx = src.m_heightIdx;
      this.m_heightDepth = src.m_heightDepth;
      this.m_heightTesselationDepth = src.m_heightTesselationDepth;
      this.m_heightContrast = src.m_heightContrast;
      this.m_heightBlendClose = src.m_heightBlendClose;
      this.m_heightBlendFar = src.m_heightBlendFar;
      this.m_heightWasChanged = src.m_heightWasChanged;
      this.m_heightTexture = src.m_heightTexture;
      this.m_aoIdx = src.m_aoIdx;
      this.m_aoPower = src.m_aoPower;
      this.m_aoWasChanged = src.m_aoWasChanged;
      this.m_aoTexture = src.m_aoTexture;
      this.m_emissionIdx = src.m_emissionIdx;
      this.m_emissionStrength = src.m_emissionStrength;
      this.m_emissionWasChanged = src.m_emissionWasChanged;
      this.m_emissionTexture = src.m_emissionTexture;
      this.m_smoothness = src.m_smoothness;
      this.m_roughnessTexture = src.m_roughnessTexture;
    }

    public Texture2D Albedo
    {
      get
      {
        return this.m_albedoTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_albedoTexture, value))
          return;
        this.m_albedoTexture = value;
        this.m_albedoWasChanged = true;
        if (Object.op_Inequality((Object) this.m_albedoTexture, (Object) null))
          this.m_name = ((Object) this.m_albedoTexture).get_name();
        else
          this.m_name = "Missing Albedo";
      }
    }

    public Texture2D Smoothness
    {
      get
      {
        return this.m_smoothnessTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_smoothnessTexture, value))
          return;
        this.m_smoothnessTexture = value;
        this.m_smoothnessWasChanged = true;
      }
    }

    public Texture2D Roughness
    {
      get
      {
        return this.m_roughnessTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_roughnessTexture, value))
          return;
        this.m_roughnessTexture = value;
        this.m_roughnessWasChanged = true;
      }
    }

    public Texture2D Normal
    {
      get
      {
        return this.m_normalTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_normalTexture, value))
          return;
        this.m_normalTexture = value;
        this.m_normalWasChanged = true;
      }
    }

    public Texture2D Height
    {
      get
      {
        return this.m_heightTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_heightTexture, value))
          return;
        this.m_heightTexture = value;
        this.m_heightWasChanged = true;
      }
    }

    public Texture2D AmbientOcclusion
    {
      get
      {
        return this.m_aoTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_aoTexture, value))
          return;
        this.m_aoTexture = value;
        this.m_aoWasChanged = true;
      }
    }

    public Texture2D Emission
    {
      get
      {
        return this.m_emissionTexture;
      }
      set
      {
        if (!CTSProfile.IsDifferentTexture(this.m_emissionTexture, value))
          return;
        this.m_emissionTexture = value;
        this.m_emissionWasChanged = true;
      }
    }

    public void ResetChangedFlags()
    {
      this.m_albedoWasChanged = false;
      this.m_normalWasChanged = false;
      this.m_heightWasChanged = false;
      this.m_aoWasChanged = false;
      this.m_emissionWasChanged = false;
    }

    public bool TextureHasChanged()
    {
      return this.m_albedoWasChanged || this.m_normalWasChanged || (this.m_heightWasChanged || this.m_aoWasChanged) || this.m_emissionWasChanged;
    }
  }
}
