// Decompiled with JetBrains decompiler
// Type: LogUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LogUtil
{
  public static bool EnableInMemoryStorage = false;
  public static int InMemoryItemMaxCount = 3;
  public static List<string> InMemoryExceptions = new List<string>();
  public static List<string> InMemoryErrors = new List<string>();

  public static string CombinePaths(params string[] paths)
  {
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    string[] strArray = paths;
    // ISSUE: reference to a compiler-generated field
    if (LogUtil.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      LogUtil.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string, string>(Path.Combine);
    }
    // ISSUE: reference to a compiler-generated field
    Func<string, string, string> fMgCache0 = LogUtil.\u003C\u003Ef__mg\u0024cache0;
    return ((IEnumerable<string>) strArray).Aggregate<string>(fMgCache0);
  }

  public static string FormatDateAsFileNameString(DateTime dt)
  {
    return string.Format("{0:0000}-{1:00}-{2:00}", (object) dt.Year, (object) dt.Month, (object) dt.Day);
  }

  public static string FormatTimeAsFileNameString(DateTime dt)
  {
    return string.Format("{0:00}-{1:00}-{2:00}", (object) dt.Hour, (object) dt.Minute, (object) dt.Second);
  }

  public static void PushInMemoryException(string exception)
  {
    LogUtil.InMemoryExceptions.Add(exception);
    while (LogUtil.InMemoryExceptions.Count > LogUtil.InMemoryItemMaxCount)
      LogUtil.InMemoryExceptions.RemoveAt(0);
  }

  public static void PushInMemoryError(string error)
  {
    LogUtil.InMemoryErrors.Add(error);
    while (LogUtil.InMemoryErrors.Count > LogUtil.InMemoryItemMaxCount)
      LogUtil.InMemoryErrors.RemoveAt(0);
  }
}
