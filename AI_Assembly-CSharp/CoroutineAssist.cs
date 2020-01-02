// Decompiled with JetBrains decompiler
// Type: CoroutineAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class CoroutineAssist
{
  private MonoBehaviour mono;
  private Func<IEnumerator> func;
  private IEnumerator nowFunc;
  private float timeoutTime;
  private bool enableTimeout;
  private float startTime;

  public CoroutineAssist(MonoBehaviour _mono, Func<IEnumerator> _func)
  {
    this.nowFunc = (IEnumerator) null;
    this.status = CoroutineAssist.Status.Idle;
    this.timeoutTime = 0.0f;
    this.enableTimeout = false;
    this.startTime = 0.0f;
    this.mono = _mono;
    this.func = _func;
  }

  public CoroutineAssist.Status status { get; set; }

  public bool IsIdle()
  {
    return this.status == CoroutineAssist.Status.Idle;
  }

  public bool IsRun()
  {
    return this.status == CoroutineAssist.Status.Run;
  }

  public bool IsPause()
  {
    return this.status == CoroutineAssist.Status.Pause;
  }

  public bool Start(bool _enableTimeout = false, float _timeout = 20f)
  {
    if (this.func == null)
      return false;
    if (this.status != CoroutineAssist.Status.Idle)
    {
      Debug.LogWarning((object) "すでに開始されています。");
      return false;
    }
    this.nowFunc = this.func();
    this.status = CoroutineAssist.Status.Run;
    if (_enableTimeout)
      this.StartTimeoutCheck(_timeout);
    this.mono.StartCoroutine(this.nowFunc);
    return true;
  }

  public bool Restart()
  {
    if (this.nowFunc == null)
      return false;
    this.mono.StartCoroutine(this.nowFunc);
    this.status = CoroutineAssist.Status.Run;
    this.startTime = Time.get_realtimeSinceStartup();
    return true;
  }

  public void Pause()
  {
    if (this.nowFunc == null)
      return;
    this.mono.StopCoroutine(this.nowFunc);
    this.status = CoroutineAssist.Status.Pause;
  }

  public void End()
  {
    if (this.nowFunc == null)
      return;
    this.mono.StopCoroutine(this.nowFunc);
    this.EndStatus();
  }

  public void EndStatus()
  {
    this.nowFunc = (IEnumerator) null;
    this.status = CoroutineAssist.Status.Idle;
  }

  public void StartTimeoutCheck(float _timeout = 20f)
  {
    this.enableTimeout = true;
    this.timeoutTime = _timeout;
    this.startTime = Time.get_realtimeSinceStartup();
  }

  public void EndTimeoutCheck()
  {
    this.enableTimeout = false;
    this.timeoutTime = 0.0f;
    this.startTime = 0.0f;
  }

  public bool TimeOutCheck()
  {
    if (this.status != CoroutineAssist.Status.Run || !this.enableTimeout || (double) (Time.get_realtimeSinceStartup() - this.startTime) <= (double) this.timeoutTime)
      return false;
    this.enableTimeout = false;
    this.End();
    return true;
  }

  public enum Status
  {
    Idle,
    Run,
    Pause,
  }
}
