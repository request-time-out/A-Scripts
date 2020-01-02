// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Idle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject.Player
{
  public class Idle : PlayerStateBase
  {
    private CommandLabel.AcceptionState _prevAcceptionState;
    private Input.ValidType _prevValidType;
    private bool _prevInteractionState;

    protected override void OnAwake(PlayerActor player)
    {
      this._prevAcceptionState = MapUIContainer.CommandLabel.Acception;
      this._prevValidType = Singleton<Input>.Instance.State;
      this._prevInteractionState = player.CurrentInteractionState;
      if (this._prevAcceptionState != CommandLabel.AcceptionState.None)
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      if (this._prevValidType != Input.ValidType.None)
      {
        Singleton<Input>.Instance.ReserveState(Input.ValidType.None);
        Singleton<Input>.Instance.SetupState();
      }
      if (!this._prevInteractionState)
        return;
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
    }

    protected override void OnRelease(PlayerActor player)
    {
      IState state = player.PlayerController.State;
      if (state is Normal || state is Onbu)
      {
        this._prevInteractionState = true;
        this._prevValidType = Input.ValidType.Action;
        this._prevAcceptionState = CommandLabel.AcceptionState.InvokeAcception;
      }
      if (this._prevInteractionState != player.CurrentInteractionState)
      {
        player.SetScheduledInteractionState(this._prevInteractionState);
        player.ReleaseInteraction();
      }
      if (this._prevValidType != Singleton<Input>.Instance.State)
      {
        Singleton<Input>.Instance.ReserveState(this._prevValidType);
        Singleton<Input>.Instance.SetupState();
      }
      if (this._prevAcceptionState == MapUIContainer.CommandLabel.Acception)
        return;
      MapUIContainer.SetCommandLabelAcception(this._prevAcceptionState);
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }
  }
}
