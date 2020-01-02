// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Correct.Process;
using IllusionUtility.GetUtility;
using Manager;
using RootMotion;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Studio
{
  public static class AddObjectAssist
  {
    public static void InitBone(
      OCIChar _ociChar,
      Transform _transformRoot,
      Dictionary<int, Info.BoneInfo> _dicBoneInfo)
    {
      Dictionary<int, OCIChar.BoneInfo> dictionary = new Dictionary<int, OCIChar.BoneInfo>();
      foreach (KeyValuePair<int, Info.BoneInfo> keyValuePair in _dicBoneInfo)
      {
        if (_ociChar.sex != 1 || keyValuePair.Value.level != 2)
        {
          GameObject loop;
          switch (keyValuePair.Value.group)
          {
            case 7:
            case 8:
            case 9:
              loop = _ociChar.charReference.GetReferenceInfo(ChaReference.RefObjKey.HeadParent).get_transform().FindLoop(keyValuePair.Value.bone);
              break;
            default:
              loop = _transformRoot.FindLoop(keyValuePair.Value.bone);
              if (Object.op_Equality((Object) loop, (Object) null))
              {
                Debug.LogWarning((object) keyValuePair.Value.bone);
                break;
              }
              break;
          }
          if (!Object.op_Equality((Object) loop, (Object) null))
          {
            OCIChar.BoneInfo boneInfo1 = (OCIChar.BoneInfo) null;
            if (dictionary.TryGetValue(keyValuePair.Value.sync, out boneInfo1))
            {
              boneInfo1.AddSyncBone(loop);
            }
            else
            {
              OIBoneInfo _boneInfo = (OIBoneInfo) null;
              if (!_ociChar.oiCharInfo.bones.TryGetValue(keyValuePair.Key, out _boneInfo))
              {
                _boneInfo = new OIBoneInfo(Studio.Studio.GetNewIndex());
                _ociChar.oiCharInfo.bones.Add(keyValuePair.Key, _boneInfo);
              }
              switch (keyValuePair.Value.group)
              {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                  _boneInfo.group = (OIBoneInfo.BoneGroup) (1 << keyValuePair.Value.group | 1);
                  break;
                case 7:
                case 8:
                case 9:
                  _boneInfo.group = OIBoneInfo.BoneGroup.Hair;
                  break;
                case 10:
                  _boneInfo.group = OIBoneInfo.BoneGroup.Neck;
                  break;
                case 11:
                case 12:
                  _boneInfo.group = OIBoneInfo.BoneGroup.Breast;
                  break;
                case 13:
                  _boneInfo.group = OIBoneInfo.BoneGroup.Skirt;
                  break;
                default:
                  _boneInfo.group = (OIBoneInfo.BoneGroup) (1 << keyValuePair.Value.group);
                  break;
              }
              _boneInfo.level = keyValuePair.Value.level;
              GuideObject _guideObject = AddObjectAssist.AddBoneGuide(loop.get_transform(), _boneInfo.dicKey, _ociChar.guideObject, keyValuePair.Value.name);
              switch (_boneInfo.group)
              {
                case OIBoneInfo.BoneGroup.RightHand:
                case OIBoneInfo.BoneGroup.LeftHand:
                  _guideObject.scaleSelect = 0.025f;
                  break;
              }
              OCIChar.BoneInfo boneInfo2 = new OCIChar.BoneInfo(_guideObject, _boneInfo, keyValuePair.Key);
              _ociChar.listBones.Add(boneInfo2);
              _guideObject.SetActive(false, true);
              if (keyValuePair.Value.no == 65)
                _ociChar.transSon = loop.get_transform();
              if (keyValuePair.Value.sync != -1)
                dictionary.Add(keyValuePair.Key, boneInfo2);
            }
          }
        }
      }
      _ociChar.UpdateFKColor(FKCtrl.parts);
    }

    private static void TransformLoop(Transform _src, List<Transform> _list)
    {
      if (Object.op_Equality((Object) _src, (Object) null))
        return;
      _list.Add(_src);
      for (int index = 0; index < _src.get_childCount(); ++index)
        AddObjectAssist.TransformLoop(_src.GetChild(index), _list);
    }

    public static void ArrangeNames(Transform _target)
    {
      IEnumerator enumerator = _target.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AddObjectAssist.ArrangeNamesLoop((Transform) enumerator.Current);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    private static void ArrangeNamesLoop(Transform _target)
    {
      string name = ((Object) _target).get_name();
      if (Regex.Match(name, "c_J_hairF[CLRU]+[a-b]_(\\d*)", RegexOptions.IgnoreCase).Success)
        ((Object) _target).set_name(name.Replace("c_J_hairF", "c_J_hair_F"));
      else if (Regex.Match(name, "c_J_hairB[CLRU]+[a-b]_(\\d*)", RegexOptions.IgnoreCase).Success)
        ((Object) _target).set_name(name.Replace("c_J_hairB", "c_J_hair_B"));
      if (_target.get_childCount() == 0)
        return;
      IEnumerator enumerator = _target.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          AddObjectAssist.ArrangeNamesLoop((Transform) enumerator.Current);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }

    public static void InitHairBone(OCIChar _ociChar, Dictionary<int, Info.BoneInfo> _dicBoneInfo)
    {
      GameObject referenceInfo = _ociChar.charReference.GetReferenceInfo(ChaReference.RefObjKey.HeadParent);
      Dictionary<int, OCIChar.BoneInfo> dictionary = new Dictionary<int, OCIChar.BoneInfo>();
      foreach (KeyValuePair<int, Info.BoneInfo> keyValuePair in _dicBoneInfo.Where<KeyValuePair<int, Info.BoneInfo>>((Func<KeyValuePair<int, Info.BoneInfo>, bool>) (b => MathfEx.RangeEqualOn<int>(7, b.Value.group, 9))))
      {
        GameObject loop = referenceInfo.get_transform().FindLoop(keyValuePair.Value.bone);
        if (!Object.op_Equality((Object) loop, (Object) null))
        {
          OCIChar.BoneInfo boneInfo1 = (OCIChar.BoneInfo) null;
          if (dictionary.TryGetValue(keyValuePair.Value.sync, out boneInfo1))
          {
            boneInfo1.AddSyncBone(loop);
          }
          else
          {
            OIBoneInfo _boneInfo = (OIBoneInfo) null;
            if (!_ociChar.oiCharInfo.bones.TryGetValue(keyValuePair.Key, out _boneInfo))
            {
              _boneInfo = new OIBoneInfo(Studio.Studio.GetNewIndex());
              _ociChar.oiCharInfo.bones.Add(keyValuePair.Key, _boneInfo);
            }
            _boneInfo.group = OIBoneInfo.BoneGroup.Hair;
            _boneInfo.level = keyValuePair.Value.level;
            GuideObject _guideObject = AddObjectAssist.AddBoneGuide(loop.get_transform(), _boneInfo.dicKey, _ociChar.guideObject, keyValuePair.Value.name);
            OCIChar.BoneInfo boneInfo2 = new OCIChar.BoneInfo(_guideObject, _boneInfo, keyValuePair.Key);
            _ociChar.listBones.Add(boneInfo2);
            _guideObject.SetActive(false, true);
            if (keyValuePair.Value.sync != -1)
              dictionary.Add(keyValuePair.Key, boneInfo2);
          }
        }
      }
      _ociChar.UpdateFKColor(OIBoneInfo.BoneGroup.Hair);
    }

    private static GuideObject AddBoneGuide(
      Transform _target,
      int _dicKey,
      GuideObject _parent,
      string _name)
    {
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(_target, _dicKey);
      guideObject.enablePos = false;
      guideObject.enableScale = false;
      guideObject.enableMaluti = false;
      guideObject.calcScale = false;
      guideObject.scaleRate = 0.5f;
      guideObject.scaleRot = 0.025f;
      guideObject.scaleSelect = 0.05f;
      guideObject.parentGuide = _parent;
      return guideObject;
    }

    public static void InitIKTarget(OCIChar _ociChar, bool _addInfo)
    {
      IKSolverFullBodyBiped solver = (IKSolverFullBodyBiped) _ociChar.finalIK.solver;
      BipedReferences references = (BipedReferences) _ociChar.finalIK.references;
      _ociChar.ikCtrl = _ociChar.preparation.IKCtrl;
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 0, solver.get_bodyEffector(), false, (Transform) references.pelvis);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 1, solver.get_leftShoulderEffector(), false, (Transform) references.leftUpperArm);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 2, solver.get_leftArmChain(), false, (Transform) references.leftForearm);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 3, solver.get_leftHandEffector(), true, (Transform) references.leftHand);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 4, solver.get_rightShoulderEffector(), false, (Transform) references.rightUpperArm);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 5, solver.get_rightArmChain(), false, (Transform) references.rightForearm);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 6, solver.get_rightHandEffector(), true, (Transform) references.rightHand);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 7, solver.get_leftThighEffector(), false, (Transform) references.leftThigh);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 8, solver.get_leftLegChain(), false, (Transform) references.leftCalf);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 9, solver.get_leftFootEffector(), true, (Transform) references.leftFoot);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 10, solver.get_rightThighEffector(), false, (Transform) references.rightThigh);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 11, solver.get_rightLegChain(), false, (Transform) references.rightCalf);
      AddObjectAssist.AddIKTarget(_ociChar, _ociChar.ikCtrl, 12, solver.get_rightFootEffector(), true, (Transform) references.rightFoot);
      if (!_addInfo)
        return;
      _ociChar.ikCtrl.InitTarget();
    }

    public static void InitAccessoryPoint(OCIChar _ociChar)
    {
      Dictionary<int, Tuple<int, int>> dictionary1 = new Dictionary<int, Tuple<int, int>>();
      ExcelData accessoryPointGroup = Singleton<Info>.Instance.accessoryPointGroup;
      int count = accessoryPointGroup.list.Count;
      Dictionary<int, TreeNodeObject> dictionary2 = new Dictionary<int, TreeNodeObject>();
      for (int index = 1; index < count; ++index)
      {
        ExcelData.Param obj = accessoryPointGroup.list[index];
        int key = int.Parse(obj.list[0]);
        string str = obj.list[1];
        string[] strArray = obj.list[2].Split('-');
        dictionary1.Add(key, new Tuple<int, int>(int.Parse(strArray[0]), int.Parse(strArray[1])));
        TreeNodeObject _root = Studio.Studio.AddNode(string.Format("グループ : {0}", (object) str), _ociChar.treeNodeObject);
        _root.treeState = !_ociChar.oiCharInfo.dicAccessGroup.ContainsKey(key) ? TreeNodeObject.TreeState.Close : _ociChar.oiCharInfo.dicAccessGroup[key];
        _root.enableChangeParent = false;
        _root.enableDelete = false;
        _root.enableCopy = false;
        dictionary2.Add(key, _root);
        _ociChar.dicAccessPoint.Add(key, new OCIChar.AccessPointInfo(_root));
      }
      foreach (KeyValuePair<int, Tuple<int, int>> keyValuePair in dictionary1)
      {
        for (int index = keyValuePair.Value.Item1; index <= keyValuePair.Value.Item2; ++index)
        {
          TreeNodeObject _parent = dictionary2[keyValuePair.Key];
          TreeNodeObject key = Studio.Studio.AddNode(string.Format("部位 : {0}", (object) ChaAccessoryDefine.AccessoryParentName.SafeGet<string>(index)), _parent);
          key.treeState = !_ociChar.oiCharInfo.dicAccessNo.ContainsKey(index) ? TreeNodeObject.TreeState.Close : _ociChar.oiCharInfo.dicAccessNo[index];
          key.enableChangeParent = false;
          key.enableDelete = false;
          key.enableCopy = false;
          key.baseColor = Utility.ConvertColor(204, 128, 164);
          key.colorSelect = key.baseColor;
          _ociChar.dicAccessoryPoint.Add(key, index);
          OCIChar.AccessPointInfo accessPointInfo = (OCIChar.AccessPointInfo) null;
          if (_ociChar.dicAccessPoint.TryGetValue(keyValuePair.Key, out accessPointInfo))
            accessPointInfo.child.Add(index, key);
        }
      }
      foreach (KeyValuePair<int, TreeNodeObject> keyValuePair in dictionary2)
        keyValuePair.Value.enableAddChild = false;
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.RefreshHierachy();
    }

    public static void LoadChild(
      List<ObjectInfo> _child,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      foreach (ObjectInfo _child1 in _child)
        AddObjectAssist.LoadChild(_child1, _parent, _parentNode);
    }

    public static void LoadChild(
      Dictionary<int, ObjectInfo> _child,
      ObjectCtrlInfo _parent = null,
      TreeNodeObject _parentNode = null)
    {
      foreach (KeyValuePair<int, ObjectInfo> keyValuePair in _child)
        AddObjectAssist.LoadChild(keyValuePair.Value, _parent, _parentNode);
    }

    public static void LoadChild(
      ObjectInfo _child,
      ObjectCtrlInfo _parent = null,
      TreeNodeObject _parentNode = null)
    {
      switch (_child.kind)
      {
        case 0:
          OICharInfo _info = _child as OICharInfo;
          if (_info.sex == 1)
          {
            AddObjectFemale.Load(_info, _parent, _parentNode);
            break;
          }
          AddObjectMale.Load(_info, _parent, _parentNode);
          break;
        case 1:
          AddObjectItem.Load(_child as OIItemInfo, _parent, _parentNode);
          break;
        case 2:
          AddObjectLight.Load(_child as OILightInfo, _parent, _parentNode);
          break;
        case 3:
          AddObjectFolder.Load(_child as OIFolderInfo, _parent, _parentNode);
          break;
        case 4:
          AddObjectRoute.Load(_child as OIRouteInfo, _parent, _parentNode);
          break;
        case 5:
          AddObjectCamera.Load(_child as OICameraInfo, _parent, _parentNode);
          break;
      }
    }

    public static void InitLookAt(OCIChar _ociChar)
    {
      bool flag = _ociChar.oiCharInfo.lookAtTarget == null;
      if (flag)
        _ociChar.oiCharInfo.lookAtTarget = new LookAtTargetInfo(Studio.Studio.GetNewIndex());
      Transform lookAtTarget = _ociChar.preparation.LookAtTarget;
      if (flag)
        _ociChar.oiCharInfo.lookAtTarget.changeAmount.pos = lookAtTarget.get_localPosition();
      GuideObject _guideObject = Singleton<GuideObjectManager>.Instance.Add(lookAtTarget, _ociChar.oiCharInfo.lookAtTarget.dicKey);
      _guideObject.enableRot = false;
      _guideObject.enableScale = false;
      _guideObject.enableMaluti = false;
      _guideObject.scaleRate = 0.5f;
      _guideObject.scaleSelect = 0.25f;
      _guideObject.parentGuide = _ociChar.guideObject;
      _guideObject.changeAmount.OnChange();
      _guideObject.mode = GuideObject.Mode.World;
      _guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
      _ociChar.lookAtInfo = new OCIChar.LookAtInfo(_guideObject, _ociChar.oiCharInfo.lookAtTarget);
      _ociChar.lookAtInfo.active = false;
    }

    public static void SetupAccessoryDynamicBones(OCIChar _ociChar)
    {
      ChaControl charInfo = _ociChar.charInfo;
      CmpAccessory[] cmpAccessory = charInfo.cmpAccessory;
      if (((IList<CmpAccessory>) cmpAccessory).IsNullOrEmpty<CmpAccessory>())
        return;
      ChaFileAccessory.PartsInfo[] parts = charInfo.nowCoordinate.accessory.parts;
      for (int index = 0; index < cmpAccessory.Length; ++index)
      {
        if (!Object.op_Equality((Object) cmpAccessory[index], (Object) null))
          cmpAccessory[index].EnableDynamicBones(!parts[index].noShake);
      }
    }

    public static void DisableComponent(OCIChar _ociChar)
    {
      BaseProcess[] componentsInChildren = (BaseProcess[]) _ociChar.charInfo.objAnim.GetComponentsInChildren<BaseProcess>(true);
      if (((IList<BaseProcess>) componentsInChildren).IsNullOrEmpty<BaseProcess>())
        return;
      foreach (Behaviour behaviour in componentsInChildren)
        behaviour.set_enabled(false);
    }

    public static void UpdateState(OCIChar _ociChar, ChaFileStatus _status)
    {
      ChaFileStatus charFileStatus = _ociChar.charFileStatus;
      charFileStatus.Copy(_status);
      for (int _id = 0; _id < charFileStatus.clothesState.Length; ++_id)
        _ociChar.SetClothesState(_id, charFileStatus.clothesState[_id]);
      for (int _id = 0; _id < charFileStatus.showAccessory.Length; ++_id)
        _ociChar.ShowAccessory(_id, charFileStatus.showAccessory[_id]);
      int[] array1 = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(_ociChar.sex != 0 ? ChaListDefine.CategoryNo.custom_eyebrow_f : ChaListDefine.CategoryNo.custom_eyebrow_m).Keys.ToArray<int>();
      _ociChar.charInfo.ChangeEyebrowPtn(!((IEnumerable<int>) array1).Contains<int>(charFileStatus.eyebrowPtn) ? 0 : charFileStatus.eyebrowPtn, true);
      int[] array2 = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(_ociChar.sex != 0 ? ChaListDefine.CategoryNo.custom_eye_f : ChaListDefine.CategoryNo.custom_eye_m).Keys.ToArray<int>();
      _ociChar.charInfo.ChangeEyesPtn(!((IEnumerable<int>) array2).Contains<int>(charFileStatus.eyesPtn) ? 0 : charFileStatus.eyesPtn, true);
      _ociChar.ChangeBlink(charFileStatus.eyesBlink);
      _ociChar.ChangeEyesOpen(charFileStatus.eyesOpenMax);
      _ociChar.charInfo.ChangeMouthPtn(charFileStatus.mouthPtn, true);
      _ociChar.ChangeMouthOpen(_ociChar.oiCharInfo.mouthOpen);
      _ociChar.ChangeHandAnime(0, _ociChar.oiCharInfo.handPtn[0]);
      _ociChar.ChangeHandAnime(1, _ociChar.oiCharInfo.handPtn[1]);
      _ociChar.ChangeLookEyesPtn(charFileStatus.eyesLookPtn, true);
      if (_ociChar.oiCharInfo.eyesByteData != null)
      {
        using (MemoryStream memoryStream = new MemoryStream(_ociChar.oiCharInfo.eyesByteData))
        {
          using (BinaryReader reader = new BinaryReader((Stream) memoryStream))
            _ociChar.charInfo.eyeLookCtrl.eyeLookScript.LoadAngle(reader);
        }
        _ociChar.oiCharInfo.eyesByteData = (byte[]) null;
      }
      if (_ociChar.oiCharInfo.neckByteData != null)
      {
        using (MemoryStream memoryStream = new MemoryStream(_ociChar.oiCharInfo.neckByteData))
        {
          using (BinaryReader reader = new BinaryReader((Stream) memoryStream))
            _ociChar.neckLookCtrl.LoadNeckLookCtrl(reader);
        }
        _ociChar.oiCharInfo.neckByteData = (byte[]) null;
      }
      _ociChar.ChangeLookNeckPtn(charFileStatus.neckLookPtn);
      for (int index = 0; index < 5; ++index)
        _ociChar.SetSiruFlags((ChaFileDefine.SiruParts) index, _ociChar.oiCharInfo.siru[index]);
      if (_ociChar.sex == 1)
        _ociChar.charInfo.ChangeHohoAkaRate(charFileStatus.hohoAkaRate);
      _ociChar.SetVisibleSon(charFileStatus.visibleSonAlways);
      _ociChar.SetTears(_ociChar.GetTears());
      _ociChar.SetTuyaRate(_ociChar.charInfo.skinGlossRate);
      _ociChar.SetWetRate(_ociChar.charInfo.wetRate);
    }

    private static OCIChar.IKInfo AddIKTarget(
      OCIChar _ociChar,
      IKCtrl _ikCtrl,
      int _no,
      IKEffector _effector,
      bool _usedRot,
      Transform _bone)
    {
      OCIChar.IKInfo ikInfo = AddObjectAssist.AddIKTarget(_ociChar, _ikCtrl, _no, (Transform) _effector.target, _usedRot, _bone, true);
      _effector.positionWeight = (__Null) 1.0;
      _effector.rotationWeight = !_usedRot ? (__Null) 0.0 : (__Null) 1.0;
      _effector.target = (__Null) ikInfo.targetObject;
      return ikInfo;
    }

    private static OCIChar.IKInfo AddIKTarget(
      OCIChar _ociChar,
      IKCtrl _ikCtrl,
      int _no,
      FBIKChain _chain,
      bool _usedRot,
      Transform _bone)
    {
      OCIChar.IKInfo ikInfo = AddObjectAssist.AddIKTarget(_ociChar, _ikCtrl, _no, (Transform) ((IKConstraintBend) _chain.bendConstraint).bendGoal, _usedRot, _bone, false);
      ((IKConstraintBend) _chain.bendConstraint).weight = (__Null) 1.0;
      ((IKConstraintBend) _chain.bendConstraint).bendGoal = (__Null) ikInfo.targetObject;
      return ikInfo;
    }

    private static OCIChar.IKInfo AddIKTarget(
      OCIChar _ociChar,
      IKCtrl _ikCtrl,
      int _no,
      Transform _target,
      bool _usedRot,
      Transform _bone,
      bool _isRed)
    {
      OIIKTargetInfo _targetInfo = (OIIKTargetInfo) null;
      bool flag = !_ociChar.oiCharInfo.ikTarget.TryGetValue(_no, out _targetInfo);
      if (flag)
      {
        _targetInfo = new OIIKTargetInfo(Studio.Studio.GetNewIndex());
        _ociChar.oiCharInfo.ikTarget.Add(_no, _targetInfo);
      }
      switch (_no)
      {
        case 0:
          _targetInfo.group = OIBoneInfo.BoneGroup.Body;
          break;
        case 1:
        case 2:
        case 3:
          _targetInfo.group = OIBoneInfo.BoneGroup.LeftArm;
          break;
        case 4:
        case 5:
        case 6:
          _targetInfo.group = OIBoneInfo.BoneGroup.RightArm;
          break;
        case 7:
        case 8:
        case 9:
          _targetInfo.group = OIBoneInfo.BoneGroup.LeftLeg;
          break;
        case 10:
        case 11:
        case 12:
          _targetInfo.group = OIBoneInfo.BoneGroup.RightLeg;
          break;
      }
      GameObject gameObject = new GameObject(((Object) _target).get_name() + "(work)");
      gameObject.get_transform().SetParent(((Component) _ociChar.charInfo).get_transform());
      GuideObject _guideObject = Singleton<GuideObjectManager>.Instance.Add(gameObject.get_transform(), _targetInfo.dicKey);
      _guideObject.mode = GuideObject.Mode.LocalIK;
      _guideObject.enableRot = _usedRot;
      _guideObject.enableScale = false;
      _guideObject.enableMaluti = false;
      _guideObject.calcScale = false;
      _guideObject.scaleRate = 0.5f;
      _guideObject.scaleRot = 0.05f;
      _guideObject.scaleSelect = 0.1f;
      _guideObject.parentGuide = _ociChar.guideObject;
      _guideObject.guideSelect.color = !_isRed ? Color.get_blue() : Color.get_red();
      _guideObject.moveCalc = GuideMove.MoveCalc.TYPE3;
      OCIChar.IKInfo ikInfo = new OCIChar.IKInfo(_guideObject, _targetInfo, _target, gameObject.get_transform(), _bone);
      if (!flag)
        _targetInfo.changeAmount.OnChange();
      _ikCtrl.addIKInfo = ikInfo;
      _ociChar.listIKTarget.Add(ikInfo);
      _guideObject.SetActive(false, true);
      return ikInfo;
    }
  }
}
