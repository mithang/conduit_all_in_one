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

import Vue from 'vue'
import BootstrapVue from 'bootstrap-vue'
import VueI18Next from '@panter/vue-i18next';
import axios from 'axios'
import VueAxios from 'vue-axios'

import './vendor.js'

import App from './App.vue'
import router from './router'
import i18next from './i18n.js';



Vue.config.productionTip = false

Vue.use(BootstrapVue);
Vue.use(VueI18Next);
Vue.use(VueAxios, axios)

var url = "http://localhost:58161/api/authors?pagenumber=1&pagesize=2&orderby=genre&fields=name,id&searchquery=g";
Vue.axios.get(url).then((response) => {
    console.log(response.data)
})
const i18n = new VueI18Next(i18next);

new Vue({
    i18n,
    router,
    render: h => h(App)
}).$mount('#app')
