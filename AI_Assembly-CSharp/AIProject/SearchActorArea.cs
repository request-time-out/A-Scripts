// Decompiled with JetBrains decompiler
// Type: AIProject.SearchActorArea
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class SearchActorArea : MonoBehaviour
  {
    [SerializeField]
    private AgentActor _agent;
    [SerializeField]
    private Vector3 _centerOffset;
    [SerializeField]
    private float _radius;
    private Dictionary<int, CollisionState> _collisionStateTable;

    public SearchActorArea()
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

    private void SetCollisionState(int key, CollisionState state)
    {
      this._collisionStateTable[key] = state;
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this._agent, (Object) null))
        return;
      float searchActorRadius = Singleton<Resources>.Instance.LocomotionProfile.SearchActorRadius;
      ((Component) this).get_transform().set_position(this._agent.Position);
      Vector3 vector3 = Vector3.op_Addition(this._agent.Position, Quaternion.op_Multiply(this._agent.Rotation, this._centerOffset));
      foreach (Actor targetActor in this._agent.TargetActors)
      {
        float num1 = Vector3.Distance(targetActor.Position, vector3);
        float num2 = this._radius + searchActorRadius;
        int instanceId = targetActor.InstanceID;
        CollisionState collisionState1;
        if (!this._collisionStateTable.TryGetValue(instanceId, out collisionState1))
        {
          CollisionState collisionState2 = CollisionState.None;
          this._collisionStateTable[instanceId] = collisionState2;
          collisionState1 = collisionState2;
        }
        if ((double) num1 <= (double) num2)
        {
          switch (collisionState1)
          {
            case CollisionState.None:
            case CollisionState.Exit:
              this.SetCollisionState(instanceId, CollisionState.Enter);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(instanceId, CollisionState.Stay);
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
              this.SetCollisionState(instanceId, CollisionState.None);
              continue;
            case CollisionState.Enter:
            case CollisionState.Stay:
              this.SetCollisionState(instanceId, CollisionState.Exit);
              continue;
            default:
              continue;
          }
        }
      }
      foreach (Actor targetActor in this._agent.TargetActors)
      {
        CollisionState collisionState;
        if (!this._collisionStateTable.TryGetValue(targetActor.InstanceID, out collisionState) || collisionState != CollisionState.Enter && collisionState != CollisionState.Exit || (collisionState == CollisionState.Enter || collisionState == CollisionState.Exit))
          ;
      }
    }
  }
}
