// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

namespace Studio
{
  public static class AddObjectLight
  {
    public static OCILight Add(int _no)
    {
      int newIndex = Studio.Studio.GetNewIndex();
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new AddObjectCommand.AddLightCommand(_no, newIndex, Studio.Studio.optionSystem.initialPosition));
      return Studio.Studio.GetCtrlInfo(newIndex) as OCILight;
    }

    public static OCILight Load(
      OILightInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      ChangeAmount _source = _info.changeAmount.Clone();
      OCILight ociLight = AddObjectLight.Load(_info, _parent, _parentNode, false, -1);
      _info.changeAmount.Copy(_source, true, true, true);
      return ociLight;
    }

    public static OCILight Load(
      OILightInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCILight ociLight = new OCILight();
      Info.LightLoadInfo lightLoadInfo = (Info.LightLoadInfo) null;
      if (!Singleton<Info>.Instance.dicLightLoadInfo.TryGetValue(_info.no, out lightLoadInfo))
      {
        Debug.LogError((object) string.Format("存在しない番号[{0}]", (object) _info.no));
        return (OCILight) null;
      }
      ociLight.objectInfo = (ObjectInfo) _info;
      GameObject gameObject = Utility.LoadAsset<GameObject>(lightLoadInfo.bundlePath, lightLoadInfo.fileName, lightLoadInfo.manifest);
      gameObject.get_transform().SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      ociLight.objectLight = gameObject;
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _info.dicKey);
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.isActive = false;
      guideObject.enableScale = false;
      guideObject.SetVisibleCenter(true);
      ociLight.guideObject = guideObject;
      ociLight.lightColor = (LightColor) gameObject.GetComponent<LightColor>();
      if (Object.op_Implicit((Object) ociLight.lightColor))
        ociLight.lightColor.color = _info.color;
      ociLight.lightTarget = lightLoadInfo.target;
      switch (lightLoadInfo.target)
      {
        case Info.LightLoadInfo.Target.Chara:
          int num1 = ociLight.light.get_cullingMask() ^ LayerMask.GetMask(new string[2]
          {
            "Map",
            "MapNoShadow"
          });
          ociLight.light.set_cullingMask(num1);
          break;
        case Info.LightLoadInfo.Target.Map:
          int num2 = ociLight.light.get_cullingMask() ^ LayerMask.GetMask(new string[1]
          {
            "Chara"
          });
          ociLight.light.set_cullingMask(num2);
          break;
      }
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) ociLight);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) ociLight);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(lightLoadInfo.name, _parent1);
      treeNodeObject.enableAddChild = false;
      treeNodeObject.treeState = _info.treeState;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      ociLight.treeNodeObject = treeNodeObject;
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) ociLight);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) ociLight);
      ociLight.Update();
      return ociLight;
    }
  }
}
