// Decompiled with JetBrains decompiler
// Type: AIProject.ActorLocomotionThirdPerson
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.Player;
using Manager;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class ActorLocomotionThirdPerson : ActorLocomotion
  {
    [Range(1f, 4f)]
    private float _gravityMultipiler = 2f;
    public float _accelerationTime = 0.2f;
    [Tooltip("空中での移動補正値")]
    public float airSpeed = 6f;
    [Tooltip("空中制御の補正値")]
    public float airControl = 2f;
    [SerializeField]
    private ActorLocomotionThirdPerson.Settings _settings = new ActorLocomotionThirdPerson.Settings(1f, 5f, 2f, 0.25f, 5f, 7f, 3f, 0.6f, 1f);
    public ActorLocomotion.AnimationState AnimState = new ActorLocomotion.AnimationState();
    public ActorAnimationThirdPerson CharacterAnimation;
    public PlayerController userControl;
    public bool lookInCameraDirection;
    protected Vector3 _moveDirection;
    private Vector3 lookPosSmooth;
    private Vector3 platformVelocity;
    private float jumpEndTime;
    private float groundDistance;
    private float stickyForce;
    private Vector3 moveDirectionVelocity;
    private Vector3 fixedDeltaPosition;
    private Quaternion fixedDeltaRotation;
    private bool fixedFrame;
    private Vector3 gravity;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _runStepLength;
    [SerializeField]
    private float _stepInterval;
    private float _stepCycle;
    private float _nextStep;
    private float _lastPosition;
    private Rigidbody _rigidbody;
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    public bool onGround { get; private set; }

    private void Awake()
    {
      this._rigidbody = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
      base.Start();
      this.lookPosSmooth = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), 10f));
      this._rigidbody.set_isKinematic(false);
      this._rigidbody.set_velocity(Vector3.get_zero());
    }

    private void OnDisable()
    {
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_velocity(Vector3.get_zero());
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_isKinematic(true);
      this.AnimState.Init();
      this._moveDirection = Vector3.get_zero();
      this.moveDirectionVelocity = Vector3.get_zero();
    }

    protected override void Start()
    {
      base.Start();
      this.lookPosSmooth = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), 10f));
    }

    public override void Move(Vector3 deltaPosition)
    {
      if (this._actor.IsKinematic)
        return;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) this._actor.StateInfo.move.x, 0.0f, (float) this._actor.StateInfo.move.z);
      switch (this._actor.Mode)
      {
        case Desire.ActionType.Normal:
        case Desire.ActionType.Date:
          vector3 = Singleton<Manager.Input>.Instance.IsDown((KeyCode) 304) || Singleton<Manager.Input>.Instance.IsDown((KeyCode) 303) ? Vector3.op_Multiply(vector3, locomotionProfile.PlayerSpeed.walkSpeed) : Vector3.op_Multiply(vector3, locomotionProfile.PlayerSpeed.normalSpeed);
          break;
        default:
          vector3 = Vector3.op_Multiply(vector3, locomotionProfile.PlayerSpeed.walkSpeed);
          break;
      }
      vector3 = Vector3.op_Addition(vector3, new Vector3((float) this.platformVelocity.x, 0.0f, (float) this.platformVelocity.z));
      if (!((this._actor as PlayerActor).PlayerController.State is Follow) && ((Behaviour) this._navMeshAgent).get_enabled())
        this._navMeshAgent.Move(Vector3.op_Multiply(vector3, Time.get_deltaTime()));
      this._actor.ForwardMLP = Mathf.Lerp(this._actor.ForwardMLP, this.onGround ? this.GetSlopeDamper(Vector3.op_Division(Vector3.op_UnaryNegation(deltaPosition), Time.get_deltaTime()), this._actor.Normal) : 1f, Time.get_deltaTime() * 5f);
      if (!float.IsNaN(this._actor.ForwardMLP))
        return;
      this._actor.ForwardMLP = 0.0f;
    }

    public void UpdateState(Actor.InputInfo state, ActorLocomotion.UpdateType updateType)
    {
      this._actor.StateInfo = state;
      this.Look(updateType);
      this.GroundCheck();
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      PlayerActor actor = this._actor as PlayerActor;
      NavMeshAgent navMeshAgent = this._actor.NavMeshAgent;
      if (actor.PlayerController.State is Follow)
      {
        Actor.InputInfo stateInfo = this._actor.StateInfo;
        ref Actor.InputInfo local = ref stateInfo;
        Vector3 vector3 = Vector3.Scale(this._actor.NavMeshAgent.get_velocity(), new Vector3(1f, 0.0f, 1f));
        Vector3 normalized = ((Vector3) ref vector3).get_normalized();
        local.move = normalized;
        this._actor.StateInfo = stateInfo;
        if ((double) navMeshAgent.get_remainingDistance() > (double) agentProfile.RunDistance && !actor.IsRunning)
          actor.IsRunning = true;
        this.AnimState.setMediumOnWalk = true;
        this.AnimState.medVelocity = locomotionProfile.AgentSpeed.walkSpeed;
        this.AnimState.maxVelocity = locomotionProfile.AgentSpeed.runSpeed;
        float num;
        if (actor.IsRunning)
        {
          this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.runSpeed);
          num = locomotionProfile.AgentSpeed.followRunSpeed;
        }
        else
        {
          this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.AgentSpeed.walkSpeed);
          num = locomotionProfile.AgentSpeed.walkSpeed;
        }
        navMeshAgent.set_speed(Mathf.Lerp(navMeshAgent.get_speed(), num, locomotionProfile.LerpSpeed));
      }
      else
      {
        switch (this._actor.Mode)
        {
          case Desire.ActionType.Normal:
          case Desire.ActionType.Date:
            this.AnimState.moveDirection = Singleton<Manager.Input>.Instance.IsDown((KeyCode) 304) || Singleton<Manager.Input>.Instance.IsDown((KeyCode) 303) ? Vector3.op_Multiply(this.MoveDirection, locomotionProfile.PlayerSpeed.walkSpeed) : Vector3.op_Multiply(this.MoveDirection, locomotionProfile.PlayerSpeed.normalSpeed);
            this.AnimState.setMediumOnWalk = true;
            this.AnimState.medVelocity = locomotionProfile.PlayerSpeed.walkSpeed;
            this.AnimState.maxVelocity = locomotionProfile.PlayerSpeed.normalSpeed;
            break;
          case Desire.ActionType.Onbu:
            this.AnimState.setMediumOnWalk = false;
            this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.PlayerSpeed.walkSpeed);
            this.AnimState.maxVelocity = locomotionProfile.PlayerSpeed.walkSpeed;
            break;
        }
      }
      this.AnimState.onGround = this.onGround;
      this.CharacterAnimation.UpdateState(this.AnimState);
      this.ProgressStepCycle(((Vector3) ref this._actor.StateInfo.move).get_magnitude());
    }

    private void ProgressStepCycle(float speed)
    {
      if (this.AnimState.onGround && (double) ((Vector3) ref this.AnimState.moveDirection).get_sqrMagnitude() > 0.0 && (double) ((Vector3) ref this._actor.StateInfo.move).get_sqrMagnitude() != 0.0)
        this._stepCycle += (((Vector3) ref this.AnimState.moveDirection).get_magnitude() + speed * this._runStepLength) * Time.get_fixedDeltaTime();
      if ((double) this._stepCycle <= (double) this._nextStep)
        return;
      this._nextStep = this._stepCycle + this._stepInterval;
    }

    private Vector3 MoveDirection
    {
      get
      {
        this._moveDirection = Vector3.SmoothDamp(this._moveDirection, new Vector3(0.0f, 0.0f, ((Vector3) ref this._actor.StateInfo.move).get_magnitude()), ref this.moveDirectionVelocity, this._accelerationTime);
        return this._moveDirection;
      }
    }

    private void Look(ActorLocomotion.UpdateType updateType)
    {
      float num = updateType != ActorLocomotion.UpdateType.Update || updateType != ActorLocomotion.UpdateType.LateUpdate ? Time.get_fixedDeltaTime() : Time.get_deltaTime();
      this.lookPosSmooth = Vector3.Lerp(this.lookPosSmooth, this._actor.StateInfo.lookPos, num * this._settings.lookResponseSpeed);
      float angleFromForward = this.GetAngleFromForward(this.GetLookDirection());
      if (Vector3.op_Equality(this._actor.StateInfo.move, Vector3.get_zero()))
        angleFromForward *= (float) (1.00999999046326 - (double) Mathf.Abs(angleFromForward) / 180.0) * this._settings.stationaryTurnSpeedMlp;
      this.RigidbodyRotateAround(this.CharacterAnimation.GetPivotPoint(), ((Component) this).get_transform().get_up(), angleFromForward * num * this._settings.movingTurnSpeed);
    }

    private Vector3 GetLookDirection()
    {
      if (Vector3.op_Inequality(this._actor.StateInfo.move, Vector3.get_zero()))
        return this._actor.StateInfo.move;
      return this.lookInCameraDirection ? Vector3.op_Subtraction(this._actor.StateInfo.lookPos, this._actor.Position) : ((Component) this).get_transform().get_forward();
    }

    private RaycastHit GetHit()
    {
      return this.GetSpherecastHit();
    }

    private void GroundCheck()
    {
      Vector3 zero = Vector3.get_zero();
      float num = 0.0f;
      RaycastHit hit = this.GetHit();
      this._actor.Normal = ((RaycastHit) ref hit).get_normal();
      bool onGround = this.onGround;
      this.onGround = false;
      this.platformVelocity = Vector3.Lerp(this.platformVelocity, zero, Time.get_deltaTime() * this._settings.platformFriction);
      this.stickyForce = num;
    }

    [Serializable]
    public struct Settings
    {
      public float stationaryTurnSpeedMlp;
      public float movingTurnSpeed;
      public float lookResponseSpeed;
      [Tooltip("着地してから再びジャンプするまでに経過しないといけない時間")]
      public float jumpRepeatDelayTime;
      [Tooltip("強制的に地面にくっつける力")]
      public float groundStickyEffect;
      [Tooltip("キャラが立つ場所にかかってる力の補間(摩擦力？)\n0にすると壁走りが出来なくなるっぽい？")]
      public float platformFriction;
      [Tooltip("地面上に居る時にY軸にかかる力の最大値")]
      public float maxVerticalVelocityOnGround;
      [Tooltip("しゃがみ中の当たり判定サイズ補正値")]
      public float crouchCapsuleScaleMlp;
      [Tooltip("移動にかかるに地面の向きを反映させる割り合い")]
      public float velocityToGroundTangentWeight;

      public Settings(
        float turnSpeedMlp,
        float turnSpeed,
        float resSpeed,
        float jumpRepeatDelay,
        float groundSticky,
        float friction,
        float maxVerticalVel,
        float capsuleScaleMlp,
        float weight)
      {
        this.stationaryTurnSpeedMlp = turnSpeedMlp;
        this.movingTurnSpeed = turnSpeed;
        this.lookResponseSpeed = resSpeed;
        this.jumpRepeatDelayTime = jumpRepeatDelay;
        this.groundStickyEffect = groundSticky;
        this.platformFriction = friction;
        this.maxVerticalVelocityOnGround = maxVerticalVel;
        this.crouchCapsuleScaleMlp = capsuleScaleMlp;
        this.velocityToGroundTangentWeight = weight;
      }
    }
  }
}
