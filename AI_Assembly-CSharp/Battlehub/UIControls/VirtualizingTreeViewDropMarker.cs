// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingTreeViewDropMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (RectTransform))]
  public class VirtualizingTreeViewDropMarker : VirtualizingItemDropMarker
  {
    private bool m_useGrid;
    private VirtualizingTreeView m_treeView;
    private RectTransform m_siblingGraphicsRectTransform;
    public GameObject ChildGraphics;

    public override ItemDropAction Action
    {
      get
      {
        return base.Action;
      }
      set
      {
        base.Action = value;
        this.ChildGraphics.SetActive(base.Action == ItemDropAction.SetLastChild);
        this.SiblingGraphics.SetActive(base.Action != ItemDropAction.SetLastChild);
      }
    }

    protected override void AwakeOverride()
    {
      base.AwakeOverride();
      this.m_treeView = (VirtualizingTreeView) ((Component) this).GetComponentInParent<VirtualizingTreeView>();
      this.m_siblingGraphicsRectTransform = (RectTransform) this.SiblingGraphics.GetComponent<RectTransform>();
      RectTransform transform = (RectTransform) ((Component) this).get_transform();
      VirtualizingScrollRect componentInChildren = (VirtualizingScrollRect) ((Component) this.m_treeView).GetComponentInChildren<VirtualizingScrollRect>();
      if (Object.op_Inequality((Object) componentInChildren, (Object) null) && componentInChildren.UseGrid)
      {
        this.m_useGrid = true;
        transform.set_anchorMin(Vector2.get_zero());
        transform.set_anchorMax(Vector2.get_zero());
      }
      else
      {
        transform.set_anchorMin(Vector2.get_zero());
        transform.set_anchorMax(new Vector2(1f, 0.0f));
      }
    }

    public override void SetTarget(VirtualizingItemContainer item)
    {
      base.SetTarget(item);
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem) item;
      if (Object.op_Inequality((Object) virtualizingTreeViewItem, (Object) null))
        this.m_siblingGraphicsRectTransform.set_offsetMin(new Vector2(virtualizingTreeViewItem.Indent, (float) this.m_siblingGraphicsRectTransform.get_offsetMin().y));
      else
        this.m_siblingGraphicsRectTransform.set_offsetMin(new Vector2(0.0f, (float) this.m_siblingGraphicsRectTransform.get_offsetMin().y));
    }

    public override void SetPosition(Vector2 position)
    {
      if (Object.op_Equality((Object) this.Item, (Object) null))
        return;
      if (!this.m_treeView.CanReparent)
      {
        base.SetPosition(position);
      }
      else
      {
        RectTransform rectTransform1 = this.Item.RectTransform;
        VirtualizingTreeViewItem virtualizingTreeViewItem = (VirtualizingTreeViewItem) this.Item;
        Vector2 sizeDelta = this.m_rectTransform.get_sizeDelta();
        ref Vector2 local1 = ref sizeDelta;
        Rect rect1 = rectTransform1.get_rect();
        double height1 = (double) ((Rect) ref rect1).get_height();
        local1.y = (__Null) height1;
        if (this.m_useGrid)
        {
          ref Vector2 local2 = ref sizeDelta;
          Rect rect2 = rectTransform1.get_rect();
          double width = (double) ((Rect) ref rect2).get_width();
          local2.x = (__Null) width;
        }
        this.m_rectTransform.set_sizeDelta(sizeDelta);
        Camera camera = (Camera) null;
        if (this.ParentCanvas.get_renderMode() == 2 || this.ParentCanvas.get_renderMode() == 1)
          camera = this.m_treeView.Camera;
        if (!this.m_treeView.CanReorder)
        {
          if (!virtualizingTreeViewItem.CanBeParent)
            return;
          this.Action = ItemDropAction.SetLastChild;
          ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
        }
        else
        {
          Vector2 vector2;
          if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform1, position, camera, ref vector2))
            return;
          // ISSUE: variable of the null type
          __Null y1 = vector2.y;
          Rect rect2 = rectTransform1.get_rect();
          double num1 = -(double) ((Rect) ref rect2).get_height() / 4.0;
          if (y1 > num1)
          {
            this.Action = ItemDropAction.SetPrevSibling;
            ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
          }
          else
          {
            // ISSUE: variable of the null type
            __Null y2 = vector2.y;
            Rect rect3 = rectTransform1.get_rect();
            double num2 = (double) ((Rect) ref rect3).get_height() / 4.0;
            Rect rect4 = rectTransform1.get_rect();
            double height2 = (double) ((Rect) ref rect4).get_height();
            double num3 = num2 - height2;
            if (y2 < num3 && !virtualizingTreeViewItem.HasChildren)
            {
              this.Action = ItemDropAction.SetNextSibling;
              RectTransform rectTransform2 = this.RectTransform;
              RectTransform rectTransform3 = rectTransform1;
              Vector3 down = Vector3.get_down();
              Rect rect5 = rectTransform1.get_rect();
              double height3 = (double) ((Rect) ref rect5).get_height();
              Vector3 vector3_1 = Vector3.op_Multiply(down, (float) height3);
              Vector3 vector3_2 = ((Transform) rectTransform3).TransformPoint(vector3_1);
              ((Transform) rectTransform2).set_position(vector3_2);
            }
            else
            {
              if (!virtualizingTreeViewItem.CanBeParent)
                return;
              this.Action = ItemDropAction.SetLastChild;
              ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
            }
          }
        }
      }
    }
  }
}
