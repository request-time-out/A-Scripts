// Decompiled with JetBrains decompiler
// Type: Studio.OptionSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ConfigScene;
using UnityEngine;

namespace Studio
{
  public class OptionSystem : BaseSystem
  {
    public float cameraSpeedX = 1f;
    public float cameraSpeedY = 1f;
    public float cameraSpeed = 1f;
    public float manipulateSize = 1f;
    public float manipuleteSpeed = 1f;
    public bool autoHide = true;
    public Color colorFKHair = Color.get_white();
    public Color colorFKNeck = Color.get_white();
    public Color colorFKBreast = Color.get_white();
    public Color colorFKBody = Color.get_white();
    public Color colorFKRightHand = Color.get_white();
    public Color colorFKLeftHand = Color.get_white();
    public Color colorFKSkirt = Color.get_white();
    public bool lineFK = true;
    public Color colorFKItem = Color.get_white();
    public float _routeLineWidth = 1f;
    public bool routePointLimit = true;
    public int initialPosition;
    public int selectedState;
    public bool autoSelect;
    public int snap;
    public int _logo;
    public bool startupLoad;

    public OptionSystem(string elementName)
      : base(elementName)
    {
    }

    public int logo
    {
      get
      {
        return Mathf.Clamp(this._logo, 0, 9);
      }
      set
      {
        this._logo = Mathf.Clamp(value, 0, 9);
      }
    }

    public float routeLineWidth
    {
      get
      {
        return this._routeLineWidth * 16f;
      }
      set
      {
        this._routeLineWidth = value / 16f;
      }
    }

    public override void Init()
    {
      this.cameraSpeedX = 1f;
      this.cameraSpeedY = 1f;
      this.cameraSpeed = 1f;
      this.manipulateSize = 1f;
      this.manipuleteSpeed = 1f;
      this.initialPosition = 0;
      this.selectedState = 0;
      this.autoHide = true;
      this.autoSelect = false;
      this.snap = 0;
      this.colorFKHair = Color.get_white();
      this.colorFKNeck = Color.get_white();
      this.colorFKBreast = Color.get_white();
      this.colorFKBody = Color.get_white();
      this.colorFKRightHand = Color.get_white();
      this.colorFKLeftHand = Color.get_white();
      this.colorFKSkirt = Color.get_white();
      this.lineFK = true;
      this.colorFKItem = Color.get_white();
      this._logo = 0;
      this._routeLineWidth = 1f;
      this.routePointLimit = true;
      this.startupLoad = false;
    }

    public Color GetFKColor(int _idx)
    {
      switch (_idx)
      {
        case 0:
          return this.colorFKHair;
        case 1:
          return this.colorFKNeck;
        case 2:
          return this.colorFKBreast;
        case 3:
          return this.colorFKBody;
        case 4:
          return this.colorFKRightHand;
        case 5:
          return this.colorFKLeftHand;
        case 6:
          return this.colorFKSkirt;
        default:
          return Color.get_white();
      }
    }

    public void SetFKColor(int _idx, Color _color)
    {
      switch (_idx)
      {
        case 0:
          this.colorFKHair = _color;
          break;
        case 1:
          this.colorFKNeck = _color;
          break;
        case 2:
          this.colorFKBreast = _color;
          break;
        case 3:
          this.colorFKBody = _color;
          break;
        case 4:
          this.colorFKRightHand = _color;
          break;
        case 5:
          this.colorFKLeftHand = _color;
          break;
        case 6:
          this.colorFKSkirt = _color;
          break;
      }
    }
  }
}
