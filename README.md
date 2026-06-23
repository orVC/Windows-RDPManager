# RDP 连接管理器

Windows 远程桌面连接管理工具，支持存储和管理多个 RDP 连接信息。

## 功能

- 添加/编辑/删除 RDP 连接（服务器地址、用户名、密码、备注）
- 一键启动远程桌面连接
- 密码使用 Windows DPAPI 加密存储，仅当前用户可解密
- 支持同时打开多个 RDP 窗口
- 数据存储在本地 SQLite 数据库

## 系统要求

| 版本 | 支持系统 | 说明 |
|------|---------|------|
| `publish\RDPManager.exe` (自包含版) | Windows 10 1607+ / Windows 11 / Windows Server 2016+ | 无需安装任何运行时，双击即用 |
| `publish-fd\RDPManager.exe` (框架依赖版) | 同上 | 需安装 [.NET 9 Desktop Runtime](https://dotnet.microsoft.com/zh-cn/download/dotnet/9.0) |

> 不支持 Windows 7 / Windows 8.1

## 使用说明

1. 运行 `RDPManager.exe`
2. 点击 **＋ 添加** 填入服务器信息
3. 在列表中选中连接，点击 **▶ 连接** 启动远程桌面
4. 支持双击编辑、右键操作（取决于按钮）

## 数据存储

数据库文件位于：
```
%LocalAppData%\RDPManager\connections.db
```

密码通过 Windows DPAPI 加密，仅当前 Windows 用户可解密查看。

## 构建

```bash
# 框架依赖版
dotnet publish .\RDPManager\RDPManager.csproj -c Release --self-contained false -p:PublishSingleFile=true -o publish-fd

# 自包含版
dotnet publish .\RDPManager\RDPManager.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## 技术栈

- **语言**: C#
- **框架**: .NET 9 + WPF
- **数据库**: SQLite (Microsoft.Data.Sqlite)
- **密码加密**: Windows DPAPI (System.Security.Cryptography.ProtectedData)
- **RDP 启动**: cmdkey + mstsc.exe
