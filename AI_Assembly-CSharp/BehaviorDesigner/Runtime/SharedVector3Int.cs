// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedVector3Int
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedVector3Int : SharedVariable<Vector3Int>
  {
    public SharedVector3Int()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedVector3Int(Vector3Int value)
    {
      SharedVector3Int sharedVector3Int = new SharedVector3Int();
      sharedVector3Int.mValue = (__Null) value;
      return sharedVector3Int;
    }
  }
}
