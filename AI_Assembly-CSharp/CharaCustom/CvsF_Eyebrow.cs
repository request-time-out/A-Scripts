// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_Eyebrow
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
  public class CvsF_Eyebrow : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscEyebrowType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csEyebrowColor;

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
      this.sscEyebrowType.SetToggleID(this.face.eyebrowId);
      this.csEyebrowColor.SetColor(this.face.eyebrowColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_Eyebrow.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyebrow += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscEyebrowType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eyebrow, ChaListDefine.KeyType.Unknown));
      this.sscEyebrowType.SetToggleID(this.face.eyebrowId);
      this.sscEyebrowType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.eyebrowId == info.id)
          return;
        this.face.eyebrowId = info.id;
        this.chaCtrl.ChangeEyebrowKind();
      });
      this.csEyebrowColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.eyebrowColor = color;
        this.chaCtrl.ChangeEyebrowColor();
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
