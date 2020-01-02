// Decompiled with JetBrains decompiler
// Type: CoroutineNameCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class CoroutineNameCache
{
  private static Dictionary<string, string> _mangledNames = new Dictionary<string, string>();

  public static string Mangle(string rawName)
  {
    string str1;
    if (CoroutineNameCache._mangledNames.TryGetValue(rawName, out str1))
      return str1;
    string str2 = rawName.Replace('<', '{').Replace('>', '}');
    CoroutineNameCache._mangledNames[rawName] = str2;
    return str2;
  }
}
