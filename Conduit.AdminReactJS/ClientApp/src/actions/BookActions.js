import {
    FULLNAME_CHANGED,
    ADDRESS_CHANGED,
    USER_INFO_FAIL,
    USER_INFO_SUCCESS,
    BANK_CHANGED,
    LOGIN_INFO_USER,
    USERS_FETCH_SUCCESS,
    USERS_FETCH_FAIL,
    USERS_FETCHING
  } from "./types";
  import { GetAuthors } from './../apis/BookApi';
  import {PageHelper} from './../utils/PageHelper';
  export const fullNameChanged = text => {
    return {
      type: FULLNAME_CHANGED,
      payload: text,
    };
  };
  
  export const addressChanged = text => {
    return {
      type: ADDRESS_CHANGED,
      payload: text,
    };
  };
  
  export const bankChanged = text => {
    return {
      type: BANK_CHANGED,
      payload: text,
    };
  };
  
  export const addUser = ({ fullname, address, bank }) => {
    return dispatch => {
      dispatch({ type: LOGIN_INFO_USER });
      setTimeout(() => {
        try {
          loginUserSuccess(dispatch, { fullname, address, bank });
        } catch (ex) {
          loginUserFail(dispatch);
        }
      },2000);
  
    };
  };
  
  const loginUserFail = dispatch => {
    dispatch({ type: USER_INFO_FAIL });
  };
  
  const loginUserSuccess = (dispatch, user) => {
    dispatch({
      type: USER_INFO_SUCCESS,
      payload: user,
    });
   
  };
  
  export const getAuthorsAction = (page,size,searchquery) => {
    return dispatch => {
      dispatch({ type: USERS_FETCHING });
      GetAuthors(page,size,searchquery).then(authors=>{
        
        var pagination=  new PageHelper(authors.headers["x-pagination"]).getPage()
        dispatch({
          type: USERS_FETCH_SUCCESS,
          payload: authors.data,
          page:pagination
        });
      }).catch(e=>{
        dispatch({ type: USERS_FETCH_FAIL });
      });
    };
  };
  