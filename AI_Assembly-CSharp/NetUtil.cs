// Decompiled with JetBrains decompiler
// Type: NetUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public static class NetUtil
{
  public static NetLogHandler LogHandler { get; set; }

  public static NetLogHandler LogErrorHandler { get; set; }

  public static void Log(string fmt, params object[] args)
  {
    if (NetUtil.LogHandler == null)
      return;
    NetUtil.LogHandler(fmt, args);
  }

  public static void LogError(string fmt, params object[] args)
  {
    if (NetUtil.LogErrorHandler == null)
      return;
    NetUtil.LogErrorHandler(fmt, args);
  }
}
