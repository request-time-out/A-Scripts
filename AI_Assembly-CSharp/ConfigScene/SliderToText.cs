// Decompiled with JetBrains decompiler
// Type: ConfigScene.SliderToText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace ConfigScene
{
  public class SliderToText : MonoBehaviour
  {
    [SerializeField]
    private Slider silder;
    [SerializeField]
    private Text text;

    public SliderToText()
    {
      base.\u002Ector();
    }

    public void Start()
    {
      this.OnValueChanged();
    }

    public void OnValueChanged()
    {
      if (Object.op_Equality((Object) this.silder, (Object) null) || Object.op_Equality((Object) this.text, (Object) null))
        return;
      this.text.set_text(this.silder.get_value().ToString("0"));
    }
  }
}
