// Decompiled with JetBrains decompiler
// Type: UI_NoControlCraftCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using UnityEngine;

public class UI_NoControlCraftCamera : MonoBehaviour
{
  public RectTransform rtCanvas;
  private CraftCamera camCtrl;

  public UI_NoControlCraftCamera()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SearchCanvas();
    if (!Object.op_Equality((Object) null, (Object) this.camCtrl) || !Object.op_Implicit((Object) Camera.get_main()))
      return;
    this.camCtrl = (CraftCamera) ((Component) Camera.get_main()).GetComponent<CraftCamera>();
  }

  private void SearchCanvas()
  {
    GameObject gameObject;
    for (gameObject = ((Component) this).get_gameObject(); !Object.op_Implicit((Object) gameObject.GetComponent<Canvas>()); gameObject = ((Component) gameObject.get_transform().get_parent()).get_gameObject())
    {
      if (Object.op_Equality((Object) null, (Object) gameObject.get_transform().get_parent()))
        return;
    }
    this.rtCanvas = gameObject.get_transform() as RectTransform;
  }

  public void Update()
  {
    if (!Singleton<Input>.Instance.IsDown(ActionID.MouseLeft) && !Singleton<Input>.Instance.IsDown(ActionID.MouseRight))
    {
      if (Object.op_Implicit((Object) this.camCtrl))
        this.camCtrl.NoCtrlCondition = (VirtualCameraController.NoCtrlFunc) (() => false);
    }
    else if (Singleton<Input>.Instance.IsDown(ActionID.MouseLeft) || Singleton<Input>.Instance.IsDown(ActionID.MouseRight))
    {
      if (Object.op_Equality((Object) null, (Object) this.rtCanvas))
        return;
      RectTransform transform = ((Component) this).get_transform() as RectTransform;
      float x = (float) Input.get_mousePosition().x;
      float y = (float) Input.get_mousePosition().y;
      if (((Transform) transform).get_position().x <= (double) x && (double) x <= ((Transform) transform).get_position().x + transform.get_sizeDelta().x * ((Transform) this.rtCanvas).get_localScale().x && (((Transform) transform).get_position().y >= (double) y && ((double) y >= ((Transform) transform).get_position().y - transform.get_sizeDelta().y * ((Transform) this.rtCanvas).get_localScale().y && Object.op_Implicit((Object) this.camCtrl))))
        this.camCtrl.NoCtrlCondition = (VirtualCameraController.NoCtrlFunc) (() => true);
    }
    if (Mathf.Approximately(Singleton<Input>.Instance.ScrollValue(), 0.0f))
    {
      if (!Object.op_Implicit((Object) this.camCtrl))
        return;
      this.camCtrl.ZoomCondition = (VirtualCameraController.NoCtrlFunc) (() => false);
    }
    else
    {
      if (Object.op_Equality((Object) null, (Object) this.rtCanvas))
        return;
      RectTransform transform = ((Component) this).get_transform() as RectTransform;
      float x = (float) Input.get_mousePosition().x;
      float y = (float) Input.get_mousePosition().y;
      if (((Transform) transform).get_position().x > (double) x || (double) x > ((Transform) transform).get_position().x + transform.get_sizeDelta().x * ((Transform) this.rtCanvas).get_localScale().x || (((Transform) transform).get_position().y < (double) y || ((double) y < ((Transform) transform).get_position().y - transform.get_sizeDelta().y * ((Transform) this.rtCanvas).get_localScale().y || !Object.op_Implicit((Object) this.camCtrl))))
        return;
      this.camCtrl.ZoomCondition = (VirtualCameraController.NoCtrlFunc) (() => true);
    }
  }
}
