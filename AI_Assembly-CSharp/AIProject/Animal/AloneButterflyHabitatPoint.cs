// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AloneButterflyHabitatPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace AIProject.Animal
{
  public class AloneButterflyHabitatPoint : SerializedMonoBehaviour
  {
    private List<AloneButterfly> _use;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private Transform _center;
    [SerializeField]
    private float _moveRadius;
    [SerializeField]
    private float _moveHeight;
    [SerializeField]
    private float _maxDelayTime;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _addAngle;
    [SerializeField]
    private float _turnAngle;
    [SerializeField]
    private float _changeTargetDistance;
    [SerializeField]
    private float _nextPointMaxDistance;
    [SerializeField]
    private float _speedDownDistance;
    [SerializeField]
    private Vector2Int _createNumRange;
    private int _userMaxCount;

    public AloneButterflyHabitatPoint()
    {
      base.\u002Ector();
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

    public float MaxDelayTime
    {
      get
      {
        return this._maxDelayTime;
      }
    }

    public float MoveSpeed
    {
      get
      {
        return this._moveSpeed;
      }
    }

    public float AddAngle
    {
      get
      {
        return this._addAngle;
      }
    }

    public float TurnAngle
    {
      get
      {
        return this._turnAngle;
      }
    }

    public float ChangeTargetDistance
    {
      get
      {
        return this._changeTargetDistance;
      }
    }

    public float NextPointMaxDistance
    {
      get
      {
        return this._nextPointMaxDistance;
      }
    }

    public float SpeedDownDistance
    {
      get
      {
        return this._speedDownDistance;
      }
    }

    public bool Available
    {
      get
      {
        return Object.op_Inequality((Object) this._center, (Object) null);
      }
    }

    public Vector2Int CreateNumRange
    {
      get
      {
        return this._createNumRange;
      }
    }

    private void Start()
    {
      this.Initialize();
    }

    private void Initialize()
    {
      if (Object.op_Equality((Object) this._prefab, (Object) null))
        return;
      if (Object.op_Equality((Object) this._center, (Object) null))
        this._center = ((Component) this).get_transform();
      this._userMaxCount = this._createNumRange.RandomRange();
      for (int index = 0; index < this._userMaxCount; ++index)
      {
        GameObject gameObject = new GameObject(string.Format("alone_butterfly_{0}", (object) index.ToString("00")));
        gameObject.get_transform().SetParent(((Component) this).get_transform(), false);
        AloneButterfly butterfly = gameObject.GetOrAddComponent<AloneButterfly>();
        butterfly.Initialize(this, this._prefab);
        ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) butterfly), (Component) this), (Action<M0>) (_ => this._use.Remove(butterfly)));
        this._use.Add(butterfly);
      }
    }
  }
}
