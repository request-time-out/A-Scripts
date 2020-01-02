// Decompiled with JetBrains decompiler
// Type: UsMain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class UsMain : IDisposable
{
  private long _tickNetInterval = 200;
  public const int MAX_CONTENT_LEN = 1024;
  private long _currentTimeInMilliseconds;
  private long _tickNetLast;
  private LogService _logServ;
  private utest _test;
  private bool _inGameGui;

  public UsMain(bool LogRemotely, bool LogIntoFile, bool InGameGui)
  {
    Application.set_runInBackground(true);
    this._logServ = new LogService(LogIntoFile, -1, true);
    this._test = new utest();
    if (LogRemotely)
      this._logServ.LogTargets += new LogTargetHandler(this.LogTarget_Remotely);
    UsNet.Instance = new UsNet();
    UsMain_NetHandlers.Instance = new UsMain_NetHandlers(UsNet.Instance.CmdExecutor);
    UsvConsole.Instance = new UsvConsole();
    GameUtil.Log("on_level loaded.", (object[]) Array.Empty<object>());
    GameInterface.Instance.Init();
    this._inGameGui = InGameGui;
  }

  private void LogTarget_Remotely(object sender, LogEventArgs args)
  {
    if (UsNet.Instance == null)
      return;
    UsCmd cmd = new UsCmd();
    cmd.WriteNetCmd(eNetCmd.SV_App_Logging);
    cmd.WriteInt16((short) args.SeqID);
    cmd.WriteInt32((int) args.LogType);
    cmd.WriteStringStripped(args.Content, (short) 1024);
    cmd.WriteFloat(args.Time);
    UsNet.Instance.SendCommand(cmd);
  }

  public void Update()
  {
    this._currentTimeInMilliseconds = DateTime.Now.Ticks / 10000L;
    if (this._currentTimeInMilliseconds - this._tickNetLast <= this._tickNetInterval)
      return;
    if (UsNet.Instance != null)
      UsNet.Instance.Update();
    this._tickNetLast = this._currentTimeInMilliseconds;
  }

  public void Dispose()
  {
    UsNet.Instance.Dispose();
    this._test.Dispose();
    this._logServ.Dispose();
  }

  public void OnLevelWasLoaded()
  {
    this._test.OnLevelWasLoaded();
  }

  public void OnGUI()
  {
    if (!this._inGameGui)
      return;
    this._test.OnGUI();
  }
}
