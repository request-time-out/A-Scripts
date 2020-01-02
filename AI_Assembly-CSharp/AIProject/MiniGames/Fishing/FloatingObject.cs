// Decompiled with JetBrains decompiler
// Type: AIProject.MiniGames.Fishing.FloatingObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace AIProject.MiniGames.Fishing
{
  public class FloatingObject : MonoBehaviour
  {
    private Collider m_collider;
    private Rigidbody m_rigidbody;
    private Vector3 voxelSize;
    private List<Vector3> voxels;
    private float initialDrag;
    private float initialAngularDrag;
    private bool useWaterBuoyancy_;
    private IDisposable returnPosAngleDisposable;

    public FloatingObject()
    {
      base.\u002Ector();
    }

    public Collider waterCollider { get; set; }

    public FishingManager fishingSystem { get; set; }

    public bool OnWater { get; private set; }

    public Func<Collider, bool> WaterEnterChecker { get; set; }

    public Func<Collider, bool> WaterStayChecker { get; set; }

    public Func<Collider, bool> WaterExitChecker { get; set; }

    public Action<Collider> WaterEnterEvent { get; set; }

    public Action<Collider> WaterStayEvent { get; set; }

    public Action<Collider> WaterExitEvent { get; set; }

    private FishingDefinePack.LureParamGroup Param
    {
      get
      {
        return Singleton<Resources>.Instance.FishingDefinePack.LureParam;
      }
    }

    public bool UseWaterBuoyancy
    {
      get
      {
        return this.useWaterBuoyancy_;
      }
      set
      {
        this.useWaterBuoyancy_ = value;
        if (this.useWaterBuoyancy_)
        {
          this.m_rigidbody = ((Component) this).get_gameObject().GetOrAddComponent<Rigidbody>();
          this.m_rigidbody.set_useGravity(true);
          this.m_rigidbody.set_constraints((RigidbodyConstraints) 122);
          this.initialDrag = this.m_rigidbody.get_drag();
          this.initialAngularDrag = this.m_rigidbody.get_angularDrag();
        }
        else if (Object.op_Inequality((Object) this.m_rigidbody, (Object) null))
        {
          Object.Destroy((Object) this.m_rigidbody);
          this.m_rigidbody = (Rigidbody) null;
        }
        if (Object.op_Inequality((Object) this.fishingSystem, (Object) null))
          ((Collider) this.fishingSystem.WaterBox).set_enabled(this.useWaterBuoyancy_);
        if (this.useWaterBuoyancy_)
        {
          if (this.returnPosAngleDisposable == null)
            return;
          this.returnPosAngleDisposable.Dispose();
        }
        else
        {
          if (this.returnPosAngleDisposable != null)
            return;
          IEnumerator _coroutine = this.ReturnPosAngle(0.25f);
          this.returnPosAngleDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => _coroutine), false));
        }
      }
    }

    [DebuggerHidden]
    private IEnumerator ReturnPosAngle(float _time)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FloatingObject.\u003CReturnPosAngle\u003Ec__Iterator0()
      {
        _time = _time,
        \u0024this = this
      };
    }

    private void Awake()
    {
      this.CreateFloatingObject();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryFixedUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Func<M0, bool>) (_ => this.UseWaterBuoyancy && this.OnWater && !this.voxels.IsNullOrEmpty<Vector3>() && Object.op_Inequality((Object) this.waterCollider, (Object) null))), (Action<M0>) (_ => this.WaterBuoyancyUpdate()));
    }

    private void OnEnable()
    {
      this.CutIntoVoxels();
    }

    private void OnDisable()
    {
      this.UseWaterBuoyancy = false;
    }

    private void CreateFloatingObject()
    {
      this.m_collider = (Collider) ((Component) this).get_gameObject().GetOrAddComponent<SphereCollider>();
      this.m_collider.set_isTrigger(true);
      this.UseWaterBuoyancy = false;
      this.CutIntoVoxels();
    }

    private void WaterBuoyancyUpdate()
    {
      Vector3 vector3 = Vector3.op_Division(this.CalculateMaxBuoyancyForce(), (float) this.voxels.Count);
      Bounds bounds = this.m_collider.get_bounds();
      float num1 = (float) ((Bounds) ref bounds).get_size().y * this.Param.NormalizedVoxelSize;
      float num2 = 0.0f;
      for (int index = 0; index < this.voxels.Count; ++index)
      {
        Vector3 _checkPos = ((Component) this).get_transform().TransformPoint(this.voxels[index]);
        Vector3 _hitPoint;
        float num3 = Mathf.Clamp((float) ((!FishingManager.CheckOnWater(_checkPos, out _hitPoint) ? (double) (float) this.fishingSystem.MoveArea.get_transform().get_position().y : (double) (float) _hitPoint.y) - _checkPos.y + (double) num1 / 2.0) / num1, 0.0f, 1f);
        num2 += num3;
        this.m_rigidbody.AddForceAtPosition(Quaternion.op_Multiply(Quaternion.Slerp(Quaternion.FromToRotation(((Component) this.waterCollider).get_transform().get_up(), Vector3.get_up()), Quaternion.get_identity(), num3), Vector3.op_Multiply(vector3, num3)), _checkPos);
      }
      float num4 = num2 / (float) this.voxels.Count;
      this.m_rigidbody.set_drag(Mathf.Lerp(this.initialDrag, this.Param.DragInWater, num4));
      this.m_rigidbody.set_angularDrag(Mathf.Lerp(this.initialAngularDrag, this.Param.AngularDragInWater, num4));
    }

    private void CutIntoVoxels()
    {
      Quaternion rotation = ((Component) this).get_transform().get_rotation();
      ((Component) this).get_transform().set_rotation(Quaternion.get_identity());
      Bounds bounds = this.m_collider.get_bounds();
      this.voxelSize = Vector3.op_Multiply(((Bounds) ref bounds).get_size(), this.Param.NormalizedVoxelSize);
      int num1 = Mathf.RoundToInt(1f / this.Param.NormalizedVoxelSize);
      this.voxels = new List<Vector3>(num1 * num1 * num1);
      int mask = LayerMask.GetMask(new string[1]
      {
        LayerMask.LayerToName(((Component) this).get_gameObject().get_layer())
      });
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
        {
          for (int index3 = 0; index3 < num1; ++index3)
          {
            float num2 = (float) (((Bounds) ref bounds).get_min().x + this.voxelSize.x * (0.5 + (double) index1));
            float num3 = (float) (((Bounds) ref bounds).get_min().y + this.voxelSize.y * (0.5 + (double) index2));
            float num4 = (float) (((Bounds) ref bounds).get_min().z + this.voxelSize.z * (0.5 + (double) index3));
            Vector3 _point;
            ((Vector3) ref _point).\u002Ector(num2, num3, num4);
            if (this.IsPointInsideCollider(_point, this.m_collider, ref bounds, mask))
              this.voxels.Add(((Component) this).get_transform().InverseTransformPoint(_point));
          }
        }
      }
      ((Component) this).get_transform().set_rotation(rotation);
    }

    private Vector3 CalculateMaxBuoyancyForce()
    {
      return Vector3.op_Multiply(this.Param.WaterDensity * (this.m_rigidbody.get_mass() / this.Param.Density), Vector3.op_UnaryNegation(Physics.get_gravity()));
    }

    private bool IsPointInsideCollider(
      Vector3 _point,
      Collider _collider,
      ref Bounds _colliderBounds,
      int _layerMask)
    {
      Vector3 size = ((Bounds) ref _colliderBounds).get_size();
      float magnitude = ((Vector3) ref size).get_magnitude();
      Ray ray;
      ((Ray) ref ray).\u002Ector(_point, Vector3.op_Subtraction(((Component) _collider).get_transform().get_position(), _point));
      RaycastHit[] raycastHitArray = new RaycastHit[3];
      int num = Physics.RaycastNonAlloc(ray, raycastHitArray, magnitude, _layerMask);
      if (0 < num)
      {
        for (int index = 0; index < num; ++index)
        {
          RaycastHit raycastHit = raycastHitArray[index];
          if (Object.op_Equality((Object) ((RaycastHit) ref raycastHit).get_collider(), (Object) _collider))
            return false;
        }
      }
      return true;
    }

    private void OnTriggerEnter(Collider other)
    {
      Func<Collider, bool> waterEnterChecker = this.WaterEnterChecker;
      bool? nullable = waterEnterChecker != null ? new bool?(waterEnterChecker(other)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      Action<Collider> waterEnterEvent = this.WaterEnterEvent;
      if (waterEnterEvent != null)
        waterEnterEvent(other);
      this.OnWater = true;
    }

    private void OnTriggerStay(Collider other)
    {
      if (this.OnWater)
        return;
      Func<Collider, bool> waterStayChecker = this.WaterStayChecker;
      bool? nullable = waterStayChecker != null ? new bool?(waterStayChecker(other)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      Action<Collider> waterStayEvent = this.WaterStayEvent;
      if (waterStayEvent != null)
        waterStayEvent(other);
      this.OnWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
      Func<Collider, bool> waterExitChecker = this.WaterExitChecker;
      bool? nullable = waterExitChecker != null ? new bool?(waterExitChecker(other)) : new bool?();
      if ((!nullable.HasValue ? 0 : (nullable.Value ? 1 : 0)) == 0)
        return;
      Action<Collider> waterExitEvent = this.WaterExitEvent;
      if (waterExitEvent != null)
        waterExitEvent(other);
      this.OnWater = false;
    }
  }
}
