// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchJoystickExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  [RequireComponent(typeof (Image))]
  public class TouchJoystickExample : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
  {
    public bool allowMouseControl;
    public int radius;
    private Vector2 origAnchoredPosition;
    private Vector3 origWorldPosition;
    private Vector2 origScreenResolution;
    private ScreenOrientation origScreenOrientation;
    [NonSerialized]
    private bool hasFinger;
    [NonSerialized]
    private int lastFingerId;

    public TouchJoystickExample()
    {
      base.\u002Ector();
    }

    public Vector2 position { get; private set; }

    private void Start()
    {
      if (SystemInfo.get_deviceType() == 1)
        this.allowMouseControl = false;
      this.StoreOrigValues();
    }

    private void Update()
    {
      if ((double) Screen.get_width() == this.origScreenResolution.x && (double) Screen.get_height() == this.origScreenResolution.y && Screen.get_orientation() == this.origScreenOrientation)
        return;
      this.Restart();
      this.StoreOrigValues();
    }

    private void Restart()
    {
      this.hasFinger = false;
      (((Component) this).get_transform() as RectTransform).set_anchoredPosition(this.origAnchoredPosition);
      this.position = Vector2.get_zero();
    }

    private void StoreOrigValues()
    {
      this.origAnchoredPosition = (((Component) this).get_transform() as RectTransform).get_anchoredPosition();
      this.origWorldPosition = ((Component) this).get_transform().get_position();
      this.origScreenResolution = new Vector2((float) Screen.get_width(), (float) Screen.get_height());
      this.origScreenOrientation = Screen.get_orientation();
    }

    private void UpdateValue(Vector3 value)
    {
      Vector3 vector3_1 = Vector3.op_Subtraction(this.origWorldPosition, value);
      vector3_1.y = -vector3_1.y;
      Vector3 vector3_2 = Vector3.op_Division(vector3_1, (float) this.radius);
      this.position = new Vector2((float) -vector3_2.x, (float) vector3_2.y);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
      if (this.hasFinger || !this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.get_pointerId()))
        return;
      this.hasFinger = true;
      this.lastFingerId = eventData.get_pointerId();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
      if (eventData.get_pointerId() != this.lastFingerId || !this.allowMouseControl && TouchJoystickExample.IsMousePointerId(eventData.get_pointerId()))
        return;
      this.Restart();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
      if (!this.hasFinger || eventData.get_pointerId() != this.lastFingerId)
        return;
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector((float) (eventData.get_position().x - this.origWorldPosition.x), (float) (eventData.get_position().y - this.origWorldPosition.y));
      Vector3 vector3_2 = Vector3.op_Addition(this.origWorldPosition, Vector3.ClampMagnitude(vector3_1, (float) this.radius));
      ((Component) this).get_transform().set_position(vector3_2);
      this.UpdateValue(vector3_2);
    }

    private static bool IsMousePointerId(int id)
    {
      return id == -1 || id == -2 || id == -3;
    }
  }
}
