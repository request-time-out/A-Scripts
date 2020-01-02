// Decompiled with JetBrains decompiler
// Type: AIProject.FallWeakness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;

namespace AIProject
{
  [TaskCategory("")]
  public class FallWeakness : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.StateType = AIProject.Definitions.State.Type.Collapse;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      PoseKeyPair weaknessId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.WeaknessID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[weaknessId.postureID][weaknessId.poseID];
      agent.Animation.LoadEventKeyTable(weaknessId.postureID, weaknessId.poseID);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = info.Layer,
        inEnableBlend = info.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = info.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = info.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = info.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        endEnableBlend = info.EndEnableBlend,
        endBlendSec = info.EndBlendRate,
        isLoop = info.MainStateInfo.IsLoop,
        loopMinTime = info.MainStateInfo.LoopMin,
        loopMaxTime = info.MainStateInfo.LoopMax,
        hasAction = info.ActionInfo.hasAction,
        loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName,
        randomCount = info.ActionInfo.randomCount,
        oldNormalizedTime = 0.0f
      };
      agent.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      agent.Animation.InitializeStates(info);
      agent.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.inFadeOutTime, actorAnimInfo2.layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      return this.Agent.Animation.PlayingInAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
    }
  }
}
