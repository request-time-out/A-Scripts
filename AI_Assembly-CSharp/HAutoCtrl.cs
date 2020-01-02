// Decompiled with JetBrains decompiler
// Type: HAutoCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

public class HAutoCtrl : MonoBehaviour
{
  private HAutoCtrl.HAutoInfo info;
  [DisabledGroup("今のスピード")]
  public float centerSpeed;
  [DisabledGroup("リープ時間")]
  public float timeLerp;
  [DisabledGroup("リープ先")]
  public Vector2 lerp;
  [Label("リープアニメーション")]
  public AnimationCurve lerpCurve;
  [DisabledGroup("抜く確率判定")]
  public bool isPulljudge;
  public HAutoCtrl.AutoLeaveItToYou autoLeave;

  public HAutoCtrl()
  {
    base.\u002Ector();
  }

  public void Load(string _strAssetPath, int _personal, int _attribute = 0)
  {
    this.lerp = new Vector2(-1f, -1f);
    if (!this.LoadAuto())
      return;
    this.autoLeave.time.minmax = new Vector2(20f, 20f);
    this.autoLeave.time.Reset();
    this.autoLeave.rate = 50;
    if (!this.LoadAutoLeaveItToYou() || !this.LoadAutoLeaveItToYouPersonality(_personal) || this.LoadAutoLeaveItToYouAttribute(_attribute))
      ;
  }

  private bool LoadAuto()
  {
    this.info = Singleton<Resources>.Instance.HSceneTable.HAutoInfo;
    return this.info != null;
  }

  private bool LoadAutoLeaveItToYou()
  {
    this.autoLeave = Singleton<Resources>.Instance.HSceneTable.HAutoLeaveItToYou;
    return this.autoLeave != null;
  }

  private bool LoadAutoLeaveItToYouPersonality(int _personal)
  {
    float num;
    if (!Singleton<Resources>.Instance.HSceneTable.autoLeavePersonalityRate.TryGetValue(_personal, out num))
      num = 1f;
    this.autoLeave.rate = Mathf.CeilToInt((float) this.autoLeave.rate * num);
    return true;
  }

  private bool LoadAutoLeaveItToYouAttribute(int _attribute)
  {
    float num;
    if (!Singleton<Resources>.Instance.HSceneTable.autoLeaveAttributeRate.TryGetValue(_attribute, out num))
      num = 1f;
    this.autoLeave.rate = Mathf.CeilToInt((float) this.autoLeave.rate * num);
    return true;
  }

  public void Reset()
  {
    this.StartInit();
    this.ReStartInit();
    this.SpeedInit();
    this.LoopMotionInit();
    this.MotionChangeInit();
    this.PullInit();
  }

  public void StartInit()
  {
    this.info.start.Reset();
  }

  public bool IsStart()
  {
    return this.info.start.IsTime();
  }

  public void ReStartInit()
  {
    this.info.reStart.Reset();
  }

  public bool IsReStart()
  {
    return this.info.reStart.IsTime();
  }

  public void SpeedInit()
  {
    this.info.speed.Reset();
    this.centerSpeed = 0.0f;
  }

  public bool AddSpeed(float _wheel, int _loop)
  {
    if (this.lerp.x >= 0.0 && this.lerp.y >= 0.0)
      return false;
    bool flag = false;
    this.centerSpeed += _wheel;
    switch (_loop)
    {
      case 0:
        if ((double) this.centerSpeed > 1.0)
        {
          flag = true;
          break;
        }
        break;
      case 1:
        if ((double) this.centerSpeed < 1.0)
        {
          flag = true;
          break;
        }
        break;
    }
    if ((double) _wheel != 0.0)
      this.info.speed.Reset();
    this.centerSpeed = _loop == 2 ? Mathf.Clamp(this.centerSpeed, 0.0f, 1f) : Mathf.Clamp(this.centerSpeed, 0.0f, 2f);
    return flag;
  }

  public void LoopMotionInit()
  {
    this.info.loopChange.Reset();
  }

  public bool ChangeLoopMotion(bool _loop)
  {
    if (this.lerp.x >= 0.0 && this.lerp.y >= 0.0 || !this.info.loopChange.IsTime())
      return false;
    this.info.loopChange.Reset();
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    shuffleRand.Init(100);
    return this.IsChangeLoop(_loop, this.info.rateWeakLoop < shuffleRand.Get());
  }

