// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsC_ClothesSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Scene;
using Manager;
using System;
using System.IO;
using UnityEngine;

namespace CharaCustom
{
  public class CvsC_ClothesSave : CvsBase
  {
    [SerializeField]
    private CustomClothesWindow clothesLoadWin;
    [SerializeField]
    private CvsC_ClothesInput clothesNameInput;
    [SerializeField]
    private CvsC_CreateCoordinateFile createCoordinateFile;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = true;
    }

    public void UpdateClothesList()
    {
      this.clothesLoadWin.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, true);
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsClothesSaveDelete += new Action(this.UpdateClothesList);
      this.UpdateClothesList();
      this.clothesLoadWin.btnDisableNotSelect01 = true;
      this.clothesLoadWin.btnDisableNotSelect02 = false;
      this.clothesLoadWin.btnDisableNotSelect03 = true;
      this.clothesLoadWin.onClick01 = (Action<CustomClothesFileInfo>) (info =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        ConfirmScene.Sentence = "本当に削除しますか？";
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          this.clothesLoadWin.SelectInfoClear();
          if (File.Exists(info.FullPath))
            File.Delete(info.FullPath);
          this.customBase.updateCvsClothesSaveDelete = true;
          this.customBase.updateCvsClothesLoad = true;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        });
        ConfirmScene.OnClickedNo = (Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
        Singleton<Game>.Instance.LoadDialog();
      });
      this.clothesLoadWin.onClick02 = (Action<CustomClothesFileInfo>) (info =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        if (!Object.op_Inequality((Object) null, (Object) this.clothesNameInput))
          return;
        this.clothesNameInput.SetupInputCoordinateNameWindow(string.Empty);
        this.clothesNameInput.actEntry = (Action<string>) (buf => this.createCoordinateFile.CreateCoordinateFile(string.Empty, buf, false));
      });
      this.clothesLoadWin.onClick03 = (Action<CustomClothesFileInfo>) (info =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        ConfirmScene.Sentence = "本当に上書きしますか？";
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          if (!Object.op_Inequality((Object) null, (Object) this.clothesNameInput))
            return;
          this.clothesNameInput.SetupInputCoordinateNameWindow(info.name);
          this.clothesNameInput.actEntry = (Action<string>) (buf => this.createCoordinateFile.CreateCoordinateFile(info.FullPath, buf, true));
        });
        ConfirmScene.OnClickedNo = (Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
        Singleton<Game>.Instance.LoadDialog();
      });
    }
  }
}
