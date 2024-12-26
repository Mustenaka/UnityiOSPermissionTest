using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class PhotoLibraryAccess : MonoBehaviour
{
    public Button loadButton;
    public Image image;

    private void Start()
    {
        loadButton.onClick.AddListener(OpenPhotoLibrary);
    }

    private void OnDestroy()
    {
        loadButton.onClick.RemoveListener(OpenPhotoLibrary);
    }

    // 声明原生插件方法
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _RequestPhotoLibraryAccess();
#endif

    public void OpenPhotoLibrary()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // 请求相册访问权限
            _RequestPhotoLibraryAccess();
            Debug.Log("Load photo library");
        }
#endif
    }

    // 这个方法会被原生插件回调
    public void OnPhotoSelected(string imagePath)
    {
        // 加载选中的图片
        StartCoroutine(LoadImage(imagePath));
    }

    private IEnumerator LoadImage(string imagePath)
    {
        WWW www = new WWW("file://" + imagePath);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            // 获取到图片,可以赋值给UI等
            Texture2D texture = www.texture;
            // 这里处理获取到的图片
        }
    }
}