// Decompiled with JetBrains decompiler
// Type: Studio.ItemShader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  [DefaultExecutionOrder(-5)]
  public static class ItemShader
  {
    static ItemShader()
    {
      ItemShader._Color = Shader.PropertyToID(nameof (_Color));
      ItemShader._Color2 = Shader.PropertyToID(nameof (_Color2));
      ItemShader._Color3 = Shader.PropertyToID(nameof (_Color3));
      ItemShader._Color4 = Shader.PropertyToID(nameof (_Color4));
      ItemShader._Color1_2 = Shader.PropertyToID(nameof (_Color1_2));
      ItemShader._Color2_2 = Shader.PropertyToID(nameof (_Color2_2));
      ItemShader._Color3_2 = Shader.PropertyToID(nameof (_Color3_2));
      ItemShader._PatternMask1 = Shader.PropertyToID(nameof (_PatternMask1));
      ItemShader._PatternMask2 = Shader.PropertyToID(nameof (_PatternMask2));
      ItemShader._PatternMask3 = Shader.PropertyToID(nameof (_PatternMask3));
      ItemShader._patternuv1 = Shader.PropertyToID(nameof (_patternuv1));
      ItemShader._patternuv2 = Shader.PropertyToID(nameof (_patternuv2));
      ItemShader._patternuv3 = Shader.PropertyToID(nameof (_patternuv3));
      ItemShader._patternuv1Rotator = Shader.PropertyToID(nameof (_patternuv1Rotator));
      ItemShader._patternuv2Rotator = Shader.PropertyToID(nameof (_patternuv2Rotator));
      ItemShader._patternuv3Rotator = Shader.PropertyToID(nameof (_patternuv3Rotator));
      ItemShader._patternclamp1 = Shader.PropertyToID(nameof (_patternclamp1));
      ItemShader._patternclamp2 = Shader.PropertyToID(nameof (_patternclamp2));
      ItemShader._patternclamp3 = Shader.PropertyToID(nameof (_patternclamp3));
      ItemShader._alpha = Shader.PropertyToID(nameof (_alpha));
      ItemShader._EmissionColor = Shader.PropertyToID(nameof (_EmissionColor));
      ItemShader._EmissionStrength = Shader.PropertyToID(nameof (_EmissionStrength));
      ItemShader._LightCancel = Shader.PropertyToID(nameof (_LightCancel));
      ItemShader._MainTex = Shader.PropertyToID(nameof (_MainTex));
      ItemShader._Metallic = Shader.PropertyToID(nameof (_Metallic));
      ItemShader._Metallic2 = Shader.PropertyToID(nameof (_Metallic2));
      ItemShader._Metallic3 = Shader.PropertyToID(nameof (_Metallic3));
      ItemShader._Metallic4 = Shader.PropertyToID(nameof (_Metallic4));
      ItemShader._Glossiness = Shader.PropertyToID(nameof (_Glossiness));
      ItemShader._Glossiness2 = Shader.PropertyToID(nameof (_Glossiness2));
      ItemShader._Glossiness3 = Shader.PropertyToID(nameof (_Glossiness3));
      ItemShader._Glossiness4 = Shader.PropertyToID(nameof (_Glossiness4));
      ItemShader._UsesWaterVolume = Shader.PropertyToID(nameof (_UsesWaterVolume));
    }

    public static int _Color { get; private set; }

    public static int _Color2 { get; private set; }

    public static int _Color3 { get; private set; }

    public static int _Color4 { get; private set; }

    public static int _PatternMask1 { get; private set; }

    public static int _PatternMask2 { get; private set; }

    public static int _PatternMask3 { get; private set; }

    public static int _Color1_2 { get; private set; }

    public static int _Color2_2 { get; private set; }

    public static int _Color3_2 { get; private set; }

    public static int _patternuv1 { get; private set; }

    public static int _patternuv2 { get; private set; }

    public static int _patternuv3 { get; private set; }

    public static int _patternuv1Rotator { get; private set; }

    public static int _patternuv2Rotator { get; private set; }

    public static int _patternuv3Rotator { get; private set; }

    public static int _patternclamp1 { get; private set; }

    public static int _patternclamp2 { get; private set; }

    public static int _patternclamp3 { get; private set; }

    public static int _alpha { get; private set; }

    public static int _EmissionColor { get; private set; }

    public static int _EmissionStrength { get; private set; }

    public static int _LightCancel { get; private set; }

    public static int _MainTex { get; private set; }

    public static int _Metallic { get; private set; }

    public static int _Metallic2 { get; private set; }

    public static int _Metallic3 { get; private set; }

    public static int _Metallic4 { get; private set; }

    public static int _Glossiness { get; private set; }

    public static int _Glossiness2 { get; private set; }

    public static int _Glossiness3 { get; private set; }

    public static int _Glossiness4 { get; private set; }

    public static int _UsesWaterVolume { get; private set; }
  }
}
