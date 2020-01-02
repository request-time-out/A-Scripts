// Decompiled with JetBrains decompiler
// Type: FadeInOutShaderColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class FadeInOutShaderColor : MonoBehaviour
{
  public string ShaderColorName;
  public float StartDelay;
  public float FadeInSpeed;
  public float FadeOutDelay;
  public float FadeOutSpeed;
  public bool UseSharedMaterial;
  public bool FadeOutAfterCollision;
  public bool UseHideStatus;
  private Material mat;
  private Color oldColor;
  private Color currentColor;
  private float oldAlpha;
  private float alpha;
  private bool canStart;
  private bool canStartFadeOut;
  private bool fadeInComplited;
  private bool fadeOutComplited;
  private bool isCollisionEnter;
  private bool isStartDelay;
  private bool isIn;
  private bool isOut;
  private EffectSettings effectSettings;
  private bool isInitialized;

  public FadeInOutShaderColor()
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

  public void UpdateMaterial(Material instanceMaterial)
  {
    this.mat = instanceMaterial;
    this.InitMaterial();
  }

  private void Start()
  {
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (Object.op_Inequality((Object) this.effectSettings, (Object) null))
      this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
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
    this.oldColor = this.mat.GetColor(this.ShaderColorName);
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
    this.isCollisionEnter = false;
    this.oldAlpha = 0.0f;
    this.alpha = 0.0f;
    this.canStart = false;
    this.currentColor = this.oldColor;
    if (this.isIn)
      this.currentColor.a = (__Null) 0.0;
    this.mat.SetColor(this.ShaderColorName, this.currentColor);
    if (this.isStartDelay)
      this.Invoke("SetupStartDelay", this.StartDelay);
    else
      this.canStart = true;
    if (this.isIn)
      return;
    if (!this.FadeOutAfterCollision)
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    this.oldAlpha = (float) this.oldColor.a;
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
    this.alpha = this.oldAlpha + Time.get_deltaTime() / this.FadeInSpeed;
    if ((double) this.alpha >= this.oldColor.a)
    {
      this.fadeInComplited = true;
      this.alpha = (float) this.oldColor.a;
      this.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
    }
    this.currentColor.a = (__Null) (double) this.alpha;
    this.mat.SetColor(this.ShaderColorName, this.currentColor);
    this.oldAlpha = this.alpha;
  }

  private void FadeOut()
  {
    this.alpha = this.oldAlpha - Time.get_deltaTime() / this.FadeOutSpeed;
    if ((double) this.alpha <= 0.0)
    {
      this.alpha = 0.0f;
      this.fadeOutComplited = true;
    }
    this.currentColor.a = (__Null) (double) this.alpha;
    this.mat.SetColor(this.ShaderColorName, this.currentColor);
    this.oldAlpha = this.alpha;
  }
}
