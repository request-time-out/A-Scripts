// Decompiled with JetBrains decompiler
// Type: DynamicBone_Ver02
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[AddComponentMenu("Dynamic Bone/Dynamic Bone Ver02")]
public class DynamicBone_Ver02 : MonoBehaviour
{
  public string Comment;
  public Transform Root;
  public float UpdateRate;
  [Range(0.0f, 100f)]
  [Tooltip("速度UP")]
  public float ReflectSpeed;
  [Range(0.0f, 10f)]
  [Tooltip("重い時に何回まで回す？正確になるけどその分重くなる")]
  public int HeavyLoopMaxCount;
  public Vector3 Gravity;
  public Vector3 Force;
  public List<DynamicBoneCollider> Colliders;
  public List<Transform> Bones;
  public List<DynamicBone_Ver02.BonePtn> Patterns;
  private Vector3 ObjectMove;
  private Vector3 ObjectPrevPosition;
  private float ObjectScale;
  private float UpdateTime;
  private float Weight;
  private List<DynamicBone_Ver02.Particle> Particles;
  public int PtnNo;
  public string DragAndDrop;

  public DynamicBone_Ver02()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.InitNodeParticle();
    this.SetupParticles();
    this.InitLocalPosition();
    if (this.IsRefTransform())
      this.setPtn(0, true);
    this.InitTransforms();
  }

  private void Update()
  {
  }

  private void LateUpdate()
  {
    if ((double) this.Weight <= 0.0)
      return;
    this.InitTransforms();
    this.UpdateDynamicBones(Time.get_deltaTime());
  }

  private void OnEnable()
  {
    this.ResetParticlesPosition();
    if (Object.op_Inequality((Object) this.Root, (Object) null))
      this.ObjectPrevPosition = this.Root.get_position();
    else
      this.ObjectPrevPosition = ((Component) this).get_transform().get_position();
  }

  private void OnDisable()
  {
    this.InitTransforms();
  }

  private void OnValidate()
  {
    this.UpdateRate = Mathf.Max(this.UpdateRate, 0.0f);
    if (!Application.get_isEditor())
      return;
    this.InitNodeParticle();
    this.SetupParticles();
    this.InitLocalPosition();
    if (this.IsRefTransform())
      this.setPtn(this.PtnNo, true);
    this.InitTransforms();
  }

  private void OnDrawGizmosSelected()
  {
    if (!((Behaviour) this).get_enabled() || Object.op_Equality((Object) this.Root, (Object) null))
      return;
    if (Application.get_isEditor() && !Application.get_isPlaying() && ((Component) this).get_transform().get_hasChanged())
    {
      this.InitNodeParticle();
      this.SetupParticles();
      this.InitLocalPosition();
      if (this.IsRefTransform())
        this.setPtn(this.PtnNo, true);
      this.InitTransforms();
    }
    Gizmos.set_color(Color.get_white());
    foreach (DynamicBone_Ver02.Particle particle1 in this.Particles)
    {
      if (particle1.ParentIndex >= 0)
      {
        DynamicBone_Ver02.Particle particle2 = this.Particles[particle1.ParentIndex];
        Gizmos.DrawLine(particle1.Position, particle2.Position);
      }
      if ((double) particle1.Radius > 0.0)
        Gizmos.DrawWireSphere(particle1.Position, particle1.Radius * this.ObjectScale);
    }
  }

  public void SetWeight(float _weight)
  {
    if ((double) this.Weight == (double) _weight)
      return;
    if ((double) _weight == 0.0)
      this.InitTransforms();
    else if ((double) this.Weight == 0.0)
    {
      this.ResetParticlesPosition();
      this.ObjectPrevPosition = !Object.op_Inequality((Object) this.Root, (Object) null) ? ((Component) this).get_transform().get_position() : this.Root.get_position();
    }
    this.Weight = _weight;
  }

  public float GetWeight()
  {
    return this.Weight;
  }

  public void setRoot(Transform _transRoot)
  {
    this.Root = _transRoot;
  }

  public DynamicBone_Ver02.Particle getParticle(int _idx)
  {
    return this.Particles.Count >= _idx ? (DynamicBone_Ver02.Particle) null : this.Particles[_idx];
  }

  public int getParticleCount()
  {
    return this.Particles.Count;
  }

  public bool setPtn(int _ptn, bool _isSameForcePtn = false)
  {
    if (this.Particles == null || this.Patterns == null || (this.Particles.Count == 0 || this.Patterns.Count == 0) || (this.Patterns.Count <= _ptn || this.Particles.Count != this.Patterns[_ptn].ParticlePtns.Count || this.PtnNo == _ptn && !_isSameForcePtn))
      return false;
    this.PtnNo = _ptn;
    this.Gravity = this.Patterns[_ptn].Gravity;
    for (int index = 0; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle = this.Particles[index];
      DynamicBone_Ver02.ParticlePtn particlePtn = this.Patterns[_ptn].ParticlePtns[index];
      particle.IsRotationCalc = particlePtn.IsRotationCalc;
      particle.Damping = particlePtn.Damping;
      particle.Elasticity = particlePtn.Elasticity;
      particle.Stiffness = particlePtn.Stiffness;
      particle.Inert = particlePtn.Inert;
      particle.ScaleNextBoneLength = particlePtn.ScaleNextBoneLength;
      particle.Radius = particlePtn.Radius;
      particle.IsMoveLimit = particlePtn.IsMoveLimit;
      particle.MoveLimitMin = particlePtn.MoveLimitMin;
      particle.MoveLimitMax = particlePtn.MoveLimitMax;
      particle.KeepLengthLimitMin = particlePtn.KeepLengthLimitMin;
      particle.KeepLengthLimitMax = particlePtn.KeepLengthLimitMax;
      particle.IsCrush = particlePtn.IsCrush;
      particle.CrushMoveAreaMin = particlePtn.CrushMoveAreaMin;
      particle.CrushMoveAreaMax = particlePtn.CrushMoveAreaMax;
      particle.CrushAddXYMin = particlePtn.CrushAddXYMin;
      particle.CrushAddXYMax = particlePtn.CrushAddXYMax;
      particle.Damping = Mathf.Clamp01(particle.Damping);
      particle.Elasticity = Mathf.Clamp01(particle.Elasticity);
      particle.Stiffness = Mathf.Clamp01(particle.Stiffness);
      particle.Inert = Mathf.Clamp01(particle.Inert);
      particle.ScaleNextBoneLength = Mathf.Max(particle.ScaleNextBoneLength, 0.0f);
      particle.Radius = Mathf.Max(particle.Radius, 0.0f);
      particle.InitLocalPosition = particlePtn.InitLocalPosition;
      particle.InitLocalRotation = particlePtn.InitLocalRotation;
      particle.InitLocalScale = particlePtn.InitLocalScale;
      particle.refTrans = particlePtn.refTrans;
      particle.LocalPosition = particlePtn.LocalPosition;
      particle.EndOffset = particlePtn.EndOffset;
    }
    return true;
  }

  public void ResetParticlesPosition()
  {
    this.ObjectPrevPosition = !Object.op_Inequality((Object) this.Root, (Object) null) ? ((Component) this).get_transform().get_position() : this.Root.get_position();
    foreach (DynamicBone_Ver02.Particle particle in this.Particles)
    {
      if (Object.op_Inequality((Object) particle.Transform, (Object) null))
      {
        particle.Position = particle.PrevPosition = particle.Transform.get_position();
      }
      else
      {
        Transform transform = this.Particles[particle.ParentIndex].Transform;
        particle.Position = particle.PrevPosition = transform.TransformPoint(particle.EndOffset);
      }
    }
  }

  public void InitLocalPosition()
  {
    List<DynamicBone_Ver02.TransformParam> transformParamList = new List<DynamicBone_Ver02.TransformParam>();
    for (int index = 0; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle = this.Particles[index];
      DynamicBone_Ver02.TransformParam transformParam = new DynamicBone_Ver02.TransformParam();
      if (Object.op_Equality((Object) particle.Transform, (Object) null))
      {
        transformParamList.Add(transformParam);
      }
      else
      {
        transformParam.pos = particle.Transform.get_localPosition();
        transformParam.rot = particle.Transform.get_localRotation();
        transformParam.scale = particle.Transform.get_localScale();
        transformParamList.Add(transformParam);
      }
    }
    for (int index1 = 0; index1 < this.Patterns.Count; ++index1)
    {
      DynamicBone_Ver02.BonePtn pattern = this.Patterns[index1];
      for (int index2 = 0; index2 < pattern.Params.Count; ++index2)
      {
        pattern.ParticlePtns[index2].InitLocalPosition = pattern.Params[index2].RefTransform.get_localPosition();
        pattern.ParticlePtns[index2].InitLocalRotation = pattern.Params[index2].RefTransform.get_localRotation();
        pattern.ParticlePtns[index2].InitLocalScale = pattern.Params[index2].RefTransform.get_localScale();
        pattern.ParticlePtns[index2].refTrans = pattern.Params[index2].RefTransform;
      }
      if (pattern.ParticlePtns.Count == this.Particles.Count)
      {
        for (int index2 = 0; index2 < this.Particles.Count; ++index2)
        {
          DynamicBone_Ver02.Particle particle = this.Particles[index2];
          if (!Object.op_Equality((Object) particle.Transform, (Object) null))
          {
            particle.Transform.set_localPosition(pattern.ParticlePtns[index2].InitLocalPosition);
            particle.Transform.set_localRotation(pattern.ParticlePtns[index2].InitLocalRotation);
            particle.Transform.set_localScale(pattern.ParticlePtns[index2].InitLocalScale);
          }
        }
      }
      for (int index2 = 1; index2 < pattern.Params.Count; ++index2)
      {
        if (Object.op_Implicit((Object) pattern.Params[index2].RefTransform) && Object.op_Implicit((Object) pattern.Params[index2 - 1].RefTransform))
          pattern.ParticlePtns[index2].LocalPosition = this.CalcLocalPosition(pattern.Params[index2].RefTransform.get_position(), pattern.Params[index2 - 1].RefTransform);
      }
    }
    for (int index = 0; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle = this.Particles[index];
      if (!Object.op_Equality((Object) particle.Transform, (Object) null))
      {
        particle.Transform.set_localPosition(transformParamList[index].pos);
        particle.Transform.set_localRotation(transformParamList[index].rot);
        particle.Transform.set_localScale(transformParamList[index].scale);
      }
    }
  }

  public void ResetPosition()
  {
    this.InitLocalPosition();
    this.setPtn(this.PtnNo, true);
    if (!((Behaviour) this).get_enabled())
      return;
    this.InitTransforms();
  }

  public bool PtnBlend(int _blendAnswerPtn, int _blendPtn1, int _blendPtn2, float _t)
  {
    if (this.Patterns == null)
      return false;
    int count = this.Patterns.Count;
    if (count <= _blendAnswerPtn || count <= _blendPtn1 || (count <= _blendPtn2 || this.Patterns[_blendAnswerPtn].ParticlePtns.Count != this.Patterns[_blendPtn1].ParticlePtns.Count) || this.Patterns[_blendPtn2].ParticlePtns.Count != this.Patterns[_blendPtn1].ParticlePtns.Count)
      return false;
    this.Patterns[_blendAnswerPtn].Gravity = Vector3.Lerp(this.Patterns[_blendPtn1].Gravity, this.Patterns[_blendPtn2].Gravity, _t);
    for (int index = 0; index < this.Patterns[_blendAnswerPtn].ParticlePtns.Count; ++index)
    {
      DynamicBone_Ver02.ParticlePtn particlePtn1 = this.Patterns[_blendAnswerPtn].ParticlePtns[index];
      DynamicBone_Ver02.ParticlePtn particlePtn2 = this.Patterns[_blendPtn1].ParticlePtns[index];
      DynamicBone_Ver02.ParticlePtn particlePtn3 = this.Patterns[_blendPtn2].ParticlePtns[index];
      particlePtn1.IsRotationCalc = particlePtn3.IsRotationCalc;
      particlePtn1.Damping = Mathf.Lerp(particlePtn2.Damping, particlePtn3.Damping, _t);
      particlePtn1.Elasticity = Mathf.Lerp(particlePtn2.Elasticity, particlePtn3.Elasticity, _t);
      particlePtn1.Stiffness = Mathf.Lerp(particlePtn2.Stiffness, particlePtn3.Stiffness, _t);
      particlePtn1.Inert = Mathf.Lerp(particlePtn2.Inert, particlePtn3.Inert, _t);
      particlePtn1.ScaleNextBoneLength = Mathf.Lerp(particlePtn2.ScaleNextBoneLength, particlePtn3.ScaleNextBoneLength, _t);
      particlePtn1.Radius = Mathf.Lerp(particlePtn2.Radius, particlePtn3.Radius, _t);
      particlePtn1.IsMoveLimit = particlePtn3.IsMoveLimit;
      particlePtn1.MoveLimitMin = Vector3.Lerp(particlePtn2.MoveLimitMin, particlePtn3.MoveLimitMin, _t);
      particlePtn1.MoveLimitMax = Vector3.Lerp(particlePtn2.MoveLimitMax, particlePtn3.MoveLimitMax, _t);
      particlePtn1.KeepLengthLimitMin = Mathf.Lerp(particlePtn2.KeepLengthLimitMin, particlePtn3.KeepLengthLimitMin, _t);
      particlePtn1.KeepLengthLimitMax = Mathf.Lerp(particlePtn2.KeepLengthLimitMax, particlePtn3.KeepLengthLimitMax, _t);
      particlePtn1.IsCrush = particlePtn3.IsCrush;
      particlePtn1.CrushMoveAreaMin = Mathf.Lerp(particlePtn2.CrushMoveAreaMin, particlePtn3.CrushMoveAreaMin, _t);
      particlePtn1.CrushMoveAreaMax = Mathf.Lerp(particlePtn2.CrushMoveAreaMax, particlePtn3.CrushMoveAreaMax, _t);
      particlePtn1.CrushAddXYMin = Mathf.Lerp(particlePtn2.CrushAddXYMin, particlePtn3.CrushAddXYMin, _t);
      particlePtn1.CrushAddXYMax = Mathf.Lerp(particlePtn2.CrushAddXYMax, particlePtn3.CrushAddXYMax, _t);
      particlePtn1.Damping = Mathf.Clamp01(particlePtn1.Damping);
      particlePtn1.Elasticity = Mathf.Clamp01(particlePtn1.Elasticity);
      particlePtn1.Stiffness = Mathf.Clamp01(particlePtn1.Stiffness);
      particlePtn1.Inert = Mathf.Clamp01(particlePtn1.Inert);
      particlePtn1.ScaleNextBoneLength = Mathf.Max(particlePtn1.ScaleNextBoneLength, 0.0f);
      particlePtn1.Radius = Mathf.Max(particlePtn1.Radius, 0.0f);
      particlePtn1.InitLocalPosition = Vector3.Lerp(particlePtn2.InitLocalPosition, particlePtn3.InitLocalPosition, _t);
      particlePtn1.InitLocalRotation = Quaternion.Lerp(particlePtn2.InitLocalRotation, particlePtn3.InitLocalRotation, _t);
      particlePtn1.InitLocalScale = Vector3.Lerp(particlePtn2.InitLocalScale, particlePtn3.InitLocalScale, _t);
      particlePtn1.refTrans = particlePtn3.refTrans;
      particlePtn1.LocalPosition = Vector3.Lerp(particlePtn2.LocalPosition, particlePtn3.LocalPosition, _t);
      particlePtn1.EndOffset = Vector3.Lerp(particlePtn2.EndOffset, particlePtn3.EndOffset, _t);
    }
    return true;
  }

  public bool setGravity(int _ptn, Vector3 _gravity, bool _isNowGravity = true)
  {
    if (this.Particles == null || this.Patterns == null || (this.Particles.Count == 0 || this.Patterns.Count == 0) || this.Patterns.Count <= _ptn)
      return false;
    if (_isNowGravity)
      this.Gravity = _gravity;
    if (_ptn < 0)
    {
      for (int index = 0; index < this.Patterns.Count; ++index)
        this.Patterns[index].Gravity = _gravity;
    }
    else
    {
      if (this.Patterns.Count <= _ptn)
        return false;
      this.Patterns[_ptn].Gravity = _gravity;
    }
    return true;
  }

  public bool setSoftParams(
    int _ptn,
    int _bone,
    float _damping,
    float _elasticity,
    float _stiffness,
    bool _isNowParam = true)
  {
    if (this.Particles == null || this.Patterns == null || (this.Particles.Count == 0 || this.Patterns.Count == 0) || this.Patterns.Count <= _ptn)
      return false;
    if (_isNowParam)
    {
      if (_bone == -1)
      {
        for (int index = 0; index < this.Particles.Count; ++index)
        {
          this.Particles[index].Damping = _damping;
          this.Particles[index].Elasticity = _elasticity;
          this.Particles[index].Stiffness = _stiffness;
        }
      }
      else if (this.Particles.Count > _bone)
      {
        this.Particles[_bone].Damping = _damping;
        this.Particles[_bone].Elasticity = _elasticity;
        this.Particles[_bone].Stiffness = _stiffness;
      }
    }
    if (_ptn < 0)
    {
      for (int index1 = 0; index1 < this.Patterns.Count; ++index1)
      {
        if (_bone == -1)
        {
          for (int index2 = 0; index2 < this.Patterns[index1].ParticlePtns.Count; ++index2)
            this.setSoftParams(this.Patterns[index1].ParticlePtns[index2], _damping, _elasticity, _stiffness);
          for (int index2 = 0; index2 < this.Patterns[index1].Params.Count; ++index2)
            this.setSoftParams(this.Patterns[index1].Params[index2], _damping, _elasticity, _stiffness);
        }
        else
        {
          if (this.Patterns[index1].ParticlePtns.Count > _bone)
            this.setSoftParams(this.Patterns[index1].ParticlePtns[_bone], _damping, _elasticity, _stiffness);
          if (this.Patterns[index1].Params.Count > _bone)
            this.setSoftParams(this.Patterns[index1].Params[_bone], _damping, _elasticity, _stiffness);
        }
      }
    }
    else
    {
      if (this.Patterns.Count <= _ptn)
        return false;
      if (_bone == -1)
      {
        for (int index = 0; index < this.Patterns[_ptn].ParticlePtns.Count; ++index)
          this.setSoftParams(this.Patterns[_ptn].ParticlePtns[index], _damping, _elasticity, _stiffness);
        for (int index = 0; index < this.Patterns[_ptn].Params.Count; ++index)
          this.setSoftParams(this.Patterns[_ptn].Params[index], _damping, _elasticity, _stiffness);
      }
      else
      {
        if (this.Patterns[_ptn].ParticlePtns.Count > _bone)
          this.setSoftParams(this.Patterns[_ptn].ParticlePtns[_bone], _damping, _elasticity, _stiffness);
        if (this.Patterns[_ptn].Params.Count > _bone)
          this.setSoftParams(this.Patterns[_ptn].Params[_bone], _damping, _elasticity, _stiffness);
      }
    }
    return true;
  }

  private bool setSoftParams(
    DynamicBone_Ver02.ParticlePtn _ptn,
    float _damping,
    float _elasticity,
    float _stiffness)
  {
    _ptn.Damping = _damping;
    _ptn.Elasticity = _elasticity;
    _ptn.Stiffness = _stiffness;
    return true;
  }

  private bool setSoftParams(
    DynamicBone_Ver02.BoneParameter _ptn,
    float _damping,
    float _elasticity,
    float _stiffness)
  {
    _ptn.Damping = _damping;
    _ptn.Elasticity = _elasticity;
    _ptn.Stiffness = _stiffness;
    return true;
  }

  public bool setSoftParamsEx(int _ptn, int _bone, float _inert, bool _isNowParam = true)
  {
    if (this.Particles == null || this.Patterns == null || (this.Particles.Count == 0 || this.Patterns.Count == 0) || this.Patterns.Count <= _ptn)
      return false;
    if (_isNowParam)
    {
      if (_bone == -1)
      {
        for (int index = 0; index < this.Particles.Count; ++index)
          this.Particles[index].Inert = _inert;
      }
      else if (this.Particles.Count > _bone)
        this.Particles[_bone].Inert = _inert;
    }
    if (_ptn < 0)
    {
      for (int index1 = 0; index1 < this.Patterns.Count; ++index1)
      {
        if (_bone == -1)
        {
          for (int index2 = 0; index2 < this.Patterns[index1].ParticlePtns.Count; ++index2)
            this.Patterns[index1].ParticlePtns[index2].Inert = _inert;
          for (int index2 = 0; index2 < this.Patterns[index1].Params.Count; ++index2)
            this.Patterns[index1].Params[index2].Inert = _inert;
        }
        else
        {
          if (this.Patterns[index1].ParticlePtns.Count > _bone)
            this.Patterns[index1].ParticlePtns[_bone].Inert = _inert;
          if (this.Patterns[index1].Params.Count > _bone)
            this.Patterns[index1].Params[_bone].Inert = _inert;
        }
      }
    }
    else
    {
      if (this.Patterns.Count <= _ptn)
        return false;
      if (_bone == -1)
      {
        for (int index = 0; index < this.Patterns[_ptn].ParticlePtns.Count; ++index)
          this.Patterns[_ptn].ParticlePtns[index].Inert = _inert;
        for (int index = 0; index < this.Patterns[_ptn].Params.Count; ++index)
          this.Patterns[_ptn].Params[index].Inert = _inert;
      }
      else
      {
        if (this.Patterns[_ptn].ParticlePtns.Count > _bone)
          this.Patterns[_ptn].ParticlePtns[_bone].Inert = _inert;
        if (this.Patterns[_ptn].Params.Count > _bone)
          this.Patterns[_ptn].Params[_bone].Inert = _inert;
      }
    }
    return true;
  }

  public bool LoadTextList(List<string> _list)
  {
    DynamicBone_Ver02.LoadInfo _info = new DynamicBone_Ver02.LoadInfo();
    int _index = 0;
    do
      ;
    while (_list.Count > _index && this.LoadText(_info, _list, ref _index));
    if (_list.Count > _index)
      return false;
    this.Comment = _info.Comment;
    this.ReflectSpeed = _info.ReflectSpeed;
    this.HeavyLoopMaxCount = _info.HeavyLoopMaxCount;
    this.Colliders = new List<DynamicBoneCollider>((IEnumerable<DynamicBoneCollider>) _info.Colliders);
    this.Bones = new List<Transform>((IEnumerable<Transform>) _info.Bones);
    this.Patterns = new List<DynamicBone_Ver02.BonePtn>();
    foreach (DynamicBone_Ver02.BonePtn pattern in _info.Patterns)
    {
      DynamicBone_Ver02.BonePtn bonePtn = new DynamicBone_Ver02.BonePtn();
      bonePtn.Name = pattern.Name;
      bonePtn.Gravity = pattern.Gravity;
      bonePtn.EndOffset = pattern.EndOffset;
      bonePtn.EndOffsetDamping = pattern.EndOffsetDamping;
      bonePtn.EndOffsetElasticity = pattern.EndOffsetElasticity;
      bonePtn.EndOffsetStiffness = pattern.EndOffsetStiffness;
      bonePtn.EndOffsetInert = pattern.EndOffsetInert;
      foreach (DynamicBone_Ver02.BoneParameter boneParameter in pattern.Params)
        bonePtn.Params.Add(new DynamicBone_Ver02.BoneParameter()
        {
          Name = boneParameter.Name,
          RefTransform = boneParameter.RefTransform,
          IsRotationCalc = boneParameter.IsRotationCalc,
          Damping = boneParameter.Damping,
          Elasticity = boneParameter.Elasticity,
          Stiffness = boneParameter.Stiffness,
          Inert = boneParameter.Inert,
          NextBoneLength = boneParameter.NextBoneLength,
          CollisionRadius = boneParameter.CollisionRadius,
          IsMoveLimit = boneParameter.IsMoveLimit,
          MoveLimitMin = boneParameter.MoveLimitMin,
          MoveLimitMax = boneParameter.MoveLimitMax,
          KeepLengthLimitMin = boneParameter.KeepLengthLimitMin,
          KeepLengthLimitMax = boneParameter.KeepLengthLimitMax,
          IsCrush = boneParameter.IsCrush,
          CrushMoveAreaMin = boneParameter.CrushMoveAreaMin,
          CrushMoveAreaMax = boneParameter.CrushMoveAreaMax,
          CrushAddXYMin = boneParameter.CrushAddXYMin,
          CrushAddXYMax = boneParameter.CrushAddXYMax
        });
      this.Patterns.Add(bonePtn);
    }
    this.InitNodeParticle();
    this.SetupParticles();
    this.InitLocalPosition();
    if (this.IsRefTransform())
      this.setPtn(0, true);
    this.InitTransforms();
    return true;
  }

  private void UpdateDynamicBones(float _deltaTime)
  {
    if (Object.op_Equality((Object) this.Root, (Object) null))
      return;
    this.ObjectScale = Mathf.Abs((float) this.Root.get_lossyScale().x);
    this.ObjectMove = Vector3.op_Subtraction(this.Root.get_position(), this.ObjectPrevPosition);
    this.ObjectPrevPosition = this.Root.get_position();
    int num1 = 1;
    if ((double) this.UpdateRate > 0.0)
    {
      float num2 = 1f / this.UpdateRate;
      this.UpdateTime += _deltaTime;
      num1 = 0;
      while ((double) this.UpdateTime >= (double) num2)
      {
        this.UpdateTime -= num2;
        if (++num1 >= this.HeavyLoopMaxCount)
        {
          this.UpdateTime = 0.0f;
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
        this.ObjectMove = Vector3.get_zero();
      }
    }
    else
      this.SkipUpdateParticles();
    this.ApplyParticlesToTransforms();
  }

  private void InitNodeParticle()
  {
    if (this.Patterns == null)
      return;
    foreach (DynamicBone_Ver02.BonePtn pattern in this.Patterns)
    {
      if (pattern.ParticlePtns != null)
        pattern.ParticlePtns.Clear();
      else
        pattern.ParticlePtns = new List<DynamicBone_Ver02.ParticlePtn>();
      if (pattern.Params.Count != this.Bones.Count)
      {
        Debug.LogWarning((object) "パラメータの数と骨の数があっていない");
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        using (IEnumerator<\u003C\u003E__AnonType0<Transform, int>> enumerator = ((IEnumerable<Transform>) this.Bones).Select<Transform, \u003C\u003E__AnonType0<Transform, int>>((Func<Transform, int, \u003C\u003E__AnonType0<Transform, int>>) ((value, idx) => new \u003C\u003E__AnonType0<Transform, int>(value, idx))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            // ISSUE: variable of a compiler-generated type
            \u003C\u003E__AnonType0<Transform, int> current = enumerator.Current;
            pattern.ParticlePtns.Add(this.AppendParticlePtn(pattern.Params[current.idx], Vector3.get_zero()));
          }
        }
        pattern.ParticlePtns.Add(this.AppendParticlePtn(new DynamicBone_Ver02.BoneParameter()
        {
          Damping = pattern.EndOffsetDamping,
          Elasticity = pattern.EndOffsetElasticity,
          Stiffness = pattern.EndOffsetStiffness,
          Inert = pattern.EndOffsetInert
        }, pattern.EndOffset));
      }
    }
  }

  private void SetupParticles()
  {
    this.Particles.Clear();
    if (Object.op_Equality((Object) this.Root, (Object) null) && this.Bones.Count > 0)
      this.Root = this.Bones[0];
    if (Object.op_Equality((Object) this.Root, (Object) null) || this.Bones == null || (this.Patterns == null || this.Bones.Count == 0) || (this.Patterns.Count == 0 || this.Bones.Count != this.Patterns[0].Params.Count))
      return;
    this.ObjectScale = (float) this.Root.get_lossyScale().x;
    this.ObjectPrevPosition = this.Root.get_position();
    this.ObjectMove = Vector3.get_zero();
    int _parentIndex = -1;
    // ISSUE: object of a compiler-generated type is created
    using (IEnumerator<\u003C\u003E__AnonType0<Transform, int>> enumerator = ((IEnumerable<Transform>) this.Bones).Select<Transform, \u003C\u003E__AnonType0<Transform, int>>((Func<Transform, int, \u003C\u003E__AnonType0<Transform, int>>) ((value, idx) => new \u003C\u003E__AnonType0<Transform, int>(value, idx))).GetEnumerator())
    {
      while (((IEnumerator) enumerator).MoveNext())
      {
        // ISSUE: variable of a compiler-generated type
        \u003C\u003E__AnonType0<Transform, int> current = enumerator.Current;
        this.AppendParticles(current.value, this.Patterns[0].Params[current.idx], Vector3.get_zero(), _parentIndex);
        ++_parentIndex;
      }
    }
    this.AppendParticles((Transform) null, new DynamicBone_Ver02.BoneParameter(), this.Patterns[0].EndOffset, _parentIndex);
  }

  private DynamicBone_Ver02.ParticlePtn AppendParticlePtn(
    DynamicBone_Ver02.BoneParameter _parameter,
    Vector3 _endOffset)
  {
    DynamicBone_Ver02.ParticlePtn particlePtn = new DynamicBone_Ver02.ParticlePtn()
    {
      IsRotationCalc = _parameter.IsRotationCalc,
      Damping = _parameter.Damping,
      Elasticity = _parameter.Elasticity,
      Stiffness = _parameter.Stiffness,
      Inert = _parameter.Inert,
      ScaleNextBoneLength = _parameter.NextBoneLength,
      Radius = _parameter.CollisionRadius,
      IsMoveLimit = _parameter.IsMoveLimit,
      MoveLimitMin = _parameter.MoveLimitMin,
      MoveLimitMax = _parameter.MoveLimitMax,
      KeepLengthLimitMin = _parameter.KeepLengthLimitMin,
      KeepLengthLimitMax = _parameter.KeepLengthLimitMax,
      IsCrush = _parameter.IsCrush,
      CrushMoveAreaMin = _parameter.CrushMoveAreaMin,
      CrushMoveAreaMax = _parameter.CrushMoveAreaMax,
      CrushAddXYMin = _parameter.CrushAddXYMin,
      CrushAddXYMax = _parameter.CrushAddXYMax
    };
    particlePtn.Damping = Mathf.Clamp01(particlePtn.Damping);
    particlePtn.Elasticity = Mathf.Clamp01(particlePtn.Elasticity);
    particlePtn.Stiffness = Mathf.Clamp01(particlePtn.Stiffness);
    particlePtn.Inert = Mathf.Clamp01(particlePtn.Inert);
    particlePtn.ScaleNextBoneLength = Mathf.Max(particlePtn.ScaleNextBoneLength, 0.0f);
    particlePtn.Radius = Mathf.Max(particlePtn.Radius, 0.0f);
    if (Object.op_Inequality((Object) _parameter.RefTransform, (Object) null))
    {
      particlePtn.InitLocalPosition = _parameter.RefTransform.get_localPosition();
      particlePtn.InitLocalRotation = _parameter.RefTransform.get_localRotation();
      particlePtn.InitLocalScale = _parameter.RefTransform.get_localScale();
      particlePtn.refTrans = _parameter.RefTransform;
    }
    else
      particlePtn.EndOffset = _endOffset;
    return particlePtn;
  }

  private DynamicBone_Ver02.Particle AppendParticles(
    Transform _transform,
    DynamicBone_Ver02.BoneParameter _parameter,
    Vector3 _endOffset,
    int _parentIndex)
  {
    DynamicBone_Ver02.Particle _particle = new DynamicBone_Ver02.Particle()
    {
      Transform = _transform,
      IsRotationCalc = _parameter.IsRotationCalc,
      Damping = _parameter.Damping,
      Elasticity = _parameter.Elasticity,
      Stiffness = _parameter.Stiffness,
      Inert = _parameter.Inert,
      ScaleNextBoneLength = _parameter.NextBoneLength,
      Radius = _parameter.CollisionRadius,
      IsMoveLimit = _parameter.IsMoveLimit,
      MoveLimitMin = _parameter.MoveLimitMin,
      MoveLimitMax = _parameter.MoveLimitMax,
      KeepLengthLimitMin = _parameter.KeepLengthLimitMin,
      KeepLengthLimitMax = _parameter.KeepLengthLimitMax,
      IsCrush = _parameter.IsCrush,
      CrushMoveAreaMin = _parameter.CrushMoveAreaMin,
      CrushMoveAreaMax = _parameter.CrushMoveAreaMax,
      CrushAddXYMin = _parameter.CrushAddXYMin,
      CrushAddXYMax = _parameter.CrushAddXYMax,
      ParentIndex = _parentIndex
    };
    _particle.Damping = Mathf.Clamp01(_particle.Damping);
    _particle.Elasticity = Mathf.Clamp01(_particle.Elasticity);
    _particle.Stiffness = Mathf.Clamp01(_particle.Stiffness);
    _particle.Inert = Mathf.Clamp01(_particle.Inert);
    _particle.ScaleNextBoneLength = Mathf.Max(_particle.ScaleNextBoneLength, 0.0f);
    _particle.Radius = Mathf.Max(_particle.Radius, 0.0f);
    if (Object.op_Inequality((Object) _transform, (Object) null))
    {
      _particle.Position = _particle.PrevPosition = _transform.get_position();
      _particle.InitLocalPosition = _transform.get_localPosition();
      _particle.InitLocalRotation = _transform.get_localRotation();
      _particle.refTrans = _transform;
      if (_parentIndex >= 0)
        this.CalcLocalPosition(_particle, this.Particles[_parentIndex]);
    }
    else
    {
      Transform transform = this.Particles[_parentIndex].Transform;
      _particle.EndOffset = _endOffset;
      _particle.Position = _particle.PrevPosition = transform.TransformPoint(_particle.EndOffset);
    }
    this.Particles.Add(_particle);
    return _particle;
  }

  private void InitTransforms()
  {
    int count = this.Particles.Count;
    for (int index = 0; index < count; ++index)
    {
      DynamicBone_Ver02.Particle particle = this.Particles[index];
      if (!Object.op_Equality((Object) particle.Transform, (Object) null))
      {
        if (Object.op_Inequality((Object) particle.refTrans, (Object) null))
        {
          particle.Transform.set_localPosition(particle.refTrans.get_localPosition());
          particle.Transform.set_localRotation(particle.refTrans.get_localRotation());
          particle.Transform.set_localScale(particle.refTrans.get_localScale());
        }
        else
        {
          particle.Transform.set_localPosition(particle.InitLocalPosition);
          particle.Transform.set_localRotation(particle.InitLocalRotation);
          particle.Transform.set_localScale(particle.InitLocalScale);
        }
      }
    }
  }

  private void UpdateParticles1()
  {
    if (this.Patterns == null || this.Patterns != null && this.Patterns.Count == 0)
      return;
    Vector3 vector3_1 = Vector3.op_Multiply(Vector3.op_Addition(this.Gravity, this.Force), this.ObjectScale);
    for (int index = 0; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle1 = this.Particles[index];
      if (particle1.ParentIndex >= 0)
      {
        Vector3 vector3_2 = Vector3.op_Multiply(Vector3.op_Subtraction(particle1.Position, particle1.PrevPosition), this.ReflectSpeed);
        Vector3 vector3_3 = Vector3.op_Multiply(this.ObjectMove, particle1.Inert);
        particle1.PrevPosition = Vector3.op_Addition(particle1.Position, vector3_3);
        DynamicBone_Ver02.Particle particle2 = particle1;
        particle2.Position = Vector3.op_Addition(particle2.Position, Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Multiply(vector3_2, 1f - particle1.Damping), vector3_1), vector3_3));
      }
      else
      {
        particle1.PrevPosition = particle1.Position;
        particle1.Position = particle1.Transform.get_position();
      }
    }
  }

  private void UpdateParticles2()
  {
    for (int index = 1; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle1 = this.Particles[index];
      DynamicBone_Ver02.Particle particle2 = this.Particles[particle1.ParentIndex];
      float num1;
      if (Object.op_Inequality((Object) particle1.Transform, (Object) null))
      {
        Vector3 vector3 = Vector3.op_Subtraction(particle2.Transform.get_position(), particle1.Transform.get_position());
        num1 = ((Vector3) ref vector3).get_magnitude();
      }
      else
        num1 = ((Vector3) ref particle1.EndOffset).get_magnitude() * this.ObjectScale;
      float num2 = Mathf.Lerp(1f, particle1.Stiffness, this.Weight);
      if ((double) num2 > 0.0 || (double) particle1.Elasticity > 0.0)
      {
        Matrix4x4 localToWorldMatrix = particle2.Transform.get_localToWorldMatrix();
        ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle2.Position));
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.LocalPosition);
        Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, particle1.Position);
        DynamicBone_Ver02.Particle particle3 = particle1;
        particle3.Position = Vector3.op_Addition(particle3.Position, Vector3.op_Multiply(vector3_2, particle1.Elasticity));
        if ((double) num2 > 0.0)
        {
          Vector3 vector3_3 = Vector3.op_Subtraction(vector3_1, particle1.Position);
          float magnitude = ((Vector3) ref vector3_3).get_magnitude();
          float num3 = (float) ((double) num1 * (1.0 - (double) num2) * 2.0);
          if ((double) magnitude > (double) num3)
          {
            DynamicBone_Ver02.Particle particle4 = particle1;
            particle4.Position = Vector3.op_Addition(particle4.Position, Vector3.op_Multiply(vector3_3, (magnitude - num3) / magnitude));
          }
        }
      }
      float particleRadius = particle1.Radius * this.ObjectScale;
      foreach (DynamicBoneCollider collider in this.Colliders)
      {
        if (Object.op_Inequality((Object) collider, (Object) null) && ((Behaviour) collider).get_enabled())
          collider.Collide(ref particle1.Position, particleRadius);
      }
      Vector3 vector3_4 = Vector3.op_Subtraction(particle2.Position, particle1.Position);
      float magnitude1 = ((Vector3) ref vector3_4).get_magnitude();
      if ((double) magnitude1 > 0.0)
      {
        float num3 = (magnitude1 - num1) / magnitude1;
        if ((double) particle1.KeepLengthLimitMin >= (double) num3)
        {
          DynamicBone_Ver02.Particle particle3 = particle1;
          particle3.Position = Vector3.op_Addition(particle3.Position, Vector3.op_Multiply(vector3_4, num3 - particle1.KeepLengthLimitMin));
        }
        else if ((double) num3 >= (double) particle1.KeepLengthLimitMax)
        {
          DynamicBone_Ver02.Particle particle3 = particle1;
          particle3.Position = Vector3.op_Addition(particle3.Position, Vector3.op_Multiply(vector3_4, num3 - particle1.KeepLengthLimitMax));
        }
      }
    }
  }

  private void SkipUpdateParticles()
  {
    for (int index = 0; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle1 = this.Particles[index];
      if (particle1.ParentIndex >= 0)
      {
        Vector3 vector3_1 = Vector3.op_Multiply(this.ObjectMove, particle1.Inert);
        DynamicBone_Ver02.Particle particle2 = particle1;
        particle2.PrevPosition = Vector3.op_Addition(particle2.PrevPosition, vector3_1);
        DynamicBone_Ver02.Particle particle3 = particle1;
        particle3.Position = Vector3.op_Addition(particle3.Position, vector3_1);
        DynamicBone_Ver02.Particle particle4 = this.Particles[particle1.ParentIndex];
        float num1;
        if (Object.op_Inequality((Object) particle1.Transform, (Object) null))
        {
          Vector3 vector3_2 = Vector3.op_Subtraction(particle4.Transform.get_position(), particle1.Transform.get_position());
          num1 = ((Vector3) ref vector3_2).get_magnitude();
        }
        else
          num1 = ((Vector3) ref particle1.EndOffset).get_magnitude() * this.ObjectScale;
        float num2 = Mathf.Lerp(1f, particle1.Stiffness, this.Weight);
        if ((double) num2 > 0.0)
        {
          Matrix4x4 localToWorldMatrix = particle4.Transform.get_localToWorldMatrix();
          ((Matrix4x4) ref localToWorldMatrix).SetColumn(3, Vector4.op_Implicit(particle4.Position));
          Vector3 vector3_2 = Vector3.op_Subtraction(!Object.op_Inequality((Object) particle1.Transform, (Object) null) ? ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.EndOffset) : ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(particle1.LocalPosition), particle1.Position);
          float magnitude = ((Vector3) ref vector3_2).get_magnitude();
          float num3 = (float) ((double) num1 * (1.0 - (double) num2) * 2.0);
          if ((double) magnitude > (double) num3)
          {
            DynamicBone_Ver02.Particle particle5 = particle1;
            particle5.Position = Vector3.op_Addition(particle5.Position, Vector3.op_Multiply(vector3_2, (magnitude - num3) / magnitude));
          }
        }
        Vector3 vector3_3 = Vector3.op_Subtraction(particle4.Position, particle1.Position);
        float magnitude1 = ((Vector3) ref vector3_3).get_magnitude();
        if ((double) magnitude1 > 0.0)
        {
          float num3 = (magnitude1 - num1) / magnitude1;
          if ((double) particle1.KeepLengthLimitMin >= (double) num3)
          {
            DynamicBone_Ver02.Particle particle5 = particle1;
            particle5.Position = Vector3.op_Addition(particle5.Position, Vector3.op_Multiply(vector3_3, num3 - particle1.KeepLengthLimitMin));
          }
          else if ((double) num3 >= (double) particle1.KeepLengthLimitMax)
          {
            DynamicBone_Ver02.Particle particle5 = particle1;
            particle5.Position = Vector3.op_Addition(particle5.Position, Vector3.op_Multiply(vector3_3, num3 - particle1.KeepLengthLimitMax));
          }
        }
      }
      else
      {
        particle1.PrevPosition = particle1.Position;
        particle1.Position = particle1.Transform.get_position();
      }
    }
  }

  private void ApplyParticlesToTransforms()
  {
    for (int index = 1; index < this.Particles.Count; ++index)
    {
      DynamicBone_Ver02.Particle particle1 = this.Particles[index];
      DynamicBone_Ver02.Particle particle2 = this.Particles[particle1.ParentIndex];
      if (particle2.IsRotationCalc)
      {
        Vector3 vector3_1 = !Object.op_Inequality((Object) particle1.Transform, (Object) null) ? particle1.EndOffset : particle1.LocalPosition;
        Vector3 vector3_2 = particle2.Transform.TransformDirection(vector3_1);
        Vector3 vector3_3 = Vector3.op_Subtraction(particle1.Position, particle2.Position);
        if ((double) ((Vector3) ref vector3_1).get_magnitude() != 0.0)
          vector3_3 = Vector3.op_Subtraction(Vector3.op_Addition(particle1.Position, Vector3.op_Multiply(Vector3.op_Multiply(vector3_2, -1f), 1f - particle2.ScaleNextBoneLength)), particle2.Position);
        Quaternion rotation = Quaternion.FromToRotation(vector3_2, vector3_3);
        particle2.Transform.set_rotation(Quaternion.op_Multiply(rotation, particle2.Transform.get_rotation()));
      }
      if (Object.op_Implicit((Object) particle1.Transform))
      {
        Matrix4x4 localToWorldMatrix = particle1.Transform.get_localToWorldMatrix();
        Matrix4x4 inverse = ((Matrix4x4) ref localToWorldMatrix).get_inverse();
        Vector3 vector3 = ((Matrix4x4) ref inverse).MultiplyPoint3x4(particle1.Position);
        if (particle1.IsCrush)
        {
          float num1;
          if (vector3.z <= 0.0)
          {
            float num2 = Mathf.Clamp01(Mathf.InverseLerp(particle1.CrushMoveAreaMin, 0.0f, (float) vector3.z));
            num1 = particle1.CrushAddXYMin * (1f - num2);
          }
          else
          {
            float num2 = Mathf.Clamp01(Mathf.InverseLerp(0.0f, particle1.CrushMoveAreaMax, (float) vector3.z));
            num1 = particle1.CrushAddXYMax * num2;
          }
          particle1.Transform.set_localScale(Vector3.op_Addition(particle1.InitLocalScale, new Vector3(num1, num1, 0.0f)));
        }
        if (particle1.IsMoveLimit)
        {
          vector3.x = (__Null) (double) Mathf.Clamp((float) vector3.x, (float) particle1.MoveLimitMin.x, (float) particle1.MoveLimitMax.x);
          vector3.y = (__Null) (double) Mathf.Clamp((float) vector3.y, (float) particle1.MoveLimitMin.y, (float) particle1.MoveLimitMax.y);
          vector3.z = (__Null) (double) Mathf.Clamp((float) vector3.z, (float) particle1.MoveLimitMin.z, (float) particle1.MoveLimitMax.z);
          particle1.Transform.set_position(((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(vector3));
        }
        else
          particle1.Transform.set_position(particle1.Position);
      }
    }
  }

  private void CalcLocalPosition(
    DynamicBone_Ver02.Particle _particle,
    DynamicBone_Ver02.Particle _parent)
  {
    _particle.LocalPosition = _parent.Transform.InverseTransformPoint(_particle.Position);
  }

  private Vector3 CalcLocalPosition(Vector3 _particle, Transform _parent)
  {
    return _parent.InverseTransformPoint(_particle);
  }

  private bool IsRefTransform()
  {
    if (this.Patterns == null)
      return false;
    foreach (DynamicBone_Ver02.BonePtn pattern in this.Patterns)
    {
      if (pattern.Params == null)
        return false;
      foreach (DynamicBone_Ver02.BoneParameter boneParameter in pattern.Params)
      {
        if (Object.op_Equality((Object) boneParameter.RefTransform, (Object) null))
          return false;
      }
    }
    return true;
  }

  private Transform FindLoop(Transform transform, string name)
  {
    if (string.Compare(name, ((Object) transform).get_name()) == 0)
      return transform;
    IEnumerator enumerator = transform.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform loop = this.FindLoop((Transform) enumerator.Current, name);
        if (Object.op_Inequality((Object) null, (Object) loop))
          return loop;
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    return (Transform) null;
  }

  private bool LoadText(DynamicBone_Ver02.LoadInfo _info, List<string> _list, ref int _index)
  {
    string[] _str = _list[_index].Split('\t');
    int length = _str.Length;
    if (length == 0)
    {
      Debug.LogWarning((object) "行に空欄があります？");
      return false;
    }
    if (_str[0].Substring(0, 2).Equals("//"))
    {
      ++_index;
      return true;
    }
    switch (_str[0])
    {
      case "#Comment":
        _info.Comment = _str[1];
        break;
      case "#ReflectSpeed":
        float result1;
        if (!float.TryParse(_str[1], out result1))
        {
          Debug.LogWarning((object) "ReflectSpeedの数値取得に失敗");
          return false;
        }
        _info.ReflectSpeed = result1;
        break;
      case "#HeavyLoopMaxCount":
        int result2;
        if (!int.TryParse(_str[1], out result2))
        {
          Debug.LogWarning((object) "HeavyLoopMaxCountの数値取得に失敗");
          return false;
        }
        _info.HeavyLoopMaxCount = result2;
        break;
      case "#Colliders name":
        for (int index = 1; index < length && (!(_str[index] == string.Empty) && !(_str[index] == " ")); ++index)
        {
          Transform loop = this.FindLoop(((Component) this).get_transform(), _str[index]);
          if (Object.op_Equality((Object) loop, (Object) null))
          {
            Debug.LogWarning((object) ("[Colliders name] " + _str[index] + " この名前のフレームは見つかりません"));
            return false;
          }
          DynamicBoneCollider component = (DynamicBoneCollider) ((Component) loop).GetComponent<DynamicBoneCollider>();
          if (Object.op_Equality((Object) component, (Object) null))
          {
            Debug.LogWarning((object) ("[Colliders name] " + _str[index] + " このフレームにDynamicBoneColliderコンポーネントは付いていません"));
            return false;
          }
          _info.Colliders.Add(component);
        }
        break;
      case "#Bone name":
        for (int index = 1; index < length && (!(_str[index] == string.Empty) && !(_str[index] == " ")); ++index)
        {
          Transform loop = this.FindLoop(((Component) this).get_transform(), _str[index]);
          if (Object.op_Equality((Object) loop, (Object) null))
          {
            Debug.LogWarning((object) ("[Bone name] " + _str[index] + " この名前のフレームは見つかりません"));
            return false;
          }
          _info.Bones.Add(loop);
        }
        break;
      case "#PtnClassMember":
        DynamicBone_Ver02.BonePtn _ptn = new DynamicBone_Ver02.BonePtn();
        if (!this.LoadPtnClassMember(_ptn, _str, _index))
          return false;
        ++_index;
        if (!this.LoadParamClassMember(_ptn, _list, ref _index))
          return false;
        _info.Patterns.Add(_ptn);
        return true;
      default:
        Debug.LogWarning((object) ("テキスト：識別不能 " + _str[0] + " [index]:" + (object) _index));
        return false;
    }
    ++_index;
    return true;
  }

  private bool LoadPtnClassMember(DynamicBone_Ver02.BonePtn _ptn, string[] _str, int _index)
  {
    int length = _str.Length;
    int _index1 = 0;
    if (!this.ChekcLength(length, ref _index1, _index, "[PtnClassMember] 表示する名前", string.Empty))
      return false;
    _ptn.Name = _str[_index1];
    float num;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] 重力X", string.Empty))
      return false;
    Vector3 vector3 = _ptn.Gravity;
    vector3.x = (__Null) (double) num;
    _ptn.Gravity = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] 重力Y", string.Empty))
      return false;
    vector3 = _ptn.Gravity;
    vector3.y = (__Null) (double) num;
    _ptn.Gravity = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] 重力Z", string.Empty))
      return false;
    vector3 = _ptn.Gravity;
    vector3.z = (__Null) (double) num;
    _ptn.Gravity = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetX", string.Empty))
      return false;
    vector3 = _ptn.EndOffset;
    vector3.x = (__Null) (double) num;
    _ptn.EndOffset = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetY", string.Empty))
      return false;
    vector3 = _ptn.EndOffset;
    vector3.y = (__Null) (double) num;
    _ptn.EndOffset = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetZ", string.Empty))
      return false;
    vector3 = _ptn.EndOffset;
    vector3.z = (__Null) (double) num;
    _ptn.EndOffset = vector3;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetの空気抵抗", string.Empty))
      return false;
    _ptn.EndOffsetDamping = num;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetの弾力", string.Empty))
      return false;
    _ptn.EndOffsetElasticity = num;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetの硬さ", string.Empty))
      return false;
    _ptn.EndOffsetStiffness = num;
    if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[PtnClassMember] EndOffsetの惰性", string.Empty))
      return false;
    _ptn.EndOffsetInert = num;
    return true;
  }

  private bool LoadParamClassMember(
    DynamicBone_Ver02.BonePtn _ptn,
    List<string> _list,
    ref int _index)
  {
    while (_list.Count > _index)
    {
      string[] _str = _list[_index].Split('\t');
      int length = _str.Length;
      int _index1 = 0;
      float num = 0.0f;
      bool flag = false;
      if (length <= _index1)
      {
        Debug.LogWarning((object) "[ParamClassMember] 行に空欄があります？");
        return false;
      }
      if (_str[_index1].Substring(0, 2).Equals("//"))
        ++_index;
      else if (!(_str[_index1] != "#ParamClassMember"))
      {
        DynamicBone_Ver02.BoneParameter boneParameter = new DynamicBone_Ver02.BoneParameter();
        if (!this.ChekcLength(length, ref _index1, _index, "[ParamClassMember] 表示する名前", string.Empty))
          return false;
        boneParameter.Name = _str[_index1];
        if (!this.ChekcLength(length, ref _index1, _index, "[ParamClassMember] 参照するフレーム", string.Empty))
          return false;
        Transform loop = this.FindLoop(((Component) this).get_transform(), _str[_index1]);
        if (Object.op_Equality((Object) loop, (Object) null))
        {
          Debug.LogWarning((object) ("[ParamClassMember] " + _str[_index1] + " この名前のフレームは見つかりません"));
          return false;
        }
        boneParameter.RefTransform = loop;
        if (!this.GetMemberBool(length, _str, ref _index1, _index, out flag, "[ParamClassMember] 回転するか ", string.Empty))
          return false;
        boneParameter.IsRotationCalc = flag;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 空気抵抗", string.Empty))
          return false;
        boneParameter.Damping = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 弾力", string.Empty))
          return false;
        boneParameter.Elasticity = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 硬さ", string.Empty))
          return false;
        boneParameter.Stiffness = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 惰性", string.Empty))
          return false;
        boneParameter.Inert = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 次の骨までの距離補正", string.Empty))
          return false;
        boneParameter.NextBoneLength = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 当たり判定の半径", string.Empty))
          return false;
        boneParameter.CollisionRadius = num;
        if (!this.GetMemberBool(length, _str, ref _index1, _index, out flag, "[ParamClassMember] 移動制限するか ", string.Empty))
          return false;
        boneParameter.IsMoveLimit = flag;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最小X", string.Empty))
          return false;
        Vector3 moveLimitMin1 = boneParameter.MoveLimitMin;
        moveLimitMin1.x = (__Null) (double) num;
        boneParameter.MoveLimitMin = moveLimitMin1;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最小Y", string.Empty))
          return false;
        Vector3 moveLimitMin2 = boneParameter.MoveLimitMin;
        moveLimitMin2.y = (__Null) (double) num;
        boneParameter.MoveLimitMin = moveLimitMin2;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最小Z", string.Empty))
          return false;
        Vector3 moveLimitMin3 = boneParameter.MoveLimitMin;
        moveLimitMin3.z = (__Null) (double) num;
        boneParameter.MoveLimitMin = moveLimitMin3;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最大X", string.Empty))
          return false;
        Vector3 moveLimitMax1 = boneParameter.MoveLimitMax;
        moveLimitMax1.x = (__Null) (double) num;
        boneParameter.MoveLimitMax = moveLimitMax1;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最大Y", string.Empty))
          return false;
        Vector3 moveLimitMax2 = boneParameter.MoveLimitMax;
        moveLimitMax2.y = (__Null) (double) num;
        boneParameter.MoveLimitMax = moveLimitMax2;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 移動制限最大Z", string.Empty))
          return false;
        Vector3 moveLimitMax3 = boneParameter.MoveLimitMax;
        moveLimitMax3.z = (__Null) (double) num;
        boneParameter.MoveLimitMax = moveLimitMax3;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 親からの長さの補正しない範囲最小", string.Empty))
          return false;
        boneParameter.KeepLengthLimitMin = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 親からの長さの補正しない範囲最大", string.Empty))
          return false;
        boneParameter.KeepLengthLimitMax = num;
        if (!this.GetMemberBool(length, _str, ref _index1, _index, out flag, "[ParamClassMember] 潰すか ", string.Empty))
          return false;
        boneParameter.IsCrush = flag;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 潰す移動判断範囲最小", string.Empty))
          return false;
        boneParameter.CrushMoveAreaMin = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 潰す移動判断範囲最大", string.Empty))
          return false;
        boneParameter.CrushMoveAreaMax = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 潰れた時に加算するXYスケール", string.Empty))
          return false;
        boneParameter.CrushAddXYMin = num;
        if (!this.GetMemberFloat(length, _str, ref _index1, _index, out num, "[ParamClassMember] 伸びた時に加算するXYスケール", string.Empty))
          return false;
        boneParameter.CrushAddXYMax = num;
        _ptn.Params.Add(boneParameter);
        ++_index;
      }
      else
        break;
    }
    return true;
  }

  private bool ChekcLength(
    int _length,
    ref int _index,
    int _line,
    string _warning = "",
    string _warning1 = "")
  {
    if (_length > ++_index)
      return true;
    Debug.LogWarning((object) (_warning + "までの設定数が足りない [" + (object) _line + "行目] " + _warning1));
    return false;
  }

  private bool GetMemberFloat(
    int _length,
    string[] _str,
    ref int _index,
    int _line,
    out float _param,
    string _warning = "",
    string _warning1 = "")
  {
    _param = 0.0f;
    if (!this.ChekcLength(_length, ref _index, _line, _warning, string.Empty))
      return false;
    if (float.TryParse(_str[_index], out _param))
      return true;
    Debug.LogWarning((object) (_warning + "が取得できない [" + (object) _line + "行目] " + _str[_index] + _warning1));
    return false;
  }

  private bool GetMemberInt(
    int _length,
    string[] _str,
    ref int _index,
    int _line,
    out int _param,
    string _warning = "",
    string _warning1 = "")
  {
    _param = 0;
    if (!this.ChekcLength(_length, ref _index, _line, _warning, string.Empty))
      return false;
    if (int.TryParse(_str[_index], out _param))
      return true;
    Debug.LogWarning((object) (_warning + "が取得できない [" + (object) _line + "行目] " + _str[_index] + _warning1));
    return false;
  }

  private bool GetMemberBool(
    int _length,
    string[] _str,
    ref int _index,
    int _line,
    out bool _param,
    string _warning = "",
    string _warning1 = "")
  {
    _param = false;
    if (!this.ChekcLength(_length, ref _index, _line, _warning, string.Empty))
      return false;
    if (_str[_index] == "false" || _str[_index] == "FALSE" || _str[_index] == "False")
    {
      _param = false;
      return true;
    }
    if (_str[_index] == "true" || _str[_index] == "TRUE" || _str[_index] == "True")
    {
      _param = true;
      return true;
    }
    Debug.LogWarning((object) (_warning + "が取得できない [" + (object) _line + "行目] " + _str[_index] + _warning1));
    return false;
  }

  private void SaveText(StreamWriter _writer)
  {
    _writer.Write("//コメント\n");
    _writer.Write("#Comment\t" + this.Comment + "\n");
    _writer.Write("//粒子のスピード\n");
    _writer.Write("#ReflectSpeed\t" + this.ReflectSpeed.ToString() + "\n");
    _writer.Write("//重い時に何回まで回すか　回数多いと正確になるけど更に重くなるよ\n");
    _writer.Write("#HeavyLoopMaxCount\t" + this.HeavyLoopMaxCount.ToString() + "\n");
    _writer.Write("//登録する当たり判定の名前\n");
    _writer.Write("#Colliders name\t");
    foreach (DynamicBoneCollider collider in this.Colliders)
      _writer.Write(((Object) ((Component) collider).get_gameObject()).get_name() + "\t");
    _writer.Write("\n");
    _writer.Write("//登録する骨の名前\n");
    _writer.Write("#Bone name\t");
    using (List<Transform>.Enumerator enumerator = this.Bones.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Transform current = enumerator.Current;
        _writer.Write(((Object) current).get_name() + "\t");
      }
    }
    _writer.Write("\n");
    foreach (DynamicBone_Ver02.BonePtn pattern in this.Patterns)
    {
      _writer.Write("//パターンの設定\n");
      _writer.Write("//PtnClass\t");
      _writer.Write("表示する名前\t");
      _writer.Write("重力 X\t");
      _writer.Write("重力 Y\t");
      _writer.Write("重力 Z\t");
      _writer.Write("EndOffset x\t");
      _writer.Write("EndOffset y\t");
      _writer.Write("EndOffset z\t");
      _writer.Write("EndOffsetの空気抵抗\t");
      _writer.Write("EndOffsetの弾力\t");
      _writer.Write("EndOffsetの硬さ\t");
      _writer.Write("EndOffsetの惰性\t");
      _writer.Write("\n");
      _writer.Write("#PtnClassMember\t");
      _writer.Write(pattern.Name + "\t");
      _writer.Write(pattern.Gravity.x.ToString() + "\t");
      _writer.Write(pattern.Gravity.y.ToString() + "\t");
      _writer.Write(pattern.Gravity.z.ToString() + "\t");
      _writer.Write(pattern.EndOffset.x.ToString() + "\t");
      _writer.Write(pattern.EndOffset.y.ToString() + "\t");
      _writer.Write(pattern.EndOffset.z.ToString() + "\t");
      _writer.Write(pattern.EndOffsetDamping.ToString() + "\t");
      _writer.Write(pattern.EndOffsetElasticity.ToString() + "\t");
      _writer.Write(pattern.EndOffsetStiffness.ToString() + "\t");
      _writer.Write(pattern.EndOffsetInert.ToString() + "\t");
      _writer.Write("\n");
      _writer.Write("//そのパターンの骨に対するパラメーター\n");
      _writer.Write("//ParamClass\t");
      _writer.Write("表示する名前\t");
      _writer.Write("参照するフレーム名\t");
      _writer.Write("回転する？\t");
      _writer.Write("空気抵抗\t");
      _writer.Write("弾力\t");
      _writer.Write("硬さ\t");
      _writer.Write("惰性\t");
      _writer.Write("次の骨までの距離補正\t");
      _writer.Write("当たり判定の半径\t");
      _writer.Write("移動制限する？\t");
      _writer.Write("移動制限最小X\t");
      _writer.Write("移動制限最小Y\t");
      _writer.Write("移動制限最小Z\t");
      _writer.Write("移動制限最大X\t");
      _writer.Write("移動制限最大Y\t");
      _writer.Write("移動制限最大Z\t");
      _writer.Write("親からの長さを補正しない範囲最小値\t");
      _writer.Write("親からの長さを補正しない範囲最大値\t");
      _writer.Write("潰す？\t");
      _writer.Write("潰す移動判断範囲最小\t");
      _writer.Write("潰す移動判断範囲最大\t");
      _writer.Write("潰れた時に加算するXYスケール\t");
      _writer.Write("伸びた時に加算するXYスケール\t");
      _writer.Write("\n");
      foreach (DynamicBone_Ver02.BoneParameter boneParameter in pattern.Params)
      {
        _writer.Write("#ParamClassMember\t");
        _writer.Write(boneParameter.Name + "\t");
        string str = string.Empty;
        if (Object.op_Inequality((Object) boneParameter.RefTransform, (Object) null))
          str = ((Object) boneParameter.RefTransform).get_name();
        _writer.Write(str + "\t");
        _writer.Write(boneParameter.IsRotationCalc.ToString() + "\t");
        _writer.Write(boneParameter.Damping.ToString() + "\t");
        _writer.Write(boneParameter.Elasticity.ToString() + "\t");
        _writer.Write(boneParameter.Stiffness.ToString() + "\t");
        _writer.Write(boneParameter.Inert.ToString() + "\t");
        _writer.Write(boneParameter.NextBoneLength.ToString() + "\t");
        _writer.Write(boneParameter.CollisionRadius.ToString() + "\t");
        _writer.Write(boneParameter.IsMoveLimit.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMin.x.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMin.y.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMin.z.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMax.x.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMax.y.ToString() + "\t");
        _writer.Write(boneParameter.MoveLimitMax.z.ToString() + "\t");
        _writer.Write(boneParameter.KeepLengthLimitMin.ToString() + "\t");
        _writer.Write(boneParameter.KeepLengthLimitMax.ToString() + "\t");
        _writer.Write(boneParameter.IsCrush.ToString() + "\t");
        _writer.Write(boneParameter.CrushMoveAreaMin.ToString() + "\t");
        _writer.Write(boneParameter.CrushMoveAreaMin.ToString() + "\t");
        _writer.Write(boneParameter.CrushAddXYMin.ToString() + "\t");
        _writer.Write(boneParameter.CrushAddXYMax.ToString() + "\t");
        _writer.Write("\n");
      }
    }
  }

  [Serializable]
  public class BoneParameter
  {
    public string Name = string.Empty;
    [Range(0.0f, 100f)]
    [Tooltip("次の骨までの長さの制御(回転に影響する：短いと回りやすい(角度が出やすい)　長いと回りにくい(角度が出にくい))")]
    public float NextBoneLength = 1f;
    [Tooltip("ローカル移動制限最小")]
    public Vector3 MoveLimitMin = Vector3.get_zero();
    [Tooltip("ローカル移動制限最大")]
    public Vector3 MoveLimitMax = Vector3.get_zero();
    [Tooltip("参照骨")]
    public Transform RefTransform;
    [Tooltip("回転させる？")]
    public bool IsRotationCalc;
    [Range(0.0f, 1f)]
    [Tooltip("空気抵抗")]
    public float Damping;
    [Range(0.0f, 1f)]
    [Tooltip("弾力(元の位置に戻ろうとする力)")]
    public float Elasticity;
    [Range(0.0f, 1f)]
    [Tooltip("硬さ(要は移動のリミット：移動制限)")]
    public float Stiffness;
    [Range(0.0f, 1f)]
    [Tooltip("惰性(ルートが動いた分を加算するか 加算すると親子付されてる感じになる？)")]
    public float Inert;
    [Tooltip("コリジョンの大きさ")]
    public float CollisionRadius;
    [Tooltip("移動制限")]
    public bool IsMoveLimit;
    [Tooltip("骨の長さを留める制限最小")]
    public float KeepLengthLimitMin;
    [Tooltip("骨の長さを留める制限最大")]
    public float KeepLengthLimitMax;
    [Tooltip("潰れ制御")]
    public bool IsCrush;
    [Tooltip("潰れ範囲最小 この間で設定されたスケール値を足す 判定はローカル位置のZ値")]
    public float CrushMoveAreaMin;
    [Tooltip("潰れ範囲最大 この間で設定されたスケール値を足す 判定はローカル位置のZ値")]
    public float CrushMoveAreaMax;
    [Tooltip("潰れた時に加算するXYスケール")]
    public float CrushAddXYMin;
    [Tooltip("伸びた時に加算するXYスケール")]
    public float CrushAddXYMax;

    public BoneParameter()
    {
      this.Name = string.Empty;
      this.IsRotationCalc = false;
      this.Damping = 0.0f;
      this.Elasticity = 0.0f;
      this.Stiffness = 0.0f;
      this.Inert = 0.0f;
      this.NextBoneLength = 1f;
      this.CollisionRadius = 0.0f;
      this.IsMoveLimit = false;
      this.MoveLimitMin = Vector3.get_zero();
      this.MoveLimitMax = Vector3.get_zero();
      this.KeepLengthLimitMin = 0.0f;
      this.KeepLengthLimitMax = 0.0f;
      this.IsCrush = false;
      this.CrushMoveAreaMin = 0.0f;
      this.CrushMoveAreaMax = 0.0f;
      this.CrushAddXYMin = 0.0f;
      this.CrushAddXYMax = 0.0f;
    }
  }

  public class ParticlePtn
  {
    public bool IsRotationCalc = true;
    public float ScaleNextBoneLength = 1f;
    public Vector3 MoveLimitMin = Vector3.get_zero();
    public Vector3 MoveLimitMax = Vector3.get_zero();
    public Vector3 EndOffset = Vector3.get_zero();
    public Vector3 InitLocalPosition = Vector3.get_zero();
    public Quaternion InitLocalRotation = Quaternion.get_identity();
    public Vector3 InitLocalScale = Vector3.get_zero();
    public Vector3 LocalPosition = Vector3.get_zero();
    public float Damping;
    public float Elasticity;
    public float Stiffness;
    public float Inert;
    public float Radius;
    public bool IsMoveLimit;
    public float KeepLengthLimitMin;
    public float KeepLengthLimitMax;
    public bool IsCrush;
    public float CrushMoveAreaMin;
    public float CrushMoveAreaMax;
    public float CrushAddXYMin;
    public float CrushAddXYMax;
    public Transform refTrans;

    public ParticlePtn()
    {
      this.Damping = 0.0f;
      this.Elasticity = 0.0f;
      this.Stiffness = 0.0f;
      this.Inert = 0.0f;
      this.Radius = 0.0f;
      this.IsRotationCalc = true;
      this.ScaleNextBoneLength = 1f;
      this.IsMoveLimit = false;
      this.MoveLimitMin = Vector3.get_zero();
      this.MoveLimitMax = Vector3.get_zero();
      this.KeepLengthLimitMin = 0.0f;
      this.KeepLengthLimitMax = 0.0f;
      this.IsCrush = false;
      this.CrushMoveAreaMin = 0.0f;
      this.CrushMoveAreaMax = 0.0f;
      this.CrushAddXYMin = 0.0f;
      this.CrushAddXYMax = 0.0f;
      this.EndOffset = Vector3.get_zero();
      this.InitLocalPosition = Vector3.get_zero();
      this.InitLocalRotation = Quaternion.get_identity();
      this.InitLocalScale = Vector3.get_zero();
      this.refTrans = (Transform) null;
      this.LocalPosition = Vector3.get_zero();
    }
  }

  [Serializable]
  public class BonePtn
  {
    public string Name = string.Empty;
    [Tooltip("重力")]
    public Vector3 Gravity = Vector3.get_zero();
    [Tooltip("最後の骨を回すために必要")]
    public Vector3 EndOffset = Vector3.get_zero();
    public List<DynamicBone_Ver02.BoneParameter> Params = new List<DynamicBone_Ver02.BoneParameter>();
    public List<DynamicBone_Ver02.ParticlePtn> ParticlePtns = new List<DynamicBone_Ver02.ParticlePtn>();
    [Range(0.0f, 1f)]
    [Tooltip("空気抵抗")]
    public float EndOffsetDamping;
    [Range(0.0f, 1f)]
    [Tooltip("弾力(元の位置に戻ろうとする力)")]
    public float EndOffsetElasticity;
    [Range(0.0f, 1f)]
    [Tooltip("硬さ(要は移動のリミット：移動制限)")]
    public float EndOffsetStiffness;
    [Range(0.0f, 1f)]
    [Tooltip("惰性(ルートが動いた分を加算するか 加算すると親子付されてる感じになる？)")]
    public float EndOffsetInert;
  }

  public class Particle
  {
    public int ParentIndex = -1;
    public bool IsRotationCalc = true;
    public float ScaleNextBoneLength = 1f;
    public Vector3 MoveLimitMin = Vector3.get_zero();
    public Vector3 MoveLimitMax = Vector3.get_zero();
    public Vector3 Position = Vector3.get_zero();
    public Vector3 PrevPosition = Vector3.get_zero();
    public Vector3 EndOffset = Vector3.get_zero();
    public Vector3 InitLocalPosition = Vector3.get_zero();
    public Quaternion InitLocalRotation = Quaternion.get_identity();
    public Vector3 InitLocalScale = Vector3.get_zero();
    public Vector3 LocalPosition = Vector3.get_zero();
    public Transform Transform;
    public float Damping;
    public float Elasticity;
    public float Stiffness;
    public float Inert;
    public float Radius;
    public bool IsMoveLimit;
    public float KeepLengthLimitMin;
    public float KeepLengthLimitMax;
    public bool IsCrush;
    public float CrushMoveAreaMin;
    public float CrushMoveAreaMax;
    public float CrushAddXYMin;
    public float CrushAddXYMax;
    public Transform refTrans;
  }

  public class LoadInfo
  {
    public List<DynamicBoneCollider> Colliders = new List<DynamicBoneCollider>();
    public List<Transform> Bones = new List<Transform>();
    public List<DynamicBone_Ver02.BonePtn> Patterns = new List<DynamicBone_Ver02.BonePtn>();
    public string Comment;
    public float ReflectSpeed;
    public int HeavyLoopMaxCount;
  }

  private class TransformParam
  {
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
  }
}
