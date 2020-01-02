// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Environment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AIProject.Definitions
{
  public static class Environment
  {
    public static ReadOnlyDictionary<TimeZone, int> TimeZoneIDTable = new ReadOnlyDictionary<TimeZone, int>((IDictionary<TimeZone, int>) new Dictionary<TimeZone, int>()
    {
      [TimeZone.Morning] = 0,
      [TimeZone.Day] = 1,
      [TimeZone.Night] = 2
    });
  }
}
