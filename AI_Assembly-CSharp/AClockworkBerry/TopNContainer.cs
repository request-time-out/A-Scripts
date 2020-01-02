// Decompiled with JetBrains decompiler
// Type: AClockworkBerry.TopNContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AClockworkBerry
{
  public class TopNContainer
  {
    public List<KeyValuePair<double, string>> TopN = new List<KeyValuePair<double, string>>(6);
    private StringBuilder m_strBuilder = new StringBuilder(256);
    public const int MaxCount = 6;

    public bool TryAdd(double value, string text)
    {
      if (this.TopN.Count == 6 && value <= this.TopN[this.TopN.Count - 1].Key)
        return false;
      bool flag = false;
      for (int index = 0; index < this.TopN.Count; ++index)
      {
        if (value > this.TopN[index].Key)
        {
          this.TopN.Insert(index, new KeyValuePair<double, string>(value, text));
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        if (this.TopN.Count < 6)
        {
          this.TopN.Add(new KeyValuePair<double, string>(value, text));
          return true;
        }
        Debug.LogWarningFormat("[TopNContainer] (大于下限却找不到插入点)。(len: {0}, lowest: {1}, value: {2})", new object[3]
        {
          (object) this.TopN.Count,
          (object) this.TopN[this.TopN.Count - 1].Key,
          (object) value
        });
        return false;
      }
      while (this.TopN.Count > 6)
        this.TopN.RemoveAt(this.TopN.Count - 1);
      return true;
    }

    public string ItemToString(int i)
    {
      this.m_strBuilder.Length = 0;
      return this.m_strBuilder.AppendFormat("({0:0.00}) {1}", (object) this.TopN[i].Key, (object) this.TopN[i].Value).ToString();
    }

    public void Clear()
    {
      this.TopN.Clear();
    }
  }
}
