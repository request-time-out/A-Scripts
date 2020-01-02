// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.WildButterfly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject.Animal
{
  public class WildButterfly : ButterflyBase
  {
    [SerializeField]
    [Tooltip("移動速度")]
    private float moveSpeed = 1f;
    [SerializeField]
    [Tooltip("回転速度")]
    private float addAngle = 90f;
    [SerializeField]
    [Tooltip("回転する角度の範囲")]
    private float turnAngle = 170f;
    [SerializeField]
    [Tooltip("角度が付きすぎている時,距離がこの距離以内の時減速する")]
    private float speedDownDistance = 2f;
    [SerializeField]
    [Tooltip("次のターゲット座標に切り替わる距離")]
    private float changeTargetDistance = 1f;
    [SerializeField]
    private float nextPointMaxDistance = 5f;
    private List<Vector3> pointList = new List<Vector3>();
    [SerializeField]
    [ReadOnly]
    [HideInEditorMode]
    private ButterflyHabitatPoint habitatPoint;
    private Animation animation;
    private IDisposable addWaypointDisposable;
    private bool forcedDepop;

    private Vector3? TargetPoint { get; set; }

    public bool IsReached
    {
      get
      {
        return !this.TargetPoint.HasValue || (double) Vector3.Distance(this.Position, this.TargetPoint.Value) <= (double) this.changeTargetDistance;
      }
    }

    public override bool ParamRisePossible
    {
      get
      {
        return false;
      }
    }

    public void Initialize(ButterflyHabitatPoint _habitatPoint)
    {
      this.Clear();
      if (Object.op_Equality((Object) (this.habitatPoint = _habitatPoint), (Object) null))
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        this.habitatPoint.SetUse(this);
        if (!this.habitatPoint.Available)
        {
          this.SetState(AnimalState.Destroyed, (Action) null);
        }
        else
        {
          MapArea ownerArea = this.habitatPoint.OwnerArea;
          this.ChunkID = !Object.op_Inequality((Object) ownerArea, (Object) null) ? 0 : ownerArea.ChunkID;
          this.LoadBody();
          this.SetStateData();
          this.animation = (Animation) ((Component) this).GetComponentInChildren<Animation>(true);
          string _paramName = string.Empty;
          float _speed = 1f;
          if (Singleton<Resources>.IsInstance() && Object.op_Inequality((Object) Singleton<Resources>.Instance.AnimalDefinePack, (Object) null))
          {
            AnimalDefinePack.AnimatorInfoGroup animatorInfo = Singleton<Resources>.Instance.AnimalDefinePack.AnimatorInfo;
            if (animatorInfo != null)
            {
              _paramName = animatorInfo.AnimationSpeedParamName;
              _speed = animatorInfo.ButterflyAnimationSpeed;
            }
          }
          this.SetAnimationSpeed(_paramName, _speed);
          Vector3 position1 = this.habitatPoint.DepopPoint.get_position();
          Vector3 position2 = this.habitatPoint.ViaPoint.get_position();
          float viaPointRadius = this.habitatPoint.ViaPointRadius;
          Vector3 randomMoveAreaPoint = this.GetRandomMoveAreaPoint();
          Vector3 vector3_1 = randomMoveAreaPoint;
          vector3_1.y = position2.y;
          Vector3 vector3_2 = Vector3.ClampMagnitude(Vector3.op_Subtraction(vector3_1, position2), viaPointRadius);
          Vector3 vector3_3 = Vector3.op_Addition(position2, vector3_2);
          this.pointList.Clear();
          this.pointList.Add(vector3_3);
          this.pointList.Add(randomMoveAreaPoint);
          randomMoveAreaPoint.y = position1.y;
          this.Rotation = Quaternion.LookRotation(Vector3.op_Subtraction(randomMoveAreaPoint, position1), Vector3.get_up());
          this.Position = position1;
          bool flag = false;
          this.MarkerEnabled = flag;
          this.BodyEnabled = flag;
          this.SetState(AnimalState.Start, (Action) null);
        }
      }
    }

    public override void Clear()
    {
      this.TargetPoint = new Vector3?();
      base.Clear();
    }

    protected override void OnDestroy()
    {
      this.Active = false;
      if (Object.op_Inequality((Object) this.habitatPoint, (Object) null))
      {
        this.habitatPoint.StopUse(this);
        this.habitatPoint = (ButterflyHabitatPoint) null;
      }
      base.OnDestroy();
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
      if (!_paramName.IsNullOrEmpty() && this.AnimatorEnabled)
        this.animator.SetFloat(_paramName, _speed);
      this.SetAnimationSpeed(_speed);
    }

    public void StartAddWaypoint()
    {
      this.StopAddWaypoint();
      this.addWaypointDisposable = ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeWhile<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => this.CurrentState == AnimalState.Locomotion)), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.AddWaypoint()));
    }

    private void AddWaypoint()
    {
      if (3 <= this.pointList.Count)
        return;
      Vector3 vector3 = !((IReadOnlyList<Vector3>) this.pointList).IsNullOrEmpty<Vector3>() ? this.pointList.Back<Vector3>() : this.Position;
      Vector3 randomMoveAreaPoint = this.GetRandomMoveAreaPoint();
      float num = Vector3.Distance(randomMoveAreaPoint, vector3);
      if ((double) this.nextPointMaxDistance >= (double) num || (double) this.GetDiameter(this.moveSpeed, this.addAngle) >= (double) num)
        return;
      this.pointList.Add(randomMoveAreaPoint);
    }

    public void StopAddWaypoint()
    {
      if (this.addWaypointDisposable != null)
        this.addWaypointDisposable.Dispose();
      this.addWaypointDisposable = (IDisposable) null;
    }

    private void FlyUpdate()
    {
      if (this.IsReached)
        this.TargetPoint = new Vector3?();
      bool hasValue = this.TargetPoint.HasValue;
      bool flag = ((IReadOnlyList<Vector3>) this.pointList).IsNullOrEmpty<Vector3>();
      if (!hasValue && flag)
        return;
      if (!hasValue && !flag)
        this.TargetPoint = new Vector3?(((IList<Vector3>) this.pointList).PopFront<Vector3>());
      if (!this.TargetPoint.HasValue)
        return;
      Vector3 vector3_1 = this.TargetPoint.Value;
      float num1 = Vector3.SignedAngle(this.Forward, Vector3.op_Subtraction(vector3_1, this.Position), Vector3.get_up());
      float deltaTime = Time.get_deltaTime();
      float num2 = Vector3.Distance(vector3_1, this.Position);
      float num3 = this.moveSpeed * deltaTime;
      float num4 = this.addAngle * deltaTime;
      float num5 = Mathf.Min(Mathf.Abs(num1), num4);
      Vector3 vector3_2 = Vector3.get_zero();
      Quaternion.get_identity();
      this.Rotation = Quaternion.op_Multiply(this.Rotation, Quaternion.Euler(0.0f, num5 * Mathf.Sign(num1), 0.0f));
      if ((double) Mathf.Abs(num1) <= (double) this.turnAngle)
      {
        if ((double) num2 < (double) this.speedDownDistance)
          num3 *= (float) (1.0 - (double) Mathf.Abs(num1) / (double) this.turnAngle);
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, this.Position);
        Vector3 vector3_4 = Vector3.op_Multiply(((Vector3) ref vector3_3).get_normalized(), num3);
        vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, Vector3.SignedAngle(vector3_3, this.Forward, Vector3.get_up()), 0.0f), vector3_4);
      }
      this.Position = Vector3.op_Addition(this.Position, vector3_2);
    }

    public override void OnTimeZoneChanged(EnvironmentSimulator _simulator)
    {
      bool flag = _simulator.Weather == Weather.Rain || _simulator.Weather == Weather.Storm;
      if (_simulator.TimeZone == AIProject.TimeZone.Night)
      {
        if (this.CurrentState == AnimalState.Depop)
          return;
        this.SetState(AnimalState.Depop, (Action) null);
      }
      else
      {
        if (this.CurrentState != AnimalState.Depop || flag || this.forcedDepop)
          return;
        this.SetState(AnimalState.Locomotion, (Action) null);
      }
    }

    public override void OnWeatherChanged(EnvironmentSimulator _simulator)
    {
      bool flag = _simulator.TimeZone == AIProject.TimeZone.Night;
      switch (_simulator.Weather)
      {
        case Weather.Rain:
        case Weather.Storm:
          if (this.CurrentState == AnimalState.Depop)
            break;
          this.SetState(AnimalState.Depop, (Action) null);
          break;
        default:
          if (this.CurrentState != AnimalState.Depop || flag || this.forcedDepop)
            break;
          this.SetState(AnimalState.Locomotion, (Action) null);
          break;
      }
    }

    public void ForcedLocomotion()
    {
      this.forcedDepop = false;
      if (this.CurrentState != AnimalState.Depop)
        return;
      this.SetState(AnimalState.Locomotion, (Action) null);
    }

    public void ForcedDepop()
    {
      this.forcedDepop = true;
      if (this.CurrentState == AnimalState.Depop || this.CurrentState == AnimalState.Destroyed)
        return;
      this.SetState(AnimalState.Depop, (Action) null);
    }

    protected override void OnStart()
    {
      this.PlayParticle();
      bool flag = true;
      this.MarkerEnabled = flag;
      this.BodyEnabled = flag;
      this.Active = true;
      this.SetState(AnimalState.Locomotion, (Action) null);
    }

    protected override void EnterLocomotion()
    {
      this.PlayInAnim(AnimationCategoryID.Locomotion, 0, (Action) null);
      this.StartAddWaypoint();
    }

    protected override void OnLocomotion()
    {
      this.FlyUpdate();
    }

    protected override void ExitLocomotion()
    {
    }

    protected override void EnterDepop()
    {
      if (Object.op_Equality((Object) this.habitatPoint, (Object) null) || !this.habitatPoint.Available)
      {
        this.SetState(AnimalState.Destroyed, (Action) null);
      }
      else
      {
        this.pointList.Clear();
        Vector3 position1 = this.habitatPoint.ViaPoint.get_position();
        float viaPointRadius = this.habitatPoint.ViaPointRadius;
        Vector3 position2 = this.Position;
        position2.y = position1.y;
        Vector3 vector3 = Vector3.ClampMagnitude(Vector3.op_Subtraction(position2, position1), viaPointRadius);
        this.TargetPoint = new Vector3?(Vector3.op_Addition(position1, vector3));
        this.pointList.Add(this.habitatPoint.DepopPoint.get_position());
      }
    }

    protected override void OnDepop()
    {
      if (((IReadOnlyList<Vector3>) this.pointList).IsNullOrEmpty<Vector3>() && !this.TargetPoint.HasValue)
        this.SetState(AnimalState.Destroyed, (Action) null);
      else
        this.FlyUpdate();
    }

    protected override void ExitDepop()
    {
      this.TargetPoint = new Vector3?();
      this.pointList.Clear();
    }

    public float GetDiameter(float _moveSpeed, float _addAngle)
    {
      return (double) _moveSpeed == 0.0 || (double) _addAngle == 0.0 ? float.PositiveInfinity : (float) (360.0 / ((double) _addAngle / (double) _moveSpeed) / 3.14159274101257);
    }

    public Vector3 GetRandomMoveAreaPoint()
    {
      if (Object.op_Equality((Object) this.habitatPoint, (Object) null) || Object.op_Equality((Object) this.habitatPoint.Center, (Object) null))
        return Vector3.get_zero();
      float moveHeight = this.habitatPoint.MoveHeight;
      float moveRadius = this.habitatPoint.MoveRadius;
      float num = Random.Range((float) (-(double) moveHeight / 2.0), moveHeight / 2f);
      Vector3 randomPosOnCircle = this.GetRandomPosOnCircle(moveRadius);
      randomPosOnCircle.y = (__Null) (double) num;
      return Vector3.op_Addition(this.habitatPoint.Center.get_position(), randomPosOnCircle);
    }
  }
}
