// Decompiled with JetBrains decompiler
// Type: Studio.GuideMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public class GuideMove : GuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 oldPos = Vector2.get_zero();
    private bool isSnap = true;
    public GuideMove.MoveAxis axis;
    [SerializeField]
    private Transform transformRoot;
    public GuideMove.MoveCalc moveCalc;
    public Action onDragAction;
    public Action onEndDragAction;
    private Camera m_Camera;
    private Dictionary<int, Vector3> dicOld;

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

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
      this.oldPos = eventData.get_pressPosition();
      this.dicOld = Singleton<GuideObjectManager>.Instance.selectObjectDictionary.ToDictionary<KeyValuePair<int, ChangeAmount>, int, Vector3>((Func<KeyValuePair<int, ChangeAmount>, int>) (v => v.Key), (Func<KeyValuePair<int, ChangeAmount>, Vector3>) (v => v.Value.pos));
      this.isSnap = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
      base.OnDrag(eventData);
      switch (this.axis)
      {
        case GuideMove.MoveAxis.X:
        case GuideMove.MoveAxis.Y:
        case GuideMove.MoveAxis.Z:
          bool _snap = false;
          Vector3 vector3_1 = this.AxisMove(eventData.get_delta(), ref _snap);
          foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            selectObject.MoveLocal(vector3_1, _snap, this.axis);
          break;
        case GuideMove.MoveAxis.XYZ:
          Vector3 vector3_2 = Vector3.op_Subtraction(this.WorldPos(eventData.get_position()), this.WorldPos(this.oldPos));
          foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            selectObject.MoveWorld(vector3_2);
          break;
        case GuideMove.MoveAxis.XY:
        case GuideMove.MoveAxis.YZ:
        case GuideMove.MoveAxis.XZ:
          Vector3 vector3_3 = Vector3.op_Subtraction(this.PlanePos(eventData.get_position()), this.PlanePos(this.oldPos));
          foreach (GuideObject selectObject in Singleton<GuideObjectManager>.Instance.selectObjects)
            selectObject.MoveWorld(vector3_3);
          break;
      }
      this.oldPos = eventData.get_position();
      if (this.onDragAction == null)
        return;
      this.onDragAction();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
      base.OnEndDrag(eventData);
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.MoveEqualsCommand(Singleton<GuideObjectManager>.Instance.selectObjectDictionary.Select<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>((Func<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.Key,
        oldValue = this.dicOld[v.Key],
        newValue = v.Value.pos
      })).ToArray<GuideCommand.EqualsInfo>()));
      if (this.onEndDragAction == null)
        return;
      this.onEndDragAction();
    }

    private Vector3 WorldPos(Vector2 _screenPos)
    {
      Plane plane;
      ((Plane) ref plane).\u002Ector(Vector3.op_Multiply(((Component) this.camera).get_transform().get_forward(), -1f), ((Component) this).get_transform().get_position());
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      return ((Plane) ref plane).Raycast(ray, ref num) ? ((Ray) ref ray).GetPoint(num) : ((Component) this).get_transform().get_position();
    }

    private Vector3 PlanePos(Vector2 _screenPos)
    {
      Plane plane;
      ((Plane) ref plane).\u002Ector(((Component) this).get_transform().get_up(), ((Component) this).get_transform().get_position());
      Ray ray = RectTransformUtility.ScreenPointToRay(Camera.get_main(), _screenPos);
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
      Vector3 vector3 = ((Component) this.camera).get_transform().TransformVector((float) (_delta.x * 0.00499999988824129), (float) (_delta.y * 0.00499999988824129), 0.0f);
      if (Input.GetKey((KeyCode) 118) && (double) ((Vector3) ref vector3).get_Item((int) this.axis) != 0.0)
      {
        float num = (double) ((Vector3) ref vector3).get_Item((int) this.axis) >= 0.0 ? 1f : -1f;
        vector3 = Vector3.get_zero();
        if (this.isSnap)
        {
          int index = Mathf.Clamp(Studio.Studio.optionSystem.snap, 0, 2);
          float[] numArray = new float[3]{ 0.01f, 0.1f, 1f };
          ((Vector3) ref vector3).set_Item((int) this.axis, numArray[index] * num);
          this.isSnap = false;
          DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.Timer(TimeSpan.FromMilliseconds(50.0)), (Action<M0>) (_ => this.isSnap = true)), (Component) this);
          _snap = true;
        }
      }
      else
        vector3 = Vector3.op_Multiply(vector3, Studio.Studio.optionSystem.manipuleteSpeed);
      switch (this.axis)
      {
        case GuideMove.MoveAxis.X:
          vector3 = this.moveCalc != GuideMove.MoveCalc.TYPE3 ? Vector3.Scale(this.transformRoot.get_right(), vector3) : this.transformRoot.TransformVector(Vector3.Scale(Vector3.get_right(), this.transformRoot.InverseTransformVector(vector3)));
          break;
        case GuideMove.MoveAxis.Y:
          vector3 = this.moveCalc != GuideMove.MoveCalc.TYPE3 ? Vector3.Scale(this.transformRoot.get_up(), vector3) : this.transformRoot.TransformVector(Vector3.Scale(Vector3.get_up(), this.transformRoot.InverseTransformVector(vector3)));
          break;
        case GuideMove.MoveAxis.Z:
          vector3 = this.moveCalc != GuideMove.MoveCalc.TYPE3 ? Vector3.Scale(this.transformRoot.get_forward(), vector3) : this.transformRoot.TransformVector(Vector3.Scale(Vector3.get_forward(), this.transformRoot.InverseTransformVector(vector3)));
          break;
      }
      return vector3;
    }

    private Vector3 Parse(Vector3 _src)
    {
      string format = string.Format("F{0}", (object) (2 - Studio.Studio.optionSystem.snap));
      ((Vector3) ref _src).set_Item((int) this.axis, float.Parse(((Vector3) ref _src).get_Item((int) this.axis).ToString(format)));
      return _src;
    }

    private void CalcType1(KeyValuePair<ChangeAmount, Transform> _pair, Vector3 _move, bool _snap)
    {
      Vector3 _src = Vector3.op_Addition(_pair.Key.pos, _move);
      _pair.Key.pos = !_snap ? _src : this.Parse(_src);
    }

    private void CalcType2(KeyValuePair<ChangeAmount, Transform> _pair, Vector3 _move, bool _snap)
    {
      Vector3 _src = Vector3.op_Addition(_pair.Key.pos, _pair.Value.InverseTransformVector(_move));
      _pair.Key.pos = !_snap ? _src : this.Parse(_src);
    }

    public enum MoveAxis
    {
      X,
      Y,
      Z,
      XYZ,
      XY,
      YZ,
      XZ,
    }

    public enum MoveCalc
    {
      TYPE1,
      TYPE2,
      TYPE3,
    }
  }
}
