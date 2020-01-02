// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedVector2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedVector2 : SharedVariable<Vector2>
  {
    public SharedVector2()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedVector2(Vector2 value)
    {
      SharedVector2 sharedVector2 = new SharedVector2();
      sharedVector2.mValue = (__Null) value;
      return sharedVector2;
    }
  }
}
