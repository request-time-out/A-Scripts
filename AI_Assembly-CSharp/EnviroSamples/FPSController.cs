// Decompiled with JetBrains decompiler
// Type: EnviroSamples.FPSController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EnviroSamples
{
  public class FPSController : MonoBehaviour
  {
    public float speed;
    public float sensitivity;
    private CharacterController player;
    public GameObject eyes;
    private float moveFB;
    private float moveLR;
    private float rotX;
    private float rotY;

    public FPSController()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.player = (CharacterController) ((Component) this).GetComponent<CharacterController>();
    }

    private void Update()
    {
      this.moveFB = Input.GetAxis("Vertical") * this.speed;
      this.moveLR = Input.GetAxis("Horizontal") * this.speed;
      this.rotX = Input.GetAxis("Mouse X") * this.sensitivity;
      this.rotY -= Input.GetAxis("Mouse Y") * this.sensitivity;
      this.rotY = Mathf.Clamp(this.rotY, -60f, 60f);
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(this.moveLR, 0.0f, this.moveFB);
      ((Component) this).get_transform().Rotate(0.0f, this.rotX, 0.0f);
      this.eyes.get_transform().set_localRotation(Quaternion.Euler(this.rotY, 0.0f, 0.0f));
      Vector3 vector3_2 = Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), vector3_1);
      ref Vector3 local = ref vector3_2;
      local.y = (__Null) (local.y - 4000.0 * (double) Time.get_deltaTime());
      this.player.Move(Vector3.op_Multiply(vector3_2, Time.get_deltaTime()));
    }
  }
}
