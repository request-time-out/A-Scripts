// Decompiled with JetBrains decompiler
// Type: LineRendererFadeInOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class LineRendererFadeInOut : MonoBehaviour
{
  public EffectSettings EffectSettings;
  public float FadeInSpeed;
  public float FadeOutSpeed;
  public float Length;
  public float StartWidth;
  public float EndWidth;
  private FadeInOutStatus fadeInOutStatus;
  private LineRenderer lineRenderer;
  private float currentStartWidth;
  private float currentEndWidth;
  private float currentLength;
  private bool isInit;
  private bool canUpdate;

  public LineRendererFadeInOut()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (Object.op_Inequality((Object) this.EffectSettings, (Object) null))
      this.EffectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.EffectSettings_CollisionEnter);
    this.lineRenderer = (LineRenderer) ((Component) this).GetComponent<LineRenderer>();
    this.fadeInOutStatus = FadeInOutStatus.In;
    this.lineRenderer.SetPosition(1, new Vector3(0.0f, 0.0f, 0.0f));
    this.lineRenderer.SetWidth(0.0f, 0.0f);
    ((Renderer) this.lineRenderer).set_enabled(true);
    this.isInit = true;
  }

  private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
  {
    this.fadeInOutStatus = FadeInOutStatus.Out;
    this.canUpdate = true;
  }

  private void OnEnable()
  {
    if (!this.isInit)
      return;
    this.fadeInOutStatus = FadeInOutStatus.In;
    this.canUpdate = true;
    ((Renderer) this.lineRenderer).set_enabled(true);
  }

  private void Update()
  {
    switch (this.fadeInOutStatus)
    {
      case FadeInOutStatus.In:
        if (!this.canUpdate)
          break;
        this.currentStartWidth += Time.get_deltaTime() * (this.StartWidth / this.FadeInSpeed);
        this.currentEndWidth += Time.get_deltaTime() * (this.EndWidth / this.FadeInSpeed);
        this.currentLength += Time.get_deltaTime() * (this.Length / this.FadeInSpeed);
        if ((double) this.currentStartWidth >= (double) this.StartWidth)
        {
          this.canUpdate = false;
          this.currentStartWidth = this.StartWidth;
          this.currentEndWidth = this.EndWidth;
          this.currentLength = this.Length;
        }
        this.lineRenderer.SetPosition(1, new Vector3(0.0f, 0.0f, this.currentLength));
        this.lineRenderer.SetWidth(this.currentStartWidth, this.currentEndWidth);
        break;
      case FadeInOutStatus.Out:
        if (!this.canUpdate)
          break;
        this.currentStartWidth -= Time.get_deltaTime() * (this.StartWidth / this.FadeOutSpeed);
        this.currentEndWidth -= Time.get_deltaTime() * (this.EndWidth / this.FadeOutSpeed);
        this.currentLength -= Time.get_deltaTime() * (this.Length / this.FadeOutSpeed);
        if ((double) this.currentStartWidth <= 0.0)
        {
          this.canUpdate = false;
          this.currentStartWidth = 0.0f;
          this.currentEndWidth = 0.0f;
          this.currentLength = 0.0f;
        }
        this.lineRenderer.SetPosition(1, new Vector3(0.0f, 0.0f, this.currentLength));
        this.lineRenderer.SetWidth(this.currentStartWidth, this.currentEndWidth);
        if (this.canUpdate)
          break;
        ((Renderer) this.lineRenderer).set_enabled(false);
        break;
    }
  }
}
