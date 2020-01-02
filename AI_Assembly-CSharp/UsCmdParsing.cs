// Decompiled with JetBrains decompiler
// Type: UsCmdParsing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UsCmdParsing
{
  private Dictionary<eNetCmd, UsCmdHandler> m_handlers = new Dictionary<eNetCmd, UsCmdHandler>();
  private Dictionary<eNetCmd, UsClientCmdHandler> m_clientHandlers = new Dictionary<eNetCmd, UsClientCmdHandler>();

  public void RegisterHandler(eNetCmd cmd, UsCmdHandler handler)
  {
    this.m_handlers[cmd] = handler;
  }

  public void RegisterClientHandler(eNetCmd cmd, UsClientCmdHandler handler)
  {
    this.m_clientHandlers[cmd] = handler;
  }

  public UsCmdExecResult Execute(UsCmd c)
  {
    try
    {
      eNetCmd eNetCmd = c.ReadNetCmd();
      UsCmdHandler usCmdHandler;
      if (!this.m_handlers.TryGetValue(eNetCmd, out usCmdHandler))
        return UsCmdExecResult.HandlerNotFound;
      return usCmdHandler(eNetCmd, c) ? UsCmdExecResult.Succ : UsCmdExecResult.Failed;
    }
    catch (Exception ex)
    {
      Console.WriteLine("[cmd] Execution failed. ({0})", (object) ex.Message);
      return UsCmdExecResult.Failed;
    }
  }

  public UsCmdExecResult ExecuteClient(string clientID, UsCmd c)
  {
    try
    {
      eNetCmd eNetCmd = c.ReadNetCmd();
      UsClientCmdHandler clientCmdHandler;
      if (!this.m_clientHandlers.TryGetValue(eNetCmd, out clientCmdHandler))
        return UsCmdExecResult.HandlerNotFound;
      return clientCmdHandler(clientID, eNetCmd, c) ? UsCmdExecResult.Succ : UsCmdExecResult.Failed;
    }
    catch (Exception ex)
    {
      Console.WriteLine("[cmd] Execution failed. ({0})", (object) ex.Message);
      return UsCmdExecResult.Failed;
    }
  }
}
