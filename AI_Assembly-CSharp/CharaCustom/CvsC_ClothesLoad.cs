// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsC_ClothesLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using MessagePack;
using System;
using UnityEngine;

namespace CharaCustom
{
  public class CvsC_ClothesLoad : CvsBase
  {
    [SerializeField]
    private CustomClothesWindow clothesLoadWin;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = true;
    }

    public void UpdateClothesList()
    {
      this.clothesLoadWin.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, false);
    }

    protected override void Start()
    {
      this.customBase.actUpdateCvsClothesLoad += new Action(this.UpdateClothesList);
      this.UpdateClothesList();
      this.clothesLoadWin.btnDisableNotSelect01 = true;
      this.clothesLoadWin.btnDisableNotSelect02 = true;
      this.clothesLoadWin.btnDisableNotSelect03 = true;
      this.clothesLoadWin.onClick01 = (Action<CustomClothesFileInfo>) (info =>
      {
        byte[] numArray = MessagePackSerializer.Serialize<ChaFileAccessory>((M0) this.chaCtrl.nowCoordinate.accessory);
        this.chaCtrl.nowCoordinate.LoadFile(info.FullPath);
        this.chaCtrl.nowCoordinate.accessory = (ChaFileAccessory) MessagePackSerializer.Deserialize<ChaFileAccessory>(numArray);
        Singleton<Character>.Instance.customLoadGCClear = false;
        this.chaCtrl.Reload(false, true, true, true, true);
        Singleton<Character>.Instance.customLoadGCClear = true;
        this.customBase.updateCustomUI = true;
        this.chaCtrl.AssignCoordinate();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Load);
      });
      this.clothesLoadWin.onClick02 = (Action<CustomClothesFileInfo>) (info =>
      {
        byte[] numArray = MessagePackSerializer.Serialize<ChaFileClothes>((M0) this.chaCtrl.nowCoordinate.clothes);
        this.chaCtrl.nowCoordinate.LoadFile(info.FullPath);
        this.chaCtrl.nowCoordinate.clothes = (ChaFileClothes) MessagePackSerializer.Deserialize<ChaFileClothes>(numArray);
        Singleton<Character>.Instance.customLoadGCClear = false;
        this.chaCtrl.Reload(false, true, true, true, true);
        Singleton<Character>.Instance.customLoadGCClear = true;
        this.customBase.updateCustomUI = true;
        this.customBase.ChangeAcsSlotName(-1);
        this.customBase.forceUpdateAcsList = true;
        this.chaCtrl.AssignCoordinate();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Load);
      });
      this.clothesLoadWin.onClick03 = (Action<CustomClothesFileInfo>) (info =>
      {
        this.chaCtrl.nowCoordinate.LoadFile(info.FullPath);
        Singleton<Character>.Instance.customLoadGCClear = false;
        this.chaCtrl.Reload(false, true, true, true, true);
        Singleton<Character>.Instance.customLoadGCClear = true;
        this.customBase.updateCustomUI = true;
        this.customBase.ChangeAcsSlotName(-1);
        this.customBase.forceUpdateAcsList = true;
        this.chaCtrl.AssignCoordinate();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Load);
      });
    }
  }
}
