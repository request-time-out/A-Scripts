// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Tutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.Player
{
  public class Tutorial : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      MapUIContainer.TutorialUI.ClosedEvent += (Action) (() => EventPoint.ChangePrevPlayerMode());
      MapUIContainer.SetActiveTutorialUI(true);
      MapUIContainer.SetVisibleHUDExceptStoryUI(false);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      switch (player.PlayerController.State)
      {
        case Normal _:
        case Onbu _:
        case Houchi _:
          EventPoint.ChangeNormalState();
          MapUIContainer.SetVisibleHUD(true);
          break;
      }
    }

    ~Tutorial()
    {
    }
  }
}
