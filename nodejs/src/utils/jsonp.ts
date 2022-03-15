/**
 * Parse JSONP string to Record
 * @param {string} jsonp - JSONP string
 * @returns {Record<string, unknown>} JSONP data
 */
export function parseJsonp (jsonp: string)
: Record<string, unknown> {
  const dataStr = jsonp
    .replace(/^(.+?)\(/, '')
    .replace(/\)$/, '')
  return JSON.parse(dataStr)
}

/**
 * Stringify Record to JSONP string
 * @param {Record} data - Data for stringify
 * @param {string} jsonp - JSONP stringcallbackName - callback function name
 * @returns {string} Stringified JSONP string
 */
export function stringifyJsonp (
  data: Record<string | number | symbol, unknown>,
  callbackName: string
): string {
  const dataStr = JSON.stringify(data)
  return `${callbackName}(${dataStr})`
}
