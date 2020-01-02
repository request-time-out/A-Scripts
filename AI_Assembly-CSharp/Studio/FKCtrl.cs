// Decompiled with JetBrains decompiler
// Type: Studio.FKCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Studio
{
  public class FKCtrl : MonoBehaviour
  {
    public static OIBoneInfo.BoneGroup[] parts = new OIBoneInfo.BoneGroup[7]
    {
      OIBoneInfo.BoneGroup.Hair,
      OIBoneInfo.BoneGroup.Neck,
      OIBoneInfo.BoneGroup.Breast,
      OIBoneInfo.BoneGroup.Body,
      OIBoneInfo.BoneGroup.RightHand,
      OIBoneInfo.BoneGroup.LeftHand,
      OIBoneInfo.BoneGroup.Skirt
    };
    private Transform m_Transform;
    private List<FKCtrl.TargetInfo> listBones;

    public FKCtrl()
    {
      base.\u002Ector();
    }

    private Transform transform
    {
      get
      {
        if (Object.op_Equality((Object) this.m_Transform, (Object) null))
          this.m_Transform = ((Component) this).get_transform();
        return this.m_Transform;
      }
    }

    private int count { get; set; }

    public void InitBones(OICharInfo _info, ChaReference _charReference)
    {
      if (_info == null)
        return;
      this.listBones.Clear();
      Dictionary<int, FKCtrl.TargetInfo> dictionary = new Dictionary<int, FKCtrl.TargetInfo>();
      foreach (KeyValuePair<int, Info.BoneInfo> keyValuePair in Singleton<Info>.Instance.dicBoneInfo)
      {
        GameObject loop;
        switch (keyValuePair.Value.group)
        {
          case 7:
          case 8:
          case 9:
            loop = _charReference.GetReferenceInfo(ChaReference.RefObjKey.HeadParent).get_transform().FindLoop(keyValuePair.Value.bone);
            break;
          default:
            loop = this.transform.FindLoop(keyValuePair.Value.bone);
            if (Object.op_Equality((Object) loop, (Object) null))
            {
              Debug.LogWarning((object) keyValuePair.Value.bone);
              break;
            }
            break;
        }
        if (!Object.op_Equality((Object) loop, (Object) null))
        {
          FKCtrl.TargetInfo targetInfo1 = (FKCtrl.TargetInfo) null;
          if (dictionary.TryGetValue(keyValuePair.Value.sync, out targetInfo1))
          {
            targetInfo1.AddSyncBone(loop);
          }
          else
          {
            OIBoneInfo oiBoneInfo = (OIBoneInfo) null;
            if (_info.bones.TryGetValue(keyValuePair.Key, out oiBoneInfo))
            {
              OIBoneInfo.BoneGroup _group;
              switch (keyValuePair.Value.group)
              {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                  _group = OIBoneInfo.BoneGroup.Body;
                  break;
                case 7:
                case 8:
                case 9:
                  _group = OIBoneInfo.BoneGroup.Hair;
                  break;
                case 10:
                  _group = OIBoneInfo.BoneGroup.Neck;
                  break;
                case 11:
                case 12:
                  _group = OIBoneInfo.BoneGroup.Breast;
                  break;
                case 13:
                  _group = OIBoneInfo.BoneGroup.Skirt;
                  break;
                default:
                  _group = (OIBoneInfo.BoneGroup) (1 << keyValuePair.Value.group);
                  break;
              }
              FKCtrl.TargetInfo targetInfo2 = new FKCtrl.TargetInfo(loop, oiBoneInfo.changeAmount, _group, keyValuePair.Value.level);
              this.listBones.Add(targetInfo2);
              if (keyValuePair.Value.sync != -1)
                dictionary.Add(keyValuePair.Key, targetInfo2);
            }
          }
        }
      }
      this.count = this.listBones.Count;
    }

    public void CopyBone()
    {
      foreach (FKCtrl.TargetInfo listBone in this.listBones)
        listBone.CopyBone();
    }

    public void CopyBone(OIBoneInfo.BoneGroup _target)
    {
      foreach (FKCtrl.TargetInfo targetInfo in this.listBones.Where<FKCtrl.TargetInfo>((Func<FKCtrl.TargetInfo, bool>) (l => (l.group & _target) != (OIBoneInfo.BoneGroup) 0)))
        targetInfo.CopyBone();
    }

    public void SetEnable(OIBoneInfo.BoneGroup _group, bool _enable)
    {
      foreach (FKCtrl.TargetInfo targetInfo in this.listBones.Where<FKCtrl.TargetInfo>((Func<FKCtrl.TargetInfo, bool>) (l => (l.group & _group) != (OIBoneInfo.BoneGroup) 0)))
        targetInfo.enable = _enable;
    }

    private void LateUpdate()
    {
      for (int index = 0; index < this.count; ++index)
        this.listBones[index].Update();
    }

    private class TargetInfo
    {
      private BoolReactiveProperty _enable = new BoolReactiveProperty(true);
      public GameObject gameObject;
      private Transform m_Transform;
      public ChangeAmount changeAmount;
      private OCIChar.SyncBoneInfo syncBoneInfo;

      public TargetInfo(
        GameObject _gameObject,
        ChangeAmount _changeAmount,
        OIBoneInfo.BoneGroup _group,
        int _level)
      {
        this.gameObject = _gameObject;
        this.changeAmount = _changeAmount;
        this.group = _group;
        this.level = _level;
        if ((this.group & OIBoneInfo.BoneGroup.Hair) == (OIBoneInfo.BoneGroup) 0 && (this.group & OIBoneInfo.BoneGroup.Skirt) == (OIBoneInfo.BoneGroup) 0 && (this.group & OIBoneInfo.BoneGroup.Body) == (OIBoneInfo.BoneGroup) 0)
          return;
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) this._enable, (Action<M0>) (_b =>
        {
          if (_b)
            return;
          this.transform.set_localRotation(Quaternion.get_identity());
          this.syncBoneInfo.SafeProc<OCIChar.SyncBoneInfo>((Action<OCIChar.SyncBoneInfo>) (_sbi => _sbi.LocalRotation = Quaternion.get_identity()));
        }));
      }

      public Transform transform
      {
        get
        {
          if (Object.op_Equality((Object) this.m_Transform, (Object) null))
            this.m_Transform = this.gameObject.get_transform();
          return this.m_Transform;
        }
      }

      public OIBoneInfo.BoneGroup group { get; private set; }

      public int level { get; private set; }

      public bool enable
      {
        get
        {
          return ((ReactiveProperty<bool>) this._enable).get_Value();
        }
        set
        {
          ((ReactiveProperty<bool>) this._enable).set_Value(value);
        }
      }

      public void CopyBone()
      {
        this.changeAmount.rot = this.transform.get_localEulerAngles();
      }

      public void AddSyncBone(GameObject _gameObject)
      {
        this.syncBoneInfo = new OCIChar.SyncBoneInfo(_gameObject);
      }

      public void Update()
      {
        if (!this.enable)
          return;
        this.transform.set_localRotation(Quaternion.Euler(this.changeAmount.rot));
        this.syncBoneInfo.SafeProc<OCIChar.SyncBoneInfo>((Action<OCIChar.SyncBoneInfo>) (_sbi => _sbi.LocalRotation = this.transform.get_localRotation()));
      }
    }
  }
}
