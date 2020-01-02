// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.SpriteChangeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Component.UI
{
  public class SpriteChangeCtrl : MonoBehaviour
  {
    public Sprite[] sprites;
    private Image _image;

    public SpriteChangeCtrl()
    {
      base.\u002Ector();
    }

    public Image image
    {
      get
      {
        return this._image;
      }
    }

    private void Awake()
    {
      this._image = (Image) ((UnityEngine.Component) this).GetComponent<Image>();
    }

    public void OnChangeValue(int _num)
    {
      if (Object.op_Equality((Object) this._image, (Object) null) || this.sprites.Length <= _num)
        return;
      bool flag = _num >= 0;
      ((Behaviour) this._image).set_enabled(flag);
      if (!flag)
        return;
      this._image.set_sprite(this.sprites[_num]);
    }

    public int GetCount()
    {
      return this.sprites.Length;
    }

    public int GetVisibleNumber()
    {
      if (Object.op_Equality((Object) this._image, (Object) null))
        return -1;
      for (int index = 0; index < this.sprites.Length; ++index)
      {
        if (Object.op_Equality((Object) this._image.get_sprite(), (Object) this.sprites[index]))
          return index;
      }
      return 0;
    }
  }
}
