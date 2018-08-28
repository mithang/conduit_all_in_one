import React, {Component} from 'react';
import ContentWrapper from '../Layout/ContentWrapper';
import {Container, Card, CardHeader, CardBody, CardTitle} from 'reactstrap';
import $ from 'jquery';
import { translate, Trans } from 'react-i18next';
import Datatable from '../Tables/Datatable';
import { connect } from 'react-redux';
import { getAuthorsAction } from './../../actions';
import _ from 'lodash';

class Author extends Component {

    state = {
        dtOptions1: {
            'paging': true, // Table pagination
            'ordering': true, // Column ordering
            'info': true, // Bottom left status text
            "lengthMenu": [[5, 10, 20, -1], [5, 10, 20, "All"]],
            responsive: true,
            // Text translation options Note the required keywords between underscores (e.g
            // _MENU_)
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
        }

    }

    // Access to internal datatable instance for customizations
    dtInstance = dtInstance => {
        const inputSearchClass = 'datatable_input_col_search';
        const columnInputs = $('tfoot .' + inputSearchClass);
        // On input keyup trigger filtering
        columnInputs.keyup(function () {
            dtInstance.fnFilter(this.value, columnInputs.index(this));
        });
    }
    
    componentDidMount() {
       this.props.getAuthorsAction();
    }
    renderItem(item){
        return (
        <tr className="gradeX">
            <td>{item.Name}</td>
            <td>{item.Age}</td>
            <td>{item.Genre}</td>
            <td>4</td>
            <td>X</td>
        </tr>);
        // <tr className="gradeC">
        //     <td>Trident</td>
        //     <td>Internet Explorer 5.0</td>
        //     <td>Win 95+</td>
        //     <td>5</td>
        //     <td>C</td>
        // </tr>

    }
    render() {
       
        return (
            <ContentWrapper>
                <div className="content-heading">
                    <div>DANH MỤC TÁC GIẢ
                        <small>Sách được cập nhật liên tục, trên hệ thống máy chủ</small>
                    </div>
                </div>

                <Datatable options={this.state.dtOptions1}>
                    <table className="table table-striped my-4 w-100">
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

                           {
                               _.map(this.props.authors,(item)=>this.renderItem(item))
                              
                            }
                        </tbody>
                    </table>
                </Datatable>

            </ContentWrapper>
        );
    }

}

//export default  translate('translations')(Author);
const mapStateToProps = ({ books }) => {
    const {authors,loading}=books;
  
    return {authors,loading};
  };
  
  const mapDispatchToProps ={
    getAuthorsAction
  };
  
  export default connect(mapStateToProps,mapDispatchToProps)(Author);
