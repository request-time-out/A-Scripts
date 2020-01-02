// Decompiled with JetBrains decompiler
// Type: Studio.OCIFolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public class OCIFolder : ObjectCtrlInfo
  {
    public GameObject objectItem;
    public Transform childRoot;

    public OIFolderInfo folderInfo
    {
      get
      {
        return this.objectInfo as OIFolderInfo;
      }
    }

    public string name
    {
      get
      {
        return this.folderInfo.name;
      }
      set
      {
        this.folderInfo.name = value;
        this.treeNodeObject.textName = value;
      }
    }

    public override void OnDelete()
    {
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
      if (!this.folderInfo.child.Contains(_child.objectInfo))
        this.folderInfo.child.Add(_child.objectInfo);
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
      if (!this.folderInfo.child.Contains(_child.objectInfo))
        this.folderInfo.child.Add(_child.objectInfo);
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
      if (!this.folderInfo.child.Remove(_child.objectInfo))
        Debug.LogError((object) "情報の消去に失敗");
      _child.parentInfo = (ObjectCtrlInfo) null;
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
    }

    public override void OnVisible(bool _visible)
    {
    }
  }
}
