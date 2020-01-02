// Decompiled with JetBrains decompiler
// Type: UIDebugCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class UIDebugCache
{
  public static Dictionary<int, string> s_nameLut = new Dictionary<int, string>();
  public static Dictionary<int, string> s_parentNameLut = new Dictionary<int, string>();

  public static string GetName(int instID)
  {
    return UIDebugCache.s_nameLut.ContainsKey(instID) ? UIDebugCache.s_nameLut[instID] : string.Empty;
  }

  public static string GetParentName(int instID)
  {
    return UIDebugCache.s_parentNameLut.ContainsKey(instID) ? UIDebugCache.s_parentNameLut[instID] : string.Empty;
  }
}
