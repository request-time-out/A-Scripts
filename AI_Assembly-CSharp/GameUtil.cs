// Decompiled with JetBrains decompiler
// Type: GameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class GameUtil
{
  public static string _log = string.Empty;
  public static Vector2 _logPosition = Vector2.get_zero();
  public static int FontSize = 16;

  public static void Log(string format, params object[] args)
  {
    GameUtil._log = string.Format(format, args) + "\r\n" + GameUtil._log;
    GameUtil._logPosition.y = (__Null) 0.0;
  }

  public static float Clamp(float value, float min, float max)
  {
    return Math.Max(Math.Min(value, max), min);
  }
}
