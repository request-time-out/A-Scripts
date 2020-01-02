// Decompiled with JetBrains decompiler
// Type: Housing.GuideRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Housing.Command;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Housing
{
  public class GuideRotation : GuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 prevScreenPos = Vector2.get_zero();
    private Vector3 prevPlanePos = Vector3.get_zero();
    private Vector3 oldRot = Vector3.get_zero();
    private Vector3 workRot = Vector3.get_zero();
    public GuideRotation.RotationAxis axis;
    private ObjectCtrl objectCtrl;
    private Camera m_Camera;
    private bool isDragButton;

    private Camera camera
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Camera, (Object) null))
          this.m_Camera = Camera.get_main();
        return this.m_Camera;
      }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      base.OnPointerDown(eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData _eventData)
    {
      if (!this.isDragButton)
        this.isDragButton = _eventData.get_button() == 0;
      this.isDragButton = _eventData.get_button() == 0;
      if (!this.isDragButton)
        return;
      this.prevScreenPos = _eventData.get_position();
      this.prevPlanePos = this.PlanePos(_eventData.get_position());
      this.objectCtrl = this.GuideObject.ObjectCtrl;
      this.oldRot = this.objectCtrl.LocalEulerAngles;
      this.workRot = this.oldRot;
    }

    public override void OnDrag(PointerEventData _eventData)
    {
      if (!this.isDragButton)
        return;
      base.OnDrag(_eventData);
      Vector3 zero = Vector3.get_zero();
      if ((double) Mathf.Abs(Vector3.Dot(((Component) this.camera).get_transform().get_forward(), ((Component) this).get_transform().get_right())) > 0.100000001490116)
      {
        Vector3 vector3_1 = this.PlanePos(_eventData.get_position());
        Vector3 vector3_2 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(this.prevPlanePos));
        Vector3 vector3_3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, 90f, 0.0f), ((Component) this).get_transform().InverseTransformPoint(vector3_1));
        float angle = this.VectorToAngle(new Vector2((float) vector3_2.x, (float) vector3_2.y), new Vector2((float) vector3_3.x, (float) vector3_3.y));
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
        ((Vector3) ref zero).set_Item((int) this.axis, angle);
        this.prevPlanePos = vector3_3;
      }
      this.prevScreenPos = _eventData.get_position();
      Quaternion quaternion = Quaternion.op_Multiply(Quaternion.Euler(this.workRot), Quaternion.Euler(zero));
      Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
      eulerAngles.x = (__Null) (eulerAngles.x % 360.0);
      eulerAngles.y = (__Null) (eulerAngles.y % 360.0);
      eulerAngles.z = (__Null) (eulerAngles.z % 360.0);
      this.workRot = eulerAngles;
      eulerAngles.y = (__Null) (double) this.Round((float) eulerAngles.y);
      Vector3 localEulerAngles = this.objectCtrl.LocalEulerAngles;
      this.objectCtrl.LocalEulerAngles = eulerAngles;
      if (!Singleton<GuideManager>.IsInstance() || Singleton<GuideManager>.Instance.CheckRot(this.objectCtrl))
        return;
      this.objectCtrl.LocalEulerAngles = localEulerAngles;
    }

    public override void OnPointerUp(PointerEventData _eventData)
    {
      if (this.isDragButton && _eventData.get_dragging())
        return;
      base.OnPointerUp(_eventData);
    }

    public override void OnEndDrag(PointerEventData _eventData)
    {
      if (!this.isDragButton)
      {
        _eventData.set_dragging(false);
        base.OnEndDrag(_eventData);
      }
      else
      {
        if (_eventData.get_button() != null)
          return;
        base.OnEndDrag(_eventData);
        if (Singleton<UndoRedoManager>.IsInstance())
          Singleton<UndoRedoManager>.Instance.Push((ICommand) new RotationCommand(this.objectCtrl, this.oldRot));
        Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (this.objectCtrl as OCItem));
        Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
      }
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

    private float Round(float _value)
    {
      bool flag = (double) _value < 0.0;
      return (float) ((double) Mathf.RoundToInt(Mathf.Abs(_value) / 90f) * 90.0 * (!flag ? 1.0 : -1.0));
    }

    public enum RotationAxis
    {
      X,
      Y,
      Z,
      XYZ,
    }
  }
}
