// Decompiled with JetBrains decompiler
// Type: CSS_UI_NoControlCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class CSS_UI_NoControlCamera : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private BaseCameraControl camCtrl;
  private bool over;

  public CSS_UI_NoControlCamera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!Object.op_Equality((Object) null, (Object) this.camCtrl))
      return;
    GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("MainCamera");
    if (!Object.op_Implicit((Object) gameObjectWithTag))
      return;
    this.camCtrl = (BaseCameraControl) gameObjectWithTag.GetComponent<BaseCameraControl>();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.over = true;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.over = false;
  }

  public void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      if (!Object.op_Implicit((Object) this.camCtrl))
        return;
      this.camCtrl.NoCtrlCondition = (BaseCameraControl.NoCtrlFunc) (() => false);
    }
    else
    {
      if (!Input.GetMouseButtonDown(0) || !this.over || !Object.op_Implicit((Object) this.camCtrl))
        return;
      this.camCtrl.NoCtrlCondition = (BaseCameraControl.NoCtrlFunc) (() => true);
    }
  }
}
