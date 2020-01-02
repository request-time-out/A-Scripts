// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_Eyelashes
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
  public class CvsF_Eyelashes : CvsBase
  {
    [Header("【設定01】----------------------")]
    [SerializeField]
    private CustomSelectScrollController sscEyelashesType;
    [Header("【設定02】----------------------")]
    [SerializeField]
    private CustomColorSet csEyelashesColor;

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
      this.sscEyelashesType.SetToggleID(this.face.eyelashesId);
      this.csEyelashesColor.SetColor(this.face.eyelashesColor);
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_Eyelashes.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsEyelashes += new Action(((CvsBase) this).UpdateCustomUI);
      this.sscEyelashesType.CreateList(CvsBase.CreateSelectList(ChaListDefine.CategoryNo.st_eyelash, ChaListDefine.KeyType.Unknown));
      this.sscEyelashesType.SetToggleID(this.face.eyelashesId);
      this.sscEyelashesType.onSelect = (Action<CustomSelectInfo>) (info =>
      {
        if (info == null || this.face.eyelashesId == info.id)
          return;
        this.face.eyelashesId = info.id;
        this.chaCtrl.ChangeEyelashesKind();
      });
      this.csEyelashesColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.face.eyelashesColor = color;
        this.chaCtrl.ChangeEyelashesColor();
      });
      this.StartCoroutine(this.SetInputText());
    }
  }
}
