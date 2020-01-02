// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.ExploderMouseLook
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class ExploderMouseLook : MonoBehaviour
  {
    public ExploderMouseLook.RotationAxes axes;
    public float sensitivityX;
    public float sensitivityY;
    public float minimumX;
    public float maximumX;
    public float minimumY;
    public float maximumY;
    private float rotationY;
    private float kickTimeout;
    private float kickBackRot;
    private bool kickBack;
    private float rotationTarget;
    public Camera mainCamera;

    public ExploderMouseLook()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      if (!CursorLocking.IsLocked)
        return;
      float num1;
      if (this.axes == ExploderMouseLook.RotationAxes.MouseXAndY)
      {
        float num2 = (float) (((Component) this).get_transform().get_localEulerAngles().y + (double) Input.GetAxis("Mouse X") * (double) this.sensitivityX);
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
        num1 = num2;
        ((Component) this.mainCamera).get_transform().set_localEulerAngles(new Vector3(-this.rotationY, 0.0f, 0.0f));
        if ((double) this.kickTimeout > 0.0)
          this.rotationTarget += Input.GetAxis("Mouse Y") * this.sensitivityY;
      }
      else if (this.axes == ExploderMouseLook.RotationAxes.MouseX)
      {
        num1 = Input.GetAxis("Mouse X") * this.sensitivityX;
        ((Component) this.mainCamera).get_transform().Rotate(0.0f, 0.0f, 0.0f);
      }
      else
      {
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
        if ((double) this.kickTimeout > 0.0)
          this.rotationTarget += Input.GetAxis("Mouse Y") * this.sensitivityY;
        num1 = (float) ((Component) this).get_transform().get_localEulerAngles().y;
        ((Component) this.mainCamera).get_transform().set_localEulerAngles(new Vector3(-this.rotationY, 0.0f, 0.0f));
      }
      this.kickTimeout -= Time.get_deltaTime();
      if ((double) this.kickTimeout > 0.0)
        this.rotationY = Mathf.Lerp(this.rotationY, this.rotationTarget, Time.get_deltaTime() * 10f);
      else if (this.kickBack)
      {
        this.kickBack = false;
        this.kickTimeout = 0.5f;
        this.rotationTarget = this.kickBackRot;
      }
      ((Component) this).get_gameObject().get_transform().set_rotation(Quaternion.Euler(0.0f, num1, 0.0f));
    }

    private void Start()
    {
      if (!Object.op_Implicit((Object) ((Component) this).GetComponent<Rigidbody>()))
        return;
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_freezeRotation(true);
    }

    public void Kick()
    {
      this.kickTimeout = 0.1f;
      this.rotationTarget = this.rotationY + (float) Random.Range(15, 20);
      this.kickBackRot = this.rotationY;
      this.kickBack = true;
    }

    public enum RotationAxes
    {
      MouseXAndY,
      MouseX,
      MouseY,
    }
  }
}
