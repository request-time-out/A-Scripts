// Decompiled with JetBrains decompiler
// Type: AllOptionsKeyPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

public static class AllOptionsKeyPro
{
  public const string AntiAliasing = "GameName.AntiAliasing";
  public const string AnisoTropic = "GameName.AnisoTropic";
  public const string Resolution = "GameName.ResolutionScreen";
  public const string ResolutionMode = "GameName.ResolutionMode";
  public const string VsyncCount = "GameName.VSyncCount";
  public const string BlendWeight = "GameName.BlendWeight";
  public const string Volumen = "GameName.Volumen";
  public const string Quality = "GameName.QualityLevel";
  public const string TextureLimit = "GameName.TextureLimit";
  public const string ShadowCascade = "GameName.ShadowCascade";
  public const string ShowFPS = "GameName.ShowFPS";
  public const string ShadowDistance = "GameName.ShadowDistance";
  public const string ShadownProjection = "GameName.ShadowProjection";
  public const string PauseAudio = "GameName.PauseAudio";
  public const string ShadowEnable = "GameName.ShadowEnable";
  public const string Brightness = "GameName.Brightness";
  public const string RealtimeReflection = "GameName.RealtimeReflection";
  public const string LodBias = "GameName.LoadBias";
  public const string HUDScale = "GameName.HudScale";

  public static int BoolToInt(bool b)
  {
    return b ? 1 : 0;
  }

  public static bool IntToBool(int i)
  {
    return i == 1;
  }
}
