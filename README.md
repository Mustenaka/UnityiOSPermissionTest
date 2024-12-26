# UnityiOSPermissionTest

Unity打包iOS并测试相册访问权限Demo，使用Unity调用iOS原生Objective-C代码，实现获取相册，同理论可以获取各种权限。

### 环境要求

- Unity版本：Unity6.0.32f1
- XCode版本：16.2
- iOS版本：18
- 开发环境：macOS（用于打包）

### 结构

主要的项目结构如下

```
Assets/
├── Plugins/
│   └── iOS/
│       └── PhotoLibraryPlugin.mm  // iOS原生插件
└── Scripts/
    └── PhotoManager.cs           // Unity主要逻辑脚本
```

确保场景包含如下的一个结构，并将代码挂在PhotoManager中：

```
- PhotoManager (Empty GameObject)
  |- Canvas
     |- Image (用于显示照片)
     |- Button (用于触发选择)
```

### iOS插件代码功能

用于实现在Unity应用中访问iOS相册的功能。插件提供了相册权限请求、图片选择和Unity回调等功能。

## 核心功能

1. iOS相册权限管理
2. 系统相册选择器调用
3. 图片文件处理
4. Unity回调通信

### 运行编译

###### Unity

Unity选择iOS打包（不论）

###### XCode

配置Target - Unity-iPhone - Signing & Capabilities 配置证书，或者 个人测试证书

配置Target - UnityFramework - Build Phases - Link Binary With Libraries - add 添加“Photos.framework”

配置Target - Unity-iPhone - Info (info.plist 文件) 添加 “Privacy - Photo Library Usage Description” 并添加权限获取提示 ｜ 添加“Privacy - Photo Library Additions Usage Description” 并添加权限获取提示

### 项目问题

1. 图片过多时加载时间略久，可以优化
2. 注意iOS的API变化，版本变化大概率没法用，本机在iOS18测试
