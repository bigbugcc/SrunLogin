# SrunLogin-äºå¸å¤§ç

[![stars](https://img.shields.io/github/stars/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/stargazers)[![stars](https://img.shields.io/github/forks/bigbugcc/SrunLogin)](https://github.com/bigbugcc/SrunLogin/network/members)

[![issues](https://img.shields.io/github/issues/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/issues)

[![Lisense](https://img.shields.io/github/license/bigbugcc/SrunLogin?style=flat-square)](https://github.com/bigbugcc/SrunLogin/blob/main/LICENSE)

[![Release](https://img.shields.io/github/v/release/bigbugcc/SrunLogin?color=blueviolet&include_prereleases&style=flat-square)](https://github.com/bigbugcc/SrunLogin/releases)

## ð¨å£°æ

â		ä½èä¸é¼å±ä»»ä½è¿è§è¿çºªè¡ä¸ºï¼ç§æçäºèç½ç`å¼æ¾`ã`å¹³ç­`ã`å±äº«`ååï¼æ­¤é¡¹ç®åªä¸ºæ¹ä¾¿æè¿æ ·éæ±çç¨æ·ï¼æ¯æå¨`Linux`ã`Windows`ã`Macos`ã`Router`ä¸ä½¿ç¨æ ¡å­ç½æä¾çä¾¿æ·æå¡ï¼å¨äº«åä¾¿æ·æå¡çåæ¶è¯·éµå®ç¸å³æ³å¾æ³è§ï¼ä¸ååæèªè´ãéç³ï¼ä¸é¼å±ä»»ä½è¿è§è¿çºªè¡ä¸ºï¼ï¼ï¼



## ðä½¿ç¨

**éç½®æä»¶åæ°è¯´æ**(å¿ç)ï¼

é¤ä¸ç½è´¦å·å¯ç å¤å¶å®åæ°å»ºè®®ä¿æé»è®¤å³å¯ã

```yaml
form:
  domain: netlogin.ynnu.edu.cn
  username: "191230320xxxxxx"    #å­¦å·
  password: "xxxxxxx"			 #å¯ç 

# åºç¡è®¤è¯åæ°(ä¿æé»è®¤å°±å¯)
meta: 
  "n": "200"
  type: "1"
  acid: "1"
  enc: srun_bx1
settings:
  basic:
    https: true		# è®¿é®åè®®
    skip_cert_verify: false
    timeout: 5
    interfaces: ""
    skip_net_check: false
    net_check_url: https://www.baidu.com/
  guardian: 		# å®æ¤æ¨¡å¼
    enable: false
    duration: 300
  daemon:			# åå°æ¨¡å¼
    enable: false
    path: .srun
  debug:			# æ¥å¿ä¿¡æ¯
    enable: false
    write_log: false
    log_path: ./
```

[ä¸è½½å¯æ§è¡ç¨åº](https://github.com/bigbugcc/SrunLogin/releases)

**Linux(amd64)ï¼**

```shell
chmod +x ./SrunLogin

# é¦æ¬¡æ§è¡ä¼èªå¨çæéç½®æä»¶config.yaml
./SrunLogin

# æå®éç½®æä»¶
./SrunLogin --config=./config.yaml
```

**Windows(amd64)**:

```powershell
# å»ºè®®ä½¿ç¨powershell
./SrunLogin

# æå®éç½®æä»¶
./SrunLogin --config=./config.yaml
```

å¨æ­¤æç å¼çï¼è¿è¡å¶ä»ç³»ç»ä»¥åæ¶æçæä½æ­¥éª¤ç±»ä¼¼ã

## â°æ³¨æ

å¨æ çº¿è·¯ç±å¨ä¸è¿è¡è¯·æ¾å°éåèªå·±è·¯ç±å¨æ¶æ`mips64/mips`çäºè¿å¶æä»¶ä¸ä¼ å°è·¯ç±å¨å³å¯ã

## :hammer_and_wrench:æå»º

**Windowsç¯å¢ç¼è¯(golang=1.17)ï¼**

```shell
# windowsçæ¬
go build main.go
```

äº¤åç¼è¯ï¼

```shell
# linux-amd64
CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build main.go

# darwin-amd64
CGO_ENABLED=0 GOOS=darwin GOARCH=amd64 go build main.go

# linux-mips
CGO_ENABLED=0 GOOS=linux GOARCH=mips go build main.go
```

## ðé¸£è°¢

> é¡¹ç®åèï¼https://github.com/Mmx233/BitSrunLoginGo

> ç»å½ç®æ³ï¼ https://github.com/coffeehat/BIT-srun-login-script
