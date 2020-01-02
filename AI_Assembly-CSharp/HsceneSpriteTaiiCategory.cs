// Decompiled with JetBrains decompiler
// Type: HsceneSpriteTaiiCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using ReMotion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HsceneSpriteTaiiCategory : HSceneSpriteCategory
{
  [Space(2f)]
  private Color[] arrowColor = new Color[3]
  {
    Color.get_white(),
    Color.get_gray(),
    Color.get_green()
  };
  private int[] ArrowChangeState = new int[2]{ -1, -1 };
  private float[] ArrowAnimTime = new float[2];
  private LinkedList<ScrollCylinderNode> lstScrollNode = new LinkedList<ScrollCylinderNode>();
  [SerializeField]
  private HSceneSprite hSprite;
  [SerializeField]
  private RotationScroll hSceneScroll;
  [SerializeField]
  private Button leftArrow;
  [SerializeField]
  private Image imgLeftArrow;
  [SerializeField]
  private Button rightArrow;
  [SerializeField]
  private Image imgRightArrow;
  [Space(2f)]
  [SerializeField]
  private float[] ArrowInitPos;
  [SerializeField]
  private float ArrowMoveVal;
  private float ArrowWaitingTime;
  private int ArrowWaitingMovePtn;
  [SerializeField]
  private float ArrowFirstHurfWaitingAnimTimeLimit;
  [SerializeField]
  private float ArrowLaterHurfWaitingAnimTimeLimit;
  [SerializeField]
  private float ArrowBigAnimTimeLimit;
  [SerializeField]
  private float ArrowWaitAnimTimeLimit;
  [SerializeField]
  private float ArrowSmallAnimTimeLimit;
  private LinkedListNode<ScrollCylinderNode> target;
  private HRotationScrollNode targetNode;
  private LinkedListNode<ScrollCylinderNode> tmp;
  private LinkedListNode<ScrollCylinderNode> btOld;

  public void Init()
  {
    this.lstScrollNode = this.hSceneScroll.GetList();
    if (this.lstScrollNode.Count <= 1)
    {
      ((Selectable) this.leftArrow).set_interactable(false);
      ((Graphic) this.imgLeftArrow).set_color(this.arrowColor[1]);
      ((Selectable) this.rightArrow).set_interactable(false);
      ((Graphic) this.imgRightArrow).set_color(this.arrowColor[1]);
    }
    if (!((Selectable) this.leftArrow).get_interactable())
    {
      ((Selectable) this.leftArrow).set_interactable(true);
      ((Graphic) this.imgLeftArrow).set_color(this.arrowColor[0]);
    }
    if (!((Selectable) this.rightArrow).get_interactable())
    {
      ((Selectable) this.rightArrow).set_interactable(true);
      ((Graphic) this.imgRightArrow).set_color(this.arrowColor[0]);
    }
    // ISSUE: method pointer
    ((UnityEvent) this.leftArrow.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
    // ISSUE: method pointer
    ((UnityEvent) this.rightArrow.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__1)));
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.hSceneScroll.GetTarget().Item2, (Object) null))
      return;
    this.tmp = new LinkedListNode<ScrollCylinderNode>(this.target == null ? (ScrollCylinderNode) null : this.target.Value);
    this.target = this.lstScrollNode.Find((ScrollCylinderNode) this.hSceneScroll.GetTarget().Item2);
    if (Object.op_Inequality((Object) this.tmp.Value, (Object) null) && this.target != null && Object.op_Inequality((Object) this.target.Value, (Object) null))
    {
      this.targetNode = (HRotationScrollNode) ((Component) this.target.Value).GetComponent<HRotationScrollNode>();
      if (this.targetNode.id != ((HRotationScrollNode) ((Component) this.tmp.Value).GetComponent<HRotationScrollNode>()).id)
      {
        if (Object.op_Equality((Object) this.tmp.Value, (Object) this.target.Previous?.Value))
        {
          this.ArrowChangeState[1] = 0;
          this.ArrowAnimTime[1] = 0.0f;
        }
        else if (Object.op_Equality((Object) this.tmp.Value, (Object) this.target.Next?.Value))
        {
          this.ArrowChangeState[0] = 0;
          this.ArrowAnimTime[0] = 0.0f;
        }
        this.hSprite.OnClickMotion(this.targetNode.id);
      }
    }
    float deltaTime = Time.get_deltaTime();
    if (this.ArrowChangeState[0] != -1)
      this.ButtonReaction(this.imgLeftArrow, ref this.ArrowChangeState[0], ref this.ArrowAnimTime[0], deltaTime);
    if (this.ArrowChangeState[1] != -1)
      this.ButtonReaction(this.imgRightArrow, ref this.ArrowChangeState[1], ref this.ArrowAnimTime[1], deltaTime);
    this.ButtonWaitReaction(deltaTime);
  }

  public RotationScroll GetHScroll()
  {
    return this.hSceneScroll;
  }

  private void ButtonReaction(
    Image _image,
    ref int ArrowChangeState,
    ref float ArrowAnimTime,
    float deltaTime)
  {
    if (ArrowChangeState == 0)
    {
      ArrowAnimTime += deltaTime / this.ArrowBigAnimTimeLimit;
      float time = Mathf.InverseLerp(0.0f, 1f, ArrowAnimTime);
      float num = EasingFunctions.EaseOutQuint(time, 1f);
      ((Graphic) _image).set_color(new Color(Mathf.Lerp((float) this.arrowColor[0].r, (float) this.arrowColor[2].r, num), Mathf.Lerp((float) this.arrowColor[0].g, (float) this.arrowColor[2].g, num), Mathf.Lerp((float) this.arrowColor[0].b, (float) this.arrowColor[2].b, num), Mathf.Lerp((float) this.arrowColor[0].a, (float) this.arrowColor[2].a, num)));
      ((Component) _image).get_transform().set_localScale(Vector3.Lerp(Vector3.get_one(), Vector3.op_Multiply(Vector3.get_one(), 1.3f), num));
      if ((double) num != 1.0)
        return;
      ArrowAnimTime = 0.0f;
      ArrowChangeState = 1;
    }
    else if (ArrowChangeState == 1)
    {
      ArrowAnimTime += deltaTime / this.ArrowWaitAnimTimeLimit;
      if ((double) Mathf.InverseLerp(0.0f, 1f, ArrowAnimTime) != 1.0)
        return;
      ArrowAnimTime = 1f;
      ArrowChangeState = 2;
    }
    else
    {
      if (ArrowChangeState != 2)
        return;
      ArrowAnimTime -= deltaTime / this.ArrowSmallAnimTimeLimit;
      float time = Mathf.InverseLerp(0.0f, 1f, ArrowAnimTime);
      float num = EasingFunctions.EaseOutQuint(time, 1f);
      ((Graphic) _image).set_color(new Color(Mathf.Lerp((float) this.arrowColor[0].r, (float) this.arrowColor[2].r, num), Mathf.Lerp((float) this.arrowColor[0].g, (float) this.arrowColor[2].g, num), Mathf.Lerp((float) this.arrowColor[0].b, (float) this.arrowColor[2].b, num), Mathf.Lerp((float) this.arrowColor[0].a, (float) this.arrowColor[2].a, num)));
      ((Component) _image).get_transform().set_localScale(Vector3.Lerp(Vector3.get_one(), Vector3.op_Multiply(Vector3.get_one(), 1.3f), num));
      if ((double) num != 0.0)
        return;
      ArrowAnimTime = 0.0f;
      ArrowChangeState = -1;
    }
  }

  private void ButtonWaitReaction(float deltaTime)
  {
    float num;
    if (this.ArrowWaitingMovePtn == 0)
    {
      this.ArrowWaitingTime += deltaTime / this.ArrowFirstHurfWaitingAnimTimeLimit;
      num = Mathf.InverseLerp(0.0f, 1f, this.ArrowWaitingTime);
      if ((double) num == 1.0)
        this.ArrowWaitingMovePtn = 1;
    }
    else
    {
      this.ArrowWaitingTime -= deltaTime / this.ArrowLaterHurfWaitingAnimTimeLimit;
      num = Mathf.InverseLerp(0.0f, 1f, this.ArrowWaitingTime);
      if ((double) num == 0.0)
        this.ArrowWaitingMovePtn = 0;
    }
    Vector3.get_zero();
    if (this.ArrowChangeState[0] == -1 && ((Selectable) this.leftArrow).get_interactable())
    {
      Vector3 localPosition = ((Component) this.imgLeftArrow).get_transform().get_localPosition();
      localPosition.x = (__Null) (double) Mathf.Lerp(this.ArrowInitPos[0], this.ArrowInitPos[0] - this.ArrowMoveVal, num);
      ((Component) this.imgLeftArrow).get_transform().set_localPosition(localPosition);
    }
    else
    {
      Vector3 localPosition = ((Component) this.imgLeftArrow).get_transform().get_localPosition();
      localPosition.x = (__Null) (double) this.ArrowInitPos[0];
      ((Component) this.imgLeftArrow).get_transform().set_localPosition(localPosition);
    }
    if (this.ArrowChangeState[1] == -1 && ((Selectable) this.rightArrow).get_interactable())
    {
      Vector3 localPosition = ((Component) this.imgRightArrow).get_transform().get_localPosition();
      localPosition.x = (__Null) (double) Mathf.Lerp(this.ArrowInitPos[1], this.ArrowInitPos[1] + this.ArrowMoveVal, num);
      ((Component) this.imgRightArrow).get_transform().set_localPosition(localPosition);
    }
    else
    {
      Vector3 localPosition = ((Component) this.imgRightArrow).get_transform().get_localPosition();
      localPosition.x = (__Null) (double) this.ArrowInitPos[1];
      ((Component) this.imgRightArrow).get_transform().set_localPosition(localPosition);
    }
  }

  public override void SetActive(bool _active, int _array = -1)
  {
    if (_array < 0)
    {
      for (int index = 0; index < this.lstButton.Count; ++index)
      {
        if (((Component) this.lstButton[index]).get_gameObject().get_activeSelf() != _active)
          ((Component) this.lstButton[index]).get_gameObject().SetActive(_active);
      }
    }
    else
    {
      if (this.lstButton.Count <= _array || ((Component) this.lstButton[_array]).get_gameObject().get_activeSelf() == _active)
        return;
      ((Component) this.lstButton[_array]).get_gameObject().SetActive(_active);
    }
  }

  public void EndProc()
  {
    ((UnityEventBase) this.leftArrow.get_onClick()).RemoveAllListeners();
    ((UnityEventBase) this.rightArrow.get_onClick()).RemoveAllListeners();
  }
}
