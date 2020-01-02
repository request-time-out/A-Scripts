// Decompiled with JetBrains decompiler
// Type: AIProject.Player.DressRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class DressRoom : PlayerStateBase
  {
    private Subject<Unit> _onEndMenu = new Subject<Unit>();

    protected override void OnAwake(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      MapUIContainer.DressRoomUI.CoordinateFilterSource = Singleton<Game>.Instance.WorldData.Environment.DressCoordinateList;
      MapUIContainer.SetActiveDressRoomUI(true);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ =>
      {
        player.Controller.ChangeState("Normal");
        MapUIContainer.DressRoomUI.CoordinateFilterSource = (List<string>) null;
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
      Singleton<Input>.Instance.SetupState();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.DressRoomUI.IsActiveControl)
        return;
      this._onEndMenu.OnNext(Unit.get_Default());
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
      DressRoom.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new DressRoom.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
