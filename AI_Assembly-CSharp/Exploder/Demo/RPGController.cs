// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.RPGController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class RPGController : MonoBehaviour
  {
    public ExploderObject exploder;
    public ExploderMouseLook MouseLookCamera;
    public Rocket Rocket;
    private float nextShotTimeout;

    public RPGController()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.Rocket.HitCallback = new Rocket.OnHit(this.OnRocketHit);
    }

    public void OnActivate()
    {
      this.Rocket.OnActivate();
    }

    private void OnRocketHit(Vector3 position)
    {
      this.nextShotTimeout = 0.6f;
      ((Component) this.exploder).get_transform().set_position(position);
      this.exploder.ExplodeSelf = false;
      this.exploder.Force = 20f;
      this.exploder.TargetFragments = 100;
      this.exploder.Radius = 10f;
      this.exploder.UseForceVector = false;
      this.exploder.ExplodeRadius();
      this.Rocket.Reset();
    }

    private void Update()
    {
      TargetType targetType = TargetManager.Instance.TargetType;
      if (Input.GetMouseButtonDown(0) && (double) this.nextShotTimeout < 0.0 && (CursorLocking.IsLocked && targetType != TargetType.UseObject))
      {
        this.MouseLookCamera.Kick();
        Ray ray = this.MouseLookCamera.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Debug.DrawRay(((Ray) ref ray).get_origin(), Vector3.op_Multiply(((Ray) ref ray).get_direction(), 10f), Color.get_red(), 0.0f);
        this.Rocket.Launch(ray);
        this.nextShotTimeout = float.MaxValue;
      }
      this.nextShotTimeout -= Time.get_deltaTime();
    }
  }
}
