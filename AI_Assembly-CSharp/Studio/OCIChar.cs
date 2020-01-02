// Decompiled with JetBrains decompiler
// Type: Studio.OCIChar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using Manager;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class OCIChar : ObjectCtrlInfo
  {
    public Dictionary<int, OCIChar.AccessPointInfo> dicAccessPoint = new Dictionary<int, OCIChar.AccessPointInfo>();
    public List<OCIChar.BoneInfo> listBones = new List<OCIChar.BoneInfo>();
    public List<OCIChar.IKInfo> listIKTarget = new List<OCIChar.IKInfo>();
    private bool[] dynamicBust = new bool[4]
    {
      true,
      true,
      true,
      true
    };
    private bool[] enablePV = new bool[8]
    {
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true
    };
    public string[] animeParam = new string[2]
    {
      "height",
      "Breast"
    };
    public Dictionary<TreeNodeObject, int> dicAccessoryPoint = new Dictionary<TreeNodeObject, int>();
    private OCIChar.LoadedAnimeInfo _loadedAnimeInfo = new OCIChar.LoadedAnimeInfo();
    public ChaReference charReference;
    public OCIChar.LookAtInfo lookAtInfo;
    public ChaControl charInfo;
    public FKCtrl fkCtrl;
    public IKCtrl ikCtrl;
    public FullBodyBipedIK finalIK;
    public NeckLookControllerVer2 neckLookCtrl;
    public DynamicBone[] skirtDynamic;
    public OptionItemCtrl optionItemCtrl;
    public bool isAnimeMotion;
    public bool isHAnime;
    public CharAnimeCtrl charAnimeCtrl;
    public YureCtrl yureCtrl;

    public OICharInfo oiCharInfo
    {
      get
      {
        return this.objectInfo as OICharInfo;
      }
    }

    public Transform transSon
    {
      get
      {
        return Object.op_Implicit((Object) this.charAnimeCtrl) ? this.charAnimeCtrl.transSon : (Transform) null;
      }
      set
      {
        if (!Object.op_Implicit((Object) this.charAnimeCtrl))
          return;
        this.charAnimeCtrl.transSon = value;
      }
    }

    public ChaFileStatus charFileStatus
    {
      get
      {
        return this.charInfo.fileStatus;
      }
    }

    public int sex
    {
      get
      {
        return (int) this.charInfo.fileParam.sex;
      }
    }

    public int HandAnimeNum
    {
      get
      {
        return this.charInfo.GetShapeIndexHandCount();
      }
    }

    public bool DynamicAnimeBustL
    {
      get
      {
        return this.dynamicBust[0];
      }
      set
      {
        this.dynamicBust[0] = value;
        this.UpdateDynamicBonesBust(0);
      }
    }

    public bool DynamicAnimeBustR
    {
      get
      {
        return this.dynamicBust[1];
      }
      set
      {
        this.dynamicBust[1] = value;
        this.UpdateDynamicBonesBust(1);
      }
    }

    public bool DynamicFKBustL
    {
      get
      {
        return this.dynamicBust[2];
      }
      set
      {
        this.dynamicBust[2] = value;
        this.UpdateDynamicBonesBust(0);
      }
    }

    public bool DynamicFKBustR
    {
      get
      {
        return this.dynamicBust[3];
      }
      set
      {
        this.dynamicBust[3] = value;
        this.UpdateDynamicBonesBust(1);
      }
    }

    public VoiceCtrl voiceCtrl
    {
      get
      {
        return this.oiCharInfo.voiceCtrl;
      }
    }

    public VoiceCtrl.Repeat voiceRepeat
    {
      get
      {
        return this.voiceCtrl.repeat;
      }
      set
      {
        this.voiceCtrl.repeat = value;
      }
    }

    private int neckPtnOld { get; set; }

    protected int breastLayer { get; set; }

    protected OCIChar.LoadedAnimeInfo loadedAnimeInfo
    {
      get
      {
        return this._loadedAnimeInfo;
      }
    }

    public Preparation preparation { get; set; }

    public override void OnDelete()
    {
      Singleton<GuideObjectManager>.Instance.Delete(this.guideObject, true);
      this.voiceCtrl.Stop();
      for (int index = 0; index < this.listBones.Count; ++index)
        Singleton<GuideObjectManager>.Instance.Delete(this.listBones[index].guideObject, true);
      for (int index = 0; index < this.listIKTarget.Count; ++index)
        Singleton<GuideObjectManager>.Instance.Delete(this.listIKTarget[index].guideObject, true);
      Singleton<GuideObjectManager>.Instance.Delete(this.lookAtInfo.guideObject, true);
      if (this.parentInfo != null)
        this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      Studio.Studio.DeleteInfo(this.objectInfo, true);
    }

    public override void OnAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      int index = -1;
      if (!this.dicAccessoryPoint.TryGetValue(_parent, out index))
      {
        Debug.LogWarning((object) "ありえないはず");
      }
      else
      {
        if (_child.parentInfo == null)
          Studio.Studio.DeleteInfo(_child.objectInfo, false);
        else
          _child.parentInfo.OnDetachChild(_child);
        if (!this.oiCharInfo.child[index].Contains(_child.objectInfo))
          this.oiCharInfo.child[index].Add(_child.objectInfo);
        bool flag = false;
        Transform accessoryParentTransform = this.charInfo.GetAccessoryParentTransform(index);
        if (!flag)
          _child.guideObject.transformTarget.SetParent(accessoryParentTransform);
        _child.guideObject.parent = accessoryParentTransform;
        _child.guideObject.mode = GuideObject.Mode.World;
        _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
        if (!flag)
        {
          _child.objectInfo.changeAmount.pos = _child.guideObject.transformTarget.get_localPosition();
          _child.objectInfo.changeAmount.rot = _child.guideObject.transformTarget.get_localEulerAngles();
        }
        else
        {
          _child.objectInfo.changeAmount.pos = _child.guideObject.parent.InverseTransformPoint(_child.objectInfo.changeAmount.pos);
          Quaternion quaternion = Quaternion.op_Multiply(Quaternion.Euler(_child.objectInfo.changeAmount.rot), Quaternion.Inverse(_child.guideObject.parent.get_rotation()));
          _child.objectInfo.changeAmount.rot = ((Quaternion) ref quaternion).get_eulerAngles();
        }
        _child.guideObject.nonconnect = flag;
        _child.guideObject.calcScale = !flag;
        _child.parentInfo = (ObjectCtrlInfo) this;
      }
    }

    public override void OnLoadAttach(TreeNodeObject _parent, ObjectCtrlInfo _child)
    {
      int index = -1;
      if (!this.dicAccessoryPoint.TryGetValue(_parent, out index))
      {
        Debug.LogWarning((object) "ありえないはず");
      }
      else
      {
        if (_child.parentInfo == null)
          Studio.Studio.DeleteInfo(_child.objectInfo, false);
        else
          _child.parentInfo.OnDetachChild(_child);
        if (!this.oiCharInfo.child[index].Contains(_child.objectInfo))
          this.oiCharInfo.child[index].Add(_child.objectInfo);
        bool flag = false;
        Transform accessoryParentTransform = this.charInfo.GetAccessoryParentTransform(index);
        if (!flag)
          _child.guideObject.transformTarget.SetParent(accessoryParentTransform);
        _child.guideObject.parent = accessoryParentTransform;
        _child.guideObject.mode = GuideObject.Mode.World;
        _child.guideObject.moveCalc = GuideMove.MoveCalc.TYPE2;
        _child.guideObject.changeAmount.OnChange();
        _child.guideObject.nonconnect = flag;
        _child.guideObject.calcScale = !flag;
        _child.parentInfo = (ObjectCtrlInfo) this;
      }
    }

    public override void OnDetach()
    {
      this.parentInfo.OnDetachChild((ObjectCtrlInfo) this);
      this.guideObject.parent = (Transform) null;
      Studio.Studio.AddInfo(this.objectInfo, (ObjectCtrlInfo) this);
      this.guideObject.transformTarget.SetParent(Singleton<Scene>.Instance.commonSpace.get_transform());
      this.objectInfo.changeAmount.pos = this.guideObject.transformTarget.get_localPosition();
      this.objectInfo.changeAmount.rot = this.guideObject.transformTarget.get_localEulerAngles();
      this.guideObject.mode = GuideObject.Mode.Local;
      this.guideObject.moveCalc = GuideMove.MoveCalc.TYPE1;
      this.treeNodeObject.ResetVisible();
    }

    public override void OnSelect(bool _select)
    {
      int layer = LayerMask.NameToLayer(!_select ? "Studio/Select" : "Studio/Col");
      this.lookAtInfo.layer = layer;
      for (int index = 0; index < this.listBones.Count; ++index)
        this.listBones[index].layer = layer;
      for (int index = 0; index < this.listIKTarget.Count; ++index)
        this.listIKTarget[index].layer = layer;
    }

    public override void OnDetachChild(ObjectCtrlInfo _child)
    {
      foreach (KeyValuePair<int, List<ObjectInfo>> keyValuePair in this.oiCharInfo.child)
      {
        if (keyValuePair.Value.Remove(_child.objectInfo))
          break;
      }
      _child.parentInfo = (ObjectCtrlInfo) null;
    }

    public override void OnSavePreprocessing()
    {
      base.OnSavePreprocessing();
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
        {
          this.neckLookCtrl.SaveNeckLookCtrl(writer);
          this.oiCharInfo.neckByteData = memoryStream.ToArray();
        }
      }
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
        {
          this.charInfo.eyeLookCtrl.eyeLookScript.SaveAngle(writer);
          this.oiCharInfo.eyesByteData = memoryStream.ToArray();
        }
      }
      AnimatorStateInfo animatorStateInfo = this.charInfo.animBody.GetCurrentAnimatorStateInfo(0);
      this.oiCharInfo.animeNormalizedTime = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
      this.oiCharInfo.dicAccessGroup = new Dictionary<int, TreeNodeObject.TreeState>();
      this.oiCharInfo.dicAccessNo = new Dictionary<int, TreeNodeObject.TreeState>();
      foreach (KeyValuePair<int, OCIChar.AccessPointInfo> keyValuePair1 in this.dicAccessPoint)
      {
        this.oiCharInfo.dicAccessGroup.Add(keyValuePair1.Key, keyValuePair1.Value.root.treeState);
        foreach (KeyValuePair<int, TreeNodeObject> keyValuePair2 in keyValuePair1.Value.child)
          this.oiCharInfo.dicAccessNo.Add(keyValuePair2.Key, keyValuePair2.Value.treeState);
      }
    }

    public override float animeSpeed
    {
      get
      {
        return this.oiCharInfo.animeSpeed;
      }
      set
      {
        this.oiCharInfo.animeSpeed = value;
        if (!Object.op_Implicit((Object) this.charInfo.animBody))
          return;
        this.charInfo.animBody.set_speed(value);
      }
    }

    public override void OnVisible(bool _visible)
    {
      this.charInfo.visibleAll = _visible;
      if (Object.op_Implicit((Object) this.optionItemCtrl))
        this.optionItemCtrl.outsideVisible = _visible;
      foreach (OCIChar.BoneInfo listBone in this.listBones)
        listBone.guideObject.visibleOutside = _visible;
      foreach (OCIChar.IKInfo ikInfo in this.listIKTarget)
        ikInfo.guideObject.visibleOutside = _visible;
      if (this.lookAtInfo == null || !Object.op_Implicit((Object) this.lookAtInfo.guideObject))
        return;
      this.lookAtInfo.guideObject.visibleOutside = _visible;
    }

    public void InitKinematic(
      GameObject _target,
      FullBodyBipedIK _finalIK,
      NeckLookControllerVer2 _neckLook,
      DynamicBone[] _hairDynamic,
      DynamicBone[] _skirtDynamic)
    {
      this.neckLookCtrl = _neckLook;
      this.neckPtnOld = this.charFileStatus.neckLookPtn;
      this.skirtDynamic = _skirtDynamic;
      this.InitFK(_target);
      for (int index = 0; index < this.listIKTarget.Count; ++index)
        this.listIKTarget[index].active = false;
      this.finalIK = _finalIK;
      ((Behaviour) this.finalIK).set_enabled(false);
    }

    public void InitFK(GameObject _target)
    {
      if (Object.op_Equality((Object) this.fkCtrl, (Object) null) && Object.op_Inequality((Object) _target, (Object) null))
        this.fkCtrl = (FKCtrl) _target.AddComponent<FKCtrl>();
      this.fkCtrl.InitBones(this.oiCharInfo, this.charReference);
      ((Behaviour) this.fkCtrl).set_enabled(false);
      for (int index = 0; index < this.listBones.Count; ++index)
        this.listBones[index].active = false;
    }

    public void ActiveKinematicMode(OICharInfo.KinematicMode _mode, bool _active, bool _force)
    {
      switch (_mode)
      {
        case OICharInfo.KinematicMode.FK:
          if (_force || ((Behaviour) this.fkCtrl).get_enabled() != _active)
          {
            ((Behaviour) this.fkCtrl).set_enabled(_active);
            this.oiCharInfo.enableFK = _active;
            OIBoneInfo.BoneGroup[] parts = FKCtrl.parts;
            for (int index = 0; index < parts.Length; ++index)
              this.ActiveFK(parts[index], _active && this.oiCharInfo.activeFK[index], true);
            if (this.oiCharInfo.enableFK)
            {
              this.ActiveKinematicMode(OICharInfo.KinematicMode.IK, false, _force);
              break;
            }
            break;
          }
          break;
        case OICharInfo.KinematicMode.IK:
          if (_force || ((Behaviour) this.finalIK).get_enabled() != _active)
          {
            ((Behaviour) this.finalIK).set_enabled(_active);
            this.oiCharInfo.enableIK = _active;
            for (int index = 0; index < 5; ++index)
              this.ActiveIK((OIBoneInfo.BoneGroup) (1 << index), _active && this.oiCharInfo.activeIK[index], true);
            if (this.oiCharInfo.enableIK)
            {
              this.ActiveKinematicMode(OICharInfo.KinematicMode.FK, false, _force);
              break;
            }
            break;
          }
          break;
      }
      for (int index = 0; index < 4; ++index)
        this.preparation.PvCopy[index] = !this.oiCharInfo.enableFK && this.enablePV[index];
    }

    public void ActiveFK(OIBoneInfo.BoneGroup _group, bool _active, bool _force = false)
    {
      OIBoneInfo.BoneGroup[] parts = FKCtrl.parts;
      for (int i = 0; i < parts.Length; ++i)
      {
        if ((_group & parts[i]) != (OIBoneInfo.BoneGroup) 0 && (_force || Utility.SetStruct<bool>(ref this.oiCharInfo.activeFK[i], _active) && this.oiCharInfo.enableFK))
        {
          this.ActiveFKGroup(parts[i], _active);
          foreach (OCIChar.BoneInfo boneInfo in this.listBones.Where<OCIChar.BoneInfo>((System.Func<OCIChar.BoneInfo, bool>) (v => (v.boneGroup & parts[i]) != (OIBoneInfo.BoneGroup) 0)))
            boneInfo.active = _force ? _active : this.oiCharInfo.enableFK & this.oiCharInfo.activeFK[i];
        }
      }
    }

    public bool IsFKGroup(OIBoneInfo.BoneGroup _group)
    {
      return this.listBones.Any<OCIChar.BoneInfo>((System.Func<OCIChar.BoneInfo, bool>) (v => (v.boneGroup & _group) != (OIBoneInfo.BoneGroup) 0));
    }

    public void InitFKBone(OIBoneInfo.BoneGroup _group)
    {
      foreach (OCIChar.BoneInfo boneInfo in this.listBones.Where<OCIChar.BoneInfo>((System.Func<OCIChar.BoneInfo, bool>) (v => (v.boneGroup & _group) != (OIBoneInfo.BoneGroup) 0)))
        boneInfo.boneInfo.changeAmount.Reset();
    }

    private void ActiveFKGroup(OIBoneInfo.BoneGroup _group, bool _active)
    {
      switch (_group)
      {
        case OIBoneInfo.BoneGroup.Neck:
          if (_active)
          {
            this.neckPtnOld = this.charFileStatus.neckLookPtn;
            this.ChangeLookNeckPtn(4);
            break;
          }
          this.ChangeLookNeckPtn(this.neckPtnOld);
          break;
        case OIBoneInfo.BoneGroup.Breast:
          this.DynamicFKBustL = !_active;
          this.DynamicFKBustR = !_active;
          break;
      }
      this.fkCtrl.SetEnable(_group, _active);
      if (_group != OIBoneInfo.BoneGroup.Hair)
      {
        if (_group != OIBoneInfo.BoneGroup.Skirt || ((IList<DynamicBone>) this.skirtDynamic).IsNullOrEmpty<DynamicBone>())
          return;
        for (int index = 0; index < this.skirtDynamic.Length; ++index)
          ((Behaviour) this.skirtDynamic[index]).set_enabled(!_active);
      }
      else
      {
        ChaFileHair.PartsInfo[] parts = this.charInfo.fileCustom.hair.parts;
        for (int index = 0; index < parts.Length; ++index)
        {
          CmpHair cmpHair = this.charInfo.cmpHair.SafeGet<CmpHair>(index);
          if (!Object.op_Equality((Object) cmpHair, (Object) null))
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in parts[index].dictBundle)
            {
              KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> v = keyValuePair;
              cmpHair.boneInfo.SafeProc<CmpHair.BoneInfo>(v.Key, (Action<CmpHair.BoneInfo>) (_info =>
              {
                if (((IList<DynamicBone>) _info.dynamicBone).IsNullOrEmpty<DynamicBone>())
                  return;
                foreach (Behaviour behaviour in ((IEnumerable<DynamicBone>) _info.dynamicBone).Where<DynamicBone>((System.Func<DynamicBone, bool>) (_v => Object.op_Inequality((Object) _v, (Object) null))))
                  behaviour.set_enabled(!_active & !v.Value.noShake);
              }));
            }
          }
        }
      }
    }

    public void ActiveIK(OIBoneInfo.BoneGroup _group, bool _active, bool _force = false)
    {
      for (int index = 0; index < 5; ++index)
      {
        OIBoneInfo.BoneGroup target = (OIBoneInfo.BoneGroup) (1 << index);
        if ((_group & target) != (OIBoneInfo.BoneGroup) 0 && (_force || Utility.SetStruct<bool>(ref this.oiCharInfo.activeIK[index], _active)))
        {
          this.ActiveIKGroup(target, _active);
          foreach (OCIChar.IKInfo ikInfo in this.listIKTarget.Where<OCIChar.IKInfo>((System.Func<OCIChar.IKInfo, bool>) (v => (v.boneGroup & target) != (OIBoneInfo.BoneGroup) 0)))
            ikInfo.active = _force ? _active : this.oiCharInfo.enableIK & this.oiCharInfo.activeIK[index];
        }
      }
    }

    private void ActiveIKGroup(OIBoneInfo.BoneGroup _group, bool _active)
    {
      IKSolverFullBodyBiped solver = (IKSolverFullBodyBiped) this.finalIK.solver;
      float num = !_active ? 0.0f : 1f;
      switch (_group)
      {
        case OIBoneInfo.BoneGroup.Body:
          ((IKMappingSpine) ((IKSolverFullBody) solver).spineMapping).twistWeight = (__Null) (double) num;
          solver.SetEffectorWeights((FullBodyBipedEffector) 0, num, num);
          break;
        case OIBoneInfo.BoneGroup.RightLeg:
          solver.get_rightLegMapping().weight = (__Null) (double) num;
          solver.SetEffectorWeights((FullBodyBipedEffector) 4, num, num);
          solver.SetEffectorWeights((FullBodyBipedEffector) 8, num, num);
          break;
        case OIBoneInfo.BoneGroup.LeftLeg:
          solver.get_leftLegMapping().weight = (__Null) (double) num;
          solver.SetEffectorWeights((FullBodyBipedEffector) 3, num, num);
          solver.SetEffectorWeights((FullBodyBipedEffector) 7, num, num);
          break;
        case OIBoneInfo.BoneGroup.RightArm:
          solver.get_rightArmMapping().weight = (__Null) (double) num;
          solver.SetEffectorWeights((FullBodyBipedEffector) 2, num, num);
          solver.SetEffectorWeights((FullBodyBipedEffector) 6, num, num);
          break;
        default:
          if (_group != OIBoneInfo.BoneGroup.LeftArm)
            break;
          solver.get_leftArmMapping().weight = (__Null) (double) num;
          solver.SetEffectorWeights((FullBodyBipedEffector) 1, num, num);
          solver.SetEffectorWeights((FullBodyBipedEffector) 5, num, num);
          break;
      }
    }

    public void UpdateFKColor(params OIBoneInfo.BoneGroup[] _parts)
    {
      if (((IList<OIBoneInfo.BoneGroup>) _parts).IsNullOrEmpty<OIBoneInfo.BoneGroup>())
        return;
      foreach (OCIChar.BoneInfo listBone in this.listBones)
      {
        OCIChar.BoneInfo v = listBone;
        int index = Array.FindIndex<OIBoneInfo.BoneGroup>(_parts, (Predicate<OIBoneInfo.BoneGroup>) (p => (p & v.boneGroup) != (OIBoneInfo.BoneGroup) 0));
        if (index != -1)
        {
          switch (_parts[index])
          {
            case OIBoneInfo.BoneGroup.Body:
              v.color = Studio.Studio.optionSystem.colorFKBody;
              continue;
            case OIBoneInfo.BoneGroup.RightHand:
              v.color = Studio.Studio.optionSystem.colorFKRightHand;
              continue;
            case OIBoneInfo.BoneGroup.LeftHand:
              v.color = Studio.Studio.optionSystem.colorFKLeftHand;
              continue;
            case OIBoneInfo.BoneGroup.Hair:
              v.color = Studio.Studio.optionSystem.colorFKHair;
              continue;
            case OIBoneInfo.BoneGroup.Neck:
              v.color = Studio.Studio.optionSystem.colorFKNeck;
              continue;
            case OIBoneInfo.BoneGroup.Breast:
              v.color = Studio.Studio.optionSystem.colorFKBreast;
              continue;
            case OIBoneInfo.BoneGroup.Skirt:
              v.color = Studio.Studio.optionSystem.colorFKSkirt;
              continue;
            default:
              continue;
          }
        }
      }
    }

    public void VisibleFKGuide(bool _visible)
    {
      foreach (OCIChar.BoneInfo listBone in this.listBones)
        listBone.guideObject.visible = _visible;
    }

    public void VisibleIKGuide(bool _visible)
    {
      foreach (OCIChar.IKInfo ikInfo in this.listIKTarget)
        ikInfo.guideObject.visible = _visible;
    }

    public void EnableExpressionCategory(int _category, bool _value)
    {
      this.oiCharInfo.expression[_category] = _value;
      this.charInfo.EnableExpressionCategory(_category, _value);
    }

    public void UpdateDynamicBonesBust(int _type = 2)
    {
      if (_type == 0 || _type == 2)
        this.EnableDynamicBonesBustAndHip(this.dynamicBust[0] & this.dynamicBust[2], 0);
      if (_type != 1 && _type != 2)
        return;
      this.EnableDynamicBonesBustAndHip(this.dynamicBust[1] & this.dynamicBust[3], 1);
    }

    public void EnableDynamicBonesBustAndHip(bool _enable, int _kind)
    {
      this.charInfo.cmpBoneBody.EnableDynamicBonesBustAndHip(_enable, _kind);
    }

    public float animePattern
    {
      get
      {
        return this.oiCharInfo.animePattern;
      }
      set
      {
        this.oiCharInfo.animePattern = value;
        if (this.isAnimeMotion)
          this.charInfo.setAnimatorParamFloat("motion", this.oiCharInfo.animePattern);
        if (!Object.op_Implicit((Object) this.optionItemCtrl))
          return;
        this.optionItemCtrl.SetMotion(this.oiCharInfo.animePattern);
      }
    }

    public float[] animeOptionParam
    {
      get
      {
        return this.oiCharInfo.animeOptionParam;
      }
    }

    public float animeOptionParam1
    {
      get
      {
        return this.oiCharInfo.animeOptionParam[0];
      }
      set
      {
        this.oiCharInfo.animeOptionParam[0] = value;
        if (!this.isHAnime || this.animeParam[0].IsNullOrEmpty())
          return;
        this.charInfo.setAnimatorParamFloat(this.animeParam[0], value);
      }
    }

    public float animeOptionParam2
    {
      get
      {
        return this.oiCharInfo.animeOptionParam[1];
      }
      set
      {
        this.oiCharInfo.animeOptionParam[1] = value;
        if (!this.isHAnime || this.animeParam[1].IsNullOrEmpty())
          return;
        this.charInfo.setAnimatorParamFloat(this.animeParam[1], value);
      }
    }

    public virtual void LoadAnime(int _group, int _category, int _no, float _normalizedTime = 0.0f)
    {
      Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>> dictionary1 = (Dictionary<int, Dictionary<int, Info.AnimeLoadInfo>>) null;
      if (!Singleton<Info>.Instance.dicAnimeLoadInfo.TryGetValue(_group, out dictionary1))
        return;
      Dictionary<int, Info.AnimeLoadInfo> dictionary2 = (Dictionary<int, Info.AnimeLoadInfo>) null;
      if (!dictionary1.TryGetValue(_category, out dictionary2))
        return;
      Info.AnimeLoadInfo _info = (Info.AnimeLoadInfo) null;
      if (!dictionary2.TryGetValue(_no, out _info))
        return;
      if (this.loadedAnimeInfo.BaseCheck(_info.bundlePath, _info.fileName))
      {
        this.charInfo.LoadAnimation(_info.bundlePath, _info.fileName, string.Empty);
        this.loadedAnimeInfo.baseFile.bundlePath = _info.bundlePath;
        this.loadedAnimeInfo.baseFile.fileName = _info.fileName;
      }
      if (_info is Info.HAnimeLoadInfo)
      {
        Info.HAnimeLoadInfo hanimeLoadInfo = _info as Info.HAnimeLoadInfo;
        if (hanimeLoadInfo.overrideFile.Check)
        {
          if (this.loadedAnimeInfo.OverrideCheck(hanimeLoadInfo.overrideFile.bundlePath, hanimeLoadInfo.overrideFile.fileName))
          {
            CommonLib.LoadAsset<RuntimeAnimatorController>(hanimeLoadInfo.overrideFile.bundlePath, hanimeLoadInfo.overrideFile.fileName, false, string.Empty).SafeProc<RuntimeAnimatorController>((Action<RuntimeAnimatorController>) (rac => this.charAnimeCtrl.animator.set_runtimeAnimatorController((RuntimeAnimatorController) Illusion.Utils.Animator.SetupAnimatorOverrideController(this.charAnimeCtrl.animator.get_runtimeAnimatorController(), rac))));
            AssetBundleManager.UnloadAssetBundle(hanimeLoadInfo.overrideFile.bundlePath, true, (string) null, false);
            this.loadedAnimeInfo.overrideFile.bundlePath = hanimeLoadInfo.overrideFile.bundlePath;
            this.loadedAnimeInfo.overrideFile.fileName = hanimeLoadInfo.overrideFile.fileName;
          }
        }
        else
          this.loadedAnimeInfo.overrideFile.Clear();
        this.isAnimeMotion = hanimeLoadInfo.isMotion;
        this.isHAnime = true;
        this.animeParam[1] = this.CheckAnimeParam("Breast1", "Breast", "breast");
        if (!this.animeParam[1].IsNullOrEmpty())
          this.charInfo.setAnimatorParamFloat(this.animeParam[1], this.charInfo.fileBody.shapeValueBody[1]);
        if (this.breastLayer != -1)
        {
          this.charAnimeCtrl.animator.SetLayerWeight(this.breastLayer, 0.0f);
          this.breastLayer = -1;
        }
        if (hanimeLoadInfo.isBreastLayer)
        {
          this.charAnimeCtrl.animator.SetLayerWeight(hanimeLoadInfo.breastLayer, 1f);
          this.breastLayer = hanimeLoadInfo.breastLayer;
          this.charAnimeCtrl.Play(_info.clip, _normalizedTime, hanimeLoadInfo.breastLayer);
        }
        if (hanimeLoadInfo.isMotion)
          this.charInfo.setAnimatorParamFloat("motion", this.oiCharInfo.animePattern);
        for (int index = 0; index < 8; ++index)
        {
          this.enablePV[index] = hanimeLoadInfo.pv[index];
          this.preparation.PvCopy[index] = !this.oiCharInfo.enableFK && this.enablePV[index];
        }
        if (!hanimeLoadInfo.yureFile.Check || !this.yureCtrl.Load(hanimeLoadInfo.yureFile.bundlePath, hanimeLoadInfo.yureFile.fileName, hanimeLoadInfo.motionID, hanimeLoadInfo.num))
          this.yureCtrl.ResetShape(true);
        this.charInfo.setAnimatorParamFloat("speed", 1f);
      }
      else
      {
        this.loadedAnimeInfo.overrideFile.Clear();
        for (int index = 0; index < 4; ++index)
        {
          this.enablePV[index] = true;
          this.preparation.PvCopy[index] = !this.oiCharInfo.enableFK && this.enablePV[index];
        }
        this.isAnimeMotion = false;
        this.isHAnime = false;
      }
      this.optionItemCtrl.LoadAnimeItem(_info, _info.clip, this.charInfo.fileBody.shapeValueBody[0], this.oiCharInfo.animePattern);
      if ((double) _normalizedTime != 0.0)
        this.charAnimeCtrl.Play(_info.clip, _normalizedTime);
      else
        this.charAnimeCtrl.Play(_info.clip);
      this.animeParam[0] = this.CheckAnimeParam("height1", "height");
      if (!this.animeParam[0].IsNullOrEmpty())
        this.charInfo.setAnimatorParamFloat(this.animeParam[0], this.charInfo.fileBody.shapeValueBody[0]);
      this.animeOptionParam1 = this.animeOptionParam1;
      this.animeOptionParam2 = this.animeOptionParam2;
      this.charAnimeCtrl.nameHadh = Animator.StringToHash(_info.clip);
      this.oiCharInfo.animeInfo.Set(_group, _category, _no);
      this.SetNipStand(this.oiCharInfo.nipple);
      this.SetSonLength(this.oiCharInfo.sonLength);
    }

    public virtual void ChangeHandAnime(int _type, int _ptn)
    {
      this.oiCharInfo.handPtn[_type] = _ptn;
      if (_ptn != 0)
        this.charInfo.SetShapeHandValue(_type, _ptn - 1, 0, 0.0f);
      this.charInfo.SetEnableShapeHand(_type, _ptn != 0);
    }

    public virtual void RestartAnime()
    {
      AnimatorStateInfo animatorStateInfo = this.charInfo.getAnimatorStateInfo(0);
      this.charInfo.animBody.Play(((AnimatorStateInfo) ref animatorStateInfo).get_shortNameHash(), 0, 0.0f);
      this.optionItemCtrl.PlayAnime();
    }

    private string CheckAnimeParam(params string[] _names)
    {
      AnimatorControllerParameter[] parameters = this.charInfo.animBody.get_parameters();
      if (((IList<AnimatorControllerParameter>) parameters).IsNullOrEmpty<AnimatorControllerParameter>())
        return string.Empty;
      for (int i = 0; i < _names.Length; ++i)
      {
        if (((IEnumerable<AnimatorControllerParameter>) parameters).FirstOrDefault<AnimatorControllerParameter>((System.Func<AnimatorControllerParameter, bool>) (p => string.CompareOrdinal(p.get_name(), _names[i]) == 0)) != null)
          return _names[i];
      }
      return string.Empty;
    }

    public virtual void ChangeChara(string _path)
    {
      foreach (OCIChar.BoneInfo boneInfo in this.listBones.Where<OCIChar.BoneInfo>((System.Func<OCIChar.BoneInfo, bool>) (v => v.boneGroup == OIBoneInfo.BoneGroup.Hair)).ToList<OCIChar.BoneInfo>())
        Singleton<GuideObjectManager>.Instance.Delete(boneInfo.guideObject, true);
      this.listBones = this.listBones.Where<OCIChar.BoneInfo>((System.Func<OCIChar.BoneInfo, bool>) (v => v.boneGroup != OIBoneInfo.BoneGroup.Hair)).ToList<OCIChar.BoneInfo>();
      foreach (int key in this.oiCharInfo.bones.Where<KeyValuePair<int, OIBoneInfo>>((System.Func<KeyValuePair<int, OIBoneInfo>, bool>) (b => b.Value.group == OIBoneInfo.BoneGroup.Hair)).Select<KeyValuePair<int, OIBoneInfo>, int>((System.Func<KeyValuePair<int, OIBoneInfo>, int>) (b => b.Key)).ToArray<int>())
        this.oiCharInfo.bones.Remove(key);
      this.skirtDynamic = (DynamicBone[]) null;
      this.charInfo.chaFile.LoadCharaFile(_path, byte.MaxValue, true, true);
      this.charInfo.ChangeNowCoordinate(false, true);
      this.charInfo.Reload(false, false, false, false, true);
      for (int index = 0; index < 2; ++index)
      {
        GameObject gameObject = this.charInfo.objHair.SafeGet<GameObject>(index);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          AddObjectAssist.ArrangeNames(gameObject.get_transform());
      }
      this.treeNodeObject.textName = this.charInfo.chaFile.parameter.fullname;
      AddObjectAssist.InitHairBone(this, Singleton<Info>.Instance.dicBoneInfo);
      this.skirtDynamic = AddObjectFemale.GetSkirtDynamic(this.charInfo.objClothes);
      this.InitFK((GameObject) null);
      // ISSUE: object of a compiler-generated type is created
      foreach (\u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int> anonType18 in ((IEnumerable<OIBoneInfo.BoneGroup>) FKCtrl.parts).Select<OIBoneInfo.BoneGroup, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>((System.Func<OIBoneInfo.BoneGroup, int, \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>>) ((p, i) => new \u003C\u003E__AnonType18<OIBoneInfo.BoneGroup, int>(p, i))))
        this.ActiveFK(anonType18.p, this.oiCharInfo.activeFK[anonType18.i], this.oiCharInfo.activeFK[anonType18.i]);
      this.ActiveKinematicMode(OICharInfo.KinematicMode.FK, this.oiCharInfo.enableFK, true);
      this.UpdateFKColor(OIBoneInfo.BoneGroup.Hair);
      this.ChangeEyesOpen(this.charFileStatus.eyesOpenMax);
      this.ChangeBlink(this.charFileStatus.eyesBlink);
      this.ChangeMouthOpen(this.oiCharInfo.mouthOpen);
    }

    public virtual void SetClothesStateAll(int _state)
    {
    }

    public virtual void SetClothesState(int _id, byte _state)
    {
      this.charInfo.SetClothesState(_id, _state, true);
    }

    public virtual void ShowAccessory(int _id, bool _flag)
    {
      this.charFileStatus.showAccessory[_id] = _flag;
    }

    public virtual void LoadClothesFile(string _path)
    {
      this.charInfo.ChangeNowCoordinate(_path, true, true);
      this.charInfo.AssignCoordinate();
    }

    public virtual void SetSiruFlags(ChaFileDefine.SiruParts _parts, byte _state)
    {
      this.oiCharInfo.siru[(int) _parts] = _state;
    }

    public virtual byte GetSiruFlags(ChaFileDefine.SiruParts _parts)
    {
      return 0;
    }

    public virtual void SetTuyaRate(float _value)
    {
      this.charInfo.skinGlossRate = _value;
    }

    public virtual void SetWetRate(float _value)
    {
      this.charInfo.wetRate = _value;
    }

    public virtual void SetNipStand(float _value)
    {
    }

    public virtual void SetVisibleSimple(bool _flag)
    {
      this.oiCharInfo.visibleSimple = _flag;
      this.charInfo.ChangeSimpleBodyDraw(_flag);
    }

    public bool GetVisibleSimple()
    {
      return this.oiCharInfo.visibleSimple;
    }

    public virtual void SetSimpleColor(Color _color)
    {
      this.oiCharInfo.simpleColor = _color;
      this.charInfo.ChangeSimpleBodyColor(_color);
    }

    public virtual void SetVisibleSon(bool _flag)
    {
      this.oiCharInfo.visibleSon = _flag;
      this.charFileStatus.visibleSonAlways = _flag;
    }

    public virtual float GetSonLength()
    {
      return this.oiCharInfo.sonLength;
    }

    public virtual void SetSonLength(float _value)
    {
      this.oiCharInfo.sonLength = _value;
    }

    public virtual void SetTears(float _state)
    {
      this.charInfo.ChangeTearsRate(_state);
    }

    public virtual float GetTears()
    {
      return this.charFileStatus.tearsRate;
    }

    public virtual void SetHohoAkaRate(float _value)
    {
      this.charInfo.ChangeHohoAkaRate(_value);
    }

    public virtual float GetHohoAkaRate()
    {
      return this.charInfo.fileStatus.hohoAkaRate;
    }

    public virtual void ChangeLookEyesPtn(int _ptn, bool _force = false)
    {
      int num = !_force ? this.charInfo.fileStatus.eyesLookPtn : -1;
      if (_ptn == 4 && num != 4)
      {
        this.charInfo.eyeLookCtrl.target = this.lookAtInfo.target;
        this.lookAtInfo.active = true;
      }
      else if (num == 4 && _ptn != 4)
      {
        this.charInfo.eyeLookCtrl.target = ((Component) Camera.get_main()).get_transform();
        this.lookAtInfo.active = false;
      }
      this.charInfo.ChangeLookEyesPtn(_ptn);
    }

    public virtual void ChangeLookNeckPtn(int _ptn)
    {
      this.charInfo.ChangeLookNeckPtn(_ptn, 1f);
    }

    public virtual void ChangeEyesOpen(float _value)
    {
      this.charInfo.ChangeEyesOpenMax(_value);
    }

    public virtual void ChangeBlink(bool _value)
    {
      this.charInfo.ChangeEyesBlinkFlag(_value);
    }

    public virtual void ChangeMouthOpen(float _value)
    {
      this.oiCharInfo.mouthOpen = _value;
      if (this.charInfo.mouthCtrl == null)
        return;
      this.charInfo.mouthCtrl.FixedRate = !this.voiceCtrl.isPlay || !this.oiCharInfo.lipSync ? _value : -1f;
    }

    public virtual void ChangeLipSync(bool _value)
    {
      this.oiCharInfo.lipSync = _value;
      this.charInfo.SetVoiceTransform(!_value ? (Transform) null : this.oiCharInfo.voiceCtrl.transVoice);
      this.ChangeMouthOpen(this.oiCharInfo.mouthOpen);
    }

    public virtual void SetVoice()
    {
      this.ChangeLipSync(this.oiCharInfo.lipSync);
    }

    public virtual void AddVoice(int _group, int _category, int _no)
    {
      this.voiceCtrl.list.Add(new VoiceCtrl.VoiceInfo(_group, _category, _no));
    }

    public virtual void DeleteVoice(int _index)
    {
      this.voiceCtrl.list.RemoveAt(_index);
      if (this.voiceCtrl.index != _index)
        return;
      this.voiceCtrl.index = -1;
      this.voiceCtrl.Stop();
    }

    public virtual void DeleteAllVoice()
    {
      this.voiceCtrl.list.Clear();
      this.voiceCtrl.Stop();
    }

    public virtual bool PlayVoice(int _index)
    {
      return this.voiceCtrl.Play(_index >= 0 ? _index : 0);
    }

    public virtual void StopVoice()
    {
      this.voiceCtrl.Stop();
    }

    public class SyncBoneInfo
    {
      private Transform _transform;

      public SyncBoneInfo(GameObject _gameObject)
      {
        this.GameObject = _gameObject;
      }

      public GameObject GameObject { get; private set; }

      private Transform Transform
      {
        get
        {
          return this._transform ?? (this._transform = this.GameObject.get_transform());
        }
      }

      public Quaternion Rotation
      {
        set
        {
          this.Transform.set_rotation(value);
        }
      }

      public Quaternion LocalRotation
      {
        set
        {
          this.Transform.set_localRotation(value);
        }
      }
    }

    public class BoneInfo
    {
      private GameObject m_GameObject;
      private OCIChar.SyncBoneInfo syncBoneInfo;

      public BoneInfo(GuideObject _guideObject, OIBoneInfo _boneInfo, int _boneID)
      {
        this.guideObject = _guideObject;
        this.boneInfo = _boneInfo;
        this.boneID = _boneID;
      }

      public GuideObject guideObject { get; private set; }

      public OIBoneInfo boneInfo { get; private set; }

      public GameObject gameObject
      {
        get
        {
          return this.m_GameObject ?? (this.m_GameObject = ((Component) this.guideObject).get_gameObject());
        }
      }

      public bool active
      {
        get
        {
          return Object.op_Inequality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
        }
        set
        {
          GameObject gameObject = this.gameObject;
          if (gameObject == null)
            return;
          gameObject.SetActiveIfDifferent(value);
        }
      }

      public OIBoneInfo.BoneGroup boneGroup
      {
        get
        {
          return this.boneInfo.group;
        }
      }

      public float scaleRate
      {
        get
        {
          return this.guideObject.scaleRate;
        }
        set
        {
          this.guideObject.scaleRate = value;
        }
      }

      public int layer
      {
        set
        {
          this.guideObject.SetLayer(this.gameObject, value);
        }
      }

      public Color color
      {
        set
        {
          this.guideObject.guideSelect.color = value;
        }
      }

      public int boneID { get; private set; }

      public Vector3 posision
      {
        get
        {
          return this.guideObject.transformTarget.get_position();
        }
      }

      public void AddSyncBone(GameObject _gameObject)
      {
        this.syncBoneInfo = new OCIChar.SyncBoneInfo(_gameObject);
        this.guideObject.changeAmount.onChangeRot += (Action) (() => this.syncBoneInfo.LocalRotation = this.gameObject.get_transform().get_localRotation());
      }
    }

    public class IKInfo
    {
      private GameObject m_GameObject;

      public IKInfo(
        GuideObject _guideObject,
        OIIKTargetInfo _targetInfo,
        Transform _base,
        Transform _target,
        Transform _bone)
      {
        this.guideObject = _guideObject;
        this.targetInfo = _targetInfo;
        this.baseObject = _base;
        this.targetObject = _target;
        this.boneObject = _bone;
      }

      public GuideObject guideObject { get; private set; }

      public OIIKTargetInfo targetInfo { get; private set; }

      public Transform baseObject { get; private set; }

      public Transform targetObject { get; private set; }

      public Transform boneObject { get; private set; }

      public GameObject gameObject
      {
        get
        {
          return this.m_GameObject ?? (this.m_GameObject = ((Component) this.guideObject).get_gameObject());
        }
      }

      public bool active
      {
        get
        {
          return Object.op_Inequality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
        }
        set
        {
          GameObject gameObject = this.gameObject;
          if (gameObject == null)
            return;
          gameObject.SetActiveIfDifferent(value);
        }
      }

      public OIBoneInfo.BoneGroup boneGroup
      {
        get
        {
          return this.targetInfo.group;
        }
      }

      public float scaleRate
      {
        get
        {
          return this.guideObject.scaleRate;
        }
        set
        {
          this.guideObject.scaleRate = value;
        }
      }

      public int layer
      {
        set
        {
          this.guideObject.SetLayer(this.gameObject, value);
        }
      }

      public void CopyBaseValue()
      {
        this.targetObject.set_position(this.baseObject.get_position());
        this.targetObject.set_eulerAngles(this.baseObject.get_eulerAngles());
        this.guideObject.changeAmount.pos = this.targetObject.get_localPosition();
        this.guideObject.changeAmount.rot = !this.guideObject.enableRot ? Vector3.get_zero() : this.targetObject.get_localEulerAngles();
      }

      public void CopyBone()
      {
        this.targetObject.set_position(this.boneObject.get_position());
        this.targetObject.set_eulerAngles(this.boneObject.get_eulerAngles());
        this.guideObject.changeAmount.pos = this.targetObject.get_localPosition();
        this.guideObject.changeAmount.rot = !this.guideObject.enableRot ? Vector3.get_zero() : this.targetObject.get_localEulerAngles();
      }

      public void CopyBoneRotation()
      {
        this.targetObject.set_eulerAngles(this.boneObject.get_eulerAngles());
        this.guideObject.changeAmount.rot = !this.guideObject.enableRot ? Vector3.get_zero() : this.targetObject.get_localEulerAngles();
      }
    }

    public class LookAtInfo
    {
      private GameObject m_GameObject;

      public LookAtInfo(GuideObject _guideObject, LookAtTargetInfo _targetInfo)
      {
        this.guideObject = _guideObject;
        this.targetInfo = _targetInfo;
      }

      public GuideObject guideObject { get; private set; }

      public LookAtTargetInfo targetInfo { get; private set; }

      public GameObject gameObject
      {
        get
        {
          return this.m_GameObject ?? (this.m_GameObject = ((Component) this.guideObject).get_gameObject());
        }
      }

      public Transform target
      {
        get
        {
          return this.guideObject.transformTarget;
        }
      }

      public bool active
      {
        get
        {
          return Object.op_Inequality((Object) this.gameObject, (Object) null) && this.gameObject.get_activeSelf();
        }
        set
        {
          GameObject gameObject = this.gameObject;
          if (gameObject == null)
            return;
          gameObject.SetActiveIfDifferent(value);
        }
      }

      public int layer
      {
        set
        {
          this.guideObject.SetLayer(this.gameObject, value);
        }
      }
    }

    public class LoadedAnimeInfo
    {
      public Info.FileInfo baseFile = new Info.FileInfo();
      public Info.FileInfo overrideFile = new Info.FileInfo();

      public bool BaseCheck(string _bundle, string _file)
      {
        return this.baseFile.bundlePath != _bundle | this.baseFile.fileName != _file;
      }

      public bool OverrideCheck(string _bundle, string _file)
      {
        return this.overrideFile.bundlePath != _bundle | this.overrideFile.fileName != _file;
      }
    }

    public class AccessPointInfo
    {
      public TreeNodeObject root;
      public Dictionary<int, TreeNodeObject> child;

      public AccessPointInfo(TreeNodeObject _root)
      {
        this.root = _root;
        this.child = new Dictionary<int, TreeNodeObject>();
      }
    }
  }
}
