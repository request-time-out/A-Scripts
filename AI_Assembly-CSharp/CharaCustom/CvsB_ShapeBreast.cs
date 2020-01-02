// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsB_ShapeBreast
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace CharaCustom
{
  public class CvsB_ShapeBreast : CvsBase
  {
    [SerializeField]
    private CustomSliderSet ssBustSize;
    [SerializeField]
    private CustomSliderSet ssBustY;
    [SerializeField]
    private CustomSliderSet ssBustRotX;
    [SerializeField]
    private CustomSliderSet ssBustX;
    [SerializeField]
    private CustomSliderSet ssBustRotY;
    [SerializeField]
    private CustomSliderSet ssBustSharp;
    [SerializeField]
    private CustomSliderSet ssAreolaBulge;
    [SerializeField]
    private CustomSliderSet ssNipWeight;
    [SerializeField]
    private CustomSliderSet ssNipStand;
    [SerializeField]
    private CustomSliderSet ssBustSoftness;
    [SerializeField]
    private CustomSliderSet ssBustWeight;
    [SerializeField]
    private CustomSliderSet ssAreolaSize;
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
      this.ssBustSoftness.SetSliderValue(this.body.bustSoftness);
      this.ssBustWeight.SetSliderValue(this.body.bustWeight);
      this.ssAreolaSize.SetSliderValue(this.body.areolaSize);
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
      return (IEnumerator) new CvsB_ShapeBreast.\u003CSetInputText\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    public void Awake()
    {
      this.shapeIdx = new int[9]
      {
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        32
      };
      this.ssShape = new CustomSliderSet[9]
      {
        this.ssBustSize,
        this.ssBustY,
        this.ssBustRotX,
        this.ssBustX,
        this.ssBustRotY,
        this.ssBustSharp,
        this.ssAreolaBulge,
        this.ssNipWeight,
        this.ssNipStand
      };
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsBodyShapeBreast += new Action(((CvsBase) this).UpdateCustomUI);
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
      this.ssBustSoftness.onChange = (Action<float>) (value =>
      {
        this.body.bustSoftness = value;
        this.chaCtrl.UpdateBustSoftness();
      });
      this.ssBustSoftness.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.bustSoftness);
      this.ssBustWeight.onChange = (Action<float>) (value =>
      {
        this.body.bustWeight = value;
        this.chaCtrl.UpdateBustGravity();
      });
      this.ssBustWeight.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.bustWeight);
      this.ssAreolaSize.onChange = (Action<float>) (value =>
      {
        this.body.areolaSize = value;
        this.chaCtrl.ChangeNipScale();
      });
      this.ssAreolaSize.onSetDefaultValue = (Func<float>) (() => this.defChaCtrl.custom.body.areolaSize);
      this.StartCoroutine(this.SetInputText());
    }
  }
}
