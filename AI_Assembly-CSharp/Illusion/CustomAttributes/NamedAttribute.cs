// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.NamedAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Illusion.CustomAttributes
{
  public class NamedAttribute : PropertyAttribute
  {
    public readonly string name;

    public NamedAttribute(string name)
    {
      this.\u002Ector();
      this.name = name;
    }
  }
}