  private bool IsChangeLoop(bool _loop, bool _changeLoop)
  {
    if (_loop == _changeLoop)
      return false;
    this.centerSpeed = 1f;
    return true;
  }

  public bool ChangeSpeed(bool _loop, Vector2 _hit)
  {
    if (this.lerp.x >= 0.0 && this.lerp.y >= 0.0 || !this.info.speed.IsTime())
      return false;
    this.info.speed.Reset();
    this.timeLerp = 0.0f;
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    if (_hit.x >= 0.0)
    {
      shuffleRand.Init(100);
      if (this.info.rateHit > shuffleRand.Get())
      {
        this.lerp.x = _loop ? (__Null) ((double) this.centerSpeed - 1.0) : (__Null) (double) this.centerSpeed;
        this.lerp.y = (__Null) (double) Random.Range((float) _hit.x, (float) _hit.y);
        return false;
      }
    }
    shuffleRand.Init(100);
    int valNow;
    do
    {
      valNow = shuffleRand.Get();
    }
    while (GlobalMethod.RangeOn<int>(valNow, (int) (_hit.x * 100.0), (int) (_hit.y * 100.0)));
    this.lerp.x = _loop ? (__Null) ((double) this.centerSpeed - 1.0) : (__Null) (double) this.centerSpeed;
    this.lerp.y = (__Null) ((double) valNow * 0.00999999977648258);
    return false;
  }

  public float GetSpeed(bool _loop)
  {
    if (this.lerp.x < 0.0 && this.lerp.y < 0.0)
      return !_loop ? this.centerSpeed : this.centerSpeed - 1f;
    this.timeLerp = Mathf.Clamp(this.timeLerp + Time.get_deltaTime(), 0.0f, this.info.lerpTimeSpeed);
    this.centerSpeed = Mathf.Lerp((float) this.lerp.x, (float) this.lerp.y, this.lerpCurve.Evaluate(Mathf.InverseLerp(0.0f, this.info.lerpTimeSpeed, this.timeLerp)));
    if (_loop)
      ++this.centerSpeed;
    if ((double) this.timeLerp >= (double) this.info.lerpTimeSpeed)
    {
      this.lerp = new Vector2(-1f, -1f);
      this.timeLerp = 0.0f;
    }
    return !_loop ? this.centerSpeed : this.centerSpeed - 1f;
  }

  public void SetSpeed(float _speed)
  {
    this.centerSpeed = _speed;
  }

  public void MotionChangeInit()
  {
    this.info.motionChange.Reset();
  }

  public bool IsChangeActionAtLoop()
  {
    if (this.lerp.x >= 0.0 && this.lerp.y >= 0.0 || !this.info.motionChange.IsTime())
      return false;
    this.info.motionChange.Reset();
    return true;
  }

  public bool IsChangeActionAtRestart()
  {
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    shuffleRand.Init(100);
    return shuffleRand.Get() < this.info.rateRestartMotionChange;
  }

  public HScene.StartMotion GetAnimation(
    List<HScene.AnimationListInfo>[] _listAnim,
    int _initiative,
    bool _isFirst = false)
  {
    bool flag = true;
    HAutoCtrl.AutoRandom autoRandom = new HAutoCtrl.AutoRandom();
    for (int index1 = 0; index1 < _listAnim.Length; ++index1)
    {
      for (int index2 = 0; index2 < _listAnim[index1].Count; ++index2)
      {
        if (_listAnim[index1][index2].nPositons.Contains(Singleton<HSceneFlagCtrl>.Instance.nPlace))
        {
          if (Singleton<HSceneManager>.Instance.Player.ChaControl.sex == (byte) 0 || Singleton<HSceneManager>.Instance.Player.ChaControl.sex == (byte) 1 && Singleton<HSceneManager>.Instance.bFutanari)
          {
            if (index1 == 4 || Object.op_Equality((Object) ((HScene) Singleton<HSceneManager>.Instance.HSceneSet.GetComponentInChildren<HScene>()).GetFemales()[1], (Object) null) && index1 == 5)
              continue;
          }
          else if (index1 != 4)
            continue;
          switch (_initiative)
          {
            case 1:
              if (_listAnim[index1][index2].nInitiativeFemale == 1 || flag && _listAnim[index1][index2].nInitiativeFemale == 2)
                break;
              continue;
            case 2:
              if (_listAnim[index1][index2].nInitiativeFemale == 2)
                break;
              continue;
            default:
              continue;
          }
          autoRandom.Add(new HAutoCtrl.AutoRandom.AutoRandomDate()
          {
            mode = index1,
            id = _listAnim[index1][index2].id
          }, (float) (10.0 + (!Singleton<HSceneManager>.Instance.HSkil.ContainsKey(_listAnim[index1][index2].nFemaleProclivity) ? 0.0 : (double) this.info.rateAddMotionChange)));
          if (_isFirst)
            break;
        }
      }
    }
    HAutoCtrl.AutoRandom.AutoRandomDate autoRandomDate = autoRandom.Random();
    return new HScene.StartMotion(autoRandomDate.mode, autoRandomDate.id);
  }

