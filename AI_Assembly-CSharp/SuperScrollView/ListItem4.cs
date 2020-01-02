// Decompiled with JetBrains decompiler
// Type: SuperScrollView.ListItem4
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
  public class ListItem4 : MonoBehaviour
  {
    public Text mMsgText;
    public Image mMsgPic;
    public Image mIcon;
    public Image mItemBg;
    public Image mArrow;
    public Text mIndexText;
    private int mItemIndex;

    public ListItem4()
    {
      base.\u002Ector();
    }

    public int ItemIndex
    {
      get
      {
        return this.mItemIndex;
      }
    }

    public void Init()
    {
    }

    public void SetItemData(ChatMsg itemData, int itemIndex)
    {
      this.mIndexText.set_text(itemIndex.ToString());
      PersonInfo personInfo = ChatMsgDataSourceMgr.Get.GetPersonInfo(itemData.mPersonId);
      this.mItemIndex = itemIndex;
      if (itemData.mMsgType == MsgTypeEnum.Str)
      {
        ((Component) this.mMsgPic).get_gameObject().SetActive(false);
        ((Component) this.mMsgText).get_gameObject().SetActive(true);
        this.mMsgText.set_text(itemData.mSrtMsg);
        ((ContentSizeFitter) ((Component) this.mMsgText).GetComponent<ContentSizeFitter>()).SetLayoutVertical();
        this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(personInfo.mHeadIcon));
        Vector2 sizeDelta = ((RectTransform) ((Component) this.mItemBg).GetComponent<RectTransform>()).get_sizeDelta();
        sizeDelta.x = (__Null) (((RectTransform) ((Component) this.mMsgText).GetComponent<RectTransform>()).get_sizeDelta().x + 20.0);
        sizeDelta.y = (__Null) (((RectTransform) ((Component) this.mMsgText).GetComponent<RectTransform>()).get_sizeDelta().y + 20.0);
        ((RectTransform) ((Component) this.mItemBg).GetComponent<RectTransform>()).set_sizeDelta(sizeDelta);
        if (personInfo.mId == 0)
        {
          ((Graphic) this.mItemBg).set_color(Color32.op_Implicit(new Color32((byte) 160, (byte) 231, (byte) 90, byte.MaxValue)));
          ((Graphic) this.mArrow).set_color(((Graphic) this.mItemBg).get_color());
        }
        else
        {
          ((Graphic) this.mItemBg).set_color(Color.get_white());
          ((Graphic) this.mArrow).set_color(((Graphic) this.mItemBg).get_color());
        }
        RectTransform component = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
        float num = (float) sizeDelta.y;
        if ((double) num < 75.0)
          num = 75f;
        component.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, num);
      }
      else
      {
        ((Component) this.mMsgPic).get_gameObject().SetActive(true);
        ((Component) this.mMsgText).get_gameObject().SetActive(false);
        this.mMsgPic.set_sprite(ResManager.Get.GetSpriteByName(itemData.mPicMsgSpriteName));
        ((Graphic) this.mMsgPic).SetNativeSize();
        this.mIcon.set_sprite(ResManager.Get.GetSpriteByName(personInfo.mHeadIcon));
        Vector2 sizeDelta = ((RectTransform) ((Component) this.mItemBg).GetComponent<RectTransform>()).get_sizeDelta();
        sizeDelta.x = (__Null) (((RectTransform) ((Component) this.mMsgPic).GetComponent<RectTransform>()).get_sizeDelta().x + 20.0);
        sizeDelta.y = (__Null) (((RectTransform) ((Component) this.mMsgPic).GetComponent<RectTransform>()).get_sizeDelta().y + 20.0);
        ((RectTransform) ((Component) this.mItemBg).GetComponent<RectTransform>()).set_sizeDelta(sizeDelta);
        if (personInfo.mId == 0)
        {
          ((Graphic) this.mItemBg).set_color(Color32.op_Implicit(new Color32((byte) 160, (byte) 231, (byte) 90, byte.MaxValue)));
          ((Graphic) this.mArrow).set_color(((Graphic) this.mItemBg).get_color());
        }
        else
        {
          ((Graphic) this.mItemBg).set_color(Color.get_white());
          ((Graphic) this.mArrow).set_color(((Graphic) this.mItemBg).get_color());
        }
        RectTransform component = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
        float num = (float) sizeDelta.y;
        if ((double) num < 75.0)
          num = 75f;
        component.SetSizeWithCurrentAnchors((RectTransform.Axis) 1, num);
      }
    }
  }
}
