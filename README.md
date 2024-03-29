# UFE Extension
![Screenshot 2022-03-29 180737](https://user-images.githubusercontent.com/61408011/160576464-eb8c0277-1dc4-4e3c-b2f8-376ee6a0fb2c.png)

UFE Extensionは、UFEエンジンに追加機能を実装するものです。
UFEの元のコードをなるべく変更することなく機能を追加することを目標に作成しています。

[English README](https://github.com/sansansan-333/UFE-Extension/blob/main/README-en.md)

## バージョン
Unity 2020.3.6

UFE2 2.4.1

## 導入 
1. このレポジトリをクローンし、Assetsフォルダ以下に置く。

2. Assets/UFE/Demos/_2DFighterにあるDemo_Fighter2Dシーンを開く。

3. シーン内に空のGameObjectを置き、それにUFEExtension.csをアタッチする。  

4. UFE/Engine/Scripts/Core/Manager/UFE.csを開き、以下のように編集する。

Before
```cs
...
public class UFE : MonoBehaviour, UFEInterface
...
```
After
```cs
...
public partial class UFE : MonoBehaviour, UFEInterface
...
```


## 使い方
初めにUFE Extensionファイルを作成する必要があります。拡張機能の設定ファイルのようなものです。

Projectビューを右クリック、Create > U.F.E. > Extension Fileを選択することで作成できます。

作られたファイルは、Extensionウインドウから編集できます。Extensionウインドウは画面上部のメニューから、Window > U.F.E. > Extensionで開けます。

作成したUFE Extensionファイルは、導入手順の2番目でアタッチしたUFEExtension.csのExtension Infoにドラッグ＆ドロップしてください。

項目別の使い方は以下を参照してください。

### AI Settings
- Override AI

CPU戦でExtension内のAIを使うかどうかを指定します。

- AI engine

Override AIがONの場合表示されます。AIの種類を指定します。BaseAIを継承しているクラスがここに表示されます。

### Record games
- Record games

Game Recordingを記録するかどうかを指定します。

- Folder

Game Recordingの保存先を指定します。

- Description

保存されるjsonのdescriptionに入る値を指定します。


## AIを自作する
UFEExtensionでは、BaseAIを継承したクラスをオリジナルのAIとして動かすことができます。  
いくつかの関数をオーバーライドする必要があります。例としてAI/SpamAI.csを参照してください。

## プラグインを追加する
UFE Extension内で外部プラグインを使いたい場合（例えばAIを作るためにニューラルネットワーク用のフレームワークを入れたい、など）、次の作業を行う必要があります。

1. Extensionフォルダ以下の好きな場所にPluginsフォルダを作る。
2. Pluginsフォルダに追加したいプラグインを置く。
3. UFE/Engine/Scripts/Core/UFE3D.asmdefを選択し、インスペクターから"Assembly References"の欄にプラグインを追加する。

（３つ目の手順が必要なのは、UFEExtension全体がUFE3D.asmdefに属しているためです。）
