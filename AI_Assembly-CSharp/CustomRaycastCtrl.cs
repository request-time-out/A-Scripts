// Decompiled with JetBrains decompiler
// Type: CustomRaycastCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomRaycastCtrl : MonoBehaviour
{
  [Button("GetRaycastCtrlComponents", "取得", new object[] {})]
  public int getRaycastCtrlComponents;
  [Button("UpdateRaycastCtrl", "全更新", new object[] {})]
  public int updateRaycastCtrl;
  [SerializeField]
  private UI_RaycastCtrl[] raycastCtrl;

  public CustomRaycastCtrl()
  {
    base.\u002Ector();
  }

  private void GetRaycastCtrlComponents()
  {
    this.raycastCtrl = ((IEnumerable<UI_RaycastCtrl>) ((Component) this).GetComponentsInChildren<UI_RaycastCtrl>(true)).ToArray<UI_RaycastCtrl>();
  }

  private void UpdateRaycastCtrl()
  {
    foreach (UI_RaycastCtrl uiRaycastCtrl in this.raycastCtrl)
      uiRaycastCtrl.Reset();
  }

  private void Reset()
  {
    this.GetRaycastCtrlComponents();
  }
}
