// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedQuaternion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedQuaternion : SharedVariable<Quaternion>
  {
    public SharedQuaternion()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedQuaternion(Quaternion value)
    {
      SharedQuaternion sharedQuaternion = new SharedQuaternion();
      sharedQuaternion.mValue = (__Null) value;
      return sharedQuaternion;
    }
  }
}
