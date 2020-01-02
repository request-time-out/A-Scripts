// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem12
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem12 : MonoBehaviour
  {
    public Text mText;
    public GameObject mArrow;
    public Button mButton;
    private int mTreeItemIndex;
    private Action<int> mClickHandler;

    public ListItem12()
    {
      base.\u002Ector();
    }

    public int TreeItemIndex
    {
      get
      {
        return this.mTreeItemIndex;
      }
    }

    public void Init()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.mButton.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnButtonClicked)));
    }

    public void SetClickCallBack(Action<int> clickHandler)
    {
      this.mClickHandler = clickHandler;
    }

    private void OnButtonClicked()
    {
      if (this.mClickHandler == null)
        return;
      this.mClickHandler(this.mTreeItemIndex);
    }

    public void SetExpand(bool expand)
    {
      if (expand)
        this.mArrow.get_transform().set_localEulerAngles(new Vector3(0.0f, 0.0f, -90f));
      else
        this.mArrow.get_transform().set_localEulerAngles(new Vector3(0.0f, 0.0f, 90f));
    }

    public void SetItemData(int treeItemIndex, bool expand)
    {
      this.mTreeItemIndex = treeItemIndex;
      this.SetExpand(expand);
    }
  }
}
