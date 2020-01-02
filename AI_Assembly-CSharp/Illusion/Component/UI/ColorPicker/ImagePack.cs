// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.ColorPicker.ImagePack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace Illusion.Component.UI.ColorPicker
{
  internal class ImagePack
  {
    public ImagePack(Image image)
    {
      this.rectTransform = ((Graphic) image).get_rectTransform();
      Rect rect1 = this.rectTransform.get_rect();
      int width = (int) ((Rect) ref rect1).get_width();
      Rect rect2 = this.rectTransform.get_rect();
      int height = (int) ((Rect) ref rect2).get_height();
      this.tex2D = new Texture2D(width, height);
      this.tex2D.Apply();
      image.set_sprite(Sprite.Create(this.tex2D, new Rect(0.0f, 0.0f, (float) ((Texture) this.tex2D).get_width(), (float) ((Texture) this.tex2D).get_height()), Vector2.get_zero()));
    }

    public RectTransform rectTransform { get; private set; }

    public Texture2D tex2D { get; private set; }

    public Vector2 size
    {
      get
      {
        return new Vector2((float) ((Texture) this.tex2D).get_width(), (float) ((Texture) this.tex2D).get_height());
      }
    }

    public bool isTex
    {
      get
      {
        return Object.op_Inequality((Object) this.tex2D, (Object) null);
      }
    }

    public void SetPixels(Color[] colors)
    {
      this.tex2D.SetPixels(colors);
      this.tex2D.Apply();
    }
  }
}
