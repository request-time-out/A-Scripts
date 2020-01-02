// Decompiled with JetBrains decompiler
// Type: Studio.ScreenEffectDefine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public static class ScreenEffectDefine
  {
    public static int ColorGradingLookupTexture = 0;
    public static float ColorGradingBlend = 0.0f;
    public static int ColorGradingSaturation = 15;
    public static int ColorGradingBrightness = -20;
    public static int ColorGradingContrast = 40;
    public static bool AmbientOcclusion = true;
    public static float AmbientOcclusionIntensity = 0.2f;
    public static float AmbientOcclusionThicknessModeifier = 2f;
    [ColorUsage(false, true)]
    public static Color AmbientOcclusionColor = Utility.ConvertColor(0, 0, 0);
    public static bool Bloom = true;
    public static float BloomIntensity = 2f;
    public static float BloomThreshold = 1f;
    public static float BloomSoftKnee = 0.76f;
    public static bool BloomClamp = true;
    public static float BloomDiffusion = 7f;
    public static Color BloomColor = Utility.ConvertColor(191, 191, 191);
    public static bool Vignette = true;
    public static bool ScreenSpaceReflections = true;
    public static bool ReflectionProbe = false;
    public static int ReflectionProbeCubemap = 0;
    public static float ReflectionProbeIntensity = 1f;
    public static bool Fog = false;
    public static bool FogExcludeFarPixels = false;
    public static float FogHeight = 50f;
    public static float FogHeightDensity = 0.01f;
    public static Color FogColor = Utility.ConvertColor(138, 168, 203);
    public static float FogDensity = 0.0005f;
    public static bool DepthOfField = false;
    public static int DepthOfFieldForcus = -1;
    public static float DepthOfFieldFocalSize = 0.4f;
    public static float DepthOfFieldAperture = 0.6f;
    public static bool SunShaft = false;
    public static int SunShaftCaster = -1;
    public static Color SunShaftThresholdColor = Utility.ConvertColor(128, 128, 128);
    public static Color SunShaftShaftsColor = Utility.ConvertColor((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static float SunShaftDistanceFallOff = 0.75f;
    public static float SunShaftBlurSize = 5f;
    public static float SunShaftIntensity = 5f;
    public static Color EnvironmentLightingSkyColor = Utility.ConvertColor(170, 188, 243);
    public static Color EnvironmentLightingEquatorColor = Utility.ConvertColor(185, 195, 205);
    public static Color EnvironmentLightingGroundColor = Utility.ConvertColor(204, 109, 41);
  }
}
