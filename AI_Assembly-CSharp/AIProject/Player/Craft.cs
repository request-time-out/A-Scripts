// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Craft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.UI;
using AIProject.UI.Recycling;
using Manager;
using System;
using UnityEngine;

namespace AIProject.Player
{
  public class Craft : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      CraftPoint currentCraftPoint = player.CurrentCraftPoint;
      if (Object.op_Equality((Object) currentCraftPoint, (Object) null) || !Singleton<MapUIContainer>.IsInstance())
      {
        player.PlayerController.ChangeState("Normal");
      }
      else
      {
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
        player.SetScheduledInteractionState(false);
        player.ReleaseInteraction();
        CraftUI craftUi = (CraftUI) null;
        RecyclingUI recyclingUi = (RecyclingUI) null;
        switch (currentCraftPoint.Kind)
        {
          case CraftPoint.CraftKind.Medicine:
            craftUi = MapUIContainer.MedicineCraftUI;
            break;
          case CraftPoint.CraftKind.Pet:
            craftUi = MapUIContainer.PetCraftUI;
            break;
          case CraftPoint.CraftKind.Recycling:
            recyclingUi = MapUIContainer.RecyclingUI;
            break;
          default:
            this.OnClosed(player);
            break;
        }
        if (Object.op_Inequality((Object) craftUi, (Object) null))
        {
          craftUi.OnClosedEvent = (Action) (() => this.OnClosed(player));
          craftUi.IsActiveControl = true;
        }
        else if (Object.op_Inequality((Object) recyclingUi, (Object) null))
        {
          recyclingUi.OnClosedEvent = (Action) (() => this.OnClosed(player));
          recyclingUi.IsActiveControl = true;
        }
        else
          this.OnClosed(player);
      }
    }

    private void OnClosed(PlayerActor player)
    {
      if (Singleton<MapUIContainer>.IsInstance())
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
      if (Singleton<Input>.IsInstance())
      {
        Input instance = Singleton<Input>.Instance;
        instance.ReserveState(Input.ValidType.Action);
        instance.SetupState();
      }
      player.PlayerController.ChangeState("Normal");
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
