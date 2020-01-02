// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.Rocket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class Rocket : MonoBehaviour
  {
    public AudioClip GunShot;
    public AudioClip Explosion;
    public AudioSource Source;
    public GameObject RocketStatic;
    public Light RocketLight;
    public float RocketVelocity;
    public Rocket.OnHit HitCallback;
    private Ray direction;
    private bool launch;
    private float launchTimeout;
    private Transform parent;
    private Vector3 shotPos;
    private float targetDistance;

    public Rocket()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.parent = ((Component) this).get_transform().get_parent();
      this.launchTimeout = float.MaxValue;
      ExploderUtils.SetActive(this.RocketStatic.get_gameObject(), false);
    }

    public void OnActivate()
    {
      ExploderUtils.SetActive(this.RocketStatic.get_gameObject(), true);
      if (!Object.op_Implicit((Object) this.parent))
        return;
      ExploderUtils.SetVisible(((Component) this).get_gameObject(), false);
    }

    public void Reset()
    {
      ExploderUtils.SetActive(this.RocketStatic.get_gameObject(), true);
    }

    public void Launch(Ray ray)
    {
      this.direction = ray;
      this.Source.PlayOneShot(this.GunShot);
      this.launchTimeout = 0.3f;
      this.launch = false;
      ExploderUtils.SetActive(this.RocketStatic.get_gameObject(), false);
      ExploderUtils.SetVisible(((Component) this).get_gameObject(), true);
      ((Component) this).get_gameObject().get_transform().set_parent(this.parent);
      ((Component) this).get_gameObject().get_transform().set_localPosition(this.RocketStatic.get_gameObject().get_transform().get_localPosition());
      ((Component) this).get_gameObject().get_transform().set_localRotation(this.RocketStatic.get_gameObject().get_transform().get_localRotation());
      ((Component) this).get_gameObject().get_transform().set_localScale(this.RocketStatic.get_gameObject().get_transform().get_localScale());
    }

    private void Update()
    {
      if ((double) this.launchTimeout < 0.0)
      {
        if (!this.launch)
        {
          this.launch = true;
          ((Component) this).get_transform().set_parent((Transform) null);
          this.RocketLight.set_intensity(2f);
          ((Ray) ref this.direction).set_origin(Vector3.op_Addition(((Ray) ref this.direction).get_origin(), Vector3.op_Multiply(((Ray) ref this.direction).get_direction(), 2f)));
          RaycastHit raycastHit;
          if (Physics.Raycast(this.direction, ref raycastHit, float.PositiveInfinity))
          {
            this.shotPos = ((Component) this).get_gameObject().get_transform().get_position();
            Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_gameObject().get_transform().get_position(), ((RaycastHit) ref raycastHit).get_point());
            this.targetDistance = ((Vector3) ref vector3).get_sqrMagnitude();
          }
          else
            this.targetDistance = 10000f;
        }
        Transform transform = ((Component) this).get_gameObject().get_transform();
        transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(((Ray) ref this.direction).get_direction(), this.RocketVelocity), Time.get_timeScale())));
        ((Component) this.RocketLight).get_transform().set_position(((Component) this).get_gameObject().get_transform().get_position());
        Vector3 vector3_1 = Vector3.op_Subtraction(this.shotPos, ((Component) this).get_gameObject().get_transform().get_position());
        if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() > (double) this.targetDistance)
        {
          this.Source.PlayOneShot(this.Explosion);
          this.HitCallback(((Component) this).get_gameObject().get_transform().get_position());
          this.launchTimeout = float.MaxValue;
          this.launch = false;
          ExploderUtils.SetVisible(((Component) this).get_gameObject(), false);
          this.RocketLight.set_intensity(0.0f);
        }
      }
      this.launchTimeout -= Time.get_deltaTime();
      if (!Input.GetKeyDown((KeyCode) 104))
        return;
      this.HitCallback(((Component) this).get_gameObject().get_transform().get_position());
    }

    public delegate void OnHit(Vector3 pos);
  }
}
