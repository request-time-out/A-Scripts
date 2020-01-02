// Decompiled with JetBrains decompiler
// Type: LoadingStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AClockworkBerry;
using System.Text;
using UnityEngine;

public class LoadingStats : MonoBehaviour
{
  public static LoadingStats Instance;
  private StringBuilder m_strBuilder;

  public LoadingStats()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    ((Behaviour) ScreenLogger.Instance).set_enabled(false);
    ScreenLogger.Instance.LogMessages = false;
    ScreenLogger.Instance.LogWarnings = false;
    ScreenLogger.Instance.LogErrors = false;
  }

  private void OnEnable()
  {
    ((Behaviour) ScreenLogger.Instance).set_enabled(true);
    ScreenLogger.Instance.Clear();
  }

  private void OnDisable()
  {
    if (!Object.op_Inequality((Object) ScreenLogger.Instance, (Object) null))
      return;
    ((Behaviour) ScreenLogger.Instance).set_enabled(false);
    ScreenLogger.Instance.Clear();
  }

  public void LogLua(string path, int sizeInBytes, bool loadFromCache)
  {
    this.m_strBuilder.Length = 0;
    this.m_strBuilder.AppendFormat("{0:0.00} ({1:0.00}kb) {2} {3}", (object) Time.get_time(), (object) ((double) sizeInBytes / 1024.0), !loadFromCache ? (object) "#LuaIO" : (object) "#LuaCache", (object) path);
    ScreenLogger.Instance.EnqueueDirectly(this.m_strBuilder.ToString(), (LogType) 3);
  }

  public void LogSync(string path, double duration)
  {
    this.m_strBuilder.Length = 0;
    this.m_strBuilder.AppendFormat("{0:0.00} ({1:0.00}ms) #Sync {2}", (object) Time.get_time(), (object) duration, (object) path);
    ScreenLogger.Instance.EnqueueDirectly(this.m_strBuilder.ToString(), (LogType) 3);
    ScreenLogger.Instance.SyncTopN.TryAdd(duration, path);
  }

  public void LogAsync(string path, double duration)
  {
    this.m_strBuilder.Length = 0;
    this.m_strBuilder.AppendFormat("{0:0.00} ({1:0.00}ms) #Async {2}", (object) Time.get_time(), (object) duration, (object) path);
    ScreenLogger.Instance.EnqueueDirectly(this.m_strBuilder.ToString(), (LogType) 2);
    ScreenLogger.Instance.AsyncTopN.TryAdd(duration, path);
  }
}
