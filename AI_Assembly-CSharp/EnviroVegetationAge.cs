// Decompiled with JetBrains decompiler
// Type: EnviroVegetationAge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

[Serializable]
public class EnviroVegetationAge
{
  public float maxAgeHours = 24f;
  public float maxAgeDays = 60f;
  public bool Loop = true;
  public float maxAgeYears;
  public bool randomStartAge;
  public float startAgeinHours;
  public double birthdayInHours;
  public int LoopFromGrowStage;
}
