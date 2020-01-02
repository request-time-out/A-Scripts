// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllersTiltDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class CustomControllersTiltDemo : MonoBehaviour
  {
    public Transform target;
    public float speed;
    private CustomController controller;
    private Player player;

    public CustomControllersTiltDemo()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      Screen.set_orientation((ScreenOrientation) 3);
      this.player = ReInput.get_players().GetPlayer(0);
      ReInput.add_InputSourceUpdateEvent(new Action(this.OnInputUpdate));
      this.controller = (CustomController) ((Player.ControllerHelper) this.player.controllers).GetControllerWithTag((ControllerType) 20, "TiltController");
    }

    private void Update()
    {
      if (Object.op_Equality((Object) this.target, (Object) null))
        return;
      Vector3 vector3 = Vector3.get_zero();
      vector3.y = (__Null) (double) this.player.GetAxis("Tilt Vertical");
      vector3.x = (__Null) (double) this.player.GetAxis("Tilt Horizontal");
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() > 1.0)
        ((Vector3) ref vector3).Normalize();
      vector3 = Vector3.op_Multiply(vector3, Time.get_deltaTime());
      this.target.Translate(Vector3.op_Multiply(vector3, this.speed));
    }

    private void OnInputUpdate()
    {
      Vector3 acceleration = Input.get_acceleration();
      this.controller.SetAxisValue(0, (float) acceleration.x);
      this.controller.SetAxisValue(1, (float) acceleration.y);
      this.controller.SetAxisValue(2, (float) acceleration.z);
    }
  }
}
