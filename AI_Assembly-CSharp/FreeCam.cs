// Decompiled with JetBrains decompiler
// Type: FreeCam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class FreeCam : MonoBehaviour
{
  public FreeCam.RotationAxes axes;
  public float sensitivityX;
  public float sensitivityY;
  public float minimumX;
  public float maximumX;
  public float minimumY;
  public float maximumY;
  public float moveSpeed;
  public bool lockHeight;
  private float rotationY;

  public FreeCam()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (this.axes == FreeCam.RotationAxes.MouseXAndY)
    {
      float num = (float) (((Component) this).get_transform().get_localEulerAngles().y + (double) Input.GetAxis("Mouse X") * (double) this.sensitivityX);
      this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
      this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
      ((Component) this).get_transform().set_localEulerAngles(new Vector3(-this.rotationY, num, 0.0f));
    }
    else if (this.axes == FreeCam.RotationAxes.MouseX)
    {
      ((Component) this).get_transform().Rotate(0.0f, Input.GetAxis("Mouse X") * this.sensitivityX, 0.0f);
    }
    else
    {
      this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
      this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
      ((Component) this).get_transform().set_localEulerAngles(new Vector3(-this.rotationY, (float) ((Component) this).get_transform().get_localEulerAngles().y, 0.0f));
    }
    float axis1 = Input.GetAxis("Horizontal");
    float axis2 = Input.GetAxis("Vertical");
    if (this.lockHeight)
    {
      Vector3 vector3 = ((Component) this).get_transform().TransformDirection(Vector3.op_Multiply(new Vector3(axis1, 0.0f, axis2), this.moveSpeed));
      vector3.y = (__Null) 0.0;
      Transform transform = ((Component) this).get_transform();
      transform.set_position(Vector3.op_Addition(transform.get_position(), vector3));
    }
    else
      ((Component) this).get_transform().Translate(Vector3.op_Multiply(new Vector3(axis1, 0.0f, axis2), this.moveSpeed));
  }

  public enum RotationAxes
  {
    MouseXAndY,
    MouseX,
    MouseY,
  }
}
