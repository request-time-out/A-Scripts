// Decompiled with JetBrains decompiler
// Type: AIProject.Player.WMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace AIProject.Player
{
  public class WMap : PlayerStateBase
  {
    private MiniMapControler minimapUI;
    private Input input;
    private float dt;
    private const float nonOpeTime = 0.4f;

    protected override void OnAwake(PlayerActor player)
    {
      this.minimapUI = Singleton<MapUIContainer>.Instance.MinimapUI;
      this.input = Singleton<Input>.Instance;
      this.dt = 0.0f;
      this.minimapUI.OpenAllMap(this.minimapUI.VisibleMode);
      Singleton<MapUIContainer>.Instance.MinimapUI.VisibleMode = 1;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      Singleton<Input>.Instance.ReserveState(Input.ValidType.UI);
      Singleton<Input>.Instance.SetupState();
      this.minimapUI.FromHomeMenu = false;
      Singleton<MapUIContainer>.Instance.MinimapUI.WarpProc = (MiniMapControler.OnWarp) (x =>
      {
        Singleton<MapUIContainer>.Instance.MinimapUI.AllMapClosedAction = (Action) (() => {});
        string prevStateName = player.PlayerController.PrevStateName;
        Singleton<Manager.Map>.Instance.WarpToBasePoint(x, (Action) (() =>
        {
          if (prevStateName == "Onbu")
            player.Controller.ChangeState("Onbu");
          else
            player.Controller.ChangeState("Normal");
          player.Controller.ChangeState("Idle");
          GC.Collect();
          if (this.minimapUI.prevVisibleMode != 0 || !Manager.Config.GameData.MiniMap)
            return;
          this.minimapUI.OpenMiniMap();
        }), (Action) (() =>
        {
          if (prevStateName == "Onbu")
            player.Controller.ChangeState("Onbu");
          else
            player.Controller.ChangeState("Normal");
          Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
          Singleton<Input>.Instance.SetupState();
          Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(true);
          Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        }));
        Singleton<MapUIContainer>.Instance.MinimapUI.WarpProc = (MiniMapControler.OnWarp) null;
      });
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      animInfo.outEnableBlend = true;
      animInfo.outBlendSec = 0.0f;
      player.Animation.AnimInfo = animInfo;
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
      this.dt += Time.get_unscaledDeltaTime();
      if ((double) this.dt < 0.400000005960464 || !this.input.IsPressedKey((KeyCode) 109) || this.minimapUI.nowCloseAllMap)
        return;
      this.minimapUI.ChangeCamera(false, false);
      this.minimapUI.WarpMoveDispose();
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }
  }
}
