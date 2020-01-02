// Decompiled with JetBrains decompiler
// Type: Housing.InfoUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Text;
using UnityEngine;

namespace Housing
{
  [Serializable]
  public class InfoUICtrl : UIDerived
  {
    [SerializeField]
    private UnityEngine.UI.Text textInfo;

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      StringBuilder stringBuilder = new StringBuilder();
      if (Singleton<Manager.Map>.IsInstance())
        stringBuilder.AppendFormat("拠点{0}", (object) (Singleton<Manager.Map>.Instance.HousingID + 1));
      if (Singleton<Game>.IsInstance() && Singleton<Game>.Instance.WorldData != null && Singleton<Manager.Map>.IsInstance())
      {
        CraftInfo craftInfo = (CraftInfo) null;
        if (Singleton<Game>.Instance.WorldData.HousingData.CraftInfos.TryGetValue(Singleton<Manager.Map>.Instance.HousingID, out craftInfo))
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendFormat("範囲 {0:#} : {1:#} : {2:#}", (object) (float) craftInfo.LimitSize.x, (object) (float) craftInfo.LimitSize.y, (object) (float) craftInfo.LimitSize.z);
        }
      }
      this.textInfo.set_text(stringBuilder.ToString());
    }

    public override void UpdateUI()
    {
    }
  }
}
