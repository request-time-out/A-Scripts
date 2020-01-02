// Decompiled with JetBrains decompiler
// Type: Studio.OCIRoute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Vectrosity;

namespace Studio
{
  public class OCIRoute : ObjectCtrlInfo
  {
    private int segments = 160;
    public const int limitNum = 10;
    public GameObject objectItem;
    public Transform childRoot;
    public TreeNodeObject childNodeRoot;
    public RouteComponent routeComponent;
    private int nowIndex;
    private SingleAssignmentDisposable disposable;
    private VectorLine line;
    private GameObject objLine;

    public OCIRoute()
    {
      this.listPoint = new List<OCIRoutePoint>();
    }

    public OIRouteInfo routeInfo
    {
      get
      {
        return this.objectInfo as OIRouteInfo;
      }
    }

    public string name
    {
      get
      {
        return this.routeInfo.name;
      }
      set
      {
        this.routeInfo.name = value;
        this.treeNodeObject.textName = value;
      }
    }

    public List<OCIRoutePoint> listPoint { get; private set; }

    public bool isPlay
    {
      get
      {
        return this.routeInfo.active;
      }
    }

    public bool isEnd
    {
      get
      {
        return this.routeInfo.route.Count > 1 && this.nowIndex >= this.listPoint.Count - 1 & !this.routeInfo.active;
      }
    }

    public bool visibleLine
    {
      get
      {
        return this.routeInfo.visibleLine;
      }
      set
      {
        this.routeInfo.visibleLine = value;
        this.SetVisible(value);
      }
    }

    public override void OnDelete()
    {
      if (this.line != null)
        VectorLine.Destroy(ref this.line);
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      Object.Destroy((Object) this.objectItem);
      if (this.parentInfo != null)
        this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      if (_child.parentInfo == null)
        Studio.Studio.DeleteInfo(_child.objectInfo, false);
      else
        _child.parentInfo.OnDetachChild(_child);
      if (!this.routeInfo.child.Contains(_child.objectInfo))
        this.routeInfo.child.Add(_child.objectInfo);
      _child.guideObject.transformTarget.SetParent(this.childRoot);
      _child.guideObject.parent = this.childRoot;
      _child.guideObject.mode = GuideObject.Mode.World;
      _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _child.objectInfo.changeAmount.pos = _child.guideObject.transformTarget.get_localPosition();
      _child.objectInfo.changeAmount.rot = _child.guideObject.transformTarget.get_localEulerAngles();
      _child.parentInfo = (ObjectCtrlInfo) this;
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      if (_child.parentInfo == null)
        Studio.Studio.DeleteInfo(_child.objectInfo, false);
      else
        _child.parentInfo.OnDetachChild(_child);
      if (!this.routeInfo.child.Contains(_child.objectInfo))
        this.routeInfo.child.Add(_child.objectInfo);
      _child.guideObject.transformTarget.SetParent(this.childRoot, false);
      _child.guideObject.parent = this.childRoot;
      _child.guideObject.mode = GuideObject.Mode.World;
      _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _child.objectInfo.changeAmount.OnChange();
      _child.parentInfo = (ObjectCtrlInfo) this;
    }

    public override void OnDetach()
    {
      this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      this.guideObject.parent = (Transform) null;
      Studio.Studio.AddInfo(this.objectInfo, (ObjectCtrlInfo) this);
      this.objectItem.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      this.objectInfo.changeAmount.pos = this.objectItem.get_transform().get_localPosition();
      this.objectInfo.changeAmount.rot = this.objectItem.get_transform().get_localEulerAngles();
      this.guideObject.mode = GuideObject.Mode.Local;
      this.guideObject.moveCalc = GuideMove.MoveCalc.TYPE1;
      this.treeNodeObject.ResetVisible();
    }

    public override void OnSelect(bool _select)
    {
    }

    public override void OnDetachChild(ObjectCtrlInfo _child)
    {
      if (!this.routeInfo.child.Remove(_child.objectInfo))
        Debug.LogError((object) "情報の消去に失敗");
      _child.parentInfo = (ObjectCtrlInfo) null;
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
    }

    public override void OnVisible(bool _visible)
    {
      if (this.line == null)
        return;
      this.line.set_active(_visible && this.visibleLine);
    }

    public void OnDeleteNode()
    {
      foreach (OCIRoutePoint ociRoutePoint in this.listPoint)
        ociRoutePoint.isParentDelete = true;
    }

