// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.PickerSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  public class PickerSlider : MonoBehaviour
  {
    [Tooltip("RedSlider")]
    [SerializeField]
    protected Slider sliderR;
    [Tooltip("GreenSlider")]
    [SerializeField]
    protected Slider sliderG;
    [Tooltip("BlueSlider")]
    [SerializeField]
    protected Slider sliderB;
    [Tooltip("AlphaSlider")]
    [SerializeField]
    protected Slider sliderA;
    public BoolReactiveProperty useAlpha;
    [SerializeField]
    protected BoolReactiveProperty _isHSV;
    private Slider[] sliders;
    private ImagePack[] imgPack;
    private Color _color;

    public PickerSlider()
    {
      base.\u002Ector();
    }

    public event Action<Color> updateColorAction;

    public Color color
    {
      get
      {
        return !this.isHSV ? this._color : this._color.HSVToRGB();
      }
      set
      {
        this._color = this.isHSV ? value.RGBToHSV() : value;
        this.SetColor(this._color);
      }
    }

    public bool isHSV
    {
      get
      {
        return ((ReactiveProperty<bool>) this._isHSV).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._isHSV).set_Value(value);
      }
    }

    public void ChangeSliderColor()
    {
      for (int index = 0; index < 3; ++index)
        this.ChangeSliderColor(index);
    }

    public void ChangeSliderColor(int index)
    {
      ImagePack imagePack = this.imgPack[index];
      if (imagePack == null || !imagePack.isTex)
        return;
      Vector2 size = imagePack.size;
      int x = (int) size.x;
      int y = (int) size.y;
      Color[] colors = new Color[y * x];
      float[] val = new float[3]
      {
        ((Color) ref this._color).get_Item(0),
        ((Color) ref this._color).get_Item(1),
        ((Color) ref this._color).get_Item(2)
      };
      Action<int> action;
      if (!this.isHSV)
      {
        action = (Action<int>) (i => colors[i] = new Color(val[0], val[1], val[2]));
      }
      else
      {
        if (index == 0)
        {
          val[1] = 1f;
          val[2] = 1f;
        }
        action = (Action<int>) (i => colors[i] = Color.HSVToRGB(val[0], val[1], val[2]));
      }
      if (y > x)
      {
        for (int index1 = 0; index1 < y; ++index1)
        {
          for (int index2 = 0; index2 < x; ++index2)
          {
            val[index] = Mathf.InverseLerp(0.0f, (float) size.y, (float) index1);
            action(index1 * x + index2);
          }
        }
      }
      else
      {
        for (int index1 = 0; index1 < y; ++index1)
        {
          for (int index2 = 0; index2 < x; ++index2)
          {
            val[index] = Mathf.InverseLerp(0.0f, (float) size.x, (float) index2);
            action(index1 * x + index2);
          }
        }
      }
      imagePack.SetPixels(colors);
    }

    public void CalcSliderValue()
    {
      for (int index = 0; index < 3; ++index)
      {
        if (!Object.op_Equality((Object) this.sliders[index], (Object) null))
          this.sliders[index].set_value(((Color) ref this._color).get_Item(index));
      }
      if (!Object.op_Inequality((Object) this.sliderA, (Object) null))
        return;
      this.sliderA.set_value((float) this._color.a);
    }

    public virtual void SetColor(Color color)
    {
      this._color = color;
      this.ChangeSliderColor();
      this.CalcSliderValue();
      this.updateColorAction.Call<Color>(this.color);
    }

    protected virtual void Awake()
    {
      this.sliders = new Slider[3]
      {
        this.sliderR,
        this.sliderG,
        this.sliderB
      };
      this.imgPack = new ImagePack[this.sliders.Length];
      for (int index = 0; index < this.sliders.Length; ++index)
      {
        Slider slider = this.sliders[index];
        if (!Object.op_Equality((Object) slider, (Object) null))
          this.imgPack[index] = new ImagePack(((UnityEngine.Component) slider).GetOrAddComponent<Image>());
      }
    }

    protected virtual void Start()
    {
      ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isHSV, (UnityEngine.Component) this), (Action<M0>) (isOn => this.SetColor(isOn ? this._color.RGBToHSV() : this._color.HSVToRGB())));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Slider>) this.sliders).Select<Slider, \u003C\u003E__AnonType32<Slider, int>>((Func<Slider, int, \u003C\u003E__AnonType32<Slider, int>>) ((p, index) => new \u003C\u003E__AnonType32<Slider, int>(p, index))).ToList<\u003C\u003E__AnonType32<Slider, int>>().ForEach((Action<\u003C\u003E__AnonType32<Slider, int>>) (p => ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) p.slider.get_onValueChanged()), (Action<M0>) (value =>
      {
        ((Color) ref this._color).set_Item(p.index, p.slider.get_value());
        this.SetColor(this._color);
      }))));
      if (!Object.op_Inequality((Object) this.sliderA, (Object) null))
        return;
      ObservableExtensions.Subscribe<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this.useAlpha, (UnityEngine.Component) this), (Action<M0>) new Action<bool>(((UnityEngine.Component) this.sliderA).get_gameObject().SetActive));
      ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.sliderA.get_onValueChanged()), (Action<M0>) (value =>
      {
        this._color.a = (__Null) (double) value;
        this.updateColorAction.Call<Color>(this.color);
      }));
    }
  }
}
