import axios from 'axios'
import { BASE_URL } from '@/config'

// Create Axios instance
const service = axios.create({
  baseURL: BASE_URL,
  timeout: 0
})

export default service
