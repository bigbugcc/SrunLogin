package tool

import (
	"regexp"
)

type reg struct{}

var Regexp reg

func (reg) MatchExisting(reg string, a string) bool {
	m, e := regexp.Match(reg, []byte(a))
	if e != nil {
		panic(e)
	}
	return m
}

func (reg) MatchValue(reg string, a string) [][]string {
	r, e := regexp.Compile(reg)
	if e != nil {
		panic(e)
	}
	m := r.FindAllStringSubmatch(a, -1)
	return m
}

func (reg) Replace(reg string, o string, n string) string {
	r, e := regexp.Compile(reg)
	if e != nil {
		panic(e)
	}
	return r.ReplaceAllString(o, n)
}
