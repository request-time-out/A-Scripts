// Decompiled with JetBrains decompiler
// Type: Housing.Extension.IsNullOrEmptyExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;

namespace Housing.Extension
{
  public static class IsNullOrEmptyExtension
  {
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
      bool? nullable = enumerable != null ? new bool?(enumerable.Any<T>()) : new bool?();
      return !nullable.GetValueOrDefault() || !nullable.HasValue;
    }
  }
}
