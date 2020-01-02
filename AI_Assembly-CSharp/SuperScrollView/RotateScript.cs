// Decompiled with JetBrains decompiler
// Type: SuperScrollView.RotateScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace SuperScrollView
{
  public class RotateScript : MonoBehaviour
  {
    public float speed;

    public RotateScript()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      Vector3 localEulerAngles = ((Component) this).get_gameObject().get_transform().get_localEulerAngles();
      localEulerAngles.z = (__Null) (localEulerAngles.z + (double) this.speed * (double) Time.get_deltaTime());
      ((Component) this).get_gameObject().get_transform().set_localEulerAngles(localEulerAngles);
    }
  }
}
