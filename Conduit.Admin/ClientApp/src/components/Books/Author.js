import React, { Component } from 'react';
import ContentWrapper from '../Layout/ContentWrapper';
import { Route, Link } from 'react-router-dom'
import {
    Container,
    Card,
    CardHeader,
    CardBody,
    CardTitle,
    CardFooter,
    Table
} from 'reactstrap';
import { translate, Trans } from 'react-i18next';
import { connect } from 'react-redux';
import { getAuthorsAction,deleteAuthorAction } from './../../actions';
import _ from 'lodash';
import Pagination from "react-js-pagination";
import 'loaders.css/loaders.css';
import 'spinkit/css/spinkit.css';
import './AuthorCSS.css';
import AuthorItem from './AuthorItem';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
//https://reactjs.org/docs/forms.html
class Author extends Component {

    constructor(props) {
        super(props);
        this.handlePageChanged = this
            .handlePageChanged
            .bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.state = {
            searchquery: '',
            modal: false,
            toasterPos: 'top-right',
            toasterType: 'info'
        }
       
    }

    componentDidMount() {
        this
            .props
            .getAuthorsAction(1, 2);

    }
    notify = (str) => {
        
        toast(str, {
            type: this.state.toasterType,
            position: this.state.toasterPos
        });

    }
    onDeleteItem(id){
        
        this.props.deleteAuthorAction(id)
    }
    renderItem(item) {
        if(!_.isEmpty(this.props.error)){
            this.notify(this.props.error)
        }
        return (
            <tr className="gradeX" key={item.Name}>
                <td>{item.Name}</td>
                <td>{item.Age}</td>
                <td>{item.Genre}</td>
                <td>4</td>
                <td>
                    <Link className="btn btn-sm btn-secondary" to={ `${this.props.match.url}/1` }>
                        <em className="fas fa-pencil-alt"></em>
                        {/*Thêm*/}
                    </Link>
                    <button type="button" className="btn btn-sm btn-danger" onClick={()=>this.onDeleteItem(item.Id)}>
                        <em className="fas fa-trash-alt"></em>
                    </button>
                    <ToastContainer />
                </td>
            </tr>
        );

    }

    renderTable() {
        // if (this.props.loading) {     return (         <div>DANG LOADING</div>     )
        // }

        return _.map(this.props.authors, item => this.renderItem(item));

    }
    renderLoading() {
        return this.props.loading
            ? "table table-striped my-4 w-100 whirl no-overlay"
            : "table table-striped my-4 w-100"
        //return this.props.loading===false?"whirl standard":""
    }
    handlePageChanged(pageNumber) {
        this
            .props
            .getAuthorsAction(pageNumber, 2, this.state.searchquery);
    }
    handleChange(event) {
        this.setState({ searchquery: event.target.value });
    }

    handleSubmit(event) {
        this.props.getAuthorsAction(1, 2, this.state.searchquery)
        event.preventDefault();
    }
    renderContent() {
        return (
            <div>
                <div className="content-heading">
                    <div>
                        DANH MỤC TÁC GIẢ
                        <small>Sách được cập nhật liên tục, trên hệ thống máy chủ</small>
                    </div>
                    <div className='ml-auto'>
                        <Link className="btn btn-secondary" to={`${this.props.match.url}/1`}>Thêm</Link>
                    </div>

                </div>
                <div className='authorMargin'>

                    <div className="d-flex align-items-center">
                        <div className="input-group">

                            {/*
                    <div className="input-group-append">
                        <Link className="btn btn-secondary" to={`${this.props.match.url}/1`}>Thêm</Link>
                        <Route path={`${this.props.match.url}/:id`} component={AuthorItem}/>
                    </div>
                */}

                        </div>

                        <form onSubmit={this.handleSubmit} className='ml-auto input-group'>
                            <input value={this.state.searchquery} onChange={this.handleChange} type="text" className="form-control" placeholder="Nhập từ khóa tìm kiếm..." />
                            <div className="input-group-append">
                                <button className="btn btn-secondary" type="Search">Tìm kiếm</button>
                            </div>
                        </form>


                    </div>

                </div>
                <Card>
                    <table
                        className={this.renderLoading()}
                        ref={el => this.el = el}>
                        <thead>
                            <tr>
                                <th data-priority="1">MÃ TÁC GIẢ</th>
                                <th>TÊN TÁC GIẢ</th>
                                <th>NĂM SINH</th>
                                <th className="sort-numeric">THỂ LOẠI</th>
                                <th className="sort-alpha" data-priority="2">SỐ LƯỢNG XB</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.renderTable()}
                        </tbody>
                    </table>
                </Card>
                <div>
                    <div className="d-flex align-items-center">

                        <div className='ml-auto'>
                            <Pagination
                                activePage={this.props.page.currentPage}
                                itemsCountPerPage={2}
                                totalItemsCount={this.props.page.totalCount}
                                pageRangeDisplayed={5}
                                onChange={this.handlePageChanged} />
                        </div>

                    </div>
                </div></div>
        );
    }
    render() {

        return (
            <ContentWrapper>

                <Route path={`${this.props.match.url}/:id`} component={AuthorItem} />
                <Route
                    exact
                    path={this.props.match.url}
                    render={() => this.renderContent()}
                />


            </ContentWrapper>
        );
    }

}

//export default  translate('translations')(Author);
const mapStateToProps = ({ books }) => {
    const { authors, loading, page, author, error } = books;
    return { authors, loading, page, author, error };
};

const mapDispatchToProps = {
    getAuthorsAction,deleteAuthorAction
};

export default connect(mapStateToProps, mapDispatchToProps)(Author);
