// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectRoute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UnityEngine;

namespace Studio
{
  public static class AddObjectRoute
  {
    public static OCIRoute Add()
    {
      return AddObjectRoute.Load(new OIRouteInfo(Studio.Studio.GetNewIndex()), (ObjectCtrlInfo) null, (TreeNodeObject) null, true, Studio.Studio.optionSystem.initialPosition);
    }

    public static OCIRoute Load(
      OIRouteInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      ChangeAmount _source = _info.changeAmount.Clone();
      OCIRoute ociRoute = AddObjectRoute.Load(_info, _parent, _parentNode, false, -1);
      _info.changeAmount.Copy(_source, true, true, true);
      AddObjectAssist.LoadChild(_info.child, (ObjectCtrlInfo) ociRoute, (TreeNodeObject) null);
      return ociRoute;
    }

    public static OCIRoute Load(
      OIRouteInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCIRoute _ocir = new OCIRoute();
      _ocir.objectInfo = (ObjectInfo) _info;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>("studio/base/00.unity3d", "p_Route", true, string.Empty);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Studio.Studio.DeleteIndex(_info.dicKey);
        return (OCIRoute) null;
      }
      gameObject.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      _ocir.objectItem = gameObject;
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _info.dicKey);
      guideObject.isActive = false;
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.enableScale = false;
      guideObject.SetVisibleCenter(true);
      _ocir.guideObject = guideObject;
      _ocir.childRoot = gameObject.get_transform().GetChild(0);
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) _ocir);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) _ocir);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject _parent2 = Studio.Studio.AddNode(_info.name, _parent1);
      _parent2.treeState = _info.treeState;
      _parent2.enableVisible = true;
      _parent2.enableChangeParent = false;
      _parent2.visible = _info.visible;
      _parent2.colorSelect = _parent2.baseColor;
      _parent2.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) _ocir).OnVisible);
      _parent2.onDelete += new Action(_ocir.OnDeleteNode);
      _parent2.checkChild += new TreeNodeObject.CheckFunc(_ocir.CheckParentLoop);
      _parent2.checkParent += new TreeNodeObject.CheckFunc(_ocir.CheckParentLoop);
      guideObject.guideSelect.treeNodeObject = _parent2;
      _ocir.treeNodeObject = _parent2;
      _ocir.routeComponent = (RouteComponent) gameObject.GetComponent<RouteComponent>();
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode("子接続先", _parent2);
      treeNodeObject.enableChangeParent = false;
      treeNodeObject.enableDelete = false;
      treeNodeObject.enableCopy = false;
      treeNodeObject.baseColor = Utility.ConvertColor(204, 128, 164);
      treeNodeObject.colorSelect = treeNodeObject.baseColor;
      _parent2.childRoot = treeNodeObject;
      _ocir.childNodeRoot = treeNodeObject;
      if (_info.route.IsNullOrEmpty<OIRoutePointInfo>())
        _ocir.routeInfo.route.Add(new OIRoutePointInfo(Studio.Studio.GetNewIndex()));
      foreach (OIRoutePointInfo _rpInfo in _info.route)
        AddObjectRoute.LoadPoint(_ocir, _rpInfo, -1);
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.RefreshHierachy();
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) _ocir);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) _ocir);
      _info.changeAmount.onChangePos += new Action(_ocir.UpdateLine);
      _info.changeAmount.onChangeRot += new Action(_ocir.UpdateLine);
      _ocir.ForceUpdateLine();
      _ocir.visibleLine = _ocir.visibleLine;
      if (_ocir.isPlay)
        _ocir.Play();
      else
        _ocir.Stop(true);
      return _ocir;
    }

    public static OCIRoutePoint AddPoint(OCIRoute _ocir)
    {
      OIRoutePointInfo _rpInfo = new OIRoutePointInfo(Studio.Studio.GetNewIndex());
      _ocir.routeInfo.route.Add(_rpInfo);
      OCIRoutePoint ociRoutePoint = AddObjectRoute.LoadPoint(_ocir, _rpInfo, 1);
      _ocir.visibleLine = _ocir.visibleLine;
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.RefreshHierachy();
      return ociRoutePoint;
    }

    public static OCIRoutePoint LoadPoint(
      OCIRoute _ocir,
      OIRoutePointInfo _rpInfo,
      int _initialPosition)
    {
      int index = !_ocir.listPoint.IsNullOrEmpty<OCIRoutePoint>() ? _ocir.listPoint.Count - 1 : -1;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>("studio/base/00.unity3d", "p_RoutePoint", true, string.Empty);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Studio.Studio.DeleteIndex(_rpInfo.dicKey);
        return (OCIRoutePoint) null;
      }
      gameObject.get_transform().SetParent(_ocir.objectItem.get_transform());
      GuideObject _guide = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _rpInfo.dicKey);
      _guide.isActive = false;
      _guide.scaleSelect = 0.1f;
      _guide.scaleRot = 0.05f;
      _guide.enablePos = index != -1;
      _guide.enableRot = index == -1;
      _guide.enableScale = false;
      _guide.mode = GuideObject.Mode.World;
      _guide.moveCalc = GuideMove.MoveCalc.TYPE2;
      TreeNodeObject childRoot = _ocir.treeNodeObject.childRoot;
      _ocir.treeNodeObject.childRoot = (TreeNodeObject) null;
      TreeNodeObject _treeNode = Studio.Studio.AddNode(_rpInfo.name, _ocir.treeNodeObject);
      _treeNode.treeState = _rpInfo.treeState;
      _treeNode.enableChangeParent = false;
      _treeNode.enableDelete = index != -1;
      _treeNode.enableAddChild = false;
      _treeNode.enableCopy = false;
      _treeNode.enableVisible = false;
      _guide.guideSelect.treeNodeObject = _treeNode;
      _ocir.treeNodeObject.childRoot = childRoot;
      OCIRoutePoint _ocirp = new OCIRoutePoint(_ocir, _rpInfo, gameObject, _guide, _treeNode);
      _ocir.listPoint.Add(_ocirp);
      _ocir.UpdateNumber();
      _treeNode.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) _ocirp).OnVisible);
      _guide.isActiveFunc += new GuideObject.IsActiveFunc(((ObjectCtrlInfo) _ocirp).OnSelect);
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) _ocirp);
      AddObjectRoute.InitAid(_ocirp);
      if (_initialPosition == 1)
      {
        if (index == -1)
        {
          _rpInfo.changeAmount.pos = _ocir.objectInfo.changeAmount.pos;
        }
        else
        {
          OCIRoutePoint ociRoutePoint = _ocir.listPoint[index];
          _rpInfo.changeAmount.pos = _ocir.objectItem.get_transform().InverseTransformPoint(ociRoutePoint.position);
        }
      }
      _rpInfo.changeAmount.OnChange();
      _rpInfo.changeAmount.onChangePosAfter += new Action(_ocir.UpdateLine);
      _rpInfo.changeAmount.onChangeRot += new Action(_ocir.UpdateLine);
      _ocirp.connection = _ocirp.connection;
      return _ocirp;
    }

    public static void InitAid(OCIRoutePoint _ocirp)
    {
      bool flag = _ocirp.routePointInfo.aidInfo == null;
      if (flag)
        _ocirp.routePointInfo.aidInfo = new OIRoutePointAidInfo(Studio.Studio.GetNewIndex());
      Transform transform = _ocirp.routePoint.objAid.get_transform();
      if (flag)
        _ocirp.routePointInfo.aidInfo.changeAmount.pos = transform.get_localPosition();
      GuideObject _guideObject = Singleton<GuideObjectManager>.Instance.Add(transform, _ocirp.routePointInfo.aidInfo.dicKey);
      _guideObject.enableRot = false;
      _guideObject.enableScale = false;
      _guideObject.enableMaluti = false;
      _guideObject.scaleSelect = 0.1f;
      _guideObject.scaleRot = 0.05f;
      _guideObject.parentGuide = _ocirp.guideObject;
      _guideObject.changeAmount.OnChange();
      _guideObject.mode = GuideObject.Mode.World;
      _guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _ocirp.pointAidInfo = new OCIRoutePoint.PointAidInfo(_guideObject, _ocirp.routePointInfo.aidInfo);
      _ocirp.pointAidInfo.active = false;
      _ocirp.routePointInfo.aidInfo.changeAmount.onChangePosAfter += new Action(_ocirp.route.UpdateLine);
    }
  }
}
