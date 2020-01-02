// Decompiled with JetBrains decompiler
// Type: ME_LightCurves
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class ME_LightCurves : MonoBehaviour
{
  public AnimationCurve LightCurve;
  public float GraphTimeMultiplier;
  public float GraphIntensityMultiplier;
  public bool IsLoop;
  private bool canUpdate;
  private float startTime;
  private Light lightSource;

  public ME_LightCurves()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.lightSource = (Light) ((Component) this).GetComponent<Light>();
    this.lightSource.set_intensity(this.LightCurve.Evaluate(0.0f));
  }

  private void OnEnable()
  {
    this.startTime = Time.get_time();
    this.canUpdate = true;
  }

  private void Update()
  {
    float num = Time.get_time() - this.startTime;
    if (this.canUpdate)
      this.lightSource.set_intensity(this.LightCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier);
    if ((double) num < (double) this.GraphTimeMultiplier)
      return;
    if (this.IsLoop)
      this.startTime = Time.get_time();
    else
      this.canUpdate = false;
  }
}
