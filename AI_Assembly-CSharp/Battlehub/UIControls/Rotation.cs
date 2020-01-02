// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.Rotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  public class Rotation : MonoBehaviour
  {
    private Vector3 m_rand;
    private float m_prevT;

    public Rotation()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.m_rand = Random.get_onUnitSphere();
    }

    private void Update()
    {
      if ((double) Time.get_time() - (double) this.m_prevT > 10.0)
      {
        this.m_rand = Random.get_onUnitSphere();
        this.m_prevT = Time.get_time();
      }
      Transform transform = ((Component) this).get_transform();
      transform.set_rotation(Quaternion.op_Multiply(transform.get_rotation(), Quaternion.AngleAxis(12.56637f * Time.get_deltaTime(), this.m_rand)));
    }
  }
}
