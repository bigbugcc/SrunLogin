# _*_ coding: utf-8 _*_
"""
Time:     2021/11/24 10:09
Author:   BigBug
Version:  V 0.1
File:     main.py
Describe: Github link: https://github.com/bigbugcc/
"""
import datetime
import time
import requests
import json

userId = "192300000000074"
password = "1122233."
url = "http://10.10.1.96"


def GetMac():
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                      "Chrome/96.0.4664.45 Safari/537.36",
        "Accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,"
                  "application/signed-exchange;v=b3;q=0.9",
        "Host": "10.10.1.96"}
    res = requests.get(url=url, headers=headers)
    return res.text.split("'")[1].split("?")[1]


def Login():
    if not GetOnlineStatus():
        urls = url + "/eportal/InterFace.do?method=login"
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                          "Chrome/96.0.4664.45 Safari/537.36",
            "Accept": "*/*",
            "Origin": "http://10.10.1.96",
            "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8"}
        querStr = GetMac()
        data = {"userId": userId, "password": password, "queryString": querStr, "passwordEncrypt": "false", }
        req = requests.post(url=urls, data=data, headers=headers)
        resp = req.text

        dicts = json.loads(resp)
        if dicts.get("result") == "success":
            print("登录成功！")
        else:
            print("登录失败！" + dicts.get("message"))


def GetOnlineStatus():
    urls = url + "/eportal/InterFace.do?method=getOnlineUserInfo"
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                      "Chrome/96.0.4664.45 Safari/537.36",
        "Accept": "*/*",
        "Origin": "http://10.10.1.96",
        "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8"}
    req = requests.post(url=urls, headers=headers)
    dicts = json.loads(req.text)
    userIndex = dicts.get("userIndex")
    if userIndex != "" and userIndex is not None:
        st = time.strftime("%Y-%m-%d %H:%M:%S", time.localtime())
        print(st + " 当前已在线 (" + dicts.get("userId") + ")")
        return 1
    return 0


def KeepService():
    urls = "https://www.baidu.com"
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) "
                      "Chrome/96.0.4664.45 Safari/537.36",
        "Accept": "*/*",
        "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8"}
    requests.get(url=urls, headers=headers)


if __name__ == '__main__':
    Login()
    while 1:
        KeepService()
        if not GetOnlineStatus():
            print("正在尝试重新登录！")
            Login()
        time.sleep(300)
