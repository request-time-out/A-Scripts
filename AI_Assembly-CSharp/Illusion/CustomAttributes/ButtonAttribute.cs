// Decompiled with JetBrains decompiler
// Type: Illusion.CustomAttributes.ButtonAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
  public sealed class ButtonAttribute : PropertyAttribute
  {
    public ButtonAttribute(string function, string name, params object[] parameters)
    {
      this.\u002Ector();
      this.Function = function;
      this.Name = name;
      this.Parameters = parameters;
    }

    public string Function { get; private set; }

    public string Name { get; private set; }

    public object[] Parameters { get; private set; }
  }
}
