// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.GroundAnimalHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class GroundAnimalHabitatPoint : AnimalPoint
  {
    [SerializeField]
    private GroundAnimalHabitatPoint.AvailableUserTypes _userType = GroundAnimalHabitatPoint.AvailableUserTypes.Cat;
    [SerializeField]
    private float _moveRadius = 100f;
    [SerializeField]
    private float _moveHeight = 20f;
    [SerializeField]
    private AnimalPoint.LocateInfo locateInfo = new AnimalPoint.LocateInfo();
    public List<Waypoint> Waypoints = new List<Waypoint>();
    public List<GroundAnimalHabitatPoint> DepopPoints = new List<GroundAnimalHabitatPoint>();
    private WildGround _user;
    [SerializeField]
    private GroundAnimalHabitatPoint.PointTypes _pointType;
    [SerializeField]
    private Transform _centerPoint;
    [SerializeField]
    private Transform _insidePoint;
    [SerializeField]
    private Transform _outsidePoint;

    public WildGround User
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

    public GroundAnimalHabitatPoint.PointTypes PointType
    {
      get
      {
        return this._pointType;
      }
    }

    public GroundAnimalHabitatPoint.AvailableUserTypes UserType
    {
      get
      {
        return this._userType;
      }
    }

    public Transform CenterPoint
    {
      get
      {
        return this._centerPoint;
      }
    }

    public Transform InsidePoint
    {
      get
      {
        return this._insidePoint;
      }
    }

    public Transform OutsidePoint
    {
      get
      {
        return this._outsidePoint;
      }
    }

    public override bool Available
    {
      get
      {
        return Object.op_Inequality((Object) this._centerPoint, (Object) null) && Object.op_Inequality((Object) this._insidePoint, (Object) null) && Object.op_Inequality((Object) this._outsidePoint, (Object) null);
      }
    }

    public bool IsPopPoint
    {
      get
      {
        return this._pointType == GroundAnimalHabitatPoint.PointTypes.Both || this._pointType == GroundAnimalHabitatPoint.PointTypes.Pop;
      }
    }

    public bool IsDepopPoint
    {
      get
      {
        return this._pointType == GroundAnimalHabitatPoint.PointTypes.Both || this._pointType == GroundAnimalHabitatPoint.PointTypes.Depop;
      }
    }

    public bool IsCatOnly
    {
      get
      {
        return this._userType == GroundAnimalHabitatPoint.AvailableUserTypes.Cat;
      }
    }

    public bool IsChickenOnly
    {
      get
      {
        return this._userType == GroundAnimalHabitatPoint.AvailableUserTypes.Chicken;
      }
    }

    public bool IsBoth
    {
      get
      {
        return this._userType == (GroundAnimalHabitatPoint.AvailableUserTypes) 0;
      }
    }

    public float MoveRadius
    {
      get
      {
        return this._moveRadius;
      }
    }

    public float MoveHeight
    {
      get
      {
        return this._moveHeight;
      }
    }

    public Vector2 CoolTime { get; set; } = Vector2.get_zero();

    public bool IsCountStop { get; set; } = true;

    public bool IsCountCoolTime { get; set; }

    public bool IsActive { get; set; }

    public float CoolTimeCounter { get; private set; }

    public Func<Vector3, bool> AddCheck { get; set; }

    public Func<GroundAnimalHabitatPoint, WildGround> AddAnimalAction { get; set; }

    protected override void Start()
    {
      base.Start();
      this.locateInfo.LocateAll();
      if (!this.Available)
        return;
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Component) this._centerPoint), (Component) this._insidePoint), (Component) this._outsidePoint), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.UpdateCoolTime()));
    }

    public bool InUsed
    {
      get
      {
        return Object.op_Inequality((Object) this._user, (Object) null);
      }
    }

    public void SetCoolTime()
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = this.CoolTime.RandomRange();
    }

    public void SetCoolTime(float _coolTime)
    {
      this.IsCountCoolTime = true;
      this.CoolTimeCounter = _coolTime;
    }

    private void UpdateCoolTime()
    {
      if (this.IsCountStop || Object.op_Inequality((Object) this._user, (Object) null) || (!this.IsActive || !this.IsCountCoolTime) || Mathf.Approximately(0.0f, Time.get_timeScale()))
        return;
      this.CoolTimeCounter -= Time.get_unscaledDeltaTime();
      if ((double) this.CoolTimeCounter > 0.0)
        return;
      this.CoolTimeCounter = 0.0f;
      if (this.AddCheck == null || this.AddAnimalAction == null || !this.AddCheck(this._insidePoint.get_position()))
        return;
      this._user = this.AddAnimalAction(this);
      this.IsCountCoolTime = Object.op_Equality((Object) this._user, (Object) null);
    }

    public bool SetUse(WildGround _animal)
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || Object.op_Inequality((Object) this._user, (Object) null) && Object.op_Inequality((Object) this._user, (Object) _animal))
        return false;
      this._user = _animal;
      return true;
    }

    public bool StopUse(WildGround _animal)
    {
      if (Object.op_Equality((Object) _animal, (Object) null) || Object.op_Equality((Object) this._user, (Object) null) || Object.op_Inequality((Object) this._user, (Object) _animal))
        return false;
      this._user = (WildGround) null;
      this.SetCoolTime();
      return true;
    }

    private void LookAt(Transform _origin, Vector3 _target)
    {
      _target.y = _origin.get_position().y;
      _origin.LookAt(_target, Vector3.get_up());
    }

    private void Awake()
    {
      if (Object.op_Equality((Object) this._centerPoint, (Object) null))
        this._centerPoint = ((Component) this).get_transform();
      if (!Object.op_Inequality((Object) this._outsidePoint, (Object) null) || !Object.op_Inequality((Object) this._insidePoint, (Object) null))
        return;
      this.LookAt(this._outsidePoint, this._insidePoint.get_position());
      this.LookAt(this._insidePoint, this._outsidePoint.get_position());
    }

    private bool AvailablePoint(Waypoint _point)
    {
      if (!((Component) _point).get_gameObject().get_activeSelf())
        return false;
      Vector3 position1 = ((Component) _point).get_transform().get_position();
      Vector3 position2 = this._centerPoint.get_position();
      return (double) this.DistanceY(position1, position2) <= (double) this._moveHeight && (double) this.DistanceXZ(position1, position2) <= (double) this._moveRadius;
    }

    private bool AvailablePoint(GroundAnimalHabitatPoint _point)
    {
      if (Object.op_Equality((Object) _point.InsidePoint, (Object) null) || !((Component) _point).get_gameObject().get_activeSelf())
        return false;
      Vector3 position1 = _point.InsidePoint.get_position();
      Vector3 position2 = this._centerPoint.get_position();
      return (double) this.DistanceY(position1, position2) <= (double) this._moveHeight && (double) this.DistanceXZ(position1, position2) <= (double) this._moveRadius;
    }

    [DebuggerHidden]
    public IEnumerator SetPointsAsync(
      Waypoint[] _waypoints,
      GroundAnimalHabitatPoint[] _depopPoints,
      int _breakCount)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new GroundAnimalHabitatPoint.\u003CSetPointsAsync\u003Ec__Iterator0()
      {
        _waypoints = _waypoints,
        _breakCount = _breakCount,
        _depopPoints = _depopPoints,
        \u0024this = this
      };
    }

    public enum PointTypes
    {
      Both,
      Pop,
      Depop,
    }

    public enum AvailableUserTypes
    {
      Cat = 1,
      Chicken = 2,
    }
  }
}
