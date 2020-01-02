// Decompiled with JetBrains decompiler
// Type: RotateAround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
  public float Speed;
  public float LifeTime;
  public float TimeDelay;
  public float SpeedFadeInTime;
  public bool UseCollision;
  public EffectSettings EffectSettings;
  private bool canUpdate;
  private float currentSpeedFadeIn;
  private float allTime;

  public RotateAround()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (this.UseCollision)
      this.EffectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.EffectSettings_CollisionEnter);
    if ((double) this.TimeDelay > 0.0)
      this.Invoke("ChangeUpdate", this.TimeDelay);
    else
      this.canUpdate = true;
  }

  private void OnEnable()
  {
    this.canUpdate = true;
    this.allTime = 0.0f;
  }

  private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    this.canUpdate = false;
  }

  private void ChangeUpdate()
  {
    this.canUpdate = true;
  }

  private void Update()
  {
    if (!this.canUpdate)
      return;
    this.allTime += Time.get_deltaTime();
    if ((double) this.allTime >= (double) this.LifeTime && (double) this.LifeTime > 9.99999974737875E-05)
      return;
    if ((double) this.SpeedFadeInTime > 1.0 / 1000.0)
    {
      if ((double) this.currentSpeedFadeIn < (double) this.Speed)
        this.currentSpeedFadeIn += Time.get_deltaTime() / this.SpeedFadeInTime * this.Speed;
      else
        this.currentSpeedFadeIn = this.Speed;
    }
    else
      this.currentSpeedFadeIn = this.Speed;
    ((Component) this).get_transform().Rotate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_forward(), Time.get_deltaTime()), this.currentSpeedFadeIn));
  }
}
