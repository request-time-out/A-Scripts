// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DeviceMenu
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
  public class DeviceMenu : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      Singleton<MapScene>.Instance.SaveProfile(true);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
      MapUIContainer.StorySupportUI.Close();
      if (MapUIContainer.OpenOnceTutorial(6, false))
        MapUIContainer.TutorialUI.ClosedEvent = (Action) (() => this.OnStart(player));
      else
        this.OnStart(player);
    }

    private void OnStart(PlayerActor player)
    {
      Singleton<Manager.Map>.Instance.AccessDeviceID = player.CurrentDevicePoint.ID;
      MapUIContainer.RefreshCommands(0, player.DeviceCommandInfos);
      MapUIContainer.CommandList.CancelEvent = (Action) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        Singleton<Manager.Map>.Instance.AccessDeviceID = -1;
        MapUIContainer.SetActiveCommandList(false);
        MapUIContainer.SetVisibleHUDExceptStoryUI(true);
        MapUIContainer.StorySupportUI.Open();
        player.Controller.ChangeState("Normal");
        player.CurrentDevicePoint = (DevicePoint) null;
      });
      MapUIContainer.SetActiveCommandList(true, "データ端末");
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
      DeviceMenu.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new DeviceMenu.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
