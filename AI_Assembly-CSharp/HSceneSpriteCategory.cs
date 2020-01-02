// Decompiled with JetBrains decompiler
// Type: HSceneSpriteCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSceneSpriteCategory : MonoBehaviour
{
  public List<Button> lstButton;

  public HSceneSpriteCategory()
  {
    base.\u002Ector();
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

  public bool GetEnable(int _array)
  {
    return this.lstButton.Count > _array && ((Selectable) this.lstButton[_array]).get_interactable();
  }

  public int GetAllEnable()
  {
    int num = 0;
    for (int index = 0; index < this.lstButton.Count; ++index)
    {
      if (((Selectable) this.lstButton[index]).get_interactable())
        ++num;
    }
    if (num == this.lstButton.Count)
      return 1;
    return num == 0 ? 0 : 2;
  }

  public virtual void SetActive(bool _active, int _array = -1)
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

  public bool[] GetActiveButton()
  {
    bool[] flagArray = new bool[this.lstButton.Count];
    for (int index = 0; index < this.lstButton.Count; ++index)
      flagArray[index] = ((Component) this.lstButton[index]).get_gameObject().get_activeSelf();
    return flagArray;
  }
}
