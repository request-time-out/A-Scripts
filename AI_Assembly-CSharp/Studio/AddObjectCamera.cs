// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Studio
{
  public static class AddObjectCamera
  {
    public static Color baseColor = Utility.ConvertColor(0, 104, 183);
    public static Color activeColor = Utility.ConvertColor(200, 0, 0);

    public static OCICamera Add()
    {
      return AddObjectCamera.Load(new OICameraInfo(Studio.Studio.GetNewIndex()), (ObjectCtrlInfo) null, (TreeNodeObject) null, true, Studio.Studio.optionSystem.initialPosition);
    }

    public static OCICamera Load(
      OICameraInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      ChangeAmount _source = _info.changeAmount.Clone();
      OCICamera ociCamera = AddObjectCamera.Load(_info, _parent, _parentNode, false, -1);
      _info.changeAmount.Copy(_source, true, true, true);
      return ociCamera;
    }

    public static OCICamera Load(
      OICameraInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCICamera ocic = new OCICamera();
      ocic.objectInfo = (ObjectInfo) _info;
      GameObject gameObject = CommonLib.LoadAsset<GameObject>("studio/base/00.unity3d", "p_koi_stu_cameraicon00_00", true, string.Empty);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Studio.Studio.DeleteIndex(_info.dicKey);
        return (OCICamera) null;
      }
      gameObject.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      ocic.objectItem = gameObject;
      ocic.meshRenderer = (MeshRenderer) gameObject.GetComponent<MeshRenderer>();
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _info.dicKey);
      guideObject.isActive = false;
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.enableScale = false;
      ocic.guideObject = guideObject;
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) ocic);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) ocic);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(_info.name, _parent1);
      treeNodeObject.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) ocic).OnVisible);
      treeNodeObject.treeState = _info.treeState;
      treeNodeObject.enableVisible = true;
      treeNodeObject.enableAddChild = false;
      treeNodeObject.visible = _info.visible;
      treeNodeObject.baseColor = !_info.active ? AddObjectCamera.baseColor : AddObjectCamera.activeColor;
      treeNodeObject.colorSelect = treeNodeObject.baseColor;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      ocic.treeNodeObject = treeNodeObject;
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnPointerClickAsObservable((UIBehaviour) treeNodeObject.buttonSelect), (Action<M0>) (_ped =>
      {
        if (_ped.get_button() != 1)
          return;
        Singleton<Studio.Studio>.Instance.ChangeCamera(ocic);
        Singleton<Studio.Studio>.Instance.manipulatePanelCtrl.UpdateInfo(5);
      }));
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) ocic);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) ocic);
      Singleton<Studio.Studio>.Instance.ChangeCamera(ocic, _info.active, false);
      return ocic;
    }
  }
}
