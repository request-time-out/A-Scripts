// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_ShapeNose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_ShapeNose : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssNoseAllY;
    [SerializeField]
    private CustomSliderSet ssNoseAllZ;
    [SerializeField]
    private CustomSliderSet ssNoseAllRotX;
    [SerializeField]
    private CustomSliderSet ssNoseAllW;
    [SerializeField]
    private CustomSliderSet ssNoseBridgeH;
    [SerializeField]
    private CustomSliderSet ssNoseBridgeW;
    [SerializeField]
    private CustomSliderSet ssNoseBridgeForm;
    [SerializeField]
    private CustomSliderSet ssNoseWingW;
    [SerializeField]
    private CustomSliderSet ssNoseWingY;
    [SerializeField]
    private CustomSliderSet ssNoseWingZ;
    [SerializeField]
    private CustomSliderSet ssNoseWingRotX;
    [SerializeField]
    private CustomSliderSet ssNoseWingRotZ;
    [SerializeField]
    private CustomSliderSet ssNoseH;
    [SerializeField]
    private CustomSliderSet ssNoseRotX;
    [SerializeField]
    private CustomSliderSet ssNoseSize;
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
      return (IEnumerator) new CvsF_ShapeNose.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[15]
      {
        32,
        33,
        34,
        35,
        36,
        37,
        38,
        39,
        40,
        41,
        42,
        43,
        44,
        45,
        46
      };
      this.ssShape = new CustomSliderSet[15]
      {
        this.ssNoseAllY,
        this.ssNoseAllZ,
        this.ssNoseAllRotX,
        this.ssNoseAllW,
        this.ssNoseBridgeH,
        this.ssNoseBridgeW,
        this.ssNoseBridgeForm,
        this.ssNoseWingW,
        this.ssNoseWingY,
        this.ssNoseWingZ,
        this.ssNoseWingRotX,
        this.ssNoseWingRotZ,
        this.ssNoseH,
        this.ssNoseRotX,
        this.ssNoseSize
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFaceShapeNose += new Action(((CvsBase) this).UpdateCustomUI);
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
