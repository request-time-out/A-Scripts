// Decompiled with JetBrains decompiler
// Type: UsNet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UsNet : IDisposable
{
  private readonly object _netLocker = new object();
  private UsCmdParsing _cmdExec = new UsCmdParsing();
  public static UsNet Instance;
  private TcpListener _tcpListener;
  private TcpClient _tcpClient;
  private bool _isListening;

  public UsNet()
  {
    try
    {
      this._tcpListener = new TcpListener(IPAddress.Any, 39980);
      this._tcpListener.Start();
      this._tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClient), (object) this._tcpListener);
      this.AddToLog("usmooth listening started at: {0}.", (object) 39980);
      this._isListening = true;
    }
    catch (Exception ex)
    {
      this.AddToLog(ex.ToString(), (object[]) Array.Empty<object>());
      throw;
    }
  }

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

  ~UsNet()
  {
    this.FreeResources();
  }

  public void Dispose()
  {
    this.CloseTcpClient();
    if (this._tcpListener == null)
      return;
    this.FreeResources();
    this.AddToLog("Listening canceled.", (object[]) Array.Empty<object>());
  }

  private void FreeResources()
  {
    if (this._tcpListener == null)
      return;
    this._tcpListener.Stop();
    this._tcpListener = (TcpListener) null;
    this._isListening = false;
  }

  private void CloseTcpClient()
  {
    if (this._tcpClient == null)
      return;
    this.AddToLog(string.Format("Disconnecting client {0}.", (object) this._tcpClient.Client.RemoteEndPoint), (object[]) Array.Empty<object>());
    this._tcpClient.Close();
    this._tcpClient = (TcpClient) null;
  }

  public void Update()
  {
    if (this._tcpClient == null)
      return;
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
          if (num2 == numArray.Length)
          {
            int num3 = (int) this._cmdExec.Execute(new UsCmd(numArray));
          }
          else
            this.AddToLog(string.Format("corrupted cmd received - len: {0}", (object) num2), (object[]) Array.Empty<object>());
        }
      }
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      this.CloseTcpClient();
    }
  }

  public void SendCommand(UsCmd cmd)
  {
    if (this._tcpClient == null || this._tcpClient.GetStream() == null)
      return;
    lock (this._netLocker)
    {
      byte[] bytes = BitConverter.GetBytes((ushort) cmd.WrittenLen);
      this._tcpClient.GetStream().Write(bytes, 0, bytes.Length);
      this._tcpClient.GetStream().Write(cmd.Buffer, 0, cmd.WrittenLen);
    }
  }

  private void OnAcceptTcpClient(IAsyncResult asyncResult)
  {
    TcpListener asyncState = (TcpListener) asyncResult.AsyncState;
    if (asyncState == null)
      return;
    asyncState.BeginAcceptTcpClient(new AsyncCallback(this.OnAcceptTcpClient), (object) asyncState);
    try
    {
      this._tcpClient = asyncState.EndAcceptTcpClient(asyncResult);
      this.AddToLog(string.Format("Client {0} connected.", (object) this._tcpClient.Client.RemoteEndPoint), (object[]) Array.Empty<object>());
    }
    catch (SocketException ex)
    {
      this.AddToLog(string.Format("<color=red>Error accepting TCP connection: {0}</color>", (object) ex.Message), (object[]) Array.Empty<object>());
    }
    catch (ObjectDisposedException ex)
    {
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      this.AddToLog(string.Format("<color=red>An error occured: {0}</color>", (object) ex.Message), (object[]) Array.Empty<object>());
    }
  }

  private void AddToLog(string text, params object[] args)
  {
    string str = args.Length <= 0 ? text : string.Format(text, args);
    if (this._tcpClient != null)
      Debug.LogFormat("<color=green>{0}</color> <color=white>{1}</color>", new object[2]
      {
        (object) this._tcpClient.Client.RemoteEndPoint,
        (object) str
      });
    else
      Debug.LogFormat("<color=green>{0}</color>", new object[1]
      {
        (object) str
      });
  }
}
