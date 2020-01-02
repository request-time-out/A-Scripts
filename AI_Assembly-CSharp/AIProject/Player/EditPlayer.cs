// Decompiled with JetBrains decompiler
// Type: AIProject.Player.EditPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class EditPlayer : PlayerStateBase
  {
    private Subject<Unit> _onEndMenu = new Subject<Unit>();
    private string _prevChaFileName = string.Empty;
    private Subject<Unit> _onEndFadeIn = new Subject<Unit>();
    private Subject<Unit> _onEndFadeOut = new Subject<Unit>();
    private byte _prevSex;

    protected override void OnAwake(PlayerActor player)
    {
      MapUIContainer.SetActivePlayerChangeUI(true);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      PlayerData playerData = Singleton<Game>.Instance.WorldData.PlayerData;
      this._prevSex = playerData.Sex;
      this._prevChaFileName = playerData.CharaFileName;
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndMenu, 1), (Action<M0>) (_ =>
      {
        if (this.CheckChange(player))
          this.StartChange(player);
        else
          player.Controller.ChangeState("DeviceMenu");
      }));
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.PlayerController.CommandArea.RefreshCommands();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      if (MapUIContainer.PlayerChangeUI.IsActiveControl)
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
      EditPlayer.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new EditPlayer.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }

    private bool CheckChange(PlayerActor player)
    {
      return (int) this._prevSex != (int) player.PlayerData.Sex || this._prevChaFileName != player.PlayerData.CharaFileName;
    }

    private void StartChange(PlayerActor player)
    {
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeIn, 1), (Action<M0>) (_ =>
      {
        this.Refresh(player);
        ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndFadeOut, 1), (Action<M0>) (__ =>
        {
          player.CurrentDevicePoint = (DevicePoint) null;
          MapUIContainer.SetVisibleHUDExceptStoryUI(true);
          MapUIContainer.StorySupportUI.Open();
          player.Controller.ChangeState("Normal");
          Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
          Singleton<Input>.Instance.SetupState();
          player.SetScheduledInteractionState(true);
          player.ReleaseInteraction();
        }));
        ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(100.0)), (Action<M0>) (__ => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 1, 1f, false), (Action<M0>) (___ => {}), (Action) (() => this._onEndFadeOut.OnNext(Unit.get_Default())))));
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) MapUIContainer.StartFade(FadeCanvas.PanelType.Blackout, (global::FadeType) 0, 1f, true), (Action<M0>) (_ => {}), (Action) (() => this._onEndFadeIn.OnNext(Unit.get_Default())));
    }

    private void Refresh(PlayerActor player)
    {
      player.ReloadChara();
      Resources.UnloadUnusedAssets();
      GC.Collect();
    }
  }
}
