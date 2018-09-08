import {
    FULLNAME_CHANGED,
    ADDRESS_CHANGED,
    USER_INFO_FAIL,
    USER_INFO_SUCCESS,
    BANK_CHANGED,
    LOGIN_INFO_USER,
    USERS_FETCH_SUCCESS,
    USERS_FETCH_FAIL,
    USERS_FETCHING,
    USERS_DELETE_FETCH_SUCCESS
  } from "./types";
  import { GetAuthors,DeleteAuthor } from './../apis/BookApi';
  import {PageHelper} from './../utils/PageHelper';
  import _ from 'lodash';
  
  export const fullNameChanged = text => {
    return {
      type: FULLNAME_CHANGED,
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
          payload: authors.data.result,
          page:pagination
        });
      }).catch(e=>{
        
        if(!_.isUndefined(e.response))
        {
          dispatch({ type: USERS_FETCH_FAIL,payload: e.response.data.errors.message });
        }
      });
    };
  };
  export const deleteAuthorAction = (id) => {
    return dispatch => {
      dispatch({ type: USERS_FETCHING });
      DeleteAuthor(id).then(author=>{
        dispatch({
          type: USERS_DELETE_FETCH_SUCCESS,
          payload: author.data.result,
          id:id
        });
      }).catch(e=>{
        if(!_.isUndefined(e.response))
        {
          dispatch({ type: USERS_FETCH_FAIL,payload: e.response.data.errors.message });
        }
      });
    };
  };
  