// Decompiled with JetBrains decompiler
// Type: AIProject.LockCameraOnUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class LockCameraOnUI : MonoBehaviour
  {
    [SerializeField]
    private RectTransform _baseCalcRect;
    [SerializeField]
    private ActorCameraControl _camera;

    public LockCameraOnUI()
    {
      base.\u002Ector();
    }

    public RectTransform BaseCalcRect
    {
      get
      {
        return this._baseCalcRect;
      }
      set
      {
        this._baseCalcRect = value;
      }
    }

    public ActorCameraControl Camera
    {
      get
      {
        return this._camera;
      }
      set
      {
        this._camera = value;
      }
    }

    private void Start()
    {
      this.SearchCanvas();
      if (Object.op_Equality((Object) this._camera, (Object) null))
        this._camera = (ActorCameraControl) ((Component) UnityEngine.Camera.get_main())?.GetComponent<ActorCameraControl>();
      ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), ((Component) this).get_gameObject()), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
    }

    private void SearchCanvas()
    {
      GameObject gameObject;
      for (gameObject = ((Component) this).get_gameObject(); !Object.op_Implicit((Object) gameObject.GetComponent<Canvas>()); gameObject = ((Component) gameObject.get_transform().get_parent()).get_gameObject())
      {
        if (Object.op_Equality((Object) gameObject.get_transform().get_parent(), (Object) null))
          return;
      }
      this._baseCalcRect = gameObject.get_transform() as RectTransform;
    }

    private void OnUpdate()
    {
      if (Object.op_Equality((Object) this._camera, (Object) null))
        return;
      if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
      {
        if (!((Behaviour) this._camera.CinemachineBrain).get_enabled())
          return;
        ((Behaviour) this._camera.CinemachineBrain).set_enabled(true);
      }
      else
      {
        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
          return;
        RectTransform transform = ((Component) this).get_transform() as RectTransform;
        float x1 = (float) Input.get_mousePosition().x;
        float y1 = (float) Input.get_mousePosition().y;
        int num1;
        if (((Transform) transform).get_position().x <= (double) x1)
        {
          double num2 = (double) x1;
          // ISSUE: variable of the null type
          __Null x2 = ((Transform) transform).get_position().x;
          Rect rect = transform.get_rect();
          double num3 = (double) ((Rect) ref rect).get_width() * ((Transform) this._baseCalcRect).get_localScale().x;
          double num4 = x2 + num3;
          num1 = num2 <= num4 ? 1 : 0;
        }
        else
          num1 = 0;
        bool flag1 = num1 != 0;
        int num5;
        if (((Transform) transform).get_position().y <= (double) y1)
        {
          double num2 = (double) y1;
          // ISSUE: variable of the null type
          __Null y2 = ((Transform) transform).get_position().y;
          Rect rect = transform.get_rect();
          double num3 = (double) ((Rect) ref rect).get_height() * ((Transform) this._baseCalcRect).get_localScale().y;
          double num4 = y2 + num3;
          num5 = num2 <= num4 ? 1 : 0;
        }
        else
          num5 = 0;
        bool flag2 = num5 != 0;
        if (!flag1 || !flag2)
          return;
        ((Behaviour) this._camera.CinemachineBrain).set_enabled(false);
      }
    }
  }
}
