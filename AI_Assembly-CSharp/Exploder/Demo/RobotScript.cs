// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.RobotScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  internal class RobotScript : MonoBehaviour
  {
    public float radius;
    public float velocity;
    private float angle;
    private Vector3 center;
    private Vector3 lastPos;

    public RobotScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.center = ((Component) this).get_gameObject().get_transform().get_position();
    }

    private void Update()
    {
      ((Animation) ((Component) this).GetComponent<Animation>()).Play();
    }

    private void FixedUpdate()
    {
      Vector3 position = ((Component) this).get_gameObject().get_transform().get_position();
      position.x = (__Null) (this.center.x + (double) Mathf.Sin(this.angle) * (double) this.radius);
      position.z = (__Null) (this.center.z + (double) Mathf.Cos(this.angle) * (double) this.radius);
      ((Component) this).get_gameObject().get_transform().set_position(position);
      ((Component) this).get_gameObject().get_transform().set_forward(Vector3.op_Subtraction(position, this.lastPos));
      this.lastPos = position;
      this.angle += Time.get_deltaTime() * this.velocity;
    }
  }
}
