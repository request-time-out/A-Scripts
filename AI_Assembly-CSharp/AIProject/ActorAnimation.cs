// Decompiled with JetBrains decompiler
// Type: AIProject.ActorAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using IllusionUtility.GetUtility;
using Manager;
using Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public abstract class ActorAnimation : MonoBehaviour
  {
    private static readonly bool[] _defaultYure = new bool[7]
    {
      true,
      true,
      true,
      true,
      true,
      true,
      true
    };
    protected static RaycastHit[] _raycastHits = new RaycastHit[3];
    [SerializeField]
    protected ActorLocomotion _character;
    public Animator Animator;
    private RuntimeAnimatorController _defaultAnimController;
    private PlayState.Info _maskState;
    protected ValueTuple<int, AudioSource, Transform, int> _loopActionVoice;
    protected MotionIK _motionIK;
    protected string _preAnimatorName;
    protected AnimatorControllerParameter[] _parameters;
    protected List<ProceduralTargetParameter> _targets;
    private float _prevNormalizedTime;
    private int _loopCount;
    private bool _ignoreAnimEvent;
    private bool _ignoreExpressionEvent;
    private bool _ignoreVoiceEvent;
    private const bool _defaultNip = true;
    private YureCtrl.BreastShapeInfo[] _shapeInfo;
    private string _animatorName;
    private int _nameHash;
    private bool _isFootStepActive;
    private FootStepInfo _prevFootStepInfo;
    private List<FootStepInfo> _footStepEvents;
    protected Dictionary<int, FootStepInfo[]> _footStepEventKeyTable;
    private List<AudioSource> _footStepAudioSources;
    private Queue<PlayState.Info> _recoveryStateFromTurn;
    private IEnumerator _inLocoAnimEnumerator;
    private IDisposable _inLocoAnimDisposable;
    private IEnumerator _inAnimEnumerator;
    private IDisposable _inAnimDisposable;
    private IEnumerator _outAnimEnumerator;
    private IDisposable _outAnimDisposable;
    private IEnumerator _actionAnimEnumerator;
    private IDisposable _actionAnimDisposable;
    private IEnumerator _turnAnimEnumerator;
    private IDisposable _turnAnimDisposable;
    private IEnumerator _onceActionAnimEnumerator;
    private IDisposable _onceActionAnimDisposable;

    protected ActorAnimation()
    {
      base.\u002Ector();
    }

    public Actor Actor { get; set; }

    public ActorLocomotion Character
    {
      get
      {
        return this._character;
      }
      set
      {
        this._character = value;
      }
    }

    public bool EnabledPoser { get; set; }

    public Animator ArmAnimator { get; set; }

    public JointPoser Poser { get; protected set; }

    public abstract Vector3 GetPivotPoint();

    public bool IsLocomotionState { get; protected set; }

    public AssetBundleInfo AnimABInfo { get; set; }

    public Queue<PlayState.Info> InStates { get; }

    public Queue<PlayState.Info> OutStates { get; }

    public List<PlayState.PlayStateInfo> ActionStates { get; }

    public Queue<PlayState.Info> ActionInStates { get; }

    public Queue<PlayState.Info> ActionOutStates { get; }

    public List<PlayState.Info> OnceActionStates { get; }

    public ActionPointInfo ActionPointInfo { get; set; }

    public DateActionPointInfo DateActionPointInfo { get; set; }

    public bool RefsActAnimInfo { get; set; }

    public ActorAnimInfo AnimInfo { get; set; }

    public Transform RecoveryPoint { get; set; }

    public AudioSource OnceActionVoice { get; protected set; }

    public ValueTuple<int, AudioSource, Transform, int> LoopActionVoice
    {
      get
      {
        return this._loopActionVoice;
      }
    }

    public MotionIK MapIK
    {
      get
      {
        return this._motionIK;
      }
    }

    public bool CanUseMapIK { get; set; }

    public float GetAngleFromForward(Vector3 worldDirection)
    {
      Vector3 vector3 = ((Component) this).get_transform().InverseTransformDirection(worldDirection);
      return Mathf.Atan2((float) vector3.x, (float) vector3.z) * 57.29578f;
    }

    public AnimatorControllerParameter[] Parameters
    {
      get
      {
        return this._parameters;
      }
    }

    public void SetDefaultAnimatorController(RuntimeAnimatorController rac)
    {
      this._defaultAnimController = rac;
    }

    public void SetAnimatorController(RuntimeAnimatorController rac)
    {
      this.Animator.set_runtimeAnimatorController(rac);
      this._parameters = this.Animator.get_parameters();
      if (this._motionIK == null)
      {
        this._motionIK = new MotionIK(this.Actor.ChaControl, false, (MotionIKData) null);
        this._motionIK.MapIK = true;
      }
      if (this._ignoreAnimEvent)
        return;
      this._motionIK.SetMapIK(((Object) this.Animator.get_runtimeAnimatorController()).get_name());
      this._nameHash = 0;
    }

    public void ResetDefaultAnimatorController()
    {
      this.SetAnimatorController(this._defaultAnimController);
    }

    protected virtual void Start()
    {
      if (Object.op_Inequality((Object) this._character, (Object) null))
      {
        ((Component) this).get_transform().set_position(((Component) this._character).get_transform().get_position());
        ((Component) this).get_transform().set_rotation(((Component) this._character).get_transform().get_rotation());
      }
      for (int index = 0; index < this._shapeInfo.Length; ++index)
        this._shapeInfo[index].MemberInit();
      ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.OnAnimatorMoveAsObservable((Component) this), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => {}));
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ =>
      {
        this.OnUpdate();
        this.Follow();
      }));
    }

    protected virtual void OnDestroy()
    {
      this.StopAllAnimCoroutine();
      using (List<AudioSource>.Enumerator enumerator = this._footStepAudioSources.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          AudioSource current = enumerator.Current;
          if (!Object.op_Equality((Object) current, (Object) null) && !Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null) && current.get_isPlaying())
            current.Stop();
        }
      }
      this.StopLoopActionVoice();
      this.StopAllActionLoopSE();
      if (!Object.op_Inequality((Object) this.OnceActionVoice, (Object) null) || !Object.op_Inequality((Object) ((Component) this.OnceActionVoice).get_gameObject(), (Object) null))
        return;
      Actor actor = this.Actor;
      ActorLocomotion actorLocomotion = !Object.op_Inequality((Object) actor, (Object) null) ? (ActorLocomotion) null : actor.Locomotor;
      Transform voiceTrans = !Object.op_Inequality((Object) actorLocomotion, (Object) null) ? (Transform) null : ((Component) actorLocomotion).get_transform();
      if (Object.op_Inequality((Object) voiceTrans, (Object) null) && Singleton<Manager.Voice>.IsInstance())
        Singleton<Manager.Voice>.Instance.Stop(voiceTrans);
      else
        Object.Destroy((Object) ((Component) this.OnceActionVoice).get_gameObject());
    }

    public string CurrentStateName { get; set; }

    public List<ProceduralTargetParameter> Targets
    {
      get
      {
        return this._targets;
      }
    }

    protected void OnUpdate()
    {
      if (this._targets != null)
      {
        AnimatorStateInfo animatorStateInfo = this.Animator.GetCurrentAnimatorStateInfo(0);
        bool flag1 = ((AnimatorStateInfo) ref animatorStateInfo).IsName(this.CurrentStateName);
        bool flag2 = this.Animator.IsInTransition(0);
        if (flag1 && (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() < 0.899999976158142 && !flag2)
        {
          MatchTargetWeightMask targetWeightMask;
          ((MatchTargetWeightMask) ref targetWeightMask).\u002Ector(Vector3.get_one(), 0.0f);
          foreach (ProceduralTargetParameter target in this._targets)
          {
            if (Object.op_Inequality((Object) target.Target, (Object) null))
              this.Animator.MatchTarget(target.Target.get_position(), Quaternion.get_identity(), (AvatarTarget) 0, targetWeightMask, target.Start, target.End);
          }
        }
      }
      this.UpdateAnimationEvent();
    }

    protected void Follow()
    {
      ((Component) this).get_transform().set_position(((Component) this._character).get_transform().get_position());
      ((Component) this).get_transform().set_rotation(((Component) this._character).get_transform().get_rotation());
    }

    public float GetFloat(string parameterName)
    {
      return this.Animator.GetFloat(parameterName);
    }

    public float GetFloat(int parameterHash)
    {
      return this.Animator.GetFloat(parameterHash);
    }

    public void SetFloat(string parameterName, float value)
    {
      this.Animator.SetFloat(parameterName, value);
    }

    public void SetFloat(int parameterHash, float value)
    {
      this.Animator.SetFloat(parameterHash, value);
    }

    public Dictionary<int, List<AnimeEventInfo>> ItemEventKeyTable { get; set; }

    public Dictionary<int, List<AnimeEventInfo>> PartnerEventKeyTable { get; set; }

    public Dictionary<int, List<AnimeSEEventInfo>> SEEventKeyTable { get; set; }

    public Dictionary<int, List<AnimeParticleEventInfo>> ParticleEventKeyTable { get; set; }

    public Dictionary<int, List<AnimeOnceVoiceEventInfo>> OnceVoiceEventKeyTable { get; set; }

    public Dictionary<int, List<int>> LoopVoiceEventKeyTable { get; set; }

    public Dictionary<int, List<AnimeEventInfo>> ClothEventKeyTable { get; set; }

    private void LoadStateEvents(
      Dictionary<int, List<AnimeEventInfo>> table,
      List<ActorAnimation.AnimatorStateEvent> list)
    {
      List<AnimeEventInfo> source;
      if (table == null || !table.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<AnimeEventInfo>())
        return;
      foreach (AnimeEventInfo animeEventInfo in source)
        list.Add(new ActorAnimation.AnimatorStateEvent()
        {
          NormalizedTime = animeEventInfo.normalizedTime,
          EventID = animeEventInfo.eventID
        });
    }

    private void LoadStateSEEvents(
      Dictionary<int, List<AnimeSEEventInfo>> table,
      List<ActorAnimation.AnimatorStateEvent_SE> list)
    {
      List<AnimeSEEventInfo> source;
      if (table.IsNullOrEmpty<int, List<AnimeSEEventInfo>>() || !table.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<AnimeSEEventInfo>())
        return;
      foreach (AnimeSEEventInfo animeSeEventInfo in source)
        list.Add(new ActorAnimation.AnimatorStateEvent_SE()
        {
          NormalizedTime = animeSeEventInfo.NormalizedTime,
          ClipID = animeSeEventInfo.ClipID,
          EventID = animeSeEventInfo.EventID,
          Root = ((Component) this.Actor.ChaControl).get_transform().FindLoop(animeSeEventInfo.Root)?.get_transform() ?? ((Component) this.Actor.Locomotor).get_transform()
        });
    }

    private void LoadStateParticleEvents(
      Dictionary<int, List<AnimeParticleEventInfo>> table,
      List<ActorAnimation.AnimatorStateEvent_Particle> list)
    {
      List<AnimeParticleEventInfo> source;
      if (table.IsNullOrEmpty<int, List<AnimeParticleEventInfo>>() || !table.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<AnimeParticleEventInfo>())
        return;
      foreach (AnimeParticleEventInfo particleEventInfo in source)
        list.Add(new ActorAnimation.AnimatorStateEvent_Particle()
        {
          NormalizedTime = particleEventInfo.NormalizedTime,
          ParticleID = particleEventInfo.ParticleID,
          EventID = particleEventInfo.EventID,
          Root = particleEventInfo.Root,
          Used = false
        });
    }

    private void LoadStateOnceVoiceEvents(
      Dictionary<int, List<AnimeOnceVoiceEventInfo>> table,
      List<ActorAnimation.AnimatorStateEvent_OnceVoice> list)
    {
      List<AnimeOnceVoiceEventInfo> source;
      if (table.IsNullOrEmpty<int, List<AnimeOnceVoiceEventInfo>>() || !table.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<AnimeOnceVoiceEventInfo>())
        return;
      foreach (AnimeOnceVoiceEventInfo onceVoiceEventInfo in source)
        list.Add(new ActorAnimation.AnimatorStateEvent_OnceVoice()
        {
          NormalizedTime = onceVoiceEventInfo.NormalizedTime,
          IDs = onceVoiceEventInfo.EventIDs,
          Used = false
        });
    }

    private void LoadStateLoopVoiceEvents(Dictionary<int, List<int>> table, List<int> list)
    {
      List<int> source;
      if (table.IsNullOrEmpty<int, List<int>>() || !table.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<int>())
        return;
      list.AddRange((IEnumerable<int>) source);
    }

    public void BeginIgnoreEvent()
    {
      this._ignoreAnimEvent = true;
    }

    public void EndIgnoreEvent()
    {
      this._ignoreAnimEvent = false;
    }

    public void BeginIgnoreExpression()
    {
      this._ignoreExpressionEvent = true;
    }

    public void EndIgnoreExpression()
    {
      this._ignoreExpressionEvent = false;
    }

    public void BeginIgnoreVoice()
    {
      this._ignoreVoiceEvent = true;
      this.StopLoopActionVoice();
    }

    public void EndIgnoreVoice()
    {
      this._ignoreVoiceEvent = false;
      if (this.PlayEventLoopVoice())
        return;
      this.StopLoopActionVoice();
    }

    public void UpdateAnimationEvent()
    {
      if (Object.op_Equality((Object) this.Animator, (Object) null) || this._ignoreAnimEvent)
        return;
      if (Object.op_Inequality((Object) this.Animator.get_runtimeAnimatorController(), (Object) null))
      {
        string name = ((Object) this.Animator.get_runtimeAnimatorController()).get_name();
        if (this._animatorName != name)
        {
          this._animatorName = name;
          this.LoadAnimatorYureInfo(name);
        }
      }
      AnimatorStateInfo animatorStateInfo = this.Animator.GetCurrentAnimatorStateInfo(0);
      if (this._nameHash != ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash())
      {
        this._nameHash = ((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash();
        this._prevNormalizedTime = 0.0f;
        this._loopCount = 0;
        this.StateItemEvents.Clear();
        this.LoadStateEvents(this.ItemEventKeyTable, this.StateItemEvents);
        this.StatePartnerEvents.Clear();
        this.LoadStateEvents(this.PartnerEventKeyTable, this.StatePartnerEvents);
        this.StateSEEvents.Clear();
        this.StopAllActionLoopSE();
        this.LoadStateSEEvents(this.SEEventKeyTable, this.StateSEEvents);
        this.LoadFootStepEventKeyTable();
        this.LoadFootStepEvents();
        this.StateParticleEvents.Clear();
        this.LoadStateParticleEvents(this.ParticleEventKeyTable, this.StateParticleEvents);
        this.StateOnceVoiceEvents.Clear();
        this.LoadStateOnceVoiceEvents(this.OnceVoiceEventKeyTable, this.StateOnceVoiceEvents);
        if (!this._ignoreExpressionEvent)
          this.LoadStateExpression(this._nameHash);
        this.LoadYureInfo(this._nameHash);
        this.StateLoopVoiceEvents.Clear();
        this.LoadStateLoopVoiceEvents(this.LoopVoiceEventKeyTable, this.StateLoopVoiceEvents);
        if (!this.PlayEventLoopVoice())
          this.StopLoopActionVoice();
        this.StateClothEvents.Clear();
        this.LoadStateEvents(this.ClothEventKeyTable, this.StateClothEvents);
        if (this._motionIK != null && this.CanUseMapIK)
          this._motionIK.Calc(this._nameHash);
      }
      float normalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
      bool loop = ((AnimatorStateInfo) ref animatorStateInfo).get_loop();
      int num = (int) normalizedTime;
      if (loop)
        normalizedTime = Mathf.Repeat(normalizedTime, 1f);
      bool isLoop = loop && (this._loopCount < num || (double) normalizedTime < (double) this._prevNormalizedTime);
      this.UpdateItemEvent(normalizedTime, isLoop);
      if (!this._ignoreExpressionEvent)
        this.UpdateExpressionEvent(normalizedTime, isLoop);
      this.UpdatePartnerEvent(normalizedTime, isLoop);
      this.UpdateSEEvent(normalizedTime, isLoop);
      this.UpdateFootStepEvent(animatorStateInfo, normalizedTime, isLoop);
      this.UpdateParticleEvent(normalizedTime, isLoop);
      if (!this._ignoreVoiceEvent)
        this.UpdateVoiceEvent(normalizedTime, isLoop);
      this.UpdateClothEvent(normalizedTime, isLoop);
      this._prevNormalizedTime = normalizedTime;
      this._loopCount = num;
      if (this._motionIK == null)
      {
        this._motionIK = new MotionIK(this.Actor.ChaControl, false, (MotionIKData) null);
        this._motionIK.MapIK = true;
      }
      else
      {
        if (Object.op_Inequality((Object) this.Animator.get_runtimeAnimatorController(), (Object) null) && this._preAnimatorName != ((Object) this.Animator.get_runtimeAnimatorController()).get_name())
        {
          this._preAnimatorName = ((Object) this.Animator.get_runtimeAnimatorController()).get_name();
          this._motionIK.SetMapIK(((Object) this.Animator.get_runtimeAnimatorController()).get_name());
        }
        this._motionIK.ChangeWeight(this._nameHash, animatorStateInfo);
      }
    }

    public Dictionary<int, YureCtrl.Info> YureTable { get; set; }

    private void LoadAnimatorYureInfo(string animatorName)
    {
      Dictionary<int, YureCtrl.Info> dictionary;
      if (Singleton<Resources>.Instance.Action.ActionYureTable.TryGetValue(animatorName, out dictionary))
        this.YureTable = dictionary;
      else
        this.ResetYure(this.Actor.ChaControl);
    }

    private void LoadYureInfo(int stateHashName)
    {
      ChaControl chaControl = this.Actor.ChaControl;
      if (this.YureTable != null)
      {
        YureCtrl.Info info;
        if (!this.YureTable.TryGetValue(stateHashName, out info))
          return;
        bool[] aIsActive = info.aIsActive;
        for (int area = 0; area < aIsActive.Length; ++area)
          chaControl.playDynamicBoneBust(area, aIsActive[area]);
        YureCtrl.BreastShapeInfo[] aBreastShape = info.aBreastShape;
        for (int index = 0; index < 2; ++index)
        {
          YureCtrl.BreastShapeInfo breastShapeInfo1 = aBreastShape[index];
          YureCtrl.BreastShapeInfo breastShapeInfo2 = this._shapeInfo[index];
          for (int id = 0; id < breastShapeInfo1.breast.Length; ++id)
          {
            bool flag1 = breastShapeInfo1.breast[id];
            bool flag2 = breastShapeInfo2.breast[id];
            if (flag1 != flag2)
            {
              if (flag1)
                chaControl.DisableShapeBodyID(index != 0 ? 1 : 0, id, false);
              else
                chaControl.DisableShapeBodyID(index != 0 ? 1 : 0, id, true);
            }
            this._shapeInfo[index].breast[id] = flag1;
          }
          if (breastShapeInfo1.nip != breastShapeInfo2.nip)
          {
            if (breastShapeInfo1.nip)
              chaControl.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
            else
              chaControl.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
            this._shapeInfo[index].nip = breastShapeInfo1.nip;
          }
        }
      }
      else
        this.ResetYure(chaControl);
    }

    private void ResetYure(ChaControl chara)
    {
      for (int area = 0; area < 4; ++area)
        chara.playDynamicBoneBust(area, true);
      for (int index = 0; index < 2; ++index)
      {
        YureCtrl.BreastShapeInfo breastShapeInfo = this._shapeInfo[index];
        for (int id = 0; id < ActorAnimation._defaultYure.Length; ++id)
        {
          bool flag1 = ActorAnimation._defaultYure[id];
          bool flag2 = breastShapeInfo.breast[id];
          if (flag1 != flag2)
          {
            if (flag1)
              chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, false);
            else
              chara.DisableShapeBodyID(index != 0 ? 1 : 0, id, true);
          }
          this._shapeInfo[index].breast[id] = flag1;
        }
        if (!breastShapeInfo.nip)
        {
          chara.DisableShapeBodyID(index != 0 ? 1 : 0, 7, false);
          this._shapeInfo[index].nip = true;
        }
      }
    }

    private void UpdateItemEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent stateItemEvent in this.StateItemEvents)
          stateItemEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent stateItemEvent in this.StateItemEvents)
        {
          if ((double) stateItemEvent.NormalizedTime > (double) normalizedTime && stateItemEvent.Used)
            stateItemEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent stateItemEvent in this.StateItemEvents)
      {
        if ((double) stateItemEvent.NormalizedTime < (double) normalizedTime && !stateItemEvent.Used)
        {
          stateItemEvent.Used = true;
          switch (stateItemEvent.EventID)
          {
            case 0:
              this.SetEnableItemRenderers((Renderer[]) this.ItemRenderers.GetElement<ValueTuple<int, Renderer[]>>(0).Item2, true);
              continue;
            case 1:
              this.SetEnableItemRenderers((Renderer[]) this.ItemRenderers.GetElement<ValueTuple<int, Renderer[]>>(0).Item2, false);
              continue;
            case 2:
              this.SetEnableItemRenderers((Renderer[]) this.ItemRenderers.GetElement<ValueTuple<int, Renderer[]>>(1).Item2, true);
              continue;
            case 3:
              this.SetEnableItemRenderers((Renderer[]) this.ItemRenderers.GetElement<ValueTuple<int, Renderer[]>>(1).Item2, false);
              continue;
            default:
              continue;
          }
        }
      }
    }

    private void SetEnableItemRenderers(Renderer[] renderers, bool enable)
    {
      if (renderers.IsNullOrEmpty<Renderer>())
        return;
      foreach (Renderer renderer in renderers)
      {
        if (!Object.op_Equality((Object) renderer, (Object) null) && renderer.get_enabled() != enable)
          renderer.set_enabled(enable);
      }
    }

    private void UpdateClothEvent(float normalizedTime, bool isLoop)
    {
      AgentActor actor = this.Actor as AgentActor;
      if (Object.op_Equality((Object) actor, (Object) null))
        return;
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent stateClothEvent in this.StateClothEvents)
          stateClothEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent stateClothEvent in this.StateClothEvents)
        {
          if ((double) stateClothEvent.NormalizedTime > (double) normalizedTime && stateClothEvent.Used)
            stateClothEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent stateClothEvent in this.StateClothEvents)
      {
        if ((double) stateClothEvent.NormalizedTime < (double) normalizedTime && !stateClothEvent.Used)
        {
          stateClothEvent.Used = true;
          switch (stateClothEvent.EventID + 1)
          {
            case 0:
              actor.ChaControl.ChangeNowCoordinate(true, true);
              continue;
            case 1:
              actor.ChaControl.SetClothesState(0, (byte) 0, true);
              actor.ChaControl.SetClothesState(1, (byte) 0, true);
              actor.ChaControl.SetClothesState(2, (byte) 0, true);
              actor.ChaControl.SetClothesState(3, (byte) 0, true);
              continue;
            case 2:
              actor.ChaControl.SetClothesState(0, (byte) 1, true);
              actor.ChaControl.SetClothesState(1, (byte) 1, true);
              actor.ChaControl.SetClothesState(2, (byte) 1, true);
              actor.ChaControl.SetClothesState(3, (byte) 1, true);
              continue;
            case 3:
              actor.ChaControl.SetClothesState(0, (byte) 2, true);
              actor.ChaControl.SetClothesState(1, (byte) 2, true);
              actor.ChaControl.SetClothesState(2, (byte) 2, true);
              actor.ChaControl.SetClothesState(3, (byte) 2, true);
              continue;
            case 4:
              actor.ChaControl.SetClothesState(0, (byte) 0, true);
              actor.ChaControl.SetClothesState(2, (byte) 0, true);
              continue;
            case 5:
              actor.ChaControl.SetClothesState(0, (byte) 1, true);
              actor.ChaControl.SetClothesState(2, (byte) 1, true);
              continue;
            case 6:
              actor.ChaControl.SetClothesState(0, (byte) 2, true);
              actor.ChaControl.SetClothesState(2, (byte) 2, true);
              continue;
            case 7:
              actor.ChaControl.SetClothesState(1, (byte) 0, true);
              actor.ChaControl.SetClothesState(3, (byte) 0, true);
              continue;
            case 8:
              actor.ChaControl.SetClothesState(1, (byte) 1, true);
              actor.ChaControl.SetClothesState(3, (byte) 1, true);
              continue;
            case 9:
              actor.ChaControl.SetClothesState(1, (byte) 2, true);
              actor.ChaControl.SetClothesState(3, (byte) 2, true);
              continue;
            case 10:
              if (!actor.AgentData.BathCoordinateFileName.IsNullOrEmpty())
              {
                actor.ChaControl.ChangeNowCoordinate(actor.AgentData.BathCoordinateFileName, true, true);
                continue;
              }
              actor.ChaControl.ChangeNowCoordinate(Singleton<Resources>.Instance.BathDefaultCoord, true, true);
              continue;
            case 11:
              string coordinateFileName = actor.AgentData.NowCoordinateFileName;
              if (!coordinateFileName.IsNullOrEmpty())
              {
                actor.ChaControl.ChangeNowCoordinate(coordinateFileName, true, true);
                continue;
              }
              actor.ChaControl.ChangeNowCoordinate(true, true);
              continue;
            default:
              continue;
          }
        }
      }
    }

    protected virtual void UpdateExpressionEvent(float normalizedTime, bool isLoop)
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
      int id = this.Actor.ID;
      foreach (ActorAnimation.ExpressionKeyframeEvent expressionKeyframe in this.ExpressionKeyframeList)
      {
        if ((double) expressionKeyframe.NormalizedTime < (double) normalizedTime && !expressionKeyframe.Used)
        {
          expressionKeyframe.Used = true;
          Singleton<Game>.Instance.GetExpression(id, expressionKeyframe.Name)?.Change(this.Actor.ChaControl);
        }
      }
    }

    private void UpdatePartnerEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent statePartnerEvent in this.StatePartnerEvents)
          statePartnerEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent statePartnerEvent in this.StatePartnerEvents)
        {
          if ((double) statePartnerEvent.NormalizedTime < (double) normalizedTime && statePartnerEvent.Used)
            statePartnerEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent statePartnerEvent in this.StatePartnerEvents)
      {
        if ((double) statePartnerEvent.NormalizedTime < (double) normalizedTime && !statePartnerEvent.Used)
        {
          statePartnerEvent.Used = true;
          if (statePartnerEvent.EventID == 0)
            ;
        }
      }
    }

    private void UpdateSEEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent_SE stateSeEvent in this.StateSEEvents)
          stateSeEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent_SE stateSeEvent in this.StateSEEvents)
        {
          if ((double) normalizedTime < (double) stateSeEvent.NormalizedTime && stateSeEvent.Used)
            stateSeEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent_SE stateSeEvent in this.StateSEEvents)
      {
        ActorAnimation.AnimatorStateEvent_SE e = stateSeEvent;
        if ((double) e.NormalizedTime < (double) normalizedTime && !e.Used)
        {
          e.Used = true;
          if (e.ClipID == -1)
          {
            ((DoorAnimation) ((Component) this.Actor.CurrentPoint)?.GetComponent<DoorAnimation>())?.PlayMoveSE(true);
          }
          else
          {
            int eventId = e.EventID;
            if (eventId < 0)
            {
              ObservableExtensions.Subscribe<long>(Observable.Take<long>((IObservable<M0>) Observable.EveryLateUpdate(), 1), (Action<M0>) (_ =>
              {
                if (Object.op_Equality((Object) e.Root, (Object) null) || !Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
                  return;
                Transform transform = ((Component) Singleton<Manager.Map>.Instance.Player?.CameraControl?.CameraComponent)?.get_transform();
                if (Object.op_Equality((Object) transform, (Object) null))
                  return;
                SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
                SoundPack.Data3D data;
                if (!soundPack.TryGetActionSEData(e.ClipID, out data))
                  return;
                float num = data.MaxDistance + soundPack.Game3DInfo.MarginMaxDistance;
                Vector3 vector3 = Vector3.op_Subtraction(e.Root.get_position(), transform.get_position());
                if ((double) num * (double) num < (double) ((Vector3) ref vector3).get_sqrMagnitude())
                  return;
                AudioSource audioSource = soundPack.Play((SoundPack.IData) data, Manager.Sound.Type.GameSE3D, 0.0f);
                if (!Object.op_Inequality((Object) audioSource, (Object) null))
                  return;
                audioSource.Stop();
                ((Component) audioSource).get_transform().set_position(e.Root.get_position());
                ((Component) audioSource).get_transform().set_rotation(e.Root.get_rotation());
                audioSource.Play();
              }));
            }
            else
            {
              int key = eventId / 2;
              int clipId = e.ClipID;
              if (eventId % 2 == 0)
              {
                Dictionary<int, ValueTuple<AudioSource, FadePlayer>> dictionary;
                bool flag = this.ActionLoopSETable.TryGetValue(key, out dictionary) && dictionary != null;
                if (!flag)
                  this.ActionLoopSETable[key] = dictionary = new Dictionary<int, ValueTuple<AudioSource, FadePlayer>>();
                if (flag)
                {
                  ValueTuple<AudioSource, FadePlayer> pair = (ValueTuple<AudioSource, FadePlayer>) null;
                  flag = dictionary.TryGetValue(clipId, out pair);
                  if (flag)
                  {
                    flag = Object.op_Inequality((Object) pair.Item1, (Object) null) && ((AudioSource) pair.Item1).get_isPlaying();
                    if (!flag)
                    {
                      this.StopActionLoopSE(pair);
                      dictionary.Remove(clipId);
                    }
                  }
                }
                if (!flag)
                {
                  if (Object.op_Equality((Object) e.Root, (Object) null) || !Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
                    break;
                  SoundPack soundPack = Singleton<Resources>.Instance.SoundPack;
                  SoundPack.Data3D data;
                  if (!soundPack.TryGetActionSEData(clipId, out data))
                    break;
                  AudioSource audio = soundPack.Play((SoundPack.IData) data, Manager.Sound.Type.GameSE3D, 0.0f);
                  if (Object.op_Inequality((Object) audio, (Object) null))
                  {
                    FadePlayer component = (FadePlayer) ((Component) audio).GetComponent<FadePlayer>();
                    audio.Stop();
                    Transform root = e.Root;
                    ((Component) audio).get_transform().set_position(root.get_position());
                    ((Component) audio).get_transform().set_rotation(root.get_rotation());
                    audio.set_loop(true);
                    audio.Play();
                    dictionary[clipId] = new ValueTuple<AudioSource, FadePlayer>(audio, component);
                    ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) audio), (Component) root), (Action<M0>) (__ => ((Component) audio).get_transform().SetPositionAndRotation(root.get_position(), root.get_rotation())));
                  }
                }
              }
              else
              {
                Dictionary<int, ValueTuple<AudioSource, FadePlayer>> dictionary;
                ValueTuple<AudioSource, FadePlayer> pair;
                if (this.ActionLoopSETable.TryGetValue(key, out dictionary) && dictionary.TryGetValue(clipId, out pair))
                {
                  this.StopActionLoopSE(pair);
                  dictionary.Remove(clipId);
                }
              }
            }
          }
        }
      }
    }

    private void UpdateParticleEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent_Particle stateParticleEvent in this.StateParticleEvents)
          stateParticleEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent_Particle stateParticleEvent in this.StateParticleEvents)
        {
          if ((double) normalizedTime < (double) stateParticleEvent.NormalizedTime && stateParticleEvent.Used)
            stateParticleEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent_Particle stateParticleEvent in this.StateParticleEvents)
      {
        if ((double) stateParticleEvent.NormalizedTime <= (double) normalizedTime && !stateParticleEvent.Used)
        {
          stateParticleEvent.Used = true;
          Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>> dictionary;
          Tuple<GameObject, ParticleSystem, ParticleSystemRenderer> tuple;
          if (this.Particles.TryGetValue(stateParticleEvent.EventID / 2, out dictionary) && dictionary.TryGetValue(stateParticleEvent.ParticleID, out tuple) && !Object.op_Equality((Object) tuple.Item2, (Object) null))
          {
            ParticleSystem particleSystem = tuple.Item2;
            bool flag = stateParticleEvent.EventID % 2 == 0;
            if (particleSystem.get_isPlaying())
              particleSystem.Stop(true, (ParticleSystemStopBehavior) 0);
            if (flag)
              particleSystem.Play(true);
          }
        }
      }
    }

    private void UpdateVoiceEvent(float normalizedTime, bool isLoop)
    {
      if (isLoop)
      {
        foreach (ActorAnimation.AnimatorStateEvent_OnceVoice stateOnceVoiceEvent in this.StateOnceVoiceEvents)
          stateOnceVoiceEvent.Used = false;
      }
      else
      {
        foreach (ActorAnimation.AnimatorStateEvent_OnceVoice stateOnceVoiceEvent in this.StateOnceVoiceEvents)
        {
          if ((double) normalizedTime < (double) stateOnceVoiceEvent.NormalizedTime && stateOnceVoiceEvent.Used)
            stateOnceVoiceEvent.Used = false;
        }
      }
      foreach (ActorAnimation.AnimatorStateEvent_OnceVoice stateOnceVoiceEvent in this.StateOnceVoiceEvents)
      {
        if ((double) stateOnceVoiceEvent.NormalizedTime <= (double) normalizedTime && !stateOnceVoiceEvent.Used)
        {
          stateOnceVoiceEvent.Used = true;
          if (!stateOnceVoiceEvent.IDs.IsNullOrEmpty<int>())
            this.PlayEventOnceVoice(stateOnceVoiceEvent.IDs.GetElement<int>(Random.Range(0, stateOnceVoiceEvent.IDs.Length)));
        }
      }
    }

    public List<ActorAnimation.AnimatorStateEvent> StateItemEvents { get; set; }

    public List<ActorAnimation.AnimatorStateEvent> StatePartnerEvents { get; set; }

    public List<ActorAnimation.AnimatorStateEvent_SE> StateSEEvents { get; set; }

    public List<ActorAnimation.AnimatorStateEvent_Particle> StateParticleEvents { get; set; }

    public List<ActorAnimation.AnimatorStateEvent_OnceVoice> StateOnceVoiceEvents { get; set; }

    public List<int> StateLoopVoiceEvents { get; set; }

    public List<ActorAnimation.AnimatorStateEvent> StateClothEvents { get; set; }

    public List<ActorAnimation.ExpressionKeyframeEvent> ExpressionKeyframeList { get; set; }

    public List<ValueTuple<int, GameObject>> Items { get; private set; }

    public List<ValueTuple<int, Renderer[]>> ItemRenderers { get; private set; }

    public Dictionary<int, ItemAnimInfo> ItemAnimatorTable { get; private set; }

    public Dictionary<int, Dictionary<int, ValueTuple<AudioSource, FadePlayer>>> ActionLoopSETable { get; private set; }

    public Dictionary<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>> Particles { get; private set; }

    public void LoadEventVarious(int eventID, int poseID, PlayState info)
    {
      this.LoadEventKeyTable(eventID, poseID);
      this.Actor.LoadEventItems(info);
      this.Actor.LoadEventParticles(eventID, poseID);
    }

    public void LoadAnimatorIfNotEquals(PlayState info)
    {
      bool flag = false;
      foreach (PlayState.Info stateInfo in info.MainStateInfo.InStateInfo.StateInfos)
      {
        int hash = Animator.StringToHash(stateInfo.stateName);
        if (!this.Animator.HasState(stateInfo.layer, hash))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      AssetBundleInfo assetBundleInfo = info.MainStateInfo.AssetBundleInfo;
      if (!((string) assetBundleInfo.asset != ((Object) this.Animator.get_runtimeAnimatorController()).get_name()))
        return;
      RuntimeAnimatorController rac = AssetUtility.LoadAsset<RuntimeAnimatorController>((string) assetBundleInfo.assetbundle, (string) assetBundleInfo.asset, string.Empty);
      if (!Object.op_Inequality((Object) rac, (Object) null))
        return;
      this.SetAnimatorController(rac);
    }

    public ActorAnimInfo SetAnimInfo(PlayState info)
    {
      this.AnimInfo = new ActorAnimInfo()
      {
        inEnableBlend = info.MainStateInfo.InStateInfo.EnableFade,
        inBlendSec = info.MainStateInfo.InStateInfo.FadeSecond,
        inFadeOutTime = info.MainStateInfo.FadeOutTime,
        outEnableBlend = info.MainStateInfo.OutStateInfo.EnableFade,
        outBlendSec = info.MainStateInfo.OutStateInfo.FadeSecond,
        directionType = info.DirectionType,
        endEnableBlend = info.EndEnableBlend,
        endBlendSec = info.EndBlendRate,
        isLoop = info.MainStateInfo.IsLoop,
        loopMinTime = info.MainStateInfo.LoopMin,
        loopMaxTime = info.MainStateInfo.LoopMax,
        hasAction = info.ActionInfo.hasAction,
        loopStateName = info.MainStateInfo.InStateInfo.StateInfos.GetElement<PlayState.Info>(info.MainStateInfo.InStateInfo.StateInfos.Length - 1).stateName,
        randomCount = info.ActionInfo.randomCount,
        oldNormalizedTime = 0.0f,
        layer = info.MainStateInfo.InStateInfo.StateInfos[0].layer
      };
      return this.AnimInfo;
    }

    public ActorAnimInfo LoadActionState(int eventID, int poseID, PlayState info)
    {
      this.LoadEventVarious(eventID, poseID, info);
      this.InitializeStates(info);
      this.LoadAnimatorIfNotEquals(info);
      return this.SetAnimInfo(info);
    }

    public void EnableItems()
    {
      using (List<ValueTuple<int, GameObject>>.Enumerator enumerator = this.Items.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, GameObject> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Item2, (Object) null))
            ((GameObject) current.Item2).SetActive(true);
        }
      }
    }

    public void DisableItems()
    {
      using (List<ValueTuple<int, GameObject>>.Enumerator enumerator = this.Items.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, GameObject> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Item2, (Object) null))
            ((GameObject) current.Item2).SetActive(false);
        }
      }
    }

    public void ClearItems()
    {
      this.ItemRenderers.Clear();
      this.ItemAnimatorTable.Clear();
      using (List<ValueTuple<int, GameObject>>.Enumerator enumerator = this.Items.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ValueTuple<int, GameObject> current = enumerator.Current;
          if (Object.op_Inequality((Object) current.Item2, (Object) null))
            Object.Destroy((Object) current.Item2);
        }
      }
      this.Items.Clear();
    }

    public void EnableParticleRenderer()
    {
      using (Dictionary<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>>.Enumerator enumerator1 = this.Particles.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>> current1 = enumerator1.Current;
          if (current1.Value != null)
          {
            using (Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>> current2 = enumerator2.Current;
                ParticleSystem particleSystem = current2.Value.Item2;
                if (Object.op_Inequality((Object) particleSystem, (Object) null) && particleSystem.get_isPlaying())
                  particleSystem.Pause(true);
                ParticleSystemRenderer particleSystemRenderer = current2.Value.Item3;
                if (Object.op_Inequality((Object) particleSystemRenderer, (Object) null) && !((Renderer) particleSystemRenderer).get_enabled())
                  ((Renderer) particleSystemRenderer).set_enabled(true);
              }
            }
          }
        }
      }
    }

    public void DisableParticleRenderer()
    {
      using (Dictionary<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>>.Enumerator enumerator1 = this.Particles.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>> current1 = enumerator1.Current;
          if (current1.Value != null)
          {
            using (Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>> current2 = enumerator2.Current;
                ParticleSystem particleSystem = current2.Value.Item2;
                if (Object.op_Inequality((Object) particleSystem, (Object) null) && particleSystem.get_isPaused())
                  particleSystem.Play(true);
                ParticleSystemRenderer particleSystemRenderer = current2.Value.Item3;
                if (Object.op_Inequality((Object) particleSystemRenderer, (Object) null) && ((Renderer) particleSystemRenderer).get_enabled())
                  ((Renderer) particleSystemRenderer).set_enabled(false);
              }
            }
          }
        }
      }
    }

    public void ClearParticles()
    {
      if (this.Particles.IsNullOrEmpty<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>>())
        return;
      using (Dictionary<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>>.Enumerator enumerator1 = this.Particles.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>> current = enumerator1.Current;
          if (!current.Value.IsNullOrEmpty<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>())
          {
            using (Dictionary<int, Tuple<GameObject, ParticleSystem, ParticleSystemRenderer>>.Enumerator enumerator2 = current.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                Tuple<GameObject, ParticleSystem, ParticleSystemRenderer> tuple = enumerator2.Current.Value;
                if (tuple != null)
                {
                  GameObject gameObject = tuple.Item1;
                  if (!Object.op_Equality((Object) gameObject, (Object) null))
                    Object.Destroy((Object) gameObject);
                }
              }
            }
          }
        }
      }
      this.Particles.Clear();
    }

    public virtual void StopLoopActionVoice()
    {
      if (Object.op_Equality((Object) this._loopActionVoice.Item2, (Object) null) || Object.op_Equality((Object) ((Component) this._loopActionVoice.Item2).get_gameObject(), (Object) null))
        return;
      if (Object.op_Inequality((Object) this._loopActionVoice.Item3, (Object) null))
        Singleton<Manager.Voice>.Instance.Stop((int) this._loopActionVoice.Item4, (Transform) this._loopActionVoice.Item3);
      else
        Object.Destroy((Object) ((Component) this._loopActionVoice.Item2).get_gameObject());
      this._loopActionVoice.Item1 = (__Null) -1;
      this._loopActionVoice.Item2 = null;
      this._loopActionVoice.Item3 = null;
      this._loopActionVoice.Item4 = (__Null) -1;
    }

    private void StopActionLoopSE(ValueTuple<AudioSource, FadePlayer> pair)
    {
      if (Object.op_Inequality((Object) pair.Item2, (Object) null) && Object.op_Inequality((Object) ((Component) pair.Item2).get_gameObject(), (Object) null))
      {
        if (Singleton<Resources>.IsInstance())
          ((FadePlayer) pair.Item2).Stop(Singleton<Resources>.Instance.SoundPack.Game3DInfo.StopFadeTime);
        else
          ((FadePlayer) pair.Item2).Stop(0.5f);
      }
      else
      {
        if (!Object.op_Inequality((Object) pair.Item1, (Object) null) || !Object.op_Inequality((Object) ((Component) pair.Item1).get_gameObject(), (Object) null))
          return;
        if (Singleton<Manager.Sound>.IsInstance())
          Singleton<Manager.Sound>.Instance.Stop(Manager.Sound.Type.GameSE3D, ((Component) pair.Item2).get_transform());
        else
          Object.Destroy((Object) ((Component) pair.Item1).get_gameObject());
      }
    }

    private void StopAllActionLoopSE()
    {
      if (this.ActionLoopSETable.IsNullOrEmpty<int, Dictionary<int, ValueTuple<AudioSource, FadePlayer>>>())
        return;
      using (Dictionary<int, Dictionary<int, ValueTuple<AudioSource, FadePlayer>>>.Enumerator enumerator1 = this.ActionLoopSETable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, ValueTuple<AudioSource, FadePlayer>>> current = enumerator1.Current;
          if (!current.Value.IsNullOrEmpty<int, ValueTuple<AudioSource, FadePlayer>>())
          {
            using (Dictionary<int, ValueTuple<AudioSource, FadePlayer>>.Enumerator enumerator2 = current.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
                this.StopActionLoopSE(enumerator2.Current.Value);
            }
          }
        }
      }
      this.ActionLoopSETable.Clear();
    }

    private void LoadFootStepEvents()
    {
      this._prevFootStepInfo = (FootStepInfo) null;
      this._footStepEvents.Clear();
      FootStepInfo[] source;
      if (this._footStepEventKeyTable == null || !this._footStepEventKeyTable.TryGetValue(this._nameHash, out source) || source.IsNullOrEmpty<FootStepInfo>())
        return;
      foreach (FootStepInfo info in source)
        this._footStepEvents.Add(new FootStepInfo(info));
    }

    public bool IsFootStepActive
    {
      get
      {
        return this._isFootStepActive;
      }
      set
      {
        this._prevFootStepInfo = (FootStepInfo) null;
        this._isFootStepActive = value;
      }
    }

    private void UpdateFootStepEvent(
      AnimatorStateInfo stateInfo,
      float normalizedTime,
      bool isLoop)
    {
      if (!this._isFootStepActive || this._footStepEvents.IsNullOrEmpty<FootStepInfo>())
        return;
      float num1 = this.GetFloat(Singleton<Resources>.Instance.DefinePack.AnimatorParameter.ForwardMove);
      FootStepInfo footStepInfo1 = (FootStepInfo) null;
      foreach (FootStepInfo footStepEvent in this._footStepEvents)
      {
        if (footStepEvent.Threshold.IsRange(num1))
        {
          footStepInfo1 = footStepEvent;
          break;
        }
      }
      if (footStepInfo1 == null || footStepInfo1.Keys.IsNullOrEmpty<FootStepInfo.Key>())
        return;
      if (footStepInfo1 != this._prevFootStepInfo)
      {
        for (int index = 0; index < footStepInfo1.Keys.Length; ++index)
          footStepInfo1.Keys[index].Execute = (double) footStepInfo1.Keys[index].Time < (double) normalizedTime;
        this._prevFootStepInfo = footStepInfo1;
      }
      else if (isLoop)
      {
        for (int index = 0; index < footStepInfo1.Keys.Length; ++index)
          footStepInfo1.Keys[index].Execute = false;
      }
      else
      {
        for (int index = 0; index < footStepInfo1.Keys.Length; ++index)
        {
          FootStepInfo.Key key = footStepInfo1.Keys[index];
          if ((double) normalizedTime < (double) key.Time && key.Execute)
            footStepInfo1.Keys[index].Execute = false;
        }
      }
      bool flag = false;
      for (int index = 0; index < footStepInfo1.Keys.Length; ++index)
      {
        FootStepInfo.Key key = footStepInfo1.Keys[index];
        if ((double) key.Time <= (double) normalizedTime && !key.Execute)
        {
          footStepInfo1.Keys[index].Execute = true;
          flag = true;
        }
      }
      if (!flag)
        return;
      this._footStepAudioSources.RemoveAll((Predicate<AudioSource>) (x => Object.op_Equality((Object) x, (Object) null) || Object.op_Equality((Object) ((Component) x).get_gameObject(), (Object) null) || !x.get_isPlaying()));
      Vector3 position = this.Actor.Position;
      Quaternion rotation = this.Actor.Rotation;
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      ActorCameraControl cameraControl = instance.Player.CameraControl;
      float num2 = Vector3.Distance(position, ((Component) cameraControl).get_transform().get_position());
      SoundPack.FootStepInfoGroup footStepInfo2 = Singleton<Resources>.Instance.SoundPack.FootStepInfo;
      if ((double) num2 > (double) footStepInfo2.PlayEnableDistance)
        return;
      AudioSource audioSource = (AudioSource) null;
      bool isBareFoot = this.Actor.ChaControl.IsBareFoot;
      byte sex = this.Actor.ChaControl.sex;
      Vector3 vector3 = Vector3.op_Addition(position, Vector3.op_Multiply(Vector3.get_up(), 15f));
      LayerMask seLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.SELayer;
      Ray ray;
      ((Ray) ref ray).\u002Ector(vector3, Vector3.get_down());
      int num3 = Physics.RaycastNonAlloc(ray, ActorAnimation._raycastHits, 50f, LayerMask.op_Implicit(seLayer));
      Weather weather = instance.Simulator.Weather;
      SoundPack.PlayAreaType areaType = SoundPack.PlayAreaType.Normal;
      if (0 < num3)
      {
        float num4 = float.MaxValue;
        string str = (string) null;
        for (int index = 0; index < num3; ++index)
        {
          RaycastHit raycastHit = ActorAnimation._raycastHits[index];
          GameObject gameObject = ((Component) ((RaycastHit) ref raycastHit).get_collider())?.get_gameObject();
          if (!Object.op_Equality((Object) gameObject, (Object) null))
          {
            float distance = ((RaycastHit) ref raycastHit).get_distance();
            if ((double) distance < (double) num4)
            {
              str = gameObject.get_tag();
              num4 = distance;
            }
          }
        }
        if (!str.IsNullOrEmpty())
          audioSource = Singleton<Resources>.Instance.SoundPack.PlayFootStep(sex, isBareFoot, str, weather, areaType);
      }
      else
      {
        MapArea mapArea;
        if (Object.op_Inequality((Object) (mapArea = this.Actor.MapArea), (Object) null))
          audioSource = Singleton<Resources>.Instance.SoundPack.PlayFootStep(sex, isBareFoot, instance.MapID, mapArea.AreaID, weather, areaType);
      }
      if (!Object.op_Inequality((Object) audioSource, (Object) null))
        return;
      audioSource.Stop();
      ((Component) audioSource).get_transform().SetPositionAndRotation(position, rotation);
      FadePlayer component = (FadePlayer) ((Component) audioSource).GetComponent<FadePlayer>();
      if (Object.op_Inequality((Object) component, (Object) null))
        component.currentVolume = audioSource.get_volume();
      audioSource.set_rolloffMode(footStepInfo2.RolloffMode);
      audioSource.set_minDistance(footStepInfo2.MinDistance);
      audioSource.set_maxDistance(footStepInfo2.MaxDistance);
      this._footStepAudioSources.Add(audioSource);
      audioSource.Play();
    }

    public void InitializeStates(PlayState info)
    {
      if (info == null)
      {
        this.InStates.Clear();
        this.OutStates.Clear();
        this.ActionStates.Clear();
      }
      else
      {
        this.InStates.Clear();
        if (!info.MainStateInfo.InStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in info.MainStateInfo.InStateInfo.StateInfos)
            this.InStates.Enqueue(stateInfo);
        }
        this.OutStates.Clear();
        if (!info.MainStateInfo.OutStateInfo.StateInfos.IsNullOrEmpty<PlayState.Info>())
        {
          foreach (PlayState.Info stateInfo in info.MainStateInfo.OutStateInfo.StateInfos)
            this.OutStates.Enqueue(stateInfo);
        }
        this.ActionStates.Clear();
        if (!info.SubStateInfos.IsNullOrEmpty<PlayState.PlayStateInfo>())
        {
          foreach (PlayState.PlayStateInfo subStateInfo in info.SubStateInfos)
            this.ActionStates.Add(subStateInfo);
        }
        this.AnimABInfo = info.MainStateInfo.AssetBundleInfo;
        this._maskState = info.MaskStateInfo;
      }
    }

    public void InitializeStates(
      PlayState.Info[] inStateInfos,
      PlayState.Info[] outStateInfos,
      AssetBundleInfo animABInfo)
    {
      this.InStates.Clear();
      if (!inStateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info inStateInfo in inStateInfos)
          this.InStates.Enqueue(inStateInfo);
      }
      this.OutStates.Clear();
      if (!outStateInfos.IsNullOrEmpty<PlayState.Info>())
      {
        foreach (PlayState.Info outStateInfo in outStateInfos)
          this.OutStates.Enqueue(outStateInfo);
      }
      this.AnimABInfo = animABInfo;
      this.ActionStates.Clear();
    }

    public void EndStates()
    {
      ActorAnimInfo animInfo = this.AnimInfo;
      if (!animInfo.endEnableBlend)
        this.CrossFadeScreen(-1f);
      if (((Object) this.Animator.get_runtimeAnimatorController()).get_name() != ((Object) this._defaultAnimController).get_name())
        this.ResetDefaultAnimatorController();
      this.Actor.SetStand(this.RecoveryPoint, animInfo.endEnableBlend, animInfo.endBlendSec, animInfo.directionType);
      this.RecoveryPoint = (Transform) null;
      this.RefsActAnimInfo = true;
    }

    public void StopAllAnimCoroutine()
    {
      this.StopInLocoAnimCoroutine();
      this.StopInAnimCoroutine();
      this.StopOutAnimCoroutine();
      this.StopActionAnimCoroutine();
      this.StopOnceActionAnimCoroutine();
      this.StopTurnAnimCoroutine();
    }

    public void PlayInLocoAnimation(bool enableFade, float fadeTime, int layer)
    {
      IEnumerator enumerator = this._inLocoAnimEnumerator = this.StartInLocoAnimation(enableFade, fadeTime, layer);
      this._inLocoAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    public void StopInLocoAnimCoroutine()
    {
      if (this._inLocoAnimDisposable == null)
        return;
      this._inLocoAnimDisposable.Dispose();
      this._inLocoAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartInLocoAnimation(bool enableFade, float fadeTime, int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartInLocoAnimation\u003Ec__Iterator0()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        \u0024this = this
      };
    }

    public void PlayInAnimation(bool enableFade, float fadeTime, float fadeOutTime, int layer)
    {
      IEnumerator enumerator = this._inAnimEnumerator = this.StartInAnimation(enableFade, fadeTime, fadeOutTime, layer);
      this._inAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    public void StopInAnimCoroutine()
    {
      if (this._inAnimDisposable == null)
        return;
      this._inAnimDisposable.Dispose();
      this._inAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartInAnimation(
      bool enableFade,
      float fadeTime,
      float fadeOutTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartInAnimation\u003Ec__Iterator1()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        fadeOutTime = fadeOutTime,
        \u0024this = this
      };
    }

    public void PlayOutAnimation(bool enableFade, float fadeTime, int layer)
    {
      IEnumerator enumerator = this._outAnimEnumerator = this.StartOutAnimation(enableFade, fadeTime, layer);
      this._outAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    public void StopOutAnimCoroutine()
    {
      if (this._outAnimDisposable == null)
        return;
      this._outAnimDisposable.Dispose();
      this._outAnimEnumerator = (IEnumerator) null;
    }

    [DebuggerHidden]
    private IEnumerator StartOutAnimation(bool enableFade, float fadeTime, int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartOutAnimation\u003Ec__Iterator2()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        \u0024this = this
      };
    }

    public void PlayActionAnimation(int layer)
    {
      IEnumerator enumerator = this._actionAnimEnumerator = this.StartActionAnimation(layer);
      this._actionAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator StartActionAnimation(int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartActionAnimation\u003Ec__Iterator3()
      {
        layer = layer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator StartActionInAnimation(
      bool enableFade,
      float fadeTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartActionInAnimation\u003Ec__Iterator4()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator StartActionOutAnimation(
      bool enableFade,
      float fadeTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartActionOutAnimation\u003Ec__Iterator5()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        \u0024this = this
      };
    }

    public void StopActionAnimCoroutine()
    {
      if (this._actionAnimDisposable == null)
        return;
      this._actionAnimDisposable.Dispose();
      this._actionAnimEnumerator = (IEnumerator) null;
    }

    public void PlayOnceActionAnimation(bool enableFade, float fadeTime, int layer)
    {
      IEnumerator enumerator = this._onceActionAnimEnumerator = this.StartOnceActionAnimation(enableFade, fadeTime, layer);
      this._onceActionAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator StartOnceActionAnimation(
      bool enableFade,
      float fadeTime,
      int layer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartOnceActionAnimation\u003Ec__Iterator6()
      {
        enableFade = enableFade,
        fadeTime = fadeTime,
        layer = layer,
        \u0024this = this
      };
    }

    public void StopOnceActionAnimCoroutine()
    {
      if (this._onceActionAnimDisposable == null)
        return;
      this._onceActionAnimDisposable.Dispose();
      this._onceActionAnimEnumerator = (IEnumerator) null;
    }

    public void PlayTurnAnimation(
      Vector3 to,
      float fadeOutTime,
      PlayState.AnimStateInfo recoverStateInfo,
      bool isFast = false)
    {
      IEnumerator enumerator = this._turnAnimEnumerator = this.StartTurnAnimation(to, fadeOutTime, recoverStateInfo, false);
      this._turnAnimDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => enumerator), false));
    }

    [DebuggerHidden]
    private IEnumerator StartTurnAnimation(
      Vector3 to,
      float fadeOutTime,
      PlayState.AnimStateInfo recoverStateInfo,
      bool isFast = false)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartTurnAnimation\u003Ec__Iterator7()
      {
        to = to,
        isFast = isFast,
        fadeOutTime = fadeOutTime,
        recoverStateInfo = recoverStateInfo,
        \u0024this = this
      };
    }

    public void PlayTurnAnimation(
      float angle,
      float fadeOutTime,
      PlayState.AnimStateInfo recoverStateInfo)
    {
      this._turnAnimEnumerator = this.StartTurnAnimation(angle, fadeOutTime, recoverStateInfo);
    }

    [DebuggerHidden]
    private IEnumerator StartTurnAnimation(
      float angle,
      float fadeOutTime,
      PlayState.AnimStateInfo recoverStateInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CStartTurnAnimation\u003Ec__Iterator8()
      {
        angle = angle,
        fadeOutTime = fadeOutTime,
        recoverStateInfo = recoverStateInfo,
        \u0024this = this
      };
    }

    public void StopTurnAnimCoroutine()
    {
      if (this._turnAnimDisposable == null)
        return;
      this._turnAnimDisposable.Dispose();
      this._turnAnimEnumerator = (IEnumerator) null;
    }

    public bool PlayingInLocoAnimation
    {
      get
      {
        return this._inLocoAnimEnumerator != null;
      }
    }

    public bool PlayingInAnimation
    {
      get
      {
        return this._inAnimEnumerator != null;
      }
    }

    public bool PlayingOutAnimation
    {
      get
      {
        return this._outAnimEnumerator != null;
      }
    }

    public bool PlayingActAnimation
    {
      get
      {
        return this._actionAnimEnumerator != null;
      }
    }

    public bool PlayingTurnAnimation
    {
      get
      {
        return this._turnAnimEnumerator != null;
      }
    }

    public bool PlayingOnceActionAnimation
    {
      get
      {
        return this._onceActionAnimEnumerator != null;
      }
    }

    public virtual void PlayAnimation(string stateName, int layer, float normalizedTime)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      if (!this._parameters.IsNullOrEmpty<AnimatorControllerParameter>())
      {
        foreach (AnimatorControllerParameter parameter in this._parameters)
        {
          if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
          {
            float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
            if (Object.op_Inequality((Object) animator, (Object) null))
              animator.SetFloat(heightParameterName, shapeBodyValue);
          }
        }
      }
      this.PlayAnimation(animator, stateName, layer, normalizedTime);
    }

    public virtual void PlayAnimation(int stateNameHash, int layer, float normalizedTime)
    {
      Animator animator = this.Animator;
      string heightParameterName = Singleton<Resources>.Instance.DefinePack.AnimatorParameter.HeightParameterName;
      foreach (AnimatorControllerParameter parameter in animator.get_parameters())
      {
        if (parameter.get_name() == heightParameterName && parameter.get_type() == 1)
        {
          float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
          if (Object.op_Inequality((Object) animator, (Object) null))
            animator.SetFloat(heightParameterName, shapeBodyValue);
        }
      }
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.Play(stateNameHash, layer, normalizedTime);
    }

    public void PlayAnimation(
      Animator animator,
      string stateName,
      int layer,
      float normalizedTime)
    {
      if (Object.op_Inequality((Object) ((Component) this).get_gameObject(), (Object) null) && Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0} Play to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.Play(stateName, layer, normalizedTime);
    }

    public virtual void CrossFadeAnimation(
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
          float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
          if (Object.op_Inequality((Object) animator, (Object) null))
            animator.SetFloat(heightParameterName, shapeBodyValue);
        }
      }
      this.CrossFadeAnimation(animator, stateName, fadeTime, layer, fixedTimeOffset);
    }

    public virtual void CrossFadeAnimation(
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
          float shapeBodyValue = this.Actor.ChaControl.GetShapeBodyValue(0);
          if (Object.op_Inequality((Object) animator, (Object) null))
            animator.SetFloat(heightParameterName, shapeBodyValue);
        }
      }
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.CrossFadeInFixedTime(stateNameHash, fadeTime, layer, fixedTimeOffset, 0.0f);
    }

    public void CrossFadeAnimation(
      Animator animator,
      string stateName,
      float fadeTime,
      int layer,
      float fixedTimeOffset)
    {
      if (Debug.get_isDebugBuild())
        Debug.Log((object) string.Format("{0}: CrossFade to {1}", (object) ((Object) ((Component) this).get_gameObject()).get_name(), (object) stateName));
      if (!Object.op_Inequality((Object) animator, (Object) null))
        return;
      animator.CrossFadeInFixedTime(stateName, fadeTime, layer, fixedTimeOffset, 0.0f);
    }

    [DebuggerHidden]
    private IEnumerator PlayState(
      Animator animator,
      PlayState.Info state,
      bool enableFade,
      float fadeTime,
      int layer,
      float fadeOutTime)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ActorAnimation.\u003CPlayState\u003Ec__Iterator9()
      {
        enableFade = enableFade,
        state = state,
        fadeTime = fadeTime,
        layer = layer,
        animator = animator,
        fadeOutTime = fadeOutTime,
        \u0024this = this
      };
    }

    protected virtual void CrossFadeItemAnimation(string stateName, float fadeTime, int layer)
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
              animator.SetFloat(heightParameterName, shapeBodyValue);
            }
          }
          this.CrossFadeAnimation(animator, stateName, fadeTime, layer, 0.0f);
        }
      }
    }

    private void CrossFadeHousingActionPointAnimation(string stateName, float fadeTime, int layer)
    {
      if (Object.op_Equality((Object) this.Actor.CurrentPoint, (Object) null))
        return;
      HousingActionPointAnimation[] componentsInChildren = (HousingActionPointAnimation[]) ((Component) this.Actor.CurrentPoint).GetComponentsInChildren<HousingActionPointAnimation>();
      if (componentsInChildren.IsNullOrEmpty<HousingActionPointAnimation>())
        return;
      foreach (HousingActionPointAnimation actionPointAnimation in componentsInChildren)
      {
        if (!Object.op_Equality((Object) actionPointAnimation.Animator, (Object) null))
          this.CrossFadeAnimation(actionPointAnimation.Animator, stateName, fadeTime, layer, 0.0f);
      }
    }

    protected virtual void PlayItemAnimation(string stateName)
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
              animator.SetFloat(heightParameterName, shapeBodyValue);
            }
          }
          this.PlayAnimation(animator, stateName, 0, 0.0f);
        }
      }
    }

    private void PlayHousingActionPointAnimation(string stateName)
    {
      if (Object.op_Equality((Object) this.Actor.CurrentPoint, (Object) null))
        return;
      HousingActionPointAnimation[] componentsInChildren = (HousingActionPointAnimation[]) ((Component) this.Actor.CurrentPoint).GetComponentsInChildren<HousingActionPointAnimation>();
      if (componentsInChildren.IsNullOrEmpty<HousingActionPointAnimation>())
        return;
      foreach (HousingActionPointAnimation actionPointAnimation in componentsInChildren)
      {
        if (!Object.op_Equality((Object) actionPointAnimation.Animator, (Object) null))
          this.PlayAnimation(actionPointAnimation.Animator, stateName, 0, 0.0f);
      }
    }

    protected virtual void LoadMatchTargetInfo(string stateName)
    {
    }

    protected virtual void LoadStateLocomotionVoice(int stateHashName)
    {
    }

    protected virtual void LoadStateExpression(int stateHashName)
    {
      Resources.ActionTable action = Singleton<Resources>.Instance.Action;
      this.ExpressionKeyframeList.Clear();
      int id = this.Actor.ID;
      Dictionary<int, List<ExpressionKeyframe>> dictionary1;
      List<ExpressionKeyframe> expressionKeyframeList;
      if (Singleton<Resources>.Instance.Action.ActionExpressionKeyframeTable.TryGetValue(id, out dictionary1) && dictionary1.TryGetValue(stateHashName, out expressionKeyframeList))
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
        if (!Singleton<Resources>.Instance.Action.ActionExpressionTable.TryGetValue(id, out dictionary2) || !dictionary2.TryGetValue(stateHashName, out key))
          return;
        Singleton<Game>.Instance.GetExpression(id, key)?.Change(this.Actor.ChaControl);
      }
    }

    public abstract void LoadEventKeyTable(int eventID, int poseID);

    public abstract void LoadSEEventKeyTable(int eventID, int poseID);

    public virtual void LoadAnimalEventKeyTable(int animalTypeID, int poseID)
    {
    }

    protected abstract void LoadFootStepEventKeyTable();

    public abstract void LoadParticleEventKeyTable(int eventID, int poseID);

    public virtual void LoadOnceVoiceEventKeyTable(int eventID, int poseID)
    {
    }

    public virtual void LoadLoopVoiceEventKeyTable(int eventID, int poseID)
    {
    }

    protected virtual void PlayEventOnceVoice(int voiceID)
    {
    }

    protected virtual bool PlayEventLoopVoice()
    {
      return !this._ignoreVoiceEvent && !this._ignoreAnimEvent && (!Object.op_Inequality((Object) this.OnceActionVoice, (Object) null) || !this.OnceActionVoice.get_isPlaying()) && !this.StateLoopVoiceEvents.IsNullOrEmpty<int>();
    }

    public void CrossFadeScreen(float time = -1f)
    {
      if (!this.Actor.ChaControl.IsVisibleInCamera)
        return;
      ActorCameraControl cameraControl = Singleton<Manager.Map>.Instance.Player.CameraControl;
      if ((double) Vector3.Distance(this.Actor.Position, ((Component) cameraControl).get_transform().get_position()) > (double) Singleton<Resources>.Instance.LocomotionProfile.CrossFadeEnableDistance)
        return;
      cameraControl.CrossFade.FadeStart(time);
    }

    public class AnimatorStateEvent
    {
      public float NormalizedTime { get; set; }

      public int EventID { get; set; }

      public bool Used { get; set; }
    }

    public class ExpressionKeyframeEvent
    {
      public float NormalizedTime { get; set; }

      public string Name { get; set; }

      public bool Used { get; set; }
    }

    public class AnimatorStateEvent_SE
    {
      public float NormalizedTime { get; set; }

      public int ClipID { get; set; } = -1;

      public int EventID { get; set; } = -1;

      public Transform Root { get; set; }

      public bool Used { get; set; }
    }

    public class AnimatorStateEvent_Particle
    {
      public float NormalizedTime { get; set; } = float.MaxValue;

      public int ParticleID { get; set; } = -1;

      public int EventID { get; set; } = -1;

      public string Root { get; set; }

      public bool Used { get; set; }
    }

    public class AnimatorStateEvent_OnceVoice
    {
      public float NormalizedTime { get; set; } = float.MaxValue;

      public int[] IDs { get; set; }

      public bool Used { get; set; }
    }
  }
}
