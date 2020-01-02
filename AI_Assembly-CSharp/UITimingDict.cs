// Decompiled with JetBrains decompiler
// Type: UITimingDict
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

public class UITimingDict
{
  private Stopwatch m_stopwatch = Stopwatch.StartNew();
  private Dictionary<int, double> m_elapsedTicks = new Dictionary<int, double>(200);
  private const string Indent = "    ";

  public void StartTiming()
  {
    this.m_stopwatch.Reset();
    this.m_stopwatch.Start();
  }

  public double StopTiming(Object w)
  {
    int instanceId = w.GetInstanceID();
    this.m_stopwatch.Stop();
    double num = (double) this.m_stopwatch.ElapsedTicks / 10000.0;
    if (this.m_elapsedTicks.ContainsKey(instanceId))
    {
      Dictionary<int, double> elapsedTicks;
      int index;
      (elapsedTicks = this.m_elapsedTicks)[index = instanceId] = elapsedTicks[index] + num;
    }
    else
      this.m_elapsedTicks.Add(instanceId, num);
    if (!UIDebugCache.s_nameLut.ContainsKey(instanceId))
      UIDebugCache.s_nameLut.Add(instanceId, w.get_name());
    return num;
  }

  public string PrintDict(int count = -1)
  {
    List<KeyValuePair<int, double>> list = this.m_elapsedTicks.ToList<KeyValuePair<int, double>>();
    list.Sort((Comparison<KeyValuePair<int, double>>) ((pair1, pair2) => Math.Sign(pair2.Value - pair1.Value)));
    if (count > 0 && count < list.Count)
      list.RemoveRange(count - 1, list.Count - count);
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<int, double> keyValuePair in list)
      stringBuilder.AppendFormat("{0}{1,-40} \t{2:0.00} \t{3:0.00}\n", (object) "    ", (object) UIDebugCache.GetName(keyValuePair.Key), (object) keyValuePair.Value, (object) (keyValuePair.Value / (1.0 / (double) Time.get_deltaTime())));
    return stringBuilder.ToString();
  }
}
