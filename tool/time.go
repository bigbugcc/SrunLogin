package tool

import (
	"fmt"
	"time"
)

type timE struct{}

var Time timE

// ToString nt为精度
func (timE) ToString(t time.Duration, nt uint) string {
	var s string
	sec := int64(t.Seconds())
	worker := func(a *int64, b int64, s string) string {
		if (*a) >= b {
			t := (*a) / b
			*a -= t * b
			return fmt.Sprint(t, s)
		}
		return ""
	}
	var t1 = []int64{
		60 * 60 * 24 * 30,
		60 * 60 * 24,
		60 * 60,
		60,
		1,
	}
	var t2 = []string{
		"月",
		"天",
		"时",
		"分",
		"秒",
	}
	for i, v := range t1 {
		if nt == 0 {
			break
		}
		s += worker(&sec, v, t2[i])
		if s != "" {
			nt--
		}
	}
	return s
}

func (*timE) IsToday(t time.Time) bool {
	now := time.Now()
	ZeroToday := time.Date(now.Year(), now.Month(), now.Day(), 0, 0, 0, 0, now.Location())
	tomorrow := now.Add(time.Hour * 24)
	ZeroTomorrow := time.Date(tomorrow.Year(), tomorrow.Month(), tomorrow.Day(), 0, 0, 0, 0, tomorrow.Location())
	if t.Sub(ZeroTomorrow) > 0 || ZeroToday.Sub(t) > 0 {
		return false
	}
	return true
}
