// Decompiled with JetBrains decompiler
// Type: SinMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class SinMovement : MonoBehaviour
{
  public float speed;
  public float magnitude;
  private Vector3 startPosition;

  public SinMovement()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.startPosition = ((Component) this).get_transform().get_position();
  }

  private void FixedUpdate()
  {
    ((Component) this).get_transform().set_position(Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), Mathf.Sin(Time.get_time() * this.speed)), this.magnitude), this.startPosition));
  }
}