    private void SetVisible(bool _flag)
    {
      bool flag = _flag & this.treeNodeObject.visible;
      if (this.line != null)
        this.line.set_active(flag);
      foreach (OCIRoutePoint ociRoutePoint in this.listPoint)
        ociRoutePoint.visible = flag;
    }

    public bool CheckParentLoop(TreeNodeObject _parent)
    {
      if (Object.op_Equality((Object) _parent, (Object) null))
        return true;
      ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(_parent);
      if (ctrlInfo != null)
      {
        switch (ctrlInfo.kind)
        {
          case 1:
            OCIItem ociItem = ctrlInfo as OCIItem;
            if (ociItem.itemInfo.group == 10 || ociItem.itemInfo.group == 15)
              return false;
            break;
          case 4:
            return false;
        }
      }
      return this.CheckParentLoop(_parent.parent);
    }

    public OCIRoutePoint AddPoint()
    {
      if (Studio.Studio.optionSystem.routePointLimit && this.routeInfo.route.Count > 10)
        return (OCIRoutePoint) null;
      OCIRoutePoint ociRoutePoint = AddObjectRoute.AddPoint(this);
      this.UpdateLine();
      return ociRoutePoint;
    }

    public void DeletePoint(OCIRoutePoint _routePoint)
    {
      this.Stop(true);
      this.treeNodeObject.RemoveChild(_routePoint.treeNodeObject, true);
      this.listPoint.Remove(_routePoint);
      this.routeInfo.route.Remove(_routePoint.routePointInfo);
      this.UpdateNumber();
      this.UpdateLine();
    }

    public bool Play()
    {
      if (this.routeInfo.route.Count <= 1)
        return false;
      this.Stop(false);
      Transform transform = this.listPoint[0].objectItem.get_transform();
      this.childRoot.SetPositionAndRotation(transform.get_position(), transform.get_rotation());
      int _index = 0;
      StudioTween _tween = this.SetPath((StudioTween) null, ref _index);
      while (_index < this.listPoint.Count)
      {
        this.SetPath(_tween, ref _index);
        if (!this.routeInfo.loop && _index == this.listPoint.Count - 1)
          break;
      }
      _tween.loopType = !this.routeInfo.loop ? StudioTween.LoopType.none : StudioTween.LoopType.loop;
      if (!this.routeInfo.loop)
        _tween.onComplete += (StudioTween.CompleteFunction) (() =>
        {
          this.routeInfo.active = false;
          Singleton<Studio.Studio>.Instance.routeControl.SetState(this.objectInfo, RouteNode.State.End);
          return true;
        });
      this.routeInfo.active = true;
      return true;
    }

    private bool Move()
    {
      StudioTween.Stop(((Component) this.childRoot).get_gameObject());
      if (this.routeInfo.loop)
      {
        if (this.nowIndex >= this.listPoint.Count)
          this.nowIndex = 0;
      }
      else if (this.nowIndex >= this.listPoint.Count - 1)
      {
        this.routeInfo.active = false;
        Singleton<Studio.Studio>.Instance.routeControl.SetState(this.objectInfo, RouteNode.State.End);
        return false;
      }
      Hashtable args = new Hashtable();
      switch (this.listPoint[this.nowIndex].connection)
      {
        case OIRoutePointInfo.Connection.Line:
          Transform[] transformArray;
          if (this.nowIndex == this.listPoint.Count - 1)
            transformArray = new Transform[2]
            {
              this.listPoint[this.listPoint.Count - 1].objectItem.get_transform(),
              this.listPoint[0].objectItem.get_transform()
            };
          else
            transformArray = this.listPoint.Skip<OCIRoutePoint>(this.nowIndex).Take<OCIRoutePoint>(2).Select<OCIRoutePoint, Transform>((Func<OCIRoutePoint, Transform>) (v => v.objectItem.get_transform())).ToArray<Transform>();
          args.Add((object) "path", (object) transformArray);
          break;
        case OIRoutePointInfo.Connection.Curve:
          List<Transform> list = ((IEnumerable<Transform>) this.listPoint[this.nowIndex].transform).ToList<Transform>();
          if (this.nowIndex + 1 >= this.listPoint.Count)
            list.Add(this.listPoint[0].objectItem.get_transform());
          else
            list.Add(this.listPoint[this.nowIndex + 1].objectItem.get_transform());
          args.Add((object) "path", (object) list.ToArray());
          break;
      }
      args.Add((object) "speed", (object) (float) ((double) this.listPoint[this.nowIndex].routePointInfo.speed * 10.0));
      args.Add((object) "easetype", (object) this.listPoint[this.nowIndex].routePointInfo.easeType);
      args.Add((object) "looptype", (object) StudioTween.LoopType.none);
      switch (this.routeInfo.orient)
      {
        case OIRouteInfo.Orient.XY:
          args.Add((object) "orienttopath", (object) true);
          break;
        case OIRouteInfo.Orient.Y:
          args.Add((object) "orienttopath", (object) true);
          args.Add((object) "axis", (object) "y");
          break;
      }
      StudioTween.MoveTo(((Component) this.childRoot).get_gameObject(), args).onComplete += new StudioTween.CompleteFunction(this.Move);
      ++this.nowIndex;
      return true;
    }

