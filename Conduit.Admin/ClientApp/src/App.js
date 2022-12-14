/*!
 *
 * Angle - Bootstrap Admin Template
 *
 * Version: 4.0
 * Author: @themicon_co
 * Website: http://themicon.co
 * License: https://wrapbootstrap.com/help/licenses
 *
 */

import React, { Component } from 'react';
import { BrowserRouter } from 'react-router-dom';
import $ from 'jquery';

// App Routes
import Routes from './Routes';

// Vendor dependencies
import "./Vendor";
// Application Styles
import './styles/bootstrap.scss';
import './styles/app.scss'
import { Provider } from "react-redux";
//import { BrowserRouter, Route, Link, Switch } from "react-router-dom";
import { createStore, applyMiddleware } from "redux";
import ReduxThunk from "redux-thunk";
import reducers from "./reducers";
// Disable warning "Synchronous XMLHttpRequest on the main thread is deprecated.."
$.ajaxPrefilter(o => o.async = true);

const store = createStore(reducers, {}, applyMiddleware(ReduxThunk));
class App extends Component {
  
  render() {
   
    // specify base href from env varible 'WP_BASE_HREF'
    // use only if application isn't served from the root
    // for development it is forced to root only
    /* global WP_BASE_HREF */
    const basename = process.env.NODE_ENV === 'development' ? '/' : (WP_BASE_HREF || '/');

    return (
      <Provider store={store}>
        <BrowserRouter basename={basename}>
            <Routes />
        </BrowserRouter>
      </Provider>
    );

  }
}

export default App;