  public void PullInit()
  {
    this.info.pull.Reset();
    this.isPulljudge = false;
  }

  public bool IsPull(bool _isInsert)
  {
    if (this.isPulljudge || !this.info.pull.IsTime())
      return false;
    this.info.pull.Reset();
    this.isPulljudge = true;
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    shuffleRand.Init(100);
    int num = shuffleRand.Get();
    return _isInsert ? (double) num < (double) this.info.rateInsertPull : (double) num < (double) this.info.rateNotInsertPull;
  }

  public bool IsAutoAutoLeaveItToYou(
    ChaControl _female,
    HScene.AnimationListInfo _ali,
    bool _isAutoLeaveItToYouButton,
    bool _isInitiative)
  {
    if (Object.op_Equality((Object) _female, (Object) null) || _ali == null)
      return false;
    AnimatorStateInfo animatorStateInfo = _female.getAnimatorStateInfo(0);
    if (_isInitiative || !_isAutoLeaveItToYouButton || (!((AnimatorStateInfo) ref animatorStateInfo).IsName("Idle") || _ali.nAnimListInfoID == 3) || !this.autoLeave.time.IsTime())
      return false;
    ShuffleRand shuffleRand = new ShuffleRand(-1);
    shuffleRand.Init(100);
    bool flag = shuffleRand.Get() < this.autoLeave.rate;
    if (!flag)
    {
      this.autoLeave.time.minmax.x = (__Null) (double) Mathf.Max((float) (this.autoLeave.time.minmax.x - 5.0), 0.0f);
      this.autoLeave.time.minmax.y = (__Null) (double) Mathf.Max((float) (this.autoLeave.time.minmax.y - 5.0), 0.0f);
    }
    else
      this.autoLeave.time.minmax = this.autoLeave.baseTime;
    this.autoLeave.time.Reset();
    return flag;
  }

  public void AutoAutoLeaveItToYouInit()
  {
    this.autoLeave.time.minmax = this.autoLeave.baseTime;
    this.autoLeave.time.Reset();
  }

  public class AutoRandom
  {
    private List<HAutoCtrl.AutoRandom.CCheck> backup = new List<HAutoCtrl.AutoRandom.CCheck>();
    private List<HAutoCtrl.AutoRandom.CCheck> checks = new List<HAutoCtrl.AutoRandom.CCheck>();
    private List<ValueTuple<Guid, int>> tmpTuple = new List<ValueTuple<Guid, int>>();
    private HAutoCtrl.AutoRandom.ListComparer backupSortCompare = new HAutoCtrl.AutoRandom.ListComparer();
    private float allVal;

    public AutoRandom()
    {
      this.allVal = 0.0f;
    }

    public bool Add(HAutoCtrl.AutoRandom.AutoRandomDate _date, float _rate)
    {
      if ((double) _rate == 0.0)
      {
        GlobalMethod.DebugLog("ランダム 追加登録個数が0", 0);
        return false;
      }
      if (this.checks.Exists((Predicate<HAutoCtrl.AutoRandom.CCheck>) (i => i.date.mode == _date.mode && i.date.id == _date.id)))
      {
        GlobalMethod.DebugLog("ランダム 重複登録", 0);
        return false;
      }
      this.backup.Add(new HAutoCtrl.AutoRandom.CCheck()
      {
        date = _date,
        rate = _rate
      });
      this.RandomSort();
      this.allVal = 0.0f;
      foreach (HAutoCtrl.AutoRandom.CCheck check in this.checks)
      {
        check.minVal = this.allVal;
        check.maxVal = this.allVal + check.rate;
        this.allVal += check.rate;
      }
      return true;
    }

