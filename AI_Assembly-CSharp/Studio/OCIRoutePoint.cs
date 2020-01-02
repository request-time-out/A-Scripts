// Decompiled with JetBrains decompiler
// Type: Studio.OCIRoutePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace Studio
{
  public class OCIRoutePoint : ObjectCtrlInfo
  {
    private int segments = 160;
    public GameObject objectItem;
    public OCIRoutePoint.PointAidInfo pointAidInfo;
    private VectorLine _line;

    public OCIRoutePoint(
      OCIRoute _route,
      OIRoutePointInfo _info,
      GameObject _obj,
      GuideObject _guide,
      TreeNodeObject _treeNode)
    {
      this.route = _route;
      this.objectInfo = (ObjectInfo) _info;
      this.objectItem = _obj;
      this.guideObject = _guide;
      this.treeNodeObject = _treeNode;
      this.routePoint = (RoutePointComponent) _obj.GetComponent<RoutePointComponent>();
      this.isParentDelete = false;
      this._line = (VectorLine) null;
    }

    public OIRoutePointInfo routePointInfo
    {
      get
      {
        return this.objectInfo as OIRoutePointInfo;
      }
    }

    public OCIRoute route { get; private set; }

    public RoutePointComponent routePoint { get; private set; }

    public string name
    {
      get
      {
        return this.routePointInfo.name;
      }
    }

    public int number
    {
      get
      {
        int result = -1;
        return int.TryParse(this.routePointInfo.name.Replace("ポイント", string.Empty), out result) ? result : 0;
      }
      set
      {
        this.routePointInfo.number = value;
        this.routePoint.textName = value != 0 ? value.ToString() : "S";
        this.treeNodeObject.textName = this.name;
      }
    }

    public Vector3 position
    {
      get
      {
        return this.objectItem.get_transform().get_position();
      }
    }

    public Transform[] transform
    {
      get
      {
        return new Transform[2]
        {
          this.objectItem.get_transform(),
          this.pointAidInfo.target
        };
      }
    }

    public List<Vector3> positions
    {
      get
      {
        List<Vector3> vector3List = new List<Vector3>();
        vector3List.Add(this.position);
        if (this.connection == OIRoutePointInfo.Connection.Curve)
          vector3List.Add(this.routePoint.objAid.get_transform().get_position());
        return vector3List;
      }
    }

    public float speed
    {
      get
      {
        return this.routePointInfo.speed;
      }
      set
      {
        this.routePointInfo.speed = value;
      }
    }

    public StudioTween.EaseType easeType
    {
      get
      {
        return this.routePointInfo.easeType;
      }
      set
      {
        this.routePointInfo.easeType = value;
      }
    }

    public OIRoutePointInfo.Connection connection
    {
      get
      {
        return this.routePointInfo.connection;
      }
      set
      {
        this.routePointInfo.connection = value;
        if (value != OIRoutePointInfo.Connection.Line)
        {
          if (value != OIRoutePointInfo.Connection.Curve)
            return;
          this.InitAidPos();
          this.pointAidInfo.active = true;
        }
        else
          this.pointAidInfo.active = false;
      }
    }

    public bool link
    {
      get
      {
        return this.routePointInfo.link;
      }
      set
      {
        this.routePointInfo.link = value;
      }
    }

    public bool isLink
    {
      get
      {
        return this.routePointInfo.link && this.routePointInfo.connection == OIRoutePointInfo.Connection.Curve;
      }
    }

    public bool isParentDelete { get; set; }

    public bool visible
    {
      set
      {
        this.routePoint.visible = value;
        this.lineActive = value;
      }
    }

    public VectorLine line
    {
      get
      {
        return this._line;
      }
    }

    public bool lineActive
    {
      set
      {
        if (this._line == null)
          return;
        this._line.set_active(value);
      }
    }

    public override void OnDelete()
    {
      if (!this.isParentDelete)
        this.route.DeletePoint(this);
      if (this._line != null)
        VectorLine.Destroy(ref this._line);
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      Singleton<GuideObjectManager>.Instance.Delete(this.pointAidInfo.guideObject, true);
      Object.Destroy((Object) this.objectItem);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
    }

    public override void OnDetach()
    {
    }

    public override void OnSelect(bool _select)
    {
      this.pointAidInfo.layer = LayerMask.NameToLayer(!_select ? "Studio/Select" : "Studio/Col");
    }

    public override void OnDetachChild(ObjectCtrlInfo _child)
    {
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
    }

    public override void OnVisible(bool _visible)
    {
      this.routePoint.visible = _visible && this.route.visibleLine;
    }

    public override ObjectCtrlInfo this[int _idx]
    {
      get
      {
        return _idx == 0 ? (ObjectCtrlInfo) this : (ObjectCtrlInfo) this.route;
      }
    }

    public void UpdateLine(Vector3 _pos)
    {
      List<Vector3> positions = this.positions;
      positions.Add(_pos);
      if (this._line == null)
      {
        switch (this.connection)
        {
          case OIRoutePointInfo.Connection.Line:
            this._line = new VectorLine("Line", positions, Studio.Studio.optionSystem.routeLineWidth, (LineType) 0);
            break;
          case OIRoutePointInfo.Connection.Curve:
            this._line = new VectorLine("Spline", new List<Vector3>(this.segments + 1), Studio.Studio.optionSystem.routeLineWidth, (LineType) 0);
            this._line.MakeSpline(positions.ToArray(), this.segments, false);
            break;
        }
        this._line.set_joins((Joins) 1);
        this._line.set_color(Color32.op_Implicit(this.route.routeInfo.color));
        this._line.set_continuousTexture(false);
        this._line.Draw3DAuto();
        this._line.set_layer(LayerMask.NameToLayer("Studio/Camera"));
        this._line.set_active(this.route.routeInfo.visibleLine);
      }
      else
      {
        switch (this.connection)
        {
          case OIRoutePointInfo.Connection.Line:
            List<Vector3> points3 = this._line.get_points3();
            for (int index = 0; index < positions.Count; ++index)
              points3[index] = positions[index];
            this._line.set_points3(points3);
            break;
          case OIRoutePointInfo.Connection.Curve:
            this._line.MakeSpline(positions.ToArray(), this.segments, false);
            break;
        }
        this._line.set_lineWidth(Studio.Studio.optionSystem.routeLineWidth);
        this._line.Draw3DAuto();
        this._line.set_active(this.route.routeInfo.visibleLine);
      }
    }

    public void DeleteLine()
    {
      if (this._line == null)
        return;
      VectorLine.Destroy(ref this._line);
    }

    private void InitAidPos()
    {
      if (this.pointAidInfo.aidInfo.isInit)
        return;
      this.DeleteLine();
      int num = this.route.listPoint.IndexOf(this);
      this.pointAidInfo.aidInfo.changeAmount.pos = this.objectItem.get_transform().InverseTransformPoint(Vector3.Lerp(this.position, num + 1 < this.route.listPoint.Count ? this.route.listPoint[num + 1].position : this.route.listPoint[0].position, 0.5f));
      this.pointAidInfo.aidInfo.isInit = true;
    }

    public void SetEnable(bool _value, bool _first = false)
    {
      this.guideObject.SetEnable(!_first ? (!_value ? 0 : 1) : -1, !_first ? -1 : (!_value ? 0 : 1), -1);
      this.pointAidInfo.guideObject.SetEnable(!_value ? 0 : 1, -1, -1);
    }

    public class PointAidInfo
    {
      private GameObject m_GameObject;

      public PointAidInfo(GuideObject _guideObject, OIRoutePointAidInfo _aidInfo)
      {
        this.guideObject = _guideObject;
        this.aidInfo = _aidInfo;
      }

      public GuideObject guideObject { get; private set; }

      public OIRoutePointAidInfo aidInfo { get; private set; }

      public GameObject gameObject
      {
        get
        {
          if (Object.op_Equality((Object) this.m_GameObject, (Object) null))
            this.m_GameObject = ((Component) this.guideObject).get_gameObject();
          return this.m_GameObject;
        }
      }

      public Transform target
      {
        get
        {
          return this.guideObject.transformTarget;
        }
      }

      public Vector3 position
      {
        get
        {
          return this.gameObject.get_transform().get_position();
        }
      }

      public Transform transform
      {
        get
        {
          return this.gameObject.get_transform();
        }
      }

      public bool active
      {
        get
        {
          return Object.op_Inequality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
        }
        set
        {
          if (!Object.op_Implicit((Object) this.gameObject))
            return;
          this.gameObject.SetActive(value);
        }
      }

      public int layer
      {
        set
        {
          this.guideObject.SetLayer(this.gameObject, value);
        }
      }
    }
  }
}
