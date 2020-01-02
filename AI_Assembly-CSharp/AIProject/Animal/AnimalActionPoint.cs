// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalActionPoint : AnimalPoint
  {
    [SerializeField]
    private AnimalTypes _animalType = AnimalTypes.All;
    [SerializeField]
    private int _mapItemID = -1;
    [SerializeField]
    private bool _enabledNavMeshAgent = true;
    [SerializeField]
    private bool _enabledPositionValue = true;
    [SerializeField]
    private AnimalActionPoint.DirectionKind _directionType = AnimalActionPoint.DirectionKind.Lock;
    [SerializeField]
    private AnimalActionPoint.AnimalActionSlotTable _actionSlotTable = new AnimalActionPoint.AnimalActionSlotTable();
    private List<IAnimalActionPointUser> bookings = new List<IAnimalActionPointUser>();
    public TimeSpan coolTimeDuration = TimeSpan.MinValue;
    [SerializeField]
    private ActionTypes _actionType;
    [SerializeField]
    private Transform _destination;
    private int? _hasCode;

    public ActionTypes ActionType
    {
      get
      {
        return this._actionType;
      }
    }

    public AnimalTypes AnimalType
    {
      get
      {
        return this._animalType;
      }
    }

    public int MapItemID
    {
      get
      {
        return this._mapItemID;
      }
    }

    public bool EnabledNavMeshAgent
    {
      get
      {
        return this._enabledNavMeshAgent;
      }
    }

    public bool EnabledPositionValue
    {
      get
      {
        return this._enabledPositionValue;
      }
    }

    public AnimalActionPoint.DirectionKind Direction
    {
      get
      {
        return this._directionType;
      }
    }

    public Vector3 Destination
    {
      get
      {
        return this._destination.get_position();
      }
    }

    public GameObject MapItem { get; set; }

    public IAnimalActionPointUser User { get; private set; }

    public bool InUsed
    {
      get
      {
        return this.User != null;
      }
    }

    public Dictionary<int, float> UsedCoolTime { get; private set; } = new Dictionary<int, float>();

    public Dictionary<int, float> SearchedCoolTime { get; private set; } = new Dictionary<int, float>();

    public void SetSearchCoolTime(IAnimalActionPointUser _animal, float _time, bool _nonSetting)
    {
      int instanceId = _animal.InstanceID;
      if (_nonSetting && this.SearchedCoolTime.ContainsKey(instanceId))
        return;
      this.SearchedCoolTime[instanceId] = _time;
    }

    public void SetUsedCoolTime(IAnimalActionPointUser _animal, float _time, bool _nonSetting)
    {
      int instanceId = _animal.InstanceID;
      if (_nonSetting && this.UsedCoolTime.ContainsKey(instanceId))
        return;
      this.UsedCoolTime[instanceId] = _time;
    }

    public bool Available(IAnimalActionPointUser _animal)
    {
      return (this.AnimalType & _animal.AnimalType) != (AnimalTypes) 0 && !this.InUsed;
    }

    public bool AvailableOutCoolTime(IAnimalActionPointUser _animal)
    {
      if ((this.AnimalType & _animal.AnimalType) == (AnimalTypes) 0 || this.InUsed)
        return false;
      int instanceId = _animal.InstanceID;
      float num;
      return (!this.UsedCoolTime.TryGetValue(instanceId, out num) || 0.0 >= (double) num) && (!this.SearchedCoolTime.TryGetValue(instanceId, out num) || 0.0 >= (double) num);
    }

    public bool HasCoolTime
    {
      get
      {
        return 0 < this.UsedCoolTime.Count || 0 < this.SearchedCoolTime.Count;
      }
    }

    private void CoolDown()
    {
      float deltaTime = Time.get_deltaTime();
      if ((double) deltaTime == 0.0)
        return;
      this.CoolDownTable(this.UsedCoolTime, deltaTime);
      this.CoolDownTable(this.SearchedCoolTime, deltaTime);
    }

    private void CoolDownTable(Dictionary<int, float> _coolTimeTable, float _deltaTime)
    {
      if (((IReadOnlyDictionary<int, float>) _coolTimeTable).IsNullOrEmpty<int, float>() || (double) _deltaTime == 0.0)
        return;
      List<int> toRelease1 = ListPool<int>.Get();
      List<int> toRelease2 = ListPool<int>.Get();
      foreach (int key in _coolTimeTable.Keys)
        toRelease1.Add(key);
      for (int index1 = 0; index1 < toRelease1.Count; ++index1)
      {
        int index2 = toRelease1[index1];
        float num = Mathf.Max(0.0f, _coolTimeTable[index2] - _deltaTime);
        if ((double) num <= 0.0)
          toRelease2.Add(index2);
        else
          _coolTimeTable[index2] = num;
      }
      for (int index = 0; index < toRelease2.Count; ++index)
        _coolTimeTable.Remove(toRelease2[index]);
      ListPool<int>.Release(toRelease1);
      ListPool<int>.Release(toRelease2);
    }

    public void ClearCoolTime()
    {
      this.UsedCoolTime.Clear();
      this.SearchedCoolTime.Clear();
    }

    public void AddBooking(IAnimalActionPointUser animal)
    {
      if (this.bookings.Contains(animal))
        return;
      this.bookings.Add(animal);
    }

    public void RemoveBooking(IAnimalActionPointUser animal)
    {
      if (!this.bookings.Contains(animal))
        return;
      this.bookings.Remove(animal);
    }

    public void RemoveAllBooking()
    {
      this.bookings.Clear();
    }

    public void RemoveAllBooking(Action<IAnimalActionPointUser> callback)
    {
      if (callback != null && !((IReadOnlyList<IAnimalActionPointUser>) this.bookings).IsNullOrEmpty<IAnimalActionPointUser>())
      {
        for (int index = 0; index < this.bookings.Count; ++index)
          callback(this.bookings[index]);
      }
      this.bookings.Clear();
    }

    public bool ContainsBooking(IAnimalActionPointUser animal)
    {
      return this.bookings.Contains(animal);
    }

    public bool MyUse(IAnimalActionPointUser animal)
    {
      return animal == this.User;
    }

    public bool SetUse(IAnimalActionPointUser animal)
    {
      if (animal == null)
        return false;
      this.RemoveBooking(animal);
      if (this.InUsed && this.User != animal)
      {
        animal.MissingActionPoint();
        return false;
      }
      this.RemoveAllBooking((Action<IAnimalActionPointUser>) (x => x?.MissingActionPoint()));
      if (this.User == null)
        this.User = animal;
      return true;
    }

    public bool StopUsing(IAnimalActionPointUser animal)
    {
      if (this.User != animal)
        return false;
      this.User = (IAnimalActionPointUser) null;
      return true;
    }

    public bool StopUsing()
    {
      if (this.User == null)
        return false;
      this.User = (IAnimalActionPointUser) null;
      return true;
    }

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.HasCoolTime)), (Action<M0>) (_ => this.CoolDown()));
    }

    public override void LocateGround()
    {
      base.LocateGround();
      foreach (AnimalActionPoint.AnimalActionSlot animalActionSlot in this._actionSlotTable)
      {
        if (Object.op_Inequality((Object) animalActionSlot.Point, (Object) animalActionSlot.RecoveryPoint))
          Point.LocateGround(animalActionSlot.RecoveryPoint);
      }
      float num = 15f;
      switch (this.LocateType)
      {
        case LocateTypes.Collider:
          AnimalPoint.RelocationOnCollider(((Component) this).get_transform(), num);
          using (AnimalActionPoint.AnimalActionSlotTable.Enumerator enumerator = this._actionSlotTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AnimalActionPoint.AnimalActionSlot current = enumerator.Current;
              if (Object.op_Inequality((Object) current.Point, (Object) current.RecoveryPoint))
                AnimalPoint.RelocationOnCollider(current.RecoveryPoint, num);
            }
            break;
          }
        case LocateTypes.NavMesh:
          AnimalPoint.RelocationOnNavMesh(((Component) this).get_transform(), num);
          using (AnimalActionPoint.AnimalActionSlotTable.Enumerator enumerator = this._actionSlotTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              AnimalActionPoint.AnimalActionSlot current = enumerator.Current;
              if (Object.op_Inequality((Object) current.Point, (Object) current.RecoveryPoint))
                AnimalPoint.RelocationOnNavMesh(current.RecoveryPoint, num);
            }
            break;
          }
      }
    }

    public override void LoadObject()
    {
    }

    public Tuple<Transform, Transform> GetSlot()
    {
      if (this._actionSlotTable.Count <= 0)
        return new Tuple<Transform, Transform>((Transform) null, (Transform) null);
      AnimalActionPoint.AnimalActionSlot animalActionSlot = this._actionSlotTable[0];
      return new Tuple<Transform, Transform>(animalActionSlot.Point, animalActionSlot.RecoveryPoint);
    }

    public Tuple<Transform, Transform> GetSlot(ActionTypes type)
    {
      for (int index = 0; index < this._actionSlotTable.Count; ++index)
      {
        AnimalActionPoint.AnimalActionSlot animalActionSlot = this._actionSlotTable[index];
        if (animalActionSlot.AcceptionKey.Contains(type))
          return new Tuple<Transform, Transform>(animalActionSlot.Point, animalActionSlot.RecoveryPoint);
      }
      return new Tuple<Transform, Transform>((Transform) null, (Transform) null);
    }

    public Tuple<Transform, Transform>[] GetSlots(ActionTypes type)
    {
      return this._actionSlotTable.Where<AnimalActionPoint.AnimalActionSlot>((Func<AnimalActionPoint.AnimalActionSlot, bool>) (x => x.AcceptionKey.Contains(type))).Select<AnimalActionPoint.AnimalActionSlot, Tuple<Transform, Transform>>((Func<AnimalActionPoint.AnimalActionSlot, Tuple<Transform, Transform>>) (x => new Tuple<Transform, Transform>(x.Point, x.RecoveryPoint))).ToArray<Tuple<Transform, Transform>>();
    }

    public void SetStand(IAnimalActionPointUser animal, Transform t)
    {
      if (animal == null || Object.op_Equality((Object) t, (Object) null))
        return;
      IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, false), false));
      iconnectableObservable.Connect();
      Vector3 position = animal.Position;
      Quaternion rotation = animal.Rotation;
      switch (this._directionType)
      {
        case AnimalActionPoint.DirectionKind.Lock:
          ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Rotation = Quaternion.Slerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value())));
          break;
        case AnimalActionPoint.DirectionKind.Look:
          Quaternion lookRotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.op_Subtraction(t.get_position(), animal.Position)));
          ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Rotation = Quaternion.Slerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value())));
          break;
      }
      if (!this._enabledPositionValue)
        return;
      ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Position = Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value())));
    }

    public void SetStand(IAnimalActionPointUser animal, Transform t, Action completeEvent)
    {
      if (animal == null || Object.op_Equality((Object) t, (Object) null))
      {
        if (completeEvent == null)
          return;
        completeEvent();
      }
      else
      {
        IConnectableObservable<TimeInterval<float>> iconnectableObservable = (IConnectableObservable<TimeInterval<float>>) Observable.Publish<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(0.2f, false), false));
        iconnectableObservable.Connect();
        Vector3 position = animal.Position;
        Quaternion rotation = animal.Rotation;
        switch (this._directionType)
        {
          case AnimalActionPoint.DirectionKind.Lock:
            ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Rotation = Quaternion.Slerp(rotation, t.get_rotation(), ((TimeInterval<float>) ref x).get_Value())));
            break;
          case AnimalActionPoint.DirectionKind.Look:
            Quaternion lookRotation = Quaternion.LookRotation(Vector3.Normalize(Vector3.op_Subtraction(t.get_position(), animal.Position)));
            ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Rotation = Quaternion.Slerp(rotation, lookRotation, ((TimeInterval<float>) ref x).get_Value())));
            break;
        }
        if (this._enabledPositionValue)
          ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) iconnectableObservable, (Action<M0>) (x => animal.Position = Vector3.Lerp(position, t.get_position(), ((TimeInterval<float>) ref x).get_Value())));
        Action comEvent = completeEvent;
        ObservableExtensions.Subscribe<TimeInterval<float>[]>(Observable.TakeUntilDestroy<TimeInterval<float>[]>((IObservable<M0>) Observable.WhenAll<TimeInterval<float>>((IObservable<M0>[]) new IObservable<TimeInterval<float>>[1]
        {
          (IObservable<TimeInterval<float>>) iconnectableObservable
        }), ((Component) this).get_gameObject()), (Action<M0>) (_ =>
        {
          Action action = comEvent;
          if (action == null)
            return;
          action();
        }));
      }
    }

    public int InstanceID
    {
      get
      {
        return (!this._hasCode.HasValue ? (this._hasCode = new int?(((Object) this).GetInstanceID())) : this._hasCode).Value;
      }
    }

    public enum DirectionKind
    {
      Free,
      Lock,
      Look,
    }

    [Serializable]
    public class AnimalActionSlotTable : IEnumerable<AnimalActionPoint.AnimalActionSlot>, IEnumerable
    {
      [SerializeField]
      private List<AnimalActionPoint.AnimalActionSlot> _table = new List<AnimalActionPoint.AnimalActionSlot>();

      public List<AnimalActionPoint.AnimalActionSlot> Table
      {
        get
        {
          return this._table;
        }
      }

      public int Count
      {
        get
        {
          return this._table.Count;
        }
      }

      public AnimalActionPoint.AnimalActionSlot this[int index]
      {
        get
        {
          return this._table[index];
        }
        set
        {
          this._table[index] = value;
        }
      }

      public void Initialize()
      {
        this.Distinct();
      }

      private void Distinct()
      {
        List<AnimalActionPoint.AnimalActionSlot> toRelease1 = ListPool<AnimalActionPoint.AnimalActionSlot>.Get();
        foreach (AnimalActionPoint.AnimalActionSlot animalActionSlot1 in this._table)
        {
          bool flag = true;
          foreach (AnimalActionPoint.AnimalActionSlot animalActionSlot2 in toRelease1)
          {
            if (Object.op_Equality((Object) animalActionSlot2.Point, (Object) animalActionSlot1.Point))
            {
              flag = false;
              break;
            }
          }
          if (flag)
            toRelease1.Add(animalActionSlot1);
        }
        List<AnimalActionPoint.AnimalActionSlot> toRelease2 = ListPool<AnimalActionPoint.AnimalActionSlot>.Get();
        foreach (AnimalActionPoint.AnimalActionSlot animalActionSlot in this._table)
        {
          if (!toRelease1.Contains(animalActionSlot))
            toRelease2.Add(animalActionSlot);
        }
        foreach (AnimalActionPoint.AnimalActionSlot animalActionSlot in toRelease2)
          this._table.Remove(animalActionSlot);
        ListPool<AnimalActionPoint.AnimalActionSlot>.Release(toRelease2);
        ListPool<AnimalActionPoint.AnimalActionSlot>.Release(toRelease1);
      }

      public AnimalActionPoint.AnimalActionSlotTable.Enumerator GetEnumerator()
      {
        return new AnimalActionPoint.AnimalActionSlotTable.Enumerator(this);
      }

      IEnumerator<AnimalActionPoint.AnimalActionSlot> IEnumerable<AnimalActionPoint.AnimalActionSlot>.GetEnumerator()
      {
        return (IEnumerator<AnimalActionPoint.AnimalActionSlot>) new AnimalActionPoint.AnimalActionSlotTable.Enumerator(this);
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return (IEnumerator) new AnimalActionPoint.AnimalActionSlotTable.Enumerator(this);
      }

      public struct Enumerator : IEnumerator<AnimalActionPoint.AnimalActionSlot>, IDisposable, IEnumerator
      {
        private List<AnimalActionPoint.AnimalActionSlot> _list;
        private int _index;
        private AnimalActionPoint.AnimalActionSlot _current;

        public Enumerator(List<AnimalActionPoint.AnimalActionSlot> list)
        {
          this._list = list;
          this._index = 0;
          this._current = (AnimalActionPoint.AnimalActionSlot) null;
        }

        public Enumerator(AnimalActionPoint.AnimalActionSlotTable table)
        {
          this._list = table._table;
          this._index = 0;
          this._current = (AnimalActionPoint.AnimalActionSlot) null;
        }

        public AnimalActionPoint.AnimalActionSlot Current
        {
          get
          {
            return this._current;
          }
        }

        object IEnumerator.Current
        {
          get
          {
            if (this._index == 0 || this._index == this._list.Count + 1)
              throw new InvalidOperationException();
            return (object) this._current;
          }
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
          if (this._index >= this._list.Count)
            return this.MoveNextRare();
          this._current = this._list[this._index];
          ++this._index;
          return true;
        }

        private bool MoveNextRare()
        {
          this._index = this._list.Count + 1;
          this._current = (AnimalActionPoint.AnimalActionSlot) null;
          return false;
        }

        void IEnumerator.Reset()
        {
          this._index = 0;
          this._current = (AnimalActionPoint.AnimalActionSlot) null;
        }
      }
    }

    [Serializable]
    public class AnimalActionSlot
    {
      [SerializeField]
      private ActionTypes _acceptionKey;
      [SerializeField]
      private Transform _point;
      [SerializeField]
      private Transform _recoveryPoint;

      public ActionTypes AcceptionKey
      {
        get
        {
          return this._acceptionKey;
        }
      }

      public Transform Point
      {
        get
        {
          return this._point;
        }
      }

      public Transform RecoveryPoint
      {
        get
        {
          return this._recoveryPoint;
        }
      }
    }
  }
}
