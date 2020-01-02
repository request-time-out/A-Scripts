// Decompiled with JetBrains decompiler
// Type: NetManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Timers;

public class NetManager : IDisposable
{
  private long INTERVAL_KeepAlive = 3000;
  private long INTERVAL_CheckingConnectionStatus = 1000;
  private long INTERVAL_ReceivingData = 200;
  private NetClient _client = new NetClient();
  private NetGuardTimer _guardTimer = new NetGuardTimer();
  private Timer _tickTimer = new Timer(100.0);
  public static NetManager Instance;
  private long _currentTimeInMilliseconds;
  private long _lastKeepAlive;
  private long _lastCheckingConnectionStatus;
  private long _lastReceivingData;

  public NetManager()
  {
    this._client.Connected += new SysPost.StdMulticastDelegation(this.OnConnected);
    this._client.Disconnected += new SysPost.StdMulticastDelegation(this.OnDisconnected);
    this._client.RegisterCmdHandler(eNetCmd.SV_HandshakeResponse, new UsCmdHandler(this.Handle_HandshakeResponse));
    this._client.RegisterCmdHandler(eNetCmd.SV_KeepAliveResponse, new UsCmdHandler(this.Handle_KeepAliveResponse));
    this._client.RegisterCmdHandler(eNetCmd.SV_ExecCommandResponse, new UsCmdHandler(this.Handle_ExecCommandResponse));
    this._guardTimer.Timeout += new SysPost.StdMulticastDelegation(this.OnGuardingTimeout);
    this._tickTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => this.Tick());
    this._tickTimer.AutoReset = true;
  }

  public bool IsConnected
  {
    get
    {
      return this._client.IsConnected;
    }
  }

  public string RemoteAddr
  {
    get
    {
      return this._client.RemoteAddr;
    }
  }

  public event SysPost.StdMulticastDelegation LogicallyConnected;

  public event SysPost.StdMulticastDelegation LogicallyDisconnected;

  public void Dispose()
  {
    this._client.Dispose();
  }

  public bool Connect(string addr)
  {
    this._client.Connect(addr, 39980);
    return true;
  }

  public void Disconnect()
  {
    this._client.Disconnect();
  }

  public void Send(UsCmd cmd)
  {
    this._client.SendPacket(cmd);
  }

  public void RegisterCmdHandler(eNetCmd cmd, UsCmdHandler handler)
  {
    this._client.RegisterCmdHandler(cmd, handler);
  }

  public void ExecuteCmd(string cmdText)
  {
    if (!this.IsConnected)
      NetUtil.Log("not connected to server, command ignored.", (object[]) Array.Empty<object>());
    else if (cmdText.Length == 0)
    {
      NetUtil.Log("the command bar is empty, try 'help' to list all supported commands.", (object[]) Array.Empty<object>());
    }
    else
    {
      UsCmd cmd = new UsCmd();
      cmd.WriteNetCmd(eNetCmd.CL_ExecCommand);
      cmd.WriteString(cmdText);
      this.Send(cmd);
      NetUtil.Log("command executed: [b]{0}[/b]", (object) cmdText);
    }
  }

  private void OnConnected(object sender, EventArgs e)
  {
    UsCmd cmd = new UsCmd();
    cmd.WriteInt16((short) 1001);
    cmd.WriteInt16((short) 0);
    cmd.WriteInt16((short) 1);
    cmd.WriteInt16((short) 0);
    this._client.SendPacket(cmd);
    this._tickTimer.Start();
    this._guardTimer.Activate();
  }

  private void OnDisconnected(object sender, EventArgs e)
  {
    this._tickTimer.Stop();
    this._guardTimer.Deactivate();
    SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.LogicallyDisconnected);
  }

  private void OnGuardingTimeout(object sender, EventArgs e)
  {
    NetUtil.LogError("guarding timeout, closing connection...", (object[]) Array.Empty<object>());
    this.Disconnect();
  }

  private bool Handle_HandshakeResponse(eNetCmd cmd, UsCmd c)
  {
    NetUtil.Log("eNetCmd.SV_HandshakeResponse received, connection validated.", (object[]) Array.Empty<object>());
    SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.LogicallyConnected);
    this._guardTimer.Deactivate();
    return true;
  }

  private bool Handle_KeepAliveResponse(eNetCmd cmd, UsCmd c)
  {
    return true;
  }

  private bool Handle_ExecCommandResponse(eNetCmd cmd, UsCmd c)
  {
    NetUtil.Log("command executing result: [b]{0}[/b]", (object) c.ReadInt32());
    return true;
  }

  private void Tick()
  {
    if (!this._client.IsConnected)
      return;
    this._currentTimeInMilliseconds = DateTime.Now.Ticks / 10000L;
    if (this._currentTimeInMilliseconds - this._lastKeepAlive > this.INTERVAL_KeepAlive)
    {
      UsCmd cmd = new UsCmd();
      cmd.WriteNetCmd(eNetCmd.CL_KeepAlive);
      this._client.SendPacket(cmd);
      this._lastKeepAlive = this._currentTimeInMilliseconds;
    }
    if (this._currentTimeInMilliseconds - this._lastCheckingConnectionStatus > this.INTERVAL_CheckingConnectionStatus)
    {
      this._client.Tick_CheckConnectionStatus();
      this._lastCheckingConnectionStatus = this._currentTimeInMilliseconds;
    }
    if (this._currentTimeInMilliseconds - this._lastReceivingData <= this.INTERVAL_ReceivingData)
      return;
    this._client.Tick_ReceivingData();
    this._lastReceivingData = this._currentTimeInMilliseconds;
  }

  public NetClient Client
  {
    get
    {
      return this._client;
    }
  }
}
