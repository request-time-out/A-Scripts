﻿// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedFloat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedFloat : SharedVariable<float>
  {
    public SharedFloat()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedFloat(float value)
    {
      SharedFloat sharedFloat = new SharedFloat();
      sharedFloat.set_Value(value);
      return sharedFloat;
    }
  }
}
