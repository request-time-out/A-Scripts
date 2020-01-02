// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class InputRow : MonoBehaviour
  {
    public Text label;
    private int rowIndex;
    private Action<int, ButtonInfo> inputFieldActivatedCallback;

    public InputRow()
    {
      base.\u002Ector();
    }

    public ButtonInfo[] buttons { get; private set; }

    public void Initialize(
      int rowIndex,
      string label,
      Action<int, ButtonInfo> inputFieldActivatedCallback)
    {
      this.rowIndex = rowIndex;
      this.label.set_text(label);
      this.inputFieldActivatedCallback = inputFieldActivatedCallback;
      this.buttons = (ButtonInfo[]) ((Component) ((Component) this).get_transform()).GetComponentsInChildren<ButtonInfo>(true);
    }

    public void OnButtonActivated(ButtonInfo buttonInfo)
    {
      if (this.inputFieldActivatedCallback == null)
        return;
      this.inputFieldActivatedCallback(this.rowIndex, buttonInfo);
    }
  }
}
