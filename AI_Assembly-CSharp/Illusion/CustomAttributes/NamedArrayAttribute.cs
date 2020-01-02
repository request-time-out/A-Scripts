// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.NamedArrayAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.CustomAttributes
{
  public class NamedArrayAttribute : PropertyAttribute
  {
    public readonly string[] names;

    public NamedArrayAttribute(params string[] names)
    {
      this.\u002Ector();
      this.names = names;
    }

    public NamedArrayAttribute(System.Type enumType)
    {
      this.\u002Ector();
      this.names = Enum.GetNames(enumType);
    }
  }
}
