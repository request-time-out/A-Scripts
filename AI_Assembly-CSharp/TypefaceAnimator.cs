// Decompiled with JetBrains decompiler
// Type: TypefaceAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
[AddComponentMenu("UI/Effects/TypefaceAnimator")]
public class TypefaceAnimator : BaseMeshEffect
{
  public TypefaceAnimator.TimeMode timeMode;
  public float duration;
  public float speed;
  public float delay;
  public TypefaceAnimator.Style style;
  public bool playOnAwake;
  [SerializeField]
  private float m_progress;
  public bool usePosition;
  public bool useRotation;
  public bool useScale;
  public bool useAlpha;
  public bool useColor;
  public UnityEvent onStart;
  public UnityEvent onComplete;
  [SerializeField]
  private int characterNumber;
  private float animationTime;
  private Coroutine playCoroutine;
  private bool m_isPlaying;
  public Vector3 positionFrom;
  public Vector3 positionTo;
  public AnimationCurve positionAnimationCurve;
  public float positionSeparation;
  public float rotationFrom;
  public float rotationTo;
  public Vector2 rotationPivot;
  public AnimationCurve rotationAnimationCurve;
  public float rotationSeparation;
  public bool scaleSyncXY;
  public float scaleFrom;
  public float scaleTo;
  public Vector2 scalePivot;
  public AnimationCurve scaleAnimationCurve;
  public float scaleFromY;
  public float scaleToY;
  public Vector2 scalePivotY;
  public AnimationCurve scaleAnimationCurveY;
  public float scaleSeparation;
  public float alphaFrom;
  public float alphaTo;
  public AnimationCurve alphaAnimationCurve;
  public float alphaSeparation;
  public Color colorFrom;
  public Color colorTo;
  public AnimationCurve colorAnimationCurve;
  public float colorSeparation;

  public TypefaceAnimator()
  {
    base.\u002Ector();
  }

  public float progress
  {
    get
    {
      return this.m_progress;
    }
    set
    {
      this.m_progress = value;
      if (!Object.op_Inequality((Object) this.get_graphic(), (Object) null))
        return;
      this.get_graphic().SetVerticesDirty();
    }
  }

  public bool isPlaying
  {
    get
    {
      return this.m_isPlaying;
    }
  }

  protected virtual void OnEnable()
  {
    if (this.playOnAwake)
      this.Play();
    base.OnEnable();
  }

  protected virtual void OnDisable()
  {
    this.Stop();
    base.OnDisable();
  }

  public void Play()
  {
    this.progress = 0.0f;
    switch (this.timeMode)
    {
      case TypefaceAnimator.TimeMode.Time:
        this.animationTime = this.duration;
        break;
      case TypefaceAnimator.TimeMode.Speed:
        this.animationTime = (float) this.characterNumber / 10f / this.speed;
        break;
    }
    switch (this.style)
    {
      case TypefaceAnimator.Style.Once:
        this.playCoroutine = ((MonoBehaviour) this).StartCoroutine(this.PlayOnceCoroutine());
        break;
      case TypefaceAnimator.Style.Loop:
        this.playCoroutine = ((MonoBehaviour) this).StartCoroutine(this.PlayLoopCoroutine());
        break;
      case TypefaceAnimator.Style.PingPong:
        this.playCoroutine = ((MonoBehaviour) this).StartCoroutine(this.PlayPingPongCoroutine());
        break;
    }
  }

  public void Stop()
  {
    if (this.playCoroutine != null)
      ((MonoBehaviour) this).StopCoroutine(this.playCoroutine);
    this.m_isPlaying = false;
    this.playCoroutine = (Coroutine) null;
  }

