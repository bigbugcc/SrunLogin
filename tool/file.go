package tool

import (
	"bufio"
	"encoding/json"
	"io/ioutil"
	"os"
	"path/filepath"
	"strings"
)

type file struct{}

var File file

func (a *file) Exists(path string) bool {
	_, err := os.Stat(path)
	if err != nil {
		if os.IsExist(err) {
			return true
		}
		return false
	}
	return true
}

func (a *file) ReadAll(path string) ([]byte, error) {
	return ioutil.ReadFile(path)
}

func (a *file) ReadJson(path string, receiver interface{}) error {
	f, e := os.Open(path)
	if e != nil {
		return e
	}
	defer f.Close()
	return json.NewDecoder(f).Decode(receiver)
}

func (a *file) WriteAll(path string, data []byte) error {
	return ioutil.WriteFile(path, data, 700)
}

func (a file) WriteJson(path string, data interface{}) error {
	f, e := os.OpenFile(path, os.O_WRONLY|os.O_CREATE, 700)
	if e != nil {
		return e
	}
	defer f.Close()
	return json.NewEncoder(f).Encode(data)
}

func (a *file) WriteJsonIntend(path string, receiver interface{}) error {
	data, err := json.MarshalIndent(receiver, "", " ")
	if err != nil {
		return err
	}
	return a.WriteAll(path, data)
}

func (a *file) Remove(path string) error {
	return os.RemoveAll(path)
}

func (a *file) Mkdir(path string) error {
	return os.MkdirAll(path, 0700)
}

func (a *file) DecodeName(name string) (string, string) {
	t := strings.Split(name, ".")
	return strings.Join(t[:len(t)-1], ""), t[len(t)-1]
}

func (a *file) Add(path string, c string, perm os.FileMode) error {
	f, err := os.OpenFile(path, os.O_WRONLY|os.O_CREATE|os.O_APPEND, perm)
	defer func(file *os.File) {
		_ = file.Close()
	}(f)
	if err != nil {
		return err
	}
	w := bufio.NewWriter(f)
	if _, err = w.WriteString(c + "\n"); err != nil {
		return err
	}
	return w.Flush()
}

func (*file) GetRuntimePath() (string, error) {
	t, err := os.Executable()
	if err != nil {
		return "", err
	}
	return filepath.Dir(t) + "/", nil
}
