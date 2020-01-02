// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.BirdFlockHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class BirdFlockHabitatPoint : AnimalPoint
  {
    [SerializeField]
    private Vector2 _coolTimeRange = Vector2.get_zero();
    [SerializeField]
    private BirdFlockHabitatPoint.BirdMoveAreaInfo[] _areaInfos = new BirdFlockHabitatPoint.BirdMoveAreaInfo[0];
    private WildBirdFlock _user;

    public WildBirdFlock User
    {
      get
      {
        return this._user;
      }
      set
      {
        this._user = value;
      }
    }

    public Vector2 CoolTimeRange
    {
      get
      {
        return this._coolTimeRange;
      }
    }

    public BirdFlockHabitatPoint.BirdMoveAreaInfo[] AreaInfos
    {
      get
      {
        return this._areaInfos;
      }
    }

    public bool InUsed
    {
      get
      {
        return Object.op_Inequality((Object) this._user, (Object) null);
      }
    }

    public override bool Available
    {
      get
      {
        return this._areaInfos != null && this._areaInfos.Exists<BirdFlockHabitatPoint.BirdMoveAreaInfo>((Predicate<BirdFlockHabitatPoint.BirdMoveAreaInfo>) (x => x != null && x.Available));
      }
    }

    public bool IsCountStop { get; set; } = true;

    public bool IsCountCoolTime { get; set; }

    public bool IsActive { get; set; }

    public float CoolTimeCounter { get; private set; }

    public Func<Vector3, bool> AddCheck { get; set; }

    public Func<BirdFlockHabitatPoint, WildBirdFlock> AddAnimalAction { get; set; }

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.UpdateCoolTime()));
    }

    public void SetCoolTime()
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = this.CoolTimeRange.RandomRange();
    }

    private void UpdateCoolTime()
    {
      if (this.IsCountStop || Object.op_Inequality((Object) this._user, (Object) null) || (!this.IsActive || !this.IsCountCoolTime) || Mathf.Approximately(0.0f, Time.get_timeScale()))
        return;
      this.CoolTimeCounter -= Time.get_unscaledDeltaTime();
      if ((double) this.CoolTimeCounter > 0.0)
        return;
      this.CoolTimeCounter = 0.0f;
      if (this.AddCheck == null || this.AddAnimalAction == null || !this.AddCheck(this.Position))
        return;
      this._user = this.AddAnimalAction(this);
      this.IsCountCoolTime = Object.op_Equality((Object) this._user, (Object) null);
    }

    public bool SetUse(WildBirdFlock _birdFlock)
    {
      if (Object.op_Equality((Object) _birdFlock, (Object) null) || Object.op_Inequality((Object) this._user, (Object) null) && Object.op_Inequality((Object) this._user, (Object) _birdFlock))
        return false;
      this._user = _birdFlock;
      return true;
    }

    public bool StopUse(WildBirdFlock _birdFlock)
    {
      if (Object.op_Equality((Object) this._user, (Object) null) || Object.op_Equality((Object) _birdFlock, (Object) null) || Object.op_Inequality((Object) this._user, (Object) _birdFlock))
        return false;
      this._user = (WildBirdFlock) null;
      this.SetCoolTime();
      return true;
    }

    public bool StopUse()
    {
      if (Object.op_Equality((Object) this._user, (Object) null))
        return false;
      this._user = (WildBirdFlock) null;
      return true;
    }

    [Serializable]
    public class BirdMoveAreaInfo
    {
      [SerializeField]
      private Vector2 _moveRect = Vector2.get_zero();
      [SerializeField]
      private Vector2Int _createNumRange = Vector2Int.get_one();
      [SerializeField]
      private Transform _startPoint;
      [SerializeField]
      private Transform _endPoint;

      public Transform StartPoint
      {
        get
        {
          return this._startPoint;
        }
      }

      public Transform EndPoint
      {
        get
        {
          return this._endPoint;
        }
      }

      public bool Available
      {
        get
        {
          return Object.op_Inequality((Object) this._startPoint, (Object) null) && Object.op_Inequality((Object) this._endPoint, (Object) null);
        }
      }

      public Vector2 MoveRect
      {
        get
        {
          return this._moveRect;
        }
      }

      public Vector2Int CreateNumRange
      {
        get
        {
          return this._createNumRange;
        }
      }
    }
  }
}
