// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem13
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem13 : MonoBehaviour
  {
    public Text mNameText;
    public Image mIcon;
    public Image[] mStarArray;
    public Text mDescText;
    public Text mDescText2;
    public Color32 mRedStarColor;
    public Color32 mGrayStarColor;
    public GameObject mContentRootObj;
    private int mItemDataIndex;
    private int mChildDataIndex;

    public ListItem13()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      for (int index1 = 0; index1 < this.mStarArray.Length; ++index1)
      {
        int index = index1;
        ClickEventListener.Get(((Component) this.mStarArray[index1]).get_gameObject()).SetClickEventHandler((Action<GameObject>) (obj => this.OnStarClicked(index)));
      }
    }

    private void OnStarClicked(int index)
    {
      ItemData childDataByIndex = TreeViewDataSourceMgr.Get.GetItemChildDataByIndex(this.mItemDataIndex, this.mChildDataIndex);
      if (childDataByIndex == null)
        return;
      childDataByIndex.mStarCount = index != 0 || childDataByIndex.mStarCount != 1 ? index + 1 : 0;
      this.SetStarCount(childDataByIndex.mStarCount);
    }

    public void SetStarCount(int count)
    {
      int index;
      for (index = 0; index < count; ++index)
        ((Graphic) this.mStarArray[index]).set_color(Color32.op_Implicit(this.mRedStarColor));
      for (; index < this.mStarArray.Length; ++index)
        ((Graphic) this.mStarArray[index]).set_color(Color32.op_Implicit(this.mGrayStarColor));
    }

    public void SetItemData(ItemData itemData, int itemIndex, int childIndex)
    {
      this.mItemDataIndex = itemIndex;
      this.mChildDataIndex = childIndex;
      this.mNameText.set_text(itemData.mName);
      this.mDescText.set_text(itemData.mFileSize.ToString() + "KB");
      this.mDescText2.set_text(itemData.mDesc);
      this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(itemData.mIcon));
      this.SetStarCount(itemData.mStarCount);
    }
  }
}
