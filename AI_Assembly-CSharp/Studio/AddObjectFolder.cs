// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectFolder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public static class AddObjectFolder
  {
    public static OCIFolder Add()
    {
      return AddObjectFolder.Load(new OIFolderInfo(Studio.Studio.GetNewIndex()), (ObjectCtrlInfo) null, (TreeNodeObject) null, true, Studio.Studio.optionSystem.initialPosition);
    }

    public static OCIFolder Load(
      OIFolderInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      ChangeAmount _source = _info.changeAmount.Clone();
      OCIFolder ociFolder = AddObjectFolder.Load(_info, _parent, _parentNode, false, -1);
      _info.changeAmount.Copy(_source, true, true, true);
      AddObjectAssist.LoadChild(_info.child, (ObjectCtrlInfo) ociFolder, (TreeNodeObject) null);
      return ociFolder;
    }

    public static OCIFolder Load(
      OIFolderInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCIFolder ociFolder = new OCIFolder();
      ociFolder.objectInfo = (ObjectInfo) _info;
      GameObject gameObject = new GameObject(_info.name);
      if (Object.op_Equality((Object) gameObject, (Object) null))
      {
        Studio.Studio.DeleteIndex(_info.dicKey);
        return (OCIFolder) null;
      }
      gameObject.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      ociFolder.objectItem = gameObject;
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _info.dicKey);
      guideObject.isActive = false;
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.enableScale = false;
      guideObject.SetVisibleCenter(true);
      ociFolder.guideObject = guideObject;
      ociFolder.childRoot = gameObject.get_transform();
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) ociFolder);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) ociFolder);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(_info.name, _parent1);
      treeNodeObject.treeState = _info.treeState;
      treeNodeObject.enableVisible = true;
      treeNodeObject.visible = _info.visible;
      treeNodeObject.baseColor = Utility.ConvertColor(180, 150, 5);
      treeNodeObject.colorSelect = treeNodeObject.baseColor;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      ociFolder.treeNodeObject = treeNodeObject;
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) ociFolder);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) ociFolder);
      return ociFolder;
    }
  }
}
