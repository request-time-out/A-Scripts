// Decompiled with JetBrains decompiler
// Type: AIProject.UI.FishingHowToUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace AIProject.UI
{
  public class FishingHowToUI : MonoBehaviour
  {
    public Image image;
    public Text text;

    public FishingHowToUI()
    {
      base.\u002Ector();
    }

    public void Set(Sprite _sprite, string _text)
    {
      if (Object.op_Implicit((Object) this.image))
        this.image.set_sprite(_sprite);
      if (!Object.op_Implicit((Object) this.text))
        return;
      this.text.set_text(_text);
    }
  }
}
