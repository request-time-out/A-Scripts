// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Paint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_Paint : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscPaintType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csPaintColor;
    [SerializeField]
    private CustomSliderSet ssPaintGloss;
    [SerializeField]
    private CustomSliderSet ssPaintMetallic;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscPaintLayout;
    [SerializeField]
    private CustomSliderSet ssPaintW;
    [SerializeField]
    private CustomSliderSet ssPaintH;
    [SerializeField]
    private CustomSliderSet ssPaintX;
    [SerializeField]
    private CustomSliderSet ssPaintY;
    [SerializeField]
    private CustomSliderSet ssPaintRot;
    private Dictionary<int, Vector4> dictPaintLayout;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssPaintGloss.SetSliderValue(this.body.paintInfo[this.SNo].glossPower);
      this.ssPaintMetallic.SetSliderValue(this.body.paintInfo[this.SNo].metallicPower);
      this.ssPaintW.SetSliderValue((float) this.body.paintInfo[this.SNo].layout.x);
      this.ssPaintH.SetSliderValue((float) this.body.paintInfo[this.SNo].layout.y);
      this.ssPaintX.SetSliderValue((float) this.body.paintInfo[this.SNo].layout.z);
      this.ssPaintY.SetSliderValue((float) this.body.paintInfo[this.SNo].layout.w);
      this.ssPaintRot.SetSliderValue(this.body.paintInfo[this.SNo].rotation);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscPaintType.SetToggleID(this.body.paintInfo[this.SNo].id);
      this.csPaintColor.SetColor(this.body.paintInfo[this.SNo].color);
      this.sscPaintLayout.SetToggleID(this.body.paintInfo[this.SNo].layoutId);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Paint.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBodyPaint += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscPaintType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_paint, ChaListDefine.KeyType.Unknown));
      this.sscPaintType.SetToggleID(this.body.paintInfo[this.SNo].id);
      this.sscPaintType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.paintInfo[this.SNo].id == info.id)
          return;
        this.body.paintInfo[this.SNo].id = info.id;
        this.chaCtrl.AddUpdateCMBodyTexFlags(false, 0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateBodyTexture();
      });
      this.csPaintColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.paintInfo[this.SNo].color = color;
        this.chaCtrl.AddUpdateCMBodyColorFlags(false, 0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintGloss.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].glossPower = value;
        this.chaCtrl.AddUpdateCMBodyGlossFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.paintInfo[this.SNo].glossPower);
      this.ssPaintMetallic.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].metallicPower = value;
        this.chaCtrl.AddUpdateCMBodyGlossFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintMetallic.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.paintInfo[this.SNo].metallicPower);
      this.sscPaintLayout.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.bodypaint_layout, ChaListDefine.KeyType.Unknown));
      this.sscPaintLayout.SetToggleID(this.body.paintInfo[this.SNo].layoutId);
      this.sscPaintLayout.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.paintInfo[this.SNo].layoutId == info.id)
          return;
        this.body.paintInfo[this.SNo].layoutId = info.id;
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintW.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].layout = new Vector4(value, (float) this.body.paintInfo[this.SNo].layout.y, (float) this.body.paintInfo[this.SNo].layout.z, (float) this.body.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintW.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.body.paintInfo[this.SNo].layout.x);
      this.ssPaintH.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].layout = new Vector4((float) this.body.paintInfo[this.SNo].layout.x, value, (float) this.body.paintInfo[this.SNo].layout.z, (float) this.body.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintH.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.body.paintInfo[this.SNo].layout.y);
      this.ssPaintX.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].layout = new Vector4((float) this.body.paintInfo[this.SNo].layout.x, (float) this.body.paintInfo[this.SNo].layout.y, value, (float) this.body.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintX.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.body.paintInfo[this.SNo].layout.z);
      this.ssPaintY.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].layout = new Vector4((float) this.body.paintInfo[this.SNo].layout.x, (float) this.body.paintInfo[this.SNo].layout.y, (float) this.body.paintInfo[this.SNo].layout.z, value);
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintY.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.body.paintInfo[this.SNo].layout.w);
      this.ssPaintRot.onChange = (Action<float>) (value =>
      {
        this.body.paintInfo[this.SNo].rotation = value;
        this.chaCtrl.AddUpdateCMBodyLayoutFlags(0 == this.SNo, 1 == this.SNo);
        this.chaCtrl.CreateBodyTexture();
      });
      this.ssPaintRot.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.paintInfo[this.SNo].rotation);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
