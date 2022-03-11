package tool

import (
	"strings"
)

type cookie struct{}

var Cookie cookie

// Decode 从header中解析cookie进map
func (*cookie) Decode(a string, o map[string]string) map[string]string {
	var t map[string]string
	if o != nil {
		t = o
	} else {
		t = make(map[string]string)
	}
	for _, v := range strings.Split(a, ";") {
		tt := strings.Split(strings.TrimSpace(v), "=")
		switch tt[0] { //滤去一些杂质
		case "path":
		case "HttpOnly":
		case "SameSite":
		case "":
		default:
			t[tt[0]] = strings.TrimSpace(strings.Join(tt[1:], ""))
		}
	}
	return t
}
