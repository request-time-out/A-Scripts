// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_MakeupPaint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_MakeupPaint : CvsBase
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
    private CustomPushScrollController pscPaintLayout;
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
      this.ssPaintGloss.SetSliderValue(this.makeup.paintInfo[this.SNo].glossPower);
      this.ssPaintMetallic.SetSliderValue(this.makeup.paintInfo[this.SNo].metallicPower);
      this.ssPaintW.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.x);
      this.ssPaintH.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.y);
      this.ssPaintX.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.z);
      this.ssPaintY.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.w);
      this.ssPaintRot.SetSliderValue(this.makeup.paintInfo[this.SNo].rotation);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscPaintType.SetToggleID(this.makeup.paintInfo[this.SNo].id);
      this.csPaintColor.SetColor(this.makeup.paintInfo[this.SNo].color);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_MakeupPaint.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFacePaint += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscPaintType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_paint, ChaListDefine.KeyType.Unknown));
      this.sscPaintType.SetToggleID(this.makeup.paintInfo[this.SNo].id);
      this.sscPaintType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.makeup.paintInfo[this.SNo].id == info.id)
          return;
        this.makeup.paintInfo[this.SNo].id = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(false, false, 0 == this.SNo, 1 == this.SNo, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.csPaintColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.makeup.paintInfo[this.SNo].color = color;
        this.chaCtrl.AddUpdateCMFaceColorFlags(false, false, 0 == this.SNo, 1 == this.SNo, false, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintGloss.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].glossPower = value;
        this.chaCtrl.AddUpdateCMFaceGlossFlags(false, 0 == this.SNo, 1 == this.SNo, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintGloss.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].glossPower);
      this.ssPaintMetallic.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].metallicPower = value;
        this.chaCtrl.AddUpdateCMFaceGlossFlags(false, 0 == this.SNo, 1 == this.SNo, false, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintMetallic.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].metallicPower);
      // ISSUE: object of a compiler-generated type is created
      this.dictPaintLayout = this.lstCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.facepaint_layout).Select<KeyValuePair<int, ListInfoBase>, \u003C\u003E__AnonType16<int, float, float, float, float>>((Func<KeyValuePair<int, ListInfoBase>, int, \u003C\u003E__AnonType16<int, float, float, float, float>>) ((val, idx) => new \u003C\u003E__AnonType16<int, float, float, float, float>(idx, val.Value.GetInfoFloat(ChaListDefine.KeyType.Scale), val.Value.GetInfoFloat(ChaListDefine.KeyType.Scale), val.Value.GetInfoFloat(ChaListDefine.KeyType.PosX), val.Value.GetInfoFloat(ChaListDefine.KeyType.PosY)))).ToDictionary<\u003C\u003E__AnonType16<int, float, float, float, float>, int, Vector4>((Func<\u003C\u003E__AnonType16<int, float, float, float, float>, int>) (v => v.idx), (Func<\u003C\u003E__AnonType16<int, float, float, float, float>, Vector4>) (v =>
      {
        Vector4 vector4 = (Vector4) null;
        vector4.x = (__Null) (double) v.x;
        vector4.y = (__Null) (double) v.y;
        vector4.z = (__Null) (double) v.z;
        vector4.w = (__Null) (double) v.w;
        return vector4;
      }));
      this.pscPaintLayout.CreateList(CvsBase.CreatePushList(ChaListDefine.CategoryNo.facepaint_layout));
      this.pscPaintLayout.onPush = (Action<CustomPushInfo>) (info =>
      {
        Vector4 vector4;
        if (info == null || !this.dictPaintLayout.TryGetValue(info.id, out vector4))
          return;
        this.makeup.paintInfo[this.SNo].layout = vector4;
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
        this.ssPaintW.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.x);
        this.ssPaintH.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.y);
        this.ssPaintX.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.z);
        this.ssPaintY.SetSliderValue((float) this.makeup.paintInfo[this.SNo].layout.w);
      });
      this.ssPaintW.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].layout = new Vector4(value, (float) this.makeup.paintInfo[this.SNo].layout.y, (float) this.makeup.paintInfo[this.SNo].layout.z, (float) this.makeup.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintW.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].layout.x);
      this.ssPaintH.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].layout = new Vector4((float) this.makeup.paintInfo[this.SNo].layout.x, value, (float) this.makeup.paintInfo[this.SNo].layout.z, (float) this.makeup.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintH.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].layout.y);
      this.ssPaintX.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].layout = new Vector4((float) this.makeup.paintInfo[this.SNo].layout.x, (float) this.makeup.paintInfo[this.SNo].layout.y, value, (float) this.makeup.paintInfo[this.SNo].layout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintX.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].layout.z);
      this.ssPaintY.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].layout = new Vector4((float) this.makeup.paintInfo[this.SNo].layout.x, (float) this.makeup.paintInfo[this.SNo].layout.y, (float) this.makeup.paintInfo[this.SNo].layout.z, value);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintY.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].layout.w);
      this.ssPaintRot.onChange = (Action<float>) (value =>
      {
        this.makeup.paintInfo[this.SNo].rotation = value;
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(0 == this.SNo, 1 == this.SNo, false);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssPaintRot.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.makeup.paintInfo[this.SNo].rotation);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
