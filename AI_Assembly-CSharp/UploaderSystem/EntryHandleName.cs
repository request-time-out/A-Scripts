// Decompiled with JetBrains decompiler
// Type: UploaderSystem.EntryHandleName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UploaderSystem
{
  public class EntryHandleName : MonoBehaviour
  {
    public string backSceneName;
    [SerializeField]
    private Canvas cvsChangeScene;
    [SerializeField]
    private InputField inpHandleName;
    [SerializeField]
    private Button btnYes;
    [SerializeField]
    private Button btnNo;
    private string handleName;
    private bool notIllusion;
    public Action onEnd;

    public EntryHandleName()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.handleName = Singleton<GameSystem>.Instance.HandleName;
      this.inpHandleName.set_text(this.handleName);
      this.inpHandleName.ActivateInputField();
      ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this.inpHandleName), (Action<M0>) (buf =>
      {
        this.notIllusion = !(buf == "イリュージョン公式");
        this.handleName = buf;
      }));
      if (Object.op_Implicit((Object) this.btnYes))
      {
        TextMeshProUGUI text = (TextMeshProUGUI) ((Component) this.btnYes).GetComponentInChildren<TextMeshProUGUI>(true);
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.btnYes), (Action<M0>) (_ =>
        {
          bool flag = !this.handleName.IsNullOrEmpty() && this.notIllusion;
          ((Selectable) this.btnYes).set_interactable(flag);
          if (!Object.op_Implicit((Object) text))
            return;
          ((Graphic) text).set_color(new Color((float) ((Graphic) text).get_color().r, (float) ((Graphic) text).get_color().g, (float) ((Graphic) text).get_color().b, !flag ? 0.5f : 1f));
        }));
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnYes), (Action<M0>) (_ =>
        {
          Singleton<GameSystem>.Instance.SaveHandleName(this.handleName);
          Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_S);
          ((Component) this.cvsChangeScene).get_gameObject().SetActive(true);
          if ("Uploader" == this.backSceneName || "Downloader" == this.backSceneName)
            Singleton<Scene>.Instance.UnLoad();
          else
            Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
            {
              levelName = "NetworkCheckScene",
              isAdd = false,
              isFade = true,
              isAsync = true
            }, true);
        }));
      }
      if (!Object.op_Implicit((Object) this.btnNo))
        return;
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnNo), (Action<M0>) (_ =>
      {
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        if ("Uploader" == this.backSceneName || "Downloader" == this.backSceneName)
          Singleton<Scene>.Instance.UnLoad();
        else
          Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
          {
            levelName = "Title",
            isFade = true
          }, false);
      }));
    }

    private void OnDestroy()
    {
      if (this.onEnd == null)
        return;
      this.onEnd();
    }
  }
}
