# SrunLogin-云师大版

[![stars](https://img.shields.io/github/stars/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/stargazers)[![stars](https://img.shields.io/github/forks/bigbugcc/SrunLogin)](https://github.com/bigbugcc/SrunLogin/network/members)

[![issues](https://img.shields.io/github/issues/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/issues)

[![Lisense](https://img.shields.io/github/license/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/blob/main/LICENSE)

[![Release](https://img.shields.io/github/v/release/bigbugcc/SrunLogin?color=blueviolet&include_prereleases&style=flat-square)](https://github.com/bigbugcc/SrunLogin/releases)

## 🎨声明

​		作者不鼓励任何违规违纪行为，秉持着互联网的`开放`、`平等`、`共享`原则，此项目只为方便有这样需求的用户，支持在`Linux`、`Windows`、`Macos`、`Router`上使用校园网提供的便捷服务，在享受便捷服务的同时请遵守相关法律法规，一切后果自负。重申：不鼓励任何违规违纪行为！！！



## 📌使用

**配置文件参数说明**(必看)：

除上网账号密码外其它参数建议保持默认即可。

```yaml
form:
  domain: netlogin.ynnu.edu.cn
  username: "191230320xxxxxx"    #学号
  password: "xxxxxxx"			 #密码

# 基础认证参数(保持默认就可)
meta: 
  "n": "200"
  type: "1"
  acid: "1"
  enc: srun_bx1
settings:
  basic:
    https: true		# 访问协议
    skip_cert_verify: false
    timeout: 5
    interfaces: ""
    skip_net_check: false
    net_check_url: https://www.baidu.com/
  guardian: 		# 守护模式
    enable: false
    duration: 300
  daemon:			# 后台模式
    enable: false
    path: .srun
  debug:			# 日志信息
    enable: false
    write_log: false
    log_path: ./
```

[下载可执行程序](https://github.com/bigbugcc/SrunLogin/releases)

**Linux(amd64)：**

```shell
chmod +x ./SrunLogin

# 首次执行会自动生成配置文件config.yaml
./SrunLogin

# 指定配置文件
./SrunLogin --config=./config.yaml
```

**Windows(amd64)**:

```powershell
# 建议使用powershell
./SrunLogin

# 指定配置文件
./SrunLogin --config=./config.yaml
```

在此抛砖引玉，运行其他系统以及架构的操作步骤类似。

## ⏰注意

在无线路由器上运行请找到适合自己路由器架构`mips64/mips`的二进制文件上传到路由器即可。

## :hammer_and_wrench:构建

**Windows环境编译(golang=1.17)：**

```shell
# windows版本
go build main.go
```

交叉编译：

```shell
# linux-amd64
CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build main.go

# darwin-amd64
CGO_ENABLED=0 GOOS=darwin GOARCH=amd64 go build main.go

# linux-mips
CGO_ENABLED=0 GOOS=linux GOARCH=mips go build main.go
```

## 🎉鸣谢

> 项目参考：https://github.com/Mmx233/BitSrunLoginGo

> 登录算法： https://github.com/coffeehat/BIT-srun-login-script
