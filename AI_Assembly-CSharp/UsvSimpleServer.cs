// Decompiled with JetBrains decompiler
// Type: UsvSimpleServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using UnityEngine;

public class UsvSimpleServer : IDisposable
{
  private Dictionary<string, TcpClient> _tcpClients = new Dictionary<string, TcpClient>();
  private UsCmdParsing _cmdExec = new UsCmdParsing();
  private List<string> _toBeRemoved = new List<string>();
  private Dictionary<string, UsvClientConsoleCmdHandler> _clientConsoleCmdHandlers = new Dictionary<string, UsvClientConsoleCmdHandler>();
  private TcpListener _tcpListener;
  private bool _isListening;

  public UsvSimpleServer()
  {
    try
    {
      this._cmdExec.RegisterClientHandler(eNetCmd.CL_Handshake, new UsClientCmdHandler(this.NetHandle_Handshake));
      this._cmdExec.RegisterClientHandler(eNetCmd.CL_KeepAlive, new UsClientCmdHandler(this.NetHandle_KeepAlive));
      this._cmdExec.RegisterClientHandler(eNetCmd.CL_ExecCommand, new UsClientCmdHandler(this.NetHandle_ExecCommand));
      this._tcpListener = new TcpListener(IPAddress.Any, 39981);
      this._tcpListener.Start();
      this._tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClient), (object) this._tcpListener);
      this.NetLog("simple server listening started at: {0}.", (object) 39981);
      this._isListening = true;
    }
    catch (Exception ex)
    {
      this.NetLog(ex.ToString(), (object[]) Array.Empty<object>());
      throw;
    }
  }

  public event UsvClientDisconnectedHandler ClientDisconnected;

  public UsCmdParsing CmdExecutor
  {
    get
    {
      return this._cmdExec;
    }
  }

  public bool IsListening
  {
    get
    {
      return this._isListening;
    }
  }

  public void Dispose()
  {
    foreach (TcpClient tcpClient in this._tcpClients.Values)
    {
      if (tcpClient != null)
      {
        this.NetLog(string.Format("Disconnecting client {0}.", (object) tcpClient.Client.RemoteEndPoint), (object[]) Array.Empty<object>());
        tcpClient.Close();
      }
    }
    this._tcpClients.Clear();
    if (this._tcpListener == null)
      return;
    this._tcpListener.Stop();
    this._tcpListener = (TcpListener) null;
    this._isListening = false;
    this.NetLog("Listening ended.", (object[]) Array.Empty<object>());
  }

  public void Update()
  {
    using (Dictionary<string, TcpClient>.Enumerator enumerator = this._tcpClients.GetEnumerator())
    {
label_12:
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, TcpClient> current = enumerator.Current;
        TcpClient tcpClient = current.Value;
        try
        {
          while (true)
          {
            int num1;
            ushort uint16;
            do
            {
              if (tcpClient != null)
              {
                if (tcpClient.Available > 0)
                {
                  byte[] buffer = new byte[2];
                  num1 = tcpClient.GetStream().Read(buffer, 0, buffer.Length);
                  uint16 = BitConverter.ToUInt16(buffer, 0);
                }
                else
                  goto label_12;
              }
              else
                goto label_12;
            }
            while (num1 <= 0 || uint16 <= (ushort) 0);
            byte[] numArray = new byte[(int) uint16];
            int num2 = tcpClient.GetStream().Read(numArray, 0, numArray.Length);
            if (num2 == numArray.Length)
            {
              int num3 = (int) this._cmdExec.ExecuteClient(current.Key, new UsCmd(numArray));
            }
            else
              this.NetLog(string.Format("corrupted cmd received - len: {0}", (object) num2), (object[]) Array.Empty<object>());
          }
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          tcpClient.Close();
          this._toBeRemoved.Add(current.Key);
          if (this.ClientDisconnected != null)
            this.ClientDisconnected(current.Key);
        }
      }
    }
    foreach (string key in this._toBeRemoved)
      this._tcpClients.Remove(key);
  }

  public TcpClient FindClient(string clientID)
  {
    TcpClient tcpClient = (TcpClient) null;
    if (!this._tcpClients.TryGetValue(clientID, out tcpClient))
    {
      this.NetLog("unknown client: {0}", (object) clientID);
      return (TcpClient) null;
    }
    if (tcpClient != null && tcpClient.GetStream() != null)
      return tcpClient;
    this.NetLog("bad client: {0}", (object) clientID);
    return (TcpClient) null;
  }

  public void SendCommand(string clientID, UsCmd cmd)
  {
    TcpClient client = this.FindClient(clientID);
    if (client == null)
      return;
    byte[] bytes = BitConverter.GetBytes((ushort) cmd.WrittenLen);
    client.GetStream().Write(bytes, 0, bytes.Length);
    client.GetStream().Write(cmd.Buffer, 0, cmd.WrittenLen);
  }

  private void OnAcceptTcpClient(IAsyncResult asyncResult)
  {
    TcpListener asyncState = (TcpListener) asyncResult.AsyncState;
    if (asyncState == null)
      return;
    asyncState.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClient), (object) asyncState);
    try
    {
      TcpClient tcpClient = asyncState.EndAcceptTcpClient(asyncResult);
      this._tcpClients.Add(tcpClient.Client.RemoteEndPoint.ToString(), tcpClient);
      this.NetLog(string.Format("Client {0} connected.", (object) tcpClient.Client.RemoteEndPoint), (object[]) Array.Empty<object>());
    }
    catch (SocketException ex)
    {
      this.NetLog(string.Format("<color=red>Error accepting TCP connection: {0}</color>", (object) ex.Message), (object[]) Array.Empty<object>());
    }
    catch (ObjectDisposedException ex)
    {
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      this.NetLog(string.Format("<color=red>An error occured: {0}</color>", (object) ex.Message), (object[]) Array.Empty<object>());
    }
  }

  private void NetLog(string text, params object[] args)
  {
    Debug.LogFormat("<color=green>{0}</color>", new object[1]
    {
      (object) (args.Length <= 0 ? text : string.Format(text, args))
    });
  }

  private void NetLogClient(string clientID, string text, params object[] args)
  {
    string str = args.Length <= 0 ? text : string.Format(text, args);
    Debug.LogFormat("<color=green>{0}</color> <color=white>{1}</color>", new object[2]
    {
      (object) clientID,
      (object) str
    });
  }

  private bool NetHandle_Handshake(string clientID, eNetCmd cmd, UsCmd c)
  {
    this.NetLog("executing handshake.", (object[]) Array.Empty<object>());
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_HandshakeResponse);
    this.SendCommand(clientID, cmd1);
    return true;
  }

  private bool NetHandle_KeepAlive(string clientID, eNetCmd cmd, UsCmd c)
  {
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_KeepAliveResponse);
    this.SendCommand(clientID, cmd1);
    return true;
  }

  public void RegisterHandlerClass(System.Type handlerClassType, object handlerInst)
  {
    foreach (MethodInfo method in handlerClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    {
      foreach (object customAttribute in method.GetCustomAttributes(typeof (ClientConsoleCmdHandler), false))
      {
        if (customAttribute is ClientConsoleCmdHandler consoleCmdHandler)
        {
          try
          {
            Delegate @delegate = Delegate.CreateDelegate(typeof (UsvClientConsoleCmdHandler), handlerInst, method);
            if ((object) @delegate != null)
              this._clientConsoleCmdHandlers[consoleCmdHandler.Command.ToLower()] = (UsvClientConsoleCmdHandler) @delegate;
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
          }
        }
      }
    }
  }

  private bool NetHandle_ExecCommand(string clientID, eNetCmd cmd, UsCmd c)
  {
    string str = c.ReadString();
    string[] args = str.Split((char[]) Array.Empty<char>());
    if (args.Length == 0)
    {
      Log.Info((object) "empty command received, ignored.", (object[]) Array.Empty<object>());
      return false;
    }
    UsvClientConsoleCmdHandler consoleCmdHandler;
    if (!this._clientConsoleCmdHandlers.TryGetValue(args[0].ToLower(), out consoleCmdHandler))
    {
      Log.Info((object) "unknown command ('{0}') received, ignored.", (object) str);
      return false;
    }
    bool flag = consoleCmdHandler(clientID, args);
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_ExecCommandResponse);
    cmd1.WriteInt32(!flag ? 0 : 1);
    this.SendCommand(clientID, cmd1);
    return true;
  }
}
