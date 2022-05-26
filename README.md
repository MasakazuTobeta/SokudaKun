
# Sokudakun (速打くん)
任意の間隔でマウスをクリックする連打ソフト.

## 動作環境
Windows 10(32/64bit), 11(32/64bit)

## 追加のライブラリ
[.NET 3.1 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/3.1/runtime?cid=getdotnetcore)のインストールが必要。

[Downloadページ](https://dotnet.microsoft.com/en-us/download/dotnet/3.1/runtime?cid=getdotnetcore)から、「Run desktop apps」の「[Download x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.25-windows-x86-installer)」もしくは「[Download x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-3.1.25-windows-x64-installer)（32bit OS）」をダウンロードしインストール。

## 使い方
### クリック間隔設定
テキストボックスにミリ秒単位で入力
### マウスクリック開始
F10キー、もしくはユーザーが設定したショートカットキーで開始。
### マウスクリック停止
LeftAltキー、もしくはユーザーが設定したショートカットキーで停止。
### ショートカットキー変更
メインウィンドウの右下⚙マークから設定ウィンドウを起動し、設定後にOKで確定。
#### 開始キー設定
Set start keysのチェックボックスをON[✓]し、任意のキーを入力。※厳密には、Keyの同時押しは認識しておらず、一定時間内にKeyを離した順序を認識している
#### 停止キー設定
Set stop keysのチェックボックスをON[✓]し、任意のキーを入力。※厳密には、Keyの同時押しは認識しておらず、一定時間内にKeyを離した順序を認識している

## 要望・不具合報告
https://github.com/MasakazuTobeta/SokudaKun/issues

## ライセンス
- [MIT License](https://github.com/MasakazuTobeta/SokudaKun/blob/master/LICENSE.md)
- フリーウェア
- 商用利用可

## Release一覧
https://github.com/MasakazuTobeta/SokudaKun/releases

### v1.0.x (2022.05.22)
* Visual C# (.NET 3.2)
* 開始/停止キーを可変化

### v0.1 (初版: 1999年頃)
* Basic系言語HSP

## 連絡先
tobeta@mlmarket.jp
