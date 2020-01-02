// Decompiled with JetBrains decompiler
// Type: Studio.LightColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class LightColor : MonoBehaviour
  {
    [SerializeField]
    private Renderer renderer;
    private Material material;

    public LightColor()
    {
      base.\u002Ector();
    }

    public Color color
    {
      set
      {
        if (!Object.op_Implicit((Object) this.material))
          return;
        this.material.set_color(value);
      }
    }

    public virtual void Awake()
    {
      this.material = this.renderer.get_material();
    }
  }
}
