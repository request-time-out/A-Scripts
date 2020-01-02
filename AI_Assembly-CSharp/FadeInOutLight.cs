// Decompiled with JetBrains decompiler
// Type: FadeInOutLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class FadeInOutLight : MonoBehaviour
{
  public float StartDelay;
  public float FadeInSpeed;
  public float FadeOutDelay;
  public float FadeOutSpeed;
  public bool FadeOutAfterCollision;
  public bool UseHideStatus;
  private Light goLight;
  private float oldIntensity;
  private float currentIntensity;
  private float startIntensity;
  private bool canStart;
  private bool canStartFadeOut;
  private bool fadeInComplited;
  private bool fadeOutComplited;
  private bool isCollisionEnter;
  private bool allComplited;
  private bool isStartDelay;
  private bool isIn;
  private bool isOut;
  private EffectSettings effectSettings;
  private bool isInitialized;

  public FadeInOutLight()
  {
    base.\u002Ector();
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

  private void Start()
  {
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null))
      this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
    this.goLight = (Light) ((Component) this).GetComponent<Light>();
    this.startIntensity = this.goLight.get_intensity();
    this.isStartDelay = (double) this.StartDelay > 1.0 / 1000.0;
    this.isIn = (double) this.FadeInSpeed > 1.0 / 1000.0;
    this.isOut = (double) this.FadeOutSpeed > 1.0 / 1000.0;
    this.InitDefaultVariables();
    this.isInitialized = true;
  }

  private void InitDefaultVariables()
  {
    this.fadeInComplited = false;
    this.fadeOutComplited = false;
    this.allComplited = false;
    this.canStartFadeOut = false;
    this.isCollisionEnter = false;
    this.oldIntensity = 0.0f;
    this.currentIntensity = 0.0f;
    this.canStart = false;
    this.goLight.set_intensity(!this.isIn ? this.startIntensity : 0.0f);
    if (this.isStartDelay)
      this.Invoke("SetupStartDelay", this.StartDelay);
    else
      this.canStart = true;
    if (this.isIn)
      return;
    if (!this.FadeOutAfterCollision)
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    this.oldIntensity = this.startIntensity;
  }

  private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    this.isCollisionEnter = true;
    if (this.isIn || !this.FadeOutAfterCollision)
      return;
    this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
  }

  private void OnEnable()
  {
    if (!this.isInitialized)
      return;
    this.InitDefaultVariables();
  }

  private void SetupStartDelay()
  {
    this.canStart = true;
  }

  private void SetupFadeOutDelay()
  {
    this.canStartFadeOut = true;
  }

  private void Update()
  {
    if (!this.canStart)
      return;
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null) && this.UseHideStatus && (this.allComplited && this.effectSettings.IsVisible))
    {
      this.allComplited = false;
      this.fadeInComplited = false;
      this.fadeOutComplited = false;
      this.InitDefaultVariables();
    }
    if (this.isIn && !this.fadeInComplited)
    {
      if (Object.op_Equality((Object) this.effectSettings, (Object) null))
        this.FadeIn();
      else if (this.UseHideStatus && this.effectSettings.IsVisible || !this.UseHideStatus)
        this.FadeIn();
    }
    if (!this.isOut || this.fadeOutComplited || !this.canStartFadeOut)
      return;
    if (Object.op_Equality((Object) this.effectSettings, (Object) null) || !this.UseHideStatus && !this.FadeOutAfterCollision)
    {
      this.FadeOut();
    }
    else
    {
      if ((!this.UseHideStatus || this.effectSettings.IsVisible) && (!this.FadeOutAfterCollision || !this.isCollisionEnter))
        return;
      this.FadeOut();
    }
  }

  private void FadeIn()
  {
    this.currentIntensity = this.oldIntensity + Time.get_deltaTime() / this.FadeInSpeed * this.startIntensity;
    if ((double) this.currentIntensity >= (double) this.startIntensity)
    {
      this.fadeInComplited = true;
      if (!this.isOut)
        this.allComplited = true;
      this.currentIntensity = this.startIntensity;
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    }
    this.goLight.set_intensity(this.currentIntensity);
    this.oldIntensity = this.currentIntensity;
  }

  private void FadeOut()
  {
    this.currentIntensity = this.oldIntensity - Time.get_deltaTime() / this.FadeOutSpeed * this.startIntensity;
    if ((double) this.currentIntensity <= 0.0)
    {
      this.currentIntensity = 0.0f;
      this.fadeOutComplited = true;
      this.allComplited = true;
    }
    this.goLight.set_intensity(this.currentIntensity);
    this.oldIntensity = this.currentIntensity;
  }
}
