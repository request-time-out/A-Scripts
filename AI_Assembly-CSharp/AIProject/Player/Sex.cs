// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Sex
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace AIProject.Player
{
  public class Sex : PlayerStateBase
  {
    protected override void OnAwake(PlayerActor player)
    {
      player.ChaControl.visibleAll = false;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      player.SetScheduledInteractionState(false);
      player.ReleaseInteraction();
      if (!Object.op_Inequality((Object) player.CommCompanion, (Object) null))
        return;
      player.CommCompanion.Animation.BeginIgnoreExpression();
    }

    [DebuggerHidden]
    public override IEnumerator End(Actor actor)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Sex.\u003CEnd\u003Ec__Iterator0 endCIterator0 = new Sex.\u003CEnd\u003Ec__Iterator0();
      return (IEnumerator) endCIterator0;
    }

    protected override void OnAfterUpdate(PlayerActor actor, Actor.InputInfo info)
    {
      actor.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnUpdate(PlayerActor actor, ref Actor.InputInfo info)
    {
      actor.NavMeshAgent.set_velocity(info.move = Vector3.get_zero());
    }
  }
}
