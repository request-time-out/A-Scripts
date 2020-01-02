// Decompiled with JetBrains decompiler
// Type: EnviroWeatherPresetCreation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroWeatherPresetCreation
{
  public static GameObject GetAssetPrefab(string name)
  {
    return (GameObject) null;
  }

  public static Cubemap GetAssetCubemap(string name)
  {
    return (Cubemap) null;
  }

  public static Texture GetAssetTexture(string name)
  {
    return (Texture) null;
  }

  public static Gradient CreateGradient()
  {
    Gradient gradient = new Gradient();
    GradientColorKey[] gradientColorKeyArray = new GradientColorKey[2];
    GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[2];
    gradientColorKeyArray[0].color = (__Null) Color.get_white();
    gradientColorKeyArray[0].time = (__Null) 0.0;
    gradientColorKeyArray[1].color = (__Null) Color.get_white();
    gradientColorKeyArray[1].time = (__Null) 0.0;
    gradientAlphaKeyArray[0].alpha = (__Null) 0.0;
    gradientAlphaKeyArray[0].time = (__Null) 0.0;
    gradientAlphaKeyArray[1].alpha = (__Null) 0.0;
    gradientAlphaKeyArray[1].time = (__Null) 1.0;
    gradient.SetKeys(gradientColorKeyArray, gradientAlphaKeyArray);
    return gradient;
  }

  public static Color GetColor(string hex)
  {
    Color color = (Color) null;
    ColorUtility.TryParseHtmlString(hex, ref color);
    return color;
  }

  public static Keyframe CreateKey(float value, float time)
  {
    Keyframe keyframe = (Keyframe) null;
    ((Keyframe) ref keyframe).set_value(value);
    ((Keyframe) ref keyframe).set_time(time);
    return keyframe;
  }

  public static Keyframe CreateKey(
    float value,
    float time,
    float inTangent,
    float outTangent)
  {
    Keyframe keyframe = (Keyframe) null;
    ((Keyframe) ref keyframe).set_value(value);
    ((Keyframe) ref keyframe).set_time(time);
    ((Keyframe) ref keyframe).set_inTangent(inTangent);
    ((Keyframe) ref keyframe).set_outTangent(outTangent);
    return keyframe;
  }
}
