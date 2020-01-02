// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.TagSelectorAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class TagSelectorAttribute : PropertyAttribute
  {
    public bool AllowUntagged;

    public TagSelectorAttribute()
    {
      base.\u002Ector();
    }
  }
}
