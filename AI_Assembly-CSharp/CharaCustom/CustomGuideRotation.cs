// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomGuideRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace CharaCustom
{
  public class CustomGuideRotation : CustomGuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 prevScreenPos = Vector2.get_zero();
    private Vector3 prevPlanePos = Vector3.get_zero();
    public CustomGuideRotation.RotationAxis axis;
    [SerializeField]
    private CustomGuideLimit guidLimit;
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

    public void OnPointerDown(PointerEventData eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      CustomGuideAssist.SetCameraMoveFlag(this.guideObject.ccv2, false);
    }

    public void OnInitializePotentialDrag(PointerEventData _eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      this.guideObject.isDrag = true;
      this.prevScreenPos = _eventData.get_position();
      this.prevPlanePos = this.PlanePos(_eventData.get_position());
    }

    private float LimitedValue(CustomGuideRotation.RotationAxis axis, float val)
    {
      float num = 0.0f;
      switch (axis)
      {
        case CustomGuideRotation.RotationAxis.X:
          float x1 = (float) this.guidLimit.limitMin.x;
          float x2 = (float) this.guidLimit.limitMax.x;
          if (this.guidLimit.limitMin.x > this.guidLimit.limitMax.x)
          {
            x1 = (float) this.guidLimit.limitMax.x;
            x2 = (float) this.guidLimit.limitMin.x;
          }
          val = (double) val <= 180.0 ? val : val - 360f;
          num = Mathf.Clamp(val, x1, x2);
          break;
        case CustomGuideRotation.RotationAxis.Y:
          float y1 = (float) this.guidLimit.limitMin.y;
          float y2 = (float) this.guidLimit.limitMax.y;
          if (this.guidLimit.limitMin.y > this.guidLimit.limitMax.y)
          {
            y1 = (float) this.guidLimit.limitMax.y;
            y2 = (float) this.guidLimit.limitMin.y;
          }
          val = (double) val <= 180.0 ? val : val - 360f;
          num = Mathf.Clamp(val, y1, y2);
          break;
        case CustomGuideRotation.RotationAxis.Z:
          float z1 = (float) this.guidLimit.limitMin.z;
          float z2 = (float) this.guidLimit.limitMax.z;
          if (this.guidLimit.limitMin.z > this.guidLimit.limitMax.z)
          {
            z1 = (float) this.guidLimit.limitMax.z;
            z2 = (float) this.guidLimit.limitMin.z;
          }
          val = (double) val <= 180.0 ? val : val - 360f;
          num = Mathf.Clamp(val, z1, z2);
          break;
      }
      return num;
    }

    public override void OnDrag(PointerEventData _eventData)
    {
      if (CustomGuideAssist.IsCameraActionFlag(this.guideObject.ccv2))
        return;
      base.OnDrag(_eventData);
      Vector3 zero = Vector3.get_zero();
      if ((double) Mathf.Abs(Vector3.Dot(((Component) this.camera).get_transform().get_forward(), ((Component) this).get_transform().get_right())) > 0.100000001490116)
      {
        Vector3 vector3_1 = this.PlanePos(_eventData.get_position());
        Vector3 vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(this.prevPlanePos));
        Vector3 vector3_3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(vector3_1));
        float angle = this.VectorToAngle(new Vector2((float) vector3_2.x, (float) vector3_2.y), new Vector2((float) vector3_3.x, (float) vector3_3.y));
        if (Object.op_Inequality((Object) null, (Object) this.guidLimit) && this.guidLimit.limited)
          ((Vector3) ref zero).set_Item((int) this.axis, this.LimitedValue(this.axis, angle));
        else
          ((Vector3) ref zero).set_Item((int) this.axis, angle);
        this.prevPlanePos = vector3_1;
      }
      else
      {
        Vector3 vector3_1 = Vector2.op_Implicit(_eventData.get_position());
        vector3_1.z = (__Null) (double) Vector3.Distance(this.prevPlanePos, ((Component) this.camera).get_transform().get_position());
        Vector3 vector3_2 = Vector2.op_Implicit(this.prevScreenPos);
        vector3_2.z = (__Null) (double) Vector3.Distance(this.prevPlanePos, ((Component) this.camera).get_transform().get_position());
        Vector3 vector3_3 = Vector3.op_Addition(this.prevPlanePos, Vector3.op_Subtraction(this.camera.ScreenToWorldPoint(vector3_1), this.camera.ScreenToWorldPoint(vector3_2)));
        Vector3 vector3_4 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(this.prevPlanePos));
        Vector3 vector3_5 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(vector3_3));
        this.prevPlanePos = vector3_3;
        float angle = this.VectorToAngle(new Vector2((float) vector3_4.x, (float) vector3_4.y), new Vector2((float) vector3_5.x, (float) vector3_5.y));
        if (Object.op_Inequality((Object) null, (Object) this.guidLimit) && this.guidLimit.limited)
          ((Vector3) ref zero).set_Item((int) this.axis, this.LimitedValue(this.axis, angle));
        else
          ((Vector3) ref zero).set_Item((int) this.axis, angle);
        this.prevPlanePos = vector3_3;
      }
      this.prevScreenPos = _eventData.get_position();
      Quaternion quaternion = Quaternion.op_Multiply(Quaternion.Euler(this.guideObject.amount.rotation), Quaternion.Euler(zero));
      Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
      eulerAngles.x = (__Null) (eulerAngles.x % 360.0);
      eulerAngles.y = (__Null) (eulerAngles.y % 360.0);
      eulerAngles.z = (__Null) (eulerAngles.z % 360.0);
      this.guideObject.amount.rotation = eulerAngles;
      this.guideObject.ctrlAxisType = (int) this.axis;
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
      CustomGuideAssist.SetCameraMoveFlag(this.guideObject.ccv2, true);
      this.guideObject.isDrag = false;
    }

    private Vector3 PlanePos(Vector2 _screenPos)
    {
      Plane plane;
      ((Plane) ref plane).\u002Ector(((Component) this).get_transform().get_right(), ((Component) this).get_transform().get_position());
      if (!((Plane) ref plane).GetSide(((Component) this.camera).get_transform().get_position()))
        ((Plane) ref plane).SetNormalAndPosition(Vector3.op_Multiply(((Component) this).get_transform().get_right(), -1f), ((Component) this).get_transform().get_position());
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      return ((Plane) ref plane).Raycast(ray, ref num) ? ((Ray) ref ray).GetPoint(num) : ((Component) this).get_transform().get_position();
    }

    private float VectorToAngle(Vector2 _v1, Vector2 _v2)
    {
      return Mathf.DeltaAngle(Mathf.Atan2((float) _v1.x, (float) _v1.y) * 57.29578f, Mathf.Atan2((float) _v2.x, (float) _v2.y) * 57.29578f);
    }

    public enum RotationAxis
    {
      X,
      Y,
      Z,
    }
  }
}
