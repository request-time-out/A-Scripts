// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomGuideMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace CharaCustom
{
  public class CustomGuideMove : CustomGuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 oldPos = Vector2.get_zero();
    [SerializeField]
    private CustomGuideLimit guidLimit;
    public CustomGuideMove.MoveAxis axis;
    private Camera m_Camera;

    private Camera camera
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Camera, (Object) null))
          this.m_Camera = Camera.get_main();
        return this.m_Camera;
      }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      this.oldPos = eventData.get_pressPosition();
    }

    public override void OnDrag(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      base.OnDrag(eventData);
      bool _snap = false;
      Vector3 _src = Vector3.op_Addition(this.guideObject.amount.position, this.axis != CustomGuideMove.MoveAxis.XYZ ? this.AxisMove(eventData.get_delta(), ref _snap) : Vector3.op_Subtraction(this.WorldPos(eventData.get_position()), this.WorldPos(this.oldPos)));
      if (Object.op_Inequality((Object) null, (Object) this.guidLimit) && this.guidLimit.limited)
      {
        Vector3 vector3_1 = this.guidLimit.trfParent.InverseTransformPoint(_src);
        float x1 = (float) this.guidLimit.limitMin.x;
        float x2 = (float) this.guidLimit.limitMax.x;
        if (this.guidLimit.limitMin.x > this.guidLimit.limitMax.x)
        {
          x1 = (float) this.guidLimit.limitMax.x;
          x2 = (float) this.guidLimit.limitMin.x;
        }
        vector3_1.x = (__Null) (double) Mathf.Clamp((float) vector3_1.x, x1, x2);
        float y1 = (float) this.guidLimit.limitMin.y;
        float y2 = (float) this.guidLimit.limitMax.y;
        if (this.guidLimit.limitMin.y > this.guidLimit.limitMax.y)
        {
          y1 = (float) this.guidLimit.limitMax.y;
          y2 = (float) this.guidLimit.limitMin.y;
        }
        vector3_1.y = (__Null) (double) Mathf.Clamp((float) vector3_1.y, y1, y2);
        float z1 = (float) this.guidLimit.limitMin.z;
        float z2 = (float) this.guidLimit.limitMax.z;
        if (this.guidLimit.limitMin.z > this.guidLimit.limitMax.z)
        {
          z1 = (float) this.guidLimit.limitMax.z;
          z2 = (float) this.guidLimit.limitMin.z;
        }
        vector3_1.z = (__Null) (double) Mathf.Clamp((float) vector3_1.z, z1, z2);
        Vector3 vector3_2 = this.guidLimit.trfParent.TransformPoint(vector3_1);
        if (this.axis == CustomGuideMove.MoveAxis.XYZ || this.axis == CustomGuideMove.MoveAxis.X)
          _src.x = vector3_2.x;
        if (this.axis == CustomGuideMove.MoveAxis.XYZ || this.axis == CustomGuideMove.MoveAxis.Y)
          _src.y = vector3_2.y;
        if (this.axis == CustomGuideMove.MoveAxis.XYZ || this.axis == CustomGuideMove.MoveAxis.Z)
          _src.z = vector3_2.z;
      }
      this.guideObject.amount.position = this.axis != CustomGuideMove.MoveAxis.XYZ ? (!_snap ? _src : this.Parse(_src)) : _src;
      this.guideObject.ctrlAxisType = (int) this.axis;
      this.oldPos = eventData.get_position();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      CustomGuideAssist.SetCameraMoveFlag(this.guideObject.ccv2, false);
      this.guideObject.isDrag = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      CustomGuideAssist.SetCameraMoveFlag(this.guideObject.ccv2, true);
      this.guideObject.isDrag = false;
    }

    private Vector3 WorldPos(Vector2 _screenPos)
    {
      Plane plane;
      ((Plane) ref plane).\u002Ector(Vector3.op_Multiply(((Component) this.camera).get_transform().get_forward(), -1f), ((Component) this).get_transform().get_position());
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      return ((Plane) ref plane).Raycast(ray, ref num) ? ((Ray) ref ray).GetPoint(num) : ((Component) this).get_transform().get_position();
    }

    private Vector3 AxisPos(Vector2 _screenPos)
    {
      Vector3 position = ((Component) this).get_transform().get_position();
      Plane plane;
      ((Plane) ref plane).\u002Ector(((Component) this).get_transform().get_forward(), position);
      if (!((Plane) ref plane).GetSide(((Component) this.camera).get_transform().get_position()))
        ((Plane) ref plane).\u002Ector(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), -1f), position);
      Vector3 up = ((Component) this).get_transform().get_up();
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      return ((Plane) ref plane).Raycast(ray, ref num) ? Vector3.Project(((Ray) ref ray).GetPoint(num), up) : Vector3.Project(position, up);
    }

    private Vector3 AxisMove(Vector2 _delta, ref bool _snap)
    {
      Vector3 vector3 = ((Component) this.camera).get_transform().TransformVector((float) (_delta.x * 0.00999999977648258), (float) (_delta.y * 0.00999999977648258), 0.0f);
      Vector3 up = ((Component) this).get_transform().get_up();
      return Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(up, ((Vector3) ref vector3).get_magnitude()), this.guideObject.speedMove), Vector3.Dot(((Vector3) ref vector3).get_normalized(), up));
    }

    private Vector3 Parse(Vector3 _src)
    {
      return _src;
    }

    public enum MoveAxis
    {
      X,
      Y,
      Z,
      XYZ,
    }
  }
}
