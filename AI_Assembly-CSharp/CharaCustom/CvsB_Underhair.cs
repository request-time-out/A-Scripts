// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_Underhair
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
  public class CvsB_Underhair : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscUnderhairType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csUnderhairColor;

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
      this.sscUnderhairType.SetToggleID(this.body.underhairId);
      this.csUnderhairColor.SetColor(this.body.underhairColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsB_Underhair.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsUnderhair += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscUnderhairType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_underhair, ChaListDefine.KeyType.Unknown));
      this.sscUnderhairType.SetToggleID(this.body.underhairId);
      this.sscUnderhairType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.body.underhairId == info.id)
          return;
        this.body.underhairId = info.id;
        this.chaCtrl.ChangeUnderHairKind();
      });
      this.csUnderhairColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.body.underhairColor = color;
        this.chaCtrl.ChangeUnderHairColor();
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
