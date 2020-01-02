// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.GrenadeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class GrenadeObject : MonoBehaviour
  {
    public AudioClip ExplosionSound;
    public AudioSource SourceExplosion;
    public Light Flash;
    public bool ExplodeFinished;
    public bool Impact;
    private bool throwing;
    private float explodeTimeoutMax;
    private bool explosionInProgress;
    public ExploderObject exploder;
    private int flashing;

    public GrenadeObject()
    {
      base.\u002Ector();
    }

    public void Throw()
    {
      this.Impact = false;
      this.throwing = true;
      this.explodeTimeoutMax = 5f;
      this.ExplodeFinished = false;
      this.flashing = -1;
    }

    public void Explode()
    {
      if (this.explosionInProgress)
        return;
      this.explosionInProgress = true;
      this.throwing = false;
      if (!this.Impact)
      {
        this.explodeTimeoutMax = 5f;
      }
      else
      {
        ((Component) this.exploder).get_transform().set_position(((Component) this).get_transform().get_position());
        this.exploder.ExplodeSelf = false;
        this.exploder.UseForceVector = false;
        this.exploder.Radius = 5f;
        this.exploder.TargetFragments = 200;
        this.exploder.Force = 20f;
        this.exploder.ExplodeRadius(new ExploderObject.OnExplosion(this.OnExplode));
        this.ExplodeFinished = false;
      }
    }

    private void OnExplode(float timeMS, ExploderObject.ExplosionState state)
    {
      if (state == ExploderObject.ExplosionState.ExplosionStarted)
      {
        ExploderUtils.SetVisible(((Component) this).get_gameObject(), false);
        this.SourceExplosion.PlayOneShot(this.ExplosionSound);
        ((Component) this.Flash).get_gameObject().get_transform().set_position(((Component) this).get_gameObject().get_transform().get_position());
        Transform transform = ((Component) this.Flash).get_gameObject().get_transform();
        transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.get_up()));
        this.flashing = 10;
      }
      if (state != ExploderObject.ExplosionState.ExplosionFinished)
        return;
      this.ExplodeFinished = true;
      this.explosionInProgress = false;
    }

    private void OnCollisionEnter(Collision other)
    {
      this.Impact = true;
      if (this.throwing || this.ExplodeFinished)
        return;
      this.Explode();
    }

    private void Update()
    {
      if (this.flashing >= 0)
      {
        if (this.flashing > 0)
        {
          this.Flash.set_intensity(5f);
          --this.flashing;
        }
        else
        {
          this.Flash.set_intensity(0.0f);
          this.flashing = -1;
        }
      }
      this.explodeTimeoutMax -= Time.get_deltaTime();
      if (this.ExplodeFinished || (double) this.explodeTimeoutMax >= 0.0)
        return;
      this.Impact = true;
      this.Explode();
    }
  }
}
