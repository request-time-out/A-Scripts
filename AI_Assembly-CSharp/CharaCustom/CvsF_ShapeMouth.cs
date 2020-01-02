// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_ShapeMouth
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_ShapeMouth : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssMouthY;
    [SerializeField]
    private CustomSliderSet ssMouthW;
    [SerializeField]
    private CustomSliderSet ssMouthH;
    [SerializeField]
    private CustomSliderSet ssMouthZ;
    [SerializeField]
    private CustomSliderSet ssMouthUpForm;
    [SerializeField]
    private CustomSliderSet ssMouthLowForm;
    [SerializeField]
    private CustomSliderSet ssMouthCornerForm;
    private CustomSliderSet[] ssShape;
    private int[] shapeIdx;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      for (int index = 0; index < this.ssShape.Length; ++index)
        this.ssShape[index].SetSliderValue(this.face.shapeValueFace[this.shapeIdx[index]]);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
    }

    [DebuggerHidden]
    public IEnumerator SetInputText()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CvsF_ShapeMouth.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[7]
      {
        47,
        48,
        49,
        50,
        51,
        52,
        53
      };
      this.ssShape = new CustomSliderSet[7]
      {
        this.ssMouthY,
        this.ssMouthW,
        this.ssMouthH,
        this.ssMouthZ,
        this.ssMouthUpForm,
        this.ssMouthLowForm,
        this.ssMouthCornerForm
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFaceShapeMouth += new Action(((CvsBase) this).UpdateCustomUI);
      for (int index = 0; index < this.ssShape.Length; ++index)
      {
        int idx = this.shapeIdx[index];
        this.ssShape[index].onChange = (Action<float>) (value =>
        {
          this.face.shapeValueFace[idx] = value;
          this.chaCtrl.SetShapeFaceValue(idx, value);
        });
        this.ssShape[index].onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.face.shapeValueFace[idx]);
      }
      this.StartCoroutine(this.SetInputText());
    }
  }
}
