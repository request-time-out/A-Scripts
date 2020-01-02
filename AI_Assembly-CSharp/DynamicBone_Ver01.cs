// Decompiled with JetBrains decompiler
// Type: DynamicBone_Ver01
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Ver01")]
public class DynamicBone_Ver01 : MonoBehaviour
{
  public string comment;
  public Transform m_Root;
  public float m_UpdateRate;
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
  public float m_AddAngleScale;
  public AnimationCurve m_AddAngleScaleDistrib;
  public float m_EndLength;
  public Vector3 m_EndOffset;
  public Vector3 m_Gravity;
  public Vector3 m_Force;
  public List<DynamicBoneCollider> m_Colliders;
  public List<DynamicBone_Ver01.BoneNode> m_Nodes;
  private Vector3 m_ObjectMove;
  private Vector3 m_ObjectPrevPosition;
  private float m_BoneTotalLength;
  private float m_ObjectScale;
  private float m_Time;
  private float m_Weight;
  private List<DynamicBone_Ver01.Particle> m_Particles;

  public DynamicBone_Ver01()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SetupParticles();
  }

  private void Update()
  {
    if ((double) this.m_Weight <= 0.0)
      return;
    this.InitTransforms();
  }

  private void LateUpdate()
  {
    if ((double) this.m_Weight <= 0.0)
      return;
    this.UpdateDynamicBones(Time.get_deltaTime());
  }

  private void OnEnable()
  {
    this.ResetParticlesPosition();
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
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
    this.m_AddAngleScale = Mathf.Max(this.m_AddAngleScale, 0.0f);
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
    foreach (DynamicBone_Ver01.Particle particle1 in this.m_Particles)
    {
      if (particle1.m_ParentIndex >= 0)
      {
        DynamicBone_Ver01.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
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
    {
      this.ResetParticlesPosition();
      this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
    }
    this.m_Weight = w;
  }

  public float GetWeight()
  {
    return this.m_Weight;
  }

  public void setRoot(Transform _transRoot)
  {
    this.m_Root = _transRoot;
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

  public void SetupParticles()
  {
    this.m_Particles.Clear();
    if (Object.op_Equality((Object) this.m_Root, (Object) null) && this.m_Nodes.Count > 0)
      this.m_Root = this.m_Nodes[0].Transform;
    if (Object.op_Equality((Object) this.m_Root, (Object) null))
      return;
    this.m_ObjectScale = (float) ((Component) this).get_transform().get_lossyScale().x;
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
    this.m_ObjectMove = Vector3.get_zero();
    this.m_BoneTotalLength = 0.0f;
    DynamicBone_Ver01.Particle particle1 = (DynamicBone_Ver01.Particle) null;
    int parentIndex = -1;
    foreach (DynamicBone_Ver01.BoneNode node in this.m_Nodes)
    {
      float boneLength = particle1 == null ? 0.0f : particle1.m_BoneLength;
      particle1 = this.AppendParticles(node, parentIndex, boneLength);
      ++parentIndex;
    }
    if ((double) this.m_EndLength > 0.0 || (double) ((Vector3) ref this.m_EndOffset).get_magnitude() != 0.0)
      this.AppendParticles(new DynamicBone_Ver01.BoneNode(), parentIndex, particle1.m_BoneLength);
    float num1 = this.m_Particles.Count > 1 ? 1f / (float) (this.m_Particles.Count - 1) : 0.0f;
    float num2 = 0.0f;
    foreach (DynamicBone_Ver01.Particle particle2 in this.m_Particles)
    {
      particle2.m_Damping = this.m_Damping;
      particle2.m_Elasticity = this.m_Elasticity;
      particle2.m_Stiffness = this.m_Stiffness;
      particle2.m_Inert = this.m_Inert;
      particle2.m_Radius = this.m_Radius;
      particle2.m_AddAngleScale = this.m_AddAngleScale;
      if (this.m_DampingDistrib.get_keys().Length > 0)
        particle2.m_Damping *= this.m_DampingDistrib.Evaluate(num2);
      if (this.m_ElasticityDistrib.get_keys().Length > 0)
        particle2.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(num2);
      if (this.m_StiffnessDistrib.get_keys().Length > 0)
        particle2.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(num2);
      if (this.m_InertDistrib.get_keys().Length > 0)
        particle2.m_Inert *= this.m_InertDistrib.Evaluate(num2);
      if (this.m_RadiusDistrib.get_keys().Length > 0)
        particle2.m_Radius *= this.m_RadiusDistrib.Evaluate(num2);
      if (this.m_AddAngleScaleDistrib.get_keys().Length > 0)
        particle2.m_AddAngleScale *= this.m_AddAngleScaleDistrib.Evaluate(num2);
      num2 += num1;
      particle2.m_Damping = Mathf.Clamp01(particle2.m_Damping);
      particle2.m_Elasticity = Mathf.Clamp01(particle2.m_Elasticity);
      particle2.m_Stiffness = Mathf.Clamp01(particle2.m_Stiffness);
      particle2.m_Inert = Mathf.Clamp01(particle2.m_Inert);
      particle2.m_Radius = Mathf.Max(particle2.m_Radius, 0.0f);
      particle2.m_AddAngleScale = Mathf.Max(particle2.m_AddAngleScale, 0.0f);
    }
  }

  public void reloadParameter()
  {
    foreach (DynamicBone_Ver01.Particle particle in this.m_Particles)
    {
      particle.m_Damping = this.m_Damping;
      particle.m_Elasticity = this.m_Elasticity;
      particle.m_Stiffness = this.m_Stiffness;
      particle.m_Inert = this.m_Inert;
      particle.m_Radius = this.m_Radius;
      particle.m_AddAngleScale = this.m_AddAngleScale;
      particle.m_Damping = Mathf.Clamp01(particle.m_Damping);
      particle.m_Elasticity = Mathf.Clamp01(particle.m_Elasticity);
      particle.m_Stiffness = Mathf.Clamp01(particle.m_Stiffness);
      particle.m_Inert = Mathf.Clamp01(particle.m_Inert);
      particle.m_Radius = Mathf.Max(particle.m_Radius, 0.0f);
      particle.m_AddAngleScale = Mathf.Max(particle.m_AddAngleScale, 0.0f);
    }
  }

  private DynamicBone_Ver01.Particle AppendParticles(
    DynamicBone_Ver01.BoneNode b,
    int parentIndex,
    float boneLength)
  {
    DynamicBone_Ver01.Particle particle = new DynamicBone_Ver01.Particle();
    particle.m_Transform = b.Transform;
    particle.m_bRotationCalc = b.RotationCalc;
    particle.m_ParentIndex = parentIndex;
    if (Object.op_Inequality((Object) b.Transform, (Object) null))
    {
      particle.m_Position = particle.m_PrevPosition = b.Transform.get_position();
      particle.m_InitLocalPosition = b.Transform.get_localPosition();
      particle.m_InitLocalRotation = b.Transform.get_localRotation();
      particle.m_InitEuler = b.Transform.get_localEulerAngles();
      if (parentIndex >= 0)
        this.CalcLocalPosition(particle, this.m_Particles[parentIndex]);
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
        particle.m_EndOffset = this.m_EndOffset;
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
    this.m_Particles.Add(particle);
    return particle;
  }

  public void InitTransforms()
  {
    foreach (DynamicBone_Ver01.Particle particle in this.m_Particles)
    {
      if (Object.op_Inequality((Object) particle.m_Transform, (Object) null))
      {
        particle.m_Transform.set_localPosition(particle.m_InitLocalPosition);
        particle.m_Transform.set_localRotation(particle.m_InitLocalRotation);
      }
    }
  }

  public void ResetParticlesPosition()
  {
    this.m_ObjectPrevPosition = ((Component) this).get_transform().get_position();
    foreach (DynamicBone_Ver01.Particle particle in this.m_Particles)
    {
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
  }

  private void UpdateParticles1()
  {
    Vector3 vector3_1 = Vector3.op_Multiply(Vector3.op_Addition(this.m_Gravity, this.m_Force), this.m_ObjectScale);
    foreach (DynamicBone_Ver01.Particle particle1 in this.m_Particles)
    {
      if (particle1.m_ParentIndex >= 0)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(particle1.m_Position, particle1.m_PrevPosition);
        Vector3 vector3_3 = Vector3.op_Multiply(this.m_ObjectMove, particle1.m_Inert);
        particle1.m_PrevPosition = Vector3.op_Addition(particle1.m_Position, vector3_3);
        DynamicBone_Ver01.Particle particle2 = particle1;
        particle2.m_Position = Vector3.op_Addition(particle2.m_Position, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(vector3_2, 1f - particle1.m_Damping), vector3_1), vector3_3));
      }
      else
      {
        particle1.m_PrevPosition = particle1.m_Position;
        particle1.m_Position = particle1.m_Transform.get_position();
      }
    }
  }

  private void UpdateParticles2()
  {
    for (int index = 1; index < this.m_Particles.Count; ++index)
    {
      DynamicBone_Ver01.Particle particle1 = this.m_Particles[index];
      DynamicBone_Ver01.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
      float num1;
      if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
      {
        Vector3 vector3 = Vector3.op_Subtraction(particle2.m_Transform.get_position(), particle1.m_Transform.get_position());
        num1 = ((Vector3) ref vector3).get_magnitude();
      }
      else
        num1 = ((Vector3) ref particle1.m_EndOffset).get_magnitude() * this.m_ObjectScale;
      float num2 = Mathf.Lerp(1f, particle1.m_Stiffness, this.m_Weight);
      if ((double) num2 > 0.0 || (double) particle1.m_Elasticity > 0.0)
      {
        Matrix4x4 localToWorldMatrix = particle2.m_Transform.get_localToWorldMatrix();
        ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle2.m_Position));
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_LocalPosition);
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
        DynamicBone_Ver01.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_2, particle1.m_Elasticity));
        if ((double) num2 > 0.0)
        {
          Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, particle1.m_Position);
          float magnitude = ((Vector3) ref vector3_3).get_magnitude();
          float num3 = (float) ((double) num1 * (1.0 - (double) num2) * 2.0);
          if ((double) magnitude > (double) num3)
          {
            DynamicBone_Ver01.Particle particle4 = particle1;
            particle4.m_Position = Vector3.op_Addition(particle4.m_Position, Vector3.op_Multiply(vector3_3, (magnitude - num3) / magnitude));
          }
        }
      }
      float particleRadius = particle1.m_Radius * this.m_ObjectScale;
      foreach (DynamicBoneCollider collider in this.m_Colliders)
      {
        if (Object.op_Inequality((Object) collider, (Object) null) && ((Behaviour) collider).get_enabled())
          collider.Collide(ref particle1.m_Position, particleRadius);
      }
      Vector3 vector3_4 = Vector3.op_Subtraction(particle2.m_Position, particle1.m_Position);
      float magnitude1 = ((Vector3) ref vector3_4).get_magnitude();
      if ((double) magnitude1 > 0.0)
      {
        DynamicBone_Ver01.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, Vector3.op_Multiply(vector3_4, (magnitude1 - num1) / magnitude1));
      }
    }
  }

  private void SkipUpdateParticles()
  {
    foreach (DynamicBone_Ver01.Particle particle1 in this.m_Particles)
    {
      if (particle1.m_ParentIndex >= 0)
      {
        Vector3 vector3_1 = Vector3.op_Multiply(this.m_ObjectMove, particle1.m_Inert);
        DynamicBone_Ver01.Particle particle2 = particle1;
        particle2.m_PrevPosition = Vector3.op_Addition(particle2.m_PrevPosition, vector3_1);
        DynamicBone_Ver01.Particle particle3 = particle1;
        particle3.m_Position = Vector3.op_Addition(particle3.m_Position, vector3_1);
        DynamicBone_Ver01.Particle particle4 = this.m_Particles[particle1.m_ParentIndex];
        float num1;
        if (Object.op_Inequality((Object) particle1.m_Transform, (Object) null))
        {
          Vector3 vector3_2 = Vector3.op_Subtraction(particle4.m_Transform.get_position(), particle1.m_Transform.get_position());
          num1 = ((Vector3) ref vector3_2).get_magnitude();
        }
        else
          num1 = ((Vector3) ref particle1.m_EndOffset).get_magnitude() * this.m_ObjectScale;
        float num2 = Mathf.Lerp(1f, particle1.m_Stiffness, this.m_Weight);
        if ((double) num2 > 0.0)
        {
          Matrix4x4 localToWorldMatrix = particle4.m_Transform.get_localToWorldMatrix();
          ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle4.m_Position));
          Vector3 vector3_2 = Vector3.op_Subtraction(!Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.m_Transform.get_localPosition()), particle1.m_Position);
          float magnitude = ((Vector3) ref vector3_2).get_magnitude();
          float num3 = (float) ((double) num1 * (1.0 - (double) num2) * 2.0);
          if ((double) magnitude > (double) num3)
          {
            DynamicBone_Ver01.Particle particle5 = particle1;
            particle5.m_Position = Vector3.op_Addition(particle5.m_Position, Vector3.op_Multiply(vector3_2, (magnitude - num3) / magnitude));
          }
        }
        Vector3 vector3_3 = Vector3.op_Subtraction(particle4.m_Position, particle1.m_Position);
        float magnitude1 = ((Vector3) ref vector3_3).get_magnitude();
        if ((double) magnitude1 > 0.0)
        {
          DynamicBone_Ver01.Particle particle5 = particle1;
          particle5.m_Position = Vector3.op_Addition(particle5.m_Position, Vector3.op_Multiply(vector3_3, (magnitude1 - num1) / magnitude1));
        }
      }
      else
      {
        particle1.m_PrevPosition = particle1.m_Position;
        particle1.m_Position = particle1.m_Transform.get_position();
      }
    }
  }

  private void ApplyParticlesToTransforms()
  {
    for (int index = 1; index < this.m_Particles.Count; ++index)
    {
      DynamicBone_Ver01.Particle particle1 = this.m_Particles[index];
      DynamicBone_Ver01.Particle particle2 = this.m_Particles[particle1.m_ParentIndex];
      if (particle2.m_bRotationCalc)
      {
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.m_Transform, (Object) null) ? particle1.m_EndOffset : particle1.m_LocalPosition;
        Vector3 vector3_2 = Vector3.op_Subtraction(particle1.m_Position, particle2.m_Position);
        Quaternion quaternion = Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(vector3_1), vector3_2);
        float num;
        Vector3 vector3_3;
        ((Quaternion) ref quaternion).ToAngleAxis(ref num, ref vector3_3);
        num *= particle1.m_AddAngleScale;
        quaternion = Quaternion.AngleAxis(num, vector3_3);
        particle2.m_Transform.set_rotation(Quaternion.op_Multiply(quaternion, particle2.m_Transform.get_rotation()));
      }
      if (Object.op_Implicit((Object) particle1.m_Transform))
        particle1.m_Transform.set_position(particle1.m_Position);
    }
  }

  private void CalcLocalPosition(
    DynamicBone_Ver01.Particle particle,
    DynamicBone_Ver01.Particle parent)
  {
    particle.m_LocalPosition = parent.m_Transform.InverseTransformPoint(particle.m_Position);
  }

  [Serializable]
  public class BoneNode
  {
    public bool RotationCalc = true;
    public Transform Transform;
  }

  private class Particle
  {
    public int m_ParentIndex = -1;
    public bool m_bRotationCalc = true;
    public Vector3 m_Position = Vector3.get_zero();
    public Vector3 m_PrevPosition = Vector3.get_zero();
    public Vector3 m_EndOffset = Vector3.get_zero();
    public Vector3 m_InitLocalPosition = Vector3.get_zero();
    public Quaternion m_InitLocalRotation = Quaternion.get_identity();
    public Vector3 m_InitEuler = Vector3.get_zero();
    public Vector3 m_LocalPosition = Vector3.get_zero();
    public Transform m_Transform;
    public float m_Damping;
    public float m_Elasticity;
    public float m_Stiffness;
    public float m_Inert;
    public float m_Radius;
    public float m_BoneLength;
    public float m_AddAngleScale;
  }
}
