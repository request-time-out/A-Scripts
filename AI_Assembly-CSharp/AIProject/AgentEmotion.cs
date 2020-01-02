// Decompiled with JetBrains decompiler
// Type: AIProject.AgentEmotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using Manager;
using System;
using UniRx;

namespace AIProject
{
  public abstract class AgentEmotion : AgentAction
  {
    protected Subject<Unit> _onEndAction;

    protected void PlayAnimation(int postureID, int poseID)
    {
      AgentActor agent = this.Agent;
      agent.SetActiveOnEquipedItem(false);
      agent.ChaControl.setAllLayerWeight(0.0f);
      PlayState playState = Singleton<Resources>.Instance.Animation.AgentActionAnimTable[postureID][poseID];
      agent.Animation.LoadEventKeyTable(postureID, poseID);
      ActorAnimInfo actorAnimInfo = new ActorAnimInfo()
      {
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
      agent.Animation.AnimInfo = actorAnimInfo;
      ActorAnimInfo animInfo = actorAnimInfo;
      agent.Animation.InitializeStates(playState.MainStateInfo.InStateInfo.StateInfos, playState.MainStateInfo.OutStateInfo.StateInfos, playState.MainStateInfo.AssetBundleInfo);
      this._onEndAction = new Subject<Unit>();
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (Action<M0>) (_ =>
      {
        agent.Animation.StopAllAnimCoroutine();
        agent.Animation.PlayOutAnimation(animInfo.outEnableBlend, animInfo.outBlendSec, animInfo.layer);
      }));
      agent.Animation.StopAllAnimCoroutine();
      agent.Animation.PlayInAnimation(animInfo.inEnableBlend, animInfo.inBlendSec, animInfo.inFadeOutTime, animInfo.layer);
    }

    public virtual TaskStatus OnUpdate()
    {
      if (this.Agent.Animation.PlayingInAnimation)
        return (TaskStatus) 3;
      if (this._onEndAction != null)
        this._onEndAction.OnNext(Unit.get_Default());
      if (this.Agent.Animation.PlayingOutAnimation)
        return (TaskStatus) 3;
      this.OnCompletedEmoteTask();
      return (TaskStatus) 2;
    }

    protected virtual void OnCompletedEmoteTask()
    {
    }

    public virtual void OnEnd()
    {
      this.Agent.SetActiveOnEquipedItem(true);
    }
  }
}
