// Decompiled with JetBrains decompiler
// Type: Oscillator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class Oscillator : MonoBehaviour
{
  public float m_Amplitude;
  public float m_Period;
  public Vector3 m_Direction;
  private Vector3 m_StartPosition;

  public Oscillator()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.m_StartPosition = ((Component) this).get_transform().get_position();
  }

  private void Update()
  {
    ((Component) this).get_transform().set_position(Vector3.op_Addition(this.m_StartPosition, Vector3.op_Multiply(Vector3.op_Multiply(this.m_Direction, this.m_Amplitude), Mathf.Sin(6.283185f * Time.get_time() / this.m_Period))));
  }
}
