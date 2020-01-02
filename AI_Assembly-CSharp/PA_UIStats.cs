// Decompiled with JetBrains decompiler
// Type: PA_UIStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PA_UIStats
{
  public List<PA_UIFrameStats> _lastSecFrames = new List<PA_UIFrameStats>(50);
  public List<PA_UIFrameStats> _cachedFrames = new List<PA_UIFrameStats>(50);
  private PA_UIFrameStats _accum = new PA_UIFrameStats();
  private PA_UIFrameStats _max = new PA_UIFrameStats();
  public static PA_UIStats Instance;
  public PA_UIFrameStats _curFrame;
  private float _lastWriteTime;

  public void BeginFrame()
  {
    if (this._cachedFrames.Count > 0)
    {
      this._curFrame = this._cachedFrames[this._cachedFrames.Count - 1];
      this._cachedFrames.RemoveAt(this._cachedFrames.Count - 1);
    }
    else
      this._curFrame = new PA_UIFrameStats();
  }

  public void EndFrame()
  {
    this._lastSecFrames.Add(this._curFrame);
    this._curFrame = (PA_UIFrameStats) null;
    if ((double) (Time.get_realtimeSinceStartup() - this._lastWriteTime) < (double) PA_UIStatsConst.WriteInterval)
      return;
    Debug.LogWarning((object) this.GenerateStatsInfo());
    for (int index = 0; index < this._lastSecFrames.Count; ++index)
      this._lastSecFrames[index].Clear();
    this._cachedFrames.AddRange((IEnumerable<PA_UIFrameStats>) this._lastSecFrames);
    this._lastSecFrames.Clear();
    this._lastWriteTime = Mathf.Floor(Time.get_realtimeSinceStartup());
  }

  private string GenerateStatsInfo()
  {
    this._accum.Clear();
    this._max.Clear();
    for (int index = 0; index < this._lastSecFrames.Count; ++index)
    {
      this._accum._wtbCnt += this._lastSecFrames[index]._wtbCnt;
      this._accum._wtbU1Cnt += this._lastSecFrames[index]._wtbU1Cnt;
      this._accum._wtbNormCnt += this._lastSecFrames[index]._wtbNormCnt;
      this._accum._totalVertCount += this._lastSecFrames[index]._totalVertCount;
      this._max._wtbCnt = Mathf.Max(this._lastSecFrames[index]._wtbCnt, this._max._wtbCnt);
      this._max._wtbU1Cnt = Mathf.Max(this._lastSecFrames[index]._wtbU1Cnt, this._max._wtbU1Cnt);
      this._max._wtbNormCnt = Mathf.Max(this._lastSecFrames[index]._wtbNormCnt, this._max._wtbNormCnt);
      this._max._totalVertCount = Mathf.Max(this._lastSecFrames[index]._totalVertCount, this._max._totalVertCount);
    }
    return string.Format("{0} {1} -- {2} {3}", (object) string.Format("accum: <cnt: {0} u1: {1} norm: {2}, vert:{3}>", (object) this._accum._wtbCnt, (object) this._accum._wtbU1Cnt, (object) this._accum._wtbNormCnt, (object) this._accum._totalVertCount), (object) string.Format("max: <cnt: {0} u1: {1} norm: {2}, vert:{3}>", (object) this._max._wtbCnt, (object) this._max._wtbU1Cnt, (object) this._max._wtbNormCnt, (object) this._max._totalVertCount), (object) string.Empty, (object) string.Empty);
  }
}
