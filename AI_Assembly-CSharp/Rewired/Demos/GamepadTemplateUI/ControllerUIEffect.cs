// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.ControllerUIEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
  [RequireComponent(typeof (Image))]
  public class ControllerUIEffect : MonoBehaviour
  {
    [SerializeField]
    private Color _highlightColor;
    private Image _image;
    private Color _color;
    private Color _origColor;
    private bool _isActive;
    private float _highlightAmount;

    public ControllerUIEffect()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this._image = (Image) ((Component) this).GetComponent<Image>();
      this._origColor = ((Graphic) this._image).get_color();
      this._color = this._origColor;
    }

    public void Activate(float amount)
    {
      amount = Mathf.Clamp01(amount);
      if (this._isActive && (double) amount == (double) this._highlightAmount)
        return;
      this._highlightAmount = amount;
      this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
      this._isActive = true;
      this.RedrawImage();
    }

    public void Deactivate()
    {
      if (!this._isActive)
        return;
      this._color = this._origColor;
      this._highlightAmount = 0.0f;
      this._isActive = false;
      this.RedrawImage();
    }

    private void RedrawImage()
    {
      ((Graphic) this._image).set_color(this._color);
      ((Behaviour) this._image).set_enabled(this._isActive);
    }
  }
}
