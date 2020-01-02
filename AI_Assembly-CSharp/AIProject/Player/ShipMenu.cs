// Decompiled with JetBrains decompiler
// Type: AIProject.Player.ShipMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.Player
{
  public class ShipMenu : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<MapScene>.Instance.SaveProfile(true);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      MapUIContainer.RefreshCommands(0, player.ShipCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (Action) (() =>
      {
        MapUIContainer.SetActiveCommandList(false);
        MapUIContainer.StorySupportUI.Open();
        player.PlayerController.ChangeState("Normal");
      });
      MapUIContainer.SetActiveCommandList(true, "移動先");
    }

    protected override void OnRelease(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
      Singleton<Input>.Instance.SetupState();
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

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ShipMenu.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new ShipMenu.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
