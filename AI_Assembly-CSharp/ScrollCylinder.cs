// Decompiled with JetBrains decompiler
// Type: ScrollCylinder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEx;

public class ScrollCylinder : MonoBehaviour
{
  public GameObject Contents;
  [Space]
  public RectTransform Atari;
  [Space]
  public ScrollCylinderNode blankObject;
  [Space]
  [SerializeField]
  private GameObject cursor;
  [SerializeField]
  private float cursorInitPos;
  [SerializeField]
  private float cursorMoveRange;
  private float cursorTime;
  private int cursorMovePtn;
  [SerializeField]
  private float cursorFirstHurfAnimTimeLimit;
  [SerializeField]
  private float cursorLaterHurfAnimTimeLimit;
  [Space]
  public float moveVal;
  [SerializeField]
  private float moveTime;
  [SerializeField]
  private List<ScrollCylinderNode> lstNodes;
  [SerializeField]
  private List<ScrollCylinderNode> lstBlankNodes;
  [SerializeField]
  private ValueTuple<int, Vector2, ScrollCylinderNode> targetNode;
  private Input input;
  private Subject<int> _onValueChange;
  [SerializeField]
  private bool InitList;
  private bool onEnter;
  private GameObject OldRect;
  public bool enableInternalScroll;

  public ScrollCylinder()
  {
    base.\u002Ector();
  }

  public IObservable<int> OnValueChangeAsObservable()
  {
    return (IObservable<int>) this._onValueChange ?? (IObservable<int>) (this._onValueChange = new Subject<int>());
  }

  private void Awake()
  {
    this.OldRect = new GameObject("Rect2");
    this.OldRect.AddComponent<RectTransform>();
    this.OldRect.get_transform().SetParent(((Component) this).get_transform(), false);
    this.OldRect.get_transform().set_localPosition(Vector3.get_zero());
    this.OldRect.SetActive(false);
    GC.Collect();
  }

