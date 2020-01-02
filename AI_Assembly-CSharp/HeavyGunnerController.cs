// Decompiled with JetBrains decompiler
// Type: HeavyGunnerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using SpriteToParticlesAsset;
using System.Collections.Generic;
using UnityEngine;

public class HeavyGunnerController : MonoBehaviour
{
  public List<SpriteToParticles> ShadowFxs;
  public List<SpriteToParticles> WeirdFxs;
  public SpriteToParticles GunPrep;
  public float Speed;
  public GameObject LookAtAim;
  public float RotationVelocity;
  private float wantedRotation;
  public float angleDisplacement;
  public Rigidbody2D rig;
  private Animator animator;
  private float ShootPrepTime;
  public GameObject BulletPrefab;
  public Transform BulletStartPos;
  public float bulletSpeed;
  public float bulletRotationSpeed;

  public HeavyGunnerController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.rig = (Rigidbody2D) ((Component) this).GetComponent<Rigidbody2D>();
    this.animator = (Animator) ((Component) this).GetComponent<Animator>();
  }

  private void Update()
  {
    float axis1 = Input.GetAxis("Vertical");
    float axis2 = Input.GetAxis("Horizontal");
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(axis2, axis1);
    if ((double) ((Vector2) ref vector2).get_magnitude() > 1.0)
      ((Vector2) ref vector2).Normalize();
    this.rig.set_velocity(Vector2.op_Implicit(new Vector3((float) vector2.x * this.Speed, (float) vector2.y * this.Speed, 0.0f)));
    Animator animator = this.animator;
    Vector2 velocity = this.rig.get_velocity();
    double magnitude = (double) ((Vector2) ref velocity).get_magnitude();
    animator.SetFloat("Speed", (float) magnitude);
    this.wantedRotation = 57.29578f * Mathf.Atan2((float) (this.LookAtAim.get_transform().get_position().y - ((Component) this).get_transform().get_position().y), (float) (this.LookAtAim.get_transform().get_position().x - ((Component) this).get_transform().get_position().x)) + this.angleDisplacement;
    ((Component) this).get_transform().set_rotation(Quaternion.RotateTowards(((Component) this).get_transform().get_rotation(), Quaternion.Euler(0.0f, 0.0f, this.wantedRotation), this.RotationVelocity * Time.get_deltaTime()));
    if (Input.GetMouseButton(0))
      this.ShootPrep();
    if (Input.GetMouseButtonUp(0))
      this.Shoot();
    if (Input.GetKeyDown((KeyCode) 122))
      this.ShadowFXToggle();
    if (!Input.GetKeyDown((KeyCode) 120))
      return;
    this.WeirdFXToggle();
  }

  public void ShootPrep()
  {
    this.ShootPrepTime += Time.get_deltaTime();
    if ((double) this.ShootPrepTime <= 0.100000001490116)
      return;
    if (!this.GunPrep.IsPlaying())
      this.GunPrep.Play();
    this.GunPrep.EmissionRate = this.ShootPrepTime * 1000f;
    if ((double) this.GunPrep.EmissionRate <= 10000.0)
      return;
    this.GunPrep.EmissionRate = 10000f;
  }

  public void Shoot()
  {
    this.animator.SetTrigger(nameof (Shoot));
    this.ShootPrepTime = 0.0f;
    this.GunPrep.Stop();
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.BulletPrefab);
    gameObject.get_transform().set_position(this.BulletStartPos.get_position());
    ((Rigidbody2D) gameObject.GetComponent<Rigidbody2D>()).AddForce(Vector2.op_Implicit(Vector3.op_Multiply(((Component) this).get_transform().get_up(), this.bulletSpeed)));
    ((Rigidbody2D) gameObject.GetComponent<Rigidbody2D>()).AddTorque(this.bulletRotationSpeed);
    Object.Destroy((Object) gameObject, 10f);
  }

  public void ShadowFXToggle()
  {
    if (this.ShadowFxs[0].IsPlaying())
    {
      foreach (SpriteToParticles spriteToParticles in this.ShadowFxs)
        spriteToParticles.Stop();
    }
    else
    {
      foreach (SpriteToParticles spriteToParticles in this.ShadowFxs)
        spriteToParticles.Play();
    }
  }

  public void WeirdFXToggle()
  {
    if (this.WeirdFxs[0].IsPlaying())
    {
      foreach (SpriteToParticles spriteToParticles in this.WeirdFxs)
        spriteToParticles.Stop();
    }
    else
    {
      foreach (SpriteToParticles spriteToParticles in this.WeirdFxs)
        spriteToParticles.Play();
    }
  }
}
