// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedVector4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedVector4 : SharedVariable<Vector4>
  {
    public SharedVector4()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedVector4(Vector4 value)
    {
      SharedVector4 sharedVector4 = new SharedVector4();
      sharedVector4.mValue = (__Null) value;
      return sharedVector4;
    }
  }
}
