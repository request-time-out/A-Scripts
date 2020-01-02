// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Skin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_Skin : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscSkinType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomSliderSet ssDetailPower;
    [SerializeField]
    private CustomSelectScrollController sscDetailType;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomColorSet csSkinColor;
    [SerializeField]
    private CustomSliderSet ssSkinGloss;
    [SerializeField]
    private CustomSliderSet ssSkinMetallic;
    [SerializeField]
    private CustomSkinColorPreset hcPreset;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssDetailPower.SetSliderValue(this.body.detailPower);
      this.ssSkinGloss.SetSliderValue(this.body.skinGlossPower);
      this.ssSkinMetallic.SetSliderValue(this.body.skinMetallicPower);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscSkinType.SetToggleID(this.body.skinId);
      this.sscDetailType.SetToggleID(this.body.detailId);
      this.csSkinColor.SetColor(this.body.skinColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Skin.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBodySkinType += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscSkinType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_skin_b : ChaListDefine.CategoryNo.mt_skin_b, ChaListDefine.KeyType.Unknown));
      this.sscSkinType.SetToggleID(this.body.skinId);
      this.sscSkinType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.skinId == info.id)
          return;
        this.body.skinId = info.id;
        this.chaCtrl.AddUpdateCMBodyTexFlags(true, false, false, false);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssDetailPower.onChange = (Action<float>) (value =>
      {
        this.body.detailPower = value;
        this.chaCtrl.ChangeBodyDetailPower();
      });
      this.ssDetailPower.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.detailPower);
      this.sscDetailType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_detail_b : ChaListDefine.CategoryNo.mt_detail_b, ChaListDefine.KeyType.Unknown));
      this.sscDetailType.SetToggleID(this.body.detailId);
      this.sscDetailType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.detailId == info.id)
          return;
        this.body.detailId = info.id;
        this.chaCtrl.AddUpdateCMBodyTexFlags(true, false, false, false);
        this.chaCtrl.CreateBodyTexture();
      });
      this.csSkinColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.skinColor = color;
        this.chaCtrl.AddUpdateCMBodyColorFlags(true, false, false, false);
        this.chaCtrl.CreateBodyTexture();
        this.chaCtrl.AddUpdateCMFaceColorFlags(true, false, false, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssSkinGloss.onChange = (Action<float>) (value =>
      {
        this.body.skinGlossPower = value;
        this.chaCtrl.ChangeBodyGlossPower();
        this.chaCtrl.ChangeFaceGlossPower();
      });
      this.ssSkinGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.skinGlossPower);
      this.ssSkinMetallic.onChange = (Action<float>) (value =>
      {
        this.body.skinMetallicPower = value;
        this.chaCtrl.ChangeBodyMetallicPower();
        this.chaCtrl.ChangeFaceMetallicPower();
      });
      this.ssSkinMetallic.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.skinMetallicPower);
      this.hcPreset.onClick = (Action<Color>) (color =>
      {
        this.body.skinColor = color;
        this.chaCtrl.AddUpdateCMBodyColorFlags(true, false, false, false);
        this.chaCtrl.CreateBodyTexture();
        this.chaCtrl.AddUpdateCMFaceColorFlags(true, false, false, false, false, false, false);
        this.chaCtrl.CreateFaceTexture();
        this.csSkinColor.SetColor(color);
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
