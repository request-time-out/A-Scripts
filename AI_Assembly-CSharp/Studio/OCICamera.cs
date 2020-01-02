// Decompiled with JetBrains decompiler
// Type: Studio.OCICamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using UniRx;
using UnityEngine;

namespace Studio
{
  public class OCICamera : ObjectCtrlInfo
  {
    private bool visibleOutside = true;
    public GameObject objectItem;
    public MeshRenderer meshRenderer;
    private SingleAssignmentDisposable disposable;
    private CameraControl cameraControl;

    public OICameraInfo cameraInfo
    {
      get
      {
        return this.objectInfo as OICameraInfo;
      }
    }

    public string name
    {
      get
      {
        return this.cameraInfo.name;
      }
      set
      {
        this.cameraInfo.name = value;
        this.treeNodeObject.textName = value;
      }
    }

    public void SetActive(bool _active)
    {
      this.cameraInfo.active = _active;
      if (_active)
      {
        if (this.disposable != null)
          return;
        this.cameraControl = Singleton<Studio.Studio>.Instance.cameraCtrl;
        this.disposable = new SingleAssignmentDisposable();
        this.disposable.set_Disposable(ObservableExtensions.Subscribe<long>((IObservable<M0>) Observable.EveryLateUpdate(), (Action<M0>) (_ => this.cameraControl.SafeProc<CameraControl>((Action<CameraControl>) (_cc => _cc.SetPositionAndRotation(this.objectItem.get_transform().get_position(), this.objectItem.get_transform().get_rotation()))))));
        this.treeNodeObject.baseColor = AddObjectCamera.activeColor;
        if (!Singleton<Studio.Studio>.Instance.treeNodeCtrl.CheckSelect(this.treeNodeObject))
          this.treeNodeObject.colorSelect = AddObjectCamera.activeColor;
        this.guideObject.visible = false;
        ((Renderer) this.meshRenderer).set_enabled(false);
      }
      else
      {
        if (this.disposable != null)
        {
          this.disposable.Dispose();
          this.disposable = (SingleAssignmentDisposable) null;
        }
        this.treeNodeObject.baseColor = AddObjectCamera.baseColor;
        if (!Singleton<Studio.Studio>.Instance.treeNodeCtrl.CheckSelect(this.treeNodeObject))
          this.treeNodeObject.colorSelect = AddObjectCamera.baseColor;
        this.guideObject.visible = true;
        ((Renderer) this.meshRenderer).set_enabled(!this.cameraInfo.active & this.visibleOutside);
      }
    }

    public override void OnDelete()
    {
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      Object.Destroy((Object) this.objectItem);
      if (this.parentInfo != null)
        this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
      Singleton<Studio.Studio>.Instance.DeleteCamera(this);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
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
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
    }

    public override void OnVisible(bool _visible)
    {
      this.visibleOutside = _visible;
      ((Renderer) this.meshRenderer).set_enabled(!this.cameraInfo.active & this.visibleOutside);
    }
  }
}
