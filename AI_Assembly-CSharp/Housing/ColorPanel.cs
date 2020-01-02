// Decompiled with JetBrains decompiler
// Type: Housing.ColorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Component.UI.ColorPicker;
using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  public class ColorPanel : MonoBehaviour
  {
    [SerializeField]
    [Tooltip("このキャンバスグループ")]
    private CanvasGroup cgWindow;
    [SerializeField]
    [Tooltip("閉じるボタン")]
    private ButtonEx btnClose;
    [SerializeField]
    [Tooltip("サンプルカラーScript")]
    private UI_SampleColor sampleColor;
    [SerializeField]
    [Tooltip("PickerのRect")]
    [Header("ピッカー関係")]
    private PickerRectA cmpPickerRect;
    [SerializeField]
    [Tooltip("PickerのSlider")]
    private PickerSliderInput cmpPickerSliderI;
    public Action onClose;

    public ColorPanel()
    {
      base.\u002Ector();
    }

    public bool isOpen
    {
      get
      {
        return (double) this.cgWindow.get_alpha() != 0.0;
      }
    }

    private bool Enable
    {
      get
      {
        return this.isOpen;
      }
      set
      {
        if (this.cgWindow == null)
          return;
        this.cgWindow.Enable(value, false);
      }
    }

    public void Setup(Color color, Action<Color> _actUpdateColor, bool _alpha = false)
    {
      this.sampleColor.SafeProc<UI_SampleColor>((Action<UI_SampleColor>) (_sc =>
      {
        _sc.SetColor(color);
        _sc.actUpdateColor = _actUpdateColor;
      }));
      this.Enable = true;
      ((ReactiveProperty<bool>) this.cmpPickerRect.isAlpha).set_Value(_alpha);
      ((ReactiveProperty<bool>) this.cmpPickerSliderI.useAlpha).set_Value(_alpha);
      this.btnClose.ClearState();
    }

    public void SetColor(Color color)
    {
      if (this.sampleColor == null)
        return;
      this.sampleColor.SetColor(color);
    }

    public void Close()
    {
      this.sampleColor.SafeProc<UI_SampleColor>((Action<UI_SampleColor>) (_sc => _sc.actUpdateColor = (Action<Color>) null));
      this.Enable = false;
      if (this.onClose == null)
        return;
      this.onClose();
    }

    private void Start()
    {
      if (this.btnClose == null)
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable((Button) this.btnClose), (Action<M0>) (_ => this.Close()));
    }
  }
}
