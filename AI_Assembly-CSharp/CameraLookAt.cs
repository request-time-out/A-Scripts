// Decompiled with JetBrains decompiler
// Type: CameraLookAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
  public GameObject target;
  public float radius;
  public float vel;

  public CameraLookAt()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
    ((Component) this).get_transform().set_position(new Vector3(this.radius * Mathf.Cos(Time.get_time() * this.vel), this.radius * Mathf.Sin(Time.get_time() * this.vel), -10f));
    if (!Object.op_Implicit((Object) ((Component) this).get_transform()) || !Object.op_Inequality((Object) this.target, (Object) null))
      return;
    ((Component) this).get_transform().LookAt(this.target.get_transform());
  }
}
