// Decompiled with JetBrains decompiler
// Type: UniqueString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class UniqueString
{
  private static Dictionary<string, string> m_strings = new Dictionary<string, string>();

  public static string Intern(string str, bool removable = true)
  {
    if (str == null)
      return (string) null;
    string str1 = UniqueString.IsInterned(str);
    if (str1 != null)
      return str1;
    if (!removable)
      return string.Intern(str);
    UniqueString.m_strings.Add(str, str);
    return str;
  }

  public static string IsInterned(string str)
  {
    if (str == null)
      return (string) null;
    string str1 = string.IsInterned(str);
    return str1 != null || UniqueString.m_strings.TryGetValue(str, out str1) ? str1 : (string) null;
  }

  public static void Clear()
  {
    UniqueString.m_strings.Clear();
  }
}
