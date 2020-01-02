// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.PickerRectA
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  public class PickerRectA : PickerRect
  {
    public BoolReactiveProperty isAlpha = new BoolReactiveProperty(true);
    [SerializeField]
    private Slider sliderA;
    private float alpha;

    public override Color ColorRGB
    {
      get
      {
        Color colorRgb = base.ColorRGB;
        colorRgb.a = (__Null) (double) this.Alpha;
        return colorRgb;
      }
      set
      {
        base.ColorRGB = value;
        this.Alpha = (float) value.a;
      }
    }

    public override float[] RGB
    {
      get
      {
        return ((IEnumerable<float>) base.RGB).Concat<float>((IEnumerable<float>) new float[1]
        {
          this.Alpha
        }).ToArray<float>();
      }
      set
      {
        base.RGB = value;
        if (value.Length < 4)
          return;
        this.Alpha = value[3];
      }
    }

    public float Alpha
    {
      get
      {
        return this.alpha;
      }
      set
      {
        this.alpha = value;
      }
    }

    public override void SetColor(HsvColor hsv, PickerRect.Control ctrlType)
    {
      this.ColorHSV = hsv;
      base.ColorRGB = HsvColor.ToRgb(hsv);
      this.SetColor(ctrlType);
    }

    public override void SetColor(Color color)
    {
      base.SetColor(color);
      this.CalcSliderAValue();
    }

    public void CalcSliderAValue()
    {
      if (Object.op_Equality((Object) this.sliderA, (Object) null))
        return;
      this.sliderA.set_value(this.Alpha);
    }

    protected override void Start()
    {
      ObservableExtensions.Subscribe<PickerRect.Mode>(Observable.TakeUntilDestroy<PickerRect.Mode>((IObservable<M0>) this._mode, (UnityEngine.Component) this), (Action<M0>) (_ => this.CalcSliderAValue()));
      if (Object.op_Inequality((Object) this.sliderA, (Object) null))
      {
        ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this.isAlpha, (UnityEngine.Component) this), (Action<M0>) new Action<bool>(((UnityEngine.Component) this.sliderA).get_gameObject().SetActive));
        ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderA.get_onValueChanged()), (Action<M0>) (value =>
        {
          this.Alpha = value;
          this.SetColor(this.ColorRGB, PickerRect.Control.None);
        }));
      }
      base.Start();
    }
  }
}
