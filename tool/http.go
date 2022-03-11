package tool

import (
	"crypto/tls"
	"encoding/json"
	"errors"
	"fmt"
	"github.com/PuerkitoBio/goquery"
	"io"
	"io/ioutil"
	"net"
	"net/http"
	"net/http/cookiejar"
	url2 "net/url"
	"reflect"
	"strings"
	"time"
)

type GenTransport struct {
	Timeout           time.Duration
	LocalAddr         net.Addr
	IdleConnTimeout   time.Duration
	SkipSslCertVerify bool
}

type FullRequest struct {
	Type              string
	Url               string
	Header            map[string]interface{}
	Query             map[string]interface{}
	Body              interface{}
	Cookie            map[string]string
	Redirect          bool
	RedirectCookieJar bool
	Transport         *http.Transport
}

type GetRequest struct {
	Url               string
	Header            map[string]interface{}
	Query             map[string]interface{}
	Cookie            map[string]string
	Redirect          bool
	RedirectCookieJar bool
	Transport         *http.Transport
}

type PostRequest struct {
	Url               string
	Header            map[string]interface{}
	Query             map[string]interface{}
	Body              interface{}
	Cookie            map[string]string
	Redirect          bool
	RedirectCookieJar bool
	Transport         *http.Transport
}

type httP struct { //HTTP操作工具包
	DefaultHeader    map[string]interface{} //默认爬虫header
	DefaultTransport *http.Transport
}

var HTTP = httP{
	DefaultHeader: map[string]interface{}{
		"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36",
	},
	DefaultTransport: &http.Transport{
		DialContext: (&net.Dialer{
			Timeout: time.Second * 30,
		}).DialContext,
		TLSHandshakeTimeout: time.Second * 30,
	},
}

// 接收指针
func (*httP) fillFullReq(Type string, s interface{}) *FullRequest {
	var r = FullRequest{
		Type: Type,
	}
	v2 := reflect.ValueOf(&r).Elem()
	t := reflect.TypeOf(s).Elem()
	v := reflect.ValueOf(s).Elem()
	for i := 0; i < v.NumField(); i++ {
		v2.FieldByName(t.Field(i).Name).Set(v.Field(i))
	}
	return &r
}

func (a *httP) GenTransport(r *GenTransport) *http.Transport {
	return &http.Transport{
		DialContext: (&net.Dialer{
			Timeout:   r.Timeout,
			LocalAddr: r.LocalAddr,
		}).DialContext,
		TLSHandshakeTimeout: r.Timeout,
		IdleConnTimeout:     r.IdleConnTimeout,
		TLSClientConfig:     &tls.Config{InsecureSkipVerify: r.SkipSslCertVerify},
	}
}

// GenRequest 生成请求 底层函数
func (a *httP) GenRequest(Type string, url string, header map[string]interface{}, query map[string]interface{}, body interface{}, cookies map[string]string) (*http.Request, error) {
	//表单
	var form string
	if body != nil {
		v := reflect.ValueOf(body)
		if _, ok := header["Content-Type"]; !ok {
			if header == nil {
				header = make(map[string]interface{}, 1)
			}
			switch v.Kind() {
			case reflect.Struct:
				header["Content-Type"] = "application/json; charset=utf-8"
			case reflect.Map:
				header["Content-Type"] = "application/x-www-form-urlencoded; charset=utf-8"
			default:
				return nil, errors.New("tool http: cannot encode body")
			}
		}
		switch {
		case strings.Contains(header["Content-Type"].(string), "x-www-form-urlencoded"):
			var data = make(url2.Values)
			switch v.Kind() {
			case reflect.Map:
				for _, key := range v.MapKeys() {
					data[fmt.Sprint(key.Interface())] = []string{fmt.Sprint(v.MapIndex(key).Interface())}
				}
			default:
				return nil, errors.New("tool http: cannot encode body")
			}
			form = data.Encode()
		case strings.Contains(header["Content-Type"].(string), "json"):
			s, e := json.Marshal(body)
			if e != nil {
				return nil, e
			}
			form = string(s)
		}
	}

	req, err := http.NewRequest(Type, url, strings.NewReader(form))
	if err != nil {
		return nil, err
	}

	//请求头
	if header != nil {
		for k, v := range a.DefaultHeader {
			if _, ok := header[k]; !ok {
				header[k] = v
			}
		}
	} else {
		header = a.DefaultHeader
	}

	for k, v := range header {
		req.Header.Add(k, fmt.Sprint(v))
	}

	//url参数
	q := req.URL.Query()
	for k, v := range query {
		q.Add(k, fmt.Sprint(v))
	}
	req.URL.RawQuery = q.Encode()

	//cookie
	for k, v := range cookies {
		req.AddCookie(&http.Cookie{
			Name:  k,
			Value: v,
		})
	}

	return req, nil
}