    private StudioTween SetPath(StudioTween _tween, ref int _index)
    {
      if (!this.routeInfo.loop && _index == this.listPoint.Count - 1)
        return _tween;
      int count = _index++;
      Hashtable args = new Hashtable();
      switch (this.listPoint[count].connection)
      {
        case OIRoutePointInfo.Connection.Line:
          Transform[] transformArray;
          if (count == this.listPoint.Count - 1)
            transformArray = new Transform[2]
            {
              this.listPoint[this.listPoint.Count - 1].objectItem.get_transform(),
              this.listPoint[0].objectItem.get_transform()
            };
          else
            transformArray = this.listPoint.Skip<OCIRoutePoint>(count).Take<OCIRoutePoint>(2).Select<OCIRoutePoint, Transform>((Func<OCIRoutePoint, Transform>) (v => v.objectItem.get_transform())).ToArray<Transform>();
          args.Add((object) "path", (object) transformArray);
          break;
        case OIRoutePointInfo.Connection.Curve:
          List<Transform> list = ((IEnumerable<Transform>) this.listPoint[count].transform).ToList<Transform>();
          while (_index < this.listPoint.Count && (this.routeInfo.loop || _index != this.listPoint.Count - 1) && this.listPoint[_index].isLink)
          {
            list.AddRange((IEnumerable<Transform>) this.listPoint[_index].transform);
            ++_index;
          }
          if (_index >= this.listPoint.Count)
            list.Add(this.listPoint[0].objectItem.get_transform());
          else
            list.Add(this.listPoint[_index].objectItem.get_transform());
          args.Add((object) "path", (object) list.ToArray());
          break;
      }
      args.Add((object) "speed", (object) (float) ((double) this.listPoint[count].routePointInfo.speed * 10.0));
      args.Add((object) "easetype", (object) this.listPoint[count].routePointInfo.easeType);
      switch (this.routeInfo.orient)
      {
        case OIRouteInfo.Orient.XY:
          args.Add((object) "orienttopath", (object) true);
          break;
        case OIRouteInfo.Orient.Y:
          args.Add((object) "orienttopath", (object) true);
          args.Add((object) "axis", (object) "y");
          break;
      }
      if (!Object.op_Inequality((Object) _tween, (Object) null))
        return StudioTween.MoveTo(((Component) this.childRoot).get_gameObject(), args);
      _tween.MoveTo(args);
      return _tween;
    }

