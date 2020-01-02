// Decompiled with JetBrains decompiler
// Type: AIChara.ChaShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  [DefaultExecutionOrder(-5)]
  public static class ChaShader
  {
    public static readonly int MainTex = Shader.PropertyToID("_MainTex");
    public static readonly int Color = Shader.PropertyToID("_Color");
    public static readonly int Color2 = Shader.PropertyToID("_Color2");
    public static readonly int Color3 = Shader.PropertyToID("_Color3");
    public static readonly int Color4 = Shader.PropertyToID("_Color4");
    public static readonly int Specular = Shader.PropertyToID("_Specular");
    public static readonly int Gloss = Shader.PropertyToID("_Gloss");
    public static readonly int Metallic = Shader.PropertyToID("_Metallic");
    public static readonly int Smoothness = Shader.PropertyToID("_Smoothness");
    public static readonly int SkinTex = Shader.PropertyToID("_MainTex");
    public static readonly int SkinCreateDetailTex = Shader.PropertyToID("_DetailMainTex");
    public static readonly int SkinOcclusionMapTex = Shader.PropertyToID("_OcclusionMap");
    public static readonly int SkinNormalMapTex = Shader.PropertyToID("_BumpMap");
    public static readonly int SkinColor = Shader.PropertyToID("_Color");
    public static readonly int SkinDetailTex = Shader.PropertyToID("_BumpMap2");
    public static readonly int SkinDetailPower = Shader.PropertyToID("_BumpScale2");
    public static readonly int Paint01Tex = Shader.PropertyToID("_Texture5");
    public static readonly int Paint01Color = Shader.PropertyToID("_Color5");
    public static readonly int Paint01Gloass = Shader.PropertyToID("_Gloss5");
    public static readonly int Paint01Metallic = Shader.PropertyToID("_Metallic5");
    public static readonly int Paint01Layout = Shader.PropertyToID("_Texture5UV");
    public static readonly int Paint01Rot = Shader.PropertyToID("_Texture5Rotator");
    public static readonly int Paint02Tex = Shader.PropertyToID("_Texture6");
    public static readonly int Paint02Color = Shader.PropertyToID("_Color6");
    public static readonly int Paint02Gloass = Shader.PropertyToID("_Gloss6");
    public static readonly int Paint02Metallic = Shader.PropertyToID("_Metallic6");
    public static readonly int Paint02Layout = Shader.PropertyToID("_Texture6UV");
    public static readonly int Paint02Rot = Shader.PropertyToID("_Texture6Rotator");
    public static readonly int EyeshadowTex = Shader.PropertyToID("_Texture11");
    public static readonly int EyeshadowColor = Shader.PropertyToID("_Color11");
    public static readonly int EyeshadowGloss = Shader.PropertyToID("_Gloss11");
    public static readonly int CheekTex = Shader.PropertyToID("_Texture10");
    public static readonly int CheekColor = Shader.PropertyToID("_Color10");
    public static readonly int CheekGloss = Shader.PropertyToID("_Gloss10");
    public static readonly int LipTex = Shader.PropertyToID("_Texture9");
    public static readonly int LipColor = Shader.PropertyToID("_Color9");
    public static readonly int LipGloss = Shader.PropertyToID("_Gloss9");
    public static readonly int MoleTex = Shader.PropertyToID("_Texture12");
    public static readonly int MoleColor = Shader.PropertyToID("_Color12");
    public static readonly int MoleLayout = Shader.PropertyToID("_Texture12UV");
    public static readonly int EyebrowTex = Shader.PropertyToID("_Texture3");
    public static readonly int EyebrowColor = Shader.PropertyToID("_Color3");
    public static readonly int EyebrowLayout = Shader.PropertyToID("_Texture3UV");
    public static readonly int EyebrowTilt = Shader.PropertyToID("_Texture3Rotator");
    public static readonly int EyesWhiteColor = Shader.PropertyToID("_Color");
    public static readonly int PupilTex = Shader.PropertyToID("_Texture2");
    public static readonly int PupilLayout = Shader.PropertyToID("_texture2uv");
    public static readonly int PupilColor = Shader.PropertyToID("_Color2");
    public static readonly int PupilEmission = Shader.PropertyToID("_Emission");
    public static readonly int PupilBlackTex = Shader.PropertyToID("_Texture3");
    public static readonly int PupilBlackColor = Shader.PropertyToID("_Color3");
    public static readonly int PupilBlackLayout = Shader.PropertyToID("_texture3uv");
    public static readonly int EyesHighlightTex = Shader.PropertyToID("_Texture4");
    public static readonly int EyesHighlightColor = Shader.PropertyToID("_Color4");
    public static readonly int EyesHighlightLayout = Shader.PropertyToID("_Texture4UV");
    public static readonly int EyesHighlightTilt = Shader.PropertyToID("_Texture4Rotator");
    public static readonly int EyesShadowRange = Shader.PropertyToID("_ShadowScale");
    public static readonly int EyelashesTex = Shader.PropertyToID("_MainTex");
    public static readonly int EyelashesColor = Shader.PropertyToID("_Color");
    public static readonly int BeardTex = Shader.PropertyToID("_Texture5");
    public static readonly int BeardColor = Shader.PropertyToID("_Color5");
    public static readonly int EyesHighlightOnOff = Shader.PropertyToID("_Smoothness");
    public static readonly int HohoAka = Shader.PropertyToID("_Texture4Scale");
    public static readonly int SunburnTex = Shader.PropertyToID("_Texture7");
    public static readonly int SunburnColor = Shader.PropertyToID("_Color7");
    public static readonly int NipTex = Shader.PropertyToID("_Texture2");
    public static readonly int NipColor = Shader.PropertyToID("_Color2");
    public static readonly int NipGloss = Shader.PropertyToID("_NipGloss");
    public static readonly int NipScale = Shader.PropertyToID("_NipScale");
    public static readonly int UnderhairTex = Shader.PropertyToID("_Texture3");
    public static readonly int UnderhairColor = Shader.PropertyToID("_Color3");
    public static readonly int NailColor = Shader.PropertyToID("_Color13");
    public static readonly int NailGloss = Shader.PropertyToID("_NailGloss");
    public static readonly int AlphaMask = Shader.PropertyToID("_AlphaMask");
    public static readonly int AlphaMask2 = Shader.PropertyToID("_AlphaMask2");
    public static readonly int alpha_a = Shader.PropertyToID("_alpha_a");
    public static readonly int alpha_b = Shader.PropertyToID("_alpha_b");
    public static readonly int alpha_c = Shader.PropertyToID("_alpha_c");
    public static readonly int alpha_d = Shader.PropertyToID("_alpha_d");
    public static readonly int HairMainColor = Shader.PropertyToID("_Color");
    public static readonly int HairTopColor = Shader.PropertyToID("_TopColor");
    public static readonly int HairUnderColor = Shader.PropertyToID("_UnderColor");
    public static readonly int HairRingoff = Shader.PropertyToID("_Ringoff");
    public static readonly int DetailMainTex = Shader.PropertyToID("_DetailMainTex");
    public static readonly int ColorMask = Shader.PropertyToID("_ColorMask");
    public static readonly int PatternMask1 = Shader.PropertyToID("_PatternMask1");
    public static readonly int PatternMask2 = Shader.PropertyToID("_PatternMask2");
    public static readonly int PatternMask3 = Shader.PropertyToID("_PatternMask3");
    public static readonly int Color1_2 = Shader.PropertyToID("_Color1_2");
    public static readonly int Color2_2 = Shader.PropertyToID("_Color2_2");
    public static readonly int Color3_2 = Shader.PropertyToID("_Color3_2");
    public static readonly int ClothesGloss1 = Shader.PropertyToID("_Glossiness");
    public static readonly int ClothesGloss2 = Shader.PropertyToID("_Glossiness2");
    public static readonly int ClothesGloss3 = Shader.PropertyToID("_Glossiness3");
    public static readonly int ClothesGloss4 = Shader.PropertyToID("_Glossiness4");
    public static readonly int Metallic2 = Shader.PropertyToID("_Metallic2");
    public static readonly int Metallic3 = Shader.PropertyToID("_Metallic3");
    public static readonly int Metallic4 = Shader.PropertyToID("_Metallic4");
    public static readonly int patternuv1 = Shader.PropertyToID("_patternuv1");
    public static readonly int patternuv2 = Shader.PropertyToID("_patternuv2");
    public static readonly int patternuv3 = Shader.PropertyToID("_patternuv3");
    public static readonly int patternuv1Rotator = Shader.PropertyToID("_patternuv1Rotator");
    public static readonly int patternuv2Rotator = Shader.PropertyToID("_patternuv2Rotator");
    public static readonly int patternuv3Rotator = Shader.PropertyToID("_patternuv3Rotator");
    public static readonly int uvScalePattern = Shader.PropertyToID("_UVScalePattern");
    public static readonly int ClothesBreak = Shader.PropertyToID("_AlphaEx");
    public static readonly int siruFrontTop = Shader.PropertyToID("_WeatheringRange1");
    public static readonly int siruFrontBot = Shader.PropertyToID("_WeatheringRange2");
    public static readonly int siruBackTop = Shader.PropertyToID("_WeatheringRange3");
    public static readonly int siruBackBot = Shader.PropertyToID("_WeatheringRange4");
    public static readonly int siruFace = Shader.PropertyToID("_WeatheringRange6");
    public static readonly int tearsRate = Shader.PropertyToID("_NamidaScale");
    public static readonly int wetRate = Shader.PropertyToID("_ExGloss");
  }
}
