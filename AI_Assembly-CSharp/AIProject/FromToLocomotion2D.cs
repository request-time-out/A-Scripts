// Decompiled with JetBrains decompiler
// Type: AIProject.FromToLocomotion2D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public abstract class FromToLocomotion2D : MonoBehaviour
  {
    [SerializeField]
    protected RectTransform _animationRoot;
    [SerializeField]
    protected EasingPair _alphaFadingTypes;
    [SerializeField]
    protected EasingPair _motionTypes;
    [SerializeField]
    protected Vector2 _source;
    [SerializeField]
    protected Vector2 _destination;
    protected const float _fadeDuration = 0.3f;

    protected FromToLocomotion2D()
    {
      base.\u002Ector();
    }

    public RectTransform AnimationRoot
    {
      get
      {
        return this._animationRoot;
      }
    }

    public Vector2 Source
    {
      get
      {
        return this._source;
      }
      set
      {
        this._source = value;
      }
    }

    public Vector2 Destination
    {
      get
      {
        return this._destination;
      }
      set
      {
        this._destination = value;
      }
    }

    private void Awake()
    {
    }

    protected void SetPosition(Vector2 diff, float t)
    {
      if (Object.op_Equality((Object) this._animationRoot, (Object) null))
        return;
      this._animationRoot.set_anchoredPosition(Vector2.op_Addition(this._destination, Vector2.op_Multiply(diff, t)));
    }
  }
}
