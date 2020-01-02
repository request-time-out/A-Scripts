// Decompiled with JetBrains decompiler
// Type: AIProject.UI.CharaEntryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using GameLoadCharaFileSystem;
using Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject.UI
{
  public class CharaEntryUI : MenuUIBehaviour
  {
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameLoadCharaWindow _lcw;
    private MenuUIBehaviour[] _menuUIList;
    private IDisposable _fadeDisposable;

    public MenuUIBehaviour[] MenuUIList
    {
      get
      {
        MenuUIBehaviour[] menuUiList = this._menuUIList;
        if (menuUiList != null)
          return menuUiList;
        return this._menuUIList = new MenuUIBehaviour[1]
        {
          (MenuUIBehaviour) this
        };
      }
    }

    protected override void OnBeforeStart()
    {
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.OnActiveChangedAsObservable(), (Action<M0>) (active => this.SetActiveControl(active)));
      KeyCodeDownCommand keyCodeDownCommand = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 324
      };
      // ISSUE: method pointer
      keyCodeDownCommand.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003COnBeforeStart\u003Em__1)));
      this._keyCommands.Add(keyCodeDownCommand);
      this._lcw.onLoadItemFunc = (Action<GameCharaFileInfo>) (dat =>
      {
        Singleton<Game>.Instance.WorldData.AgentTable[Singleton<Manager.Map>.Instance.AccessDeviceID].CharaFileName = dat.FileName;
        Singleton<Manager.Map>.Instance.Player.PlayerController.ChangeState("CharaEnter");
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        this.Close();
      });
      this._lcw.onClickRightFunc = (Action) null;
      this._lcw.onCloseWindowFunc = (GameLoadCharaWindow.OnCloseWindowFunc) (() =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        this.Close();
      });
      this._lcw.onCharaCreateClickAction = (Action<int>) (sex =>
      {
        if (Singleton<Scene>.Instance.IsNowLoadingFade)
          return;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        CharaCustom.CharaCustom.modeNew = true;
        CharaCustom.CharaCustom.modeSex = (byte) 1;
        CharaCustom.CharaCustom.actEixt = (Action) null;
        CharaCustom.CharaCustom.nextScene = Singleton<Resources>.Instance.DefinePack.SceneNames.MapScene;
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "CharaCustom",
          isAdd = false,
          isFade = true
        }, false);
      });
    }

    private void Close()
    {
      this.IsActiveControl = false;
    }

    private void SetActiveControl(bool active)
    {
      Input instance = Singleton<Input>.Instance;
      IEnumerator coroutine;
      if (active)
      {
        Time.set_timeScale(0.0f);
        this._lcw.useDownload = true;
        List<string> stringList = ListPool<string>.Get();
        WorldData autoData = Singleton<Game>.Instance.Data.AutoData;
        if (autoData != null)
        {
          stringList.Add(autoData.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in autoData.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
        foreach (KeyValuePair<int, WorldData> world in Singleton<Game>.Instance.Data.WorldList)
        {
          stringList.Add(world.Value.PlayerData.CharaFileName);
          foreach (KeyValuePair<int, AgentData> keyValuePair in world.Value.AgentTable)
            stringList.Add(keyValuePair.Value.CharaFileName);
        }
        this._lcw.ReCreateList(true, false);
        instance.FocusLevel = 0;
        instance.MenuElements = this.MenuUIList;
        coroutine = this.OpenCoroutine();
      }
      else
      {
        instance.ClearMenuElements();
        instance.FocusLevel = -1;
        coroutine = this.CloseCoroutine();
      }
      if (this._fadeDisposable != null)
        this._fadeDisposable.Dispose();
      this._fadeDisposable = ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.FromCoroutine((Func<IEnumerator>) (() => coroutine), false), (Action<M0>) (_ => {}), (Action<Exception>) (ex => Debug.LogException(ex)));
    }

    [DebuggerHidden]
    private IEnumerator OpenCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaEntryUI.\u003COpenCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    [DebuggerHidden]
    private IEnumerator CloseCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new CharaEntryUI.\u003CCloseCoroutine\u003Ec__Iterator1()
      {
        \u0024this = this
      };
    }
  }
}
