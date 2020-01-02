// Decompiled with JetBrains decompiler
// Type: CharaCustom.ObjectCategoryBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace CharaCustom
{
  public class ObjectCategoryBehaviour : MonoBehaviour
  {
    public List<GameObject> lstObj;

    public ObjectCategoryBehaviour()
    {
      base.\u002Ector();
    }

    public GameObject GetObject(int _array)
    {
      return this.lstObj.Count <= _array ? (GameObject) null : this.lstObj[_array];
    }

    public int GetAllEnable()
    {
      int num = 0;
      for (int index = 0; index < this.lstObj.Count; ++index)
      {
        if (!Object.op_Equality((Object) this.lstObj[index], (Object) null) && this.lstObj[index].get_activeSelf())
          ++num;
      }
      if (num == this.lstObj.Count)
        return 1;
      return num == 0 ? 0 : 2;
    }

    public int GetCount()
    {
      return this.lstObj.Count;
    }

    public bool GetActive(int _array)
    {
      return this.lstObj.Count > _array && !Object.op_Equality((Object) this.lstObj[_array], (Object) null) && this.lstObj[_array].get_activeSelf();
    }

    public void SetActive(bool _active, int _array = -1)
    {
      if (_array < 0)
      {
        for (int index = 0; index < this.lstObj.Count; ++index)
        {
          if (!Object.op_Equality((Object) this.lstObj[index], (Object) null) && this.lstObj[index].get_activeSelf() != _active)
            this.lstObj[index].get_gameObject().SetActive(_active);
        }
      }
      else
      {
        if (this.lstObj.Count <= _array || !Object.op_Implicit((Object) this.lstObj[_array]) || this.lstObj[_array].get_activeSelf() == _active)
          return;
        this.lstObj[_array].get_gameObject().SetActive(_active);
      }
    }

    public void SetActiveToggle(int _array)
    {
      for (int index = 0; index < this.lstObj.Count; ++index)
      {
        if (!Object.op_Equality((Object) this.lstObj[index], (Object) null))
          this.lstObj[index].get_gameObject().SetActive(_array == index);
      }
    }

    public bool IsEmpty(int _array)
    {
      return this.lstObj.Count <= _array || Object.op_Equality((Object) this.lstObj[_array], (Object) null);
    }
  }
}
