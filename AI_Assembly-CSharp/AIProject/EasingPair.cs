// Decompiled with JetBrains decompiler
// Type: AIProject.EasingPair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace AIProject
{
  [Serializable]
  public struct EasingPair
  {
    public MotionType @in;
    public MotionType @out;

    public EasingPair(MotionType inType, MotionType outType)
    {
      this.@in = inType;
      this.@out = outType;
    }
  }
}
