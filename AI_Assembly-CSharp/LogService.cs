// Decompiled with JetBrains decompiler
// Type: LogService
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using UnityEngine;

public class LogService : IDisposable
{
  private bool _useMemBuf = true;
  public static int UserDefinedMemBufSize;
  private string _logPath;
  private StreamWriter _logWriter;
  private ushort _seqID;
  private int _assertCount;
  private int _errorCount;
  private int _exceptionCount;
  private bool _disposed;
  private LogBuffer _memBuf;
  private string _lastWrittenContent;
  private LogType _lastWrittenType;
  private int _foldedCount;
  private bool _reentranceGuard;

  public LogService(bool logIntoFile, int oldLogsKeptDays, bool useMemBuf)
  {
    this.RegisterCallback();
    this._useMemBuf = useMemBuf;
    this._memBuf = new LogBuffer(LogService.UserDefinedMemBufSize);
    if (logIntoFile)
    {
      try
      {
        DateTime now = DateTime.Now;
        string str = LogUtil.CombinePaths(Application.get_persistentDataPath(), "log", LogUtil.FormatDateAsFileNameString(now));
        Directory.CreateDirectory(str);
        string fileName = Path.Combine(str, LogUtil.FormatDateAsFileNameString(now) + (object) '_' + LogUtil.FormatTimeAsFileNameString(now) + ".txt");
        this._logWriter = new System.IO.FileInfo(fileName).CreateText();
        this._logWriter.AutoFlush = true;
        this._logPath = fileName;
        Log.Info((object) "'Log Into File' enabled, file opened successfully. ('{0}')", (object) this._logPath);
        LogService.LastLogFile = this._logPath;
      }
      catch (Exception ex)
      {
        Log.Info((object) "'Log Into File' enabled but failed to open file.", (object[]) Array.Empty<object>());
        Log.Exception(ex);
      }
    }
    else
    {
      Log.Info((object) "'Log Into File' disabled.", (object[]) Array.Empty<object>());
      LogService.LastLogFile = string.Empty;
    }
    if (oldLogsKeptDays <= 0)
      return;
    try
    {
      this.CleanupLogsOlderThan(oldLogsKeptDays);
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
      Log.Error((object) "Cleaning up logs ({0}) failed.", (object) oldLogsKeptDays);
    }
  }

  public event LogTargetHandler LogTargets;

  public bool UseMemBuf
  {
    get
    {
      return this._useMemBuf;
    }
    set
    {
      this._useMemBuf = value;
      this.FlushLogWriting();
    }
  }

  public void Dispose()
  {
    this.FlushLogWriting();
    Log.Info((object) "destroying log service...", (object[]) Array.Empty<object>());
    if (this._logWriter != null)
      this._logWriter.Close();
    this._disposed = true;
  }

  public void WriteLog(string content, LogType type)
  {
    if (LogUtil.EnableInMemoryStorage)
    {
      if (type != null)
      {
        if (type == 4)
          LogUtil.PushInMemoryException(content);
      }
      else
        LogUtil.PushInMemoryError(content);
    }
    if (this._useMemBuf)
    {
      if (type == null || Encoding.Default.GetByteCount(content) > this._memBuf.BufSize)
      {
        this.FlushMemBuffer();
        if (this._logWriter == null)
          return;
        this._logWriter.Write(content);
      }
      else
      {
        if (this._memBuf.Receive(content))
          return;
        this.FlushMemBuffer();
        this._memBuf.Receive(content);
      }
    }
    else
    {
      if (this._logWriter == null)
        return;
      this._logWriter.Write(content);
    }
  }

  public void FlushLogWriting()
  {
    this.FlushFoldedMessage();
  }

  private void CleanupLogsOlderThan(int days)
  {
    string strB = LogUtil.FormatDateAsFileNameString(DateTime.Now.Subtract(TimeSpan.FromDays((double) days)));
    DirectoryInfo[] directories = new DirectoryInfo(LogUtil.CombinePaths(Application.get_persistentDataPath(), "log")).GetDirectories();
    List<string> stringList = new List<string>();
    foreach (DirectoryInfo directoryInfo in directories)
    {
      if (string.CompareOrdinal(directoryInfo.Name, strB) <= 0)
        stringList.Add(directoryInfo.FullName);
    }
    foreach (string path in stringList)
    {
      Directory.Delete(path, true);
      Log.Info((object) "[ Log Cleanup ]: {0}", (object) path);
    }
  }

  private void RegisterCallback()
  {
    // ISSUE: method pointer
    Application.add_logMessageReceivedThreaded(new Application.LogCallback((object) this, __methodptr(OnLogReceived)));
  }

  private void WriteTrace(string content)
  {
    this.OnLogReceived(content, string.Empty, (LogType) 3);
  }

  private void OnLogReceived(string condition, string stackTrace, LogType type)
  {
    if (this._disposed)
      throw new Exception(string.Format("LogService used after being disposed. (content:{0})", (object) condition));
    if (this._reentranceGuard)
      throw new Exception(string.Format("LogService Reentrance occurred. (content:{0})", (object) condition));
    this._reentranceGuard = true;
    ++this._seqID;
    switch ((int) type)
    {
      case 0:
        ++this._errorCount;
        break;
      case 1:
        ++this._assertCount;
        break;
      case 4:
        ++this._exceptionCount;
        break;
    }
    try
    {
      if (type == 4)
        condition = string.Format("{0}\r\n  {1}", (object) condition, (object) stackTrace.Replace("\n", "\r\n  "));
      if (condition == this._lastWrittenContent)
      {
        ++this._foldedCount;
      }
      else
      {
        this.FlushFoldedMessage();
        this.WriteLog(string.Format("{0:0.00} {1}: {2}\r\n", (object) Time.get_realtimeSinceStartup(), (object) type, (object) condition), type);
        this._lastWrittenContent = condition;
        this._lastWrittenType = type;
      }
      if (this.LogTargets == null)
        return;
      foreach (LogTargetHandler invocation in this.LogTargets.GetInvocationList())
      {
        ISynchronizeInvoke target = invocation.Target as ISynchronizeInvoke;
        LogEventArgs args = new LogEventArgs((int) this._seqID, type, condition, stackTrace, Time.get_realtimeSinceStartup());
        if (target != null && target.InvokeRequired)
          target.Invoke((Delegate) invocation, new object[2]
          {
            (object) this,
            (object) args
          });
        else
          invocation((object) this, args);
      }
    }
    catch (Exception ex)
    {
      Log.Exception(ex);
    }
    finally
    {
      this._reentranceGuard = false;
    }
  }

  private void FlushMemBuffer()
  {
    if (!this._useMemBuf)
      return;
    if (this._logWriter != null && this._memBuf.BufWrittenBytes > 0)
      this._logWriter.Write(Encoding.Default.GetString(this._memBuf.Buf, 0, this._memBuf.BufWrittenBytes));
    this._memBuf.Clear();
  }

  private void FlushFoldedMessage()
  {
    this.FlushMemBuffer();
    if (this._foldedCount <= 0)
      return;
    if (this._logWriter != null)
      this._logWriter.Write(string.Format("{0:0.00} {1}: --<< folded {2} messages >>--\r\n", (object) Time.get_realtimeSinceStartup(), (object) this._lastWrittenType, (object) this._foldedCount));
    this._foldedCount = 0;
  }

  public static string LastLogFile { get; set; }
}
