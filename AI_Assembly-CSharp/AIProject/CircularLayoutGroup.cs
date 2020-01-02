// Decompiled with JetBrains decompiler
// Type: AIProject.CircularLayoutGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  [DisallowMultipleComponent]
  [ExecuteInEditMode]
  public class CircularLayoutGroup : UIBehaviour, ILayoutGroup, IScrollHandler, ILayoutController, IEventSystemHandler
  {
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private float _startAngle;
    [SerializeField]
    [Range(15f, 360f)]
    private float _overallAngle;
    [SerializeField]
    private CircularLayoutGroup.Anchor _childAlignment;
    [SerializeField]
    private Vector2 _cellSize;
    [SerializeField]
    private int _rowCellNum;
    [SerializeField]
    private float _rowMargin;
    private float _scrollAngle;

    public CircularLayoutGroup()
    {
      base.\u002Ector();
    }

    public CircularLayoutGroup.Anchor ChildAlignment
    {
      get
      {
        return this._childAlignment;
      }
      set
      {
        this._childAlignment = value;
      }
    }

    public int GetRowCount()
    {
      return Mathf.CeilToInt((float) ((double) ((Component) this).get_transform().get_childCount() / (double) this._rowCellNum - 1.0 / 1000.0));
    }

    private void Update()
    {
      this.Arrange();
    }

    public void SetLayoutHorizontal()
    {
    }

    public void SetLayoutVertical()
    {
      this.Arrange();
    }

    private void Arrange()
    {
      int childCount = ((Component) this).get_transform().get_childCount();
      int num = Mathf.CeilToInt((float) ((double) childCount / (double) this._rowCellNum - 1.0 / 1000.0));
      for (int r = 0; r < num; ++r)
      {
        float radius = this._radius + ((float) this._cellSize.y + this._rowMargin) * (float) r;
        int cellNum = childCount - this._rowCellNum * r;
        if (cellNum >= this._rowCellNum)
          this.ArrangeRow(r, this._rowCellNum, radius);
        else
          this.ArrangeRow(r, cellNum, radius);
      }
    }

    private void ArrangeRow(int r, int cellNum, float radius)
    {
      float num1 = this._overallAngle / (this._childAlignment != CircularLayoutGroup.Anchor.Center ? (float) (this._rowCellNum - 1) : (cellNum <= 1 ? (float) cellNum : (float) (cellNum - 1)));
      float num2 = 90f + this._startAngle + this._scrollAngle;
      float num3 = this._overallAngle / 2f;
      int num4 = this._rowCellNum * r;
      for (int index = 0; index < cellNum; ++index)
      {
        RectTransform child = ((Component) this).get_transform().GetChild(num4 + index) as RectTransform;
        child.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, (float) this._cellSize.x);
        child.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, (float) this._cellSize.y);
        float num5 = num1 * (float) index;
        if (this._childAlignment == CircularLayoutGroup.Anchor.Right)
          num5 = -num5;
        float num6 = num5 + num2;
        if (this._childAlignment == CircularLayoutGroup.Anchor.Left)
          num6 -= num3;
        else if (this._childAlignment == CircularLayoutGroup.Anchor.Right)
          num6 += num3;
        else if (cellNum > 1)
          num6 -= num3;
        child.set_anchoredPosition(Vector2.op_Addition(this._offset, Vector2.op_Multiply(new Vector2(Mathf.Cos(num6 * ((float) Math.PI / 180f)), Mathf.Sin(num6 * ((float) Math.PI / 180f))), radius)));
      }
    }

    public void OnScroll(PointerEventData eventData)
    {
      this._scrollAngle += (float) eventData.get_scrollDelta().y;
      this.Arrange();
    }

    public enum Anchor
    {
      Left,
      Center,
      Right,
    }
  }
}
