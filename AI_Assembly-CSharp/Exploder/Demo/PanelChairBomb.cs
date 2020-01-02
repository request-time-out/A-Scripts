// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.PanelChairBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class PanelChairBomb : UseObject
  {
    public ExploderObject Exploder;
    public GameObject ChairBomb;
    public AudioSource SourceExplosion;
    public AudioClip ExplosionSound;
    public Light Flash;
    private int flashing;

    public override void Use()
    {
      base.Use();
      ((Component) this.Exploder).get_transform().set_position(this.ChairBomb.get_transform().get_position());
      this.Exploder.ExplodeSelf = false;
      this.Exploder.UseForceVector = false;
      this.Exploder.Radius = 10f;
      this.Exploder.TargetFragments = 300;
      this.Exploder.Force = 30f;
      this.Exploder.ExplodeRadius(new ExploderObject.OnExplosion(this.OnExplode));
    }

    private void OnExplode(float timeMS, ExploderObject.ExplosionState state)
    {
      if (state != ExploderObject.ExplosionState.ExplosionStarted)
        return;
      this.SourceExplosion.PlayOneShot(this.ExplosionSound);
      ((Component) this.Flash).get_gameObject().get_transform().set_position(this.ChairBomb.get_transform().get_position());
      Transform transform = ((Component) this.Flash).get_gameObject().get_transform();
      transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.get_up()));
      this.flashing = 10;
    }

    private void Update()
    {
      if (this.flashing > 0)
      {
        this.Flash.set_intensity(5f);
        --this.flashing;
      }
      else
        this.Flash.set_intensity(0.0f);
    }
  }
}
