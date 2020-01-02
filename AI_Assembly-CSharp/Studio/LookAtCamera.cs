// Decompiled with JetBrains decompiler
// Type: Studio.LookAtCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class LookAtCamera : MonoBehaviour
  {
    private Transform target;

    public LookAtCamera()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.target = ((Component) Camera.get_main()).get_transform();
    }

    private void Update()
    {
      ((Component) this).get_transform().LookAt(this.target.get_position(), this.target.get_up());
    }
  }
}
