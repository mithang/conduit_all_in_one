import React, {Component} from 'react';
import PropTypes from 'prop-types';
import $ from 'jquery';
import _ from 'lodash';
// Datatables
require('datatables.net-bs')
require('datatables.net-bs4')
require('datatables.net-bs4/css/dataTables.bootstrap4.css')
require('datatables.net-buttons')
require('datatables.net-buttons-bs')
require('datatables.net-responsive')
require('datatables.net-responsive-bs')
require('datatables.net-responsive-bs/css/responsive.bootstrap.css')
require('datatables.net-buttons/js/buttons.colVis') // Column visibility
require('datatables.net-buttons/js/buttons.html5') // HTML 5 file export
require('datatables.net-buttons/js/buttons.flash') // Flash file export
require('datatables.net-buttons/js/buttons.print') // Print view button
require('datatables.net-keytable');
require('datatables.net-keytable-bs/css/keyTable.bootstrap.css')
require('jszip/dist/jszip');
require('pdfmake/build/pdfmake');
require('pdfmake/build/vfs_fonts');

/**
 * Wrapper component for dataTable plugin
 * Only DOM child elements, componets are not supported (e.g. <Table>)
 */
export default class Tb extends Component {

    componentDidMount() {
        this.el = $(this.el)
        this.el.DataTable({
            processing:true,
                    paging: true, // Table pagination
                    ordering: true, // Column ordering
                    info: true, // Bottom left status text
                    lengthMenu: [
                        [
                            5, 10, 20, -1
                        ],
                        [5, 10, 20, "All"]
                    ],
                    responsive: true,
                destroy: true,
                oLanguage: {
                    sSearch: '<em class="fa fa-search"></em>',
                    sLengthMenu: '_MENU_ records per page',
                    info: 'Showing page _PAGE_ of _PAGES_',
                    zeroRecords: 'Nothing found - sorry',
                    infoEmpty: 'No records available',
                    infoFiltered: '(filtered from _MAX_ total records)',
                    oPaginate: {
                        sNext: '<em class="fa fa-caret-right"></em>',
                        sPrevious: '<em class="fa fa-caret-left"></em>'
                    }
                }
            })
    }

    componentWillUnmount() {
        //this.el.DataTable.destroy(true)
    }
    renderItem(item) {
       
        return (
            <tr className="gradeX" key={item.Name}>
                <td>{item.Name}</td>
                <td>{item.Age}</td>
                <td>{item.Genre}</td>
                <td>4</td>
                <td>X</td>
            </tr>
        );

    }
    renderTable() {
        // if (this.props.loading) {
        //     return (
        //         <div>DANG LOADING</div>
        //     )
        // }
        
        return _.map(this.props.data, item => this.renderItem(item));

    }
    renderLoading(){
        return this.props.loading?"whirl no-overlay":""
        //return this.props.loading===false?"whirl standard":""
    }
    render() {
        return (
            <div>
               
                <table className={`table table-striped my-4 w-100 ${this.renderLoading()}`} ref={el => this.el = el}>
                    <thead>
                        <tr>
                            <th data-priority="1">MÃ SÁCH</th>
                            <th>TÊN SÁCH</th>
                            <th>NĂM PHÁT HÀNH</th>
                            <th className="sort-numeric">THỂ LOẠI</th>
                            <th className="sort-alpha" data-priority="2">SỐ LƯỢNG XB</th>
                        </tr>
                    </thead>
                    <tbody>
                       {this.renderTable()}
                    </tbody>
                </table>
            </div>
        )
    }
}
