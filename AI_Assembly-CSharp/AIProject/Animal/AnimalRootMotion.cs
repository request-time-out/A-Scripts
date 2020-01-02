// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.AnimalRootMotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIProject.Animal
{
  public class AnimalRootMotion : MonoBehaviour
  {
    [SerializeField]
    private Animator animator;
    public Action<Vector3, Quaternion> OnMove;

    public AnimalRootMotion()
    {
      base.\u002Ector();
    }

    public Animator Animator
    {
      get
      {
        return this.animator;
      }
      set
      {
        this.animator = value;
        this.ActiveAnimator = Object.op_Inequality((Object) this.animator, (Object) null);
      }
    }

    public bool ActiveAnimator { get; private set; }

    public bool UpdateRootMotion { get; set; }

    private void Awake()
    {
      if (!Object.op_Equality((Object) this.Animator, (Object) null))
        return;
      this.Animator = (Animator) ((Component) this).GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
      if (!this.ActiveAnimator || !this.UpdateRootMotion)
        return;
      Vector3 rootPosition = this.Animator.get_rootPosition();
      Quaternion rootRotation = this.Animator.get_rootRotation();
      if (this.OnMove == null)
        return;
      this.OnMove(rootPosition, rootRotation);
    }
  }
}
