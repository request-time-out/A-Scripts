// Decompiled with JetBrains decompiler
// Type: MetaballJoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MetaballJoint : MonoBehaviour
{
  public Transform transformJoint;
  public float limitLength;
  [SerializeField]
  [Tooltip("確認用表示")]
  private float length;

  public MetaballJoint()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Implicit((Object) this.transformJoint))
      return;
    this.length = Vector3.Distance(this.transformJoint.get_position(), ((Component) this).get_transform().get_position());
  }

  private void Update()
  {
    Vector3 vector3 = Vector3.op_Subtraction(this.transformJoint.get_position(), ((Component) this).get_transform().get_position());
    float magnitude = ((Vector3) ref vector3).get_magnitude();
    float num = magnitude - this.length;
    if ((double) num > 0.0)
    {
      ((Component) this).get_transform().Translate(Vector3.op_Multiply(vector3, (num - this.limitLength) / magnitude), (Space) 0);
    }
    else
    {
      if ((double) num >= 0.0)
        return;
      ((Component) this).get_transform().Translate(Vector3.op_Multiply(vector3, (num + this.limitLength) / magnitude), (Space) 0);
    }
  }
}
