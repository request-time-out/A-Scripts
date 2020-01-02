// Decompiled with JetBrains decompiler
// Type: AnimatorMoveWatcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AnimatorMoveWatcher : MonoBehaviour
{
  [SerializeField]
  private Animator _animator;

  public AnimatorMoveWatcher()
  {
    base.\u002Ector();
  }

  private void OnAnimatorMove()
  {
    Transform transform1 = ((Component) this).get_transform();
    transform1.set_position(Vector3.op_Addition(transform1.get_position(), this._animator.get_deltaPosition()));
    Transform transform2 = ((Component) this).get_transform();
    transform2.set_rotation(Quaternion.op_Multiply(transform2.get_rotation(), this._animator.get_deltaRotation()));
  }

  private void Reset()
  {
    this._animator = (Animator) ((Component) this).GetComponent<Animator>();
  }
}
