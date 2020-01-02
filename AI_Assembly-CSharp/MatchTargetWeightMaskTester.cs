// Decompiled with JetBrains decompiler
// Type: MatchTargetWeightMaskTester
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class MatchTargetWeightMaskTester : MonoBehaviour
{
  [SerializeField]
  private Animator _animator;
  [SerializeField]
  private string _stateName;
  [SerializeField]
  private AvatarTarget _targetBodyPart;
  [SerializeField]
  private MatchTargetWeightMaskTester.TargetParameter[] _targets;
  [Header("Weights")]
  [SerializeField]
  private Vector3 _positionWeight;
  [SerializeField]
  private float _rotationWeight;
  private MatchTargetWeightMask _weightMask;
  private bool _started;

  public MatchTargetWeightMaskTester()
  {
    base.\u002Ector();
  }

  public Animator Animator
  {
    get
    {
      return this._animator;
    }
    set
    {
      this._animator = value;
    }
  }

  public string StateName
  {
    get
    {
      return this._stateName;
    }
    set
    {
      this._stateName = value;
    }
  }

  public AvatarTarget TargetBodyPart
  {
    get
    {
      return this._targetBodyPart;
    }
    set
    {
      this._targetBodyPart = value;
    }
  }

  public MatchTargetWeightMaskTester.TargetParameter[] Targets
  {
    get
    {
      return this._targets;
    }
    set
    {
      this._targets = value;
    }
  }

  public Vector3 PositionWeight
  {
    get
    {
      return this._positionWeight;
    }
    set
    {
      this._positionWeight = value;
    }
  }

  public float RotationWeight
  {
    get
    {
      return this._rotationWeight;
    }
    set
    {
      this._rotationWeight = value;
    }
  }

  public bool IsPlaying { get; set; }

  private void Update()
  {
    AnimatorStateInfo animatorStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);
    bool flag1 = ((AnimatorStateInfo) ref animatorStateInfo).IsName(this._stateName);
    bool flag2 = this._animator.IsInTransition(0);
    if (this.IsPlaying)
    {
      if (flag1 && (double) ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime() < 0.899999976158142 && !flag2)
      {
        MatchTargetWeightMask targetWeightMask;
        ((MatchTargetWeightMask) ref targetWeightMask).\u002Ector(this._positionWeight, this._rotationWeight);
        foreach (MatchTargetWeightMaskTester.TargetParameter target in this._targets)
        {
          if (Object.op_Inequality((Object) target.Target, (Object) null))
            this._animator.MatchTarget(target.Target.get_position(), target.Target.get_rotation(), this._targetBodyPart, targetWeightMask, target.Start, target.End);
        }
      }
      if (!flag1 && this._started)
        this.IsPlaying = false;
      if (this._started)
        return;
      this._started = true;
    }
    else
      this._started = false;
  }

  public void PlayAnim()
  {
    ((Component) this).get_transform().set_position(Vector3.get_zero());
    this.Animator.Play(this.StateName, 0, 0.0f);
    this.IsPlaying = true;
  }

  private void Reset()
  {
    this._animator = (Animator) ((Component) this).GetComponent<Animator>();
  }

  [Serializable]
  public class TargetParameter
  {
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _end = 1f;
    [SerializeField]
    [Range(0.0f, 1f)]
    private float _start;
    [SerializeField]
    private Transform _target;

    public float Start
    {
      get
      {
        return this._start;
      }
      set
      {
        this._start = value;
      }
    }

    public float End
    {
      get
      {
        return this._end;
      }
      set
      {
        this._end = value;
      }
    }

    public Transform Target
    {
      get
      {
        return this._target;
      }
      set
      {
        this._target = value;
      }
    }
  }
}
