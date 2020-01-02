// Decompiled with JetBrains decompiler
// Type: AIProject.PlayIdleOnTutorialMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class PlayIdleOnTutorialMode : AgentAction
  {
    private string _loopStateName = string.Empty;
    private bool _notNeet;
    private AgentActor _agent;
    private ActorAnimation _animation;

    public virtual void OnStart()
    {
      ((Task) this).OnStart();
      this._agent = this.Agent;
      this._animation = this._agent.Animation;
      this._agent.DeactivateNavMeshAgent();
      if (this._notNeet = Object.op_Equality((Object) this._animation, (Object) null))
        return;
      this._loopStateName = string.Empty;
      PoseKeyPair pair;
      if (!Singleton<Resources>.IsInstance() || !this.TryGetPoseID(this._agent.ChaControl.fileParam.personality, out pair))
        return;
      PlayState info = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[pair.postureID][pair.poseID];
      this._loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName;
      AnimatorStateInfo animatorStateInfo = this._animation.Animator.GetCurrentAnimatorStateInfo(0);
      if (this._notNeet = ((AnimatorStateInfo) ref animatorStateInfo).IsName(this._loopStateName))
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
        isLoop = info.MainStateInfo.IsLoop
      };
      this._animation.AnimInfo = actorAnimInfo1;
      ActorAnimInfo actorAnimInfo2 = actorAnimInfo1;
      this._animation.StopAllAnimCoroutine();
      this._animation.PlayInAnimation(actorAnimInfo2.inEnableBlend, actorAnimInfo2.inBlendSec, actorAnimInfo2.inFadeOutTime, actorAnimInfo2.layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this._notNeet)
        return (TaskStatus) 2;
      return this._animation.PlayingInAnimation ? (TaskStatus) 3 : (TaskStatus) 2;
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
