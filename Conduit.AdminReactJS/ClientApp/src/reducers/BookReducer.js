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
  } from "../actions/types";
  
  const INITIAL_STATE = {
    author: null,
    error: "",
    loading: true,
    authors: [],
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
      case USER_INFO_SUCCESS:
        return { ...state, ...INITIAL_STATE, user: action.payload };
      case USER_INFO_FAIL:
        //return { ...state, error: 'Authentication Failed.', password: '', loading: false };
        return {
          ...state,
          error: "Chứng thực thất bại !",
          author: "",
          loading: false,
        };
      case USERS_FETCHING:
        return { ...state, loading:true };
      case USERS_FETCH_SUCCESS:
        return { ...state, authors: action.payload, loading: false  };
      case USERS_FETCH_FAIL:
        return { ...state, error: action.payload, author: "", loading: false };
      default:
        return state;
    }
  };
  