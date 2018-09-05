import {
    FULLNAME_CHANGED,
    ADDRESS_CHANGED,
    USER_INFO_SUCCESS,
    USER_INFO_FAIL,
    LOGIN_INFO_USER,
    BANK_CHANGED,
    USERS_FETCHING,
    USERS_FETCH_SUCCESS,
    USERS_FETCH_FAIL,
    USERS_DELETE_FETCH_SUCCESS
  } from "../actions/types";
  import { Page } from "../utils/PageHelper";
  import _ from 'lodash';
  const INITIAL_STATE = {
    author: {},
    error: "",
    loading: true,
    authors: [],
    page:new Page("",1,"",1,1,1),
    searchquery:''
  };
  
  export default (state = INITIAL_STATE, action) => {
    switch (action.type) {
      case FULLNAME_CHANGED:
        return { ...state, fullname: action.payload };
      case ADDRESS_CHANGED:
        return { ...state, address: action.payload };
      case BANK_CHANGED:
        return { ...state, bank: action.payload };
      case LOGIN_INFO_USER:
        return { ...state, loading: true, error: "" };
      case USERS_DELETE_FETCH_SUCCESS:
        var evens = _.remove(state.authors, (n)=> {
          return n.Id != action.id;
        });
        return { ...state,authors:evens, author: action.payload, loading: false,error:"OKKKKK"};
           case USERS_FETCHING:
        return { ...state, loading:true };
      case USERS_FETCH_SUCCESS:
        return { ...state, authors: action.payload, loading: false,page: action.page };
      case USERS_FETCH_FAIL:
        return { ...state, error: action.payload, author: "", loading: false };
      default:
        return state;
    }
  };
  