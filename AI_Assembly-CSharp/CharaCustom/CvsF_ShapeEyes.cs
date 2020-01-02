// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsF_ShapeEyes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsF_ShapeEyes : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssEyeY;
    [SerializeField]
    private CustomSliderSet ssEyeX;
    [SerializeField]
    private CustomSliderSet ssEyeZ;
    [SerializeField]
    private CustomSliderSet ssEyeW;
    [SerializeField]
    private CustomSliderSet ssEyeH;
    [SerializeField]
    private CustomSliderSet ssEyeRotZ;
    [SerializeField]
    private CustomSliderSet ssEyeRotY;
    [SerializeField]
    private CustomSliderSet ssEyeInX;
    [SerializeField]
    private CustomSliderSet ssEyeOutX;
    [SerializeField]
    private CustomSliderSet ssEyeInY;
    [SerializeField]
    private CustomSliderSet ssEyeOutY;
    [SerializeField]
    private CustomSliderSet ssEyelidForm01;
    [SerializeField]
    private CustomSliderSet ssEyelidForm02;
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
      return (IEnumerator) new CvsF_ShapeEyes.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[13]
      {
        19,
        20,
        21,
        22,
        23,
        24,
        25,
        26,
        27,
        28,
        29,
        30,
        31
      };
      this.ssShape = new CustomSliderSet[13]
      {
        this.ssEyeY,
        this.ssEyeX,
        this.ssEyeZ,
        this.ssEyeW,
        this.ssEyeH,
        this.ssEyeRotZ,
        this.ssEyeRotY,
        this.ssEyeInX,
        this.ssEyeOutX,
        this.ssEyeInY,
        this.ssEyeOutY,
        this.ssEyelidForm01,
        this.ssEyelidForm02
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsFaceShapeEyes += new Action(((CvsBase) this).UpdateCustomUI);
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
