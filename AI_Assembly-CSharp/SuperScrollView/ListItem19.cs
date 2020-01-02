// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem19
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem19 : MonoBehaviour
  {
    public Text mNameText;
    public Image mIcon;
    public Image mStarIcon;
    public Text mStarCount;
    public Text mRowText;
    public Text mColumnText;
    public Color32 mRedStarColor;
    public Color32 mGrayStarColor;
    public Toggle mToggle;
    private int mItemDataIndex;

    public ListItem19()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      ClickEventListener.Get(((Component) this.mStarIcon).get_gameObject()).SetClickEventHandler(new Action<GameObject>(this.OnStarClicked));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.mToggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnToggleValueChanged)));
    }

    private void OnToggleValueChanged(bool check)
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(this.mItemDataIndex);
      if (itemDataByIndex == null)
        return;
      itemDataByIndex.mChecked = check;
    }

    private void OnStarClicked(GameObject obj)
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(this.mItemDataIndex);
      if (itemDataByIndex == null)
        return;
      if (itemDataByIndex.mStarCount == 5)
        itemDataByIndex.mStarCount = 0;
      else
        ++itemDataByIndex.mStarCount;
      this.SetStarCount(itemDataByIndex.mStarCount);
    }

    public void SetStarCount(int count)
    {
      this.mStarCount.set_text(count.ToString());
      if (count == 0)
        ((Graphic) this.mStarIcon).set_color(Color32.op_Implicit(this.mGrayStarColor));
      else
        ((Graphic) this.mStarIcon).set_color(Color32.op_Implicit(this.mRedStarColor));
    }

    public void SetItemData(ItemData itemData, int itemIndex, int row, int column)
    {
      this.mItemDataIndex = itemIndex;
      this.mNameText.set_text(itemData.mName);
      this.mRowText.set_text("Row: " + (object) row);
      this.mColumnText.set_text("Column: " + (object) column);
      this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(itemData.mIcon));
      this.SetStarCount(itemData.mStarCount);
      this.mToggle.set_isOn(itemData.mChecked);
    }
  }
}
