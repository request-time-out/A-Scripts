// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimationMerchant
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

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
  public class ActorAnimationMerchant : ActorAnimation
  {
    [SerializeField]
    private FullBodyBipedIK ik;

    public FullBodyBipedIK IK
    {
      get
      {
        return this.ik;
      }
      set
      {
        this.ik = value;
        if (!Object.op_Inequality((Object) this.ik, (Object) null))
          return;
        __Null solver = this.ik.solver;
        ((IKSolver) solver).OnPostUpdate = (__Null) Delegate.Combine((Delegate) ((IKSolver) solver).OnPostUpdate, (Delegate) new IKSolver.UpdateDelegate((object) this, __methodptr(AfterFBBIK)));
        ((Behaviour) this.ik).set_enabled(true);
      }
    }

    public override Vector3 GetPivotPoint()
    {
      return ((Component) this).get_transform().get_position();
    }

    protected override void Start()
    {
      base.Start();
      if (Object.op_Equality((Object) this.ik, (Object) null))
      {
        this.ik = (FullBodyBipedIK) ((Component) this).GetComponentInChildren<FullBodyBipedIK>(true);
        if (Object.op_Inequality((Object) this.ik, (Object) null))
        {
          ((Behaviour) this.ik).set_enabled(true);
          // ISSUE: variable of the null type
          __Null solver = this.ik.solver;
          // ISSUE: method pointer
          ((IKSolver) solver).OnPostUpdate = (__Null) Delegate.Combine((Delegate) ((IKSolver) solver).OnPostUpdate, (Delegate) new IKSolver.UpdateDelegate((object) this, __methodptr(AfterFBBIK)));
        }
      }
      if (Object.op_Equality((Object) this.Animator, (Object) null))
        this.Animator = (Animator) ((Component) this).GetComponentInChildren<Animator>(true);
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
    }

    protected void OnAnimatorMove()
    {
      Transform transform1 = ((Component) this.Character).get_transform();
      transform1.set_localPosition(Vector3.op_Addition(transform1.get_localPosition(), this.Animator.get_deltaPosition()));
      Transform transform2 = ((Component) this.Character).get_transform();
      transform2.set_localRotation(Quaternion.op_Multiply(transform2.get_localRotation(), this.Animator.get_deltaRotation()));
    }

    protected override void LoadMatchTargetInfo(string _stateName)
    {
      List<AnimeMoveInfo> animeMoveInfoList;
      if (!Singleton<Resources>.Instance.Animation.MerchantMoveInfoTable.TryGetValue(Animator.StringToHash(_stateName), out animeMoveInfoList))
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

    private void AfterFBBIK()
    {
    }

    public override void LoadEventKeyTable(int eventID, int poseID)
    {
      this.LoadActionEventKeyTable(eventID, poseID);
    }

    public void LoadActionEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> itemEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantCommonItemEventKeyTable;
      if (!itemEventKeyTable.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>())
        this.LoadEventKeyTable(eventID, poseID, itemEventKeyTable);
      this.LoadActionSEEventKeyTable(eventID, poseID);
      this.LoadActionParticleEventKeyTable(eventID, poseID);
      this.LoadActionOnceVoiceEventKeyTable(eventID, poseID);
      this.LoadActionLoopVoiceEventKeyTable(eventID, poseID);
    }

    public void LoadMerchantEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> itemEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantOnlyItemEventKeyTable;
      if (!itemEventKeyTable.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>>())
        this.LoadEventKeyTable(eventID, poseID, itemEventKeyTable);
      this.LoadMerchantSEEventKeyTable(eventID, poseID);
      this.LoadMerchantParticleEventKeyTable(eventID, poseID);
      this.LoadMerchantOnceVoiceEventKeyTable(eventID, poseID);
      this.LoadMerchantLoopVoiceEventKeyTable(eventID, poseID);
    }

    private void LoadEventKeyTable(
      int eventID,
      int poseID,
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeEventInfo>>>> tableGroup)
    {
      Dictionary<int, Dictionary<int, List<AnimeEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeEventInfo>> dictionary2;
      if (tableGroup.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.ItemEventKeyTable = dictionary2;
      else
        this.ItemEventKeyTable = (Dictionary<int, List<AnimeEventInfo>>) null;
    }

    public override void LoadSEEventKeyTable(int eventID, int poseID)
    {
      this.LoadActionSEEventKeyTable(eventID, poseID);
    }

    public void LoadActionSEEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> commonSeEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantCommonSEEventKeyTable;
      this.LoadSEEventKeyTable(eventID, poseID, commonSeEventKeyTable);
    }

    public void LoadMerchantSEEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> onlySeEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantOnlySEEventKeyTable;
      this.LoadSEEventKeyTable(eventID, poseID, onlySeEventKeyTable);
    }

    private void LoadSEEventKeyTable(
      int eventID,
      int poseID,
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>> tableGroup)
    {
      Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeSEEventInfo>> dictionary2;
      if (!tableGroup.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<AnimeSEEventInfo>>>>() && tableGroup.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.SEEventKeyTable = dictionary2;
      else
        this.SEEventKeyTable = (Dictionary<int, List<AnimeSEEventInfo>>) null;
    }

    public override void LoadParticleEventKeyTable(int eventID, int poseID)
    {
      this.LoadActionParticleEventKeyTable(eventID, poseID);
    }

    public void LoadActionParticleEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> particleEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantCommonParticleEventKeyTable;
      this.LoadParticleEventKeyTable(eventID, poseID, particleEventKeyTable);
    }

    public void LoadMerchantParticleEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> particleEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantOnlyParticleEventKeyTable;
      this.LoadParticleEventKeyTable(eventID, poseID, particleEventKeyTable);
    }

    private void LoadParticleEventKeyTable(
      int eventID,
      int poseID,
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>> tableGroup)
    {
      Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeParticleEventInfo>> dictionary2;
      if (!tableGroup.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<AnimeParticleEventInfo>>>>() && tableGroup.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.ParticleEventKeyTable = dictionary2;
      else
        this.ParticleEventKeyTable = (Dictionary<int, List<AnimeParticleEventInfo>>) null;
    }

    public override void LoadOnceVoiceEventKeyTable(int eventID, int poseID)
    {
      this.LoadActionOnceVoiceEventKeyTable(eventID, poseID);
    }

    public void LoadActionOnceVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> voiceEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantCommonOnceVoiceEventKeyTable;
      this.LoadOnceVoiceEventKeyTable(eventID, poseID, voiceEventKeyTable);
    }

    public void LoadMerchantOnceVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> voiceEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantOnlyOnceVoiceEventKeyTable;
      this.LoadOnceVoiceEventKeyTable(eventID, poseID, voiceEventKeyTable);
    }

    private void LoadOnceVoiceEventKeyTable(
      int eventID,
      int poseID,
      Dictionary<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>> tableGroup)
    {
      Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>> dictionary1;
      Dictionary<int, List<AnimeOnceVoiceEventInfo>> dictionary2;
      if (!tableGroup.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<AnimeOnceVoiceEventInfo>>>>() && tableGroup.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.OnceVoiceEventKeyTable = dictionary2;
      else
        this.OnceVoiceEventKeyTable = (Dictionary<int, List<AnimeOnceVoiceEventInfo>>) null;
    }

    public override void LoadLoopVoiceEventKeyTable(int eventID, int poseID)
    {
      this.LoadActionLoopVoiceEventKeyTable(eventID, poseID);
    }

    public void LoadActionLoopVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> voiceEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantCommonLoopVoiceEventKeyTable;
      this.LoadLoopVoiceEventKeyTable(eventID, poseID, voiceEventKeyTable);
    }

    public void LoadMerchantLoopVoiceEventKeyTable(int eventID, int poseID)
    {
      Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> voiceEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantOnlyLoopVoiceEventKeyTable;
      this.LoadLoopVoiceEventKeyTable(eventID, poseID, voiceEventKeyTable);
    }

    private void LoadLoopVoiceEventKeyTable(
      int eventID,
      int poseID,
      Dictionary<int, Dictionary<int, Dictionary<int, List<int>>>> tableGroup)
    {
      Dictionary<int, Dictionary<int, List<int>>> dictionary1;
      Dictionary<int, List<int>> dictionary2;
      if (!tableGroup.IsNullOrEmpty<int, Dictionary<int, Dictionary<int, List<int>>>>() && tableGroup.TryGetValue(eventID, out dictionary1) && dictionary1.TryGetValue(poseID, out dictionary2))
        this.LoopVoiceEventKeyTable = dictionary2;
      else
        this.LoopVoiceEventKeyTable = (Dictionary<int, List<int>>) null;
    }

    protected override void PlayEventOnceVoice(int voiceID)
    {
      AssetBundleInfo info;
      if (!Singleton<Resources>.Instance.Sound.TryGetMapActionVoiceInfo(-90, voiceID, out info))
        return;
      Transform transform1 = ((Component) this.Actor.Locomotor).get_transform();
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int num = -90;
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
      int stateLoopVoiceEvent = this.StateLoopVoiceEvents[Random.Range(0, this.StateLoopVoiceEvents.Count)];
      if (this._loopActionVoice.Item1 == stateLoopVoiceEvent && Object.op_Inequality((Object) this._loopActionVoice.Item2, (Object) null))
        return false;
      int personalID = -90;
      AssetBundleInfo info;
      if (!Singleton<Resources>.Instance.Sound.TryGetMapActionVoiceInfo(personalID, stateLoopVoiceEvent, out info))
        return false;
      Transform transform1 = ((Component) this.Actor.Locomotor).get_transform();
      Manager.Voice instance = Singleton<Manager.Voice>.Instance;
      int num = personalID;
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
      this._loopActionVoice.Item4 = (__Null) personalID;
      return Object.op_Inequality((Object) this._loopActionVoice.Item2, (Object) null);
    }

    protected override void LoadFootStepEventKeyTable()
    {
      this._footStepEventKeyTable = Singleton<Resources>.Instance.Animation.MerchantFootStepEventKeyTable;
    }

    public void UpdateState(ActorLocomotion.AnimationState state)
    {
      if (Mathf.Approximately(Time.get_deltaTime(), 0.0f) || Object.op_Equality((Object) this.Animator, (Object) null) || !((Behaviour) this.Animator).get_isActiveAndEnabled())
        return;
      DefinePack definePack = Singleton<Resources>.Instance.DefinePack;
      string directionParameterName = definePack.AnimatorParameter.DirectionParameterName;
      string forwardMove = definePack.AnimatorParameter.ForwardMove;
      string heightParameterName = definePack.AnimatorParameter.HeightParameterName;
      float num1 = !state.setMediumOnWalk ? Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z) : (state.moveDirection.z <= (double) state.medVelocity || Mathf.Approximately((float) state.moveDirection.z, state.medVelocity) ? Mathf.Lerp(0.0f, 0.5f, Mathf.InverseLerp(0.0f, Mathf.InverseLerp(0.0f, state.maxVelocity, state.medVelocity), Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z))) : Mathf.InverseLerp(0.0f, state.maxVelocity, (float) state.moveDirection.z));
      foreach (AnimatorControllerParameter parameter in this.Animator.get_parameters())
      {
        if (!this.Actor.IsSlave && parameter.get_name() == forwardMove && parameter.get_type() == 1)
        {
          float num2 = Mathf.Lerp(this.Animator.GetFloat(forwardMove), num1, Time.get_deltaTime() * Singleton<Resources>.Instance.LocomotionProfile.LerpSpeed);
          this.Animator.SetFloat(forwardMove, num2);
        }
        if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
        {
          float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
          this.Animator.SetFloat(heightParameterName, shapeBodyValue);
        }
      }
    }

    private void OnLateUpdate()
    {
      this.Follow();
      if (!Mathf.Approximately(Vector3.Angle(((Component) this).get_transform().get_up(), Vector3.get_up()), 0.0f))
        ;
    }

    public ActorAnimationMerchant CloneComponent(GameObject target)
    {
      ActorAnimationMerchant animationMerchant = (ActorAnimationMerchant) target.AddComponent<ActorAnimationMerchant>();
      animationMerchant._character = this._character;
      animationMerchant.Animator = this.Animator;
      animationMerchant.EnabledPoser = this.EnabledPoser;
      animationMerchant.ArmAnimator = this.ArmAnimator;
      animationMerchant.Poser = this.Poser;
      animationMerchant.IsLocomotionState = this.IsLocomotionState;
      animationMerchant.ik = this.ik;
      return animationMerchant;
    }
  }
}
