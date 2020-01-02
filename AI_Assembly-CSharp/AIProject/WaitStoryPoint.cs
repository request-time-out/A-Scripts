// Decompiled with JetBrains decompiler
// Type: AIProject.WaitStoryPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.SaveData;
using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AIProject
{
  [TaskCategory("")]
  public class WaitStoryPoint : AgentAction
  {
    private string _loopStateName = string.Empty;
    [SerializeField]
    private bool _adjustAngle;
    private AgentActor _agent;
    private ActorAnimation _animation;
    private NavMeshAgent _navMeshAgent;
    private StoryPoint _point;
    private bool _missing;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._animation = this._agent.Animation;
      this._navMeshAgent = this._agent.NavMeshAgent;
      this._point = this.Agent.TargetStoryPoint;
      this._loopStateName = string.Empty;
      this._agent.TutorialCanTalk = false;
      if (this._missing = Object.op_Equality((Object) this._agent, (Object) null) || Object.op_Equality((Object) this._animation, (Object) null) || Object.op_Equality((Object) this._navMeshAgent, (Object) null) || Object.op_Equality((Object) this._point, (Object) null))
        return;
      if (this._adjustAngle)
        this.PlayTurnAnimation();
      else
        this.PlayIdleAnimation();
      this._agent.DeactivateNavMeshAgent();
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._missing)
        return (TaskStatus) 1;
      if (this._agent.Animation.PlayingTurnAnimation)
        return (TaskStatus) 3;
      this._agent.TutorialCanTalk = this.CanTalk();
      return (TaskStatus) 3;
    }

    public virtual void OnEnd()
    {
      if (Object.op_Inequality((Object) this._agent, (Object) null))
      {
        this._agent.ClearItems();
        this._agent.ClearParticles();
        this._agent.TutorialCanTalk = false;
      }
      else
      {
        this._agent = this.Agent;
        this._agent.ClearItems();
        this._agent.ClearParticles();
        this._agent.TutorialCanTalk = false;
      }
      ((Task) this).OnEnd();
    }

    private bool CanTalk()
    {
      Tutorial.ActionType tutorialType = this._agent.TutorialType;
      int num;
      switch (tutorialType)
      {
        case Tutorial.ActionType.PassFishingRod:
        case Tutorial.ActionType.FoodRequest:
        case Tutorial.ActionType.WaitAtBase:
          num = 1;
          break;
        default:
          num = tutorialType == Tutorial.ActionType.GrilledFishRequest ? 1 : 0;
          break;
      }
      if (num == 0)
        return false;
      switch (tutorialType)
      {
        case Tutorial.ActionType.PassFishingRod:
          return true;
        case Tutorial.ActionType.FoodRequest:
          if (Singleton<Manager.Map>.IsInstance() && Singleton<Resources>.IsInstance() && Manager.Map.GetTutorialProgress() == 7)
          {
            List<StuffItem> itemList = Singleton<Manager.Map>.Instance?.Player?.PlayerData?.ItemList;
            List<FishingDefinePack.ItemIDPair> fishList = Singleton<Resources>.Instance?.FishingDefinePack?.IDInfo?.FishList;
            if (itemList.IsNullOrEmpty<StuffItem>() || fishList.IsNullOrEmpty<FishingDefinePack.ItemIDPair>())
              return false;
            using (List<FishingDefinePack.ItemIDPair>.Enumerator enumerator = fishList.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                FishingDefinePack.ItemIDPair fishID = enumerator.Current;
                if (itemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == fishID.CategoryID && x.ID == fishID.ItemID && 0 < x.Count)))
                  return true;
              }
              break;
            }
          }
          else
            break;
        case Tutorial.ActionType.WaitAtBase:
          return true;
        case Tutorial.ActionType.GrilledFishRequest:
          if (Singleton<Manager.Map>.IsInstance() && Singleton<Resources>.IsInstance() && Manager.Map.GetTutorialProgress() == 13)
          {
            List<StuffItem> itemList = Singleton<Manager.Map>.Instance?.Player?.PlayerData?.ItemList;
            FishingDefinePack.ItemIDPair? grilledFishID = Singleton<Resources>.Instance?.FishingDefinePack?.IDInfo?.GrilledFish;
            return !itemList.IsNullOrEmpty<StuffItem>() && grilledFishID.HasValue && itemList.Exists((Predicate<StuffItem>) (x => x.CategoryID == grilledFishID.Value.CategoryID && x.ID == grilledFishID.Value.ItemID && 0 < x.Count));
          }
          break;
      }
      return false;
    }

    private void PlayTurnAnimation()
    {
      PlayState.AnimStateInfo personalIdleState = this._agent.GetTutorialPersonalIdleState();
      Vector3 to = Vector3.op_Addition(Vector3.op_Multiply(this._point.Forward, 30f), this._agent.Position);
      float num = Vector3.Angle(Vector3.op_Subtraction(to, this._agent.Position), this._agent.Forward);
      if (Singleton<Resources>.IsInstance())
      {
        float turnEnableAngle = Singleton<Resources>.Instance.LocomotionProfile.TurnEnableAngle;
        if ((double) num < (double) turnEnableAngle)
        {
          ((Component) this._agent.Locomotor).get_transform().LookAt(to, Vector3.get_up());
          Vector3 eulerAngles = ((Component) this._agent.Locomotor).get_transform().get_eulerAngles();
          eulerAngles.x = (__Null) (double) (eulerAngles.z = (__Null) 0.0f);
          ((Component) this._agent.Locomotor).get_transform().set_eulerAngles(eulerAngles);
          return;
        }
      }
      this._agent.Animation.StopAllAnimCoroutine();
      this._animation.PlayTurnAnimation(to, 1f, personalIdleState, false);
    }

    private void PlayIdleAnimation()
    {
      PoseKeyPair pair;
      if (!Singleton<Resources>.IsInstance() || !this.TryGetPoseID(this._agent.ChaControl.fileParam.personality, out pair))
        return;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[pair.postureID][pair.poseID];
      this._loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
      AnimatorStateInfo animatorStateInfo = this._agent.Animation.Animator.GetCurrentAnimatorStateInfo(0);
      if (((AnimatorStateInfo) ref animatorStateInfo).IsName(this._loopStateName))
        return;
      this._animation.InitializeStates(info);
      this._animation.LoadEventKeyTable(pair.postureID, pair.poseID);
      ActorAnimInfo actorAnimInfo1 = new ActorAnimInfo()
      {
        layer = info.Layer,
        inEnableBlend = info.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = info.MainStateInfo.InStateInfo.FadeSecond,
        outEnableBlend = info.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = info.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        isLoop = info.MainStateInfo.IsLoop,
        endEnableBlend = info.EndEnableBlend,
        endBlendSec = info.EndBlendRate
      };
      this._animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      this._animation.StopAllAnimCoroutine();
      this._animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.inFadeOutTime, actorAnimInfo2.layer);
    }

    private bool TryGetPoseID(int personalID, out PoseKeyPair pair)
    {
      Dictionary<int, PoseKeyPair> tutorialIdlePoseTable = Singleton<Resources>.Instance.AgentProfile.TutorialIdlePoseTable;
      if (tutorialIdlePoseTable.IsNullOrEmpty<int, PoseKeyPair>())
      {
        pair = new PoseKeyPair();
        return false;
      }
      if (tutorialIdlePoseTable.TryGetValue(personalID, out pair))
        return true;
      if (tutorialIdlePoseTable.TryGetValue(0, out pair))
      {
        Debug.LogWarning((object) "指定した性格IDでは待機IDが取得できなかったので０番で取得");
        return true;
      }
      pair = new PoseKeyPair();
      return false;
    }
  }
}
