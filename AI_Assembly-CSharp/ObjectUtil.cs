// Decompiled with JetBrains decompiler
// Type: ObjectUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Runtime.CompilerServices;

public class ObjectUtil
{
  public static string GetHashCodeString(object obj)
  {
    return RuntimeHelpers.GetHashCode(obj).ToString("X8");
  }
}
