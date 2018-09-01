import axios from "./CmeAxiosConfig";


export const GetInfoApp = (keyApp, token = "", os) =>
  axios.post("GetInfoApp", {
    KeyCodeActive: keyApp,
    Token: token,
    IOS: os
  });

 
export const GetAuthors = (page=1,size=2,searchquery='') =>
  axios.get(`api/authors?pagenumber=${page}&pagesize=${size}&searchquery=${searchquery}`);
