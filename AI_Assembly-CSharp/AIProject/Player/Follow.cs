// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Follow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.Player
{
  public class Follow : PlayerStateBase
  {
    private CommandLabel.AcceptionState _prevAcceptionState;
    private bool _moved;
    private int _prevAvoidancePriority;

    protected override void OnAwake(PlayerActor player)
    {
      this._prevAcceptionState = MapUIContainer.CommandLabel.Acception;
      if (this._prevAcceptionState != CommandLabel.AcceptionState.None)
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      player.Animation.RefsActAnimInfo = true;
      player.ActivateTransfer();
      player.SetActiveOnEquipedItem(true);
      player.ResetCoolTime();
      if (player.CameraControl.Mode == CameraMode.ActionNotMove || player.CameraControl.Mode == CameraMode.ActionFreeLook)
      {
        player.CameraControl.Mode = CameraMode.Normal;
        player.CameraControl.RecoverShotType();
      }
      this._prevAvoidancePriority = player.NavMeshAgent.get_avoidancePriority();
      player.NavMeshAgent.set_avoidancePriority(99);
      Vector3 destination = this.DesiredPosition(player.Partner);
      this.SetDestination(player, destination);
      player.Partner.NavMeshAgent.set_obstacleAvoidanceType((ObstacleAvoidanceType) 0);
    }

    public override void Release(Actor actor, EventType type)
    {
      this.OnRelease(actor as PlayerActor);
    }

    protected override void OnRelease(PlayerActor player)
    {
      switch (player.PlayerController.State)
      {
        case Normal _:
        case Onbu _:
          this._prevAcceptionState = CommandLabel.AcceptionState.InvokeAcception;
          break;
      }
      player.NavMeshAgent.set_avoidancePriority(this._prevAvoidancePriority);
      if (Object.op_Inequality((Object) player.Partner, (Object) null))
        player.Partner.NavMeshAgent.set_obstacleAvoidanceType((ObstacleAvoidanceType) 4);
      if (this._prevAcceptionState == MapUIContainer.CommandLabel.Acception)
        return;
      MapUIContainer.SetCommandLabelAcception(this._prevAcceptionState);
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      AgentActor agentPartner = player.AgentPartner;
      if (Object.op_Equality((Object) agentPartner, (Object) null))
        return;
      AgentProfile agentProfile = Singleton<Resources>.Instance.AgentProfile;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      NavMeshAgent navMeshAgent1 = player.NavMeshAgent;
      if (navMeshAgent1.get_isOnOffMeshLink())
      {
        this.Stop(player);
        OffMeshLinkData currentOffMeshLinkData1 = navMeshAgent1.get_currentOffMeshLinkData();
        if (!Object.op_Inequality((Object) ((OffMeshLinkData) ref currentOffMeshLinkData1).get_offMeshLink(), (Object) null))
          return;
        NavMeshAgent navMeshAgent2 = player.NavMeshAgent;
        M0 m0;
        if (navMeshAgent2 == null)
        {
          m0 = (M0) null;
        }
        else
        {
          OffMeshLinkData currentOffMeshLinkData2 = navMeshAgent2.get_currentOffMeshLinkData();
          m0 = ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData2).get_offMeshLink())?.GetComponent<ActionPoint>();
        }
        ActionPoint point = (ActionPoint) m0;
        if (!Object.op_Inequality((Object) point, (Object) null) || !point.OffMeshAvailablePoint((Actor) player))
          return;
        if (point is DoorPoint)
        {
          player.CurrentPoint = point;
          player.PlayerController.ChangeState("DoorOpen", point, (Action) null);
        }
        else
        {
          player.CurrentPoint = point;
          player.PlayerController.ChangeState("Move", point, (Action) null);
        }
      }
      else
      {
        Vector3 destination = this.DesiredPosition((Actor) agentPartner);
        if ((double) Vector3.Distance(destination, player.Position) >= (double) agentProfile.RestDistance)
        {
          this.SetDestination(player, destination);
          this._moved = true;
        }
        else
        {
          NavMeshPathStatus pathStatus = navMeshAgent1.get_pathStatus();
          if (pathStatus == 1 || pathStatus == 2)
          {
            if ((double) Vector3.Distance(player.Position, agentPartner.Position) >= (double) agentProfile.RestDistance)
              return;
            this.Stop(player);
            if (!player.IsRunning)
              return;
            player.IsRunning = false;
          }
          else
          {
            if (navMeshAgent1.get_pathPending())
              return;
            if ((double) navMeshAgent1.get_remainingDistance() < (double) agentProfile.RestDistance && player.IsRunning)
              player.IsRunning = false;
            if (!this._moved || (double) navMeshAgent1.get_remainingDistance() >= (double) navMeshAgent1.get_stoppingDistance())
              return;
            this.Stop(player);
            this._moved = false;
          }
        }
      }
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    private Vector3 DesiredPosition(Actor partner)
    {
      return Vector3.op_Addition(partner.Position, Quaternion.op_Multiply(partner.Rotation, Vector3.get_back()));
    }

    private bool SetDestination(PlayerActor player, Vector3 destination)
    {
      if (player.NavMeshAgent.get_isStopped())
        player.NavMeshAgent.set_isStopped(false);
      return player.NavMeshAgent.SetDestination(destination);
    }

    private void Stop(PlayerActor player)
    {
      if (!player.NavMeshAgent.get_hasPath())
        return;
      player.NavMeshAgent.set_isStopped(true);
    }
  }
}
