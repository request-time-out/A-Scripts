// Decompiled with JetBrains decompiler
// Type: Housing.SaveLoad.SizeTab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Housing.SaveLoad
{
  public class SizeTab : MonoBehaviour
  {
    public Toggle toggle;
    public UnityEngine.UI.Text text;

    public SizeTab()
    {
      base.\u002Ector();
    }

    public string Text
    {
      set
      {
        this.text.set_text(value);
      }
    }
  }
}
