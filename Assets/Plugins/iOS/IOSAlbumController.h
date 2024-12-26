// IOSAlbumController.h
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface IOSAlbumController : NSObject <UIImagePickerControllerDelegate, UINavigationControllerDelegate>

// 打开相册
+ (void)openPhotoLibrary;

// 处理图片选择后的回调
+ (void)didSelectImage:(UIImage *)image;

@end