// Decompiled with JetBrains decompiler
// Type: EnviroVegetationSeasons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

[Serializable]
public class EnviroVegetationSeasons
{
  public bool GrowInSpring = true;
  public bool GrowInSummer = true;
  public bool GrowInAutumn = true;
  public bool GrowInWinter = true;
  public EnviroVegetationSeasons.SeasonAction seasonAction;

  public enum SeasonAction
  {
    SpawnDeadPrefab,
    Deactivate,
    Destroy,
  }
}
