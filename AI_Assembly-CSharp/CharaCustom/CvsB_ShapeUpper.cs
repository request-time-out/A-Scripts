// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_ShapeUpper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_ShapeUpper : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssNeckW;
    [SerializeField]
    private CustomSliderSet ssNeckZ;
    [SerializeField]
    private CustomSliderSet ssBodyShoulderW;
    [SerializeField]
    private CustomSliderSet ssBodyShoulderZ;
    [SerializeField]
    private CustomSliderSet ssBodyUpW;
    [SerializeField]
    private CustomSliderSet ssBodyUpZ;
    [SerializeField]
    private CustomSliderSet ssBodyLowW;
    [SerializeField]
    private CustomSliderSet ssBodyLowZ;
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
        this.ssShape[index].SetSliderValue(this.body.shapeValueBody[this.shapeIdx[index]]);
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
      return (IEnumerator) new CvsB_ShapeUpper.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[8]
      {
        10,
        11,
        12,
        13,
        14,
        15,
        16,
        17
      };
      this.ssShape = new CustomSliderSet[8]
      {
        this.ssNeckW,
        this.ssNeckZ,
        this.ssBodyShoulderW,
        this.ssBodyShoulderZ,
        this.ssBodyUpW,
        this.ssBodyUpZ,
        this.ssBodyLowW,
        this.ssBodyLowZ
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBodyShapeUpper += new Action(((CvsBase) this).UpdateCustomUI);
      for (int index = 0; index < this.ssShape.Length; ++index)
      {
        int idx = this.shapeIdx[index];
        this.ssShape[index].onChange = (Action<float>) (value =>
        {
          this.body.shapeValueBody[idx] = value;
          this.chaCtrl.SetShapeBodyValue(idx, value);
        });
        this.ssShape[index].onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.shapeValueBody[idx]);
      }
      this.StartCoroutine(this.SetInputText());
    }
  }
}
