// Decompiled with JetBrains decompiler
// Type: EnviroLightning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class EnviroLightning : MonoBehaviour
{
  public EnviroLightning()
  {
    base.\u002Ector();
  }

  public void Lightning()
  {
    this.StartCoroutine(this.LightningBolt());
  }

  public void StopLightning()
  {
    this.StopAllCoroutines();
    ((Behaviour) ((Component) this).GetComponent<Light>()).set_enabled(false);
    EnviroSky.instance.thunder = 0.0f;
  }

  [DebuggerHidden]
  public IEnumerator LightningBolt()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnviroLightning.\u003CLightningBolt\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
