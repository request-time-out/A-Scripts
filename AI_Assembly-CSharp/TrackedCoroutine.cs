// Decompiled with JetBrains decompiler
// Type: TrackedCoroutine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

public class TrackedCoroutine : IEnumerator
{
  private int _seqID;
  private IEnumerator _routine;
  private string _mangledName;
  private static Stopwatch _stopWatch;
  private static int _seqNext;

  public TrackedCoroutine(IEnumerator routine)
  {
    this._routine = routine;
    this._mangledName = CoroutineNameCache.Mangle(this._routine.GetType().ToString());
    this._seqID = TrackedCoroutine._seqNext++;
    RuntimeCoroutineStats.Instance.MarkCreation(this._seqID, this._mangledName);
  }

  object IEnumerator.Current
  {
    get
    {
      return this._routine.Current;
    }
  }

  public bool MoveNext()
  {
    if (TrackedCoroutine._stopWatch == null)
      TrackedCoroutine._stopWatch = Stopwatch.StartNew();
    TrackedCoroutine._stopWatch.Reset();
    TrackedCoroutine._stopWatch.Start();
    bool flag = this._routine.MoveNext();
    TrackedCoroutine._stopWatch.Stop();
    float timeConsumed = (float) TrackedCoroutine._stopWatch.ElapsedTicks / (float) Stopwatch.Frequency;
    RuntimeCoroutineStats.Instance.MarkMoveNext(this._seqID, timeConsumed);
    if (!flag)
      RuntimeCoroutineStats.Instance.MarkTermination(this._seqID);
    return flag;
  }

  public void Reset()
  {
    this._routine.Reset();
  }
}
