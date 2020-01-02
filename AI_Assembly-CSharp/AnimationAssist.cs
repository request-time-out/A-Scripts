// Decompiled with JetBrains decompiler
// Type: AnimationAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class AnimationAssist
{
  private Animation animation;
  private AnimationAssist.ANMCTRLST data;

  public AnimationAssist(Animation _animation)
  {
    this.animation = _animation;
    int num = 0;
    this.data = new AnimationAssist.ANMCTRLST(_animation.GetClipCount());
    IEnumerator enumerator = this.animation.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        AnimationState current = (AnimationState) enumerator.Current;
        this.data.info[num++] = new AnimationAssist.ANMCTRLINFOST(current);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public Animation NowAnimation
  {
    get
    {
      return this.animation;
    }
  }

  public AnimationState NowAnimationState
  {
    get
    {
      return this.animation.get_Item(this.GetID(this.data.NowPtn));
    }
  }

  public string GetID(int id)
  {
    return (long) (uint) id >= (long) this.data.info.Length ? string.Empty : this.data.info[id].Name;
  }

  public AnimationAssist.ANMCTRLST Data
  {
    get
    {
      return this.data;
    }
  }

  public bool IsAnimeEnd()
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (nowAnimationState.get_wrapMode() == 8 | nowAnimationState.get_wrapMode() == 2 | nowAnimationState.get_wrapMode() == 4)
    {
      if (this.GetInfo(-1).isReverse)
      {
        if ((double) nowAnimationState.get_time() <= 0.0)
          return true;
      }
      else if ((double) nowAnimationState.get_time() >= (double) nowAnimationState.get_length())
        return true;
    }
    return !this.animation.get_isPlaying();
  }

  public void Update()
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(-1);
    if ((double) this.data.msTime != -1.0)
    {
      if ((double) Time.get_timeScale() == 0.0)
      {
        AnimationState animationState = nowAnimationState;
        animationState.set_time(animationState.get_time() + this.data.msTime);
      }
      else
        this.data.msTime = Time.get_deltaTime();
    }
    if (nowAnimationState.get_wrapMode() == 2 && info.LoopNum > 0)
    {
      if (this.IsAnimeEnd())
      {
        ++info.LoopCnt;
        if (info.isReverse)
          nowAnimationState.set_time(nowAnimationState.get_length());
        else
          nowAnimationState.set_time(0.0f);
      }
      if (info.LoopCnt > info.LoopNum)
        nowAnimationState.set_wrapMode((WrapMode) 0);
    }
    if (nowAnimationState.get_wrapMode() == null && this.IsAnimeEnd() && (info.LinkNo != -1 && this.data.NowPtn != info.LinkNo))
      this.Play(info.LinkNo, -1f, 0.3f, 0, (WrapMode) 0);
    if (!this.IsAnimeEnd())
      return;
    AnimationState animationState1 = this.animation.get_Item(this.GetID(this.data.BeforePtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState1))
      return;
    animationState1.set_wrapMode(this.GetInfo(this.data.BeforePtn).baseMode);
  }

  public void LoopSet(int ptn, int LoopNum)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(ptn);
    if (info != null)
    {
      info.LoopNum = LoopNum;
      info.LoopCnt = 0;
    }
    AnimationState animationState = this.animation.get_Item(this.GetID(ptn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return;
    animationState.set_wrapMode(LoopNum != -1 ? (WrapMode) 2 : (WrapMode) 0);
  }

  public void SpeedSetAll(float speed)
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return;
    AnimationAssist.ANMCTRLST data = this.data;
    float num1 = speed;
    nowAnimationState.set_speed(num1);
    double num2 = (double) num1;
    data.speed = (float) num2;
  }

  public void SpeedSet(float speed)
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return;
    nowAnimationState.set_speed(speed);
  }

  public void ReStart()
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(-1);
    if (info != null)
      info.isStop = false;
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return;
    nowAnimationState.set_speed(this.data.speed);
    this.Play(string.Empty, nowAnimationState.get_time(), 0.3f, 0, (WrapMode) 0);
  }

  public void Stop()
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(-1);
    if (info != null)
      info.isStop = true;
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return;
    nowAnimationState.set_speed(0.0f);
    float time = nowAnimationState.get_time();
    this.animation.Stop();
    nowAnimationState.set_time(time);
  }

  public int GetNowPtn()
  {
    return this.data.NowPtn;
  }

  public void Play(int id, float time = -1f, float fadeSpeed = 0.3f, int layer = 0, WrapMode mode = 0)
  {
    this.Play(this.GetID(id), time, fadeSpeed, layer, mode);
  }

  public void Play(string name = "", float time = -1f, float fadeSpeed = 0.3f, int layer = 0, WrapMode mode = 0)
  {
    if (name == string.Empty)
      name = this.GetID(this.data.NowPtn);
    AnimationState animationState1 = this.animation.get_Item(name);
    if (TrackedReference.op_Equality((TrackedReference) animationState1, (TrackedReference) null))
      return;
    AnimationState animationState2 = this.animation.get_Item(this.GetID(this.data.BeforePtn));
    if (TrackedReference.op_Implicit((TrackedReference) animationState2))
      animationState2.set_wrapMode(this.GetInfo(this.data.BeforePtn).baseMode);
    for (int id = 0; id < this.data.info.Length; ++id)
    {
      if (name == this.GetID(id))
      {
        this.data.BeforePtn = this.data.NowPtn;
        this.data.NowPtn = id;
        break;
      }
    }
    animationState1.set_speed(this.data.speed);
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(-1);
    if (info.isStop)
    {
      animationState1.set_speed(0.0f);
    }
    else
    {
      if (info.isReverse)
      {
        if ((double) animationState1.get_speed() > 0.0)
        {
          AnimationState animationState3 = animationState1;
          animationState3.set_speed(animationState3.get_speed() * -1f);
        }
        animationState1.set_time(animationState1.get_length() - time);
        animationState1.set_time(Mathf.Clamp(animationState1.get_time(), 0.0f, animationState1.get_length()));
      }
      else if ((double) time >= 0.0)
        animationState1.set_time(time);
      animationState1.set_layer(layer);
      if (mode != null)
        animationState1.set_wrapMode(mode);
      if ((double) fadeSpeed == 0.0)
      {
        this.animation.Play(name);
      }
      else
      {
        if (animationState1.get_wrapMode() == null)
          animationState1.set_wrapMode((WrapMode) 8);
        this.animation.CrossFade(name, fadeSpeed);
      }
      Debug.Log((object) name);
    }
  }

  public void PlayOverride(int id, float time = -1f, float fadeSpeed = 0.3f, int layer = 1)
  {
    this.PlayOverride(this.GetID(id), time, fadeSpeed, layer);
  }

  public void PlayOverride(string name = "", float time = -1f, float fadeSpeed = 0.3f, int layer = 1)
  {
    Debug.Log((object) "Override");
    this.Play(name, time, fadeSpeed, layer, (WrapMode) 1);
  }

  public void Fusion(int id, float weight = 0.5f, int layer = 1)
  {
    this.Fusion(this.GetID(id), weight, layer);
  }

  public void Fusion(string name = "", float weight = 0.5f, int layer = 1)
  {
    if (name == string.Empty)
      name = this.GetID(this.data.NowPtn);
    AnimationState animationState = this.animation.get_Item(name);
    if (TrackedReference.op_Equality((TrackedReference) animationState, (TrackedReference) null))
      return;
    Debug.Log((object) nameof (Fusion));
    this.Play(name, -1f, 0.0f, layer, (WrapMode) 0);
    animationState.set_weight(weight);
  }

  private void PlaySync(Animation anime, int num)
  {
    AnimationState animationState = anime.get_Item(this.GetID(num));
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) animationState) || !TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return;
    nowAnimationState.set_time(animationState.get_time());
  }

  public AnimationAssist.ANMCTRLINFOST GetInfo(int nPtn = -1)
  {
    if (nPtn == -1)
      return this.data.info[this.data.NowPtn];
    return (long) (uint) nPtn >= (long) this.data.info.Length ? (AnimationAssist.ANMCTRLINFOST) null : this.data.info[nPtn];
  }

  public bool SetNowFrame(float nowFrame)
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return false;
    nowAnimationState.set_time(nowFrame);
    return true;
  }

  public bool GetNowFrame(ref float nowFrame)
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return false;
    nowFrame = nowAnimationState.get_time();
    return true;
  }

  public bool GetNowEndFrame(ref float endFrame)
  {
    AnimationState nowAnimationState = this.NowAnimationState;
    if (!TrackedReference.op_Implicit((TrackedReference) nowAnimationState))
      return false;
    endFrame = nowAnimationState.get_length();
    return true;
  }

  public bool GetPtnNow(ref int nowPtn)
  {
    nowPtn = this.data.NowPtn;
    return true;
  }

  public bool GetPtnBefore(ref int beforePtn)
  {
    beforePtn = this.data.BeforePtn;
    return true;
  }

  public bool GetName(int nPtn, ref string name)
  {
    name = this.GetID(nPtn);
    return name != string.Empty;
  }

  public bool GetEndFrame(int nPtn, ref float endFrame)
  {
    AnimationState animationState = this.animation.get_Item(this.GetID(nPtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return false;
    endFrame = animationState.get_length();
    return true;
  }

  public bool SetSpeed(int nPtn, float speed)
  {
    AnimationState animationState = this.animation.get_Item(this.GetID(nPtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return false;
    animationState.set_speed(speed);
    return true;
  }

  public bool GetSpeed(int nPtn, ref float speed)
  {
    AnimationState animationState = this.animation.get_Item(this.GetID(nPtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return false;
    speed = animationState.get_speed();
    return true;
  }

  public bool SetWrapMode(int nPtn, WrapMode mode)
  {
    AnimationState animationState = this.animation.get_Item(this.GetID(nPtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return false;
    animationState.set_wrapMode(mode);
    return true;
  }

  public bool GetWrapMode(int nPtn, ref WrapMode mode)
  {
    AnimationState animationState = this.animation.get_Item(this.GetID(nPtn));
    if (!TrackedReference.op_Implicit((TrackedReference) animationState))
      return false;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(int&) ref mode = (int) animationState.get_wrapMode();
    return true;
  }

  public bool SetLoopCnt(int nPtn, int loopCnt)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    info.LoopCnt = loopCnt;
    return true;
  }

  public bool GetLoopCnt(int nPtn, ref int loopCnt)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    loopCnt = info.LoopCnt;
    return true;
  }

  public bool SetLoopNum(int nPtn, int loopNum)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    info.LoopNum = loopNum;
    return true;
  }

  public bool GetLoopNum(int nPtn, ref int loopNum)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    loopNum = info.LoopNum;
    return true;
  }

  public bool SetLinkNo(int nPtn, int LinkNo)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    info.LinkNo = LinkNo;
    return true;
  }

  public bool GetLinkNo(int nPtn, ref int LinkNo)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    LinkNo = info.LinkNo;
    return true;
  }

  public bool SetReverseFlag(int nPtn, bool isReverse)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    info.isReverse = isReverse;
    return true;
  }

  public bool GetReverseFlag(int nPtn, ref bool isReverse)
  {
    AnimationAssist.ANMCTRLINFOST info = this.GetInfo(nPtn);
    if (info == null)
      return false;
    isReverse = info.isReverse;
    return true;
  }

  public class ANMCTRLINFOST
  {
    private string name;
    public int LoopCnt;
    public int LoopNum;
    public int LinkNo;
    public bool isReverse;
    public bool isStop;
    public WrapMode baseMode;

    public ANMCTRLINFOST(AnimationState state)
    {
      this.name = state.get_name();
      this.LoopCnt = 0;
      this.LoopNum = 0;
      this.LinkNo = -1;
      this.isReverse = (double) state.get_speed() < 0.0;
      this.isStop = (double) state.get_speed() == 0.0;
      this.baseMode = state.get_wrapMode();
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }
  }

  public class ANMCTRLST
  {
    public AnimationAssist.ANMCTRLINFOST[] info;
    public bool fChange;
    public int BeforePtn;
    public int NowPtn;
    public float msTime;
    public float speed;

    public ANMCTRLST(int num)
    {
      this.info = new AnimationAssist.ANMCTRLINFOST[num];
      this.fChange = false;
      this.BeforePtn = -1;
      this.NowPtn = 0;
      this.msTime = -1f;
      this.speed = 1f;
    }
  }
}