  private void Start()
  {
    this.input = Singleton<Input>.Instance;
    PointerEnterTrigger pointerEnterTrigger = (PointerEnterTrigger) ((Component) this.Atari).get_gameObject().AddComponent<PointerEnterTrigger>();
    UITrigger.TriggerEvent triggerEvent1 = new UITrigger.TriggerEvent();
    ((UITrigger) pointerEnterTrigger).get_Triggers().Add(triggerEvent1);
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) triggerEvent1).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CStart\u003Em__0)));
    PointerExitTrigger pointerExitTrigger = (PointerExitTrigger) ((Component) this.Atari).get_gameObject().AddComponent<PointerExitTrigger>();
    UITrigger.TriggerEvent triggerEvent2 = new UITrigger.TriggerEvent();
    ((UITrigger) pointerExitTrigger).get_Triggers().Add(triggerEvent2);
    // ISSUE: method pointer
    ((UnityEvent<BaseEventData>) triggerEvent2).AddListener(new UnityAction<BaseEventData>((object) this, __methodptr(\u003CStart\u003Em__1)));
    if (!this.InitList)
      return;
    this.ListNodeSet((List<ScrollCylinderNode>) null);
  }

  public void BlankSet(ScrollCylinderNode Node, bool AddLast = false)
  {
    if (!AddLast)
      this.lstBlankNodes.Clear();
    if (Object.op_Equality((Object) this.blankObject, (Object) null))
      return;
    Vector2 sizeDelta = ((RectTransform) ((Component) Node).GetComponent<RectTransform>()).get_sizeDelta();
    RectTransform component = (RectTransform) ((Component) ((Component) this).get_transform()).GetComponent<RectTransform>();
    int[] numArray1 = new int[2];
    Rect rect1 = component.get_rect();
    numArray1[0] = (int) ((double) ((Rect) ref rect1).get_height() / sizeDelta.y);
    Rect rect2 = component.get_rect();
    numArray1[1] = (int) ((double) ((Rect) ref rect2).get_width() / sizeDelta.x);
    int[] numArray2 = numArray1;
    int num = Mathf.Max(numArray2[0], numArray2[1]);
    if (this.lstBlankNodes.Count == (num % 2 == 0 ? num : num - 1))
      return;
    numArray2[0] /= 2;
    numArray2[1] /= 2;
    for (int index1 = 0; index1 < numArray2.Length; ++index1)
    {
      for (int index2 = 0; index2 < numArray2[index1]; ++index2)
      {
        ScrollCylinderNode scrollCylinderNode = (ScrollCylinderNode) Object.Instantiate<ScrollCylinderNode>((M0) this.blankObject);
        ((Component) scrollCylinderNode).get_transform().SetParent(this.Contents.get_transform(), false);
        ((Component) scrollCylinderNode).get_gameObject().SetActive(true);
        this.lstBlankNodes.Add(scrollCylinderNode);
      }
    }
  }

  public void ClearBlank()
  {
    foreach (Component lstBlankNode in this.lstBlankNodes)
      Object.Destroy((Object) lstBlankNode.get_gameObject());
    this.lstBlankNodes.Clear();
  }

  public void ListNodeSet(List<ScrollCylinderNode> hSceneScrollNodes = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    ScrollCylinder.\u003CListNodeSet\u003Ec__AnonStorey0 nodeSetCAnonStorey0 = new ScrollCylinder.\u003CListNodeSet\u003Ec__AnonStorey0();
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.\u0024this = this;
    this.lstNodes.Clear();
    if (hSceneScrollNodes != null)
      this.lstNodes.AddRange((IEnumerable<ScrollCylinderNode>) hSceneScrollNodes);
    else
      this.lstNodes.AddRange((IEnumerable<ScrollCylinderNode>) this.Contents.GetComponentsInChildren<ScrollCylinderNode>());
    if (this.lstNodes.Count == 0)
      return;
    if (Object.op_Inequality((Object) this.cursor, (Object) null))
      this.cursor.SetActive(this.lstNodes.Count != 0);
    this.BlankSet(this.lstNodes[0], true);
    for (int index = 0; index < this.lstNodes.Count; ++index)
      ((Component) this.lstNodes[index]).get_transform().set_localScale(new Vector3(1f, 1f, 1f));
    for (int index = 0; index < this.lstBlankNodes.Count; ++index)
      ((Component) this.lstBlankNodes[index]).get_transform().set_localScale(new Vector3(1f, 1f, 1f));
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsRt = (RectTransform) this.Contents.GetComponent<RectTransform>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsPosition = Vector2.op_Implicit(nodeSetCAnonStorey0.ContentsRt.get_anchoredPosition());
    if (!this.OldRect.get_activeSelf())
      this.OldRect.SetActive(true);
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.rt2 = (RectTransform) this.OldRect.GetComponent<RectTransform>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.OldRectSet(nodeSetCAnonStorey0.ContentsRt, ref nodeSetCAnonStorey0.rt2);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.position2 = nodeSetCAnonStorey0.ContentsPosition;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsPosition = this.LimitPos(nodeSetCAnonStorey0.ContentsRt, nodeSetCAnonStorey0.ContentsPosition);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsRt.set_anchoredPosition(Vector2.op_Implicit(nodeSetCAnonStorey0.ContentsPosition));
    PointerEnterTrigger pointerEnterTrigger1 = (PointerEnterTrigger) null;
    UITrigger.TriggerEvent triggerEvent1 = new UITrigger.TriggerEvent();
    for (int index1 = 0; index1 < this.lstNodes.Count; ++index1)
    {
      int index2 = index1;
      pointerEnterTrigger1 = (PointerEnterTrigger) null;
      PointerEnterTrigger component = (PointerEnterTrigger) ((Component) this.lstNodes[index2]).get_gameObject().GetComponent<PointerEnterTrigger>();
      ((Component) this.lstNodes[index2]).get_gameObject().SetActive(true);
      if (!Object.op_Inequality((Object) component, (Object) null))
      {
        PointerEnterTrigger pointerEnterTrigger2 = (PointerEnterTrigger) ((Component) this.lstNodes[index2]).get_gameObject().AddComponent<PointerEnterTrigger>();
        UITrigger.TriggerEvent triggerEvent2 = new UITrigger.TriggerEvent();
        ((UITrigger) pointerEnterTrigger2).get_Triggers().Add(triggerEvent2);
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) triggerEvent2).AddListener(new UnityAction<BaseEventData>((object) nodeSetCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }
    // ISSUE: reference to a compiler-generated field
    Rect rect1 = nodeSetCAnonStorey0.ContentsRt.get_rect();
    if ((double) ((Rect) ref rect1).get_width() != 0.0)
    {
      // ISSUE: reference to a compiler-generated field
      Rect rect2 = nodeSetCAnonStorey0.ContentsRt.get_rect();
      if ((double) ((Rect) ref rect2).get_height() != 0.0)
      {
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) new Action<Unit>(nodeSetCAnonStorey0.\u003C\u003Em__2));
        return;
      }
    }
    // ISSUE: reference to a compiler-generated method
    ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) new Action<Unit>(nodeSetCAnonStorey0.\u003C\u003Em__1));
  }

  private void Update()
  {
    if (this.lstNodes.Count == 0)
      return;
    this.MoveContents();
    this.ChangeNode();
  }

  private void MoveContents()
  {
    RectTransform component1 = (RectTransform) this.Contents.GetComponent<RectTransform>();
    Vector3 ContentsPosition = Vector2.op_Implicit(component1.get_anchoredPosition());
    if (this.enableInternalScroll)
    {
      float num = this.input.ScrollValue();
      if (this.onEnter)
      {
        if ((double) num < 0.0 && this.targetNode.Item1 < this.lstNodes.Count - 1)
        {
          int index = this.targetNode.Item1 + 1;
          RectTransform component2 = (RectTransform) ((Component) this.lstNodes[index]).GetComponent<RectTransform>();
          Vector2 vector2;
          vector2.x = (__Null) (component1.get_anchoredPosition().x - component1.get_sizeDelta().x / 2.0 + component2.get_anchoredPosition().x);
          vector2.y = (__Null) (component1.get_anchoredPosition().y + component1.get_sizeDelta().y / 2.0 + component2.get_anchoredPosition().y);
          vector2.x = ContentsPosition.x - vector2.x;
          vector2.y = ContentsPosition.y - vector2.y;
          this.targetNode = new ValueTuple<int, Vector2, ScrollCylinderNode>(index, vector2, this.lstNodes[index]);
          if (this._onValueChange != null)
            this._onValueChange.OnNext(index);
        }
        else if ((double) num > 0.0 && this.targetNode.Item1 > 0)
        {
          int index = this.targetNode.Item1 - 1;
          RectTransform component2 = (RectTransform) ((Component) this.lstNodes[index]).GetComponent<RectTransform>();
          Vector2 vector2;
          vector2.x = (__Null) (component1.get_anchoredPosition().x - component1.get_sizeDelta().x / 2.0 + component2.get_anchoredPosition().x);
          vector2.y = (__Null) (component1.get_anchoredPosition().y + component1.get_sizeDelta().y / 2.0 + component2.get_anchoredPosition().y);
          vector2.x = ContentsPosition.x - vector2.x;
          vector2.y = ContentsPosition.y - vector2.y;
          this.targetNode = new ValueTuple<int, Vector2, ScrollCylinderNode>(index, vector2, this.lstNodes[index]);
          if (this._onValueChange != null)
            this._onValueChange.OnNext(index);
        }
      }
    }
    Vector2 vector2_1 = (Vector2) this.targetNode.Item2;
    float num1 = 0.0f;
    ContentsPosition.x = (__Null) (double) Mathf.SmoothDamp((float) ContentsPosition.x, (float) vector2_1.x, ref num1, this.moveTime, float.PositiveInfinity, Time.get_deltaTime());
    float num2 = 0.0f;
    ContentsPosition.y = (__Null) (double) Mathf.SmoothDamp((float) ContentsPosition.y, (float) vector2_1.y, ref num2, this.moveTime, float.PositiveInfinity, Time.get_deltaTime());
    ContentsPosition = this.LimitPos(component1, ContentsPosition);
    component1.set_anchoredPosition(Vector2.op_Implicit(ContentsPosition));
  }

  private void ChangeNode()
  {
    float deltaTime = Time.get_deltaTime();
    this.ChangeNodeColor();
    this.ChangeNodeScl();
    if (!Object.op_Inequality((Object) this.cursor, (Object) null))
      return;
    this.CursorMove(deltaTime);
  }

  private void ChangeNodeColor()
  {
    for (int index1 = 0; index1 < this.lstNodes.Count; ++index1)
    {
      int index2 = index1;
      if (index2 == this.targetNode.Item1)
        this.lstNodes[index2].ChangeBGAlpha(0);
      else if (index2 == this.targetNode.Item1 - 1 || index2 == this.targetNode.Item1 + 1)
        this.lstNodes[index2].ChangeBGAlpha(1);
      else if (index2 == this.targetNode.Item1 - 2 || index2 == this.targetNode.Item1 + 2)
        this.lstNodes[index2].ChangeBGAlpha(2);
      else
        this.lstNodes[index2].ChangeBGAlpha(3);
    }
    int num = this.lstBlankNodes.Count / 2;
    for (int index = 0; index < this.lstBlankNodes.Count; ++index)
    {
      if (this.lstNodes.Count == 1)
      {
        if (index == num - 2 || index == num + 1)
          this.lstBlankNodes[index].ChangeBGAlpha(2);
        else if (index == num - 1 || index == num)
          this.lstBlankNodes[index].ChangeBGAlpha(1);
      }
      else if (this.targetNode.Item1 == null)
      {
        if (index == num - 2)
          this.lstBlankNodes[index].ChangeBGAlpha(2);
        else if (index == num - 1)
          this.lstBlankNodes[index].ChangeBGAlpha(1);
        else
          this.lstBlankNodes[index].ChangeBGAlpha(3);
      }
      else if (this.targetNode.Item1 == this.lstNodes.Count - 1)
      {
        if (index == num)
          this.lstBlankNodes[index].ChangeBGAlpha(1);
        else if (index == num + 1)
          this.lstBlankNodes[index].ChangeBGAlpha(2);
        else
          this.lstBlankNodes[index].ChangeBGAlpha(3);
      }
      else
        this.lstBlankNodes[index].ChangeBGAlpha(3);
    }
  }

  private void ChangeNodeScl()
  {
    for (int index1 = 0; index1 < this.lstNodes.Count; ++index1)
    {
      int index2 = index1;
      if (index2 == this.targetNode.Item1)
        this.lstNodes[index2].ChangeScale(0);
      else if (index2 == this.targetNode.Item1 - 1 || index2 == this.targetNode.Item1 + 1)
        this.lstNodes[index2].ChangeScale(1);
      else if (index2 == this.targetNode.Item1 - 2 || index2 == this.targetNode.Item1 + 2)
        this.lstNodes[index2].ChangeScale(2);
      else
        this.lstNodes[index2].ChangeScale(3);
    }
    int num = this.lstBlankNodes.Count / 2;
    for (int index = 0; index < this.lstBlankNodes.Count; ++index)
    {
      if (this.lstNodes.Count == 1)
      {
        if (index == num - 2 || index == num + 1)
          this.lstBlankNodes[index].ChangeScale(2);
        else if (index == num - 1 || index == num)
          this.lstBlankNodes[index].ChangeScale(1);
      }
      else if (this.targetNode.Item1 == null)
      {
        if (index == num - 2)
          this.lstBlankNodes[index].ChangeScale(2);
        else if (index == num - 1)
          this.lstBlankNodes[index].ChangeScale(1);
        else
          this.lstBlankNodes[index].ChangeScale(3);
      }
      else if (this.targetNode.Item1 == this.lstNodes.Count - 1)
      {
        if (index == num)
          this.lstBlankNodes[index].ChangeScale(1);
        else if (index == num + 1)
          this.lstBlankNodes[index].ChangeScale(2);
        else
          this.lstBlankNodes[index].ChangeScale(3);
      }
      else
        this.lstBlankNodes[index].ChangeScale(3);
    }
  }

  private void CursorMove(float deltaTime)
  {
    float num;
    if (this.cursorMovePtn == 0)
    {
      this.cursorTime += deltaTime / this.cursorFirstHurfAnimTimeLimit;
      num = Mathf.InverseLerp(0.0f, 1f, this.cursorTime);
      if ((double) num == 1.0)
        this.cursorMovePtn = 1;
    }
    else
    {
      this.cursorTime -= deltaTime / this.cursorLaterHurfAnimTimeLimit;
      num = Mathf.InverseLerp(0.0f, 1f, this.cursorTime);
      if ((double) num == 0.0)
        this.cursorMovePtn = 0;
    }
    Vector3.get_zero();
    Vector3 vector3 = Vector2.op_Implicit(((RectTransform) this.cursor.GetComponent<RectTransform>()).get_anchoredPosition());
    vector3.x = (__Null) (double) Mathf.Lerp(this.cursorInitPos - this.cursorMoveRange / 2f, this.cursorInitPos + this.cursorMoveRange / 2f, num);
    ((RectTransform) this.cursor.GetComponent<RectTransform>()).set_anchoredPosition(Vector2.op_Implicit(vector3));
  }

  public List<ScrollCylinderNode> GetList()
  {
    return this.lstNodes;
  }

  public ValueTuple<int, Vector2, ScrollCylinderNode> GetTarget()
  {
    return this.targetNode;
  }

  public void SetTarget(ScrollCylinderNode target)
  {
    RectTransform component1 = (RectTransform) this.Contents.GetComponent<RectTransform>();
    Vector3 vector3 = Vector2.op_Implicit(component1.get_anchoredPosition());
    for (int index1 = 0; index1 < this.lstNodes.Count; ++index1)
    {
      int index2 = index1;
      if (!Object.op_Inequality((Object) this.lstNodes[index2], (Object) target))
      {
        RectTransform component2 = (RectTransform) ((Component) this.lstNodes[index2]).GetComponent<RectTransform>();
        Vector2 vector2;
        ref Vector2 local1 = ref vector2;
        // ISSUE: variable of the null type
        __Null x = component1.get_anchoredPosition().x;
        Rect rect1 = component1.get_rect();
        double num1 = (double) ((Rect) ref rect1).get_width() / 2.0;
        double num2 = x - num1 + component2.get_anchoredPosition().x;
        local1.x = (__Null) num2;
        ref Vector2 local2 = ref vector2;
        // ISSUE: variable of the null type
        __Null y = component1.get_anchoredPosition().y;
        Rect rect2 = component1.get_rect();
        double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
        double num4 = y + num3 + component2.get_anchoredPosition().y;
        local2.y = (__Null) num4;
        vector2.x = vector3.x - vector2.x;
        vector2.y = vector3.y - vector2.y;
        this.targetNode = new ValueTuple<int, Vector2, ScrollCylinderNode>(index2, vector2, target);
        if (this._onValueChange == null)
          break;
        this._onValueChange.OnNext(index2);
        break;
      }
    }
  }

  private Vector3 LimitPos(RectTransform ContentsRt, Vector3 ContentsPosition)
  {
    RectTransform component1 = (RectTransform) ((Component) ((Component) this).get_transform()).GetComponent<RectTransform>();
    float num1 = 0.0f;
    float num2 = 0.0f;
    // ISSUE: variable of the null type
    __Null y1 = component1.get_sizeDelta().y;
    Rect rect1 = ContentsRt.get_rect();
    double height = (double) ((Rect) ref rect1).get_height();
    if (y1 / height % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null y2 = ContentsPosition.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num1 = (float) (y2 + num3 - component1.get_sizeDelta().y / 2.0);
      // ISSUE: variable of the null type
      __Null y3 = ContentsPosition.y;
      Rect rect3 = ContentsRt.get_rect();
      double num4 = (double) ((Rect) ref rect3).get_height() / 2.0;
      num2 = (float) (y3 - num4 - -component1.get_sizeDelta().y / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes[0], (Object) null))
    {
      RectTransform component2 = (RectTransform) ((Component) this.lstNodes[0]).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null y2 = ContentsPosition.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num1 = (float) (y2 + num3 - component2.get_sizeDelta().y / 2.0 - component1.get_sizeDelta().y / 2.0);
      // ISSUE: variable of the null type
      __Null y3 = ContentsPosition.y;
      Rect rect3 = ContentsRt.get_rect();
      double num4 = (double) ((Rect) ref rect3).get_height() / 2.0;
      num2 = (float) (y3 - num4 + component2.get_sizeDelta().y / 2.0 - -component1.get_sizeDelta().y / 2.0);
    }
    if ((double) num1 <= 0.0)
    {
      ref Vector3 local = ref ContentsPosition;
      local.y = (__Null) (local.y + (double) Mathf.Abs(num1));
    }
    else if ((double) num2 >= 0.0)
    {
      ref Vector3 local = ref ContentsPosition;
      local.y = (__Null) (local.y - (double) Mathf.Abs(num2));
    }
    float num5 = 0.0f;
    float num6 = 0.0f;
    // ISSUE: variable of the null type
    __Null x1 = component1.get_sizeDelta().x;
    Rect rect4 = ContentsRt.get_rect();
    double width = (double) ((Rect) ref rect4).get_width();
    if (x1 / width % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null x2 = ContentsPosition.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num5 = (float) (x2 - num3 - -component1.get_sizeDelta().x / 2.0);
      // ISSUE: variable of the null type
      __Null x3 = ContentsPosition.x;
      Rect rect3 = ContentsRt.get_rect();
      double num4 = (double) ((Rect) ref rect3).get_width() / 2.0;
      num6 = (float) (x3 + num4 - component1.get_sizeDelta().x / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes[0], (Object) null))
    {
      RectTransform component2 = (RectTransform) ((Component) this.lstNodes[0]).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null x2 = ContentsPosition.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num5 = (float) (x2 + num3 - component2.get_sizeDelta().x / 2.0 - component1.get_sizeDelta().x / 2.0);
      // ISSUE: variable of the null type
      __Null x3 = ContentsPosition.x;
      Rect rect3 = ContentsRt.get_rect();
      double num4 = (double) ((Rect) ref rect3).get_width() / 2.0;
      num6 = (float) (x3 - num4 + component2.get_sizeDelta().x / 2.0 - -component1.get_sizeDelta().x / 2.0);
    }
    if ((double) num5 >= 0.0)
    {
      ref Vector3 local = ref ContentsPosition;
      local.x = (__Null) (local.x - (double) Mathf.Abs(num5));
    }
    else if ((double) num6 <= 0.0)
    {
      ref Vector3 local = ref ContentsPosition;
      local.x = (__Null) (local.x + (double) Mathf.Abs(num6));
    }
    return ContentsPosition;
  }

  private void InitTargrt(RectTransform ContentsRt, Vector3 position)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    RectTransform component1 = (RectTransform) ((Component) ((Component) this).get_transform()).GetComponent<RectTransform>();
    // ISSUE: variable of the null type
    __Null x1 = component1.get_sizeDelta().x;
    Rect rect1 = ContentsRt.get_rect();
    double width = (double) ((Rect) ref rect1).get_width();
    if (x1 / width % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null x2 = position.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num1 = (float) (x2 - num3 - -component1.get_sizeDelta().x / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes[0], (Object) null))
    {
      RectTransform component2 = (RectTransform) ((Component) this.lstNodes[0]).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null x2 = position.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num1 = (float) (x2 - num3 - component2.get_sizeDelta().x / 2.0 - -component1.get_sizeDelta().x / 2.0);
    }
    // ISSUE: variable of the null type
    __Null y1 = component1.get_sizeDelta().y;
    Rect rect3 = ContentsRt.get_rect();
    double height = (double) ((Rect) ref rect3).get_height();
    if (y1 / height % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null y2 = position.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num2 = (float) (y2 + num3 - component1.get_sizeDelta().y / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes[0], (Object) null))
    {
      RectTransform component2 = (RectTransform) ((Component) this.lstNodes[0]).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null y2 = position.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num2 = (float) (y2 + num3 - component2.get_sizeDelta().y / 2.0 - component1.get_sizeDelta().y / 2.0);
    }
    this.targetNode = new ValueTuple<int, Vector2, ScrollCylinderNode>(0, new Vector2((float) position.x - num1, (float) position.y - num2), this.lstNodes[0]);
    if (this._onValueChange == null)
      return;
    this._onValueChange.OnNext(0);
  }

  private void OldRectSet(RectTransform src, ref RectTransform newRect)
  {
    newRect.set_anchoredPosition(src.get_anchoredPosition());
    newRect.set_anchoredPosition3D(src.get_anchoredPosition3D());
    newRect.set_anchorMax(src.get_anchorMax());
    newRect.set_anchorMin(src.get_anchorMin());
    newRect.set_offsetMax(src.get_offsetMax());
    newRect.set_offsetMin(src.get_offsetMin());
    newRect.set_pivot(src.get_pivot());
    newRect.set_sizeDelta(src.get_sizeDelta());
  }
}
