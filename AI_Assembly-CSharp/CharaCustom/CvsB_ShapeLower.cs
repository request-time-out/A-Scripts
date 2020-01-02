// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_ShapeLower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_ShapeLower : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssWaistY;
    [SerializeField]
    private CustomSliderSet ssWaistUpW;
    [SerializeField]
    private CustomSliderSet ssWaistUpZ;
    [SerializeField]
    private CustomSliderSet ssWaistLowW;
    [SerializeField]
    private CustomSliderSet ssWaistLowZ;
    [SerializeField]
    private CustomSliderSet ssHip;
    [SerializeField]
    private CustomSliderSet ssHipRotX;
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
      return (IEnumerator) new CvsB_ShapeLower.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[7]
      {
        18,
        19,
        20,
        21,
        22,
        23,
        24
      };
      this.ssShape = new CustomSliderSet[7]
      {
        this.ssWaistY,
        this.ssWaistUpW,
        this.ssWaistUpZ,
        this.ssWaistLowW,
        this.ssWaistLowZ,
        this.ssHip,
        this.ssHipRotX
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBodyShapeWhole += new Action(((CvsBase) this).UpdateCustomUI);
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
