// Decompiled with JetBrains decompiler
// Type: Studio.ItemFKCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Studio
{
  public class ItemFKCtrl : MonoBehaviour
  {
    private List<ItemFKCtrl.TargetInfo> listBones;

    public ItemFKCtrl()
    {
      base.\u002Ector();
    }

    private int count { get; set; }

    public void InitBone(OCIItem _ociItem, Info.ItemLoadInfo _loadInfo, bool _isNew)
    {
      Transform transform = _ociItem.objectItem.get_transform();
      _ociItem.listBones = new List<OCIChar.BoneInfo>();
      foreach (string bone in _loadInfo.bones)
      {
        GameObject loop = transform.FindLoop(bone);
        if (Object.op_Equality((Object) loop, (Object) null))
        {
          Debug.LogWarning((object) string.Format("無い : {0}", (object) bone));
        }
        else
        {
          OIBoneInfo _boneInfo = (OIBoneInfo) null;
          if (!_ociItem.itemInfo.bones.TryGetValue(bone, out _boneInfo))
          {
            _boneInfo = new OIBoneInfo(Studio.Studio.GetNewIndex());
            _ociItem.itemInfo.bones.Add(bone, _boneInfo);
          }
          GuideObject _guideObject = Singleton<GuideObjectManager>.Instance.Add(loop.get_transform(), _boneInfo.dicKey);
          _guideObject.enablePos = false;
          _guideObject.enableScale = false;
          _guideObject.enableMaluti = false;
          _guideObject.calcScale = false;
          _guideObject.scaleRate = 0.5f;
          _guideObject.scaleRot = 0.025f;
          _guideObject.scaleSelect = 0.05f;
          _guideObject.parentGuide = _ociItem.guideObject;
          _ociItem.listBones.Add(new OCIChar.BoneInfo(_guideObject, _boneInfo, -1));
          _guideObject.SetActive(false, true);
          this.listBones.Add(new ItemFKCtrl.TargetInfo(loop, _boneInfo.changeAmount, _isNew));
        }
      }
      this.count = this.listBones.Count;
    }

    private void OnDisable()
    {
      for (int index = 0; index < this.count; ++index)
        this.listBones[index].CopyBase();
    }

    private void LateUpdate()
    {
      for (int index = 0; index < this.count; ++index)
        this.listBones[index].Update();
    }

    private class TargetInfo
    {
      private Vector3 baseRot = Vector3.get_zero();
      public GameObject gameObject;
      private Transform m_Transform;
      public ChangeAmount changeAmount;

      public TargetInfo(GameObject _gameObject, ChangeAmount _changeAmount, bool _new)
      {
        this.gameObject = _gameObject;
        this.changeAmount = _changeAmount;
        if (_new)
          this.CopyBone();
        this.changeAmount.defRot = this.transform.get_localEulerAngles();
        this.baseRot = this.transform.get_localEulerAngles();
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

      public void CopyBone()
      {
        this.changeAmount.rot = this.transform.get_localEulerAngles();
      }

      public void CopyBase()
      {
        this.transform.set_localEulerAngles(this.baseRot);
      }

      public void Update()
      {
        this.transform.set_localRotation(Quaternion.Euler(this.changeAmount.rot));
      }
    }
  }
}
