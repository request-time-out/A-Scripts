// Decompiled with JetBrains decompiler
// Type: AIProject.ActorLocomotionAgent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using Manager;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class ActorLocomotionAgent : ActorLocomotion
  {
    private static readonly Vector3 _zxOne = new Vector3(1f, 0.0f, 1f);
    [SerializeField]
    private float _stationaryTurnSpeedMlp = 1f;
    [SerializeField]
    private float _movingTurnSpeed = 5f;
    [SerializeField]
    private float _platformFriction = 7f;
    [SerializeField]
    [Range(1f, 4f)]
    private float _gravityMultiplier = 2f;
    public float AccelerationTime = 0.2f;
    private Vector3 _moveDirection = Vector3.get_zero();
    private Vector3 _moveDirectionVelocity = Vector3.get_zero();
    private Vector3 _platformVelocity = Vector3.get_zero();
    public ActorAnimationAgent CharacterAnimation;
    private float _groundDistance;
    private float _lastHeight;

    public ActorLocomotion.AnimationState AnimState { get; set; }

    private void OnEnable()
    {
      this.Start();
    }

    private void OnDisable()
    {
      this.AnimState.Init();
      this._moveDirection = Vector3.get_zero();
      this._moveDirectionVelocity = Vector3.get_zero();
    }

    public override void Move(Vector3 deltaPosition)
    {
      if (this._actor.IsKinematic || Mathf.Approximately(Time.get_timeScale(), 0.0f) || Mathf.Approximately(Time.get_deltaTime(), 0.0f))
        return;
      this._actor.ForwardMLP = Mathf.Lerp(this._actor.ForwardMLP, this._actor.IsOnGround ? this.GetSlopeDamper(Vector3.op_Division(Vector3.op_UnaryNegation(deltaPosition), Time.get_deltaTime()), this._actor.Normal) : 1f, Time.get_deltaTime() * 5f);
    }

    public void UpdateState()
    {
      this.CalcAnimSpeed();
      this.Look();
      this.GroundCheck();
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      AgentActor actor = this._actor as AgentActor;
      StuffItem carryingItem = actor.AgentData.CarryingItem;
      int caseID;
      if (carryingItem != null && !agentProfile.CanStandEatItems.Exists<ItemIDKeyPair>((Predicate<ItemIDKeyPair>) (pair => pair.categoryID == carryingItem.CategoryID && pair.itemID == carryingItem.ID)))
      {
        caseID = 0;
      }
      else
      {
        int id = actor.AgentData.SickState.ID;
        Weather weather = Singleton<Manager.Map>.Instance.Simulator.Weather;
        switch (id)
        {
          case 3:
            caseID = 1;
            break;
          case 4:
            caseID = 0;
            break;
          default:
            StuffItem equipedUmbrellaItem = actor.AgentData.EquipedUmbrellaItem;
            CommonDefine.ItemIDDefines itemIdDefine = Singleton<Resources>.Instance.CommonDefine.ItemIDDefine;
            if (equipedUmbrellaItem != null && equipedUmbrellaItem.CategoryID == itemIdDefine.UmbrellaID.categoryID && equipedUmbrellaItem.ID == itemIdDefine.UmbrellaID.itemID)
            {
              if (weather == Weather.Rain || weather == Weather.Storm)
              {
                caseID = 0;
                break;
              }
              this.SetLocomotionInfo(actor, weather, out caseID);
              break;
            }
            this.SetLocomotionInfo(actor, weather, out caseID);
            break;
        }
      }
      ActorLocomotion.AnimationState animState = this.AnimState;
      NavMeshAgent navMeshAgent = this._actor.NavMeshAgent;
      if (actor.Mode == Desire.ActionType.Date)
      {
        if (((Behaviour) navMeshAgent).get_enabled() && !navMeshAgent.get_pathPending())
        {
          if ((double) navMeshAgent.get_remainingDistance() > (double) agentProfile.RunDistance && !actor.IsRunning)
            actor.IsRunning = true;
          float num;
          if (actor.IsRunning)
          {
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.followRunSpeed);
            num = locomotionProfile.AgentSpeed.followRunSpeed;
          }
          else
          {
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
            num = locomotionProfile.AgentSpeed.walkSpeed;
          }
          animState.setMediumOnWalk = true;
          animState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
          animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
          navMeshAgent.set_speed(Mathf.Lerp(navMeshAgent.get_speed(), num, locomotionProfile.LerpSpeed));
        }
        else
          animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
      }
      else if (actor.Mode == Desire.ActionType.TakeHPoint || actor.Mode == Desire.ActionType.ChaseYobai || actor.Mode == Desire.ActionType.ComeSleepTogether)
      {
        if (((Behaviour) navMeshAgent).get_enabled() && !navMeshAgent.get_pathPending())
          navMeshAgent.set_speed(locomotionProfile.AgentSpeed.runSpeed);
        animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed);
        animState.setMediumOnWalk = true;
        animState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
        animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
      }
      else if (actor.Mode == Desire.ActionType.WalkWithAgentFollow || actor.BehaviorResources.Mode == Desire.ActionType.WalkWithAgentFollow)
      {
        if (((Behaviour) navMeshAgent).get_enabled() && !navMeshAgent.get_pathPending())
        {
          float num;
          if ((double) navMeshAgent.get_remainingDistance() > (double) agentProfile.RunDistance)
          {
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed);
            num = locomotionProfile.AgentSpeed.runSpeed;
          }
          else
          {
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
            num = locomotionProfile.AgentSpeed.walkSpeed;
          }
          animState.setMediumOnWalk = true;
          animState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
          animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
          navMeshAgent.set_speed(Mathf.Lerp(navMeshAgent.get_speed(), num, locomotionProfile.LerpSpeed));
        }
        else
          animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.followRunSpeed);
      }
      else if (this._actor.Mode == Desire.ActionType.Escape)
      {
        if (((Behaviour) navMeshAgent).get_enabled() && !navMeshAgent.get_pathPending())
        {
          float escapeSpeed = locomotionProfile.AgentSpeed.escapeSpeed;
          animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, escapeSpeed);
          navMeshAgent.set_speed(Mathf.Lerp(navMeshAgent.get_speed(), escapeSpeed, locomotionProfile.LerpSpeed));
        }
      }
      else
      {
        if (actor.TutorialMode)
        {
          switch (Manager.Map.GetTutorialProgress())
          {
            case 14:
            case 15:
              caseID = actor.TutorialLocomoCaseID;
              break;
          }
        }
        switch (caseID)
        {
          case 0:
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
            animState.setMediumOnWalk = false;
            animState.maxVelocity = locomotionProfile.AgentSpeed.walkSpeed;
            break;
          case 1:
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
            animState.setMediumOnWalk = true;
            animState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
            animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
            break;
          case 2:
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed);
            animState.setMediumOnWalk = false;
            animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
            break;
          case 100:
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.tutorialWalkSpeed);
            animState.setMediumOnWalk = true;
            animState.medVelocity = locomotionProfile.AgentSpeed.tutorialWalkSpeed;
            animState.maxVelocity = locomotionProfile.AgentSpeed.tutorialRunSpeed;
            break;
          case 101:
            animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.tutorialRunSpeed);
            animState.setMediumOnWalk = false;
            animState.maxVelocity = locomotionProfile.AgentSpeed.tutorialRunSpeed;
            break;
          default:
            float num = actor.AgentData.StatsTable[5] * agentProfile.MustRunMotivationPercent;
            int desireKey = Desire.GetDesireKey(actor.RequestedDesire);
            float? motivation = actor.GetMotivation(desireKey);
            if (motivation.HasValue && (double) motivation.Value < (double) num)
              animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed);
            else if (Object.op_Inequality((Object) actor.MapArea, (Object) null))
            {
              int areaId = actor.MapArea.AreaID;
              animState.moveDirection = !Object.op_Inequality((Object) actor.TargetInSightActionPoint, (Object) null) ? (!Object.op_Inequality((Object) actor.DestWaypoint, (Object) null) ? Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed) : (actor.DestWaypoint.OwnerArea.AreaID != areaId ? Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed) : Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed))) : (actor.TargetInSightActionPoint.OwnerArea.AreaID != areaId ? Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed) : Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed));
            }
            else
              animState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
            animState.setMediumOnWalk = true;
            animState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
            animState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
            animState.onGround = this._actor.IsOnGround;
            break;
        }
        actor.UpdateLocomotionSpeed(actor.DestWaypoint);
      }
      this.AnimState = animState;
      this.CharacterAnimation.UpdateState(animState);
    }

    private void SetLocomotionInfo(AgentActor agent, Weather weather, out int caseID)
    {
      switch (agent.Mode)
      {
        case Desire.ActionType.EndTaskGift:
        case Desire.ActionType.EndTaskH:
          caseID = 0;
          break;
        default:
          if (weather == Weather.Rain || weather == Weather.Storm)
          {
            caseID = 2;
            break;
          }
          caseID = -1;
          break;
      }
    }

    public Vector3 MoveDirection
    {
      get
      {
        Vector3 moveDirection = this._moveDirection;
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, ((Vector3) ref this._actor.StateInfo.move).get_magnitude());
        Vector3 normalized = ((Vector3) ref vector3).get_normalized();
        ref Vector3 local = ref this._moveDirectionVelocity;
        double accelerationTime = (double) this.AccelerationTime;
        this._moveDirection = Vector3.SmoothDamp(moveDirection, normalized, ref local, (float) accelerationTime);
        return this._moveDirection;
      }
    }

    private void CalcAnimSpeed()
    {
      Actor.InputInfo inputInfo = new Actor.InputInfo();
      ref Actor.InputInfo local = ref inputInfo;
      Vector3 vector3 = Vector3.Scale(this._actor.NavMeshAgent.get_velocity(), ActorLocomotionAgent._zxOne);
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      local.move = normalized;
      this._actor.StateInfo = inputInfo;
    }

    private void Look()
    {
      float angleFromForward = this.GetAngleFromForward(this.LookDirection);
      if (Vector3.op_Equality(this._actor.StateInfo.move, Vector3.get_zero()))
        angleFromForward *= (float) (1.0 - (double) Mathf.Abs(angleFromForward) / 180.0) * this._stationaryTurnSpeedMlp;
      this.RigidbodyRotateAround(this.CharacterAnimation.GetPivotPoint(), ((Component) this).get_transform().get_up(), angleFromForward * Time.get_deltaTime() * this._movingTurnSpeed);
    }

    private Vector3 LookDirection
    {
      get
      {
        return Vector3.op_Inequality(this._actor.StateInfo.move, Vector3.get_zero()) ? this._actor.StateInfo.move : ((Component) this).get_transform().get_forward();
      }
    }

    private RaycastHit GetHit()
    {
      return this.GetSpherecastHit();
    }

    private void GroundCheck()
    {
      Vector3 vector3 = Vector3.get_zero();
      RaycastHit hit = this.GetHit();
      this._actor.Normal = ((RaycastHit) ref hit).get_normal();
      bool isOnGround = this._actor.IsOnGround;
      this._actor.IsOnGround = false;
      if ((double) this._groundDistance < (isOnGround ? (double) this._airbornThreshold : (double) this._airbornThreshold * 0.5) && Object.op_Inequality((Object) ((RaycastHit) ref hit).get_collider(), (Object) null))
      {
        if (Object.op_Inequality((Object) ((RaycastHit) ref hit).get_rigidbody(), (Object) null))
          vector3 = ((RaycastHit) ref hit).get_rigidbody().GetPointVelocity(((RaycastHit) ref hit).get_point());
        this._actor.IsOnGround = true;
      }
      this._platformVelocity = Vector3.Lerp(this._platformVelocity, vector3, Time.get_deltaTime() * this._platformFriction);
    }
  }
}
