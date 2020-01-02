// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.ShotgunController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class ShotgunController : MonoBehaviour
  {
    public AudioClip GunShot;
    public AudioClip Reload;
    public AudioSource Source;
    public ExploderMouseLook MouseLookCamera;
    public Light Flash;
    public Animation ReloadAnim;
    public AnimationClip HideAnim;
    public GameObject MuzzleFlash;
    private int flashing;
    private float reloadTimeout;
    private float nextShotTimeout;
    private TargetType lastTarget;
    public ExploderObject exploder;

    public ShotgunController()
    {
      base.\u002Ector();
    }

    public void OnActivate()
    {
      ExploderUtils.SetActive(this.MuzzleFlash, false);
    }

    private void Update()
    {
      GameObject gameObject = (GameObject) null;
      TargetType targetType = TargetManager.Instance.TargetType;
      if (targetType == TargetType.UseObject)
      {
        if (this.lastTarget == TargetType.UseObject)
          ;
        this.lastTarget = TargetType.UseObject;
      }
      if (this.lastTarget != TargetType.UseObject)
        ;
      this.lastTarget = targetType;
      Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
      Debug.DrawRay(((Ray) ref ray).get_origin(), Vector3.op_Multiply(((Ray) ref ray).get_direction(), 10f), Color.get_red(), 0.0f);
      if (targetType == TargetType.DestroyableObject)
        gameObject = TargetManager.Instance.TargetObject;
      if (Input.GetMouseButtonDown(0) && (double) this.nextShotTimeout < 0.0 && CursorLocking.IsLocked)
      {
        if (targetType != TargetType.UseObject)
        {
          this.Source.PlayOneShot(this.GunShot);
          this.MouseLookCamera.Kick();
          this.reloadTimeout = 0.3f;
          this.flashing = 5;
        }
        if (Object.op_Implicit((Object) gameObject))
        {
          ((Component) this.exploder).get_transform().set_position(ExploderUtils.GetCentroid(gameObject));
          this.exploder.ExplodeSelf = false;
          ExploderObject exploder = this.exploder;
          Vector3 direction = ((Ray) ref ray).get_direction();
          Vector3 normalized = ((Vector3) ref direction).get_normalized();
          exploder.ForceVector = normalized;
          this.exploder.Force = 10f;
          this.exploder.UseForceVector = true;
          this.exploder.TargetFragments = 30;
          this.exploder.Radius = 1f;
          this.exploder.ExplodeRadius();
        }
        this.nextShotTimeout = 0.6f;
      }
      this.nextShotTimeout -= Time.get_deltaTime();
      if (this.flashing > 0)
      {
        this.Flash.set_intensity(1f);
        ExploderUtils.SetActive(this.MuzzleFlash, true);
        --this.flashing;
      }
      else
      {
        this.Flash.set_intensity(0.0f);
        ExploderUtils.SetActive(this.MuzzleFlash, false);
      }
      this.reloadTimeout -= Time.get_deltaTime();
      if ((double) this.reloadTimeout >= 0.0)
        return;
      this.reloadTimeout = float.MaxValue;
      this.Source.PlayOneShot(this.Reload);
      this.ReloadAnim.Play();
    }
  }
}
