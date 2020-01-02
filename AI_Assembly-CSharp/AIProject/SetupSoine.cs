// Decompiled with JetBrains decompiler
// Type: AIProject.SetupSoine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class SetupSoine : AgentAction
  {
    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      ActionPoint sightActionPoint = agent.TargetInSightActionPoint;
      agent.CurrentPoint = sightActionPoint;
      ActionPoint actionPoint = sightActionPoint;
      actionPoint.SetSlot((Actor) agent);
      PoseKeyPair sleepTogetherRight = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherRight;
      PoseKeyPair sleepTogetherLeft = Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SleepTogetherLeft;
      bool flag = false;
      ActionPointInfo outInfo = new ActionPointInfo();
      if (Object.op_Inequality((Object) actionPoint, (Object) null))
        flag = actionPoint.FindAgentActionPointInfo(EventType.Sleep, sleepTogetherRight.poseID, out outInfo) || actionPoint.FindAgentActionPointInfo(EventType.Sleep, sleepTogetherLeft.poseID, out outInfo);
      if (!flag)
      {
        agent.ChangeBehavior(Desire.ActionType.Normal);
        return (TaskStatus) 1;
      }
      Transform t = ((Component) actionPoint).get_transform().FindLoop(outInfo.baseNullName)?.get_transform() ?? ((Component) actionPoint).get_transform();
      GameObject loop = ((Component) actionPoint).get_transform().FindLoop(outInfo.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[outInfo.eventID][outInfo.poseID];
      ActorAnimInfo actorAnimInfo = agent.Animation.LoadActionState(outInfo.eventID, outInfo.poseID, info);
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.DisableActionFlag();
      agent.DeactivateNavMeshAgent();
      agent.IsKinematic = true;
      agent.Animation.PlayInAnimation(actorAnimInfo.inEnableBlend, actorAnimInfo.inBlendSec, info.MainStateInfo.FadeOutTime, actorAnimInfo.layer);
      agent.SetStand(t, info.MainStateInfo.InStateInfo.EnableFade, info.MainStateInfo.InStateInfo.FadeSecond, info.DirectionType);
      agent.SetCurrentSchedule(actorAnimInfo.isLoop, "添い寝", actorAnimInfo.loopMinTime, actorAnimInfo.loopMaxTime, actorAnimInfo.hasAction, true);
      agent.ChangeBehavior(Desire.ActionType.EndTaskSleepAfterDate);
      return (TaskStatus) 2;
    }
  }
}
