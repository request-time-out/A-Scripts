// Decompiled with JetBrains decompiler
// Type: EnviroStartInSeason
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroStartInSeason : MonoBehaviour
{
  public EnviroSeasons.Seasons startInSeason;

  public EnviroStartInSeason()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Inequality((Object) EnviroSky.instance, (Object) null))
      return;
    switch (this.startInSeason)
    {
      case EnviroSeasons.Seasons.Spring:
        EnviroSky.instance.GameTime.Days = 1;
        break;
      case EnviroSeasons.Seasons.Summer:
        EnviroSky.instance.GameTime.Days = (int) EnviroSky.instance.seasonsSettings.SpringInDays;
        break;
      case EnviroSeasons.Seasons.Autumn:
        EnviroSky.instance.GameTime.Days = (int) EnviroSky.instance.seasonsSettings.SpringInDays + (int) EnviroSky.instance.seasonsSettings.SummerInDays;
        break;
      case EnviroSeasons.Seasons.Winter:
        EnviroSky.instance.GameTime.Days = (int) EnviroSky.instance.seasonsSettings.SpringInDays + (int) EnviroSky.instance.seasonsSettings.SummerInDays + (int) EnviroSky.instance.seasonsSettings.AutumnInDays;
        break;
    }
  }
}
