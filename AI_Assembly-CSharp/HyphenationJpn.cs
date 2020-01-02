// Decompiled with JetBrains decompiler
// Type: HyphenationJpn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class HyphenationJpn : UIBehaviour
{
  private static readonly string RITCH_TEXT_REPLACE = "(\\<color=.*\\>|</color>|\\<size=.n\\>|</size>|<b>|</b>|<i>|</i>)";
  private static readonly char[] HYP_FRONT = ",)]｝、。）〕〉》」』】〙〗〟’”｠»ァィゥェォッャュョヮヵヶっぁぃぅぇぉっゃゅょゎ‐゠–〜ー?!！？‼⁇⁈⁉・:;。.".ToCharArray();
  private static readonly char[] HYP_BACK = "(（[｛〔〈《「『【〘〖〝‘“｟«".ToCharArray();
  private static readonly char[] HYP_LATIN = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789<>=/().,".ToCharArray();
  [TextArea(3, 10)]
  [SerializeField]
  private string str;
  private RectTransform _rectTransform;
  private UnityEngine.UI.Text _text;

  public HyphenationJpn()
  {
    base.\u002Ector();
  }

  [Conditional("UNITY_EDITOR")]
  private void CheckHeight()
  {
    if (!((UIBehaviour) this.text).IsActive() || !this.IsHeightOver(this.text))
      return;
    Debug.LogWarning((object) this.text.get_text());
  }

  public void SetText(UnityEngine.UI.Text text)
  {
    this._text = text;
  }

  public void SetText(string str)
  {
    this.str = str;
    this.UpdateText(this.str);
  }

  public float textWidth
  {
    get
    {
      Rect rect = this.rectTransform.get_rect();
      return ((Rect) ref rect).get_width();
    }
    set
    {
      this.rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, value);
    }
  }

  public int fontSize
  {
    get
    {
      return this.text.get_fontSize();
    }
    set
    {
      this.text.set_fontSize(value);
    }
  }

  private RectTransform rectTransform
  {
    get
    {
      return ((Component) this).GetComponentCache<RectTransform>(ref this._rectTransform);
    }
  }

  private UnityEngine.UI.Text text
  {
    get
    {
      return ((Component) this).GetComponentCache<UnityEngine.UI.Text>(ref this._text);
    }
  }

  protected virtual void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    this.UpdateText(this.str);
  }

  private void UpdateText(string str)
  {
    this.text.set_text(this.GetFormatedText(this.text, str));
  }

  private bool IsHeightOver(UnityEngine.UI.Text textComp)
  {
    double preferredHeight = (double) textComp.get_preferredHeight();
    Rect rect = this.rectTransform.get_rect();
    double height = (double) ((Rect) ref rect).get_height();
    return preferredHeight > height;
  }

  private bool IsLineCountOver(UnityEngine.UI.Text textComp, int lineCount)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < lineCount; ++index)
      stringBuilder.Append("\n");
    textComp.set_text(stringBuilder.ToString());
    double preferredHeight = (double) textComp.get_preferredHeight();
    Rect rect = this.rectTransform.get_rect();
    double height = (double) ((Rect) ref rect).get_height();
    return preferredHeight > height;
  }

  private float GetSpaceWidth(UnityEngine.UI.Text textComp)
  {
    return this.GetTextWidth(textComp, "m m") - this.GetTextWidth(textComp, "mm");
  }

  private float GetTextWidth(UnityEngine.UI.Text textComp, string message)
  {
    if (this._text.get_supportRichText())
      message = Regex.Replace(message, HyphenationJpn.RITCH_TEXT_REPLACE, string.Empty);
    textComp.set_text(message);
    return textComp.get_preferredWidth();
  }

  private string GetFormatedText(UnityEngine.UI.Text textComp, string msg)
  {
    if (string.IsNullOrEmpty(msg))
      return string.Empty;
    Rect rect = this.rectTransform.get_rect();
    float width = ((Rect) ref rect).get_width();
    float spaceWidth = this.GetSpaceWidth(textComp);
    textComp.set_horizontalOverflow((HorizontalWrapMode) 1);
    StringBuilder stringBuilder = new StringBuilder();
    int num1 = 0;
    float num2 = 0.0f;
    string str = "\n";
    bool flag1 = HyphenationJpn.CHECK_HYP_BACK(msg[0]);
    bool flag2 = false;
    foreach (string word in this.GetWordList(msg))
    {
      float num3 = this.GetTextWidth(textComp, word);
      num2 += num3;
      if (word == str)
      {
        num2 = 0.0f + spaceWidth * 2f;
        ++num1;
      }
      else
      {
        if (word == " ")
          num2 += spaceWidth;
        if (flag1)
        {
          if (!flag2)
            flag2 = this.IsLineCountOver(textComp, num1 + 1);
          if (flag2)
            num3 = 0.0f;
          if ((double) num2 > (double) width - (double) num3)
          {
            stringBuilder.Append(str);
            stringBuilder.Append("　");
            num2 = this.GetTextWidth(textComp, word) + spaceWidth * 2f;
            ++num1;
          }
        }
        else if ((double) num2 > (double) width)
        {
          stringBuilder.Append(str);
          num2 = this.GetTextWidth(textComp, word);
          ++num1;
        }
      }
      stringBuilder.Append(word);
    }
    return stringBuilder.ToString();
  }

  private List<string> GetWordList(string tmpText)
  {
    List<string> stringList = new List<string>();
    StringBuilder stringBuilder = new StringBuilder();
    char minValue = char.MinValue;
    for (int index = 0; index < tmpText.Length; ++index)
    {
      char ch1 = tmpText[index];
      char ch2 = index >= tmpText.Length - 1 ? minValue : tmpText[index + 1];
      char ch3;
      char ch4 = index <= 0 ? minValue : (ch3 = tmpText[index - 1]);
      stringBuilder.Append(ch1);
      if (HyphenationJpn.IsLatin(ch1) && HyphenationJpn.IsLatin(ch4) && (HyphenationJpn.IsLatin(ch1) && !HyphenationJpn.IsLatin(ch4)) || !HyphenationJpn.IsLatin(ch1) && HyphenationJpn.CHECK_HYP_BACK(ch4) || (!HyphenationJpn.IsLatin(ch2) && !HyphenationJpn.CHECK_HYP_FRONT(ch2) && !HyphenationJpn.CHECK_HYP_BACK(ch1) || index == tmpText.Length - 1))
      {
        stringList.Add(stringBuilder.ToString());
        stringBuilder = new StringBuilder();
      }
    }
    return stringList;
  }

  private static bool CHECK_HYP_FRONT(char str)
  {
    return Array.Exists<char>(HyphenationJpn.HYP_FRONT, (Predicate<char>) (item => (int) item == (int) str));
  }

  private static bool CHECK_HYP_BACK(char str)
  {
    return Array.Exists<char>(HyphenationJpn.HYP_BACK, (Predicate<char>) (item => (int) item == (int) str));
  }

  private static bool IsLatin(char s)
  {
    return Array.Exists<char>(HyphenationJpn.HYP_LATIN, (Predicate<char>) (item => (int) item == (int) s));
  }
}
