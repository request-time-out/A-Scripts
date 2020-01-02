// Decompiled with JetBrains decompiler
// Type: PlayfulSystems.ImageSlicedMirror
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace PlayfulSystems
{
  public class ImageSlicedMirror : Image
  {
    private static readonly Vector2[] s_VertScratch = new Vector2[4];
    private static readonly Vector2[] s_UVScratch = new Vector2[4];
    private static readonly float[] s_UVMultiplierScratch = new float[4];

    public ImageSlicedMirror()
    {
      base.\u002Ector();
    }

    protected virtual void OnPopulateMesh(VertexHelper toFill)
    {
      if (Object.op_Equality((Object) this.get_overrideSprite(), (Object) null))
        base.OnPopulateMesh(toFill);
      else if (this.get_hasBorder() && this.get_type() == 1)
        this.GenerateSlicedFilledSprite(toFill);
      else
        base.OnPopulateMesh(toFill);
    }

    private void GenerateSlicedFilledSprite(VertexHelper toFill)
    {
      Vector4 outer;
      Vector4 inner;
      Vector4 vector4_1;
      Vector4 vector4_2;
      if (Object.op_Inequality((Object) this.get_overrideSprite(), (Object) null))
      {
        outer = DataUtility.GetOuterUV(this.get_overrideSprite());
        inner = DataUtility.GetInnerUV(this.get_overrideSprite());
        vector4_1 = DataUtility.GetPadding(this.get_overrideSprite());
        vector4_2 = this.get_overrideSprite().get_border();
      }
      else
      {
        outer = Vector4.get_zero();
        inner = Vector4.get_zero();
        vector4_1 = Vector4.get_zero();
        vector4_2 = Vector4.get_zero();
      }
      Rect pixelAdjustedRect = ((Graphic) this).GetPixelAdjustedRect();
      Vector4 adjustedBorders = this.GetAdjustedBorders(Vector4.op_Division(vector4_2, this.get_pixelsPerUnit()), pixelAdjustedRect);
      Vector4 padding = Vector4.op_Division(vector4_1, this.get_pixelsPerUnit());
      this.SetSlicedVerts(pixelAdjustedRect, adjustedBorders, padding);
      this.SetSlicedUVs(outer, inner, adjustedBorders);
      toFill.Clear();
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = index1 + 1;
        for (int index3 = 0; index3 < 3; ++index3)
        {
          if (this.get_fillCenter() || index1 != 1 || index3 != 1)
          {
            int index4 = index3 + 1;
            ImageSlicedMirror.AddQuad(toFill, new Vector2((float) ImageSlicedMirror.s_VertScratch[index1].x, (float) ImageSlicedMirror.s_VertScratch[index3].y), new Vector2((float) ImageSlicedMirror.s_VertScratch[index2].x, (float) ImageSlicedMirror.s_VertScratch[index4].y), Color32.op_Implicit(((Graphic) this).get_color()), new Vector2((float) ImageSlicedMirror.s_UVScratch[index1].x, (float) ImageSlicedMirror.s_UVScratch[index3].y), new Vector2((float) ImageSlicedMirror.s_UVScratch[index2].x, (float) ImageSlicedMirror.s_UVScratch[index4].y));
          }
        }
      }
    }

    private void SetSlicedVerts(Rect rect, Vector4 border, Vector4 padding)
    {
      ImageSlicedMirror.s_VertScratch[0] = new Vector2((float) padding.x, (float) padding.y);
      ImageSlicedMirror.s_VertScratch[3] = new Vector2(((Rect) ref rect).get_width() - (float) padding.z, ((Rect) ref rect).get_height() - (float) padding.w);
      ImageSlicedMirror.s_VertScratch[1].x = border.x;
      ImageSlicedMirror.s_VertScratch[1].y = border.y;
      ImageSlicedMirror.s_VertScratch[2].x = (__Null) ((double) ((Rect) ref rect).get_width() - border.z);
      ImageSlicedMirror.s_VertScratch[2].y = (__Null) ((double) ((Rect) ref rect).get_height() - border.w);
      for (int index = 0; index < 4; ++index)
      {
        ref Vector2 local1 = ref ImageSlicedMirror.s_VertScratch[index];
        local1.x = (__Null) (local1.x + (double) ((Rect) ref rect).get_x());
        ref Vector2 local2 = ref ImageSlicedMirror.s_VertScratch[index];
        local2.y = (__Null) (local2.y + (double) ((Rect) ref rect).get_y());
      }
    }

    private void SetSlicedUVs(Vector4 outer, Vector4 inner, Vector4 border)
    {
      bool flag1 = border.x < this.get_overrideSprite().get_border().x || border.z < this.get_overrideSprite().get_border().z;
      bool flag2 = border.y < this.get_overrideSprite().get_border().y || border.w < this.get_overrideSprite().get_border().w;
      if (!flag1 && !flag2)
      {
        ImageSlicedMirror.s_UVScratch[0] = new Vector2((float) outer.x, (float) outer.y);
        ImageSlicedMirror.s_UVScratch[1] = new Vector2((float) inner.x, (float) inner.y);
        ImageSlicedMirror.s_UVScratch[2] = new Vector2((float) inner.z, (float) inner.w);
        ImageSlicedMirror.s_UVScratch[3] = new Vector2((float) outer.z, (float) outer.w);
      }
      else
      {
        ImageSlicedMirror.s_UVMultiplierScratch[0] = border.x == 0.0 || !flag1 ? 1f : (float) (double) (border.x / this.get_overrideSprite().get_border().x);
        ImageSlicedMirror.s_UVMultiplierScratch[1] = border.y == 0.0 || !flag2 ? 1f : (float) (double) (border.y / this.get_overrideSprite().get_border().y);
        ImageSlicedMirror.s_UVMultiplierScratch[2] = border.z == 0.0 || !flag1 ? 1f : (float) (double) (border.z / this.get_overrideSprite().get_border().z);
        ImageSlicedMirror.s_UVMultiplierScratch[3] = border.w == 0.0 || !flag2 ? 1f : (float) (double) (border.w / this.get_overrideSprite().get_border().w);
        ImageSlicedMirror.s_UVScratch[0] = new Vector2((float) outer.x, (float) outer.y);
        ImageSlicedMirror.s_UVScratch[1] = new Vector2((float) inner.x * ImageSlicedMirror.s_UVMultiplierScratch[0], (float) inner.y * ImageSlicedMirror.s_UVMultiplierScratch[1]);
        ImageSlicedMirror.s_UVScratch[2] = new Vector2((float) (outer.z - (outer.z - inner.z) * (double) ImageSlicedMirror.s_UVMultiplierScratch[2]), (float) (outer.w - (outer.w - inner.w) * (double) ImageSlicedMirror.s_UVMultiplierScratch[3]));
        ImageSlicedMirror.s_UVScratch[3] = new Vector2((float) outer.z, (float) outer.w);
      }
    }

    private static void AddQuad(
      VertexHelper vertexHelper,
      Vector2 posMin,
      Vector2 posMax,
      Color32 color,
      Vector2 uvMin,
      Vector2 uvMax)
    {
      int currentVertCount = vertexHelper.get_currentVertCount();
      vertexHelper.AddVert(new Vector3((float) posMin.x, (float) posMin.y, 0.0f), color, new Vector2((float) uvMin.x, (float) uvMin.y));
      vertexHelper.AddVert(new Vector3((float) posMin.x, (float) posMax.y, 0.0f), color, new Vector2((float) uvMin.x, (float) uvMax.y));
      vertexHelper.AddVert(new Vector3((float) posMax.x, (float) posMax.y, 0.0f), color, new Vector2((float) uvMax.x, (float) uvMax.y));
      vertexHelper.AddVert(new Vector3((float) posMax.x, (float) posMin.y, 0.0f), color, new Vector2((float) uvMax.x, (float) uvMin.y));
      vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
      vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
    {
      for (int index = 0; index <= 1; ++index)
      {
        float num1 = ((Vector4) ref border).get_Item(index) + ((Vector4) ref border).get_Item(index + 2);
        Vector2 size1 = ((Rect) ref rect).get_size();
        if ((double) ((Vector2) ref size1).get_Item(index) < (double) num1 && (double) num1 != 0.0)
        {
          Vector2 size2 = ((Rect) ref rect).get_size();
          float num2 = ((Vector2) ref size2).get_Item(index) / num1;
          // ISSUE: variable of a reference type
          Vector4& local1;
          int num3;
          ((Vector4) (local1 = ref border)).set_Item(num3 = index, ((Vector4) ref local1).get_Item(num3) * num2);
          // ISSUE: variable of a reference type
          Vector4& local2;
          int num4;
          ((Vector4) (local2 = ref border)).set_Item(num4 = index + 2, ((Vector4) ref local2).get_Item(num4) * num2);
        }
      }
      return border;
    }
  }
}
