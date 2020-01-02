// Decompiled with JetBrains decompiler
// Type: ADVFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ADVFade : MonoBehaviour
{
  public bool isStartRun;
  [SerializeField]
  private Image filterFront;
  [SerializeField]
  private Image filterBack;
  private bool isEnd;
  private string frontImageAssetBundleName;
  private string backImageAssetBundleName;
  private ADVFade.Fade front;
  private ADVFade.Fade back;
  private int frontIndex;
  private int backIndex;
  private readonly Color initColor;

  public ADVFade()
  {
    base.\u002Ector();
  }

  public Image FilterFront
  {
    get
    {
      return this.filterFront;
    }
  }

  public Image FilterBack
  {
    get
    {
      return this.filterBack;
    }
  }

  public int FrontIndex
  {
    get
    {
      return this.frontIndex;
    }
  }

  public int BackIndex
  {
    get
    {
      return this.backIndex;
    }
  }

  public bool IsFadeInEnd
  {
    get
    {
      return this.front.isFadeInEnd || this.back.isFadeInEnd;
    }
  }

  public bool IsEnd
  {
    get
    {
      return this.isEnd;
    }
  }

  public void Initialize()
  {
    if (this.front == null)
      this.frontIndex = ((Transform) ((Graphic) this.filterFront).get_rectTransform()).GetSiblingIndex();
    if (this.back == null)
      this.backIndex = ((Transform) ((Graphic) this.filterBack).get_rectTransform()).GetSiblingIndex();
    ((Graphic) this.filterFront).set_color(this.initColor);
    this.front = new ADVFade.Fade(this.filterFront, this.initColor, 0.0f, true, true);
    ((Graphic) this.filterBack).set_color(this.initColor);
    this.back = new ADVFade.Fade(this.filterBack, this.initColor, 0.0f, true, true);
  }

  public void SetColor(bool isFront, Color color)
  {
    (!isFront ? (Graphic) this.back.filter : (Graphic) this.front.filter).set_color(color);
  }

  public void CrossFadeAlpha(bool isFront, float alpha, float time, bool ignoreTimeScale)
  {
    Color color = !isFront ? ((Graphic) this.back.filter).get_color() : ((Graphic) this.front.filter).get_color();
    color.a = (__Null) (double) alpha;
    this.CrossFadeColor(isFront, color, time, ignoreTimeScale);
  }

  public void CrossFadeColor(bool isFront, Color color, float time, bool ignoreTimeScale)
  {
    if (isFront)
      this.front = new ADVFade.Fade(this.front.filter, color, time, ignoreTimeScale, true);
    else
      this.back = new ADVFade.Fade(this.back.filter, color, time, ignoreTimeScale, true);
    this.isEnd = false;
  }

  public void LoadSprite(bool isFront, string bundleName, string assetName)
  {
    AssetBundleLoadAssetOperation loadAssetOperation = AssetBundleManager.LoadAsset(bundleName, assetName, typeof (Sprite), (string) null);
    Sprite asset1 = loadAssetOperation.GetAsset<Sprite>();
    if (Object.op_Equality((Object) asset1, (Object) null))
    {
      Texture2D asset2 = loadAssetOperation.GetAsset<Texture2D>();
      asset1 = Sprite.Create(asset2, new Rect(0.0f, 0.0f, (float) ((Texture) asset2).get_width(), (float) ((Texture) asset2).get_height()), Vector2.get_zero());
    }
    if (isFront)
    {
      this.front.filter.set_sprite(asset1);
      if (!this.frontImageAssetBundleName.IsNullOrEmpty())
        AssetBundleManager.UnloadAssetBundle(this.frontImageAssetBundleName, false, (string) null, false);
      this.frontImageAssetBundleName = bundleName;
    }
    else
    {
      this.back.filter.set_sprite(asset1);
      if (!this.backImageAssetBundleName.IsNullOrEmpty())
        AssetBundleManager.UnloadAssetBundle(this.backImageAssetBundleName, false, (string) null, false);
      this.backImageAssetBundleName = bundleName;
    }
  }

  public void ReleaseSprite(bool isFront)
  {
    if (isFront)
    {
      this.front.filter.set_sprite((Sprite) null);
      if (this.frontImageAssetBundleName.IsNullOrEmpty())
        return;
      AssetBundleManager.UnloadAssetBundle(this.frontImageAssetBundleName, false, (string) null, false);
      this.frontImageAssetBundleName = (string) null;
    }
    else
    {
      this.back.filter.set_sprite((Sprite) null);
      if (this.backImageAssetBundleName.IsNullOrEmpty())
        return;
      AssetBundleManager.UnloadAssetBundle(this.backImageAssetBundleName, false, (string) null, false);
      this.backImageAssetBundleName = (string) null;
    }
  }

  private void Awake()
  {
    this.Initialize();
  }

  private void Update()
  {
    this.isEnd = true;
    if (this.front.Update() && this.front.isFadeOutEnd)
    {
      ((Graphic) this.filterFront).set_raycastTarget(false);
    }
    else
    {
      this.isEnd = false;
      ((Graphic) this.filterFront).set_raycastTarget(true);
    }
    if (this.back.Update() && this.back.isFadeOutEnd)
      return;
    this.isEnd = false;
  }

  private void OnDestroy()
  {
    if (!Singleton<AssetBundleManager>.IsInstance())
      return;
    this.ReleaseSprite(true);
    this.ReleaseSprite(false);
  }

  public class Fade
  {
    public Image filter;
    public Color initColor;
    public Color color;
    public float time;
    public float timer;
    public bool ignoreTimeScale;

    public Fade(Image filter, Color color, float time, bool ignoreTimeScale, bool isUpdate = true)
    {
      this.filter = filter;
      this.initColor = ((Graphic) filter).get_color();
      this.color = color;
      this.time = time;
      this.ignoreTimeScale = ignoreTimeScale;
      this.timer = 0.0f;
      if (!isUpdate)
        return;
      this.Update();
    }

    public bool isFadeInEnd
    {
      get
      {
        return ((Graphic) this.filter).get_color().a == 1.0;
      }
    }

    public bool isFadeOutEnd
    {
      get
      {
        return ((Graphic) this.filter).get_color().a == 0.0;
      }
    }

    public bool IsEnd
    {
      get
      {
        return (double) this.time == (double) this.timer;
      }
    }

    public bool Update()
    {
      float num = Time.get_deltaTime();
      if (this.ignoreTimeScale)
        num = Time.get_unscaledDeltaTime();
      this.timer += num;
      this.timer = Mathf.Min(this.timer, this.time);
      ((Graphic) this.filter).set_color(Color.Lerp(this.initColor, this.color, (double) this.time != 0.0 ? Mathf.InverseLerp(0.0f, this.time, this.timer) : 1f));
      return this.IsEnd;
    }
  }
}
