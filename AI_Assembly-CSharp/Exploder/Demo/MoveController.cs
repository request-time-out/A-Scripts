// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.MoveController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class MoveController : MonoBehaviour
  {
    public float speed;
    public float jumpSpeed;
    public float gravity;
    private Vector3 moveDirection;
    private CharacterController controller;

    public MoveController()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.controller = (CharacterController) ((Component) this).GetComponent<CharacterController>();
    }

    private void Update()
    {
      if (this.controller.get_isGrounded())
      {
        this.moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        this.moveDirection = ((Component) this).get_transform().TransformDirection(this.moveDirection);
        MoveController moveController = this;
        moveController.moveDirection = Vector3.op_Multiply(moveController.moveDirection, this.speed);
        if (Input.GetButton("Jump"))
          this.moveDirection.y = (__Null) (double) this.jumpSpeed;
      }
      ref Vector3 local = ref this.moveDirection;
      local.y = (__Null) (local.y - (double) this.gravity * (double) Time.get_deltaTime());
      this.controller.Move(Vector3.op_Multiply(this.moveDirection, Time.get_deltaTime()));
    }
  }
}
