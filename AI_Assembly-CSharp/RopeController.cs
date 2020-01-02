// Decompiled with JetBrains decompiler
// Type: RopeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using PicoGames.QuickRopes;
using UnityEngine;

[RequireComponent(typeof (QuickRope))]
public class RopeController : MonoBehaviour
{
  [Min(1f)]
  public int minJointCount;
  [Min(0.001f)]
  public float maxSpeed;
  [Range(0.0f, 1f)]
  public float acceleration;
  [Range(0.001f, 1f)]
  public float dampening;
  private QuickRope rope;

  public RopeController()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.rope = (QuickRope) ((Component) this).GetComponent<QuickRope>();
    if (this.rope.Spline.IsLooped)
    {
      ((Behaviour) this).set_enabled(false);
    }
    else
    {
      this.rope.minLinkCount = this.minJointCount;
      if (!this.rope.canResize)
      {
        this.rope.maxLinkCount = this.rope.Links.Length + 1;
        this.rope.canResize = true;
        this.rope.Generate();
      }
      this.rope.Links[this.rope.Links.Length - 1].Rigidbody.set_isKinematic(true);
    }
  }

  private void Update()
  {
    this.rope.velocityAccel = this.acceleration;
    this.rope.velocityDampen = this.dampening;
    if (Input.GetKey((KeyCode) 273))
      this.rope.Velocity = this.maxSpeed;
    else if (Input.GetKey((KeyCode) 274))
      this.rope.Velocity = -this.maxSpeed;
    else
      this.rope.Velocity = 0.0f;
  }
}
