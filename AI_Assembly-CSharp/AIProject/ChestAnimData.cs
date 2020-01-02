// Decompiled with JetBrains decompiler
// Type: AIProject.ChestAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class ChestAnimData : ActionPointAnimData
  {
    private static Dictionary<int, Animator> _table = new Dictionary<int, Animator>();

    public static ReadOnlyDictionary<int, Animator> Table { get; } = new ReadOnlyDictionary<int, Animator>((IDictionary<int, Animator>) ChestAnimData._table);

    protected override void Awake()
    {
      ChestAnimData._table[this._id] = this._animator;
    }
  }
}
