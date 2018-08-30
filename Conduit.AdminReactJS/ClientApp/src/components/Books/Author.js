import React, {Component} from 'react';
import ContentWrapper from '../Layout/ContentWrapper';
import {
    Container,
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    CardFooter
} from 'reactstrap';
import $ from 'jquery';
import {translate, Trans} from 'react-i18next';
import Datatable from '../Tables/Datatable';
import Tb from '../Tables/Tb';
import {connect} from 'react-redux';
import {getAuthorsAction} from './../../actions';
import _ from 'lodash';
import Pagination from "react-js-pagination";
import 'loaders.css/loaders.css';
import 'spinkit/css/spinkit.css';
import './AuthorCSS.css';
class Author extends Component {

    constructor(props) {
        super(props);
        this.handlePageChanged = this
            .handlePageChanged
            .bind(this);

        this.state = {
            activePage: 15
        };
    }

    componentDidMount() {
        this
            .props
            .getAuthorsAction();

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
    renderTest() {
        if (this.props.loading) {
            return (
                <div>DANG TẢI DỮ LIỆU ...</div>
            )
        }

        return <Tb data={this.props.authors} loading={this.props.loading}></Tb>
    }
    renderTable() {
        // if (this.props.loading) {     return (         <div>DANG LOADING</div>     )
        // }

        return _.map(this.props.authors, item => this.renderItem(item));

    }
    renderLoading() {
        return this.props.loading
            ? "whirl no-overlay"
            : ""
        //return this.props.loading===false?"whirl standard":""
    }
    handlePageChanged(pageNumber) {
        console.log(`active page is ${pageNumber}`);
        this.setState({activePage: pageNumber});
    }
    render() {

        return (
            <ContentWrapper>
                <div className="content-heading">
                    <div>DANH MỤC TÁC GIẢ
                        <small>Sách được cập nhật liên tục, trên hệ thống máy chủ</small>
                    </div>
                </div>
                <Card>
                    <CardHeader>
                        
                    <div className="d-flex align-items-center">
                    <div className="input-group">
                    <select className="custom-select" id="inputGroupSelect04">
                        <option value="0">Bulk action</option>
                        <option value="1">Delete</option>
                        <option value="2">Clone</option>
                        <option value="3">Export</option>
                    </select>
                    <div className="input-group-append">
                        <button className="btn btn-secondary" type="button">Apply</button>
                    </div>

                    <input type="text" class="form-control" placeholder="Search"/>
                    <div class="input-group-append">
                        <button class="btn btn-secondary" type="Search">Button</button>
                    </div>
                </div>
                            <div className='ml-auto'>
                                <Pagination
                                    activePage={this.state.activePage}
                                    itemsCountPerPage={10}
                                    totalItemsCount={450}
                                    pageRangeDisplayed={5}
                                    onChange={this.handlePageChanged}/>
                            </div>

                        </div>
                            
                     

                    </CardHeader>
                    <CardBody>
                    <table
                        className={`table table-striped my-4 w-100 ${this.renderLoading()}`}
                        ref={el => this.el = el}>
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
                    </CardBody>
                    <CardFooter>
                        <div className="d-flex align-items-center">

                            <div className='ml-auto'>
                                <Pagination
                                    activePage={this.state.activePage}
                                    itemsCountPerPage={10}
                                    totalItemsCount={450}
                                    pageRangeDisplayed={5}
                                    onChange={this.handlePageChanged}/>
                            </div>

                        </div>
                    </CardFooter>
                </Card>
            </ContentWrapper>
        );
    }

}

//export default  translate('translations')(Author);
const mapStateToProps = ({books}) => {
    const {authors, loading} = books;

    return {authors, loading};
};

const mapDispatchToProps = {
    getAuthorsAction
};

export default connect(mapStateToProps, mapDispatchToProps)(Author);
