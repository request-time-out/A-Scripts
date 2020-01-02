// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXLightFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EpicToonFX
{
  public class ETFXLightFade : MonoBehaviour
  {
    [Header("Seconds to dim the light")]
    public float life;
    public bool killAfterLife;
    private Light li;
    private float initIntensity;

    public ETFXLightFade()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (Object.op_Implicit((Object) ((Component) this).get_gameObject().GetComponent<Light>()))
      {
        this.li = (Light) ((Component) this).get_gameObject().GetComponent<Light>();
        this.initIntensity = this.li.get_intensity();
      }
      else
        MonoBehaviour.print((object) ("No light object found on " + ((Object) ((Component) this).get_gameObject()).get_name()));
    }

    private void Update()
    {
      if (!Object.op_Implicit((Object) ((Component) this).get_gameObject().GetComponent<Light>()))
        return;
      Light li = this.li;
      li.set_intensity(li.get_intensity() - this.initIntensity * (Time.get_deltaTime() / this.life));
      if (!this.killAfterLife || (double) this.li.get_intensity() > 0.0)
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject().GetComponent<Light>());
    }
  }
}
