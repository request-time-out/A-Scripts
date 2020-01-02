// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXPitchRandomizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace EpicToonFX
{
  public class ETFXPitchRandomizer : MonoBehaviour
  {
    public float randomPercent;

    public ETFXPitchRandomizer()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      M0 component = ((Component) ((Component) this).get_transform()).GetComponent<AudioSource>();
      ((AudioSource) component).set_pitch(((AudioSource) component).get_pitch() * (1f + Random.Range((float) (-(double) this.randomPercent / 100.0), this.randomPercent / 100f)));
    }
  }
}
