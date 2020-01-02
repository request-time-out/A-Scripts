// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.VirtualizingExternalDragItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace Battlehub.UIControls
{
  public class VirtualizingExternalDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler, IEventSystemHandler
  {
    public VirtualizingTreeView TreeView;

    public VirtualizingExternalDragItem()
    {
      base.\u002Ector();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
      this.TreeView.ExternalBeginDrag(Vector2.op_Implicit(eventData.get_position()));
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      this.TreeView.ExternalItemDrag(Vector2.op_Implicit(eventData.get_position()));
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
      if (this.TreeView.DropTarget != null)
        this.TreeView.AddChild(this.TreeView.DropTarget, (object) new GameObject());
      this.TreeView.ExternalItemDrop();
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
      if (this.TreeView.DropTarget != null)
      {
        GameObject dropTarget = (GameObject) this.TreeView.DropTarget;
        GameObject gameObject = new GameObject();
        VirtualizingTreeViewItem itemContainer1 = (VirtualizingTreeViewItem) this.TreeView.GetItemContainer(this.TreeView.DropTarget);
        if (this.TreeView.DropAction == ItemDropAction.SetLastChild)
        {
          gameObject.get_transform().SetParent(dropTarget.get_transform());
          this.TreeView.AddChild(this.TreeView.DropTarget, (object) gameObject);
          itemContainer1.CanExpand = true;
          itemContainer1.IsExpanded = true;
        }
        else if (this.TreeView.DropAction != ItemDropAction.None)
        {
          int index = this.TreeView.DropAction != ItemDropAction.SetNextSibling ? this.TreeView.IndexOf((object) dropTarget) : this.TreeView.IndexOf((object) dropTarget) + 1;
          gameObject.get_transform().SetParent(dropTarget.get_transform().get_parent());
          gameObject.get_transform().SetSiblingIndex(index);
          TreeViewItemContainerData itemContainerData = (TreeViewItemContainerData) this.TreeView.Insert(index, (object) gameObject);
          VirtualizingTreeViewItem itemContainer2 = (VirtualizingTreeViewItem) this.TreeView.GetItemContainer((object) gameObject);
          if (Object.op_Inequality((Object) itemContainer2, (Object) null))
            itemContainer2.Parent = itemContainer1.Parent;
          else
            itemContainerData.Parent = itemContainer1.Parent;
        }
      }
      this.TreeView.ExternalItemDrop();
    }
  }
}
