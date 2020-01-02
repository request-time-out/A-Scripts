// Decompiled with JetBrains decompiler
// Type: Housing.GuideMove
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Housing.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Housing
{
  public class GuideMove : GuideBase, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
  {
    private Vector2 oldPos = Vector2.get_zero();
    public GuideMove.MoveAxis axis;
    [SerializeField]
    private Transform transformRoot;
    public GuideMove.MoveCalc moveCalc;
    public Action onDragAction;
    public Action onEndDragAction;
    private Camera m_Camera;
    private Dictionary<ObjectCtrl, GuideMove.PosInfo> dicTargetAndOld;
    private bool isLocal;
    private bool isDragButton;

    private Camera camera
    {
      get
      {
        return this.m_Camera ?? (this.m_Camera = Camera.get_main());
      }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
      if (eventData.get_button() != null)
        return;
      base.OnPointerDown(eventData);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
      if (!this.isDragButton)
        this.isDragButton = eventData.get_button() == 0;
      if (!this.isDragButton)
      {
        eventData.set_dragging(false);
      }
      else
      {
        this.oldPos = eventData.get_pressPosition();
        this.dicTargetAndOld = ((IEnumerable<ObjectCtrl>) Singleton<Selection>.Instance.SelectObjects).Where<ObjectCtrl>((Func<ObjectCtrl, bool>) (v => v is OCItem)).ToDictionary<ObjectCtrl, ObjectCtrl, GuideMove.PosInfo>((Func<ObjectCtrl, ObjectCtrl>) (v => v), (Func<ObjectCtrl, GuideMove.PosInfo>) (v => new GuideMove.PosInfo(v.Position)));
        this.isLocal = Singleton<GuideManager>.IsInstance() && Object.op_Implicit((Object) Singleton<GuideManager>.Instance.TransformRoot);
      }
    }

    public override void OnDrag(PointerEventData eventData)
    {
      if (!this.isDragButton || eventData.get_button() != null)
        return;
      base.OnDrag(eventData);
      switch (this.axis)
      {
        case GuideMove.MoveAxis.X:
        case GuideMove.MoveAxis.Y:
        case GuideMove.MoveAxis.Z:
          Vector3 _move1 = Vector3.op_Subtraction(this.WorldPos(eventData.get_position()), this.WorldPos(this.oldPos));
          Vector3 vector3_1 = !this.isLocal ? this.AxisMove(_move1) : this.AxisLocalMove(_move1);
          bool flag = false;
          foreach (KeyValuePair<ObjectCtrl, GuideMove.PosInfo> keyValuePair in this.dicTargetAndOld)
          {
            keyValuePair.Value.work = keyValuePair.Key.Position;
            Vector3 _pos = Vector3.op_Addition(keyValuePair.Value.old, vector3_1);
            if (Singleton<GuideManager>.IsInstance())
            {
              if (Singleton<GuideManager>.Instance.CorrectPos(keyValuePair.Key, ref _pos))
              {
                keyValuePair.Value.sub = this.InverseTransformVector(Vector3.op_Subtraction(Vector3.op_Addition(keyValuePair.Value.old, vector3_1), _pos));
                keyValuePair.Value.IsHit = true;
                flag = true;
              }
              else
                keyValuePair.Value.IsHit = false;
            }
            keyValuePair.Key.Position = _pos;
          }
          if (flag)
          {
            IEnumerable<Vector3> source = this.dicTargetAndOld.Where<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>>((Func<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>, bool>) (v => v.Value.IsHit)).Select<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>, Vector3>((Func<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>, Vector3>) (v => v.Value.sub));
            Vector3 vector3_2;
            if ((double) ((Vector3) ref vector3_1).get_Item((int) this.axis) > 0.0)
            {
              Vector3 _move2 = source.First<Vector3>();
              using (IEnumerator<Vector3> enumerator = source.GetEnumerator())
              {
                while (((IEnumerator) enumerator).MoveNext())
                {
                  Vector3 current = enumerator.Current;
                  _move2 = Vector3.Max(_move2, current);
                }
              }
              vector3_2 = Vector3.op_Subtraction(vector3_1, this.TransformVector(_move2));
            }
            else
            {
              Vector3 _move2 = source.First<Vector3>();
              using (IEnumerator<Vector3> enumerator = source.GetEnumerator())
              {
                while (((IEnumerator) enumerator).MoveNext())
                {
                  Vector3 current = enumerator.Current;
                  _move2 = Vector3.Min(_move2, current);
                }
              }
              vector3_2 = Vector3.op_Subtraction(vector3_1, this.TransformVector(_move2));
            }
            using (Dictionary<ObjectCtrl, GuideMove.PosInfo>.Enumerator enumerator = this.dicTargetAndOld.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                KeyValuePair<ObjectCtrl, GuideMove.PosInfo> current = enumerator.Current;
                Vector3 _pos = Vector3.op_Addition(current.Value.old, vector3_2);
                if (Singleton<GuideManager>.IsInstance())
                  Singleton<GuideManager>.Instance.CorrectPos(current.Key, ref _pos);
                current.Key.Position = _pos;
              }
              break;
            }
          }
          else
            break;
      }
      if (this.onDragAction == null)
        return;
      this.onDragAction();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
      if (this.isDragButton && eventData.get_dragging())
        return;
      base.OnPointerUp(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
      if (!this.isDragButton)
      {
        eventData.set_dragging(false);
        base.OnEndDrag(eventData);
      }
      else
      {
        if (eventData.get_button() != null)
          return;
        base.OnEndDrag(eventData);
        if (Singleton<UndoRedoManager>.IsInstance())
          Singleton<UndoRedoManager>.Instance.Push((ICommand) new MoveCommand(this.dicTargetAndOld.Select<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>, MoveCommand.Info>((Func<KeyValuePair<ObjectCtrl, GuideMove.PosInfo>, MoveCommand.Info>) (v => new MoveCommand.Info(v.Key, v.Value.old))).ToArray<MoveCommand.Info>()));
        foreach (KeyValuePair<ObjectCtrl, GuideMove.PosInfo> keyValuePair in this.dicTargetAndOld)
          Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (keyValuePair.Key as OCItem));
        Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
        if (this.onEndDragAction == null)
          return;
        this.onEndDragAction();
      }
    }

    private Vector3 WorldPos(Vector2 _screenPos)
    {
      Plane plane;
      ((Plane) ref plane).\u002Ector(Vector3.op_Multiply(((Component) this.camera).get_transform().get_forward(), -1f), ((Component) this).get_transform().get_position());
      Ray ray = RectTransformUtility.ScreenPointToRay(this.camera, _screenPos);
      float num = 0.0f;
      return ((Plane) ref plane).Raycast(ray, ref num) ? ((Ray) ref ray).GetPoint(num) : ((Component) this).get_transform().get_position();
    }

    private Vector3 AxisMove(Vector3 _move)
    {
      return this.Floor(Vector3.Scale(((Component) this).get_transform().get_up(), _move));
    }

    private Vector3 AxisLocalMove(Vector3 _move)
    {
      if (!Singleton<GuideManager>.IsInstance() || !Object.op_Implicit((Object) Singleton<GuideManager>.Instance.TransformRoot))
        return Vector3.get_zero();
      Transform transformRoot = Singleton<GuideManager>.Instance.TransformRoot;
      Vector3 vector3_1 = transformRoot.InverseTransformDirection(((Component) this).get_transform().get_up());
      Vector3 vector3_2 = transformRoot.InverseTransformVector(_move);
      return transformRoot.TransformVector(this.Floor(Vector3.Scale(vector3_1, vector3_2)));
    }

    private Vector3 InverseTransformVector(Vector3 _move)
    {
      return !Singleton<GuideManager>.IsInstance() || !Object.op_Implicit((Object) Singleton<GuideManager>.Instance.TransformRoot) ? _move : Singleton<GuideManager>.Instance.TransformRoot.InverseTransformVector(_move);
    }

    private Vector3 TransformVector(Vector3 _move)
    {
      return !Singleton<GuideManager>.IsInstance() || !Object.op_Implicit((Object) Singleton<GuideManager>.Instance.TransformRoot) ? _move : Singleton<GuideManager>.Instance.TransformRoot.TransformVector(_move);
    }

    private float Floor(float _value)
    {
      bool flag = (double) _value < 0.0;
      return Mathf.Floor(Mathf.Abs(_value)) * (!flag ? 1f : -1f);
    }

    private Vector3 Floor(Vector3 _value)
    {
      return new Vector3(this.Floor((float) _value.x), this.Floor((float) _value.y), this.Floor((float) _value.z));
    }

    public enum MoveAxis
    {
      X,
      Y,
      Z,
    }

    public enum MoveCalc
    {
      TYPE1,
      TYPE2,
    }

    private class PosInfo
    {
      public Vector3 old = Vector3.get_zero();
      public Vector3 work = Vector3.get_zero();
      public Vector3 sub = Vector3.get_zero();

      public PosInfo(Vector3 _old)
      {
        this.old = _old;
      }

      public bool IsHit { get; set; }
    }
  }
}
