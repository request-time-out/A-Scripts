// Decompiled with JetBrains decompiler
// Type: HSceneSpriteClothBtn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class HSceneSpriteClothBtn : MonoBehaviour
{
  private int state;
  public Button[] buttons;

  public HSceneSpriteClothBtn()
  {
    base.\u002Ector();
  }

  private int State
  {
    get
    {
      return this.state;
    }
  }

  public void SetButton(int State)
  {
    this.state = State;
    if (State >= this.buttons.Length)
      this.state = this.buttons.Length - 1;
    for (int index = 0; index < this.buttons.Length; ++index)
    {
      if (index != this.state)
        ((Component) this.buttons[index]).get_gameObject().SetActive(false);
      else
        ((Component) this.buttons[index]).get_gameObject().SetActive(true);
    }
  }
}
