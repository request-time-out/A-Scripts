// Decompiled with JetBrains decompiler
// Type: DynamicBoneV2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone V2")]
public class DynamicBoneV2 : MonoBehaviour
{
  public Transform m_Root;
  public float m_UpdateRate;
  public DynamicBoneV2.UpdateMode m_UpdateMode;
  [Range(0.0f, 1f)]
  public float m_Damping;
  public AnimationCurve m_DampingDistrib;
  [Range(0.0f, 1f)]
  public float m_Elasticity;
  public AnimationCurve m_ElasticityDistrib;
  [Range(0.0f, 1f)]
  public float m_Stiffness;
  public AnimationCurve m_StiffnessDistrib;
  [Range(0.0f, 1f)]
  public float m_Inert;
  public AnimationCurve m_InertDistrib;
  public float m_Radius;
  public AnimationCurve m_RadiusDistrib;
  public float m_EndLength;
  public Vector3 m_EndOffset;
  public Vector3 m_Gravity;
  public Vector3 m_Force;
  public List<DynamicBoneCollider> m_Colliders;
  public List<Transform> m_Exclusions;
  public DynamicBoneV2.FreezeAxis m_FreezeAxis;
  public bool m_DistantDisable;
  public Transform m_ReferenceObject;
  public float m_DistanceToObject;
  public List<Transform> m_notRolls;
  private Vector3 m_LocalGravity;
  private Vector3 m_ObjectMove;
  private Vector3 m_ObjectPrevPosition;
  private float m_BoneTotalLength;
  private float m_ObjectScale;
  private float m_Time;
  private float m_Weight;
  private bool m_DistantDisabled;
  private List<DynamicBoneV2.Particle> m_Particles;

