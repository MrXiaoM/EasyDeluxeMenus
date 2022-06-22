﻿# EasyDeluxeMenus [WIP] 树形菜单编辑器

## 介绍

这是一个 DeluxeMenus 菜单编辑器，可以帮助你简单便捷全面地编写一个 DeluxeMenus 菜单。

目前还在制作中，敬请期待。

## 进度
* [x] 菜单读写
* [x] 仿 Minecraft 悬浮提示
* [x] 可视化物品图标选择器
* [ ] 软件设置
* [x] 编辑菜单设置
* [ ] 编辑物品设置
* [ ] 编辑点击命令
* [ ] 编辑点击需求
* [x] 生成预览
* [ ] 支持CHEST类型以外的菜单

## 使用库
* [aaubry/YamlDotNet](https://github.com/aaubry/YamlDotNet)
* [NingShenTian/CsharpJson](https://github.com/NingShenTian/CsharpJson)

## 使用字体
* [GNU Unifont Glyphs](http://www.unifoundry.com/unifont/index.html)

## 参考文档
* [PlaceholderAPI/wiki](https://github.com/PlaceholderAPI/PlaceholderAPI/wiki)
* [HelpChat Wiki/DeluxeMenus](https://wiki.helpch.at/clips-plugins/deluxemenus)

## 构建

请在编译之前，把 `items.7z` 解压，并确保项目目录中存在文件 `items/_config.json`。避免编译后不会将默认资源复制到编译目录。

这么做的原因是，`items` 内文件过多，若不压缩，提交或者拉取本项目可能会花费过多时间。
