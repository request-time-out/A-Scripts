// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Warp
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
  public class Warp : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      MapUIContainer.RefreshCommands(0, player.WarpCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (Action) (() => player.CancelWarp());
      MapUIContainer.SetActiveCommandList(true, "ワープ");
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
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Warp.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new Warp.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
