// Decompiled with JetBrains decompiler
// Type: SceneFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class SceneFade : SimpleFade
{
  private int sortingOrder = (int) short.MinValue;
  private Canvas canvas;
  private RectTransform canvasRt;
  private Image image;

  protected override void Awake()
  {
    base.Awake();
    this.image = (Image) ((Component) this).GetComponent<Image>();
    this.canvas = ((Graphic) this.image).get_canvas();
    this.canvasRt = ((Component) this.canvas).get_transform() as RectTransform;
    this.sortingOrder = this.canvas.get_sortingOrder();
  }

  protected override void Update()
  {
    base.Update();
    ((Graphic) this.image).set_color(this._Color);
    RectTransform rectTransform = ((Graphic) this.image).get_rectTransform();
    Rect rect1 = this.canvasRt.get_rect();
    double width = (double) ((Rect) ref rect1).get_width();
    Rect rect2 = this.canvasRt.get_rect();
    double height = (double) ((Rect) ref rect2).get_height();
    Vector2 vector2 = new Vector2((float) width, (float) height);
    rectTransform.set_sizeDelta(vector2);
    int sortingOrder = this.canvas.get_sortingOrder();
    this.canvas.set_sortingOrder(!this.IsFadeNow ? (int) short.MinValue : this.sortingOrder);
    if (sortingOrder == this.canvas.get_sortingOrder())
      return;
    ((Behaviour) this.canvas).set_enabled(false);
    ((Behaviour) this.canvas).set_enabled(true);
  }

  protected override void OnGUI()
  {
  }
}
