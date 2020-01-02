// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedVector2Int
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedVector2Int : SharedVariable<Vector2Int>
  {
    public SharedVector2Int()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedVector2Int(Vector2Int value)
    {
      SharedVector2Int sharedVector2Int = new SharedVector2Int();
      sharedVector2Int.mValue = (__Null) value;
      return sharedVector2Int;
    }
  }
}
