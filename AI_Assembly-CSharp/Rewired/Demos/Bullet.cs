// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.Bullet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class Bullet : MonoBehaviour
  {
    public float lifeTime;
    private bool die;
    private float deathTime;

    public Bullet()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if ((double) this.lifeTime <= 0.0)
        return;
      this.deathTime = Time.get_time() + this.lifeTime;
      this.die = true;
    }

    private void Update()
    {
      if (!this.die || (double) Time.get_time() < (double) this.deathTime)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }
}
