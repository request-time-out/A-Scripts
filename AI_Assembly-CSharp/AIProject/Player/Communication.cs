// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Communication
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace AIProject.Player
{
  public class Communication : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      player.SetActiveOnEquipedItem(false);
      player.ChaControl.setAllLayerWeight(0.0f);
      player.ChaControl.visibleAll = false;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      ActorAnimInfo animInfo = player.Animation.AnimInfo;
      animInfo.outBlendSec = 0.0f;
      animInfo.outEnableBlend = true;
      player.Animation.AnimInfo = animInfo;
      if (!Object.op_Inequality((Object) player.CommCompanion, (Object) null))
        return;
      player.CommCompanion.Animation.BeginIgnoreExpression();
      player.CommCompanion.Animation.BeginIgnoreVoice();
    }

    protected override void OnRelease(PlayerActor player)
    {
      player.SetScheduledInteractionState(true);
      player.ReleaseInteraction();
      Singleton<Input>.Instance.ReserveState(Input.ValidType.Action);
      Singleton<Input>.Instance.SetupState();
      if (!Object.op_Inequality((Object) player.CommCompanion, (Object) null))
        return;
      player.CommCompanion.Animation.EndIgnoreExpression();
      player.CommCompanion.Animation.EndIgnoreVoice();
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      player.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }
  }
}
