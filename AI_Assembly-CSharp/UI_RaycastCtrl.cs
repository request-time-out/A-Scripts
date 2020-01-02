// Decompiled with JetBrains decompiler
// Type: UI_RaycastCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_RaycastCtrl : MonoBehaviour
{
  [SerializeField]
  private CanvasGroup[] canvasGrp;
  [SerializeField]
  private Image[] imgRaycastTargetOn;

  public UI_RaycastCtrl()
  {
    base.\u002Ector();
  }

  private void GetImageComponents()
  {
    this.imgRaycastTargetOn = ((IEnumerable<Image>) ((Component) this).GetComponentsInChildren<Image>(true)).Where<Image>((Func<Image, bool>) (img => ((Object) ((Component) img).get_gameObject()).get_name() != "Viewport")).Where<Image>((Func<Image, bool>) (img => ((Graphic) img).get_raycastTarget())).ToArray<Image>();
  }

  private void GetCanvasGroup()
  {
    List<CanvasGroup> canvasGroupList = new List<CanvasGroup>();
    CanvasGroup component1 = (CanvasGroup) ((Component) this).GetComponent<CanvasGroup>();
    if (Object.op_Inequality((Object) null, (Object) component1))
      canvasGroupList.Add(component1);
    Transform parent = ((Component) this).get_transform().get_parent();
    if (Object.op_Equality((Object) null, (Object) parent))
      return;
    while (true)
    {
      CanvasGroup component2 = (CanvasGroup) ((Component) parent).GetComponent<CanvasGroup>();
      if (Object.op_Inequality((Object) null, (Object) component2))
        canvasGroupList.Add(component2);
      if (!Object.op_Equality((Object) null, (Object) parent.get_parent()))
        parent = parent.get_parent();
      else
        break;
    }
    this.canvasGrp = canvasGroupList.ToArray();
  }

  public void ChangeRaycastTarget(bool enable)
  {
    foreach (Graphic graphic in this.imgRaycastTargetOn)
      graphic.set_raycastTarget(enable);
  }

  public void Reset()
  {
    this.GetCanvasGroup();
    this.GetImageComponents();
  }

  private void Update()
  {
    if (this.canvasGrp == null || this.imgRaycastTargetOn == null)
      return;
    bool enable = true;
    foreach (CanvasGroup canvasGroup in this.canvasGrp)
    {
      if (!canvasGroup.get_blocksRaycasts())
      {
        enable = false;
        break;
      }
    }
    this.ChangeRaycastTarget(enable);
  }
}
