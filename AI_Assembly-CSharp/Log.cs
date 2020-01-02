// Decompiled with JetBrains decompiler
// Type: Log
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public static class Log
{
  public static LogLevel LogLevel = LogLevel.Info;

  public static void Info(object msg, params object[] args)
  {
    if (Log.LogLevel < LogLevel.Info)
      return;
    Debug.Log(Log._format(msg, args));
  }

  public static void InfoEx(object msg, Object context)
  {
    if (Log.LogLevel < LogLevel.Info)
      return;
    Debug.Log(msg, context);
  }

  public static void Warning(object msg, params object[] args)
  {
    if (Log.LogLevel < LogLevel.Warning)
      return;
    Debug.LogWarning(Log._format(msg, args));
  }

  public static void Error(object msg, params object[] args)
  {
    if (Log.LogLevel < LogLevel.Error)
      return;
    Debug.LogError(Log._format(msg, args));
  }

  public static void Exception(Exception ex)
  {
    if (Log.LogLevel < LogLevel.Error)
      return;
    Debug.LogException(ex);
  }

  public static void Assert(bool condition)
  {
    if (Log.LogLevel < LogLevel.Error)
      return;
    Log.Assert(condition, string.Empty, true);
  }

  public static void Assert(bool condition, string assertString)
  {
    if (Log.LogLevel < LogLevel.Error)
      return;
    Log.Assert(condition, assertString, false);
  }

  public static void Assert(bool condition, string assertString, bool pauseOnFail)
  {
    if (condition || Log.LogLevel < LogLevel.Error)
      return;
    Debug.LogError((object) ("assert failed! " + assertString));
    if (!pauseOnFail)
      return;
    Debug.Break();
  }

  private static object _format(object msg, params object[] args)
  {
    string format = msg as string;
    return args.Length == 0 || string.IsNullOrEmpty(format) ? msg : (object) string.Format(format, args);
  }
}
