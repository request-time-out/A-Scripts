// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.UGUIToggleExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine.UI;

namespace Illusion.Extensions
{
  public static class UGUIToggleExtensions
  {
    public static void SetIsOnWithoutCallback(this Toggle self, bool isOn)
    {
      Toggle.ToggleEvent onValueChanged = (Toggle.ToggleEvent) self.onValueChanged;
      self.onValueChanged = (__Null) new Toggle.ToggleEvent();
      self.set_isOn(isOn);
      self.onValueChanged = (__Null) onValueChanged;
    }
  }
}
