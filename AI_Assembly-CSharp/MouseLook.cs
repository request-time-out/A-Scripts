// Decompiled with JetBrains decompiler
// Type: MouseLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MouseLook : MonoBehaviour
{
  public MouseLook.RotationAxes axes;
  public float sensitivityX;
  public float sensitivityY;
  public float minimumX;
  public float maximumX;
  public float minimumY;
  public float maximumY;
  public float forwardSpeedScale;
  public float strafeSpeedScale;
  private float rotationX;
  private float rotationY;
  private bool look;
  private Quaternion originalRotation;

  public MouseLook()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    if (GUIUtility.get_hotControl() != 0)
      return;
    if (Input.GetMouseButtonDown(0))
      this.look = true;
    if (Input.GetMouseButtonUp(0))
      this.look = false;
    if (this.look)
    {
      if (this.axes == MouseLook.RotationAxes.MouseXAndY)
      {
        this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
        this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(this.rotationX, Vector3.get_up())), Quaternion.AngleAxis(this.rotationY, Vector3.get_left())));
      }
      else if (this.axes == MouseLook.RotationAxes.MouseX)
      {
        this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
        this.rotationX = MouseLook.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(this.rotationX, Vector3.get_up())));
      }
      else
      {
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationY = MouseLook.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(this.rotationY, Vector3.get_left())));
      }
    }
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
    Vector3.op_Multiply(((Component) this).get_transform().TransformDirection(vector3), 10f);
    float num1 = Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303) ? 150f : 50f;
    float num2 = Input.GetAxis("Vertical") * this.forwardSpeedScale * num1;
    float num3 = Input.GetAxis("Horizontal") * this.strafeSpeedScale * num1;
    if ((double) num2 != 0.0)
    {
      Transform transform = ((Component) this).get_transform();
      transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_forward(), num2)));
    }
    if ((double) num3 == 0.0)
      return;
    Transform transform1 = ((Component) this).get_transform();
    transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(((Component) this).get_transform().get_right(), num3)));
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) ((Component) this).GetComponent<Rigidbody>()))
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_freezeRotation(true);
    this.originalRotation = ((Component) this).get_transform().get_localRotation();
    this.look = false;
  }

  public static float ClampAngle(float angle, float min, float max)
  {
    if ((double) angle < -360.0)
      angle += 360f;
    if ((double) angle > 360.0)
      angle -= 360f;
    return Mathf.Clamp(angle, min, max);
  }

  public enum RotationAxes
  {
    MouseXAndY,
    MouseX,
    MouseY,
  }
}
