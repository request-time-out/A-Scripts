// Decompiled with JetBrains decompiler
// Type: TestEntryCustom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using CharaCustom;
using Illusion.Extensions;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TestEntryCustom : BaseLoader
{
  [SerializeField]
  private Button btnNewMale;
  [SerializeField]
  private Button btnNewFemale;
  [SerializeField]
  private Button btnEditMale;
  [SerializeField]
  private Button btnEditFemale;
  [SerializeField]
  private GameObject objCharaSelect;
  [SerializeField]
  private CustomCharaWindow charaLoadWin;
  [SerializeField]
  private Button btnUploader;
  [SerializeField]
  private Button btnDownloader;

  public void UpdateCharasList(bool modeNew, int modeSex)
  {
    this.charaLoadWin.UpdateWindow(modeNew, modeSex, false, (List<CustomCharaFileInfo>) null);
  }

  private void Start()
  {
    this.charaLoadWin.btnDisableNotSelect01 = true;
    this.charaLoadWin.btnDisableNotSelect02 = true;
    this.charaLoadWin.btnDisableNotSelect03 = true;
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNewMale), (Action<M0>) (_ =>
    {
      CharaCustom.CharaCustom.modeNew = true;
      CharaCustom.CharaCustom.modeSex = (byte) 0;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "CharaCustom",
        isAdd = false,
        isFade = true,
        isAsync = true,
        isDrawProgressBar = false
      }, false);
    }));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNewFemale), (Action<M0>) (_ =>
    {
      CharaCustom.CharaCustom.modeNew = true;
      CharaCustom.CharaCustom.modeSex = (byte) 1;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "CharaCustom",
        isAdd = false,
        isFade = true,
        isAsync = true,
        isDrawProgressBar = false
      }, false);
    }));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnEditMale), (Action<M0>) (_ =>
    {
      CharaCustom.CharaCustom.modeNew = false;
      CharaCustom.CharaCustom.modeSex = (byte) 0;
      this.objCharaSelect.SetActiveIfDifferent(true);
      this.UpdateCharasList(false, 0);
    }));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnEditFemale), (Action<M0>) (_ =>
    {
      CharaCustom.CharaCustom.modeNew = false;
      CharaCustom.CharaCustom.modeSex = (byte) 1;
      this.objCharaSelect.SetActiveIfDifferent(true);
      this.UpdateCharasList(false, 1);
    }));
    this.charaLoadWin.onClick03 = (Action<CustomCharaFileInfo, int>) ((info, flags) =>
    {
      CharaCustom.CharaCustom.editCharaFileName = info.FileName;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "CharaCustom",
        isAdd = false,
        isFade = true,
        isAsync = true,
        isDrawProgressBar = false
      }, false);
    });
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnUploader), (Action<M0>) (_ =>
    {
      bool flag = !Singleton<GameSystem>.Instance.HandleName.IsNullOrEmpty();
      Singleton<GameSystem>.Instance.networkSceneName = "Uploader";
      if (flag)
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "NetworkCheckScene",
          isAdd = false,
          isFade = true,
          isAsync = true
        }, true);
      else
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "EntryHandleName",
          isFade = true
        }, false);
    }));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnDownloader), (Action<M0>) (_ =>
    {
      bool flag = !Singleton<GameSystem>.Instance.HandleName.IsNullOrEmpty();
      Singleton<GameSystem>.Instance.networkSceneName = "Downloader";
      if (flag)
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "NetworkCheckScene",
          isAdd = false,
          isFade = true,
          isAsync = true
        }, true);
      else
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "EntryHandleName",
          isFade = true
        }, false);
    }));
  }
}
