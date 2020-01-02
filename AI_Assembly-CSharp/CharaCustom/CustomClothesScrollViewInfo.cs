// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomClothesScrollViewInfo
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
  public class CustomClothesScrollViewInfo : MonoBehaviour
  {
    [SerializeField]
    private CustomClothesScrollViewInfo.RowInfo[] rows;
    [SerializeField]
    private Texture2D texEmpty;

    public CustomClothesScrollViewInfo()
    {
      base.\u002Ector();
    }

    public void SetData(
      int _index,
      CustomClothesScrollController.ScrollData _data,
      Action<bool> _onClickAction,
      Action<string> _onPointerEnter,
      Action _onPointerExit)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CustomClothesScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new CustomClothesScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0();
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
      if (dataCAnonStorey0._info.pngData != null || !dataCAnonStorey0._info.FullPath.IsNullOrEmpty())
      {
        if (Object.op_Implicit((Object) this.rows[_index].imgThumb.get_texture()))
          Object.Destroy((Object) this.rows[_index].imgThumb.get_texture());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.rows[_index].imgThumb.set_texture((Texture) PngAssist.ChangeTextureFromByte(dataCAnonStorey0._info.pngData ?? PngFile.LoadPngBytes(dataCAnonStorey0._info.FullPath), 0, 0, (TextureFormat) 5, false));
      }
      else
        this.rows[_index].imgThumb.set_texture((Texture) this.texEmpty);
      // ISSUE: reference to a compiler-generated field
      this.rows[_index].info = dataCAnonStorey0._info;
    }

    public void SetToggleON(int _index, bool _isOn)
    {
      this.rows[_index].tgl.SetIsOnWithoutCallback(_isOn);
    }

    public CustomClothesFileInfo GetListInfo(int _index)
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
      public RawImage imgThumb;
      public CustomClothesFileInfo info;
    }
  }
}
