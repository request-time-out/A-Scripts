// Decompiled with JetBrains decompiler
// Type: AIProject.ActorLocomotionMerchant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using Manager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  public class ActorLocomotionMerchant : ActorLocomotion
  {
    private static Vector3 ZXOne = new Vector3(1f, 0.0f, 1f);
    public float AccelerationTime = 0.2f;
    private ActorLocomotion.AnimationState AnimState = new ActorLocomotion.AnimationState();
    private Vector3 _moveDirection = Vector3.get_zero();
    private Vector3 _moveDirectionVelocity = Vector3.get_zero();
    public ActorAnimationMerchant CharacterAnimation;
    [HideInEditorMode]
    [DisableInPlayMode]
    public int MovePoseID;

    private void OnDisable()
    {
      this.AnimState.Init();
      this._moveDirection = this._moveDirectionVelocity = Vector3.get_zero();
    }

    public override void Move(Vector3 deltaPosition)
    {
    }

    public void UpdateState()
    {
      this.CalcAnimSpeed();
      MerchantActor actor = this._actor as MerchantActor;
      NavMeshAgent navMeshAgent = actor.NavMeshAgent;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      if (actor.CurrentMode == Merchant.ActionType.GotoLesbianSpotFollow)
      {
        if (((Behaviour) navMeshAgent).get_isActiveAndEnabled() && !navMeshAgent.get_pathPending())
        {
          if (!actor.IsRunning && (double) agentProfile.RunDistance < (double) navMeshAgent.get_remainingDistance())
            actor.IsRunning = true;
          float num;
          if (actor.IsRunning)
          {
            this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.MerchantSpeed.runSpeed);
            num = locomotionProfile.MerchantSpeed.runSpeed;
          }
          else
          {
            this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.MerchantSpeed.walkSpeed);
            num = locomotionProfile.MerchantSpeed.walkSpeed;
          }
          navMeshAgent.set_speed(Mathf.Lerp(navMeshAgent.get_speed(), num, locomotionProfile.LerpSpeed));
        }
        else
          this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.MerchantSpeed.walkSpeed);
      }
      else if (this.MovePoseID == 0)
      {
        this.AnimState.moveDirection = Vector3.op_Multiply(this.MoveDirection, locomotionProfile.MerchantSpeed.walkSpeed);
        this.AnimState.setMediumOnWalk = true;
        this.AnimState.medVelocity = locomotionProfile.MerchantSpeed.walkSpeed;
        this.AnimState.maxVelocity = locomotionProfile.MerchantSpeed.runSpeed;
      }
      else
      {
        this.AnimState.moveDirection = Vector3.get_zero();
        this.AnimState.setMediumOnWalk = false;
        this.AnimState.maxVelocity = locomotionProfile.MerchantSpeed.walkSpeed;
      }
      this.CharacterAnimation.UpdateState(this.AnimState);
    }

    private Vector3 MoveDirection
    {
      get
      {
        Vector3 moveDirection = this._moveDirection;
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector(0.0f, 0.0f, ((Vector3) ref this._actor.StateInfo.move).get_magnitude());
        Vector3 normalized = ((Vector3) ref vector3).get_normalized();
        ref Vector3 local = ref this._moveDirectionVelocity;
        double accelerationTime = (double) this.AccelerationTime;
        return this._moveDirection = Vector3.SmoothDamp(moveDirection, normalized, ref local, (float) accelerationTime);
      }
    }

    private void CalcAnimSpeed()
    {
      Actor.InputInfo inputInfo = new Actor.InputInfo();
      ref Actor.InputInfo local1 = ref inputInfo;
      Vector3 vector3 = Vector3.Scale(this._actor.NavMeshAgent.get_velocity(), ActorLocomotionMerchant.ZXOne);
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      local1.move = normalized;
      Vector3 velocity = this._actor.NavMeshAgent.get_velocity();
      float num = Mathf.InverseLerp(0.0f, (this._actor as MerchantActor).DestinationSpeed, ((Vector3) ref velocity).get_magnitude());
      ref Actor.InputInfo local2 = ref inputInfo;
      local2.move = Vector3.op_Multiply(local2.move, num);
      this._actor.StateInfo = inputInfo;
    }
  }
}
