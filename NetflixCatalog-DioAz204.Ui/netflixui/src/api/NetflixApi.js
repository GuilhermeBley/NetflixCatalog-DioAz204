// axiosConfig.js
import axios from 'axios';

const API_BASE_URL = "http://localhost:7242"
const instance = create();

function create(){
  console.debug('API URL: ' + API_BASE_URL);
  return axios.create({
    baseURL: API_BASE_URL,
    timeout: 1000 * 60 * 5 // five minutes
  });
}

export default instance;