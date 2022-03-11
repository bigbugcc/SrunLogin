package controllers

import (
	"github.com/bigbugcc/SrunLogin/global"
	BitSrun "github.com/bigbugcc/SrunLogin/v1"
	"github.com/bigbugcc/SrunLogin/v1/transfer"
	"net"
)

// Login 登录逻辑
func Login(output bool, skipCheck bool, localAddr net.Addr) error {
	return BitSrun.Login(&srunTransfer.Login{
		Https:       global.Config.Settings.Basic.Https,
		Debug:       global.Config.Settings.Debug.Enable,
		WriteLog:    global.Config.Settings.Debug.WriteLog,
		OutPut:      output,
		CheckNet:    !skipCheck,
		CheckNetUrl: global.Config.Settings.Basic.NetCheckUrl,
		LoginInfo: srunTransfer.LoginInfo{
			Form: &global.Config.Form,
			Meta: &global.Config.Meta,
		},
		Transport: global.Transports(localAddr),
	})
}
