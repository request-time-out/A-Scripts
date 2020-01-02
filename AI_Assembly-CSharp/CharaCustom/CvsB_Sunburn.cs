// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Sunburn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_Sunburn : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscSunburnType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csSunburnColor;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.sscSunburnType.SetToggleID(this.body.sunburnId);
      this.csSunburnColor.SetColor(this.body.sunburnColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Sunburn.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsSunburn += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscSunburnType.CreateList(CvsBase.CreateSelectList(this.chaCtrl.sex != (byte) 0 ? ChaListDefine.CategoryNo.ft_sunburn : ChaListDefine.CategoryNo.mt_sunburn, ChaListDefine.KeyType.Unknown));
      this.sscSunburnType.SetToggleID(this.body.sunburnId);
      this.sscSunburnType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.sunburnId == info.id)
          return;
        this.body.sunburnId = info.id;
        this.chaCtrl.AddUpdateCMBodyTexFlags(false, false, false, true);
        this.chaCtrl.CreateBodyTexture();
      });
      this.csSunburnColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.sunburnColor = color;
        this.chaCtrl.AddUpdateCMBodyColorFlags(false, false, false, true);
        this.chaCtrl.CreateBodyTexture();
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