    private void RandomSort()
    {
      this.tmpTuple.Clear();
      for (int index = 0; index < this.backup.Count; ++index)
        this.tmpTuple.Add(new ValueTuple<Guid, int>(Guid.NewGuid(), index));
      this.tmpTuple.Sort((IComparer<ValueTuple<Guid, int>>) this.backupSortCompare);
      this.checks.Clear();
      for (int index = 0; index < this.tmpTuple.Count; ++index)
        this.checks.Add(this.backup[(int) this.tmpTuple[index].Item2]);
    }

    public HAutoCtrl.AutoRandom.AutoRandomDate Random()
    {
      if (this.IsEmpty())
        return new HAutoCtrl.AutoRandom.AutoRandomDate();
      float randVal = Random.Range(0.0f, this.allVal);
      HAutoCtrl.AutoRandom.CCheck ccheck = this.checks.Find((Predicate<HAutoCtrl.AutoRandom.CCheck>) (x => (double) randVal >= (double) x.minVal && (double) randVal <= (double) x.maxVal));
      return ccheck == null ? new HAutoCtrl.AutoRandom.AutoRandomDate() : ccheck.date;
    }

    public bool IsEmpty()
    {
      return this.backup.Count == 0;
    }

    public void Clear()
    {
      this.allVal = 0.0f;
      this.checks.Clear();
      this.backup.Clear();
    }

    public class AutoRandomDate
    {
      public int mode = -1;
      public int id = -1;
    }

    protected class CCheck
    {
      public HAutoCtrl.AutoRandom.AutoRandomDate date;
      public float rate;
      public float minVal;
      public float maxVal;
    }

    private class ListComparer : IComparer<ValueTuple<Guid, int>>
    {
      public int Compare(ValueTuple<Guid, int> a, ValueTuple<Guid, int> b)
      {
        return this.SortCompare<Guid>((Guid) a.Item1, (Guid) b.Item1);
      }

      private int SortCompare<T>(T a, T b) where T : IComparable
      {
        return a.CompareTo((object) b);
      }
    }
  }

  [Serializable]
  public class AutoTime
  {
    [DisabledGroup("最小最大")]
    public Vector2 minmax;
    [DisabledGroup("時間まで")]
    public float time;
    [DisabledGroup("経過時間")]
    public float timeDelta;

    public void Reset()
    {
      this.time = Random.Range((float) this.minmax.x, (float) this.minmax.y);
      this.timeDelta = 0.0f;
    }

    public bool IsTime()
    {
      this.timeDelta = Mathf.Clamp(this.timeDelta + Time.get_deltaTime(), 0.0f, this.time);
      return (double) this.timeDelta >= (double) this.time;
    }
  }

  [Serializable]
  public class AutoLeaveItToYou
  {
    public HAutoCtrl.AutoTime time = new HAutoCtrl.AutoTime();
    [Label("元の変更時間")]
    public Vector2 baseTime = Vector2.get_zero();
    [Label("おまかせにいく確率")]
    public int rate = 50;
  }

  [Serializable]
  public class HAutoInfo
  {
    public HAutoCtrl.AutoTime start = new HAutoCtrl.AutoTime();
    public HAutoCtrl.AutoTime reStart = new HAutoCtrl.AutoTime();
    public HAutoCtrl.AutoTime speed = new HAutoCtrl.AutoTime();
    public HAutoCtrl.AutoTime loopChange = new HAutoCtrl.AutoTime();
    public HAutoCtrl.AutoTime motionChange = new HAutoCtrl.AutoTime();
    public HAutoCtrl.AutoTime pull = new HAutoCtrl.AutoTime();
    [DisabledGroup("スピード変更のリープ時間")]
    public float lerpTimeSpeed;
    [DisabledGroup("弱ループ率")]
    public int rateWeakLoop;
    [DisabledGroup("当たりに向かう率")]
    public int rateHit;
    [DisabledGroup("体位変更性癖加算")]
    public float rateAddMotionChange;
    [DisabledGroup("リスタート時の体位変更率")]
    public int rateRestartMotionChange;
    [DisabledGroup("中出しした時に抜く確率")]
    public float rateInsertPull;
    [DisabledGroup("中出しされてない時に抜く確率")]
    public float rateNotInsertPull;
  }
}
