// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsC_Clothes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsC_Clothes : CvsBase
  {
    private int backSNo = -1;
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscClothesType;
    [SerializeField]
    private Button btnColorAllReset;
    [Header("【設定02～04】-------------------")]
    [SerializeField]
    private CustomClothesColorSet[] ccsColorSet;
    [Header("【設定05】----------------------")]
    [SerializeField]
    private CustomSliderSet ssBreak;
    [SerializeField]
    private Toggle tglOption01;
    [SerializeField]
    private Toggle tglOption02;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    public void UpdateClothesList()
    {
      this.sscClothesType.CreateList(CvsBase.CreateSelectList(new ChaListDefine.CategoryNo[8]
      {
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_top : ChaListDefine.CategoryNo.mo_top,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_bot : ChaListDefine.CategoryNo.mo_bot,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_inner_t : ChaListDefine.CategoryNo.unknown,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_inner_b : ChaListDefine.CategoryNo.unknown,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_gloves : ChaListDefine.CategoryNo.mo_gloves,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_panst : ChaListDefine.CategoryNo.unknown,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_socks : ChaListDefine.CategoryNo.unknown,
        this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.fo_shoes : ChaListDefine.CategoryNo.mo_shoes
      }[this.SNo], ChaListDefine.KeyType.Unknown));
    }

    public void RestrictClothesMenu()
    {
      CmpClothes cmpClothe = this.chaCtrl.cmpClothes[this.SNo];
      if (Object.op_Equality((Object) null, (Object) cmpClothe))
      {
        this.ShowOrHideTab(false, 1, 2, 3, 4);
      }
      else
      {
        this.ShowOrHideTab(true, 1, 2, 3, 4);
        List<int> intList = new List<int>();
        if (!cmpClothe.useColorN01 && !cmpClothe.useColorA01)
          intList.Add(1);
        this.ccsColorSet[0].EnableColorAlpha(cmpClothe.useColorA01);
        if (!cmpClothe.useColorN02 && !cmpClothe.useColorA02)
          intList.Add(2);
        this.ccsColorSet[1].EnableColorAlpha(cmpClothe.useColorA02);
        if (!cmpClothe.useColorN03 && !cmpClothe.useColorA03)
          intList.Add(3);
        this.ccsColorSet[2].EnableColorAlpha(cmpClothe.useColorA03);
        this.ShowOrHideTab(false, intList.ToArray());
        bool active1 = false;
        bool active2 = false;
        bool active3 = false;
        if (Object.op_Implicit((Object) this.ssBreak))
        {
          active1 = cmpClothe.useBreak;
          ((Component) this.ssBreak).get_gameObject().SetActiveIfDifferent(active1);
        }
        if (Object.op_Implicit((Object) this.tglOption01))
        {
          active2 = cmpClothe.objOpt01 != null && 0 != cmpClothe.objOpt01.Length;
          ((Component) this.tglOption01).get_gameObject().SetActiveIfDifferent(active2);
        }
        if (Object.op_Implicit((Object) this.tglOption02))
        {
          active3 = cmpClothe.objOpt02 != null && 0 != cmpClothe.objOpt02.Length;
          ((Component) this.tglOption02).get_gameObject().SetActiveIfDifferent(active3);
        }
        this.ShowOrHideTab((active1 || active2 ? 1 : (active3 ? 1 : 0)) != 0, 4);
      }
    }

    private void CalculateUI()
    {
      this.ssBreak.SetSliderValue(this.nowClothes.parts[this.SNo].breakRate);
    }

    public override void UpdateCustomUI()
    {
      if (this.backSNo != this.SNo)
      {
        this.customBase.ChangeClothesStateAuto(new int[8]
        {
          0,
          0,
          1,
          1,
          1,
          1,
          1,
          0
        }[this.SNo]);
        this.UpdateClothesList();
        for (int _idx = 0; _idx < this.ccsColorSet.Length; ++_idx)
          this.ccsColorSet[_idx].Initialize(this.SNo, _idx);
        this.backSNo = this.SNo;
      }
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscClothesType.SetToggleID(this.nowClothes.parts[this.SNo].id);
      if (Object.op_Implicit((Object) this.tglOption01))
        this.tglOption01.SetIsOnWithoutCallback(!this.nowClothes.parts[this.SNo].hideOpt[0]);
      if (Object.op_Implicit((Object) this.tglOption02))
        this.tglOption02.SetIsOnWithoutCallback(!this.nowClothes.parts[this.SNo].hideOpt[1]);
      for (int index = 0; index < this.ccsColorSet.Length; ++index)
        this.ccsColorSet[index].UpdateCustomUI();
      this.customBase.RestrictSubMenu();
      this.RestrictClothesMenu();
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsC_Clothes.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsClothes += new Action(((CvsBase) this).UpdateCustomUI);
      this.customBase.RestrictSubMenu();
      this.UpdateClothesList();
      this.sscClothesType.SetToggleID(this.nowClothes.parts[this.SNo].id);
      this.sscClothesType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.nowClothes.parts[this.SNo].id == info.id)
          return;
        this.chaCtrl.ChangeClothes(this.SNo, info.id, false);
        this.orgClothes.parts[this.SNo].id = this.nowClothes.parts[this.SNo].id;
        for (int index = 0; index < 3; ++index)
        {
          this.orgClothes.parts[this.SNo].colorInfo[index].baseColor = this.nowClothes.parts[this.SNo].colorInfo[index].baseColor;
          this.ccsColorSet[index].UpdateCustomUI();
        }
        if (this.SNo == 0 || this.SNo == 2)
          this.customBase.RestrictSubMenu();
        this.RestrictClothesMenu();
      });
      if (Object.op_Implicit((Object) this.btnColorAllReset))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnColorAllReset), (Action<M0>) (_ =>
        {
          this.chaCtrl.SetClothesDefaultSetting(this.SNo);
          for (int index = 0; index < 3; ++index)
          {
            byte[] numArray = MessagePackSerializer.Serialize<ChaFileClothes.PartsInfo.ColorInfo>((M0) this.nowClothes.parts[this.SNo].colorInfo[index]);
            this.orgClothes.parts[this.SNo].colorInfo[index] = (ChaFileClothes.PartsInfo.ColorInfo) MessagePackSerializer.Deserialize<ChaFileClothes.PartsInfo.ColorInfo>(numArray);
          }
          this.chaCtrl.ChangeCustomClothes(this.SNo, true, true, true, true);
          for (int index = 0; index < this.ccsColorSet.Length; ++index)
            this.ccsColorSet[index].UpdateCustomUI();
        }));
      this.ccsColorSet[0].Initialize(this.SNo, 0);
      this.ccsColorSet[1].Initialize(this.SNo, 1);
      this.ccsColorSet[2].Initialize(this.SNo, 2);
      this.ssBreak.onChange = (Action<float>) (value =>
      {
        this.nowClothes.parts[this.SNo].breakRate = value;
        this.orgClothes.parts[this.SNo].breakRate = value;
        this.chaCtrl.ChangeBreakClothes(this.SNo);
      });
      this.ssBreak.onSetDefaultValue = (Func<float>) (() => 0.0f);
      if (Object.op_Implicit((Object) this.tglOption01))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglOption01), (Action<M0>) (isOn =>
        {
          this.nowClothes.parts[this.SNo].hideOpt[0] = !isOn;
          this.orgClothes.parts[this.SNo].hideOpt[0] = !isOn;
        }));
      if (Object.op_Implicit((Object) this.tglOption02))
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglOption02), (Action<M0>) (isOn =>
        {
          this.nowClothes.parts[this.SNo].hideOpt[1] = !isOn;
          this.orgClothes.parts[this.SNo].hideOpt[1] = !isOn;
        }));
      this.StartCoroutine(this.SetInputText());
      this.backSNo = this.SNo;
    }
  }
}
