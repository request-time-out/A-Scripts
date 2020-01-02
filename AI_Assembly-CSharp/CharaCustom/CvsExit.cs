// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsExit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using AIProject;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsExit : MonoBehaviour
  {
    [SerializeField]
    private Button btnExit;
    [SerializeField]
    private PopupCheck popupEndNew;
    [SerializeField]
    private PopupCheck popupEndEdit;

    public CvsExit()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    private void Start()
    {
      if (!Object.op_Implicit((Object) this.btnExit))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnExit), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        if (this.customBase.modeNew)
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.popupEndNew.actYes = (Action) (() => this.ExitScene(false));
          this.popupEndNew.SetupWindow(string.Empty, string.Empty, string.Empty, string.Empty);
        }
        else
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          this.popupEndEdit.actYes = (Action) (() => this.ExitScene(true));
          this.popupEndEdit.actYes2 = (Action) (() => this.ExitScene(false));
          this.popupEndEdit.SetupWindow(string.Empty, string.Empty, string.Empty, string.Empty);
        }
      }));
    }

    public void ExitScene(bool saveChara)
    {
      if (saveChara)
        this.chaCtrl.chaFile.SaveCharaFile(this.customBase.editSaveFileName, byte.MaxValue, false);
      ((Component) this.customBase.customCtrl.cvsChangeScene).get_gameObject().SetActive(true);
      this.customBase.customSettingSave.Save();
      if (this.customBase.nextSceneName.IsNullOrEmpty())
        Singleton<Scene>.Instance.UnLoad();
      else
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = this.customBase.nextSceneName,
          isAdd = false,
          isFade = true,
          isAsync = true,
          isDrawProgressBar = false
        }, false);
    }
  }
}
