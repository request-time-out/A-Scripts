// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ProceduralImage.ModifierID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace UnityEngine.UI.ProceduralImage
{
  [AttributeUsage(AttributeTargets.Class)]
  public class ModifierID : Attribute
  {
    private string name;

    public ModifierID(string name)
    {
      this.name = name;
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }
  }
}
