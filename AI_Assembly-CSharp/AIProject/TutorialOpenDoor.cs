// Decompiled with JetBrains decompiler
// Type: AIProject.TutorialOpenDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class TutorialOpenDoor : AgentTutorialMoveAction
  {
    public override void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.DoorOpen;
      OffMeshLinkData currentOffMeshLinkData = agent.NavMeshAgent.get_currentOffMeshLinkData();
      DoorPoint component1 = (DoorPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<DoorPoint>();
      agent.TargetInSightActionPoint = (ActionPoint) component1;
      this._actionMotion = new PoseKeyPair()
      {
        postureID = -1,
        poseID = -1
      };
      base.OnStart();
      if (!Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
        return;
      DoorAnimation component2 = (DoorAnimation) ((Component) agent.CurrentPoint).GetComponent<DoorAnimation>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      ActionPointInfo actionPointInfo = agent.Animation.ActionPointInfo;
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[actionPointInfo.eventID][actionPointInfo.poseID];
      component2.Load(playState.MainStateInfo.InStateInfo.StateInfos);
      ActorAnimInfo animInfo = agent.Animation.AnimInfo;
      component2.PlayAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, playState.MainStateInfo.FadeOutTime, animInfo.layer);
    }

    protected override void OnCompletedStateTask()
    {
      DoorPoint currentPoint = this.Agent.CurrentPoint as DoorPoint;
      if (currentPoint.OpenType == DoorPoint.OpenTypeState.Right || currentPoint.OpenType == DoorPoint.OpenTypeState.Right90)
        currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenRight, true);
      else
        currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenLeft, true);
    }
  }
}
