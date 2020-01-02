// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalSearchActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalSearchActionPoint : MonoBehaviour
  {
    [SerializeField]
    private AnimalBase animal;
    [SerializeField]
    [Tooltip("視界に入る距離")]
    private float _visibleDistance;
    [SerializeField]
    [Tooltip("視界に入る高さ")]
    private float _visibleHeight;
    [SerializeField]
    [Tooltip("視界に入る左右の角度")]
    private float _visibleAngle;
    private Dictionary<int, CollisionState> collisionStateTable;

    public AnimalSearchActionPoint()
    {
      base.\u002Ector();
    }

    public float VisibleDistance
    {
      get
      {
        return this._visibleDistance;
      }
    }

    public float VisibleHeight
    {
      get
      {
        return this._visibleHeight;
      }
    }

    public float VisibleAngle
    {
      get
      {
        return this._visibleAngle;
      }
    }

    public List<AnimalActionPoint> SearchPoints { get; private set; }

    public List<AnimalActionPoint> VisibleList { get; private set; }

    public bool SearchEnabled { get; set; }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.SearchEnabled)), (Action<M0>) (_ => this.OnUpdate()));
    }

    public void SetSearchEnabled(bool _enabled, bool _clearCollision = true)
    {
      if (_enabled == this.SearchEnabled)
        return;
      this.SearchEnabled = _enabled;
      if (this.SearchEnabled || !_clearCollision)
        return;
      this.ClearCollisionState();
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this.animal, (Object) null))
        return;
      ((Component) this.animal).get_transform();
      foreach (AnimalActionPoint searchPoint in this.SearchPoints)
      {
        if (!Object.op_Equality((Object) searchPoint, (Object) null))
        {
          int instanceId = searchPoint.InstanceID;
          CollisionState collisionState1;
          if (!this.collisionStateTable.TryGetValue(instanceId, out collisionState1))
          {
            CollisionState collisionState2 = CollisionState.None;
            this.collisionStateTable[instanceId] = collisionState2;
            collisionState1 = collisionState2;
          }
          if (((Component) searchPoint).get_gameObject().get_activeSelf() && this.animal.CheckTargetOnArea(searchPoint.Destination, this._visibleDistance, this._visibleHeight, this._visibleAngle))
          {
            switch (collisionState1)
            {
              case CollisionState.None:
              case CollisionState.Exit:
                this.collisionStateTable[instanceId] = CollisionState.Enter;
                this.OnEnter(searchPoint);
                continue;
              case CollisionState.Enter:
              case CollisionState.Stay:
                this.collisionStateTable[instanceId] = CollisionState.Stay;
                continue;
              default:
                continue;
            }
          }
          else
          {
            switch (collisionState1)
            {
              case CollisionState.None:
              case CollisionState.Exit:
                this.collisionStateTable[instanceId] = CollisionState.None;
                continue;
              case CollisionState.Enter:
              case CollisionState.Stay:
                this.collisionStateTable[instanceId] = CollisionState.Exit;
                this.OnExit(searchPoint);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void OnEnter(AnimalActionPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null) || this.VisibleList.Contains(point))
        return;
      this.VisibleList.Add(point);
    }

    private void OnExit(AnimalActionPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null))
        return;
      this.VisibleList.Remove(point);
    }

    public void RefreshQueryPoints()
    {
      AnimalActionPoint[] animalActionPoints = Singleton<Manager.Map>.Instance.PointAgent.AnimalActionPoints;
      this.SearchPoints.Clear();
      for (int index = 0; index < animalActionPoints.Length; ++index)
      {
        AnimalActionPoint animalActionPoint = animalActionPoints[index];
        if (Object.op_Inequality((Object) animalActionPoint, (Object) null))
          this.SearchPoints.Add(animalActionPoint);
      }
    }

    public void ClearCollisionState()
    {
      this.VisibleList.Clear();
      this.collisionStateTable.Clear();
    }
  }
}
