// Decompiled with JetBrains decompiler
// Type: UI_ColorPresets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Component.UI;
using Illusion.Extensions;
using IllusionUtility.GetUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ColorPresets : MonoBehaviour
{
  private const string colorPresetsFile = "ColorPresets.json";
  private const string sampleAssetBundle = "custom/colorsample.unity3d";
  private const string sampleAsset = "ColorPresets";
  private UI_ColorPresets.ColorInfo colorInfo;
  private string saveDir;
  private const int presetMax = 77;
  [SerializeField]
  private UI_ToggleEx[] tglFile;
  [SerializeField]
  private GameObject objTemplate;
  [SerializeField]
  private GameObject objNew;
  private Image imgNew;
  private Transform trfParent;
  private List<UI_ColorPresetsInfo> lstInfo;
  private Color _color;

  public UI_ColorPresets()
  {
    base.\u002Ector();
  }

  public event Action<Color> updateColorAction;

  public Color color
  {
    get
    {
      return this._color;
    }
    set
    {
      this._color = value;
      this.SetColor(this._color);
    }
  }

  private void Awake()
  {
    if (Object.op_Equality((Object) null, (Object) this.objNew))
      this.objNew = ((UnityEngine.Component) this).get_transform().FindLoop("New");
    if (Object.op_Inequality((Object) null, (Object) this.objNew))
    {
      Transform transform = this.objNew.get_transform().Find("imgColor");
      if (Object.op_Inequality((Object) null, (Object) transform))
        this.imgNew = (Image) ((UnityEngine.Component) transform).GetComponent<Image>();
      this.trfParent = this.objNew.get_transform().get_parent();
    }
    if (Object.op_Equality((Object) null, (Object) this.objTemplate))
      this.objTemplate = ((UnityEngine.Component) this).get_transform().FindLoop("TemplateColor");
    this.lstInfo.Clear();
  }

  private void Start()
  {
    this.saveDir = UserData.Path + "Custom/";
    this.LoadPresets();
    for (int index = 0; index < this.tglFile.Length; ++index)
      this.tglFile[index].set_isOn(false);
    if (!this.tglFile.SafeProc<UI_ToggleEx>(this.colorInfo.select, (Action<UI_ToggleEx>) (_t => _t.set_isOn(true))))
    {
      this.colorInfo.select = Mathf.Clamp(this.colorInfo.select, 0, this.tglFile.Length - 1);
      this.tglFile[this.colorInfo.select].set_isOn(true);
    }
    this.SetPreset(false);
    if (Object.op_Inequality((Object) null, (Object) this.objNew))
    {
      this.trfParent = this.objNew.get_transform().get_parent();
      Button component = (Button) this.objNew.GetComponent<Button>();
      if (Object.op_Inequality((Object) null, (Object) component))
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(component), (Action<M0>) (_ =>
        {
          this.AddNewPreset(this.color, false);
          this.SavePresets();
        }));
      this.objNew.SetActiveIfDifferent(3 != this.colorInfo.select);
      if (77 <= this.lstInfo.Count)
        this.objNew.SetActiveIfDifferent(false);
    }
    for (int index = 0; index < this.tglFile.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.tglFile[index].onValueChanged).AddListener(new UnityAction<bool>((object) new UI_ColorPresets.\u003CStart\u003Ec__AnonStorey0()
      {
        \u0024this = this,
        no = index
      }, __methodptr(\u003C\u003Em__0)));
    }
  }

  public int GetSelectIndex()
  {
    for (int index = 0; index < this.tglFile.Length; ++index)
    {
      if (this.tglFile[index].get_isOn())
        return index;
    }
    return 0;
  }

  public void SetColor(Color c)
  {
    if (!Object.op_Inequality((Object) null, (Object) this.objNew) || !Object.op_Inequality((Object) null, (Object) this.imgNew))
      return;
    ((Graphic) this.imgNew).set_color(c);
  }

  public void AddNewPreset(Color addColor, bool load = false)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    UI_ColorPresets.\u003CAddNewPreset\u003Ec__AnonStorey2 presetCAnonStorey2 = new UI_ColorPresets.\u003CAddNewPreset\u003Ec__AnonStorey2();
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey2.\u0024this = this;
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey2.addObj = (GameObject) Object.Instantiate<GameObject>((M0) this.objTemplate, this.trfParent);
    // ISSUE: reference to a compiler-generated field
    if (!Object.op_Inequality((Object) null, (Object) presetCAnonStorey2.addObj))
      return;
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    UI_ColorPresets.\u003CAddNewPreset\u003Ec__AnonStorey1 presetCAnonStorey1 = new UI_ColorPresets.\u003CAddNewPreset\u003Ec__AnonStorey1();
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey1.\u003C\u003Ef__ref\u00242 = presetCAnonStorey2;
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey1.idx = this.GetSelectIndex();
    // ISSUE: reference to a compiler-generated field
    ((Object) presetCAnonStorey2.addObj).set_name(string.Format("PresetColor", (object[]) Array.Empty<object>()));
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey2.addObj.get_transform().SetSiblingIndex(this.lstInfo.Count);
    this.objNew.get_transform().SetSiblingIndex(77);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey1.cpi = (UI_ColorPresetsInfo) presetCAnonStorey2.addObj.GetComponent<UI_ColorPresetsInfo>();
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey1.cpi.color = addColor;
    // ISSUE: reference to a compiler-generated field
    if (Object.op_Inequality((Object) null, (Object) presetCAnonStorey1.cpi.image))
    {
      // ISSUE: reference to a compiler-generated field
      ((Graphic) presetCAnonStorey1.cpi.image).set_color(addColor);
    }
    // ISSUE: reference to a compiler-generated field
    this.lstInfo.Add(presetCAnonStorey1.cpi);
    // ISSUE: reference to a compiler-generated field
    MouseButtonCheck mouseButtonCheck1 = (MouseButtonCheck) presetCAnonStorey2.addObj.AddComponent<MouseButtonCheck>();
    mouseButtonCheck1.isLeft = false;
    mouseButtonCheck1.isCenter = false;
    // ISSUE: method pointer
    mouseButtonCheck1.onPointerUp.AddListener(new UnityAction<PointerEventData>((object) presetCAnonStorey1, __methodptr(\u003C\u003Em__0)));
    // ISSUE: reference to a compiler-generated field
    UI_OnMouseOverMessageEx component = (UI_OnMouseOverMessageEx) presetCAnonStorey2.addObj.GetComponent<UI_OnMouseOverMessageEx>();
    if (Object.op_Inequality((Object) null, (Object) component))
      component.showMsgNo = this.colorInfo.select != 3 ? 0 : 1;
    // ISSUE: reference to a compiler-generated field
    MouseButtonCheck mouseButtonCheck2 = (MouseButtonCheck) presetCAnonStorey2.addObj.AddComponent<MouseButtonCheck>();
    mouseButtonCheck2.isRight = false;
    mouseButtonCheck2.isCenter = false;
    // ISSUE: method pointer
    mouseButtonCheck2.onPointerUp.AddListener(new UnityAction<PointerEventData>((object) presetCAnonStorey1, __methodptr(\u003C\u003Em__1)));
    if (!load)
    {
      // ISSUE: reference to a compiler-generated field
      this.colorInfo.SetList(presetCAnonStorey1.idx, this.lstInfo.Select<UI_ColorPresetsInfo, Color>((Func<UI_ColorPresetsInfo, Color>) (info => info.color)).ToList<Color>());
    }
    // ISSUE: reference to a compiler-generated field
    presetCAnonStorey2.addObj.SetActiveIfDifferent(true);
    if (77 > this.lstInfo.Count)
      return;
    this.objNew.SetActiveIfDifferent(false);
  }

  public void SetPreset(bool delete = false)
  {
    for (int index = this.lstInfo.Count - 1; index >= 0; --index)
      Object.Destroy((Object) ((UnityEngine.Component) this.lstInfo[index]).get_gameObject());
    this.lstInfo.Clear();
    int select = this.colorInfo.select;
    if (delete)
      this.colorInfo.DeleteList(select);
    List<Color> list = this.colorInfo.GetList(select);
    int count = list.Count;
    for (int index = 0; index < count; ++index)
      this.AddNewPreset(list[index], true);
  }

  public void SavePresets()
  {
    string path = this.saveDir + "ColorPresets.json";
    if (!Directory.Exists(this.saveDir))
      Directory.CreateDirectory(this.saveDir);
    string json = JsonUtility.ToJson((object) this.colorInfo);
    File.WriteAllText(path, json);
  }

  public void LoadPresets()
  {
    string path = this.saveDir + "ColorPresets.json";
    if (!File.Exists(path))
    {
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>("custom/colorsample.unity3d", "ColorPresets", false, string.Empty);
      if (!Object.op_Inequality((Object) null, (Object) textAsset))
        return;
      this.colorInfo = (UI_ColorPresets.ColorInfo) JsonUtility.FromJson<UI_ColorPresets.ColorInfo>(textAsset.get_text());
      AssetBundleManager.UnloadAssetBundle("custom/colorsample.unity3d", true, (string) null, false);
      this.SavePresets();
    }
    else
    {
      this.colorInfo = (UI_ColorPresets.ColorInfo) JsonUtility.FromJson<UI_ColorPresets.ColorInfo>(File.ReadAllText(path));
      if (this.colorInfo.lstColorSample.Count != 0)
        return;
      TextAsset textAsset = CommonLib.LoadAsset<TextAsset>("custom/colorsample.unity3d", "ColorPresets", false, string.Empty);
      if (!Object.op_Inequality((Object) null, (Object) textAsset))
        return;
      UI_ColorPresets.ColorInfo colorInfo = (UI_ColorPresets.ColorInfo) JsonUtility.FromJson<UI_ColorPresets.ColorInfo>(textAsset.get_text());
      AssetBundleManager.UnloadAssetBundle("custom/colorsample.unity3d", true, (string) null, false);
      this.colorInfo.lstColorSample = new List<Color>((IEnumerable<Color>) colorInfo.lstColorSample);
      this.SavePresets();
    }
  }

  public class ColorInfo
  {
    public int select = 3;
    public List<Color> lstColor01 = new List<Color>();
    public List<Color> lstColor02 = new List<Color>();
    public List<Color> lstColor03 = new List<Color>();
    public List<Color> lstColorSample = new List<Color>();

    public void SetList(int idx, List<Color> lst)
    {
      switch (idx)
      {
        case 0:
          this.lstColor01 = lst;
          break;
        case 1:
          this.lstColor02 = lst;
          break;
        case 2:
          this.lstColor03 = lst;
          break;
        case 3:
          this.lstColorSample = lst;
          break;
      }
    }

    public List<Color> GetList(int idx)
    {
      switch (idx)
      {
        case 0:
          return this.lstColor01;
        case 1:
          return this.lstColor02;
        case 2:
          return this.lstColor03;
        case 3:
          return this.lstColorSample;
        default:
          return (List<Color>) null;
      }
    }

    public void DeleteList(int idx)
    {
      switch (idx)
      {
        case 0:
          this.lstColor01.Clear();
          break;
        case 1:
          this.lstColor02.Clear();
          break;
        case 2:
          this.lstColor03.Clear();
          break;
        case 3:
          this.lstColorSample.Clear();
          break;
      }
    }
  }
}
