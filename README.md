# TestTool

>>>>>>>Base64InOutZIP<<<<<<<
ZIPToString.exe
[IN.ZIP] 转换为String 出力在 [out.txt] 里
StringToZIP.exe
[out.txt] 的字符串 变成 元ZIP文件[OUT.ZIP]

>>>>>>>EngW<<<<<<<
时间指定后，在规定时间内最大可输出字符数

>>>>>>>japanWord<<<<<<<
单词

>>>>>>>MapSearch<<<<<<<
地图上，从开始寻找最近路到终点

>>>>>>>MouseFinger<<<<<<<
1 设置两次点击的间隔时间。
2 点击开始
3 将鼠标移动到想不停点击的地方
4 鼠标不动，点击空格键
5 开始自动点击
6 想停止时，再次点击空格键
7 从2开始循环，开始新一次的程序执行

>>>>>>>Win2048<<<<<<<
2048








# YourFirstRepoDK20170906

`20170906`

# GitHub.comのアカウント

Sign-in失敗しないように慎重にお願いいたします

- IDが、 **イニシャル＋社員番号** 例 `nt022345`  
- パスワードが、 **社員番号＋イニシャル** `022345nt`

---

## 本日のマテリアル

[GitHub for developers](http://ikeike443.github.io/training-kit/courses/github-for-developers.html)  
[GitHub Services On-demand training](https://services.github.com/on-demand/)

### gitツール

[Git for Windows](https://git-for-windows.github.io/)  
[GitHub Desktop](https://desktop.github.com/)  

### git資料

[Gitチートシート](https://services.github.com/on-demand/downloads/ja/github-git-cheat-sheet.pdf) - 印刷用PDF  
[Pro Git 2 eBook](http://git-scm.com/book/ja/v2) - Gitの一番詳しい電子書籍。無料。

### 絵文字

[Emoji seacher](http://emoji.muan.co/) - 絵文字検索

### GitHub

[GitHub Guides](https://guides.github.com/)  
[GitHub Training & Guides](https://www.youtube.com/watch?v=FyfwLX4HAxM&list=PLg7s6cbtAD15G8lNyoaYDuKZSKyJrgwB-) - Youtube (日本語字幕付)   
[What is GitHub?](https://www.youtube.com/watch?v=w3jLJU7DT5E)

## git setup

### アカウント

nameとemailはGitHubに作ったアカウントと一致させてください。  
`$ git config --global user.name "name"`  
`$ git config --global user.email "email address"`  

確認  
`$ git config --list | grep name`  
`$ git config --list | grep email`  

### 表示と改行コードの設定

`$ git config --global color.ui auto`  

Windows  
`$ git config --global core.autocrlf true`  
Mac/Linux  
`$ git config --global core.autocrlf input`  

**改行コードを変更しない場合**  
`$ git config --global core.autocrlf false`

### Windowsユーザ

Git Bashでの日本語の文字化け対策  
`$ git config --global core.quotepath false`  
  
CommitメッセージをvimではなくNotepadで入力する場合。  
`$ git config --global core.editor notepad`  
*NotepadはSJISのため日本語はWEB上で文字化けしますのでご注意ください。*  
  
サクラエディタを指定してもOK。  
`$ git config --global core.editor "'C:/Program Files/sakura/sakura.exe' -CODE=4"`  
*sakura.exeのパスは、各自のローカルでご確認ください。*

秀丸(未確認 :bow: )  
`$ git config –global core.editor “‘C:/program files/hidemaru/hidemaru.exe’ //fu8”`

## 以下は必要に応じて設定を行ってください

### HTTP Proxy
`$ git config --global http.proxy http://proxy02.kentaku.co.jp:12080`  
#### 消し方
`$ git config --global --unset http.proxy`

### その他

GitHub Enterpriseを自己署名証明書で運用する場合(GitHub Enterprise使用する場合のみ)  
`$ git config --global http.sslVerify false`

### テスト
