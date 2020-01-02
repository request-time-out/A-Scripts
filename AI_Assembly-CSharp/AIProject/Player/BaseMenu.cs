// Decompiled with JetBrains decompiler
// Type: AIProject.Player.BaseMenu
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
  public class BaseMenu : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      MapUIContainer.RefreshCommands(0, player.BaseCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (Action) (() =>
      {
        MapUIContainer.SetActiveCommandList(false);
        MapUIContainer.SetVisibleHUDExceptStoryUI(true);
        MapUIContainer.StorySupportUI.Open();
        player.Controller.ChangeState("Normal");
      });
      MapUIContainer.SetActiveCommandList(true, "拠点");
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
      BaseMenu.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new BaseMenu.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
