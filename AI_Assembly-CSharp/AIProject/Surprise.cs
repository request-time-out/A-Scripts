// Decompiled with JetBrains decompiler
// Type: AIProject.Surprise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  [TaskCategory("")]
  public class Surprise : AgentAction
  {
    private bool _updatedMotivation;

    public virtual void OnStart()
    {
      AgentActor agent = this.Agent;
      agent.StopNavMeshAgent();
      agent.ChangeStaticNavMeshAgentAvoidance();
      agent.HPositionID = agent.HPositionSubID;
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.InStates.Clear();
      if (agent.SurprisePoseID.HasValue)
      {
        PoseKeyPair poseKeyPair = agent.SurprisePoseID.Value;
        PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[poseKeyPair.postureID][poseKeyPair.poseID];
        agent.Animation.LoadEventKeyTable(poseKeyPair.postureID, poseKeyPair.poseID);
        if (!playState.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in playState.MainStateInfo.InStateInfo.StateInfos)
            agent.Animation.InStates.Enqueue(stateInfo);
        }
      }
      agent.Animation.OutStates.Clear();
      agent.MotivationInEncounter = agent.AgentData.StatsTable[5];
      List<PlayState.ItemInfo> itemList;
      if (Singleton<Resources>.Instance.Animation.SurpriseItemList.TryGetValue(((Object) agent.Animation.Animator).get_name(), out itemList))
        agent.LoadEventItems(itemList);
      agent.Animation.PlayInLocoAnimation(false, 0.0f, 0);
      agent.UpdateMotivation = true;
    }

    public virtual TaskStatus OnUpdate()
    {
      AgentActor agent = this.Agent;
      if (!agent.SurprisePoseID.HasValue)
      {
        Debug.LogError((object) "ビクつきモーション指定がなかった");
        return (TaskStatus) 2;
      }
      if (agent.Animation.PlayingInLocoAnimation)
        return (TaskStatus) 3;
      if ((double) agent.MotivationInEncounter <= 0.0)
      {
        this.Complete();
        return (TaskStatus) 2;
      }
      if (!agent.ReleasableCommand || !agent.IsFarPlayerInSurprise)
        return (TaskStatus) 3;
      this.Complete();
      return (TaskStatus) 2;
    }

    private void Complete()
    {
      AgentActor agent = this.Agent;
      agent.Animation.CrossFadeScreen(-1f);
      agent.SetStand(agent.Animation.RecoveryPoint, false, 0.0f, 0);
      agent.Animation.RecoveryPoint = (Transform) null;
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
      Desire.Type key;
      if (Desire.ModeTable.TryGetValue(agent.PrevMode, out key))
      {
        int desireKey = Desire.GetDesireKey(key);
        agent.SetDesire(desireKey, 0.0f);
      }
      agent.Animation.ResetDefaultAnimatorController();
      agent.ChaControl.SetClothesStateAll((byte) 0);
      agent.ActivateNavMeshAgent();
    }

    public virtual void OnEnd()
    {
      AgentActor agent = this.Agent;
      agent.ClearItems();
      agent.ChangeDynamicNavMeshAgentAvoidance();
      agent.UpdateMotivation = false;
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
