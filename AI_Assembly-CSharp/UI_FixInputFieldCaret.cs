// Decompiled with JetBrains decompiler
// Type: UI_FixInputFieldCaret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FixInputFieldCaret : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
  public float correctY;

  public UI_FixInputFieldCaret()
  {
    base.\u002Ector();
  }

  public void OnSelect(BaseEventData eventData)
  {
    InputField component = (InputField) ((Component) this).get_gameObject().GetComponent<InputField>();
    if (!Object.op_Inequality((Object) null, (Object) component))
      return;
    RectTransform rectTransform1 = (RectTransform) null;
    if (Object.op_Implicit((Object) component.get_textComponent()))
      rectTransform1 = ((Component) component.get_textComponent()).get_transform() as RectTransform;
    RectTransform rectTransform2 = (RectTransform) ((Component) this).get_transform().Find(((Object) ((Component) this).get_gameObject()).get_name() + " Input Caret");
    if (!Object.op_Inequality((Object) rectTransform1, (Object) null) || !Object.op_Inequality((Object) rectTransform2, (Object) null))
      return;
    Vector2 anchoredPosition = rectTransform1.get_anchoredPosition();
    ref Vector2 local = ref anchoredPosition;
    local.y = (__Null) (local.y + (double) this.correctY);
    rectTransform2.set_anchoredPosition(anchoredPosition);
  }
}
