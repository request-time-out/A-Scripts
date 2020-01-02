// Decompiled with JetBrains decompiler
// Type: AIProject.SearchArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class SearchArea : MonoBehaviour
  {
    [SerializeField]
    private AgentActor _agent;
    [SerializeField]
    private float _radius;
    private Dictionary<int, CollisionState> _collisionStateTable;
    private List<ActionPoint> _searchPoints;

    public SearchArea()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (!Object.op_Equality((Object) this._agent, (Object) null))
        return;
      this._agent = (AgentActor) ((Component) this).GetComponentInChildren<AgentActor>();
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this._agent, (Object) null))
        return;
      foreach (ActionPoint searchPoint in this._searchPoints)
      {
        if (!Object.op_Equality((Object) searchPoint, (Object) null))
        {
          int instanceId = searchPoint.InstanceID;
          CollisionState collisionState1;
          if (!this._collisionStateTable.TryGetValue(instanceId, out collisionState1))
          {
            CollisionState collisionState2 = CollisionState.None;
            this._collisionStateTable[instanceId] = collisionState2;
            collisionState1 = collisionState2;
          }
          if ((double) Vector3.Distance(((Component) this).get_transform().get_position(), searchPoint.CommandCenter) < (double) this._radius + (double) searchPoint.Radius)
          {
            switch (collisionState1)
            {
              case CollisionState.None:
              case CollisionState.Exit:
                this._collisionStateTable[instanceId] = CollisionState.Enter;
                this.OnEnter(searchPoint);
                continue;
              case CollisionState.Enter:
              case CollisionState.Stay:
                this._collisionStateTable[instanceId] = CollisionState.Stay;
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
                this._collisionStateTable[instanceId] = CollisionState.None;
                continue;
              case CollisionState.Enter:
              case CollisionState.Stay:
                this._collisionStateTable[instanceId] = CollisionState.Exit;
                this.OnExit(searchPoint);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void OnEnter(ActionPoint point)
    {
      if (Object.op_Equality((Object) point, (Object) null) || Object.op_Inequality((Object) point.Actor, (Object) null))
        return;
      this._agent.AddActionPoint(point);
    }

    private void OnExit(ActionPoint point)
    {
      if (Object.op_Equality((Object) point?.Actor, (Object) null))
        return;
      this._agent.RemoveActionPoint(point);
    }

    public void RefreshQueryPoints()
    {
      ActionPoint[] actionPoints = Singleton<Manager.Map>.Instance.PointAgent.ActionPoints;
      this._searchPoints.Clear();
      this._searchPoints.AddRange((IEnumerable<ActionPoint>) actionPoints);
    }

    public void AddPoint(ActionPoint ap)
    {
      if (Object.op_Equality((Object) ap, (Object) null))
        return;
      this._searchPoints.Add(ap);
    }

    public void AddPoints(ActionPoint[] ap)
    {
      this._searchPoints.AddRange((IEnumerable<ActionPoint>) ap);
    }

    public bool RemovePoint(ActionPoint ap)
    {
      return !Object.op_Equality((Object) ap, (Object) null) && this._searchPoints.Remove(ap);
    }
  }
}
