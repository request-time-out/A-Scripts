// Decompiled with JetBrains decompiler
// Type: Housing.GuideManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Housing
{
  public class GuideManager : Singleton<GuideManager>
  {
    [SerializeField]
    [Header("基本設計")]
    private MeshCollider meshCollider;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    [Header("操作軸")]
    private GameObject objManipulator;
    private GuideObject m_guideObject;

    public Transform TransformRoot { get; set; }

    public Vector3 GridArea { get; private set; } = new Vector3(200f, 100f, 200f);

    public GuideObject GuideObject
    {
      get
      {
        return this.m_guideObject ?? (this.m_guideObject = (GuideObject) this.objManipulator.GetComponent<GuideObject>());
      }
    }

    public bool IsGuide { get; set; }

    public bool VisibleGrid
    {
      get
      {
        return ((Renderer) this.meshRenderer).get_enabled();
      }
      set
      {
        ((Renderer) this.meshRenderer).set_enabled(value);
      }
    }

    public void Init(Vector3 _gridArea)
    {
      this.GridArea = _gridArea;
      ((Component) this.meshRenderer).get_transform().set_localScale(new Vector3((float) (_gridArea.x * 0.100000001490116), 1f, (float) (_gridArea.z * 0.100000001490116)));
      Material material = ((Renderer) this.meshRenderer).get_material();
      material.set_mainTextureScale(new Vector2((float) _gridArea.x, (float) _gridArea.z));
      ((Renderer) this.meshRenderer).set_material(material);
    }

    public bool CorrectPos(ObjectCtrl _objectCtrl, ref Vector3 _pos)
    {
      Vector3 zero1 = Vector3.get_zero();
      Vector3 zero2 = Vector3.get_zero();
      _objectCtrl.GetLocalMinMax(_pos, _objectCtrl.Rotation, this.TransformRoot, ref zero1, ref zero2);
      Vector3 vector3 = Vector3.op_Multiply(this.GridArea, 0.5f);
      Vector3 zero3 = Vector3.get_zero();
      bool flag = false;
      if (-vector3.x > zero1.x)
      {
        zero3.x = vector3.x + zero1.x;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      else if (vector3.x < zero2.x)
      {
        zero3.x = zero2.x - vector3.x;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      if (0.0 > zero1.y)
      {
        zero3.y = zero1.y;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      else if (this.GridArea.y < zero2.y)
      {
        zero3.y = zero2.y - this.GridArea.y;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      if (-vector3.z > zero1.z)
      {
        zero3.z = vector3.z + zero1.z;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      else if (vector3.z < zero2.z)
      {
        zero3.z = zero2.z - vector3.z;
        flag = ((flag ? 1 : 0) | 1) != 0;
      }
      _pos = Vector3.op_Subtraction(_pos, this.TransformRoot.TransformVector(zero3));
      return flag;
    }

    public bool CheckRot(ObjectCtrl _objectCtrl)
    {
      Vector3 zero1 = Vector3.get_zero();
      Vector3 zero2 = Vector3.get_zero();
      _objectCtrl.GetLocalMinMax(_objectCtrl.Position, _objectCtrl.Rotation, this.TransformRoot, ref zero1, ref zero2);
      Vector3Int vector3Int1;
      ((Vector3Int) ref vector3Int1).\u002Ector(this.FloorToInt((float) zero1.x), this.FloorToInt((float) zero1.y), this.FloorToInt((float) zero1.z));
      Vector3Int vector3Int2;
      ((Vector3Int) ref vector3Int2).\u002Ector(this.FloorToInt((float) zero2.x), this.FloorToInt((float) zero2.y), this.FloorToInt((float) zero2.z));
      Vector3 vector3 = Vector3.op_Multiply(this.GridArea, 0.5f);
      return -vector3.x <= (double) ((Vector3Int) ref vector3Int1).get_x() && vector3.x >= (double) ((Vector3Int) ref vector3Int2).get_x() && 0 <= ((Vector3Int) ref vector3Int1).get_y() && (this.GridArea.y >= (double) ((Vector3Int) ref vector3Int2).get_y() && -vector3.z <= (double) ((Vector3Int) ref vector3Int1).get_z() && vector3.z >= (double) ((Vector3Int) ref vector3Int2).get_z());
    }

    private int FloorToInt(float _value)
    {
      bool flag = (double) _value < 0.0;
      return Mathf.FloorToInt(Mathf.Abs(_value)) * (!flag ? 1 : -1);
    }

    public bool NoCameraCtrl()
    {
      return this.IsGuide;
    }

    private void OnSelect(ObjectCtrl[] _objectCtrls)
    {
      ObjectCtrl _objectCtrl = (ObjectCtrl) null;
      if (!((IList<ObjectCtrl>) _objectCtrls).IsNullOrEmpty<ObjectCtrl>())
        _objectCtrl = ((IEnumerable<ObjectCtrl>) _objectCtrls).FirstOrDefault<ObjectCtrl>((Func<ObjectCtrl, bool>) (_oc => _oc is OCItem));
      this.GuideObject.SetTarget(_objectCtrl);
    }

    private void Start()
    {
      Singleton<Selection>.Instance.onSelectFunc += new Action<ObjectCtrl[]>(this.OnSelect);
      this.GuideObject.visible = false;
    }
  }
}
