// Decompiled with JetBrains decompiler
// Type: AIProject.Masturbation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [TaskCategory("")]
  public class Masturbation : AgentAction
  {
    private bool _updatedMotivation;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.EventKey = EventType.Masturbation;
      ((Task) this).OnStart();
      agent.CurrentPoint = this.Agent.TargetInSightActionPoint;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      agent.ElectNextPoint();
      if (Object.op_Equality((Object) agent.CurrentPoint, (Object) null))
        return;
      agent.CurrentPoint.SetActiveMapItemObjs(false);
      ActionPointInfo actionPointInfo1 = agent.TargetInSightActionPoint.GetActionPointInfo(agent);
      agent.Animation.ActionPointInfo = actionPointInfo1;
      ActionPointInfo actionPointInfo2 = actionPointInfo1;
      Transform t = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.baseNullName)?.get_transform() ?? ((Component) agent.CurrentPoint).get_transform();
      GameObject loop = ((Component) agent.CurrentPoint).get_transform().FindLoop(actionPointInfo2.recoveryNullName);
      agent.Animation.RecoveryPoint = loop?.get_transform();
      int eventId = actionPointInfo2.eventID;
      agent.ActionID = eventId;
      int actionID = eventId;
      int poseId = actionPointInfo2.poseID;
      agent.PoseID = poseId;
      int poseID = poseId;
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[actionID][poseID];
      agent.LoadActionFlag(actionID, poseID);
      agent.DeactivateNavMeshAgent();
      agent.Animation.StopAllAnimCoroutine();
      HScene.AnimationListInfo info = Singleton<Resources>.Instance.HSceneTable.lstAnimInfo[3][actionPointInfo2.poseID];
      agent.Animation.BeginIgnoreEvent();
      AssetBundleInfo assetBundleInfo = playState.MainStateInfo.AssetBundleInfo;
      RuntimeAnimatorController rac = AssetUtility.LoadAsset<RuntimeAnimatorController>((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset, string.Empty);
      agent.Animation.SetAnimatorController(rac);
      agent.StartMasturbationSequence(info);
      agent.CurrentPoint.SetSlot((Actor) agent);
      agent.SetStand(t, false, 0.0f, 0);
      agent.SurprisePoseID = new PoseKeyPair?(Singleton<Resources>.Instance.AgentProfile.PoseIDTable.SurpriseMasturbationID);
      agent.UpdateMotivation = true;
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.PlayingMasturbationSequence)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    public virtual void OnEnd()
    {
      ((Task) this).OnEnd();
      AgentActor agent = this.Agent;
      agent.StopMasturbationSequence();
      agent.Animation.EndIgnoreEvent();
      agent.UpdateMotivation = false;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.Animation.CrossFadeScreen(-1f);
      agent.ActivateNavMeshAgent();
      agent.SetActiveOnEquipedItem(true);
      agent.SetStand(agent.Animation.RecoveryPoint, false, 0.0f, 0);
      agent.Animation.RecoveryPoint = (Transform) null;
      int desireKey = Desire.GetDesireKey(Desire.Type.H);
      agent.SetDesire(desireKey, 0.0f);
      agent.UpdateStatus(agent.ActionID, agent.PoseID);
      agent.ApplySituationResultParameter(31);
      agent.ClearItems();
      agent.ClearParticles();
      agent.ResetActionFlag();
      if (Object.op_Inequality((Object) agent.CurrentPoint, (Object) null))
      {
        agent.CurrentPoint.SetActiveMapItemObjs(true);
        agent.CurrentPoint.ReleaseSlot((Actor) agent);
        agent.CurrentPoint = (ActionPoint) null;
        agent.PrevActionPoint = agent.TargetInSightActionPoint;
      }
      agent.TargetInSightActionPoint = (ActionPoint) null;
      agent.SurprisePoseID = new PoseKeyPair?();
      agent.Animation.EndIgnoreEvent();
      agent.Animation.ResetDefaultAnimatorController();
    }

    public virtual void OnPause(bool paused)
    {
      AgentActor agent = this.Agent;
      if (paused)
      {
        this._updatedMotivation = paused;
        agent.UpdateMotivation = false;
      }
      else
        agent.UpdateMotivation = this._updatedMotivation;
    }
  }
}
