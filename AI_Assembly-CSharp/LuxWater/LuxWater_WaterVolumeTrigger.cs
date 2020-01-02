// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_WaterVolumeTrigger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater
{
  public class LuxWater_WaterVolumeTrigger : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.cetbv2etlk23")]
    public Camera cam;
    public bool active;

    public LuxWater_WaterVolumeTrigger()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      if (!Object.op_Equality((Object) this.cam, (Object) null))
        return;
      Camera component = (Camera) ((Component) this).GetComponent<Camera>();
      if (Object.op_Inequality((Object) component, (Object) null))
        this.cam = component;
      else
        this.active = false;
    }
  }
}
