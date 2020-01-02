// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.RangeIntLabelAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Field)]
  public class RangeIntLabelAttribute : PropertyAttribute
  {
    public string label;
    public int min;
    public int max;

    public RangeIntLabelAttribute(string label, int min, int max)
    {
      this.\u002Ector();
      this.label = label;
      this.min = min;
      this.max = max;
    }
  }
}
