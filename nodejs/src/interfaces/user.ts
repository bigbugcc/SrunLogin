/**
 * 登录请求参数
 */
export interface LoginRequestParams {
  /** 回调函数名称 */
  callback: string
  /** 操作，登录操作为 `login` */
  action: 'login'
  /** 用户名 */
  username: string
  /** 密码，MD5 加密，`{MD5}` 前缀 */
  password: string
  /** ac_id 默认为 `1` */
  ac_id: string
  /** 客户端 IP */
  ip: string
  /** 验证所用的字符串，SHA1 加密 */
  chksum: string
  /** 加密的登录信息，`{SRBX1}` 前缀 */
  info: string
  /** n 默认为 `200` */
  n: number
  /** type 默认为 `1` */
  type: number
  /** 客户端操作系统，默认为 `Windows 10` */
  os: string
  /** 操作系统名称，默认为 `Windows` */
  name: string
  /** 多端登录，默认为 `0` */
  double_stack: number
  /** 13 位时间戳 */
  _: number
}

/**
 * 登录请求 `info` 加密所需的数据
 * 必须按以下的顺序设置数据
 */
export interface LoginInfo {
  /** 用户名 */
  username: string
  /** 明文密码 */
  password: string
  /** 客户端 IP */
  ip: string
  /** acid 默认为 `1` */
  acid: string | number
  /** 深澜软件版本，默认为 `srun_bx1` */
  enc_ver: 'srun_bx1'
}

/**
 * 登录请求 `chkstr` 所需的数据
 */
export interface ChkStrData {
  /** 用户名 */
  username: string
  /** 加密后的密码，不带 `{MD5}` 前缀 */
  encryptedPassword: string
  /** ac_id 默认为 `1` */
  ac_id: string
  /** 客户端 IP */
  ip: string
  /** n 默认为 `200` */
  n: number
  /** type 默认为 `1` */
  type: number
  /** 加密的登录信息，`{SRBX1}` 前缀 */
  i: string
}

/**
 * 登录成功响应数据
 */
export interface LoginSuccessResponse {
  ServerFlag: number
  ServicesIntfServerIP: string
  ServicesIntfServerPort: string
  access_token: string
  checkout_date: number
  client_ip: string
  ecode: number
  /** 为 `ok` 则登录成功 */
  error: string
  error_msg: string
  online_ip: string
  ploy_msg: string
  real_name: string
  remain_flux: number
  remain_times: number
  res: string
  srun_ver: string
  suc_msg: string
  sysver: string
  username: string
  wallet_balance: number
}

/**
 * 登录失败响应数据
 */
export interface LoginFailureResponse {
  client_ip: string
  ecode: string
  /** 登录失败信息 */
  error: string
  error_msg: string
  online_ip: string
  res: string
  srun_ver: string
  st: number
}
