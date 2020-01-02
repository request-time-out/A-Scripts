// Decompiled with JetBrains decompiler
// Type: LuxWater.Demo.LuxWater_ExtendedFlycam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater.Demo
{
  public class LuxWater_ExtendedFlycam : MonoBehaviour
  {
    public float cameraSensitivity;
    public float climbSpeed;
    public float normalMoveSpeed;
    public float slowMoveFactor;
    public float fastMoveFactor;
    private float rotationX;
    private float rotationY;
    private bool isOrtho;
    private Camera cam;

    public LuxWater_ExtendedFlycam()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.rotationX = (float) ((Component) this).get_transform().get_eulerAngles().y;
      this.cam = (Camera) ((Component) this).GetComponent<Camera>();
      if (!Object.op_Inequality((Object) this.cam, (Object) null))
        return;
      this.isOrtho = this.cam.get_orthographic();
    }

    private void Update()
    {
      float deltaTime = Time.get_deltaTime();
      this.rotationX += Input.GetAxis("Mouse X") * this.cameraSensitivity * deltaTime;
      this.rotationY += Input.GetAxis("Mouse Y") * this.cameraSensitivity * deltaTime;
      this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
      ((Component) this).get_transform().set_localRotation(Quaternion.Slerp(((Component) this).get_transform().get_localRotation(), Quaternion.op_Multiply(Quaternion.AngleAxis(this.rotationX, Vector3.get_up()), Quaternion.AngleAxis(this.rotationY, Vector3.get_left())), deltaTime * 6f));
      if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
      {
        Transform transform1 = ((Component) this).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.normalMoveSpeed * this.fastMoveFactor), Input.GetAxis("Vertical")), deltaTime)));
        Transform transform2 = ((Component) this).get_transform();
        transform2.set_position(Vector3.op_Addition(transform2.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.normalMoveSpeed * this.fastMoveFactor), Input.GetAxis("Horizontal")), deltaTime)));
      }
      else if (Input.GetKey((KeyCode) 306) || Input.GetKey((KeyCode) 305))
      {
        Transform transform1 = ((Component) this).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.normalMoveSpeed * this.slowMoveFactor), Input.GetAxis("Vertical")), deltaTime)));
        Transform transform2 = ((Component) this).get_transform();
        transform2.set_position(Vector3.op_Addition(transform2.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.normalMoveSpeed * this.slowMoveFactor), Input.GetAxis("Horizontal")), deltaTime)));
      }
      else
      {
        if (this.isOrtho)
        {
          Camera cam = this.cam;
          cam.set_orthographicSize(cam.get_orthographicSize() * (float) (1.0 - (double) Input.GetAxis("Vertical") * (double) deltaTime));
        }
        else
        {
          Transform transform = ((Component) this).get_transform();
          transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.normalMoveSpeed), Input.GetAxis("Vertical")), deltaTime)));
        }
        Transform transform1 = ((Component) this).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.normalMoveSpeed), Input.GetAxis("Horizontal")), deltaTime)));
      }
      if (Input.GetKey((KeyCode) 113))
      {
        Transform transform = ((Component) this).get_transform();
        transform.set_position(Vector3.op_Subtraction(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), deltaTime)));
      }
      if (!Input.GetKey((KeyCode) 101))
        return;
      Transform transform3 = ((Component) this).get_transform();
      transform3.set_position(Vector3.op_Addition(transform3.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), deltaTime)));
    }
  }
}
