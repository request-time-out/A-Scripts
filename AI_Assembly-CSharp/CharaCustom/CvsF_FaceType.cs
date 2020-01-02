// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_FaceType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_FaceType : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscFaceType;
    [SerializeField]
    private CustomPushScrollController pscFacePreset;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscSkinType;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomSliderSet ssDetailPower;
    [SerializeField]
    private CustomSelectScrollController sscDetailType;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssDetailPower.SetSliderValue(this.face.detailPower);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.UpdateSkinList();
      this.sscFaceType.SetToggleID(this.face.headId);
      this.sscSkinType.SetToggleID(this.face.skinId);
      this.sscDetailType.SetToggleID(this.face.detailId);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_FaceType.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void UpdateSkinList()
    {
      this.sscSkinType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_f : ChaListDefine.CategoryNo.mt_skin_f, ChaListDefine.KeyType.HeadID).Where<CustomSelectInfo>((Func<CustomSelectInfo, bool>) (x => x.limitIndex == this.face.headId)).ToList<CustomSelectInfo>());
    }

    public List<CustomPushInfo> CreateFacePresetList(
      ChaListDefine.CategoryNo cateNo)
    {
      Dictionary<int, ListInfoBase> categoryInfo = Singleton<Character>.Instance.chaListCtrl.GetCategoryInfo(cateNo);
      int[] array = categoryInfo.Keys.ToArray<int>();
      List<CustomPushInfo> customPushInfoList = new List<CustomPushInfo>();
      for (int index = 0; index < categoryInfo.Count; ++index)
        customPushInfoList.Add(new CustomPushInfo()
        {
          category = categoryInfo[array[index]].Category,
          id = categoryInfo[array[index]].Id,
          name = categoryInfo[array[index]].Name,
          assetBundle = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.ThumbAB),
          assetName = categoryInfo[array[index]].GetInfo(ChaListDefine.KeyType.Preset)
        });
      return customPushInfoList;
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFaceType += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscFaceType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_head : ChaListDefine.CategoryNo.mo_head, ChaListDefine.KeyType.Unknown));
      this.sscFaceType.SetToggleID(this.face.headId);
      this.sscFaceType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.headId == info.id)
          return;
        this.chaCtrl.ChangeHead(info.id, false);
        this.UpdateSkinList();
        this.sscSkinType.SetToggleID(this.face.skinId);
      });
      this.pscFacePreset.CreateList(this.CreateFacePresetList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_head : ChaListDefine.CategoryNo.mo_head));
      this.pscFacePreset.onPush = (Action<CustomPushInfo>) (info =>
      {
        if (info == null)
          return;
        this.face.headId = info.id;
        this.chaCtrl.chaFile.LoadFacePreset();
        Singleton<Character>.Instance.customLoadGCClear = false;
        this.chaCtrl.Reload(true, false, true, true, true);
        Singleton<Character>.Instance.customLoadGCClear = true;
        this.customBase.updateCvsFaceType = true;
        this.customBase.updateCvsFaceShapeWhole = true;
        this.customBase.updateCvsFaceShapeChin = true;
        this.customBase.updateCvsFaceShapeCheek = true;
        this.customBase.updateCvsFaceShapeEyebrow = true;
        this.customBase.updateCvsFaceShapeEyes = true;
        this.customBase.updateCvsFaceShapeNose = true;
        this.customBase.updateCvsFaceShapeMouth = true;
        this.customBase.updateCvsFaceShapeEar = true;
        this.customBase.updateCvsMole = true;
        this.customBase.updateCvsEyeLR = true;
        this.customBase.updateCvsEyeEtc = true;
        this.customBase.updateCvsEyeHL = true;
        this.customBase.updateCvsEyebrow = true;
        this.customBase.updateCvsEyelashes = true;
        this.customBase.updateCvsEyeshadow = true;
        this.customBase.updateCvsCheek = true;
        this.customBase.updateCvsLip = true;
        this.customBase.updateCvsFacePaint = true;
        this.customBase.SetUpdateToggleSetting();
      });
      this.UpdateSkinList();
      this.sscSkinType.SetToggleID(this.face.skinId);
      this.sscSkinType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.skinId == info.id)
          return;
        this.face.skinId = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(true, false, false, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.sscDetailType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_f : ChaListDefine.CategoryNo.mt_detail_f, ChaListDefine.KeyType.Unknown));
      this.sscDetailType.SetToggleID(this.face.detailId);
      this.sscDetailType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.detailId == info.id)
          return;
        this.face.detailId = info.id;
        this.chaCtrl.ChangeFaceDetailKind();
      });
      this.ssDetailPower.onChange = (Action<float>) (value =>
      {
        this.face.detailPower = value;
        this.chaCtrl.ChangeFaceDetailPower();
      });
      this.ssDetailPower.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.detailPower);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
