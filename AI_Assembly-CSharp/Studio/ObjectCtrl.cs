// Decompiled with JetBrains decompiler
// Type: Studio.ObjectCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class ObjectCtrl : Singleton<ObjectCtrl>
  {
    [SerializeField]
    private float moveRate = 0.005f;
    [SerializeField]
    private MapDragButton[] mapDragButton;
    [SerializeField]
    private Transform transformBase;
    private Dictionary<int, Vector3> dicOld;

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        ((Component) this).get_gameObject().SetActive(value);
      }
    }

    private void OnBeginDragTrans()
    {
      this.dicOld = Singleton<GuideObjectManager>.Instance.selectObjectDictionary.ToDictionary<KeyValuePair<int, ChangeAmount>, int, Vector3>((Func<KeyValuePair<int, ChangeAmount>, int>) (v => v.Key), (Func<KeyValuePair<int, ChangeAmount>, Vector3>) (v => v.Value.pos));
    }

    private void OnEndDragTrans()
    {
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.MoveEqualsCommand(Singleton<GuideObjectManager>.Instance.selectObjectDictionary.Select<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>((Func<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.Key,
        oldValue = this.dicOld[v.Key],
        newValue = v.Value.pos
      })).ToArray<GuideCommand.EqualsInfo>()));
    }

    private void OnDragTransXZ()
    {
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(Input.GetAxis("Mouse X"), 0.0f, Input.GetAxis("Mouse Y"));
      Vector3 vector3_2 = ((Component) Camera.get_main()).get_transform().TransformVector(Vector3.op_Multiply(vector3_1, this.moveRate));
      vector3_2.y = (__Null) 0.0;
      foreach (GuideObject guideObject in ((IEnumerable<GuideObject>) Singleton<GuideObjectManager>.Instance.selectObjects).Where<GuideObject>((Func<GuideObject, bool>) (v => v.enablePos)))
        guideObject.MoveLocal(vector3_2);
    }

    private void OnDragTransY()
    {
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(0.0f, Input.GetAxis("Mouse Y"), 0.0f);
      Vector3 vector3_2 = ((Component) Camera.get_main()).get_transform().TransformVector(Vector3.op_Multiply(vector3_1, this.moveRate));
      vector3_2.x = (__Null) 0.0;
      vector3_2.z = (__Null) 0.0;
      foreach (GuideObject guideObject in ((IEnumerable<GuideObject>) Singleton<GuideObjectManager>.Instance.selectObjects).Where<GuideObject>((Func<GuideObject, bool>) (v => v.enablePos)))
        guideObject.MoveLocal(vector3_2);
    }

    private void OnBeginDragRot()
    {
      this.dicOld = Singleton<GuideObjectManager>.Instance.selectObjectDictionary.ToDictionary<KeyValuePair<int, ChangeAmount>, int, Vector3>((Func<KeyValuePair<int, ChangeAmount>, int>) (v => v.Key), (Func<KeyValuePair<int, ChangeAmount>, Vector3>) (v => v.Value.rot));
    }

    private void OnEndDragRot()
    {
      Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.RotationEqualsCommand(Singleton<GuideObjectManager>.Instance.selectObjectDictionary.Select<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>((Func<KeyValuePair<int, ChangeAmount>, GuideCommand.EqualsInfo>) (v => new GuideCommand.EqualsInfo()
      {
        dicKey = v.Key,
        oldValue = this.dicOld[v.Key],
        newValue = v.Value.rot
      })).ToArray<GuideCommand.EqualsInfo>()));
    }

    private void OnDragRotX()
    {
      float axis = Input.GetAxis("Mouse Y");
      Vector3 right = this.transformBase.get_right();
      foreach (GuideObject guideObject in ((IEnumerable<GuideObject>) Singleton<GuideObjectManager>.Instance.selectObjects).Where<GuideObject>((Func<GuideObject, bool>) (v => v.enableRot)))
        guideObject.Rotation(right, axis);
    }

    private void OnDragRotY()
    {
      float _angle = Input.GetAxis("Mouse X") * -1f;
      Vector3 up = this.transformBase.get_up();
      foreach (GuideObject guideObject in ((IEnumerable<GuideObject>) Singleton<GuideObjectManager>.Instance.selectObjects).Where<GuideObject>((Func<GuideObject, bool>) (v => v.enableRot)))
        guideObject.Rotation(up, _angle);
    }

    private void OnDragRotZ()
    {
      float _angle = Input.GetAxis("Mouse X") * -1f;
      Vector3 forward = this.transformBase.get_forward();
      foreach (GuideObject guideObject in ((IEnumerable<GuideObject>) Singleton<GuideObjectManager>.Instance.selectObjects).Where<GuideObject>((Func<GuideObject, bool>) (v => v.enableRot)))
        guideObject.Rotation(forward, _angle);
    }

    protected override void Awake()
    {
      if (!this.CheckInstance())
        return;
      this.mapDragButton[0].onBeginDragFunc += new Action(this.OnBeginDragTrans);
      this.mapDragButton[0].onDragFunc += new Action(this.OnDragTransXZ);
      this.mapDragButton[0].onEndDragFunc += new Action(this.OnEndDragTrans);
      this.mapDragButton[1].onBeginDragFunc += new Action(this.OnBeginDragTrans);
      this.mapDragButton[1].onDragFunc += new Action(this.OnDragTransY);
      this.mapDragButton[1].onEndDragFunc += new Action(this.OnEndDragTrans);
      for (int index = 0; index < 3; ++index)
      {
        this.mapDragButton[2 + index].onBeginDragFunc += new Action(this.OnBeginDragRot);
        this.mapDragButton[2 + index].onEndDragFunc += new Action(this.OnEndDragRot);
      }
      this.mapDragButton[2].onDragFunc += new Action(this.OnDragRotX);
      this.mapDragButton[3].onDragFunc += new Action(this.OnDragRotY);
      this.mapDragButton[4].onDragFunc += new Action(this.OnDragRotZ);
    }
  }
}
