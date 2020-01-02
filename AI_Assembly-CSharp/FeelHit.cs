// Decompiled with JetBrains decompiler
// Type: FeelHit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

public class FeelHit
{
  private List<FeelHit.FeelInfo> lstHitInfo = new List<FeelHit.FeelInfo>();

  public void FeelHitInit(int _personality = 0)
  {
    int key = _personality;
    if (key == 90)
      key = 5;
    if (!Singleton<Resources>.Instance.HSceneTable.DicLstHitInfo.TryGetValue(key, out this.lstHitInfo))
      this.lstHitInfo = new List<FeelHit.FeelInfo>();
    if (this.lstHitInfo.Count == 0)
      return;
    foreach (FeelHit.FeelInfo feelInfo in this.lstHitInfo)
    {
      feelInfo.CreateHit();
      feelInfo.InitTime();
    }
  }

  public bool isHit(int _state, int _loop, float _power)
  {
    Vector2 hitArea = this.GetHitArea(_state, _loop);
    return GlobalMethod.RangeOn<float>(_power, (float) hitArea.x, (float) hitArea.y);
  }

  public Vector2 GetHitArea(int _state, int _loop)
  {
    return _state >= this.lstHitInfo.Count ? new Vector2(1.1f, 1.1f) : this.lstHitInfo[_state].Get(_loop);
  }

  public bool ChangeHit(int _state, int _loop)
  {
    return this.lstHitInfo[_state].ChangeHit(_loop);
  }

  public void InitTime()
  {
    for (int index = 0; index < this.lstHitInfo.Count; ++index)
      this.lstHitInfo[index].InitTime();
  }

  public struct FeelHitInfo
  {
    public Vector2 area;
    public float rate;
  }

  public class FeelInfo
  {
    public List<Vector2> lstHit = new List<Vector2>();
    public List<FeelHit.FeelHitInfo> lstHitArea = new List<FeelHit.FeelHitInfo>();
    public List<float> lstChangeTime = new List<float>();
    public List<float> lstChangeDeltaTime = new List<float>();
    private Vector2 tmpv;

    public void CreateHit()
    {
      foreach (FeelHit.FeelHitInfo feelHitInfo in this.lstHitArea)
      {
        this.tmpv = Vector2.get_zero();
        this.tmpv.x = (__Null) (double) Random.Range(5, 95 - (int) feelHitInfo.rate);
        this.tmpv.y = (__Null) (this.tmpv.x + (double) feelHitInfo.rate);
        this.lstHit.Add(this.tmpv);
      }
    }

    public bool ChangeHit(int _state)
    {
      if (_state >= this.lstChangeTime.Count || _state >= this.lstChangeDeltaTime.Count || (_state >= this.lstHitArea.Count || _state >= this.lstHit.Count))
        return false;
      List<float> lstChangeDeltaTime;
      int index;
      (lstChangeDeltaTime = this.lstChangeDeltaTime)[index = _state] = lstChangeDeltaTime[index] + Time.get_deltaTime();
      if ((double) this.lstChangeDeltaTime[_state] >= (double) this.lstChangeTime[_state])
      {
        this.lstChangeTime[_state] = Random.Range((float) this.lstHitArea[_state].area.x, (float) this.lstHitArea[_state].area.y);
        this.lstChangeDeltaTime[_state] = 0.0f;
        Vector2 vector2 = this.lstHit[_state];
        vector2.x = (__Null) (double) Random.Range(5, 95 - (int) this.lstHitArea[_state].rate);
        vector2.y = (__Null) (vector2.x + (double) this.lstHitArea[_state].rate);
        this.lstHit[_state] = vector2;
      }
      return true;
    }

    public void InitTime()
    {
      this.lstChangeTime.Clear();
      this.lstChangeDeltaTime.Clear();
      foreach (FeelHit.FeelHitInfo feelHitInfo in this.lstHitArea)
      {
        this.lstChangeTime.Add(Random.Range((float) feelHitInfo.area.x, (float) feelHitInfo.area.y));
        this.lstChangeDeltaTime.Add(0.0f);
      }
    }

    public Vector2 Get(int _state)
    {
      return _state >= this.lstHit.Count ? new Vector2(1.1f, 1.1f) : Vector2.op_Multiply(this.lstHit[_state], 0.01f);
    }
  }
}
