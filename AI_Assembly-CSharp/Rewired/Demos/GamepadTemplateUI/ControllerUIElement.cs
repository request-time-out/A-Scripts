// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.ControllerUIElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
  [RequireComponent(typeof (Image))]
  public class ControllerUIElement : MonoBehaviour
  {
    [SerializeField]
    private Color _highlightColor;
    [SerializeField]
    private ControllerUIEffect _positiveUIEffect;
    [SerializeField]
    private ControllerUIEffect _negativeUIEffect;
    [SerializeField]
    private Text _label;
    [SerializeField]
    private Text _positiveLabel;
    [SerializeField]
    private Text _negativeLabel;
    [SerializeField]
    private ControllerUIElement[] _childElements;
    private Image _image;
    private Color _color;
    private Color _origColor;
    private bool _isActive;
    private float _highlightAmount;

    public ControllerUIElement()
    {
      base.\u002Ector();
    }

    private bool hasEffects
    {
      get
      {
        return Object.op_Inequality((Object) this._positiveUIEffect, (Object) null) || Object.op_Inequality((Object) this._negativeUIEffect, (Object) null);
      }
    }

    private void Awake()
    {
      this._image = (Image) ((Component) this).GetComponent<Image>();
      this._origColor = ((Graphic) this._image).get_color();
      this._color = this._origColor;
      this.ClearLabels();
    }

    public void Activate(float amount)
    {
      amount = Mathf.Clamp(amount, -1f, 1f);
      if (this.hasEffects)
      {
        if ((double) amount < 0.0 && Object.op_Inequality((Object) this._negativeUIEffect, (Object) null))
          this._negativeUIEffect.Activate(Mathf.Abs(amount));
        if ((double) amount > 0.0 && Object.op_Inequality((Object) this._positiveUIEffect, (Object) null))
          this._positiveUIEffect.Activate(Mathf.Abs(amount));
      }
      else
      {
        if (this._isActive && (double) amount == (double) this._highlightAmount)
          return;
        this._highlightAmount = amount;
        this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
      }
      this._isActive = true;
      this.RedrawImage();
      if (this._childElements.Length == 0)
        return;
      for (int index = 0; index < this._childElements.Length; ++index)
      {
        if (!Object.op_Equality((Object) this._childElements[index], (Object) null))
          this._childElements[index].Activate(amount);
      }
    }

    public void Deactivate()
    {
      if (!this._isActive)
        return;
      this._color = this._origColor;
      this._highlightAmount = 0.0f;
      if (Object.op_Inequality((Object) this._positiveUIEffect, (Object) null))
        this._positiveUIEffect.Deactivate();
      if (Object.op_Inequality((Object) this._negativeUIEffect, (Object) null))
        this._negativeUIEffect.Deactivate();
      this._isActive = false;
      this.RedrawImage();
      if (this._childElements.Length == 0)
        return;
      for (int index = 0; index < this._childElements.Length; ++index)
      {
        if (!Object.op_Equality((Object) this._childElements[index], (Object) null))
          this._childElements[index].Deactivate();
      }
    }

    public void SetLabel(string text, AxisRange labelType)
    {
      Text text1;
      switch ((int) labelType)
      {
        case 0:
          text1 = this._label;
          break;
        case 1:
          text1 = this._positiveLabel;
          break;
        case 2:
          text1 = this._negativeLabel;
          break;
        default:
          text1 = (Text) null;
          break;
      }
      if (Object.op_Inequality((Object) text1, (Object) null))
        text1.set_text(text);
      if (this._childElements.Length == 0)
        return;
      for (int index = 0; index < this._childElements.Length; ++index)
      {
        if (!Object.op_Equality((Object) this._childElements[index], (Object) null))
          this._childElements[index].SetLabel(text, labelType);
      }
    }

    public void ClearLabels()
    {
      if (Object.op_Inequality((Object) this._label, (Object) null))
        this._label.set_text(string.Empty);
      if (Object.op_Inequality((Object) this._positiveLabel, (Object) null))
        this._positiveLabel.set_text(string.Empty);
      if (Object.op_Inequality((Object) this._negativeLabel, (Object) null))
        this._negativeLabel.set_text(string.Empty);
      if (this._childElements.Length == 0)
        return;
      for (int index = 0; index < this._childElements.Length; ++index)
      {
        if (!Object.op_Equality((Object) this._childElements[index], (Object) null))
          this._childElements[index].ClearLabels();
      }
    }

    private void RedrawImage()
    {
      ((Graphic) this._image).set_color(this._color);
    }
  }
}
