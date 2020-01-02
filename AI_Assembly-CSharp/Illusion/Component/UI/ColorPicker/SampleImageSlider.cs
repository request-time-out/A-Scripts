// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.SampleImageSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  public class SampleImageSlider : SampleImage
  {
    [SerializeField]
    private PickerSlider slider;

    private void Start()
    {
      this.slider.color = ((Graphic) this.image).get_color();
      this.slider.updateColorAction += (Action<Color>) (color => ((Graphic) this.image).set_color(color));
    }
  }
}
