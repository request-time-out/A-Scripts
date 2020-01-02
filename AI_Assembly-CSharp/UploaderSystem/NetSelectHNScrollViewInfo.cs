// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetSelectHNScrollViewInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UploaderSystem
{
  [DisallowMultipleComponent]
  public class NetSelectHNScrollViewInfo : MonoBehaviour
  {
    [SerializeField]
    private NetSelectHNScrollViewInfo.RowInfo row;

    public NetSelectHNScrollViewInfo()
    {
      base.\u002Ector();
    }

    public void SetData(NetworkInfo.SelectHNInfo _data, Action<bool> _onValueChange)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetSelectHNScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0 dataCAnonStorey0 = new NetSelectHNScrollViewInfo.\u003CSetData\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      dataCAnonStorey0._onValueChange = _onValueChange;
      ((UnityEventBase) this.row.tglItem.onValueChanged).RemoveAllListeners();
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.row.tglItem.onValueChanged).AddListener(new UnityAction<bool>((object) dataCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      this.row.text.set_text(_data.drawname);
      this.row.info = _data;
    }

    public void SetToggleON(bool _isOn)
    {
      this.row.tglItem.set_isOn(_isOn);
    }

    public NetworkInfo.SelectHNInfo GetListInfo()
    {
      return this.row.info;
    }

    [Serializable]
    public class RowInfo
    {
      public Toggle tglItem;
      public Text text;
      public NetworkInfo.SelectHNInfo info;
    }
  }
}
