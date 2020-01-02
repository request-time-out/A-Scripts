// Decompiled with JetBrains decompiler
// Type: AnimationSpeedDebuff
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AnimationSpeedDebuff : MonoBehaviour
{
  public AnimationCurve AnimationSpeenOnTime;
  public float MaxTime;
  private Animator myAnimator;
  private Transform root;
  private float oldSpeed;
  private float time;

  public AnimationSpeedDebuff()
  {
    base.\u002Ector();
  }

  private void GetAnimatorOnParent(Transform t)
  {
    Animator component = (Animator) ((Component) t.get_parent()).GetComponent<Animator>();
    if (Object.op_Equality((Object) component, (Object) null))
    {
      if (Object.op_Equality((Object) this.root, (Object) t.get_parent()))
        return;
      this.GetAnimatorOnParent(t.get_parent());
    }
    else
      this.myAnimator = component;
  }

  private void Start()
  {
    this.root = ((Component) this).get_transform().get_root();
    this.GetAnimatorOnParent(((Component) this).get_transform());
    if (Object.op_Equality((Object) this.myAnimator, (Object) null))
      return;
    this.oldSpeed = this.myAnimator.get_speed();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.myAnimator, (Object) null) || this.AnimationSpeenOnTime.get_length() == 0)
      return;
    this.time += Time.get_deltaTime();
    this.myAnimator.set_speed(Mathf.Clamp01(this.AnimationSpeenOnTime.Evaluate(this.time / this.MaxTime) * this.oldSpeed));
  }
}
