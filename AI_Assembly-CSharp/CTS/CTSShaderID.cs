// Decompiled with JetBrains decompiler
// Type: CTS.CTSShaderID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CTS
{
  public static class CTSShaderID
  {
    public static readonly int Texture_Array_Albedo = Shader.PropertyToID("_Texture_Array_Albedo");
    public static readonly int Texture_Array_Normal = Shader.PropertyToID("_Texture_Array_Normal");
    public static readonly int Texture_Splat_1 = Shader.PropertyToID("_Texture_Splat_1");
    public static readonly int Texture_Splat_2 = Shader.PropertyToID("_Texture_Splat_2");
    public static readonly int Texture_Splat_3 = Shader.PropertyToID("_Texture_Splat_3");
    public static readonly int Texture_Splat_4 = Shader.PropertyToID("_Texture_Splat_4");
    public static readonly int UV_Mix_Power = Shader.PropertyToID("_UV_Mix_Power");
    public static readonly int UV_Mix_Start_Distance = Shader.PropertyToID("_UV_Mix_Start_Distance");
    public static readonly int Perlin_Normal_Tiling_Close = Shader.PropertyToID("_Perlin_Normal_Tiling_Close");
    public static readonly int Perlin_Normal_Tiling_Far = Shader.PropertyToID("_Perlin_Normal_Tiling_Far");
    public static readonly int Perlin_Normal_Power = Shader.PropertyToID("_Perlin_Normal_Power");
    public static readonly int Perlin_Normal_Power_Close = Shader.PropertyToID("_Perlin_Normal_Power_Close");
    public static readonly int Terrain_Smoothness = Shader.PropertyToID("_Terrain_Smoothness");
    public static readonly int Terrain_Specular = Shader.PropertyToID("_Terrain_Specular");
    public static readonly int TessValue = Shader.PropertyToID("_TessValue");
    public static readonly int TessMin = Shader.PropertyToID("_TessMin");
    public static readonly int TessMax = Shader.PropertyToID("_TessMax");
    public static readonly int TessPhongStrength = Shader.PropertyToID("_TessPhongStrength");
    public static readonly int TessDistance = Shader.PropertyToID("_TessDistance");
    public static readonly int Ambient_Occlusion_Type = Shader.PropertyToID("_Ambient_Occlusion_Type");
    public static readonly int Remove_Vert_Height = Shader.PropertyToID("_Remove_Vert_Height");
    public static readonly int Texture_Additional_Masks = Shader.PropertyToID("_Texture_Additional_Masks");
    public static readonly int Use_AO = Shader.PropertyToID("_Use_AO");
    public static readonly int Use_AO_Texture = Shader.PropertyToID("_Use_AO_Texture");
    public static readonly int Ambient_Occlusion_Power = Shader.PropertyToID("_Ambient_Occlusion_Power");
    public static readonly int Texture_Perlin_Normal_Index = Shader.PropertyToID("_Texture_Perlin_Normal_Index");
    public static readonly int Global_Normalmap_Power = Shader.PropertyToID("_Global_Normalmap_Power");
    public static readonly int Global_Normal_Map = Shader.PropertyToID("_Global_Normal_Map");
    public static readonly int Global_Color_Map_Far_Power = Shader.PropertyToID("_Global_Color_Map_Far_Power");
    public static readonly int Global_Color_Map_Close_Power = Shader.PropertyToID("_Global_Color_Map_Close_Power");
    public static readonly int Global_Color_Opacity_Power = Shader.PropertyToID("_Global_Color_Opacity_Power");
    public static readonly int Global_Color_Map = Shader.PropertyToID("_Global_Color_Map");
    public static readonly int Geological_Map_Offset_Close = Shader.PropertyToID("_Geological_Map_Offset_Close");
    public static readonly int Geological_Map_Close_Power = Shader.PropertyToID("_Geological_Map_Close_Power");
    public static readonly int Geological_Tiling_Close = Shader.PropertyToID("_Geological_Tiling_Close");
    public static readonly int Geological_Map_Offset_Far = Shader.PropertyToID("_Geological_Map_Offset_Far");
    public static readonly int Geological_Map_Far_Power = Shader.PropertyToID("_Geological_Map_Far_Power");
    public static readonly int Geological_Tiling_Far = Shader.PropertyToID("_Geological_Tiling_Far");
    public static readonly int Texture_Geological_Map = Shader.PropertyToID("_Texture_Geological_Map");
    public static readonly int Texture_Snow_Index = Shader.PropertyToID("_Texture_Snow_Index");
    public static readonly int Texture_Snow_Normal_Index = Shader.PropertyToID("_Texture_Snow_Normal_Index");
    public static readonly int Texture_Snow_H_AO_Index = Shader.PropertyToID("_Texture_Snow_H_AO_Index");
    public static readonly int Snow_Amount = Shader.PropertyToID("_Snow_Amount");
    public static readonly int Snow_Maximum_Angle = Shader.PropertyToID("_Snow_Maximum_Angle");
    public static readonly int Snow_Maximum_Angle_Hardness = Shader.PropertyToID("_Snow_Maximum_Angle_Hardness");
    public static readonly int Snow_Min_Height = Shader.PropertyToID("_Snow_Min_Height");
    public static readonly int Snow_Min_Height_Blending = Shader.PropertyToID("_Snow_Min_Height_Blending");
    public static readonly int Snow_Noise_Power = Shader.PropertyToID("_Snow_Noise_Power");
    public static readonly int Snow_Noise_Tiling = Shader.PropertyToID("_Snow_Noise_Tiling");
    public static readonly int Snow_Normal_Scale = Shader.PropertyToID("_Snow_Normal_Scale");
    public static readonly int Snow_Perlin_Power = Shader.PropertyToID("_Snow_Perlin_Power");
    public static readonly int Snow_Tiling = Shader.PropertyToID("_Snow_Tiling");
    public static readonly int Snow_Tiling_Far_Multiplier = Shader.PropertyToID("_Snow_Tiling_Far_Multiplier");
    public static readonly int Snow_Brightness = Shader.PropertyToID("_Snow_Brightness");
    public static readonly int Snow_Blend_Normal = Shader.PropertyToID("_Snow_Blend_Normal");
    public static readonly int Snow_Smoothness = Shader.PropertyToID("_Snow_Smoothness");
    public static readonly int Snow_Specular = Shader.PropertyToID("_Snow_Specular");
    public static readonly int Snow_Heightblend_Close = Shader.PropertyToID("_Snow_Heightblend_Close");
    public static readonly int Snow_Heightblend_Far = Shader.PropertyToID("_Snow_Heightblend_Far");
    public static readonly int Snow_Height_Contrast = Shader.PropertyToID("_Snow_Height_Contrast");
    public static readonly int Snow_Heightmap_Depth = Shader.PropertyToID("_Snow_Heightmap_Depth");
    public static readonly int Snow_Heightmap_MinHeight = Shader.PropertyToID("_Snow_Heightmap_MinHeight");
    public static readonly int Snow_Heightmap_MaxHeight = Shader.PropertyToID("_Snow_Heightmap_MaxHeight");
    public static readonly int Snow_Ambient_Occlusion_Power = Shader.PropertyToID("_Snow_Ambient_Occlusion_Power");
    public static readonly int Snow_Tesselation_Depth = Shader.PropertyToID("_Snow_Tesselation_Depth");
    public static readonly int Snow_Color = Shader.PropertyToID("_Snow_Color");
    public static readonly int Texture_Snow_Average = Shader.PropertyToID("_Texture_Snow_Average");
    public static readonly int Texture_Glitter = Shader.PropertyToID("_Texture_Glitter");
    public static readonly int Glitter_Color_Power = Shader.PropertyToID("_Gliter_Color_Power");
    public static readonly int Glitter_Noise_Threshold = Shader.PropertyToID("_Glitter_Noise_Treshold");
    public static readonly int Glitter_Specular = Shader.PropertyToID("_Glitter_Specular");
    public static readonly int Glitter_Smoothness = Shader.PropertyToID("_Glitter_Smoothness");
    public static readonly int Glitter_Refreshing_Speed = Shader.PropertyToID("_Glitter_Refreshing_Speed");
    public static readonly int Glitter_Tiling = Shader.PropertyToID("_Glitter_Tiling");
    public static readonly int[] Texture_X_Albedo_Index = new int[16];
    public static readonly int[] Texture_X_Normal_Index = new int[16];
    public static readonly int[] Texture_X_H_AO_Index = new int[16];
    public static readonly int[] Texture_X_Tiling = new int[16];
    public static readonly int[] Texture_X_Far_Multiplier = new int[16];
    public static readonly int[] Texture_X_Perlin_Power = new int[16];
    public static readonly int[] Texture_X_Snow_Reduction = new int[16];
    public static readonly int[] Texture_X_Geological_Power = new int[16];
    public static readonly int[] Texture_X_Heightmap_Depth = new int[16];
    public static readonly int[] Texture_X_Height_Contrast = new int[16];
    public static readonly int[] Texture_X_Heightblend_Close = new int[16];
    public static readonly int[] Texture_X_Heightblend_Far = new int[16];
    public static readonly int[] Texture_X_Tesselation_Depth = new int[16];
    public static readonly int[] Texture_X_Heightmap_MinHeight = new int[16];
    public static readonly int[] Texture_X_Heightmap_MaxHeight = new int[16];
    public static readonly int[] Texture_X_AO_Power = new int[16];
    public static readonly int[] Texture_X_Normal_Power = new int[16];
    public static readonly int[] Texture_X_Triplanar = new int[16];
    public static readonly int[] Texture_X_Average = new int[16];
    public static readonly int[] Texture_X_Color = new int[16];

    static CTSShaderID()
    {
      for (int index = 1; index <= 16; ++index)
      {
        CTSShaderID.Texture_X_Albedo_Index[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Albedo_Index", (object) index));
        CTSShaderID.Texture_X_Normal_Index[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Normal_Index", (object) index));
        CTSShaderID.Texture_X_H_AO_Index[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_H_AO_Index", (object) index));
        CTSShaderID.Texture_X_Tiling[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Tiling", (object) index));
        CTSShaderID.Texture_X_Far_Multiplier[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Far_Multiplier", (object) index));
        CTSShaderID.Texture_X_Perlin_Power[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Perlin_Power", (object) index));
        CTSShaderID.Texture_X_Snow_Reduction[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Snow_Reduction", (object) index));
        CTSShaderID.Texture_X_Geological_Power[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Geological_Power", (object) index));
        CTSShaderID.Texture_X_Heightmap_Depth[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Heightmap_Depth", (object) index));
        CTSShaderID.Texture_X_Height_Contrast[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Height_Contrast", (object) index));
        CTSShaderID.Texture_X_Heightblend_Close[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Heightblend_Close", (object) index));
        CTSShaderID.Texture_X_Heightblend_Far[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Heightblend_Far", (object) index));
        CTSShaderID.Texture_X_Tesselation_Depth[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Tesselation_Depth", (object) index));
        CTSShaderID.Texture_X_Heightmap_MinHeight[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Heightmap_MinHeight", (object) index));
        CTSShaderID.Texture_X_Heightmap_MaxHeight[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Heightmap_MaxHeight", (object) index));
        CTSShaderID.Texture_X_AO_Power[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_AO_Power", (object) index));
        CTSShaderID.Texture_X_Normal_Power[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Normal_Power", (object) index));
        CTSShaderID.Texture_X_Triplanar[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Triplanar", (object) index));
        CTSShaderID.Texture_X_Average[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Average", (object) index));
        CTSShaderID.Texture_X_Color[index - 1] = Shader.PropertyToID(string.Format("_Texture_{0}_Color", (object) index));
      }
    }
  }
}
