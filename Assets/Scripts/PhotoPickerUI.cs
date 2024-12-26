using UnityEngine;
using UnityEngine.UI;
using Plugins;

/// <summary>
/// 相片选择UI：
///     Unity侧引用IOS代码，在iOS平台时，通过调用IOSAlbumController.openPhotoLibrary()打开相册选择图片
/// </summary>
public class PhotoPickerUI : MonoBehaviour
{
    public Button pickImageButton; // 按钮
    public Image selectedImage;    // 用来显示选中的图片的UI Image组件

    void Start()
    {
        // 为按钮添加点击事件
        pickImageButton.onClick.AddListener(OnPickImageButtonClicked);
    }

    private void OnDestroy()
    {
        // 移除按钮点击事件
        pickImageButton.onClick.RemoveListener(OnPickImageButtonClicked);
    }

    /// <summary>
    /// 按钮点击事件：打开相册选择图片
    ///     WARNING: 该方法只在iOS平台调用
    /// </summary>
    private void OnPickImageButtonClicked()
    {
        Debug.Log("OnPickImageButtonClicked");
        
        // 只在iOS平台调用打开相册功能
        #if UNITY_IOS && !UNITY_EDITOR
        // IOSAlbumController.openPhotoLibrary();
        iOSCall.openPhotoLibrary();
        #endif
    }

    /// <summary>
    /// Unity接收从iOS返回的图片Base64字符串
    /// </summary>
    /// <param name="base64Image"></param>
    public void OnImageSelected(string base64Image)
    {
        // 将Base64字符串转换为图片
        byte[] imageBytes = System.Convert.FromBase64String(base64Image);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageBytes); // 将图片数据转换为纹理

        // 设置Image组件的纹理为选中的图片
        selectedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}