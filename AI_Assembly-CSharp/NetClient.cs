// Decompiled with JetBrains decompiler
// Type: NetClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using System.Net.Sockets;

public class NetClient : IDisposable
{
  private string _host = string.Empty;
  private UsCmdParsing _cmdParser = new UsCmdParsing();
  private int _port;
  private TcpClient _tcpClient;

  public event SysPost.StdMulticastDelegation Connected;

  public event SysPost.StdMulticastDelegation Disconnected;

  public bool IsConnected
  {
    get
    {
      return this._tcpClient != null;
    }
  }

  public string RemoteAddr
  {
    get
    {
      return this.IsConnected ? this._tcpClient.Client.RemoteEndPoint.ToString() : string.Empty;
    }
  }

  public void Connect(string host, int port)
  {
    this._host = host;
    this._port = port;
    this._tcpClient = new TcpClient();
    this._tcpClient.BeginConnect(this._host, this._port, new AsyncCallback(this.OnConnect), (object) this._tcpClient);
    NetUtil.Log("connecting to [u]{0}:{1}[/u]...", (object) host, (object) port);
  }

  public void Disconnect()
  {
    if (this._tcpClient == null)
      return;
    this._tcpClient.Close();
    this._tcpClient = (TcpClient) null;
    this._host = string.Empty;
    this._port = 0;
    NetUtil.Log("connection closed.", (object[]) Array.Empty<object>());
    SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.Disconnected);
  }

  public void RegisterCmdHandler(eNetCmd cmd, UsCmdHandler handler)
  {
    this._cmdParser.RegisterHandler(cmd, handler);
  }

  public void Tick_CheckConnectionStatus()
  {
    try
    {
      if (!this._tcpClient.Connected)
      {
        NetUtil.Log("disconnection detected. (_tcpClient.Connected == false).", (object[]) Array.Empty<object>());
        throw new Exception();
      }
      if (this._tcpClient.Client.Poll(0, SelectMode.SelectRead) && this._tcpClient.Client.Receive(new byte[1], SocketFlags.Peek) == 0)
      {
        NetUtil.Log("disconnection detected. (failed to read by Poll/Receive).", (object[]) Array.Empty<object>());
        throw new IOException();
      }
    }
    catch (Exception ex)
    {
      this.DisconnectOnError("disconnection detected while checking connection status.", ex);
    }
  }

  public void Tick_ReceivingData()
  {
    try
    {
      while (this._tcpClient.Available > 0)
      {
        byte[] buffer = new byte[2];
        int num1 = this._tcpClient.GetStream().Read(buffer, 0, buffer.Length);
        ushort uint16 = BitConverter.ToUInt16(buffer, 0);
        if (num1 > 0 && uint16 > (ushort) 0)
        {
          byte[] numArray = new byte[(int) uint16];
          int num2 = this._tcpClient.GetStream().Read(numArray, 0, numArray.Length);
          switch (this._cmdParser.Execute(new UsCmd(numArray)))
          {
            case UsCmdExecResult.Failed:
              NetUtil.Log("net cmd execution failed: {0}.", (object) new UsCmd(numArray).ReadNetCmd());
              break;
            case UsCmdExecResult.HandlerNotFound:
              NetUtil.Log("net unknown cmd: {0}.", (object) new UsCmd(numArray).ReadNetCmd());
              break;
          }
          int num3 = num2 + 1;
        }
      }
    }
    catch (Exception ex)
    {
      this.DisconnectOnError("error detected while receiving data.", ex);
    }
  }

  public void Dispose()
  {
    this.Disconnect();
  }

  public void SendPacket(UsCmd cmd)
  {
    try
    {
      byte[] bytes = BitConverter.GetBytes((ushort) cmd.WrittenLen);
      this._tcpClient.GetStream().Write(bytes, 0, bytes.Length);
      this._tcpClient.GetStream().Write(cmd.Buffer, 0, cmd.WrittenLen);
    }
    catch (Exception ex)
    {
      this.DisconnectOnError("error detected while sending data.", ex);
    }
  }

  private void OnConnect(IAsyncResult asyncResult)
  {
    TcpClient asyncState = (TcpClient) asyncResult.AsyncState;
    try
    {
      if (!asyncState.Connected)
        throw new Exception();
      NetUtil.Log("connected successfully.", (object[]) Array.Empty<object>());
      SysPost.InvokeMulticast((object) this, (MulticastDelegate) this.Connected);
    }
    catch (Exception ex)
    {
      this.DisconnectOnError("connection failed while handling OnConnect().", ex);
    }
  }

  private void DisconnectOnError(string info, Exception ex)
  {
    NetUtil.Log(info, (object[]) Array.Empty<object>());
    NetUtil.Log(ex.ToString(), (object[]) Array.Empty<object>());
    this.Disconnect();
  }
}
