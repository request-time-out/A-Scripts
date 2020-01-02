// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.PickerRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  public class PickerRect : MonoBehaviour
  {
    private float[] _values;
    [SerializeField]
    protected PickerRect.ModeReactiveProperty _mode;
    [NamedArray(typeof (PickerRect.Mode))]
    [SerializeField]
    private Toggle[] modeChangeToggles;
    [SerializeField]
    public Info info;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private RectTransform pointer;
    private ImagePack[] imgPack;

    public PickerRect()
    {
      base.\u002Ector();
    }

    public virtual Color ColorRGB
    {
      get
      {
        float[] rgb = this.RGB;
        return new Color(rgb[0], rgb[1], rgb[2]);
      }
      set
      {
        this.RGB = new float[3]
        {
          ((Color) ref value).get_Item(0),
          ((Color) ref value).get_Item(1),
          ((Color) ref value).get_Item(2)
        };
      }
    }

    public virtual float[] RGB
    {
      get
      {
        return new float[3]
        {
          this.Red,
          this.Green,
          this.Blue
        };
      }
      set
      {
        this.Red = value[0];
        this.Green = value[1];
        this.Blue = value[2];
      }
    }

    public HsvColor ColorHSV
    {
      get
      {
        float[] hsv = this.HSV;
        return new HsvColor(hsv[0], hsv[1], hsv[2]);
      }
      set
      {
        this.HSV = new float[3]
        {
          value[0],
          value[1],
          value[2]
        };
      }
    }

    public float[] HSV
    {
      get
      {
        return new float[3]
        {
          this.Hue,
          this.Saturation,
          this.Value
        };
      }
      set
      {
        this.Hue = value[0];
        this.Saturation = value[1];
        this.Value = value[2];
      }
    }

    public float Hue
    {
      get
      {
        return this._values[0];
      }
      set
      {
        this._values[0] = value;
      }
    }

    public float Saturation
    {
      get
      {
        return this._values[1];
      }
      set
      {
        this._values[1] = value;
      }
    }

    public float Value
    {
      get
      {
        return this._values[2];
      }
      set
      {
        this._values[2] = value;
      }
    }

    public float Red
    {
      get
      {
        return this._values[3];
      }
      set
      {
        this._values[3] = value;
      }
    }

    public float Green
    {
      get
      {
        return this._values[4];
      }
      set
      {
        this._values[4] = value;
      }
    }

    public float Blue
    {
      get
      {
        return this._values[5];
      }
      set
      {
        this._values[5] = value;
      }
    }

    public event Action<Color> updateColorAction;

    public PickerRect.Mode mode
    {
      get
      {
        return this._mode.get_Value();
      }
      set
      {
        this._mode.set_Value(value);
      }
    }

    private float[] RateHSV
    {
      get
      {
        return new float[3]
        {
          Mathf.InverseLerp(0.0f, 360f, this.Hue),
          this.Saturation,
          this.Value
        };
      }
    }

    public void ChangeRectColor()
    {
      ImagePack imagePack = this.imgPack[0];
      if (imagePack == null || !imagePack.isTex)
        return;
      int index1 = (int) this.mode % 3;
      Vector2 size = imagePack.size;
      int x = (int) size.x;
      int y = (int) size.y;
      Color[] colors = new Color[y * x];
      switch (this.mode)
      {
        case PickerRect.Mode.Hue:
        case PickerRect.Mode.Saturation:
        case PickerRect.Mode.Value:
          float[] rateHsv = this.RateHSV;
          int[,] numArray1 = new int[3, 3]
          {
            {
              1,
              2,
              0
            },
            {
              0,
              2,
              1
            },
            {
              0,
              1,
              2
            }
          };
          for (int index2 = 0; index2 < y; ++index2)
          {
            for (int index3 = 0; index3 < x; ++index3)
            {
              rateHsv[numArray1[index1, 0]] = Mathf.InverseLerp(0.0f, (float) size.x, (float) index3);
              rateHsv[numArray1[index1, 1]] = Mathf.InverseLerp(0.0f, (float) size.y, (float) index2);
              colors[index2 * x + index3] = HsvColor.ToRgb(360f * rateHsv[0], rateHsv[1], rateHsv[2]);
            }
          }
          break;
        case PickerRect.Mode.Red:
        case PickerRect.Mode.Green:
        case PickerRect.Mode.Blue:
          float[] rgb = this.RGB;
          int[,] numArray2 = new int[3, 3]
          {
            {
              2,
              1,
              0
            },
            {
              2,
              0,
              1
            },
            {
              0,
              1,
              2
            }
          };
          for (int index2 = 0; index2 < y; ++index2)
          {
            for (int index3 = 0; index3 < x; ++index3)
            {
              rgb[numArray2[index1, 0]] = Mathf.InverseLerp(0.0f, (float) size.x, (float) index3);
              rgb[numArray2[index1, 1]] = Mathf.InverseLerp(0.0f, (float) size.y, (float) index2);
              colors[index2 * x + index3] = new Color(rgb[0], rgb[1], rgb[2]);
            }
          }
          break;
      }
      imagePack.SetPixels(colors);
    }

    public void ChangeSliderColor()
    {
      ImagePack imagePack = this.imgPack[1];
      if (imagePack == null || !imagePack.isTex)
        return;
      int index1 = (int) this.mode % 3;
      Vector2 size = imagePack.size;
      int x = (int) size.x;
      int y = (int) size.y;
      Color[] colors = new Color[y * x];
      switch (this.mode)
      {
        case PickerRect.Mode.Hue:
        case PickerRect.Mode.Saturation:
        case PickerRect.Mode.Value:
          float[] rateHsv = this.RateHSV;
          if (this.mode == PickerRect.Mode.Hue)
          {
            rateHsv[1] = 1f;
            rateHsv[2] = 1f;
          }
          for (int index2 = 0; index2 < y; ++index2)
          {
            for (int index3 = 0; index3 < x; ++index3)
            {
              rateHsv[index1] = Mathf.InverseLerp(0.0f, (float) size.y, (float) index2);
              colors[index2 * x + index3] = HsvColor.ToRgb(rateHsv[0] * 360f, rateHsv[1], rateHsv[2]);
            }
          }
          break;
        case PickerRect.Mode.Red:
        case PickerRect.Mode.Green:
        case PickerRect.Mode.Blue:
          float[] rgb = this.RGB;
          for (int index2 = 0; index2 < y; ++index2)
          {
            for (int index3 = 0; index3 < x; ++index3)
            {
              rgb[index1] = Mathf.InverseLerp(0.0f, (float) size.y, (float) index2);
              colors[index2 * x + index3] = new Color(rgb[0], rgb[1], rgb[2]);
            }
          }
          break;
      }
      imagePack.SetPixels(colors);
    }

    public void CalcRectPointer()
    {
      if (Object.op_Equality((Object) this.pointer, (Object) null))
        return;
      Rect rect = this.imgPack[0].rectTransform.get_rect();
      Action<float[], int, int> action = (Action<float[], int, int>) ((val, x, y) => this.pointer.set_anchoredPosition(new Vector2(((Rect) ref rect).get_width() * val[x], ((Rect) ref rect).get_height() * val[y])));
      switch (this.mode)
      {
        case PickerRect.Mode.Hue:
          action(this.RateHSV, 1, 2);
          break;
        case PickerRect.Mode.Saturation:
          action(this.RateHSV, 0, 2);
          break;
        case PickerRect.Mode.Value:
          action(this.RateHSV, 0, 1);
          break;
        case PickerRect.Mode.Red:
          action(this.RGB, 2, 1);
          break;
        case PickerRect.Mode.Green:
          action(this.RGB, 2, 0);
          break;
        case PickerRect.Mode.Blue:
          action(this.RGB, 0, 1);
          break;
      }
    }

    public void CalcSliderValue()
    {
      if (Object.op_Equality((Object) this.slider, (Object) null))
        return;
      switch (this.mode)
      {
        case PickerRect.Mode.Hue:
        case PickerRect.Mode.Saturation:
        case PickerRect.Mode.Value:
          this.slider.set_value(this.RateHSV[(int) this.mode]);
          break;
        case PickerRect.Mode.Red:
        case PickerRect.Mode.Green:
        case PickerRect.Mode.Blue:
          this.slider.set_value(this.RGB[(int) this.mode % 3]);
          break;
      }
    }

    public virtual void SetColor(HsvColor hsv, PickerRect.Control ctrlType)
    {
      this.ColorHSV = hsv;
      this.ColorRGB = HsvColor.ToRgb(hsv);
      this.SetColor(ctrlType);
    }

    public virtual void SetColor(Color rgb, PickerRect.Control ctrlType)
    {
      this.ColorRGB = rgb;
      this.ColorHSV = HsvColor.FromRgb(rgb);
      this.SetColor(ctrlType);
    }

    public virtual void SetColor(PickerRect.Control ctrlType)
    {
      switch (ctrlType)
      {
        case PickerRect.Control.Rect:
          this.ChangeSliderColor();
          break;
        case PickerRect.Control.Slider:
          this.ChangeRectColor();
          break;
      }
      this.updateColorAction.Call<Color>(this.ColorRGB);
    }

    public virtual void SetColor(Color color)
    {
      this.ColorRGB = color;
      this.ColorHSV = HsvColor.FromRgb(color);
      this.CalcRectPointer();
      this.CalcSliderValue();
    }

    protected void Awake()
    {
      this.ColorHSV = new HsvColor(0.0f, 0.0f, 1f);
      this.ColorRGB = Color.get_white();
      Image[] imageArray = new Image[2]
      {
        ((UnityEngine.Component) this.info).GetOrAddComponent<Image>(),
        ((UnityEngine.Component) this.slider).GetOrAddComponent<Image>()
      };
      this.imgPack = new ImagePack[imageArray.Length];
      for (int index = 0; index < this.imgPack.Length; ++index)
        this.imgPack[index] = new ImagePack(imageArray[index]);
    }

    protected virtual void Start()
    {
      ObservableExtensions.Subscribe<PickerRect.Mode>(Observable.TakeUntilDestroy<PickerRect.Mode>((IObservable<M0>) this._mode, (UnityEngine.Component) this), (Action<M0>) (_ =>
      {
        this.CalcRectPointer();
        this.CalcSliderValue();
        this.ChangeRectColor();
        this.ChangeSliderColor();
      }));
      if (((IEnumerable<Toggle>) this.modeChangeToggles).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.modeChangeToggles).Select<Toggle, \u003C\u003E__AnonType31<Toggle, PickerRect.Mode>>((Func<Toggle, int, \u003C\u003E__AnonType31<Toggle, PickerRect.Mode>>) ((toggle, index) => new \u003C\u003E__AnonType31<Toggle, PickerRect.Mode>(toggle, (PickerRect.Mode) index))).Where<\u003C\u003E__AnonType31<Toggle, PickerRect.Mode>>((Func<\u003C\u003E__AnonType31<Toggle, PickerRect.Mode>, bool>) (item => Object.op_Inequality((Object) item.toggle, (Object) null))).ToList<\u003C\u003E__AnonType31<Toggle, PickerRect.Mode>>().ForEach((Action<\u003C\u003E__AnonType31<Toggle, PickerRect.Mode>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.toggle), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ => this.mode = item.mode))));
      }
      if (Object.op_Inequality((Object) this.slider, (Object) null))
      {
        PickerRect.Control ctrl = PickerRect.Control.Slider;
        Action<Func<HsvColor, HsvColor>> hsv = (Action<Func<HsvColor, HsvColor>>) (func => this.SetColor(func(this.ColorHSV), ctrl));
        Action<Func<Color, Color>> rgb = (Action<Func<Color, Color>>) (func => this.SetColor(func(this.ColorRGB), ctrl));
        ObservableExtensions.Subscribe<float>(UnityEventExtensions.AsObservable<float>((UnityEvent<M0>) this.slider.get_onValueChanged()), (Action<M0>) (value =>
        {
          switch (this.mode)
          {
            case PickerRect.Mode.Hue:
              hsv((Func<HsvColor, HsvColor>) (c =>
              {
                c.H = value * 360f;
                return c;
              }));
              break;
            case PickerRect.Mode.Saturation:
              hsv((Func<HsvColor, HsvColor>) (c =>
              {
                c.S = value;
                return c;
              }));
              break;
            case PickerRect.Mode.Value:
              hsv((Func<HsvColor, HsvColor>) (c =>
              {
                c.V = value;
                return c;
              }));
              break;
            case PickerRect.Mode.Red:
              rgb((Func<Color, Color>) (c =>
              {
                c.r = (__Null) (double) value;
                return c;
              }));
              break;
            case PickerRect.Mode.Green:
              rgb((Func<Color, Color>) (c =>
              {
                c.g = (__Null) (double) value;
                return c;
              }));
              break;
            case PickerRect.Mode.Blue:
              rgb((Func<Color, Color>) (c =>
              {
                c.b = (__Null) (double) value;
                return c;
              }));
              break;
          }
        }));
      }
      ObservableExtensions.Subscribe<Vector2>(Observable.DistinctUntilChanged<Vector2>((IObservable<M0>) Observable.Select<Unit, Vector2>(Observable.Where<Unit>(Observable.Where<Unit>(Observable.SkipWhile<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((UnityEngine.Component) this), (Func<M0, bool>) (_ => Object.op_Equality((Object) this.info, (Object) null) || Object.op_Equality((Object) this.pointer, (Object) null))), (Func<M0, bool>) (_ => ((Behaviour) this).get_enabled())), (Func<M0, bool>) (_ => this.info.isOn)), (Func<M0, M1>) (_ => this.info.imagePos))), (Action<M0>) (pos =>
      {
        this.pointer.set_anchoredPosition(pos);
        Vector2 imageRate = this.info.imageRate;
        PickerRect.Control ctrlType = PickerRect.Control.Rect;
        switch (this.mode)
        {
          case PickerRect.Mode.Hue:
            HsvColor colorHsv1 = this.ColorHSV;
            colorHsv1.S = (float) imageRate.x;
            colorHsv1.V = (float) imageRate.y;
            this.SetColor(colorHsv1, ctrlType);
            break;
          case PickerRect.Mode.Saturation:
            HsvColor colorHsv2 = this.ColorHSV;
            colorHsv2.H = (float) (imageRate.x * 360.0);
            colorHsv2.V = (float) imageRate.y;
            this.SetColor(colorHsv2, ctrlType);
            break;
          case PickerRect.Mode.Value:
            HsvColor colorHsv3 = this.ColorHSV;
            colorHsv3.H = (float) (imageRate.x * 360.0);
            colorHsv3.S = (float) imageRate.y;
            this.SetColor(colorHsv3, ctrlType);
            break;
          case PickerRect.Mode.Red:
            Color colorRgb1 = this.ColorRGB;
            colorRgb1.b = imageRate.x;
            colorRgb1.g = imageRate.y;
            this.SetColor(colorRgb1, ctrlType);
            break;
          case PickerRect.Mode.Green:
            Color colorRgb2 = this.ColorRGB;
            colorRgb2.b = imageRate.x;
            colorRgb2.r = imageRate.y;
            this.SetColor(colorRgb2, ctrlType);
            break;
          case PickerRect.Mode.Blue:
            Color colorRgb3 = this.ColorRGB;
            colorRgb3.r = imageRate.x;
            colorRgb3.g = imageRate.y;
            this.SetColor(colorRgb3, ctrlType);
            break;
        }
      }));
    }

    [ContextMenu("Setup")]
    protected void Setup()
    {
      this.modeChangeToggles = (Toggle[]) ((UnityEngine.Component) this).GetComponentsInChildren<Toggle>();
      this.info = (Info) ((UnityEngine.Component) this).GetComponentInChildren<Info>();
      if (((UnityEngine.Component) this.info).get_transform().get_childCount() == 0)
        return;
      this.pointer = (RectTransform) ((UnityEngine.Component) ((UnityEngine.Component) this.info).get_transform().GetChild(0)).GetComponentInChildren<RectTransform>();
    }

    public enum Mode
    {
      Hue,
      Saturation,
      Value,
      Red,
      Green,
      Blue,
    }

    public enum Control
    {
      None,
      Rect,
      Slider,
    }

    [Serializable]
    public class ModeReactiveProperty : ReactiveProperty<PickerRect.Mode>
    {
      public ModeReactiveProperty()
      {
        base.\u002Ector();
      }

      public ModeReactiveProperty(PickerRect.Mode initialValue)
      {
        base.\u002Ector(initialValue);
      }
    }
  }
}
