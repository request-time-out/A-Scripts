// Decompiled with JetBrains decompiler
// Type: DigitalRuby.RainMaker.DemoScriptStartRainOnSpaceBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace DigitalRuby.RainMaker
{
  public class DemoScriptStartRainOnSpaceBar : MonoBehaviour
  {
    public BaseRainScript RainScript;

    public DemoScriptStartRainOnSpaceBar()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (Object.op_Equality((Object) this.RainScript, (Object) null))
        return;
      this.RainScript.EnableWind = false;
    }

    private void Update()
    {
      if (Object.op_Equality((Object) this.RainScript, (Object) null) || !Input.GetKeyDown((KeyCode) 32))
        return;
      this.RainScript.RainIntensity = (double) this.RainScript.RainIntensity != 0.0 ? 0.0f : 1f;
      this.RainScript.EnableWind = !this.RainScript.EnableWind;
    }
  }
}
