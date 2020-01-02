// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_CharaSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using AIProject.Scene;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CharaCustom
{
  public class CvsO_CharaSave : CvsBase
  {
    [SerializeField]
    private CustomCharaWindow charaLoadWin;

    public override void ChangeMenuFunc()
    {
      base.ChangeMenuFunc();
      this.customBase.customCtrl.showColorCvs = false;
      this.customBase.customCtrl.showFileList = true;
    }

    public void UpdateCharasList()
    {
      this.charaLoadWin.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, true, (List<CustomCharaFileInfo>) null);
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsCharaSaveDelete += new Action(this.UpdateCharasList);
      this.UpdateCharasList();
      this.charaLoadWin.btnDisableNotSelect01 = true;
      this.charaLoadWin.btnDisableNotSelect02 = false;
      this.charaLoadWin.btnDisableNotSelect03 = true;
      this.charaLoadWin.onClick01 = (Action<CustomCharaFileInfo>) (info =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        ConfirmScene.Sentence = !info.gameRegistration ? "本当に削除しますか？" : "本当に削除しますか？\n" + "このキャラにはパラメータが含まれています。".Coloring("#DE4529FF").Size(24);
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          this.charaLoadWin.SelectInfoClear();
          if (File.Exists(info.FullPath))
            File.Delete(info.FullPath);
          this.customBase.updateCvsCharaSaveDelete = true;
          this.customBase.updateCvsCharaLoad = true;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        });
        ConfirmScene.OnClickedNo = (Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
        Singleton<Game>.Instance.LoadDialog();
      });
      this.charaLoadWin.onClick02 = (Action<CustomCharaFileInfo>) (info =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        this.customBase.customCtrl.saveMode = true;
      });
      this.charaLoadWin.onClick03 = (Action<CustomCharaFileInfo, int>) ((info, flags) =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        ConfirmScene.Sentence = !info.gameRegistration ? "本当に上書きしますか？" : "本当に上書きしますか？\n" + "上書きするとパラメータは初期化されます。".Coloring("#DE4529FF").Size(24);
        ConfirmScene.OnClickedYes = (Action) (() =>
        {
          this.customBase.customCtrl.overwriteSavePath = info.FullPath;
          this.customBase.customCtrl.saveMode = true;
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        });
        ConfirmScene.OnClickedNo = (Action) (() => Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel));
        Singleton<Game>.Instance.LoadDialog();
      });
    }
  }
}
