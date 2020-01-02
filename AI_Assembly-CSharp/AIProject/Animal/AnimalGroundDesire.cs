// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalGroundDesire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Animal
{
  public class AnimalGroundDesire : AnimalGround
  {
    [SerializeField]
    protected AnimalDesireController desireController;
    [SerializeField]
    private AnimalSearchActionPoint _searchAction;

    public AnimalSearchActionPoint SearchAction
    {
      get
      {
        return this._searchAction;
      }
    }

    public List<AnimalActionPoint> ActionPoints
    {
      get
      {
        return Object.op_Inequality((Object) this.SearchAction, (Object) null) ? this.SearchAction.SearchPoints : (List<AnimalActionPoint>) null;
      }
    }

    public List<AnimalActionPoint> VisibleActionPoints
    {
      get
      {
        return Object.op_Inequality((Object) this.SearchAction, (Object) null) ? this.SearchAction.VisibleList : (List<AnimalActionPoint>) null;
      }
    }

    public override bool WaitPossible
    {
      get
      {
        return this.CurrentState == AnimalState.Idle || this.CurrentState == AnimalState.Locomotion || (this.CurrentState == AnimalState.Grooming || this.CurrentState == AnimalState.MoveEars) || (this.CurrentState == AnimalState.Roar || this.CurrentState == AnimalState.Peck || (this.CurrentState == AnimalState.Action0 || this.CurrentState == AnimalState.Action1)) || this.CurrentState == AnimalState.Action2;
      }
    }

    protected override void Awake()
    {
      base.Awake();
      if (!Object.op_Implicit((Object) this.desireController))
        this.desireController = (AnimalDesireController) ((Component) this).GetComponentInChildren<AnimalDesireController>(true);
      if (!Object.op_Implicit((Object) this.desireController))
        return;
      this.desireController.DesireFilledEvent = new Func<DesireType, bool>(this.DesireFilledEvent);
      this.desireController.ChangedCandidateDesireEvent = new Func<DesireType, bool>(this.ChangedCandidateDesire);
    }

    public override void SetSearchTargetEnabled(bool _enabled, bool _clearCollision = true)
    {
      if (Object.op_Inequality((Object) this.SearchActor, (Object) null))
        this.SearchActor.SetSearchEnabled(_enabled, _clearCollision);
      if (!Object.op_Inequality((Object) this.SearchAction, (Object) null))
        return;
      this.SearchAction.SetSearchEnabled(_enabled, _clearCollision);
    }

    public override void RefreshSearchTarget()
    {
      if (Object.op_Inequality((Object) this.SearchAction, (Object) null))
        this.SearchAction.RefreshQueryPoints();
      if (!Object.op_Inequality((Object) this.SearchActor, (Object) null))
        return;
      this.SearchActor.RefreshSearchActorTable();
    }

    public override void Clear()
    {
      this.desireController.Clear();
      base.Clear();
    }

    public void ReduceDesireParameter(bool _success, DesireType _changeDesire)
    {
      List<AnimalState> animalStateList;
      if (!this.desireController.TargetStateTable.TryGetValue(_changeDesire, out animalStateList) || !animalStateList.Contains(this.CurrentState))
        return;
      this.desireController.ReduceParameter(_success, _changeDesire);
    }

    protected override void ChangedStateEvent()
    {
      base.ChangedStateEvent();
      if (this.desireController.HasCurrentDesire)
        this.ReduceDesireParameter(true, this.desireController.CurrentDesire);
      if (!this.desireController.HasCandidateDesire)
        return;
      this.ReduceDesireParameter(true, this.desireController.CandidateDesire);
    }

    public override void OnMinuteUpdate(TimeSpan _deltaTime)
    {
      base.OnMinuteUpdate(_deltaTime);
      this.desireController.OnMinuteUpdate();
    }

    protected virtual bool DesireFilledEvent(DesireType _desireType)
    {
      return false;
    }

    protected virtual bool ChangedCandidateDesire(DesireType _desireType)
    {
      return !this.desireController.HasCurrentDesire;
    }

    public override bool SetActionPoint(
      AnimalActionPoint _actionPoint,
      AnimalState _nextState,
      ActionTypes _actionType)
    {
      if (!((Behaviour) this.Agent).get_enabled())
        return this.SetActionPointNonAgent(_actionPoint, _nextState, _actionType);
      if (Object.op_Equality((Object) _actionPoint, (Object) null) || Object.op_Equality((Object) this.actionPoint, (Object) _actionPoint) || _nextState == AnimalState.None)
        return false;
      if (this.calculatePath == null)
        this.calculatePath = new NavMeshPath();
      if (!this.Agent.CalculatePath(_actionPoint.Destination, this.calculatePath) || this.calculatePath.get_status() != null || !this.Agent.SetPath(this.calculatePath))
        return false;
      this.calculatePath = (NavMeshPath) null;
      this.ClearCurrentWaypoint();
      this.destination = new Vector3?();
      this.LocomotionCount = 1;
      this.NextState = _nextState;
      if (Object.op_Inequality((Object) this.actionPoint, (Object) null))
        this.actionPoint.RemoveBooking((IAnimalActionPointUser) this);
      this.actionPoint = _actionPoint;
      this.actionPoint.AddBooking((IAnimalActionPointUser) this);
      this.ActionType = _actionType;
      return true;
    }

    public bool SetActionPointNonAgent(
      AnimalActionPoint _actionPoint,
      AnimalState _nextState,
      ActionTypes _actionType)
    {
      if (Object.op_Equality((Object) _actionPoint, (Object) null) || Object.op_Equality((Object) this.actionPoint, (Object) _actionPoint) || _nextState == AnimalState.None)
        return false;
      this.ClearCurrentWaypoint();
      this.LocomotionCount = 0;
      this.NextState = _nextState;
      if (Object.op_Inequality((Object) this.actionPoint, (Object) null))
        this.actionPoint.RemoveBooking((IAnimalActionPointUser) this);
      this.actionPoint = _actionPoint;
      this.actionPoint.AddBooking((IAnimalActionPointUser) this);
      this.ActionType = _actionType;
      return true;
    }

    protected AnimalActionPoint[] GetActionPoints(ActionTypes _actionType)
    {
      List<AnimalActionPoint> toRelease = ListPool<AnimalActionPoint>.Get();
      for (int index = 0; index < this.ActionPoints.Count; ++index)
      {
        AnimalActionPoint actionPoint = this.ActionPoints[index];
        if (actionPoint.Available((IAnimalActionPointUser) this) && (actionPoint.ActionType & _actionType) != ActionTypes.None)
          toRelease.Add(actionPoint);
      }
      AnimalActionPoint[] animalActionPointArray = new AnimalActionPoint[toRelease.Count];
      for (int index = 0; index < toRelease.Count; ++index)
        animalActionPointArray[index] = toRelease[index];
      ListPool<AnimalActionPoint>.Release(toRelease);
      return animalActionPointArray;
    }

    protected virtual void EnterAction()
    {
      if (!this.HasActionPoint)
        return;
      if (this.IsNearPoint(this.actionPoint.Destination) && this.ActionType.Contains(this.actionPoint.ActionType))
      {
        this.actionPoint.SetUse((IAnimalActionPointUser) this);
        this.actionPoint.SetStand((IAnimalActionPointUser) this, this.actionPoint.GetSlot(this.ActionType).Item1);
        this.prevAgentEnabled = ((Behaviour) this.Agent).get_enabled();
        if (this.prevAgentEnabled == this.actionPoint.EnabledNavMeshAgent)
          return;
        ((Behaviour) this.Agent).set_enabled(this.actionPoint.EnabledNavMeshAgent);
      }
      else
        this.MissingActionPoint();
    }

    protected virtual void EnterAction(Action _endEvent)
    {
      this.EnterAction();
      this.ScheduleEndEvent = _endEvent;
    }

    protected virtual void OnAction()
    {
      if (this.schedule.managing)
      {
        if (this.schedule.enable)
          return;
        this.ClearSchedule();
        this.StateEndEvent();
      }
      else
      {
        if (this.AnimationKeepWaiting())
          return;
        this.StateEndEvent();
      }
    }

    protected virtual void ExitAction()
    {
      if (!this.HasActionPoint)
        return;
      if (this.actionPoint.MyUse((IAnimalActionPointUser) this))
      {
        float _time = 180f;
        if (Singleton<Resources>.IsInstance())
        {
          AnimalDefinePack.AllAnimalInfoGroup allAnimalInfo = Singleton<Resources>.Instance.AnimalDefinePack.AllAnimalInfo;
          if (allAnimalInfo != null)
            _time = allAnimalInfo.ActionPointUsedCoolTime;
        }
        this.actionPoint.SetUsedCoolTime((IAnimalActionPointUser) this, _time, false);
        this.actionPoint.StopUsing();
        this.actionPoint.SetStand((IAnimalActionPointUser) this, this.actionPoint.GetSlot(this.ActionType).Item2);
      }
      if (this.prevAgentEnabled != ((Behaviour) this.Agent).get_enabled())
        ((Behaviour) this.Agent).set_enabled(this.prevAgentEnabled);
      this.actionPoint = (AnimalActionPoint) null;
    }

    protected override void EnterSleep()
    {
      this.ModelInfo.EyesShapeInfo.SetBlendShape(100f);
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
      this.PlayInAnim(AnimationCategoryID.Sleep, (Action) null);
      this.SetSchedule(this.CurrentAnimState);
    }

    protected override void OnSleep()
    {
      this.OnAction();
    }

    protected override void ExitSleep()
    {
      this.ModelInfo.EyesShapeInfo.SetBlendShape(0.0f);
      this.ExitAction();
    }

    protected override void EnterAction0()
    {
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
      this.PlayInAnim(AnimationCategoryID.Action, (Action) null);
      this.SetSchedule(this.CurrentAnimState);
    }

    protected override void OnAction0()
    {
      this.OnAction();
    }

    protected override void ExitAction0()
    {
      this.ExitAction();
    }

    protected override void EnterAction1()
    {
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
    }

    protected override void OnAction1()
    {
      this.OnAction();
    }

    protected override void ExitAction1()
    {
      this.ExitAction();
    }

    protected override void EnterAction2()
    {
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
    }

    protected override void OnAction2()
    {
      this.OnAction();
    }

    protected override void ExitAction2()
    {
      this.ExitAction();
    }

    protected override void EnterAction3()
    {
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
    }

    protected override void OnAction3()
    {
      this.OnAction();
    }

    protected override void ExitAction3()
    {
      this.ExitAction();
    }

    protected override void EnterAction4()
    {
      this.EnterAction((Action) (() => this.SetState(AnimalState.Locomotion, (Action) null)));
    }

    protected override void OnAction4()
    {
      this.OnAction();
    }

    protected override void ExitAction4()
    {
      this.ExitAction();
    }
  }
}
