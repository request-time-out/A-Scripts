// Decompiled with JetBrains decompiler
// Type: AIProject.RootMotion.HandsHolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ReMotion;
using RootMotion.FinalIK;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.RootMotion
{
  public class HandsHolder : MonoBehaviour
  {
    [SerializeField]
    private Animator _rightHandAnimator;
    [SerializeField]
    private Animator _leftHandAnimator;
    [SerializeField]
    private FullBodyBipedIK _rightHandIK;
    [SerializeField]
    private FullBodyBipedIK _leftHandIK;
    [SerializeField]
    private Transform _rightHandTarget;
    [SerializeField]
    private Transform _leftHandTarget;
    [SerializeField]
    private Transform _rightElboTarget;
    [SerializeField]
    private Transform _baseTransform;
    [SerializeField]
    private Transform _targetTransform;
    private bool _enabledTarget;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _crossFade;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _effectiveDistance;
    [SerializeField]
    private Vector3 _offset;
    private float _weight;
    private bool _enabledHolding;
    private IDisposable _disposable;
    private Subject<Unit> _update;
    private Subject<Unit> _beforeLateUpdate;
    private Subject<Unit> _afterLateUpdate;

    public HandsHolder()
    {
      base.\u002Ector();
    }

    public Animator RightHandAnimator
    {
      get
      {
        return this._rightHandAnimator;
      }
      set
      {
        this._rightHandAnimator = value;
      }
    }

    public Animator LeftHandAnimator
    {
      get
      {
        return this._leftHandAnimator;
      }
      set
      {
        this._leftHandAnimator = value;
      }
    }

    public FullBodyBipedIK RightHandIK
    {
      get
      {
        return this._rightHandIK;
      }
      set
      {
        this._rightHandIK = value;
      }
    }

    public FullBodyBipedIK LeftHandIK
    {
      get
      {
        return this._leftHandIK;
      }
      set
      {
        this._leftHandIK = value;
      }
    }

    public Transform RightHandTarget
    {
      get
      {
        return this._rightHandTarget;
      }
      set
      {
        this._rightHandTarget = value;
      }
    }

    public Transform LeftHandTarget
    {
      get
      {
        return this._leftHandTarget;
      }
      set
      {
        this._leftHandTarget = value;
      }
    }

    public Transform RightElboTarget
    {
      get
      {
        return this._rightElboTarget;
      }
      set
      {
        this._rightElboTarget = value;
      }
    }

    public Transform RightLookTarget { get; set; }

    public Transform LeftLookTarget { get; set; }

    public Transform BaseTransform
    {
      get
      {
        return this._baseTransform;
      }
    }

    public Transform TargetTransform
    {
      get
      {
        return this._targetTransform;
      }
    }

    public float CrossFade
    {
      get
      {
        return this._crossFade;
      }
      set
      {
        this._crossFade = value;
      }
    }

    public float Speed
    {
      get
      {
        return this._speed;
      }
      set
      {
        this._speed = value;
      }
    }

    public float EffectiveDistance
    {
      get
      {
        return this._effectiveDistance;
      }
      set
      {
        this._effectiveDistance = value;
      }
    }

    public float MinDistance { get; set; }

    public bool EnabledHolding
    {
      get
      {
        return this._enabledHolding;
      }
      set
      {
        if (this._enabledHolding == value)
          return;
        this._enabledHolding = value;
        if (this._disposable != null)
          this._disposable.Dispose();
        float startWeight = this._weight;
        float duration = (float) ((!value ? (double) startWeight : 1.0 - (double) startWeight) * 0.5);
        if ((double) duration <= 0.0)
          return;
        if (!value)
          this._enabledTarget = false;
        this._disposable = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.Linear(duration, false), false), (Action<M0>) (x =>
        {
          float num = !value ? Mathf.Lerp(startWeight, 0.0f, ((TimeInterval<float>) ref x).get_Value()) : Mathf.Lerp(startWeight, 1f, ((TimeInterval<float>) ref x).get_Value());
          if (Object.op_Inequality((Object) this._rightHandAnimator, (Object) null))
            this._rightHandAnimator.SetLayerWeight(2, num);
          this._leftHandAnimator.SetLayerWeight(3, num);
          this._weight = num;
          if (!Object.op_Inequality((Object) this._rightHandIK, (Object) null))
            return;
          ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightHandEffector().positionWeight = (__Null) (double) num;
          ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightHandEffector().rotationWeight = (__Null) (double) num;
          if (!Object.op_Inequality((Object) this._rightElboTarget, (Object) null))
            return;
          ((IKConstraintBend) ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightArmChain().bendConstraint).weight = (__Null) (double) num;
        }), (Action) (() =>
        {
          if (!value)
            return;
          this._enabledTarget = true;
        }));
      }
    }

    public float Weight
    {
      get
      {
        return this._weight;
      }
      set
      {
        this._weight = value;
      }
    }

    public IObservable<Unit> OnUpdateAsOsbervable()
    {
      return (IObservable<Unit>) this._update ?? (IObservable<Unit>) (this._update = new Subject<Unit>());
    }

    public IObservable<Unit> OnBeforeLateUpdateAsObservable()
    {
      return (IObservable<Unit>) this._beforeLateUpdate ?? (IObservable<Unit>) (this._beforeLateUpdate = new Subject<Unit>());
    }

    public IObservable<Unit> OnAfterLateUpdateAsObservable()
    {
      return (IObservable<Unit>) this._afterLateUpdate ?? (IObservable<Unit>) (this._afterLateUpdate = new Subject<Unit>());
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this.RightLookTarget, (Object) null) || Object.op_Equality((Object) this.LeftLookTarget, (Object) null))
      {
        this.EnabledHolding = false;
      }
      else
      {
        bool flag1 = (double) Vector3.Distance(((Component) this._rightHandIK).get_transform().get_position(), ((Component) this._leftHandIK).get_transform().get_position()) > (double) this._effectiveDistance;
        Vector3 position1 = ((Component) this._rightHandIK).get_transform().get_position();
        position1.y = (__Null) 0.0;
        Vector3 position2 = ((Component) this._leftHandIK).get_transform().get_position();
        position2.y = (__Null) 0.0;
        Vector3 vector3 = Vector3.Normalize(Vector3.op_Subtraction(position1, position2));
        Quaternion rotation1 = Quaternion.FromToRotation(vector3, ((Component) this._rightHandIK).get_transform().get_forward());
        Quaternion rotation2 = Quaternion.FromToRotation(vector3, ((Component) this._leftHandIK).get_transform().get_forward());
        bool flag2 = ((Quaternion) ref rotation1).get_eulerAngles().y < 30.0 || ((Quaternion) ref rotation1).get_eulerAngles().y > 150.0;
        bool flag3 = ((Quaternion) ref rotation1).get_eulerAngles().y < 30.0 || ((Quaternion) ref rotation2).get_eulerAngles().y > 150.0;
        Quaternion rotation3 = Quaternion.FromToRotation(((Component) this._rightHandIK).get_transform().get_forward(), ((Component) this._leftHandIK).get_transform().get_forward());
        float num = (float) ((Quaternion) ref rotation3).get_eulerAngles().y;
        if ((double) num > 180.0)
          num = (float) (360.0 - ((Quaternion) ref rotation3).get_eulerAngles().y);
        bool flag4 = (double) num > 60.0;
        this.EnabledHolding = (flag1 || flag2 || flag3 ? 1 : (flag4 ? 1 : 0)) == 0;
        if (this._update == null)
          return;
        this._update.OnNext(Unit.get_Default());
      }
    }

    private void LateUpdate()
    {
      if (Object.op_Equality((Object) this.RightLookTarget, (Object) null) || Object.op_Equality((Object) this.LeftLookTarget, (Object) null))
        return;
      if (this._beforeLateUpdate != null)
        this._beforeLateUpdate.OnNext(Unit.get_Default());
      if (this._enabledTarget)
        this._targetTransform.set_rotation(this._rightHandTarget.get_rotation());
      if ((double) this._weight > 0.0)
      {
        ((Transform) ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightHandEffector().target).set_position(this._rightHandTarget.get_position());
        ((Transform) ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightHandEffector().target).set_rotation(this._targetTransform.get_rotation());
        if (Object.op_Inequality((Object) this._rightElboTarget, (Object) null))
          ((Transform) ((IKConstraintBend) ((IKSolverFullBodyBiped) this._rightHandIK.solver).get_rightArmChain().bendConstraint).bendGoal).set_position(this._rightElboTarget.get_position());
      }
      if (this._afterLateUpdate == null)
        return;
      this._afterLateUpdate.OnNext(Unit.get_Default());
    }
  }
}
