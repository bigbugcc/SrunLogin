import { getUserInfo, portal } from '@api/user'
import {
  getChallengeStr,
  encryptPassword,
  encryptInfo,
  getChkstr
} from '@srun/encrypt'
import {
  LoginRequestParams,
  LoginInfo,
  ChkStrData,
  LoginSuccessResponse,
  LoginFailureResponse
} from '@interfaces/user'

/**
 * 获取本机 IP 地址
 * @returns {Promise<string>} 本机 IP
 */
export async function getIP (): Promise<string> {
  // 获取用户信息
  const userInfo = await getUserInfo()
  return (userInfo.client_ip || userInfo.online_ip) as string
}

/**
 * 登录校园网
 * @param {string} username - 用户名
 * @param {string} password - 密码
 */
export async function login (
  username: string,
  password: string
): Promise<LoginSuccessResponse | LoginFailureResponse> {
  // 获取 IP
  const ip: string = await getIP()

  // 获取加密所需的 challenge
  const challenge: string = await getChallengeStr(username, ip)

  // 设置登录信息
  const loginInfo: LoginInfo = {
    username,
    password,
    ip,
    acid: '1',
    enc_ver: 'srun_bx1'
  }
  const encryptedInfo: string = '{SRBX1}' + encryptInfo(loginInfo, challenge)

  // 加密用户密码
  const encryptedPassword: string = encryptPassword(password, challenge)

  // 设置请求验证字符串
  const chkstrData: ChkStrData = {
    username,
    encryptedPassword,
    ac_id: '1',
    ip,
    n: 200,
    type: 1,
    i: encryptedInfo
  }
  const chksum: string = getChkstr(chkstrData, challenge)

  // 初始化登录参数
  const loginParams: LoginRequestParams = {
    callback: 'callback',
    action: 'login',
    username,
    password: '{MD5}' + encryptedPassword,
    ac_id: '1',
    ip,
    chksum,
    info: encryptedInfo,
    n: 200,
    type: 1,
    os: 'Windows 10',
    name: 'Windows',
    double_stack: 0,
    _: Date.now()
  }

  // 发起登录请求
  const loginRes = await portal(loginParams as unknown as Record<string, unknown>)
  return loginRes as unknown as LoginSuccessResponse | LoginFailureResponse
}
