// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AloneButterfly
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
  public class AloneButterfly : SerializedMonoBehaviour
  {
    private List<Vector3> _pointList;
    private Vector3? TargetPoint;
    private Animation _animation;
    private GameObject _bodyObject;

    public AloneButterfly()
    {
      base.\u002Ector();
    }

    public AloneButterflyHabitatPoint _habitatPoint { get; set; }

    public bool IsReached
    {
      get
      {
        return !this.TargetPoint.HasValue || (double) Vector3.Distance(this.Position, this.TargetPoint.Value) <= (double) this._habitatPoint.ChangeTargetDistance;
      }
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
      set
      {
        ((Component) this).get_transform().set_position(value);
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this).get_transform().get_rotation();
      }
      set
      {
        ((Component) this).get_transform().set_rotation(value);
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this).get_transform().get_forward();
      }
    }

    public void SetAnimationSpeed(float speed)
    {
      if (!Object.op_Inequality((Object) this._animation, (Object) null) || !((Behaviour) this._animation).get_isActiveAndEnabled())
        return;
      IEnumerator enumerator = this._animation.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          ((AnimationState) enumerator.Current).set_speed(speed);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public void Initialize(AloneButterflyHabitatPoint habitatPoint, GameObject prefabObj)
    {
      if (Object.op_Equality((Object) habitatPoint, (Object) null))
        return;
      this._habitatPoint = habitatPoint;
      ObservableExtensions.Subscribe<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromSeconds((double) Random.Range(0.0f, this._habitatPoint.MaxDelayTime))), (Component) habitatPoint), (Component) this), (Action<M0>) (_ =>
      {
        this._bodyObject = (GameObject) Object.Instantiate<GameObject>((M0) prefabObj, ((Component) this).get_transform());
        Transform transform = this._bodyObject.get_transform();
        Vector3 zero = Vector3.get_zero();
        this._bodyObject.get_transform().set_localEulerAngles(zero);
        Vector3 vector3 = zero;
        transform.set_localPosition(vector3);
        this._bodyObject.SetActive(true);
        AnimalDefinePack animalDefinePack = Singleton<Resources>.Instance.AnimalDefinePack;
        float speed = !Object.op_Inequality((Object) animalDefinePack, (Object) null) ? 1f : animalDefinePack.AnimatorInfo.ButterflyAnimationSpeed;
        this._animation = (Animation) ((Component) this).GetComponentInChildren<Animation>(true);
        this.SetAnimationSpeed(speed);
        this.Position = this.GetRandomMoveAreaPoint();
        ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this._habitatPoint), (Component) this), (Func<M0, bool>) (__ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (__ =>
        {
          this.AddWaypoint();
          this.OnUpdate();
        }));
      }));
    }

    public float GetDiameter(float moveSpeed, float addAngle)
    {
      return (double) moveSpeed == 0.0 || (double) addAngle == 0.0 ? float.PositiveInfinity : (float) (360.0 / ((double) addAngle / (double) moveSpeed) / 3.14159274101257);
    }

    protected Vector3 GetRandomPosOnCircle(float radius)
    {
      float num1 = (float) ((double) Random.get_value() * 3.14159274101257 * 2.0);
      float num2 = radius * Mathf.Sqrt(Random.get_value());
      return new Vector3(num2 * Mathf.Cos(num1), 0.0f, num2 * Mathf.Sin(num1));
    }

    private Vector3 GetRandomMoveAreaPoint()
    {
      if (Object.op_Equality((Object) this._habitatPoint, (Object) null) || Object.op_Equality((Object) this._habitatPoint.Center, (Object) null))
        return Vector3.get_zero();
      float moveHeight = this._habitatPoint.MoveHeight;
      float moveRadius = this._habitatPoint.MoveRadius;
      float num = Random.Range((float) (-(double) moveHeight / 2.0), moveHeight / 2f);
      Vector3 randomPosOnCircle = this.GetRandomPosOnCircle(moveRadius);
      randomPosOnCircle.y = (__Null) (double) num;
      return Vector3.op_Addition(this._habitatPoint.Center.get_position(), randomPosOnCircle);
    }

    private void AddWaypoint()
    {
      if (3 <= this._pointList.Count)
        return;
      Vector3 vector3 = !((IReadOnlyList<Vector3>) this._pointList).IsNullOrEmpty<Vector3>() ? this._pointList.Back<Vector3>() : this.Position;
      Vector3 randomMoveAreaPoint = this.GetRandomMoveAreaPoint();
      float num = Vector3.Distance(randomMoveAreaPoint, vector3);
      if ((double) this._habitatPoint.NextPointMaxDistance >= (double) num || (double) this.GetDiameter(this._habitatPoint.MoveSpeed, this._habitatPoint.AddAngle) >= (double) num)
        return;
      this._pointList.Add(randomMoveAreaPoint);
    }

    private void OnUpdate()
    {
      if (this.IsReached)
        this.TargetPoint = new Vector3?();
      bool hasValue = this.TargetPoint.HasValue;
      bool flag = ((IReadOnlyList<Vector3>) this._pointList).IsNullOrEmpty<Vector3>();
      if (!hasValue && flag)
        return;
      if (!hasValue && !flag)
        this.TargetPoint = new Vector3?(((IList<Vector3>) this._pointList).PopFront<Vector3>());
      if (!this.TargetPoint.HasValue)
        return;
      Vector3 vector3_1 = this.TargetPoint.Value;
      float num1 = Vector3.SignedAngle(this.Forward, Vector3.op_Subtraction(vector3_1, this.Position), Vector3.get_up());
      float deltaTime = Time.get_deltaTime();
      float num2 = Vector3.Distance(vector3_1, this.Position);
      float num3 = this._habitatPoint.MoveSpeed * deltaTime;
      float num4 = this._habitatPoint.AddAngle * deltaTime;
      float num5 = Mathf.Min(Mathf.Abs(num1), num4);
      Vector3 vector3_2 = Vector3.get_zero();
      Quaternion.get_identity();
      this.Rotation = Quaternion.op_Multiply(this.Rotation, Quaternion.Euler(0.0f, num5 * Mathf.Sign(num1), 0.0f));
      if ((double) Mathf.Abs(num1) <= (double) this._habitatPoint.TurnAngle)
      {
        if ((double) num2 < (double) this._habitatPoint.SpeedDownDistance)
          num3 *= (float) (1.0 - (double) Mathf.Abs(num1) / (double) this._habitatPoint.TurnAngle);
        Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, this.Position);
        Vector3 vector3_4 = Vector3.op_Multiply(((Vector3) ref vector3_3).get_normalized(), num3);
        vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, Vector3.SignedAngle(vector3_3, this.Forward, Vector3.get_up()), 0.0f), vector3_4);
      }
      this.Position = Vector3.op_Addition(this.Position, vector3_2);
    }
  }
}
