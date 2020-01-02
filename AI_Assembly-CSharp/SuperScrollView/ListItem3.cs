// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem3 : MonoBehaviour
  {
    public Text mNameText;
    public Image mIcon;
    public Text mDescText;
    private int mItemIndex;
    public Toggle mToggle;

    public ListItem3()
    {
      base.\u002Ector();
    }

    public void Init()
    {
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.mToggle.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnToggleValueChanged)));
    }

    private void OnToggleValueChanged(bool check)
    {
      ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(this.mItemIndex);
      if (itemDataByIndex == null)
        return;
      itemDataByIndex.mChecked = check;
    }

    public void SetItemData(ItemData itemData, int itemIndex)
    {
      this.mItemIndex = itemIndex;
      this.mNameText.set_text(itemData.mName);
      this.mDescText.set_text(itemData.mDesc);
      this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(itemData.mIcon));
      this.mToggle.set_isOn(itemData.mChecked);
    }
  }
}
