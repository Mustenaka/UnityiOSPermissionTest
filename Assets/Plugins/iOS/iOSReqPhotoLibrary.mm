#import <Foundation/Foundation.h>
#import <Photos/Photos.h>
#import <UIKit/UIKit.h>

// Unity回调声明
extern "C" {
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
}

@interface PhotoPickerDelegate : NSObject <UIImagePickerControllerDelegate, UINavigationControllerDelegate>
+ (void)presentImagePickerFromViewController:(UIViewController *)viewController;
@end

@implementation PhotoPickerDelegate

+ (void)presentImagePickerFromViewController:(UIViewController *)viewController {
    static PhotoPickerDelegate *delegate = nil;
    if (!delegate) {
        delegate = [[PhotoPickerDelegate alloc] init];
    }
    
    UIImagePickerController *picker = [[UIImagePickerController alloc] init];
    picker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    picker.delegate = delegate;
    [viewController presentViewController:picker animated:YES completion:nil];
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey,id> *)info {
    UIImage *selectedImage = info[UIImagePickerControllerOriginalImage];
    
    // 创建临时文件路径
    NSString *tempDir = NSTemporaryDirectory();
    NSString *fileName = [NSString stringWithFormat:@"selected_photo_%d.jpg", (int)[[NSDate date] timeIntervalSince1970]];
    NSString *filePath = [tempDir stringByAppendingPathComponent:fileName];
    
    // 保存图片
    NSData *imageData = UIImageJPEGRepresentation(selectedImage, 0.8);
    [imageData writeToFile:filePath atomically:YES];
    
    // 发送路径回Unity
    UnitySendMessage("PhotoManager", "OnPhotoSelected", [filePath UTF8String]);
    
    // 关闭选择器
    [picker dismissViewControllerAnimated:YES completion:nil];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    [picker dismissViewControllerAnimated:YES completion:nil];
}

@end

// C函数实现
extern "C" {
    void openPhotoLibrary() {
        dispatch_async(dispatch_get_main_queue(), ^{
            UIViewController *rootViewController = UnityGetGLViewController();
            
            PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
            if (status == PHAuthorizationStatusNotDetermined) {
                [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus newStatus) {
                    if (newStatus == PHAuthorizationStatusAuthorized) {
                        dispatch_async(dispatch_get_main_queue(), ^{
                            [PhotoPickerDelegate presentImagePickerFromViewController:rootViewController];
                        });
                    }
                }];
            } else if (status == PHAuthorizationStatusAuthorized) {
                [PhotoPickerDelegate presentImagePickerFromViewController:rootViewController];
            }
        });
    }
}