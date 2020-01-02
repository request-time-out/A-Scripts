// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchButtonExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (Image))]
  public class TouchButtonExample : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    public bool allowMouseControl;

    public TouchButtonExample()
    {
      base.\u002Ector();
    }

    public bool isPressed { get; private set; }

    private void Awake()
    {
      if (SystemInfo.get_deviceType() != 1)
        return;
      this.allowMouseControl = false;
    }

    private void Restart()
    {
      this.isPressed = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.get_pointerId()))
        return;
      this.isPressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      if (!this.allowMouseControl && TouchButtonExample.IsMousePointerId(eventData.get_pointerId()))
        return;
      this.isPressed = false;
    }

    private static bool IsMousePointerId(int id)
    {
      return id == -1 || id == -2 || id == -3;
    }
  }
}
