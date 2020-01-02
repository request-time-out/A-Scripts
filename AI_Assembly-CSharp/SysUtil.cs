// Decompiled with JetBrains decompiler
// Type: SysUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

public class SysUtil
{
  public static string FormatDateAsFileNameString(DateTime dt)
  {
    return string.Format("{0:0000}-{1:00}-{2:00}", (object) dt.Year, (object) dt.Month, (object) dt.Day);
  }

  public static string FormatTimeAsFileNameString(DateTime dt)
  {
    return string.Format("{0:00}-{1:00}-{2:00}", (object) dt.Hour, (object) dt.Minute, (object) dt.Second);
  }
}
