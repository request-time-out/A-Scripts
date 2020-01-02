// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_Mole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_Mole : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscMole;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csMole;
    [Header("【設定03】----------------------")]
    [SerializeField]
    private CustomPushScrollController pscMoleLayout;
    [SerializeField]
    private CustomSliderSet ssMoleW;
    [SerializeField]
    private CustomSliderSet ssMoleH;
    [SerializeField]
    private CustomSliderSet ssMoleX;
    [SerializeField]
    private CustomSliderSet ssMoleY;
    private Dictionary<int, Vector4> dictMoleLayout;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ssMoleW.SetSliderValue((float) this.face.moleLayout.x);
      this.ssMoleH.SetSliderValue((float) this.face.moleLayout.y);
      this.ssMoleX.SetSliderValue((float) this.face.moleLayout.z);
      this.ssMoleY.SetSliderValue((float) this.face.moleLayout.w);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscMole.SetToggleID(this.face.moleId);
      this.csMole.SetColor(this.face.moleColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_Mole.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsMole += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscMole.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_mole, ChaListDefine.KeyType.Unknown));
      this.sscMole.SetToggleID(this.face.moleId);
      this.sscMole.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.moleId == info.id)
          return;
        this.face.moleId = info.id;
        this.chaCtrl.AddUpdateCMFaceTexFlags(false, false, false, false, false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.csMole.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.moleColor = color;
        this.chaCtrl.AddUpdateCMFaceColorFlags(false, false, false, false, false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      // ISSUE: object of a compiler-generated type is created
      this.dictMoleLayout = this.lstCtrl.GetCategoryInfo(ChaListDefine.CategoryNo.mole_layout).Select<KeyValuePair<int, ListInfoBase>, \u003C\u003E__AnonType16<int, float, float, float, float>>((Func<KeyValuePair<int, ListInfoBase>, int, \u003C\u003E__AnonType16<int, float, float, float, float>>) ((val, idx) => new \u003C\u003E__AnonType16<int, float, float, float, float>(idx, val.Value.GetInfoFloat(ChaListDefine.KeyType.Scale), val.Value.GetInfoFloat(ChaListDefine.KeyType.Scale), val.Value.GetInfoFloat(ChaListDefine.KeyType.PosX), val.Value.GetInfoFloat(ChaListDefine.KeyType.PosY)))).ToDictionary<\u003C\u003E__AnonType16<int, float, float, float, float>, int, Vector4>((Func<\u003C\u003E__AnonType16<int, float, float, float, float>, int>) (v => v.idx), (Func<\u003C\u003E__AnonType16<int, float, float, float, float>, Vector4>) (v =>
      {
        Vector4 vector4 = (Vector4) null;
        vector4.x = (__Null) (double) v.x;
        vector4.y = (__Null) (double) v.y;
        vector4.z = (__Null) (double) v.z;
        vector4.w = (__Null) (double) v.w;
        return vector4;
      }));
      this.pscMoleLayout.CreateList(CvsBase.CreatePushList(ChaListDefine.CategoryNo.mole_layout));
      this.pscMoleLayout.onPush = (Action<CustomPushInfo>) (info =>
      {
        Vector4 vector4;
        if (info == null || !this.dictMoleLayout.TryGetValue(info.id, out vector4))
          return;
        this.face.moleLayout = vector4;
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(false, false, true);
        this.chaCtrl.CreateFaceTexture();
        this.ssMoleW.SetSliderValue((float) this.face.moleLayout.x);
        this.ssMoleH.SetSliderValue((float) this.face.moleLayout.y);
        this.ssMoleX.SetSliderValue((float) this.face.moleLayout.z);
        this.ssMoleY.SetSliderValue((float) this.face.moleLayout.w);
      });
      this.ssMoleW.onChange = (Action<float>) (value =>
      {
        this.face.moleLayout = new Vector4(value, (float) this.face.moleLayout.y, (float) this.face.moleLayout.z, (float) this.face.moleLayout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssMoleW.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.moleLayout.x);
      this.ssMoleH.onChange = (Action<float>) (value =>
      {
        this.face.moleLayout = new Vector4((float) this.face.moleLayout.x, value, (float) this.face.moleLayout.z, (float) this.face.moleLayout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssMoleH.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.moleLayout.y);
      this.ssMoleX.onChange = (Action<float>) (value =>
      {
        this.face.moleLayout = new Vector4((float) this.face.moleLayout.x, (float) this.face.moleLayout.y, value, (float) this.face.moleLayout.w);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssMoleX.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.moleLayout.z);
      this.ssMoleY.onChange = (Action<float>) (value =>
      {
        this.face.moleLayout = new Vector4((float) this.face.moleLayout.x, (float) this.face.moleLayout.y, (float) this.face.moleLayout.z, value);
        this.chaCtrl.AddUpdateCMFaceLayoutFlags(false, false, true);
        this.chaCtrl.CreateFaceTexture();
      });
      this.ssMoleY.onSetDefaultValue = (Func<float>) (() => (float) this.defChaCtrl.custom.face.moleLayout.w);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
