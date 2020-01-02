// Decompiled with JetBrains decompiler
// Type: RuntimeCoroutineStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RuntimeCoroutineStats
{
  public static RuntimeCoroutineStats Instance = new RuntimeCoroutineStats();
  private List<CoroutineActivity> _activities = new List<CoroutineActivity>();
  private HashSet<int> _activeCoroutines = new HashSet<int>();
  private OnCoStatsBroadcast _onBroadcast;
  private OnCoStatsAnalyzer2File _onAnalyzer2File;
  private bool _broadcastStarted;

  public void MarkCreation(int seq, string mangledName)
  {
    if (!this._broadcastStarted)
    {
      Debug.LogErrorFormat("[CoStats] error: invalid broadcast while coroutine '{0}' is being created, ignored.", new object[1]
      {
        (object) seq
      });
    }
    else
    {
      CoroutineCreation coroutineCreation = new CoroutineCreation(seq);
      coroutineCreation.mangledName = mangledName;
      coroutineCreation.stacktrace = StackTraceUtility.ExtractStackTrace();
      coroutineCreation.curFrame = Time.get_frameCount();
      this._activities.Add((CoroutineActivity) coroutineCreation);
      this._activeCoroutines.Add(seq);
    }
  }

  public void MarkMoveNext(int seq, float timeConsumed)
  {
    if (!this._broadcastStarted)
      Debug.LogErrorFormat("[CoStats] error: invalid broadcast while coroutine '{0}' is performing MoveNext(), ignored.", new object[1]
      {
        (object) seq
      });
    else if (!this._activeCoroutines.Contains(seq))
    {
      Debug.LogErrorFormat("[CoStats] error: coroutine '{0}' is performing MoveNext() but could not be found in '_activeCoroutines', ignored.", new object[1]
      {
        (object) seq
      });
    }
    else
    {
      CoroutineExecution coroutineExecution = new CoroutineExecution(seq);
      coroutineExecution.timeConsumed = timeConsumed;
      coroutineExecution.curFrame = Time.get_frameCount();
      this._activities.Add((CoroutineActivity) coroutineExecution);
    }
  }

  public void MarkTermination(int seq)
  {
    if (!this._broadcastStarted)
      Debug.LogErrorFormat("[CoStats] error: invalid broadcast while coroutine '{0}' is being terminated, ignored.", new object[1]
      {
        (object) seq
      });
    else if (!this._activeCoroutines.Contains(seq))
    {
      Debug.LogErrorFormat("[CoStats] error: coroutine '{0}' is terminating but could not be found in '_activeCoroutines', ignored.", new object[1]
      {
        (object) seq
      });
    }
    else
    {
      CoroutineTermination coroutineTermination = new CoroutineTermination(seq);
      coroutineTermination.curFrame = Time.get_frameCount();
      this._activities.Add((CoroutineActivity) coroutineTermination);
      this._activeCoroutines.Remove(seq);
    }
  }

  [DebuggerHidden]
  public IEnumerator BroadcastCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new RuntimeCoroutineStats.\u003CBroadcastCoroutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public event OnCoStatsBroadcast OnBroadcast
  {
    add
    {
      this._onBroadcast -= value;
      this._onBroadcast += value;
    }
    remove
    {
      this._onBroadcast -= value;
    }
  }

  public event OnCoStatsAnalyzer2File OnAnalyzer2File
  {
    add
    {
      this._onAnalyzer2File -= value;
      this._onAnalyzer2File += value;
    }
    remove
    {
      this._onAnalyzer2File -= value;
    }
  }

  private bool hasBroadcastReceivers()
  {
    return this._onBroadcast != null && this._onBroadcast.GetInvocationList().Length > 0;
  }

  private bool hasCoStatsAnalyzer2File()
  {
    return this._onAnalyzer2File != null && this._onAnalyzer2File.GetInvocationList().Length > 0;
  }
}
