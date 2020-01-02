// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectFemale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public static class AddObjectFemale
  {
    public static OCICharFemale Add(string _path)
    {
      ChaControl chara = Singleton<Character>.Instance.CreateChara((byte) 1, Singleton<Scene>.Instance.commonSpace, -1, (ChaFileControl) null);
      chara.chaFile.LoadCharaFile(_path, byte.MaxValue, true, true);
      chara.fileStatus.neckLookPtn = 3;
      OICharInfo _info = new OICharInfo(chara.chaFile, Studio.Studio.GetNewIndex());
      return AddObjectFemale.Add(chara, _info, (ObjectCtrlInfo) null, (TreeNodeObject) null, true, Studio.Studio.optionSystem.initialPosition);
    }

    public static OCICharFemale Load(
      OICharInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode)
    {
      OCICharFemale ociCharFemale = AddObjectFemale.Add(Singleton<Character>.Instance.CreateChara((byte) 1, Singleton<Scene>.Instance.commonSpace, -1, _info.charFile), _info, _parent, _parentNode, false, -1);
      foreach (KeyValuePair<int, List<ObjectInfo>> keyValuePair in _info.child)
      {
        KeyValuePair<int, List<ObjectInfo>> v = keyValuePair;
        AddObjectAssist.LoadChild(v.Value, (ObjectCtrlInfo) ociCharFemale, ociCharFemale.dicAccessoryPoint.First<KeyValuePair<TreeNodeObject, int>>((Func<KeyValuePair<TreeNodeObject, int>, bool>) (x => x.Value == v.Key)).Key);
      }
      return ociCharFemale;
    }

    private static OCICharFemale Add(
      ChaControl _female,
      OICharInfo _info,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo,
      int _initialPosition)
    {
      OCICharFemale ociCharFemale = new OCICharFemale();
      ChaFileStatus _status = new ChaFileStatus();
      _status.Copy(_female.fileStatus);
      _female.ChangeNowCoordinate(false, true);
      _female.Load(true);
      _female.InitializeExpression(1, true);
      ociCharFemale.charInfo = _female;
      ociCharFemale.charReference = (ChaReference) _female;
      ociCharFemale.preparation = (Preparation) _female.objAnim.GetComponent<Preparation>();
      ociCharFemale.finalIK = _female.fullBodyIK;
      ociCharFemale.charInfo.hideMoz = false;
      for (int index = 0; index < 2; ++index)
      {
        GameObject gameObject = _female.objHair.SafeGet<GameObject>(index);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          AddObjectAssist.ArrangeNames(gameObject.get_transform());
      }
      AddObjectAssist.SetupAccessoryDynamicBones((OCIChar) ociCharFemale);
      AddObjectAssist.DisableComponent((OCIChar) ociCharFemale);
      ociCharFemale.objectInfo = (ObjectInfo) _info;
      GuideObject guideObject = Singleton<GuideObjectManager>.Instance.Add(((Component) _female).get_transform(), _info.dicKey);
      guideObject.scaleSelect = 0.1f;
      guideObject.scaleRot = 0.05f;
      guideObject.isActiveFunc += new GuideObject.IsActiveFunc(((ObjectCtrlInfo) ociCharFemale).OnSelect);
      guideObject.SetVisibleCenter(true);
      ociCharFemale.guideObject = guideObject;
      ociCharFemale.optionItemCtrl = (OptionItemCtrl) ((Component) _female).get_gameObject().AddComponent<OptionItemCtrl>();
      ociCharFemale.optionItemCtrl.animator = _female.animBody;
      ociCharFemale.optionItemCtrl.oiCharInfo = _info;
      _info.changeAmount.onChangeScale += new Action<Vector3>(ociCharFemale.optionItemCtrl.ChangeScale);
      ociCharFemale.charAnimeCtrl = ociCharFemale.preparation?.CharAnimeCtrl;
      if (Object.op_Implicit((Object) ociCharFemale.charAnimeCtrl))
        ociCharFemale.charAnimeCtrl.oiCharInfo = _info;
      ociCharFemale.yureCtrl = ociCharFemale.preparation.YureCtrl;
      ociCharFemale.yureCtrl.Init((OCIChar) ociCharFemale);
      if (_info.animeInfo.group == 0 && _info.animeInfo.category == 2 && _info.animeInfo.no == 11)
      {
        int group = _info.animeInfo.group;
        int category = _info.animeInfo.category;
        int no = _info.animeInfo.no;
        float animeNormalizedTime = _info.animeNormalizedTime;
        ociCharFemale.LoadAnime(0, 1, 0, 0.0f);
        _female.animBody.Update(0.0f);
        _info.animeInfo.group = group;
        _info.animeInfo.category = category;
        _info.animeInfo.no = no;
        _info.animeNormalizedTime = animeNormalizedTime;
      }
      IKSolver ikSolver = ((IK) ociCharFemale.finalIK).GetIKSolver();
      if (!ikSolver.get_initiated())
        ikSolver.Initiate(((Component) ociCharFemale.finalIK).get_transform());
      if (_addInfo)
        Studio.Studio.AddInfo((ObjectInfo) _info, (ObjectCtrlInfo) ociCharFemale);
      else
        Studio.Studio.AddObjectCtrlInfo((ObjectCtrlInfo) ociCharFemale);
      TreeNodeObject _parent1 = !Object.op_Inequality((Object) _parentNode, (Object) null) ? (_parent == null ? (TreeNodeObject) null : _parent.treeNodeObject) : _parentNode;
      TreeNodeObject treeNodeObject = Studio.Studio.AddNode(_info.charFile.parameter.fullname, _parent1);
      treeNodeObject.enableChangeParent = true;
      treeNodeObject.treeState = _info.treeState;
      treeNodeObject.onVisible += new TreeNodeObject.OnVisibleFunc(((ObjectCtrlInfo) ociCharFemale).OnVisible);
      treeNodeObject.enableVisible = true;
      treeNodeObject.visible = _info.visible;
      guideObject.guideSelect.treeNodeObject = treeNodeObject;
      ociCharFemale.treeNodeObject = treeNodeObject;
      _info.changeAmount.OnChange();
      AddObjectAssist.InitBone((OCIChar) ociCharFemale, _female.objBodyBone.get_transform(), Singleton<Info>.Instance.dicBoneInfo);
      AddObjectAssist.InitIKTarget((OCIChar) ociCharFemale, _addInfo);
      AddObjectAssist.InitLookAt((OCIChar) ociCharFemale);
      AddObjectAssist.InitAccessoryPoint((OCIChar) ociCharFemale);
      ociCharFemale.voiceCtrl.ociChar = (OCIChar) ociCharFemale;
      ociCharFemale.InitKinematic(((Component) _female).get_gameObject(), ociCharFemale.finalIK, _female.neckLookCtrl, (DynamicBone[]) null, AddObjectFemale.GetSkirtDynamic(_female.objClothes));
      treeNodeObject.enableAddChild = false;
      Studio.Studio.AddCtrlInfo((ObjectCtrlInfo) ociCharFemale);
      _parent?.OnLoadAttach(!Object.op_Inequality((Object) _parentNode, (Object) null) ? _parent.treeNodeObject : _parentNode, (ObjectCtrlInfo) ociCharFemale);
      if (_initialPosition == 1)
        _info.changeAmount.pos = Singleton<Studio.Studio>.Instance.cameraCtrl.targetPos;
      ociCharFemale.LoadAnime(_info.animeInfo.group, _info.animeInfo.category, _info.animeInfo.no, _info.animeNormalizedTime);
      for (int index = 0; index < 5; ++index)
        ociCharFemale.ActiveIK((OIBoneInfo.BoneGroup) (1 << index), _info.activeIK[index], false);
      ociCharFemale.ActiveKinematicMode(OICharInfo.KinematicMode.IK, _info.enableIK, true);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int> anonType18 in ((IEnumerable<OIBoneInfo.BoneGroup>) FKCtrl.parts).Select<OIBoneInfo.BoneGroup, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>((Func<OIBoneInfo.BoneGroup, int, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>) ((p, i) => new \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>(p, i))))
        ociCharFemale.ActiveFK(anonType18.p, ociCharFemale.oiCharInfo.activeFK[anonType18.i], false);
      ociCharFemale.ActiveKinematicMode(OICharInfo.KinematicMode.FK, _info.enableFK, true);
      for (int categoryNo = 0; categoryNo < _info.expression.Length; ++categoryNo)
        ociCharFemale.charInfo.EnableExpressionCategory(categoryNo, _info.expression[categoryNo]);
      ociCharFemale.animeSpeed = ociCharFemale.animeSpeed;
      ociCharFemale.animeOptionParam1 = ociCharFemale.animeOptionParam1;
      ociCharFemale.animeOptionParam2 = ociCharFemale.animeOptionParam2;
      foreach (byte num in _female.fileStatus.siruLv)
        num = (byte) 0;
      _status.visibleSonAlways = _info.visibleSon;
      ociCharFemale.SetSonLength(_info.sonLength);
      ociCharFemale.SetVisibleSimple(_info.visibleSimple);
      ociCharFemale.SetSimpleColor(_info.simpleColor);
      AddObjectAssist.UpdateState((OCIChar) ociCharFemale, _status);
      return ociCharFemale;
    }

    public static DynamicBone[] GetHairDynamic(GameObject[] _objHair)
    {
      if (((IList<GameObject>) _objHair).IsNullOrEmpty<GameObject>())
        return (DynamicBone[]) null;
      List<DynamicBone> source = new List<DynamicBone>();
      using (IEnumerator<GameObject> enumerator = ((IEnumerable<GameObject>) _objHair).Where<GameObject>((Func<GameObject, bool>) (o => Object.op_Inequality((Object) o, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          GameObject current = enumerator.Current;
          source.AddRange((IEnumerable<DynamicBone>) current.GetComponents<DynamicBone>());
        }
      }
      return source.Where<DynamicBone>((Func<DynamicBone, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).ToArray<DynamicBone>();
    }

    public static DynamicBone[] GetSkirtDynamic(GameObject[] _objClothes)
    {
      if (((IList<GameObject>) _objClothes).IsNullOrEmpty<GameObject>())
        return (DynamicBone[]) null;
      string[] array = Singleton<Info>.Instance.dicBoneInfo.Where<KeyValuePair<int, Info.BoneInfo>>((Func<KeyValuePair<int, Info.BoneInfo>, bool>) (v => v.Value.group == 13)).Select<KeyValuePair<int, Info.BoneInfo>, string>((Func<KeyValuePair<int, Info.BoneInfo>, string>) (v => v.Value.bone)).ToArray<string>();
      List<DynamicBone> dynamicBoneList = new List<DynamicBone>();
      int[] numArray = new int[2]{ 0, 1 };
      foreach (int index in numArray)
      {
        DynamicBone[] skirtDynamic = AddObjectFemale.GetSkirtDynamic(_objClothes[index], array);
        if (!((IList<DynamicBone>) skirtDynamic).IsNullOrEmpty<DynamicBone>())
          dynamicBoneList.AddRange((IEnumerable<DynamicBone>) skirtDynamic);
      }
      return dynamicBoneList.ToArray();
    }

    private static DynamicBone[] GetSkirtDynamic(GameObject _object, string[] _target)
    {
      return Object.op_Equality((Object) _object, (Object) null) ? (DynamicBone[]) null : ((IEnumerable<DynamicBone>) _object.GetComponentsInChildren<DynamicBone>()).Where<DynamicBone>((Func<DynamicBone, bool>) (v => AddObjectFemale.CheckNameLoop(v.m_Root, _target))).ToArray<DynamicBone>();
    }

    private static bool CheckNameLoop(Transform _transform, string[] _target)
    {
      if (Object.op_Equality((Object) _transform, (Object) null))
        return false;
      if (((IEnumerable<string>) _target).Contains<string>(((Object) _transform).get_name()))
        return true;
      if (_transform.get_childCount() == 0)
        return false;
      for (int index = 0; index < _transform.get_childCount(); ++index)
      {
        if (AddObjectFemale.CheckNameLoop(_transform.GetChild(index), _target))
          return true;
      }
      return false;
    }

    [DebuggerHidden]
    public static IEnumerator AddCoroutine(AddObjectFemale.NecessaryInfo _necessary)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AddObjectFemale.\u003CAddCoroutine\u003Ec__Iterator0()
      {
        _necessary = _necessary
      };
    }

    [DebuggerHidden]
    private static IEnumerator AddCoroutine(
      AddObjectFemale.NecessaryInfo _necessary,
      ObjectCtrlInfo _parent,
      TreeNodeObject _parentNode,
      bool _addInfo)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AddObjectFemale.\u003CAddCoroutine\u003Ec__Iterator1()
      {
        _necessary = _necessary,
        _addInfo = _addInfo,
        _parentNode = _parentNode,
        _parent = _parent
      };
    }

    public class NecessaryInfo
    {
      public bool addInfo = true;
      public OCICharFemale ocicf;
      public ChaControl chaControl;
      public OICharInfo oICharInfo;
      public ObjectCtrlInfo parent;
      public TreeNodeObject parentNode;

      public NecessaryInfo(string _path)
      {
        this.path = _path;
        this.waitTime = new Info.WaitTime();
      }

      public string path { get; private set; }

      public Info.WaitTime waitTime { get; private set; }

      public bool isOver
      {
        get
        {
          return this.waitTime.isOver;
        }
      }

      public void Next()
      {
        this.waitTime.Next();
      }
    }
  }
}
