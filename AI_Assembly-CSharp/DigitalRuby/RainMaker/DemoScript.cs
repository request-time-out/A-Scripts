// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.DemoScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.RainMaker
{
  public class DemoScript : MonoBehaviour
  {
    public RainScript RainScript;
    public Toggle MouseLookToggle;
    public Toggle FlashlightToggle;
    public Slider RainSlider;
    public Light Flashlight;
    public GameObject Sun;
    private DemoScript.RotationAxes axes;
    private float sensitivityX;
    private float sensitivityY;
    private float minimumX;
    private float maximumX;
    private float minimumY;
    private float maximumY;
    private float rotationX;
    private float rotationY;
    private Quaternion originalRotation;

    public DemoScript()
    {
      base.\u002Ector();
    }

    private void UpdateRain()
    {
      if (!Object.op_Inequality((Object) this.RainScript, (Object) null))
        return;
      if (Input.GetKeyDown((KeyCode) 49))
        this.RainScript.RainIntensity = 0.0f;
      else if (Input.GetKeyDown((KeyCode) 50))
        this.RainScript.RainIntensity = 0.2f;
      else if (Input.GetKeyDown((KeyCode) 51))
      {
        this.RainScript.RainIntensity = 0.5f;
      }
      else
      {
        if (!Input.GetKeyDown((KeyCode) 52))
          return;
        this.RainScript.RainIntensity = 0.8f;
      }
    }

    private void UpdateMovement()
    {
      float num = 5f * Time.get_deltaTime();
      if (Input.GetKey((KeyCode) 119))
        ((Component) Camera.get_main()).get_transform().Translate(0.0f, 0.0f, num);
      else if (Input.GetKey((KeyCode) 115))
        ((Component) Camera.get_main()).get_transform().Translate(0.0f, 0.0f, -num);
      if (Input.GetKey((KeyCode) 97))
        ((Component) Camera.get_main()).get_transform().Translate(-num, 0.0f, 0.0f);
      else if (Input.GetKey((KeyCode) 100))
        ((Component) Camera.get_main()).get_transform().Translate(num, 0.0f, 0.0f);
      if (!Input.GetKeyDown((KeyCode) 102))
        return;
      this.FlashlightToggle.set_isOn(!this.FlashlightToggle.get_isOn());
    }

    private void UpdateMouseLook()
    {
      if (Input.GetKeyDown((KeyCode) 32) || Input.GetKeyDown((KeyCode) 109))
        this.MouseLookToggle.set_isOn(!this.MouseLookToggle.get_isOn());
      if (!this.MouseLookToggle.get_isOn())
        return;
      if (this.axes == DemoScript.RotationAxes.MouseXAndY)
      {
        this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationX = DemoScript.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
        this.rotationY = DemoScript.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(this.rotationX, Vector3.get_up())), Quaternion.AngleAxis(this.rotationY, Vector3.op_UnaryNegation(Vector3.get_right()))));
      }
      else if (this.axes == DemoScript.RotationAxes.MouseX)
      {
        this.rotationX += Input.GetAxis("Mouse X") * this.sensitivityX;
        this.rotationX = DemoScript.ClampAngle(this.rotationX, this.minimumX, this.maximumX);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(this.rotationX, Vector3.get_up())));
      }
      else
      {
        this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
        this.rotationY = DemoScript.ClampAngle(this.rotationY, this.minimumY, this.maximumY);
        ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(this.originalRotation, Quaternion.AngleAxis(-this.rotationY, Vector3.get_right())));
      }
    }

    public void RainSliderChanged(float val)
    {
      this.RainScript.RainIntensity = val;
    }

    public void MouseLookChanged(bool val)
    {
      this.MouseLookToggle.set_isOn(val);
    }

    public void FlashlightChanged(bool val)
    {
      this.FlashlightToggle.set_isOn(val);
      ((Behaviour) this.Flashlight).set_enabled(val);
    }

    public void DawnDuskSliderChanged(float val)
    {
      this.Sun.get_transform().set_rotation(Quaternion.Euler(val, 0.0f, 0.0f));
    }

    public void FollowCameraChanged(bool val)
    {
      this.RainScript.FollowCamera = val;
    }

    private void Start()
    {
      this.originalRotation = ((Component) this).get_transform().get_localRotation();
      RainScript rainScript = this.RainScript;
      float num1 = 0.5f;
      this.RainSlider.set_value(num1);
      double num2 = (double) num1;
      rainScript.RainIntensity = (float) num2;
      this.RainScript.EnableWind = true;
    }

    private void Update()
    {
      this.UpdateRain();
      this.UpdateMovement();
      this.UpdateMouseLook();
    }

    public static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }

    private enum RotationAxes
    {
      MouseXAndY,
      MouseX,
      MouseY,
    }
  }
}
