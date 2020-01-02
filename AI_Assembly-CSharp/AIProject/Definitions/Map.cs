// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Map
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEx;

namespace AIProject.Definitions
{
  public static class Map
  {
    public static Dictionary<AIProject.TimeZone, string> TimeZoneNameTable = new Dictionary<AIProject.TimeZone, string>()
    {
      [AIProject.TimeZone.Morning] = "朝",
      [AIProject.TimeZone.Day] = "昼",
      [AIProject.TimeZone.Night] = "夜"
    };
    public static Dictionary<Weather, string> WeatherNameTable = new Dictionary<Weather, string>()
    {
      [Weather.Clear] = "快晴",
      [Weather.Cloud1] = "晴れ（少量）",
      [Weather.Cloud2] = "晴れ（大量）",
      [Weather.Cloud3] = "曇り",
      [Weather.Cloud4] = "曇天",
      [Weather.Rain] = "雨",
      [Weather.Storm] = "大雨",
      [Weather.Fog] = "霧"
    };
    public static Dictionary<Temperature, string> TemperatureNameTable = new Dictionary<Temperature, string>()
    {
      [Temperature.Normal] = "普通",
      [Temperature.Hot] = "暑い",
      [Temperature.Cold] = "寒い"
    };
    public static readonly DateTime EndTimeOfDay = new DateTime(1, 1, 1, 23, 59, 59);
    public static readonly ValueTuple<int, string, MapArea.AreaType>[] AreaList = new ValueTuple<int, string, MapArea.AreaType>[3]
    {
      new ValueTuple<int, string, MapArea.AreaType>(0, "NormalArea", MapArea.AreaType.Normal),
      new ValueTuple<int, string, MapArea.AreaType>(1, "UnderRoofArea", MapArea.AreaType.Indoor),
      new ValueTuple<int, string, MapArea.AreaType>(2, "PrivateRoom", MapArea.AreaType.Private)
    };
    public const int PlayerID = -99;
    public const int MerchantID = -90;

    public enum FootStepSE
    {
      Sand,
      Soil,
      Grass,
      Rock,
      Metal,
      Wood,
      WoodFlooring,
      Wet,
      Water,
      Snow,
    }
  }
}
