// Decompiled with JetBrains decompiler
// Type: AIProject.Player.ItemBox
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
  public class ItemBox : PlayerStateBase
  {
    private Subject<Unit> _onEndInAnimation = new Subject<Unit>();
    private Subject<Unit> _onEndMenu = new Subject<Unit>();
    private ChestAnimation _chestAnimation;

    protected override void OnAwake(PlayerActor player)
    {
      player.EventKey = EventType.StorageIn;
      if (Object.op_Inequality((Object) player.CurrentPoint, (Object) null))
      {
        this._chestAnimation = (ChestAnimation) ((Component) player.CurrentPoint).GetComponent<ChestAnimation>();
        if (Object.op_Inequality((Object) this._chestAnimation, (Object) null))
          this._chestAnimation.PlayInAnimation();
      }
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndInAnimation, 1), (Action<M0>) (_ => MapUIContainer.SetActiveItemBoxUI(true)), (Action) (() => ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ =>
      {
        MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
        player.Controller.ChangeState("Normal");
      }))));
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      animInfo.outEnableBlend = true;
      animInfo.outBlendSec = 0.0f;
      player.Animation.AnimInfo = animInfo;
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (!Object.op_Inequality((Object) this._chestAnimation, (Object) null))
        return;
      this._chestAnimation.PlayOutAnimation();
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      this.OnEndInAnimation();
      this.OnEndMenu();
    }

    private void OnEndInAnimation()
    {
      if (Object.op_Inequality((Object) this._chestAnimation, (Object) null) && this._chestAnimation.PlayingInAniamtion || this._onEndInAnimation == null)
        return;
      this._onEndInAnimation.OnNext(Unit.get_Default());
    }

    private void OnEndMenu()
    {
      if (MapUIContainer.ItemBoxUI.IsActiveControl)
        return;
      this._onEndMenu.OnNext(Unit.get_Default());
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ItemBox.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new ItemBox.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }
  }
}
