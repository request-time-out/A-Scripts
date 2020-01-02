// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomPushScrollViewInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  [DisallowMultipleComponent]
  public class CustomPushScrollViewInfo : MonoBehaviour
  {
    [SerializeField]
    private CustomPushScrollViewInfo.RowInfo[] rows;

    public CustomPushScrollViewInfo()
    {
      base.\u002Ector();
    }

    public void SetData(
      int _index,
      CustomPushInfo _info,
      Action _onClickAction,
      Action<string> _onPointerEnter,
      Action _onPointerExit)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomPushScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new CustomPushScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onClickAction = _onClickAction;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onPointerEnter = _onPointerEnter;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._info = _info;
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onPointerExit = _onPointerExit;
      // ISSUE: reference to a compiler-generated field
      bool active = dataCAnonStorey0._info != null;
      ((Component) this.rows[_index].btn).get_gameObject().SetActiveIfDifferent(active);
      ((UnityEventBase) this.rows[_index].btn.get_onClick()).RemoveAllListeners();
      if (!active)
        return;
      ((UnityEventBase) this.rows[_index].btn.get_onClick()).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent) this.rows[_index].btn.get_onClick()).AddListener(new UnityAction((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      EventTriggerNoScroll eventTriggerNoScroll = (EventTriggerNoScroll) ((Component) this.rows[_index].btn).get_gameObject().AddComponent<EventTriggerNoScroll>();
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(dataCAnonStorey0._info.assetBundle, dataCAnonStorey0._info.assetName, false, string.Empty);
      if (Object.op_Implicit((Object) texture2D))
        this.rows[_index].imgThumb.set_sprite(Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), new Vector2(0.5f, 0.5f)));
      else
        this.rows[_index].imgThumb.set_sprite((Sprite) null);
      // ISSUE: reference to a compiler-generated field
      this.rows[_index].info = dataCAnonStorey0._info;
    }

    public CustomPushInfo GetListInfo(int _index)
    {
      return this.rows[_index].info;
    }

    [Serializable]
    public class RowInfo
    {
      public Button btn;
      public Image imgThumb;
      public CustomPushInfo info;
    }
  }
}
