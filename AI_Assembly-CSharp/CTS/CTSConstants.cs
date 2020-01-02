// Decompiled with JetBrains decompiler
// Type: CTS.CTSConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace CTS
{
  public static class CTSConstants
  {
    public static readonly int MajorVersion = 1;
    public static readonly int MinorVersion = 8;
    public static readonly string CTSPresentSymbol = "CTS_PRESENT";
    public const string CTSShaderName = "CTS/CTS Terrain";
    public const string CTSShaderMeshBlenderName = "CTS/CTS_Model_Blend";
    public const string CTSShaderMeshBlenderAdvancedName = "CTS/CTS_Model_Blend_Advanced";
    public const string CTSShaderLiteName = "CTS/CTS Terrain Shader Lite";
    public const string CTSShaderBasicName = "CTS/CTS Terrain Shader Basic";
    public const string CTSShaderBasicCutoutName = "CTS/CTS Terrain Shader Basic CutOut";
    public const string CTSShaderAdvancedName = "CTS/CTS Terrain Shader Advanced";
    public const string CTSShaderAdvancedCutoutName = "CTS/CTS Terrain Shader Advanced CutOut";
    public const string CTSShaderTesselatedName = "CTS/CTS Terrain Shader Advanced Tess";
    public const string CTSShaderTesselatedCutoutName = "CTS/CTS Terrain Shader Advanced Tess CutOut";

    public static int GetTextureSize(CTSConstants.TextureSize size)
    {
      switch (size)
      {
        case CTSConstants.TextureSize.Texture_64:
          return 64;
        case CTSConstants.TextureSize.Texture_128:
          return 128;
        case CTSConstants.TextureSize.Texture_256:
          return 256;
        case CTSConstants.TextureSize.Texture_512:
          return 512;
        case CTSConstants.TextureSize.Texture_1024:
          return 1024;
        case CTSConstants.TextureSize.Texture_2048:
          return 2048;
        case CTSConstants.TextureSize.Texture_4096:
          return 4096;
        case CTSConstants.TextureSize.Texture_8192:
          return 8192;
        default:
          return 0;
      }
    }

    public enum ShaderType
    {
      Unity,
      Basic,
      Advanced,
      Tesselation,
      Lite,
    }

    public enum ShaderMode
    {
      DesignTime,
      RunTime,
    }

    public enum AOType
    {
      None,
      NormalMapBased,
      TextureBased,
    }

    public enum TextureSize
    {
      Texture_64,
      Texture_128,
      Texture_256,
      Texture_512,
      Texture_1024,
      Texture_2048,
      Texture_4096,
      Texture_8192,
    }

    public enum TextureType
    {
      Albedo,
      Normal,
      AmbientOcclusion,
      Height,
      Splat,
      Emission,
    }

    public enum TextureChannel
    {
      R,
      G,
      B,
      A,
    }

    [Flags]
    public enum TerrainChangedFlags
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
