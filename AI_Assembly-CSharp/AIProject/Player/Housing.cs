// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Housing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Housing : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      if (Singleton<MapUIContainer>.IsInstance())
        MapUIContainer.NicknameUI.CanvasAlpha = 0.0f;
      Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (Action<M0>) (__ => {}), (Action) (() =>
      {
        player.SetActiveOnEquipedItem(false);
        player.ChaControl.setAllLayerWeight(0.0f);
        Singleton<Manager.Map>.Instance.DisableEntity();
        Singleton<Manager.Map>.Instance.Simulator.EnabledTimeProgression = false;
        ((Behaviour) player.CameraControl.CameraComponent).set_enabled(false);
        Singleton<Manager.Housing>.Instance.StartHousing();
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.InvokeAcception);
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
      Housing.\u003COnEnd\u003Ec__Iterator0 onEndCIterator0 = new Housing.\u003COnEnd\u003Ec__Iterator0();
      return (IEnumerator) onEndCIterator0;
    }
  }
}
