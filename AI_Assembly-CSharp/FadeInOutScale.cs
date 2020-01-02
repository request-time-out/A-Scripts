// Decompiled with JetBrains decompiler
// Type: FadeInOutScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class FadeInOutScale : MonoBehaviour
{
  public FadeInOutStatus FadeInOutStatus;
  public float Speed;
  public float MaxScale;
  private Vector3 oldScale;
  private float time;
  private float oldSin;
  private bool updateTime;
  private bool canUpdate;
  private Transform t;
  private EffectSettings effectSettings;
  private bool isInitialized;
  private bool isCollisionEnter;

  public FadeInOutScale()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.t = ((Component) this).get_transform();
    this.oldScale = this.t.get_localScale();
    this.isInitialized = true;
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (!Object.op_Inequality((Object) this.effectSettings, (Object) null))
      return;
    this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
  }

  private void GetEffectSettingsComponent(Transform tr)
  {
    Transform parent = tr.get_parent();
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    this.effectSettings = (EffectSettings) ((Component) parent).GetComponentInChildren<EffectSettings>();
    if (!Object.op_Equality((Object) this.effectSettings, (Object) null))
      return;
    this.GetEffectSettingsComponent(((Component) parent).get_transform());
  }

  public void InitDefaultVariables()
  {
    if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision)
    {
      this.t.set_localScale(this.oldScale);
      this.canUpdate = false;
    }
    else
    {
      this.t.set_localScale(Vector3.get_zero());
      this.canUpdate = true;
    }
    this.updateTime = true;
    this.time = 0.0f;
    this.oldSin = 0.0f;
    this.isCollisionEnter = false;
  }

  private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    this.isCollisionEnter = true;
    this.canUpdate = true;
  }

  private void OnEnable()
  {
    if (!this.isInitialized)
      return;
    this.InitDefaultVariables();
  }

  private void Update()
  {
    if (!this.canUpdate)
      return;
    if (this.updateTime)
    {
      this.time = Time.get_time();
      this.updateTime = false;
    }
    float num1 = Mathf.Sin((Time.get_time() - this.time) / this.Speed);
    float num2;
    if ((double) this.oldSin > (double) num1)
    {
      this.canUpdate = false;
      num2 = this.MaxScale;
    }
    else
      num2 = num1 * this.MaxScale;
    if (this.FadeInOutStatus == FadeInOutStatus.In)
    {
      if ((double) num2 < (double) this.MaxScale)
        this.t.set_localScale(new Vector3((float) this.oldScale.x * num2, (float) this.oldScale.y * num2, (float) this.oldScale.z * num2));
      else
        this.t.set_localScale(new Vector3(this.MaxScale, this.MaxScale, this.MaxScale));
    }
    if (this.FadeInOutStatus == FadeInOutStatus.Out)
    {
      if ((double) num2 > 0.0)
        this.t.set_localScale(new Vector3((float) ((double) this.MaxScale * this.oldScale.x - this.oldScale.x * (double) num2), (float) ((double) this.MaxScale * this.oldScale.y - this.oldScale.y * (double) num2), (float) ((double) this.MaxScale * this.oldScale.z - this.oldScale.z * (double) num2)));
      else
        this.t.set_localScale(Vector3.get_zero());
    }
    if (this.FadeInOutStatus == FadeInOutStatus.OutAfterCollision && this.isCollisionEnter)
    {
      if ((double) num2 > 0.0)
        this.t.set_localScale(new Vector3((float) ((double) this.MaxScale * this.oldScale.x - this.oldScale.x * (double) num2), (float) ((double) this.MaxScale * this.oldScale.y - this.oldScale.y * (double) num2), (float) ((double) this.MaxScale * this.oldScale.z - this.oldScale.z * (double) num2)));
      else
        this.t.set_localScale(Vector3.get_zero());
    }
    this.oldSin = num1;
  }
}
