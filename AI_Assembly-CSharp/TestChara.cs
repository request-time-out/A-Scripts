// Decompiled with JetBrains decompiler
// Type: TestChara
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using IllusionUtility.GetUtility;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

public class TestChara : MonoBehaviour
{
  private static List<ValueTuple<string, string>> _charaAssetBundleList = new List<ValueTuple<string, string>>();
  private static string[] _boneNames = new string[2]
  {
    "cf_J",
    "cm_J"
  };
  private static string[] _m_mozaicparts = new string[2]
  {
    "cm_o_dan00",
    "cm_o_dan_f"
  };
  private static string[] _f_mozaicparts = new string[2]
  {
    "o_mnpa",
    "o_mnpb"
  };
  private static readonly int _Smoothness = Shader.PropertyToID(nameof (_Smoothness));
  private static readonly int[] _idx = new int[7]
  {
    2,
    3,
    4,
    5,
    6,
    7,
    8
  };
  private static readonly int _NamidaScale = Shader.PropertyToID(nameof (_NamidaScale));
  private static readonly int _Texture4Scale = Shader.PropertyToID(nameof (_Texture4Scale));
  [SerializeField]
  [Label("初期化時にキャラを作成するか")]
  private bool _loadOnStart;
  [SerializeField]
  private bool _applySelfAnimatorParameter;
  [Header("Bone")]
  [SerializeField]
  private GameObject _boneBody;
  [SerializeField]
  private GameObject _boneHead;
  [SerializeField]
  private GameObject _body;
  [SerializeField]
  private GameObject _head;
  [SerializeField]
  private GameObject _hairBack;
  [SerializeField]
  private GameObject _hairFront;
  [SerializeField]
  private GameObject _clothTop;
  [SerializeField]
  private GameObject _clothBot;
  [SerializeField]
  private GameObject _bra;
  [SerializeField]
  private GameObject _shorts;
  [SerializeField]
  private GameObject _glove;
  [SerializeField]
  private GameObject _socks;
  [SerializeField]
  private GameObject _shoes;
  [SerializeField]
  [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
  private TestChara.AcsGenerateInfo[] _acsArray;
  [SerializeField]
  private Animator _animBody;
  [SerializeField]
  private RuntimeAnimatorController _rac;
  [SerializeField]
  private GameObject _objHitBody;
  [SerializeField]
  private GameObject _objHitHead;
  [SerializeField]
  private string _shapeFaceName;
  private ShapeHeadInfoFemale _shapeFace;
  private ShapeBodyInfoFemale _shapeBody;
  protected AssignedAnotherWeights _aaWeightsBody;
  protected AssignedAnotherWeights _aaWeightsHead;
  private float _nipStand;
  private Dictionary<int, GameObject> _dictRefObj;
  private Dictionary<int, GameObject> _dictRefObjNew;
  private float[] _shapeValuesFace;
  private float[] _shapeValues;
  private bool _updateShapeFace;
  private bool _updateShapeBody;
  private readonly int[] _bustChangeSettingPtn;
  private bool _visibleMozaic;
  private List<GameObject> _objMozaicList;
  private AnimationKeyInfo _anmKeyInfo;
  [SerializeField]
  private byte _sex;
  private float _tearsRate;
  private float _hohoAkaRate;
  private string deleteBodyBoneName;
  private string deleteHeadBoneName;
  private float[] _workValue;
  private float[] _defaultBustValue;
  private bool _disableShapeMouth;
  private bool[] _disableShapeBustLAry;
  private bool[] _disableShapeBustRAry;
  private bool _disableShapeNipL;
  private bool _disableShapeNipR;
  public const ulong FbxTypeBone = 1;
  public const ulong FbxTypeBody = 2;
  public const ulong FbxTypeHead = 3;
  public const ulong FbxTypeHairB = 4;
  public const ulong FbxTypeHairF = 5;
  public const ulong FbxTypeHairS = 6;
  public const ulong FbxTypeHairO = 7;
  public const ulong FbxTypeBase = 8;
  public const ulong FbxTypeCTop = 9;
  public const ulong FbxTypeCBot = 10;
  public const ulong FbxTypeSwim = 11;
  public const ulong FbxTypeSTop = 12;
  public const ulong FbxTypeSBot = 13;
  public const ulong FbxTypeBra = 14;
  public const ulong FbxTypeShorts = 15;
  public const ulong FbxTypePanst = 16;
  public const ulong FbxTypeGloves = 17;
  public const ulong FbxTypeSocks = 18;
  public const ulong FbxTypeShoes = 19;
  public const ulong FbxTypeAcs01 = 20;
  public const ulong FbxTypeAcs02 = 21;
  public const ulong FbxTypeAcs03 = 22;
  public const ulong FbxTypeAcs04 = 23;
  public const ulong FbxTypeAcs05 = 24;
  public const ulong FbxTypeAcs06 = 25;
  public const ulong FbxTypeAcs07 = 26;
  public const ulong FbxTypeAcs08 = 27;
  public const ulong FbxTypeAcs09 = 28;
  public const ulong FbxTypeAcs10 = 29;
  public const ulong FbxTypeBeard = 30;
  public const ulong FbxTypeSiruTop = 31;
  public const ulong FbxTypeSiruBot = 32;
  public const ulong FbxTypeSiruBra = 33;
  public const ulong FbxTypeSiruShorts = 34;
  public const ulong FbxTypeSiruSwim = 35;
  public const ulong FbxTypeSiruHairB = 36;
  public const ulong FbxTypeSiruHairF = 37;
  private Dictionary<int, List<GameObject>> _dictTagObj;

  public TestChara()
  {
    base.\u002Ector();
  }

  public static SortedDictionary<int, TestChara> CharaTable { get; private set; } = new SortedDictionary<int, TestChara>();

  public static bool BareFootState { get; set; } = true;

  public static TestChara CreateFemale(GameObject parent, int id)
  {
    int num = TestChara.SearchUnusedNo();
    string ab = "actor/prefabs/00.unity3d";
    string manifest = "abdata";
    GameObject gameObject = CommonLib.LoadAsset<GameObject>(ab, "TestFemale", false, manifest);
    if (!TestChara._charaAssetBundleList.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == ab && (string) x.Item2 == manifest)))
      TestChara._charaAssetBundleList.Add(new ValueTuple<string, string>(ab, manifest));
    GameObject objRoot = (GameObject) Object.Instantiate<GameObject>((M0) gameObject);
    ((Object) objRoot).set_name(string.Format("chaF{0}", (object) num.ToString("00")));
    if (Object.op_Implicit((Object) parent))
      objRoot.get_transform().SetParent(parent.get_transform(), false);
    TestChara component = (TestChara) objRoot.GetComponent<TestChara>();
    if (Object.op_Implicit((Object) component))
      component.InitializeFemale(objRoot, id, num);
    TestChara.CharaTable.Add(num, component);
    return component;
  }

  public static TestChara CreateMale(GameObject parent, int id)
  {
    int num = TestChara.SearchUnusedNo();
    string ab = "actor/prefabs/00.unity3d";
    string manifest = "abdata";
    GameObject gameObject = CommonLib.LoadAsset<GameObject>(ab, "TestMale", false, manifest);
    if (!TestChara._charaAssetBundleList.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == ab && (string) x.Item2 == manifest)))
      TestChara._charaAssetBundleList.Add(new ValueTuple<string, string>(ab, manifest));
    GameObject objRoot = (GameObject) Object.Instantiate<GameObject>((M0) gameObject);
    ((Object) objRoot).set_name(string.Format("chaM{0}", (object) num.ToString("00")));
    if (Object.op_Implicit((Object) parent))
      objRoot.get_transform().SetParent(parent.get_transform(), false);
    TestChara component = (TestChara) objRoot.GetComponent<TestChara>();
    if (Object.op_Implicit((Object) component))
      component.InitializeMale(objRoot, id, num);
    TestChara.CharaTable.Add(num, component);
    return component;
  }

  public static TestChara CreateMerchant(GameObject parent, int id)
  {
    int num = TestChara.SearchUnusedNo();
    string ab = "actor/prefabs/00.unity3d";
    string manifest = "abdata";
    GameObject gameObject = CommonLib.LoadAsset<GameObject>(ab, "TestMerchant", false, manifest);
    if (!TestChara._charaAssetBundleList.Exists((Predicate<ValueTuple<string, string>>) (x => (string) x.Item1 == ab && (string) x.Item2 == manifest)))
      TestChara._charaAssetBundleList.Add(new ValueTuple<string, string>(ab, manifest));
    GameObject objRoot = (GameObject) Object.Instantiate<GameObject>((M0) gameObject);
    ((Object) objRoot).set_name(string.Format("chaF{0}", (object) num.ToString("00")));
    if (Object.op_Implicit((Object) parent))
      objRoot.get_transform().SetParent(parent.get_transform(), false);
    TestChara component = (TestChara) objRoot.GetComponent<TestChara>();
    if (Object.op_Implicit((Object) component))
      component.InitializeFemale(objRoot, id, num);
    TestChara.CharaTable.Add(num, component);
    return component;
  }

  public static bool Delete(TestChara chara)
  {
    foreach (KeyValuePair<int, TestChara> keyValuePair in TestChara.CharaTable)
    {
      if (Object.op_Equality((Object) keyValuePair.Value, (Object) chara))
      {
        if (Object.op_Implicit((Object) keyValuePair.Value))
          Object.Destroy((Object) ((Component) keyValuePair.Value).get_gameObject());
        TestChara.CharaTable.Remove(keyValuePair.Key);
        return true;
      }
    }
    return false;
  }

  public static void DeleteCharaAlll()
  {
    foreach (KeyValuePair<int, TestChara> keyValuePair in TestChara.CharaTable)
    {
      if (Object.op_Implicit((Object) keyValuePair.Value))
        Object.Destroy((Object) ((Component) keyValuePair.Value).get_gameObject());
    }
    TestChara.CharaTable.Clear();
  }

  private static int SearchUnusedNo()
  {
    int num = 0;
    foreach (KeyValuePair<int, TestChara> keyValuePair in TestChara.CharaTable)
    {
      if (num == keyValuePair.Key)
        ++num;
      else
        break;
    }
    return num;
  }

  public bool ApplySelf
  {
    get
    {
      return this._applySelfAnimatorParameter;
    }
    set
    {
      this._applySelfAnimatorParameter = value;
    }
  }

  public Animator AnimBody
  {
    get
    {
      return this._animBody;
    }
  }

  public RuntimeAnimatorController Rac
  {
    get
    {
      return this._rac;
    }
  }

  public int CharID { get; private set; }

  public int LoadNo { get; private set; }

  public GameObject ObjRoot { get; private set; }

  public GameObject ObjAnim { get; private set; }

  public GameObject ObjBodyBone { get; private set; }

  public GameObject ObjHeadBone { get; private set; }

  public GameObject ObjBody { get; private set; }

  public GameObject ObjHead { get; private set; }

  public GameObject ObjHairBack { get; private set; }

  public GameObject ObjHairFront { get; private set; }

  public GameObject ObjClothTop { get; private set; }

  public GameObject ObjClothBot { get; private set; }

  public GameObject ObjBra { get; private set; }

  public GameObject ObjShorts { get; private set; }

  public GameObject ObjGlove { get; private set; }

  public GameObject ObjSocks { get; private set; }

  public GameObject ObjShoes { get; private set; }

  public GameObject[] ObjAcsArray { get; private set; }

  public GameObject ObjHitBody { get; protected set; }

  public GameObject ObjHitHead { get; protected set; }

  public Dictionary<ChaControlDefine.DynamicBoneKind, DynamicBone_Ver02> DictDynamicBone { get; private set; }

  public NeckLookControllerVer2 NeckLookControl { get; private set; }

  public EyeLookController EyeLookControl { get; private set; }

  public FaceBlendShape FBSCtrl { get; private set; }

  public FBSCtrlEyebrow EyebrowCtrl { get; private set; }

  public FBSCtrlEyes EyesCtrl { get; private set; }

  public FBSCtrlMouth MouthCtrl { get; private set; }

  public byte[] SiruNewLv { get; private set; }

  public Renderer FaceRenderer { get; private set; }

  public Renderer EyeRendererL { get; private set; }

  public Renderer EyeRendererR { get; private set; }

  public Renderer TearRenderer { get; private set; }

  public Material FaceMaterial { get; private set; }

  public Material EyeLMaterial { get; private set; }

  public Material EyeRMaterial { get; private set; }

  public Material TearMaterial { get; private set; }

  public bool ResetupDynamicBone { get; set; }

  public bool ResetupDynamicBoneBust { get; set; }

  public bool UpdateBustSize { get; set; }

  public GameObject ObjTop { get; private set; }

  public bool VisibleMozaiz
  {
    get
    {
      return this._visibleMozaic;
    }
    set
    {
      if (this._visibleMozaic == value)
        return;
      this._visibleMozaic = value;
      using (List<GameObject>.Enumerator enumerator = this._objMozaicList.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.SetActive(value);
      }
    }
  }

  public bool LoadEnd { get; private set; }

  public byte Sex
  {
    get
    {
      return this._sex;
    }
    set
    {
      this._sex = value;
    }
  }

  public bool VisibleAll { get; set; }

  public bool HideEyesHighlight { get; set; }

  public float TearsRate
  {
    get
    {
      return this._tearsRate;
    }
    set
    {
      this.ChangeTearsRate(value);
    }
  }

  public float HohoAkaRate
  {
    get
    {
      return this._hohoAkaRate;
    }
    set
    {
      this.ChangeHohoAkaRate(value);
    }
  }

  public int EyebrowPtn { get; set; }

  public int EyesPtn { get; set; }

  public int MouthPtn { get; set; }

  public float EyesOpen { get; set; }

  public float MouthOpen { get; set; }

  public int EyesLookPtn { get; set; }

  public int NeckLookPtn { get; set; }

  public bool EyesBlink { get; set; }

  private void Start()
  {
    if (!this._loadOnStart)
      return;
    TestChara.CharaTable[TestChara.SearchUnusedNo()] = this;
    this.Load();
  }

  public void InitializeFemale(GameObject objRoot, int id, int no)
  {
    this.VisibleAll = true;
    this.ObjRoot = objRoot;
    this.CharID = id;
    this.LoadNo = no;
    this.Sex = (byte) 1;
    this.deleteBodyBoneName = "cf_J_Root";
    this.deleteHeadBoneName = "cf_J_FaceRoot";
    this.SiruNewLv = new byte[Enum.GetValues(typeof (ChaFileDefine.SiruParts)).Length];
    for (int index = 0; index < this.SiruNewLv.Length; ++index)
      this.SiruNewLv[index] = (byte) 0;
    this.FaceMaterial = (Material) null;
    this.LoadEnd = false;
  }

  public void InitializeMale(GameObject objRoot, int id, int no)
  {
    this.VisibleAll = true;
    this.ObjRoot = objRoot;
    this.CharID = id;
    this.LoadNo = no;
    this.Sex = (byte) 0;
    this.deleteBodyBoneName = "cf_J_Root";
    this.deleteHeadBoneName = "cf_J_FaceRoot";
    this.LoadEnd = false;
  }

  public void SetActiveTop(bool active)
  {
    if (!Object.op_Implicit((Object) this.ObjTop))
      return;
    this.ObjTop.SetActive(active);
  }

  public bool GetActiveTop()
  {
    return Object.op_Implicit((Object) this.ObjTop) && this.ObjTop.get_activeSelf();
  }

  public void Load()
  {
    if (this._shapeFace == null)
      return;
    if (!TestChara.CharaTable.ContainsValue(this))
      TestChara.CharaTable.Add(TestChara.SearchUnusedNo(), this);
    this.ObjTop = new GameObject("BodyTop");
    this.ObjTop.get_transform().SetParent(((Component) this).get_transform(), false);
    this.ObjTop.get_transform().set_localPosition(Vector3.get_zero());
    this.ObjTop.get_transform().set_localRotation(Quaternion.get_identity());
    if (Object.op_Implicit((Object) this._boneBody))
    {
      this.ObjAnim = (GameObject) Object.Instantiate<GameObject>((M0) this._boneBody, this.ObjTop.get_transform());
      this.ObjAnim.get_transform().set_localPosition(Vector3.get_zero());
      this.ObjAnim.get_transform().set_localRotation(Quaternion.get_identity());
      this._animBody = (Animator) this.ObjAnim.GetComponent<Animator>();
      foreach (DynamicBone_Ver02 componentsInChild in (DynamicBone_Ver02[]) this.ObjAnim.GetComponentsInChildren<DynamicBone_Ver02>(true))
      {
        switch (componentsInChild.Comment)
        {
          case "Mune_L":
            this.DictDynamicBone[ChaControlDefine.DynamicBoneKind.BreastL] = componentsInChild;
            break;
          case "Mune_R":
            this.DictDynamicBone[ChaControlDefine.DynamicBoneKind.BreastR] = componentsInChild;
            break;
          case "Siri_L":
            this.DictDynamicBone[ChaControlDefine.DynamicBoneKind.HipL] = componentsInChild;
            break;
          case "Siri_R":
            this.DictDynamicBone[ChaControlDefine.DynamicBoneKind.HipR] = componentsInChild;
            break;
        }
      }
      this.ObjBodyBone = this.ObjAnim.get_transform().FindLoop("cf_J_Root");
      if (Object.op_Implicit((Object) this.ObjBodyBone))
      {
        this._aaWeightsBody.CreateBoneListMultiple(this.ObjBodyBone, TestChara._boneNames);
        this.CreateReferenceInfo(1UL, this.ObjBodyBone);
        NeckLookControllerVer2[] componentsInChildren = (NeckLookControllerVer2[]) this.ObjBodyBone.GetComponentsInChildren<NeckLookControllerVer2>(true);
        if (componentsInChildren.Length != 0)
          this.NeckLookControl = componentsInChildren[0];
        string anmKeyInfoName = "cf_anmShapeBody";
        string cateInfoName = "cf_custombody";
        Transform transform = this.ObjBodyBone.get_transform();
        this._shapeBody.InitShapeInfo(string.Empty, "list/customshape.unity3d", "list/customshape.unity3d", anmKeyInfoName, cateInfoName, transform);
        int category1 = ChaFileDefine.cf_bodyshapename.Length - 1;
        for (int category2 = 0; category2 < category1; ++category2)
          this._shapeBody.ChangeValue(category2, 0.5f);
        this._shapeBody.ChangeValue(0, 1f);
        this._shapeBody.ChangeValue(category1, this._nipStand);
      }
      this.ObjAnim.SetActive(true);
      if (Object.op_Implicit((Object) this._boneHead))
      {
        this.ObjHeadBone = (GameObject) Object.Instantiate<GameObject>((M0) this._boneHead, this.GetReferenceInfo(CharReference.RefObjKey.HeadParent).get_transform());
        this.ObjHeadBone.get_transform().set_localPosition(Vector3.get_zero());
        this.ObjHeadBone.get_transform().set_localRotation(Quaternion.get_identity());
        EyeLookController[] componentsInChildren1 = (EyeLookController[]) this.ObjHeadBone.GetComponentsInChildren<EyeLookController>(true);
        if (componentsInChildren1.Length != 0)
          this.EyeLookControl = componentsInChildren1[0];
        if (Object.op_Implicit((Object) this.EyeLookControl))
        {
          EyeLookCalc component = (EyeLookCalc) ((Component) this.EyeLookControl).GetComponent<EyeLookCalc>();
          if (Object.op_Implicit((Object) component))
            component.Init();
        }
        this._aaWeightsHead.CreateBoneList(this.ObjHeadBone, "cf_J");
        this.ObjHeadBone.SetActive(true);
        if (Object.op_Implicit((Object) this._body))
        {
          this.ObjBody = (GameObject) Object.Instantiate<GameObject>((M0) this._body, this.ObjTop.get_transform());
          this.ObjBody.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjBody.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjBody, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo) ? (Transform) null : referenceInfo.get_transform());
          Object.Destroy((Object) this.ObjBody.GetComponent<Animator>());
          Object.Destroy((Object) this.ObjBody.GetComponent<FullBodyBipedIK>());
          foreach (Object component in (DynamicBone[]) this.ObjBody.GetComponents<DynamicBone>())
            Object.Destroy(component);
          foreach (Object component in (DynamicBone_Ver02[]) this.ObjBody.GetComponents<DynamicBone_Ver02>())
            Object.Destroy(component);
          foreach (Object componentsInChild in (DynamicBoneCollider[]) this.ObjBody.GetComponentsInChildren<DynamicBoneCollider>())
            Object.Destroy(componentsInChild);
          this.ObjBody.SetActive(true);
        }
        foreach (string name in this._sex != (byte) 0 ? TestChara._f_mozaicparts : TestChara._m_mozaicparts)
        {
          GameObject loop = this.ObjBody.get_transform().FindLoop(name);
          if (!Object.op_Equality((Object) loop, (Object) null))
            this._objMozaicList.Add(loop);
        }
        using (List<GameObject>.Enumerator enumerator = this._objMozaicList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (Object.op_Inequality((Object) current, (Object) null))
              current.SetActive(false);
          }
        }
        if (Object.op_Implicit((Object) this._head))
        {
          this.ObjHead = (GameObject) Object.Instantiate<GameObject>((M0) this._head, Vector3.get_zero(), Quaternion.get_identity(), this.ObjHeadBone.get_transform());
          this.ObjHead.get_transform().SetParent(this.ObjHeadBone.get_transform(), false);
          this.ObjHead.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjHead.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject loop1 = this.ObjHead.get_transform().FindLoop("o_head");
          if (Object.op_Implicit((Object) loop1))
          {
            this.FaceRenderer = (Renderer) loop1.GetComponent<Renderer>();
            this.FaceMaterial = this.FaceRenderer?.get_material();
          }
          GameObject loop2 = this.ObjHead.get_transform().FindLoop("o_eyebase_L");
          if (Object.op_Implicit((Object) loop2))
          {
            this.EyeRendererL = (Renderer) loop2.GetComponent<Renderer>();
            this.EyeLMaterial = this.EyeRendererL?.get_material();
          }
          GameObject loop3 = this.ObjHead.get_transform().FindLoop("o_eyebase_R");
          if (Object.op_Implicit((Object) loop3))
          {
            this.EyeRendererR = (Renderer) loop3.GetComponent<Renderer>();
            this.EyeRMaterial = this.EyeRendererR?.get_material();
          }
          GameObject loop4 = this.ObjHead.get_transform().FindLoop("o_namida");
          if (Object.op_Implicit((Object) loop4))
          {
            this.TearRenderer = (Renderer) loop4.GetComponent<Renderer>();
            this.TearMaterial = this.TearRenderer?.get_material();
          }
          EyeLookController[] componentsInChildren2 = (EyeLookController[]) this.ObjHead.GetComponentsInChildren<EyeLookController>();
          if (!((IList<EyeLookController>) componentsInChildren2).IsNullOrEmpty<EyeLookController>())
          {
            foreach (EyeLookController eyeLookController in componentsInChildren2)
            {
              EyeLookCalc component = (EyeLookCalc) ((Component) eyeLookController).GetComponent<EyeLookCalc>();
              if (Object.op_Implicit((Object) component))
                Object.Destroy((Object) component);
              Object.Destroy((Object) eyeLookController);
            }
          }
          CommonLib.CopySameNameTransform(this.ObjHeadBone.get_transform(), this.ObjHead.get_transform());
          this.ObjHead.get_transform().SetParent(this.ObjHeadBone.get_transform(), false);
          this._aaWeightsHead.AssignedWeights(this.ObjHead, "cf_J_FaceRoot", (Transform) null);
          this.FBSCtrl = (FaceBlendShape) this.ObjHead.GetComponent<FaceBlendShape>();
          if (Object.op_Inequality((Object) this.FBSCtrl, (Object) null))
          {
            this.EyebrowCtrl = this.FBSCtrl.EyebrowCtrl;
            this.EyesCtrl = this.FBSCtrl.EyesCtrl;
            this.MouthCtrl = this.FBSCtrl.MouthCtrl;
            this.ChangeEyebrowPtn(0, true);
            this.ChangeEyesPtn(0, true);
            this.ChangeMouthPtn(0, true);
          }
          this.CreateReferenceInfo(3UL, this.ObjHeadBone);
          this.CreateTagInfo(3UL, this.ObjHeadBone);
          string shapeFaceName = this._shapeFaceName;
          string cateInfoName = "cf_customhead";
          this._shapeFace.InitShapeInfo(string.Empty, "chara/cf_head_00.unity3d", "list/customshape.unity3d", shapeFaceName, cateInfoName, this.ObjHeadBone.get_transform());
          for (int category = 0; category < ChaFileDefine.cf_headshapename.Length; ++category)
            this._shapeFace.ChangeValue(category, 0.5f);
          this.ObjHead.SetActive(true);
        }
        DynamicBoneCollider[] componentsInChildren3 = (DynamicBoneCollider[]) this.ObjAnim.GetComponentsInChildren<DynamicBoneCollider>(true);
        GameObject referenceInfo1 = this.GetReferenceInfo(CharReference.RefObjKey.HairParent);
        try
        {
          if (Object.op_Implicit((Object) this._hairBack))
          {
            this.ObjHairBack = (GameObject) Object.Instantiate<GameObject>((M0) this._hairBack, referenceInfo1.get_transform());
            this.ObjHairBack.get_transform().set_localPosition(Vector3.get_zero());
            this.ObjHairBack.get_transform().set_localRotation(Quaternion.get_identity());
            Dictionary<string, GameObject> dictBone = this._aaWeightsBody.dictBone;
            foreach (DynamicBone componentsInChild in (DynamicBone[]) this.ObjHairBack.GetComponentsInChildren<DynamicBone>(true))
            {
              if (componentsInChild.m_Colliders != null)
              {
                componentsInChild.m_Colliders.Clear();
                for (int index = 0; index < componentsInChildren3.Length; ++index)
                  componentsInChild.m_Colliders.Add((DynamicBoneColliderBase) componentsInChildren3[index]);
              }
            }
            foreach (DynamicBone_Ver01 componentsInChild in (DynamicBone_Ver01[]) this.ObjHairBack.GetComponentsInChildren<DynamicBone_Ver01>(true))
            {
              if (componentsInChild.m_Colliders != null)
              {
                componentsInChild.m_Colliders.Clear();
                for (int index = 0; index < componentsInChildren3.Length; ++index)
                  componentsInChild.m_Colliders.Add(componentsInChildren3[index]);
              }
            }
          }
          if (Object.op_Implicit((Object) this._hairFront))
          {
            this.ObjHairFront = (GameObject) Object.Instantiate<GameObject>((M0) this._hairFront, referenceInfo1.get_transform());
            this.ObjHairFront.get_transform().set_localPosition(Vector3.get_zero());
            this.ObjHairFront.get_transform().set_localRotation(Quaternion.get_identity());
            Dictionary<string, GameObject> dictBone = this._aaWeightsBody.dictBone;
            foreach (DynamicBone componentsInChild in (DynamicBone[]) this.ObjHairFront.GetComponentsInChildren<DynamicBone>(true))
            {
              if (componentsInChild.m_Colliders != null)
              {
                componentsInChild.m_Colliders.Clear();
                for (int index = 0; index < componentsInChildren3.Length; ++index)
                  componentsInChild.m_Colliders.Add((DynamicBoneColliderBase) componentsInChildren3[index]);
              }
            }
            foreach (DynamicBone_Ver01 componentsInChild in (DynamicBone_Ver01[]) this.ObjHairFront.GetComponentsInChildren<DynamicBone_Ver01>(true))
            {
              if (componentsInChild.m_Colliders != null)
              {
                componentsInChild.m_Colliders.Clear();
                for (int index = 0; index < componentsInChildren3.Length; ++index)
                  componentsInChild.m_Colliders.Add(componentsInChildren3[index]);
              }
            }
          }
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
        }
        if (Object.op_Implicit((Object) this._clothTop))
        {
          this.ObjClothTop = (GameObject) Object.Instantiate<GameObject>((M0) this._clothTop, this.ObjTop.get_transform());
          this.ObjClothTop.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjClothTop.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjClothTop, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
          Dictionary<string, GameObject> dictBone = this._aaWeightsBody.dictBone;
          foreach (DynamicBone componentsInChild in (DynamicBone[]) this.ObjClothTop.GetComponentsInChildren<DynamicBone>(true))
          {
            if (Object.op_Implicit((Object) componentsInChild.m_Root))
            {
              using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<string, GameObject> current = enumerator.Current;
                  if (!(current.Key != ((Object) componentsInChild.m_Root).get_name()))
                  {
                    componentsInChild.m_Root = current.Value.get_transform();
                    break;
                  }
                }
              }
            }
            if (componentsInChild.m_Exclusions != null && componentsInChild.m_Exclusions.Count != 0)
            {
              for (int index = 0; index < componentsInChild.m_Exclusions.Count; ++index)
              {
                if (!Object.op_Equality((Object) componentsInChild.m_Exclusions[index], (Object) null))
                {
                  using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<string, GameObject> current = enumerator.Current;
                      if (!(current.Key != ((Object) componentsInChild.m_Exclusions[index]).get_name()))
                      {
                        componentsInChild.m_Exclusions[index] = current.Value.get_transform();
                        break;
                      }
                    }
                  }
                }
              }
            }
            if (componentsInChild.m_notRolls != null && componentsInChild.m_notRolls.Count != 0)
            {
              for (int index = 0; index < componentsInChild.m_notRolls.Count; ++index)
              {
                if (!Object.op_Equality((Object) componentsInChild.m_notRolls[index], (Object) null))
                {
                  using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<string, GameObject> current = enumerator.Current;
                      if (!(current.Key != ((Object) componentsInChild.m_notRolls[index]).get_name()))
                      {
                        componentsInChild.m_notRolls[index] = current.Value.get_transform();
                        break;
                      }
                    }
                  }
                }
              }
            }
            if (componentsInChild.m_Colliders != null)
            {
              componentsInChild.m_Colliders.Clear();
              for (int index = 0; index < componentsInChildren3.Length; ++index)
                componentsInChild.m_Colliders.Add((DynamicBoneColliderBase) componentsInChildren3[index]);
            }
          }
          this.ObjClothTop.SetActive(true);
        }
        if (Object.op_Implicit((Object) this._clothBot))
        {
          this.ObjClothBot = (GameObject) Object.Instantiate<GameObject>((M0) this._clothBot, this.ObjTop.get_transform());
          this.ObjClothBot.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjClothBot.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjClothBot, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
          Dictionary<string, GameObject> dictBone = this._aaWeightsBody.dictBone;
          foreach (DynamicBone componentsInChild in (DynamicBone[]) this.ObjClothBot.GetComponentsInChildren<DynamicBone>(true))
          {
            if (Object.op_Implicit((Object) componentsInChild.m_Root))
            {
              using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  KeyValuePair<string, GameObject> current = enumerator.Current;
                  if (!(current.Key != ((Object) componentsInChild.m_Root).get_name()))
                  {
                    componentsInChild.m_Root = current.Value.get_transform();
                    break;
                  }
                }
              }
            }
            if (componentsInChild.m_Exclusions != null && componentsInChild.m_Exclusions.Count != 0)
            {
              for (int index = 0; index < componentsInChild.m_Exclusions.Count; ++index)
              {
                if (!Object.op_Equality((Object) componentsInChild.m_Exclusions[index], (Object) null))
                {
                  using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<string, GameObject> current = enumerator.Current;
                      if (!(current.Key != ((Object) componentsInChild.m_Exclusions[index]).get_name()))
                      {
                        componentsInChild.m_Exclusions[index] = current.Value.get_transform();
                        break;
                      }
                    }
                  }
                }
              }
            }
            if (componentsInChild.m_notRolls != null && componentsInChild.m_notRolls.Count != 0)
            {
              for (int index = 0; index < componentsInChild.m_notRolls.Count; ++index)
              {
                if (!Object.op_Equality((Object) componentsInChild.m_notRolls[index], (Object) null))
                {
                  using (Dictionary<string, GameObject>.Enumerator enumerator = dictBone.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      KeyValuePair<string, GameObject> current = enumerator.Current;
                      if (!(current.Key != ((Object) componentsInChild.m_notRolls[index]).get_name()))
                      {
                        componentsInChild.m_notRolls[index] = current.Value.get_transform();
                        break;
                      }
                    }
                  }
                }
              }
            }
            if (componentsInChild.m_Colliders != null)
            {
              componentsInChild.m_Colliders.Clear();
              for (int index = 0; index < componentsInChildren3.Length; ++index)
                componentsInChild.m_Colliders.Add((DynamicBoneColliderBase) componentsInChildren3[index]);
            }
          }
          this.ObjClothBot.SetActive(true);
        }
        if (Object.op_Implicit((Object) this._bra))
        {
          this.ObjBra = (GameObject) Object.Instantiate<GameObject>((M0) this._bra, this.ObjTop.get_transform());
          this.ObjBra.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjBra.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjBra, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
        }
        if (Object.op_Implicit((Object) this._shorts))
        {
          this.ObjShorts = (GameObject) Object.Instantiate<GameObject>((M0) this._shorts, this.ObjTop.get_transform());
          this.ObjShorts.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjShorts.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjShorts, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
          this.ObjShorts.SetActive(true);
        }
        if (Object.op_Implicit((Object) this._glove))
        {
          this.ObjGlove = (GameObject) Object.Instantiate<GameObject>((M0) this._glove, this.ObjTop.get_transform());
          this.ObjGlove.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjGlove.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjGlove, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
        }
        if (Object.op_Implicit((Object) this._socks))
        {
          this.ObjSocks = (GameObject) Object.Instantiate<GameObject>((M0) this._socks, this.ObjTop.get_transform());
          this.ObjSocks.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjSocks.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjSocks, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
        }
        if (Object.op_Implicit((Object) this._shoes))
        {
          this.ObjShoes = (GameObject) Object.Instantiate<GameObject>((M0) this._shoes, this.ObjTop.get_transform());
          this.ObjShoes.get_transform().set_localPosition(Vector3.get_zero());
          this.ObjShoes.get_transform().set_localRotation(Quaternion.get_identity());
          GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.A_ROOTBONE);
          this._aaWeightsBody.AssignedWeights(this.ObjShoes, "cf_J_Root", !Object.op_Implicit((Object) referenceInfo2) ? (Transform) null : referenceInfo2.get_transform());
        }
        for (int index = 0; index < this._acsArray.Length; ++index)
        {
          TestChara.AcsGenerateInfo acs = this._acsArray[index];
          if (!Object.op_Equality((Object) acs.ACSObj, (Object) null))
          {
            GameObject referenceInfoNew = this.GetReferenceInfo_New(acs.Key);
            if (!Object.op_Equality((Object) referenceInfoNew, (Object) null))
            {
              GameObject gameObject = this.ObjAcsArray[index] = (GameObject) Object.Instantiate<GameObject>((M0) acs.ACSObj, referenceInfoNew.get_transform());
              gameObject.get_transform().set_localPosition(Vector3.get_zero());
              gameObject.get_transform().set_localRotation(Quaternion.get_identity());
            }
          }
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();
        this.LoadEnd = true;
        for (int index = 0; index < this._shapeValuesFace.Length; ++index)
          this._shapeValuesFace[index] = 0.5f;
        for (int index = 0; index < this._shapeValues.Length; ++index)
          this._shapeValues[index] = 0.5f;
        this._shapeValues[0] = 1f;
        this._animBody.set_runtimeAnimatorController(this._rac);
        if (Object.op_Inequality((Object) this._animBody.get_runtimeAnimatorController(), (Object) null))
        {
          RuntimeAnimatorController rac = this._animBody.get_runtimeAnimatorController();
          this._animBody.set_runtimeAnimatorController((RuntimeAnimatorController) null);
          ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Observable.NextFrame((FrameCountType) 0), (Action<M0>) (_ => this._animBody.set_runtimeAnimatorController(rac)));
        }
        this.NeckLookControl.ptnNo = 3;
        this.VisibleAll = true;
      }
      else
        Debug.LogError((object) "頭の骨がない");
    }
    else
      Debug.LogError((object) "体の骨がない");
  }

  private void CreateReferenceInfo(ulong flags, GameObject objRef)
  {
    if (Object.op_Equality((Object) objRef, (Object) null))
      return;
    FindAssist findAssist = new FindAssist();
    findAssist.Initialize(objRef.get_transform());
    switch (flags)
    {
      case 1:
        this._dictRefObj[8] = findAssist.GetObjectFromName("N_Neck");
        this._dictRefObj[9] = findAssist.GetObjectFromName("N_Chest");
        this._dictRefObj[10] = findAssist.GetObjectFromName("N_Wrist_L");
        this._dictRefObj[11] = findAssist.GetObjectFromName("N_Wrist_R");
        this._dictRefObj[12] = findAssist.GetObjectFromName("N_Arm_L");
        this._dictRefObj[13] = findAssist.GetObjectFromName("N_Arm_R");
        this._dictRefObj[14] = findAssist.GetObjectFromName("N_Index_L");
        this._dictRefObj[15] = findAssist.GetObjectFromName("N_Index_R");
        this._dictRefObj[16] = findAssist.GetObjectFromName("N_Middle_L");
        this._dictRefObj[17] = findAssist.GetObjectFromName("N_Middle_R");
        this._dictRefObj[18] = findAssist.GetObjectFromName("N_Ring_L");
        this._dictRefObj[19] = findAssist.GetObjectFromName("N_Ring_R");
        this._dictRefObj[20] = findAssist.GetObjectFromName("N_Leg_L");
        this._dictRefObj[21] = findAssist.GetObjectFromName("N_Leg_R");
        this._dictRefObj[22] = findAssist.GetObjectFromName("N_Ankle_L");
        this._dictRefObj[23] = findAssist.GetObjectFromName("N_Ankle_R");
        this._dictRefObj[24] = findAssist.GetObjectFromName("N_Tikubi_L");
        this._dictRefObj[25] = findAssist.GetObjectFromName("N_Tikubi_R");
        this._dictRefObj[26] = findAssist.GetObjectFromName("N_Waist");
        this._dictRefObj[27] = findAssist.GetObjectFromName("N_Shoulder_L");
        this._dictRefObj[28] = findAssist.GetObjectFromName("N_Shoulder_R");
        this._dictRefObj[29] = findAssist.GetObjectFromName("N_Hand_L");
        this._dictRefObj[30] = findAssist.GetObjectFromName("N_Hand_R");
        this._dictRefObj[42] = findAssist.GetObjectFromName("cf_J_Hips");
        this._dictRefObj[0] = findAssist.GetObjectFromName("cf_J_Head_s");
        this._dictRefObj[32] = findAssist.GetObjectFromName("cf_J_Kokan_dam");
        this._dictRefObjNew[15] = findAssist.GetObjectFromName("N_Neck");
        this._dictRefObjNew[16] = findAssist.GetObjectFromName("N_Chest_f");
        this._dictRefObjNew[17] = findAssist.GetObjectFromName("N_Chest");
        this._dictRefObjNew[18] = findAssist.GetObjectFromName("N_Tikubi_L");
        this._dictRefObjNew[19] = findAssist.GetObjectFromName("N_Tikubi_R");
        this._dictRefObjNew[20] = findAssist.GetObjectFromName("N_Back");
        this._dictRefObjNew[21] = findAssist.GetObjectFromName("N_Back_L");
        this._dictRefObjNew[22] = findAssist.GetObjectFromName("N_Back_R");
        this._dictRefObjNew[23] = findAssist.GetObjectFromName("N_Waist");
        this._dictRefObjNew[24] = findAssist.GetObjectFromName("N_Waist_f");
        this._dictRefObjNew[25] = findAssist.GetObjectFromName("N_Waist_b");
        this._dictRefObjNew[26] = findAssist.GetObjectFromName("N_Waist_L");
        this._dictRefObjNew[27] = findAssist.GetObjectFromName("N_Waist_R");
        this._dictRefObjNew[28] = findAssist.GetObjectFromName("N_Leg_L");
        this._dictRefObjNew[29] = findAssist.GetObjectFromName("N_Leg_R");
        this._dictRefObjNew[30] = findAssist.GetObjectFromName("N_Knee_L");
        this._dictRefObjNew[31] = findAssist.GetObjectFromName("N_Knee_R");
        this._dictRefObjNew[32] = findAssist.GetObjectFromName("N_Ankle_L");
        this._dictRefObjNew[33] = findAssist.GetObjectFromName("N_Ankle_R");
        this._dictRefObjNew[34] = findAssist.GetObjectFromName("N_Foot_L");
        this._dictRefObjNew[35] = findAssist.GetObjectFromName("N_Foot_R");
        this._dictRefObjNew[36] = findAssist.GetObjectFromName("N_Shoulder_L");
        this._dictRefObjNew[37] = findAssist.GetObjectFromName("N_Shoulder_R");
        this._dictRefObjNew[38] = findAssist.GetObjectFromName("N_Elbo_L");
        this._dictRefObjNew[39] = findAssist.GetObjectFromName("N_Elbo_R");
        this._dictRefObjNew[40] = findAssist.GetObjectFromName("N_Arm_L");
        this._dictRefObjNew[41] = findAssist.GetObjectFromName("N_Arm_R");
        this._dictRefObjNew[42] = findAssist.GetObjectFromName("N_Wrist_L");
        this._dictRefObjNew[43] = findAssist.GetObjectFromName("N_Wrist_R");
        this._dictRefObjNew[44] = findAssist.GetObjectFromName("N_Hand_L");
        this._dictRefObjNew[45] = findAssist.GetObjectFromName("N_Hand_R");
        this._dictRefObjNew[46] = findAssist.GetObjectFromName("N_Index_L");
        this._dictRefObjNew[47] = findAssist.GetObjectFromName("N_Index_R");
        this._dictRefObjNew[48] = findAssist.GetObjectFromName("N_Middle_L");
        this._dictRefObjNew[49] = findAssist.GetObjectFromName("N_Middle_R");
        this._dictRefObjNew[50] = findAssist.GetObjectFromName("N_Ring_L");
        this._dictRefObjNew[51] = findAssist.GetObjectFromName("N_Ring_R");
        this._dictRefObjNew[52] = findAssist.GetObjectFromName("N_Dan");
        this._dictRefObjNew[53] = findAssist.GetObjectFromName("N_Kokan");
        this._dictRefObjNew[54] = findAssist.GetObjectFromName("N_Ana");
        this._dictRefObjNew[0] = findAssist.GetObjectFromName("cf_J_Head_s");
        break;
      case 3:
        this._dictRefObj[2] = findAssist.GetObjectFromName("N_Head");
        this._dictRefObj[3] = findAssist.GetObjectFromName("N_Megane");
        this._dictRefObj[4] = findAssist.GetObjectFromName("N_Earring_L");
        this._dictRefObj[5] = findAssist.GetObjectFromName("N_Earring_R");
        this._dictRefObj[6] = findAssist.GetObjectFromName("N_Mouth");
        this._dictRefObj[7] = findAssist.GetObjectFromName("N_Nose");
        this._dictRefObj[41] = findAssist.GetObjectFromName("cf_J_MouthMove");
        this._dictRefObj[1] = findAssist.GetObjectFromName("N_hair_Root");
        this._dictRefObj[34] = findAssist.GetObjectFromName("cf_O_sita");
        this._dictRefObj[38] = findAssist.GetObjectFromName("cf_O_namida01");
        this._dictRefObj[39] = findAssist.GetObjectFromName("cf_O_namida02");
        this._dictRefObj[40] = findAssist.GetObjectFromName("cf_O_namida03");
        this._dictRefObjNew[7] = findAssist.GetObjectFromName("N_Head");
        this._dictRefObjNew[10] = findAssist.GetObjectFromName("N_Megane");
        this._dictRefObjNew[11] = findAssist.GetObjectFromName("N_Earring_L");
        this._dictRefObjNew[12] = findAssist.GetObjectFromName("N_Earring_R");
        this._dictRefObjNew[14] = findAssist.GetObjectFromName("N_Mouth");
        this._dictRefObjNew[13] = findAssist.GetObjectFromName("N_Nose");
        this._dictRefObjNew[1] = findAssist.GetObjectFromName("N_hair_Root");
        this._dictRefObjNew[2] = findAssist.GetObjectFromName("N_Hair_twin_L");
        this._dictRefObjNew[3] = findAssist.GetObjectFromName("N_Hair_twin_R");
        this._dictRefObjNew[4] = findAssist.GetObjectFromName("N_Hair_pin_L");
        this._dictRefObjNew[5] = findAssist.GetObjectFromName("N_Hair_pin_R");
        this._dictRefObjNew[6] = findAssist.GetObjectFromName("N_Head_top");
        this._dictRefObjNew[8] = findAssist.GetObjectFromName("N_Hitai");
        this._dictRefObjNew[9] = findAssist.GetObjectFromName("N_Face");
        break;
      case 8:
        this._dictRefObj[35] = findAssist.GetObjectFromName("N_tang");
        this._dictRefObj[36] = findAssist.GetObjectFromName("N_mnpb");
        this._dictRefObj[37] = findAssist.GetObjectFromName("N_tikubi");
        break;
    }
  }

  public GameObject GetReferenceInfo(CharReference.RefObjKey key)
  {
    GameObject gameObject;
    this._dictRefObj.TryGetValue((int) key, out gameObject);
    return gameObject;
  }

  public GameObject GetReferenceInfo_New(CharReference.RefObjKey_New key)
  {
    GameObject gameObject;
    this._dictRefObjNew.TryGetValue((int) key, out gameObject);
    return gameObject;
  }

  public float GetShapeFaceValue(int index)
  {
    int length = ChaFileDefine.cf_headshapename.Length;
    return index >= length ? 0.0f : this._shapeValuesFace[index];
  }

  public bool SetShapeFaceValue(int index, float value)
  {
    int length = ChaFileDefine.cf_headshapename.Length;
    if (index >= length)
      return false;
    this._shapeValuesFace[index] = Mathf.Clamp(value, 0.0f, 1f);
    if (this._shapeFace != null && this._shapeFace.InitEnd)
      this._shapeFace.ChangeValue(index, value);
    this._updateShapeFace = true;
    return true;
  }

  public void UpdateShapeFace()
  {
    if (this._shapeFace == null || !this._shapeFace.InitEnd)
      return;
    int[] numArray = new int[4]{ 55, 56, 57, 58 };
    if (this._disableShapeMouth)
    {
      for (int index = 0; index < numArray.Length; ++index)
        this._shapeFace.ChangeValue(numArray[index], 0.5f);
    }
    else
    {
      for (int index = 0; index < numArray.Length; ++index)
        this._shapeFace.ChangeValue(numArray[index], this._shapeValuesFace[numArray[index]]);
    }
    this._shapeFace.Update();
  }

  public void UpdateShapeBody()
  {
    if (this._shapeBody == null || !this._shapeBody.InitEnd)
      return;
    this._shapeBody.updateMask = 31;
    this._shapeBody.Update();
    if (this._disableShapeNipL)
    {
      this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length - 1, 0.0f);
      this._shapeBody.updateMask = 4;
      this._shapeBody.Update();
    }
    if (this._disableShapeNipR)
    {
      this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length - 1, 0.0f);
      this._shapeBody.updateMask = 8;
      this._shapeBody.Update();
    }
    for (int index = 0; index < this._disableShapeBustLAry.Length; ++index)
    {
      int category = ChaFileDefine.cf_BustShapeMaskID[index];
      float num = !this._disableShapeBustLAry[index] ? this._shapeValues[category] : this._defaultBustValue[index];
      this._shapeBody.ChangeValue(category, num);
    }
    this._shapeBody.updateMask = 1;
    this._shapeBody.Update();
    for (int index = 0; index < this._disableShapeBustRAry.Length; ++index)
    {
      int category = ChaFileDefine.cf_BustShapeMaskID[index];
      float num = !this._disableShapeBustRAry[index] ? this._shapeValues[category] : this._defaultBustValue[index];
      this._shapeBody.ChangeValue(category, num);
    }
    this._shapeBody.updateMask = 2;
    this._shapeBody.Update();
  }

  public float GetShapeBodyValue(int index)
  {
    int length = ChaFileDefine.cf_bodyshapename.Length;
    if (index >= length)
      return 0.0f;
    return index == length - 1 ? this._nipStand : this._shapeValues[index];
  }

  public bool SetShapeBodyValue(int index, float value)
  {
    int length = ChaFileDefine.cf_bodyshapename.Length;
    if (index >= length)
      return false;
    if (index == length - 1)
      this._nipStand = value;
    else
      this._shapeValues[index] = Mathf.Clamp(value, 0.0f, 1f);
    if (this._shapeBody != null && this._shapeBody.InitEnd)
      this._shapeBody.ChangeValue(index, value);
    this._updateShapeBody = true;
    return true;
  }

  public void ForceUpdate()
  {
    this.UpdateVisible();
  }

  public void Release()
  {
    if (Object.op_Implicit((Object) this.ObjTop))
      Object.Destroy((Object) this.ObjTop);
    this.ReleaseSub();
    this.ReleaseTagAll();
    this.NeckLookControl = (NeckLookControllerVer2) null;
    this.EyeLookControl = (EyeLookController) null;
    this.FBSCtrl = (FaceBlendShape) null;
    this.EyesCtrl = (FBSCtrlEyes) null;
    this.MouthCtrl = (FBSCtrlMouth) null;
    this.ObjTop = (GameObject) null;
    this.ObjAnim = (GameObject) null;
    this.ObjBodyBone = (GameObject) null;
    this.ObjBody = (GameObject) null;
    this.ObjHeadBone = (GameObject) null;
    this.ObjHead = (GameObject) null;
  }

  private void ReleaseSub()
  {
    this.DictDynamicBone = (Dictionary<ChaControlDefine.DynamicBoneKind, DynamicBone_Ver02>) null;
    for (int index = 0; index < this.SiruNewLv.Length; ++index)
      this.SiruNewLv[index] = (byte) 0;
    Object.Destroy((Object) this.FaceMaterial);
    this.FaceMaterial = (Material) null;
  }

  private void UpdateVisible()
  {
    bool flag = false;
    if (YS_Assist.SetActiveControl(this.ObjBody, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjHead, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjHairBack, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjHairFront, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjClothTop, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjClothBot, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjBra, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjShorts, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjGlove, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjSocks, this.VisibleAll))
      flag = true;
    if (YS_Assist.SetActiveControl(this.ObjShoes, this.VisibleAll))
      flag = true;
    foreach (GameObject objAcs in this.ObjAcsArray)
    {
      if (YS_Assist.SetActiveControl(objAcs, this.VisibleAll))
        flag = true;
    }
    if (!flag)
      ;
  }

  private void Update()
  {
    this.ForceUpdate();
    if (!this._applySelfAnimatorParameter || Object.op_Equality((Object) this._animBody, (Object) null) || Object.op_Equality((Object) this._animBody.get_runtimeAnimatorController(), (Object) null))
      return;
    float shapeBodyValue1 = this.GetShapeBodyValue(0);
    float shapeBodyValue2 = this.GetShapeBodyValue(1);
    foreach (AnimatorControllerParameter parameter in this._animBody.get_parameters())
    {
      switch (parameter.get_name().ToLower())
      {
        case "height":
          if (parameter.get_type() == 1)
          {
            this._animBody.SetFloat(parameter.get_name(), shapeBodyValue1);
            break;
          }
          break;
        case "breast":
          if (parameter.get_type() == 1)
          {
            this._animBody.SetFloat(parameter.get_name(), shapeBodyValue2);
            break;
          }
          break;
      }
    }
  }

  private void UpdateBustSoftnessAndGravity()
  {
  }

  private void ForceLateUpdate()
  {
    if (!this.LoadEnd)
      return;
    if (this._updateShapeFace)
    {
      this.UpdateShapeFace();
      this._updateShapeFace = false;
    }
    if (this._updateShapeBody)
    {
      this.UpdateShapeBody();
      this._updateShapeBody = false;
    }
    this.LateUpdateSub();
    if (!this.ResetupDynamicBone)
      return;
    this.ResetDynamicBone();
    this.ResetupDynamicBone = false;
  }

  private void LateUpdateSub()
  {
    if (this.ResetupDynamicBoneBust)
    {
      this.ResetupDynamicBones();
      this.UpdateBustSoftnessAndGravity();
      this.ResetupDynamicBoneBust = false;
    }
    if (!this.UpdateBustSize)
      return;
    this.UpdateBustSize = false;
  }

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) this.ObjAnim, (Object) null) || Object.op_Equality((Object) this.ObjHeadBone, (Object) null) || (Object.op_Equality((Object) this.ObjBody, (Object) null) || Object.op_Equality((Object) this.ObjHead, (Object) null)))
      return;
    this.ForceLateUpdate();
  }

  public void AnimPlay(string stateName)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null))
      return;
    this.AnimBody.Play(stateName);
  }

  public AnimatorStateInfo GetAnimatorStateInfo(int layer)
  {
    return Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null) ? (AnimatorStateInfo) null : this.AnimBody.GetCurrentAnimatorStateInfo(layer);
  }

  public bool PlaySync(AnimatorStateInfo syncState, int layer)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    this.AnimBody.Play(((AnimatorStateInfo) ref syncState).get_shortNameHash(), layer, ((AnimatorStateInfo) ref syncState).get_normalizedTime());
    return true;
  }

  public bool PlaySync(int nameHash, int layer, float normalizedTime)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    this.AnimBody.Play(nameHash, layer, normalizedTime);
    return true;
  }

  public bool PlaySync(string stateName, int layer, float normalizedTime)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    this.AnimBody.Play(stateName, layer, normalizedTime);
    return true;
  }

  public bool SetLayerWeight(float weight, int layer)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    this.AnimBody.SetLayerWeight(layer, weight);
    return true;
  }

  public bool SetAllLayerWeight(float weight)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    for (int index = 1; index < this.AnimBody.get_layerCount(); ++index)
      this.AnimBody.SetLayerWeight(index, weight);
    return true;
  }

  public float GetLayerWeight(int layer)
  {
    return Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null) ? 0.0f : this.AnimBody.GetLayerWeight(layer);
  }

  public bool InitDynamicBone()
  {
    foreach (KeyValuePair<ChaControlDefine.DynamicBoneKind, DynamicBone_Ver02> keyValuePair in this.DictDynamicBone)
    {
      if (Object.op_Inequality((Object) keyValuePair.Value, (Object) null))
        keyValuePair.Value.ResetParticlesPosition();
    }
    return true;
  }

  public void ResetDynamicBone()
  {
    foreach (DynamicBone componentsInChild in (DynamicBone[]) this.ObjTop.GetComponentsInChildren<DynamicBone>(true))
      componentsInChild.ResetParticlesPosition();
  }

  private bool ResetupDynamicBones()
  {
    DynamicBone_Ver02 dynamicBoneVer02 = (DynamicBone_Ver02) null;
    if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.BreastL, out dynamicBoneVer02))
      dynamicBoneVer02.ResetPosition();
    if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.BreastR, out dynamicBoneVer02))
      dynamicBoneVer02.ResetPosition();
    if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.HipL, out dynamicBoneVer02))
      dynamicBoneVer02.ResetPosition();
    if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.HipR, out dynamicBoneVer02))
      dynamicBoneVer02.ResetPosition();
    return true;
  }

  public bool PlayYure(int area, bool play)
  {
    DynamicBone_Ver02 dynamicBoneVer02 = (DynamicBone_Ver02) null;
    if (area == 0)
    {
      if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.BreastL, out dynamicBoneVer02))
        ((Behaviour) dynamicBoneVer02).set_enabled(play);
      if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.BreastR, out dynamicBoneVer02))
        ((Behaviour) dynamicBoneVer02).set_enabled(play);
    }
    else
    {
      if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.HipL, out dynamicBoneVer02))
        ((Behaviour) dynamicBoneVer02).set_enabled(play);
      if (this.DictDynamicBone.TryGetValue(ChaControlDefine.DynamicBoneKind.HipR, out dynamicBoneVer02))
        ((Behaviour) dynamicBoneVer02).set_enabled(play);
    }
    return true;
  }

  public bool PlayYure(ChaControlDefine.DynamicBoneKind area, bool play)
  {
    DynamicBone_Ver02 dynamicBoneVer02 = (DynamicBone_Ver02) null;
    if (this.DictDynamicBone.TryGetValue(area, out dynamicBoneVer02))
      ((Behaviour) dynamicBoneVer02).set_enabled(play);
    return true;
  }

  public DynamicBone_Ver02 GetDynamicBone(ChaControlDefine.DynamicBoneKind area)
  {
    DynamicBone_Ver02 dynamicBoneVer02 = (DynamicBone_Ver02) null;
    this.DictDynamicBone.TryGetValue(area, out dynamicBoneVer02);
    return dynamicBoneVer02;
  }

  public bool IsBlend(int layer)
  {
    return !Object.op_Equality((Object) this.AnimBody, (Object) null) && !Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null) && this.AnimBody.IsInTransition(layer);
  }

  public Vector3 GetPosition()
  {
    return Object.op_Equality((Object) this.ObjTop, (Object) null) ? Vector3.get_zero() : this.ObjTop.get_transform().get_localPosition();
  }

  public void SetPosition(float x, float y, float z)
  {
    this.ObjTop.get_transform().set_localPosition(new Vector3(x, y, z));
  }

  public void SetPosition(Vector3 pos)
  {
    this.ObjTop.get_transform().set_localPosition(pos);
  }

  public void SetRotation(float x, float y, float z)
  {
    if (Object.op_Equality((Object) this.ObjTop, (Object) null))
      return;
    this.ObjTop.get_transform().set_localEulerAngles(new Vector3(x, y, z));
  }

  public void SetRotation(Vector3 rot)
  {
    if (Object.op_Equality((Object) this.ObjTop, (Object) null))
      return;
    this.ObjTop.get_transform().set_localEulerAngles(rot);
  }

  public Vector3 GetRotation()
  {
    return Object.op_Equality((Object) this.ObjTop, (Object) null) ? Vector3.get_zero() : this.ObjTop.get_transform().get_localEulerAngles();
  }

  public void ChangeEyebrowPtn(int ptn, bool blend = true)
  {
    this.EyebrowPtn = ptn;
    this.EyebrowCtrl?.ChangePtn(ptn, blend);
  }

  public void ChangeEyebrowOpen(float value, bool fixedValue = false)
  {
    FBSCtrlEyebrow eyebrowCtrl = this.EyebrowCtrl;
    if (eyebrowCtrl == null)
      return;
    float num = Mathf.Clamp(value, 0.0f, 1f);
    if (fixedValue)
    {
      eyebrowCtrl.FixedRate = num;
    }
    else
    {
      eyebrowCtrl.FixedRate = -1f;
      eyebrowCtrl.OpenMax = num;
    }
  }

  public void ChangeEyebrowOpenMin(float value)
  {
    if (this.EyebrowCtrl == null)
      return;
    this.EyebrowCtrl.OpenMin = value;
  }

  public void ChangeEyebrowOpenMax(float value)
  {
    FBSCtrlEyebrow eyebrowCtrl = this.EyebrowCtrl;
    if (eyebrowCtrl == null)
      return;
    eyebrowCtrl.OpenMax = value;
  }

  public void HideEyeHighlight(bool hide)
  {
    this.HideEyesHighlight = hide;
    float num = !hide ? 1f : 0.0f;
    Material eyeLmaterial = this.EyeLMaterial;
    Material eyeRmaterial = this.EyeRMaterial;
    if (Object.op_Equality((Object) eyeLmaterial, (Object) null) || Object.op_Equality((Object) eyeRmaterial, (Object) null))
    {
      Debug.LogError((object) "瞳のハイライトのレンダラーが取得できていません");
    }
    else
    {
      if (Object.op_Inequality((Object) eyeLmaterial, (Object) null))
        eyeLmaterial.SetFloat(TestChara._Smoothness, num);
      else
        Debug.LogError((object) "瞳(左)のハイライトのマテリアルが取得できません");
      if (Object.op_Inequality((Object) eyeRmaterial, (Object) null))
        eyeRmaterial.SetFloat(TestChara._Smoothness, num);
      else
        Debug.LogError((object) "瞳(右)のハイライトのマテリアルが取得できません");
    }
  }

  public void ChangeEyesPtn(int ptn, bool blend = true)
  {
    this.EyesPtn = ptn;
    this.EyesCtrl?.ChangePtn(ptn, blend);
  }

  public void ChangeEyesOpen(float value, bool fixedValue = false)
  {
    FBSCtrlEyes eyesCtrl = this.EyesCtrl;
    if (eyesCtrl == null)
      return;
    float num = Mathf.Clamp(value, 0.0f, 1f);
    if (fixedValue)
    {
      eyesCtrl.FixedRate = num;
    }
    else
    {
      eyesCtrl.FixedRate = -1f;
      eyesCtrl.OpenMax = num;
    }
  }

  public void ChangeEyesOpenMin(float value)
  {
    if (this.EyesCtrl == null)
      return;
    this.EyesCtrl.OpenMin = value;
  }

  public void ChangeEyesOpenMax(float value)
  {
    FBSCtrlEyes eyesCtrl = this.EyesCtrl;
    if (eyesCtrl == null)
      return;
    eyesCtrl.OpenMax = value;
  }

  public void ChangeBlinkFlag(bool blink)
  {
    this.EyesBlink = blink;
    FaceBlendShape fbsCtrl = this.FBSCtrl;
    if (Object.op_Equality((Object) fbsCtrl, (Object) null) || fbsCtrl.BlinkCtrl == null)
      return;
    this.EyesBlink = blink;
    fbsCtrl.BlinkCtrl.SetFixedFlags(!blink ? (byte) 1 : (byte) 0);
  }

  public bool GetBlinkFlag()
  {
    return this.EyesBlink;
  }

  public void ChangeMouthPtn(int ptn, bool blend = true)
  {
    this.MouthPtn = ptn;
    if (this.MouthCtrl == null)
      return;
    this.MouthCtrl.ChangePtn(ptn, blend);
    if (this.Sex == (byte) 1)
      this.ChangeMouthPtnSubFemale(ptn, blend);
    else
      this.ChangeMouthPtnSubMale(ptn, blend);
  }

  public void ChangeMouthOpen(float value, bool fixedValue = false)
  {
    FBSCtrlMouth mouthCtrl = this.MouthCtrl;
    if (mouthCtrl == null)
      return;
    float num = Mathf.Clamp(value, 0.0f, 1f);
    if (fixedValue)
    {
      mouthCtrl.FixedRate = num;
    }
    else
    {
      mouthCtrl.OpenMax = num;
      mouthCtrl.FixedRate = -1f;
    }
  }

  public void ChangeMouthOpenMin(float value)
  {
    FBSCtrlMouth mouthCtrl = this.MouthCtrl;
    if (mouthCtrl == null)
      return;
    mouthCtrl.OpenMin = value;
  }

  public void ChangeMouthOpenMax(float value)
  {
    FBSCtrlMouth mouthCtrl = this.MouthCtrl;
    if (mouthCtrl == null)
      return;
    mouthCtrl.OpenMax = value;
  }

  private void ChangeMouthPtnSubFemale(int ptn, bool blend)
  {
    if (ptn == 35 || ptn == 36)
      this.ChangeTongueState((byte) 1);
    else
      this.ChangeTongueState((byte) 0);
  }

  private void ChangeMouthPtnSubMale(int ptn, bool blend)
  {
    if (ptn == 10)
      this.ChangeTongueState((byte) 1);
    else
      this.ChangeTongueState((byte) 0);
  }

  public void ChangeTongueState(byte state)
  {
  }

  public void SetNipStand(float value)
  {
    this.SetShapeBodyValue(ChaFileDefine.cf_bodyshapename.Length - 1, value);
  }

  public byte GetSiruFlags(ChaFileDefine.SiruParts parts)
  {
    return this.SiruNewLv[(int) parts];
  }

  public void SetSiruFlags(ChaFileDefine.SiruParts parts, byte lv)
  {
    this.SiruNewLv[(int) parts] = lv;
  }

  public void SetTuyaRate(float rate)
  {
  }

  public bool SetPlay(string stateName, int layer)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return false;
    this.AnimBody.Play(stateName, layer);
    return true;
  }

  public void SetAnimatorParamBool(string strAnmName, bool flag)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Implicit((Object) this.AnimBody.get_runtimeAnimatorController()))
      return;
    this.AnimBody.SetBool(strAnmName, flag);
  }

  public bool GetAnimatorParamBool(string strAnmName)
  {
    return !Object.op_Equality((Object) this.AnimBody, (Object) null) && !Object.op_Implicit((Object) this.AnimBody.get_runtimeAnimatorController()) && this.AnimBody.GetBool(strAnmName);
  }

  public void SetAnimatorParamFloat(string strAnmName, float value)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null) || Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null))
      return;
    this.AnimBody.SetFloat(strAnmName, value);
  }

  public bool IsParameterInAnimator(string strParameter)
  {
    return !Object.op_Equality((Object) this.AnimBody, (Object) null) && !Object.op_Equality((Object) this.AnimBody.get_runtimeAnimatorController(), (Object) null) && Array.FindIndex<AnimatorControllerParameter>(this.AnimBody.get_parameters(), (Predicate<AnimatorControllerParameter>) (p => p.get_name() == strParameter)) != -1;
  }

  public void ChangeLookEyesTarget(int targetno, Transform trfTarg = null, float rate = 1f)
  {
    if (Object.op_Equality((Object) this.EyeLookControl, (Object) null))
      return;
    this.EyeLookControl.target = (Transform) null;
    if (targetno == 0)
    {
      if (!Object.op_Implicit((Object) Camera.get_main()))
        return;
      this.EyeLookControl.target = ((Component) Camera.get_main()).get_transform();
    }
    else
    {
      if (!Object.op_Implicit((Object) trfTarg))
        return;
      this.EyeLookControl.target = trfTarg;
    }
  }

  public void ChangeLookEyesPtn(int ptn)
  {
    this.EyesLookPtn = ptn;
    if (Object.op_Equality((Object) this.EyeLookControl, (Object) null))
      return;
    this.EyeLookControl.ptnNo = ptn;
  }

  public void ChangeLookNeckTarget(int targetNo, Transform trfTarg = null, float rate = 1f)
  {
    if (Object.op_Equality((Object) this.NeckLookControl, (Object) null))
      return;
    this.NeckLookControl.target = (Transform) null;
    if (targetNo == 0)
    {
      if (!Object.op_Implicit((Object) Camera.get_main()))
        return;
      this.NeckLookControl.target = ((Component) Camera.get_main()).get_transform();
    }
    else
    {
      if (!Object.op_Implicit((Object) trfTarg))
        return;
      this.NeckLookControl.target = trfTarg;
    }
  }

  public void ChangeLookNeckPtn(int ptn)
  {
    this.NeckLookPtn = ptn;
    if (Object.op_Equality((Object) this.NeckLookControl, (Object) null))
      return;
    this.NeckLookControl.ptnNo = ptn;
  }

  public bool LookAt(Transform lookat, bool position)
  {
    GameObject referenceInfo1 = this.GetReferenceInfo(CharReference.RefObjKey.H_Kokan);
    GameObject referenceInfo2 = this.GetReferenceInfo(CharReference.RefObjKey.H_DanDir);
    if (Object.op_Equality((Object) referenceInfo1, (Object) null) || Object.op_Equality((Object) lookat, (Object) null))
      return false;
    referenceInfo1.get_transform().LookAt(lookat);
    if (position && Object.op_Inequality((Object) referenceInfo2, (Object) null))
      referenceInfo2.get_transform().set_position(lookat.get_position());
    return true;
  }

  public void LoadHitObject()
  {
    this.ReleaseHitObject();
    this.ObjHitBody = (GameObject) Object.Instantiate<GameObject>((M0) this._objHitBody);
    if (Object.op_Implicit((Object) this.ObjHitBody))
    {
      this.ObjHitBody.get_transform().SetParent(this.ObjTop.get_transform(), false);
      this._aaWeightsBody.AssignedWeights(this.ObjHitBody, this.deleteBodyBoneName, (Transform) null);
      foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.ObjHitBody.GetComponentsInChildren<SkinnedCollisionHelper>(true))
        componentsInChild.Init();
    }
    if (this.Sex == (byte) 0 || !Object.op_Implicit((Object) this._objHitHead))
      return;
    this.ObjHitHead = (GameObject) Object.Instantiate<GameObject>((M0) this._objHitHead);
    if (!Object.op_Implicit((Object) this.ObjHitHead))
      return;
    this.ObjHitHead.get_transform().SetParent(this.ObjTop.get_transform(), false);
    this._aaWeightsHead.AssignedWeights(this.ObjHitHead, this.deleteHeadBoneName, (Transform) null);
    foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.ObjHitHead.GetComponentsInChildren<SkinnedCollisionHelper>(true))
      componentsInChild.Init();
  }

  public void ReleaseHitObject()
  {
    if (Object.op_Implicit((Object) this.ObjHitBody))
    {
      foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.ObjHitBody.GetComponentsInChildren<SkinnedCollisionHelper>(true))
        componentsInChild.Release();
      Object.Destroy((Object) this.ObjHitBody);
      this.ObjHitBody = (GameObject) null;
    }
    if (!Object.op_Implicit((Object) this.ObjHitHead))
      return;
    foreach (SkinnedCollisionHelper componentsInChild in (SkinnedCollisionHelper[]) this.ObjHitHead.GetComponentsInChildren<SkinnedCollisionHelper>(true))
      componentsInChild.Release();
    Object.Destroy((Object) this.ObjHitHead);
    this.ObjHitHead = (GameObject) null;
  }

  public RuntimeAnimatorController LoadAnimation(
    string assetBundleName,
    string assetName)
  {
    if (Object.op_Equality((Object) this.AnimBody, (Object) null))
      return (RuntimeAnimatorController) null;
    RuntimeAnimatorController animatorController = CommonLib.LoadAsset<RuntimeAnimatorController>(assetBundleName, assetName, false, string.Empty);
    if (Object.op_Equality((Object) animatorController, (Object) null))
      return (RuntimeAnimatorController) null;
    this.AnimBody.set_runtimeAnimatorController(animatorController);
    AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
    return animatorController;
  }

  public void ChangeTearsRate(float value)
  {
    this._tearsRate = value;
    if (!Object.op_Implicit((Object) this.TearMaterial))
      return;
    this.TearMaterial.GetFloat(TestChara._NamidaScale);
    float num = Mathf.Lerp(0.0f, 1f, value);
    this.TearMaterial.SetFloat(TestChara._NamidaScale, num);
  }

  public void ChangeHohoAkaRate(float value)
  {
    this._hohoAkaRate = value;
    if (!Object.op_Implicit((Object) this.FaceMaterial))
      return;
    this.FaceMaterial.GetFloat(TestChara._Texture4Scale);
    float num = Mathf.Lerp(0.0f, 1f, value);
    this.FaceMaterial.SetFloat(TestChara._Texture4Scale, num);
  }

  public void DisableShapeMouth()
  {
    this._updateShapeFace = true;
    this._disableShapeMouth = true;
  }

  public void EnableShapeMouth()
  {
    this._updateShapeFace = true;
    this._disableShapeMouth = false;
  }

  public void DisableShapeBust(int lr, int idx)
  {
    if (this._shapeBody == null || !this._shapeBody.InitEnd)
      return;
    for (int index = 0; index < TestChara._idx.Length; ++index)
      this._shapeBody.ChangeValue(TestChara._idx[index], this._shapeValues[TestChara._idx[index]]);
    this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length - 1, this._nipStand);
    this._updateShapeBody = true;
    switch (lr)
    {
      case 0:
        this._disableShapeBustLAry[idx] = true;
        break;
      case 1:
        this._disableShapeBustRAry[idx] = true;
        break;
      default:
        this._disableShapeBustLAry[idx] = true;
        this._disableShapeBustRAry[idx] = true;
        break;
    }
    this.ResetupDynamicBoneBust = true;
  }

  public void EnableShapeBust(int lr, int idx)
  {
    if (this._shapeBody == null || !this._shapeBody.InitEnd)
      return;
    for (int index = 0; index < TestChara._idx.Length; ++index)
      this._shapeBody.ChangeValue(TestChara._idx[index], this._shapeValues[TestChara._idx[index]]);
    this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length - 1, this._nipStand);
    this._updateShapeBody = true;
    switch (lr)
    {
      case 0:
        this._disableShapeBustLAry[idx] = false;
        break;
      case 1:
        this._disableShapeBustRAry[idx] = false;
        break;
      default:
        this._disableShapeBustLAry[idx] = false;
        this._disableShapeBustRAry[idx] = false;
        break;
    }
    this.ResetupDynamicBoneBust = true;
  }

  public void DisableShapeNip(int lr)
  {
    if (this._shapeBody == null || !this._shapeBody.InitEnd)
      return;
    for (int index = 0; index < TestChara._idx.Length; ++index)
      this._shapeBody.ChangeValue(TestChara._idx[index], this._shapeValues[TestChara._idx[index]]);
    this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length, this._nipStand);
    this._updateShapeBody = true;
    switch (lr)
    {
      case 0:
        this._disableShapeNipL = true;
        break;
      case 1:
        this._disableShapeNipR = true;
        break;
      default:
        this._disableShapeNipL = true;
        this._disableShapeNipR = true;
        break;
    }
    this.ResetupDynamicBoneBust = true;
  }

  public void EnableShapeNip(int lr)
  {
    if (this._shapeBody == null || !this._shapeBody.InitEnd)
      return;
    for (int index = 0; index < TestChara._idx.Length; ++index)
      this._shapeBody.ChangeValue(TestChara._idx[index], this._shapeValues[TestChara._idx[index]]);
    this._shapeBody.ChangeValue(ChaFileDefine.cf_bodyshapename.Length - 1, this._nipStand);
    this._updateShapeBody = true;
    switch (lr)
    {
      case 0:
        this._disableShapeNipL = false;
        break;
      case 1:
        this._disableShapeNipR = false;
        break;
      default:
        this._disableShapeNipL = false;
        this._disableShapeNipR = false;
        break;
    }
    this.ResetupDynamicBoneBust = true;
  }

  private void AddListToTag(CharReference.TagObjKey key, List<GameObject> add)
  {
    if (add == null)
      return;
    List<GameObject> gameObjectList = (List<GameObject>) null;
    if (this._dictTagObj.TryGetValue((int) key, out gameObjectList))
      gameObjectList.AddRange((IEnumerable<GameObject>) add);
    else
      this._dictTagObj[(int) key] = add;
  }

  public void CreateTagInfo(ulong flags, GameObject objTag)
  {
    TestChara testChara = this;
    if (Object.op_Equality((Object) null, (Object) objTag))
      return;
    FindAssist findAssist = new FindAssist();
    findAssist.Initialize(objTag.get_transform());
    switch (flags)
    {
      case 3:
        this.AddListToTag(CharReference.TagObjKey.ObjSkinFace, findAssist.GetObjectFromTag("ObjSkinFace"));
        this.AddListToTag(CharReference.TagObjKey.ObjEyebrow, findAssist.GetObjectFromTag("ObjEyebrow"));
        this.AddListToTag(CharReference.TagObjKey.ObjEyeL, findAssist.GetObjectFromTag("ObjEyeL"));
        this.AddListToTag(CharReference.TagObjKey.ObjEyeR, findAssist.GetObjectFromTag("ObjEyeR"));
        this.AddListToTag(CharReference.TagObjKey.ObjEyeW, findAssist.GetObjectFromTag("ObjEyeW"));
        if (testChara.Sex == (byte) 0)
          break;
        this.AddListToTag(CharReference.TagObjKey.ObjEyeHi, findAssist.GetObjectFromTag("ObjEyeHi"));
        this.AddListToTag(CharReference.TagObjKey.ObjEyelashes, findAssist.GetObjectFromTag("ObjEyelashes"));
        break;
      case 4:
        this.AddListToTag(CharReference.TagObjKey.ObjHairB, findAssist.GetObjectFromTag("ObjHair"));
        this.AddListToTag(CharReference.TagObjKey.ObjHairAcsB, findAssist.GetObjectFromTag("ObjHairAcs"));
        break;
      case 5:
        this.AddListToTag(CharReference.TagObjKey.ObjHairF, findAssist.GetObjectFromTag("ObjHair"));
        this.AddListToTag(CharReference.TagObjKey.ObjHairAcsF, findAssist.GetObjectFromTag("ObjHairAcs"));
        break;
      case 6:
        this.AddListToTag(CharReference.TagObjKey.ObjHairS, findAssist.GetObjectFromTag("ObjHair"));
        this.AddListToTag(CharReference.TagObjKey.ObjHairAcsS, findAssist.GetObjectFromTag("ObjHairAcs"));
        break;
      case 7:
        this.AddListToTag(CharReference.TagObjKey.ObjHairO, findAssist.GetObjectFromTag("ObjHair"));
        this.AddListToTag(CharReference.TagObjKey.ObjHairAcsO, findAssist.GetObjectFromTag("ObjHairAcs"));
        break;
      case 8:
        if (testChara.Sex == (byte) 0)
          break;
        this.AddListToTag(CharReference.TagObjKey.ObjNip, findAssist.GetObjectFromTag("ObjNip"));
        this.AddListToTag(CharReference.TagObjKey.ObjNail, findAssist.GetObjectFromTag("ObjNail"));
        this.AddListToTag(CharReference.TagObjKey.ObjUnderHair, findAssist.GetObjectFromTag("ObjUnderHair"));
        break;
      case 9:
        this.AddListToTag(CharReference.TagObjKey.ObjSkinBody, findAssist.GetObjectFromTag("ObjSkinBody"));
        this.AddListToTag(CharReference.TagObjKey.ObjCTop, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 10:
        this.AddListToTag(CharReference.TagObjKey.ObjCBot, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 11:
        this.AddListToTag(CharReference.TagObjKey.ObjSwim, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 12:
        this.AddListToTag(CharReference.TagObjKey.ObjSTop, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 13:
        this.AddListToTag(CharReference.TagObjKey.ObjSBot, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 14:
        this.AddListToTag(CharReference.TagObjKey.ObjBra, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 15:
        this.AddListToTag(CharReference.TagObjKey.ObjShorts, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 16:
        this.AddListToTag(CharReference.TagObjKey.ObjPanst, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 17:
        this.AddListToTag(CharReference.TagObjKey.ObjGloves, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 18:
        this.AddListToTag(CharReference.TagObjKey.ObjSocks, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 19:
        this.AddListToTag(CharReference.TagObjKey.ObjShoes, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 20:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot01, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 21:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot02, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 22:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot03, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 23:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot04, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 24:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot05, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 25:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot06, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 26:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot07, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 27:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot08, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 28:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot09, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 29:
        this.AddListToTag(CharReference.TagObjKey.ObjASlot10, findAssist.GetObjectFromTag("ObjColor"));
        break;
      case 30:
        this.AddListToTag(CharReference.TagObjKey.ObjBeard, findAssist.GetObjectFromTag("ObjBeard"));
        break;
    }
  }

  public void ReleaseTagObject(ulong flags)
  {
    switch (flags)
    {
      case 3:
        this._dictTagObj[8].Clear();
        this._dictTagObj[9].Clear();
        this._dictTagObj[10].Clear();
        this._dictTagObj[11].Clear();
        this._dictTagObj[12].Clear();
        this._dictTagObj[13].Clear();
        this._dictTagObj[15].Clear();
        break;
      case 4:
        this._dictTagObj[0].Clear();
        this._dictTagObj[4].Clear();
        break;
      case 5:
        this._dictTagObj[1].Clear();
        this._dictTagObj[5].Clear();
        break;
      case 6:
        this._dictTagObj[2].Clear();
        this._dictTagObj[6].Clear();
        break;
      case 7:
        this._dictTagObj[3].Clear();
        this._dictTagObj[7].Clear();
        break;
      case 8:
        this._dictTagObj[17].Clear();
        this._dictTagObj[16].Clear();
        this._dictTagObj[18].Clear();
        break;
      case 9:
        this._dictTagObj[19].Clear();
        this._dictTagObj[20].Clear();
        break;
      case 10:
        this._dictTagObj[21].Clear();
        break;
      case 11:
        this._dictTagObj[28].Clear();
        break;
      case 12:
        this._dictTagObj[29].Clear();
        break;
      case 13:
        this._dictTagObj[30].Clear();
        break;
      case 14:
        this._dictTagObj[22].Clear();
        break;
      case 15:
        this._dictTagObj[23].Clear();
        break;
      case 16:
        this._dictTagObj[25].Clear();
        break;
      case 17:
        this._dictTagObj[24].Clear();
        break;
      case 18:
        this._dictTagObj[26].Clear();
        break;
      case 19:
        this._dictTagObj[27].Clear();
        break;
      case 20:
        this._dictTagObj[31].Clear();
        break;
      case 21:
        this._dictTagObj[32].Clear();
        break;
      case 22:
        this._dictTagObj[33].Clear();
        break;
      case 23:
        this._dictTagObj[34].Clear();
        break;
      case 24:
        this._dictTagObj[35].Clear();
        break;
      case 25:
        this._dictTagObj[36].Clear();
        break;
      case 26:
        this._dictTagObj[37].Clear();
        break;
      case 27:
        this._dictTagObj[38].Clear();
        break;
      case 28:
        this._dictTagObj[39].Clear();
        break;
      case 29:
        this._dictTagObj[40].Clear();
        break;
      case 30:
        this._dictTagObj[14].Clear();
        break;
    }
  }

  public void ReleaseTagAll()
  {
    foreach (CharReference.TagObjKey tagObjKey in Enum.GetValues(typeof (CharReference.TagObjKey)))
      this._dictTagObj[(int) tagObjKey].Clear();
  }

  public List<GameObject> GetTagInfo(CharReference.TagObjKey key)
  {
    List<GameObject> gameObjectList = (List<GameObject>) null;
    return this._dictTagObj.TryGetValue((int) key, out gameObjectList) ? new List<GameObject>((IEnumerable<GameObject>) gameObjectList) : (List<GameObject>) null;
  }

  [Serializable]
  public class AcsGenerateInfo
  {
    [SerializeField]
    private CharReference.RefObjKey_New _key;
    [SerializeField]
    [HideInInspector]
    private CharReference.RefObjKey _parentKey;
    [SerializeField]
    private GameObject _acsObj;

    public CharReference.RefObjKey_New Key
    {
      get
      {
        return this._key;
      }
    }

    public CharReference.RefObjKey ParentKey
    {
      get
      {
        return this._parentKey;
      }
    }

    public GameObject ACSObj
    {
      get
      {
        return this._acsObj;
      }
    }
  }
}
