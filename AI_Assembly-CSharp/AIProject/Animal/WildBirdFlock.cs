// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.WildBirdFlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Animal.Resources;
using AIProject.Scene;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal
{
  public class WildBirdFlock : AnimalBase
  {
    [SerializeField]
    [Min(0.0f)]
    private float flapSpeed = 5f;
    [SerializeField]
    [MinMaxSlider(0.0f, 100f, true)]
    private Vector2 nextBirdWaitSecondTime = Vector2.get_zero();
    private List<WildBirdFlock.Bird> birds = new List<WildBirdFlock.Bird>();
    [SerializeField]
    [ReadOnly]
    [HideInEditorMode]
    private BirdFlockHabitatPoint habitatPoint;
    private BirdFlockHabitatPoint.BirdMoveAreaInfo areaInfo;
    private int birdNum;
    private float deathDistance;

    public override bool IsNeutralCommand
    {
      get
      {
        return false;
      }
    }

    public void Initialize(BirdFlockHabitatPoint _habitatPoint)
    {
      this.Clear();
      if (Object.op_Equality((Object) (this.habitatPoint = _habitatPoint), (Object) null))
        this.SetState(AnimalState.Destroyed, (Action) null);
      else if (!this.habitatPoint.SetUse(this))
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        this.areaInfo = (BirdFlockHabitatPoint.BirdMoveAreaInfo) null;
        List<BirdFlockHabitatPoint.BirdMoveAreaInfo> birdMoveAreaInfoList = ListPool<BirdFlockHabitatPoint.BirdMoveAreaInfo>.Get();
        birdMoveAreaInfoList.AddRange((IEnumerable<BirdFlockHabitatPoint.BirdMoveAreaInfo>) this.habitatPoint.AreaInfos);
        while (!((IReadOnlyList<BirdFlockHabitatPoint.BirdMoveAreaInfo>) birdMoveAreaInfoList).IsNullOrEmpty<BirdFlockHabitatPoint.BirdMoveAreaInfo>())
        {
          this.areaInfo = birdMoveAreaInfoList.GetRand<BirdFlockHabitatPoint.BirdMoveAreaInfo>();
          if (this.areaInfo == null || this.areaInfo.Available)
            ;
        }
        ListPool<BirdFlockHabitatPoint.BirdMoveAreaInfo>.Release(birdMoveAreaInfoList);
        if (this.areaInfo == null)
        {
          this.SetState(AnimalState.Destroyed, (Action) null);
        }
        else
        {
          this.SetStateData();
          MapArea ownerArea = this.habitatPoint.OwnerArea;
          this.ChunkID = !Object.op_Inequality((Object) ownerArea, (Object) null) ? 0 : ownerArea.ChunkID;
          this.birdNum = this.areaInfo.CreateNumRange.RandomRange();
          Vector3 position1 = this.areaInfo.StartPoint.get_position();
          Vector3 position2 = this.areaInfo.EndPoint.get_position();
          this.deathDistance = Vector3.Distance(position1, position2);
          this.Position = position1;
          this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(position2, position1), Vector3.get_up());
          this.MarkerEnabled = true;
          this.SetState(AnimalState.Locomotion, (Action) null);
        }
      }
    }

    public override void Clear()
    {
      base.Clear();
      if (!((IReadOnlyList<WildBirdFlock.Bird>) this.birds).IsNullOrEmpty<WildBirdFlock.Bird>())
      {
        foreach (WildBirdFlock.Bird bird in this.birds)
          bird?.Destroy();
        this.birds.Clear();
      }
      if (!Object.op_Inequality((Object) this.habitatPoint, (Object) null))
        return;
      this.habitatPoint.StopUse(this);
      this.habitatPoint = (BirdFlockHabitatPoint) null;
    }

    public override bool BodyEnabled
    {
      get
      {
        if (((IReadOnlyList<WildBirdFlock.Bird>) this.birds).IsNullOrEmpty<WildBirdFlock.Bird>())
          return false;
        foreach (WildBirdFlock.Bird bird in this.birds)
        {
          if (bird != null && bird.BodyEnabled)
            return true;
        }
        return false;
      }
      set
      {
        if (((IReadOnlyList<WildBirdFlock.Bird>) this.birds).IsNullOrEmpty<WildBirdFlock.Bird>())
          return;
        foreach (WildBirdFlock.Bird bird in this.birds)
        {
          if (bird != null)
            bird.BodyEnabled = value;
        }
      }
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      foreach (WildBirdFlock.Bird bird in this.birds)
        bird?.Destroy();
      this.birds.Clear();
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
      {
        this.habitatPoint.StopUse(this);
        this.habitatPoint = (BirdFlockHabitatPoint) null;
      }
      base.OnDestroy();
    }

    [DebuggerHidden]
    private IEnumerator StartBirdFlockFlap(int _birdNum)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new WildBirdFlock.\u003CStartBirdFlockFlap\u003Ec__Iterator0()
      {
        _birdNum = _birdNum,
        \u0024this = this
      };
    }

    protected override void EnterLocomotion()
    {
      this.SetPlayAnimState(AnimationCategoryID.Locomotion, 0);
      IEnumerator _coroutine = this.StartBirdFlockFlap(this.birdNum);
      ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), ((Component) this).get_gameObject()));
    }

    protected override void OnLocomotion()
    {
      this.Position = Vector3.op_Addition(this.Position, Vector3.op_Multiply(Vector3.op_Multiply(this.Forward, this.flapSpeed), Time.get_deltaTime()));
      for (int index = 0; index < this.birds.Count; ++index)
      {
        WildBirdFlock.Bird bird = this.birds[index];
        if ((double) this.deathDistance < (double) Vector3.Distance(bird.Position, bird.startPosition))
        {
          bird.Destroy();
          this.birds.RemoveAt(index);
          --index;
        }
      }
      if (!((IReadOnlyList<WildBirdFlock.Bird>) this.birds).IsNullOrEmpty<WildBirdFlock.Bird>())
        return;
      this.SetState(AnimalState.Destroyed, (Action) null);
    }

    public class Bird
    {
      public Vector3 startPosition = Vector3.get_zero();
      public GameObject obj;
      public Animator animator;
      public Animation animation;
      public Renderer[] bodyRenderers;
      private IDisposable inPlayAnimationDisposable;
      private IDisposable outPlayAnimationDisposable;

      public bool AnimatorEnable
      {
        get
        {
          return Object.op_Inequality((Object) this.animator, (Object) null) && ((Behaviour) this.animator).get_isActiveAndEnabled();
        }
      }

      public bool AnimatorControllerEnable
      {
        get
        {
          return this.AnimatorEnable && Object.op_Inequality((Object) this.animator.get_runtimeAnimatorController(), (Object) null);
        }
      }

      public bool AnimationEnable
      {
        get
        {
          return Object.op_Inequality((Object) this.animation, (Object) null) && ((Behaviour) this.animation).get_isActiveAndEnabled();
        }
      }

      public Vector3 Position
      {
        get
        {
          return Object.op_Equality((Object) this.obj, (Object) null) ? Vector3.get_zero() : this.obj.get_transform().get_position();
        }
        set
        {
          if (Object.op_Equality((Object) this.obj, (Object) null))
            return;
          this.obj.get_transform().set_position(value);
        }
      }

      public Quaternion Rotation
      {
        get
        {
          return Object.op_Equality((Object) this.obj, (Object) null) ? Quaternion.get_identity() : this.obj.get_transform().get_rotation();
        }
        set
        {
          if (Object.op_Equality((Object) this.obj, (Object) null))
            return;
          this.obj.get_transform().set_rotation(value);
        }
      }

      public Vector3 LocalPosition
      {
        get
        {
          return Object.op_Equality((Object) this.obj, (Object) null) ? Vector3.get_zero() : this.obj.get_transform().get_localPosition();
        }
        set
        {
          if (Object.op_Equality((Object) this.obj, (Object) null))
            return;
          this.obj.get_transform().set_localPosition(value);
        }
      }

      public Quaternion LocalRotation
      {
        get
        {
          return Object.op_Equality((Object) this.obj, (Object) null) ? Quaternion.get_identity() : this.obj.get_transform().get_localRotation();
        }
        set
        {
          if (Object.op_Equality((Object) this.obj, (Object) null))
            return;
          this.obj.get_transform().set_localRotation(value);
        }
      }

      public bool BodyEnabled
      {
        get
        {
          if (((IReadOnlyList<Renderer>) this.bodyRenderers).IsNullOrEmpty<Renderer>())
            return false;
          bool flag = false;
          foreach (Renderer bodyRenderer in this.bodyRenderers)
          {
            if (!Object.op_Equality((Object) bodyRenderer, (Object) null))
              flag |= bodyRenderer.get_enabled();
          }
          return flag;
        }
        set
        {
          if (((IReadOnlyList<Renderer>) this.bodyRenderers).IsNullOrEmpty<Renderer>())
            return;
          foreach (Renderer bodyRenderer in this.bodyRenderers)
          {
            if (!Object.op_Equality((Object) bodyRenderer, (Object) null) && bodyRenderer.get_enabled() != value)
              bodyRenderer.set_enabled(value);
          }
        }
      }

      public bool PlayingInAnimation
      {
        get
        {
          return this.inPlayAnimationDisposable != null;
        }
      }

      public bool PlayingOutAnimation
      {
        get
        {
          return this.outPlayAnimationDisposable != null;
        }
      }

      public AnimalPlayState PlayState { get; private set; }

      private Queue<AnimalPlayState.StateInfo> InAnimState { get; } = new Queue<AnimalPlayState.StateInfo>();

      private Queue<AnimalPlayState.StateInfo> OutAnimState { get; } = new Queue<AnimalPlayState.StateInfo>();

      public void StopChangeAnimation()
      {
        if (this.inPlayAnimationDisposable != null)
          this.inPlayAnimationDisposable.Dispose();
        this.inPlayAnimationDisposable = (IDisposable) null;
        if (this.outPlayAnimationDisposable != null)
          this.outPlayAnimationDisposable.Dispose();
        this.outPlayAnimationDisposable = (IDisposable) null;
      }

      public bool SetPlayAnimState(AnimalPlayState _playState)
      {
        this.InAnimState.Clear();
        this.OutAnimState.Clear();
        this.PlayState = _playState;
        if (!this.AnimatorEnable || _playState == null)
          return false;
        this.animator.set_runtimeAnimatorController(_playState.MainStateInfo.Controller);
        AnimalPlayState.PlayStateInfo mainStateInfo = _playState.MainStateInfo;
        if (!((IReadOnlyList<AnimalPlayState.StateInfo>) mainStateInfo.InStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
        {
          foreach (AnimalPlayState.StateInfo inStateInfo in mainStateInfo.InStateInfos)
            this.InAnimState.Enqueue(inStateInfo);
        }
        if (!((IReadOnlyList<AnimalPlayState.StateInfo>) mainStateInfo.OutStateInfos).IsNullOrEmpty<AnimalPlayState.StateInfo>())
        {
          foreach (AnimalPlayState.StateInfo outStateInfo in mainStateInfo.OutStateInfos)
            this.OutAnimState.Enqueue(outStateInfo);
        }
        return true;
      }

      public void StartInAnimation()
      {
        if (!this.AnimatorEnable || this.PlayState == null)
          return;
        this.StopChangeAnimation();
        IEnumerator _coroutine = this.StartAnimationCoroutine(this.InAnimState, this.PlayState.MainStateInfo.InFadeEnable, this.PlayState.MainStateInfo.InFadeSecond);
        this.inPlayAnimationDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), this.obj), (Action<M0>) (_ => {}), (Action) (() => this.inPlayAnimationDisposable = (IDisposable) null));
      }

      public void StartOutAnimation()
      {
        if (!this.AnimatorEnable || this.PlayState == null)
          return;
        this.StopChangeAnimation();
        IEnumerator _coroutine = this.StartAnimationCoroutine(this.OutAnimState, this.PlayState.MainStateInfo.OutFadeEnable, this.PlayState.MainStateInfo.OutFadeSecond);
        this.outPlayAnimationDisposable = ObservableExtensions.Subscribe<Unit>(Observable.TakeUntilDestroy<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false), this.obj), (Action<M0>) (_ => {}), (Action) (() => this.outPlayAnimationDisposable = (IDisposable) null));
      }

      [DebuggerHidden]
      private IEnumerator StartAnimationCoroutine(
        Queue<AnimalPlayState.StateInfo> _states,
        bool _fadeEnable,
        float _fadeSecond)
      {
        // ISSUE: object of a compiler-generated type is created
        return (IEnumerator) new WildBirdFlock.Bird.\u003CStartAnimationCoroutine\u003Ec__Iterator0()
        {
          _states = _states,
          _fadeEnable = _fadeEnable,
          _fadeSecond = _fadeSecond,
          \u0024this = this
        };
      }

      private void SetAnimationSpeed(float _speed)
      {
        if (Object.op_Equality((Object) this.animation, (Object) null))
          return;
        IEnumerator enumerator = this.animation.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            ((AnimationState) enumerator.Current).set_speed(_speed);
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }

      public void SetAnimationSpeed(string _paramName, float _speed)
      {
        if (!_paramName.IsNullOrEmpty() && Object.op_Inequality((Object) this.animator, (Object) null))
          this.animator.SetFloat(_paramName, _speed);
        this.SetAnimationSpeed(_speed);
      }

      public void LoadBody(AssetBundleInfo _assetInfo, Transform _parent)
      {
        this.Destroy();
        GameObject gameObject = CommonLib.LoadAsset<GameObject>((string) _assetInfo.assetbundle, (string) _assetInfo.asset, false, (string) _assetInfo.manifest);
        if (Object.op_Equality((Object) gameObject, (Object) null))
          return;
        MapScene.AddAssetBundlePath((string) _assetInfo.assetbundle, (string) _assetInfo.manifest);
        this.obj = (GameObject) Object.Instantiate<GameObject>((M0) gameObject);
        this.obj.get_transform().SetParent(_parent, false);
        this.animator = (Animator) this.obj.GetComponentInChildren<Animator>(true);
        this.animation = (Animation) this.obj.GetComponentInChildren<Animation>(true);
        this.bodyRenderers = (Renderer[]) this.obj.GetComponentsInChildren<Renderer>(true);
      }

      public void Destroy()
      {
        this.StopChangeAnimation();
        if (Object.op_Inequality((Object) this.obj, (Object) null))
          Object.Destroy((Object) this.obj);
        this.obj = (GameObject) null;
        this.animator = (Animator) null;
        this.animation = (Animation) null;
        this.bodyRenderers = (Renderer[]) null;
      }
    }
  }
}
