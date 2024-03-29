﻿// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.TreeViewDropMarker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Battlehub.UIControls
{
  [RequireComponent(typeof (RectTransform))]
  public class TreeViewDropMarker : ItemDropMarker
  {
    private TreeView m_treeView;
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
      this.m_treeView = (TreeView) ((Component) this).GetComponentInParent<TreeView>();
      this.m_siblingGraphicsRectTransform = (RectTransform) this.SiblingGraphics.GetComponent<RectTransform>();
    }

    public override void SetTraget(ItemContainer item)
    {
      base.SetTraget(item);
      if (Object.op_Equality((Object) item, (Object) null))
        return;
      TreeViewItem treeViewItem = (TreeViewItem) item;
      if (Object.op_Inequality((Object) treeViewItem, (Object) null))
        this.m_siblingGraphicsRectTransform.set_offsetMin(new Vector2((float) treeViewItem.Indent, (float) this.m_siblingGraphicsRectTransform.get_offsetMin().y));
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
        TreeViewItem treeViewItem = (TreeViewItem) this.Item;
        Camera camera = (Camera) null;
        if (this.ParentCanvas.get_renderMode() == 2 || this.ParentCanvas.get_renderMode() == 1)
          camera = this.m_treeView.Camera;
        if (!this.m_treeView.CanReorder)
        {
          if (!treeViewItem.CanDrop)
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
          Rect rect1 = rectTransform1.get_rect();
          double num1 = -(double) ((Rect) ref rect1).get_height() / 4.0;
          if (y1 > num1)
          {
            this.Action = ItemDropAction.SetPrevSibling;
            ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
          }
          else
          {
            // ISSUE: variable of the null type
            __Null y2 = vector2.y;
            Rect rect2 = rectTransform1.get_rect();
            double num2 = (double) ((Rect) ref rect2).get_height() / 4.0;
            Rect rect3 = rectTransform1.get_rect();
            double height = (double) ((Rect) ref rect3).get_height();
            double num3 = num2 - height;
            if (y2 < num3 && !treeViewItem.HasChildren)
            {
              this.Action = ItemDropAction.SetNextSibling;
              ((Transform) this.RectTransform).set_position(((Transform) rectTransform1).get_position());
              RectTransform rectTransform2 = this.RectTransform;
              Vector3 localPosition = ((Transform) this.RectTransform).get_localPosition();
              Rect rect4 = rectTransform1.get_rect();
              Vector3 vector3_1 = new Vector3(0.0f, (float) ((double) ((Rect) ref rect4).get_height() * (double) this.ParentCanvas.get_scaleFactor()), 0.0f);
              Vector3 vector3_2 = Vector3.op_Subtraction(localPosition, vector3_1);
              ((Transform) rectTransform2).set_localPosition(vector3_2);
            }
            else
            {
              if (!treeViewItem.CanDrop)
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
