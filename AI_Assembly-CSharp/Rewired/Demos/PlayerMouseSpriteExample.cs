// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.PlayerMouseSpriteExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class PlayerMouseSpriteExample : MonoBehaviour
  {
    [Tooltip("The Player that will control the mouse")]
    public int playerId;
    [Tooltip("The Rewired Action used for the mouse horizontal axis.")]
    public string horizontalAction;
    [Tooltip("The Rewired Action used for the mouse vertical axis.")]
    public string verticalAction;
    [Tooltip("The Rewired Action used for the mouse wheel axis.")]
    public string wheelAction;
    [Tooltip("The Rewired Action used for the mouse left button.")]
    public string leftButtonAction;
    [Tooltip("The Rewired Action used for the mouse right button.")]
    public string rightButtonAction;
    [Tooltip("The Rewired Action used for the mouse middle button.")]
    public string middleButtonAction;
    [Tooltip("The distance from the camera that the pointer will be drawn.")]
    public float distanceFromCamera;
    [Tooltip("The scale of the sprite pointer.")]
    public float spriteScale;
    [Tooltip("The pointer prefab.")]
    public GameObject pointerPrefab;
    [Tooltip("The click effect prefab.")]
    public GameObject clickEffectPrefab;
    [Tooltip("Should the hardware pointer be hidden?")]
    public bool hideHardwarePointer;
    [NonSerialized]
    private GameObject pointer;
    [NonSerialized]
    private PlayerMouse mouse;

    public PlayerMouseSpriteExample()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.pointer = (GameObject) Object.Instantiate<GameObject>((M0) this.pointerPrefab);
      this.pointer.get_transform().set_localScale(new Vector3(this.spriteScale, this.spriteScale, this.spriteScale));
      if (this.hideHardwarePointer)
        Cursor.set_visible(false);
      this.mouse = PlayerMouse.Factory.Create();
      ((PlayerController) this.mouse).set_playerId(this.playerId);
      ((PlayerController.ElementWithSource) this.mouse.get_xAxis()).set_actionName(this.horizontalAction);
      ((PlayerController.ElementWithSource) this.mouse.get_yAxis()).set_actionName(this.verticalAction);
      ((PlayerController.ElementWithSource) this.mouse.get_wheel().get_yAxis()).set_actionName(this.wheelAction);
      ((PlayerController.ElementWithSource) this.mouse.get_leftButton()).set_actionName(this.leftButtonAction);
      ((PlayerController.ElementWithSource) this.mouse.get_rightButton()).set_actionName(this.rightButtonAction);
      ((PlayerController.ElementWithSource) this.mouse.get_middleButton()).set_actionName(this.middleButtonAction);
      this.mouse.set_pointerSpeed(1f);
      this.mouse.get_wheel().get_yAxis().set_repeatRate(5f);
      this.mouse.set_screenPosition(new Vector2((float) Screen.get_width() * 0.5f, (float) Screen.get_height() * 0.5f));
      this.mouse.add_ScreenPositionChangedEvent(new Action<Vector2>(this.OnScreenPositionChanged));
      this.OnScreenPositionChanged(this.mouse.get_screenPosition());
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      this.pointer.get_transform().Rotate(Vector3.get_forward(), ((PlayerController.Axis) this.mouse.get_wheel().get_yAxis()).get_value() * 20f);
      if (this.mouse.get_leftButton().get_justPressed())
        this.CreateClickEffect(new Color(0.0f, 1f, 0.0f, 1f));
      if (this.mouse.get_rightButton().get_justPressed())
        this.CreateClickEffect(new Color(1f, 0.0f, 0.0f, 1f));
      if (!this.mouse.get_middleButton().get_justPressed())
        return;
      this.CreateClickEffect(new Color(1f, 1f, 0.0f, 1f));
    }

    private void CreateClickEffect(Color color)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.clickEffectPrefab);
      gameObject.get_transform().set_localScale(new Vector3(this.spriteScale, this.spriteScale, this.spriteScale));
      gameObject.get_transform().set_position(Camera.get_main().ScreenToWorldPoint(new Vector3((float) this.mouse.get_screenPosition().x, (float) this.mouse.get_screenPosition().y, this.distanceFromCamera)));
      ((SpriteRenderer) gameObject.GetComponentInChildren<SpriteRenderer>()).set_color(color);
      Object.Destroy((Object) gameObject, 0.5f);
    }

    private void OnScreenPositionChanged(Vector2 position)
    {
      this.pointer.get_transform().set_position(Camera.get_main().ScreenToWorldPoint(new Vector3((float) position.x, (float) position.y, this.distanceFromCamera)));
    }
  }
}
