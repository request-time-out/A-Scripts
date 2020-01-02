// Decompiled with JetBrains decompiler
// Type: ME_MouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class ME_MouseOrbit : MonoBehaviour
{
  public GameObject target;
  public float distance;
  public float xSpeed;
  public float ySpeed;
  public float yMinLimit;
  public float yMaxLimit;
  private float x;
  private float y;
  private float prevDistance;

  public ME_MouseOrbit()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
    this.x = (float) eulerAngles.y;
    this.y = (float) eulerAngles.x;
  }

  private void LateUpdate()
  {
    if ((double) this.distance < 2.0)
      this.distance = 2f;
    this.distance -= Input.GetAxis("Mouse ScrollWheel") * 2f;
    if (Object.op_Implicit((Object) this.target) && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
    {
      Vector3 mousePosition = Input.get_mousePosition();
      float num1 = 1f;
      if ((double) Screen.get_dpi() < 1.0)
        num1 = 1f;
      float num2 = (double) Screen.get_dpi() >= 200.0 ? Screen.get_dpi() / 200f : 1f;
      if (mousePosition.x < 380.0 * (double) num2 && (double) Screen.get_height() - mousePosition.y < 250.0 * (double) num2)
        return;
      Cursor.set_visible(false);
      Cursor.set_lockState((CursorLockMode) 1);
      this.x += (float) ((double) Input.GetAxis("Mouse X") * (double) this.xSpeed * 0.0199999995529652);
      this.y -= (float) ((double) Input.GetAxis("Mouse Y") * (double) this.ySpeed * 0.0199999995529652);
      this.y = ME_MouseOrbit.ClampAngle(this.y, this.yMinLimit, this.yMaxLimit);
      Quaternion quaternion = Quaternion.Euler(this.y, this.x, 0.0f);
      Vector3 vector3 = Vector3.op_Addition(Quaternion.op_Multiply(quaternion, new Vector3(0.0f, 0.0f, -this.distance)), this.target.get_transform().get_position());
      ((Component) this).get_transform().set_rotation(quaternion);
      ((Component) this).get_transform().set_position(vector3);
    }
    else
    {
      Cursor.set_visible(true);
      Cursor.set_lockState((CursorLockMode) 0);
    }
    if ((double) Math.Abs(this.prevDistance - this.distance) <= 1.0 / 1000.0)
      return;
    this.prevDistance = this.distance;
    Quaternion quaternion1 = Quaternion.Euler(this.y, this.x, 0.0f);
    Vector3 vector3_1 = Vector3.op_Addition(Quaternion.op_Multiply(quaternion1, new Vector3(0.0f, 0.0f, -this.distance)), this.target.get_transform().get_position());
    ((Component) this).get_transform().set_rotation(quaternion1);
    ((Component) this).get_transform().set_position(vector3_1);
  }

  private static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }
}
