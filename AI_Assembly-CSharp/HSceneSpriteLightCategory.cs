// Decompiled with JetBrains decompiler
// Type: HSceneSpriteLightCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSceneSpriteLightCategory : MonoBehaviour
{
  public List<Slider> lstSlider;
  public List<Button> lstButton;

  public HSceneSpriteLightCategory()
  {
    base.\u002Ector();
  }

  public void SetValue(float _value, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstSlider.Count; ++index)
        this.lstSlider[index].set_value(_value);
    }
    else
    {
      if (this.lstSlider.Count <= _array)
        return;
      this.lstSlider[_array].set_value(_value);
    }
  }

  public float GetValue(int _array)
  {
    return this.lstSlider.Count <= _array ? 0.0f : this.lstSlider[_array].get_value();
  }

  public void SetEnable(bool _enable, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstButton.Count; ++index)
      {
        if (((Selectable) this.lstButton[index]).get_interactable() != _enable)
          ((Selectable) this.lstButton[index]).set_interactable(_enable);
      }
    }
    else
    {
      if (this.lstButton.Count <= _array || ((Selectable) this.lstButton[_array]).get_interactable() == _enable)
        return;
      ((Selectable) this.lstButton[_array]).set_interactable(_enable);
    }
  }

  public void SetActive(bool _active, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstButton.Count; ++index)
      {
        if (((Behaviour) this.lstButton[index]).get_isActiveAndEnabled() != _active)
          ((Component) this.lstButton[index]).get_gameObject().SetActive(_active);
      }
    }
    else
    {
      if (this.lstButton.Count <= _array || ((Behaviour) this.lstButton[_array]).get_isActiveAndEnabled() == _active)
        return;
      ((Component) this.lstButton[_array]).get_gameObject().SetActive(_active);
    }
  }
}
