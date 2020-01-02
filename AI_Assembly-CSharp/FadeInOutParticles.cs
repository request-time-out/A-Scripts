// Decompiled with JetBrains decompiler
// Type: FadeInOutParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class FadeInOutParticles : MonoBehaviour
{
  private EffectSettings effectSettings;
  private ParticleSystem[] particles;
  private bool oldVisibleStat;

  public FadeInOutParticles()
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
    this.particles = (ParticleSystem[]) ((Component) this.effectSettings).GetComponentsInChildren<ParticleSystem>();
    this.oldVisibleStat = this.effectSettings.IsVisible;
  }

  private void Update()
  {
    if (this.effectSettings.IsVisible != this.oldVisibleStat)
    {
      if (this.effectSettings.IsVisible)
      {
        foreach (ParticleSystem particle in this.particles)
        {
          if (this.effectSettings.IsVisible)
            particle.Play();
          ParticleSystem.EmissionModule emission = particle.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(true);
        }
      }
      else
      {
        foreach (ParticleSystem particle in this.particles)
        {
          if (!this.effectSettings.IsVisible)
            particle.Stop();
          ParticleSystem.EmissionModule emission = particle.get_emission();
          ((ParticleSystem.EmissionModule) ref emission).set_enabled(false);
        }
      }
    }
    this.oldVisibleStat = this.effectSettings.IsVisible;
  }
}
