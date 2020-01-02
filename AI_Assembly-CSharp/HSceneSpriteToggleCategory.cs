// Decompiled with JetBrains decompiler
// Type: HSceneSpriteToggleCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HSceneSpriteToggleCategory : MonoBehaviour
{
  public List<Toggle> lstToggle;

  public HSceneSpriteToggleCategory()
  {
    base.\u002Ector();
  }

  public int GetToggleNum()
  {
    return this.lstToggle.Count;
  }

  public void SetEnable(bool _enable, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstToggle.Count; ++index)
      {
        if (((Selectable) this.lstToggle[index]).get_interactable() != _enable)
          ((Selectable) this.lstToggle[index]).set_interactable(_enable);
      }
    }
    else
    {
      if (this.lstToggle.Count <= _array || ((Selectable) this.lstToggle[_array]).get_interactable() == _enable)
        return;
      ((Selectable) this.lstToggle[_array]).set_interactable(_enable);
    }
  }

  public bool GetEnable(int _array)
  {
    return this.lstToggle.Count > _array && ((Selectable) this.lstToggle[_array]).get_interactable();
  }

  public int GetAllEnable()
  {
    int num = 0;
    for (int index = 0; index < this.lstToggle.Count; ++index)
    {
      if (((Selectable) this.lstToggle[index]).get_interactable())
        ++num;
    }
    if (num == this.lstToggle.Count)
      return 1;
    return num == 0 ? 0 : 2;
  }

  public void SetActive(bool _active, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstToggle.Count; ++index)
      {
        if (((Behaviour) this.lstToggle[index]).get_isActiveAndEnabled() != _active)
          ((Component) this.lstToggle[index]).get_gameObject().SetActive(_active);
      }
    }
    else
    {
      if (this.lstToggle.Count <= _array || ((Behaviour) this.lstToggle[_array]).get_isActiveAndEnabled() == _active)
        return;
      ((Component) this.lstToggle[_array]).get_gameObject().SetActive(_active);
    }
  }

  public bool GetActive(int _array)
  {
    return this.lstToggle.Count > _array && ((Behaviour) this.lstToggle[_array]).get_isActiveAndEnabled();
  }

  public void SetCheck(bool _check, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstToggle.Count; ++index)
        this.lstToggle[index].set_isOn(_check);
    }
    else
    {
      if (this.lstToggle.Count <= _array)
        return;
      this.lstToggle[_array].set_isOn(_check);
    }
  }
}
