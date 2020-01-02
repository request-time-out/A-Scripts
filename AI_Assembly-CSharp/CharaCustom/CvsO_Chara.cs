// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_Chara
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsO_Chara : CvsBase
  {
    private ChaRandomName randomName = new ChaRandomName();
    [SerializeField]
    private InputField inpName;
    [SerializeField]
    private Button btnRandom;
    [SerializeField]
    private Dropdown ddBirthMonth;
    [SerializeField]
    private Dropdown ddBirthDay;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    private void CalculateUI()
    {
      this.ddBirthMonth.set_value((int) this.parameter.birthMonth - 1);
      this.ddBirthDay.set_value((int) this.parameter.birthDay - 1);
    }

    public override void UpdateCustomUI()
    {
      base.UpdateCustomUI();
      this.CalculateUI();
      this.inpName.set_text(this.parameter.fullname);
    }

    public void UpdateBirthDayDD()
    {
      int num = (int) this.parameter.birthDay - 1;
      this.ddBirthDay.ClearOptions();
      int[] numArray = new int[12]
      {
        31,
        29,
        31,
        30,
        31,
        30,
        31,
        31,
        30,
        31,
        30,
        31
      };
      List<string> stringList = new List<string>();
      for (int index = 0; index < numArray[(int) this.parameter.birthMonth - 1]; ++index)
        stringList.Add((index + 1).ToString());
      this.ddBirthDay.AddOptions(stringList);
      if (num > numArray[(int) this.parameter.birthMonth - 1] - 1)
        this.ddBirthDay.set_value(0);
      else
        this.ddBirthDay.set_value(num);
      this.parameter.birthDay = (byte) (this.ddBirthDay.get_value() + 1);
    }

    protected override void Start()
    {
      base.Start();
      this.UpdateBirthDayDD();
      this.customBase.lstInputField.Add(this.inpName);
      this.customBase.actUpdateCvsChara += new Action(((CvsBase) this).UpdateCustomUI);
      if (Object.op_Implicit((Object) this.inpName))
        this.inpName.ActivateInputField();
      if (Object.op_Implicit((Object) this.inpName))
        ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this.inpName), (Action<M0>) (str =>
        {
          this.parameter.fullname = str;
          this.customBase.changeCharaName = true;
        }));
      this.randomName.Initialize();
      if (Object.op_Implicit((Object) this.btnRandom))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnRandom), (Action<M0>) (_ =>
        {
          this.inpName.set_text(this.randomName.GetRandName(this.chaCtrl.sex));
          this.parameter.fullname = this.inpName.get_text();
          this.customBase.changeCharaName = true;
        }));
      // ISSUE: method pointer
      ((UnityEvent<int>) this.ddBirthMonth.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(\u003CStart\u003Em__2)));
      // ISSUE: method pointer
      ((UnityEvent<int>) this.ddBirthDay.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(\u003CStart\u003Em__3)));
    }
  }
}
