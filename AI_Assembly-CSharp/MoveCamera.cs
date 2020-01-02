// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MoveCamera : MonoBehaviour
{
  public float turnSpeed;
  public float panSpeed;
  public float zoomSpeed;
  private Vector3 mouseOrigin;
  private bool isPanning;
  private bool isRotating;
  private bool isZooming;

  public MoveCamera()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      this.mouseOrigin = Input.get_mousePosition();
      this.isRotating = true;
    }
    if (Input.GetMouseButtonDown(1))
    {
      this.mouseOrigin = Input.get_mousePosition();
      this.isPanning = true;
    }
    if (Input.GetMouseButtonDown(2))
    {
      this.mouseOrigin = Input.get_mousePosition();
      this.isZooming = true;
    }
    if (!Input.GetMouseButton(0))
      this.isRotating = false;
    if (!Input.GetMouseButton(1))
      this.isPanning = false;
    if (!Input.GetMouseButton(2))
      this.isZooming = false;
    if (this.isRotating)
    {
      Vector3 viewportPoint = Camera.get_main().ScreenToViewportPoint(Vector3.op_Subtraction(Input.get_mousePosition(), this.mouseOrigin));
      ((Component) this).get_transform().RotateAround(((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_right(), (float) -viewportPoint.y * this.turnSpeed);
      ((Component) this).get_transform().RotateAround(((Component) this).get_transform().get_position(), Vector3.get_up(), (float) viewportPoint.x * this.turnSpeed);
    }
    if (this.isPanning)
    {
      Vector3 viewportPoint = Camera.get_main().ScreenToViewportPoint(Vector3.op_Subtraction(Input.get_mousePosition(), this.mouseOrigin));
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) viewportPoint.x * this.panSpeed, (float) viewportPoint.y * this.panSpeed, 0.0f);
      ((Component) this).get_transform().Translate(vector3, (Space) 1);
    }
    if (!this.isZooming)
      return;
    ((Component) this).get_transform().Translate(Vector3.op_Multiply((float) Camera.get_main().ScreenToViewportPoint(Vector3.op_Subtraction(Input.get_mousePosition(), this.mouseOrigin)).y * this.zoomSpeed, ((Component) this).get_transform().get_forward()), (Space) 0);
  }
}
