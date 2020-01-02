// Decompiled with JetBrains decompiler
// Type: ScrollToSelected
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (ScrollRect))]
public class ScrollToSelected : MonoBehaviour
{
  public float scrollSpeed;
  private ScrollRect m_ScrollRect;
  private RectTransform m_RectTransform;
  private RectTransform m_ContentRectTransform;
  private RectTransform m_SelectedRectTransform;

  public ScrollToSelected()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.m_ScrollRect = (ScrollRect) ((Component) this).GetComponent<ScrollRect>();
    this.m_RectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>();
    this.m_ContentRectTransform = this.m_ScrollRect.get_content();
  }

  private void Update()
  {
    this.UpdateScrollToSelected();
  }

  private void UpdateScrollToSelected()
  {
    GameObject selectedGameObject = EventSystem.get_current().get_currentSelectedGameObject();
    if (Object.op_Equality((Object) selectedGameObject, (Object) null) || Object.op_Inequality((Object) selectedGameObject.get_transform().get_parent(), (Object) ((Component) this.m_ContentRectTransform).get_transform()))
      return;
    this.m_SelectedRectTransform = (RectTransform) selectedGameObject.GetComponent<RectTransform>();
    Vector3 vector3 = Vector3.op_Subtraction(((Transform) this.m_RectTransform).get_localPosition(), ((Transform) this.m_SelectedRectTransform).get_localPosition());
    Rect rect1 = this.m_ContentRectTransform.get_rect();
    double height1 = (double) ((Rect) ref rect1).get_height();
    Rect rect2 = this.m_RectTransform.get_rect();
    double height2 = (double) ((Rect) ref rect2).get_height();
    float num1 = (float) (height1 - height2);
    Rect rect3 = this.m_ContentRectTransform.get_rect();
    float num2 = ((Rect) ref rect3).get_height() - (float) vector3.y;
    float num3 = (float) this.m_ScrollRect.get_normalizedPosition().y * num1;
    double num4 = (double) num3;
    Rect rect4 = this.m_SelectedRectTransform.get_rect();
    double num5 = (double) ((Rect) ref rect4).get_height() / 2.0;
    double num6 = num4 - num5;
    Rect rect5 = this.m_RectTransform.get_rect();
    double height3 = (double) ((Rect) ref rect5).get_height();
    float num7 = (float) (num6 + height3);
    double num8 = (double) num3;
    Rect rect6 = this.m_SelectedRectTransform.get_rect();
    double num9 = (double) ((Rect) ref rect6).get_height() / 2.0;
    float num10 = (float) (num8 + num9);
    if ((double) num2 > (double) num7)
    {
      float num11 = num2 - num7;
      this.m_ScrollRect.set_normalizedPosition(Vector2.Lerp(this.m_ScrollRect.get_normalizedPosition(), new Vector2(0.0f, (num3 + num11) / num1), this.scrollSpeed * Time.get_deltaTime()));
    }
    else
    {
      if ((double) num2 >= (double) num10)
        return;
      float num11 = num2 - num10;
      this.m_ScrollRect.set_normalizedPosition(Vector2.Lerp(this.m_ScrollRect.get_normalizedPosition(), new Vector2(0.0f, (num3 + num11) / num1), this.scrollSpeed * Time.get_deltaTime()));
    }
  }
}
