// Decompiled with JetBrains decompiler
// Type: LogView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class LogView : MonoBehaviour
{
  [SerializeField]
  private Processing processing;
  [SerializeField]
  private Canvas canvasLog;
  [SerializeField]
  private RectTransform rtfScroll;
  [SerializeField]
  private RectTransform rtfContent;
  [SerializeField]
  private UnityEngine.UI.Text textLog;
  [SerializeField]
  private Button btnClose;
  private Dictionary<int, UnityEngine.UI.Text> dictTextLog;
  private StringBuilder sbAdd;
  public Action onClose;

  public LogView()
  {
    base.\u002Ector();
  }

  public bool IsActive
  {
    get
    {
      return this.processing.update;
    }
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) this.btnClose))
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnClose), (Action<M0>) (_ =>
      {
        this.SetActiveCanvas(false);
        if (this.onClose == null)
          return;
        this.onClose();
      }));
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this), (Action<M0>) (_ =>
    {
      if (Object.op_Equality((Object) null, (Object) this.rtfScroll) || Object.op_Equality((Object) null, (Object) this.rtfContent) || (Object.op_Equality((Object) null, (Object) this.textLog) || this.sbAdd.Length == 0))
        return;
      UnityEngine.UI.Text text = (UnityEngine.UI.Text) Object.Instantiate<UnityEngine.UI.Text>((M0) this.textLog);
      ((Component) text).get_transform().SetParent(((Component) this.rtfContent).get_transform(), false);
      text.set_text(this.sbAdd.ToString().TrimEnd('\r', '\n'));
      ((Graphic) text).get_rectTransform().set_sizeDelta(new Vector2((float) ((Graphic) text).get_rectTransform().get_sizeDelta().x, text.get_preferredHeight()));
      ((Component) text).get_gameObject().SetActive(true);
      this.sbAdd.Length = 0;
    }));
  }

  public void SetActiveCanvas(bool active)
  {
    if (!Object.op_Implicit((Object) this.canvasLog))
      return;
    ((Component) this.canvasLog).get_gameObject().SetActive(active);
  }

  public void AddLog(string format, params object[] args)
  {
    string str = format;
    for (int index = 0; index < args.Length; ++index)
      str = str.Replace("{" + (object) index + "}", args[index].ToString());
    this.sbAdd.Append(string.Format("{0}\n", (object) str));
  }

  public void AddLog(Color color, string format, params object[] args)
  {
    string str = format;
    for (int index = 0; index < args.Length; ++index)
      str = str.Replace("{" + (object) index + "}", args[index].ToString());
    this.sbAdd.Append(string.Format("<color=#{0}>{1}</color>\n", (object) ColorUtility.ToHtmlStringRGBA(color), (object) str));
  }

  public void StartLog()
  {
    for (int index = ((Transform) this.rtfContent).get_childCount() - 1; index >= 0; --index)
      Object.Destroy((Object) ((Component) ((Transform) this.rtfContent).GetChild(index)).get_gameObject());
    Resources.UnloadUnusedAssets();
    this.dictTextLog.Clear();
    this.sbAdd.Length = 0;
    this.processing.update = true;
    if (Object.op_Implicit((Object) this.btnClose))
      ((Selectable) this.btnClose).set_interactable(false);
    this.SetActiveCanvas(true);
  }

  public void EndLog()
  {
    this.processing.update = false;
    if (Object.op_Implicit((Object) this.btnClose))
      ((Selectable) this.btnClose).set_interactable(true);
    Resources.UnloadUnusedAssets();
  }

  private void Update()
  {
  }
}
