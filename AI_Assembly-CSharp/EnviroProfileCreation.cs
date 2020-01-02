// Decompiled with JetBrains decompiler
// Type: EnviroProfileCreation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class EnviroProfileCreation
{
  public static void SetupDefaults(EnviroProfile profile)
  {
    List<Color> clrs1 = new List<Color>();
    List<float> times1 = new List<float>();
    clrs1.Add(EnviroProfileCreation.GetColor("#4C5570"));
    times1.Add(0.0f);
    clrs1.Add(EnviroProfileCreation.GetColor("#4C5570"));
    times1.Add(0.46f);
    clrs1.Add(EnviroProfileCreation.GetColor("#C98842"));
    times1.Add(0.51f);
    clrs1.Add(EnviroProfileCreation.GetColor("#EAC8A4"));
    times1.Add(0.56f);
    clrs1.Add(EnviroProfileCreation.GetColor("#EADCCE"));
    times1.Add(1f);
    profile.lightSettings.LightColor = EnviroProfileCreation.CreateGradient(clrs1, times1);
    List<Color> clrs2 = new List<Color>();
    List<float> times2 = new List<float>();
    profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.0f));
    profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.42f));
    profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.75f, 0.5f, 5f, 5f));
    profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(1.5f, 1f));
    profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.01f, 0.0f));
    profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.01f, 0.42f));
    profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.6f, 0.5f, 2f, 2f));
    profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 1f));
    profile.lightSettings.shadowIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 0.0f));
    profile.lightSettings.shadowIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 1f));
    profile.lightSettings.ambientIntensity.AddKey(EnviroProfileCreation.CreateKey(0.75f, 0.0f));
    profile.lightSettings.ambientIntensity.AddKey(EnviroProfileCreation.CreateKey(0.75f, 1f));
    clrs2.Add(EnviroProfileCreation.GetColor("#4C5570"));
    times2.Add(0.0f);
    clrs2.Add(EnviroProfileCreation.GetColor("#4C5570"));
    times2.Add(0.46f);
    clrs2.Add(EnviroProfileCreation.GetColor("#C98842"));
    times2.Add(0.51f);
    clrs2.Add(EnviroProfileCreation.GetColor("#99B2C3"));
    times2.Add(0.57f);
    clrs2.Add(EnviroProfileCreation.GetColor("#99B2C3"));
    times2.Add(1f);
    profile.lightSettings.ambientSkyColor = EnviroProfileCreation.CreateGradient(clrs2, times2);
    List<Color> clrs3 = new List<Color>();
    List<float> times3 = new List<float>();
    profile.lightSettings.ambientEquatorColor = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#2E3344"), 0.0f, EnviroProfileCreation.GetColor("#414852"), 1f);
    profile.lightSettings.ambientGroundColor = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#272B39"), 0.0f, EnviroProfileCreation.GetColor("#3E3631"), 1f);
    profile.skySettings.scatteringCurve.AddKey(EnviroProfileCreation.CreateKey(-25f, 0.0f));
    profile.skySettings.scatteringCurve.AddKey(EnviroProfileCreation.CreateKey(-10f, 0.5f, 55f, 55f));
    profile.skySettings.scatteringCurve.AddKey(EnviroProfileCreation.CreateKey(6.5f, 0.52f, 35f, 35f));
    profile.skySettings.scatteringCurve.AddKey(EnviroProfileCreation.CreateKey(11f, 1f));
    clrs3.Add(EnviroProfileCreation.GetColor("#8492C8"));
    times3.Add(0.0f);
    clrs3.Add(EnviroProfileCreation.GetColor("#8492C8"));
    times3.Add(0.45f);
    clrs3.Add(EnviroProfileCreation.GetColor("#FFB69C"));
    times3.Add(0.527f);
    clrs3.Add(EnviroProfileCreation.GetColor("#D2D2D2"));
    times3.Add(0.75f);
    clrs3.Add(EnviroProfileCreation.GetColor("#D2D2D2"));
    times3.Add(1f);
    profile.skySettings.scatteringColor = EnviroProfileCreation.CreateGradient(clrs3, times3);
    List<Color> clrs4 = new List<Color>();
    List<float> times4 = new List<float>();
    clrs4.Add(EnviroProfileCreation.GetColor("#0A0300"));
    times4.Add(0.0f);
    clrs4.Add(EnviroProfileCreation.GetColor("#FF6211"));
    times4.Add(0.45f);
    clrs4.Add(EnviroProfileCreation.GetColor("#FF6917"));
    times4.Add(0.55f);
    clrs4.Add(EnviroProfileCreation.GetColor("#FFE2CB"));
    times4.Add(0.75f);
    clrs4.Add(EnviroProfileCreation.GetColor("#FFFFFF"));
    times4.Add(1f);
    profile.skySettings.sunDiskColor = EnviroProfileCreation.CreateGradient(clrs4, times4);
    List<Color> clrs5 = new List<Color>();
    List<float> times5 = new List<float>();
    profile.skySettings.moonGlow.AddKey(EnviroProfileCreation.CreateKey(1f, 0.0f));
    profile.skySettings.moonGlow.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.65f));
    profile.skySettings.moonGlow.AddKey(EnviroProfileCreation.CreateKey(0.0f, 1f));
    profile.skySettings.skyLuminence.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.0f));
    profile.skySettings.skyLuminence.AddKey(EnviroProfileCreation.CreateKey(0.15f, 0.5f));
    profile.skySettings.skyLuminence.AddKey(EnviroProfileCreation.CreateKey(0.105f, 0.62f));
    profile.skySettings.skyLuminence.AddKey(EnviroProfileCreation.CreateKey(0.1f, 1f));
    profile.skySettings.skyColorPower.AddKey(EnviroProfileCreation.CreateKey(1.5f, 0.0f));
    profile.skySettings.skyColorPower.AddKey(EnviroProfileCreation.CreateKey(1.25f, 1f));
    profile.skySettings.starsIntensity.AddKey(EnviroProfileCreation.CreateKey(0.3f, 0.0f));
    profile.skySettings.starsIntensity.AddKey(EnviroProfileCreation.CreateKey(0.015f, 0.5f));
    profile.skySettings.starsIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.6f));
    profile.skySettings.starsIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 1f));
    profile.skySettings.moonTexture = EnviroProfileCreation.GetAssetTexture("tex_enviro_moon");
    profile.skySettings.starsCubeMap = EnviroProfileCreation.GetAssetCubemap("cube_enviro_stars");
    profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 0.0f));
    profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.015f, 0.5f));
    profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.6f));
    profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 1f));
    profile.skySettings.galaxyCubeMap = EnviroProfileCreation.GetAssetCubemap("cube_enviro_galaxy");
    Texture assetTexture1 = EnviroProfileCreation.GetAssetTexture("tex_enviro_noise");
    Texture assetTexture2 = EnviroProfileCreation.GetAssetTexture("tex_enviro_cirrus");
    if (Object.op_Equality((Object) assetTexture1, (Object) null) || Object.op_Equality((Object) assetTexture2, (Object) null))
      Debug.Log((object) "Cannot find cloud textures");
    profile.cloudsSettings.flatCloudsNoiseTexture = assetTexture1;
    profile.cloudsSettings.cirrusCloudsTexture = assetTexture2;
    profile.cloudsSettings.volumeCloudsMoonColor = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#232228"), 0.0f, EnviroProfileCreation.GetColor("#B6BCDC"), 1f);
    clrs5.Add(EnviroProfileCreation.GetColor("#17171A"));
    times5.Add(0.0f);
    clrs5.Add(EnviroProfileCreation.GetColor("#17171A"));
    times5.Add(0.455f);
    clrs5.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
    times5.Add(0.48f);
    clrs5.Add(EnviroProfileCreation.GetColor("#EEB279"));
    times5.Add(0.53f);
    clrs5.Add(EnviroProfileCreation.GetColor("#EEF0FF"));
    times5.Add(0.6f);
    clrs5.Add(EnviroProfileCreation.GetColor("#ECEEFF"));
    times5.Add(1f);
    profile.cloudsSettings.cirrusCloudsColor = EnviroProfileCreation.CreateGradient(clrs5, times5);
    List<Color> clrs6 = new List<Color>();
    List<float> times6 = new List<float>();
    clrs6.Add(EnviroProfileCreation.GetColor("#17171A"));
    times6.Add(0.0f);
    clrs6.Add(EnviroProfileCreation.GetColor("#17171A"));
    times6.Add(0.455f);
    clrs6.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
    times6.Add(0.48f);
    clrs6.Add(EnviroProfileCreation.GetColor("#EEB279"));
    times6.Add(0.53f);
    clrs6.Add(EnviroProfileCreation.GetColor("#EEF0FF"));
    times6.Add(0.6f);
    clrs6.Add(EnviroProfileCreation.GetColor("#ECEEFF"));
    times6.Add(1f);
    profile.cloudsSettings.flatCloudsColor = EnviroProfileCreation.CreateGradient(clrs6, times6);
    List<Color> clrs7 = new List<Color>();
    List<float> times7 = new List<float>();
    clrs7.Add(EnviroProfileCreation.GetColor("#17171A"));
    times7.Add(0.0f);
    clrs7.Add(EnviroProfileCreation.GetColor("#17171A"));
    times7.Add(0.455f);
    clrs7.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
    times7.Add(0.48f);
    clrs7.Add(EnviroProfileCreation.GetColor("#EEB279"));
    times7.Add(0.53f);
    clrs7.Add(EnviroProfileCreation.GetColor("#CECECE"));
    times7.Add(0.58f);
    clrs7.Add(EnviroProfileCreation.GetColor("#CECECE"));
    times7.Add(1f);
    profile.cloudsSettings.volumeCloudsColor = EnviroProfileCreation.CreateGradient(clrs7, times7);
    List<Color> clrs8 = new List<Color>();
    List<float> times8 = new List<float>();
    profile.cloudsSettings.directLightIntensity = new AnimationCurve();
    profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.02f, 0.0f));
    profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.15f, 0.495f));
    profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.15f, 1f));
    profile.cloudsSettings.ambientLightIntensity = new AnimationCurve();
    profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.017f, 0.0f));
    profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.46f));
    profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.35f, 0.617f));
    profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.32f, 1f));
    clrs8.Add(EnviroProfileCreation.GetColor("#FF703C"));
    times8.Add(0.0f);
    clrs8.Add(EnviroProfileCreation.GetColor("#FF5D00"));
    times8.Add(0.47f);
    clrs8.Add(EnviroProfileCreation.GetColor("#FFF4DF"));
    times8.Add(0.65f);
    clrs8.Add(EnviroProfileCreation.GetColor("#FFFFFF"));
    times8.Add(1f);
    profile.lightshaftsSettings.lightShaftsColorSun = EnviroProfileCreation.CreateGradient(clrs8, times8);
    List<Color> clrs9 = new List<Color>();
    List<float> times9 = new List<float>();
    profile.lightshaftsSettings.lightShaftsColorMoon = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#94A8E5"), 0.0f, EnviroProfileCreation.GetColor("#94A8E5"), 1f);
    clrs9.Add(EnviroProfileCreation.GetColor("#1D1D1D"));
    times9.Add(0.0f);
    clrs9.Add(EnviroProfileCreation.GetColor("#1D1D1D"));
    times9.Add(0.43f);
    clrs9.Add(EnviroProfileCreation.GetColor("#A6A6A6"));
    times9.Add(0.54f);
    clrs9.Add(EnviroProfileCreation.GetColor("#D0D0D0"));
    times9.Add(0.65f);
    clrs9.Add(EnviroProfileCreation.GetColor("#C3C3C3"));
    times9.Add(1f);
    profile.lightshaftsSettings.thresholdColorSun = EnviroProfileCreation.CreateGradient(clrs9, times9);
    List<Color> colorList = new List<Color>();
    List<float> floatList = new List<float>();
    profile.lightshaftsSettings.thresholdColorMoon = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#0B0B0B"), 0.0f, EnviroProfileCreation.GetColor("#000000"), 1f);
    profile.weatherSettings.lightningEffect = EnviroProfileCreation.GetAssetPrefab("Enviro_Lightning_Strike");
    for (int index = 0; index < 8; ++index)
      profile.audioSettings.ThunderSFX.Add(EnviroProfileCreation.GetAudioClip("SFX_Thunder_" + (object) (index + 1)));
    profile.weatherSettings.lightningEffect = EnviroProfileCreation.GetAssetPrefab("Enviro_Lightning_Strike");
  }

  public static bool UpdateProfile(EnviroProfile profile, string fromV, string toV)
  {
    if (Object.op_Equality((Object) profile, (Object) null))
      return false;
    if ((fromV == "1.9.0" || fromV == "1.9.1") && toV == "2.0.5")
    {
      profile.lightSettings.directLightSunIntensity = new AnimationCurve();
      profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.0f));
      profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.42f));
      profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(0.75f, 0.5f, 5f, 5f));
      profile.lightSettings.directLightSunIntensity.AddKey(EnviroProfileCreation.CreateKey(1.5f, 1f));
      profile.lightSettings.directLightMoonIntensity = new AnimationCurve();
      profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.01f, 0.0f));
      profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.01f, 0.42f));
      profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(0.6f, 0.5f, 2f, 2f));
      profile.lightSettings.directLightMoonIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 1f));
      profile.lightSettings.shadowIntensity = new AnimationCurve();
      profile.lightSettings.shadowIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 0.0f));
      profile.lightSettings.shadowIntensity.AddKey(EnviroProfileCreation.CreateKey(1f, 1f));
      profile.cloudsSettings.directLightIntensity = new AnimationCurve();
      profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.02f, 0.0f));
      profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.15f, 0.495f));
      profile.cloudsSettings.directLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.15f, 1f));
      profile.cloudsSettings.ambientLightIntensity = new AnimationCurve();
      profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.017f, 0.0f));
      profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.46f));
      profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.35f, 0.617f));
      profile.cloudsSettings.ambientLightIntensity.AddKey(EnviroProfileCreation.CreateKey(0.32f, 1f));
      profile.skySettings.moonColor = EnviroProfileCreation.GetColor("#9C9D9EFF");
      profile.skySettings.moonGlowColor = EnviroProfileCreation.GetColor("#4D4D4DFF");
      List<Color> clrs1 = new List<Color>();
      List<float> times1 = new List<float>();
      clrs1.Add(EnviroProfileCreation.GetColor("#17171A"));
      times1.Add(0.0f);
      clrs1.Add(EnviroProfileCreation.GetColor("#17171A"));
      times1.Add(0.455f);
      clrs1.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
      times1.Add(0.48f);
      clrs1.Add(EnviroProfileCreation.GetColor("#EEB279"));
      times1.Add(0.53f);
      clrs1.Add(EnviroProfileCreation.GetColor("#EEF0FF"));
      times1.Add(0.6f);
      clrs1.Add(EnviroProfileCreation.GetColor("#ECEEFF"));
      times1.Add(1f);
      profile.cloudsSettings.cirrusCloudsColor = EnviroProfileCreation.CreateGradient(clrs1, times1);
      List<Color> clrs2 = new List<Color>();
      List<float> times2 = new List<float>();
      clrs2.Add(EnviroProfileCreation.GetColor("#17171A"));
      times2.Add(0.0f);
      clrs2.Add(EnviroProfileCreation.GetColor("#17171A"));
      times2.Add(0.455f);
      clrs2.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
      times2.Add(0.48f);
      clrs2.Add(EnviroProfileCreation.GetColor("#EEB279"));
      times2.Add(0.53f);
      clrs2.Add(EnviroProfileCreation.GetColor("#EEF0FF"));
      times2.Add(0.6f);
      clrs2.Add(EnviroProfileCreation.GetColor("#ECEEFF"));
      times2.Add(1f);
      profile.cloudsSettings.flatCloudsColor = EnviroProfileCreation.CreateGradient(clrs2, times2);
      List<Color> clrs3 = new List<Color>();
      List<float> times3 = new List<float>();
      clrs3.Add(EnviroProfileCreation.GetColor("#17171A"));
      times3.Add(0.0f);
      clrs3.Add(EnviroProfileCreation.GetColor("#17171A"));
      times3.Add(0.455f);
      clrs3.Add(EnviroProfileCreation.GetColor("#3D3D3B"));
      times3.Add(0.48f);
      clrs3.Add(EnviroProfileCreation.GetColor("#EEB279"));
      times3.Add(0.53f);
      clrs3.Add(EnviroProfileCreation.GetColor("#CECECE"));
      times3.Add(0.58f);
      clrs3.Add(EnviroProfileCreation.GetColor("#CECECE"));
      times3.Add(1f);
      profile.cloudsSettings.volumeCloudsColor = EnviroProfileCreation.CreateGradient(clrs3, times3);
      profile.cloudsSettings.volumeCloudsMoonColor = EnviroProfileCreation.CreateGradient(EnviroProfileCreation.GetColor("#232228"), 0.0f, EnviroProfileCreation.GetColor("#B6BCDC"), 1f);
      Texture assetTexture1 = EnviroProfileCreation.GetAssetTexture("tex_enviro_noise");
      Texture assetTexture2 = EnviroProfileCreation.GetAssetTexture("tex_enviro_cirrus");
      if (Object.op_Equality((Object) assetTexture1, (Object) null) || Object.op_Equality((Object) assetTexture2, (Object) null))
        Debug.Log((object) "Cannot find cloud textures");
      profile.cloudsSettings.flatCloudsNoiseTexture = assetTexture1;
      profile.cloudsSettings.cirrusCloudsTexture = assetTexture2;
      profile.skySettings.moonTexture = EnviroProfileCreation.GetAssetTexture("tex_enviro_moon");
      profile.skySettings.moonBrightness = 1f;
      profile.skySettings.galaxyIntensity = new AnimationCurve();
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.4f, 0.0f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.015f, 0.5f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.6f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 1f));
      profile.skySettings.galaxyCubeMap = EnviroProfileCreation.GetAssetCubemap("cube_enviro_galaxy");
      profile.weatherSettings.lightningEffect = EnviroProfileCreation.GetAssetPrefab("Enviro_Lightning_Strike");
      profile.version = toV;
      return true;
    }
    if ((fromV == "2.0.0" || fromV == "2.0.1" || fromV == "2.0.2") && toV == "2.0.5")
    {
      profile.skySettings.galaxyIntensity = new AnimationCurve();
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.4f, 0.0f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.015f, 0.5f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 0.6f));
      profile.skySettings.galaxyIntensity.AddKey(EnviroProfileCreation.CreateKey(0.0f, 1f));
      profile.skySettings.galaxyCubeMap = EnviroProfileCreation.GetAssetCubemap("cube_enviro_galaxy");
      profile.weatherSettings.lightningEffect = EnviroProfileCreation.GetAssetPrefab("Enviro_Lightning_Strike");
      profile.version = toV;
      return true;
    }
    if (fromV == "2.0.3" && toV == "2.0.5")
    {
      profile.weatherSettings.lightningEffect = EnviroProfileCreation.GetAssetPrefab("Enviro_Lightning_Strike");
      profile.version = toV;
      return true;
    }
    if (!(fromV == "2.0.4") || !(toV == "2.0.5"))
      return false;
    profile.version = toV;
    return true;
  }

  public static GameObject GetAssetPrefab(string name)
  {
    return (GameObject) null;
  }

  public static AudioClip GetAudioClip(string name)
  {
    return (AudioClip) null;
  }

  public static Cubemap GetAssetCubemap(string name)
  {
    return (Cubemap) null;
  }

  public static Texture GetAssetTexture(string name)
  {
    return (Texture) null;
  }

  public static Gradient CreateGradient(Color clr1, float time1, Color clr2, float time2)
  {
    Gradient gradient = new Gradient();
    GradientColorKey[] gradientColorKeyArray = new GradientColorKey[2];
    GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[2];
    gradientColorKeyArray[0].color = (__Null) clr1;
    gradientColorKeyArray[0].time = (__Null) (double) time1;
    gradientColorKeyArray[1].color = (__Null) clr2;
    gradientColorKeyArray[1].time = (__Null) (double) time2;
    gradientAlphaKeyArray[0].alpha = (__Null) 1.0;
    gradientAlphaKeyArray[0].time = (__Null) 0.0;
    gradientAlphaKeyArray[1].alpha = (__Null) 1.0;
    gradientAlphaKeyArray[1].time = (__Null) 1.0;
    gradient.SetKeys(gradientColorKeyArray, gradientAlphaKeyArray);
    return gradient;
  }

  public static Gradient CreateGradient(List<Color> clrs, List<float> times)
  {
    Gradient gradient = new Gradient();
    GradientColorKey[] gradientColorKeyArray = new GradientColorKey[clrs.Count];
    GradientAlphaKey[] gradientAlphaKeyArray = new GradientAlphaKey[2];
    for (int index = 0; index < clrs.Count; ++index)
    {
      gradientColorKeyArray[index].color = (__Null) clrs[index];
      gradientColorKeyArray[index].time = (__Null) (double) times[index];
    }
    gradientAlphaKeyArray[0].alpha = (__Null) 1.0;
    gradientAlphaKeyArray[0].time = (__Null) 0.0;
    gradientAlphaKeyArray[1].alpha = (__Null) 1.0;
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
