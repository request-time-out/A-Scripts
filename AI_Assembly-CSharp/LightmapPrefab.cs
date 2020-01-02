// Decompiled with JetBrains decompiler
// Type: LightmapPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class LightmapPrefab : MonoBehaviour
{
  [SerializeField]
  private LightmapPrefab.LightmapParameter[] lightmapParameters;

  public LightmapPrefab()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.Resetup();
  }

  [ContextMenu("Setup")]
  private void Setup()
  {
    Renderer[] array = ((IEnumerable<Renderer>) ((Component) this).GetComponentsInChildren<Renderer>()).Where<Renderer>((Func<Renderer, bool>) (v => v.get_enabled() && v.get_lightmapIndex() >= 0)).ToArray<Renderer>();
    this.lightmapParameters = new LightmapPrefab.LightmapParameter[array.Length];
    for (int index = 0; index < array.Length; ++index)
    {
      Renderer renderer = array[index];
      this.lightmapParameters[index] = new LightmapPrefab.LightmapParameter()
      {
        lightmapIndex = renderer.get_lightmapIndex(),
        scaleOffset = renderer.get_lightmapScaleOffset(),
        renderer = renderer
      };
    }
  }

  [ContextMenu("Resetup")]
  public void Resetup()
  {
    if (this.lightmapParameters == null)
      return;
    int length = this.lightmapParameters.Length;
    for (int index = 0; index < length; ++index)
    {
      if (this.lightmapParameters[index] != null)
        this.lightmapParameters[index].UpdateLightmapUVs();
    }
  }

  [Serializable]
  private class LightmapParameter
  {
    public int lightmapIndex = -1;
    public Vector4 scaleOffset = Vector4.get_zero();
    public Renderer renderer;

    public void UpdateLightmapUVs()
    {
      if (Object.op_Equality((Object) this.renderer, (Object) null) || this.lightmapIndex < 0)
        return;
      this.renderer.set_lightmapScaleOffset(this.scaleOffset);
      this.renderer.set_lightmapIndex(this.lightmapIndex);
    }
  }
}
