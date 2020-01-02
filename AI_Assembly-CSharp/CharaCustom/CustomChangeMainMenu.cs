// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomChangeMainMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace CharaCustom
{
  public class CustomChangeMainMenu : UI_ToggleGroupCtrl
  {
    [SerializeField]
    private CanvasGroup cvgClothesSave;
    [SerializeField]
    private CanvasGroup cvgClothesLoad;
    [SerializeField]
    private CanvasGroup cvgCharaSave;
    [SerializeField]
    private CanvasGroup cvgCharaLoad;
    [SerializeField]
    private CvsA_Slot cvsA_Slot;
    [SerializeField]
    private CvsH_Hair cvsH_Hair;

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    public override void Start()
    {
      base.Start();
      if (!((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Any<UI_ToggleGroupCtrl.ItemInfo>())
        return;
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<UI_ToggleGroupCtrl.ItemInfo>) this.items).Select<UI_ToggleGroupCtrl.ItemInfo, \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<UI_ToggleGroupCtrl.ItemInfo, int, \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>) ((val, idx) => new \u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>(val, idx))).Where<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>((Func<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>, bool>) (item => item.val != null && Object.op_Inequality((Object) item.val.tglItem, (Object) null))).ToList<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>().ForEach((Action<\u003C\u003E__AnonType15<UI_ToggleGroupCtrl.ItemInfo, int>>) (item => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.Skip<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(item.val.tglItem), 1), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        this.ChangeWindowSetting(item.idx);
      }))));
    }

    public bool IsSelectAccessory()
    {
      return this.items[4].tglItem.get_isOn();
    }

    public void ChangeWindowSetting(int no)
    {
      switch (no)
      {
        case 0:
          this.customBase.ChangeClothesStateAuto(0);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = false;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showAcsControllerAll = false;
          this.customBase.showHairController = false;
          break;
        case 1:
          this.customBase.ChangeClothesStateAuto(2);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = false;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showAcsControllerAll = false;
          this.customBase.showHairController = false;
          break;
        case 2:
          this.customBase.ChangeClothesStateAuto(0);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = false;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showAcsControllerAll = false;
          if (!Object.op_Implicit((Object) this.cvsH_Hair) || !Object.op_Implicit((Object) this.customBase.chaCtrl))
            break;
          this.customBase.showHairController = Object.op_Inequality((Object) null, (Object) this.customBase.chaCtrl.cmpHair[this.cvsH_Hair.SNo]);
          break;
        case 3:
          this.customBase.ChangeClothesStateAuto(0);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = (double) this.cvgClothesSave.get_alpha() == 1.0 || (double) this.cvgClothesLoad.get_alpha() == 1.0;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showAcsControllerAll = false;
          this.customBase.showHairController = false;
          break;
        case 4:
          this.customBase.ChangeClothesStateAuto(0);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = false;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showHairController = false;
          if (!Object.op_Implicit((Object) this.cvsA_Slot) || !Object.op_Implicit((Object) this.customBase.chaCtrl))
            break;
          this.customBase.showAcsControllerAll = this.customBase.chaCtrl.IsAccessory(this.cvsA_Slot.SNo);
          break;
        case 5:
          this.customBase.ChangeClothesStateAuto(0);
          this.customBase.customCtrl.showColorCvs = false;
          this.customBase.customCtrl.showFileList = (double) this.cvgCharaSave.get_alpha() == 1.0 || (double) this.cvgCharaLoad.get_alpha() == 1.0;
          this.customBase.customCtrl.showPattern = false;
          this.customBase.showAcsControllerAll = false;
          this.customBase.showHairController = false;
          break;
      }
    }
  }
}
