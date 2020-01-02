// Decompiled with JetBrains decompiler
// Type: FadeInOutShaderFloat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class FadeInOutShaderFloat : MonoBehaviour
{
  public string PropertyName;
  public float MaxFloat;
  public float StartDelay;
  public float FadeInSpeed;
  public float FadeOutDelay;
  public float FadeOutSpeed;
  public bool FadeOutAfterCollision;
  public bool UseHideStatus;
  private Material OwnMaterial;
  private Material mat;
  private float oldFloat;
  private float currentFloat;
  private bool canStart;
  private bool canStartFadeOut;
  private bool fadeInComplited;
  private bool fadeOutComplited;
  private bool previousFrameVisibleStatus;
  private bool isCollisionEnter;
  private bool isStartDelay;
  private bool isIn;
  private bool isOut;
  private EffectSettings effectSettings;
  private bool isInitialized;

  public FadeInOutShaderFloat()
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
    this.InitMaterial();
  }

  public void UpdateMaterial(Material instanceMaterial)
  {
    this.mat = instanceMaterial;
    this.InitMaterial();
  }

  private void InitMaterial()
  {
    if (this.isInitialized)
      return;
    if (Object.op_Inequality((Object) ((Component) this).GetComponent<Renderer>(), (Object) null))
    {
      this.mat = ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material();
    }
    else
    {
      LineRenderer component1 = (LineRenderer) ((Component) this).GetComponent<LineRenderer>();
      if (Object.op_Inequality((Object) component1, (Object) null))
      {
        this.mat = ((Renderer) component1).get_material();
      }
      else
      {
        Projector component2 = (Projector) ((Component) this).GetComponent<Projector>();
        if (Object.op_Inequality((Object) component2, (Object) null))
        {
          if (!((Object) component2.get_material()).get_name().EndsWith("(Instance)"))
          {
            Projector projector = component2;
            Material material1 = new Material(component2.get_material());
            ((Object) material1).set_name(((Object) component2.get_material()).get_name() + " (Instance)");
            Material material2 = material1;
            projector.set_material(material2);
          }
          this.mat = component2.get_material();
        }
      }
    }
    if (Object.op_Equality((Object) this.mat, (Object) null))
      return;
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
    this.canStartFadeOut = false;
    this.canStart = false;
    this.isCollisionEnter = false;
    this.oldFloat = 0.0f;
    this.currentFloat = this.MaxFloat;
    if (this.isIn)
      this.currentFloat = 0.0f;
    this.mat.SetFloat(this.PropertyName, this.currentFloat);
    if (this.isStartDelay)
      this.Invoke("SetupStartDelay", this.StartDelay);
    else
      this.canStart = true;
    if (this.isIn)
      return;
    if (!this.FadeOutAfterCollision)
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    this.oldFloat = this.MaxFloat;
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
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null) && this.UseHideStatus)
    {
      if (!this.effectSettings.IsVisible && this.fadeInComplited)
        this.fadeInComplited = false;
      if (this.effectSettings.IsVisible && this.fadeOutComplited)
        this.fadeOutComplited = false;
    }
    if (this.UseHideStatus)
    {
      if (this.isIn && Object.op_Inequality((Object) this.effectSettings, (Object) null) && (this.effectSettings.IsVisible && !this.fadeInComplited))
        this.FadeIn();
      if (!this.isOut || !Object.op_Inequality((Object) this.effectSettings, (Object) null) || (this.effectSettings.IsVisible || this.fadeOutComplited))
        return;
      this.FadeOut();
    }
    else if (!this.FadeOutAfterCollision)
    {
      if (this.isIn && !this.fadeInComplited)
        this.FadeIn();
      if (!this.isOut || !this.canStartFadeOut || this.fadeOutComplited)
        return;
      this.FadeOut();
    }
    else
    {
      if (this.isIn && !this.fadeInComplited)
        this.FadeIn();
      if (!this.isOut || !this.isCollisionEnter || (!this.canStartFadeOut || this.fadeOutComplited))
        return;
      this.FadeOut();
    }
  }

  private void FadeIn()
  {
    this.currentFloat = this.oldFloat + Time.get_deltaTime() / this.FadeInSpeed * this.MaxFloat;
    if ((double) this.currentFloat >= (double) this.MaxFloat)
    {
      this.fadeInComplited = true;
      this.currentFloat = this.MaxFloat;
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    }
    this.mat.SetFloat(this.PropertyName, this.currentFloat);
    this.oldFloat = this.currentFloat;
  }

  private void FadeOut()
  {
    this.currentFloat = this.oldFloat - Time.get_deltaTime() / this.FadeOutSpeed * this.MaxFloat;
    if ((double) this.currentFloat <= 0.0)
    {
      this.currentFloat = 0.0f;
      this.fadeOutComplited = true;
    }
    this.mat.SetFloat(this.PropertyName, this.currentFloat);
    this.oldFloat = this.currentFloat;
  }
}
