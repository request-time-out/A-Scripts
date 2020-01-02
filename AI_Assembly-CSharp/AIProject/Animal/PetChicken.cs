// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.PetChicken
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using Housing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  [RequireComponent(typeof (NavMeshAgent))]
  public class PetChicken : MovingPetAnimal
  {
    public FarmPoint FarmPoint { get; set; }

    public int ChickenIndex { get; set; }

    public void Initialize(FarmPoint farmPoint)
    {
      this.Clear();
      FarmPoint farmPoint1 = farmPoint;
      this.FarmPoint = farmPoint1;
      if (Object.op_Equality((Object) farmPoint1, (Object) null))
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        this._restrictedPointList = this.FarmPoint.ChickenWaypointList;
        List<Waypoint> toRelease = ListPool<Waypoint>.Get();
        if (!this._restrictedPointList.IsNullOrEmpty<Waypoint>())
        {
          foreach (Waypoint restrictedPoint in (IEnumerable<Waypoint>) this._restrictedPointList)
          {
            if (Object.op_Inequality((Object) restrictedPoint, (Object) null) && restrictedPoint.Available((INavMeshActor) this))
              toRelease.Add(restrictedPoint);
          }
        }
        while (!((IReadOnlyList<Waypoint>) toRelease).IsNullOrEmpty<Waypoint>())
        {
          int index = Random.Range(0, toRelease.Count);
          Waypoint waypoint = toRelease[index];
          toRelease.RemoveAt(index);
          if (waypoint.Available((INavMeshActor) this))
          {
            this._destination = waypoint;
            break;
          }
        }
        ListPool<Waypoint>.Release(toRelease);
        Dictionary<int, AnimalModelInfo> source;
        if (Singleton<Manager.Resources>.Instance.AnimalTable.ModelInfoTable.TryGetValue(this.AnimalData.AnimalTypeID, out source) && !((IReadOnlyDictionary<int, AnimalModelInfo>) source).IsNullOrEmpty<int, AnimalModelInfo>())
        {
          KeyValuePair<int, AnimalModelInfo> keyValuePair = source.Rand<int, AnimalModelInfo>();
          this.AnimalData.ModelID = keyValuePair.Key;
          this.SetModelInfo(keyValuePair.Value);
        }
        this.LoadBody();
        this.SetStateData();
        if (Object.op_Equality((Object) this._nicknameRoot, (Object) null))
        {
          Transform transform = !Object.op_Inequality((Object) this.bodyObject, (Object) null) ? ((Component) this).get_transform() : this.bodyObject.get_transform();
          this._nicknameRoot = new GameObject("Nickname Root").get_transform();
          this._nicknameRoot.SetParent(transform, false);
          this._nicknameRoot.set_localPosition(new Vector3(0.0f, this.NicknameHeightOffset, 0.0f));
        }
        bool flag = false;
        this.MarkerEnabled = flag;
        this.BodyEnabled = flag;
        ((Behaviour) this.Agent).set_enabled(false);
        this._originPriority = Singleton<Manager.Resources>.Instance.AnimalDefinePack.AgentInfo.GroundAnimalStartPriority;
        this._originPriority += this.ChickenIndex;
        this.Agent.set_avoidancePriority(this._originPriority);
        if (Object.op_Inequality((Object) this._destination, (Object) null))
        {
          this._destination.Reserver = (INavMeshActor) this;
          this.Position = ((Component) this._destination).get_transform().get_position();
          this.Rotation = Quaternion.Euler(0.0f, Random.Range(0.0f, 360f), 0.0f);
        }
        this.SetState(AnimalState.Start, (Action) null);
      }
    }

    public override void Initialize(AIProject.SaveData.AnimalData animalData)
    {
      AIProject.SaveData.AnimalData animalData1 = animalData;
      this.AnimalData = animalData1;
      if (animalData1 == null)
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        animalData.AnimalID = this.AnimalID;
        int registerId = animalData.RegisterID;
      }
    }

    public override void Initialize(PetHomePoint _homePoint)
    {
    }

    public override void ReturnHome()
    {
      if (this._restrictedPointList.IsNullOrEmpty<Waypoint>())
        return;
      this.ReleaseDestination();
      this.ReleaseAgentPath();
      List<Waypoint> waypointList = ListPool<Waypoint>.Get();
      foreach (Waypoint restrictedPoint in (IEnumerable<Waypoint>) this._restrictedPointList)
      {
        if (Object.op_Inequality((Object) restrictedPoint, (Object) null) && restrictedPoint.Available((INavMeshActor) this))
          waypointList.Add(restrictedPoint);
      }
      this._destination = waypointList.Rand<Waypoint>();
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        ItemComponent itemComponent = !Object.op_Inequality((Object) this.FarmPoint, (Object) null) ? (ItemComponent) null : (ItemComponent) ((Component) this.FarmPoint).GetComponentInParent<ItemComponent>();
        if (Object.op_Inequality((Object) itemComponent, (Object) null))
          this.Position = ((Component) itemComponent).get_transform().get_position();
      }
      else
      {
        this._destination.Reserver = (INavMeshActor) this;
        this.Position = ((Component) this._destination).get_transform().get_position();
      }
      ListPool<Waypoint>.Release(waypointList);
      this.SetState(AnimalState.Idle, (Action) null);
    }

    protected override void EnterStart()
    {
      this.SetState(AnimalState.Idle, (Action) null);
      this.Agent.set_avoidancePriority(0);
    }

    protected override void ExitStart()
    {
      bool flag = true;
      this.MarkerEnabled = flag;
      this.BodyEnabled = flag;
      this.ActivateNavMeshAgent();
      this.StartWaypointRetention();
      this.Active = true;
    }

    protected override void EnterIdle()
    {
      this.PlayInAnim(AnimationCategoryID.Idle, 0, (Action) null);
      this.Agent.set_avoidancePriority(0);
    }

    protected override void OnIdle()
    {
      if (this.AnimationKeepWaiting())
        return;
      this.SetState(AnimalState.Locomotion, (Action) null);
    }

    protected override void ExitIdle()
    {
    }

    protected override void EnterLocomotion()
    {
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.ActivateNavMeshAgent();
      this.Agent.set_avoidancePriority(this._originPriority);
      this.Agent.set_speed(this.WalkSpeed);
      this.Agent.set_stoppingDistance(this.NormalStoppingDistance);
      this._arrivalCount = 0;
      this._arrivalLimit = this.NarrowArrivalLimit.RandomRange();
      this.Agent.set_isStopped(false);
      this.SetNextMovePoint();
    }

    protected override void OnLocomotion()
    {
      if (Object.op_Equality((Object) this._destination, (Object) null))
      {
        this.StateCounter = 0.0f;
        if (this._arrivalCount >= this._arrivalLimit)
          return;
        this.SetNextMovePoint();
      }
      else
      {
        if (!this.ClosedDestination())
          return;
        ++this._arrivalCount;
        if (this._arrivalCount < this._arrivalLimit)
        {
          this.SetNextMovePoint();
        }
        else
        {
          AnimalState _nextState = Random.Range(0, 100) >= 95 ? AnimalState.Eat : AnimalState.Idle;
          if (_nextState == AnimalState.Eat)
          {
            bool flag = Singleton<Manager.Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Resources>.Instance.AnimalDefinePack, (Object) null);
            if (flag)
            {
              AnimalDefinePack.ChickenCoopWaypointSettings coopWaypointSetting = Singleton<Manager.Resources>.Instance.AnimalDefinePack.ChickenCoopWaypointSetting;
              NavMeshHit navMeshHit;
              flag = NavMesh.FindClosestEdge(this.Position, ref navMeshHit, this.Agent.get_areaMask()) && (double) coopWaypointSetting.CanEatEdgeDistance <= (double) ((NavMeshHit) ref navMeshHit).get_distance();
            }
            if (!flag)
              _nextState = AnimalState.Idle;
          }
          this.SetState(_nextState, (Action) null);
        }
      }
    }

    protected override void ExitLocomotion()
    {
      this.SetFloat(this._locomotionParamName, 0.0f);
      this.ReleaseAgentPath();
      if (this.CurrentState == AnimalState.Destroyed)
        return;
      this.Agent.set_isStopped(true);
    }

    protected override void AnimationLocomotion()
    {
      Vector3 velocity = this.Agent.get_velocity();
      this.SetFloat(this._locomotionParamName, !Mathf.Approximately(((Vector3) ref velocity).get_magnitude(), 0.0f) ? 0.5f : 0.0f);
    }

    protected override void EnterRoar()
    {
      this.PlayInAnim(AnimationCategoryID.Action, 0, (Action) null);
      this.Agent.set_avoidancePriority(0);
    }

    protected override void OnRoar()
    {
      if (this.AnimationKeepWaiting())
        return;
      this.SetState(AnimalState.Idle, (Action) null);
    }

    protected override void ExitRoar()
    {
    }

    protected override void EnterEat()
    {
      this.PlayInAnim(AnimationCategoryID.Action, 0, (Action) null);
      this.Agent.set_avoidancePriority(0);
    }

    protected override void OnEat()
    {
      if (this.AnimationKeepWaiting())
        return;
      this.SetState(!AnimalBase.RandomBool ? AnimalState.Locomotion : AnimalState.Idle, (Action) null);
    }

    protected override void ExitEat()
    {
    }

    protected override void EnterAction0()
    {
    }

    protected override void OnAction0()
    {
    }

    protected override void ExitAction0()
    {
    }

    protected override void OnDestroy()
    {
      this.ReleaseDestination();
      this.ReleaseMovePointList();
      base.OnDestroy();
    }
  }
}
