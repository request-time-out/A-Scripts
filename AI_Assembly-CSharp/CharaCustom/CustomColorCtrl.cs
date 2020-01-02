// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomColorCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Component.UI.ColorPicker;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomColorCtrl : MonoBehaviour
  {
    [Tooltip("このキャンバスグループ")]
    [SerializeField]
    private CanvasGroup cgWindow;
    [Tooltip("閉じるボタン")]
    [SerializeField]
    private Button btnClose;
    [Tooltip("サンプルカラーScript")]
    [SerializeField]
    private UI_SampleColor sampleColor;
    [Tooltip("PickerのRect")]
    [SerializeField]
    private PickerRectA cmpPickerRect;
    [Tooltip("PickerのSlider")]
    [SerializeField]
    private PickerSliderInput cmpPickerSliderI;
    public CustomColorSet linkCustomColorSet;

    public CustomColorCtrl()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    public bool isOpen
    {
      get
      {
        return (double) this.cgWindow.get_alpha() != 0.0;
      }
    }

    public void Setup(
      CustomColorSet ccSet,
      Color color,
      Action<Color> _actUpdateColor,
      bool _useAlpha)
    {
      this.linkCustomColorSet = ccSet;
      if (Object.op_Inequality((Object) null, (Object) this.sampleColor))
      {
        this.sampleColor.SetColor(color);
        this.sampleColor.actUpdateColor = _actUpdateColor;
      }
      this.customBase.customCtrl.showColorCvs = true;
      ((ReactiveProperty<bool>) this.cmpPickerRect.isAlpha).set_Value(_useAlpha);
      ((ReactiveProperty<bool>) this.cmpPickerSliderI.useAlpha).set_Value(_useAlpha);
    }

    public void EnableAlpha(bool enable)
    {
      ((ReactiveProperty<bool>) this.cmpPickerRect.isAlpha).set_Value(enable);
      ((ReactiveProperty<bool>) this.cmpPickerSliderI.useAlpha).set_Value(enable);
    }

    public void SetColor(CustomColorSet ccSet, Color color)
    {
      if (Object.op_Inequality((Object) ccSet, (Object) this.linkCustomColorSet) || !Object.op_Inequality((Object) null, (Object) this.sampleColor))
        return;
      this.sampleColor.SetColor(color);
    }

    public void Close()
    {
      if (Object.op_Inequality((Object) null, (Object) this.sampleColor))
        this.sampleColor.actUpdateColor = (Action<Color>) null;
      this.customBase.customCtrl.showColorCvs = false;
      this.linkCustomColorSet = (CustomColorSet) null;
    }

    private void Start()
    {
      if (!Object.op_Implicit((Object) this.btnClose))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (_ => this.Close()));
    }
  }
}
