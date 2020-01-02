// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_Beard
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
  public class CvsF_Beard : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscBeardType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csBeardColor;

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
      this.sscBeardType.SetToggleID(this.face.beardId);
      this.csBeardColor.SetColor(this.face.beardColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_Beard.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBeard += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscBeardType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.mt_beard, ChaListDefine.KeyType.Unknown));
      this.sscBeardType.SetToggleID(this.face.beardId);
      this.sscBeardType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.beardId == info.id)
          return;
        this.face.beardId = info.id;
        this.chaCtrl.ChangeBeardKind();
      });
      this.csBeardColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.beardColor = color;
        this.chaCtrl.ChangeBeardColor();
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
