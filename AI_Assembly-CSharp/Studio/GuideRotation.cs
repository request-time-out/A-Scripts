// Decompiled with JetBrains decompiler
// Type: Studio.GuideRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideRotation : GuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 prevScreenPos = Vector2.get_zero();
    private Vector3 prevPlanePos = Vector3.get_zero();
    public GuideRotation.RotationAxis axis;
    private Dictionary<int, Vector3> dicOldRot;
    private Dictionary<int, ChangeAmount> dicChangeAmount;
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
    }

    public void OnInitializePotentialDrag(PointerEventData _eventData)
    {
      this.prevScreenPos = _eventData.get_position();
      this.prevPlanePos = this.PlanePos(_eventData.get_position());
      this.dicChangeAmount = Singleton<GuideObjectManager>.Instance.selectObjectDictionary;
      this.dicOldRot = this.dicChangeAmount.ToDictionary<KeyValuePair<int, ChangeAmount>, int, Vector3>((Func<KeyValuePair<int, ChangeAmount>, int>) (v => v.Key), (Func<KeyValuePair<int, ChangeAmount>, Vector3>) (v => v.Value.rot));
    }

    public override void OnDrag(PointerEventData _eventData)
    {
      base.OnDrag(_eventData);
      if (this.axis == GuideRotation.RotationAxis.XYZ)
      {
        foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
        {
          selectObject.Rotation(((Component) this.camera).get_transform().get_up(), (float) -_eventData.get_delta().x);
          selectObject.Rotation(((Component) this.camera).get_transform().get_right(), (float) _eventData.get_delta().y);
        }
      }
      else
      {
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
        foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicChangeAmount)
        {
          Quaternion quaternion = Quaternion.op_Multiply(Quaternion.Euler(keyValuePair.Value.rot), Quaternion.Euler(zero));
          Vector3 eulerAngles = ((Quaternion) ref quaternion).get_eulerAngles();
          eulerAngles.x = (__Null) (eulerAngles.x % 360.0);
          eulerAngles.y = (__Null) (eulerAngles.y % 360.0);
          eulerAngles.z = (__Null) (eulerAngles.z % 360.0);
          keyValuePair.Value.rot = eulerAngles;
        }
      }
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
    }

    public override void OnEndDrag(PointerEventData _eventData)
    {
      base.OnEndDrag(_eventData);
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.RotationEqualsCommand(((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, GuideCommand.EqualsInfo>((Func<int, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v,
        oldValue = this.dicOldRot[v],
        newValue = this.dicChangeAmount[v].rot
      })).ToArray<GuideCommand.EqualsInfo>()));
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
      XYZ,
    }
  }
}
