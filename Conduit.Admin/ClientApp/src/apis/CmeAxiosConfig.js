import { UrlPublish } from "../utils/Constraint";
import axios from "axios";
var instance = axios.create({
  baseURL: UrlPublish
});
//instance.defaults.baseURL = UrlPublishCme;
instance.defaults.timeout = 10000;
//axios.defaults.headers.common['Authorization'] = AUTH_TOKEN;
//axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
export default instance;
