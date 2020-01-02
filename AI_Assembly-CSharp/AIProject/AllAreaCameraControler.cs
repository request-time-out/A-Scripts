// Decompiled with JetBrains decompiler
// Type: AIProject.AllAreaCameraControler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;
using AIProject.Scene;
using Manager;
using ReMotion;
using Rewired;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class AllAreaCameraControler : MonoBehaviour
  {
    private Image Cursor;
    private GameObject PutGirlName;
    private Image clickedLabelImage;
    private Text clickedLabelText;
    private AllAreaMapUI areaMapUI;
    private WarpListUI warpListUI;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Camera camUI;
    [SerializeField]
    private MiniMapControler MiniMap;
    [SerializeField]
    private float[] ScreenSize;
    [SerializeField]
    private int DefaultScreenSizeID;
    [Space]
    [SerializeField]
    private Vector2[] _MinCameraMoveLimit;
    [SerializeField]
    private Vector2[] _MaxCameraMoveLimit;
    private Vector2 CameraMoveLimitMin;
    private Vector2 CameraMoveLimitMax;
    [SerializeField]
    private float cursorMoveSpeed;
    [SerializeField]
    private float dragMoveSpeed;
    [Space]
    [SerializeField]
    private float iconSeachRange;
    [SerializeField]
    private float ZoomTime;
    [SerializeField]
    private float WheelZoomSpeed;
    [Space]
    [SerializeField]
    private float MarkerAppearTime;
    [SerializeField]
    private float MaxMarkerWaitTime;
    [Space]
    [SerializeField]
    private float MarkerChangeTime;
    [SerializeField]
    private float MarkerMinScl;
    [SerializeField]
    private float MarkerMaxScl;
    private float sclTime;
    private float MarkerChangeWaitTime;
    private bool MarkerChangeWait;
    private int ChangeMarkerMode;
    private Image[] GirlIcons;
    private Image MerchantIcon;
    private Canvas MerchantIconCanvas;
    private Manager.Input Input;
    private Vector3 cursolWorldPos;
    private float DefaultSize;
    private Vector2 CursorDefaultSize;
    private Vector3 TargetPos;
    private int PreZoomPattern;
    private int ZoomPattern;
    private float startZoom;
    private float targetZoom;
    private float NowZoomTime;
    private bool nowZooming;
    private bool SetCamCommmandBuf;
    [SerializeField]
    private MiniMapDepthTexture depthTexture;
    [SerializeField]
    private AllAreaCameraEffector cameraEffector;
    private Dictionary<MapUIActionCategory, BoolReactiveProperty> ActionFilter;
    private BoolReactiveProperty ActionFilterShip;
    private List<MiniMapControler.PointIconInfo> actionPointIcon;
    private List<MiniMapControler.PointIconInfo> actionPointHousingIcon;
    private List<Image> ActionPointIcon;
    private List<Image> ActionPointHousingIcon;
    private List<MiniMapControler.PetIconInfo> PetPointIcon;
    private List<MiniMapControler.IconInfo> BasePointIcon;
    private List<MiniMapControler.IconInfo> DevicePointIcon;
    private List<MiniMapControler.IconInfo> FarmPointIcon;
    private List<MiniMapControler.IconInfo> FarmPointHousingIcon;
    private List<MiniMapControler.IconInfo> EventPointIcon;
    private List<MiniMapControler.IconInfo> ShipPointIcon;
    private List<MiniMapControler.IconInfo> CraftPointIcon;
    private List<MiniMapControler.IconInfo> JukePointIcon;
    private bool[] IsLimit;
    private AllAreaCameraControler.CalcOnIconInfo OnActionPoint;
    public static bool NowWarp;
    private const int nCameraPosYoffset = 100;
    [SerializeField]
    private int ChickenIconID;
    [SerializeField]
    private int DragDeskIconID;
    [SerializeField]
    private int PetUnionIconID;
    [SerializeField]
    private int JukeIconID;
    public IDisposable WarpSelectSubscriber;

    public AllAreaCameraControler()
    {
      base.\u002Ector();
    }

    public Image ClickedLabelImage
    {
      get
      {
        return this.clickedLabelImage;
      }
    }

    public Text ClickedLabelText
    {
      get
      {
        return this.clickedLabelText;
      }
    }

    public void Init()
    {
      this.Input = Singleton<Manager.Input>.Instance;
      this.DefaultSize = this.cam.get_orthographicSize();
      if (this.ActionFilter != null)
        this.ActionFilter.Clear();
      for (int index = 0; index < 29; ++index)
        this.ActionFilter.Add((MapUIActionCategory) index, new BoolReactiveProperty(true));
      this.ActionFilterShip = new BoolReactiveProperty(true);
      this.areaMapUI = MapUIContainer.AllAreaMapUI;
      AllAreaMapObjects component = (AllAreaMapObjects) ((Component) this.areaMapUI).GetComponent<AllAreaMapObjects>();
      this.Cursor = component.Cursor;
      this.PutGirlName = component.PutGirlName;
      this.warpListUI = component.WarpListUI;
      this.clickedLabelImage = component.ClickedLabelImage;
      this.clickedLabelText = component.ClickedLabelText;
      Sprite sprite = this.clickedLabelImage.get_sprite();
      Singleton<Resources>.Instance.itemIconTables.ActorIconTable.TryGetValue(Singleton<Manager.Map>.Instance.Player.ID, out sprite);
      this.clickedLabelImage.set_sprite(sprite);
      this.clickedLabelText.set_text("：プレイヤー");
      this.CursorDefaultSize = ((Graphic) this.Cursor).get_rectTransform().get_sizeDelta();
      this.actionPointIcon = this.MiniMap.GetActionIconList();
      this.actionPointHousingIcon = this.MiniMap.GetActionHousingIconList();
      this.PetPointIcon = this.MiniMap.GetPetIconList();
      this.BasePointIcon = this.MiniMap.GetIconList(0);
      this.DevicePointIcon = this.MiniMap.GetIconList(1);
      this.FarmPointIcon = this.MiniMap.GetIconList(2);
      this.FarmPointHousingIcon = this.MiniMap.GetIconList(4);
      this.EventPointIcon = this.MiniMap.GetIconList(3);
      this.ShipPointIcon = this.MiniMap.GetIconList(5);
      this.CraftPointIcon = this.MiniMap.GetIconList(6);
      this.JukePointIcon = this.MiniMap.GetIconList(7);
      if (this.ActionPointIcon != null)
        this.ActionPointIcon.Clear();
      for (int index = 0; index < this.actionPointIcon.Count; ++index)
        this.ActionPointIcon.Add((Image) this.actionPointIcon[index].Icon.GetComponentInChildren<Image>());
      if (this.ActionPointHousingIcon != null)
        this.ActionPointHousingIcon.Clear();
      for (int index = 0; index < this.actionPointHousingIcon.Count; ++index)
        this.ActionPointHousingIcon.Add((Image) this.actionPointHousingIcon[index].Icon.GetComponentInChildren<Image>());
      this.InitPosition();
      Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(this.camUI, Singleton<Manager.Map>.Instance.Merchant.Position)).z = (__Null) 0.0;
      this.GirlIcons = this.MiniMap.GetGirlsIcon();
      this.MerchantIcon = Singleton<MapUIContainer>.Instance.MinimapUI.GetMerchantIcon();
      this.MerchantIconCanvas = Singleton<MapUIContainer>.Instance.MinimapUI.GetMerchantCanvas();
      for (int index1 = 0; index1 < 30; ++index1)
      {
        MapUIActionCategory index2 = (MapUIActionCategory) index1;
        if (index2 != MapUIActionCategory.HARBOR)
          ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.ActionFilter[index2], (System.Action<M0>) (x => this.CheckShowIconsFilter()));
        else
          ObservableExtensions.Subscribe<bool>((IObservable<M0>) this.ActionFilterShip, (System.Action<M0>) (x => this.CheckShowIconsFilter()));
      }
    }

    public void InitPosition()
    {
      Vector3 position1 = ((Component) this).get_transform().get_position();
      this.CameraMoveLimitMin = this._MinCameraMoveLimit[1];
      this.CameraMoveLimitMax = this._MaxCameraMoveLimit[1];
      Vector3 position2 = Singleton<Manager.Map>.Instance.Player.Position;
      position1.x = position2.x;
      position1.z = position2.z;
      this.IsLimit[0] = position1.x <= this.CameraMoveLimitMin.x;
      this.IsLimit[1] = position1.x >= this.CameraMoveLimitMax.x;
      this.IsLimit[2] = position1.z <= this.CameraMoveLimitMin.y;
      this.IsLimit[3] = position1.z >= this.CameraMoveLimitMax.y;
      if (this.IsLimit[0])
        position1.x = this.CameraMoveLimitMin.x;
      else if (this.IsLimit[1])
        position1.x = this.CameraMoveLimitMax.x;
      if (this.IsLimit[2])
        position1.z = this.CameraMoveLimitMin.y;
      else if (this.IsLimit[3])
        position1.z = this.CameraMoveLimitMax.y;
      ((Component) this).get_transform().set_position(position1);
      this.cursolWorldPos = position2;
      float num1 = (float) (this.cursolWorldPos.x - Singleton<Manager.Map>.Instance.Player.Position.x);
      float num2 = (float) (this.cursolWorldPos.z - Singleton<Manager.Map>.Instance.Player.Position.z);
      float num3 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      this.OnActionPoint.Name = "プレイヤー";
      this.OnActionPoint.OnIcon = true;
      this.OnActionPoint.Distance = num3;
      this.OnActionPoint.Position = Singleton<Manager.Map>.Instance.Player.Position;
      this.OnActionPoint.CanWarp = false;
      this.OnActionPoint.TargetObj = ((Component) Singleton<Manager.Map>.Instance.Player).get_gameObject();
      this.OnActionPoint.kind = 0;
      this.OnActionPoint.num = 0;
      this.TargetPos = this.OnActionPoint.Position;
      Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(this.camUI, this.cursolWorldPos);
      position2.x = screenPoint.x;
      position2.y = screenPoint.y;
      position2.z = (__Null) 0.0;
      ((Component) this.Cursor).get_transform().set_position(position2);
      ((Graphic) this.Cursor).set_color(new Color((float) ((Graphic) this.Cursor).get_color().r, (float) ((Graphic) this.Cursor).get_color().g, (float) ((Graphic) this.Cursor).get_color().b, 0.0f));
    }

    private void Update()
    {
      if (!((Behaviour) this.Cursor).get_enabled())
        ((Behaviour) this.Cursor).set_enabled(true);
      ((Component) this.Cursor).get_transform().get_position();
      Vector3 pos1 = GameCursor.pos;
      Vector3 position = ((Component) this).get_transform().get_position();
      bool flag1 = Object.op_Inequality((Object) Singleton<Game>.Instance.Dialog, (Object) null);
      if (!AllAreaCameraControler.NowWarp)
      {
        float num1 = this.Input.ScrollValue();
        if (!this.nowZooming)
        {
          bool flag2 = Object.op_Inequality((Object) EventSystem.get_current(), (Object) null) && !EventSystem.get_current().IsPointerOverGameObject();
          if ((double) num1 == 0.0)
          {
            if (this.ZoomPattern > 0 && (this.Input.IsPressedKey((KeyCode) 93) || this.Input.IsPressedKey(ActionID.RightShoulder2)))
            {
              --this.ZoomPattern;
              this.startZoom = this.cam.get_orthographicSize();
              this.NowZoomTime = 0.0f;
              this.targetZoom = this.ScreenSize[this.ZoomPattern];
            }
            else if (this.ZoomPattern < this.ScreenSize.Length - 1 && (this.Input.IsPressedKey((KeyCode) 61) || this.Input.IsPressedKey(ActionID.LeftShoulder2)))
            {
              ++this.ZoomPattern;
              this.startZoom = this.cam.get_orthographicSize();
              this.NowZoomTime = 0.0f;
              this.targetZoom = this.ScreenSize[this.ZoomPattern];
            }
            else if (this.Input.IsPressedKey((KeyCode) 59))
            {
              this.ZoomPattern = this.DefaultScreenSizeID;
              this.startZoom = this.cam.get_orthographicSize();
              this.NowZoomTime = 0.0f;
              this.targetZoom = this.ScreenSize[this.ZoomPattern];
            }
          }
          else if ((double) num1 != 0.0 && flag2)
          {
            float num2 = this.WheelZoomSpeed * num1;
            if ((double) num1 > 0.0 && (double) this.cam.get_orthographicSize() != (double) this.ScreenSize[0] || (double) num1 < 0.0 && (double) this.cam.get_orthographicSize() != (double) this.ScreenSize[this.ScreenSize.Length - 1])
            {
              Camera cam = this.cam;
              cam.set_orthographicSize(cam.get_orthographicSize() - num2);
              this.startZoom = this.targetZoom;
              float num3;
              if ((double) num1 < 0.0)
              {
                if ((double) this.cam.get_orthographicSize() > (double) this.ScreenSize[this.ScreenSize.Length - 1])
                  num3 = num2 + (this.cam.get_orthographicSize() - this.ScreenSize[this.ScreenSize.Length - 1]);
              }
              else if ((double) this.cam.get_orthographicSize() < (double) this.ScreenSize[0])
                num3 = num2 - (this.cam.get_orthographicSize() - this.ScreenSize[0]);
              this.cam.set_orthographicSize(Mathf.Clamp(this.cam.get_orthographicSize(), this.ScreenSize[0], this.ScreenSize[this.ScreenSize.Length - 1]));
              if ((double) this.cam.get_orthographicSize() <= (double) this.ScreenSize[1])
              {
                float num4 = Mathf.InverseLerp(this.ScreenSize[0], this.ScreenSize[1], this.cam.get_orthographicSize());
                this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[0], this._MinCameraMoveLimit[1], num4);
                this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[0], this._MaxCameraMoveLimit[1], num4);
              }
              else
              {
                float num4 = Mathf.InverseLerp(this.ScreenSize[1], this.ScreenSize[2], this.cam.get_orthographicSize());
                this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[1], this._MinCameraMoveLimit[2], num4);
                this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[1], this._MaxCameraMoveLimit[2], num4);
              }
            }
          }
        }
        this.CameraZoom();
        this.CheckShowIconsFilter();
        Vector2 cameraAxis = this.Input.CameraAxis;
        bool flag3 = true;
        Controller activeController = ((Player.ControllerHelper) ReInput.get_players().GetPlayer(0).controllers).GetLastActiveController();
        bool flag4 = activeController == null ? flag3 : activeController.get_type() == 1;
        if (!flag1 && !flag4)
        {
          ref Vector3 local1 = ref position;
          local1.x = (__Null) (local1.x - cameraAxis.x * (double) this.cursorMoveSpeed);
          ref Vector3 local2 = ref position;
          local2.z = (__Null) (local2.z - cameraAxis.y * (double) this.cursorMoveSpeed);
        }
        this.cam.ScreenToWorldPoint(pos1);
        if (this.Input.IsDown(ActionID.MouseLeft))
        {
          ref Vector3 local1 = ref position;
          local1.x = (__Null) (local1.x + this.Input.MouseAxis.x * (double) this.dragMoveSpeed * (double) this.cam.get_orthographicSize() / (double) this.ScreenSize[1]);
          ref Vector3 local2 = ref position;
          local2.z = (__Null) (local2.z + this.Input.MouseAxis.y * (double) this.dragMoveSpeed * (double) this.cam.get_orthographicSize() / (double) this.ScreenSize[1]);
        }
      }
      this.IsLimit[0] = position.x <= this.CameraMoveLimitMin.x;
      this.IsLimit[1] = position.x >= this.CameraMoveLimitMax.x;
      this.IsLimit[2] = position.z <= this.CameraMoveLimitMin.y;
      this.IsLimit[3] = position.z >= this.CameraMoveLimitMax.y;
      if (this.IsLimit[0])
        position.x = this.CameraMoveLimitMin.x;
      else if (this.IsLimit[1])
        position.x = this.CameraMoveLimitMax.x;
      if (this.IsLimit[2])
        position.z = this.CameraMoveLimitMin.y;
      else if (this.IsLimit[3])
        position.z = this.CameraMoveLimitMax.y;
      if (!flag1)
        ((Component) this).get_transform().set_position(position);
      if (this.OnActionPoint.kind == 1)
        this.TargetPos = ((Actor) this.OnActionPoint.TargetObj.GetComponentInChildren<AgentActor>()).Position;
      else if (this.OnActionPoint.kind == 2)
        this.TargetPos = ((Actor) this.OnActionPoint.TargetObj.GetComponentInChildren<MerchantActor>()).Position;
      if (this.TargetPos.x != this.cursolWorldPos.x && this.TargetPos.z != this.cursolWorldPos.z)
        this.cursolWorldPos = this.TargetPos;
      Vector3 vector3 = Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(this.camUI, this.cursolWorldPos));
      vector3.z = (__Null) 0.0;
      if (this.IsLimit[0])
      {
        float x = (float) RectTransformUtility.WorldToScreenPoint(this.camUI, new Vector3((float) (this.CameraMoveLimitMax.x + (double) this.cam.get_orthographicSize() * (double) this.cam.get_aspect()), 0.0f, 0.0f)).x;
        vector3.x = (__Null) (double) Mathf.Clamp((float) vector3.x, x, (float) (Screen.get_width() - 1) - (float) (this.CursorDefaultSize.x / 2.0));
      }
      else if (this.IsLimit[1])
      {
        float x = (float) RectTransformUtility.WorldToScreenPoint(this.camUI, new Vector3((float) (this.CameraMoveLimitMin.x - (double) this.cam.get_orthographicSize() * (double) this.cam.get_aspect()), 0.0f, 0.0f)).x;
        vector3.x = (__Null) (double) Mathf.Clamp((float) vector3.x, (float) (this.CursorDefaultSize.x / 2.0), x - (float) (this.CursorDefaultSize.x / 2.0));
      }
      if (this.IsLimit[2])
      {
        float y = (float) RectTransformUtility.WorldToScreenPoint(this.camUI, new Vector3(0.0f, 0.0f, (float) this.CameraMoveLimitMax.y + this.cam.get_orthographicSize())).y;
        vector3.y = (__Null) (double) Mathf.Clamp((float) vector3.y, y + (float) (this.CursorDefaultSize.y / 2.0), (float) (Screen.get_height() - 1) - (float) (this.CursorDefaultSize.y / 2.0));
      }
      else if (this.IsLimit[3])
      {
        float y = (float) RectTransformUtility.WorldToScreenPoint(this.camUI, new Vector3(0.0f, 0.0f, (float) this.CameraMoveLimitMin.y - this.cam.get_orthographicSize())).y;
        vector3.y = (__Null) (double) Mathf.Clamp((float) vector3.y, (float) (this.CursorDefaultSize.y / 2.0), y - (float) (this.CursorDefaultSize.y / 2.0));
      }
      ((Component) this.Cursor).get_transform().set_position(vector3);
      this.ChangeMarkerRotAndSize();
      this.camUI.set_orthographicSize(this.cam.get_orthographicSize());
      if (!this.Input.IsPressedKey(ActionID.MouseLeft) || flag1 || AllAreaCameraControler.NowWarp)
        return;
      Vector3 pos2 = GameCursor.pos;
      pos2.z = (__Null) 0.0;
      Vector3[] tmpPos = new Vector3[2]
      {
        this.cam.ScreenToWorldPoint(pos2),
        ((Component) this.cam).get_transform().get_position()
      };
      this.CheckNamePanel(tmpPos[0]);
      if (!this.OnActionPoint.OnIcon || !this.OnActionPoint.CanWarp)
        return;
      tmpPos[0].y = tmpPos[1].y;
      if (this.WarpSelectSubscriber != null)
      {
        if (AllAreaCameraControler.NowWarp)
          AllAreaCameraControler.NowWarp = false;
        this.WarpSelectSubscriber.Dispose();
      }
      this.WarpSelectSubscriber = ObservableExtensions.Subscribe<TimeInterval<float>>((IObservable<M0>) Observable.FrameTimeInterval<float>((IObservable<M0>) ObservableEasing.EaseOutQuint(1f, true), true), (System.Action<M0>) (x =>
      {
        if (!AllAreaCameraControler.NowWarp)
          AllAreaCameraControler.NowWarp = true;
        ((Component) this.cam).get_transform().set_position(Vector3.Lerp(tmpPos[1], tmpPos[0], ((TimeInterval<float>) ref x).get_Value()));
      }), (System.Action) (() => AllAreaCameraControler.NowWarp = false));
      ConfirmScene.Sentence = "このポイントに移動しますか";
      ConfirmScene.OnClickedYes = (System.Action) (() =>
      {
        MiniMapControler.OnWarp warpProc = this.MiniMap.WarpProc;
        if (warpProc != null)
          warpProc((BasePoint) ((Component) this.OnActionPoint.Point).GetComponent<BasePoint>());
        this.MiniMap.WarpProc = (MiniMapControler.OnWarp) null;
        this.MiniMap.ChangeCamera(false, true);
        this.Input.FocusLevel = 0;
        this.Input.ReserveState(Manager.Input.ValidType.Action);
        this.Input.SetupState();
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(true);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.OK_L);
        if (!AllAreaCameraControler.NowWarp)
          return;
        AllAreaCameraControler.NowWarp = false;
      });
      ConfirmScene.OnClickedNo = (System.Action) (() =>
      {
        this.Input.ClearMenuElements();
        this.Input.FocusLevel = this.warpListUI.FocusLevel;
        this.Input.MenuElements = this.warpListUI.MenuUIList;
        this.Input.ReserveState(Manager.Input.ValidType.UI);
        this.Input.SetupState();
        Singleton<Manager.Map>.Instance.Player.SetScheduledInteractionState(false);
        Singleton<Manager.Map>.Instance.Player.ReleaseInteraction();
        Singleton<Resources>.Instance.SoundPack.Play(SoundPack.SystemSE.Cancel);
        if (!AllAreaCameraControler.NowWarp)
          return;
        AllAreaCameraControler.NowWarp = false;
      });
      Singleton<Game>.Instance.LoadDialog();
    }

    public void CursorSet()
    {
      this.ChangeMarkerMode = 1;
      this.sclTime = 0.0f;
    }

    private void ChangeMarkerRotAndSize()
    {
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(1f, 1f, 1f);
      float num1 = (float) ((Component) this.Cursor).get_transform().get_localScale().x;
      Color color = ((Graphic) this.Cursor).get_color();
      Outline component = (Outline) ((Component) this.Cursor).GetComponent<Outline>();
      Color effectColor = ((Shadow) component).get_effectColor();
      if (this.ChangeMarkerMode == 1)
      {
        if (this.MarkerChangeWait)
        {
          if ((double) this.MarkerChangeWaitTime < (double) this.MaxMarkerWaitTime)
          {
            this.MarkerChangeWaitTime += Time.get_unscaledDeltaTime();
            return;
          }
          this.MarkerChangeWait = false;
          this.sclTime = 0.0f;
          this.ChangeMarkerMode = 0;
          return;
        }
        float num2 = this.sclTime / this.MarkerAppearTime;
        if ((double) num2 >= 1.0)
          num2 = 1f;
        num1 = Mathf.Lerp(2f, this.MarkerMinScl / (this.cam.get_orthographicSize() / this.ScreenSize[0]), num2);
        color.a = (__Null) (double) Mathf.Lerp(0.0f, 1f, num2);
        effectColor.a = (__Null) (double) Mathf.Lerp(0.0f, 1f, num2);
        if (Mathf.Approximately(num2, 1f))
        {
          this.MarkerChangeWait = true;
          this.MarkerChangeWaitTime = 0.0f;
        }
        ((Graphic) this.Cursor).set_color(color);
        ((Shadow) component).set_effectColor(effectColor);
      }
      else if (this.ChangeMarkerMode == 0)
      {
        if (Mathf.Approximately((float) color.a, 0.0f))
        {
          color.a = (__Null) 1.0;
          effectColor.a = (__Null) 1.0;
          ((Graphic) this.Cursor).set_color(color);
          ((Shadow) component).set_effectColor(effectColor);
        }
        num1 = (float) (((double) this.MarkerMaxScl - (double) this.MarkerMinScl) * ((double) Mathf.Sin(this.sclTime * 360f / this.MarkerChangeTime * ((float) Math.PI / 180f)) + 1.0) * 0.5 + (double) this.MarkerMinScl / ((double) this.cam.get_orthographicSize() / (double) this.ScreenSize[0]));
      }
      this.sclTime += Time.get_unscaledDeltaTime();
      vector3.x = (__Null) (double) (vector3.y = (__Null) num1);
      ((Component) this.Cursor).get_transform().set_localScale(vector3);
    }

    private void CheckNamePanel(Vector3 pos)
    {
      float[] numArray = new float[2];
      float resultLength = 0.0f;
      bool flag1 = false;
      numArray[0] = this.iconSeachRange * this.iconSeachRange;
      numArray[1] = numArray[0];
      AllAreaCameraControler.CalcOnIconInfo[] calcOnIconInfoArray = new AllAreaCameraControler.CalcOnIconInfo[14];
      for (int index = 0; index < calcOnIconInfoArray.Length; ++index)
      {
        calcOnIconInfoArray[index] = new AllAreaCameraControler.CalcOnIconInfo();
        calcOnIconInfoArray[index].Name = string.Empty;
        calcOnIconInfoArray[index].OnIcon = false;
        calcOnIconInfoArray[index].Distance = numArray[0];
        calcOnIconInfoArray[index].Position = Vector3.get_zero();
        calcOnIconInfoArray[index].CanWarp = false;
        calcOnIconInfoArray[index].TargetObj = (GameObject) null;
        calcOnIconInfoArray[index].Point = (Point) null;
      }
      if (this.CheckSerch(pos, Singleton<Manager.Map>.Instance.Player.Position, numArray[1], ref resultLength))
      {
        calcOnIconInfoArray[0].Name = "プレイヤー";
        calcOnIconInfoArray[0].OnIcon = true;
        calcOnIconInfoArray[0].Distance = resultLength;
        calcOnIconInfoArray[0].Position = Singleton<Manager.Map>.Instance.Player.Position;
        calcOnIconInfoArray[0].CanWarp = false;
        calcOnIconInfoArray[0].TargetObj = ((Component) Singleton<Manager.Map>.Instance.Player).get_gameObject();
        calcOnIconInfoArray[0].kind = 0;
        calcOnIconInfoArray[0].num = 0;
      }
      numArray[1] = numArray[0];
      using (IEnumerator<KeyValuePair<int, AgentActor>> enumerator = Singleton<Manager.Map>.Instance.AgentTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<int, AgentActor> current = enumerator.Current;
          if (!Object.op_Equality((Object) current.Value, (Object) null) && this.CheckSerch(pos, current.Value.Position, numArray[1], ref resultLength))
          {
            numArray[1] = resultLength;
            calcOnIconInfoArray[1].Name = current.Value.CharaName;
            calcOnIconInfoArray[1].OnIcon = true;
            calcOnIconInfoArray[1].Distance = resultLength;
            calcOnIconInfoArray[1].Position = current.Value.Position;
            calcOnIconInfoArray[1].CanWarp = false;
            calcOnIconInfoArray[1].TargetObj = ((Component) current.Value).get_gameObject();
            calcOnIconInfoArray[1].kind = 1;
            calcOnIconInfoArray[1].num = current.Key;
          }
        }
      }
      numArray[1] = numArray[0];
      if (this.CheckSerch(pos, Singleton<Manager.Map>.Instance.Merchant.Position, numArray[1], ref resultLength))
      {
        numArray[1] = resultLength;
        calcOnIconInfoArray[2].Name = Singleton<Manager.Map>.Instance.Merchant.CharaName;
        calcOnIconInfoArray[2].OnIcon = Singleton<Manager.Map>.Instance.Merchant.CurrentMode != Merchant.ActionType.Absent;
        calcOnIconInfoArray[2].Distance = resultLength;
        calcOnIconInfoArray[2].Position = Singleton<Manager.Map>.Instance.Merchant.Position;
        calcOnIconInfoArray[2].CanWarp = false;
        calcOnIconInfoArray[2].TargetObj = ((Component) Singleton<Manager.Map>.Instance.Merchant).get_gameObject();
        calcOnIconInfoArray[2].kind = 2;
        calcOnIconInfoArray[2].num = 0;
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.ActionPointIcon.Count; ++index)
      {
        if (((Behaviour) this.ActionPointIcon[index]).get_enabled() && this.actionPointIcon[index].Icon.get_activeSelf() && this.CheckSerch(pos, ((Component) this.ActionPointIcon[index]).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[3].Name = this.actionPointIcon[index].Name;
          calcOnIconInfoArray[3].OnIcon = true;
          calcOnIconInfoArray[3].Distance = resultLength;
          calcOnIconInfoArray[3].Position = ((Component) this.ActionPointIcon[index]).get_transform().get_position();
          calcOnIconInfoArray[3].CanWarp = false;
          calcOnIconInfoArray[3].TargetObj = ((Component) this.actionPointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[3].Point = (Point) this.actionPointIcon[index].Point;
          calcOnIconInfoArray[3].kind = 3;
          calcOnIconInfoArray[3].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.PetPointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.PetPointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.PetPointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[4].Name = this.PetPointIcon[index].Name;
          calcOnIconInfoArray[4].OnIcon = true;
          calcOnIconInfoArray[4].Distance = resultLength;
          calcOnIconInfoArray[4].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[4].CanWarp = false;
          calcOnIconInfoArray[4].TargetObj = this.PetPointIcon[index].obj;
          calcOnIconInfoArray[4].kind = 4;
          calcOnIconInfoArray[4].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.BasePointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.BasePointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.BasePointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[5].Name = this.BasePointIcon[index].Name;
          bool flag2 = false;
          if (!Singleton<Manager.Map>.Instance.GetBasePointOpenState(((BasePoint) ((Component) this.BasePointIcon[index].Point).GetComponent<BasePoint>()).ID, out flag2))
            flag2 = false;
          calcOnIconInfoArray[5].OnIcon = true;
          calcOnIconInfoArray[5].Distance = resultLength;
          calcOnIconInfoArray[5].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[5].CanWarp = flag2;
          calcOnIconInfoArray[5].TargetObj = ((Component) this.BasePointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[5].Point = this.BasePointIcon[index].Point;
          calcOnIconInfoArray[5].kind = 5;
          calcOnIconInfoArray[5].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.DevicePointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.DevicePointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.DevicePointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[6].Name = this.DevicePointIcon[index].Name;
          calcOnIconInfoArray[6].OnIcon = true;
          calcOnIconInfoArray[6].Distance = resultLength;
          calcOnIconInfoArray[6].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[6].CanWarp = false;
          calcOnIconInfoArray[6].TargetObj = ((Component) this.DevicePointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[6].Point = this.DevicePointIcon[index].Point;
          calcOnIconInfoArray[6].kind = 6;
          calcOnIconInfoArray[6].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.FarmPointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.FarmPointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.FarmPointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[7].Name = this.FarmPointIcon[index].Name;
          calcOnIconInfoArray[7].OnIcon = true;
          calcOnIconInfoArray[7].Distance = resultLength;
          calcOnIconInfoArray[7].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[7].CanWarp = false;
          calcOnIconInfoArray[7].TargetObj = ((Component) this.FarmPointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[7].Point = this.FarmPointIcon[index].Point;
          calcOnIconInfoArray[7].kind = 7;
          calcOnIconInfoArray[7].num = index;
        }
      }
      for (int index = 0; index < this.EventPointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.EventPointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.EventPointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[8].Name = this.EventPointIcon[index].Name;
          calcOnIconInfoArray[8].OnIcon = true;
          calcOnIconInfoArray[8].Distance = resultLength;
          calcOnIconInfoArray[8].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[8].CanWarp = false;
          calcOnIconInfoArray[8].TargetObj = ((Component) this.EventPointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[8].Point = this.EventPointIcon[index].Point;
          calcOnIconInfoArray[8].kind = 8;
          calcOnIconInfoArray[8].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.ActionPointHousingIcon.Count; ++index)
      {
        if (((Behaviour) this.ActionPointHousingIcon[index]).get_enabled() && this.actionPointHousingIcon[index].Icon.get_activeSelf() && this.CheckSerch(pos, ((Component) this.ActionPointHousingIcon[index]).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[9].Name = this.actionPointHousingIcon[index].Name;
          calcOnIconInfoArray[9].OnIcon = true;
          calcOnIconInfoArray[9].Distance = resultLength;
          calcOnIconInfoArray[9].Position = ((Component) this.ActionPointHousingIcon[index]).get_transform().get_position();
          calcOnIconInfoArray[9].CanWarp = false;
          calcOnIconInfoArray[9].TargetObj = ((Component) this.actionPointHousingIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[9].Point = (Point) this.actionPointHousingIcon[index].Point;
          calcOnIconInfoArray[9].kind = 3;
          calcOnIconInfoArray[9].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.FarmPointHousingIcon.Count; ++index)
      {
        if (((FarmPoint) ((Component) this.FarmPointHousingIcon[index].Point).GetComponent<FarmPoint>()).Kind != FarmPoint.FarmKind.Well)
        {
          Image componentInChildren = (Image) ((Component) this.FarmPointHousingIcon[index].Icon).GetComponentInChildren<Image>();
          if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.FarmPointHousingIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
          {
            numArray[1] = resultLength;
            calcOnIconInfoArray[10].Name = this.FarmPointHousingIcon[index].Name;
            calcOnIconInfoArray[10].OnIcon = true;
            calcOnIconInfoArray[10].Distance = resultLength;
            calcOnIconInfoArray[10].Position = ((Component) componentInChildren).get_transform().get_position();
            calcOnIconInfoArray[10].CanWarp = false;
            calcOnIconInfoArray[10].TargetObj = ((Component) this.FarmPointHousingIcon[index].Point).get_gameObject();
            calcOnIconInfoArray[10].Point = this.FarmPointHousingIcon[index].Point;
            calcOnIconInfoArray[10].kind = 10;
            calcOnIconInfoArray[10].num = index;
          }
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.ShipPointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.ShipPointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.ShipPointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[11].Name = this.ShipPointIcon[index].Name;
          calcOnIconInfoArray[11].OnIcon = true;
          calcOnIconInfoArray[11].Distance = resultLength;
          calcOnIconInfoArray[11].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[11].CanWarp = false;
          calcOnIconInfoArray[11].TargetObj = ((Component) this.ShipPointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[11].Point = this.ShipPointIcon[index].Point;
          calcOnIconInfoArray[11].kind = 11;
          calcOnIconInfoArray[11].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.CraftPointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.CraftPointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.CraftPointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[12].Name = this.CraftPointIcon[index].Name;
          calcOnIconInfoArray[12].OnIcon = true;
          calcOnIconInfoArray[12].Distance = resultLength;
          calcOnIconInfoArray[12].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[12].CanWarp = false;
          calcOnIconInfoArray[12].TargetObj = ((Component) this.CraftPointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[12].Point = this.CraftPointIcon[index].Point;
          calcOnIconInfoArray[12].kind = 12;
          calcOnIconInfoArray[12].num = index;
        }
      }
      numArray[1] = numArray[0];
      for (int index = 0; index < this.JukePointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.JukePointIcon[index].Icon).GetComponentInChildren<Image>();
        if (((Behaviour) componentInChildren).get_enabled() && ((Component) this.JukePointIcon[index].Icon).get_gameObject().get_activeSelf() && this.CheckSerch(pos, ((Component) componentInChildren).get_transform().get_position(), numArray[1], ref resultLength))
        {
          numArray[1] = resultLength;
          calcOnIconInfoArray[13].Name = this.JukePointIcon[index].Name;
          calcOnIconInfoArray[13].OnIcon = true;
          calcOnIconInfoArray[13].Distance = resultLength;
          calcOnIconInfoArray[13].Position = ((Component) componentInChildren).get_transform().get_position();
          calcOnIconInfoArray[13].CanWarp = false;
          calcOnIconInfoArray[13].TargetObj = ((Component) this.JukePointIcon[index].Point).get_gameObject();
          calcOnIconInfoArray[13].Point = this.JukePointIcon[index].Point;
          calcOnIconInfoArray[13].kind = 13;
          calcOnIconInfoArray[13].num = index;
        }
      }
      for (int index = 0; index < calcOnIconInfoArray.Length; ++index)
        flag1 |= calcOnIconInfoArray[index].OnIcon;
      int index1 = 0;
      if (flag1)
      {
        for (int index2 = 1; index2 < calcOnIconInfoArray.Length; ++index2)
        {
          int index3 = index2;
          if (calcOnIconInfoArray[index3].OnIcon)
            index1 = (double) calcOnIconInfoArray[index1].Distance >= (double) calcOnIconInfoArray[index3].Distance ? index3 : index1;
        }
        this.TargetPos = calcOnIconInfoArray[index1].Position;
        this.OnActionPoint.OnIcon = calcOnIconInfoArray[index1].OnIcon;
        this.OnActionPoint.CanWarp = calcOnIconInfoArray[index1].CanWarp;
        this.OnActionPoint.TargetObj = calcOnIconInfoArray[index1].TargetObj;
        this.OnActionPoint.Position = calcOnIconInfoArray[index1].Position;
        this.OnActionPoint.Distance = calcOnIconInfoArray[index1].Distance;
        this.OnActionPoint.Point = calcOnIconInfoArray[index1].Point;
        this.OnActionPoint.kind = calcOnIconInfoArray[index1].kind;
        this.OnActionPoint.num = calcOnIconInfoArray[index1].num;
        if (!this.OnActionPoint.OnIcon)
          return;
        this.clickedLabelText.set_text(string.Format("：{0}", (object) calcOnIconInfoArray[index1].Name));
        Sprite sprite = (Sprite) null;
        switch (this.OnActionPoint.kind)
        {
          case 0:
            Singleton<Resources>.Instance.itemIconTables.ActorIconTable.TryGetValue(Singleton<Manager.Map>.Instance.Player.ID, out sprite);
            break;
          case 1:
            Singleton<Resources>.Instance.itemIconTables.ActorIconTable.TryGetValue(Singleton<Manager.Map>.Instance.AgentTable.get_Item(this.OnActionPoint.num).ID, out sprite);
            break;
          case 2:
            sprite = ((Image) ((Component) this.MerchantIconCanvas).GetComponentInChildren<Image>()).get_sprite();
            break;
          case 3:
          case 9:
            ActionPoint component = (ActionPoint) ((Component) this.OnActionPoint.Point).GetComponent<ActionPoint>();
            int key1;
            Singleton<Resources>.Instance.itemIconTables.MiniMapIcon.TryGetValue(component.IDList.Length <= 0 ? component.ID : component.IDList[0], out key1);
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key1, out sprite);
            break;
          case 4:
            sprite = Singleton<Resources>.Instance.itemIconTables.CategoryIcon[9].Item2;
            break;
          case 5:
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.DefinePack.MinimapUIDefine.BaseIconID, out sprite);
            break;
          case 6:
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.DefinePack.MinimapUIDefine.DeviceIconID, out sprite);
            break;
          case 7:
          case 10:
            int key2 = Singleton<Resources>.Instance.DefinePack.MinimapUIDefine.FarmIconID;
            if (((FarmPoint) ((Component) this.OnActionPoint.Point).GetComponent<FarmPoint>()).Kind == FarmPoint.FarmKind.ChickenCoop)
              key2 = this.ChickenIconID;
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key2, out sprite);
            break;
          case 8:
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.DefinePack.MinimapUIDefine.EventIconID, out sprite);
            break;
          case 11:
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(Singleton<Resources>.Instance.CommonDefine.Icon.ShipIconID, out sprite);
            break;
          case 12:
            int key3 = this.DragDeskIconID;
            if (((CraftPoint) ((Component) this.OnActionPoint.Point).GetComponent<CraftPoint>()).Kind == CraftPoint.CraftKind.Pet)
              key3 = this.PetUnionIconID;
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(key3, out sprite);
            break;
          case 13:
            Singleton<Resources>.Instance.itemIconTables.ActionIconTable.TryGetValue(this.JukeIconID, out sprite);
            break;
        }
        this.clickedLabelImage.set_sprite(sprite);
        this.CursorSet();
      }
      else
        this.OnActionPoint.OnIcon = false;
    }

    private bool CheckSerch(
      Vector3 CursorPos,
      Vector3 TargetPos,
      float minLength,
      ref float resultLength)
    {
      float num1 = (float) (CursorPos.x - TargetPos.x);
      float num2 = (float) (CursorPos.z - TargetPos.z);
      resultLength = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      return (double) resultLength < (double) minLength;
    }

    public void Restart()
    {
      Vector3 position1 = ((Component) this).get_transform().get_position();
      this.CameraMoveLimitMin = this._MinCameraMoveLimit[1];
      this.CameraMoveLimitMax = this._MaxCameraMoveLimit[1];
      this.cam.set_orthographicSize(this.DefaultSize);
      this.camUI.set_orthographicSize(this.cam.get_orthographicSize());
      Vector3 position2 = Singleton<Manager.Map>.Instance.Player.Position;
      position1.x = position2.x;
      position1.z = position2.z;
      this.IsLimit[0] = position1.x <= this.CameraMoveLimitMin.x;
      this.IsLimit[1] = position1.x >= this.CameraMoveLimitMax.x;
      this.IsLimit[2] = position1.z <= this.CameraMoveLimitMin.y;
      this.IsLimit[3] = position1.z >= this.CameraMoveLimitMax.y;
      if (this.IsLimit[0])
        position1.x = this.CameraMoveLimitMin.x;
      else if (this.IsLimit[1])
        position1.x = this.CameraMoveLimitMax.x;
      if (this.IsLimit[2])
        position1.z = this.CameraMoveLimitMin.y;
      else if (this.IsLimit[3])
        position1.z = this.CameraMoveLimitMax.y;
      ((Component) this).get_transform().set_position(position1);
      Vector3 vector3 = Vector2.op_Implicit(RectTransformUtility.WorldToScreenPoint(this.camUI, position2));
      vector3.z = (__Null) 0.0;
      ((Component) this.Cursor).get_transform().set_position(vector3);
      this.GirlIcons = this.MiniMap.GetGirlsIcon();
      this.CheckShowIconsFilter();
    }

    private void CameraZoom()
    {
      this.NowZoomTime += Time.get_unscaledDeltaTime();
      float num1 = this.NowZoomTime / this.ZoomTime;
      if ((double) num1 >= 1.0)
        num1 = 1f;
      if ((double) this.startZoom < (double) this.targetZoom)
      {
        float num2 = Mathf.Abs(Mathf.Lerp(this.startZoom, this.targetZoom, num1) - this.cam.get_orthographicSize());
        Camera cam = this.cam;
        cam.set_orthographicSize(cam.get_orthographicSize() + num2);
        this.nowZooming = true;
        if ((double) this.cam.get_orthographicSize() <= (double) this.ScreenSize[1])
        {
          float num3 = Mathf.InverseLerp(this.ScreenSize[0], this.ScreenSize[1], this.cam.get_orthographicSize());
          this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[0], this._MinCameraMoveLimit[1], num3);
          this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[0], this._MaxCameraMoveLimit[1], num3);
        }
        else
        {
          float num3 = Mathf.InverseLerp(this.ScreenSize[1], this.ScreenSize[2], this.cam.get_orthographicSize());
          this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[1], this._MinCameraMoveLimit[2], num3);
          this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[1], this._MaxCameraMoveLimit[2], num3);
        }
      }
      else if ((double) this.startZoom > (double) this.targetZoom)
      {
        float num2 = Mathf.Abs(Mathf.Lerp(this.startZoom, this.targetZoom, num1) - this.cam.get_orthographicSize());
        Camera cam = this.cam;
        cam.set_orthographicSize(cam.get_orthographicSize() - num2);
        this.nowZooming = true;
        if ((double) this.cam.get_orthographicSize() <= (double) this.ScreenSize[1])
        {
          float num3 = Mathf.InverseLerp(this.ScreenSize[0], this.ScreenSize[1], this.cam.get_orthographicSize());
          this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[0], this._MinCameraMoveLimit[1], num3);
          this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[0], this._MaxCameraMoveLimit[1], num3);
        }
        else
        {
          float num3 = Mathf.InverseLerp(this.ScreenSize[1], this.ScreenSize[2], this.cam.get_orthographicSize());
          this.CameraMoveLimitMin = Vector2.Lerp(this._MinCameraMoveLimit[1], this._MinCameraMoveLimit[2], num3);
          this.CameraMoveLimitMax = Vector2.Lerp(this._MaxCameraMoveLimit[1], this._MaxCameraMoveLimit[2], num3);
        }
      }
      if ((double) num1 != 1.0)
        return;
      this.startZoom = this.targetZoom;
      this.nowZooming = false;
      if (this.ZoomPattern == this.DefaultScreenSizeID && this.PreZoomPattern != this.ZoomPattern)
      {
        this.CameraMoveLimitMin = this._MinCameraMoveLimit[1];
        this.CameraMoveLimitMax = this._MaxCameraMoveLimit[1];
      }
      this.PreZoomPattern = this.ZoomPattern;
    }

    public void ChangeTargetPos(Vector3 newPos)
    {
      this.TargetPos = newPos;
      this.CheckNamePanel(this.TargetPos);
    }

    public void ChangeactionFilter(MapUIActionCategory category, bool val)
    {
      if (category == MapUIActionCategory.HARBOR)
      {
        if (!this.areaMapUI.GameClear)
          return;
        ((ReactiveProperty<bool>) this.ActionFilterShip).set_Value(val);
      }
      else
        ((ReactiveProperty<bool>) this.ActionFilter[category]).set_Value(val);
    }

    public void ChangeActionFilterAllTgl(bool val)
    {
      ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.ALL]).set_Value(val);
    }

    private void CheckShowIconsFilter()
    {
      for (int index = 0; index < this.actionPointIcon.Count; ++index)
        ((Behaviour) this.ActionPointIcon[index]).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[this.actionPointIcon[index].Category]).get_Value());
      for (int index = 0; index < this.actionPointHousingIcon.Count; ++index)
        ((Behaviour) this.ActionPointHousingIcon[index]).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[this.actionPointHousingIcon[index].Category]).get_Value());
      if (this.GirlIcons != null)
      {
        for (int index = 0; index < this.GirlIcons.Length; ++index)
          ((Behaviour) this.GirlIcons[index]).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.HEROIN]).get_Value());
      }
      if (Object.op_Inequality((Object) this.MerchantIcon, (Object) null) && Singleton<Manager.Map>.Instance.Merchant.CurrentMode != Merchant.ActionType.Absent)
        ((Behaviour) this.MerchantIcon).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.SHOP]).get_Value());
      if (this.PetPointIcon != null && this.PetPointIcon.Count > 0)
      {
        for (int index = 0; index < this.PetPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.PetPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.PET]).get_Value());
        }
      }
      if (this.BasePointIcon != null && this.BasePointIcon.Count > 0)
      {
        for (int index = 0; index < this.BasePointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.BasePointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.WARP_POINT]).get_Value());
        }
      }
      if (this.DevicePointIcon != null && this.DevicePointIcon.Count > 0)
      {
        for (int index = 0; index < this.DevicePointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.DevicePointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.DRESSER]).get_Value());
        }
      }
      if (this.FarmPointIcon != null && this.FarmPointIcon.Count > 0)
      {
        bool flag1 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.FARM]).get_Value();
        bool flag2 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.CHICKENHOUSE]).get_Value();
        for (int index = 0; index < this.FarmPointIcon.Count; ++index)
        {
          FarmPoint.FarmKind kind = ((FarmPoint) ((Component) this.FarmPointIcon[index].Point).GetComponent<FarmPoint>()).Kind;
          if (kind != FarmPoint.FarmKind.Well)
          {
            Image componentInChildren = (Image) ((Component) this.FarmPointIcon[index].Icon).GetComponentInChildren<Image>(true);
            if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            {
              switch (kind)
              {
                case FarmPoint.FarmKind.Plant:
                  ((Behaviour) componentInChildren).set_enabled(flag1);
                  continue;
                case FarmPoint.FarmKind.ChickenCoop:
                  ((Behaviour) componentInChildren).set_enabled(flag2);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (this.FarmPointHousingIcon != null && this.FarmPointHousingIcon.Count > 0)
      {
        bool flag1 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.FARM]).get_Value();
        bool flag2 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.CHICKENHOUSE]).get_Value();
        for (int index = 0; index < this.FarmPointHousingIcon.Count; ++index)
        {
          FarmPoint.FarmKind kind = ((FarmPoint) ((Component) this.FarmPointHousingIcon[index].Point).GetComponent<FarmPoint>()).Kind;
          if (kind != FarmPoint.FarmKind.Well)
          {
            Image componentInChildren = (Image) ((Component) this.FarmPointHousingIcon[index].Icon).GetComponentInChildren<Image>(true);
            if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            {
              switch (kind)
              {
                case FarmPoint.FarmKind.Plant:
                  ((Behaviour) componentInChildren).set_enabled(flag1);
                  continue;
                case FarmPoint.FarmKind.ChickenCoop:
                  ((Behaviour) componentInChildren).set_enabled(flag2);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (this.ShipPointIcon != null && this.ShipPointIcon.Count > 0)
      {
        for (int index = 0; index < this.ShipPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.ShipPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(((ReactiveProperty<bool>) this.ActionFilterShip).get_Value());
        }
      }
      if (this.CraftPointIcon != null && this.CraftPointIcon.Count > 0)
      {
        bool flag1 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.DRAGDESK]).get_Value();
        bool flag2 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.PETUNION]).get_Value();
        bool flag3 = ((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.RECYCLE]).get_Value();
        for (int index = 0; index < this.CraftPointIcon.Count; ++index)
        {
          CraftPoint.CraftKind kind = ((CraftPoint) ((Component) this.CraftPointIcon[index].Point).GetComponent<CraftPoint>()).Kind;
          Image componentInChildren = (Image) ((Component) this.CraftPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          {
            switch (kind)
            {
              case CraftPoint.CraftKind.Medicine:
                ((Behaviour) componentInChildren).set_enabled(flag1);
                continue;
              case CraftPoint.CraftKind.Pet:
                ((Behaviour) componentInChildren).set_enabled(flag2);
                continue;
              case CraftPoint.CraftKind.Recycling:
                ((Behaviour) componentInChildren).set_enabled(flag3);
                continue;
              default:
                continue;
            }
          }
        }
      }
      if (this.JukePointIcon == null || this.JukePointIcon.Count <= 0)
        return;
      for (int index = 0; index < this.JukePointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.JukePointIcon[index].Icon).GetComponentInChildren<Image>(true);
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          ((Behaviour) componentInChildren).set_enabled(((ReactiveProperty<bool>) this.ActionFilter[MapUIActionCategory.MUSIC]).get_Value());
      }
    }

    public void ShowAllIcon()
    {
      for (int index = 0; index < this.actionPointIcon.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.ActionPointIcon[index], (Object) null))
          ((Behaviour) this.ActionPointIcon[index]).set_enabled(true);
      }
      for (int index = 0; index < this.actionPointHousingIcon.Count; ++index)
      {
        if (Object.op_Inequality((Object) this.ActionPointHousingIcon[index], (Object) null))
          ((Behaviour) this.ActionPointHousingIcon[index]).set_enabled(true);
      }
      this.GirlIcons = this.MiniMap.GetGirlsIcon();
      if (this.GirlIcons != null)
      {
        for (int index = 0; index < this.GirlIcons.Length; ++index)
        {
          if (Object.op_Inequality((Object) this.GirlIcons[index], (Object) null))
            ((Behaviour) this.GirlIcons[index]).set_enabled(true);
        }
      }
      if (Object.op_Inequality((Object) this.MerchantIcon, (Object) null) && Singleton<Manager.Map>.Instance.Merchant.CurrentMode != Merchant.ActionType.Absent && Object.op_Inequality((Object) this.MerchantIcon, (Object) null))
        ((Behaviour) this.MerchantIcon).set_enabled(true);
      if (this.PetPointIcon != null)
      {
        for (int index = 0; index < this.PetPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.PetPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.BasePointIcon != null)
      {
        for (int index = 0; index < this.BasePointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.BasePointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.DevicePointIcon != null)
      {
        for (int index = 0; index < this.DevicePointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.DevicePointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.FarmPointIcon != null)
      {
        for (int index = 0; index < this.FarmPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.FarmPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.FarmPointHousingIcon != null)
      {
        for (int index = 0; index < this.FarmPointHousingIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.FarmPointHousingIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.EventPointIcon != null)
      {
        for (int index = 0; index < this.EventPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.EventPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.ShipPointIcon != null)
      {
        for (int index = 0; index < this.ShipPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.ShipPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.CraftPointIcon != null)
      {
        for (int index = 0; index < this.CraftPointIcon.Count; ++index)
        {
          Image componentInChildren = (Image) ((Component) this.CraftPointIcon[index].Icon).GetComponentInChildren<Image>(true);
          if (Object.op_Inequality((Object) componentInChildren, (Object) null))
            ((Behaviour) componentInChildren).set_enabled(true);
        }
      }
      if (this.JukePointIcon == null)
        return;
      for (int index = 0; index < this.JukePointIcon.Count; ++index)
      {
        Image componentInChildren = (Image) ((Component) this.JukePointIcon[index].Icon).GetComponentInChildren<Image>(true);
        if (Object.op_Inequality((Object) componentInChildren, (Object) null))
          ((Behaviour) componentInChildren).set_enabled(true);
      }
    }

    public void RefleshActionHousingIcon()
    {
      for (int index = 0; index < this.ActionPointHousingIcon.Count; ++index)
        Object.Destroy((Object) ((Component) this.ActionPointHousingIcon[index]).get_gameObject());
      this.ActionPointHousingIcon.Clear();
      this.actionPointHousingIcon = this.MiniMap.GetActionHousingIconList();
      for (int index = 0; index < this.actionPointHousingIcon.Count; ++index)
        this.ActionPointHousingIcon.Add((Image) this.actionPointHousingIcon[index].Icon.GetComponentInChildren<Image>());
    }

    public void RefleshFarmHousingIcon()
    {
      this.FarmPointHousingIcon = this.MiniMap.GetIconList(4);
    }

    public void RefleshCraftIcon()
    {
      this.CraftPointIcon = this.MiniMap.GetIconList(6);
    }

    public void RefleshJukeIcon()
    {
      this.JukePointIcon = this.MiniMap.GetIconList(7);
    }

    public void SetPetPointIcon()
    {
      this.PetPointIcon = this.MiniMap.GetPetIconList();
    }

    public void AllIconReflesh()
    {
      this.actionPointIcon = this.MiniMap.GetActionIconList();
      this.actionPointHousingIcon = this.MiniMap.GetActionHousingIconList();
      this.PetPointIcon = this.MiniMap.GetPetIconList();
      this.BasePointIcon = this.MiniMap.GetIconList(0);
      this.DevicePointIcon = this.MiniMap.GetIconList(1);
      this.FarmPointIcon = this.MiniMap.GetIconList(2);
      this.FarmPointHousingIcon = this.MiniMap.GetIconList(4);
      this.EventPointIcon = this.MiniMap.GetIconList(3);
      this.ShipPointIcon = this.MiniMap.GetIconList(5);
      this.CraftPointIcon = this.MiniMap.GetIconList(6);
      this.JukePointIcon = this.MiniMap.GetIconList(7);
      if (this.ActionPointIcon != null)
      {
        using (List<Image>.Enumerator enumerator = this.ActionPointIcon.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Image current = enumerator.Current;
            if (!Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
              Object.Destroy((Object) ((Component) current).get_gameObject());
          }
        }
        this.ActionPointIcon.Clear();
      }
      if (this.ActionPointHousingIcon == null)
        return;
      using (List<Image>.Enumerator enumerator = this.ActionPointHousingIcon.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Image current = enumerator.Current;
          if (!Object.op_Equality((Object) ((Component) current).get_gameObject(), (Object) null))
            Object.Destroy((Object) ((Component) current).get_gameObject());
        }
      }
      this.ActionPointHousingIcon.Clear();
    }

    public void SetCameraCommandBuffer()
    {
      if (this.SetCamCommmandBuf)
        return;
      if (Object.op_Inequality((Object) this.depthTexture, (Object) null))
        this.depthTexture.Initialize();
      if (Object.op_Inequality((Object) this.cameraEffector, (Object) null))
        this.cameraEffector.Initialize();
      this.SetCamCommmandBuf = true;
    }

    public struct CalcOnIconInfo
    {
      public string Name;
      public bool OnIcon;
      public float Distance;
      public Vector3 Position;
      public bool CanWarp;
      public GameObject TargetObj;
      public Point Point;
      public MiniMapControler.IconInfo info;
      public int kind;
      public int num;
    }
  }
}
