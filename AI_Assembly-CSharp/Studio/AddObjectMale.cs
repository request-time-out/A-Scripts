// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectMale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public static class AddObjectMale
  {
    public static OCICharMale Add(string _path)
    {
      ChaControl chara = Singleton<Character>.Instance.CreateChara((byte) 0, Singleton<Scene>.Instance.commonSpace, -1, (ChaFileControl) null);
      chara.chaFile.LoadCharaFile(_path, byte.MaxValue, true, true);
      chara.fileStatus.neckLookPtn = 3;
      OICharInfo _info = new OICharInfo(chara.chaFile, Studio.Studio.GetNewIndex());
      return AddObjectMale.Add(chara, _info, (ObjectCtrlInfo) null, (TreeNodeObject) null, true, Studio.Studio.optionSystem.initialPosition);
    }

    public static OCICharMale Load(
      OICharInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      OCICharMale ociCharMale = AddObjectMale.Add(Singleton<Character>.Instance.CreateChara((byte) 0, Singleton<Scene>.Instance.commonSpace, -1, _info.charFile), _info, _parent, _parentNode, false, -1);
      foreach (KeyValuePair<int, List<ObjectInfo>> keyValuePair in _info.child)
      {
        KeyValuePair<int, List<ObjectInfo>> v = keyValuePair;
        AddObjectAssist.LoadChild(v.Value, (ObjectCtrlInfo) ociCharMale, ociCharMale.dicAccessoryPoint.First<KeyValuePair<TreeNodeObject, int>>((Func<KeyValuePair<TreeNodeObject, int>, bool>) (x => x.Value == v.Key)).Key);
      }
      return ociCharMale;
    }

    private static OCICharMale Add(
      ChaControl _male,
      OICharInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCICharMale ociCharMale = new OCICharMale();
      ChaFileStatus _status = new ChaFileStatus();
      _status.Copy(_male.fileStatus);
      _male.ChangeNowCoordinate(false, true);
      _male.Load(true);
      _male.InitializeExpression(1, true);
      ociCharMale.charInfo = _male;
      ociCharMale.charReference = (ChaReference) _male;
      ociCharMale.preparation = (Preparation) _male.objAnim.GetComponent<Preparation>();
      ociCharMale.finalIK = _male.fullBodyIK;
      for (int index = 0; index < 2; ++index)
      {
        GameObject gameObject = _male.objHair.SafeGet<GameObject>(index);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          AddObjectAssist.ArrangeNames(gameObject.get_transform());
      }
      AddObjectAssist.SetupAccessoryDynamicBones((OCIChar) ociCharMale);
      AddObjectAssist.DisableComponent((OCIChar) ociCharMale);
      ociCharMale.objectInfo = (ObjectInfo) _info;
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(((Component) _male).get_transform(), _info.dicKey);
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.isActiveFunc += new GuideObject.IsActiveFunc(((ObjectCtrlInfo) ociCharMale).OnSelect);
      guideObject.SetVisibleCenter(true);
      ociCharMale.guideObject = guideObject;
      ociCharMale.optionItemCtrl = (OptionItemCtrl) ((Component) _male).get_gameObject().AddComponent<OptionItemCtrl>();
      ociCharMale.optionItemCtrl.animator = _male.animBody;
      ociCharMale.optionItemCtrl.oiCharInfo = _info;
      _info.changeAmount.onChangeScale += new Action<Vector3>(ociCharMale.optionItemCtrl.ChangeScale);
      ociCharMale.charAnimeCtrl = ociCharMale.preparation.CharAnimeCtrl;
      ociCharMale.charAnimeCtrl.oiCharInfo = _info;
      ociCharMale.yureCtrl = ociCharMale.preparation.YureCtrl;
      ociCharMale.yureCtrl.Init((OCIChar) ociCharMale);
      int group = _info.animeInfo.group;
      int category = _info.animeInfo.category;
      int no = _info.animeInfo.no;
      float animeNormalizedTime = _info.animeNormalizedTime;
      ociCharMale.LoadAnime(0, 0, 1, 0.0f);
      _male.animBody.Update(0.0f);
      _info.animeInfo.group = group;
      _info.animeInfo.category = category;
      _info.animeInfo.no = no;
      _info.animeNormalizedTime = animeNormalizedTime;
      IKSolver ikSolver = ((IK) ociCharMale.finalIK).GetIKSolver();
      if (!ikSolver.get_initiated())
        ikSolver.Initiate(((Component) ociCharMale.finalIK).get_transform());
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) ociCharMale);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) ociCharMale);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(_info.charFile.parameter.fullname, _parent1);
      treeNodeObject.enableChangeParent = true;
      treeNodeObject.treeState = _info.treeState;
      treeNodeObject.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) ociCharMale).OnVisible);
      treeNodeObject.enableVisible = true;
      treeNodeObject.visible = _info.visible;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      ociCharMale.treeNodeObject = treeNodeObject;
      AddObjectAssist.InitBone((OCIChar) ociCharMale, _male.objBodyBone.get_transform(), Singleton<Info>.Instance.dicBoneInfo);
      AddObjectAssist.InitIKTarget((OCIChar) ociCharMale, _addInfo);
      AddObjectAssist.InitLookAt((OCIChar) ociCharMale);
      AddObjectAssist.InitAccessoryPoint((OCIChar) ociCharMale);
      ociCharMale.voiceCtrl.ociChar = (OCIChar) ociCharMale;
      List<DynamicBone> source = new List<DynamicBone>();
      foreach (GameObject gameObject in _male.objHair)
        source.AddRange((IEnumerable<DynamicBone>) gameObject.GetComponents<DynamicBone>());
      ociCharMale.InitKinematic(((Component) _male).get_gameObject(), ociCharMale.finalIK, _male.neckLookCtrl, source.Where<DynamicBone>((Func<DynamicBone, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).ToArray<DynamicBone>(), (DynamicBone[]) null);
      treeNodeObject.enableAddChild = false;
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      _info.changeAmount.OnChange();
      treeNodeObject.treeState = TreeNodeObject.TreeState.Close;
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) ociCharMale);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) ociCharMale);
      ociCharMale.LoadAnime(_info.animeInfo.group, _info.animeInfo.category, _info.animeInfo.no, _info.animeNormalizedTime);
      ociCharMale.ActiveKinematicMode(OICharInfo.KinematicMode.IK, _info.enableIK, true);
      for (int index = 0; index < 5; ++index)
        ociCharMale.ActiveIK((OIBoneInfo.BoneGroup) (1 << index), _info.activeIK[index], false);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int> anonType18 in ((IEnumerable<OIBoneInfo.BoneGroup>) FKCtrl.parts).Select<OIBoneInfo.BoneGroup, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>((Func<OIBoneInfo.BoneGroup, int, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>) ((p, i) => new \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>(p, i))))
        ociCharMale.ActiveFK(anonType18.p, ociCharMale.oiCharInfo.activeFK[anonType18.i], ociCharMale.oiCharInfo.activeFK[anonType18.i]);
      ociCharMale.ActiveKinematicMode(OICharInfo.KinematicMode.FK, _info.enableFK, true);
      for (int categoryNo = 0; categoryNo < _info.expression.Length; ++categoryNo)
        ociCharMale.charInfo.EnableExpressionCategory(categoryNo, _info.expression[categoryNo]);
      ociCharMale.animeSpeed = ociCharMale.animeSpeed;
      ociCharMale.animeOptionParam1 = ociCharMale.animeOptionParam1;
      ociCharMale.animeOptionParam2 = ociCharMale.animeOptionParam2;
      _status.visibleSonAlways = _info.visibleSon;
      ociCharMale.SetSonLength(_info.sonLength);
      ociCharMale.SetVisibleSimple(_info.visibleSimple);
      ociCharMale.SetSimpleColor(_info.simpleColor);
      AddObjectAssist.UpdateState((OCIChar) ociCharMale, _status);
      return ociCharMale;
    }
  }
}