    public void Stop(bool _copy = true)
    {
      StudioTween.Stop(((Component) this.childRoot).get_gameObject());
      if (this.disposable != null)
      {
        this.disposable.Dispose();
        this.disposable = (SingleAssignmentDisposable) null;
      }
      if (!this.listPoint.IsNullOrEmpty<OCIRoutePoint>() && _copy)
      {
        this.disposable = new SingleAssignmentDisposable();
        this.disposable.set_Disposable((IDisposable) DisposableExtensions.AddTo<IDisposable>((M0) ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ =>
        {
          Transform transform = this.listPoint[0].objectItem.get_transform();
          this.childRoot.SetPositionAndRotation(transform.get_position(), transform.get_rotation());
        })), (Component) this.childRoot));
      }
      this.nowIndex = 0;
      this.routeInfo.active = false;
    }

    public void UpdateLine()
    {
      if (this.routeInfo.route.Count <= 1)
      {
        this.DeleteLine();
      }
      else
      {
        bool flag1 = this.line == null;
        int index1 = 0;
        int capacity = 0;
        while (index1 < this.listPoint.Count)
        {
          switch (this.listPoint[index1].connection)
          {
            case OIRoutePointInfo.Connection.Line:
              ++capacity;
              ++index1;
              if (index1 >= this.listPoint.Count && this.routeInfo.loop)
              {
                ++capacity;
                continue;
              }
              continue;
            case OIRoutePointInfo.Connection.Curve:
              if (!this.routeInfo.loop && index1 == this.listPoint.Count - 1)
              {
                capacity += (this.routeInfo.loop || index1 != this.listPoint.Count - 1 ? this.segments : 0) + 1;
                ++index1;
                continue;
              }
              int num1 = 1;
              for (++index1; index1 < this.listPoint.Count && (this.routeInfo.loop || index1 != this.listPoint.Count - 1) && this.listPoint[index1].isLink; ++index1)
                ++num1;
              capacity += this.segments * num1 + 1;
              continue;
            default:
              continue;
          }
        }
        if (!flag1 && this.line.get_points3().Count != capacity)
        {
          VectorLine.Destroy(ref this.line);
          flag1 = true;
        }
        if (flag1)
        {
          this.line = new VectorLine("Spline", new List<Vector3>(capacity), Studio.Studio.optionSystem.routeLineWidth, (LineType) 0);
          this.objLine = GameObject.Find("Spline");
          if (Object.op_Implicit((Object) this.objLine))
          {
            ((Object) this.objLine).set_name("Spline " + this.routeInfo.name);
            this.objLine.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
          }
        }
        int index2 = 0;
        int index3 = 0;
        while (index2 < this.listPoint.Count)
        {
          switch (this.listPoint[index2].connection)
          {
            case OIRoutePointInfo.Connection.Line:
              List<Vector3> points3_1 = this.line.get_points3();
              points3_1[index3] = this.listPoint[index2].position;
              ++index2;
              ++index3;
              if (index2 >= this.listPoint.Count && this.routeInfo.loop)
                points3_1[index3] = this.listPoint[0].position;
              this.line.set_points3(points3_1);
              continue;
            case OIRoutePointInfo.Connection.Curve:
              if (!this.routeInfo.loop && index2 == this.listPoint.Count - 1)
              {
                List<Vector3> points3_2 = this.line.get_points3();
                points3_2[index3] = this.listPoint[index2].position;
                ++index2;
                ++index3;
                this.line.set_points3(points3_2);
                continue;
              }
              List<Transform> list = ((IEnumerable<Transform>) this.listPoint[index2].transform).ToList<Transform>();
              int index4 = index2 + 1;
              int num2 = 1;
              for (; index4 < this.listPoint.Count && (this.routeInfo.loop || index4 != this.listPoint.Count - 1) && this.listPoint[index4].isLink; ++index4)
              {
                list.AddRange((IEnumerable<Transform>) this.listPoint[index4].transform);
                ++num2;
              }
              bool flag2 = index2 == 0 && index4 >= this.listPoint.Count && this.routeInfo.loop;
              if (index4 >= this.listPoint.Count)
              {
                if (!flag2)
                  list.Add(this.listPoint[0].objectItem.get_transform());
              }
              else
                list.Add(this.listPoint[index4].objectItem.get_transform());
              this.line.MakeSpline(((IEnumerable<Transform>) list).Select<Transform, Vector3>((Func<Transform, Vector3>) (v => v.get_position())).ToArray<Vector3>(), this.segments * num2, index3, flag2);
              index2 = index4;
              index3 += this.segments * num2 + 1;
              continue;
            default:
              continue;
          }
        }
        this.line.set_joins((Joins) 1);
        this.line.set_color(Color32.op_Implicit(this.routeInfo.color));
        this.line.set_continuousTexture(false);
        this.line.set_lineWidth(Studio.Studio.optionSystem.routeLineWidth);
        this.line.Draw3DAuto();
        this.line.set_layer(LayerMask.NameToLayer("Studio/Route"));
        if (!flag1)
          return;
        this.line.set_active(this.routeInfo.visibleLine);
        Renderer component = (Renderer) this.objLine.GetComponent<Renderer>();
        if (!Object.op_Implicit((Object) component))
          return;
        component.get_material().set_renderQueue(2900);
      }
    }

    public void ForceUpdateLine()
    {
      this.DeleteLine();
      this.UpdateLine();
    }

    public void DeleteLine()
    {
      if (this.line == null)
        return;
      VectorLine.Destroy(ref this.line);
    }

    public void SetLineColor(Color _color)
    {
      if (this.line == null)
        return;
      this.line.SetColor(Color32.op_Implicit(_color));
    }

    public void UpdateNumber()
    {
      for (int index = 0; index < this.listPoint.Count; ++index)
        this.listPoint[index].number = index;
    }
  }
}
