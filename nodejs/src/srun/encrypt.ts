import { getChallenge } from '@api/challenge'
import { HmacMD5, enc, SHA1 } from 'crypto-js'
import { LoginInfo, ChkStrData } from '@interfaces/user'
import { xEncode, base64Encode } from '@srun/srunLib'

/**
 * 获取加密所需的 challenge 字符串
 * @param {string} username - 用户名
 * @param {string} ip - 本机 IP
 * @returns {string} challenge 字符串
 */
export async function getChallengeStr (
  username: string,
  ip: string
): Promise<string> {
  const challengeInfo = await getChallenge(username, ip)
  return challengeInfo.challenge as string
}

/**
 * 加密密码
 * @param password - 用户密码
 * @param token - 从服务端获取的 challenge
 * @returns MD5 加密结果
 */
export function encryptPassword (
  password: string,
  token: string
): string {
  const passwordWordArray = enc.Utf8.parse(password)
  const tokenWordArray = enc.Utf8.parse(token)
  return HmacMD5(passwordWordArray, tokenWordArray).toString()
}

/**
 * 加密登录信息
 * @param info - 登录信息所需的数据
 * @param token - 从服务端获取的 challenge
 * @returns 加密后的登录信息
 */
export function encryptInfo (
  info: LoginInfo,
  token: string
): string {
  const xEncodeRes: string = xEncode(JSON.stringify(info), token) as string
  return base64Encode(xEncodeRes)
}

/**
 * 获取登录请求验证字符串
 * @param data - 验证字符串所需的数据
 * @param token - 从服务端获取的 challenge
 * @returns 登录请求验证字符串
 */
export function getChkstr (
  data: ChkStrData,
  token: string
): string {
  let chkstr: string = ''
  chkstr = token + data.username
  chkstr += token + data.encryptedPassword
  chkstr += token + data.ac_id
  chkstr += token + data.ip
  chkstr += token + data.n
  chkstr += token + data.type
  chkstr += token + data.i
  const wordArray = enc.Utf8.parse(chkstr)
  return SHA1(wordArray).toString()
}
