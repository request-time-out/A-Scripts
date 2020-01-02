// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsA_Slot
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
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsA_Slot : CvsBase
  {
    private int backSNo = -1;
    [SerializeField]
    private CustomChangeMainMenu mainMenu;
    [Header("【設定01】----------------------")]
    [SerializeField]
    private Toggle[] tglType;
    [SerializeField]
    private GameObject objAcsSelect;
    [SerializeField]
    private CustomSelectScrollController sscAcs;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private GameObject[] objColorGrp;
    [SerializeField]
    private Text[] textColorTitle;
    [SerializeField]
    private CustomColorSet[] csColor;
    [SerializeField]
    private CustomSliderSet[] ssGloss;
    [SerializeField]
    private CustomSliderSet[] ssMetallic;
    [SerializeField]
    private Button btnDefaultColor;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomColorSet csHairBaseColor;
    [SerializeField]
    private CustomColorSet csHairTopColor;
    [SerializeField]
    private CustomColorSet csHairUnderColor;
    [SerializeField]
    private CustomColorSet csHairSpecular;
    [SerializeField]
    private CustomSliderSet ssHairMetallic;
    [SerializeField]
    private CustomSliderSet ssHairSmoothness;
    [SerializeField]
    private Button[] btnGetHairColor;
    [Header("【設定04】----------------------")]
    [SerializeField]
    private Toggle[] tglParent;
    [SerializeField]
    private Button btnDefaultParent;
    [Header("【設定05】----------------------")]
    [SerializeField]
    private Toggle tglNoShake;
    [SerializeField]
    private CustomAcsCorrectSet[] acCorrect;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    public void UpdateAcsList(int ForceNo = -1)
    {
      this.sscAcs.CreateList(CvsBase.CreateSelectList(new ChaListDefine.CategoryNo[14]
      {
        ChaListDefine.CategoryNo.ao_none,
        ChaListDefine.CategoryNo.ao_head,
        ChaListDefine.CategoryNo.ao_ear,
        ChaListDefine.CategoryNo.ao_glasses,
        ChaListDefine.CategoryNo.ao_face,
        ChaListDefine.CategoryNo.ao_neck,
        ChaListDefine.CategoryNo.ao_shoulder,
        ChaListDefine.CategoryNo.ao_chest,
        ChaListDefine.CategoryNo.ao_waist,
        ChaListDefine.CategoryNo.ao_back,
        ChaListDefine.CategoryNo.ao_arm,
        ChaListDefine.CategoryNo.ao_hand,
        ChaListDefine.CategoryNo.ao_leg,
        ChaListDefine.CategoryNo.ao_kokan
      }[ForceNo != -1 ? ForceNo : this.nowAcs.parts[this.SNo].type - 350], ChaListDefine.KeyType.Unknown));
    }

    public void RestrictAcsMenu()
    {
      if (this.nowAcs.parts[this.SNo].type == 350)
      {
        if (Object.op_Implicit((Object) this.objAcsSelect))
          this.objAcsSelect.SetActiveIfDifferent(false);
        this.ShowOrHideTab(false, 1, 2, 3, 4);
      }
      else
      {
        CmpAccessory cmpAccessory = this.chaCtrl.cmpAccessory[this.SNo];
        if (Object.op_Equality((Object) null, (Object) cmpAccessory))
          return;
        if (Object.op_Inequality((Object) null, (Object) this.objAcsSelect))
          this.objAcsSelect.SetActiveIfDifferent(true);
        this.ShowOrHideTab(true, 1, 2, 3, 4);
        if (cmpAccessory.typeHair)
        {
          this.ShowOrHideTab(false, 1);
        }
        else
        {
          if (!cmpAccessory.useColor01 && !cmpAccessory.useColor02 && !cmpAccessory.useColor03 && (cmpAccessory.rendAlpha == null || cmpAccessory.rendAlpha.Length == 0))
            this.ShowOrHideTab(false, 1);
          this.ShowOrHideTab(false, 2);
        }
        if (!cmpAccessory.typeHair)
        {
          int num1 = 1;
          if (Object.op_Inequality((Object) null, (Object) this.objColorGrp[0]))
            this.objColorGrp[0].SetActiveIfDifferent(cmpAccessory.useColor01);
          if (Object.op_Inequality((Object) null, (Object) this.objColorGrp[1]))
            this.objColorGrp[1].SetActiveIfDifferent(cmpAccessory.useColor02);
          if (Object.op_Inequality((Object) null, (Object) this.objColorGrp[2]))
            this.objColorGrp[2].SetActiveIfDifferent(cmpAccessory.useColor03);
          if (Object.op_Inequality((Object) null, (Object) this.objColorGrp[3]))
            this.objColorGrp[3].SetActiveIfDifferent(cmpAccessory.rendAlpha != null && 0 != cmpAccessory.rendAlpha.Length);
          if (Object.op_Inequality((Object) null, (Object) this.textColorTitle[0]) && cmpAccessory.useColor01)
            this.textColorTitle[0].set_text(string.Format("{0}{1}", (object) "カラー", (object) num1++));
          if (Object.op_Inequality((Object) null, (Object) this.textColorTitle[1]) && cmpAccessory.useColor02)
            this.textColorTitle[1].set_text(string.Format("{0}{1}", (object) "カラー", (object) num1++));
          if (Object.op_Inequality((Object) null, (Object) this.textColorTitle[2]) && cmpAccessory.useColor03)
            this.textColorTitle[2].set_text(string.Format("{0}{1}", (object) "カラー", (object) num1++));
          if (!Object.op_Inequality((Object) null, (Object) this.textColorTitle[3]) || cmpAccessory.rendAlpha == null || cmpAccessory.rendAlpha.Length == 0)
            return;
          Text text = this.textColorTitle[3];
          int num2 = num1;
          int num3 = num2 + 1;
          string str = string.Format("{0}{1}", (object) "カラー", (object) num2);
          text.set_text(str);
        }
        else
        {
          if (Object.op_Inequality((Object) null, (Object) this.csHairTopColor))
            ((Component) this.csHairTopColor).get_gameObject().SetActiveIfDifferent(cmpAccessory.useColor02);
          if (!Object.op_Inequality((Object) null, (Object) this.csHairUnderColor))
            return;
          ((Component) this.csHairUnderColor).get_gameObject().SetActiveIfDifferent(cmpAccessory.useColor03);
        }
      }
    }

    public void SetDefaultColor()
    {
      CmpAccessory cmpAccessory = this.chaCtrl.cmpAccessory[this.SNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory) || cmpAccessory.typeHair)
        return;
      this.chaCtrl.SetAccessoryDefaultColor(this.SNo);
      for (int index = 0; index < 4; ++index)
      {
        byte[] numArray = MessagePackSerializer.Serialize<ChaFileAccessory.PartsInfo.ColorInfo>((M0) this.nowAcs.parts[this.SNo].colorInfo[index]);
        this.orgAcs.parts[this.SNo].colorInfo[index] = (ChaFileAccessory.PartsInfo.ColorInfo) MessagePackSerializer.Deserialize<ChaFileAccessory.PartsInfo.ColorInfo>(numArray);
      }
    }

    public void ChangeAcsType(int idx)
    {
      if (this.nowAcs.parts[this.SNo].type - 350 == idx)
        return;
      this.nowAcs.parts[this.SNo].type = 350 + idx;
      this.orgAcs.parts[this.SNo].type = this.nowAcs.parts[this.SNo].type;
      this.nowAcs.parts[this.SNo].parentKey = string.Empty;
      for (int index = 0; index < 2; ++index)
      {
        this.orgAcs.parts[this.SNo].addMove[index, 0] = this.nowAcs.parts[this.SNo].addMove[index, 0] = Vector3.get_zero();
        this.orgAcs.parts[this.SNo].addMove[index, 1] = this.nowAcs.parts[this.SNo].addMove[index, 1] = Vector3.get_zero();
        this.orgAcs.parts[this.SNo].addMove[index, 2] = this.nowAcs.parts[this.SNo].addMove[index, 2] = Vector3.get_one();
      }
      this.chaCtrl.ChangeAccessory(this.SNo, this.nowAcs.parts[this.SNo].type, ChaAccessoryDefine.AccessoryDefaultIndex[idx], string.Empty, true);
      this.SetDefaultColor();
      this.chaCtrl.ChangeAccessoryColor(this.SNo);
      this.orgAcs.parts[this.SNo].id = this.nowAcs.parts[this.SNo].id;
      this.orgAcs.parts[this.SNo].parentKey = this.nowAcs.parts[this.SNo].parentKey;
      this.nowAcs.parts[this.SNo].noShake = false;
      this.orgAcs.parts[this.SNo].noShake = false;
      this.customBase.ChangeAcsSlotName(this.SNo);
      this.UpdateAcsList(-1);
      this.customBase.forceUpdateAcsList = true;
      this.UpdateCustomUI();
      this.customBase.showAcsControllerAll = this.customBase.chaCtrl.IsAccessory(this.SNo);
    }

    public void ChangeAcsId(int id)
    {
      bool flag1 = false;
      if (this.chaCtrl.cmpAccessory != null && Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo]))
        flag1 = this.chaCtrl.cmpAccessory[this.SNo].typeHair;
      this.chaCtrl.ChangeAccessory(this.SNo, this.nowAcs.parts[this.SNo].type, id, string.Empty, false);
      this.SetDefaultColor();
      this.chaCtrl.ChangeAccessoryColor(this.SNo);
      bool flag2 = false;
      if (this.chaCtrl.cmpAccessory != null && Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo]))
        flag2 = this.chaCtrl.cmpAccessory[this.SNo].typeHair;
      if (!flag1 && flag2)
        this.ChangeHairTypeAccessoryColor(0);
      this.orgAcs.parts[this.SNo].id = this.nowAcs.parts[this.SNo].id;
      this.orgAcs.parts[this.SNo].parentKey = this.nowAcs.parts[this.SNo].parentKey;
      this.nowAcs.parts[this.SNo].noShake = false;
      this.orgAcs.parts[this.SNo].noShake = false;
      this.customBase.ChangeAcsSlotName(this.SNo);
      this.UpdateCustomUI();
    }

    public void ChangeAcsParent(int idx)
    {
      string parentStr = ((IEnumerable<string>) Enum.GetNames(typeof (ChaAccessoryDefine.AccessoryParentKey))).Where<string>((Func<string, bool>) (key => key != "none")).ToArray<string>()[idx];
      if (!(this.nowAcs.parts[this.SNo].parentKey != parentStr))
        return;
      this.chaCtrl.ChangeAccessoryParent(this.SNo, parentStr);
      this.orgAcs.parts[this.SNo].parentKey = this.nowAcs.parts[this.SNo].parentKey;
    }

    private void CalculateUI()
    {
      CmpAccessory cmpAccessory = this.chaCtrl.cmpAccessory[this.SNo];
      if (Object.op_Equality((Object) null, (Object) cmpAccessory))
        return;
      if (!cmpAccessory.typeHair)
      {
        for (int index = 0; index < this.ssGloss.Length; ++index)
          this.ssGloss[index].SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[index].glossPower);
        for (int index = 0; index < this.ssMetallic.Length; ++index)
          this.ssMetallic[index].SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[index].metallicPower);
      }
      else
      {
        this.ssHairSmoothness.SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[0].smoothnessPower);
        this.ssHairMetallic.SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[0].metallicPower);
      }
    }

    public override void UpdateCustomUI()
    {
      if (this.backSNo != this.SNo)
      {
        this.UpdateAcsList(-1);
        for (int _correctNo = 0; _correctNo < this.acCorrect.Length; ++_correctNo)
          this.acCorrect[_correctNo].Initialize(this.SNo, _correctNo);
        this.backSNo = this.SNo;
      }
      else if (this.customBase.forceUpdateAcsList)
      {
        this.UpdateAcsList(-1);
        this.customBase.forceUpdateAcsList = false;
      }
      this.customBase.showAcsControllerAll = this.customBase.chaCtrl.IsAccessory(this.SNo);
      if (!this.mainMenu.IsSelectAccessory())
        this.customBase.showAcsControllerAll = false;
      base.UpdateCustomUI();
      this.CalculateUI();
      int num1 = this.nowAcs.parts[this.SNo].type - 350;
      for (int index = 0; index < this.tglType.Length; ++index)
        this.tglType[index].SetIsOnWithoutCallback(num1 == index);
      CmpAccessory cmpAccessory = this.chaCtrl.cmpAccessory[this.SNo];
      bool flag = false;
      if (Object.op_Inequality((Object) null, (Object) cmpAccessory))
        flag = cmpAccessory.typeHair;
      if (flag)
      {
        this.csHairBaseColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[0].color);
        this.csHairTopColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[1].color);
        this.csHairUnderColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[2].color);
        this.csHairSpecular.SetColor(this.nowAcs.parts[this.SNo].colorInfo[3].color);
      }
      else
      {
        for (int index = 0; index < this.csColor.Length; ++index)
          this.csColor[index].SetColor(this.nowAcs.parts[this.SNo].colorInfo[index].color);
      }
      this.sscAcs.SetToggleID(this.nowAcs.parts[this.SNo].id);
      int num2 = ChaAccessoryDefine.GetAccessoryParentInt(this.nowAcs.parts[this.SNo].parentKey) - 1;
      if (0 <= num2)
      {
        for (int index = 0; index < this.tglParent.Length; ++index)
          this.tglParent[index].SetIsOnWithoutCallback(num2 == index);
      }
      this.tglNoShake.SetIsOnWithoutCallback(this.nowAcs.parts[this.SNo].noShake);
      bool[] flagArray = new bool[2];
      if (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo]))
      {
        if (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo].trfMove01))
          flagArray[0] = true;
        if (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo].trfMove02))
          flagArray[1] = true;
      }
      for (int index = 0; index < this.acCorrect.Length; ++index)
      {
        ((Component) this.acCorrect[index]).get_gameObject().SetActiveIfDifferent(flagArray[index]);
        this.acCorrect[index].UpdateCustomUI();
        this.customBase.showAcsController[index] = flagArray[index];
      }
      this.RestrictAcsMenu();
      if (!Object.op_Inequality((Object) null, (Object) this.titleText))
        return;
      this.titleText.set_text(string.Format("{0:00} {1}", (object) (this.SNo + 1), (object) this.chaCtrl.lstCtrl.GetListInfo((ChaListDefine.CategoryNo) this.nowAcs.parts[this.SNo].type, this.nowAcs.parts[this.SNo].id).Name));
    }

    public void ChangeHairTypeAccessoryColor(int hairPartsNo)
    {
      this.nowAcs.parts[this.SNo].colorInfo[0].color = this.hair.parts[hairPartsNo].baseColor;
      this.nowAcs.parts[this.SNo].colorInfo[1].color = this.hair.parts[hairPartsNo].topColor;
      this.nowAcs.parts[this.SNo].colorInfo[2].color = this.hair.parts[hairPartsNo].underColor;
      this.nowAcs.parts[this.SNo].colorInfo[3].color = this.hair.parts[hairPartsNo].specular;
      this.nowAcs.parts[this.SNo].colorInfo[0].smoothnessPower = this.hair.parts[hairPartsNo].smoothness;
      this.nowAcs.parts[this.SNo].colorInfo[0].metallicPower = this.hair.parts[hairPartsNo].metallic;
      this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
      for (int index = 0; index < 4; ++index)
      {
        byte[] numArray = MessagePackSerializer.Serialize<ChaFileAccessory.PartsInfo.ColorInfo>((M0) this.nowAcs.parts[this.SNo].colorInfo[index]);
        this.orgAcs.parts[this.SNo].colorInfo[index] = (ChaFileAccessory.PartsInfo.ColorInfo) MessagePackSerializer.Deserialize<ChaFileAccessory.PartsInfo.ColorInfo>(numArray);
      }
      this.csHairBaseColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[0].color);
      this.csHairTopColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[1].color);
      this.csHairUnderColor.SetColor(this.nowAcs.parts[this.SNo].colorInfo[2].color);
      this.csHairSpecular.SetColor(this.nowAcs.parts[this.SNo].colorInfo[3].color);
      this.ssHairSmoothness.SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[0].smoothnessPower);
      this.ssHairMetallic.SetSliderValue(this.nowAcs.parts[this.SNo].colorInfo[0].metallicPower);
    }

    public void ShortcutChangeGuidType(int type)
    {
      bool flag = false;
      foreach (CustomAcsCorrectSet customAcsCorrectSet in this.acCorrect)
      {
        if (customAcsCorrectSet.IsDrag())
          flag = true;
      }
      if (flag)
        return;
      foreach (CustomAcsCorrectSet customAcsCorrectSet in this.acCorrect)
        customAcsCorrectSet.ShortcutChangeGuidType(type);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsA_Slot.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.ChangeAcsSlotName(-1);
      this.customBase.actUpdateCvsAccessory += new Action(((CvsBase) this).UpdateCustomUI);
      if (((IEnumerable<Toggle>) this.tglType).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglType).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (tgl => Object.op_Inequality((Object) tgl.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (tgl => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) tgl.val.onValueChanged), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn => this.ChangeAcsType(tgl.idx)))));
      }
      this.UpdateAcsList(-1);
      this.sscAcs.SetToggleID(this.nowAcs.parts[this.SNo].id);
      this.sscAcs.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.nowAcs.parts[this.SNo].id == info.id)
          return;
        this.ChangeAcsId(info.id);
      });
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CustomColorSet>) this.csColor).Select<CustomColorSet, \u003C\u003E__AnonType15<CustomColorSet, int>>((Func<CustomColorSet, int, \u003C\u003E__AnonType15<CustomColorSet, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CustomColorSet, int>(val, idx))).Where<\u003C\u003E__AnonType15<CustomColorSet, int>>((Func<\u003C\u003E__AnonType15<CustomColorSet, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<CustomColorSet, int>>().ForEach((Action<\u003C\u003E__AnonType15<CustomColorSet, int>>) (item => item.val.actUpdateColor = (Action<Color>) (color =>
      {
        this.nowAcs.parts[this.SNo].colorInfo[item.idx].color = color;
        this.orgAcs.parts[this.SNo].colorInfo[item.idx].color = color;
        this.chaCtrl.ChangeAccessoryColor(this.SNo);
      })));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CustomSliderSet>) this.ssGloss).Select<CustomSliderSet, \u003C\u003E__AnonType15<CustomSliderSet, int>>((Func<CustomSliderSet, int, \u003C\u003E__AnonType15<CustomSliderSet, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CustomSliderSet, int>(val, idx))).Where<\u003C\u003E__AnonType15<CustomSliderSet, int>>((Func<\u003C\u003E__AnonType15<CustomSliderSet, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<CustomSliderSet, int>>().ForEach((Action<\u003C\u003E__AnonType15<CustomSliderSet, int>>) (item =>
      {
        item.val.onChange = (Action<float>) (value =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[item.idx].glossPower = value;
          this.orgAcs.parts[this.SNo].colorInfo[item.idx].glossPower = value;
          this.chaCtrl.ChangeAccessoryColor(this.SNo);
        });
        item.val.onSetDefaultValue = (Func<float>) (() =>
        {
          if (Object.op_Equality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo]))
            return 0.0f;
          switch (item.idx)
          {
            case 0:
              return this.chaCtrl.cmpAccessory[this.SNo].defGlossPower01;
            case 1:
              return this.chaCtrl.cmpAccessory[this.SNo].defGlossPower02;
            case 2:
              return this.chaCtrl.cmpAccessory[this.SNo].defGlossPower03;
            case 3:
              return this.chaCtrl.cmpAccessory[this.SNo].defGlossPower04;
            default:
              return 0.0f;
          }
        });
      }));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<CustomSliderSet>) this.ssMetallic).Select<CustomSliderSet, \u003C\u003E__AnonType15<CustomSliderSet, int>>((Func<CustomSliderSet, int, \u003C\u003E__AnonType15<CustomSliderSet, int>>) ((val, idx) => new \u003C\u003E__AnonType15<CustomSliderSet, int>(val, idx))).Where<\u003C\u003E__AnonType15<CustomSliderSet, int>>((Func<\u003C\u003E__AnonType15<CustomSliderSet, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<CustomSliderSet, int>>().ForEach((Action<\u003C\u003E__AnonType15<CustomSliderSet, int>>) (item =>
      {
        item.val.onChange = (Action<float>) (value =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[item.idx].metallicPower = value;
          this.orgAcs.parts[this.SNo].colorInfo[item.idx].metallicPower = value;
          this.chaCtrl.ChangeAccessoryColor(this.SNo);
        });
        item.val.onSetDefaultValue = (Func<float>) (() =>
        {
          if (Object.op_Equality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.SNo]))
            return 0.0f;
          switch (item.idx)
          {
            case 0:
              return this.chaCtrl.cmpAccessory[this.SNo].defMetallicPower01;
            case 1:
              return this.chaCtrl.cmpAccessory[this.SNo].defMetallicPower02;
            case 2:
              return this.chaCtrl.cmpAccessory[this.SNo].defMetallicPower03;
            case 3:
              return this.chaCtrl.cmpAccessory[this.SNo].defMetallicPower04;
            default:
              return 0.0f;
          }
        });
      }));
      if (Object.op_Inequality((Object) null, (Object) this.btnDefaultColor))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnDefaultColor), (Action<M0>) (_ =>
        {
          this.SetDefaultColor();
          this.chaCtrl.ChangeAccessoryColor(this.SNo);
          this.UpdateCustomUI();
        }));
      if (Object.op_Implicit((Object) this.csHairBaseColor))
        this.csHairBaseColor.actUpdateColor = (Action<Color>) (color =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[0].color = color;
          this.orgAcs.parts[this.SNo].colorInfo[0].color = color;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      if (Object.op_Implicit((Object) this.csHairTopColor))
        this.csHairTopColor.actUpdateColor = (Action<Color>) (color =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[1].color = color;
          this.orgAcs.parts[this.SNo].colorInfo[1].color = color;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      if (Object.op_Implicit((Object) this.csHairUnderColor))
        this.csHairUnderColor.actUpdateColor = (Action<Color>) (color =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[2].color = color;
          this.orgAcs.parts[this.SNo].colorInfo[2].color = color;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      if (Object.op_Implicit((Object) this.csHairSpecular))
        this.csHairSpecular.actUpdateColor = (Action<Color>) (color =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[3].color = color;
          this.orgAcs.parts[this.SNo].colorInfo[3].color = color;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      if (Object.op_Implicit((Object) this.ssHairMetallic))
        this.ssHairMetallic.onChange = (Action<float>) (value =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[0].metallicPower = value;
          this.orgAcs.parts[this.SNo].colorInfo[0].metallicPower = value;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      if (Object.op_Implicit((Object) this.ssHairSmoothness))
        this.ssHairSmoothness.onChange = (Action<float>) (value =>
        {
          this.nowAcs.parts[this.SNo].colorInfo[0].smoothnessPower = value;
          this.orgAcs.parts[this.SNo].colorInfo[0].smoothnessPower = value;
          this.chaCtrl.ChangeHairTypeAccessoryColor(this.SNo);
        });
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<Button>) this.btnGetHairColor).Select<Button, \u003C\u003E__AnonType15<Button, int>>((Func<Button, int, \u003C\u003E__AnonType15<Button, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Button, int>(val, idx))).Where<\u003C\u003E__AnonType15<Button, int>>((Func<\u003C\u003E__AnonType15<Button, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Button, int>>().ForEach((Action<\u003C\u003E__AnonType15<Button, int>>) (item => ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(item.val), (Action<M0>) (_ => this.ChangeHairTypeAccessoryColor(item.idx)))));
      if (((IEnumerable<Toggle>) this.tglParent).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglParent).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (tgl => Object.op_Inequality((Object) tgl.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (tgl => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) tgl.val.onValueChanged), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn => this.ChangeAcsParent(tgl.idx)))));
      }
      if (Object.op_Inequality((Object) null, (Object) this.btnDefaultParent))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnDefaultParent), (Action<M0>) (_ =>
        {
          this.chaCtrl.ChangeAccessoryParent(this.SNo, this.chaCtrl.GetAccessoryDefaultParentStr(this.SNo));
          this.orgAcs.parts[this.SNo].parentKey = this.nowAcs.parts[this.SNo].parentKey;
          this.UpdateCustomUI();
        }));
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglNoShake.onValueChanged), (Action<M0>) (isOn =>
      {
        this.nowAcs.parts[this.SNo].noShake = isOn;
        this.orgAcs.parts[this.SNo].noShake = isOn;
      }));
      GameObject[] gameObjectArray = new GameObject[2]
      {
        this.customBase.objAcs01ControllerTop,
        this.customBase.objAcs02ControllerTop
      };
      for (int _correctNo = 0; _correctNo < this.acCorrect.Length; ++_correctNo)
      {
        this.acCorrect[_correctNo].CreateGuid(gameObjectArray[_correctNo]);
        this.acCorrect[_correctNo].Initialize(this.SNo, _correctNo);
      }
      this.StartCoroutine(this.SetInputText());
      this.backSNo = this.SNo;
    }
  }
}
