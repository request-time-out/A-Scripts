// Decompiled with JetBrains decompiler
// Type: EnviroReflections
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroReflections : MonoBehaviour
{
  public ReflectionProbe probe;
  public float ReflectionUpdateInGameHours;
  private double lastUpdate;

  public EnviroReflections()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Equality((Object) this.probe, (Object) null))
      return;
    this.probe = (ReflectionProbe) ((Component) this).GetComponent<ReflectionProbe>();
  }

  private void UpdateProbe()
  {
    this.probe.RenderProbe();
    this.lastUpdate = EnviroSky.instance.currentTimeInHours;
  }

  private void Update()
  {
    if (Object.op_Equality((Object) EnviroSky.instance, (Object) null) || EnviroSky.instance.currentTimeInHours <= this.lastUpdate + (double) this.ReflectionUpdateInGameHours && EnviroSky.instance.currentTimeInHours >= this.lastUpdate - (double) this.ReflectionUpdateInGameHours)
      return;
    this.UpdateProbe();
  }
}
