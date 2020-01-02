// Decompiled with JetBrains decompiler
// Type: ME_ParticleTrails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ME_ParticleTrails : MonoBehaviour
{
  public GameObject TrailPrefab;
  private ParticleSystem ps;
  private ParticleSystem.Particle[] particles;
  private Dictionary<uint, GameObject> hashTrails;
  private Dictionary<uint, GameObject> newHashTrails;
  private List<GameObject> currentGO;

  public ME_ParticleTrails()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.ps = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
    ParticleSystem.MainModule main = this.ps.get_main();
    this.particles = new ParticleSystem.Particle[((ParticleSystem.MainModule) ref main).get_maxParticles()];
  }

  private void OnEnable()
  {
    this.InvokeRepeating("ClearEmptyHashes", 1f, 1f);
  }

  private void OnDisable()
  {
    this.Clear();
    this.CancelInvoke("ClearEmptyHashes");
  }

  public void Clear()
  {
    using (List<GameObject>.Enumerator enumerator = this.currentGO.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Object.Destroy((Object) enumerator.Current);
    }
    this.currentGO.Clear();
  }

  private void Update()
  {
    this.UpdateTrail();
  }

  private void UpdateTrail()
  {
    this.newHashTrails.Clear();
    int particles = this.ps.GetParticles(this.particles);
    for (int index = 0; index < particles; ++index)
    {
      if (!this.hashTrails.ContainsKey(((ParticleSystem.Particle) ref this.particles[index]).get_randomSeed()))
      {
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.TrailPrefab, ((Component) this).get_transform().get_position(), (Quaternion) null);
        gameObject.get_transform().set_parent(((Component) this).get_transform());
        this.currentGO.Add(gameObject);
        this.newHashTrails.Add(((ParticleSystem.Particle) ref this.particles[index]).get_randomSeed(), gameObject);
        LineRenderer component = (LineRenderer) gameObject.GetComponent<LineRenderer>();
        component.set_widthMultiplier(component.get_widthMultiplier() * ((ParticleSystem.Particle) ref this.particles[index]).get_startSize());
      }
      else
      {
        GameObject hashTrail = this.hashTrails[((ParticleSystem.Particle) ref this.particles[index]).get_randomSeed()];
        if (Object.op_Inequality((Object) hashTrail, (Object) null))
        {
          LineRenderer component = (LineRenderer) hashTrail.GetComponent<LineRenderer>();
          LineRenderer lineRenderer1 = component;
          lineRenderer1.set_startColor(Color.op_Multiply(lineRenderer1.get_startColor(), Color32.op_Implicit(((ParticleSystem.Particle) ref this.particles[index]).GetCurrentColor(this.ps))));
          LineRenderer lineRenderer2 = component;
          lineRenderer2.set_endColor(Color.op_Multiply(lineRenderer2.get_endColor(), Color32.op_Implicit(((ParticleSystem.Particle) ref this.particles[index]).GetCurrentColor(this.ps))));
          ParticleSystem.MainModule main1 = this.ps.get_main();
          if (((ParticleSystem.MainModule) ref main1).get_simulationSpace() == 1)
            hashTrail.get_transform().set_position(((ParticleSystem.Particle) ref this.particles[index]).get_position());
          ParticleSystem.MainModule main2 = this.ps.get_main();
          if (((ParticleSystem.MainModule) ref main2).get_simulationSpace() == null)
            hashTrail.get_transform().set_position(((Component) this.ps).get_transform().TransformPoint(((ParticleSystem.Particle) ref this.particles[index]).get_position()));
          this.newHashTrails.Add(((ParticleSystem.Particle) ref this.particles[index]).get_randomSeed(), hashTrail);
        }
        this.hashTrails.Remove(((ParticleSystem.Particle) ref this.particles[index]).get_randomSeed());
      }
    }
    using (Dictionary<uint, GameObject>.Enumerator enumerator = this.hashTrails.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<uint, GameObject> current = enumerator.Current;
        if (Object.op_Inequality((Object) current.Value, (Object) null))
          ((ME_TrailRendererNoise) current.Value.GetComponent<ME_TrailRendererNoise>()).IsActive = false;
      }
    }
    this.AddRange<uint, GameObject>(this.hashTrails, this.newHashTrails);
  }

  public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
  {
    if (collection == null)
      return;
    foreach (KeyValuePair<T, S> keyValuePair in collection)
    {
      if (!source.ContainsKey(keyValuePair.Key))
        source.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private void ClearEmptyHashes()
  {
    this.hashTrails = ((IEnumerable<KeyValuePair<uint, GameObject>>) this.hashTrails).Where<KeyValuePair<uint, GameObject>>((Func<KeyValuePair<uint, GameObject>, bool>) (h => Object.op_Inequality((Object) h.Value, (Object) null))).ToDictionary<KeyValuePair<uint, GameObject>, uint, GameObject>((Func<KeyValuePair<uint, GameObject>, uint>) (h => h.Key), (Func<KeyValuePair<uint, GameObject>, GameObject>) (h => h.Value));
  }
}
