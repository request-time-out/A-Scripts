// Decompiled with JetBrains decompiler
// Type: Exploder.Profiler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace Exploder
{
  public static class Profiler
  {
    private static readonly Dictionary<string, Stopwatch> timeSegments = new Dictionary<string, Stopwatch>();

    public static void Start(string key)
    {
      Stopwatch stopwatch1 = (Stopwatch) null;
      if (Profiler.timeSegments.TryGetValue(key, out stopwatch1))
      {
        stopwatch1.Reset();
        stopwatch1.Start();
      }
      else
      {
        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch2.Start();
        Profiler.timeSegments.Add(key, stopwatch2);
      }
    }

    public static void End(string key)
    {
      Profiler.timeSegments[key].Stop();
    }

    public static string[] PrintResults()
    {
      string[] strArray = new string[Profiler.timeSegments.Count];
      int num = 0;
      foreach (KeyValuePair<string, Stopwatch> timeSegment in Profiler.timeSegments)
        strArray[num++] = timeSegment.Key + " " + timeSegment.Value.ElapsedMilliseconds.ToString() + " [ms]";
      return strArray;
    }
  }
}
