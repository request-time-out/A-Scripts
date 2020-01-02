// Decompiled with JetBrains decompiler
// Type: StringExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

internal static class StringExtensions
{
  public static bool ContainsAll(this string str, IEnumerable<string> needles)
  {
    return str != null && needles.All<string>(new Func<string, bool>(str.Contains));
  }

  public static bool ContainsAny(this string str, IEnumerable<string> needles)
  {
    return str != null && needles.Any<string>(new Func<string, bool>(str.Contains));
  }

  public static bool ContainsAll(this string str, params string[] needles)
  {
    return str.ContainsAll(((IEnumerable<string>) needles).AsEnumerable<string>());
  }

  public static bool ContainsAny(this string str, params string[] needles)
  {
    return str.ContainsAny(((IEnumerable<string>) needles).AsEnumerable<string>());
  }
}
