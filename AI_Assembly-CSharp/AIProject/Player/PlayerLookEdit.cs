﻿// Decompiled with JetBrains decompiler
// Type: AIProject.Player.PlayerLookEdit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class PlayerLookEdit : PlayerStateBase
  {
    private Subject<Unit> _onEndMenu = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      MapUIContainer.SetActivePlayerLookEditUI(true);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ => player.Controller.ChangeState("DeviceMenu")));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.PlayerController.CommandArea.RefreshCommands();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.PlayerLookEditUI.IsActiveControl)
        return;
      this._onEndMenu.OnNext(Unit.get_Default());
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
      PlayerLookEdit.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new PlayerLookEdit.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
