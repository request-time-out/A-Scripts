// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.MulticastDelegateExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace Illusion.Extensions
{
  public static class MulticastDelegateExtensions
  {
    public static int GetLength(this MulticastDelegate self)
    {
      return (object) self == null || self.GetInvocationList() == null ? 0 : self.GetInvocationList().Length;
    }
  }
}
