// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.ThrowObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class ThrowObject : MonoBehaviour
  {
    private float destroyTimer;

    public ThrowObject()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.destroyTimer = 10f;
    }

    private void Update()
    {
      this.destroyTimer -= Time.get_deltaTime();
      if ((double) this.destroyTimer >= 0.0)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }
}
