using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections;

public class PhotoManager : MonoBehaviour
{
    [SerializeField]
    private Image targetImage; // 用于显示照片的Image组件
    
    [SerializeField]
    private Button selectButton; // 选择照片的按钮
    
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void openPhotoLibrary();
#endif
    
    // 添加组件检查
    private void Awake()
    {
        // 如果没有手动赋值，尝试在子对象中查找
        if (targetImage == null)
        {
            targetImage = GetComponentInChildren<Image>();
        }
        
        if (selectButton == null)
        {
            selectButton = GetComponentInChildren<Button>();
        }
    }
    
    private void Start()
    {
        // 确保已赋值
        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OnSelectButtonClick);
        }
    }

    private void OnDestroy()
    {
        if (selectButton != null)
        {
            selectButton.onClick.RemoveListener(OnSelectButtonClick);
        }
    }

    void OnSelectButtonClick()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            openPhotoLibrary();
        }
#endif
    }
    
    // 这个方法将被iOS原生代码调用
    public void OnPhotoSelected(string imagePath)
    {
        StartCoroutine(LoadSelectedImage(imagePath));
    }
    
    private IEnumerator LoadSelectedImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            Debug.LogError("Image path is null or empty");
            yield break;
        }

        using (WWW www = new WWW("file://" + imagePath))
        {
            yield return www;
            
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Error loading image: " + www.error);
                yield break;
            }
            
            // 创建一个新的Texture2D
            Texture2D texture = new Texture2D(2, 2);
            www.LoadImageIntoTexture(texture);
            
            // 创建Sprite并设置到Image组件
            if (targetImage != null)
            {
                Sprite sprite = Sprite.Create(texture, 
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));
                    
                targetImage.sprite = sprite;
            }
        }
    }
}