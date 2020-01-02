// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputFieldInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class InputFieldInfo : UIElementInfo
  {
    public int actionId { get; set; }

    public AxisRange axisRange { get; set; }

    public int actionElementMapId { get; set; }

    public ControllerType controllerType { get; set; }

    public int controllerId { get; set; }
  }
}
