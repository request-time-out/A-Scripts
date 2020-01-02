// Decompiled with JetBrains decompiler
// Type: AssetUsageStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetUsageStats : IDisposable
{
  private DateTime _lastWriteTime = DateTime.Now;
  private string _logDir = string.Empty;
  private GameObject _luaAsset = new GameObject("LuaAsset");
  private Stopwatch m_syncTimer = new Stopwatch();
  private Dictionary<object, AssetRequestInfo> InProgressAsyncObjects = new Dictionary<object, AssetRequestInfo>();
  public static AssetUsageStats Instance;
  private bool _enableTracking;
  private StreamWriter _logWriter;

  public AssetUsageStats(bool enableTracking)
  {
    this._enableTracking = enableTracking;
    if (!this._enableTracking)
      return;
    try
    {
      this._logDir = Path.Combine(Application.get_persistentDataPath(), "asset_stats");
      if (Directory.Exists(this._logDir))
        return;
      Directory.CreateDirectory(this._logDir);
    }
    catch (Exception ex)
    {
      this._logDir = string.Empty;
      this._enableTracking = false;
      this.LogError(ex.Message);
      this.LogError("Failed to prepare the stats dir, aborted.");
    }
  }

  public bool EnableTracking
  {
    get
    {
      return this._enableTracking;
    }
  }

  private void LogInfo(string info)
  {
    Debug.LogFormat("[AssetUsageStats] (info): {0} ", new object[1]
    {
      (object) info
    });
  }

  private void LogError(string err)
  {
    Debug.LogErrorFormat("[AssetUsageStats] (err): {0} ", new object[1]
    {
      (object) err
    });
  }

  public bool PrepareWriter()
  {
    if (!this._enableTracking)
      return false;
    DateTime now = DateTime.Now;
    if (this._logWriter != null)
    {
      if (now.Hour == this._lastWriteTime.Hour && now.Minute / 10 == this._lastWriteTime.Minute / 10)
        return true;
      this.LogInfo("Switching file at: " + now.ToString());
      this.CloseWriter();
    }
    try
    {
      string str1 = !Application.get_isEditor() ? IPManager.GetIP(ADDRESSFAM.IPv4) : "editor";
      string str2 = "000";
      string str3 = "000";
      string path = Path.Combine(this._logDir, string.Format("{0}_{1}-{2}_{3}_{4}.txt", (object) str1, (object) SysUtil.FormatDateAsFileNameString(now), (object) SysUtil.FormatTimeAsFileNameString(now), (object) str2, (object) str3));
      if (!File.Exists(path))
      {
        File.Create(path).Dispose();
        this.LogInfo("Creating new text successfully at: " + path);
      }
      if (this._logWriter == null)
      {
        this._logWriter = new StreamWriter(path, true);
        this._logWriter.AutoFlush = true;
      }
      return true;
    }
    catch (Exception ex)
    {
      this._enableTracking = false;
      this.LogError("Creating new text failed: " + ex.Message);
      this.CloseWriter();
      return false;
    }
  }

  public void CloseWriter()
  {
    if (this._logWriter == null)
      return;
    try
    {
      this._logWriter.Close();
    }
    catch (Exception ex)
    {
      this.LogError(ex.Message);
    }
    finally
    {
      this._logWriter = (StreamWriter) null;
      this.LogInfo("Writer closed.");
    }
  }

  public void Dispose()
  {
    if (!this._enableTracking || this._logWriter == null)
      return;
    this.CloseWriter();
    this._enableTracking = false;
  }

  public void TrackLuaRequest(string path, int bytes, bool loadFromCache)
  {
    if (!this._enableTracking)
      return;
    LoadingStats.Instance.LogLua(path, bytes, loadFromCache);
    AssetRequestInfo req = this.NewRequest(path);
    req.requestType = ResourceRequestType.Ordinary;
    this.TrackRequestWithObject(req, (Object) this._luaAsset);
  }

  public void TrackSyncStartTiming()
  {
    if (!this._enableTracking)
      return;
    this.m_syncTimer.Reset();
    this.m_syncTimer.Start();
  }

  public double TrackSyncStopTiming(string path)
  {
    if (!this._enableTracking)
      return 0.0;
    this.m_syncTimer.Stop();
    double duration = (double) this.m_syncTimer.ElapsedTicks / 10000.0;
    LoadingStats.Instance.LogSync(path, duration);
    return duration;
  }

  public void TrackSyncRequest(Object spawned, string path)
  {
    if (!this._enableTracking)
      return;
    double num = this.TrackSyncStopTiming(path);
    AssetRequestInfo req = this.NewRequest(path);
    req.duration = num;
    req.requestType = ResourceRequestType.Ordinary;
    this.TrackRequestWithObject(req, spawned);
  }

  public void TrackResourcesDotLoad(Object loaded, string path)
  {
    if (!this._enableTracking)
      return;
    double num = this.TrackSyncStopTiming(path);
    AssetRequestInfo req = this.NewRequest(path);
    req.duration = num;
    req.requestType = ResourceRequestType.Ordinary;
    this.TrackRequestWithObject(req, loaded);
  }

  public void TrackAsyncRequest(object handle, string path)
  {
    if (!this._enableTracking)
      return;
    this.InProgressAsyncObjects[handle] = this.NewRequest(path);
  }

  public void TrackAsyncDone(object handle, Object target)
  {
    AssetRequestInfo req;
    if (!this._enableTracking || Object.op_Equality(target, (Object) null) || !this.InProgressAsyncObjects.TryGetValue(handle, out req))
      return;
    req.requestType = ResourceRequestType.Async;
    req.duration = 0.0;
    LoadingStats.Instance.LogAsync(req.resourcePath, 0.0);
    this.TrackRequestWithObject(req, target);
    this.InProgressAsyncObjects.Remove(handle);
  }

  public void TrackSceneLoaded(string sceneName)
  {
    if (!this._enableTracking)
      return;
    Scene activeScene = SceneManager.GetActiveScene();
    foreach (Object rootGameObject in ((Scene) ref activeScene).GetRootGameObjects())
      this.TrackSyncRequest(rootGameObject, sceneName + ".unity");
  }

  private AssetRequestInfo NewRequest(string path)
  {
    return new AssetRequestInfo() { resourcePath = path };
  }

  private void TrackRequestWithObject(AssetRequestInfo req, Object obj)
  {
    if (Object.op_Equality(obj, (Object) null) || !this._enableTracking || !this.PrepareWriter())
      return;
    if (!((Behaviour) LoadingStats.Instance).get_enabled())
      return;
    try
    {
      req.RecordObject(obj);
      string str = req.ToString();
      if (this._logWriter != null && !string.IsNullOrEmpty(str) && req.duration >= 1.0)
        this._logWriter.WriteLine(str);
      this._lastWriteTime = DateTime.Now;
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("[ResourceTracker.TrackRequestWithObject] error: {0} \n {1} \n {2}", new object[3]
      {
        (object) ex.Message,
        req == null ? (object) string.Empty : (object) req.ToString(),
        (object) ex.StackTrace
      });
    }
  }

  private static void ThreadedUploading(object obj)
  {
    if (obj is AssetUsageStats)
      ;
  }

  private void UploadFiles()
  {
    // ISSUE: reference to a compiler-generated field
    if (AssetUsageStats.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      AssetUsageStats.\u003C\u003Ef__mg\u0024cache0 = new ParameterizedThreadStart(AssetUsageStats.ThreadedUploading);
    }
    // ISSUE: reference to a compiler-generated field
    Thread thread = new Thread(AssetUsageStats.\u003C\u003Ef__mg\u0024cache0);
    thread.Start((object) this);
    thread.Join();
  }
}
