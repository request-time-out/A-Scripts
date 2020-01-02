// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DoorClose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class DoorClose : PlayerStateBase
  {
    protected int _currentState = -1;
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private DoorAnimation _doorAnimation;

    protected override void OnAwake(PlayerActor player)
    {
      DoorPoint currentPoint = player.CurrentPoint as DoorPoint;
      if (Object.op_Inequality((Object) currentPoint, (Object) null))
      {
        DoorPoint.OpenPattern openState = currentPoint.OpenState;
        currentPoint.SetOpenState(DoorPoint.OpenPattern.Close, true);
        this._doorAnimation = (DoorAnimation) ((Component) currentPoint).GetComponent<DoorAnimation>();
        if (Object.op_Inequality((Object) this._doorAnimation, (Object) null))
          this._doorAnimation.PlayCloseAnimation(openState);
        currentPoint.SetBookingUser((Actor) player);
      }
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (Action<M0>) (_ => this.Elapsed(player)));
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      animInfo.outEnableBlend = true;
      animInfo.outBlendSec = 0.0f;
      player.Animation.AnimInfo = animInfo;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (Object.op_Inequality((Object) this._doorAnimation, (Object) null) && this._doorAnimation.PlayingCloseAnim || this._onEndAction == null)
        return;
      this._onEndAction.OnNext(Unit.get_Default());
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    private void Elapsed(PlayerActor player)
    {
      player.PlayerController.CommandArea.RefreshCommands();
      player.CurrentPoint?.RemoveBookingUser((Actor) player);
      if (player.PlayerController.PrevStateName == "Onbu")
        player.Controller.ChangeState(player.PlayerController.PrevStateName);
      else
        player.Controller.ChangeState("Normal");
    }
  }
}
