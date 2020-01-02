// Decompiled with JetBrains decompiler
// Type: AIProject.Recipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEx;

namespace AIProject
{
  public class Recipe
  {
    private ValueTuple<int, int, int>[] _materials;

    public int ID { get; set; }

    public int CategoryID { get; set; }

    public int ItemID { get; set; }

    public string Name { get; set; }

    public int Priority { get; set; }

    public float Rate { get; set; }

    public bool IsVisible { get; set; }

    public ValueTuple<int, int, int>[] Materials
    {
      get
      {
        return ((IEnumerable<ValueTuple<int, int, int>>) this._materials).ToArray<ValueTuple<int, int, int>>();
      }
      set
      {
        this._materials = ((IEnumerable<ValueTuple<int, int, int>>) value).ToArray<ValueTuple<int, int, int>>();
      }
    }
  }
}
