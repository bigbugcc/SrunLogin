import request from '@utils/request'
import { parseJsonp } from '@/utils/jsonp'

/**
 * 获取用户信息 API
 * @returns {Promise<Record<string, unknown>>} 用户信息
 */
export async function getUserInfo ()
: Promise<Record<string, unknown>> {
  const res = await request({
    url: '/cgi-bin/rad_user_info',
    method: 'GET',
    params: {
      callback: 'callback',
      _: Date.now()
    }
  })
  return parseJsonp(res.data)
}

/**
 * 用户操作 API
 * @param {Record<string, unknown>} params - 用户操作请求参数
 * @returns {Promise<Record<string, unknown>>} 请求结果
 */
export async function portal (params: Record<string, unknown>)
: Promise<Record<string, unknown>> {
  const res = await request({
    url: '/cgi-bin/srun_portal',
    method: 'GET',
    params
  })
  return parseJsonp(res.data)
}
