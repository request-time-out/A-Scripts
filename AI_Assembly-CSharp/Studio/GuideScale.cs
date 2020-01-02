// Decompiled with JetBrains decompiler
// Type: Studio.GuideScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideScale : GuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private float speed = 1f / 1000f;
    private Vector2 prevPos = Vector2.get_zero();
    public GuideScale.ScaleAxis axis;
    [SerializeField]
    private Transform transformRoot;
    private Camera m_Camera;
    private Dictionary<int, Vector3> dicOldScale;
    private Dictionary<int, ChangeAmount> dicChangeAmount;

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
      this.prevPos = _eventData.get_position();
      this.dicChangeAmount = Singleton<GuideObjectManager>.Instance.selectObjectDictionary;
      this.dicOldScale = this.dicChangeAmount.ToDictionary<KeyValuePair<int, ChangeAmount>, int, Vector3>((Func<KeyValuePair<int, ChangeAmount>, int>) (v => v.Key), (Func<KeyValuePair<int, ChangeAmount>, Vector3>) (v => v.Value.scale));
    }

    public override void OnDrag(PointerEventData _eventData)
    {
      base.OnDrag(_eventData);
      Vector3.get_zero();
      Vector3 vector3_1;
      if (this.axis == GuideScale.ScaleAxis.XYZ)
      {
        Vector2 delta = _eventData.get_delta();
        vector3_1 = Vector3.op_Multiply(Vector3.get_one(), (float) (delta.x + delta.y) * this.speed);
      }
      else
        vector3_1 = this.AxisMove(_eventData.get_delta());
      foreach (KeyValuePair<int, ChangeAmount> keyValuePair in this.dicChangeAmount)
      {
        Vector3 vector3_2 = keyValuePair.Value.scale;
        vector3_2 = Vector3.op_Addition(vector3_2, vector3_1);
        vector3_2.x = (__Null) (double) Mathf.Clamp((float) vector3_2.x, 0.01f, 9999999f);
        vector3_2.y = (__Null) (double) Mathf.Clamp((float) vector3_2.y, 0.01f, 9999999f);
        vector3_2.z = (__Null) (double) Mathf.Clamp((float) vector3_2.z, 0.01f, 9999999f);
        keyValuePair.Value.scale = vector3_2;
      }
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
    }

    public override void OnEndDrag(PointerEventData _eventData)
    {
      base.OnEndDrag(_eventData);
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.ScaleEqualsCommand(((IEnumerable<int>) Singleton<GuideObjectManager>.Instance.selectObjectKey).Select<int, GuideCommand.EqualsInfo>((Func<int, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v,
        oldValue = this.dicOldScale[v],
        newValue = this.dicChangeAmount[v].scale
      })).ToArray<GuideCommand.EqualsInfo>()));
    }

    private Vector3 AxisPos(Vector2 _screenPos)
    {
      Vector3 position = ((Component) this).get_transform().get_position();
      Plane plane;
      ((Plane) ref plane).\u002Ector(Vector3.op_Multiply(((Component) this.camera).get_transform().get_forward(), -1f), position);
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      Vector3 vector3_1 = Vector3.op_Subtraction(!((Plane) ref plane).Raycast(ray, ref num) ? position : ((Ray) ref ray).GetPoint(num), position);
      Vector3 vector3_2 = ((Component) this).get_transform().get_up();
      switch (this.axis)
      {
        case GuideScale.ScaleAxis.X:
          vector3_2 = Vector3.get_right();
          break;
        case GuideScale.ScaleAxis.Y:
          vector3_2 = Vector3.get_up();
          break;
        case GuideScale.ScaleAxis.Z:
          vector3_2 = Vector3.get_forward();
          break;
      }
      return Vector3.Project(vector3_1, vector3_2);
    }

    private Vector3 AxisMove(Vector2 _delta)
    {
      Vector3 vector3 = ((Component) this).get_transform().InverseTransformVector(Vector3.op_Multiply(((Component) this.camera).get_transform().TransformVector((float) (_delta.x * 0.00499999988824129), (float) (_delta.y * 0.00499999988824129), 0.0f), Studio.Studio.optionSystem.manipuleteSpeed));
      switch (this.axis)
      {
        case GuideScale.ScaleAxis.X:
          vector3 = Vector3.Scale(vector3, Vector3.get_right());
          break;
        case GuideScale.ScaleAxis.Y:
          vector3 = Vector3.Scale(vector3, Vector3.get_up());
          break;
        case GuideScale.ScaleAxis.Z:
          vector3 = Vector3.Scale(vector3, Vector3.get_forward());
          break;
      }
      return vector3;
    }

    public enum ScaleAxis
    {
      X,
      Y,
      Z,
      XYZ,
    }
  }
}
