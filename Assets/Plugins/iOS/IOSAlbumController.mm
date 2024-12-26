#import "IOSAlbumController.h"
#import "UnityAppController.h"
#import <MobileCoreServices/MobileCoreServices.h>

@implementation IOSAlbumController

// 打开相册
+ (void)openPhotoLibrary {
    dispatch_async(dispatch_get_main_queue(), ^{
        // 获取Unity应用的控制器实例
        UnityAppController *appController = (UnityAppController *)[UnityAppController sharedAppController];
        
        // 获取根视图控制器
        UIViewController *rootViewController = appController.window.rootViewController;
        
        // 创建UIImagePickerController来打开相册
        UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
        imagePicker.delegate = (id<UIImagePickerControllerDelegate, UINavigationControllerDelegate>)appController;
        imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
        imagePicker.mediaTypes = @[(NSString *)kUTTypeImage];
        
        // 显示图片选择器
        [rootViewController presentViewController:imagePicker animated:YES completion:nil];
    });
}

// 图片选择完成后的回调
+ (void)didSelectImage:(UIImage *)image {
    NSData *imageData = UIImagePNGRepresentation(image);
    NSString *base64String = [imageData base64EncodedStringWithOptions:0];

    // 将Base64字符串传递给Unity
    UnitySendMessage("Canvas", "OnImageSelected", [base64String UTF8String]);
}

+ (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    [picker dismissViewControllerAnimated:YES completion:nil];
}

@end