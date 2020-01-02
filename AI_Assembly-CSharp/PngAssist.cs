// Decompiled with JetBrains decompiler
// Type: PngAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using OutputLogControl;
using System.IO;
using UnityEngine;

public static class PngAssist
{
  public static Sprite LoadSpriteFromFile(string path, int width, int height, Vector2 pivot)
  {
    if (!File.Exists(path))
    {
      OutputLog.Error(path + " が存在しない", true, "Log");
      return (Sprite) null;
    }
    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
      using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
      {
        byte[] numArray = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
        Texture2D texture2D = new Texture2D(width, height);
        if (Object.op_Equality((Object) null, (Object) texture2D))
          return (Sprite) null;
        ImageConversion.LoadImage(texture2D, numArray);
        if (width == 0 || height == 0)
        {
          width = ((Texture) texture2D).get_width();
          height = ((Texture) texture2D).get_height();
        }
        return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) width, (float) height), pivot);
      }
    }
  }

  public static Texture2D LoadTexture2DFromAssetBundle(
    string assetBundleName,
    string assetName)
  {
    TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
    AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
    Texture2D texture2D = new Texture2D(0, 0, (TextureFormat) 5, false, true);
    if (Object.op_Equality((Object) null, (Object) texture2D))
      return (Texture2D) null;
    ImageConversion.LoadImage(texture2D, textAsset.get_bytes());
    return texture2D;
  }

  public static Sprite LoadSpriteFromAssetBundle(
    string assetBundleName,
    string assetName,
    int width,
    int height,
    Vector2 pivot)
  {
    TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(assetBundleName, assetName, false, string.Empty);
    AssetBundleManager.UnloadAssetBundle(assetBundleName, true, (string) null, false);
    Texture2D texture2D = new Texture2D(width, height);
    if (Object.op_Equality((Object) null, (Object) texture2D))
      return (Sprite) null;
    ImageConversion.LoadImage(texture2D, textAsset.get_bytes());
    if (width == 0 || height == 0)
    {
      width = ((Texture) texture2D).get_width();
      height = ((Texture) texture2D).get_height();
    }
    return Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) width, (float) height), pivot);
  }

  public static Texture2D ChangeTextureFromByte(
    byte[] data,
    int width = 0,
    int height = 0,
    TextureFormat format = 5,
    bool mipmap = false)
  {
    Texture2D texture2D = new Texture2D(width, height, format, mipmap);
    if (Object.op_Equality((Object) null, (Object) texture2D))
      return (Texture2D) null;
    ImageConversion.LoadImage(texture2D, data);
    return texture2D;
  }

  public static void SavePng(
    BinaryWriter writer,
    int capW = 504,
    int capH = 704,
    int createW = 252,
    int createH = 352,
    float renderRate = 1f,
    bool drawBackSp = true,
    bool drawFrontSp = true)
  {
    byte[] pngData = (byte[]) null;
    PngAssist.CreatePng(ref pngData, capW, capH, createW, createH, renderRate, drawBackSp, drawFrontSp);
    if (pngData == null)
      return;
    writer.Write(pngData);
  }

  public static void CreatePng(
    ref byte[] pngData,
    int capW = 504,
    int capH = 704,
    int createW = 252,
    int createH = 352,
    float renderRate = 1f,
    bool drawBackSp = true,
    bool drawFrontSp = true)
  {
    GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("SpriteTop");
    Vector2 screenSize = ScreenInfo.GetScreenSize();
    float screenRate = ScreenInfo.GetScreenRate();
    float screenCorrectY = ScreenInfo.GetScreenCorrectY();
    float num1 = (float) (720.0 * (double) screenRate / screenSize.y);
    int num2 = capW;
    int num3 = capH;
    int num4 = (int) ((double) num2 * (double) renderRate);
    int num5 = (int) ((double) num3 * (double) renderRate);
    RenderTexture renderTexture = QualitySettings.get_antiAliasing() != 0 ? RenderTexture.GetTemporary((int) (1280.0 * (double) renderRate / (double) num1), (int) (720.0 * (double) renderRate / (double) num1), 24, (RenderTextureFormat) 0, (RenderTextureReadWrite) 0, QualitySettings.get_antiAliasing()) : RenderTexture.GetTemporary((int) (1280.0 * (double) renderRate / (double) num1), (int) (720.0 * (double) renderRate / (double) num1), 24);
    if (drawBackSp && Object.op_Inequality((Object) null, (Object) gameObjectWithTag))
    {
      GameObject loop = gameObjectWithTag.get_transform().FindLoop("BackSpCam");
      if (Object.op_Inequality((Object) null, (Object) loop))
      {
        Camera component = (Camera) loop.GetComponent<Camera>();
        if (Object.op_Inequality((Object) null, (Object) component))
        {
          component.set_targetTexture(renderTexture);
          component.Render();
          component.set_targetTexture((RenderTexture) null);
        }
      }
    }
    if (Object.op_Inequality((Object) null, (Object) Camera.get_main()))
    {
      Camera main = Camera.get_main();
      RenderTexture targetTexture = main.get_targetTexture();
      Rect rect = main.get_rect();
      main.set_targetTexture(renderTexture);
      main.Render();
      main.set_targetTexture(targetTexture);
      main.set_rect(rect);
    }
    if (drawFrontSp && Object.op_Inequality((Object) null, (Object) gameObjectWithTag))
    {
      GameObject loop = gameObjectWithTag.get_transform().FindLoop("FrontSpCam");
      if (Object.op_Inequality((Object) null, (Object) loop))
      {
        Camera component = (Camera) loop.GetComponent<Camera>();
        if (Object.op_Inequality((Object) null, (Object) component))
        {
          component.set_targetTexture(renderTexture);
          component.Render();
          component.set_targetTexture((RenderTexture) null);
        }
      }
    }
    Texture2D tex = new Texture2D(num4, num5, (TextureFormat) 5, false, true);
    RenderTexture.set_active(renderTexture);
    float num6 = (float) ((double) (1280 - capW) / 2.0 * (double) renderRate + (1280.0 / (double) num1 - 1280.0) * 0.5 * (double) renderRate);
    float num7 = (float) ((double) (720 - capH) / 2.0 * (double) renderRate + (double) screenCorrectY / (double) screenRate * (double) renderRate);
    tex.ReadPixels(new Rect(num6, num7, (float) num4, (float) num5), 0, 0);
    tex.Apply();
    RenderTexture.set_active((RenderTexture) null);
    RenderTexture.ReleaseTemporary(renderTexture);
    if (num4 != createW || num5 != createH)
      TextureScale.Bilinear(tex, createW, createH);
    pngData = ImageConversion.EncodeToPNG(tex);
    Object.Destroy((Object) tex);
  }

  public static void CreatePngScreen(ref byte[] pngData, int createW, int createH)
  {
    Vector2 screenSize = ScreenInfo.GetScreenSize();
    int x = (int) screenSize.x;
    int y = (int) screenSize.y;
    Texture2D tex = new Texture2D(x, y, (TextureFormat) 3, false);
    RenderTexture renderTexture = QualitySettings.get_antiAliasing() != 0 ? RenderTexture.GetTemporary(x, y, 24, (RenderTextureFormat) 7, (RenderTextureReadWrite) 0, QualitySettings.get_antiAliasing()) : RenderTexture.GetTemporary(x, y, 24);
    if (Object.op_Inequality((Object) null, (Object) Camera.get_main()))
    {
      Camera main = Camera.get_main();
      RenderTexture targetTexture = main.get_targetTexture();
      Rect rect = main.get_rect();
      main.set_targetTexture(renderTexture);
      main.Render();
      main.set_targetTexture(targetTexture);
      main.set_rect(rect);
    }
    RenderTexture.set_active(renderTexture);
    tex.ReadPixels(new Rect(0.0f, 0.0f, (float) x, (float) y), 0, 0);
    tex.Apply();
    RenderTexture.set_active((RenderTexture) null);
    RenderTexture.ReleaseTemporary(renderTexture);
    TextureScale.Bilinear(tex, createW, createH);
    pngData = ImageConversion.EncodeToPNG(tex);
    Object.Destroy((Object) tex);
  }

  public static Sprite LoadSpriteFromFile(string path)
  {
    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
      long pngSize = PngFile.GetPngSize((Stream) fileStream);
      if (pngSize == 0L)
        return (Sprite) null;
      using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
      {
        byte[] data = binaryReader.ReadBytes((int) pngSize);
        int width = 0;
        int height = 0;
        Texture2D texture2D = PngAssist.ChangeTextureFromPngByte(data, ref width, ref height, (TextureFormat) 5, false);
        return Object.op_Equality((Object) null, (Object) texture2D) ? (Sprite) null : Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) width, (float) height), new Vector2(0.5f, 0.5f));
      }
    }
  }

  public static Texture2D ChangeTextureFromPngByte(
    byte[] data,
    ref int width,
    ref int height,
    TextureFormat format = 5,
    bool mipmap = false)
  {
    Texture2D texture2D = new Texture2D(width, height, format, mipmap);
    if (Object.op_Equality((Object) null, (Object) texture2D))
      return (Texture2D) null;
    ImageConversion.LoadImage(texture2D, data);
    texture2D.Apply(true, true);
    width = ((Texture) texture2D).get_width();
    height = ((Texture) texture2D).get_height();
    return texture2D;
  }

  public static Texture2D LoadTexture(string _path)
  {
    using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read))
    {
      long pngSize = PngFile.GetPngSize((Stream) fileStream);
      if (pngSize == 0L)
        return (Texture2D) null;
      using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
      {
        byte[] data = binaryReader.ReadBytes((int) pngSize);
        int width = 0;
        int height = 0;
        return PngAssist.ChangeTextureFromPngByte(data, ref width, ref height, (TextureFormat) 5, false);
      }
    }
  }
}
