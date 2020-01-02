// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.GrenadeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class GrenadeController : MonoBehaviour
  {
    public AudioClip Throw;
    public AudioSource Source;
    public GrenadeObject Grenade;
    public Camera MainCamera;
    private float explodeTimer;
    private float throwTimer;
    private bool exploding;

    public GrenadeController()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.throwTimer = float.MaxValue;
      this.explodeTimer = float.MaxValue;
      this.exploding = false;
    }

    private void Update()
    {
      if (Input.GetKeyDown((KeyCode) 103) && !this.exploding)
      {
        this.throwTimer = 0.4f;
        this.Source.PlayOneShot(this.Throw);
        this.explodeTimer = 2f;
        this.exploding = true;
        this.Grenade.Throw();
        ExploderUtils.SetVisible(((Component) this).get_gameObject(), false);
      }
      this.throwTimer -= Time.get_deltaTime();
      if ((double) this.throwTimer < 0.0)
      {
        this.throwTimer = float.MaxValue;
        ExploderUtils.SetVisible(((Component) this.Grenade).get_gameObject(), true);
        ExploderUtils.SetActive(((Component) this.Grenade).get_gameObject(), true);
        ((Component) this.Grenade).get_transform().set_position(((Component) this).get_gameObject().get_transform().get_position());
        ((Rigidbody) ((Component) this.Grenade).GetComponent<Rigidbody>()).set_velocity(Vector3.op_Multiply(((Component) this.MainCamera).get_transform().get_forward(), 20f));
      }
      this.explodeTimer -= Time.get_deltaTime();
      if ((double) this.explodeTimer < 0.0)
      {
        this.Grenade.Explode();
        this.explodeTimer = float.MaxValue;
      }
      if (!this.Grenade.ExplodeFinished)
        return;
      this.exploding = false;
      ExploderUtils.SetVisible(((Component) this).get_gameObject(), true);
    }
  }
}
