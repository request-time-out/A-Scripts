// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.EnumFlags2Attribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
  public sealed class EnumFlags2Attribute : PropertyAttribute
  {
    public string label;
    public int line;

    public EnumFlags2Attribute(string label, int _oneline = -1)
    {
      this.\u002Ector();
      this.label = label;
      this.line = _oneline;
    }
  }
}
