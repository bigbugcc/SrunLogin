import request from '@utils/request'
import { parseJsonp } from '@/utils/jsonp'

/**
 * 获取加密所需的 challenge 信息
 * @param {string} username - 用户名
 * @param {string} ip - 本机 IP
 * @returns {string} challenge 信息
 */
export async function getChallenge (username: string, ip: string)
: Promise<Record<string, unknown>> {
  const res = await request({
    url: '/cgi-bin/get_challenge',
    method: 'GET',
    params: {
      callback: 'callback',
      username,
      ip,
      _: Date.now()
    }
  })
  return parseJsonp(res.data)
}
