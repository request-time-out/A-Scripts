// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsH_Hair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsH_Hair : CvsBase
  {
    private List<CustomHairBundleSet> lstHairBundleSet = new List<CustomHairBundleSet>();
    private int backSNo = -1;
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscHairType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csBaseColor;
    [SerializeField]
    private CustomColorSet csTopColor;
    [SerializeField]
    private CustomColorSet csUnderColor;
    [SerializeField]
    private CustomColorSet csSpecular;
    [SerializeField]
    private CustomSliderSet ssMetallic;
    [SerializeField]
    private CustomSliderSet ssSmoothness;
    [SerializeField]
    private CustomHairColorPreset hcPreset;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomColorSet[] csAcsColor;
    [Header("【設定04】----------------------")]
    [SerializeField]
    private Transform trfContent;
    [SerializeField]
    private GameObject tmpBundleObj;
    [SerializeField]
    private Button btnCorrectAllReset;
    [SerializeField]
    private Toggle tglGuidDraw;
    [SerializeField]
    private Toggle[] tglGuidType;
    [SerializeField]
    private Slider sldGuidSpeed;
    [SerializeField]
    private Slider sldGuidScale;

    public bool allReset { get; set; }

    private bool sameSetting
    {
      get
      {
        return this.hair.sameSetting;
      }
    }

    private bool autoSetting
    {
      get
      {
        return this.hair.autoSetting;
      }
    }

    private bool ctrlTogether
    {
      get
      {
        return this.hair.ctrlTogether;
      }
    }

    private CustomBase.CustomSettingSave.HairCtrlSetting hairCtrlSetting
    {
      get
      {
        return this.customBase.customSettingSave.hairCtrlSetting;
      }
    }

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    public void UpdateHairList()
    {
      this.sscHairType.CreateList(CvsBase.CreateSelectList(new ChaListDefine.CategoryNo[4]
      {
        ChaListDefine.CategoryNo.so_hair_b,
        ChaListDefine.CategoryNo.so_hair_f,
        ChaListDefine.CategoryNo.so_hair_s,
        ChaListDefine.CategoryNo.so_hair_o
      }[this.SNo], ChaListDefine.KeyType.Unknown));
    }

    private void CalculateUI()
    {
      this.ssMetallic.SetSliderValue(this.hair.parts[this.SNo].metallic);
      this.ssSmoothness.SetSliderValue(this.hair.parts[this.SNo].smoothness);
      this.sldGuidSpeed.set_value(this.hairCtrlSetting.controllerSpeed);
      this.sldGuidScale.set_value(this.hairCtrlSetting.controllerScale);
    }

    public override void UpdateCustomUI()
    {
      if (this.backSNo != this.SNo)
      {
        this.UpdateHairList();
        this.backSNo = this.SNo;
      }
      base.UpdateCustomUI();
      this.CalculateUI();
      this.tglGuidDraw.SetIsOnWithoutCallback(this.hairCtrlSetting.drawController);
      this.tglGuidType[this.hairCtrlSetting.controllerType].SetIsOnWithoutCallback(true);
      this.tglGuidType[this.hairCtrlSetting.controllerType & 1].SetIsOnWithoutCallback(false);
      this.sscHairType.SetToggleID(this.hair.parts[this.SNo].id);
      this.csBaseColor.SetColor(this.hair.parts[this.SNo].baseColor);
      this.csTopColor.SetColor(this.hair.parts[this.SNo].topColor);
      this.csUnderColor.SetColor(this.hair.parts[this.SNo].underColor);
      this.csSpecular.SetColor(this.hair.parts[this.SNo].specular);
      this.SetDrawSettingByHair();
    }

    public void UpdateDrawControllerState()
    {
      int controllerType = this.hairCtrlSetting.controllerType;
      bool drawController = this.hairCtrlSetting.drawController;
      float controllerSpeed = this.hairCtrlSetting.controllerSpeed;
      float controllerScale = this.hairCtrlSetting.controllerScale;
      this.tglGuidDraw.SetIsOnWithoutCallback(drawController);
      this.tglGuidType[controllerType].SetIsOnWithoutCallback(true);
      this.sldGuidSpeed.set_value(controllerSpeed);
      this.sldGuidScale.set_value(controllerScale);
    }

    public void SetDrawSettingByHair()
    {
      if (this.chaCtrl.cmpHair == null)
        return;
      this.lstHairBundleSet.Clear();
      for (int index = this.trfContent.get_childCount() - 1; index >= 0; --index)
      {
        Transform child = this.trfContent.GetChild(index);
        if (!(((Object) child).get_name() == "CtrlSetting") && !(((Object) child).get_name() == "control"))
        {
          child.SetParent((Transform) null);
          ((Object) child).set_name("delete_reserve");
          Object.Destroy((Object) ((Component) child).get_gameObject());
        }
      }
      if (Object.op_Inequality((Object) null, (Object) this.customBase.objHairControllerTop))
      {
        for (int index = this.customBase.objHairControllerTop.get_transform().get_childCount() - 1; index >= 0; --index)
        {
          Transform child = this.customBase.objHairControllerTop.get_transform().GetChild(index);
          child.SetParent((Transform) null);
          ((Object) child).set_name("delete_reserve");
          Object.Destroy((Object) ((Component) child).get_gameObject());
        }
      }
      this.customBase.customCtrl.camCtrl.ClearListCollider();
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl.cmpHair[this.SNo]))
      {
        this.ShowOrHideTab(false, 1, 2, 3);
      }
      else
      {
        this.ShowOrHideTab(true, 1);
        bool[] flagArray = new bool[3]
        {
          this.chaCtrl.cmpHair[this.SNo].useAcsColor01,
          this.chaCtrl.cmpHair[this.SNo].useAcsColor02,
          this.chaCtrl.cmpHair[this.SNo].useAcsColor03
        };
        this.ShowOrHideTab((flagArray[0] | flagArray[1] | flagArray[2] ? 1 : 0) != 0, 2);
        this.customBase.drawTopHairColor = this.chaCtrl.cmpHair[this.SNo].useTopColor;
        this.customBase.drawUnderHairColor = this.chaCtrl.cmpHair[this.SNo].useUnderColor;
        for (int index = 0; index < this.csAcsColor.Length; ++index)
        {
          ((Component) this.csAcsColor[index]).get_gameObject().SetActiveIfDifferent(flagArray[index]);
          this.csAcsColor[index].SetColor(this.hair.parts[this.SNo].acsColorInfo[index].color);
        }
        int titleNo = 1;
        for (int _idx = 0; _idx < this.hair.parts[this.SNo].dictBundle.Count; ++_idx)
        {
          if (!Object.op_Equality((Object) null, (Object) this.chaCtrl.cmpHair[this.SNo].boneInfo[_idx].trfCorrect))
          {
            GameObject self = (GameObject) Object.Instantiate<GameObject>((M0) this.tmpBundleObj);
            self.get_transform().SetParent(this.trfContent, false);
            CustomHairBundleSet component = (CustomHairBundleSet) self.GetComponent<CustomHairBundleSet>();
            component.CreateGuid(this.customBase.objHairControllerTop, this.chaCtrl.cmpHair[this.SNo].boneInfo[_idx]);
            component.Initialize(this.SNo, _idx, titleNo);
            this.lstHairBundleSet.Add(component);
            self.SetActiveIfDifferent(true);
            ++titleNo;
          }
        }
        this.ShowOrHideTab((0 != this.lstHairBundleSet.Count ? 1 : 0) != 0, 3);
      }
    }

    public void UpdateAllBundleUI(int excludeIdx = -1)
    {
      this.allReset = true;
      int count = this.lstHairBundleSet.Count;
      for (int index = 0; index < count; ++index)
      {
        if (index != excludeIdx)
          this.lstHairBundleSet[index].UpdateCustomUI();
      }
      this.allReset = false;
    }

    public void UpdateGuidType()
    {
      if (this.lstHairBundleSet == null)
        return;
      int count = this.lstHairBundleSet.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.lstHairBundleSet[index].cmpGuid))
          this.lstHairBundleSet[index].cmpGuid.SetMode(this.hairCtrlSetting.controllerType);
      }
    }

    public void UpdateGuidSpeed()
    {
      if (this.lstHairBundleSet == null)
        return;
      int count = this.lstHairBundleSet.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.lstHairBundleSet[index].cmpGuid))
          this.lstHairBundleSet[index].cmpGuid.speedMove = this.hairCtrlSetting.controllerSpeed;
      }
    }

    public void UpdateGuidScale()
    {
      if (this.lstHairBundleSet == null)
        return;
      int count = this.lstHairBundleSet.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.lstHairBundleSet[index].cmpGuid))
        {
          this.lstHairBundleSet[index].cmpGuid.scaleAxis = this.hairCtrlSetting.controllerScale;
          this.lstHairBundleSet[index].cmpGuid.UpdateScale();
        }
      }
    }

    public bool IsDrag()
    {
      if (this.lstHairBundleSet == null)
        return false;
      int count = this.lstHairBundleSet.Count;
      for (int index = 0; index < count; ++index)
      {
        if (!Object.op_Equality((Object) null, (Object) this.lstHairBundleSet[index].cmpGuid) && this.lstHairBundleSet[index].isDrag)
          return true;
      }
      return false;
    }

    public void ShortcutChangeGuidType(int type)
    {
      if (this.IsDrag())
        return;
      this.tglGuidType[type].set_isOn(true);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsH_Hair.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsHair += new Action(((CvsBase) this).UpdateCustomUI);
      this.UpdateHairList();
      this.sscHairType.SetToggleID(this.hair.parts[this.SNo].id);
      this.sscHairType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.hair.parts[this.SNo].id == info.id)
          return;
        this.chaCtrl.ChangeHair(this.SNo, info.id, false);
        this.chaCtrl.SetHairAcsDefaultColorParameterOnly(this.SNo);
        this.chaCtrl.ChangeSettingHairAcsColor(this.SNo);
        this.SetDrawSettingByHair();
      });
      this.csBaseColor.actUpdateColor = (Action<Color>) (color =>
      {
        if (this.autoSetting)
        {
          Color topColor;
          Color underColor;
          Color specular;
          this.chaCtrl.CreateHairColor(color, out topColor, out underColor, out specular);
          for (int parts = 0; parts < this.hair.parts.Length; ++parts)
          {
            if (this.sameSetting || parts == this.SNo)
            {
              this.hair.parts[parts].baseColor = color;
              this.hair.parts[parts].topColor = topColor;
              this.hair.parts[parts].underColor = underColor;
              this.hair.parts[parts].specular = specular;
              this.chaCtrl.ChangeSettingHairColor(parts, true, this.autoSetting, this.autoSetting);
              this.chaCtrl.ChangeSettingHairSpecular(parts);
              this.csTopColor.SetColor(this.hair.parts[this.SNo].topColor);
              this.csUnderColor.SetColor(this.hair.parts[this.SNo].underColor);
              this.csSpecular.SetColor(this.hair.parts[this.SNo].specular);
            }
          }
        }
        else
        {
          for (int parts = 0; parts < this.hair.parts.Length; ++parts)
          {
            if (this.sameSetting || parts == this.SNo)
            {
              this.hair.parts[parts].baseColor = color;
              this.chaCtrl.ChangeSettingHairColor(parts, true, this.autoSetting, this.autoSetting);
            }
          }
        }
      });
      this.csTopColor.actUpdateColor = (Action<Color>) (color =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].topColor = color;
            this.chaCtrl.ChangeSettingHairColor(parts, false, true, false);
          }
        }
      });
      this.csUnderColor.actUpdateColor = (Action<Color>) (color =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].underColor = color;
            this.chaCtrl.ChangeSettingHairColor(parts, false, false, true);
          }
        }
      });
      this.csSpecular.actUpdateColor = (Action<Color>) (color =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].specular = color;
            this.chaCtrl.ChangeSettingHairSpecular(parts);
          }
        }
      });
      this.ssMetallic.onChange = (Action<float>) (value =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].metallic = value;
            this.chaCtrl.ChangeSettingHairMetallic(parts);
          }
        }
      });
      this.ssMetallic.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.hair.parts[this.SNo].metallic);
      this.ssSmoothness.onChange = (Action<float>) (value =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].smoothness = value;
            this.chaCtrl.ChangeSettingHairSmoothness(parts);
          }
        }
      });
      this.ssSmoothness.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.hair.parts[this.SNo].smoothness);
      this.hcPreset.onClick = (Action<CustomHairColorPreset.HairColorInfo>) (preset =>
      {
        for (int parts = 0; parts < this.hair.parts.Length; ++parts)
        {
          if (this.sameSetting || parts == this.SNo)
          {
            this.hair.parts[parts].baseColor = preset.baseColor;
            this.hair.parts[parts].topColor = preset.topColor;
            this.hair.parts[parts].underColor = preset.underColor;
            this.hair.parts[parts].specular = preset.specular;
            this.hair.parts[parts].metallic = preset.metallic;
            this.hair.parts[parts].smoothness = preset.smoothness;
            this.chaCtrl.ChangeSettingHairColor(parts, true, true, true);
            this.chaCtrl.ChangeSettingHairSpecular(parts);
            this.chaCtrl.ChangeSettingHairMetallic(parts);
            this.chaCtrl.ChangeSettingHairSmoothness(parts);
            this.csBaseColor.SetColor(this.hair.parts[parts].baseColor);
            this.csTopColor.SetColor(this.hair.parts[parts].topColor);
            this.csUnderColor.SetColor(this.hair.parts[parts].underColor);
            this.csSpecular.SetColor(this.hair.parts[parts].specular);
            this.ssMetallic.SetSliderValue(this.hair.parts[parts].metallic);
            this.ssSmoothness.SetSliderValue(this.hair.parts[parts].smoothness);
          }
        }
      });
      if (this.csAcsColor != null && ((IEnumerable<CustomColorSet>) this.csAcsColor).Any<CustomColorSet>())
        ((IEnumerable<CustomColorSet>) this.csAcsColor).ToList<CustomColorSet>().ForEach((Action<CustomColorSet>) (item => item.actUpdateColor = (Action<Color>) (color =>
        {
          this.hair.parts[this.SNo].acsColorInfo[0].color = color;
          this.chaCtrl.ChangeSettingHairAcsColor(this.SNo);
        })));
      if (Object.op_Implicit((Object) this.btnCorrectAllReset))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCorrectAllReset), (Action<M0>) (_ =>
        {
          this.chaCtrl.SetDefaultHairCorrectPosRateAll(this.SNo);
          this.chaCtrl.SetDefaultHairCorrectRotRateAll(this.SNo);
          this.UpdateAllBundleUI(-1);
        }));
      ObservableExtensions.Subscribe<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) this.tglGuidDraw.onValueChanged), (Action<M0>) (isOn => this.hairCtrlSetting.drawController = isOn));
      if (((IEnumerable<Toggle>) this.tglGuidType).Any<Toggle>())
      {
        // ISSUE: object of a compiler-generated type is created
        ((IEnumerable<Toggle>) this.tglGuidType).Select<Toggle, \u003C\u003E__AnonType15<Toggle, int>>((Func<Toggle, int, \u003C\u003E__AnonType15<Toggle, int>>) ((val, idx) => new \u003C\u003E__AnonType15<Toggle, int>(val, idx))).Where<\u003C\u003E__AnonType15<Toggle, int>>((Func<\u003C\u003E__AnonType15<Toggle, int>, bool>) (item => Object.op_Inequality((Object) item.val, (Object) null))).ToList<\u003C\u003E__AnonType15<Toggle, int>>().ForEach((Action<\u003C\u003E__AnonType15<Toggle, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(UnityEventExtensions.AsObservable<bool>((UnityEvent<M0>) item.val.onValueChanged), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn =>
        {
          this.hairCtrlSetting.controllerType = item.idx;
          this.UpdateGuidType();
        }))));
      }
      ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldGuidSpeed), (Action<M0>) (val =>
      {
        this.hairCtrlSetting.controllerSpeed = val;
        this.UpdateGuidSpeed();
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldGuidSpeed), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldGuidSpeed.set_value(Mathf.Clamp(this.sldGuidSpeed.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.1f, 1f));
      }));
      ObservableExtensions.Subscribe<float>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.sldGuidScale), (Action<M0>) (val =>
      {
        this.hairCtrlSetting.controllerScale = val;
        this.UpdateGuidScale();
      }));
      ObservableExtensions.Subscribe<PointerEventData>((IObservable<M0>) ObservableTriggerExtensions.OnScrollAsObservable((UIBehaviour) this.sldGuidScale), (Action<M0>) (scl =>
      {
        if (!this.customBase.sliderControlWheel)
          return;
        this.sldGuidScale.set_value(Mathf.Clamp(this.sldGuidScale.get_value() + (float) (scl.get_scrollDelta().y * -0.00999999977648258), 0.3f, 3f));
      }));
      this.UpdateDrawControllerState();
      this.StartCoroutine(this.SetInputText());
      this.backSNo = this.SNo;
    }
  }
}
