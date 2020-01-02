// Decompiled with JetBrains decompiler
// Type: AQUAS.AQUAS_Walk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AQUAS
{
  public class AQUAS_Walk : MonoBehaviour
  {
    public float m_moveSpeed;
    public CharacterController m_controller;

    public AQUAS_Walk()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (!Object.op_Equality((Object) this.m_controller, (Object) null))
        return;
      this.m_controller = (CharacterController) ((Component) this).GetComponent<CharacterController>();
    }

    private void Update()
    {
      if (!Object.op_Inequality((Object) this.m_controller, (Object) null) || !((Collider) this.m_controller).get_enabled())
        return;
      this.m_controller.Move(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Input.GetAxis("Vertical"), ((Component) this).get_transform().TransformDirection(Vector3.get_forward())), this.m_moveSpeed), Time.get_deltaTime()));
      this.m_controller.Move(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Input.GetAxis("Horizontal"), ((Component) this).get_transform().TransformDirection(Vector3.get_right())), this.m_moveSpeed), Time.get_deltaTime()));
      this.m_controller.SimpleMove(Physics.get_gravity());
    }
  }
}
