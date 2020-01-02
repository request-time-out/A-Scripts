// Decompiled with JetBrains decompiler
// Type: CTS.CTSFly
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CTS
{
  public class CTSFly : MonoBehaviour
  {
    public float cameraSensitivity;
    public float climbSpeed;
    public float normalMoveSpeed;
    public float fastMoveFactor;

    public CTSFly()
    {
      base.\u002Ector();
    }

    private void Start()
    {
    }

    private void Update()
    {
      if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
      {
        Transform transform1 = ((Component) this).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.normalMoveSpeed * this.fastMoveFactor), Input.GetAxis("Vertical")), Time.get_deltaTime())));
        Transform transform2 = ((Component) this).get_transform();
        transform2.set_position(Vector3.op_Addition(transform2.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.normalMoveSpeed * this.fastMoveFactor), Input.GetAxis("Horizontal")), Time.get_deltaTime())));
      }
      else
      {
        Transform transform1 = ((Component) this).get_transform();
        transform1.set_position(Vector3.op_Addition(transform1.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.normalMoveSpeed), Input.GetAxis("Vertical")), Time.get_deltaTime())));
        Transform transform2 = ((Component) this).get_transform();
        transform2.set_position(Vector3.op_Addition(transform2.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_right(), this.normalMoveSpeed), Input.GetAxis("Horizontal")), Time.get_deltaTime())));
      }
      if (Input.GetKey((KeyCode) 101))
      {
        if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
        {
          Transform transform = ((Component) this).get_transform();
          transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), this.fastMoveFactor), Time.get_deltaTime())));
        }
        else
        {
          Transform transform = ((Component) this).get_transform();
          transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), Time.get_deltaTime())));
        }
      }
      if (!Input.GetKey((KeyCode) 113))
        return;
      if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
      {
        Transform transform = ((Component) this).get_transform();
        transform.set_position(Vector3.op_Subtraction(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), this.fastMoveFactor), Time.get_deltaTime())));
      }
      else
      {
        Transform transform = ((Component) this).get_transform();
        transform.set_position(Vector3.op_Subtraction(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.climbSpeed), Time.get_deltaTime())));
      }
    }
  }
}
