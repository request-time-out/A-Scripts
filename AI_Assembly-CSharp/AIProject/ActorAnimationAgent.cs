// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimationAgent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using IllusionUtility.GetUtility;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class ActorAnimationAgent : ActorAnimation
  {
    [SerializeField]
    private FullBodyBipedIK _ik;
    private Subject<Unit> _endAction;
    private Subject<Unit> _actionPlay;
    private Subject<Unit> _completeAction;
    private float _currentVelocity;

    public FullBodyBipedIK IK
    {
      get
      {
        return this._ik;
      }
      set
      {
        this._ik = value;
        if (!Object.op_Implicit((Object) this._ik))
          return;
        __Null solver = this._ik.solver;
        ((IKSolver) solver).OnPostUpdate = (__Null) Delegate.Combine((Delegate) ((IKSolver) solver).OnPostUpdate, (Delegate) new IKSolver.UpdateDelegate((object) this, __methodptr(AfterFBBIK)));
        ((Behaviour) this._ik).set_enabled(true);
      }
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this._ik, (Object) null))
      {
        this._ik = (FullBodyBipedIK) ((Component) this).GetComponentInChildren<FullBodyBipedIK>(true);
        if (Object.op_Implicit((Object) this._ik))
        {
          ((Behaviour) this._ik).set_enabled(true);
          // ISSUE: variable of the null type
          __Null solver = this._ik.solver;
          // ISSUE: method pointer
          ((IKSolver) solver).OnPostUpdate = (__Null) Delegate.Combine((Delegate) ((IKSolver) solver).OnPostUpdate, (Delegate) new IKSolver.UpdateDelegate((object) this, __methodptr(AfterFBBIK)));
        }
      }
      if (Object.op_Equality((Object) this.Animator, (Object) null))
        this.Animator = (Animator) ((Component) this).GetComponentInChildren<Animator>();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
    }

    protected void OnAnimatorMove()
    {
      Transform transform1 = ((Component) this.Character).get_transform();
      transform1.set_localPosition(Vector3.op_Addition(transform1.get_localPosition(), this.Animator.get_deltaPosition()));
      Transform transform2 = ((Component) this.Character).get_transform();
      transform2.set_localRotation(Quaternion.op_Multiply(transform2.get_localRotation(), this.Animator.get_deltaRotation()));
    }

    public override Vector3 GetPivotPoint()
    {
      return ((Component) this).get_transform().get_position();
    }

    public IObservable<Unit> OnEndActionAsObservable()
    {
      return (IObservable<Unit>) this._endAction ?? (IObservable<Unit>) (this._endAction = new Subject<Unit>());
    }

    public IObservable<Unit> OnActionPlayAsObservable()
    {
      return (IObservable<Unit>) this._actionPlay ?? (IObservable<Unit>) (this._actionPlay = new Subject<Unit>());
    }

    public IObservable<Unit> OnCompleteActionAsObservable()
    {
      return (IObservable<Unit>) this._completeAction ?? (IObservable<Unit>) (this._completeAction = new Subject<Unit>());
    }

    public TaskStatus OnUpdateActionState()
    {
      if (this.PlayingInAnimation)
        return (TaskStatus) 3;
      ActorAnimInfo animInfo = this.AnimInfo;
      if (animInfo.isLoop)
      {
        if (this.PlayingActAnimation)
          return (TaskStatus) 3;
        if (!(this.Actor as AgentActor).Schedule.enabled)
        {
          if (this._endAction != null)
            this._endAction.OnNext(Unit.get_Default());
          if (this.PlayingOutAnimation)
            return (TaskStatus) 3;
          if (this._completeAction != null)
            this._completeAction.OnNext(Unit.get_Default());
          return (TaskStatus) 2;
        }
        AnimatorStateInfo animatorStateInfo = this.Animator.GetCurrentAnimatorStateInfo(0);
        if (((AnimatorStateInfo) ref animatorStateInfo).IsName(animInfo.loopStateName) && (double) (((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() - animInfo.oldNormalizedTime) > 1.0)
        {
          animInfo.oldNormalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
          if (Random.Range(0, animInfo.randomCount) == 0)
          {
            if (this._actionPlay != null)
              this._actionPlay.OnNext(Unit.get_Default());
            animInfo.oldNormalizedTime = 0.0f;
          }
        }
        return (TaskStatus) 3;
      }
      if (this._endAction != null)
        this._endAction.OnNext(Unit.get_Default());
      if (this.PlayingOutAnimation)
        return (TaskStatus) 3;
      if (this._completeAction != null)
        this._completeAction.OnNext(Unit.get_Default());
      return (TaskStatus) 2;
    }

    protected override void LoadMatchTargetInfo(string stateName)
    {
      List<AnimeMoveInfo> animeMoveInfoList;
      if (!Singleton<Resources>.Instance.Animation.AgentMoveInfoTable.TryGetValue(Animator.StringToHash(stateName), out animeMoveInfoList))
        return;
      foreach (AnimeMoveInfo animeMoveInfo in animeMoveInfoList)
      {
        GameObject loop = ((Component) this.Actor.CurrentPoint).get_transform().FindLoop(animeMoveInfo.movePoint);
        ProceduralTargetParameter proceduralTargetParameter = new ProceduralTargetParameter()
        {
          Start = animeMoveInfo.start,
          End = animeMoveInfo.end
        };
        if (Object.op_Inequality((Object) loop, (Object) null))
          proceduralTargetParameter.Target = loop.get_transform();
        this.Targets.Add(proceduralTargetParameter);
      }
    }

    protected override void LoadStateLocomotionVoice(int stateHashName)
    {
      int voiceID;
      if (Object.op_Equality((Object) (this.Actor as AgentActor), (Object) null) || !Singleton<Resources>.Instance.Action.AgentLocomotionBreathTable.TryGetValue(stateHashName, out voiceID) || this._loopActionVoice.Item1 == voiceID && Object.op_Inequality((Object) this._loopActionVoice.Item2, (Object) null))
        return;
      int personality = this.Actor.ChaControl.fileParam.personality;
      AssetBundleInfo info;
      if (!Singleton<Resources>.Instance.Sound.TryGetMapActionVoiceInfo(personality, voiceID, out info))
        return;
      Transform transform1 = ((Component) this.Actor.Locomotor).get_transform();
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int num = personality;
      string assetbundle = (string) info.assetbundle;
      string asset = (string) info.asset;
      Transform transform2 = transform1;
      int no = num;
      string assetBundleName = assetbundle;
      string assetName = asset;
      Transform voiceTrans = transform2;
      Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, 1f, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, false, true, false);
      this.Actor.ChaControl.SetVoiceTransform(trfVoice);
      this._loopActionVoice.Item1 = (__Null) voiceID;
      this._loopActionVoice.Item2 = (__Null) ((Component) trfVoice).GetComponent<AudioSource>();
      this._loopActionVoice.Item3 = (__Null) transform1;
      this._loopActionVoice.Item4 = (__Null) personality;
    }

    protected override void LoadStateExpression(int stateHashName)
    {
      Resources.ActionTable action = Singleton<Resources>.Instance.Action;
      this.ExpressionKeyframeList.Clear();
      int personality = this.Actor.ChaControl.fileParam.personality;
      Dictionary<int, List<ExpressionKeyframe>> dictionary1;
      List<ExpressionKeyframe> expressionKeyframeList;
      if (action.ActionExpressionKeyframeTable.TryGetValue(personality, out dictionary1) && dictionary1.TryGetValue(stateHashName, out expressionKeyframeList))
      {
        foreach (ExpressionKeyframe expressionKeyframe in expressionKeyframeList)
          this.ExpressionKeyframeList.Add(new ActorAnimation.ExpressionKeyframeEvent()
          {
            NormalizedTime = expressionKeyframe.normalizedTime,
            Name = expressionKeyframe.expressionName
          });
      }
      else
      {
        Dictionary<int, string> dictionary2;
        string key;
        if (!Singleton<Resources>.Instance.Action.ActionExpressionTable.TryGetValue(personality, out dictionary2) || !dictionary2.TryGetValue(stateHashName, out key))
          return;
        Singleton<Game>.Instance.GetExpression(personality, key)?.Change(this.Actor.ChaControl);
      }
    }

    protected override void UpdateExpressionEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.ExpressionKeyframeEvent expressionKeyframe in this.ExpressionKeyframeList)
          expressionKeyframe.Used = false;
      }
      else
      {
        foreach (ActorAnimation.ExpressionKeyframeEvent expressionKeyframe in this.ExpressionKeyframeList)
        {
          if ((double) expressionKeyframe.NormalizedTime > (double) normalizedTime && expressionKeyframe.Used)
            expressionKeyframe.Used = false;
        }
      }
      int personality = this.Actor.ChaControl.fileParam.personality;
      foreach (ActorAnimation.ExpressionKeyframeEvent expressionKeyframe in this.ExpressionKeyframeList)
      {
        if ((double) expressionKeyframe.NormalizedTime < (double) normalizedTime && !expressionKeyframe.Used)
        {
          expressionKeyframe.Used = true;
          Singleton<Game>.Instance.GetExpression(personality, expressionKeyframe.Name)?.Change(this.Actor.ChaControl);
        }
      }
    }

    public override void LoadEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeEventInfo>> dictionary2;
      if (Singleton<Resources>.Instance.Animation.AgentItemEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.ItemEventKeyTable = dictionary2;
      Dictionary<int, List<AnimeEventInfo>> dictionary3;
      if (Singleton<Resources>.Instance.Animation.AgentChangeClothEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary3))
        this.ClothEventKeyTable = dictionary3;
      this.LoadSEEventKeyTable(eventID, poseID);
      this.LoadParticleEventKeyTable(eventID, poseID);
      this.LoadOnceVoiceEventKeyTable(eventID, poseID);
      this.LoadLoopVoiceEventKeyTable(eventID, poseID);
    }

    public override void LoadAnimalEventKeyTable(int animalTypeID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeEventInfo>> dictionary2;
      if (!Singleton<Resources>.Instance.Animation.AgentAnimalEventKeyTable.TryGetValue(animalTypeID, out dictionary1) || !dictionary1.TryGetValue(poseID, out dictionary2))
        return;
      this.ItemEventKeyTable = dictionary2;
    }

    public override void LoadSEEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeSEEventInfo>> dictionary2;
      if (Singleton<Resources>.Instance.Animation.AgentActSEEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.SEEventKeyTable = dictionary2;
      else
        this.SEEventKeyTable = (Dictionary<int, List<AnimeSEEventInfo>>) null;
    }

    public override void LoadParticleEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeParticleEventInfo>> dictionary2;
      if (Singleton<Resources>.Instance.Animation.AgentActParticleEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.ParticleEventKeyTable = dictionary2;
      else
        this.ParticleEventKeyTable = (Dictionary<int, List<AnimeParticleEventInfo>>) null;
    }

    public override void LoadOnceVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeOnceVoiceEventInfo>> dictionary2;
      if (Singleton<Resources>.Instance.Animation.AgentActOnceVoiceEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.OnceVoiceEventKeyTable = dictionary2;
      else
        this.OnceVoiceEventKeyTable = (Dictionary<int, List<AnimeOnceVoiceEventInfo>>) null;
    }

    public override void LoadLoopVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, List<int>>> dictionary1;
      Dictionary<int, List<int>> dictionary2;
      if (Singleton<Resources>.Instance.Animation.AgentActLoopVoiceEventKeyTable.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.LoopVoiceEventKeyTable = dictionary2;
      else
        this.LoopVoiceEventKeyTable = (Dictionary<int, List<int>>) null;
    }

    protected override void PlayEventOnceVoice(int voiceID)
    {
      AgentActor actor = this.Actor as AgentActor;
      if (Object.op_Equality((Object) actor, (Object) null))
        return;
      int personality = actor.ChaControl.fileParam.personality;
      AssetBundleInfo info;
      if (!Singleton<Resources>.Instance.Sound.TryGetMapActionVoiceInfo(personality, voiceID, out info))
        return;
      Transform transform1 = ((Component) this.Actor.Locomotor).get_transform();
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int num = personality;
      string assetbundle = (string) info.assetbundle;
      string asset = (string) info.asset;
      Transform transform2 = transform1;
      int no = num;
      string assetBundleName = assetbundle;
      string assetName = asset;
      Transform voiceTrans = transform2;
      Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, 1f, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, true, true, false);
      this.Actor.ChaControl.SetVoiceTransform(trfVoice);
      this.OnceActionVoice = (AudioSource) ((Component) trfVoice).GetComponent<AudioSource>();
      if (!Object.op_Inequality((Object) this.OnceActionVoice, (Object) null))
        return;
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnDestroyAsObservable((Component) this.OnceActionVoice), ((Component) this).get_gameObject()), (Action<M0>) (_ =>
      {
        if (this.PlayEventLoopVoice())
          return;
        this.StopLoopActionVoice();
      }));
    }

    protected override bool PlayEventLoopVoice()
    {
      if (!base.PlayEventLoopVoice())
        return false;
      AgentActor actor = this.Actor as AgentActor;
      if (Object.op_Equality((Object) actor, (Object) null))
        return false;
      int stateLoopVoiceEvent = this.StateLoopVoiceEvents[Random.Range(0, this.StateLoopVoiceEvents.Count)];
      if (this._loopActionVoice.Item1 == stateLoopVoiceEvent && Object.op_Inequality((Object) this._loopActionVoice.Item2, (Object) null))
        return false;
      int personality = actor.ChaControl.fileParam.personality;
      AssetBundleInfo info;
      if (!Singleton<Resources>.Instance.Sound.TryGetMapActionVoiceInfo(personality, stateLoopVoiceEvent, out info))
        return false;
      Transform transform1 = ((Component) this.Actor.Locomotor).get_transform();
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int num = personality;
      string assetbundle = (string) info.assetbundle;
      string asset = (string) info.asset;
      Transform transform2 = transform1;
      int no = num;
      string assetBundleName = assetbundle;
      string assetName = asset;
      Transform voiceTrans = transform2;
      Transform trfVoice = instance.OnecePlayChara(no, assetBundleName, assetName, 1f, 0.0f, 0.0f, true, voiceTrans, Manager.Voice.Type.PCM, -1, false, true, false);
      this.Actor.ChaControl.SetVoiceTransform(trfVoice);
      this._loopActionVoice.Item1 = (__Null) stateLoopVoiceEvent;
      this._loopActionVoice.Item2 = (__Null) ((Component) trfVoice).GetComponent<AudioSource>();
      this._loopActionVoice.Item3 = (__Null) transform1;
      this._loopActionVoice.Item4 = (__Null) personality;
      return Object.op_Inequality((Object) this._loopActionVoice.Item2, (Object) null);
    }

    protected override void LoadFootStepEventKeyTable()
    {
      this._footStepEventKeyTable = Singleton<Resources>.Instance.Animation.AgentFootStepEventKeyTable;
    }

    public void UpdateState(ActorLocomotion.AnimationState state)
    {
      if (Mathf.Approximately(Time.get_deltaTime(), 0.0f) || Object.op_Equality((Object) this.Animator, (Object) null))
        return;
      AnimatorStateInfo animatorStateInfo = this.Animator.GetCurrentAnimatorStateInfo(0);
      this.IsLocomotionState = ((AnimatorStateInfo) ref animatorStateInfo).IsName("Locomotion");
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      string directionParameterName = definePack.AnimatorParameter.DirectionParameterName;
      string forwardMove = definePack.AnimatorParameter.ForwardMove;
      string heightParameterName = definePack.AnimatorParameter.HeightParameterName;
      float num1 = !state.setMediumOnWalk ? Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z) : (state.moveDirection.z < (double) state.medVelocity || Mathf.Approximately((float) state.moveDirection.z, state.medVelocity) ? Mathf.Lerp(0.0f, 0.5f, Mathf.InverseLerp(0.0f, Mathf.InverseLerp(0.0f, state.maxVelocity, state.medVelocity), Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z))) : Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z));
      if (!this.Actor.IsSlave)
      {
        float num2 = Mathf.Lerp(this.Animator.GetFloat(forwardMove), num1, Time.get_deltaTime() * Singleton<Resources>.Instance.LocomotionProfile.LerpSpeed);
        this.Animator.SetFloat(forwardMove, num2);
      }
      float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
      this.Animator.SetFloat(heightParameterName, shapeBodyValue);
    }

    private void AfterFBBIK()
    {
    }

    private void OnLateUpdate()
    {
      this.Follow();
      if ((double) Vector3.Angle(((Component) this).get_transform().get_up(), Vector3.get_up()) > 0.00999999977648258)
        ;
    }

    private void RotateEffector(IKEffector effector, Quaternion rotation, float mlp)
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(((Transform) effector.bone).get_position(), ((Component) this).get_transform().get_position());
      Vector3 vector3_2 = Vector3.op_Subtraction(Quaternion.op_Multiply(rotation, vector3_1), vector3_1);
      IKEffector ikEffector = effector;
      ikEffector.positionOffset = (__Null) Vector3.op_Addition((Vector3) ikEffector.positionOffset, Vector3.op_Multiply(vector3_2, mlp));
    }

    public void ForceChangeAnime()
    {
      if (!Object.op_Implicit((Object) this.Animator) || !Object.op_Inequality((Object) this.Animator.get_runtimeAnimatorController(), (Object) null))
        return;
      this.Animator.SetFloat(Singleton<Resources>.Instance.DefinePack.AnimatorParameter.ForwardMove, 0.0f);
    }

    public ActorAnimationAgent CloneComponent(GameObject target)
    {
      ActorAnimationAgent actorAnimationAgent = (ActorAnimationAgent) target.AddComponent<ActorAnimationAgent>();
      actorAnimationAgent._character = this._character;
      actorAnimationAgent.Animator = this.Animator;
      actorAnimationAgent.EnabledPoser = this.EnabledPoser;
      actorAnimationAgent.ArmAnimator = this.ArmAnimator;
      actorAnimationAgent.Poser = this.Poser;
      actorAnimationAgent.IsLocomotionState = this.IsLocomotionState;
      actorAnimationAgent._ik = this._ik;
      return actorAnimationAgent;
    }
  }
}
