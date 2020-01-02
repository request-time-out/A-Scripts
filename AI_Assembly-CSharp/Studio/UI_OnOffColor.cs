// Decompiled with JetBrains decompiler
// Type: Studio.UI_OnOffColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class UI_OnOffColor : MonoBehaviour
  {
    public Image[] images;
    public Color onColor;
    public Color offColor;

    public UI_OnOffColor()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      Toggle component = (Toggle) ((Component) this).GetComponent<Toggle>();
      if (!Object.op_Implicit((Object) component))
        return;
      this.OnChange(component.get_isOn());
    }

    public void OnChange(bool check)
    {
      if (this.images == null)
        return;
      Color color = !check ? this.offColor : this.onColor;
      foreach (Graphic image in this.images)
        image.set_color(color);
    }
  }
}
