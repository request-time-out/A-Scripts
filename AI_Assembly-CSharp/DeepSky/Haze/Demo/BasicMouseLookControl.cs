// Decompiled with JetBrains decompiler
// Type: DeepSky.Haze.Demo.BasicMouseLookControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DeepSky.Haze.Demo
{
  public class BasicMouseLookControl : MonoBehaviour
  {
    [SerializeField]
    private float m_XSensitivity;
    [SerializeField]
    private float m_YSensitivity;
    private Quaternion m_StartRotation;
    private float m_X;
    private float m_Y;

    public BasicMouseLookControl()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.m_StartRotation = ((Component) this).get_transform().get_localRotation();
    }

    private void Update()
    {
      this.m_X += Input.GetAxis("Mouse X") * this.m_XSensitivity;
      this.m_Y += Input.GetAxis("Mouse Y") * this.m_YSensitivity;
      if ((double) this.m_X > 360.0)
        this.m_X = 0.0f;
      else if ((double) this.m_X < 0.0)
        this.m_X = 360f;
      if ((double) this.m_Y > 60.0)
        this.m_Y = 60f;
      else if ((double) this.m_Y < -60.0)
        this.m_Y = -60f;
      ((Component) this).get_transform().set_localRotation(Quaternion.op_Multiply(Quaternion.op_Multiply(this.m_StartRotation, Quaternion.AngleAxis(this.m_X, Vector3.get_up())), Quaternion.AngleAxis(this.m_Y, Vector3.get_left())));
      if (!Input.GetKeyUp((KeyCode) 27))
        return;
      Application.Quit();
    }
  }
}
