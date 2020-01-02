// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomSelectScrollViewInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  [DisallowMultipleComponent]
  public class CustomSelectScrollViewInfo : MonoBehaviour
  {
    [SerializeField]
    private CustomSelectScrollViewInfo.RowInfo[] rows;

    public CustomSelectScrollViewInfo()
    {
      base.\u002Ector();
    }

    public void SetData(
      int _index,
      CustomSelectScrollController.ScrollData _data,
      Action<bool> _onClickAction,
      Action<string> _onPointerEnter,
      Action _onPointerExit)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomSelectScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new CustomSelectScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onClickAction = _onClickAction;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onPointerEnter = _onPointerEnter;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onPointerExit = _onPointerExit;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._info = _data?.info;
      // ISSUE: reference to a compiler-generated field
      bool active = dataCAnonStorey0._info != null;
      ((Component) this.rows[_index].tgl).get_gameObject().SetActiveIfDifferent(active);
      ((UnityEventBase) this.rows[_index].tgl.onValueChanged).RemoveAllListeners();
      if (!active)
        return;
      _data.toggle = this.rows[_index].tgl;
      ((UnityEventBase) this.rows[_index].tgl.onValueChanged).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.rows[_index].tgl.onValueChanged).AddListener(new UnityAction<bool>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      EventTriggerNoScroll eventTriggerNoScroll = (EventTriggerNoScroll) ((Component) this.rows[_index].tgl).get_gameObject().AddComponent<EventTriggerNoScroll>();
      eventTriggerNoScroll.triggers = new List<EventTriggerNoScroll.Entry>();
      EventTriggerNoScroll.Entry entry1 = new EventTriggerNoScroll.Entry();
      entry1.eventID = (EventTriggerType) 0;
      // ISSUE: method pointer
      entry1.callback.AddListener(new UnityAction<BaseEventData>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      eventTriggerNoScroll.triggers.Add(entry1);
      EventTriggerNoScroll.Entry entry2 = new EventTriggerNoScroll.Entry();
      entry2.eventID = (EventTriggerType) 1;
      // ISSUE: method pointer
      entry2.callback.AddListener(new UnityAction<BaseEventData>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__2)));
      eventTriggerNoScroll.triggers.Add(entry2);
      this.rows[_index].tgl.SetIsOnWithoutCallback(false);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(dataCAnonStorey0._info.assetBundle, dataCAnonStorey0._info.assetName, false, string.Empty);
      if (Object.op_Implicit((Object) texture2D))
        this.rows[_index].imgThumb.set_sprite(Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), new Vector2(0.5f, 0.5f)));
      else
        this.rows[_index].imgThumb.set_sprite((Sprite) null);
      if (Object.op_Implicit((Object) this.rows[_index].imgNew))
      {
        // ISSUE: reference to a compiler-generated field
        ((Component) this.rows[_index].imgNew).get_gameObject().SetActiveIfDifferent(dataCAnonStorey0._info.newItem);
      }
      // ISSUE: reference to a compiler-generated field
      this.rows[_index].info = dataCAnonStorey0._info;
    }

    public void SetToggleON(int _index, bool _isOn)
    {
      this.rows[_index].tgl.SetIsOnWithoutCallback(_isOn);
    }

    public void SetNewFlagOff(int _index)
    {
      ((Component) this.rows[_index].imgNew).get_gameObject().SetActiveIfDifferent(false);
      this.rows[_index].info.newItem = false;
      Singleton<Character>.Instance.chaListCtrl.AddItemID(this.rows[_index].info.category, this.rows[_index].info.id, (byte) 2);
    }

    public CustomSelectInfo GetListInfo(int _index)
    {
      return this.rows[_index].info;
    }

    public void Disable(bool disable)
    {
    }

    public void Disvisible(bool disvisible)
    {
    }

    [Serializable]
    public class RowInfo
    {
      public Toggle tgl;
      public Image imgThumb;
      public Image imgNew;
      public CustomSelectInfo info;
    }
  }
}
