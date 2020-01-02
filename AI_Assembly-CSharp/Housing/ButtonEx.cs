// Decompiled with JetBrains decompiler
// Type: Housing.ButtonEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.UI;

namespace Housing
{
  public class ButtonEx : Button
  {
    public ButtonEx()
    {
      base.\u002Ector();
    }

    public void ClearState()
    {
      ((Selectable) this).InstantClearState();
    }
  }
}
