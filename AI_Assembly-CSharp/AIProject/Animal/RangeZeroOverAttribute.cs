﻿// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.RangeZeroOverAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.Animal
{
  [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field)]
  public class RangeZeroOverAttribute : PropertyAttribute
  {
    public string label;

    public RangeZeroOverAttribute(string _label)
    {
      this.\u002Ector();
      this.label = _label;
    }
  }
}