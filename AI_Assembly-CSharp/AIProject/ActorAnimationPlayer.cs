// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimationPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class ActorAnimationPlayer : ActorAnimationThirdPerson
  {
    [SerializeField]
    private float _turnSensitivity = 0.2f;
    [SerializeField]
    private float _turnSpeed = 5f;
    [SerializeField]
    private float _runCycleLegOffset = 0.2f;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float _animSpeedMultiplier = 1f;
    [SerializeField]
    private FullBodyBipedIK _ik;

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
        ((Behaviour) this._ik).set_enabled(true);
      }
    }

    public float TurnSpeed
    {
      get
      {
        return this._turnSpeed;
      }
    }

    public float RunCycleLegOffset
    {
      get
      {
        return this._runCycleLegOffset;
      }
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this._ik, (Object) null))
      {
        this._ik = (FullBodyBipedIK) ((Component) this).GetComponentInChildren<FullBodyBipedIK>();
        if (Object.op_Implicit((Object) this._ik))
          ((Behaviour) this._ik).set_enabled(true);
      }
      if (Object.op_Equality((Object) this.Animator, (Object) null))
        this.Animator = (Animator) ((Component) this).GetComponentInChildren<Animator>();
      this._lastForward = ((Component) this).get_transform().get_forward();
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

    protected override void LoadMatchTargetInfo(string stateName)
    {
      int hash = Animator.StringToHash(stateName);
      Dictionary<int, List<AnimeMoveInfo>> dictionary;
      List<AnimeMoveInfo> animeMoveInfoList;
      if (!Singleton<Resources>.Instance.Animation.PlayerMoveInfoTable.TryGetValue((int) this.Actor.ChaControl.sex, out dictionary) || !dictionary.TryGetValue(hash, out animeMoveInfoList))
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
      int personality = this.Actor.ID - (int) this.Actor.ChaControl.sex;
      foreach (ActorAnimation.ExpressionKeyframeEvent expressionKeyframe in this.ExpressionKeyframeList)
      {
        if ((double) expressionKeyframe.NormalizedTime < (double) normalizedTime && !expressionKeyframe.Used)
        {
          expressionKeyframe.Used = true;
          Singleton<Game>.Instance.GetExpression(personality, expressionKeyframe.Name)?.Change(this.Actor.ChaControl);
        }
      }
    }

    protected override void LoadStateLocomotionVoice(int stateHashName)
    {
    }

    protected override void LoadStateExpression(int stateHashName)
    {
      Resources.ActionTable action = Singleton<Resources>.Instance.Action;
      this.ExpressionKeyframeList.Clear();
      int num = this.Actor.ID - (int) this.Actor.ChaControl.sex;
      Dictionary<int, List<ExpressionKeyframe>> dictionary1;
      List<ExpressionKeyframe> expressionKeyframeList;
      if (action.ActionExpressionKeyframeTable.TryGetValue(num, out dictionary1) && dictionary1.TryGetValue(stateHashName, out expressionKeyframeList))
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
        if (!action.ActionExpressionTable.TryGetValue(num, out dictionary2) || !dictionary2.TryGetValue(stateHashName, out key))
          return;
        Singleton<Game>.Instance.GetExpression(num, key)?.Change(this.Actor.ChaControl);
      }
    }

    public override void LoadEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> dictionary1;
      Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary2;
      Dictionary<int, List<AnimeEventInfo>> dictionary3;
      if (Singleton<Resources>.Instance.Animation.PlayerItemEventKeyTable.TryGetValue((int) this.Actor.ChaControl.sex, out dictionary1) && dictionary1.TryGetValue(eventID, out dictionary2) && dictionary2.TryGetValue(poseID, out dictionary3))
        this.ItemEventKeyTable = dictionary3;
      this.LoadSEEventKeyTable(eventID, poseID);
      this.LoadParticleEventKeyTable(eventID, poseID);
      this.LoadOnceVoiceEventKeyTable(eventID, poseID);
      this.LoadLoopVoiceEventKeyTable(eventID, poseID);
    }

    public override void LoadSEEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> dictionary1;
      Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>> dictionary2;
      Dictionary<int, List<AnimeSEEventInfo>> dictionary3;
      if (Singleton<Resources>.Instance.Animation.PlayerActSEEventKeyTable.TryGetValue((int) this.Actor.ChaControl.sex, out dictionary1) && dictionary1.TryGetValue(eventID, out dictionary2) && dictionary2.TryGetValue(poseID, out dictionary3))
        this.SEEventKeyTable = dictionary3;
      else
        this.SEEventKeyTable = (Dictionary<int, List<AnimeSEEventInfo>>) null;
    }

    public override void LoadParticleEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> dictionary1;
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary2;
      Dictionary<int, List<AnimeParticleEventInfo>> dictionary3;
      if (Singleton<Resources>.Instance.Animation.PlayerActParticleEventKeyTable.TryGetValue((int) this.Actor.ChaControl.sex, out dictionary1) && dictionary1.TryGetValue(eventID, out dictionary2) && dictionary2.TryGetValue(poseID, out dictionary3))
        this.ParticleEventKeyTable = dictionary3;
      else
        this.ParticleEventKeyTable = (Dictionary<int, List<AnimeParticleEventInfo>>) null;
    }

    public override void LoadOnceVoiceEventKeyTable(int eventID, int poseID)
    {
      this.OnceVoiceEventKeyTable = (Dictionary<int, List<AnimeOnceVoiceEventInfo>>) null;
    }

    public override void LoadLoopVoiceEventKeyTable(int eventID, int poseID)
    {
      this.LoopVoiceEventKeyTable = (Dictionary<int, List<int>>) null;
    }

    protected override void PlayEventOnceVoice(int voiceID)
    {
      if (!Object.op_Equality((Object) (this.Actor as PlayerActor), (Object) null))
        ;
    }

    protected override bool PlayEventLoopVoice()
    {
      return (!base.PlayEventLoopVoice() || !Object.op_Equality((Object) (this.Actor as PlayerActor), (Object) null)) && false;
    }

    protected override void LoadFootStepEventKeyTable()
    {
      PlayerActor actor = this.Actor as PlayerActor;
      if (Object.op_Equality((Object) actor, (Object) null))
      {
        this._footStepEventKeyTable = (Dictionary<int, FootStepInfo[]>) null;
      }
      else
      {
        Dictionary<int, FootStepInfo[]> dictionary;
        Singleton<Resources>.Instance.Animation.PlayerFootStepEventKeyTable.TryGetValue((int) actor.ChaControl.sex, out dictionary);
        this._footStepEventKeyTable = dictionary;
      }
    }

    public override void UpdateState(ActorLocomotion.AnimationState state)
    {
      if (Mathf.Approximately(Time.get_deltaTime(), 0.0f) || Object.op_Equality((Object) this.Animator, (Object) null))
        return;
      AnimatorStateInfo animatorStateInfo = this.Animator.GetCurrentAnimatorStateInfo(0);
      this.IsLocomotionState = ((AnimatorStateInfo) ref animatorStateInfo).IsName("Locomotion") || ((AnimatorStateInfo) ref animatorStateInfo).IsName("Grounded Strafe");
      float num1 = -this.GetAngleFromForward(this._lastForward);
      this._lastForward = ((Component) this).get_transform().get_forward();
      float num2 = num1 * (this._turnSensitivity * 0.01f);
      string forwardMove = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.ForwardMove;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      float num3 = !state.setMediumOnWalk ? Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z) : (state.moveDirection.z < (double) state.medVelocity || Mathf.Approximately((float) state.moveDirection.z, state.medVelocity) ? Mathf.Lerp(0.0f, 0.5f, Mathf.InverseLerp(0.0f, Mathf.InverseLerp(0.0f, state.maxVelocity, state.medVelocity), Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z))) : Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z));
      float num4 = Mathf.Lerp(this.Animator.GetFloat(forwardMove), num3, Time.get_deltaTime() * Singleton<Resources>.Instance.LocomotionProfile.LerpSpeed);
      this.Animator.SetFloat(forwardMove, num4);
      PlayerActor actor = this.Actor as PlayerActor;
      if (Object.op_Inequality((Object) actor, (Object) null) && Object.op_Inequality((Object) actor.Partner, (Object) null) && actor.Partner.IsSlave)
      {
        actor.Partner.Animation.Animator.SetFloat(forwardMove, num4);
        float shapeBodyValue = actor.Partner.ChaControl.GetShapeBodyValue(0);
        this.Animator.SetFloat(heightParameterName, shapeBodyValue);
      }
      this.Animator.set_speed(!state.onGround || state.moveDirection.z <= 0.0 ? 1f : this._animSpeedMultiplier);
    }

    protected override void OnLateUpdate()
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
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      this.Animator.SetFloat(definePack.AnimatorParameter.ForwardMove, 0.0f);
      this.Animator.Play(definePack.AnimatorState.IdleState, 0, 0.0f);
    }

    public ActorAnimationPlayer CloneComponent(GameObject target)
    {
      ActorAnimationPlayer actorAnimationPlayer = (ActorAnimationPlayer) target.AddComponent<ActorAnimationPlayer>();
      actorAnimationPlayer._character = this._character;
      actorAnimationPlayer.Animator = this.Animator;
      actorAnimationPlayer.EnabledPoser = this.EnabledPoser;
      actorAnimationPlayer.ArmAnimator = this.ArmAnimator;
      actorAnimationPlayer.Poser = this.Poser;
      actorAnimationPlayer.IsLocomotionState = this.IsLocomotionState;
      actorAnimationPlayer._ik = this._ik;
      actorAnimationPlayer._turnSensitivity = this._turnSensitivity;
      return actorAnimationPlayer;
    }

    public override void PlayAnimation(string stateName, int layer, float normalizedTime)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      if (!this._parameters.IsNullOrEmpty<AnimatorControllerParameter>())
      {
        foreach (AnimatorControllerParameter parameter in this._parameters)
        {
          if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
          {
            float num = 0.75f;
            if (Object.op_Inequality((Object) animator, (Object) null))
              animator.SetFloat(heightParameterName, num);
          }
        }
      }
      this.PlayAnimation(animator, stateName, layer, normalizedTime);
    }

    public override void PlayAnimation(int stateNameHash, int layer, float normalizedTime)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      if (!this._parameters.IsNullOrEmpty<AnimatorControllerParameter>())
      {
        foreach (AnimatorControllerParameter parameter in this._parameters)
        {
          if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
          {
            float num = 0.75f;
            if (Object.op_Inequality((Object) animator, (Object) null))
              animator.SetFloat(heightParameterName, num);
          }
        }
      }
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.Play(stateNameHash, layer, normalizedTime);
    }

    public override void CrossFadeAnimation(
      string stateName,
      float fadeTime,
      int layer,
      float fixedTimeOffset)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      foreach (AnimatorControllerParameter parameter in this._parameters)
      {
        if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
        {
          float num = 0.75f;
          if (Object.op_Inequality((Object) animator, (Object) null))
            animator.SetFloat(heightParameterName, num);
        }
      }
      this.CrossFadeAnimation(animator, stateName, fadeTime, layer, fixedTimeOffset);
    }

    public override void CrossFadeAnimation(
      int stateNameHash,
      float fadeTime,
      int layer,
      float fixedTimeOffset)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      foreach (AnimatorControllerParameter parameter in this._parameters)
      {
        if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
        {
          float num = 0.75f;
          if (Object.op_Inequality((Object) animator, (Object) null))
            animator.SetFloat(heightParameterName, num);
        }
      }
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.CrossFadeInFixedTime(stateNameHash, fadeTime, layer, fixedTimeOffset, 0.0f);
    }

    protected override void PlayItemAnimation(string stateName)
    {
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      foreach (KeyValuePair<int, ItemAnimInfo> keyValuePair in this.ItemAnimatorTable)
      {
        Animator animator = keyValuePair.Value.Animator;
        if (keyValuePair.Value.Sync)
        {
          foreach (AnimatorControllerParameter parameter in keyValuePair.Value.Parameters)
          {
            if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
            {
              float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
              if (Object.op_Inequality((Object) animator, (Object) null))
                animator.SetFloat(heightParameterName, shapeBodyValue);
            }
          }
          this.PlayAnimation(animator, stateName, 0, 0.0f);
        }
      }
    }

    protected override void CrossFadeItemAnimation(string stateName, float fadeTime, int layer)
    {
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      foreach (KeyValuePair<int, ItemAnimInfo> keyValuePair in this.ItemAnimatorTable)
      {
        Animator animator = keyValuePair.Value.Animator;
        if (keyValuePair.Value.Sync)
        {
          foreach (AnimatorControllerParameter parameter in keyValuePair.Value.Parameters)
          {
            if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
            {
              float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
              if (Object.op_Inequality((Object) animator, (Object) null))
                animator.SetFloat(heightParameterName, shapeBodyValue);
            }
          }
          this.CrossFadeAnimation(animator, stateName, fadeTime, layer, 0.0f);
        }
      }
    }
  }
}
