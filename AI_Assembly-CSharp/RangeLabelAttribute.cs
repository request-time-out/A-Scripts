// Decompiled with JetBrains decompiler
// Type: RangeLabelAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class RangeLabelAttribute : PropertyAttribute
{
  public string label;
  public float min;
  public float max;

  public RangeLabelAttribute(string label, float min, float max)
  {
    this.\u002Ector();
    this.label = label;
    this.min = min;
    this.max = max;
  }
}