  [DebuggerHidden]
  private IEnumerator PlayOnceCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new TypefaceAnimator.\u003CPlayOnceCoroutine\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator PlayLoopCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new TypefaceAnimator.\u003CPlayLoopCoroutine\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator PlayPingPongCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new TypefaceAnimator.\u003CPlayPingPongCoroutine\u003Ec__Iterator2()
    {
      \u0024this = this
    };
  }

  public virtual void ModifyMesh(VertexHelper vertexHelper)
  {
    if (!((UIBehaviour) this).IsActive() || vertexHelper.get_currentVertCount() == 0)
      return;
    List<UIVertex> uiVertexList1 = new List<UIVertex>();
    vertexHelper.GetUIVertexStream(uiVertexList1);
    List<UIVertex> verts = new List<UIVertex>();
    for (int index = 0; index < uiVertexList1.Count; ++index)
    {
      switch (index % 6)
      {
        case 0:
        case 1:
        case 2:
        case 4:
          verts.Add(uiVertexList1[index]);
          break;
      }
    }
    this.ModifyVertices(verts);
    List<UIVertex> uiVertexList2 = new List<UIVertex>(uiVertexList1.Count);
    for (int index1 = 0; index1 < uiVertexList1.Count / 6; ++index1)
    {
      int index2 = index1 * 4;
      uiVertexList2.Add(verts[index2]);
      uiVertexList2.Add(verts[index2 + 1]);
      uiVertexList2.Add(verts[index2 + 2]);
      uiVertexList2.Add(verts[index2 + 2]);
      uiVertexList2.Add(verts[index2 + 3]);
      uiVertexList2.Add(verts[index2]);
    }
    vertexHelper.Clear();
    vertexHelper.AddUIVertexTriangleStream(uiVertexList2);
  }

  public void ModifyVertices(List<UIVertex> verts)
  {
    if (!((UIBehaviour) this).IsActive())
      return;
    this.Modify(verts);
  }

  private void Modify(List<UIVertex> verts)
  {
    this.characterNumber = verts.Count / 4;
    for (int index = 0; index < verts.Count; ++index)
    {
      if (index % 4 == 0)
      {
        int currentCharacterNumber = index / 4;
        UIVertex vert1 = verts[index];
        UIVertex vert2 = verts[index + 1];
        UIVertex vert3 = verts[index + 2];
        UIVertex vert4 = verts[index + 3];
        if (this.usePosition)
        {
          Vector3 vector3 = Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction(this.positionTo, this.positionFrom), this.positionAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.positionSeparation))), this.positionFrom);
          ref UIVertex local1 = ref vert1;
          local1.position = (__Null) Vector3.op_Addition((Vector3) local1.position, vector3);
          ref UIVertex local2 = ref vert2;
          local2.position = (__Null) Vector3.op_Addition((Vector3) local2.position, vector3);
          ref UIVertex local3 = ref vert3;
          local3.position = (__Null) Vector3.op_Addition((Vector3) local3.position, vector3);
          ref UIVertex local4 = ref vert4;
          local4.position = (__Null) Vector3.op_Addition((Vector3) local4.position, vector3);
        }
        if (this.useScale)
        {
          if (this.scaleSyncXY)
          {
            float num1 = (this.scaleTo - this.scaleFrom) * this.scaleAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.scaleSeparation)) + this.scaleFrom;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num2 = (float) (((^(Vector3&) ref vert2.position).x - (^(Vector3&) ref vert4.position).x) * this.scalePivot.x + (^(Vector3&) ref vert4.position).x);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num3 = (float) (((^(Vector3&) ref vert2.position).y - (^(Vector3&) ref vert4.position).y) * this.scalePivot.y + (^(Vector3&) ref vert4.position).y);
            Vector3 vector3;
            ((Vector3) ref vector3).\u002Ector(num2, num3, 0.0f);
            vert1.position = (__Null) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert1.position, vector3), num1), vector3);
            vert2.position = (__Null) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert2.position, vector3), num1), vector3);
            vert3.position = (__Null) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert3.position, vector3), num1), vector3);
            vert4.position = (__Null) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert4.position, vector3), num1), vector3);
          }
          else
          {
            float num1 = (this.scaleTo - this.scaleFrom) * this.scaleAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.scaleSeparation)) + this.scaleFrom;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num2 = (float) (((^(Vector3&) ref vert2.position).x - (^(Vector3&) ref vert4.position).x) * this.scalePivot.x + (^(Vector3&) ref vert4.position).x);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num3 = (float) (((^(Vector3&) ref vert2.position).y - (^(Vector3&) ref vert4.position).y) * this.scalePivot.y + (^(Vector3&) ref vert4.position).y);
            Vector3 vector3;
            ((Vector3) ref vector3).\u002Ector(num2, num3, 0.0f);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert1.position = (__Null) new Vector3((float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert1.position, vector3), num1), vector3).x, (float) (^(Vector3&) ref vert1.position).y, (float) (^(Vector3&) ref vert1.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert2.position = (__Null) new Vector3((float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert2.position, vector3), num1), vector3).x, (float) (^(Vector3&) ref vert2.position).y, (float) (^(Vector3&) ref vert2.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert3.position = (__Null) new Vector3((float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert3.position, vector3), num1), vector3).x, (float) (^(Vector3&) ref vert3.position).y, (float) (^(Vector3&) ref vert3.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert4.position = (__Null) new Vector3((float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert4.position, vector3), num1), vector3).x, (float) (^(Vector3&) ref vert4.position).y, (float) (^(Vector3&) ref vert4.position).z);
            float num4 = (this.scaleToY - this.scaleFromY) * this.scaleAnimationCurveY.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.scaleSeparation)) + this.scaleFromY;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num5 = (float) (((^(Vector3&) ref vert2.position).x - (^(Vector3&) ref vert4.position).x) * this.scalePivotY.x + (^(Vector3&) ref vert4.position).x);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            float num6 = (float) (((^(Vector3&) ref vert2.position).y - (^(Vector3&) ref vert4.position).y) * this.scalePivotY.y + (^(Vector3&) ref vert4.position).y);
            ((Vector3) ref vector3).\u002Ector(num5, num6, 0.0f);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert1.position = (__Null) new Vector3((float) (^(Vector3&) ref vert1.position).x, (float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert1.position, vector3), num4), vector3).y, (float) (^(Vector3&) ref vert1.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert2.position = (__Null) new Vector3((float) (^(Vector3&) ref vert2.position).x, (float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert2.position, vector3), num4), vector3).y, (float) (^(Vector3&) ref vert2.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert3.position = (__Null) new Vector3((float) (^(Vector3&) ref vert3.position).x, (float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert3.position, vector3), num4), vector3).y, (float) (^(Vector3&) ref vert3.position).z);
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            vert4.position = (__Null) new Vector3((float) (^(Vector3&) ref vert4.position).x, (float) Vector3.op_Addition(Vector3.op_Multiply(Vector3.op_Subtraction((Vector3) vert4.position, vector3), num4), vector3).y, (float) (^(Vector3&) ref vert4.position).z);
          }
        }
        if (this.useRotation)
        {
          float num1 = (this.rotationTo - this.rotationFrom) * this.rotationAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.rotationSeparation)) + this.rotationFrom;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          float num2 = (float) (((^(Vector3&) ref vert2.position).x - (^(Vector3&) ref vert4.position).x) * this.rotationPivot.x + (^(Vector3&) ref vert4.position).x);
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          float num3 = (float) (((^(Vector3&) ref vert2.position).y - (^(Vector3&) ref vert4.position).y) * this.rotationPivot.y + (^(Vector3&) ref vert4.position).y);
          Vector3 vector3;
          ((Vector3) ref vector3).\u002Ector(num2, num3, 0.0f);
          vert1.position = (__Null) Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_forward()), Vector3.op_Subtraction((Vector3) vert1.position, vector3)), vector3);
          vert2.position = (__Null) Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_forward()), Vector3.op_Subtraction((Vector3) vert2.position, vector3)), vector3);
          vert3.position = (__Null) Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_forward()), Vector3.op_Subtraction((Vector3) vert3.position, vector3)), vector3);
          vert4.position = (__Null) Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.AngleAxis(num1, Vector3.get_forward()), Vector3.op_Subtraction((Vector3) vert4.position, vector3)), vector3);
        }
        Color color = Color32.op_Implicit((Color32) vert1.color);
        if (this.useColor)
        {
          color = Color.op_Addition(Color.op_Multiply(Color.op_Subtraction(this.colorTo, this.colorFrom), this.colorAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.colorSeparation))), this.colorFrom);
          vert1.color = vert2.color = vert3.color = vert4.color = (__Null) Color32.op_Implicit(color);
        }
        if (this.useAlpha)
        {
          float num = (this.alphaTo - this.alphaFrom) * this.alphaAnimationCurve.Evaluate(TypefaceAnimator.SeparationRate(this.progress, currentCharacterNumber, this.characterNumber, this.alphaSeparation)) + this.alphaFrom;
          ((Color) ref color).\u002Ector((float) color.r, (float) color.g, (float) color.b, (float) color.a * num);
          vert1.color = vert2.color = vert3.color = vert4.color = (__Null) Color32.op_Implicit(color);
        }
        verts[index] = vert1;
        verts[index + 1] = vert2;
        verts[index + 2] = vert3;
        verts[index + 3] = vert4;
      }
    }
  }

  private static float SeparationRate(
    float progress,
    int currentCharacterNumber,
    int characterNumber,
    float separation)
  {
    return Mathf.Clamp01((float) (((double) progress - (double) currentCharacterNumber * (double) separation / (double) characterNumber) / ((double) separation / (double) characterNumber + 1.0 - (double) separation)));
  }

  public enum TimeMode
  {
    Time,
    Speed,
  }

  public enum Style
  {
    Once,
    Loop,
    PingPong,
  }
}
