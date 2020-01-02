// Decompiled with JetBrains decompiler
// Type: RotationScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEx;

public class RotationScroll : MonoBehaviour
{
  public GameObject Contents;
  [Space]
  [SerializeField]
  public RectTransform SelectRect;
  [Space]
  public RectTransform Atari;
  [SerializeField]
  private float moveTime;
  [SerializeField]
  private LinkedList<ScrollCylinderNode> lstNodes;
  [SerializeField]
  private ValueTuple<Vector2, ScrollCylinderNode> targetNode;
  private Input input;
  [SerializeField]
  private bool InitList;
  [SerializeField]
  private int MaxView;
  private bool onEnter;
  public HRotationScrollNode[] NodeList;
  public ScrollDir scrollMode;

  public RotationScroll()
  {
    base.\u002Ector();
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
    this.ListNodeSet((List<ScrollCylinderNode>) null, true);
  }

  public void ListNodeSet(List<ScrollCylinderNode> ScrollNodes = null, bool targetInit = true)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    RotationScroll.\u003CListNodeSet\u003Ec__AnonStorey0 nodeSetCAnonStorey0 = new RotationScroll.\u003CListNodeSet\u003Ec__AnonStorey0();
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.\u0024this = this;
    this.lstNodes.Clear();
    if (ScrollNodes != null)
    {
      for (int index = 0; index < ScrollNodes.Count; ++index)
      {
        RectTransform component = (RectTransform) ((Component) ScrollNodes[index]).GetComponent<RectTransform>();
        if (ScrollNodes.Count < this.MaxView)
        {
          RectTransform rectTransform = component;
          Rect rect1 = component.get_rect();
          double num1 = (double) ((Rect) ref rect1).get_width() / 2.0;
          Rect rect2 = component.get_rect();
          double num2 = -(double) ((Rect) ref rect2).get_height() / 2.0;
          Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
          double num3;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num3 = (double) ((Rect) ref rect3).get_width() * (double) (this.MaxView - 1);
          }
          double num4;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num4 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num4 = -(double) ((Rect) ref rect3).get_height() * (double) (this.MaxView - 1);
          }
          Vector2 vector2_2 = Vector2.op_Multiply(new Vector2((float) num3, (float) num4), (float) index);
          Vector2 vector2_3 = Vector2.op_Addition(vector2_1, vector2_2);
          rectTransform.set_anchoredPosition(vector2_3);
        }
        else
        {
          RectTransform rectTransform = component;
          Rect rect1 = component.get_rect();
          double num1 = (double) ((Rect) ref rect1).get_width() / 2.0;
          Rect rect2 = component.get_rect();
          double num2 = -(double) ((Rect) ref rect2).get_height() / 2.0;
          Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
          double num3;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num3 = (double) ((Rect) ref rect3).get_width();
          }
          double num4;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num4 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num4 = -(double) ((Rect) ref rect3).get_height();
          }
          Vector2 vector2_2 = Vector2.op_Multiply(new Vector2((float) num3, (float) num4), (float) index);
          Vector2 vector2_3 = Vector2.op_Addition(vector2_1, vector2_2);
          rectTransform.set_anchoredPosition(vector2_3);
        }
        this.lstNodes.AddLast(ScrollNodes[index]);
      }
    }
    else
    {
      ScrollCylinderNode[] componentsInChildren = (ScrollCylinderNode[]) this.Contents.GetComponentsInChildren<ScrollCylinderNode>();
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        RectTransform component = (RectTransform) ((Component) componentsInChildren[index]).GetComponent<RectTransform>();
        if (componentsInChildren.Length < this.MaxView)
        {
          RectTransform rectTransform = component;
          Rect rect1 = component.get_rect();
          double num1 = (double) ((Rect) ref rect1).get_width() / 2.0;
          Rect rect2 = component.get_rect();
          double num2 = -(double) ((Rect) ref rect2).get_height() / 2.0;
          Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
          double num3;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num3 = (double) ((Rect) ref rect3).get_width() * (double) (this.MaxView - 1);
          }
          double num4;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num4 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num4 = -(double) ((Rect) ref rect3).get_height() * (double) (this.MaxView - 1);
          }
          Vector2 vector2_2 = Vector2.op_Multiply(new Vector2((float) num3, (float) num4), (float) index);
          Vector2 vector2_3 = Vector2.op_Addition(vector2_1, vector2_2);
          rectTransform.set_anchoredPosition(vector2_3);
        }
        else
        {
          RectTransform rectTransform = component;
          Rect rect1 = component.get_rect();
          double num1 = (double) ((Rect) ref rect1).get_width() / 2.0;
          Rect rect2 = component.get_rect();
          double num2 = -(double) ((Rect) ref rect2).get_height() / 2.0;
          Vector2 vector2_1 = new Vector2((float) num1, (float) num2);
          double num3;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num3 = (double) ((Rect) ref rect3).get_width();
          }
          double num4;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num4 = 0.0;
          }
          else
          {
            Rect rect3 = component.get_rect();
            num4 = -(double) ((Rect) ref rect3).get_height();
          }
          Vector2 vector2_2 = Vector2.op_Multiply(new Vector2((float) num3, (float) num4), (float) index);
          Vector2 vector2_3 = Vector2.op_Addition(vector2_1, vector2_2);
          rectTransform.set_anchoredPosition(vector2_3);
        }
        this.lstNodes.AddLast(componentsInChildren[index]);
      }
    }
    if (this.lstNodes.Count == 0)
      return;
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsRt = (RectTransform) this.Contents.GetComponent<RectTransform>();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsPosition = Vector2.op_Implicit(nodeSetCAnonStorey0.ContentsRt.get_anchoredPosition());
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.rt2 = (RectTransform) Object.Instantiate<RectTransform>((M0) nodeSetCAnonStorey0.ContentsRt);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.position2 = nodeSetCAnonStorey0.ContentsPosition;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsPosition = this.LimitPos(nodeSetCAnonStorey0.ContentsRt, nodeSetCAnonStorey0.ContentsPosition, this.scrollMode != ScrollDir.Vertical ? 0 : 1);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    nodeSetCAnonStorey0.ContentsRt.set_anchoredPosition(Vector2.op_Implicit(nodeSetCAnonStorey0.ContentsPosition));
    PointerEnterTrigger pointerEnterTrigger1 = (PointerEnterTrigger) null;
    UITrigger.TriggerEvent triggerEvent1 = new UITrigger.TriggerEvent();
    foreach (ScrollCylinderNode lstNode in this.lstNodes)
    {
      pointerEnterTrigger1 = (PointerEnterTrigger) null;
      PointerEnterTrigger component = (PointerEnterTrigger) ((Component) lstNode).get_gameObject().GetComponent<PointerEnterTrigger>();
      ((Component) lstNode).get_gameObject().SetActive(true);
      if (!Object.op_Inequality((Object) component, (Object) null))
      {
        PointerEnterTrigger pointerEnterTrigger2 = (PointerEnterTrigger) ((Component) lstNode).get_gameObject().AddComponent<PointerEnterTrigger>();
        UITrigger.TriggerEvent triggerEvent2 = new UITrigger.TriggerEvent();
        ((UITrigger) pointerEnterTrigger2).get_Triggers().Add(triggerEvent2);
        // ISSUE: method pointer
        ((UnityEvent<BaseEventData>) triggerEvent2).AddListener(new UnityAction<BaseEventData>((object) nodeSetCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      }
    }
    if (!targetInit)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!Object.op_Inequality((Object) nodeSetCAnonStorey0.rt2, (Object) null) || !Object.op_Inequality((Object) ((Component) nodeSetCAnonStorey0.rt2).get_gameObject(), (Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      Object.Destroy((Object) ((Component) nodeSetCAnonStorey0.rt2).get_gameObject());
    }
    else
    {
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
    float num1 = this.input.ScrollValue();
    LinkedListNode<ScrollCylinderNode> linkedListNode1 = this.lstNodes.Find((ScrollCylinderNode) this.targetNode.Item2);
    if (linkedListNode1 == null)
      return;
    if (this.lstNodes.Count == 1)
      num1 = 0.0f;
    if (this.onEnter)
    {
      if ((double) num1 < 0.0)
      {
        LinkedListNode<ScrollCylinderNode> linkedListNode2 = this.lstNodes.Count < this.MaxView ? linkedListNode1.Next ?? this.lstNodes.First : linkedListNode1.Next;
        RectTransform component2 = (RectTransform) ((Component) linkedListNode2.Value).GetComponent<RectTransform>();
        if (this.lstNodes.Count < this.MaxView)
        {
          RectTransform component3 = (RectTransform) ((Component) linkedListNode1.Value).GetComponent<RectTransform>();
          RectTransform rectTransform = component2;
          Vector2 anchoredPosition = component3.get_anchoredPosition();
          double num2;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num2 = 0.0;
          }
          else
          {
            Rect rect = component3.get_rect();
            num2 = (double) ((Rect) ref rect).get_width() * (double) (this.MaxView - 1);
          }
          double num3;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect = component3.get_rect();
            num3 = -(double) ((Rect) ref rect).get_height() * (double) (this.MaxView - 1);
          }
          Vector2 vector2_1 = new Vector2((float) num2, (float) num3);
          Vector2 vector2_2 = Vector2.op_Addition(anchoredPosition, vector2_1);
          rectTransform.set_anchoredPosition(vector2_2);
          if (linkedListNode2 == this.lstNodes.First)
          {
            LinkedListNode<ScrollCylinderNode> node = new LinkedListNode<ScrollCylinderNode>(this.lstNodes.First.Value);
            this.lstNodes.RemoveFirst();
            this.lstNodes.AddLast(node);
            linkedListNode2 = this.lstNodes.Last;
            component2 = (RectTransform) ((Component) linkedListNode2.Value).GetComponent<RectTransform>();
          }
        }
        Vector2 vector2;
        vector2.x = (__Null) (component1.get_anchoredPosition().x - component1.get_sizeDelta().x / 2.0 + component2.get_anchoredPosition().x);
        vector2.y = (__Null) (component1.get_anchoredPosition().y + component1.get_sizeDelta().y / 2.0 + component2.get_anchoredPosition().y);
        vector2.x = ContentsPosition.x - vector2.x;
        vector2.y = ContentsPosition.y - vector2.y;
        this.targetNode = new ValueTuple<Vector2, ScrollCylinderNode>(vector2, linkedListNode2.Value);
        linkedListNode1 = linkedListNode2;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
      }
      else if ((double) num1 > 0.0)
      {
        LinkedListNode<ScrollCylinderNode> linkedListNode2 = this.lstNodes.Count < this.MaxView ? linkedListNode1.Previous ?? this.lstNodes.Last : linkedListNode1.Previous;
        RectTransform component2 = (RectTransform) ((Component) linkedListNode2.Value).GetComponent<RectTransform>();
        if (this.lstNodes.Count < this.MaxView)
        {
          RectTransform component3 = (RectTransform) ((Component) linkedListNode1.Value).GetComponent<RectTransform>();
          RectTransform rectTransform = component2;
          Vector2 anchoredPosition = component3.get_anchoredPosition();
          double num2;
          if (this.scrollMode == ScrollDir.Vertical)
          {
            num2 = 0.0;
          }
          else
          {
            Rect rect = component3.get_rect();
            num2 = -(double) ((Rect) ref rect).get_width() * (double) (this.MaxView - 1);
          }
          double num3;
          if (this.scrollMode == ScrollDir.Horizontal)
          {
            num3 = 0.0;
          }
          else
          {
            Rect rect = component3.get_rect();
            num3 = (double) ((Rect) ref rect).get_height() * (double) (this.MaxView - 1);
          }
          Vector2 vector2_1 = new Vector2((float) num2, (float) num3);
          Vector2 vector2_2 = Vector2.op_Addition(anchoredPosition, vector2_1);
          rectTransform.set_anchoredPosition(vector2_2);
          if (linkedListNode2 == this.lstNodes.Last)
          {
            LinkedListNode<ScrollCylinderNode> node = new LinkedListNode<ScrollCylinderNode>(this.lstNodes.Last.Value);
            this.lstNodes.RemoveLast();
            this.lstNodes.AddFirst(node);
            linkedListNode2 = this.lstNodes.First;
            component2 = (RectTransform) ((Component) linkedListNode2.Value).GetComponent<RectTransform>();
          }
        }
        Vector2 vector2;
        vector2.x = (__Null) (component1.get_anchoredPosition().x - component1.get_sizeDelta().x / 2.0 + component2.get_anchoredPosition().x);
        vector2.y = (__Null) (component1.get_anchoredPosition().y + component1.get_sizeDelta().y / 2.0 + component2.get_anchoredPosition().y);
        vector2.x = ContentsPosition.x - vector2.x;
        vector2.y = ContentsPosition.y - vector2.y;
        this.targetNode = new ValueTuple<Vector2, ScrollCylinderNode>(vector2, linkedListNode2.Value);
        linkedListNode1 = linkedListNode2;
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Select);
      }
    }
    if (this.lstNodes.Count >= this.MaxView)
    {
      if (linkedListNode1.Previous == null)
      {
        LinkedListNode<ScrollCylinderNode> node = new LinkedListNode<ScrollCylinderNode>(this.lstNodes.Last.Value);
        this.lstNodes.RemoveLast();
        this.lstNodes.AddFirst(node);
        RectTransform[] rectTransformArray = new RectTransform[2]
        {
          (RectTransform) ((Component) node.Value).GetComponent<RectTransform>(),
          (RectTransform) ((Component) linkedListNode1.Value).GetComponent<RectTransform>()
        };
        Vector2 anchoredPosition = rectTransformArray[0].get_anchoredPosition();
        if (rectTransformArray[0].get_anchoredPosition().x == rectTransformArray[1].get_anchoredPosition().x)
        {
          ref Vector2 local = ref anchoredPosition;
          // ISSUE: variable of the null type
          __Null y = rectTransformArray[1].get_anchoredPosition().y;
          Rect rect = rectTransformArray[1].get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double num2 = y + height;
          local.y = (__Null) num2;
        }
        if (rectTransformArray[0].get_anchoredPosition().y == rectTransformArray[1].get_anchoredPosition().y)
        {
          ref Vector2 local = ref anchoredPosition;
          // ISSUE: variable of the null type
          __Null x = rectTransformArray[1].get_anchoredPosition().x;
          Rect rect = rectTransformArray[1].get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double num2 = x - width;
          local.x = (__Null) num2;
        }
        rectTransformArray[0].set_anchoredPosition(anchoredPosition);
      }
      if (linkedListNode1.Next == null)
      {
        LinkedListNode<ScrollCylinderNode> node = new LinkedListNode<ScrollCylinderNode>(this.lstNodes.First.Value);
        this.lstNodes.RemoveFirst();
        this.lstNodes.AddLast(node);
        RectTransform[] rectTransformArray = new RectTransform[2]
        {
          (RectTransform) ((Component) node.Value).GetComponent<RectTransform>(),
          (RectTransform) ((Component) linkedListNode1.Value).GetComponent<RectTransform>()
        };
        Vector2 anchoredPosition = rectTransformArray[0].get_anchoredPosition();
        if (rectTransformArray[0].get_anchoredPosition().x == rectTransformArray[1].get_anchoredPosition().x)
        {
          ref Vector2 local = ref anchoredPosition;
          // ISSUE: variable of the null type
          __Null y = rectTransformArray[1].get_anchoredPosition().y;
          Rect rect = rectTransformArray[0].get_rect();
          double height = (double) ((Rect) ref rect).get_height();
          double num2 = y - height;
          local.y = (__Null) num2;
        }
        if (rectTransformArray[0].get_anchoredPosition().y == rectTransformArray[1].get_anchoredPosition().y)
        {
          ref Vector2 local = ref anchoredPosition;
          // ISSUE: variable of the null type
          __Null x = rectTransformArray[1].get_anchoredPosition().x;
          Rect rect = rectTransformArray[0].get_rect();
          double width = (double) ((Rect) ref rect).get_width();
          double num2 = x + width;
          local.x = (__Null) num2;
        }
        rectTransformArray[0].set_anchoredPosition(anchoredPosition);
      }
    }
    Vector2 vector2_3 = (Vector2) this.targetNode.Item1;
    float num4 = 0.0f;
    ContentsPosition.x = (__Null) (double) Mathf.SmoothDamp((float) ContentsPosition.x, (float) vector2_3.x, ref num4, this.moveTime, float.PositiveInfinity, Time.get_deltaTime());
    num4 = 0.0f;
    ContentsPosition.y = (__Null) (double) Mathf.SmoothDamp((float) ContentsPosition.y, (float) vector2_3.y, ref num4, this.moveTime, float.PositiveInfinity, Time.get_deltaTime());
    ContentsPosition = this.LimitPos(component1, ContentsPosition, this.scrollMode != ScrollDir.Vertical ? 0 : 1);
    component1.set_anchoredPosition(Vector2.op_Implicit(ContentsPosition));
  }

  private void ChangeNode()
  {
    Time.get_deltaTime();
    this.ChangeNodeColor();
    this.ChangeNodeScl();
  }

  private void ChangeNodeColor()
  {
    LinkedListNode<ScrollCylinderNode> linkedListNode = this.lstNodes.Find((ScrollCylinderNode) this.targetNode.Item2);
    if (linkedListNode == null)
      return;
    foreach (ScrollCylinderNode lstNode in this.lstNodes)
    {
      if (Object.op_Equality((Object) lstNode, (Object) linkedListNode.Value))
        lstNode.ChangeBGAlpha(0);
      else if (Object.op_Equality((Object) lstNode, (Object) linkedListNode.Previous?.Value) || Object.op_Equality((Object) lstNode, (Object) linkedListNode.Next?.Value))
        lstNode.ChangeBGAlpha(1);
      else
        lstNode.ChangeBGAlpha(3);
    }
  }

  private void ChangeNodeScl()
  {
    LinkedListNode<ScrollCylinderNode> linkedListNode = this.lstNodes.Find((ScrollCylinderNode) this.targetNode.Item2);
    if (linkedListNode == null)
      return;
    foreach (ScrollCylinderNode lstNode in this.lstNodes)
    {
      if (Object.op_Equality((Object) lstNode, (Object) linkedListNode.Value))
        lstNode.ChangeScale(0);
      else if (Object.op_Equality((Object) lstNode, (Object) linkedListNode.Previous?.Value) || Object.op_Equality((Object) lstNode, (Object) linkedListNode.Next?.Value))
        lstNode.ChangeScale(1);
      else
        lstNode.ChangeScale(3);
    }
  }

  public LinkedList<ScrollCylinderNode> GetList()
  {
    return this.lstNodes;
  }

  public ValueTuple<Vector2, ScrollCylinderNode> GetTarget()
  {
    return this.targetNode;
  }

  public void SetTarget(ScrollCylinderNode target)
  {
    RectTransform component1 = (RectTransform) this.Contents.GetComponent<RectTransform>();
    Vector3 vector3 = Vector2.op_Implicit(component1.get_anchoredPosition());
    foreach (ScrollCylinderNode lstNode in this.lstNodes)
    {
      if (!Object.op_Inequality((Object) lstNode, (Object) target))
      {
        RectTransform component2 = (RectTransform) ((Component) lstNode).GetComponent<RectTransform>();
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
        this.targetNode = new ValueTuple<Vector2, ScrollCylinderNode>(vector2, target);
        break;
      }
    }
  }

  public void SetTarget(int taii)
  {
    RectTransform component1 = (RectTransform) this.Contents.GetComponent<RectTransform>();
    Vector3 vector3 = Vector2.op_Implicit(component1.get_anchoredPosition());
    foreach (ScrollCylinderNode lstNode in this.lstNodes)
    {
      if (((HRotationScrollNode) ((Component) lstNode).GetComponent<HRotationScrollNode>()).id == taii)
      {
        RectTransform component2 = (RectTransform) ((Component) lstNode).GetComponent<RectTransform>();
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
        this.targetNode = new ValueTuple<Vector2, ScrollCylinderNode>(vector2, lstNode);
        break;
      }
    }
  }

  private Vector3 LimitPos(RectTransform ContentsRt, Vector3 ContentsPosition, int LimitDir)
  {
    RectTransform component1 = (RectTransform) ((Component) ((Component) this).get_transform()).GetComponent<RectTransform>();
    if (LimitDir == 0)
    {
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
      else if (Object.op_Inequality((Object) this.lstNodes.First.Value, (Object) null))
      {
        RectTransform component2 = (RectTransform) ((Component) this.lstNodes.First.Value).GetComponent<RectTransform>();
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
    }
    else
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      // ISSUE: variable of the null type
      __Null x1 = component1.get_sizeDelta().x;
      Rect rect1 = ContentsRt.get_rect();
      double width = (double) ((Rect) ref rect1).get_width();
      if (x1 / width % 2.0 != 0.0)
      {
        // ISSUE: variable of the null type
        __Null x2 = ContentsPosition.x;
        Rect rect2 = ContentsRt.get_rect();
        double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
        num1 = (float) (x2 - num3 - -component1.get_sizeDelta().x / 2.0);
        // ISSUE: variable of the null type
        __Null x3 = ContentsPosition.x;
        Rect rect3 = ContentsRt.get_rect();
        double num4 = (double) ((Rect) ref rect3).get_width() / 2.0;
        num2 = (float) (x3 + num4 - component1.get_sizeDelta().x / 2.0);
      }
      else if (Object.op_Inequality((Object) this.lstNodes.First.Value, (Object) null))
      {
        RectTransform component2 = (RectTransform) ((Component) this.lstNodes.First.Value).GetComponent<RectTransform>();
        // ISSUE: variable of the null type
        __Null x2 = ContentsPosition.x;
        Rect rect2 = ContentsRt.get_rect();
        double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
        num1 = (float) (x2 + num3 - component2.get_sizeDelta().x / 2.0 - component1.get_sizeDelta().x / 2.0);
        // ISSUE: variable of the null type
        __Null x3 = ContentsPosition.x;
        Rect rect3 = ContentsRt.get_rect();
        double num4 = (double) ((Rect) ref rect3).get_width() / 2.0;
        num2 = (float) (x3 - num4 + component2.get_sizeDelta().x / 2.0 - -component1.get_sizeDelta().x / 2.0);
      }
      if ((double) num1 >= 0.0)
      {
        ref Vector3 local = ref ContentsPosition;
        local.x = (__Null) (local.x - (double) Mathf.Abs(num1));
      }
      else if ((double) num2 <= 0.0)
      {
        ref Vector3 local = ref ContentsPosition;
        local.x = (__Null) (local.x + (double) Mathf.Abs(num2));
      }
    }
    return ContentsPosition;
  }

  private void InitTargrt(RectTransform ContentsRt, Vector3 position)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    RectTransform selectRect = this.SelectRect;
    // ISSUE: variable of the null type
    __Null x1 = selectRect.get_sizeDelta().x;
    Rect rect1 = ContentsRt.get_rect();
    double width = (double) ((Rect) ref rect1).get_width();
    if (x1 / width % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null x2 = position.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num1 = (float) (x2 - num3 - -selectRect.get_sizeDelta().x / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes.First.Value, (Object) null))
    {
      RectTransform component = (RectTransform) ((Component) this.lstNodes.First.Value).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null x2 = position.x;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_width() / 2.0;
      num1 = (float) (x2 - num3 - component.get_sizeDelta().x / 2.0 - -selectRect.get_sizeDelta().x / 2.0);
    }
    // ISSUE: variable of the null type
    __Null y1 = selectRect.get_sizeDelta().y;
    Rect rect3 = ContentsRt.get_rect();
    double height = (double) ((Rect) ref rect3).get_height();
    if (y1 / height % 2.0 != 0.0)
    {
      // ISSUE: variable of the null type
      __Null y2 = position.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num2 = (float) (y2 + num3 - selectRect.get_sizeDelta().y / 2.0);
    }
    else if (Object.op_Inequality((Object) this.lstNodes.First.Value, (Object) null))
    {
      RectTransform component = (RectTransform) ((Component) this.lstNodes.First.Value).GetComponent<RectTransform>();
      // ISSUE: variable of the null type
      __Null y2 = position.y;
      Rect rect2 = ContentsRt.get_rect();
      double num3 = (double) ((Rect) ref rect2).get_height() / 2.0;
      num2 = (float) (y2 + num3 - component.get_sizeDelta().y / 2.0 - selectRect.get_sizeDelta().y / 2.0);
    }
    float num4 = (float) position.x - num1;
    float num5 = (float) position.y - num2;
    ScrollCylinderNode scrollCylinderNode = (ScrollCylinderNode) null;
    foreach (HRotationScrollNode node in this.NodeList)
    {
      if (((Component) node).get_gameObject().get_activeSelf())
      {
        scrollCylinderNode = (ScrollCylinderNode) node;
        break;
      }
    }
    this.targetNode = new ValueTuple<Vector2, ScrollCylinderNode>(new Vector2(num4, num5), scrollCylinderNode);
  }

  public void ListClear()
  {
    this.lstNodes.Clear();
  }
}
