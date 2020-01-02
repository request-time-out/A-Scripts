// Decompiled with JetBrains decompiler
// Type: HitPSControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class HitPSControl : MonoBehaviour
{
  public HitPSControl()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    ParticleSystem component = (ParticleSystem) ((Component) this).GetComponent<ParticleSystem>();
    GameObject gameObject = ((Component) this).get_gameObject();
    ParticleSystem.MainModule main = component.get_main();
    double duration = (double) ((ParticleSystem.MainModule) ref main).get_duration();
    Object.Destroy((Object) gameObject, (float) duration);
  }

  private void Update()
  {
  }
}
