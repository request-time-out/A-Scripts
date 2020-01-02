// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_PlanarWaterTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace LuxWater
{
  [ExecuteInEditMode]
  public class LuxWater_PlanarWaterTile : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.nu6w5ylbucb7")]
    public LuxWater_PlanarReflection reflection;

    public LuxWater_PlanarWaterTile()
    {
      base.\u002Ector();
    }

    public void OnEnable()
    {
      this.AcquireComponents();
    }

    private void AcquireComponents()
    {
      if (Object.op_Implicit((Object) this.reflection))
        return;
      if (Object.op_Implicit((Object) ((Component) this).get_transform().get_parent()))
        this.reflection = (LuxWater_PlanarReflection) ((Component) ((Component) this).get_transform().get_parent()).GetComponent<LuxWater_PlanarReflection>();
      else
        this.reflection = (LuxWater_PlanarReflection) ((Component) ((Component) this).get_transform()).GetComponent<LuxWater_PlanarReflection>();
    }

    public void OnWillRenderObject()
    {
      if (!Object.op_Implicit((Object) this.reflection))
        return;
      this.reflection.WaterTileBeingRendered(((Component) this).get_transform(), Camera.get_current());
    }
  }
}
