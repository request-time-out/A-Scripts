// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedVector3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedVector3 : SharedVariable<Vector3>
  {
    public SharedVector3()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedVector3(Vector3 value)
    {
      SharedVector3 sharedVector3 = new SharedVector3();
      sharedVector3.mValue = (__Null) value;
      return sharedVector3;
    }
  }
}
