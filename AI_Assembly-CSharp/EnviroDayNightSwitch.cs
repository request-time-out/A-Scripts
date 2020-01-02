// Decompiled with JetBrains decompiler
// Type: EnviroDayNightSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class EnviroDayNightSwitch : MonoBehaviour
{
  private Light[] lightsArray;

  public EnviroDayNightSwitch()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.lightsArray = (Light[]) ((Component) this).GetComponentsInChildren<Light>();
    EnviroSky.instance.OnDayTime += (EnviroSky.isDay) (() => this.Deactivate());
    EnviroSky.instance.OnNightTime += (EnviroSky.isNightE) (() => this.Activate());
    if (EnviroSky.instance.isNight)
      this.Activate();
    else
      this.Deactivate();
  }

  private void Activate()
  {
    for (int index = 0; index < this.lightsArray.Length; ++index)
      ((Behaviour) this.lightsArray[index]).set_enabled(true);
  }

  private void Deactivate()
  {
    for (int index = 0; index < this.lightsArray.Length; ++index)
      ((Behaviour) this.lightsArray[index]).set_enabled(false);
  }
}
