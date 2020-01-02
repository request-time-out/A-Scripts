// Decompiled with JetBrains decompiler
// Type: AIProject.Hyphenation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AIProject
{
  public class Hyphenation : UIBehaviour
  {
    private static readonly string RichTextReplace = "(\\<color=.*\\>|</color>)\\<size=.n\\>|</size>|<b>|</b><i>|</i>";
    private static readonly char[] AheadHyphenationSet = ",)]｝、。）〕〉》」』】〙〗〟’”｠»ァィゥェォッャュョヮヵヶっぁぃぅぇぉっゃゅょゎ‐゠–〜ー?!！？‼⁇⁈⁉・:;。.".ToCharArray();
    private static readonly char[] BehindHyphenationSet = "(（[｛〔〈《「『【〘〖〝‘“｟«".ToCharArray();
    private static readonly char[] LatinHyphenationSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789<>=/().,".ToCharArray();
    [SerializeField]
    private string _text;
    private UnityEngine.UI.Text _label;
    private RectTransform _rectTransform;

    public Hyphenation()
    {
      base.\u002Ector();
    }

    public UnityEngine.UI.Text Label
    {
      get
      {
        return this._label ?? (this._label = (UnityEngine.UI.Text) ((Component) this).GetComponent<UnityEngine.UI.Text>());
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this._rectTransform ?? (this._rectTransform = ((Component) this).get_transform() as RectTransform);
      }
    }

    public float TextWidth
    {
      get
      {
        Rect rect = this.RectTransform.get_rect();
        return ((Rect) ref rect).get_width();
      }
      set
      {
        this.RectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, value);
      }
    }

    public int FontSize
    {
      get
      {
        return this._label.get_fontSize();
      }
      set
      {
        this._label.set_fontSize(value);
      }
    }

    public void Set(string t)
    {
      this._text = t;
    }

    private void UpdateText(string str)
    {
      this.Label.set_text(this.GetFormattedText(this.Label, str));
    }

    private bool IsHeightOver(UnityEngine.UI.Text label)
    {
      double preferredHeight = (double) label.get_preferredHeight();
      Rect rect = this.RectTransform.get_rect();
      double height = (double) ((Rect) ref rect).get_height();
      return preferredHeight > height;
    }

    private bool IsLineCountOver(UnityEngine.UI.Text label, int lineCount)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < lineCount; ++index)
        stringBuilder.Append("\n");
      label.set_text(stringBuilder.ToString());
      double preferredHeight = (double) label.get_preferredHeight();
      Rect rect = this.RectTransform.get_rect();
      double height = (double) ((Rect) ref rect).get_height();
      return preferredHeight > height;
    }

    private float GetSpaceWidth(UnityEngine.UI.Text label)
    {
      return this.GetTextWidth(label, "m m") - this.GetTextWidth(label, "mm");
    }

    private float GetTextWidth(UnityEngine.UI.Text label, string message)
    {
      if (this._label.get_supportRichText())
        message = Regex.Replace(message, Hyphenation.RichTextReplace, string.Empty);
      label.set_text(message);
      return label.get_preferredWidth();
    }

    private string GetFormattedText(UnityEngine.UI.Text label, string msg)
    {
      if (msg.IsNullOrEmpty())
        return string.Empty;
      Rect rect = this.RectTransform.get_rect();
      float width = ((Rect) ref rect).get_width();
      float spaceWidth = this.GetSpaceWidth(label);
      label.set_horizontalOverflow((HorizontalWrapMode) 1);
      StringBuilder stringBuilder = new StringBuilder();
      int num1 = 0;
      float num2 = 0.0f;
      string str = "\n";
      bool flag1 = Hyphenation.ExistsBehindHyphenation(msg[0]);
      bool flag2 = false;
      foreach (string word in this.GetWordList(msg))
      {
        float num3 = this.GetTextWidth(label, word);
        num2 += width;
        if (word == str)
        {
          num2 = 0.0f + spaceWidth * 2f;
          ++num1;
        }
        else
        {
          if (word == string.Empty)
            num2 += spaceWidth;
          if (flag1)
          {
            if (!flag2)
              flag2 = this.IsLineCountOver(label, num1 - 1);
            if (flag2)
              num3 = 0.0f;
            if ((double) num2 > (double) width - (double) num3)
            {
              stringBuilder.Append(str);
              stringBuilder.Append("　");
              num2 = this.GetTextWidth(label, word) + spaceWidth * 2f;
              ++num1;
            }
          }
          else if ((double) num2 > (double) width)
          {
            stringBuilder.Append(str);
            num2 = this.GetTextWidth(label, word);
            ++num1;
          }
        }
        stringBuilder.Append(word);
      }
      return stringBuilder.ToString();
    }

    private List<string> GetWordList(string source)
    {
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      char minValue = char.MinValue;
      for (int index = 0; index < source.Length; ++index)
      {
        char c1 = source[index];
        char c2 = index >= source.Length - 1 ? minValue : source[index + 1];
        char c3 = index <= 0 ? minValue : source[index - 1];
        stringBuilder.Append(c1);
        bool flag1 = Hyphenation.ExistsLatin(c1) && Hyphenation.ExistsLatin(c3) && (Hyphenation.ExistsLatin(c2) && !Hyphenation.ExistsLatin(c3));
        bool flag2 = !Hyphenation.ExistsLatin(c1) && Hyphenation.ExistsBehindHyphenation(c3);
        bool flag3 = !Hyphenation.ExistsLatin(c2) && !Hyphenation.ExistsAheadHyphenation(c2) && !Hyphenation.ExistsBehindHyphenation(c1);
        bool flag4 = index == source.Length - 1;
        if (flag1 || flag2 || (flag3 || flag4))
        {
          stringList.Add(stringBuilder.ToString());
          stringBuilder = new StringBuilder();
        }
      }
      return stringList;
    }

    private static bool ExistsAheadHyphenation(char c)
    {
      foreach (int aheadHyphenation in Hyphenation.AheadHyphenationSet)
      {
        if (aheadHyphenation == (int) c)
          return true;
      }
      return false;
    }

    private static bool ExistsBehindHyphenation(char c)
    {
      foreach (int behindHyphenation in Hyphenation.BehindHyphenationSet)
      {
        if (behindHyphenation == (int) c)
          return true;
      }
      return false;
    }

    private static bool ExistsLatin(char c)
    {
      foreach (int latinHyphenation in Hyphenation.LatinHyphenationSet)
      {
        if (latinHyphenation == (int) c)
          return true;
      }
      return false;
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
      base.OnRectTransformDimensionsChange();
      this.UpdateText(this._text);
    }
  }
}
