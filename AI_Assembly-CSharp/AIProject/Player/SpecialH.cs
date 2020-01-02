// Decompiled with JetBrains decompiler
// Type: AIProject.Player.SpecialH
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.Player
{
  public class SpecialH : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      DateActionPointInfo outInfo;
      player.CurrentPoint.TryGetPlayerDateActionPointInfo(player.ChaControl.sex, EventType.Lesbian, out outInfo);
      player.HPoseID = outInfo.poseIDA;
      MapUIContainer.RefreshCommands(0, player.SpecialHCommandInfo);
      MapUIContainer.CommandList.CancelEvent = (Action) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        MapUIContainer.SetActiveCommandList(false);
        player.Controller.ChangeState("Normal");
        player.ReleaseCurrentPoint();
        if (Object.op_Inequality((Object) player.PlayerController.CommandArea, (Object) null))
          ((Behaviour) player.PlayerController.CommandArea).set_enabled(true);
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        player.ActivateNavMeshAgent();
        player.IsKinematic = false;
      });
      MapUIContainer.SetActiveCommandList(true, "特殊エッチ");
    }

    protected override void OnRelease(PlayerActor player)
    {
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
    protected override IEnumerator OnEnd(PlayerActor player)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SpecialH.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new SpecialH.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
