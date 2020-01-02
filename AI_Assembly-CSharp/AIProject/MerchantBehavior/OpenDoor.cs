// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.OpenDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class OpenDoor : MerchantMoveAction
  {
    private EventType prevEventKey;
    private bool isDoorOpen;

    public override void OnStart()
    {
      this.prevEventKey = this.Merchant.EventKey;
      this.Merchant.EventKey = EventType.DoorOpen;
      OffMeshLinkData currentOffMeshLinkData = this.Merchant.NavMeshAgent.get_currentOffMeshLinkData();
      DoorPoint component1 = (DoorPoint) ((Component) ((OffMeshLinkData) ref currentOffMeshLinkData).get_offMeshLink()).GetComponent<DoorPoint>();
      this.isDoorOpen = !((OffMeshLinkData) ref currentOffMeshLinkData).get_activated() || Object.op_Equality((Object) component1, (Object) null) || component1.IsOpen;
      this.CurrentPoint = !this.isDoorOpen ? (ActionPoint) component1 : (ActionPoint) null;
      if (this.isDoorOpen)
        this.Merchant.EventKey = this.prevEventKey;
      base.OnStart();
      if (!Object.op_Inequality((Object) this.CurrentPoint, (Object) null))
        return;
      DoorAnimation component2 = (DoorAnimation) ((Component) this.CurrentPoint).GetComponent<DoorAnimation>();
      if (!Object.op_Inequality((Object) component2, (Object) null))
        return;
      ActionPointInfo outInfo;
      this.CurrentPoint.TryGetAgentActionPointInfo(EventType.DoorOpen, out outInfo);
      PlayState playState = Singleton<Resources>.Instance.Animation.MerchantCommonActionAnimStateTable[outInfo.eventID][outInfo.poseID];
      component2.Load(playState.MainStateInfo.InStateInfo.StateInfos);
      component2.PlayAnimation(this.animInfo.inEnableBlend, this.animInfo.inBlendSec, this.animInfo.inFadeOutTime, this.animInfo.layer);
    }

    public override TaskStatus OnUpdate()
    {
      if (!this.isDoorOpen)
        return base.OnUpdate();
      NavMeshAgent navMeshAgent = this.Merchant.NavMeshAgent;
      if (Object.op_Inequality((Object) navMeshAgent, (Object) null) && ((Behaviour) navMeshAgent).get_isActiveAndEnabled())
        navMeshAgent.ResetPath();
      return (TaskStatus) 2;
    }

    protected override void OnCompletedStateTask()
    {
      if (Object.op_Equality((Object) this.CurrentPoint, (Object) null))
        return;
      DoorPoint currentPoint = this.CurrentPoint as DoorPoint;
      if (currentPoint.OpenType == DoorPoint.OpenTypeState.Right)
        currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenRight, true);
      else
        currentPoint.SetOpenState(DoorPoint.OpenPattern.OpenLeft, true);
    }
  }
}
