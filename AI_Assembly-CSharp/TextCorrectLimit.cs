// Decompiled with JetBrains decompiler
// Type: TextCorrectLimit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public static class TextCorrectLimit
{
  public static string CorrectString(Text text, string baseStr, string endStr)
  {
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) ((Component) text).get_gameObject(), (Transform) null, false);
    Text component = (Text) gameObject.GetComponent<Text>();
    float spaceWidth = TextCorrectLimit.GetSpaceWidth(component);
    int length1 = endStr.Length;
    float num1 = 0.0f;
    for (int startIndex = 0; startIndex < length1; ++startIndex)
      num1 += endStr[startIndex] != ' ' ? TextCorrectLimit.GetTextWidth(component, endStr.Substring(startIndex, 1)) : spaceWidth;
    Rect rect = ((Graphic) text).get_rectTransform().get_rect();
    float num2 = ((Rect) ref rect).get_width() - num1;
    int length2 = baseStr.Length;
    int length3 = 0;
    float num3 = 0.0f;
    for (int startIndex = 0; startIndex < length2; ++startIndex)
    {
      num3 += baseStr[startIndex] != ' ' ? TextCorrectLimit.GetTextWidth(component, baseStr.Substring(startIndex, 1)) : spaceWidth;
      if ((double) num3 < (double) num2)
        ++length3;
      else
        break;
    }
    Object.Destroy((Object) gameObject);
    return baseStr.Substring(0, length3) + (length3 != length2 ? endStr : string.Empty);
  }

  public static void Correct(Text text, string baseStr, string endStr)
  {
    text.set_text(TextCorrectLimit.CorrectString(text, baseStr, endStr));
  }

  private static float GetSpaceWidth(Text textComp)
  {
    return TextCorrectLimit.GetTextWidth(textComp, "m m") - TextCorrectLimit.GetTextWidth(textComp, "mm");
  }

  private static float GetTextWidth(Text textComp, string message)
  {
    textComp.set_text(message);
    return textComp.get_preferredWidth();
  }
}
