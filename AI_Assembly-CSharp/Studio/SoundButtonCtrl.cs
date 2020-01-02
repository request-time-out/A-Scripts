// Decompiled with JetBrains decompiler
// Type: Studio.SoundButtonCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class SoundButtonCtrl : MonoBehaviour
  {
    [SerializeField]
    private SoundButtonCtrl.CommonInfo[] ciRoot;
    private int select;

    public SoundButtonCtrl()
    {
      base.\u002Ector();
    }

    public void OnClickButton(int _idx)
    {
      this.select = this.select != _idx ? _idx : -1;
      for (int index = 0; index < this.ciRoot.Length; ++index)
        this.ciRoot[index].active = index == this.select;
    }

    private void Start()
    {
      this.select = -1;
    }

    [Serializable]
    private class CommonInfo
    {
      public GameObject obj;
      public Button button;

      public bool active
      {
        set
        {
          if (!Object.op_Implicit((Object) this.obj) || this.obj.get_activeSelf() == value)
            return;
          this.obj.SetActive(value);
          ((Graphic) ((Selectable) this.button).get_image()).set_color(!value ? Color.get_white() : Color.get_green());
        }
      }
    }
  }
}
