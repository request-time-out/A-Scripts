// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXMouseOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EpicToonFX
{
  public class ETFXMouseOrbit : MonoBehaviour
  {
    public Transform target;
    public float distance;
    public float xSpeed;
    public float ySpeed;
    public float yMinLimit;
    public float yMaxLimit;
    public float distanceMin;
    public float distanceMax;
    public float smoothTime;
    private float rotationYAxis;
    private float rotationXAxis;
    private float velocityX;
    private float velocityY;

    public ETFXMouseOrbit()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Vector3 eulerAngles = ((Component) this).get_transform().get_eulerAngles();
      this.rotationYAxis = (float) eulerAngles.y;
      this.rotationXAxis = (float) eulerAngles.x;
      if (!Object.op_Implicit((Object) ((Component) this).GetComponent<Rigidbody>()))
        return;
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_freezeRotation(true);
    }

    private void LateUpdate()
    {
      if (!Object.op_Implicit((Object) this.target))
        return;
      if (Input.GetMouseButton(1))
      {
        this.velocityX += (float) ((double) this.xSpeed * (double) Input.GetAxis("Mouse X") * (double) this.distance * 0.0199999995529652);
        this.velocityY += (float) ((double) this.ySpeed * (double) Input.GetAxis("Mouse Y") * 0.0199999995529652);
      }
      this.rotationYAxis += this.velocityX;
      this.rotationXAxis -= this.velocityY;
      this.rotationXAxis = ETFXMouseOrbit.ClampAngle(this.rotationXAxis, this.yMinLimit, this.yMaxLimit);
      Quaternion quaternion = Quaternion.Euler(this.rotationXAxis, this.rotationYAxis, 0.0f);
      this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMin, this.distanceMax);
      RaycastHit raycastHit;
      if (Physics.Linecast(this.target.get_position(), ((Component) this).get_transform().get_position(), ref raycastHit))
        this.distance -= ((RaycastHit) ref raycastHit).get_distance();
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(0.0f, 0.0f, -this.distance);
      Vector3 vector3_2 = Vector3.op_Addition(Quaternion.op_Multiply(quaternion, vector3_1), this.target.get_position());
      ((Component) this).get_transform().set_rotation(quaternion);
      ((Component) this).get_transform().set_position(vector3_2);
      this.velocityX = Mathf.Lerp(this.velocityX, 0.0f, Time.get_deltaTime() * this.smoothTime);
      this.velocityY = Mathf.Lerp(this.velocityY, 0.0f, Time.get_deltaTime() * this.smoothTime);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }
  }
}