  public DynamicBoneV2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SetupParticles();
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryLateUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnLateUpdate()));
  }

  private void FixedUpdate()
  {
    if (this.m_UpdateMode != DynamicBoneV2.UpdateMode.AnimatePhysics)
      return;
    this.PreUpdate();
  }

  private void OnUpdate()
  {
    if (this.m_UpdateMode == DynamicBoneV2.UpdateMode.AnimatePhysics)
      return;
    this.PreUpdate();
  }

  private void OnLateUpdate()
  {
    if (this.m_DistantDisable)
      this.CheckDistance();
    if ((double) this.m_Weight <= 0.0 || this.m_DistantDisable && this.m_DistantDisabled)
      return;
    this.UpdateDynamicBones(Time.get_deltaTime());
  }

  private void PreUpdate()
  {
    if ((double) this.m_Weight <= 0.0 || this.m_DistantDisable && this.m_DistantDisabled)
      return;
    this.InitTransforms();
  }

  private void CheckDistance()
  {
    Transform transform = this.m_ReferenceObject;
    if (Object.op_Equality((Object) transform, (Object) null) && Object.op_Inequality((Object) Camera.get_main(), (Object) null))
      transform = ((Component) Camera.get_main()).get_transform();
    if (!Object.op_Inequality((Object) transform, (Object) null))
      return;
    Vector3 vector3 = Vector3.op_Subtraction(transform.get_position(), ((Component) this).get_transform().get_position());
    bool flag = (double) ((Vector3) ref vector3).get_sqrMagnitude() > (double) this.m_DistanceToObject * (double) this.m_DistanceToObject;
    if (flag == this.m_DistantDisabled)
      return;
    if (!flag)
      this.ResetParticlesPosition();
    this.m_DistantDisabled = flag;
  }

  private void OnEnable()
  {
    this.ResetParticlesPosition();
  }

  private void OnDisable()
  {
    this.InitTransforms();
  }

  private void OnValidate()
  {
    this.m_UpdateRate = Mathf.Max(this.m_UpdateRate, 0.0f);
    this.m_Damping = Mathf.Clamp01(this.m_Damping);
    this.m_Elasticity = Mathf.Clamp01(this.m_Elasticity);
    this.m_Stiffness = Mathf.Clamp01(this.m_Stiffness);
    this.m_Inert = Mathf.Clamp01(this.m_Inert);
    this.m_Radius = Mathf.Max(this.m_Radius, 0.0f);
    if (!Application.get_isEditor() || !Application.get_isPlaying())
      return;
    this.InitTransforms();
    this.SetupParticles();
  }

  private void OnDrawGizmosSelected()
  {
    if (!((Behaviour) this).get_enabled() || Object.op_Equality((Object) this.m_Root, (Object) null))
      return;
    if (Application.get_isEditor() && !Application.get_isPlaying() && ((Component) this).get_transform().get_hasChanged())
    {
      this.InitTransforms();
      this.SetupParticles();
    }
    Gizmos.set_color(Color.get_white());
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index];
      if (particle1.m_ParentIndex >= 0)
      {
        DynamicBoneV2.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
        Gizmos.DrawLine(particle1.m_Position, particle2.m_Position);
      }
      if ((double) particle1.m_Radius > 0.0)
        Gizmos.DrawWireSphere(particle1.m_Position, particle1.m_Radius * this.m_ObjectScale);
    }
  }

  public void SetWeight(float w)
  {
    if ((double) this.m_Weight == (double) w)
      return;
    if ((double) w == 0.0)
      this.InitTransforms();
    else if ((double) this.m_Weight == 0.0)
      this.ResetParticlesPosition();
    this.m_Weight = w;
  }

  public float GetWeight()
  {
    return this.m_Weight;
  }

  private void UpdateDynamicBones(float t)
  {
    if (Object.op_Equality((Object) this.m_Root, (Object) null))
      return;
    this.m_ObjectScale = Mathf.Abs((float) ((Component) this).get_transform().get_lossyScale().x);
    this.m_ObjectMove = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this.m_ObjectPrevPosition);
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
    int num1 = 1;
    if ((double) this.m_UpdateRate > 0.0)
    {
      float num2 = 1f / this.m_UpdateRate;
      this.m_Time += t;
      num1 = 0;
      while ((double) this.m_Time >= (double) num2)
      {
        this.m_Time -= num2;
        if (++num1 >= 3)
        {
          this.m_Time = 0.0f;
          break;
        }
      }
    }
    if (num1 > 0)
    {
      for (int index = 0; index < num1; ++index)
      {
        this.UpdateParticles1();
        this.UpdateParticles2();
        this.m_ObjectMove = Vector3.get_zero();
      }
    }
    else
      this.SkipUpdateParticles();
    this.ApplyParticlesToTransforms();
  }

  private void SetupParticles()
  {
    this.m_Particles.Clear();
    if (Object.op_Equality((Object) this.m_Root, (Object) null))
      return;
    this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
    this.m_ObjectScale = Mathf.Abs((float) ((Component) this).get_transform().get_lossyScale().x);
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
    this.m_ObjectMove = Vector3.get_zero();
    this.m_BoneTotalLength = 0.0f;
    this.AppendParticles(this.m_Root, -1, 0.0f);
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle = this.m_Particles[index];
      particle.m_Damping = this.m_Damping;
      particle.m_Elasticity = this.m_Elasticity;
      particle.m_Stiffness = this.m_Stiffness;
      particle.m_Inert = this.m_Inert;
      particle.m_Radius = this.m_Radius;
      if ((double) this.m_BoneTotalLength > 0.0)
      {
        float num = particle.m_BoneLength / this.m_BoneTotalLength;
        if (this.m_DampingDistrib != null && this.m_DampingDistrib.get_keys().Length > 0)
          particle.m_Damping *= this.m_DampingDistrib.Evaluate(num);
        if (this.m_ElasticityDistrib != null && this.m_ElasticityDistrib.get_keys().Length > 0)
          particle.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(num);
        if (this.m_StiffnessDistrib != null && this.m_StiffnessDistrib.get_keys().Length > 0)
          particle.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(num);
        if (this.m_InertDistrib != null && this.m_InertDistrib.get_keys().Length > 0)
          particle.m_Inert *= this.m_InertDistrib.Evaluate(num);
        if (this.m_RadiusDistrib != null && this.m_RadiusDistrib.get_keys().Length > 0)
          particle.m_Radius *= this.m_RadiusDistrib.Evaluate(num);
      }
      particle.m_Damping = Mathf.Clamp01(particle.m_Damping);
      particle.m_Elasticity = Mathf.Clamp01(particle.m_Elasticity);
      particle.m_Stiffness = Mathf.Clamp01(particle.m_Stiffness);
      particle.m_Inert = Mathf.Clamp01(particle.m_Inert);
      particle.m_Radius = Mathf.Max(particle.m_Radius, 0.0f);
    }
  }

  private void AppendParticles(Transform b, int parentIndex, float boneLength)
  {
    DynamicBoneV2.Particle particle = new DynamicBoneV2.Particle();
    particle.m_Transform = b;
    particle.m_ParentIndex = parentIndex;
    if (Object.op_Inequality((Object) b, (Object) null))
    {
      particle.m_Position = particle.m_PrevPosition = b.get_position();
      particle.m_InitLocalPosition = b.get_localPosition();
      particle.m_InitLocalRotation = b.get_localRotation();
    }
    else
    {
      Transform transform = this.m_Particles[parentIndex].m_Transform;
      if ((double) this.m_EndLength > 0.0)
      {
        Transform parent = transform.get_parent();
        particle.m_EndOffset = !Object.op_Inequality((Object) parent, (Object) null) ? new Vector3(this.m_EndLength, 0.0f, 0.0f) : Vector3.op_Multiply(transform.InverseTransformPoint(Vector3.op_Subtraction(Vector3.op_Multiply(transform.get_position(), 2f), parent.get_position())), this.m_EndLength);
      }
      else
        particle.m_EndOffset = transform.InverseTransformPoint(Vector3.op_Addition(((Component) this).get_transform().TransformDirection(this.m_EndOffset), transform.get_position()));
      particle.m_Position = particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset);
    }
    if (parentIndex >= 0)
    {
      double num = (double) boneLength;
      Vector3 vector3 = Vector3.op_Subtraction(this.m_Particles[parentIndex].m_Transform.get_position(), particle.m_Position);
      double magnitude = (double) ((Vector3) ref vector3).get_magnitude();
      boneLength = (float) (num + magnitude);
      particle.m_BoneLength = boneLength;
      this.m_BoneTotalLength = Mathf.Max(this.m_BoneTotalLength, boneLength);
    }
    int count = this.m_Particles.Count;
    this.m_Particles.Add(particle);
    bool flag1 = false;
    int num1 = 0;
    if (!Object.op_Inequality((Object) b, (Object) null))
      return;
    for (int index1 = 0; index1 < b.get_childCount(); ++index1)
    {
      bool flag2 = false;
      if (this.m_Exclusions != null)
      {
        for (int index2 = 0; index2 < this.m_Exclusions.Count; ++index2)
        {
          if (Object.op_Equality((Object) this.m_Exclusions[index2], (Object) b.GetChild(index1)))
          {
            flag2 = true;
            break;
          }
        }
      }
      if (!flag2)
      {
        for (int index2 = 0; index2 < this.m_notRolls.Count; ++index2)
        {
          if (Object.op_Equality((Object) this.m_notRolls[index2], (Object) b.GetChild(index1)))
          {
            flag1 = true;
            flag2 = true;
            num1 = index1;
            break;
          }
        }
      }
      if (!flag2)
        this.AppendParticles(b.GetChild(index1), count, boneLength);
    }
    if (flag1)
    {
      for (int index1 = 0; index1 < b.GetChild(num1).get_childCount(); ++index1)
      {
        bool flag2 = false;
        for (int index2 = 0; index2 < this.m_Exclusions.Count; ++index2)
        {
          if (Object.op_Equality((Object) this.m_Exclusions[index2], (Object) b.GetChild(num1).GetChild(index1)))
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
        {
          for (int index2 = 0; index2 < this.m_notRolls.Count; ++index2)
          {
            if (Object.op_Equality((Object) this.m_notRolls[index2], (Object) b.GetChild(num1).GetChild(index1)))
            {
              flag2 = true;
              break;
            }
          }
        }
        if (!flag2)
          this.AppendParticles(b.GetChild(num1).GetChild(index1), count, boneLength);
      }
    }
    if (b.get_childCount() != 0 || (double) this.m_EndLength <= 0.0 && !Vector3.op_Inequality(this.m_EndOffset, Vector3.get_zero()))
      return;
    this.AppendParticles((Transform) null, count, boneLength);
  }

  private void InitTransforms()
  {
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle = this.m_Particles[index];
      if (Object.op_Inequality((Object) particle.m_Transform, (Object) null))
      {
        particle.m_Transform.set_localPosition(particle.m_InitLocalPosition);
        particle.m_Transform.set_localRotation(particle.m_InitLocalRotation);
      }
    }
  }

  public void ResetParticlesPosition()
  {
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle = this.m_Particles[index];
      if (Object.op_Inequality((Object) particle.m_Transform, (Object) null))
      {
        particle.m_Position = particle.m_PrevPosition = particle.m_Transform.get_position();
      }
      else
      {
        Transform transform = this.m_Particles[particle.m_ParentIndex].m_Transform;
        particle.m_Position = particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset);
      }
    }
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
  }

  private void UpdateParticles1()
  {
    Vector3 gravity = this.m_Gravity;
    Vector3 normalized = ((Vector3) ref this.m_Gravity).get_normalized();
    Vector3 vector3_1 = this.m_Root.TransformDirection(this.m_LocalGravity);
    Vector3 vector3_2 = Vector3.op_Multiply(normalized, Mathf.Max(Vector3.Dot(vector3_1, normalized), 0.0f));
    Vector3 vector3_3 = Vector3.op_Multiply(Vector3.op_Addition(Vector3.op_Subtraction(gravity, vector3_2), this.m_Force), this.m_ObjectScale);
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index];
      if (particle1.m_ParentIndex >= 0)
      {
        Vector3 vector3_4 = Vector3.op_Subtraction(particle1.m_Position, particle1.m_PrevPosition);
        Vector3 vector3_5 = Vector3.op_Multiply(this.m_ObjectMove, particle1.m_Inert);
        particle1.m_PrevPosition = Vector3.op_Addition(particle1.m_Position, vector3_5);
        DynamicBoneV2.Particle particle2 = particle1;
        particle2.m_Position = Vector3.op_Addition(particle2.m_Position, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(vector3_4, 1f - particle1.m_Damping), vector3_3), vector3_5));
      }
      else
      {
        particle1.m_PrevPosition = particle1.m_Position;
        particle1.m_Position = particle1.m_Transform.get_position();
      }
    }
  }

  private void UpdateParticles2WithJob()
  {
    Plane plane = (Plane) null;
    for (int index = 1; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index];
      DynamicBoneV2.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
      float magnitude1;
      if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
      {
        Vector3 vector3 = Vector3.op_Subtraction(particle2.m_Transform.get_position(), particle1.m_Transform.get_position());
        magnitude1 = ((Vector3) ref vector3).get_magnitude();
      }
      else
      {
        Matrix4x4 localToWorldMatrix = particle2.m_Transform.get_localToWorldMatrix();
        Vector3 vector3 = ((Matrix4x4) ref localToWorldMatrix).MultiplyVector(particle1.m_EndOffset);
        magnitude1 = ((Vector3) ref vector3).get_magnitude();
      }
      float num1 = Mathf.Lerp(1f, particle1.m_Stiffness, this.m_Weight);
      if ((double) num1 > 0.0 || (double) particle1.m_Elasticity > 0.0)
      {
        Matrix4x4 localToWorldMatrix = particle2.m_Transform.get_localToWorldMatrix();
        ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle2.m_Position));
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_Transform.get_localPosition());
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_2, particle1.m_Elasticity));
        if ((double) num1 > 0.0)
        {
          Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
          float magnitude2 = ((Vector3) ref vector3_3).get_magnitude();
          float num2 = (float) ((double) magnitude1 * (1.0 - (double) num1) * 2.0);
          if ((double) magnitude2 > (double) num2)
          {
            DynamicBoneV2.Particle particle4 = particle1;
            particle4.m_Position = Vector3.op_Addition(particle4.m_Position, Vector3.op_Multiply(vector3_3, (magnitude2 - num2) / magnitude2));
          }
        }
      }
      if (this.m_Colliders != null)
      {
        int count = this.m_Colliders.Count;
        NativeArray<DynamicBoneColliderBase.Bound> nativeArray1;
        ((NativeArray<DynamicBoneColliderBase.Bound>) ref nativeArray1).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<float> nativeArray2;
        ((NativeArray<float>) ref nativeArray2).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<float> nativeArray3;
        ((NativeArray<float>) ref nativeArray3).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<Vector3> nativeArray4;
        ((NativeArray<Vector3>) ref nativeArray4).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<Vector3> nativeArray5;
        ((NativeArray<Vector3>) ref nativeArray5).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<Vector3> nativeArray6;
        ((NativeArray<Vector3>) ref nativeArray6).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<float> nativeArray7;
        ((NativeArray<float>) ref nativeArray7).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        NativeArray<bool> nativeArray8;
        ((NativeArray<bool>) ref nativeArray8).\u002Ector(count, (Allocator) 2, (NativeArrayOptions) 1);
        DynamicBoneV2.ColliderJob colliderJob = new DynamicBoneV2.ColliderJob()
        {
          boundAry = nativeArray1,
          particlePosition = particle1.m_Position,
          particleRadiusAry = nativeArray2,
          capsuleHeightAry = nativeArray3,
          centerAry = nativeArray4,
          c0Ary = nativeArray5,
          c1Ary = nativeArray6,
          radiusAry = nativeArray7,
          enabledAry = nativeArray8
        };
        JobHandle jobHandle = IJobParallelForExtensions.Schedule<DynamicBoneV2.ColliderJob>((M0) colliderJob, count, 0, (JobHandle) null);
        ((JobHandle) ref jobHandle).Complete();
        particle1.m_Position = colliderJob.particlePosition;
        ((NativeArray<DynamicBoneColliderBase.Bound>) ref nativeArray1).Dispose();
        ((NativeArray<float>) ref nativeArray2).Dispose();
        ((NativeArray<float>) ref nativeArray3).Dispose();
        ((NativeArray<Vector3>) ref nativeArray4).Dispose();
        ((NativeArray<Vector3>) ref nativeArray5).Dispose();
        ((NativeArray<Vector3>) ref nativeArray6).Dispose();
        ((NativeArray<float>) ref nativeArray7).Dispose();
        ((NativeArray<bool>) ref nativeArray8).Dispose();
      }
      if (this.m_FreezeAxis != DynamicBoneV2.FreezeAxis.None)
      {
        switch (this.m_FreezeAxis)
        {
          case DynamicBoneV2.FreezeAxis.X:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_right(), particle2.m_Position);
            break;
          case DynamicBoneV2.FreezeAxis.Y:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_up(), particle2.m_Position);
            break;
          case DynamicBoneV2.FreezeAxis.Z:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_forward(), particle2.m_Position);
            break;
        }
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Subtraction(particle3.m_Position, Vector3.op_Multiply(((Plane) ref plane).get_normal(), ((Plane) ref plane).GetDistanceToPoint(particle1.m_Position)));
      }
      Vector3 vector3_4 = Vector3.op_Subtraction(particle2.m_Position, particle1.m_Position);
      float magnitude3 = ((Vector3) ref vector3_4).get_magnitude();
      if ((double) magnitude3 > 0.0)
      {
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_4, (magnitude3 - magnitude1) / magnitude3));
      }
    }
  }

  private void UpdateParticles2()
  {
    Plane plane = (Plane) null;
    for (int index1 = 1; index1 < this.m_Particles.Count; ++index1)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index1];
      DynamicBoneV2.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
      float magnitude1;
      if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
      {
        Vector3 vector3 = Vector3.op_Subtraction(particle2.m_Transform.get_position(), particle1.m_Transform.get_position());
        magnitude1 = ((Vector3) ref vector3).get_magnitude();
      }
      else
      {
        Matrix4x4 localToWorldMatrix = particle2.m_Transform.get_localToWorldMatrix();
        Vector3 vector3 = ((Matrix4x4) ref localToWorldMatrix).MultiplyVector(particle1.m_EndOffset);
        magnitude1 = ((Vector3) ref vector3).get_magnitude();
      }
      float num1 = Mathf.Lerp(1f, particle1.m_Stiffness, this.m_Weight);
      if ((double) num1 > 0.0 || (double) particle1.m_Elasticity > 0.0)
      {
        Matrix4x4 localToWorldMatrix = particle2.m_Transform.get_localToWorldMatrix();
        ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle2.m_Position));
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_Transform.get_localPosition());
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_2, particle1.m_Elasticity));
        if ((double) num1 > 0.0)
        {
          Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
          float magnitude2 = ((Vector3) ref vector3_3).get_magnitude();
          float num2 = (float) ((double) magnitude1 * (1.0 - (double) num1) * 2.0);
          if ((double) magnitude2 > (double) num2)
          {
            DynamicBoneV2.Particle particle4 = particle1;
            particle4.m_Position = Vector3.op_Addition(particle4.m_Position, Vector3.op_Multiply(vector3_3, (magnitude2 - num2) / magnitude2));
          }
        }
      }
      if (this.m_Colliders != null)
      {
        float particleRadius = particle1.m_Radius * this.m_ObjectScale;
        for (int index2 = 0; index2 < this.m_Colliders.Count; ++index2)
        {
          DynamicBoneCollider collider = this.m_Colliders[index2];
          if (Object.op_Inequality((Object) collider, (Object) null) && ((Behaviour) collider).get_enabled())
            collider.Collide(ref particle1.m_Position, particleRadius);
        }
      }
      if (this.m_FreezeAxis != DynamicBoneV2.FreezeAxis.None)
      {
        switch (this.m_FreezeAxis)
        {
          case DynamicBoneV2.FreezeAxis.X:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_right(), particle2.m_Position);
            break;
          case DynamicBoneV2.FreezeAxis.Y:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_up(), particle2.m_Position);
            break;
          case DynamicBoneV2.FreezeAxis.Z:
            ((Plane) ref plane).SetNormalAndPosition(particle2.m_Transform.get_forward(), particle2.m_Position);
            break;
        }
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Subtraction(particle3.m_Position, Vector3.op_Multiply(((Plane) ref plane).get_normal(), ((Plane) ref plane).GetDistanceToPoint(particle1.m_Position)));
      }
      Vector3 vector3_4 = Vector3.op_Subtraction(particle2.m_Position, particle1.m_Position);
      float magnitude3 = ((Vector3) ref vector3_4).get_magnitude();
      if ((double) magnitude3 > 0.0)
      {
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_4, (magnitude3 - magnitude1) / magnitude3));
      }
    }
  }

  private void SkipUpdateParticles()
  {
    for (int index = 0; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index];
      if (particle1.m_ParentIndex >= 0)
      {
        DynamicBoneV2.Particle particle2 = particle1;
        particle2.m_PrevPosition = Vector3.op_Addition(particle2.m_PrevPosition, this.m_ObjectMove);
        DynamicBoneV2.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, this.m_ObjectMove);
        DynamicBoneV2.Particle particle4 = this.m_Particles[particle1.m_ParentIndex];
        float magnitude1;
        if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
        {
          Vector3 vector3 = Vector3.op_Subtraction(particle4.m_Transform.get_position(), particle1.m_Transform.get_position());
          magnitude1 = ((Vector3) ref vector3).get_magnitude();
        }
        else
        {
          Matrix4x4 localToWorldMatrix = particle4.m_Transform.get_localToWorldMatrix();
          Vector3 vector3 = ((Matrix4x4) ref localToWorldMatrix).MultiplyVector(particle1.m_EndOffset);
          magnitude1 = ((Vector3) ref vector3).get_magnitude();
        }
        float num1 = Mathf.Lerp(1f, particle1.m_Stiffness, this.m_Weight);
        if ((double) num1 > 0.0)
        {
          Matrix4x4 localToWorldMatrix = particle4.m_Transform.get_localToWorldMatrix();
          ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle4.m_Position));
          Vector3 vector3 = Vector3.op_Subtraction(!Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_Transform.get_localPosition()), particle1.m_Position);
          float magnitude2 = ((Vector3) ref vector3).get_magnitude();
          float num2 = (float) ((double) magnitude1 * (1.0 - (double) num1) * 2.0);
          if ((double) magnitude2 > (double) num2)
          {
            DynamicBoneV2.Particle particle5 = particle1;
            particle5.m_Position = Vector3.op_Addition(particle5.m_Position, Vector3.op_Multiply(vector3, (magnitude2 - num2) / magnitude2));
          }
        }
        Vector3 vector3_1 = Vector3.op_Subtraction(particle4.m_Position, particle1.m_Position);
        float magnitude3 = ((Vector3) ref vector3_1).get_magnitude();
        if ((double) magnitude3 > 0.0)
        {
          DynamicBoneV2.Particle particle5 = particle1;
          particle5.m_Position = Vector3.op_Addition(particle5.m_Position, Vector3.op_Multiply(vector3_1, (magnitude3 - magnitude1) / magnitude3));
        }
      }
      else
      {
        particle1.m_PrevPosition = particle1.m_Position;
        particle1.m_Position = particle1.m_Transform.get_position();
      }
    }
  }

  private static Vector3 MirrorVector(Vector3 v, Vector3 axis)
  {
    return Vector3.op_Subtraction(v, Vector3.op_Multiply(axis, Vector3.Dot(v, axis) * 2f));
  }

  private void ApplyParticlesToTransforms()
  {
    for (int index = 1; index < this.m_Particles.Count; ++index)
    {
      DynamicBoneV2.Particle particle1 = this.m_Particles[index];
      DynamicBoneV2.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
      if (particle2.m_Transform.get_childCount() <= 1)
      {
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? particle1.m_EndOffset : particle1.m_Transform.get_localPosition();
        Vector3 vector3_2 = Vector3.op_Subtraction(particle1.m_Position, particle2.m_Position);
        Quaternion rotation = Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(vector3_1), vector3_2);
        particle2.m_Transform.set_rotation(Quaternion.op_Multiply(rotation, particle2.m_Transform.get_rotation()));
      }
      if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
        particle1.m_Transform.set_position(particle1.m_Position);
    }
  }

  public enum UpdateMode
  {
    Normal,
    AnimatePhysics,
    UnscaledTime,
  }

  public enum FreezeAxis
  {
    None,
    X,
    Y,
    Z,
  }

  private class Particle
  {
    public int m_ParentIndex = -1;
    public Vector3 m_Position = Vector3.get_zero();
    public Vector3 m_PrevPosition = Vector3.get_zero();
    public Vector3 m_EndOffset = Vector3.get_zero();
    public Vector3 m_InitLocalPosition = Vector3.get_zero();
    public Quaternion m_InitLocalRotation = Quaternion.get_identity();
    public Transform m_Transform;
    public float m_Damping;
    public float m_Elasticity;
    public float m_Stiffness;
    public float m_Inert;
    public float m_Radius;
    public float m_BoneLength;
  }

  private struct ColliderJob : IJobParallelFor
  {
    [ReadOnly]
    public NativeArray<DynamicBoneColliderBase.Bound> boundAry;
    public Vector3 particlePosition;
    [ReadOnly]
    public NativeArray<float> particleRadiusAry;
    [ReadOnly]
    public NativeArray<float> capsuleHeightAry;
    [ReadOnly]
    public NativeArray<Vector3> centerAry;
    [ReadOnly]
    public NativeArray<Vector3> c0Ary;
    [ReadOnly]
    public NativeArray<Vector3> c1Ary;
    [ReadOnly]
    public NativeArray<float> radiusAry;
    [ReadOnly]
    public NativeArray<bool> enabledAry;

    public void Execute(int index)
    {
      if ((double) ((NativeArray<float>) ref this.capsuleHeightAry).get_Item(index) <= 0.0)
      {
        Vector3 vector3 = Vector3.op_Subtraction(this.particlePosition, ((NativeArray<Vector3>) ref this.centerAry).get_Item(index));
        float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
        if (((NativeArray<DynamicBoneColliderBase.Bound>) ref this.boundAry).get_Item(index) == DynamicBoneColliderBase.Bound.Outside)
        {
          float num1 = ((NativeArray<float>) ref this.radiusAry).get_Item(index) + ((NativeArray<float>) ref this.particleRadiusAry).get_Item(index);
          float num2 = num1 * num1;
          if ((double) sqrMagnitude <= 0.0 || (double) sqrMagnitude >= (double) num2)
            return;
          float num3 = Mathf.Sqrt(sqrMagnitude);
          this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.centerAry).get_Item(index), Vector3.op_Multiply(vector3, num1 / num3));
        }
        else
        {
          float num1 = ((NativeArray<float>) ref this.radiusAry).get_Item(index) - ((NativeArray<float>) ref this.particleRadiusAry).get_Item(index);
          float num2 = num1 * num1;
          if ((double) sqrMagnitude <= (double) num2)
            return;
          float num3 = Mathf.Sqrt(sqrMagnitude);
          this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.centerAry).get_Item(index), Vector3.op_Multiply(vector3, num1 / num3));
        }
      }
      else if (((NativeArray<DynamicBoneColliderBase.Bound>) ref this.boundAry).get_Item(index) == DynamicBoneColliderBase.Bound.Outside)
      {
        float num1 = ((NativeArray<float>) ref this.radiusAry).get_Item(index) + ((NativeArray<float>) ref this.particleRadiusAry).get_Item(index);
        float num2 = num1 * num1;
        Vector3 vector3_1 = Vector3.op_Subtraction(((NativeArray<Vector3>) ref this.c1Ary).get_Item(index), ((NativeArray<Vector3>) ref this.c0Ary).get_Item(index));
        Vector3 vector3_2 = Vector3.op_Subtraction(this.particlePosition, ((NativeArray<Vector3>) ref this.c0Ary).get_Item(index));
        float num3 = Vector3.Dot(vector3_2, vector3_1);
        if ((double) num3 <= 0.0)
        {
          float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
          if ((double) sqrMagnitude <= 0.0 || (double) sqrMagnitude >= (double) num2)
            return;
          float num4 = Mathf.Sqrt(sqrMagnitude);
          this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.c0Ary).get_Item(index), Vector3.op_Multiply(vector3_2, num1 / num4));
        }
        else
        {
          float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
          if ((double) num3 >= (double) sqrMagnitude1)
          {
            Vector3 vector3_3 = Vector3.op_Subtraction(this.particlePosition, ((NativeArray<Vector3>) ref this.c1Ary).get_Item(index));
            float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
            if ((double) sqrMagnitude2 <= 0.0 || (double) sqrMagnitude2 >= (double) num2)
              return;
            float num4 = Mathf.Sqrt(sqrMagnitude2);
            this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.c1Ary).get_Item(index), Vector3.op_Multiply(vector3_3, num1 / num4));
          }
          else
          {
            if ((double) sqrMagnitude1 <= 0.0)
              return;
            float num4 = num3 / sqrMagnitude1;
            Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, Vector3.op_Multiply(vector3_1, num4));
            float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
            if ((double) sqrMagnitude2 <= 0.0 || (double) sqrMagnitude2 >= (double) num2)
              return;
            float num5 = Mathf.Sqrt(sqrMagnitude2);
            ref DynamicBoneV2.ColliderJob local = ref this;
            local.particlePosition = Vector3.op_Addition(local.particlePosition, Vector3.op_Multiply(vector3_3, (num1 - num5) / num5));
          }
        }
      }
      else
      {
        float num1 = ((NativeArray<float>) ref this.radiusAry).get_Item(index) - ((NativeArray<float>) ref this.particleRadiusAry).get_Item(index);
        float num2 = num1 * num1;
        Vector3 vector3_1 = Vector3.op_Subtraction(((NativeArray<Vector3>) ref this.c1Ary).get_Item(index), ((NativeArray<Vector3>) ref this.c0Ary).get_Item(index));
        Vector3 vector3_2 = Vector3.op_Subtraction(this.particlePosition, ((NativeArray<Vector3>) ref this.c0Ary).get_Item(index));
        float num3 = Vector3.Dot(vector3_2, vector3_1);
        if ((double) num3 <= 0.0)
        {
          float sqrMagnitude = ((Vector3) ref vector3_2).get_sqrMagnitude();
          if ((double) sqrMagnitude <= (double) num2)
            return;
          float num4 = Mathf.Sqrt(sqrMagnitude);
          this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.c0Ary).get_Item(index), Vector3.op_Multiply(vector3_2, num1 / num4));
        }
        else
        {
          float sqrMagnitude1 = ((Vector3) ref vector3_1).get_sqrMagnitude();
          if ((double) num3 >= (double) sqrMagnitude1)
          {
            Vector3 vector3_3 = Vector3.op_Subtraction(this.particlePosition, ((NativeArray<Vector3>) ref this.c1Ary).get_Item(index));
            float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
            if ((double) sqrMagnitude2 <= (double) num2)
              return;
            float num4 = Mathf.Sqrt(sqrMagnitude2);
            this.particlePosition = Vector3.op_Addition(((NativeArray<Vector3>) ref this.c1Ary).get_Item(index), Vector3.op_Multiply(vector3_3, num1 / num4));
          }
          else
          {
            if ((double) sqrMagnitude1 <= 0.0)
              return;
            float num4 = num3 / sqrMagnitude1;
            Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, Vector3.op_Multiply(vector3_1, num4));
            float sqrMagnitude2 = ((Vector3) ref vector3_3).get_sqrMagnitude();
            if ((double) sqrMagnitude2 <= (double) num2)
              return;
            float num5 = Mathf.Sqrt(sqrMagnitude2);
            ref DynamicBoneV2.ColliderJob local = ref this;
            local.particlePosition = Vector3.op_Addition(local.particlePosition, Vector3.op_Multiply(vector3_3, (num1 - num5) / num5));
          }
        }
      }
    }
  }
}
