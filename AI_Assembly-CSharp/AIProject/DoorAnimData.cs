// Decompiled with JetBrains decompiler
// Type: AIProject.DoorAnimData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class DoorAnimData : ActionPointAnimData
  {
    private static Dictionary<int, Animator> _table = new Dictionary<int, Animator>();
    private static Dictionary<int, DoorMatType> _matTable = new Dictionary<int, DoorMatType>();
    [SerializeField]
    private DoorMatType _matType;

    public static ReadOnlyDictionary<int, Animator> Table { get; } = new ReadOnlyDictionary<int, Animator>((IDictionary<int, Animator>) DoorAnimData._table);

    public static ReadOnlyDictionary<int, DoorMatType> MatTable { get; } = new ReadOnlyDictionary<int, DoorMatType>((IDictionary<int, DoorMatType>) DoorAnimData._matTable);

    protected override void Awake()
    {
      DoorAnimData._table[this._id] = this._animator;
      DoorAnimData._matTable[this._id] = this._matType;
    }
  }
}
