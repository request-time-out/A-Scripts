// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsA_Copy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsA_Copy : CvsBase
  {
    [SerializeField]
    private UI_ToggleEx[] tglSrc;
    [SerializeField]
    private Text[] textSrc;
    [SerializeField]
    private UI_ToggleEx[] tglDst;
    [SerializeField]
    private Text[] textDst;
    [SerializeField]
    private Toggle tglChgParentLR;
    [SerializeField]
    private Button btnCopySlot;
    [SerializeField]
    private Button btnCopy01;
    [SerializeField]
    private Button btnCopy02;
    [SerializeField]
    private Button btnRevLR01;
    [SerializeField]
    private Button btnRevLR02;
    [SerializeField]
    private Button btnRevTB01;
    [SerializeField]
    private Button btnRevTB02;
    private int selSrc;
    private int selDst;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = false;
    }

    public void CalculateUI()
    {
      for (int index = 0; index < 20; ++index)
      {
        ListInfoBase listInfo = this.chaCtrl.lstCtrl.GetListInfo((ChaListDefine.CategoryNo) this.nowAcs.parts[index].type, this.nowAcs.parts[index].id);
        if (listInfo == null)
        {
          this.textDst[index].set_text("なし");
          this.textSrc[index].set_text("なし");
        }
        else
        {
          TextCorrectLimit.Correct(this.textDst[index], listInfo.Name, "…");
          this.textSrc[index].set_text(this.textDst[index].get_text());
        }
      }
    }

    public override void UpdateCustomUI()
    {
      this.CalculateUI();
    }

    private void CopyAccessory()
    {
      this.nowAcs.parts[this.selDst] = (ChaFileAccessory.PartsInfo) MessagePackSerializer.Deserialize<ChaFileAccessory.PartsInfo>(MessagePackSerializer.Serialize<ChaFileAccessory.PartsInfo>((M0) this.nowAcs.parts[this.selSrc]));
      if (this.tglChgParentLR.get_isOn())
      {
        string reverseParent = ChaAccessoryDefine.GetReverseParent(this.nowAcs.parts[this.selDst].parentKey);
        if (string.Empty != reverseParent)
          this.nowAcs.parts[this.selDst].parentKey = reverseParent;
      }
      this.chaCtrl.AssignCoordinate();
      Singleton<Character>.Instance.customLoadGCClear = false;
      this.chaCtrl.Reload(false, true, true, true, true);
      Singleton<Character>.Instance.customLoadGCClear = true;
      this.CalculateUI();
      this.customBase.ChangeAcsSlotName(-1);
      this.customBase.forceUpdateAcsList = true;
      this.customBase.updateCvsAccessory = true;
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsAcsCopy += new Action(((CvsBase) this).UpdateCustomUI);
      this.tglDst[this.selDst].set_isOn(true);
      this.tglSrc[this.selSrc].set_isOn(true);
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<UI_ToggleEx>) this.tglSrc).Select<UI_ToggleEx, \u003C\u003E__AnonType9<UI_ToggleEx, int>>((Func<UI_ToggleEx, int, \u003C\u003E__AnonType9<UI_ToggleEx, int>>) ((p, index) => new \u003C\u003E__AnonType9<UI_ToggleEx, int>(p, index))).ToList<\u003C\u003E__AnonType9<UI_ToggleEx, int>>().ForEach((Action<\u003C\u003E__AnonType9<UI_ToggleEx, int>>) (p => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) p.tgl), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn => this.selSrc = p.index))));
      // ISSUE: object of a compiler-generated type is created
      ((IEnumerable<UI_ToggleEx>) this.tglDst).Select<UI_ToggleEx, \u003C\u003E__AnonType9<UI_ToggleEx, int>>((Func<UI_ToggleEx, int, \u003C\u003E__AnonType9<UI_ToggleEx, int>>) ((p, index) => new \u003C\u003E__AnonType9<UI_ToggleEx, int>(p, index))).ToList<\u003C\u003E__AnonType9<UI_ToggleEx, int>>().ForEach((Action<\u003C\u003E__AnonType9<UI_ToggleEx, int>>) (p => ObservableExtensions.Subscribe<bool>(Observable.Where<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable((Toggle) p.tgl), (Func<M0, bool>) (isOn => isOn)), (Action<M0>) (isOn => this.selDst = p.index))));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCopySlot), (Action<M0>) (_ =>
      {
        this.CopyAccessory();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnCopySlot), (Action<M0>) (_ => ((Selectable) this.btnCopySlot).set_interactable(this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCopy01), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
          this.nowAcs.parts[this.selDst].addMove[0, index] = this.orgAcs.parts[this.selDst].addMove[0, index] = this.nowAcs.parts[this.selSrc].addMove[0, index];
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnCopy01), (Action<M0>) (_ => ((Selectable) this.btnCopy01).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove01) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove01) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnCopy02), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
          this.nowAcs.parts[this.selDst].addMove[1, index] = this.orgAcs.parts[this.selDst].addMove[1, index] = this.nowAcs.parts[this.selSrc].addMove[1, index];
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnCopy02), (Action<M0>) (_ => ((Selectable) this.btnCopy02).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove02) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove02) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnRevLR01), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
        {
          Vector3 vector3 = this.nowAcs.parts[this.selSrc].addMove[0, index];
          if (index == 1)
          {
            ref Vector3 local1 = ref vector3;
            local1.y = (__Null) (local1.y + 180.0);
            if (vector3.y >= 360.0)
            {
              ref Vector3 local2 = ref vector3;
              local2.y = (__Null) (local2.y - 360.0);
            }
          }
          this.nowAcs.parts[this.selDst].addMove[0, index] = this.orgAcs.parts[this.selDst].addMove[0, index] = vector3;
        }
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnRevLR01), (Action<M0>) (_ => ((Selectable) this.btnRevLR01).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove01) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove01) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnRevLR02), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
        {
          Vector3 vector3 = this.nowAcs.parts[this.selSrc].addMove[1, index];
          if (index == 1)
          {
            ref Vector3 local1 = ref vector3;
            local1.y = (__Null) (local1.y + 180.0);
            if (vector3.y >= 360.0)
            {
              ref Vector3 local2 = ref vector3;
              local2.y = (__Null) (local2.y - 360.0);
            }
          }
          this.nowAcs.parts[this.selDst].addMove[1, index] = this.orgAcs.parts[this.selDst].addMove[1, index] = vector3;
        }
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnRevLR02), (Action<M0>) (_ => ((Selectable) this.btnRevLR02).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove02) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove02) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnRevTB01), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
        {
          Vector3 vector3 = this.nowAcs.parts[this.selSrc].addMove[0, index];
          if (index == 1)
          {
            ref Vector3 local1 = ref vector3;
            local1.x = (__Null) (local1.x + 180.0);
            if (vector3.x >= 360.0)
            {
              ref Vector3 local2 = ref vector3;
              local2.x = (__Null) (local2.x - 360.0);
            }
          }
          this.nowAcs.parts[this.selDst].addMove[0, index] = this.orgAcs.parts[this.selDst].addMove[0, index] = vector3;
        }
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnRevTB01), (Action<M0>) (_ => ((Selectable) this.btnRevTB01).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove01) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove01) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnRevTB02), (Action<M0>) (_ =>
      {
        for (int index = 0; index < 3; ++index)
        {
          Vector3 vector3 = this.nowAcs.parts[this.selSrc].addMove[1, index];
          if (index == 1)
          {
            ref Vector3 local1 = ref vector3;
            local1.x = (__Null) (local1.x + 180.0);
            if (vector3.x >= 360.0)
            {
              ref Vector3 local2 = ref vector3;
              local2.x = (__Null) (local2.x - 360.0);
            }
          }
          this.nowAcs.parts[this.selDst].addMove[1, index] = this.orgAcs.parts[this.selDst].addMove[1, index] = vector3;
        }
        this.chaCtrl.UpdateAccessoryMoveFromInfo(this.selDst);
        this.customBase.updateCvsAccessory = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnRevTB02), (Action<M0>) (_ => ((Selectable) this.btnRevTB02).set_interactable(((((true ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selDst].trfMove02) ? 1 : 0))) != 0 ? 1 : 0) & (!Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc]) ? 0 : (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpAccessory[this.selSrc].trfMove02) ? 1 : 0))) != 0 & this.selSrc != this.selDst)));
    }
  }
}
