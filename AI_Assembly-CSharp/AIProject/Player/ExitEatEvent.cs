// Decompiled with JetBrains decompiler
// Type: AIProject.Player.ExitEatEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using UnityEngine;

namespace AIProject.Player
{
  public class ExitEatEvent : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = EventType.Eat;
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.RefreshCommands(0, player.ExitEatEventCommandInfo);
      MapUIContainer.SetActiveCommandList(true, "食事");
      MapUIContainer.CommandList.CancelEvent = (Action) null;
      player.OldEnabledHoldingHand = ((Behaviour) player.HandsHolder).get_enabled();
      if (!player.OldEnabledHoldingHand)
        return;
      ((Behaviour) player.HandsHolder).set_enabled(false);
      if (!player.HandsHolder.EnabledHolding)
        return;
      player.HandsHolder.EnabledHolding = false;
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override IEnumerator OnEnd(PlayerActor player)
    {
      return base.OnEnd(player);
    }
  }
}
