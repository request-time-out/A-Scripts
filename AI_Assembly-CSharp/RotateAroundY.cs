// Decompiled with JetBrains decompiler
// Type: RotateAroundY
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
  public float rotateSpeed;

  public RotateAroundY()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    ((Component) this).get_transform().Rotate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), Time.get_deltaTime()), this.rotateSpeed));
  }
}
