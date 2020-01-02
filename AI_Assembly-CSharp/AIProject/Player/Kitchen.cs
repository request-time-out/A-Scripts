// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Kitchen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.Player
{
  public class Kitchen : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (MapUIContainer.OpenOnceTutorial(2, false))
        MapUIContainer.TutorialUI.ClosedEvent = (Action) (() => this.OnStart(player));
      else
        this.OnStart(player);
    }

    private void OnStart(PlayerActor player)
    {
      MapUIContainer.RefreshCommands(0, player.CookCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (Action) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        MapUIContainer.SetActiveCommandList(false);
        MapUIContainer.SetVisibleHUDExceptStoryUI(true);
        MapUIContainer.StorySupportUI.Open();
        player.Controller.ChangeState("Normal");
        player.ReleaseCurrentPoint();
        if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
          ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        player.ActivateNavMeshAgent();
        player.IsKinematic = false;
      });
      MapUIContainer.SetActiveCommandList(true, "料理");
    }

    public override void Release(Actor actor, EventType type)
    {
      this.OnRelease(actor as PlayerActor);
    }

    protected override void OnRelease(PlayerActor player)
    {
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    [DebuggerHidden]
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Kitchen.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new Kitchen.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