// DefaultReader 执行请求获得io reader的默认流程
func (a *httP) DefaultReader(r *FullRequest) (*http.Response, error) {
	req, e := a.GenRequest(r.Type, r.Url, r.Header, r.Query, r.Body, r.Cookie)
	if e != nil {
		return nil, e
	}

	if r.Transport == nil {
		r.Transport = a.DefaultTransport
	}
	var client = &http.Client{
		Transport: r.Transport,
	}

	if !r.Redirect {
		client.CheckRedirect = func(req *http.Request, via []*http.Request) error {
			return http.ErrUseLastResponse
		}
	} else if r.RedirectCookieJar {
		jar, e := cookiejar.New(nil)
		if e != nil {
			return nil, e
		}
		client.Jar = jar
		if r.Cookie != nil {
			client.CheckRedirect = func(req *http.Request, via []*http.Request) error {
				u, _ := url2.Parse(r.Url)
				for _, v := range jar.Cookies(u) {
					r.Cookie[v.Name] = v.Value
				}
				return nil
			}
		}
	}

	resp, err := client.Do(req)
	if err != nil {
		return nil, err
	}

	return resp, nil
}

// PostReader 执行POST请求，获得io reader
func (a *httP) PostReader(r *PostRequest) (*http.Response, error) {
	return a.DefaultReader(a.fillFullReq("POST", r))
}

// GetReader 执行GET请求，获得io reader
func (a *httP) GetReader(r *GetRequest) (*http.Response, error) {
	return a.DefaultReader(a.fillFullReq("GET", r))
}

func (*httP) ReadResBodyToByte(i io.ReadCloser) ([]byte, error) {
	defer func() {
		_ = i.Close()
	}()
	return ioutil.ReadAll(i)
}

func (a *httP) ReadResBodyToString(i io.ReadCloser) (string, error) {
	d, e := a.ReadResBodyToByte(i)
	return string(d), e
}

// DecodeResBodyToMap 读取io reader中返回的json写入map
func (a *httP) DecodeResBodyToMap(i io.ReadCloser) (map[string]interface{}, error) {
	var t map[string]interface{}
	return t, json.NewDecoder(i).Decode(&t)
}

// Post 表单请求快捷方式
func (a *httP) Post(r *PostRequest) (*http.Response, map[string]interface{}, error) {
	res, e := a.PostReader(r)
	if e != nil {
		return nil, nil, e
	}
	c, e := a.DecodeResBodyToMap(res.Body)
	return res, c, nil
}

// Get 表单请求快捷方式
func (a *httP) Get(r *GetRequest) (*http.Response, map[string]interface{}, error) {
	res, e := a.GetReader(r)
	if e != nil {
		return nil, nil, e
	}
	c, e := a.DecodeResBodyToMap(res.Body)
	return res, c, nil
}

func (a *httP) PostBytes(r *PostRequest) (*http.Response, []byte, error) {
	res, e := a.PostReader(r)
	if e != nil {
		return nil, nil, e
	}
	c, e := a.ReadResBodyToByte(res.Body)
	return res, c, nil
}

func (a *httP) GetBytes(r *GetRequest) (*http.Response, []byte, error) {
	res, e := a.GetReader(r)
	if e != nil {
		return nil, nil, e
	}
	c, e := a.ReadResBodyToByte(res.Body)
	return res, c, nil
}

func (a *httP) PostString(r *PostRequest) (*http.Response, string, error) {
	res, e := a.PostReader(r)
	if e != nil {
		return nil, "", e
	}
	c, e := a.ReadResBodyToString(res.Body)
	return res, c, nil
}

func (a *httP) GetString(r *GetRequest) (*http.Response, string, error) {
	res, e := a.GetReader(r)
	if e != nil {
		return nil, "", e
	}
	c, e := a.ReadResBodyToString(res.Body)
	return res, c, nil
}

func (a httP) DefaultGoquery(r *FullRequest) (*goquery.Document, error) {
	res, e := a.DefaultReader(r)
	if e != nil {
		return nil, e
	}
	d, e := goquery.NewDocumentFromReader(res.Body)
	_ = res.Body.Close()
	return d, e
}

func (a httP) GetGoquery(r *GetRequest) (*goquery.Document, error) {
	return a.DefaultGoquery(a.fillFullReq("GET", r))
}

func (a httP) PostGoquery(r *PostRequest) (*goquery.Document, error) {
	return a.DefaultGoquery(a.fillFullReq("POST", r))
}
