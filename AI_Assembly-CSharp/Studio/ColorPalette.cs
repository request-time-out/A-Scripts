// Decompiled with JetBrains decompiler
// Type: Studio.ColorPalette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Component.UI.ColorPicker;
using Illusion.Extensions;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class ColorPalette : Singleton<ColorPalette>
  {
    private BoolReactiveProperty _visible = new BoolReactiveProperty(false);
    private bool _outsideVisible = true;
    [SerializeField]
    private Canvas canvas;
    [Tooltip("このキャンバスグループ")]
    [SerializeField]
    private CanvasGroup cgWindow;
    [Tooltip("ウィンドウタイトル")]
    [SerializeField]
    private TextMeshProUGUI textWinTitle;
    [Tooltip("閉じるボタン")]
    [SerializeField]
    private Button btnClose;
    [Tooltip("サンプルカラーScript")]
    [SerializeField]
    private SampleColor sampleColor;
    [Tooltip("PickerのRect")]
    [SerializeField]
    private PickerRectA cmpPickerRect;
    [Tooltip("PickerのSlider")]
    [SerializeField]
    private PickerSliderInput cmpPickerSliderI;

    public bool isOpen
    {
      get
      {
        return (double) this.cgWindow.get_alpha() != 0.0;
      }
    }

    public bool visible
    {
      get
      {
        return ((ReactiveProperty<bool>) this._visible).get_Value();
      }
      set
      {
        ((ReactiveProperty<bool>) this._visible).set_Value(value);
      }
    }

    public bool outsideVisible
    {
      set
      {
        this._outsideVisible = value;
        if (!Object.op_Implicit((Object) this.cgWindow))
          return;
        this.cgWindow.Enable(((ReactiveProperty<bool>) this._visible).get_Value() && this._outsideVisible, false);
      }
    }

    public void Setup(string winTitle, Color color, Action<Color> _actUpdateColor, bool _useAlpha)
    {
      if (Object.op_Implicit((Object) this.textWinTitle) && !winTitle.IsNullOrEmpty())
        ((TMP_Text) this.textWinTitle).set_text(winTitle);
      if (Object.op_Inequality((Object) null, (Object) this.sampleColor))
      {
        this.sampleColor.SetColor(color);
        this.sampleColor.actUpdateColor = _actUpdateColor;
      }
      this.visible = true;
      ((ReactiveProperty<bool>) this.cmpPickerRect.isAlpha).set_Value(_useAlpha);
      ((ReactiveProperty<bool>) this.cmpPickerSliderI.useAlpha).set_Value(_useAlpha);
    }

    public void Close()
    {
      if (Object.op_Implicit((Object) this.textWinTitle))
        ((TMP_Text) this.textWinTitle).set_text(string.Empty);
      if (Object.op_Inequality((Object) null, (Object) this.sampleColor))
        this.sampleColor.actUpdateColor = (Action<Color>) null;
      if (!Object.op_Implicit((Object) this.cgWindow))
        return;
      this.cgWindow.Enable(false, false);
    }

    public bool Check(string _text)
    {
      return !_text.IsNullOrEmpty() && ((TMP_Text) this.textWinTitle).get_text() == _text;
    }

    protected override void Awake()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._visible, (Action<M0>) (b =>
      {
        if (Object.op_Implicit((Object) this.cgWindow))
          this.cgWindow.Enable(b && this._outsideVisible, false);
        if (!b)
          this.Close();
        if (!this.isOpen)
          return;
        SortCanvas.select = this.canvas;
      }));
      if (Object.op_Implicit((Object) this.btnClose))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (_ => this.Close()));
      this.Close();
    }
  }
}
