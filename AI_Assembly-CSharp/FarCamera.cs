// Decompiled with JetBrains decompiler
// Type: FarCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
public class FarCamera : MonoBehaviour
{
  public GameObject target;
  private Vector3 _relativePosition;

  public FarCamera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this._relativePosition = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this.target.get_transform().get_position());
  }

  private void FixedUpdate()
  {
    ((Component) this).get_transform().set_position(Vector3.op_Addition(this.target.get_transform().get_position(), this._relativePosition));
  }
}
