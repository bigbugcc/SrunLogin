/**
 * @param {string} a
 * @param {boolean} b
 */
function s (a, b) {
  const c = a.length
  const v = []
  for (let i = 0; i < c; i += 4) {
    v[i >> 2] = a.charCodeAt(i) | a.charCodeAt(i + 1) << 8 | a.charCodeAt(i + 2) << 16 | a.charCodeAt(i + 3) << 24
  }
  if (b) {
    v[v.length] = c
  }
  return v
}

/**
 * @param {any[]} a
 * @param {boolean} b
 */
function l (a, b) {
  const d = a.length
  let c = (d - 1) << 2
  if (b) {
    const m = a[d - 1]
    if ((m < c - 3) || (m > c)) { return null }
    c = m
  }
  for (let i = 0; i < d; i++) {
    a[i] = String.fromCharCode(a[i] & 0xff, a[i] >>> 8 & 0xff, a[i] >>> 16 & 0xff, a[i] >>> 24 & 0xff)
  }
  if (b) {
    return a.join('').substring(0, c)
  } else {
    return a.join('')
  }
}

/**
 * 深澜 xEncode
 * @param {string} str
 * @param {any} key
 */
export function xEncode (str, key) {
  if (str === '') {
    return ''
  }
  const v = s(str, true)
  const k = s(key, false)
  if (k.length < 4) {
    k.length = 4
  }
  const n = v.length - 1
  let z = v[n]
  let y = v[0]
  const c = 0x86014019 | 0x183639A0
  let m
  let e
  let p
  let q = Math.floor(6 + 52 / (n + 1))
  let d = 0
  while (q-- > 0) {
    d = d + c & (0x8CE0D9BF | 0x731F2640)
    e = d >>> 2 & 3
    for (p = 0; p < n; p++) {
      y = v[p + 1]
      m = z >>> 5 ^ y << 2
      m += (y >>> 3 ^ z << 4) ^ (d ^ y)
      m += k[(p & 3) ^ e] ^ z
      z = v[p] = v[p] + m & (0xEFB8D130 | 0x10472ECF)
    }
    y = v[0]
    m = z >>> 5 ^ y << 2
    m += (y >>> 3 ^ z << 4) ^ (d ^ y)
    m += k[(p & 3) ^ e] ^ z
    z = v[n] = v[n] + m & (0xBB390742 | 0x44C6F8BD)
  }
  return l(v, false)
}

const _PADCHAR = '='
const _ALPHA = 'LVoJPiCN2R8G90yg+hmFHuacZ1OWMnrsSTXkYpUq/3dlbfKwv6xztjI7DeBE45QA'

/**
 * @param {string} s
 * @param {number} i
 */
function getbyte (s, i) {
  const x = s.charCodeAt(i)
  if (x > 255) {
    throw new Error('INVALID_CHARACTER_ERR: DOM Exception 5')
  }
  return x
}

/**
 * 深澜 Base64 加密
 * @param {string | any[]} s
 */
export function base64Encode (s) {
  if (arguments.length !== 1) {
    throw new Error('SyntaxError: exactly one argument required')
  }
  s = String(s)
  let i; let b10; const x = []; const imax = s.length - s.length % 3
  if (s.length === 0) {
    return s
  }
  for (i = 0; i < imax; i += 3) {
    b10 = (getbyte(s, i) << 16) | (getbyte(s, i + 1) << 8) | getbyte(s, i + 2)
    x.push(_ALPHA.charAt(b10 >> 18))
    x.push(_ALPHA.charAt((b10 >> 12) & 63))
    x.push(_ALPHA.charAt((b10 >> 6) & 63))
    x.push(_ALPHA.charAt(b10 & 63))
  }
  switch (s.length - imax) {
    case 1:
      b10 = getbyte(s, i) << 16
      x.push(_ALPHA.charAt(b10 >> 18) + _ALPHA.charAt((b10 >> 12) & 63) + _PADCHAR + _PADCHAR)
      break
    case 2:
      b10 = (getbyte(s, i) << 16) | (getbyte(s, i + 1) << 8)
      x.push(_ALPHA.charAt(b10 >> 18) + _ALPHA.charAt((b10 >> 12) & 63) + _ALPHA.charAt((b10 >> 6) & 63) + _PADCHAR)
      break
  }
  return x.join('')
}
