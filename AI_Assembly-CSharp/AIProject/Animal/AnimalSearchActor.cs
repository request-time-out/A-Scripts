// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalSearchActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal
{
  public class AnimalSearchActor : MonoBehaviour
  {
    [SerializeField]
    private AnimalBase animal;
    private Dictionary<int, ValueTuple<CollisionState, Actor>> nearCollisionStateTable;
    private Dictionary<int, ValueTuple<CollisionState, Actor>> farCollisionStateTable;
    [SerializeField]
    private Vector3 centerOffset;
    [SerializeField]
    private float nearSearchRadius;
    [SerializeField]
    private float farSearchRadius;
    [NonSerialized]
    public Action<PlayerActor> OnNearPlayerActorEnterEvent;
    [NonSerialized]
    public Action<PlayerActor> OnNearPlayerActorStayEvent;
    [NonSerialized]
    public Action<PlayerActor> OnNearPlayerActorExitEvent;
    [NonSerialized]
    public Action<AgentActor> OnNearAgentActorEnterEvent;
    [NonSerialized]
    public Action<AgentActor> OnNearAgentActorStayEvent;
    [NonSerialized]
    public Action<AgentActor> OnNearAgentActorExitEvent;
    [NonSerialized]
    public Action<Actor> OnNearActorEnterEvent;
    [NonSerialized]
    public Action<Actor> OnNearActorStayEvent;
    [NonSerialized]
    public Action<Actor> OnNearActorExitEvent;
    [NonSerialized]
    public Action<PlayerActor> OnFarPlayerActorEnterEvent;
    [NonSerialized]
    public Action<PlayerActor> OnFarPlayerActorStayEvent;
    [NonSerialized]
    public Action<PlayerActor> OnFarPlayerActorExitEvent;
    [NonSerialized]
    public Action<AgentActor> OnFarAgentActorEnterEvent;
    [NonSerialized]
    public Action<AgentActor> OnFarAgentActorStayEvent;
    [NonSerialized]
    public Action<AgentActor> OnFarAgentActorExitEvent;
    [NonSerialized]
    public Action<Actor> OnFarActorEnterEvent;
    [NonSerialized]
    public Action<Actor> OnFarActorStayEvent;
    [NonSerialized]
    public Action<Actor> OnFarActorExitEvent;

    public AnimalSearchActor()
    {
      base.\u002Ector();
    }

    private PlayerActor Player
    {
      get
      {
        return Singleton<Manager.Map>.IsInstance() ? Singleton<Manager.Map>.Instance.Player : (PlayerActor) null;
      }
    }

    private ReadOnlyDictionary<int, AgentActor> AgentActors
    {
      get
      {
        return Singleton<Manager.Map>.Instance?.AgentTable;
      }
    }

    private List<Actor> Actors
    {
      get
      {
        return Singleton<Manager.Map>.Instance?.Actors;
      }
    }

    public List<PlayerActor> VisiblePlayerActors { get; private set; }

    public List<AgentActor> VisibleAgentActors { get; private set; }

    public List<Actor> SearchActors { get; private set; }

    public List<Actor> VisibleActors { get; private set; }

    public Vector3 CenterOffset
    {
      get
      {
        return this.centerOffset;
      }
    }

    public float NearSearchRadius
    {
      get
      {
        return this.nearSearchRadius;
      }
    }

    public float FarSearchRadius
    {
      get
      {
        return this.farSearchRadius;
      }
    }

    public bool SearchEnabled { get; private set; }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDisable<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.SearchEnabled)), (Action<M0>) (_ => this.OnUpdate()));
    }

    public void SetSearchEnabled(bool _enabled, bool _clearCollision = false)
    {
      if (_enabled == this.SearchEnabled)
        return;
      this.SearchEnabled = _enabled;
      if (this.SearchEnabled || !_clearCollision)
        return;
      this.VisiblePlayerActors.Clear();
      this.VisibleAgentActors.Clear();
      this.VisibleActors.Clear();
      this.nearCollisionStateTable.Clear();
      this.farCollisionStateTable.Clear();
    }

    private void SetCollisionState(
      Dictionary<int, ValueTuple<CollisionState, Actor>> _collisionStateTable,
      int _key,
      CollisionState _state,
      Actor _actor)
    {
      ValueTuple<CollisionState, Actor> valueTuple;
      if (_collisionStateTable.TryGetValue(_key, out valueTuple))
      {
        valueTuple.Item1 = (__Null) _state;
        valueTuple.Item2 = (__Null) _actor;
        _collisionStateTable[_key] = valueTuple;
      }
      else
        _collisionStateTable[_key] = new ValueTuple<CollisionState, Actor>(_state, _actor);
    }

    public Vector3 SearchPoint
    {
      get
      {
        return Object.op_Implicit((Object) this.animal) ? Vector3.op_Addition(this.animal.Position, Quaternion.op_Multiply(this.animal.Rotation, this.centerOffset)) : Vector3.get_zero();
      }
    }

    private void OnUpdate()
    {
      ((Component) this.animal).get_transform();
      foreach (Actor searchActor in this.SearchActors)
      {
        if (!Object.op_Equality((Object) searchActor, (Object) null))
        {
          int instanceId = searchActor.InstanceID;
          float num = Vector3.Distance(searchActor.Position, this.SearchPoint);
          ValueTuple<CollisionState, Actor> valueTuple1;
          if (!this.farCollisionStateTable.TryGetValue(instanceId, out valueTuple1))
          {
            ValueTuple<CollisionState, Actor> valueTuple2 = new ValueTuple<CollisionState, Actor>(CollisionState.None, searchActor);
            this.farCollisionStateTable[instanceId] = valueTuple2;
            valueTuple1 = valueTuple2;
          }
          if ((double) num <= (double) this.FarSearchRadius)
          {
            switch ((int) valueTuple1.Item1)
            {
              case 0:
              case 3:
                this.SetCollisionState(this.farCollisionStateTable, instanceId, CollisionState.Enter, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnFarEnter(searchActor as PlayerActor);
                    break;
                  case AgentActor _:
                    this.OnFarEnter(searchActor as AgentActor);
                    break;
                  default:
                    this.OnFarEnter(searchActor);
                    break;
                }
                break;
              case 1:
              case 2:
                this.SetCollisionState(this.farCollisionStateTable, instanceId, CollisionState.Stay, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnFarStay(searchActor as PlayerActor);
                    break;
                  case AgentActor _:
                    this.OnFarStay(searchActor as AgentActor);
                    break;
                  default:
                    this.OnFarStay(searchActor);
                    break;
                }
                break;
            }
          }
          else
          {
            switch ((int) valueTuple1.Item1)
            {
              case 0:
              case 3:
                this.SetCollisionState(this.farCollisionStateTable, instanceId, CollisionState.None, searchActor);
                break;
              case 1:
              case 2:
                this.SetCollisionState(this.farCollisionStateTable, instanceId, CollisionState.Exit, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnFarExit(searchActor as PlayerActor);
                    break;
                  case AgentActor _:
                    this.OnFarExit(searchActor as AgentActor);
                    break;
                  default:
                    this.OnFarExit(searchActor);
                    break;
                }
                break;
            }
          }
          ValueTuple<CollisionState, Actor> valueTuple3;
          if (!this.nearCollisionStateTable.TryGetValue(instanceId, out valueTuple3))
          {
            ValueTuple<CollisionState, Actor> valueTuple2 = new ValueTuple<CollisionState, Actor>(CollisionState.None, searchActor);
            this.nearCollisionStateTable[instanceId] = valueTuple2;
            valueTuple3 = valueTuple2;
          }
          if ((double) num <= (double) this.NearSearchRadius)
          {
            switch ((int) valueTuple3.Item1)
            {
              case 0:
              case 3:
                this.SetCollisionState(this.nearCollisionStateTable, instanceId, CollisionState.Enter, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnNearEnter(searchActor as PlayerActor);
                    continue;
                  case AgentActor _:
                    this.OnNearEnter(searchActor as AgentActor);
                    continue;
                  default:
                    this.OnNearEnter(searchActor);
                    continue;
                }
              case 1:
              case 2:
                this.SetCollisionState(this.nearCollisionStateTable, instanceId, CollisionState.Stay, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnNearStay(searchActor as PlayerActor);
                    continue;
                  case AgentActor _:
                    this.OnNearStay(searchActor as AgentActor);
                    continue;
                  default:
                    this.OnNearStay(searchActor);
                    continue;
                }
              default:
                continue;
            }
          }
          else
          {
            switch ((int) valueTuple3.Item1)
            {
              case 0:
              case 3:
                this.SetCollisionState(this.nearCollisionStateTable, instanceId, CollisionState.None, searchActor);
                continue;
              case 1:
              case 2:
                this.SetCollisionState(this.nearCollisionStateTable, instanceId, CollisionState.Exit, searchActor);
                switch (searchActor)
                {
                  case PlayerActor _:
                    this.OnNearExit(searchActor as PlayerActor);
                    continue;
                  case AgentActor _:
                    this.OnNearExit(searchActor as AgentActor);
                    continue;
                  default:
                    this.OnNearExit(searchActor);
                    continue;
                }
              default:
                continue;
            }
          }
        }
      }
    }

    private void OnNearEnter(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearPlayerActorEnterEvent != null)
        this.OnNearPlayerActorEnterEvent(_actor);
      this.OnNearEnter((Actor) _actor);
    }

    private void OnNearStay(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearPlayerActorStayEvent != null)
        this.OnNearPlayerActorStayEvent(_actor);
      this.OnNearStay((Actor) _actor);
    }

    private void OnNearExit(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearPlayerActorExitEvent != null)
        this.OnNearPlayerActorExitEvent(_actor);
      this.OnNearExit((Actor) _actor);
    }

    private void OnNearEnter(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearAgentActorEnterEvent != null)
        this.OnNearAgentActorEnterEvent(_actor);
      this.OnNearEnter((Actor) _actor);
    }

    private void OnNearStay(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearAgentActorStayEvent != null)
        this.OnNearAgentActorStayEvent(_actor);
      this.OnNearStay((Actor) _actor);
    }

    private void OnNearExit(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnNearAgentActorExitEvent != null)
        this.OnNearAgentActorExitEvent(_actor);
      this.OnNearExit((Actor) _actor);
    }

    private void OnNearEnter(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null) || this.OnNearActorEnterEvent == null)
        return;
      this.OnNearActorEnterEvent(_actor);
    }

    private void OnNearStay(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null) || this.OnNearActorStayEvent == null)
        return;
      this.OnNearActorStayEvent(_actor);
    }

    private void OnNearExit(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null) || this.OnNearActorExitEvent == null)
        return;
      this.OnNearActorExitEvent(_actor);
    }

    private void OnFarEnter(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (!this.VisiblePlayerActors.Contains(_actor))
        this.VisiblePlayerActors.Add(_actor);
      if (this.OnFarPlayerActorEnterEvent != null)
        this.OnFarPlayerActorEnterEvent(_actor);
      this.OnFarEnter((Actor) _actor);
    }

    private void OnFarStay(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnFarPlayerActorStayEvent != null)
        this.OnFarPlayerActorStayEvent(_actor);
      this.OnFarStay((Actor) _actor);
    }

    private void OnFarExit(PlayerActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
      {
        this.VisiblePlayerActors.RemoveAll((Predicate<PlayerActor>) (x => Object.op_Equality((Object) x, (Object) null)));
      }
      else
      {
        this.VisiblePlayerActors.Remove(_actor);
        if (this.OnFarPlayerActorExitEvent != null)
          this.OnFarPlayerActorExitEvent(_actor);
        this.OnFarExit((Actor) _actor);
      }
    }

    private void OnFarEnter(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (!this.VisibleAgentActors.Contains(_actor))
        this.VisibleAgentActors.Add(_actor);
      if (this.OnFarAgentActorEnterEvent != null)
        this.OnFarAgentActorEnterEvent(_actor);
      this.OnFarEnter((Actor) _actor);
    }

    private void OnFarStay(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (this.OnFarAgentActorStayEvent != null)
        this.OnFarAgentActorStayEvent(_actor);
      this.OnFarStay((Actor) _actor);
    }

    private void OnFarExit(AgentActor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
      {
        this.VisibleAgentActors.RemoveAll((Predicate<AgentActor>) (x => Object.op_Equality((Object) x, (Object) null)));
      }
      else
      {
        this.VisibleAgentActors.Remove(_actor);
        if (this.OnFarAgentActorExitEvent != null)
          this.OnFarAgentActorExitEvent(_actor);
        this.OnFarExit((Actor) _actor);
      }
    }

    private void OnFarEnter(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
        return;
      if (!this.VisibleActors.Contains(_actor))
        this.VisibleActors.Add(_actor);
      if (this.OnFarActorEnterEvent == null)
        return;
      this.OnFarActorEnterEvent(_actor);
    }

    private void OnFarStay(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null) || this.OnFarActorStayEvent == null)
        return;
      this.OnFarActorStayEvent(_actor);
    }

    private void OnFarExit(Actor _actor)
    {
      if (Object.op_Equality((Object) _actor, (Object) null))
      {
        this.VisibleActors.RemoveAll((Predicate<Actor>) (x => Object.op_Equality((Object) x, (Object) null)));
      }
      else
      {
        this.VisibleActors.Remove(_actor);
        if (this.OnFarActorExitEvent == null)
          return;
        this.OnFarActorExitEvent(_actor);
      }
    }

    public bool OnSearchArea(Vector3 _checkPoint)
    {
      return (double) Vector3.Distance(_checkPoint, this.SearchPoint) <= (double) this.farSearchRadius;
    }

    public bool CheckPlayerOnNearSearchArea()
    {
      PlayerActor player = this.Player;
      ValueTuple<CollisionState, Actor> valueTuple;
      if (!Object.op_Implicit((Object) player) || !this.nearCollisionStateTable.TryGetValue(player.InstanceID, out valueTuple))
        return false;
      switch ((CollisionState) valueTuple.Item1)
      {
        case CollisionState.Enter:
        case CollisionState.Stay:
          return true;
        default:
          return false;
      }
    }

    public bool CheckPlayerOnFarSearchArea()
    {
      PlayerActor player = this.Player;
      ValueTuple<CollisionState, Actor> valueTuple;
      if (!Object.op_Implicit((Object) player) || !this.farCollisionStateTable.TryGetValue(player.InstanceID, out valueTuple))
        return false;
      switch ((CollisionState) valueTuple.Item1)
      {
        case CollisionState.Enter:
        case CollisionState.Stay:
          return true;
        default:
          return false;
      }
    }

    public void RefreshSearchActorTable()
    {
      this.SearchActors.Clear();
      if (Object.op_Inequality((Object) this.Player, (Object) null))
        this.SearchActors.Add((Actor) this.Player);
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = this.AgentActors.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Value, (Object) null))
            this.SearchActors.Add((Actor) current.Value);
        }
      }
    }

    public void ClearCollisionState()
    {
      this.nearCollisionStateTable.Clear();
      this.farCollisionStateTable.Clear();
      this.VisiblePlayerActors.Clear();
      this.VisibleAgentActors.Clear();
      this.VisibleActors.Clear();
    }

    public void ClearCollisionStateWithExit()
    {
      while (!((IReadOnlyList<PlayerActor>) this.VisiblePlayerActors).IsNullOrEmpty<PlayerActor>())
        this.OnFarExit(this.VisiblePlayerActors[0]);
      while (!((IReadOnlyList<AgentActor>) this.VisibleAgentActors).IsNullOrEmpty<AgentActor>())
        this.OnFarExit(this.VisibleAgentActors[0]);
      while (!((IReadOnlyList<Actor>) this.VisibleActors).IsNullOrEmpty<Actor>())
        this.OnFarExit(this.VisibleActors[0]);
      this.ClearCollisionState();
    }

    private void OnDestroy()
    {
      this.OnNearPlayerActorEnterEvent = (Action<PlayerActor>) null;
      this.OnNearPlayerActorStayEvent = (Action<PlayerActor>) null;
      this.OnNearPlayerActorExitEvent = (Action<PlayerActor>) null;
      this.OnNearAgentActorEnterEvent = (Action<AgentActor>) null;
      this.OnNearAgentActorStayEvent = (Action<AgentActor>) null;
      this.OnNearAgentActorExitEvent = (Action<AgentActor>) null;
      this.OnNearActorEnterEvent = (Action<Actor>) null;
      this.OnNearActorStayEvent = (Action<Actor>) null;
      this.OnNearActorExitEvent = (Action<Actor>) null;
      this.OnFarPlayerActorEnterEvent = (Action<PlayerActor>) null;
      this.OnFarPlayerActorStayEvent = (Action<PlayerActor>) null;
      this.OnFarPlayerActorExitEvent = (Action<PlayerActor>) null;
      this.OnFarAgentActorEnterEvent = (Action<AgentActor>) null;
      this.OnFarAgentActorStayEvent = (Action<AgentActor>) null;
      this.OnFarAgentActorExitEvent = (Action<AgentActor>) null;
      this.OnFarActorEnterEvent = (Action<Actor>) null;
      this.OnFarActorStayEvent = (Action<Actor>) null;
      this.OnFarActorExitEvent = (Action<Actor>) null;
    }
  }
}
