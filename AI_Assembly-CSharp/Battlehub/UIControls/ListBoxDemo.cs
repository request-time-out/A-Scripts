// Decompiled with JetBrains decompiler
// Type: Battlehub.UIControls.ListBoxDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
  public class ListBoxDemo : MonoBehaviour
  {
    public ListBox ListBox;

    public ListBoxDemo()
    {
      base.\u002Ector();
    }

    public static bool IsPrefab(Transform This)
    {
      if (Application.get_isEditor() && !Application.get_isPlaying())
        throw new InvalidOperationException("Does not work in edit mode");
      Scene scene = ((Component) This).get_gameObject().get_scene();
      return ((Scene) ref scene).get_buildIndex() < 0;
    }

    private void Start()
    {
      if (!Object.op_Implicit((Object) this.ListBox))
      {
        Debug.LogError((object) "Set ListBox field");
      }
      else
      {
        this.ListBox.ItemDataBinding += new EventHandler<ItemDataBindingArgs>(this.OnItemDataBinding);
        this.ListBox.SelectionChanged += new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
        this.ListBox.ItemsRemoved += new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
        this.ListBox.ItemBeginDrag += new EventHandler<ItemArgs>(this.OnItemBeginDrag);
        this.ListBox.ItemDrop += new EventHandler<ItemDropArgs>(this.OnItemDrop);
        this.ListBox.ItemEndDrag += new EventHandler<ItemArgs>(this.OnItemEndDrag);
        this.ListBox.Items = (IEnumerable) ((IEnumerable<GameObject>) Resources.FindObjectsOfTypeAll<GameObject>()).Where<GameObject>((Func<GameObject, bool>) (go => !ListBoxDemo.IsPrefab(go.get_transform()) && Object.op_Equality((Object) go.get_transform().get_parent(), (Object) null))).OrderBy<GameObject, int>((Func<GameObject, int>) (t => t.get_transform().GetSiblingIndex()));
      }
    }

    private void OnDestroy()
    {
      if (!Object.op_Implicit((Object) this.ListBox))
        return;
      this.ListBox.ItemDataBinding -= new EventHandler<ItemDataBindingArgs>(this.OnItemDataBinding);
      this.ListBox.SelectionChanged -= new EventHandler<SelectionChangedArgs>(this.OnSelectionChanged);
      this.ListBox.ItemsRemoved -= new EventHandler<ItemsRemovedArgs>(this.OnItemsRemoved);
      this.ListBox.ItemBeginDrag -= new EventHandler<ItemArgs>(this.OnItemBeginDrag);
      this.ListBox.ItemDrop -= new EventHandler<ItemDropArgs>(this.OnItemDrop);
      this.ListBox.ItemEndDrag -= new EventHandler<ItemArgs>(this.OnItemEndDrag);
    }

    private void OnItemExpanding(object sender, ItemExpandingArgs e)
    {
      GameObject gameObject = (GameObject) e.Item;
      if (gameObject.get_transform().get_childCount() <= 0)
        return;
      GameObject[] gameObjectArray = new GameObject[gameObject.get_transform().get_childCount()];
      for (int index = 0; index < gameObjectArray.Length; ++index)
        gameObjectArray[index] = ((Component) gameObject.get_transform().GetChild(index)).get_gameObject();
      e.Children = (IEnumerable) gameObjectArray;
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
    }

    private void OnItemsRemoved(object sender, ItemsRemovedArgs e)
    {
      for (int index = 0; index < e.Items.Length; ++index)
      {
        GameObject gameObject = (GameObject) e.Items[index];
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          Object.Destroy((Object) gameObject);
      }
    }

    private void OnItemDataBinding(object sender, ItemDataBindingArgs e)
    {
      GameObject gameObject = e.Item as GameObject;
      if (!Object.op_Inequality((Object) gameObject, (Object) null))
        return;
      ((Text) e.ItemPresenter.GetComponentInChildren<Text>(true)).set_text(((Object) gameObject).get_name());
    }

    private void OnItemBeginDrag(object sender, ItemArgs e)
    {
    }

    private void OnItemDrop(object sender, ItemDropArgs e)
    {
      Transform transform1 = ((GameObject) e.DropTarget).get_transform();
      if (e.Action == ItemDropAction.SetLastChild)
      {
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          transform2.SetParent(transform1, true);
          transform2.SetAsLastSibling();
        }
      }
      else if (e.Action == ItemDropAction.SetNextSibling)
      {
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          if (Object.op_Inequality((Object) transform2.get_parent(), (Object) transform1.get_parent()))
            transform2.SetParent(transform1.get_parent(), true);
          int siblingIndex = transform1.GetSiblingIndex();
          transform2.SetSiblingIndex(siblingIndex + 1);
        }
      }
      else
      {
        if (e.Action != ItemDropAction.SetPrevSibling)
          return;
        for (int index = 0; index < e.DragItems.Length; ++index)
        {
          Transform transform2 = ((GameObject) e.DragItems[index]).get_transform();
          if (Object.op_Inequality((Object) transform2.get_parent(), (Object) transform1.get_parent()))
            transform2.SetParent(transform1.get_parent(), true);
          int siblingIndex = transform1.GetSiblingIndex();
          transform2.SetSiblingIndex(siblingIndex);
        }
      }
    }

    private void OnItemEndDrag(object sender, ItemArgs e)
    {
    }
  }
}
