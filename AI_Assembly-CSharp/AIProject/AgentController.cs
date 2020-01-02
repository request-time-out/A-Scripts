// Decompiled with JetBrains decompiler
// Type: AIProject.AgentController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [RequireComponent(typeof (ActorLocomotion))]
  public class AgentController : ActorController
  {
    [SerializeField]
    private float _distanceReached = 0.5f;
    private DateTime _startTime = DateTime.MinValue;
    private TimeSpan _extendedTime = TimeSpan.MinValue;
    [SerializeField]
    private ActorLocomotionAgent _character;
    [SerializeField]
    private SearchArea _searchArea;
    private bool _sleepedSchedule;

    public SearchArea SearchArea
    {
      get
      {
        return this._searchArea;
      }
    }

    public float DistanceReached
    {
      get
      {
        return this._distanceReached;
      }
    }

    public int PrevID { get; private set; } = -1;

    public EventType PrevEvent { get; private set; }

    public Rarelity PrevRarelity { get; private set; } = Rarelity.N;

    public AgentController.PermissionStatus Permission { get; private set; }

    public bool SleepedSchedule
    {
      get
      {
        return this._sleepedSchedule;
      }
      set
      {
        if (value == this._sleepedSchedule)
          return;
        this._sleepedSchedule = value;
        EnvironmentSimulator simulator = Singleton<Manager.Map>.Instance.Simulator;
        if (value)
          this._startTime = simulator.Now;
        else
          this._extendedTime = simulator.Now - this._startTime;
      }
    }

    private void OnDisable()
    {
      if (!Object.op_Implicit((Object) this._character))
        return;
      ((Behaviour) this._character).set_enabled(false);
    }

    public override void StartBehavior()
    {
      if (!Object.op_Equality((Object) this._character, (Object) null))
        return;
      this._character = (ActorLocomotionAgent) ((Component) this).GetComponent<ActorLocomotionAgent>();
    }

    protected override void Start()
    {
      base.Start();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (System.Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      bool? nullable = this._actor != null ? new bool?(this._actor.IsInit) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      if (this._character != null)
        this._character.Move(Vector3.get_zero());
      if (!Singleton<Scene>.Instance.IsNowLoadingFade)
        ;
    }

    protected override void SubFixedUpdate()
    {
      if (this._character == null)
        return;
      this._character.UpdateState();
    }

    public AgentController.PermissionStatus GetPermission(ActionPoint point)
    {
      AgentActor actor = this._actor as AgentActor;
      ValueTuple<EventType, Desire.Type>[] valuePairs = Desire.ValuePairs;
      Desire.Type requestedDesire = actor.RequestedDesire;
      if (!point.IsNeutralCommand)
      {
        Debug.Log((object) "行動ポイントが満員御礼");
        AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
        this.Permission = permissionStatus1;
        AgentController.PermissionStatus permissionStatus2;
        return permissionStatus2 = permissionStatus1;
      }
      AgentController.PermissionStatus permissionStatus3;
      if (AgentController.AnyDesire(valuePairs, requestedDesire))
      {
        EventType type = (EventType) AgentController.FirstDesire(valuePairs, requestedDesire).Item1;
        if (requestedDesire == Desire.Type.Bath && (double) actor.ChaControl.fileGameInfo.flavorState[2] < (double) Singleton<Resources>.Instance.StatusProfile.CanDressBorder)
          type = EventType.Bath;
        if (point.AgentEventType.Contains(type))
        {
          if (point is SearchActionPoint)
          {
            SearchActionPoint searchActionPoint = point as SearchActionPoint;
            StuffItem itemInfo = actor.AgentData.EquipedSearchItem(searchActionPoint.TableID);
            if (actor.SearchAreaID == 0)
            {
              if (searchActionPoint.TableID == 0 || searchActionPoint.TableID == 1 || searchActionPoint.TableID == 2)
              {
                if (searchActionPoint.CanSearch(EventType.Search, itemInfo))
                {
                  AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Permission;
                  this.Permission = permissionStatus1;
                  permissionStatus3 = permissionStatus1;
                }
                else
                {
                  AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
                  this.Permission = permissionStatus1;
                  permissionStatus3 = permissionStatus1;
                }
              }
              else
              {
                AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
                this.Permission = permissionStatus1;
                permissionStatus3 = permissionStatus1;
              }
            }
            else if (actor.SearchAreaID == searchActionPoint.TableID)
            {
              if (searchActionPoint.CanSearch(EventType.Search, itemInfo))
              {
                AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Permission;
                this.Permission = permissionStatus1;
                permissionStatus3 = permissionStatus1;
              }
              else
              {
                AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
                this.Permission = permissionStatus1;
                permissionStatus3 = permissionStatus1;
              }
            }
            else
            {
              AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
              this.Permission = permissionStatus1;
              permissionStatus3 = permissionStatus1;
            }
          }
          else
          {
            AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Permission;
            this.Permission = permissionStatus1;
            permissionStatus3 = permissionStatus1;
          }
        }
        else
        {
          AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
          this.Permission = permissionStatus1;
          permissionStatus3 = permissionStatus1;
        }
      }
      else
      {
        AgentController.PermissionStatus permissionStatus1 = AgentController.PermissionStatus.Prohibition;
        this.Permission = permissionStatus1;
        permissionStatus3 = permissionStatus1;
      }
      return permissionStatus3;
    }

    private static bool AnyDesire(
      ReadOnlyDictionary<EventType, Desire.Type> table,
      Desire.Type type)
    {
      using (IEnumerator<KeyValuePair<EventType, Desire.Type>> enumerator = table.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.Value == type)
            return true;
        }
      }
      return false;
    }

    private static bool AnyDesire(ValueTuple<EventType, Desire.Type>[] collection, Desire.Type type)
    {
      foreach (ValueTuple<EventType, Desire.Type> valueTuple in collection)
      {
        if ((Desire.Type) valueTuple.Item2 == type)
          return true;
      }
      return false;
    }

    private static KeyValuePair<EventType, Desire.Type> FirstDesire(
      ReadOnlyDictionary<EventType, Desire.Type> table,
      Desire.Type type)
    {
      using (IEnumerator<KeyValuePair<EventType, Desire.Type>> enumerator = table.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<EventType, Desire.Type> current = enumerator.Current;
          if (current.Value == type)
            return current;
        }
      }
      return new KeyValuePair<EventType, Desire.Type>();
    }

    public static ValueTuple<EventType, Desire.Type> FirstDesire(
      ValueTuple<EventType, Desire.Type>[] collection,
      Desire.Type type)
    {
      foreach (ValueTuple<EventType, Desire.Type> valueTuple in collection)
      {
        if ((Desire.Type) valueTuple.Item2 == type)
          return valueTuple;
      }
      return (ValueTuple<EventType, Desire.Type>) null;
    }

    private void OnTriggerExit(Collider other)
    {
    }

    public override void ChangeState(string stateName)
    {
      if (Singleton<Scene>.Instance.AddSceneName.IsNullOrEmpty())
        ;
    }

    public enum PermissionStatus
    {
      None,
      Prohibition,
      Permission,
    }
  }
}
