// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ButterflyHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class ButterflyHabitatPoint : AnimalPoint
  {
    private Dictionary<int, WildButterfly> _user = new Dictionary<int, WildButterfly>();
    private List<WildButterfly> _userList = new List<WildButterfly>();
    [SerializeField]
    [Tooltip("移動可能範囲(半径)")]
    private float _moveRadius = 10f;
    [SerializeField]
    [Tooltip("移動可能範囲(高さ)")]
    private float _moveHeight = 5f;
    [SerializeField]
    [Tooltip("出現経由ポイントの通過可能半径")]
    private float _viaPointRadius = 3f;
    [SerializeField]
    private Vector2Int _createNumRange = Vector2Int.get_one();
    private Queue<IDisposable> _createDisposableList = new Queue<IDisposable>();
    [SerializeField]
    [Tooltip("移動可能領域の中心")]
    private Transform _center;
    [SerializeField]
    [Tooltip("経由ポイント")]
    private Transform _viaPoint;
    [SerializeField]
    [Tooltip("消失ポイント")]
    private Transform _depopPoint;

    public Dictionary<int, WildButterfly> User
    {
      get
      {
        return this._user;
      }
    }

    public List<WildButterfly> UserList
    {
      get
      {
        return this._userList;
      }
    }

    public Transform Center
    {
      get
      {
        return this._center;
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

    public float ViaPointRadius
    {
      get
      {
        return this._viaPointRadius;
      }
    }

    public Transform ViaPoint
    {
      get
      {
        return this._viaPoint;
      }
    }

    public Transform DepopPoint
    {
      get
      {
        return this._depopPoint;
      }
    }

    public override bool Available
    {
      get
      {
        return Object.op_Inequality((Object) this._center, (Object) null) && Object.op_Inequality((Object) this._viaPoint, (Object) null) && Object.op_Inequality((Object) this._depopPoint, (Object) null);
      }
    }

    public Vector2Int CreateNumRange
    {
      get
      {
        return this._createNumRange;
      }
    }

    public bool InUsed
    {
      get
      {
        return !((IReadOnlyDictionary<int, WildButterfly>) this._user).IsNullOrEmpty<int, WildButterfly>();
      }
    }

    public int UserCount
    {
      get
      {
        return this._user.Count;
      }
    }

    public bool IsStop { get; set; } = true;

    public bool IsActive { get; set; }

    public bool IsCreate { get; set; }

    public Func<Vector3, bool> AddCheck { get; set; }

    public Func<ButterflyHabitatPoint, WildButterfly> AddAnimalAction { get; set; }

    private void Awake()
    {
      if (Object.op_Implicit((Object) this._center))
        return;
      this._center = ((Component) this).get_transform();
    }

    protected override void Start()
    {
      base.Start();
      if (!this.Available)
        Object.Destroy((Object) this);
      else
        ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Component) this._center), (Component) this._viaPoint), (Component) this._depopPoint), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.CreateCheck()));
    }

    private void CreateCheck()
    {
      if (this.IsStop || !this.IsActive || (this.IsCreate || Mathf.Approximately(0.0f, Time.get_timeScale())) || (this.AddCheck == null || this.AddAnimalAction == null))
        return;
      this.CreateButterfly();
      this.IsCreate = true;
    }

    private void CreateButterfly()
    {
      int num1 = this._createNumRange.RandomRange();
      this._userList.RemoveAll((Predicate<WildButterfly>) (x => Object.op_Equality((Object) x, (Object) null) || Object.op_Equality((Object) ((Component) x).get_gameObject(), (Object) null)));
      int num2 = num1 - this._userList.Count;
      if (num2 == 0)
      {
        if (((IReadOnlyList<WildButterfly>) this._userList).IsNullOrEmpty<WildButterfly>())
          return;
        foreach (WildButterfly user in this._userList)
          user.ForcedLocomotion();
      }
      else if (0 < num2)
      {
        if (!((IReadOnlyList<WildButterfly>) this._userList).IsNullOrEmpty<WildButterfly>())
        {
          foreach (WildButterfly user in this._userList)
            user.ForcedLocomotion();
        }
        while (0 < this._createDisposableList.Count)
          this._createDisposableList.Dequeue()?.Dispose();
        for (int index = 0; index < num2; ++index)
          this._createDisposableList.Enqueue(ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) Random.Range(0.0f, 4f))), (Component) this), (Component) this._center), (Component) this._viaPoint), (Component) this._depopPoint), (Action<M0>) (_ =>
          {
            if (this.AddCheck == null || !this.AddCheck(this._center.get_position()))
              return;
            Func<ButterflyHabitatPoint, WildButterfly> addAnimalAction = this.AddAnimalAction;
            if (addAnimalAction == null)
              return;
            WildButterfly wildButterfly = addAnimalAction(this);
          }), (Action<Exception>) (ex => {}), (Action) (() => {})));
      }
      else
      {
        if (num2 >= 0)
          return;
        int num3 = Mathf.Abs(num2);
        for (int index = 0; index < this._userList.Count; ++index)
        {
          WildButterfly user = this._userList[index];
          if (index < num3)
            user.ForcedDepop();
          else
            user.ForcedLocomotion();
        }
      }
    }

    public bool SetUse(WildButterfly _butterfly)
    {
      if (!this._userList.Contains(_butterfly))
        this._userList.Add(_butterfly);
      return this._user.AddNonContains<int, WildButterfly>(_butterfly.InstanceID, _butterfly);
    }

    public bool StopUse(WildButterfly _butterfly)
    {
      if (this._userList.Contains(_butterfly))
        this._userList.Remove(_butterfly);
      this._userList.RemoveAll((Predicate<WildButterfly>) (x => Object.op_Equality((Object) x, (Object) null) || Object.op_Equality((Object) ((Component) x).get_gameObject(), (Object) null)));
      return this._user.Remove(_butterfly.InstanceID);
    }
  }
}
