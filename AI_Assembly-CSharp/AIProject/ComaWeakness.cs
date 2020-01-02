// Decompiled with JetBrains decompiler
// Type: AIProject.ComaWeakness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class ComaWeakness : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      if (Object.op_Equality((Object) agent.CurrentPoint, (Object) null))
        agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.DeactivateNavMeshAgent();
      PoseKeyPair comaId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.ComaID;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[comaId.postureID][comaId.poseID];
      agent.Animation.LoadEventKeyTable(comaId.postureID, comaId.poseID);
      agent.Animation.InitializeStates(info);
      agent.LoadActionFlag(comaId.postureID, comaId.poseID);
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
      agent.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.inFadeOutTime, actorAnimInfo2.layer);
      ActionPointInfo outInfo;
      agent.CurrentPoint.TryGetAgentActionPointInfo(EventType.Sleep, out outInfo);
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      agent.SetCurrentSchedule(true, outInfo.actionName, 175, 185, false, false);
      if (!agent.AgentData.ScheduleEnabled)
        return;
      Actor.BehaviorSchedule schedule = agent.Schedule;
      schedule.enabled = agent.AgentData.ScheduleEnabled;
      schedule.elapsedTime = agent.AgentData.ScheduleElapsedTime;
      schedule.duration = agent.AgentData.ScheduleDuration;
      agent.Schedule = schedule;
      agent.AgentData.ScheduleEnabled = false;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (agent.Schedule.enabled)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.ApplySituationResultParameter(36);
      agent.ClearItems();
      agent.ClearParticles();
      agent.Animation.EndStates();
      agent.ResetActionFlag();
      agent.SetDefaultStateHousingItem();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      agent.TargetInSightActionPoint = (ActionPoint) null;
    }
  }
}
