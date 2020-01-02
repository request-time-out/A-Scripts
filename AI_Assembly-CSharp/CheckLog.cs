// Decompiled with JetBrains decompiler
// Type: CheckLog
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class CheckLog : MonoBehaviour
{
  [SerializeField]
  private RectTransform rtfScroll;
  [SerializeField]
  private RectTransform rtfContent;
  [SerializeField]
  private Text tmpText;
  private ScrollRect scrollR;
  private bool updateNormalizePosition;
  private List<Text> lstText;

  public CheckLog()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.scrollR = (ScrollRect) ((Component) this.rtfScroll).GetComponent<ScrollRect>();
    if (!Object.op_Implicit((Object) this.scrollR))
      return;
    ObservableExtensions.Subscribe<Unit>(Observable.Where<Unit>((IObservable<M0>) ObservableTriggerExtensions.UpdateAsObservable((Component) this.scrollR), (Func<M0, bool>) (_ => this.updateNormalizePosition)), (Action<M0>) (_ =>
    {
      this.updateNormalizePosition = false;
      this.scrollR.set_verticalNormalizedPosition(0.0f);
    }));
  }

  private Text CloneText(string str)
  {
    Text text = (Text) Object.Instantiate<Text>((M0) this.tmpText);
    ((Component) text).get_transform().SetParent(((Component) this.rtfContent).get_transform(), false);
    text.set_text(str);
    ((Graphic) text).get_rectTransform().set_sizeDelta(new Vector2((float) ((Graphic) text).get_rectTransform().get_sizeDelta().x, text.get_preferredHeight()));
    ((Component) text).get_gameObject().SetActive(true);
    this.updateNormalizePosition = true;
    return text;
  }

  public int AddLog(string format, params object[] args)
  {
    string str = format;
    for (int index = 0; index < args.Length; ++index)
      str = str.Replace("{" + (object) index + "}", args[index].ToString());
    this.lstText.Add(this.CloneText(str));
    return this.lstText.Count - 1;
  }

  public void UpdateLog(int index, string format, params object[] args)
  {
    if (index >= this.lstText.Count)
      return;
    string str = format;
    for (int index1 = 0; index1 < args.Length; ++index1)
      str = str.Replace("{" + (object) index1 + "}", args[index1].ToString());
    this.lstText[index].set_text(str);
  }

  public int AddLog(Color color, string format, params object[] args)
  {
    string str = format;
    for (int index = 0; index < args.Length; ++index)
      str = str.Replace("{" + (object) index + "}", args[index].ToString());
    this.lstText.Add(this.CloneText(string.Format("<color=#{0}>{1}</color>\n", (object) ColorUtility.ToHtmlStringRGBA(color), (object) str)));
    return this.lstText.Count - 1;
  }

  public void UpdateLog(int index, Color color, string format, params object[] args)
  {
    if (index >= this.lstText.Count)
      return;
    string str1 = format;
    for (int index1 = 0; index1 < args.Length; ++index1)
      str1 = str1.Replace("{" + (object) index1 + "}", args[index1].ToString());
    string str2 = string.Format("<color=#{0}>{1}</color>\n", (object) ColorUtility.ToHtmlStringRGBA(color), (object) str1);
    this.lstText[index].set_text(str2);
  }

  public void StartLog()
  {
    for (int index = ((Transform) this.rtfContent).get_childCount() - 1; index >= 0; --index)
      Object.Destroy((Object) ((Component) ((Transform) this.rtfContent).GetChild(index)).get_gameObject());
    Resources.UnloadUnusedAssets();
    this.lstText.Clear();
  }

  public void EndLog()
  {
    Resources.UnloadUnusedAssets();
  }

  private void Update()
  {
  }
}
