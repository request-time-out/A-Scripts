// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomHairBundleSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomHairBundleSet : MonoBehaviour
  {
    [SerializeField]
    private CvsH_Hair cvsH_Hair;
    [SerializeField]
    private Text title;
    [SerializeField]
    private CustomSliderSet[] ssMove;
    [SerializeField]
    private CustomSliderSet[] ssRot;
    [SerializeField]
    private Toggle tglNoShake;
    [SerializeField]
    private Toggle tglGuidDraw;
    [SerializeField]
    private GameObject tmpObjGuid;

    public CustomHairBundleSet()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    private ChaFileHair hair
    {
      get
      {
        return this.chaCtrl.fileHair;
      }
    }

    private CustomBase.CustomSettingSave.HairCtrlSetting hairCtrlSetting
    {
      get
      {
        return this.customBase.customSettingSave.hairCtrlSetting;
      }
    }

    private bool ctrlTogether
    {
      get
      {
        return this.hair.ctrlTogether;
      }
    }

    public int parts { get; set; }

    public int idx { get; set; }

    public bool reset { get; set; }

    public CustomGuideObject cmpGuid { get; private set; }

    public bool isDrag { get; private set; }

    public void UpdateCustomUI()
    {
      ChaFileHair.PartsInfo.BundleInfo bundleInfo;
      if (this.parts == -1 || this.idx == -1 || !this.hair.parts[this.parts].dictBundle.TryGetValue(this.idx, out bundleInfo))
        return;
      this.ssMove[0].SetSliderValue((float) bundleInfo.moveRate.x);
      this.ssMove[1].SetSliderValue((float) bundleInfo.moveRate.y);
      this.ssMove[2].SetSliderValue((float) bundleInfo.moveRate.z);
      this.ssRot[0].SetSliderValue((float) bundleInfo.rotRate.x);
      this.ssRot[1].SetSliderValue((float) bundleInfo.rotRate.y);
      this.ssRot[2].SetSliderValue((float) bundleInfo.rotRate.z);
    }

    public void SetControllerTransform()
    {
      Transform trfCorrect = this.chaCtrl.cmpHair[this.parts].boneInfo[this.idx].trfCorrect;
      if (Object.op_Equality((Object) null, (Object) trfCorrect) || Object.op_Equality((Object) null, (Object) this.cmpGuid))
        return;
      this.cmpGuid.amount.position = trfCorrect.get_position();
      this.cmpGuid.amount.rotation = trfCorrect.get_eulerAngles();
    }

    public void SetHairTransform(bool updateInfo, int ctrlAxisType)
    {
      Transform trfCorrect = this.chaCtrl.cmpHair[this.parts].boneInfo[this.idx].trfCorrect;
      if (Object.op_Equality((Object) null, (Object) trfCorrect) || Object.op_Equality((Object) null, (Object) this.cmpGuid))
        return;
      int _flag = 1;
      switch (ctrlAxisType)
      {
        case 0:
          _flag = 1;
          break;
        case 1:
          _flag = 2;
          break;
        case 2:
          _flag = 4;
          break;
        case 3:
          _flag = 7;
          break;
      }
      if (this.customBase.customSettingSave.hairCtrlSetting.controllerType == 0)
      {
        trfCorrect.set_position(this.cmpGuid.amount.position);
        if (updateInfo)
        {
          this.chaCtrl.SetHairCorrectPosValue(this.parts, this.idx, this.cmpGuid.amount.position, _flag);
          this.chaCtrl.ChangeSettingHairCorrectPos(this.parts, this.idx);
        }
      }
      else
      {
        trfCorrect.set_eulerAngles(this.cmpGuid.amount.rotation);
        if (updateInfo)
        {
          this.chaCtrl.SetHairCorrectRotValue(this.parts, this.idx, this.cmpGuid.amount.rotation, _flag);
          this.chaCtrl.ChangeSettingHairCorrectRot(this.parts, this.idx);
        }
      }
      this.UpdateCustomUI();
    }

    public void CreateGuid(GameObject objParent, CmpHair.BoneInfo binfo)
    {
      Transform transform = !Object.op_Equality((Object) null, (Object) objParent) ? objParent.get_transform() : (Transform) null;
      GameObject self = (GameObject) Object.Instantiate<GameObject>((M0) this.tmpObjGuid);
      self.SetActiveIfDifferent(true);
      self.get_transform().SetParent(transform);
      this.cmpGuid = (CustomGuideObject) self.GetComponent<CustomGuideObject>();
      this.cmpGuid.SetMode(this.hairCtrlSetting.controllerType);
      this.cmpGuid.speedMove = this.hairCtrlSetting.controllerSpeed;
      this.cmpGuid.scaleAxis = this.hairCtrlSetting.controllerScale;
      this.cmpGuid.UpdateScale();
      ObjectCategoryBehaviour component1 = (ObjectCategoryBehaviour) self.GetComponent<ObjectCategoryBehaviour>();
      CustomGuideLimit component2 = (CustomGuideLimit) component1.GetObject(0).GetComponent<CustomGuideLimit>();
      component2.limited = true;
      component2.trfParent = binfo.trfCorrect.get_parent();
      component2.limitMin = binfo.posMin;
      component2.limitMax = binfo.posMax;
      CustomGuideLimit component3 = (CustomGuideLimit) component1.GetObject(1).GetComponent<CustomGuideLimit>();
      component3.limited = true;
      component3.trfParent = binfo.trfCorrect.get_parent();
      component3.limitMin = binfo.rotMin;
      component3.limitMax = binfo.rotMax;
    }

    public void Initialize(int _parts, int _idx, int titleNo)
    {
      this.parts = _parts;
      this.idx = _idx;
      if (this.parts == -1 || this.idx == -1)
        return;
      ChaFileHair.PartsInfo.BundleInfo bi;
      if (this.hair.parts[this.parts].dictBundle.TryGetValue(this.idx, out bi))
      {
        this.ssMove[0].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.moveRate.x));
        this.ssMove[1].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.moveRate.y));
        this.ssMove[2].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.moveRate.z));
        this.ssRot[0].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.rotRate.x));
        this.ssRot[1].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.rotRate.y));
        this.ssRot[2].SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) bi.rotRate.z));
        Vector3 vDefPosRate;
        this.chaCtrl.GetDefaultHairCorrectPosRate(this.parts, this.idx, out vDefPosRate);
        Vector3 vDefRotRate;
        this.chaCtrl.GetDefaultHairCorrectRotRate(this.parts, this.idx, out vDefRotRate);
        this.ssMove[0].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector(value, (float) bi.moveRate.y, (float) bi.moveRate.z);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPosAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPos(this.parts, this.idx);
          }
        });
        this.ssMove[0].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefPosRate.x;
        });
        this.ssMove[0].onEndSetDefaultValue = (Action) (() => this.reset = false);
        this.ssMove[1].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector((float) bi.moveRate.x, value, (float) bi.moveRate.z);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPosAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPos(this.parts, this.idx);
          }
        });
        this.ssMove[1].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefPosRate.y;
        });
        this.ssMove[1].onEndSetDefaultValue = (Action) (() => this.reset = false);
        this.ssMove[2].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector((float) bi.moveRate.x, (float) bi.moveRate.y, value);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPosAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.moveRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectPos(this.parts, this.idx);
          }
        });
        this.ssMove[2].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefPosRate.z;
        });
        this.ssMove[2].onEndSetDefaultValue = (Action) (() => this.reset = false);
        this.ssRot[0].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector(value, (float) bi.rotRate.y, (float) bi.rotRate.z);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRotAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRot(this.parts, this.idx);
          }
        });
        this.ssRot[0].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefRotRate.x;
        });
        this.ssRot[0].onEndSetDefaultValue = (Action) (() => this.reset = false);
        this.ssRot[1].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector((float) bi.rotRate.x, value, (float) bi.rotRate.z);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRotAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRot(this.parts, this.idx);
          }
        });
        this.ssRot[1].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefRotRate.y;
        });
        this.ssRot[1].onEndSetDefaultValue = (Action) (() => this.reset = false);
        this.ssRot[2].onChange = (Action<float>) (value =>
        {
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector((float) bi.rotRate.x, (float) bi.rotRate.y, value);
          if (this.ctrlTogether && !this.reset && !this.cvsH_Hair.allReset)
          {
            foreach (KeyValuePair<int, ChaFileHair.PartsInfo.BundleInfo> keyValuePair in this.hair.parts[this.parts].dictBundle)
              keyValuePair.Value.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRotAll(this.parts);
            this.cvsH_Hair.UpdateAllBundleUI(this.idx);
          }
          else
          {
            bi.rotRate = vector3;
            this.chaCtrl.ChangeSettingHairCorrectRot(this.parts, this.idx);
          }
        });
        this.ssRot[2].onSetDefaultValue = (Func<float>) (() =>
        {
          this.reset = true;
          return (float) vDefRotRate.z;
        });
        this.ssRot[2].onEndSetDefaultValue = (Action) (() => this.reset = false);
        if (Object.op_Implicit((Object) this.tglNoShake))
        {
          this.tglNoShake.SetIsOnWithoutCallback(bi.noShake);
          ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglNoShake), (Action<M0>) (isOn =>
          {
            if (bi.noShake == isOn)
              return;
            bi.noShake = isOn;
          }));
        }
        if (Object.op_Implicit((Object) this.tglGuidDraw))
        {
          this.tglGuidDraw.SetIsOnWithoutCallback(true);
          ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.tglGuidDraw), (Action<M0>) (isOn =>
          {
            if (!Object.op_Inequality((Object) null, (Object) this.cmpGuid))
              return;
            ((Component) this.cmpGuid).get_gameObject().SetActiveIfDifferent(isOn);
          }));
        }
      }
      if (Object.op_Implicit((Object) this.title))
        this.title.set_text("調整" + titleNo.ToString("00"));
      this.UpdateCustomUI();
    }

    private void LateUpdate()
    {
      if (!Object.op_Inequality((Object) null, (Object) this.cmpGuid) || !((Component) this.cmpGuid).get_gameObject().get_activeInHierarchy())
        return;
      if (this.cmpGuid.isDrag)
        this.SetHairTransform(false, this.cmpGuid.ctrlAxisType);
      else if (this.isDrag)
        this.SetHairTransform(true, this.cmpGuid.ctrlAxisType);
      else
        this.SetControllerTransform();
      this.isDrag = this.cmpGuid.isDrag;
      this.customBase.cursorDraw = !this.cmpGuid.isDrag;
    }
  }
}
