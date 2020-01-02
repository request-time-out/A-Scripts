// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_CharaLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharaCustom
{
  public class CvsO_CharaLoad : CvsBase
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
      this.charaLoadWin.UpdateWindow(this.customBase.modeNew, (int) this.customBase.modeSex, false, (List<CustomCharaFileInfo>) null);
    }

    protected override void Start()
    {
      base.Start();
      this.customBase.actUpdateCvsCharaLoad += new Action(this.UpdateCharasList);
      this.UpdateCharasList();
      this.charaLoadWin.btnDisableNotSelect01 = true;
      this.charaLoadWin.btnDisableNotSelect02 = true;
      this.charaLoadWin.btnDisableNotSelect03 = true;
      this.charaLoadWin.onClick03 = (Action<CustomCharaFileInfo, int>) ((info, flags) =>
      {
        bool face = 0 != (flags & 1);
        bool body = 0 != (flags & 2);
        bool hair = 0 != (flags & 4);
        bool coordinate = 0 != (flags & 8);
        bool parameter = (flags & 16) != 0 && this.customBase.modeNew;
        this.chaCtrl.chaFile.LoadFileLimited(info.FullPath, this.chaCtrl.sex, face, body, hair, parameter, coordinate);
        this.chaCtrl.ChangeNowCoordinate(false, true);
        Singleton<Character>.Instance.customLoadGCClear = false;
        this.chaCtrl.Reload(!coordinate, !face, !hair, !body, true);
        Singleton<Character>.Instance.customLoadGCClear = true;
        this.customBase.updateCustomUI = true;
        for (int slotNo = 0; slotNo < 20; ++slotNo)
          this.customBase.ChangeAcsSlotName(slotNo);
        this.customBase.SetUpdateToggleSetting();
        this.customBase.forceUpdateAcsList = true;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Load);
      });
    }
  }
}
