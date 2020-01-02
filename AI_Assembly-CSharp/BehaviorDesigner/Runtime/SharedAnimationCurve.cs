// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedAnimationCurve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedAnimationCurve : SharedVariable<AnimationCurve>
  {
    public SharedAnimationCurve()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedAnimationCurve(AnimationCurve value)
    {
      SharedAnimationCurve sharedAnimationCurve = new SharedAnimationCurve();
      sharedAnimationCurve.mValue = (__Null) value;
      return sharedAnimationCurve;
    }
  }
}
