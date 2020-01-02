// Decompiled with JetBrains decompiler
// Type: HSceneSpriteCategories
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEx;

public class HSceneSpriteCategories : MonoBehaviour
{
  [SerializeField]
  private GameObject[] MainCategory;
  private Button[] btMainCategory;
  private bool[] bMainCategory;
  public bool[] MainCategoryActive;
  [SerializeField]
  private GameObject[] SubCategory;
  private Button[] btSubCategory;
  private bool[] bSubCategory;
  public bool[] SubCategoryActive;
  [SerializeField]
  [Space]
  private float[] MainPosX;
  [SerializeField]
  private float[] SubPosX;
  [SerializeField]
  [Space]
  private float smoothTime;
  private bool canUse;

  public HSceneSpriteCategories()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.MainCategoryActive = new bool[this.MainCategory.Length];
    this.SubCategoryActive = new bool[this.SubCategory.Length];
    for (int index = 0; index < this.MainCategoryActive.Length; ++index)
      this.MainCategoryActive[index] = false;
    for (int index = 0; index < this.SubCategoryActive.Length; ++index)
      this.SubCategoryActive[index] = false;
    PointerEnterTrigger pointerEnterTrigger = (PointerEnterTrigger) null;
    UITrigger.TriggerEvent triggerEvent1 = new UITrigger.TriggerEvent();
    PointerExitTrigger pointerExitTrigger = (PointerExitTrigger) null;
    UITrigger.TriggerEvent triggerEvent2 = new UITrigger.TriggerEvent();
    this.bMainCategory = new bool[this.MainCategory.Length];
    this.btMainCategory = new Button[this.MainCategory.Length];
    for (int index = 0; index < this.MainCategory.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteCategories.\u003CStart\u003Ec__AnonStorey0 startCAnonStorey0 = new HSceneSpriteCategories.\u003CStart\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey0.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey0.no = index;
      pointerEnterTrigger = (PointerEnterTrigger) null;
      UITrigger.TriggerEvent triggerEvent3 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      ((UITrigger) this.MainCategory[startCAnonStorey0.no].get_gameObject().AddComponent<PointerEnterTrigger>()).get_Triggers().Add(triggerEvent3);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent3).AddListener(new UnityAction<BaseEventData>((object) startCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      pointerExitTrigger = (PointerExitTrigger) null;
      UITrigger.TriggerEvent triggerEvent4 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      ((UITrigger) this.MainCategory[startCAnonStorey0.no].get_gameObject().AddComponent<PointerExitTrigger>()).get_Triggers().Add(triggerEvent4);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent4).AddListener(new UnityAction<BaseEventData>((object) startCAnonStorey0, __methodptr(\u003C\u003Em__1)));
      // ISSUE: reference to a compiler-generated field
      this.bMainCategory[startCAnonStorey0.no] = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.btMainCategory[startCAnonStorey0.no] = (Button) this.MainCategory[startCAnonStorey0.no].GetComponentInChildren<Button>();
    }
    this.bSubCategory = new bool[this.SubCategory.Length];
    this.btSubCategory = new Button[this.SubCategory.Length];
    for (int index = 0; index < this.SubCategory.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      HSceneSpriteCategories.\u003CStart\u003Ec__AnonStorey1 startCAnonStorey1 = new HSceneSpriteCategories.\u003CStart\u003Ec__AnonStorey1();
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey1.\u0024this = this;
      // ISSUE: reference to a compiler-generated field
      startCAnonStorey1.no = index;
      pointerEnterTrigger = (PointerEnterTrigger) null;
      UITrigger.TriggerEvent triggerEvent3 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      ((UITrigger) this.SubCategory[startCAnonStorey1.no].get_gameObject().AddComponent<PointerEnterTrigger>()).get_Triggers().Add(triggerEvent3);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent3).AddListener(new UnityAction<BaseEventData>((object) startCAnonStorey1, __methodptr(\u003C\u003Em__0)));
      pointerExitTrigger = (PointerExitTrigger) null;
      UITrigger.TriggerEvent triggerEvent4 = new UITrigger.TriggerEvent();
      // ISSUE: reference to a compiler-generated field
      ((UITrigger) this.SubCategory[startCAnonStorey1.no].get_gameObject().AddComponent<PointerExitTrigger>()).get_Triggers().Add(triggerEvent4);
      // ISSUE: method pointer
      ((UnityEvent<BaseEventData>) triggerEvent4).AddListener(new UnityAction<BaseEventData>((object) startCAnonStorey1, __methodptr(\u003C\u003Em__1)));
      // ISSUE: reference to a compiler-generated field
      this.bSubCategory[startCAnonStorey1.no] = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.btSubCategory[startCAnonStorey1.no] = (Button) this.SubCategory[startCAnonStorey1.no].GetComponentInChildren<Button>();
    }
  }

  private void Update()
  {
    float deltaTime = Time.get_deltaTime();
    Vector3.get_zero();
    float num;
    Vector3 vector3;
    for (int index1 = 0; index1 < this.btMainCategory.Length; ++index1)
    {
      int index2 = index1;
      num = 0.0f;
      RectTransform component = (RectTransform) ((Component) this.btMainCategory[index2]).GetComponent<RectTransform>();
      vector3 = Vector2.op_Implicit(component.get_anchoredPosition());
      vector3.x = this.bMainCategory[index2] || this.MainCategoryActive[index2] ? (__Null) (double) Mathf.SmoothDamp((float) vector3.x, this.MainPosX[0], ref num, this.smoothTime, float.PositiveInfinity, deltaTime) : (__Null) (double) Mathf.SmoothDamp((float) vector3.x, this.MainPosX[1], ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
      component.set_anchoredPosition(Vector2.op_Implicit(vector3));
    }
    for (int index1 = 0; index1 < this.btSubCategory.Length; ++index1)
    {
      int index2 = index1;
      num = 0.0f;
      RectTransform component = (RectTransform) ((Component) this.btSubCategory[index2]).GetComponent<RectTransform>();
      vector3 = Vector2.op_Implicit(component.get_anchoredPosition());
      vector3.x = this.bSubCategory[index2] || this.SubCategoryActive[index2] ? (__Null) (double) Mathf.SmoothDamp((float) vector3.x, this.SubPosX[0], ref num, this.smoothTime, float.PositiveInfinity, deltaTime) : (__Null) (double) Mathf.SmoothDamp((float) vector3.x, this.SubPosX[1], ref num, this.smoothTime, float.PositiveInfinity, deltaTime);
      component.set_anchoredPosition(Vector2.op_Implicit(vector3));
    }
  }

  public void Changebuttonactive(bool val)
  {
    if (this.canUse == val)
      return;
    this.canUse = val;
    for (int index = 0; index < this.btMainCategory.Length; ++index)
    {
      if (!val)
        this.bMainCategory[index] = val;
      if (index <= 2 || Singleton<HSceneManager>.Instance.EventKind != HSceneManager.HEvent.GyakuYobai || Singleton<HSceneFlagCtrl>.Instance.initiative != 2)
        ((Selectable) this.btMainCategory[index]).set_interactable(val);
    }
    for (int index = 0; index < this.btSubCategory.Length; ++index)
    {
      if (!val)
        this.bSubCategory[index] = val;
      ((Selectable) this.btSubCategory[index]).set_interactable(val);
    }
  }

  public void AllForceClose(int mode = 0)
  {
    Vector3 zero = Vector3.get_zero();
    for (int index = 0; index < this.btMainCategory.Length; ++index)
    {
      RectTransform component = (RectTransform) ((Component) this.btMainCategory[index]).GetComponent<RectTransform>();
      zero.x = (__Null) (double) this.MainPosX[1];
      component.set_anchoredPosition(Vector2.op_Implicit(zero));
    }
    for (int index1 = 0; index1 < this.btSubCategory.Length; ++index1)
    {
      int index2 = index1;
      if (mode != 1 || index2 != 1)
      {
        RectTransform component = (RectTransform) ((Component) this.btSubCategory[index2]).GetComponent<RectTransform>();
        zero.x = (__Null) (double) this.SubPosX[1];
        component.set_anchoredPosition(Vector2.op_Implicit(zero));
      }
    }
  }
}
