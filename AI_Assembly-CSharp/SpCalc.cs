// Decompiled with JetBrains decompiler
// Type: SpCalc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.SetUtility;
using UnityEngine;

public class SpCalc : MonoBehaviour
{
  public Vector2 Pos;
  public Vector2 Scale;
  public byte CorrectX;
  public byte CorrectY;

  public SpCalc()
  {
    base.\u002Ector();
  }

  private Vector2 GetPivotInSprite(Sprite sprite)
  {
    Vector2 vector2 = (Vector2) null;
    if (Object.op_Inequality((Object) null, (Object) sprite))
    {
      Bounds bounds1 = sprite.get_bounds();
      if (((Bounds) ref bounds1).get_size().x != 0.0)
      {
        ref Vector2 local = ref vector2;
        Bounds bounds2 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null x1 = ((Bounds) ref bounds2).get_center().x;
        Bounds bounds3 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null x2 = ((Bounds) ref bounds3).get_size().x;
        double num = 0.5 - x1 / x2;
        local.x = (__Null) num;
      }
      Bounds bounds4 = sprite.get_bounds();
      if (((Bounds) ref bounds4).get_size().y != 0.0)
      {
        ref Vector2 local = ref vector2;
        Bounds bounds2 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null y1 = ((Bounds) ref bounds2).get_center().y;
        Bounds bounds3 = sprite.get_bounds();
        // ISSUE: variable of the null type
        __Null y2 = ((Bounds) ref bounds3).get_size().y;
        double num = 0.5 - y1 / y2;
        local.y = (__Null) num;
      }
    }
    return vector2;
  }

  private void Update()
  {
    this.Calc();
  }

  public void Calc()
  {
    Transform parent = ((Component) this).get_transform().get_parent();
    if (Object.op_Equality((Object) null, (Object) parent))
      return;
    SpRoot component1 = (SpRoot) ((Component) parent).GetComponent<SpRoot>();
    if (Object.op_Equality((Object) null, (Object) component1))
      return;
    SpriteRenderer component2 = (SpriteRenderer) ((Component) ((Component) this).get_gameObject().get_transform()).GetComponent<SpriteRenderer>();
    if (Object.op_Equality((Object) null, (Object) component2) || Object.op_Equality((Object) null, (Object) component2.get_sprite()))
      return;
    float baseScreenWidth = component1.baseScreenWidth;
    float baseScreenHeight = component1.baseScreenHeight;
    float spriteRate = component1.GetSpriteRate();
    float spriteCorrectY = component1.GetSpriteCorrectY();
    Vector2 pivotInSprite = this.GetPivotInSprite(component2.get_sprite());
    float x1 = (float) pivotInSprite.x;
    float num1 = (float) (1.0 - pivotInSprite.y);
    // ISSUE: variable of the null type
    __Null x2 = this.Pos.x;
    double num2 = (double) baseScreenWidth * 0.5;
    Rect rect1 = component2.get_sprite().get_rect();
    double num3 = (double) ((Rect) ref rect1).get_width() * (double) x1;
    double num4 = num2 - num3;
    float x3 = (float) ((x2 - num4) * (double) spriteRate * 0.00999999977648258);
    double num5 = (double) baseScreenHeight * 0.5;
    Rect rect2 = component2.get_sprite().get_rect();
    double num6 = (double) ((Rect) ref rect2).get_height() * (double) num1;
    float y1 = (float) ((num5 - num6 - this.Pos.y) * (double) spriteRate * 0.00999999977648258);
    if (this.CorrectY == (byte) 0)
      y1 += spriteCorrectY;
    else if (this.CorrectY == (byte) 2)
      y1 -= spriteCorrectY;
    ((Component) component2).get_transform().SetLocalPosition(x3, y1, 0.0f);
    float x4 = spriteRate * (float) this.Scale.x;
    float y2 = spriteRate * (float) this.Scale.y;
    ((Component) component2).get_transform().SetLocalScale(x4, y2, 1f);
  }
}
