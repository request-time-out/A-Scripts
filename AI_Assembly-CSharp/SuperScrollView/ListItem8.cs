// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem8
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem8 : MonoBehaviour
  {
    public Text mNameText;
    public Image mIcon;
    public Image[] mStarArray;
    public Text mDescText;
    public GameObject mExpandContentRoot;
    public Text mClickTip;
    public Button mExpandBtn;
    public Color32 mRedStarColor;
    public Color32 mGrayStarColor;
    private int mItemDataIndex;
    private bool mIsExpand;

    public ListItem8()
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
      // ISSUE: method pointer
      ((UnityEvent) this.mExpandBtn.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnExpandBtnClicked)));
    }

    public void OnExpandChanged()
    {
      RectTransform component = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
      if (this.mIsExpand)
      {
        component.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 284f);
        this.mExpandContentRoot.SetActive(true);
        this.mClickTip.set_text("Shrink");
      }
      else
      {
        component.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, 143f);
        this.mExpandContentRoot.SetActive(false);
        this.mClickTip.set_text("Expand");
      }
    }

    private void OnExpandBtnClicked()
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(this.mItemDataIndex);
      if (itemDataByIndex == null)
        return;
      this.mIsExpand = !this.mIsExpand;
      itemDataByIndex.mIsExpand = this.mIsExpand;
      this.OnExpandChanged();
      LoopListViewItem2 component = (LoopListViewItem2) ((Component) this).get_gameObject().GetComponent<LoopListViewItem2>();
      component.ParentListView.OnItemSizeChanged(component.ItemIndex);
    }

    private void OnStarClicked(int index)
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(this.mItemDataIndex);
      if (itemDataByIndex == null)
        return;
      itemDataByIndex.mStarCount = index != 0 || itemDataByIndex.mStarCount != 1 ? index + 1 : 0;
      this.SetStarCount(itemDataByIndex.mStarCount);
    }

    public void SetStarCount(int count)
    {
      int index;
      for (index = 0; index < count; ++index)
        ((Graphic) this.mStarArray[index]).set_color(Color32.op_Implicit(this.mRedStarColor));
      for (; index < this.mStarArray.Length; ++index)
        ((Graphic) this.mStarArray[index]).set_color(Color32.op_Implicit(this.mGrayStarColor));
    }

    public void SetItemData(ItemData itemData, int itemIndex)
    {
      this.mItemDataIndex = itemIndex;
      this.mNameText.set_text(itemData.mName);
      this.mDescText.set_text(itemData.mFileSize.ToString() + "KB");
      this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(itemData.mIcon));
      this.SetStarCount(itemData.mStarCount);
      this.mIsExpand = itemData.mIsExpand;
      this.OnExpandChanged();
    }
  }
}
