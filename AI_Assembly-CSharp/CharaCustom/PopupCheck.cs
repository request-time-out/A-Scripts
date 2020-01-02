// Decompiled with JetBrains decompiler
// Type: CharaCustom.PopupCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Illusion.Extensions;
using Manager;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class PopupCheck : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button btnYes;
    [SerializeField]
    private Text textYes;
    [SerializeField]
    private Button btnYes2;
    [SerializeField]
    private Text textYes2;
    [SerializeField]
    private Button btnNo;
    [SerializeField]
    private Text textNo;
    [SerializeField]
    private Text textMsg;
    public Action actYes;
    public Action actYes2;
    public Action actNo;

    public PopupCheck()
    {
      base.\u002Ector();
    }

    public void SetupWindow(string msg = "", string yes = "", string yes2 = "", string no = "")
    {
      if (!msg.IsNullOrEmpty())
        this.textMsg.set_text(msg);
      if (!yes.IsNullOrEmpty())
        this.textYes.set_text(yes);
      if (!yes2.IsNullOrEmpty())
        this.textYes2.set_text(yes2);
      if (!no.IsNullOrEmpty())
        this.textNo.set_text(no);
      this.canvasGroup.Enable(true, false);
    }

    private void Start()
    {
      if (Object.op_Implicit((Object) this.btnYes))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnYes), (Action<M0>) (_ =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
          if (this.actYes != null)
            this.actYes();
          this.canvasGroup.Enable(false, false);
        }));
      if (Object.op_Implicit((Object) this.btnYes2))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnYes2), (Action<M0>) (_ =>
        {
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
          if (this.actYes2 != null)
            this.actYes2();
          this.canvasGroup.Enable(false, false);
        }));
      if (!Object.op_Implicit((Object) this.btnNo))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNo), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        if (this.actNo != null)
          this.actNo();
        this.canvasGroup.Enable(false, false);
      }));
    }

    private void Update()
    {
      if ((double) this.canvasGroup.get_alpha() != 1.0 || !Input.GetMouseButtonDown(1))
        return;
      Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
      if (this.actNo != null)
        this.actNo();
      this.canvasGroup.Enable(false, false);
    }
  }
}
