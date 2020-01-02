// Decompiled with JetBrains decompiler
// Type: CharaCustom.CvsO_SubmenuEx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CvsO_SubmenuEx : MonoBehaviour
  {
    [SerializeField]
    private CvsO_Fusion coFusion;
    [SerializeField]
    private Button btnFusion;
    [SerializeField]
    private Button btnConfig;
    [SerializeField]
    private Button btnDrawMenu;
    [SerializeField]
    private Button btnDefaultLayout;
    [SerializeField]
    private Button btnUpdatePng;
    [SerializeField]
    private CustomUIDrag[] customUIDrags;

    public CvsO_SubmenuEx()
    {
      base.\u002Ector();
    }

    protected CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private void Start()
    {
      if (Object.op_Implicit((Object) this.btnFusion))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnFusion), (Action<M0>) (_ =>
        {
          this.coFusion.UpdateCharasList();
          this.customBase.customCtrl.showFusionCvs = true;
        }));
      if (Object.op_Implicit((Object) this.btnConfig))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnConfig), (Action<M0>) (_ =>
        {
          if (Object.op_Equality((Object) null, (Object) Singleton<Game>.Instance.Config))
            Singleton<Game>.Instance.LoadConfig();
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        }));
      if (Object.op_Implicit((Object) this.btnDrawMenu))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnDrawMenu), (Action<M0>) (_ => this.customBase.customCtrl.showDrawMenu = true));
      if (Object.op_Implicit((Object) this.btnDefaultLayout))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnDefaultLayout), (Action<M0>) (_ =>
        {
          this.customBase.customSettingSave.ResetWinLayout();
          if (this.customUIDrags == null)
            return;
          for (int index = 0; index < this.customUIDrags.Length; ++index)
            this.customUIDrags[index].UpdatePosition();
        }));
      if (!Object.op_Implicit((Object) this.btnUpdatePng))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnUpdatePng), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
        this.customBase.customCtrl.updatePng = true;
      }));
    }
  }
}
