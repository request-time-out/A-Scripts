// Decompiled with JetBrains decompiler
// Type: AIProject.WokenUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using UnityEngine;

namespace AIProject
{
  public class WokenUp : AgentAction
  {
    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Sleep;
      agent.CurrentPoint = agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      agent.DeactivateNavMeshAgent();
      agent.DisableActionFlag();
      ActionPointInfo actionPointInfo1 = agent.TargetInSightActionPoint.GetActionPointInfo(agent);
      agent.Animation.ActionPointInfo = actionPointInfo1;
      ActionPointInfo actionPointInfo2 = actionPointInfo1;
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      PoseKeyPair wakenUpId = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.WakenUpID;
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[wakenUpId.postureID][wakenUpId.poseID];
      agent.Animation.LoadEventKeyTable(wakenUpId.postureID, wakenUpId.poseID);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = playState.Layer,
        inEnableBlend = playState.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = playState.MainStateInfo.InStateInfo.FadeSecond,
        inFadeOutTime = playState.MainStateInfo.FadeOutTime,
        outEnableBlend = playState.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = playState.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = playState.DirectionType,
        endEnableBlend = playState.EndEnableBlend,
        endBlendSec = playState.EndBlendRate,
        isLoop = playState.MainStateInfo.IsLoop,
        loopMinTime = playState.MainStateInfo.LoopMin,
        loopMaxTime = playState.MainStateInfo.LoopMax,
        hasAction = playState.ActionInfo.hasAction,
        loopStateName = playState.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(playState.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName,
        randomCount = playState.ActionInfo.randomCount,
        oldNormalizedTime = 0.0f
      };
      agent.Animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      if (!playState.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info stateInfo in playState.MainStateInfo.InStateInfo.StateInfos)
          agent.Animation.InStates.Enqueue(stateInfo);
      }
      agent.Animation.OutStates.Clear();
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.inFadeOutTime, actorAnimInfo2.layer);
      agent.SleepTrigger = false;
      agent.AgentData.YobaiTrigger = false;
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, playState.MainStateInfo.InStateInfo.EnableFade, playState.MainStateInfo.InStateInfo.FadeSecond, playState.DirectionType);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.ResetActionFlag();
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      agent.Animation.EndStates();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
      }
      agent.EventKey = (EventType) 0;
      agent.PrevActionPoint = agent.TargetInSightActionPoint;
      if (Object.op_Inequality((Object) agent.TargetInSightActionPoint, (Object) null))
        agent.TargetInSightActionPoint.Reserver = (Actor) null;
      agent.TargetInSightActionPoint = (ActionPoint) null;
      agent.ApplySituationResultParameter(32);
    }
  }
}
